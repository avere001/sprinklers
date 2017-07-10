using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class Billboard : IClickableMenu
	{
		private Texture2D billboardTexture;

		public const int basewidth = 338;

		public const int baseWidth_calendar = 301;

		public const int baseheight = 198;

		private bool dailyQuestBoard;

		public ClickableComponent acceptQuestButton;

		public List<ClickableTextureComponent> calendarDays;

		private string hoverText = "";

		public Billboard(bool dailyQuest = false) : base(0, 0, 0, 0, true)
		{
			if (!Game1.player.hasOrWillReceiveMail("checkedBulletinOnce"))
			{
				Game1.player.mailReceived.Add("checkedBulletinOnce");
				(Game1.getLocationFromName("Town") as Town).checkedBoard();
			}
			this.dailyQuestBoard = dailyQuest;
			this.billboardTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\Billboard");
			this.width = (dailyQuest ? 338 : 301) * Game1.pixelZoom;
			this.height = 198 * Game1.pixelZoom;
			Vector2 topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0);
			this.xPositionOnScreen = (int)topLeftPositionForCenteringOnScreen.X;
			this.yPositionOnScreen = (int)topLeftPositionForCenteringOnScreen.Y;
			this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0])).X + Game1.pixelZoom * 6, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0])).Y + Game1.pixelZoom * 6), "")
			{
				myID = 0
			};
			this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 5 * Game1.pixelZoom, this.yPositionOnScreen, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
			Game1.playSound("bigSelect");
			if (!dailyQuest)
			{
				this.calendarDays = new List<ClickableTextureComponent>();
				Dictionary<int, NPC> dictionary = new Dictionary<int, NPC>();
				foreach (NPC current in Utility.getAllCharacters())
				{
					if (current.birthday_Season != null && current.birthday_Season.Equals(Game1.currentSeason) && !dictionary.ContainsKey(current.birthday_Day) && (Game1.player.friendships.ContainsKey(current.name) || (!current.name.Equals("Dwarf") && !current.name.Equals("Sandy") && !current.name.Equals("Krobus"))))
					{
						dictionary.Add(current.birthday_Day, current);
					}
				}
				for (int i = 1; i <= 28; i++)
				{
					string text = "";
					string text2 = "";
					NPC nPC = dictionary.ContainsKey(i) ? dictionary[i] : null;
					if (Utility.isFestivalDay(i, Game1.currentSeason))
					{
						text = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + i)["name"];
					}
					else if (nPC != null)
					{
						if (nPC.name.Last<char>() == 's' || (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.de && (nPC.name.Last<char>() == 'x' || nPC.name.Last<char>() == 'ß' || nPC.name.Last<char>() == 'z')))
						{
							text2 = Game1.content.LoadString("Strings\\UI:Billboard_SBirthday", new object[]
							{
								nPC.displayName
							});
						}
						else
						{
							text2 = Game1.content.LoadString("Strings\\UI:Billboard_Birthday", new object[]
							{
								nPC.displayName
							});
						}
					}
					this.calendarDays.Add(new ClickableTextureComponent(text, new Rectangle(this.xPositionOnScreen + 38 * Game1.pixelZoom + (i - 1) % 7 * 32 * Game1.pixelZoom, this.yPositionOnScreen + 50 * Game1.pixelZoom + (i - 1) / 7 * 32 * Game1.pixelZoom, 31 * Game1.pixelZoom, 31 * Game1.pixelZoom), text, text2, (nPC != null) ? nPC.sprite.Texture : null, (nPC != null) ? new Rectangle(0, 0, 16, 24) : Rectangle.Empty, 1f, false)
					{
						myID = i,
						rightNeighborID = ((i % 7 != 0) ? (i + 1) : -1),
						leftNeighborID = ((i % 7 != 1) ? (i - 1) : -1),
						downNeighborID = i + 7,
						upNeighborID = ((i > 7) ? (i - 7) : -1)
					});
				}
			}
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(this.dailyQuestBoard ? 0 : 1);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			Game1.activeClickableMenu = new Billboard(this.dailyQuestBoard);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			Game1.playSound("bigDeSelect");
			base.exitThisMenu(true);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (this.dailyQuestBoard && Game1.questOfTheDay != null && (!Game1.questOfTheDay.accepted || Game1.questOfTheDay.currentObjective == null || Game1.questOfTheDay.currentObjective.Length == 0) && this.acceptQuestButton.containsPoint(x, y))
			{
				Game1.playSound("newArtifact");
				Game1.questOfTheDay.dailyQuest = true;
				Game1.questOfTheDay.accepted = true;
				Game1.questOfTheDay.canBeCancelled = true;
				Game1.questOfTheDay.daysLeft = 2;
				Game1.player.questLog.Add(Game1.questOfTheDay);
			}
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.hoverText = "";
			if (this.dailyQuestBoard && Game1.questOfTheDay != null && !Game1.questOfTheDay.accepted)
			{
				float scale = this.acceptQuestButton.scale;
				this.acceptQuestButton.scale = (this.acceptQuestButton.bounds.Contains(x, y) ? 1.5f : 1f);
				if (this.acceptQuestButton.scale > scale)
				{
					Game1.playSound("Cowboy_gunshot");
				}
			}
			if (this.calendarDays != null)
			{
				foreach (ClickableTextureComponent current in this.calendarDays)
				{
					if (current.bounds.Contains(x, y))
					{
						if (current.hoverText.Length > 0)
						{
							this.hoverText = current.hoverText;
						}
						else
						{
							this.hoverText = current.label;
						}
					}
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			b.Draw(this.billboardTexture, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(this.dailyQuestBoard ? new Rectangle(0, 0, 338, 198) : new Rectangle(0, 198, 301, 198)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			if (!this.dailyQuestBoard)
			{
				b.DrawString(Game1.dialogueFont, Utility.getSeasonNameFromNumber(Utility.getSeasonNumber(Game1.currentSeason)), new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 / 2), (float)(this.yPositionOnScreen + Game1.tileSize * 5 / 4)), Game1.textColor);
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:Billboard_Year", new object[]
				{
					Game1.year
				}), new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 7), (float)(this.yPositionOnScreen + Game1.tileSize * 5 / 4)), Game1.textColor);
				for (int i = 0; i < this.calendarDays.Count; i++)
				{
					if (this.calendarDays[i].name.Length > 0)
					{
						Utility.drawWithShadow(b, this.billboardTexture, new Vector2((float)(this.calendarDays[i].bounds.X + Game1.pixelZoom * 10), (float)(this.calendarDays[i].bounds.Y + Game1.pixelZoom * 14) - Game1.dialogueButtonScale / 2f), new Rectangle(1 + (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 600.0 / 100.0) * 14, 398, 14, 12), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					}
					else if (this.calendarDays[i].hoverText.Length > 0)
					{
						b.Draw(this.calendarDays[i].texture, new Vector2((float)(this.calendarDays[i].bounds.X + Game1.pixelZoom * 10), (float)(this.calendarDays[i].bounds.Y + 7 * Game1.pixelZoom)), new Rectangle?(this.calendarDays[i].sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
					}
					if (Game1.dayOfMonth > i + 1)
					{
						b.Draw(Game1.staminaRect, this.calendarDays[i].bounds, Color.Gray * 0.25f);
					}
					else if (Game1.dayOfMonth == i + 1)
					{
						int num = (int)((float)Game1.pixelZoom * Game1.dialogueButtonScale / 8f);
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), this.calendarDays[i].bounds.X - num, this.calendarDays[i].bounds.Y - num, this.calendarDays[i].bounds.Width + num * 2, this.calendarDays[i].bounds.Height + num * 2, Color.Blue, (float)Game1.pixelZoom, false);
					}
				}
			}
			else if (Game1.questOfTheDay == null || Game1.questOfTheDay.currentObjective == null || Game1.questOfTheDay.currentObjective.Length == 0)
			{
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:Billboard_NothingPosted", new object[0]), new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6), (float)(this.yPositionOnScreen + Game1.tileSize * 5)), Game1.textColor);
			}
			else
			{
				string text = Game1.parseText(Game1.questOfTheDay.questDescription, Game1.dialogueFont, Game1.tileSize * 10);
				Utility.drawTextWithShadow(b, text, Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 + Game1.tileSize / 2), (float)(this.yPositionOnScreen + Game1.tileSize * 4)), Game1.textColor, 1f, -1f, -1, -1, 0.5f, 3);
				if (!Game1.questOfTheDay.accepted)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.acceptQuestButton.bounds.X, this.acceptQuestButton.bounds.Y, this.acceptQuestButton.bounds.Width, this.acceptQuestButton.bounds.Height, (this.acceptQuestButton.scale > 1f) ? Color.LightPink : Color.White, (float)Game1.pixelZoom * this.acceptQuestButton.scale, true);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0]), Game1.dialogueFont, new Vector2((float)(this.acceptQuestButton.bounds.X + Game1.pixelZoom * 3), (float)(this.acceptQuestButton.bounds.Y + (LocalizedContentManager.CurrentLanguageLatin ? (Game1.pixelZoom * 4) : (Game1.pixelZoom * 3)))), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
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
