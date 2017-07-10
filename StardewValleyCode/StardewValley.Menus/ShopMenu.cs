using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class ShopMenu : IClickableMenu
	{
		public const int region_shopButtonModifier = 3546;

		public const int region_upArrow = 97865;

		public const int region_downArrow = 97866;

		public const int howManyRecipesFitOnPage = 28;

		public const int infiniteStock = 2147483647;

		public const int salePriceIndex = 0;

		public const int stockIndex = 1;

		public const int extraTradeItemIndex = 2;

		public const int itemsPerPage = 4;

		public const int numberRequiredForExtraItemTrade = 5;

		private string descriptionText = "";

		private string hoverText = "";

		private string boldTitleText = "";

		public InventoryMenu inventory;

		private Item heldItem;

		private Item hoveredItem;

		private Texture2D wallpapers;

		private Texture2D floors;

		private int lastWallpaperFloorPrice;

		private TemporaryAnimatedSprite poof;

		private Rectangle scrollBarRunner;

		private List<Item> forSale = new List<Item>();

		public List<ClickableComponent> forSaleButtons = new List<ClickableComponent>();

		private List<int> categoriesToSellHere = new List<int>();

		private Dictionary<Item, int[]> itemPriceAndStock = new Dictionary<Item, int[]>();

		private float sellPercentage = 1f;

		private List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();

		private int hoverPrice = -1;

		private int currency;

		private int currentItemIndex;

		public ClickableTextureComponent upArrow;

		public ClickableTextureComponent downArrow;

		public ClickableTextureComponent scrollBar;

		public NPC portraitPerson;

		public string potraitPersonDialogue;

		private bool scrolling;

		public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null) : this(itemPriceAndStock.Keys.ToList<Item>(), currency, who)
		{
			this.itemPriceAndStock = itemPriceAndStock;
			if (this.potraitPersonDialogue == null)
			{
				this.setUpShopOwner(who);
			}
		}

		public ShopMenu(List<Item> itemsForSale, int currency = 0, string who = null) : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 1000 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
		{
			this.currency = currency;
			if (Game1.viewport.Width < 1500)
			{
				this.xPositionOnScreen = Game1.tileSize / 2;
			}
			Game1.player.forceCanMove();
			Game1.playSound("dwop");
			this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, null, new InventoryMenu.highlightThisItem(this.highlightItemToSell), -1, 3, 0, 0, true)
			{
				showGrayedOutSlots = true
			};
			this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
			this.currency = currency;
			int arg_17E_0 = this.xPositionOnScreen;
			int arg_184_0 = IClickableMenu.borderWidth;
			int arg_18A_0 = IClickableMenu.spaceToClearSideBorder;
			int arg_191_0 = this.yPositionOnScreen;
			int arg_197_0 = IClickableMenu.borderWidth;
			int arg_19D_0 = IClickableMenu.spaceToClearTopBorder;
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false)
			{
				myID = 97865,
				downNeighborID = 106,
				leftNeighborID = 3546
			};
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false)
			{
				myID = 106,
				upNeighborID = 97865,
				leftNeighborID = 3546
			};
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
			for (int i = 0; i < 4; i++)
			{
				this.forSaleButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + i * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat(i))
				{
					myID = i + 3546,
					upNeighborID = ((i > 0) ? (i + 3546 - 1) : -7777),
					downNeighborID = ((i < 3 && i < itemsForSale.Count) ? (i + 3546 + 1) : -7777),
					rightNeighborID = 97865,
					fullyImmutable = true
				});
			}
			foreach (Item current in itemsForSale)
			{
				if (current is StardewValley.Object && (current as StardewValley.Object).isRecipe)
				{
					if (Game1.player.knowsRecipe(current.Name))
					{
						continue;
					}
					current.Stack = 1;
				}
				this.forSale.Add(current);
				this.itemPriceAndStock.Add(current, new int[]
				{
					current.salePrice(),
					current.Stack
				});
			}
			if (this.itemPriceAndStock.Count >= 2)
			{
				this.setUpShopOwner(who);
			}
			string name = Game1.currentLocation.name;
			if (!(name == "SeedShop"))
			{
				if (!(name == "Blacksmith"))
				{
					if (!(name == "ScienceHouse"))
					{
						if (!(name == "AnimalShop"))
						{
							if (!(name == "FishShop"))
							{
								if (name == "AdventureGuild")
								{
									this.categoriesToSellHere.AddRange(new int[]
									{
										-28,
										-98,
										-97,
										-96
									});
								}
							}
							else
							{
								this.categoriesToSellHere.AddRange(new int[]
								{
									-4,
									-23,
									-21,
									-22
								});
							}
						}
						else
						{
							this.categoriesToSellHere.AddRange(new int[]
							{
								-18,
								-6,
								-5,
								-14
							});
						}
					}
					else
					{
						this.categoriesToSellHere.AddRange(new int[]
						{
							-16
						});
					}
				}
				else
				{
					this.categoriesToSellHere.AddRange(new int[]
					{
						-12,
						-2,
						-15
					});
				}
			}
			else
			{
				this.categoriesToSellHere.AddRange(new int[]
				{
					-81,
					-75,
					-79,
					-80,
					-74,
					-17,
					-18,
					-6,
					-26,
					-5,
					-14,
					-19,
					-7,
					-25
				});
			}
			Game1.currentLocation.Name.Equals("SeedShop");
			if (Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
			if (direction != 2)
			{
				if (direction == 0 && this.currentItemIndex > 0)
				{
					this.upArrowPressed();
					this.currentlySnappedComponent = base.getComponentWithID(3546);
					this.snapCursorToCurrentSnappedComponent();
				}
				return;
			}
			if (this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
			{
				this.currentItemIndex++;
				return;
			}
			int num = -1;
			for (int i = 0; i < 12; i++)
			{
				this.inventory.inventory[i].upNeighborID = oldID;
				if (num == -1 && this.heldItem != null && this.inventory.actualInventory != null && this.inventory.actualInventory.Count > i && this.inventory.actualInventory[i] == null)
				{
					num = i;
				}
			}
			this.currentlySnappedComponent = base.getComponentWithID((num != -1) ? num : 0);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(3546);
			this.snapCursorToCurrentSnappedComponent();
		}

		public void setUpShopOwner(string who)
		{
			if (who != null)
			{
				Random random = new Random((int)(Game1.uniqueIDForThisGame + (ulong)Game1.stats.DaysPlayed));
				string text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11457", new object[0]);
				uint num = <PrivateImplementationDetails>.ComputeStringHash(who);
				if (num <= 1771728057u)
				{
					if (num <= 1305917497u)
					{
						if (num != 208794864u)
						{
							if (num != 1089105211u)
							{
								if (num == 1305917497u)
								{
									if (who == "Krobus")
									{
										this.portraitPerson = Game1.getCharacterFromName("Krobus", false);
										text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11497", new object[0]);
									}
								}
							}
							else if (who == "Dwarf")
							{
								this.portraitPerson = Game1.getCharacterFromName("Dwarf", false);
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11492", new object[0]);
							}
						}
						else if (who == "Pierre")
						{
							this.portraitPerson = Game1.getCharacterFromName("Pierre", false);
							switch (Game1.dayOfMonth % 7)
							{
							case 0:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11487", new object[0]);
								break;
							case 1:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11481", new object[0]);
								break;
							case 2:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11482", new object[0]);
								break;
							case 3:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11483", new object[0]);
								break;
							case 4:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11484", new object[0]);
								break;
							case 5:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11485", new object[0]);
								break;
							case 6:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11486", new object[0]);
								break;
							}
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11488", new object[0]) + text;
							if (Game1.dayOfMonth == 28)
							{
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11489", new object[0]);
							}
						}
					}
					else if (num != 1409564722u)
					{
						if (num != 1639180769u)
						{
							if (num == 1771728057u)
							{
								if (who == "ClintUpgrade")
								{
									this.portraitPerson = Game1.getCharacterFromName("Clint", false);
									text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11474", new object[0]);
								}
							}
						}
						else if (who == "HatMouse")
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11494", new object[0]);
						}
					}
					else if (who == "Traveler")
					{
						switch (random.Next(5))
						{
						case 0:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11499", new object[0]);
							break;
						case 1:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11500", new object[0]);
							break;
						case 2:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11501", new object[0]);
							break;
						case 3:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11502", new object[]
							{
								this.itemPriceAndStock.ElementAt(random.Next(this.itemPriceAndStock.Count)).Key.DisplayName
							});
							break;
						case 4:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11504", new object[0]);
							break;
						}
					}
				}
				else if (num <= 2750361957u)
				{
					if (num != 2379602843u)
					{
						if (num != 2711797968u)
						{
							if (num == 2750361957u)
							{
								if (who == "Marnie")
								{
									this.portraitPerson = Game1.getCharacterFromName("Marnie", false);
									text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11507", new object[0]);
									if (random.NextDouble() < 0.0001)
									{
										text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11508", new object[0]);
									}
								}
							}
						}
						else if (who == "Marlon")
						{
							this.portraitPerson = Game1.getCharacterFromName("Marlon", false);
							switch (random.Next(4))
							{
							case 0:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11517", new object[0]);
								break;
							case 1:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11518", new object[0]);
								break;
							case 2:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11519", new object[0]);
								break;
							case 3:
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11520", new object[0]);
								break;
							}
							if (random.NextDouble() < 0.001)
							{
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11521", new object[0]);
							}
						}
					}
					else if (who == "Robin")
					{
						this.portraitPerson = Game1.getCharacterFromName("Robin", false);
						switch (Game1.random.Next(5))
						{
						case 0:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11460", new object[0]);
							break;
						case 1:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11461", new object[0]);
							break;
						case 2:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11462", new object[0]);
							break;
						case 3:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11463", new object[0]);
							break;
						case 4:
						{
							string displayName = this.itemPriceAndStock.ElementAt(Game1.random.Next(2, this.itemPriceAndStock.Count)).Key.DisplayName;
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11464", new object[]
							{
								displayName,
								Lexicon.getRandomPositiveAdjectiveForEventOrPerson(null),
								Game1.getProperArticleForWord(displayName)
							});
							break;
						}
						}
					}
				}
				else if (num <= 3818424508u)
				{
					if (num != 3015695534u)
					{
						if (num == 3818424508u)
						{
							if (who == "Willy")
							{
								this.portraitPerson = Game1.getCharacterFromName("Willy", false);
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11477", new object[0]);
								if (Game1.random.NextDouble() < 0.05)
								{
									text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11478", new object[0]);
								}
							}
						}
					}
					else if (who == "Gus")
					{
						this.portraitPerson = Game1.getCharacterFromName("Gus", false);
						switch (Game1.random.Next(4))
						{
						case 0:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11511", new object[0]);
							break;
						case 1:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11512", new object[]
							{
								this.itemPriceAndStock.ElementAt(random.Next(this.itemPriceAndStock.Count)).Key.DisplayName
							});
							break;
						case 2:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11513", new object[0]);
							break;
						case 3:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11514", new object[0]);
							break;
						}
					}
				}
				else if (num != 3845337251u)
				{
					if (num == 4194582670u)
					{
						if (who == "Sandy")
						{
							this.portraitPerson = Game1.getCharacterFromName("Sandy", false);
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11524", new object[0]);
							if (random.NextDouble() < 0.0001)
							{
								text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11525", new object[0]);
							}
						}
					}
				}
				else if (who == "Clint")
				{
					this.portraitPerson = Game1.getCharacterFromName("Clint", false);
					switch (Game1.random.Next(3))
					{
					case 0:
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11469", new object[0]);
						break;
					case 1:
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11470", new object[0]);
						break;
					case 2:
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11471", new object[0]);
						break;
					}
				}
				this.potraitPersonDialogue = Game1.parseText(text, Game1.dialogueFont, Game1.tileSize * 5 - Game1.pixelZoom * 4);
			}
		}

		public bool highlightItemToSell(Item i)
		{
			return this.categoriesToSellHere.Contains(i.category);
		}

		public static int getPlayerCurrencyAmount(Farmer who, int currencyType)
		{
			switch (currencyType)
			{
			case 0:
				return who.Money;
			case 1:
				return who.festivalScore;
			case 2:
				return who.clubCoins;
			default:
				return 0;
			}
		}

		public override void leftClickHeld(int x, int y)
		{
			base.leftClickHeld(x, y);
			if (this.scrolling)
			{
				int arg_E8_0 = this.scrollBar.bounds.Y;
				this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
				float num = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
				this.currentItemIndex = Math.Min(this.forSale.Count - 4, Math.Max(0, (int)((float)this.forSale.Count * num)));
				this.setScrollBarToCurrentIndex();
				if (arg_E8_0 != this.scrollBar.bounds.Y)
				{
					Game1.playSound("shiny4");
				}
			}
		}

		public override void releaseLeftClick(int x, int y)
		{
			base.releaseLeftClick(x, y);
			this.scrolling = false;
		}

		private void setScrollBarToCurrentIndex()
		{
			if (this.forSale.Count > 0)
			{
				this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.forSale.Count - 4 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
				if (this.currentItemIndex == this.forSale.Count - 4)
				{
					this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
				}
			}
		}

		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shiny4");
				return;
			}
			if (direction < 0 && this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
			{
				this.downArrowPressed();
				Game1.playSound("shiny4");
			}
		}

		private void downArrowPressed()
		{
			this.downArrow.scale = this.downArrow.baseScale;
			this.currentItemIndex++;
			this.setScrollBarToCurrentIndex();
		}

		private void upArrowPressed()
		{
			this.upArrow.scale = this.upArrow.baseScale;
			this.currentItemIndex--;
			this.setScrollBarToCurrentIndex();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, true);
			if (Game1.activeClickableMenu == null)
			{
				return;
			}
			Vector2 vector = this.inventory.snapToClickableComponent(x, y);
			if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
			{
				this.downArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.scrollBar.containsPoint(x, y))
			{
				this.scrolling = true;
			}
			else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen && y < this.yPositionOnScreen + this.height)
			{
				this.scrolling = true;
				this.leftClickHeld(x, y);
				this.releaseLeftClick(x, y);
			}
			this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
			if (this.heldItem == null)
			{
				Item item = this.inventory.leftClick(x, y, null, false);
				if (item != null)
				{
					ShopMenu.chargePlayer(Game1.player, this.currency, -((int)((item is StardewValley.Object) ? ((float)(item as StardewValley.Object).sellToStorePrice() * this.sellPercentage) : ((float)(item.salePrice() / 2) * this.sellPercentage)) * item.Stack));
					int num = item.Stack / 8 + 2;
					for (int i = 0; i < num; i++)
					{
						this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, vector + new Vector2(32f, 32f), false, false)
						{
							alphaFade = 0.025f,
							motion = new Vector2((float)Game1.random.Next(-3, 4), -4f),
							acceleration = new Vector2(0f, 0.5f),
							delayBeforeAnimationStart = i * 25,
							scale = (float)Game1.pixelZoom * 0.5f
						});
						this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, vector + new Vector2(32f, 32f), false, false)
						{
							scale = (float)Game1.pixelZoom,
							alphaFade = 0.025f,
							delayBeforeAnimationStart = i * 50,
							motion = Utility.getVelocityTowardPoint(new Point((int)vector.X + 32, (int)vector.Y + 32), new Vector2((float)(this.xPositionOnScreen - Game1.pixelZoom * 9), (float)(this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4)), 8f),
							acceleration = Utility.getVelocityTowardPoint(new Point((int)vector.X + 32, (int)vector.Y + 32), new Vector2((float)(this.xPositionOnScreen - Game1.pixelZoom * 9), (float)(this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4)), 0.5f)
						});
					}
					if (item is StardewValley.Object && (item as StardewValley.Object).edibility != -300)
					{
						for (int j = 0; j < item.Stack; j++)
						{
							if (Game1.random.NextDouble() < 0.039999999105930328)
							{
								(Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(item.getOne());
							}
						}
					}
					Game1.playSound("sell");
					Game1.playSound("purchase");
					if (this.inventory.getItemAt(x, y) == null)
					{
						this.animations.Add(new TemporaryAnimatedSprite(5, vector + new Vector2(32f, 32f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
						{
							motion = new Vector2(0f, -0.5f)
						});
					}
				}
			}
			else
			{
				this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
			}
			for (int k = 0; k < this.forSaleButtons.Count; k++)
			{
				if (this.currentItemIndex + k < this.forSale.Count && this.forSaleButtons[k].containsPoint(x, y))
				{
					int num2 = this.currentItemIndex + k;
					if (this.forSale[num2] != null)
					{
						int num3 = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) / Math.Max(1, this.itemPriceAndStock[this.forSale[num2]][0])), Math.Max(1, this.itemPriceAndStock[this.forSale[num2]][1])) : 1;
						num3 = Math.Min(num3, this.forSale[num2].maximumStackSize());
						if (num3 == -1)
						{
							num3 = 1;
						}
						if (num3 > 0 && this.tryToPurchaseItem(this.forSale[num2], this.heldItem, num3, x, y, num2))
						{
							this.itemPriceAndStock.Remove(this.forSale[num2]);
							this.forSale.RemoveAt(num2);
						}
						else if (num3 <= 0)
						{
							Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
							Game1.playSound("cancel");
						}
						if (this.heldItem != null && Game1.options.SnappyMenus && Game1.activeClickableMenu != null && Game1.activeClickableMenu is ShopMenu && Game1.player.addItemToInventoryBool(this.heldItem, false))
						{
							this.heldItem = null;
							DelayedAction.playSoundAfterDelay("coin", 100);
						}
					}
					this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
					return;
				}
			}
			if (this.readyToClose() && (x < this.xPositionOnScreen - Game1.tileSize || y < this.yPositionOnScreen - Game1.tileSize || x > this.xPositionOnScreen + this.width + Game1.tileSize * 2 || y > this.yPositionOnScreen + this.height + Game1.tileSize))
			{
				base.exitThisMenu(true);
			}
		}

		public override bool readyToClose()
		{
			return this.heldItem == null && this.animations.Count == 0;
		}

		public override void emergencyShutDown()
		{
			base.emergencyShutDown();
			if (this.heldItem != null)
			{
				Game1.player.addItemToInventoryBool(this.heldItem, false);
				Game1.playSound("coin");
			}
		}

		public static void chargePlayer(Farmer who, int currencyType, int amount)
		{
			switch (currencyType)
			{
			case 0:
				who.Money -= amount;
				return;
			case 1:
				who.festivalScore -= amount;
				return;
			case 2:
				who.clubCoins -= amount;
				return;
			default:
				return;
			}
		}

		private bool tryToPurchaseItem(Item item, Item heldItem, int numberToBuy, int x, int y, int indexInForSaleList)
		{
			if (heldItem == null)
			{
				int num = this.itemPriceAndStock[item][0] * numberToBuy;
				int num2 = -1;
				if (this.itemPriceAndStock[item].Length > 2)
				{
					num2 = this.itemPriceAndStock[item][2];
				}
				if (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= num && (num2 == -1 || Game1.player.hasItemInInventory(num2, 5, 0)))
				{
					this.heldItem = item.getOne();
					this.heldItem.Stack = numberToBuy;
					if (!Game1.player.couldInventoryAcceptThisItem(this.heldItem))
					{
						Game1.playSound("smallSelect");
						this.heldItem = null;
						return false;
					}
					if (this.itemPriceAndStock[item][1] != 2147483647)
					{
						this.itemPriceAndStock[item][1] -= numberToBuy;
						this.forSale[indexInForSaleList].Stack -= numberToBuy;
					}
					ShopMenu.chargePlayer(Game1.player, this.currency, num);
					if (num2 != -1)
					{
						Game1.player.removeItemsFromInventory(num2, 5);
					}
					if (item.actionWhenPurchased())
					{
						if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).isRecipe)
						{
							string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
							try
							{
								if ((this.heldItem as StardewValley.Object).category == -7)
								{
									Game1.player.cookingRecipes.Add(key, 0);
								}
								else
								{
									Game1.player.craftingRecipes.Add(key, 0);
								}
								Game1.playSound("newRecipe");
							}
							catch (Exception)
							{
							}
							heldItem = null;
							this.heldItem = null;
						}
					}
					else if (Game1.mouseClickPolling > 300)
					{
						Game1.playSound("purchaseRepeat");
					}
					else
					{
						Game1.playSound("purchaseClick");
					}
				}
				else
				{
					Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
					Game1.playSound("cancel");
				}
			}
			else if (heldItem.Name.Equals(item.Name))
			{
				numberToBuy = Math.Min(numberToBuy, heldItem.maximumStackSize() - heldItem.Stack);
				if (numberToBuy > 0)
				{
					int num3 = this.itemPriceAndStock[item][0] * numberToBuy;
					int num4 = -1;
					if (this.itemPriceAndStock[item].Length > 2)
					{
						num4 = this.itemPriceAndStock[item][2];
					}
					if (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= num3)
					{
						this.heldItem.Stack += numberToBuy;
						if (this.itemPriceAndStock[item][1] != 2147483647)
						{
							this.itemPriceAndStock[item][1] -= numberToBuy;
						}
						ShopMenu.chargePlayer(Game1.player, this.currency, num3);
						if (Game1.mouseClickPolling > 300)
						{
							Game1.playSound("purchaseRepeat");
						}
						else
						{
							Game1.playSound("purchaseClick");
						}
						if (num4 != -1)
						{
							Game1.player.removeItemsFromInventory(num4, 5);
						}
						if (item.actionWhenPurchased())
						{
							this.heldItem = null;
						}
					}
					else
					{
						Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
						Game1.playSound("cancel");
					}
				}
			}
			if (this.itemPriceAndStock[item][1] <= 0)
			{
				this.hoveredItem = null;
				return true;
			}
			return false;
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			Vector2 vector = this.inventory.snapToClickableComponent(x, y);
			if (this.heldItem == null)
			{
				Item item = this.inventory.rightClick(x, y, null, false);
				if (item != null)
				{
					ShopMenu.chargePlayer(Game1.player, this.currency, -((int)((item is StardewValley.Object) ? ((float)(item as StardewValley.Object).sellToStorePrice() * this.sellPercentage) : ((float)(item.salePrice() / 2) * this.sellPercentage)) * item.Stack));
					item = null;
					if (Game1.mouseClickPolling > 300)
					{
						Game1.playSound("purchaseRepeat");
					}
					else
					{
						Game1.playSound("purchaseClick");
					}
					this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * Game1.tileSize, 256, Game1.tileSize, Game1.tileSize), 9999f, 1, 999, vector + new Vector2(32f, 32f), false, false)
					{
						alphaFade = 0.025f,
						motion = Utility.getVelocityTowardPoint(new Point((int)vector.X + 32, (int)vector.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 12f),
						acceleration = Utility.getVelocityTowardPoint(new Point((int)vector.X + 32, (int)vector.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 0.5f)
					});
					if (item is StardewValley.Object && (item as StardewValley.Object).edibility != -300 && Game1.random.NextDouble() < 0.039999999105930328)
					{
						(Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(item.getOne());
					}
					if (this.inventory.getItemAt(x, y) == null)
					{
						Game1.playSound("sell");
						this.animations.Add(new TemporaryAnimatedSprite(5, vector + new Vector2(32f, 32f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
						{
							motion = new Vector2(0f, -0.5f)
						});
					}
				}
			}
			else
			{
				this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
			}
			for (int i = 0; i < this.forSaleButtons.Count; i++)
			{
				if (this.currentItemIndex + i < this.forSale.Count && this.forSaleButtons[i].containsPoint(x, y))
				{
					int num = this.currentItemIndex + i;
					if (this.forSale[num] != null)
					{
						int num2 = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) / this.itemPriceAndStock[this.forSale[num]][0]), this.itemPriceAndStock[this.forSale[num]][1]) : 1;
						if (num2 > 0 && this.tryToPurchaseItem(this.forSale[num], this.heldItem, num2, x, y, num))
						{
							this.itemPriceAndStock.Remove(this.forSale[num]);
							this.forSale.RemoveAt(num);
						}
						if (this.heldItem != null && Game1.options.SnappyMenus && Game1.activeClickableMenu != null && Game1.activeClickableMenu is ShopMenu && Game1.player.addItemToInventoryBool(this.heldItem, false))
						{
							this.heldItem = null;
							DelayedAction.playSoundAfterDelay("coin", 100);
						}
					}
					return;
				}
			}
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.descriptionText = "";
			this.hoverText = "";
			this.hoveredItem = null;
			this.hoverPrice = -1;
			this.boldTitleText = "";
			this.upArrow.tryHover(x, y, 0.1f);
			this.downArrow.tryHover(x, y, 0.1f);
			this.scrollBar.tryHover(x, y, 0.1f);
			if (this.scrolling)
			{
				return;
			}
			for (int i = 0; i < this.forSaleButtons.Count; i++)
			{
				if (this.currentItemIndex + i < this.forSale.Count && this.forSaleButtons[i].containsPoint(x, y))
				{
					Item item = this.forSale[this.currentItemIndex + i];
					this.hoverText = item.getDescription();
					this.boldTitleText = item.DisplayName;
					this.hoverPrice = ((this.itemPriceAndStock != null && this.itemPriceAndStock.ContainsKey(item)) ? this.itemPriceAndStock[item][0] : item.salePrice());
					this.hoveredItem = item;
					this.forSaleButtons[i].scale = Math.Min(this.forSaleButtons[i].scale + 0.03f, 1.1f);
				}
				else
				{
					this.forSaleButtons[i].scale = Math.Max(1f, this.forSaleButtons[i].scale - 0.03f);
				}
			}
			if (this.heldItem == null)
			{
				foreach (ClickableComponent current in this.inventory.inventory)
				{
					if (current.containsPoint(x, y))
					{
						Item itemFromClickableComponent = this.inventory.getItemFromClickableComponent(current);
						if (itemFromClickableComponent != null && this.highlightItemToSell(itemFromClickableComponent))
						{
							this.hoverText = itemFromClickableComponent.DisplayName + " x" + itemFromClickableComponent.Stack;
							this.hoverPrice = (int)((itemFromClickableComponent is StardewValley.Object) ? ((float)(itemFromClickableComponent as StardewValley.Object).sellToStorePrice() * this.sellPercentage) : ((float)(itemFromClickableComponent.salePrice() / 2) * this.sellPercentage)) * itemFromClickableComponent.Stack;
						}
					}
				}
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.poof != null && this.poof.update(time))
			{
				this.poof = null;
			}
		}

		public void drawCurrency(SpriteBatch b)
		{
			int num = this.currency;
			if (num != 0)
			{
				return;
			}
			Game1.dayTimeMoneyBox.drawMoneyBox(b, this.xPositionOnScreen - Game1.pixelZoom * 9, this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 3);
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (b == Buttons.RightTrigger || b == Buttons.LeftTrigger)
			{
				if (this.currentlySnappedComponent != null && this.currentlySnappedComponent.myID >= 3546)
				{
					int num = -1;
					for (int i = 0; i < 12; i++)
					{
						this.inventory.inventory[i].upNeighborID = 3546 + this.forSaleButtons.Count - 1;
						if (num == -1 && this.heldItem != null && this.inventory.actualInventory != null && this.inventory.actualInventory.Count > i && this.inventory.actualInventory[i] == null)
						{
							num = i;
						}
					}
					this.currentlySnappedComponent = base.getComponentWithID((num != -1) ? num : 0);
					this.snapCursorToCurrentSnappedComponent();
				}
				else
				{
					this.snapToDefaultClickableComponent();
				}
				Game1.playSound("shiny4");
			}
		}

		private int getHoveredItemExtraItemIndex()
		{
			if (this.itemPriceAndStock != null && this.hoveredItem != null && this.itemPriceAndStock.ContainsKey(this.hoveredItem) && this.itemPriceAndStock[this.hoveredItem].Length > 2)
			{
				return this.itemPriceAndStock[this.hoveredItem][2];
			}
			return -1;
		}

		private int getHoveredItemExtraItemAmount()
		{
			return 5;
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2;
			this.width = 1000 + IClickableMenu.borderWidth * 2;
			this.height = 600 + IClickableMenu.borderWidth * 2;
			base.initializeUpperRightCloseButton();
			if (Game1.viewport.Width < 1500)
			{
				this.xPositionOnScreen = Game1.tileSize / 2;
			}
			Game1.player.forceCanMove();
			this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, null, new InventoryMenu.highlightThisItem(this.highlightItemToSell), -1, 3, 0, 0, true)
			{
				showGrayedOutSlots = true
			};
			this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
			int arg_113_0 = this.xPositionOnScreen;
			int arg_119_0 = IClickableMenu.borderWidth;
			int arg_11F_0 = IClickableMenu.spaceToClearSideBorder;
			int arg_126_0 = this.yPositionOnScreen;
			int arg_12C_0 = IClickableMenu.borderWidth;
			int arg_132_0 = IClickableMenu.spaceToClearTopBorder;
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
			this.forSaleButtons.Clear();
			for (int i = 0; i < 4; i++)
			{
				this.forSaleButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + i * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat(i)));
			}
		}

		public override void draw(SpriteBatch b)
		{
			if (!Game1.options.showMenuBackground)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			}
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen + this.width - this.inventory.width - Game1.tileSize / 2 - Game1.pixelZoom * 6, this.yPositionOnScreen + this.height - Game1.tileSize * 4 + Game1.pixelZoom * 10, this.inventory.width + Game1.pixelZoom * 14, this.height - Game1.tileSize * 7 + Game1.pixelZoom * 5, Color.White, (float)Game1.pixelZoom, true);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height - Game1.tileSize * 4 + Game1.tileSize / 2 + Game1.pixelZoom, Color.White, (float)Game1.pixelZoom, true);
			this.drawCurrency(b);
			for (int i = 0; i < this.forSaleButtons.Count; i++)
			{
				if (this.currentItemIndex + i < this.forSale.Count)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.forSaleButtons[i].bounds.X, this.forSaleButtons[i].bounds.Y, this.forSaleButtons[i].bounds.Width, this.forSaleButtons[i].bounds.Height, (this.forSaleButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && !this.scrolling) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, false);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.forSaleButtons[i].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 3), (float)(this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 6 - Game1.pixelZoom)), new Rectangle?(new Rectangle(296, 363, 18, 18)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
					this.forSale[this.currentItemIndex + i].drawInMenu(b, new Vector2((float)(this.forSaleButtons[i].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 2), (float)(this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 6)), 1f);
					SpriteText.drawString(b, this.forSale[this.currentItemIndex + i].DisplayName, this.forSaleButtons[i].bounds.X + Game1.tileSize * 3 / 2 + Game1.pixelZoom * 2, this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 7, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					SpriteText.drawString(b, this.itemPriceAndStock[this.forSale[this.currentItemIndex + i]][0] + " ", this.forSaleButtons[i].bounds.Right - SpriteText.getWidthOfString(this.itemPriceAndStock[this.forSale[this.currentItemIndex + i]][0] + " ") - Game1.pixelZoom * 15, this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 7, 999999, -1, 999999, (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= this.itemPriceAndStock[this.forSale[this.currentItemIndex + i]][0]) ? 1f : 0.5f, 0.88f, false, -1, "", -1);
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.forSaleButtons[i].bounds.Right - Game1.pixelZoom * 13), (float)(this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 10 - Game1.pixelZoom)), new Rectangle(193 + this.currency * 9, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
				}
			}
			if (this.forSale.Count == 0)
			{
				SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11583", new object[0]), this.xPositionOnScreen + this.width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11583", new object[0])) / 2, this.yPositionOnScreen + this.height / 2 - Game1.tileSize * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
			}
			this.inventory.draw(b);
			for (int j = this.animations.Count - 1; j >= 0; j--)
			{
				if (this.animations[j].update(Game1.currentGameTime))
				{
					this.animations.RemoveAt(j);
				}
				else
				{
					this.animations[j].draw(b, true, 0, 0);
				}
			}
			if (this.poof != null)
			{
				this.poof.draw(b, false, 0, 0);
			}
			this.upArrow.draw(b);
			this.downArrow.draw(b);
			if (this.forSale.Count > 4)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, true);
				this.scrollBar.draw(b);
			}
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawToolTip(b, this.hoverText, this.boldTitleText, this.hoveredItem, this.heldItem != null, -1, this.currency, this.getHoveredItemExtraItemIndex(), this.getHoveredItemExtraItemAmount(), null, this.hoverPrice);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
			}
			base.draw(b);
			if (Game1.viewport.Width > 800 && Game1.options.showMerchantPortraits)
			{
				if (this.portraitPerson != null)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 80 * Game1.pixelZoom), (float)this.yPositionOnScreen), new Rectangle(603, 414, 74, 74), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.91f, -1, -1, 0.35f);
					if (this.portraitPerson.Portrait != null)
					{
						b.Draw(this.portraitPerson.Portrait, new Vector2((float)(this.xPositionOnScreen - 80 * Game1.pixelZoom + Game1.pixelZoom * 5), (float)(this.yPositionOnScreen + Game1.pixelZoom * 5)), new Rectangle?(new Rectangle(0, 0, 64, 64)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.92f);
					}
				}
				if (this.potraitPersonDialogue != null)
				{
					IClickableMenu.drawHoverText(b, this.potraitPersonDialogue, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, this.xPositionOnScreen - (int)Game1.dialogueFont.MeasureString(this.potraitPersonDialogue).X - Game1.tileSize, this.yPositionOnScreen + ((this.portraitPerson != null) ? (78 * Game1.pixelZoom) : 0), 1f, null);
				}
			}
			base.drawMouse(b);
		}
	}
}
