using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class TutorialMenu : IClickableMenu
	{
		public const int farmingTab = 0;

		public const int fishingTab = 1;

		public const int miningTab = 2;

		public const int craftingTab = 3;

		public const int constructionTab = 4;

		public const int friendshipTab = 5;

		public const int townTab = 6;

		public const int animalsTab = 7;

		private int currentTab = -1;

		private List<ClickableTextureComponent> topics = new List<ClickableTextureComponent>();

		private ClickableTextureComponent backButton;

		private ClickableTextureComponent okButton;

		private List<ClickableTextureComponent> icons = new List<ClickableTextureComponent>();

		public TutorialMenu() : base(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize * 3, 600 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize * 3, false)
		{
			int x = this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2;
			int num = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11805", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 276, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11807", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 142, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11809", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 334, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11811", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 308, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11813", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 395, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11815", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 458, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11817", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 102, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(x, num, this.width, Game1.tileSize), Game1.content.LoadString("Strings\\StringsFromCSFiles:TutorialMenu.cs.11819", new object[0]), "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(x, num, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 403, -1, -1), 1f, false));
			num += Game1.tileSize + 4;
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 3 / 4, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.currentTab == -1)
			{
				for (int i = 0; i < this.topics.Count; i++)
				{
					if (this.topics[i].containsPoint(x, y))
					{
						this.currentTab = i;
						Game1.playSound("smallSelect");
						break;
					}
				}
			}
			if (this.currentTab != -1 && this.backButton.containsPoint(x, y))
			{
				this.currentTab = -1;
				Game1.playSound("bigDeSelect");
				return;
			}
			if (this.currentTab == -1 && this.okButton.containsPoint(x, y))
			{
				Game1.playSound("bigDeSelect");
				Game1.exitActiveMenu();
				if (Game1.currentLocation.currentEvent != null)
				{
					Event expr_AE = Game1.currentLocation.currentEvent;
					int currentCommand = expr_AE.CurrentCommand;
					expr_AE.CurrentCommand = currentCommand + 1;
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			foreach (ClickableComponent current in this.topics)
			{
				if (current.containsPoint(x, y))
				{
					current.scale = 2f;
				}
				else
				{
					current.scale = 1f;
				}
			}
			if (this.okButton.containsPoint(x, y))
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
			}
			else
			{
				this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
			}
			if (this.backButton.containsPoint(x, y))
			{
				this.backButton.scale = Math.Min(this.backButton.scale + 0.02f, this.backButton.baseScale + 0.1f);
				return;
			}
			this.backButton.scale = Math.Max(this.backButton.scale - 0.02f, this.backButton.baseScale);
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			if (this.currentTab != -1)
			{
				this.backButton.draw(b);
				b.Draw(this.topics[this.currentTab].texture, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), new Rectangle?(this.topics[this.currentTab].texture.Bounds), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.89f);
			}
			else
			{
				foreach (ClickableTextureComponent current in this.topics)
				{
					Color color = (current.scale > 1f) ? Color.Blue : Game1.textColor;
					b.DrawString(Game1.smallFont, current.label, new Vector2((float)(current.bounds.X + Game1.tileSize + 16), (float)(current.bounds.Y + Game1.tileSize / 3)), color);
				}
				using (List<ClickableTextureComponent>.Enumerator enumerator = this.icons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				this.okButton.draw(b);
			}
			base.drawMouse(b);
		}
	}
}
