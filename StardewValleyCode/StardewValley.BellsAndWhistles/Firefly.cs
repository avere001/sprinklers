using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.BellsAndWhistles
{
	public class Firefly : Critter
	{
		private bool glowing;

		private int glowTimer;

		private int id;

		private Vector2 motion;

		private LightSource light;

		public Firefly()
		{
		}

		public Firefly(Vector2 position)
		{
			this.baseFrame = -1;
			this.position = position * (float)Game1.tileSize;
			this.startingPosition = position * (float)Game1.tileSize;
			this.motion = new Vector2((float)Game1.random.Next(-10, 11) * 0.1f, (float)Game1.random.Next(-10, 11) * 0.1f);
			this.id = (int)(position.X * 10099f + position.Y * 77f + (float)Game1.random.Next(99999));
			this.light = new LightSource(4, position, (float)Game1.random.Next(4, 6) * 0.1f, Color.Purple * 0.8f, this.id);
			this.glowing = true;
			Game1.currentLightSources.Add(this.light);
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			this.position += this.motion;
			this.motion.X = this.motion.X + (float)Game1.random.Next(-1, 2) * 0.1f;
			this.motion.Y = this.motion.Y + (float)Game1.random.Next(-1, 2) * 0.1f;
			if (this.motion.X < -1f)
			{
				this.motion.X = -1f;
			}
			if (this.motion.X > 1f)
			{
				this.motion.X = 1f;
			}
			if (this.motion.Y < -1f)
			{
				this.motion.Y = -1f;
			}
			if (this.motion.Y > 1f)
			{
				this.motion.Y = 1f;
			}
			if (this.glowing)
			{
				this.light.position = this.position;
			}
			return this.position.X < (float)(-(float)Game1.tileSize * 2) || this.position.Y < (float)(-(float)Game1.tileSize * 2) || this.position.X > (float)environment.map.DisplayWidth || this.position.Y > (float)environment.map.DisplayHeight;
		}

		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			b.Draw(Game1.staminaRect, Game1.GlobalToLocal(this.position), new Rectangle?(Game1.staminaRect.Bounds), this.glowing ? Color.White : Color.Brown, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
		}
	}
}
