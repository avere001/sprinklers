using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.Minigames
{
	public class RobotBlastoff : IMinigame
	{
		public const float backGroundSpeed = 0.25f;

		public const float robotSpeed = 0.3f;

		public const int skyLength = 2560;

		public int millisecondsSinceStart;

		public int backgroundPosition = -2560 + Game1.graphics.GraphicsDevice.Viewport.Height;

		public int smokeTimer = 500;

		public Vector2 robotPosition = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)Game1.graphics.GraphicsDevice.Viewport.Height);

		public List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();

		public bool overrideFreeMouseMovement()
		{
			return false;
		}

		public bool tick(GameTime time)
		{
			this.millisecondsSinceStart += time.ElapsedGameTime.Milliseconds;
			float num = 1.35f - 0.85f * (5f / Math.Max(5f, this.robotPosition.Y / 20f));
			this.backgroundPosition += (int)(0.25f * (float)time.ElapsedGameTime.Milliseconds * num) / 2;
			this.robotPosition.Y = this.robotPosition.Y - 0.3f * (float)time.ElapsedGameTime.Milliseconds / 4f;
			this.smokeTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.smokeTimer <= 0)
			{
				this.smokeTimer = 350;
				this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(143, 1828, 15, 20), 1500f, 4, 0, this.robotPosition + new Vector2(0f, (float)(18 * Game1.pixelZoom)), false, false)
				{
					motion = new Vector2(0f, -0.9f),
					acceleration = new Vector2(-0.001f, 0.006f),
					scale = (float)Game1.pixelZoom,
					scaleChange = 0.002f,
					alphaFade = 0.0025f
				});
			}
			for (int i = this.tempSprites.Count - 1; i >= 0; i--)
			{
				if (this.tempSprites[i].update(time))
				{
					this.tempSprites.RemoveAt(i);
				}
			}
			if (this.robotPosition.Y < 0f && Game1.random.NextDouble() < 0.005)
			{
				this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(256, 1680, 16, 16), 80f, 5, 0, new Vector2((float)Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Width), (float)Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Height / 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					motion = new Vector2(4f, 4f)
				});
			}
			if (this.robotPosition.Y < (float)(-(float)Game1.tileSize * 8) && !Game1.globalFade)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFade), 0.006f);
			}
			return false;
		}

		public void afterFade()
		{
			Game1.currentMinigame = null;
			Game1.globalFadeToClear(null, 0.02f);
			if (Game1.currentLocation.currentEvent != null)
			{
				Event expr_27 = Game1.currentLocation.currentEvent;
				int currentCommand = expr_27.CurrentCommand;
				expr_27.CurrentCommand = currentCommand + 1;
				Game1.currentLocation.temporarySprites.Clear();
			}
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
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
			if (k == Keys.Escape)
			{
				this.robotPosition.Y = -1000f;
				this.tempSprites.Clear();
			}
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.mouseCursors, new Rectangle(0, this.backgroundPosition, Game1.graphics.GraphicsDevice.Viewport.Width, 2560), new Rectangle?(new Rectangle(264, 1858, 1, 84)), Color.White);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)this.backgroundPosition), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.5f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.75f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, this.robotPosition + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)), new Rectangle?(new Rectangle(206 + this.millisecondsSinceStart / 50 % 4 * 15, 1827, 15, 27)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.tempSprites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b, true, 0, 0);
				}
			}
			b.End();
		}

		public void changeScreenSize()
		{
			this.backgroundPosition = 2560 - Game1.graphics.GraphicsDevice.Viewport.Height;
			this.robotPosition = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)Game1.graphics.GraphicsDevice.Viewport.Height);
		}

		public void unload()
		{
		}

		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		public string minigameId()
		{
			return null;
		}
	}
}
