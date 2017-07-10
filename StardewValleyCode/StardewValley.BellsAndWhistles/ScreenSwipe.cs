using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class ScreenSwipe
	{
		public const int swipe_bundleComplete = 0;

		public const int borderPixelWidth = 7;

		private Rectangle bgSource;

		private Rectangle flairSource;

		private Rectangle messageSource;

		private Rectangle movingFlairSource;

		private Rectangle bgDest;

		private int yPosition;

		private int durationAfterSwipe;

		private int originalBGSourceXLimit;

		private List<Vector2> flairPositions = new List<Vector2>();

		private Vector2 messagePosition;

		private Vector2 movingFlairPosition;

		private Vector2 movingFlairMotion;

		private float swipeVelocity;

		public ScreenSwipe(int which, float swipeVelocity = -1f, int durationAfterSwipe = -1)
		{
			Game1.playSound("throw");
			if (swipeVelocity == -1f)
			{
				swipeVelocity = 5f;
			}
			if (durationAfterSwipe == -1)
			{
				durationAfterSwipe = 2700;
			}
			this.swipeVelocity = swipeVelocity;
			this.durationAfterSwipe = durationAfterSwipe;
			Vector2 vector = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2));
			if (which == 0)
			{
				this.messageSource = new Rectangle(128, 1367, 110, 14);
			}
			if (which == 0)
			{
				this.bgSource = new Rectangle(128, 1296, 1, 71);
				this.flairSource = new Rectangle(144, 1303, 144, 58);
				this.movingFlairSource = new Rectangle(643, 768, 8, 13);
				this.originalBGSourceXLimit = this.bgSource.X + this.bgSource.Width;
				this.yPosition = (int)vector.Y - this.bgSource.Height * Game1.pixelZoom / 2;
				this.messagePosition = new Vector2(vector.X - (float)(this.messageSource.Width * Game1.pixelZoom / 2), vector.Y - (float)(this.messageSource.Height * Game1.pixelZoom / 2));
				this.flairPositions.Add(new Vector2(this.messagePosition.X - (float)(this.flairSource.Width * Game1.pixelZoom) - (float)Game1.tileSize, (float)(this.yPosition + 7 * Game1.pixelZoom)));
				this.flairPositions.Add(new Vector2(this.messagePosition.X + (float)(this.messageSource.Width * Game1.pixelZoom) + (float)Game1.tileSize, (float)(this.yPosition + 7 * Game1.pixelZoom)));
				this.movingFlairPosition = new Vector2(this.messagePosition.X + (float)(this.messageSource.Width * Game1.pixelZoom) + (float)(Game1.tileSize * 3), vector.Y + (float)(Game1.tileSize / 2));
				this.movingFlairMotion = new Vector2(0f, -0.5f);
			}
			this.bgDest = new Rectangle(0, this.yPosition, this.bgSource.Width * Game1.pixelZoom, this.bgSource.Height * Game1.pixelZoom);
		}

		public bool update(GameTime time)
		{
			if (this.durationAfterSwipe > 0 && this.bgDest.Width <= Game1.viewport.Width)
			{
				this.bgDest.Width = this.bgDest.Width + (int)((double)this.swipeVelocity * time.ElapsedGameTime.TotalMilliseconds);
				if (this.bgDest.Width > Game1.viewport.Width)
				{
					Game1.playSound("newRecord");
				}
			}
			else if (this.durationAfterSwipe <= 0)
			{
				this.bgDest.X = this.bgDest.X + (int)((double)this.swipeVelocity * time.ElapsedGameTime.TotalMilliseconds);
				for (int i = 0; i < this.flairPositions.Count; i++)
				{
					if ((float)this.bgDest.X > this.flairPositions[i].X)
					{
						this.flairPositions[i] = new Vector2((float)this.bgDest.X, this.flairPositions[i].Y);
					}
				}
				if ((float)this.bgDest.X > this.messagePosition.X)
				{
					this.messagePosition = new Vector2((float)this.bgDest.X, this.messagePosition.Y);
				}
				if ((float)this.bgDest.X > this.movingFlairPosition.X)
				{
					this.movingFlairPosition = new Vector2((float)this.bgDest.X, this.movingFlairPosition.Y);
				}
			}
			if (this.bgDest.Width > Game1.viewport.Width && this.durationAfterSwipe > 0)
			{
				if (Game1.oldMouseState.LeftButton == ButtonState.Pressed)
				{
					this.durationAfterSwipe = 0;
				}
				this.durationAfterSwipe -= (int)time.ElapsedGameTime.TotalMilliseconds;
				if (this.durationAfterSwipe <= 0)
				{
					Game1.playSound("tinyWhip");
				}
			}
			this.movingFlairPosition += this.movingFlairMotion;
			return this.bgDest.X > Game1.viewport.Width;
		}

		public Rectangle getAdjustedSourceRect(Rectangle sourceRect, float xStartPosition)
		{
			if (xStartPosition > (float)this.bgDest.Width || xStartPosition + (float)(sourceRect.Width * Game1.pixelZoom) < (float)this.bgDest.X)
			{
				return Rectangle.Empty;
			}
			int num = (int)Math.Max((float)sourceRect.X, (float)sourceRect.X + ((float)this.bgDest.X - xStartPosition) / (float)Game1.pixelZoom);
			return new Rectangle(num, sourceRect.Y, (int)Math.Min((float)(sourceRect.Width - (num - sourceRect.X) / Game1.pixelZoom), ((float)this.bgDest.Width - xStartPosition) / (float)Game1.pixelZoom), sourceRect.Height);
		}

		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, this.bgDest, new Rectangle?(this.bgSource), Color.White);
			foreach (Vector2 current in this.flairPositions)
			{
				Rectangle adjustedSourceRect = this.getAdjustedSourceRect(this.flairSource, current.X);
				if (adjustedSourceRect.Right >= this.originalBGSourceXLimit)
				{
					adjustedSourceRect.Width = this.originalBGSourceXLimit - adjustedSourceRect.X;
				}
				b.Draw(Game1.mouseCursors, current, new Rectangle?(adjustedSourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			b.Draw(Game1.mouseCursors, this.movingFlairPosition, new Rectangle?(this.getAdjustedSourceRect(this.movingFlairSource, this.movingFlairPosition.X)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, this.messagePosition, new Rectangle?(this.getAdjustedSourceRect(this.messageSource, this.messagePosition.X)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
		}
	}
}
