using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Menus
{
	public class ButtonTutorialMenu : IClickableMenu
	{
		public const int move_run_check = 0;

		public const int useTool_menu = 1;

		public const float movementSpeed = 0.2f;

		public new const int width = 42;

		public new const int height = 109;

		private int timerToclose = 15000;

		private int which;

		private static int current;

		private int myID;

		public ButtonTutorialMenu(int which) : base(-42 * Game1.pixelZoom, Game1.viewport.Height / 2 - 109 * Game1.pixelZoom / 2, 42 * Game1.pixelZoom, 109 * Game1.pixelZoom, false)
		{
			this.which = which;
			ButtonTutorialMenu.current++;
			this.myID = ButtonTutorialMenu.current;
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.myID != ButtonTutorialMenu.current)
			{
				this.destroy = true;
			}
			if (this.xPositionOnScreen < 0 && this.timerToclose > 0)
			{
				this.xPositionOnScreen += (int)((float)time.ElapsedGameTime.Milliseconds * 0.2f);
				if (this.xPositionOnScreen >= 0)
				{
					this.xPositionOnScreen = 0;
					return;
				}
			}
			else
			{
				this.timerToclose -= time.ElapsedGameTime.Milliseconds;
				if (this.timerToclose <= 0)
				{
					if (this.xPositionOnScreen >= -42 * Game1.pixelZoom - Game1.tileSize)
					{
						this.xPositionOnScreen -= (int)((float)time.ElapsedGameTime.Milliseconds * 0.2f);
						return;
					}
					this.destroy = true;
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			if (this.destroy)
			{
				return;
			}
			if (!Game1.options.gamepadControls)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(new Rectangle(275 + this.which * 42, 0, 42, 109)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.82f);
				return;
			}
			b.Draw(Game1.controllerMaps, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(Utility.controllerMapSourceRect(new Rectangle(512 + this.which * 42 * 2, 0, 84, 218))), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom / 2f, SpriteEffects.None, 0.82f);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}
	}
}
