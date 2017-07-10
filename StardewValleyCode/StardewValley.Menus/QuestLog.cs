using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class QuestLog : IClickableMenu
	{
		public const int questsPerPage = 6;

		public const int region_forwardButton = 101;

		public const int region_backButton = 102;

		public const int region_rewardBox = 103;

		public const int region_cancelQuestButton = 104;

		private List<List<Quest>> pages;

		public List<ClickableComponent> questLogButtons;

		private int currentPage;

		private int questPage = -1;

		public ClickableTextureComponent forwardButton;

		public ClickableTextureComponent backButton;

		public ClickableTextureComponent rewardBox;

		public ClickableTextureComponent cancelQuestButton;

		private string hoverText = "";

		public QuestLog() : base(0, 0, 0, 0, true)
		{
			Game1.playSound("bigSelect");
			this.paginateQuests();
			this.width = Game1.tileSize * 13;
			this.height = Game1.tileSize * 9;
			Vector2 topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0);
			this.xPositionOnScreen = (int)topLeftPositionForCenteringOnScreen.X;
			this.yPositionOnScreen = (int)topLeftPositionForCenteringOnScreen.Y + Game1.tileSize / 2;
			this.questLogButtons = new List<ClickableComponent>();
			for (int i = 0; i < 6; i++)
			{
				this.questLogButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + i * ((this.height - Game1.tileSize / 2) / 6), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize / 2) / 6 + Game1.pixelZoom), string.Concat(i))
				{
					myID = i,
					downNeighborID = -7777,
					upNeighborID = ((i > 0) ? (i - 1) : -1),
					rightNeighborID = -7777,
					leftNeighborID = -7777,
					fullyImmutable = true
				});
			}
			this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 5 * Game1.pixelZoom, this.yPositionOnScreen - 2 * Game1.pixelZoom, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize, this.yPositionOnScreen + Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 102,
				rightNeighborID = -7777
			};
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - 12 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 101
			};
			this.rewardBox = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.pixelZoom * 20, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(293, 360, 24, 24), (float)Game1.pixelZoom, true)
			{
				myID = 103
			};
			this.cancelQuestButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom, this.yPositionOnScreen + this.height + Game1.pixelZoom, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float)Game1.pixelZoom, true)
			{
				myID = 104
			};
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
			if (oldID >= 0 && oldID < 6 && this.questPage == -1)
			{
				if (direction == 2)
				{
					if (oldID < 5 && this.pages[this.currentPage].Count - 1 > oldID)
					{
						this.currentlySnappedComponent = base.getComponentWithID(oldID + 1);
					}
				}
				else if (direction == 1)
				{
					if (this.currentPage < this.pages.Count - 1)
					{
						this.currentlySnappedComponent = base.getComponentWithID(101);
						this.currentlySnappedComponent.leftNeighborID = oldID;
					}
				}
				else if (direction == 3 && this.currentPage > 0)
				{
					this.currentlySnappedComponent = base.getComponentWithID(102);
					this.currentlySnappedComponent.rightNeighborID = oldID;
				}
			}
			else if (oldID == 102)
			{
				if (this.questPage != -1)
				{
					return;
				}
				this.currentlySnappedComponent = base.getComponentWithID(0);
			}
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(0);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void receiveGamePadButton(Buttons b)
		{
			if (b == Buttons.RightTrigger && this.questPage == -1 && this.currentPage < this.pages.Count - 1)
			{
				this.nonQuestPageForwardButton();
				return;
			}
			if (b == Buttons.LeftTrigger && this.questPage == -1 && this.currentPage > 0)
			{
				this.nonQuestPageBackButton();
			}
		}

		private void paginateQuests()
		{
			this.pages = new List<List<Quest>>();
			for (int i = Game1.player.questLog.Count - 1; i >= 0; i--)
			{
				if (Game1.player.questLog[i] == null || Game1.player.questLog[i].destroy)
				{
					Game1.player.questLog.RemoveAt(i);
				}
				else
				{
					int num = Game1.player.questLog.Count - 1 - i;
					if (this.pages.Count <= num / 6)
					{
						this.pages.Add(new List<Quest>());
					}
					this.pages[num / 6].Add(Game1.player.questLog[i]);
				}
			}
			this.currentPage = Math.Min(Math.Max(this.currentPage, 0), this.pages.Count - 1);
			this.questPage = -1;
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			base.performHoverAction(x, y);
			if (this.questPage == -1)
			{
				for (int i = 0; i < this.questLogButtons.Count; i++)
				{
					if (this.pages.Count > 0 && this.pages[0].Count > i && this.questLogButtons[i].containsPoint(x, y) && !this.questLogButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
					{
						Game1.playSound("Cowboy_gunshot");
					}
				}
			}
			else if (this.pages[this.currentPage][this.questPage].canBeCancelled && this.cancelQuestButton.containsPoint(x, y))
			{
				this.hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11364", new object[0]);
			}
			this.forwardButton.tryHover(x, y, 0.2f);
			this.backButton.tryHover(x, y, 0.2f);
			this.cancelQuestButton.tryHover(x, y, 0.2f);
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (Game1.options.doesInputListContain(Game1.options.journalButton, key) && this.readyToClose())
			{
				Game1.exitActiveMenu();
				Game1.playSound("bigDeSelect");
			}
		}

		private void nonQuestPageForwardButton()
		{
			this.currentPage++;
			Game1.playSound("shwip");
			if (Game1.options.SnappyMenus && this.currentPage == this.pages.Count - 1)
			{
				this.currentlySnappedComponent = base.getComponentWithID(0);
				this.snapCursorToCurrentSnappedComponent();
			}
		}

		private void nonQuestPageBackButton()
		{
			this.currentPage--;
			Game1.playSound("shwip");
			if (Game1.options.SnappyMenus && this.currentPage == 0)
			{
				this.currentlySnappedComponent = base.getComponentWithID(0);
				this.snapCursorToCurrentSnappedComponent();
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (Game1.activeClickableMenu == null)
			{
				return;
			}
			if (this.questPage == -1)
			{
				for (int i = 0; i < this.questLogButtons.Count; i++)
				{
					if (this.pages.Count > 0 && this.pages[this.currentPage].Count > i && this.questLogButtons[i].containsPoint(x, y))
					{
						Game1.playSound("smallSelect");
						this.questPage = i;
						this.pages[this.currentPage][i].showNew = false;
						if (Game1.options.SnappyMenus)
						{
							this.currentlySnappedComponent = base.getComponentWithID(102);
							this.currentlySnappedComponent.downNeighborID = ((this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].moneyReward > 0) ? 103 : (this.pages[this.currentPage][this.questPage].canBeCancelled ? 104 : -1));
							this.snapCursorToCurrentSnappedComponent();
						}
						return;
					}
				}
				if (this.currentPage < this.pages.Count - 1 && this.forwardButton.containsPoint(x, y))
				{
					this.nonQuestPageForwardButton();
					return;
				}
				if (this.currentPage > 0 && this.backButton.containsPoint(x, y))
				{
					this.nonQuestPageBackButton();
					return;
				}
				Game1.playSound("bigDeSelect");
				base.exitThisMenu(true);
				return;
			}
			else
			{
				if (this.questPage != -1 && this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].moneyReward > 0 && this.rewardBox.containsPoint(x, y))
				{
					Game1.player.Money += this.pages[this.currentPage][this.questPage].moneyReward;
					Game1.playSound("purchaseRepeat");
					this.pages[this.currentPage][this.questPage].moneyReward = 0;
					this.pages[this.currentPage][this.questPage].destroy = true;
					return;
				}
				if (this.questPage != -1 && !this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].canBeCancelled && this.cancelQuestButton.containsPoint(x, y))
				{
					this.pages[this.currentPage][this.questPage].accepted = false;
					Game1.player.questLog.Remove(this.pages[this.currentPage][this.questPage]);
					this.pages[this.currentPage].RemoveAt(this.questPage);
					this.questPage = -1;
					Game1.playSound("trashcan");
					if (Game1.options.SnappyMenus && this.currentPage == 0)
					{
						this.currentlySnappedComponent = base.getComponentWithID(0);
						this.snapCursorToCurrentSnappedComponent();
						return;
					}
				}
				else
				{
					this.exitQuestPage();
				}
				return;
			}
		}

		public void exitQuestPage()
		{
			if (this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].moneyReward <= 0)
			{
				this.pages[this.currentPage][this.questPage].destroy = true;
			}
			if (this.pages[this.currentPage][this.questPage].destroy)
			{
				Game1.player.questLog.Remove(this.pages[this.currentPage][this.questPage]);
				this.pages[this.currentPage].RemoveAt(this.questPage);
			}
			this.questPage = -1;
			this.paginateQuests();
			Game1.playSound("shwip");
			if (Game1.options.SnappyMenus)
			{
				this.snapToDefaultClickableComponent();
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.questPage != -1 && this.pages[this.currentPage][this.questPage].hasReward())
			{
				this.rewardBox.scale = this.rewardBox.baseScale + Game1.dialogueButtonScale / 20f;
			}
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11373", new object[0]), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen - Game1.tileSize, "", 1f, -1, 0, 0.88f, false);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.White, (float)Game1.pixelZoom, true);
			if (this.questPage == -1)
			{
				for (int i = 0; i < this.questLogButtons.Count; i++)
				{
					if (this.pages.Count<List<Quest>>() > 0 && this.pages[this.currentPage].Count<Quest>() > i)
					{
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.questLogButtons[i].bounds.X, this.questLogButtons[i].bounds.Y, this.questLogButtons[i].bounds.Width, this.questLogButtons[i].bounds.Height, this.questLogButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, false);
						if (this.pages[this.currentPage][i].showNew || this.pages[this.currentPage][i].completed)
						{
							Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.questLogButtons[i].bounds.X + Game1.tileSize + Game1.pixelZoom), (float)(this.questLogButtons[i].bounds.Y + Game1.pixelZoom * 11)), new Rectangle(this.pages[this.currentPage][i].completed ? 341 : 317, 410, 23, 9), Color.White, 0f, new Vector2(11f, 4f), (float)Game1.pixelZoom + Game1.dialogueButtonScale * 10f / 250f, false, 0.99f, -1, -1, 0.35f);
						}
						else
						{
							Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.questLogButtons[i].bounds.X + Game1.tileSize / 2), (float)(this.questLogButtons[i].bounds.Y + Game1.pixelZoom * 7)), this.pages[this.currentPage][i].dailyQuest ? new Rectangle(410, 501, 9, 9) : new Rectangle(395 + (this.pages[this.currentPage][i].dailyQuest ? 3 : 0), 497, 3, 8), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.99f, -1, -1, 0.35f);
						}
						bool arg_3A1_0 = this.pages[this.currentPage][i].dailyQuest;
						SpriteText.drawString(b, this.pages[this.currentPage][i].questTitle, this.questLogButtons[i].bounds.X + Game1.tileSize * 2 + Game1.pixelZoom, this.questLogButtons[i].bounds.Y + Game1.pixelZoom * 5, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					}
				}
			}
			else
			{
				SpriteText.drawStringHorizontallyCenteredAt(b, this.pages[this.currentPage][this.questPage].questTitle, this.xPositionOnScreen + this.width / 2 + ((this.pages[this.currentPage][this.questPage].dailyQuest && this.pages[this.currentPage][this.questPage].daysLeft > 0) ? (Math.Max(8 * Game1.pixelZoom, SpriteText.getWidthOfString(this.pages[this.currentPage][this.questPage].questTitle) / 3) - 8 * Game1.pixelZoom) : 0), this.yPositionOnScreen + Game1.tileSize / 2, 999999, -1, 999999, 1f, 0.88f, false, -1);
				if (this.pages[this.currentPage][this.questPage].dailyQuest && this.pages[this.currentPage][this.questPage].daysLeft > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.pixelZoom * 8), (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2)), new Rectangle(410, 501, 9, 9), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.99f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.parseText((this.pages[this.currentPage][this.questPage].daysLeft > 1) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11374", new object[]
					{
						this.pages[this.currentPage][this.questPage].daysLeft
					}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11375", new object[]
					{
						this.pages[this.currentPage][this.questPage].daysLeft
					}), Game1.dialogueFont, this.width - Game1.tileSize * 2), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + 20 * Game1.pixelZoom), (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
				Utility.drawTextWithShadow(b, Game1.parseText(this.pages[this.currentPage][this.questPage].questDescription, Game1.dialogueFont, this.width - Game1.tileSize * 2), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize), (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				float num = (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 2) + Game1.dialogueFont.MeasureString(Game1.parseText(this.pages[this.currentPage][this.questPage].questDescription, Game1.dialogueFont, this.width - Game1.tileSize * 2)).Y + (float)(Game1.tileSize / 2);
				if (this.pages[this.currentPage][this.questPage].completed)
				{
					SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11376", new object[0]), this.xPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom, this.rewardBox.bounds.Y + Game1.tileSize / 3 + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					this.rewardBox.draw(b);
					if (this.pages[this.currentPage][this.questPage].moneyReward > 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.rewardBox.bounds.X + Game1.pixelZoom * 4), (float)(this.rewardBox.bounds.Y + Game1.pixelZoom * 4) - Game1.dialogueButtonScale / 2f), new Rectangle?(new Rectangle(280, 410, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
						SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
						{
							this.pages[this.currentPage][this.questPage].moneyReward
						}), this.xPositionOnScreen + Game1.tileSize * 7, this.rewardBox.bounds.Y + Game1.tileSize / 3 + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					}
				}
				else
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 / 2) + (float)(Game1.pixelZoom * 2) * Game1.dialogueButtonScale / 10f, num), new Rectangle(412, 495, 5, 4), Color.White, 1.57079637f, Vector2.Zero, -1f, false, -1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.parseText(this.pages[this.currentPage][this.questPage].currentObjective, Game1.dialogueFont, this.width - Game1.tileSize * 4), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 8 / 4), num - (float)(Game1.pixelZoom * 2)), Color.DarkBlue, 1f, -1f, -1, -1, 1f, 3);
					if (this.pages[this.currentPage][this.questPage].canBeCancelled)
					{
						this.cancelQuestButton.draw(b);
					}
				}
			}
			if (this.currentPage < this.pages.Count - 1 && this.questPage == -1)
			{
				this.forwardButton.draw(b);
			}
			if (this.currentPage > 0 || this.questPage != -1)
			{
				this.backButton.draw(b);
			}
			base.draw(b);
			Game1.mouseCursorTransparency = 1f;
			base.drawMouse(b);
			if (this.hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
