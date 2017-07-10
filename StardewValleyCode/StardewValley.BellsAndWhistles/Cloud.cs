using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.BellsAndWhistles
{
	public class Cloud : Critter
	{
		public const int width = 147;

		public const int height = 100;

		public int zoom = 5;

		private bool verticalFlip;

		private bool horizontalFlip;

		public Cloud()
		{
		}

		public Cloud(Vector2 position)
		{
			this.position = position * (float)Game1.tileSize;
			this.startingPosition = position;
			this.verticalFlip = (Game1.random.NextDouble() < 0.5);
			this.horizontalFlip = (Game1.random.NextDouble() < 0.5);
			this.zoom = Game1.random.Next(4, 7);
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			this.position.Y = this.position.Y - (float)time.ElapsedGameTime.TotalMilliseconds * 0.02f;
			this.position.X = this.position.X - (float)time.ElapsedGameTime.TotalMilliseconds * 0.02f;
			return this.position.X < (float)(-147 * this.zoom) || this.position.Y < (float)(-100 * this.zoom);
		}

		public override Rectangle getBoundingBox(int xOffset, int yOffset)
		{
			return new Rectangle((int)this.position.X, (int)this.position.Y, 147 * this.zoom, 100 * this.zoom);
		}

		public override void draw(SpriteBatch b)
		{
		}

		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(this.position), new Rectangle?(new Rectangle(128, 0, 146, 99)), Color.White, (this.verticalFlip && this.horizontalFlip) ? 3.14159274f : 0f, Vector2.Zero, (float)this.zoom, (this.verticalFlip && !this.horizontalFlip) ? SpriteEffects.FlipVertically : ((this.horizontalFlip && !this.verticalFlip) ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f);
		}
	}
}
