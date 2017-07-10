using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class MineElevatorMenu : IClickableMenu
	{
		public List<ClickableComponent> elevators = new List<ClickableComponent>();

		public MineElevatorMenu() : base(0, 0, 0, 0, true)
		{
			int num = Math.Min(Game1.mine.lowestLevelReached, 120) / 5;
			this.width = ((num > 50) ? ((Game1.tileSize * 3 / 4 - 4) * 11 + IClickableMenu.borderWidth * 2) : Math.Min((Game1.tileSize * 3 / 4 - 4) * 5 + IClickableMenu.borderWidth * 2, num * (Game1.tileSize * 3 / 4 - 4) + IClickableMenu.borderWidth * 2));
			this.height = Math.Max(Game1.tileSize + IClickableMenu.borderWidth * 3, num * (Game1.tileSize * 3 / 4 - 4) / (this.width - IClickableMenu.borderWidth) * (Game1.tileSize * 3 / 4 - 4) + Game1.tileSize + IClickableMenu.borderWidth * 3);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - this.width / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - this.height / 2;
			Game1.playSound("crystal");
			int num2 = this.width / (Game1.tileSize - 20) - 1;
			int num3 = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
			int num4 = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.borderWidth / 3;
			this.elevators.Add(new ClickableComponent(new Rectangle(num3, num4, Game1.tileSize * 3 / 4 - 4, Game1.tileSize * 3 / 4 - 4), string.Concat(0))
			{
				myID = 0,
				rightNeighborID = 1,
				downNeighborID = num2
			});
			num3 = num3 + Game1.tileSize - 20;
			if (num3 > this.xPositionOnScreen + this.width - IClickableMenu.borderWidth)
			{
				num3 = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
				num4 += Game1.tileSize - 20;
			}
			for (int i = 1; i <= num; i++)
			{
				this.elevators.Add(new ClickableComponent(new Rectangle(num3, num4, Game1.tileSize * 3 / 4 - 4, Game1.tileSize * 3 / 4 - 4), string.Concat(i * 5))
				{
					myID = i,
					rightNeighborID = ((i % num2 == num2 - 1) ? -1 : (i + 1)),
					leftNeighborID = ((i % num2 == 0) ? -1 : (i - 1)),
					downNeighborID = i + num2,
					upNeighborID = i - num2
				});
				num3 = num3 + Game1.tileSize - 20;
				if (num3 > this.xPositionOnScreen + this.width - IClickableMenu.borderWidth)
				{
					num3 = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
					num4 += Game1.tileSize - 20;
				}
			}
			base.initializeUpperRightCloseButton();
			if (Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(0);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.isWithinBounds(x, y))
			{
				foreach (ClickableComponent current in this.elevators)
				{
					if (current.containsPoint(x, y))
					{
						Game1.playSound("smallSelect");
						if (Convert.ToInt32(current.name) == 0)
						{
							if (!Game1.currentLocation.Equals(Game1.mine))
							{
								return;
							}
							Game1.warpFarmer("Mine", 17, 4, true);
							Game1.exitActiveMenu();
							Game1.changeMusicTrack("none");
						}
						else
						{
							if (Game1.currentLocation.Equals(Game1.mine) && Convert.ToInt32(current.name) == Game1.mine.mineLevel)
							{
								return;
							}
							Game1.player.ridingMineElevator = true;
							Game1.enterMine(false, Convert.ToInt32(current.name), null);
							Game1.exitActiveMenu();
						}
					}
				}
				base.receiveLeftClick(x, y, true);
				return;
			}
			Game1.exitActiveMenu();
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			foreach (ClickableComponent current in this.elevators)
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
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize + Game1.tileSize / 8, this.width + Game1.tileSize / 3, this.height + Game1.tileSize, false, true, null, false);
			foreach (ClickableComponent current in this.elevators)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(current.bounds.X - Game1.pixelZoom), (float)(current.bounds.Y + Game1.pixelZoom)), new Rectangle?(new Rectangle((current.scale > 1f) ? 267 : 256, 256, 10, 10)), Color.Black * 0.5f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.865f);
				b.Draw(Game1.mouseCursors, new Vector2((float)current.bounds.X, (float)current.bounds.Y), new Rectangle?(new Rectangle((current.scale > 1f) ? 267 : 256, 256, 10, 10)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.868f);
				Vector2 position = new Vector2((float)(current.bounds.X + 16 + NumberSprite.numberOfDigits(Convert.ToInt32(current.name)) * 6), (float)(current.bounds.Y + Game1.pixelZoom * 6 - NumberSprite.getHeight() / 4));
				NumberSprite.draw(Convert.ToInt32(current.name), b, position, ((Game1.mine.mineLevel == Convert.ToInt32(current.name) && Game1.currentLocation.Equals(Game1.mine)) || (Convert.ToInt32(current.name) == 0 && !Game1.currentLocation.Equals(Game1.mine))) ? (Color.Gray * 0.75f) : Color.Gold, 0.5f, 0.86f, 1f, 0, 0);
			}
			base.drawMouse(b);
			base.draw(b);
		}
	}
}
