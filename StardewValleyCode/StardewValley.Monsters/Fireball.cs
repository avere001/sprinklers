using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
	public class Fireball : Monster
	{
		private bool homing;

		private new double id;

		private float dx;

		private float dy;

		public Fireball()
		{
		}

		public Fireball(Vector2 position) : base("Fireball", position)
		{
			this.scale = (float)Game1.pixelZoom + (float)Game1.random.Next(-20, 20) / 100f;
			base.IsWalkingTowardPlayer = false;
			this.homing = true;
			this.id = Game1.random.NextDouble();
			this.sprite.spriteWidth = 8;
			this.sprite.spriteHeight = 8;
			this.sprite.UpdateSourceRect();
		}

		public Fireball(Vector2 position, int xVelocity, int yVelocity) : base("Fireball", position)
		{
			this.scale = (float)Game1.pixelZoom + (float)Game1.random.Next(-20, 20) / 100f;
			base.IsWalkingTowardPlayer = false;
			this.dx = (float)xVelocity;
			this.dy = (float)yVelocity;
			this.id = Game1.random.NextDouble();
			this.sprite.spriteWidth = 8;
			this.sprite.spriteHeight = 8;
			this.sprite.UpdateSourceRect();
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Fireball"), 0, 8, 8);
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			if (isBomb)
			{
				return 0;
			}
			this.health -= damage;
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(362, 100f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5));
			return 1;
		}

		public override Rectangle GetBoundingBox()
		{
			return new Rectangle((int)this.position.X, (int)this.position.Y, this.sprite.getWidth(), this.sprite.getHeight());
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.homing)
			{
				if (this.position.X < Game1.player.position.X - 12f)
				{
					this.dx = Math.Min(this.dx + 0.8f, 8f);
				}
				else if (this.position.X > Game1.player.position.X + 12f)
				{
					this.dx = Math.Max(this.dx - 0.8f, -8f);
				}
				if (this.position.Y + (float)(Game1.tileSize / 4) < (float)(Game1.player.getStandingY() - 6))
				{
					this.dy = Math.Max(this.dy - 0.8f, -8f);
				}
				else if (this.position.Y + (float)(Game1.tileSize / 4) > (float)(Game1.player.getStandingY() + 6))
				{
					this.dy = Math.Min(this.dy + 0.8f, 8f);
				}
			}
			this.position.X = this.position.X + this.dx;
			this.position.Y = this.position.Y - this.dy;
			if (Game1.currentLocation.isCollidingPosition(this.GetBoundingBox(), Game1.viewport, false, 8, true, this) || this.GetBoundingBox().Intersects(Game1.player.GetBoundingBox()))
			{
				for (int i = Game1.currentLocation.characters.Count - 1; i >= 0; i--)
				{
					if (Game1.currentLocation.characters[i].name.Equals("Fireball") && ((Fireball)Game1.currentLocation.characters[i]).id == this.id)
					{
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(362, 100f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5));
						Game1.currentLocation.characters.RemoveAt(i);
						return;
					}
				}
			}
		}
	}
}
