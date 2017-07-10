using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Objects;
using System;
using System.Collections.Generic;

namespace StardewValley.Minigames
{
	public class Test : IMinigame
	{
		public List<Wallpaper> wallpaper = new List<Wallpaper>();

		public Test()
		{
			for (int i = 0; i < 40; i++)
			{
				this.wallpaper.Add(new Wallpaper(i, true));
			}
		}

		public bool overrideFreeMouseMovement()
		{
			return false;
		}

		public bool tick(GameTime time)
		{
			return false;
		}

		public void afterFade()
		{
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			Game1.currentMinigame = null;
		}

		public void leftClickHeld(int x, int y)
		{
		}

		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public void releaseLeftClick(int x, int y)
		{
		}

		public void releaseRightClick(int x, int y)
		{
		}

		public void receiveKeyPress(Keys k)
		{
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, 2000, 2000), Color.White);
			Vector2 vector = new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4));
			for (int i = 0; i < this.wallpaper.Count; i++)
			{
				this.wallpaper[i].drawInMenu(b, vector, 1f);
				vector.X += (float)(Game1.tileSize * 2);
				if (vector.X >= (float)(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2))
				{
					vector.X = (float)(Game1.tileSize / 4);
					vector.Y += (float)(Game1.tileSize * 2);
				}
			}
			b.End();
		}

		public void changeScreenSize()
		{
		}

		public void unload()
		{
		}

		public void receiveEventPoke(int data)
		{
		}

		public string minigameId()
		{
			return null;
		}
	}
}
