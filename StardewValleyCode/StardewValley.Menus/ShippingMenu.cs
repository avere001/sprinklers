using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class ShippingMenu : IClickableMenu
	{
		public const int region_okbutton = 101;

		public const int region_forwardButton = 102;

		public const int region_backButton = 103;

		public const int farming_category = 0;

		public const int foraging_category = 1;

		public const int fishing_category = 2;

		public const int mining_category = 3;

		public const int other_category = 4;

		public const int total_category = 5;

		public const int timePerIntroCategory = 500;

		public const int outroFadeTime = 800;

		public const int smokeRate = 100;

		public const int categorylabelHeight = 25;

		public const int itemsPerCategoryPage = 9;

		public int currentPage = -1;

		public int currentTab;

		public List<ClickableTextureComponent> categories = new List<ClickableTextureComponent>();

		public ClickableTextureComponent okButton;

		public ClickableTextureComponent forwardButton;

		public ClickableTextureComponent backButton;

		private List<int> categoryTotals = new List<int>();

		private List<MoneyDial> categoryDials = new List<MoneyDial>();

		private List<List<Item>> categoryItems = new List<List<Item>>();

		private int categoryLabelsWidth;

		private int plusButtonWidth;

		private int itemSlotWidth;

		private int itemAndPlusButtonWidth;

		private int totalWidth;

		private int centerX;

		private int centerY;

		private int introTimer = 3500;

		private int outroFadeTimer;

		private int outroPauseBeforeDateChange;

		private int finalOutroTimer;

		private int smokeTimer;

		private int dayPlaqueY;

		private float weatherX;

		private bool outro;

		private bool newDayPlaque;

		private bool savedYet;

		public List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();

		private SaveGameMenu saveGameMenu;

		public ShippingMenu(List<Item> items) : base(Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, false)
		{
			this.parseItems(items);
			if (!Game1.wasRainingYesterday)
			{
				Game1.changeMusicTrack(Game1.currentSeason.Equals("summer") ? "nightTime" : "none");
			}
			this.categoryLabelsWidth = Game1.tileSize * 8;
			this.plusButtonWidth = 10 * Game1.pixelZoom;
			this.itemSlotWidth = 24 * Game1.pixelZoom;
			this.itemAndPlusButtonWidth = this.plusButtonWidth + this.itemSlotWidth + 2 * Game1.pixelZoom;
			this.totalWidth = this.categoryLabelsWidth + this.itemAndPlusButtonWidth;
			this.centerX = Game1.viewport.Width / 2;
			this.centerY = Game1.viewport.Height / 2;
			int num = -1;
			for (int i = 0; i < 6; i++)
			{
				this.categories.Add(new ClickableTextureComponent("", new Rectangle(this.centerX + this.totalWidth / 2 - this.plusButtonWidth, this.centerY - 25 * Game1.pixelZoom * 3 + i * 27 * Game1.pixelZoom, this.plusButtonWidth, 11 * Game1.pixelZoom), "", this.getCategoryName(i), Game1.mouseCursors, new Rectangle(392, 361, 10, 11), (float)Game1.pixelZoom, false)
				{
					visible = (i < 5 && this.categoryItems[i].Count > 0),
					myID = i,
					downNeighborID = ((i < 4) ? (i + 1) : 101),
					upNeighborID = ((i > 0) ? num : -1),
					upNeighborImmutable = true
				});
				num = ((i < 5 && this.categoryItems[i].Count > 0) ? i : num);
			}
			this.dayPlaqueY = this.categories[0].bounds.Y - Game1.tileSize * 2;
			Rectangle bounds = new Rectangle(this.centerX + this.totalWidth / 2 - this.itemAndPlusButtonWidth + Game1.tileSize / 2, this.centerY + 25 * Game1.pixelZoom * 3 - Game1.tileSize, Game1.tileSize, Game1.tileSize);
			this.okButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11382", new object[0]), bounds, null, Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11382", new object[0]), Game1.mouseCursors, new Rectangle(128, 256, 64, 64), 1f, false)
			{
				myID = 101,
				upNeighborID = num
			};
			if (Game1.options.gamepadControls)
			{
				Mouse.SetPosition(bounds.Center.X, bounds.Center.Y);
				Game1.lastCursorMotionWasMouse = false;
			}
			this.backButton = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), null, "", Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 103,
				rightNeighborID = -7777
			};
			this.forwardButton = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), null, "", Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 102,
				leftNeighborID = 103
			};
			if (Game1.dayOfMonth == 25 && Game1.currentSeason.Equals("winter"))
			{
				Vector2 position = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(0, 200));
				Rectangle sourceRect = new Rectangle(640, 800, 32, 16);
				int numberOfLoops = 1000;
				TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 80f, 2, numberOfLoops, position, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
				temporaryAnimatedSprite.motion = new Vector2(-4f, 0f);
				temporaryAnimatedSprite.delayBeforeAnimationStart = 3000;
				this.animations.Add(temporaryAnimatedSprite);
			}
			Game1.stats.checkForShippingAchievements();
			if (!Game1.player.achievements.Contains(34) && Utility.hasFarmerShippedAllItems())
			{
				Game1.getAchievement(34);
			}
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
			if (oldID == 103 && direction == 1 && this.showForwardButton())
			{
				this.currentlySnappedComponent = base.getComponentWithID(102);
				this.snapCursorToCurrentSnappedComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(101);
			this.snapCursorToCurrentSnappedComponent();
		}

		public void parseItems(List<Item> items)
		{
			Utility.consolidateStacks(items);
			for (int i = 0; i < 6; i++)
			{
				this.categoryItems.Add(new List<Item>());
				this.categoryTotals.Add(0);
				this.categoryDials.Add(new MoneyDial(7, i == 5));
			}
			foreach (Item current in items)
			{
				if (current is StardewValley.Object)
				{
					StardewValley.Object @object = current as StardewValley.Object;
					int categoryIndexForObject = this.getCategoryIndexForObject(@object);
					this.categoryItems[categoryIndexForObject].Add(@object);
					List<int> list = this.categoryTotals;
					int index = categoryIndexForObject;
					list[index] += @object.sellToStorePrice() * @object.Stack;
					Game1.stats.itemsShipped += (uint)@object.Stack;
					if (@object.Category == -75 || @object.Category == -79)
					{
						Game1.stats.CropsShipped += (uint)@object.Stack;
					}
					if (@object.countsForShippedCollection())
					{
						Game1.player.shippedBasic(@object.parentSheetIndex, @object.stack);
					}
				}
			}
			for (int j = 0; j < 5; j++)
			{
				List<int> list = this.categoryTotals;
				list[5] = list[5] + this.categoryTotals[j];
				this.categoryItems[5].AddRange(this.categoryItems[j]);
				this.categoryDials[j].currentValue = this.categoryTotals[j];
				this.categoryDials[j].previousTargetValue = this.categoryDials[j].currentValue;
			}
			this.categoryDials[5].currentValue = this.categoryTotals[5];
			Game1.player.Money += this.categoryTotals[5];
			Game1.setRichPresence("earnings", this.categoryTotals[5]);
		}

		public int getCategoryIndexForObject(StardewValley.Object o)
		{
			int num = o.parentSheetIndex;
			if (num <= 402)
			{
				if (num != 296 && num != 396 && num != 402)
				{
					goto IL_55;
				}
			}
			else if (num <= 410)
			{
				if (num != 406 && num != 410)
				{
					goto IL_55;
				}
			}
			else if (num != 414 && num != 418)
			{
				goto IL_55;
			}
			return 1;
			IL_55:
			num = o.category;
			if (num <= -23)
			{
				switch (num)
				{
				case -81:
					break;
				case -80:
				case -79:
				case -75:
					return 0;
				case -78:
				case -77:
				case -76:
					return 4;
				default:
					switch (num)
					{
					case -27:
					case -23:
						break;
					case -26:
						return 0;
					case -25:
					case -24:
						return 4;
					default:
						return 4;
					}
					break;
				}
				return 1;
			}
			if (num != -20)
			{
				switch (num)
				{
				case -15:
				case -12:
					break;
				case -14:
					return 0;
				case -13:
					return 4;
				default:
					switch (num)
					{
					case -6:
					case -5:
						return 0;
					case -4:
						return 2;
					case -3:
						return 4;
					case -2:
						break;
					default:
						return 4;
					}
					break;
				}
				return 3;
			}
			return 2;
		}

		public string getCategoryName(int index)
		{
			switch (index)
			{
			case 0:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11389", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11390", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11391", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11392", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11393", new object[0]);
			case 5:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11394", new object[0]);
			default:
				return "";
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.saveGameMenu != null)
			{
				this.saveGameMenu.update(time);
				if (this.saveGameMenu.quit)
				{
					this.saveGameMenu = null;
					this.savedYet = true;
				}
			}
			this.weatherX += (float)time.ElapsedGameTime.Milliseconds * 0.03f;
			for (int i = this.animations.Count - 1; i >= 0; i--)
			{
				if (this.animations[i].update(time))
				{
					this.animations.RemoveAt(i);
				}
			}
			if (this.outro)
			{
				if (this.outroFadeTimer > 0)
				{
					this.outroFadeTimer -= time.ElapsedGameTime.Milliseconds;
				}
				else if (this.outroFadeTimer <= 0 && this.dayPlaqueY < this.centerY - Game1.tileSize)
				{
					if (this.animations.Count > 0)
					{
						this.animations.Clear();
					}
					this.dayPlaqueY += (int)Math.Ceiling((double)((float)time.ElapsedGameTime.Milliseconds * 0.35f));
					if (this.dayPlaqueY >= this.centerY - Game1.tileSize)
					{
						this.outroPauseBeforeDateChange = 700;
					}
				}
				else if (this.outroPauseBeforeDateChange > 0)
				{
					this.outroPauseBeforeDateChange -= time.ElapsedGameTime.Milliseconds;
					if (this.outroPauseBeforeDateChange <= 0)
					{
						this.newDayPlaque = true;
						Game1.playSound("newRecipe");
						if (!Game1.currentSeason.Equals("winter"))
						{
							DelayedAction.playSoundAfterDelay(Game1.isRaining ? "rainsound" : "rooster", 1500);
						}
						this.finalOutroTimer = 2000;
						this.animations.Clear();
						if (!this.savedYet)
						{
							if (this.saveGameMenu == null)
							{
								this.saveGameMenu = new SaveGameMenu();
							}
							return;
						}
					}
				}
				else if (this.finalOutroTimer > 0 && this.savedYet)
				{
					this.finalOutroTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.finalOutroTimer <= 0)
					{
						base.exitThisMenu(false);
					}
				}
			}
			if (this.introTimer >= 0)
			{
				int arg_26E_0 = this.introTimer;
				this.introTimer -= time.ElapsedGameTime.Milliseconds * ((Game1.oldMouseState.LeftButton == ButtonState.Pressed) ? 3 : 1);
				if (arg_26E_0 % 500 < this.introTimer % 500 && this.introTimer <= 3000)
				{
					int num = 4 - this.introTimer / 500;
					if (num < 6 && num > -1)
					{
						if (this.categoryItems[num].Count > 0)
						{
							Game1.playSound(this.getCategorySound(num));
							this.categoryDials[num].currentValue = 0;
							this.categoryDials[num].previousTargetValue = 0;
						}
						else
						{
							Game1.playSound("stoneStep");
						}
					}
				}
				if (this.introTimer < 0)
				{
					Game1.playSound("money");
					this.categoryDials[5].currentValue = 0;
					this.categoryDials[5].previousTargetValue = 0;
					return;
				}
			}
			else if (Game1.dayOfMonth != 28 && !this.outro)
			{
				if (!Game1.wasRainingYesterday)
				{
					Vector2 vector = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(200));
					Rectangle sourceRect = new Rectangle(640, 752, 16, 16);
					int num2 = Game1.random.Next(1, 4);
					if (Game1.random.NextDouble() < 0.001)
					{
						bool flag = Game1.random.NextDouble() < 0.5;
						if (Game1.random.NextDouble() < 0.5)
						{
							this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(640, 826, 16, 8), 40f, 4, 0, new Vector2((float)Game1.random.Next(this.centerX * 2), (float)Game1.random.Next(this.centerY)), false, flag)
							{
								rotation = 3.14159274f,
								scale = (float)Game1.pixelZoom,
								motion = new Vector2((float)(flag ? -8 : 8), 8f),
								local = true
							});
						}
						else
						{
							this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(258, 1680, 16, 16), 40f, 4, 0, new Vector2((float)Game1.random.Next(this.centerX * 2), (float)Game1.random.Next(this.centerY)), false, flag)
							{
								scale = (float)Game1.pixelZoom,
								motion = new Vector2((float)(flag ? -8 : 8), 8f),
								local = true
							});
						}
					}
					else if (Game1.random.NextDouble() < 0.0002)
					{
						vector = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(4, Game1.tileSize * 4));
						TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.staminaRect, new Rectangle(0, 0, 1, 1), 9999f, 1, 10000, vector, false, false, 0.01f, 0f, Color.White * (0.25f + (float)Game1.random.NextDouble()), 4f, 0f, 0f, 0f, true);
						temporaryAnimatedSprite.motion = new Vector2(-0.25f, 0f);
						this.animations.Add(temporaryAnimatedSprite);
					}
					else if (Game1.random.NextDouble() < 5E-05)
					{
						vector = new Vector2((float)Game1.viewport.Width, (float)(Game1.viewport.Height - Game1.tileSize * 3));
						for (int j = 0; j < num2; j++)
						{
							TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(60, 101), 4, 100, vector + new Vector2((float)((j + 1) * Game1.random.Next(15, 18)), (float)((j + 1) * -20)), false, false, 0.01f, 0f, Color.Black, 4f, 0f, 0f, 0f, true);
							temporaryAnimatedSprite2.motion = new Vector2(-1f, 0f);
							this.animations.Add(temporaryAnimatedSprite2);
							temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(60, 101), 4, 100, vector + new Vector2((float)((j + 1) * Game1.random.Next(15, 18)), (float)((j + 1) * 20)), false, false, 0.01f, 0f, Color.Black, 4f, 0f, 0f, 0f, true);
							temporaryAnimatedSprite2.motion = new Vector2(-1f, 0f);
							this.animations.Add(temporaryAnimatedSprite2);
						}
					}
					else if (Game1.random.NextDouble() < 1E-05)
					{
						sourceRect = new Rectangle(640, 784, 16, 16);
						TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 75f, 4, 1000, vector, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
						temporaryAnimatedSprite3.motion = new Vector2(-3f, 0f);
						temporaryAnimatedSprite3.yPeriodic = true;
						temporaryAnimatedSprite3.yPeriodicLoopTime = 1000f;
						temporaryAnimatedSprite3.yPeriodicRange = (float)(Game1.tileSize / 8);
						temporaryAnimatedSprite3.shakeIntensity = 0.5f;
						this.animations.Add(temporaryAnimatedSprite3);
					}
				}
				this.smokeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.smokeTimer <= 0)
				{
					this.smokeTimer = 50;
					this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(684, 1075, 1, 1), 1000f, 1, 1000, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize * 3 / 4 + Game1.pixelZoom * 3), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.pixelZoom * 5)), false, false)
					{
						color = (Game1.wasRainingYesterday ? Color.SlateGray : Color.White),
						scale = (float)Game1.pixelZoom,
						scaleChange = 0f,
						alphaFade = 0.0025f,
						motion = new Vector2(0f, (float)(-(float)Game1.random.Next(25, 75)) / 100f / 4f),
						acceleration = new Vector2(-0.001f, 0f)
					});
				}
			}
		}

		public string getCategorySound(int which)
		{
			switch (which)
			{
			case 0:
				if (!(this.categoryItems[0][0] as StardewValley.Object).isAnimalProduct())
				{
					return "harvest";
				}
				return "cluck";
			case 1:
				return "leafrustle";
			case 2:
				return "button1";
			case 3:
				return "hammer";
			case 4:
				return "coin";
			case 5:
				return "money";
			default:
				return "stoneStep";
			}
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			if (this.currentPage == -1)
			{
				this.okButton.tryHover(x, y, 0.1f);
				using (List<ClickableTextureComponent>.Enumerator enumerator = this.categories.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent current = enumerator.Current;
						if (current.containsPoint(x, y))
						{
							current.sourceRect.X = 402;
						}
						else
						{
							current.sourceRect.X = 392;
						}
					}
					return;
				}
			}
			this.backButton.tryHover(x, y, 0.5f);
			this.forwardButton.tryHover(x, y, 0.5f);
		}

		public override void receiveKeyPress(Keys key)
		{
			if (this.introTimer <= 0 && !Game1.options.gamepadControls && (key.Equals(Keys.Escape) || Game1.options.doesInputListContain(Game1.options.menuButton, key)))
			{
				this.receiveLeftClick(this.okButton.bounds.Center.X, this.okButton.bounds.Center.Y, true);
				return;
			}
			if (this.introTimer <= 0 && (!Game1.options.gamepadControls || !Game1.options.doesInputListContain(Game1.options.menuButton, key)))
			{
				base.receiveKeyPress(key);
			}
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (b == Buttons.B && this.currentPage != -1)
			{
				if (this.currentTab == 0)
				{
					if (Game1.options.SnappyMenus)
					{
						this.currentlySnappedComponent = base.getComponentWithID(this.currentPage);
						this.snapCursorToCurrentSnappedComponent();
					}
					this.currentPage = -1;
				}
				else
				{
					this.currentTab--;
				}
				Game1.playSound("shwip");
				return;
			}
			if ((b == Buttons.Start || b == Buttons.B) && this.currentPage == -1 && !this.outro)
			{
				if (this.introTimer <= 0)
				{
					this.okClicked();
					return;
				}
				this.introTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds * 2;
			}
		}

		private void okClicked()
		{
			this.outro = true;
			this.outroFadeTimer = 800;
			Game1.playSound("bigDeSelect");
			Game1.changeMusicTrack("none");
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.outro && !this.savedYet)
			{
				SaveGameMenu arg_16_0 = this.saveGameMenu;
				return;
			}
			if (this.savedYet)
			{
				return;
			}
			base.receiveLeftClick(x, y, playSound);
			if (this.currentPage == -1 && this.introTimer <= 0 && this.okButton.containsPoint(x, y))
			{
				this.okClicked();
			}
			if (this.currentPage == -1)
			{
				int i = 0;
				while (i < this.categories.Count)
				{
					if (this.categories[i].visible && this.categories[i].containsPoint(x, y))
					{
						this.currentPage = i;
						Game1.playSound("shwip");
						if (Game1.options.SnappyMenus)
						{
							this.currentlySnappedComponent = base.getComponentWithID(103);
							this.snapCursorToCurrentSnappedComponent();
							return;
						}
						return;
					}
					else
					{
						i++;
					}
				}
				return;
			}
			if (this.backButton.containsPoint(x, y))
			{
				if (this.currentTab == 0)
				{
					if (Game1.options.SnappyMenus)
					{
						this.currentlySnappedComponent = base.getComponentWithID(this.currentPage);
						this.snapCursorToCurrentSnappedComponent();
					}
					this.currentPage = -1;
				}
				else
				{
					this.currentTab--;
				}
				Game1.playSound("shwip");
				return;
			}
			if (this.showForwardButton() && this.forwardButton.containsPoint(x, y))
			{
				this.currentTab++;
				Game1.playSound("shwip");
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public bool showForwardButton()
		{
			return this.categoryItems[this.currentPage].Count > 9 * (this.currentTab + 1);
		}

		public override void draw(SpriteBatch b)
		{
			if (Game1.wasRainingYesterday)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Game1.currentSeason.Equals("winter") ? Color.LightSlateGray : (Color.SlateGray * (1f - (float)this.introTimer / 3500f)));
				b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Game1.currentSeason.Equals("winter") ? Color.LightSlateGray : (Color.SlateGray * (1f - (float)this.introTimer / 3500f)));
				for (int i = -61 * Game1.pixelZoom; i < Game1.viewport.Width + 61 * Game1.pixelZoom; i += 61 * Game1.pixelZoom)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)i + this.weatherX / 2f % (float)(61 * Game1.pixelZoom), (float)(Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.DarkSlateGray * 1f * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(30, 62, 50)) * (0.5f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(30, 62, 50)) * (0.5f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(30, 62, 50)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(30, 62, 50)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize / 2), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				for (int j = -61 * Game1.pixelZoom; j < Game1.viewport.Width + 61 * Game1.pixelZoom; j += 61 * Game1.pixelZoom)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)j + this.weatherX % (float)(61 * Game1.pixelZoom), (float)(-(float)Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.SlateGray * 0.85f * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.animations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
				for (int k = -61 * Game1.pixelZoom; k < Game1.viewport.Width + 61 * Game1.pixelZoom; k += 61 * Game1.pixelZoom)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)k + this.weatherX * 1.5f % (float)(61 * Game1.pixelZoom), (float)(-(float)Game1.tileSize * 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.LightSlateGray * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
				}
			}
			else
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (1f - (float)this.introTimer / 3500f));
				b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (1f - (float)this.introTimer / 3500f));
				b.Draw(Game1.mouseCursors, new Vector2(0f, 0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), 0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				if (Game1.dayOfMonth == 28)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.viewport.Width - 44 * Game1.pixelZoom), (float)Game1.pixelZoom), new Rectangle?(new Rectangle(642, 835, 43, 43)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(0, 20, 40)) * (0.65f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(0, 20, 40)) * (0.65f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(0, 32, 20)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(0, 32, 20)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize / 2), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			if (!this.outro && !Game1.wasRainingYesterday)
			{
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.animations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
			}
			if (this.currentPage == -1)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, Utility.getYesterdaysDate(), Game1.viewport.Width / 2, this.categories[0].bounds.Y - Game1.tileSize * 2, "", 1f, -1, 0, 0.88f, false);
				int num = -5 * Game1.pixelZoom;
				int num2 = 0;
				foreach (ClickableTextureComponent current in this.categories)
				{
					if (this.introTimer < 2500 - num2 * 500)
					{
						Vector2 vector = current.getVector2() + new Vector2((float)(Game1.pixelZoom * 3), (float)(-(float)Game1.pixelZoom * 2));
						if (current.visible)
						{
							current.draw(b);
							b.Draw(Game1.mouseCursors, vector + new Vector2((float)(-26 * Game1.pixelZoom), (float)(num + Game1.pixelZoom)), new Rectangle?(new Rectangle(293, 360, 24, 24)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
							this.categoryItems[num2][0].drawInMenu(b, vector + new Vector2((float)(-22 * Game1.pixelZoom), (float)(num + Game1.pixelZoom * 4)), 1f, 1f, 0.9f, false);
						}
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), (int)(vector.X + (float)(-(float)this.itemSlotWidth) - (float)this.categoryLabelsWidth - (float)(Game1.pixelZoom * 3)), (int)(vector.Y + (float)num), this.categoryLabelsWidth, 26 * Game1.pixelZoom, Color.White, (float)Game1.pixelZoom, false);
						SpriteText.drawString(b, current.hoverText, (int)vector.X - this.itemSlotWidth - this.categoryLabelsWidth + Game1.pixelZoom * 2, (int)vector.Y + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
						for (int l = 0; l < 6; l++)
						{
							b.Draw(Game1.mouseCursors, vector + new Vector2((float)(-(float)this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 6 + l * 6 * Game1.pixelZoom), (float)(3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(355, 476, 7, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						}
						this.categoryDials[num2].draw(b, vector + new Vector2((float)(-(float)this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 12 + Game1.pixelZoom), (float)(5 * Game1.pixelZoom)), this.categoryTotals[num2]);
						b.Draw(Game1.mouseCursors, vector + new Vector2((float)(-(float)this.itemSlotWidth - Game1.tileSize - Game1.pixelZoom), (float)(3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(408, 476, 9, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
					}
					num2++;
				}
				if (this.introTimer <= 0)
				{
					this.okButton.draw(b);
				}
			}
			else
			{
				IClickableMenu.drawTextureBox(b, Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, Color.White);
				Vector2 vector2 = new Vector2((float)(this.xPositionOnScreen + Game1.tileSize / 2), (float)(this.yPositionOnScreen + Game1.tileSize / 2));
				for (int m = this.currentTab * 9; m < this.currentTab * 9 + 9; m++)
				{
					if (this.categoryItems[this.currentPage].Count > m)
					{
						this.categoryItems[this.currentPage][m].drawInMenu(b, vector2, 1f, 1f, 1f, true);
						if (LocalizedContentManager.CurrentLanguageLatin)
						{
							SpriteText.drawString(b, this.categoryItems[this.currentPage][m].DisplayName + ((this.categoryItems[this.currentPage][m].Stack > 1) ? (" x" + this.categoryItems[this.currentPage][m].Stack) : ""), (int)vector2.X + Game1.tileSize + Game1.pixelZoom * 3, (int)vector2.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
							string text = ".";
							for (int n = 0; n < this.width - Game1.tileSize * 3 / 2 - SpriteText.getWidthOfString(this.categoryItems[this.currentPage][m].DisplayName + ((this.categoryItems[this.currentPage][m].Stack > 1) ? (" x" + this.categoryItems[this.currentPage][m].Stack) : "") + Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
							{
								(this.categoryItems[this.currentPage][m] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][m] as StardewValley.Object).Stack
							})); n += SpriteText.getWidthOfString(" ."))
							{
								text += " .";
							}
							SpriteText.drawString(b, text, (int)vector2.X + Game1.tileSize * 5 / 4 + SpriteText.getWidthOfString(this.categoryItems[this.currentPage][m].DisplayName + ((this.categoryItems[this.currentPage][m].Stack > 1) ? (" x" + this.categoryItems[this.currentPage][m].Stack) : "")), (int)vector2.Y + Game1.tileSize / 8, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
							SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
							{
								(this.categoryItems[this.currentPage][m] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][m] as StardewValley.Object).Stack
							}), (int)vector2.X + this.width - Game1.tileSize - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
							{
								(this.categoryItems[this.currentPage][m] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][m] as StardewValley.Object).Stack
							})), (int)vector2.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
						}
						else
						{
							string text2 = this.categoryItems[this.currentPage][m].DisplayName + ((this.categoryItems[this.currentPage][m].Stack > 1) ? (" x" + this.categoryItems[this.currentPage][m].Stack) : ".");
							string text3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
							{
								(this.categoryItems[this.currentPage][m] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][m] as StardewValley.Object).Stack
							});
							int x = (int)vector2.X + this.width - Game1.tileSize - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
							{
								(this.categoryItems[this.currentPage][m] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][m] as StardewValley.Object).Stack
							}));
							SpriteText.getWidthOfString(text2 + text3);
							while (SpriteText.getWidthOfString(text2 + text3) < 1155 - Game1.tileSize / 2)
							{
								text2 += " .";
							}
							if (SpriteText.getWidthOfString(text2 + text3) >= 1155)
							{
								text2 = text2.Remove(text2.Length - 1);
							}
							SpriteText.drawString(b, text2, (int)vector2.X + Game1.tileSize + Game1.pixelZoom * 3, (int)vector2.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
							SpriteText.drawString(b, text3, x, (int)vector2.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
						}
						vector2.Y += (float)(Game1.tileSize + Game1.pixelZoom);
					}
				}
				this.backButton.draw(b);
				if (this.showForwardButton())
				{
					this.forwardButton.draw(b);
				}
			}
			if (this.outro)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.Black * (1f - (float)this.outroFadeTimer / 800f));
				SpriteText.drawStringWithScrollCenteredAt(b, this.newDayPlaque ? Utility.getDateString(0) : Utility.getYesterdaysDate(), Game1.viewport.Width / 2, this.dayPlaqueY, "", 1f, -1, 0, 0.88f, false);
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.animations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
				if (this.finalOutroTimer > 0)
				{
					b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * (1f - (float)this.finalOutroTimer / 2000f));
				}
			}
			if (this.saveGameMenu != null)
			{
				this.saveGameMenu.draw(b);
			}
			if (!Game1.options.SnappyMenus || (this.introTimer <= 0 && !this.outro))
			{
				base.drawMouse(b);
			}
		}
	}
}
