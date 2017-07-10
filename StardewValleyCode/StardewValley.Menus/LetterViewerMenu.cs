using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class LetterViewerMenu : IClickableMenu
	{
		public const int region_backButton = 101;

		public const int region_forwardButton = 102;

		public const int region_acceptQuestButton = 103;

		public const int region_itemGrabButton = 104;

		public const int letterWidth = 320;

		public const int letterHeight = 180;

		public Texture2D letterTexture;

		private int moneyIncluded;

		private int questID = -1;

		private string learnedRecipe = "";

		private string cookingOrCrafting = "";

		private string mailTitle;

		private List<string> mailMessage = new List<string>();

		private int page;

		public List<ClickableComponent> itemsToGrab = new List<ClickableComponent>();

		private float scale;

		private bool isMail;

		public ClickableTextureComponent backButton;

		public ClickableTextureComponent forwardButton;

		public ClickableComponent acceptQuestButton;

		public const float scaleChange = 0.003f;

		public LetterViewerMenu(string text) : base((int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			Game1.playSound("shwip");
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 101,
				rightNeighborID = 102
			};
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 102,
				leftNeighborID = 101
			};
			this.letterTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\letterBG");
			this.mailMessage = SpriteText.getStringBrokenIntoSectionsOfHeight(text, this.width - Game1.tileSize / 2, this.height - Game1.tileSize * 2);
		}

		public LetterViewerMenu(string mail, string mailTitle) : base((int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			this.isMail = true;
			Game1.playSound("shwip");
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 101,
				rightNeighborID = 102
			};
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 102,
				leftNeighborID = 101
			};
			this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0])).X + Game1.pixelZoom * 6, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0])).Y + Game1.pixelZoom * 6), "")
			{
				myID = 103,
				rightNeighborID = 102,
				leftNeighborID = 101
			};
			this.mailTitle = mailTitle;
			this.letterTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\letterBG");
			if (mail.Contains("¦"))
			{
				mail = (Game1.player.IsMale ? mail.Substring(0, mail.IndexOf("¦")) : mail.Substring(mail.IndexOf("¦") + 1));
			}
			if (mail.Contains("%item"))
			{
				string text = mail.Substring(mail.IndexOf("%item"), mail.IndexOf("%%") + 2 - mail.IndexOf("%item"));
				string[] array = text.Split(new char[]
				{
					' '
				});
				mail = mail.Replace(text, "");
				if (array[1].Equals("object"))
				{
					int maxValue = array.Length - 1;
					int num = Game1.random.Next(2, maxValue);
					num -= num % 2;
					StardewValley.Object item = new StardewValley.Object(Vector2.Zero, Convert.ToInt32(array[num]), Convert.ToInt32(array[num + 1]));
					this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), item)
					{
						myID = 104,
						leftNeighborID = 101,
						rightNeighborID = 102
					});
					this.backButton.rightNeighborID = 104;
					this.forwardButton.leftNeighborID = 104;
				}
				else if (array[1].Equals("tools"))
				{
					for (int i = 2; i < array.Length; i++)
					{
						Item item2 = null;
						string a = array[i];
						if (!(a == "Axe"))
						{
							if (!(a == "Hoe"))
							{
								if (!(a == "Can"))
								{
									if (!(a == "Scythe"))
									{
										if (a == "Pickaxe")
										{
											item2 = new Pickaxe();
										}
									}
									else
									{
										item2 = new MeleeWeapon(47);
									}
								}
								else
								{
									item2 = new WateringCan();
								}
							}
							else
							{
								item2 = new Hoe();
							}
						}
						else
						{
							item2 = new Axe();
						}
						if (item2 != null)
						{
							this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), item2));
						}
					}
				}
				else if (array[1].Equals("bigobject"))
				{
					int maxValue2 = array.Length - 1;
					int num2 = Game1.random.Next(2, maxValue2);
					StardewValley.Object item3 = new StardewValley.Object(Vector2.Zero, Convert.ToInt32(array[num2]), false);
					this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), item3)
					{
						myID = 104,
						leftNeighborID = 101,
						rightNeighborID = 102
					});
					this.backButton.rightNeighborID = 104;
					this.forwardButton.leftNeighborID = 104;
				}
				else if (array[1].Equals("money"))
				{
					int num3 = (array.Length > 4) ? Game1.random.Next(Convert.ToInt32(array[2]), Convert.ToInt32(array[3])) : Convert.ToInt32(array[2]);
					num3 -= num3 % 10;
					Game1.player.Money += num3;
					this.moneyIncluded = num3;
				}
				else if (array[1].Equals("quest"))
				{
					this.questID = Convert.ToInt32(array[2].Replace("%%", ""));
					if (array.Length > 4)
					{
						if (!Game1.player.mailReceived.Contains("NOQUEST_" + this.questID))
						{
							Game1.player.addQuest(this.questID);
						}
						this.questID = -1;
					}
					this.backButton.rightNeighborID = 103;
					this.forwardButton.leftNeighborID = 103;
				}
				else
				{
					if (array[1].Equals("cookingRecipe"))
					{
						Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
						using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string current = enumerator.Current;
								string[] array2 = dictionary[current].Split(new char[]
								{
									'/'
								});
								string[] array3 = array2[3].Split(new char[]
								{
									' '
								});
								if (array3[0].Equals("f") && array3[1].Equals(mailTitle.Replace("Cooking", "")) && Game1.player.friendships[array3[1]][0] >= Convert.ToInt32(array3[2]) * 250 && !Game1.player.cookingRecipes.ContainsKey(current))
								{
									Game1.player.cookingRecipes.Add(current, 0);
									this.learnedRecipe = current;
									if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
									{
										this.learnedRecipe = array2[array2.Length - 1];
									}
									this.cookingOrCrafting = Game1.content.LoadString("Strings\\UI:LearnedRecipe_cooking", new object[0]);
									break;
								}
							}
							goto IL_864;
						}
					}
					if (array[1].Equals("craftingRecipe"))
					{
						this.learnedRecipe = array[2].Replace('_', ' ');
						Game1.player.craftingRecipes.Add(this.learnedRecipe, 0);
						this.cookingOrCrafting = Game1.content.LoadString("Strings\\UI:LearnedRecipe_crafting", new object[0]);
					}
				}
			}
			IL_864:
			Random r = new Random((int)(Game1.uniqueIDForThisGame / 2uL) - Game1.year);
			mail = mail.Replace("%secretsanta", Utility.getRandomTownNPC(r, Utility.getFarmerNumberFromFarmer(Game1.player)).displayName);
			this.mailMessage = SpriteText.getStringBrokenIntoSectionsOfHeight(mail, this.width - Game1.tileSize, this.height - Game1.tileSize * 2);
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
				if (this.mailMessage != null && this.mailMessage.Count <= 1)
				{
					this.backButton.myID = -100;
					this.forwardButton.myID = -100;
				}
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			if (this.questID != -1)
			{
				this.currentlySnappedComponent = base.getComponentWithID(103);
			}
			else if (this.itemsToGrab != null && this.itemsToGrab.Count > 0)
			{
				this.currentlySnappedComponent = base.getComponentWithID(104);
			}
			else
			{
				this.currentlySnappedComponent = base.getComponentWithID(102);
			}
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X;
			this.yPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 101,
				rightNeighborID = 102
			};
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 102,
				leftNeighborID = 101
			};
			this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0])).X + Game1.pixelZoom * 6, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0])).Y + Game1.pixelZoom * 6), "")
			{
				myID = 103,
				rightNeighborID = 102,
				leftNeighborID = 101
			};
			using (List<ClickableComponent>.Enumerator enumerator = this.itemsToGrab.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.bounds = new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom);
				}
			}
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (b == Buttons.LeftTrigger && this.page > 0)
			{
				this.page--;
				Game1.playSound("shwip");
				return;
			}
			if (b == Buttons.RightTrigger && this.page < this.mailMessage.Count - 1)
			{
				this.page++;
				Game1.playSound("shwip");
				return;
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.scale < 1f)
			{
				return;
			}
			base.receiveLeftClick(x, y, playSound);
			if (Game1.activeClickableMenu == null && Game1.currentMinigame == null)
			{
				this.unload();
				return;
			}
			foreach (ClickableComponent current in this.itemsToGrab)
			{
				if (current.containsPoint(x, y) && current.item != null)
				{
					Game1.playSound("coin");
					Game1.player.addItemByMenuIfNecessary(current.item, null);
					current.item = null;
					return;
				}
			}
			if (this.backButton.containsPoint(x, y) && this.page > 0)
			{
				this.page--;
				Game1.playSound("shwip");
				return;
			}
			if (this.forwardButton.containsPoint(x, y) && this.page < this.mailMessage.Count - 1)
			{
				this.page++;
				Game1.playSound("shwip");
				return;
			}
			if (this.questID != -1 && this.acceptQuestButton.containsPoint(x, y))
			{
				Game1.player.addQuest(this.questID);
				this.questID = -1;
				Game1.playSound("newArtifact");
				return;
			}
			if (this.isWithinBounds(x, y))
			{
				if (this.page < this.mailMessage.Count - 1)
				{
					this.page++;
					Game1.playSound("shwip");
				}
				else if (this.page == this.mailMessage.Count - 1 && this.mailMessage.Count > 1)
				{
					this.page = 0;
					Game1.playSound("shwip");
				}
				if (this.mailMessage.Count == 1 && !this.isMail)
				{
					base.exitThisMenuNoSound();
					Game1.playSound("shwip");
					return;
				}
			}
			else if (!this.itemsLeftToGrab())
			{
				base.exitThisMenuNoSound();
				Game1.playSound("shwip");
			}
		}

		public bool itemsLeftToGrab()
		{
			if (this.itemsToGrab == null)
			{
				return false;
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.itemsToGrab.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			foreach (ClickableComponent current in this.itemsToGrab)
			{
				if (current.containsPoint(x, y))
				{
					current.scale = Math.Min(current.scale + 0.03f, 1.1f);
				}
				else
				{
					current.scale = Math.Max(1f, current.scale - 0.03f);
				}
			}
			this.backButton.tryHover(x, y, 0.6f);
			this.forwardButton.tryHover(x, y, 0.6f);
			if (this.questID != -1)
			{
				float num = this.acceptQuestButton.scale;
				this.acceptQuestButton.scale = (this.acceptQuestButton.bounds.Contains(x, y) ? 1.5f : 1f);
				if (this.acceptQuestButton.scale > num)
				{
					Game1.playSound("Cowboy_gunshot");
				}
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.scale < 1f)
			{
				this.scale += (float)time.ElapsedGameTime.Milliseconds * 0.003f;
				if (this.scale >= 1f)
				{
					this.scale = 1f;
				}
			}
			if (this.page < this.mailMessage.Count - 1 && !this.forwardButton.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
			{
				this.forwardButton.scale = 4f + (float)Math.Sin((double)((float)time.TotalGameTime.Milliseconds) / 201.06192982974676) / 1.5f;
			}
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			b.Draw(this.letterTexture, new Vector2((float)(this.xPositionOnScreen + this.width / 2), (float)(this.yPositionOnScreen + this.height / 2)), new Rectangle?(new Rectangle(0, 0, 320, 180)), Color.White, 0f, new Vector2(160f, 90f), (float)Game1.pixelZoom * this.scale, SpriteEffects.None, 0.86f);
			if (this.scale == 1f)
			{
				SpriteText.drawString(b, this.mailMessage[this.page], this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize / 2, 999999, this.width - Game1.tileSize, 999999, 0.75f, 0.865f, false, -1, "", -1);
				foreach (ClickableComponent current in this.itemsToGrab)
				{
					b.Draw(this.letterTexture, current.bounds, new Rectangle?(new Rectangle(0, 180, 24, 24)), Color.White);
					if (current.item != null)
					{
						current.item.drawInMenu(b, new Vector2((float)(current.bounds.X + 4 * Game1.pixelZoom), (float)(current.bounds.Y + 4 * Game1.pixelZoom)), current.scale);
					}
				}
				if (this.moneyIncluded > 0)
				{
					string s = Game1.content.LoadString("Strings\\UI:LetterViewer_MoneyIncluded", new object[]
					{
						this.moneyIncluded
					});
					SpriteText.drawString(b, s, this.xPositionOnScreen + this.width / 2 - SpriteText.getWidthOfString(s) / 2, this.yPositionOnScreen + this.height - Game1.tileSize * 3 / 2, 999999, -1, 9999, 0.75f, 0.865f, false, -1, "", -1);
				}
				else if (this.learnedRecipe != null && this.learnedRecipe.Length > 0)
				{
					string s2 = Game1.content.LoadString("Strings\\UI:LetterViewer_LearnedRecipe", new object[]
					{
						this.cookingOrCrafting
					});
					SpriteText.drawStringHorizontallyCenteredAt(b, s2, this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - SpriteText.getHeightOfString(s2, 999999) * 2, 999999, this.width - Game1.tileSize, 9999, 0.65f, 0.865f, false, -1);
					SpriteText.drawStringHorizontallyCenteredAt(b, Game1.content.LoadString("Strings\\UI:LetterViewer_LearnedRecipeName", new object[]
					{
						this.learnedRecipe
					}), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - SpriteText.getHeightOfString("t", 999999), 999999, this.width - Game1.tileSize, 9999, 0.9f, 0.865f, false, -1);
				}
				base.draw(b);
				if (this.page < this.mailMessage.Count - 1)
				{
					this.forwardButton.draw(b);
				}
				if (this.page > 0)
				{
					this.backButton.draw(b);
				}
				if (this.questID != -1)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.acceptQuestButton.bounds.X, this.acceptQuestButton.bounds.Y, this.acceptQuestButton.bounds.Width, this.acceptQuestButton.bounds.Height, (this.acceptQuestButton.scale > 1f) ? Color.LightPink : Color.White, (float)Game1.pixelZoom * this.acceptQuestButton.scale, true);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0]), Game1.dialogueFont, new Vector2((float)(this.acceptQuestButton.bounds.X + Game1.pixelZoom * 3), (float)(this.acceptQuestButton.bounds.Y + (LocalizedContentManager.CurrentLanguageLatin ? (Game1.pixelZoom * 4) : (Game1.pixelZoom * 3)))), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
			}
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		public void unload()
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.receiveLeftClick(x, y, playSound);
		}
	}
}
