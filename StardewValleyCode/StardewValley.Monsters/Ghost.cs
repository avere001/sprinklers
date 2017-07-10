using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	public class Ghost : Monster
	{
		public const float rotationIncrement = 0.0490873866f;

		private int wasHitCounter;

		private float targetRotation;

		private bool turningRight;

		private bool seenPlayer;

		private int identifier = Game1.random.Next(-99999, 99999);

		private new int yOffset;

		private int yOffsetExtra;

		public Ghost()
		{
		}

		public Ghost(Vector2 position) : base("Ghost", position)
		{
			this.slipperiness = 8;
			this.isGlider = true;
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Ghost"));
		}

		public override void drawAboveAllLayers(SpriteBatch b)
		{
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 3 + this.yOffset)), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), Color.White, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + (float)this.yOffset / 20f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			this.slipperiness = 8;
			Utility.addSprinklesToLocation(Game1.currentLocation, base.getTileX(), base.getTileY(), 2, 2, 101, 50, Color.LightBlue, null, false);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
				{
					this.health -= damage * 3 / 4;
					Game1.currentLocation.debris.Add(new Debris(string.Concat(damage * 3 / 4), 1, new Vector2((float)base.getStandingX(), (float)base.getStandingY()), Color.LightBlue, 1f, 0f));
				}
				this.health -= num;
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
				base.setTrajectory(xTrajectory, yTrajectory);
			}
			this.addedSpeed = -1;
			Utility.removeLightSource(this.identifier);
			return num;
		}

		public override void deathAnimation()
		{
			Game1.playSound("ghost");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 24), 100f, 4, 0, this.position, false, false, 0.9f, 0.001f, Color.White, (float)Game1.pixelZoom, 0.01f, 0f, 0.0490873866f, false));
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			this.yOffset = (int)(Math.Sin((double)((float)time.TotalGameTime.Milliseconds / 1000f) * 6.2831853071795862) * 20.0) - this.yOffsetExtra;
			bool flag = false;
			foreach (LightSource current in Game1.currentLightSources)
			{
				if (current.identifier == this.identifier)
				{
					current.position = new Vector2(this.position.X + (float)(Game1.tileSize / 2), this.position.Y + (float)Game1.tileSize + (float)this.yOffset);
					flag = true;
				}
			}
			if (!flag)
			{
				Game1.currentLightSources.Add(new LightSource(5, new Vector2(this.position.X + 8f, this.position.Y + (float)Game1.tileSize), 1f, Color.White * 0.7f, this.identifier));
			}
			float num = (float)(-(float)(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X));
			float num2 = (float)(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
			float num3 = 400f;
			num /= num3;
			num2 /= num3;
			if (this.wasHitCounter <= 0)
			{
				this.targetRotation = (float)Math.Atan2((double)(-(double)num2), (double)num) - 1.57079637f;
				if ((double)(Math.Abs(this.targetRotation) - Math.Abs(this.rotation)) > 2.748893571891069 && Game1.random.NextDouble() < 0.5)
				{
					this.turningRight = true;
				}
				else if ((double)(Math.Abs(this.targetRotation) - Math.Abs(this.rotation)) < 0.39269908169872414)
				{
					this.turningRight = false;
				}
				if (this.turningRight)
				{
					this.rotation -= (float)Math.Sign(this.targetRotation - this.rotation) * 0.0490873866f;
				}
				else
				{
					this.rotation += (float)Math.Sign(this.targetRotation - this.rotation) * 0.0490873866f;
				}
				this.rotation %= 6.28318548f;
				this.wasHitCounter = 0;
			}
			float num4 = Math.Min(4f, Math.Max(1f, 5f - num3 / (float)Game1.tileSize / 2f));
			num = (float)Math.Cos((double)this.rotation + 1.5707963267948966);
			num2 = -(float)Math.Sin((double)this.rotation + 1.5707963267948966);
			this.xVelocity += -num * num4 / 6f + (float)Game1.random.Next(-10, 10) / 100f;
			this.yVelocity += -num2 * num4 / 6f + (float)Game1.random.Next(-10, 10) / 100f;
			if (Math.Abs(this.xVelocity) > Math.Abs(-num * 5f))
			{
				this.xVelocity -= -num * num4 / 6f;
			}
			if (Math.Abs(this.yVelocity) > Math.Abs(-num2 * 5f))
			{
				this.yVelocity -= -num2 * num4 / 6f;
			}
			base.faceGeneralDirection(Game1.player.getStandingPosition(), 0);
			if (this.GetBoundingBox().Intersects(Game1.player.GetBoundingBox()))
			{
				int num5 = 0;
				Vector2 vector = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-12, 12)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-12, 12)));
				while (num5 < 3 && (vector.X >= (float)Game1.currentLocation.map.GetLayer("Back").LayerWidth || vector.Y >= (float)Game1.currentLocation.map.GetLayer("Back").LayerHeight || vector.X < 0f || vector.Y < 0f || Game1.currentLocation.map.GetLayer("Back").Tiles[(int)vector.X, (int)vector.Y] == null || !Game1.currentLocation.isTilePassable(new Location((int)vector.X, (int)vector.Y), Game1.viewport) || vector.Equals(new Vector2((float)(Game1.player.getStandingX() / Game1.tileSize), (float)(Game1.player.getStandingY() / Game1.tileSize)))))
				{
					vector = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-12, 12)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-12, 12)));
					num5++;
				}
				if (num5 < 3)
				{
					this.position = new Vector2(vector.X * (float)Game1.tileSize, vector.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2));
					this.Halt();
				}
			}
		}
	}
}
