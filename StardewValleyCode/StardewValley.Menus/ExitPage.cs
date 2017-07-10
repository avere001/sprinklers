using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Menus
{
	public class ExitPage : IClickableMenu
	{
		public ClickableComponent exitToTitle;

		public ClickableComponent exitToDesktop;

		public ExitPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			Vector2 vector = new Vector2((float)(this.xPositionOnScreen + width / 2 - (int)((Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToTitle", new object[0])).X + (float)Game1.tileSize) / 2f)), (float)(this.yPositionOnScreen + Game1.tileSize * 4 - Game1.tileSize / 2));
			this.exitToTitle = new ClickableComponent(new Rectangle((int)vector.X, (int)vector.Y, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToTitle", new object[0])).X + Game1.tileSize, Game1.tileSize * 3 / 2), "", Game1.content.LoadString("Strings\\UI:ExitToTitle", new object[0]))
			{
				myID = 535,
				upNeighborID = 12347,
				downNeighborID = 536
			};
			vector = new Vector2((float)(this.xPositionOnScreen + width / 2 - (int)((Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToDesktop", new object[0])).X + (float)Game1.tileSize) / 2f)), (float)(this.yPositionOnScreen + Game1.tileSize * 6 + Game1.pixelZoom * 2 - Game1.tileSize / 2));
			this.exitToDesktop = new ClickableComponent(new Rectangle((int)vector.X, (int)vector.Y, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToDesktop", new object[0])).X + Game1.tileSize, Game1.tileSize * 3 / 2), "", Game1.content.LoadString("Strings\\UI:ExitToDesktop", new object[0]))
			{
				myID = 536,
				upNeighborID = 535
			};
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(12347);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.conventionMode)
			{
				return;
			}
			if (this.exitToTitle.containsPoint(x, y) && this.exitToTitle.visible)
			{
				Game1.playSound("bigDeSelect");
				Game1.ExitToTitle();
			}
			if (this.exitToDesktop.containsPoint(x, y) && this.exitToDesktop.visible)
			{
				Game1.playSound("bigDeSelect");
				Game1.quit = true;
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			if (this.exitToTitle.containsPoint(x, y) && this.exitToTitle.visible)
			{
				if (this.exitToTitle.scale == 0f)
				{
					Game1.playSound("Cowboy_gunshot");
				}
				this.exitToTitle.scale = 1f;
			}
			else
			{
				this.exitToTitle.scale = 0f;
			}
			if (this.exitToDesktop.containsPoint(x, y) && this.exitToDesktop.visible)
			{
				if (this.exitToDesktop.scale == 0f)
				{
					Game1.playSound("Cowboy_gunshot");
				}
				this.exitToDesktop.scale = 1f;
				return;
			}
			this.exitToDesktop.scale = 0f;
		}

		public override void draw(SpriteBatch b)
		{
			if (this.exitToTitle.visible)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), this.exitToTitle.bounds.X, this.exitToTitle.bounds.Y, this.exitToTitle.bounds.Width, this.exitToTitle.bounds.Height, (this.exitToTitle.scale > 0f) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, true);
				Utility.drawTextWithShadow(b, this.exitToTitle.label, Game1.dialogueFont, new Vector2((float)this.exitToTitle.bounds.Center.X, (float)(this.exitToTitle.bounds.Center.Y + Game1.pixelZoom)) - Game1.dialogueFont.MeasureString(this.exitToTitle.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0f, 3);
			}
			if (this.exitToDesktop.visible)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), this.exitToDesktop.bounds.X, this.exitToDesktop.bounds.Y, this.exitToDesktop.bounds.Width, this.exitToDesktop.bounds.Height, (this.exitToDesktop.scale > 0f) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, true);
				Utility.drawTextWithShadow(b, this.exitToDesktop.label, Game1.dialogueFont, new Vector2((float)this.exitToDesktop.bounds.Center.X, (float)(this.exitToDesktop.bounds.Center.Y + Game1.pixelZoom)) - Game1.dialogueFont.MeasureString(this.exitToDesktop.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0f, 3);
			}
		}
	}
}
