using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class LevelUpMenu : IClickableMenu
	{
		public const int region_okButton = 101;

		public const int region_leftProfession = 102;

		public const int region_rightProfession = 103;

		public const int basewidth = 768;

		public const int baseheight = 512;

		public bool informationUp;

		public bool isActive;

		public bool isProfessionChooser;

		private int currentLevel;

		private int currentSkill;

		private int timerBeforeStart;

		private float scale;

		private Color leftProfessionColor = Game1.textColor;

		private Color rightProfessionColor = Game1.textColor;

		private MouseState oldMouseState;

		public ClickableTextureComponent starIcon;

		public ClickableTextureComponent okButton;

		public ClickableComponent leftProfession;

		public ClickableComponent rightProfession;

		private List<CraftingRecipe> newCraftingRecipes = new List<CraftingRecipe>();

		private List<string> extraInfoForLevel = new List<string>();

		private List<string> leftProfessionDescription = new List<string>();

		private List<string> rightProfessionDescription = new List<string>();

		private Rectangle sourceRectForLevelIcon;

		private string title;

		private List<int> professionsToChoose = new List<int>();

		private List<TemporaryAnimatedSprite> littleStars = new List<TemporaryAnimatedSprite>();

		public LevelUpMenu() : base(Game1.viewport.Width / 2 - 384, Game1.viewport.Height / 2 - 256, 768, 512, false)
		{
			this.width = Game1.tileSize * 12;
			this.height = Game1.tileSize * 8;
			this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
			{
				myID = 101
			};
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public LevelUpMenu(int skill, int level) : base(Game1.viewport.Width / 2 - 384, Game1.viewport.Height / 2 - 256, 768, 512, false)
		{
			this.timerBeforeStart = 250;
			this.isActive = true;
			this.width = Game1.tileSize * 15;
			this.height = Game1.tileSize * 8;
			this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
			{
				myID = 101
			};
			this.newCraftingRecipes.Clear();
			this.extraInfoForLevel.Clear();
			Game1.player.completelyStopAnimatingOrDoingAction();
			this.informationUp = true;
			this.isProfessionChooser = false;
			this.currentLevel = level;
			this.currentSkill = skill;
			if (level == 10)
			{
				Game1.getSteamAchievement("Achievement_SingularTalent");
				if (Game1.player.farmingLevel == 10 && Game1.player.miningLevel == 10 && Game1.player.fishingLevel == 10 && Game1.player.foragingLevel == 10 && Game1.player.combatLevel == 10)
				{
					Game1.getSteamAchievement("Achievement_MasterOfTheFiveWays");
				}
			}
			this.title = Game1.content.LoadString("Strings\\UI:LevelUp_Title", new object[]
			{
				this.currentLevel,
				Farmer.getSkillDisplayNameFromIndex(this.currentSkill)
			});
			this.extraInfoForLevel = this.getExtraInfoForLevel(this.currentSkill, this.currentLevel);
			switch (this.currentSkill)
			{
			case 0:
				this.sourceRectForLevelIcon = new Rectangle(0, 0, 16, 16);
				break;
			case 1:
				this.sourceRectForLevelIcon = new Rectangle(16, 0, 16, 16);
				break;
			case 2:
				this.sourceRectForLevelIcon = new Rectangle(80, 0, 16, 16);
				break;
			case 3:
				this.sourceRectForLevelIcon = new Rectangle(32, 0, 16, 16);
				break;
			case 4:
				this.sourceRectForLevelIcon = new Rectangle(128, 16, 16, 16);
				break;
			case 5:
				this.sourceRectForLevelIcon = new Rectangle(64, 0, 16, 16);
				break;
			}
			if ((this.currentLevel == 5 || this.currentLevel == 10) && this.currentSkill != 5)
			{
				this.professionsToChoose.Clear();
				this.isProfessionChooser = true;
				if (this.currentLevel == 5)
				{
					this.professionsToChoose.Add(this.currentSkill * 6);
					this.professionsToChoose.Add(this.currentSkill * 6 + 1);
				}
				else if (Game1.player.professions.Contains(this.currentSkill * 6))
				{
					this.professionsToChoose.Add(this.currentSkill * 6 + 2);
					this.professionsToChoose.Add(this.currentSkill * 6 + 3);
				}
				else
				{
					this.professionsToChoose.Add(this.currentSkill * 6 + 4);
					this.professionsToChoose.Add(this.currentSkill * 6 + 5);
				}
				this.leftProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[0]);
				this.rightProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[1]);
			}
			int num = 0;
			foreach (KeyValuePair<string, string> current in CraftingRecipe.craftingRecipes)
			{
				string text = current.Value.Split(new char[]
				{
					'/'
				})[4];
				if (text.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && text.Contains(string.Concat(this.currentLevel)))
				{
					this.newCraftingRecipes.Add(new CraftingRecipe(current.Key, false));
					if (!Game1.player.craftingRecipes.ContainsKey(current.Key))
					{
						Game1.player.craftingRecipes.Add(current.Key, 0);
					}
					num += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
				}
			}
			foreach (KeyValuePair<string, string> current2 in CraftingRecipe.cookingRecipes)
			{
				string text2 = current2.Value.Split(new char[]
				{
					'/'
				})[3];
				if (text2.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && text2.Contains(string.Concat(this.currentLevel)))
				{
					this.newCraftingRecipes.Add(new CraftingRecipe(current2.Key, true));
					if (!Game1.player.cookingRecipes.ContainsKey(current2.Key))
					{
						Game1.player.cookingRecipes.Add(current2.Key, 0);
						if (!Game1.player.hasOrWillReceiveMail("robinKitchenLetter"))
						{
							Game1.mailbox.Enqueue("robinKitchenLetter");
						}
					}
					num += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
				}
			}
			this.height = num + Game1.tileSize * 4 + this.extraInfoForLevel.Count * Game1.tileSize * 3 / 4;
			Game1.player.freezePause = 100;
			this.gameWindowSizeChanged(Rectangle.Empty, Rectangle.Empty);
			if (Game1.options.SnappyMenus)
			{
				if (this.isProfessionChooser)
				{
					this.leftProfession = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen + Game1.tileSize * 2, this.width / 2, this.height), "")
					{
						myID = 102,
						rightNeighborID = 103
					};
					this.rightProfession = new ClickableComponent(new Rectangle(this.width / 2 + this.xPositionOnScreen, this.yPositionOnScreen + Game1.tileSize * 2, this.width / 2, this.height), "")
					{
						myID = 103,
						leftNeighborID = 102
					};
				}
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			if (this.isProfessionChooser)
			{
				this.currentlySnappedComponent = base.getComponentWithID(103);
				Game1.setMousePosition(this.xPositionOnScreen + this.width + Game1.tileSize, this.yPositionOnScreen + this.height + Game1.tileSize);
				return;
			}
			this.currentlySnappedComponent = base.getComponentWithID(101);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = Game1.viewport.Width / 2 - this.width / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - this.height / 2;
			this.okButton.bounds = new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public List<string> getExtraInfoForLevel(int whichSkill, int whichLevel)
		{
			List<string> list = new List<string>();
			switch (whichSkill)
			{
			case 0:
				list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Farming1", new object[0]));
				list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Farming2", new object[0]));
				break;
			case 1:
				list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Fishing", new object[0]));
				break;
			case 2:
				list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Foraging1", new object[0]));
				if (whichLevel == 1)
				{
					list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Foraging2", new object[0]));
				}
				if (whichLevel == 4 || whichLevel == 8)
				{
					list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Foraging3", new object[0]));
				}
				break;
			case 3:
				list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Mining", new object[0]));
				break;
			case 4:
				list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Combat", new object[0]));
				break;
			case 5:
				list.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Luck", new object[0]));
				break;
			}
			return list;
		}

		private static void addProfessionDescriptions(List<string> descriptions, string professionName)
		{
			descriptions.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ProfessionName_" + professionName, new object[0]));
			descriptions.AddRange(Game1.content.LoadString("Strings\\UI:LevelUp_ProfessionDescription_" + professionName, new object[0]).Split(new char[]
			{
				'\n'
			}));
		}

		private static string getProfessionName(int whichProfession)
		{
			switch (whichProfession)
			{
			case 0:
				return "Rancher";
			case 1:
				return "Tiller";
			case 2:
				return "Coopmaster";
			case 3:
				return "Shepherd";
			case 4:
				return "Artisan";
			case 5:
				return "Agriculturist";
			case 6:
				return "Fisher";
			case 7:
				return "Trapper";
			case 8:
				return "Angler";
			case 9:
				return "Pirate";
			case 10:
				return "Mariner";
			case 11:
				return "Luremaster";
			case 12:
				return "Forester";
			case 13:
				return "Gatherer";
			case 14:
				return "Lumberjack";
			case 15:
				return "Tapper";
			case 16:
				return "Botanist";
			case 17:
				return "Tracker";
			case 18:
				return "Miner";
			case 19:
				return "Geologist";
			case 20:
				return "Blacksmith";
			case 21:
				return "Prospector";
			case 22:
				return "Excavator";
			case 23:
				return "Gemologist";
			case 24:
				return "Fighter";
			case 25:
				return "Scout";
			case 26:
				return "Brute";
			case 27:
				return "Defender";
			case 28:
				return "Acrobat";
			}
			return "Desperado";
		}

		public static List<string> getProfessionDescription(int whichProfession)
		{
			List<string> expr_05 = new List<string>();
			LevelUpMenu.addProfessionDescriptions(expr_05, LevelUpMenu.getProfessionName(whichProfession));
			return expr_05;
		}

		public static string getProfessionTitleFromNumber(int whichProfession)
		{
			return Game1.content.LoadString("Strings\\UI:LevelUp_ProfessionName_" + LevelUpMenu.getProfessionName(whichProfession), new object[0]);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if ((b == Buttons.Start || b == Buttons.B) && !this.isProfessionChooser && this.isActive)
			{
				this.okButtonClicked();
			}
		}

		public void getImmediateProfessionPerk(int whichProfession)
		{
			if (whichProfession == 24)
			{
				Game1.player.maxHealth += 15;
				return;
			}
			if (whichProfession != 27)
			{
				return;
			}
			Game1.player.maxHealth += 25;
		}

		public override void update(GameTime time)
		{
			if (!this.isActive)
			{
				base.exitThisMenu(true);
				return;
			}
			for (int i = this.littleStars.Count - 1; i >= 0; i--)
			{
				if (this.littleStars[i].update(time))
				{
					this.littleStars.RemoveAt(i);
				}
			}
			if (Game1.random.NextDouble() < 0.03)
			{
				Vector2 vector = new Vector2(0f, (float)(Game1.random.Next(this.yPositionOnScreen - Game1.tileSize * 2, this.yPositionOnScreen - Game1.pixelZoom) / (Game1.pixelZoom * 5) * Game1.pixelZoom * 5 + Game1.tileSize / 2));
				if (Game1.random.NextDouble() < 0.5)
				{
					vector.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 - 57 * Game1.pixelZoom, this.xPositionOnScreen + this.width / 2 - 33 * Game1.pixelZoom);
				}
				else
				{
					vector.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 + 29 * Game1.pixelZoom, this.xPositionOnScreen + this.width - 40 * Game1.pixelZoom);
				}
				if (vector.Y < (float)(this.yPositionOnScreen - Game1.tileSize - Game1.pixelZoom * 2))
				{
					vector.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 - 29 * Game1.pixelZoom, this.xPositionOnScreen + this.width / 2 + 29 * Game1.pixelZoom);
				}
				vector.X = vector.X / (float)(Game1.pixelZoom * 5) * (float)Game1.pixelZoom * 5f;
				this.littleStars.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(364, 79, 5, 5), 80f, 7, 1, vector, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					local = true
				});
			}
			if (this.timerBeforeStart > 0)
			{
				this.timerBeforeStart -= time.ElapsedGameTime.Milliseconds;
				return;
			}
			if (this.isActive && this.isProfessionChooser)
			{
				this.leftProfessionColor = Game1.textColor;
				this.rightProfessionColor = Game1.textColor;
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.player.freezePause = 100;
				if (Game1.getMouseY() > this.yPositionOnScreen + Game1.tileSize * 3 && Game1.getMouseY() < this.yPositionOnScreen + this.height)
				{
					if (Game1.getMouseX() > this.xPositionOnScreen && Game1.getMouseX() < this.xPositionOnScreen + this.width / 2)
					{
						this.leftProfessionColor = Color.Green;
						if (((Mouse.GetState().LeftButton == ButtonState.Pressed && this.oldMouseState.LeftButton == ButtonState.Released) || (Game1.options.gamepadControls && GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
						{
							Game1.player.professions.Add(this.professionsToChoose[0]);
							this.getImmediateProfessionPerk(this.professionsToChoose[0]);
							this.isActive = false;
							this.informationUp = false;
							this.isProfessionChooser = false;
						}
					}
					else if (Game1.getMouseX() > this.xPositionOnScreen + this.width / 2 && Game1.getMouseX() < this.xPositionOnScreen + this.width)
					{
						this.rightProfessionColor = Color.Green;
						if (((Mouse.GetState().LeftButton == ButtonState.Pressed && this.oldMouseState.LeftButton == ButtonState.Released) || (Game1.options.gamepadControls && GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
						{
							Game1.player.professions.Add(this.professionsToChoose[1]);
							this.getImmediateProfessionPerk(this.professionsToChoose[1]);
							this.isActive = false;
							this.informationUp = false;
							this.isProfessionChooser = false;
						}
					}
				}
				this.height = Game1.tileSize * 8;
			}
			this.oldMouseState = Mouse.GetState();
			if (this.isActive && !this.informationUp && this.starIcon != null)
			{
				if (this.starIcon.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
				{
					this.starIcon.sourceRect.X = 294;
				}
				else
				{
					this.starIcon.sourceRect.X = 310;
				}
			}
			if (this.isActive && this.starIcon != null && !this.informationUp && (this.oldMouseState.LeftButton == ButtonState.Pressed || (Game1.options.gamepadControls && Game1.oldPadState.IsButtonDown(Buttons.A))) && this.starIcon.containsPoint(this.oldMouseState.X, this.oldMouseState.Y))
			{
				this.newCraftingRecipes.Clear();
				this.extraInfoForLevel.Clear();
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.playSound("bigSelect");
				this.informationUp = true;
				this.isProfessionChooser = false;
				this.currentLevel = Game1.player.newLevels.First<Point>().Y;
				this.currentSkill = Game1.player.newLevels.First<Point>().X;
				this.title = Game1.content.LoadString("Strings\\UI:LevelUp_Title", new object[]
				{
					this.currentLevel,
					Farmer.getSkillDisplayNameFromIndex(this.currentSkill)
				});
				this.extraInfoForLevel = this.getExtraInfoForLevel(this.currentSkill, this.currentLevel);
				switch (this.currentSkill)
				{
				case 0:
					this.sourceRectForLevelIcon = new Rectangle(0, 0, 16, 16);
					break;
				case 1:
					this.sourceRectForLevelIcon = new Rectangle(16, 0, 16, 16);
					break;
				case 2:
					this.sourceRectForLevelIcon = new Rectangle(80, 0, 16, 16);
					break;
				case 3:
					this.sourceRectForLevelIcon = new Rectangle(32, 0, 16, 16);
					break;
				case 4:
					this.sourceRectForLevelIcon = new Rectangle(128, 16, 16, 16);
					break;
				case 5:
					this.sourceRectForLevelIcon = new Rectangle(64, 0, 16, 16);
					break;
				}
				if ((this.currentLevel == 5 || this.currentLevel == 10) && this.currentSkill != 5)
				{
					this.professionsToChoose.Clear();
					this.isProfessionChooser = true;
					if (this.currentLevel == 5)
					{
						this.professionsToChoose.Add(this.currentSkill * 6);
						this.professionsToChoose.Add(this.currentSkill * 6 + 1);
					}
					else if (Game1.player.professions.Contains(this.currentSkill * 6))
					{
						this.professionsToChoose.Add(this.currentSkill * 6 + 2);
						this.professionsToChoose.Add(this.currentSkill * 6 + 3);
					}
					else
					{
						this.professionsToChoose.Add(this.currentSkill * 6 + 4);
						this.professionsToChoose.Add(this.currentSkill * 6 + 5);
					}
					this.leftProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[0]);
					this.rightProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[1]);
				}
				int num = 0;
				foreach (KeyValuePair<string, string> current in CraftingRecipe.craftingRecipes)
				{
					string text = current.Value.Split(new char[]
					{
						'/'
					})[4];
					if (text.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && text.Contains(string.Concat(this.currentLevel)))
					{
						this.newCraftingRecipes.Add(new CraftingRecipe(current.Key, false));
						if (!Game1.player.craftingRecipes.ContainsKey(current.Key))
						{
							Game1.player.craftingRecipes.Add(current.Key, 0);
						}
						num += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
					}
				}
				foreach (KeyValuePair<string, string> current2 in CraftingRecipe.cookingRecipes)
				{
					string text2 = current2.Value.Split(new char[]
					{
						'/'
					})[3];
					if (text2.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && text2.Contains(string.Concat(this.currentLevel)))
					{
						this.newCraftingRecipes.Add(new CraftingRecipe(current2.Key, true));
						if (!Game1.player.cookingRecipes.ContainsKey(current2.Key))
						{
							Game1.player.cookingRecipes.Add(current2.Key, 0);
						}
						num += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
					}
				}
				this.height = num + Game1.tileSize * 4 + this.extraInfoForLevel.Count * Game1.tileSize * 3 / 4;
				Game1.player.freezePause = 100;
			}
			if (this.isActive && this.informationUp)
			{
				Game1.player.completelyStopAnimatingOrDoingAction();
				if (this.okButton.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && !this.isProfessionChooser)
				{
					this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
					if ((this.oldMouseState.LeftButton == ButtonState.Pressed || (Game1.options.gamepadControls && Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
					{
						this.okButtonClicked();
					}
				}
				else
				{
					this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
				}
				Game1.player.freezePause = 100;
			}
		}

		public void okButtonClicked()
		{
			this.getLevelPerk(this.currentSkill, this.currentLevel);
			this.isActive = false;
			this.informationUp = false;
		}

		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.SnappyMenus && ((!Game1.options.doesInputListContain(Game1.options.cancelButton, key) && !Game1.options.doesInputListContain(Game1.options.menuButton, key)) || !this.isProfessionChooser))
			{
				base.receiveKeyPress(key);
			}
		}

		public void getLevelPerk(int skill, int level)
		{
			if (skill != 1)
			{
				if (skill == 4)
				{
					Game1.player.maxHealth += 5;
				}
			}
			else if (level != 2)
			{
				if (level == 6)
				{
					if (!Game1.player.hasOrWillReceiveMail("fishing6"))
					{
						Game1.addMailForTomorrow("fishing6", false, false);
					}
				}
			}
			else if (!Game1.player.hasOrWillReceiveMail("fishing2"))
			{
				Game1.addMailForTomorrow("fishing2", false, false);
			}
			Game1.player.health = Game1.player.maxHealth;
			Game1.player.Stamina = (float)Game1.player.maxStamina;
		}

		public override void draw(SpriteBatch b)
		{
			if (this.timerBeforeStart > 0)
			{
				return;
			}
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.littleStars.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b, false, 0, 0);
				}
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + this.width / 2 - 58 * Game1.pixelZoom / 2), (float)(this.yPositionOnScreen - Game1.tileSize / 2 + Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(363, 87, 58, 22)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			if (!this.informationUp && this.isActive && this.starIcon != null)
			{
				this.starIcon.draw(b);
				return;
			}
			if (this.informationUp)
			{
				if (this.isProfessionChooser)
				{
					Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
					base.drawHorizontalPartition(b, this.yPositionOnScreen + Game1.tileSize * 3, false);
					base.drawVerticalIntersectingPartition(b, this.xPositionOnScreen + this.width / 2 - Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize * 3);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					b.DrawString(Game1.dialogueFont, this.title, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.title).X / 2f, (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), Game1.textColor);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + this.width - IClickableMenu.spaceToClearSideBorder - IClickableMenu.borderWidth - Game1.tileSize), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					string text = Game1.content.LoadString("Strings\\UI:LevelUp_ChooseProfession", new object[0]);
					b.DrawString(Game1.smallFont, text, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(text).X / 2f, (float)(this.yPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearTopBorder)), Game1.textColor);
					b.DrawString(Game1.dialogueFont, this.leftProfessionDescription[0], new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2)), this.leftProfessionColor);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2 - Game1.tileSize * 7 / 4), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 - Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.professionsToChoose[0] % 6 * 16, 624 + this.professionsToChoose[0] / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
					for (int i = 1; i < this.leftProfessionDescription.Count; i++)
					{
						b.DrawString(Game1.smallFont, Game1.parseText(this.leftProfessionDescription[i], Game1.smallFont, this.width / 2 - 64), new Vector2((float)(-4 + this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 2 + 8 + Game1.tileSize * (i + 1))), this.leftProfessionColor);
					}
					b.DrawString(Game1.dialogueFont, this.rightProfessionDescription[0], new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2)), this.rightProfessionColor);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width - Game1.tileSize * 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 - Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.professionsToChoose[1] % 6 * 16, 624 + this.professionsToChoose[1] / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
					for (int j = 1; j < this.rightProfessionDescription.Count; j++)
					{
						b.DrawString(Game1.smallFont, Game1.parseText(this.rightProfessionDescription[j], Game1.smallFont, this.width / 2 - 48), new Vector2((float)(-4 + this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 2 + 8 + Game1.tileSize * (j + 1))), this.rightProfessionColor);
					}
				}
				else
				{
					Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					b.DrawString(Game1.dialogueFont, this.title, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.title).X / 2f, (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), Game1.textColor);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + this.width - IClickableMenu.spaceToClearSideBorder - IClickableMenu.borderWidth - Game1.tileSize), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					int num = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 4;
					foreach (string current in this.extraInfoForLevel)
					{
						b.DrawString(Game1.smallFont, current, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(current).X / 2f, (float)num), Game1.textColor);
						num += Game1.tileSize * 3 / 4;
					}
					foreach (CraftingRecipe current2 in this.newCraftingRecipes)
					{
						string text2 = Game1.content.LoadString("Strings\\UI:LearnedRecipe_" + (current2.isCookingRecipe ? "cooking" : "crafting"), new object[0]);
						string text3 = Game1.content.LoadString("Strings\\UI:LevelUp_NewRecipe", new object[]
						{
							text2,
							current2.DisplayName
						});
						b.DrawString(Game1.smallFont, text3, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(text3).X / 2f - (float)Game1.tileSize, (float)(num + (current2.bigCraftable ? (Game1.tileSize * 3 / 5) : (Game1.tileSize / 5)))), Game1.textColor);
						current2.drawMenuView(b, (int)((float)(this.xPositionOnScreen + this.width / 2) + Game1.smallFont.MeasureString(text3).X / 2f - (float)(Game1.tileSize * 3 / 4)), num - Game1.tileSize / 4, 0.88f, true);
						num += (current2.bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize) + Game1.pixelZoom * 2;
					}
					this.okButton.draw(b);
				}
				base.drawMouse(b);
			}
		}
	}
}
