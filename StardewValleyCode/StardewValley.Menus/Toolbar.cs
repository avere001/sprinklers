using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class Toolbar : IClickableMenu
	{
		private List<ClickableComponent> buttons = new List<ClickableComponent>();

		private new int yPositionOnScreen;

		private string hoverTitle = "";

		private Item hoverItem;

		private float transparency = 1f;

		public Rectangle toolbarTextSource = new Rectangle(0, 256, 60, 60);

		public Toolbar() : base(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize, Game1.viewport.Height, Game1.tileSize * 14, Game1.tileSize * 3 + Game1.tileSize / 4, false)
		{
			for (int i = 0; i < 12; i++)
			{
				this.buttons.Add(new ClickableComponent(new Rectangle(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + i * Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8, Game1.tileSize, Game1.tileSize), string.Concat(i)));
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!Game1.player.usingTool)
			{
				foreach (ClickableComponent current in this.buttons)
				{
					if (current.containsPoint(x, y))
					{
						Game1.player.CurrentToolIndex = Convert.ToInt32(current.name);
						if (Game1.player.ActiveObject != null)
						{
							Game1.player.showCarrying();
							Game1.playSound("pickUpItem");
							break;
						}
						Game1.player.showNotCarrying();
						Game1.playSound("stoneStep");
						break;
					}
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			this.hoverItem = null;
			foreach (ClickableComponent current in this.buttons)
			{
				if (current.containsPoint(x, y))
				{
					int num = Convert.ToInt32(current.name);
					if (num < Game1.player.items.Count && Game1.player.items[num] != null)
					{
						current.scale = Math.Min(current.scale + 0.05f, 1.1f);
						this.hoverTitle = Game1.player.items[num].DisplayName;
						this.hoverItem = Game1.player.items[num];
					}
				}
				else
				{
					current.scale = Math.Max(current.scale - 0.025f, 1f);
				}
			}
		}

		public void shifted(bool right)
		{
			if (right)
			{
				for (int i = 0; i < this.buttons.Count; i++)
				{
					this.buttons[i].scale = 1f + (float)i * 0.03f;
				}
				return;
			}
			for (int j = this.buttons.Count - 1; j >= 0; j--)
			{
				this.buttons[j].scale = 1f + (float)(11 - j) * 0.03f;
			}
		}

		public override void update(GameTime time)
		{
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			for (int i = 0; i < 12; i++)
			{
				this.buttons[i].bounds = new Rectangle(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + i * Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8, Game1.tileSize, Game1.tileSize);
			}
		}

		public override bool isWithinBounds(int x, int y)
		{
			return new Rectangle(this.buttons.First<ClickableComponent>().bounds.X, this.buttons.First<ClickableComponent>().bounds.Y, this.buttons.Last<ClickableComponent>().bounds.X - this.buttons.First<ClickableComponent>().bounds.X + Game1.tileSize, Game1.tileSize).Contains(x, y);
		}

		public override void draw(SpriteBatch b)
		{
			if (Game1.activeClickableMenu != null)
			{
				return;
			}
			Point center = Game1.player.GetBoundingBox().Center;
			Vector2 globalPosition = new Vector2((float)center.X, (float)center.Y);
			Vector2 vector = Game1.GlobalToLocal(Game1.viewport, globalPosition);
			bool flag;
			if (Game1.options.pinToolbarToggle)
			{
				flag = false;
				this.transparency = Math.Min(1f, this.transparency + 0.075f);
				if (vector.Y > (float)(Game1.viewport.Height - Game1.tileSize * 3))
				{
					this.transparency = Math.Max(0.33f, this.transparency - 0.15f);
				}
			}
			else
			{
				flag = (vector.Y > (float)(Game1.viewport.Height / 2 + Game1.tileSize));
				this.transparency = 1f;
			}
			int num = Utility.makeSafeMarginY(8);
			int arg_146_0 = this.yPositionOnScreen;
			if (!flag)
			{
				this.yPositionOnScreen = Game1.viewport.Height;
				this.yPositionOnScreen += 8;
				this.yPositionOnScreen -= num;
			}
			else
			{
				this.yPositionOnScreen = Game1.tileSize + Game1.tileSize * 3 / 4;
				this.yPositionOnScreen -= 8;
				this.yPositionOnScreen += num;
			}
			if (arg_146_0 != this.yPositionOnScreen)
			{
				for (int i = 0; i < 12; i++)
				{
					this.buttons[i].bounds.Y = this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8;
				}
			}
			IClickableMenu.drawTextureBox(b, Game1.menuTexture, this.toolbarTextSource, Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.pixelZoom * 4, this.yPositionOnScreen - Game1.tileSize * 3 / 2 - Game1.pixelZoom * 2, Game1.tileSize * 12 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Color.White * this.transparency, 1f, false);
			for (int j = 0; j < 12; j++)
			{
				Vector2 vector2 = new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + j * Game1.tileSize), (float)(this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8));
				b.Draw(Game1.menuTexture, vector2, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, (Game1.player.CurrentToolIndex == j) ? 56 : 10, -1, -1)), Color.White * this.transparency);
				string text = (j == 9) ? "0" : ((j == 10) ? "-" : ((j == 11) ? "=" : string.Concat(j + 1)));
				b.DrawString(Game1.tinyFont, text, vector2 + new Vector2(4f, -8f), Color.DimGray * this.transparency);
			}
			for (int k = 0; k < 12; k++)
			{
				this.buttons[k].scale = Math.Max(1f, this.buttons[k].scale - 0.025f);
				Vector2 location = new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + k * Game1.tileSize), (float)(this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8));
				if (Game1.player.items.Count > k && Game1.player.items.ElementAt(k) != null)
				{
					Game1.player.items[k].drawInMenu(b, location, (Game1.player.CurrentToolIndex == k) ? 0.9f : (this.buttons.ElementAt(k).scale * 0.8f), this.transparency, 0.88f);
				}
			}
			if (this.hoverItem != null)
			{
				IClickableMenu.drawToolTip(b, this.hoverItem.getDescription(), this.hoverItem.DisplayName, this.hoverItem, false, -1, 0, -1, -1, null, -1);
				this.hoverItem = null;
			}
		}
	}
}
