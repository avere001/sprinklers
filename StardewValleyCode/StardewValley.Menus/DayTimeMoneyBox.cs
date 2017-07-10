using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;

namespace StardewValley.Menus
{
	public class DayTimeMoneyBox : IClickableMenu
	{
		public new const int width = 300;

		public new const int height = 284;

		public Vector2 position;

		private Rectangle sourceRect;

		public MoneyDial moneyDial = new MoneyDial(8, true);

		public int timeShakeTimer;

		public int moneyShakeTimer;

		public int questPulseTimer;

		public int whenToPulseTimer;

		public ClickableTextureComponent questButton;

		public ClickableTextureComponent zoomOutButton;

		public ClickableTextureComponent zoomInButton;

		private string hoverText = "";

		public DayTimeMoneyBox() : base(Game1.viewport.Width - 300 + Game1.tileSize / 2, Game1.tileSize / 8, 300, 284, false)
		{
			this.position = new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen);
			this.sourceRect = new Rectangle(333, 431, 71, 43);
			this.questButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 220, this.yPositionOnScreen + 240, 44, 46), Game1.mouseCursors, new Rectangle(383, 493, 11, 14), 4f, false);
			this.zoomOutButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 23, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(177, 345, 7, 8), 4f, false);
			this.zoomInButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 31, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(184, 345, 7, 8), 4f, false);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			this.updatePosition();
			if (Game1.player.questLog.Count > 0 && this.questButton.containsPoint(x, y))
			{
				Game1.activeClickableMenu = new QuestLog();
			}
			if (Game1.options.zoomButtons)
			{
				if (this.zoomInButton.containsPoint(x, y) && Game1.options.zoomLevel < 1.25f)
				{
					int num = (int)Math.Round((double)(Game1.options.zoomLevel * 100f));
					num -= num % 5;
					num += 5;
					Game1.options.zoomLevel = Math.Min(1.25f, (float)num / 100f);
					Program.gamePtr.refreshWindowSettings();
					Game1.playSound("drumkit6");
					Game1.setMousePosition(this.zoomInButton.bounds.Center);
					return;
				}
				if (this.zoomOutButton.containsPoint(x, y) && Game1.options.zoomLevel > 0.75f)
				{
					int num2 = (int)Math.Round((double)(Game1.options.zoomLevel * 100f));
					num2 -= num2 % 5;
					num2 -= 5;
					Game1.options.zoomLevel = Math.Max(0.75f, (float)num2 / 100f);
					Program.gamePtr.refreshWindowSettings();
					Game1.playSound("drumkit6");
					Game1.setMousePosition(this.zoomOutButton.bounds.Center);
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.updatePosition();
		}

		public void questIconPulse()
		{
			this.questPulseTimer = 2000;
		}

		public override void performHoverAction(int x, int y)
		{
			this.updatePosition();
			this.hoverText = "";
			if (Game1.player.questLog.Count > 0 && this.questButton.containsPoint(x, y))
			{
				this.hoverText = Game1.content.LoadString("Strings\\UI:QuestButton_Hover", new object[]
				{
					Game1.options.journalButton[0].ToString()
				});
			}
			if (Game1.options.zoomButtons)
			{
				if (this.zoomInButton.containsPoint(x, y))
				{
					this.hoverText = Game1.content.LoadString("Strings\\UI:ZoomInButton_Hover", new object[0]);
				}
				if (this.zoomOutButton.containsPoint(x, y))
				{
					this.hoverText = Game1.content.LoadString("Strings\\UI:ZoomOutButton_Hover", new object[0]);
				}
			}
		}

		public void drawMoneyBox(SpriteBatch b, int overrideX = -1, int overrideY = -1)
		{
			this.updatePosition();
			b.Draw(Game1.mouseCursors, ((overrideY != -1) ? new Vector2((overrideX == -1) ? this.position.X : ((float)overrideX), (float)(overrideY - 172)) : this.position) + new Vector2((float)(28 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0)), (float)(172 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0))), new Rectangle?(new Rectangle(340, 472, 65, 17)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			this.moneyDial.draw(b, ((overrideY != -1) ? new Vector2((overrideX == -1) ? this.position.X : ((float)overrideX), (float)(overrideY - 172)) : this.position) + new Vector2((float)(68 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0)), (float)(196 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0))), Game1.player.money);
			if (this.moneyShakeTimer > 0)
			{
				this.moneyShakeTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			}
		}

		public override void draw(SpriteBatch b)
		{
			this.updatePosition();
			if (this.timeShakeTimer > 0)
			{
				this.timeShakeTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			}
			if (this.questPulseTimer > 0)
			{
				this.questPulseTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			}
			if (this.whenToPulseTimer >= 0)
			{
				this.whenToPulseTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (this.whenToPulseTimer <= 0)
				{
					this.whenToPulseTimer = 3000;
					if (Game1.player.hasNewQuestActivity())
					{
						this.questPulseTimer = 1000;
					}
				}
			}
			b.Draw(Game1.mouseCursors, this.position, new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			string text = (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja) ? string.Concat(new object[]
			{
				Game1.dayOfMonth,
				"日 (",
				Game1.shortDayDisplayNameFromDayOfSeason(Game1.dayOfMonth),
				")"
			}) : ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh) ? string.Concat(new object[]
			{
				Game1.shortDayDisplayNameFromDayOfSeason(Game1.dayOfMonth),
				" ",
				Game1.dayOfMonth,
				"日"
			}) : (Game1.shortDayDisplayNameFromDayOfSeason(Game1.dayOfMonth) + ". " + Game1.dayOfMonth));
			Vector2 vector = Game1.dialogueFont.MeasureString(text);
			Vector2 value = new Vector2((float)this.sourceRect.X * 0.55f - vector.X / 2f, (float)this.sourceRect.Y * (LocalizedContentManager.CurrentLanguageLatin ? 0.1f : 0.1f) - vector.Y / 2f);
			Utility.drawTextWithShadow(b, text, Game1.dialogueFont, this.position + value, Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
			b.Draw(Game1.mouseCursors, this.position + new Vector2(212f, 68f), new Rectangle?(new Rectangle(406, 441 + Utility.getSeasonNumber(Game1.currentSeason) * 8, 12, 8)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			b.Draw(Game1.mouseCursors, this.position + new Vector2(116f, 68f), new Rectangle?(new Rectangle(317 + 12 * Game1.weatherIcon, 421, 12, 8)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			string text2 = (Game1.timeOfDay % 100 == 0) ? "0" : "";
			string text3 = (Game1.timeOfDay / 100 % 12 == 0) ? "12" : string.Concat(Game1.timeOfDay / 100 % 12);
			switch (LocalizedContentManager.CurrentLanguageCode)
			{
			case LocalizedContentManager.LanguageCode.en:
			case LocalizedContentManager.LanguageCode.ja:
				text3 = ((Game1.timeOfDay / 100 % 12 == 0) ? "12" : string.Concat(Game1.timeOfDay / 100 % 12));
				break;
			case LocalizedContentManager.LanguageCode.ru:
			case LocalizedContentManager.LanguageCode.pt:
			case LocalizedContentManager.LanguageCode.es:
			case LocalizedContentManager.LanguageCode.de:
			case LocalizedContentManager.LanguageCode.th:
				text3 = string.Concat(Game1.timeOfDay / 100 % 24);
				text3 = ((Game1.timeOfDay / 100 % 24 <= 9) ? ("0" + text3) : text3);
				break;
			case LocalizedContentManager.LanguageCode.zh:
				text3 = ((Game1.timeOfDay / 100 % 24 == 0) ? "00" : ((Game1.timeOfDay / 100 % 12 == 0) ? "12" : string.Concat(Game1.timeOfDay / 100 % 12)));
				break;
			}
			string text4 = string.Concat(new object[]
			{
				text3,
				":",
				Game1.timeOfDay % 100,
				text2
			});
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
			{
				text4 = text4 + " " + ((Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371", new object[0]));
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
			{
				text4 = ((Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? (Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370", new object[0]) + " " + text4) : (Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371", new object[0]) + " " + text4));
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh)
			{
				text4 = ((Game1.timeOfDay < 600 || Game1.timeOfDay >= 2400) ? ("凌晨 " + text4) : ((Game1.timeOfDay < 1200) ? (Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370", new object[0]) + " " + text4) : ((Game1.timeOfDay < 1300) ? ("中午  " + text4) : ((Game1.timeOfDay < 1900) ? (Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371", new object[0]) + " " + text4) : ("晚上  " + text4)))));
			}
			Vector2 vector2 = Game1.dialogueFont.MeasureString(text4);
			Vector2 value2 = new Vector2((float)this.sourceRect.X * 0.55f - vector2.X / 2f + (float)((this.timeShakeTimer > 0) ? Game1.random.Next(-2, 3) : 0), (float)this.sourceRect.Y * (LocalizedContentManager.CurrentLanguageLatin ? 0.31f : 0.31f) - vector2.Y / 2f + (float)((this.timeShakeTimer > 0) ? Game1.random.Next(-2, 3) : 0));
			bool flag = Game1.shouldTimePass() || Game1.fadeToBlack || Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 2000.0 > 1000.0;
			Utility.drawTextWithShadow(b, text4, Game1.dialogueFont, this.position + value2, (Game1.timeOfDay >= 2400) ? Color.Red : (Game1.textColor * (flag ? 1f : 0.5f)), 1f, -1f, -1, -1, 1f, 3);
			int num = (int)((float)(Game1.timeOfDay - Game1.timeOfDay % 100) + (float)(Game1.timeOfDay % 100 / 10) * 16.66f);
			if (Game1.player.questLog.Count > 0)
			{
				this.questButton.draw(b);
				if (this.questPulseTimer > 0)
				{
					float num2 = 1f / (Math.Max(300f, (float)Math.Abs(this.questPulseTimer % 1000 - 500)) / 500f);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.questButton.bounds.X + 6 * Game1.pixelZoom), (float)(this.questButton.bounds.Y + 8 * Game1.pixelZoom)) + ((num2 > 1f) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(395, 497, 3, 8)), Color.White, 0f, new Vector2(2f, 4f), (float)Game1.pixelZoom * num2, SpriteEffects.None, 0.99f);
				}
			}
			if (Game1.options.zoomButtons)
			{
				this.zoomInButton.draw(b, Color.White * ((Game1.options.zoomLevel >= 1.25f) ? 0.5f : 1f), 1f);
				this.zoomOutButton.draw(b, Color.White * ((Game1.options.zoomLevel <= 0.75f) ? 0.5f : 1f), 1f);
			}
			this.drawMoneyBox(b, -1, -1);
			if (!this.hoverText.Equals("") && this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			b.Draw(Game1.mouseCursors, this.position + new Vector2(88f, 88f), new Rectangle?(new Rectangle(324, 477, 7, 19)), Color.White, (float)(3.1415926535897931 + Math.Min(3.1415926535897931, (double)(((float)num + (float)Game1.gameTimeInterval / 7000f * 16.6f - 600f) / 2000f) * 3.1415926535897931)), new Vector2(3f, 17f), 4f, SpriteEffects.None, 0.9f);
		}

		private void updatePosition()
		{
			this.position = new Vector2((float)(Game1.viewport.Width - 300), (float)(Game1.tileSize / 8));
			if (Game1.isOutdoorMapSmallerThanViewport())
			{
				this.position = new Vector2(Math.Min(this.position.X, (float)(-(float)Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize - 300)), (float)(Game1.tileSize / 8));
			}
			Utility.makeSafe(ref this.position, 300, 284);
			Game1.debugOutput = "position = " + this.position.X;
			this.xPositionOnScreen = (int)this.position.X;
			this.yPositionOnScreen = (int)this.position.Y;
			this.questButton.bounds = new Rectangle(this.xPositionOnScreen + 212, this.yPositionOnScreen + 240, 44, 46);
			this.zoomOutButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 23, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(177, 345, 7, 8), 4f, false);
			this.zoomInButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 31, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(184, 345, 7, 8), 4f, false);
		}
	}
}
