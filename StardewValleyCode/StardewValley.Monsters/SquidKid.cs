using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;
using System;

namespace StardewValley.Monsters
{
	public class SquidKid : Monster
	{
		private float lastFireball;

		private new int yOffset;

		public SquidKid()
		{
		}

		public SquidKid(Vector2 position) : base("Squid Kid", position)
		{
			this.sprite.spriteHeight = 16;
			base.IsWalkingTowardPlayer = false;
			this.sprite.UpdateSourceRect();
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Squid Kid"));
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				this.health -= num;
				base.setTrajectory(xTrajectory, yTrajectory);
				Game1.playSound("hitEnemy");
				this.sprite.CurrentFrame = this.sprite.CurrentFrame - this.sprite.CurrentFrame % 4 + 3;
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return num;
		}

		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 64, 16, 16), 70f, 7, 0, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), false, false)
			{
				scale = (float)Game1.pixelZoom
			});
			Game1.playSound("fireball");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 100
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 200
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 300
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 400
			});
		}

		public override void drawAboveAllLayers(SpriteBatch b)
		{
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 3 + this.yOffset)), new Rectangle?(base.Sprite.SourceRect), Color.White, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + (float)this.yOffset / 20f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			base.faceGeneralDirection(Game1.player.position, 0);
			this.yOffset = (int)(Math.Sin((double)((float)time.TotalGameTime.Milliseconds / 2000f) * 6.2831853071795862) * 15.0);
			if (this.sprite.CurrentFrame % 4 != 0 && Game1.random.NextDouble() < 0.1)
			{
				this.sprite.CurrentFrame -= this.sprite.CurrentFrame % 4;
			}
			if (Game1.random.NextDouble() < 0.01)
			{
				AnimatedSprite expr_AC = this.sprite;
				int currentFrame = expr_AC.CurrentFrame;
				expr_AC.CurrentFrame = currentFrame + 1;
			}
			this.lastFireball = Math.Max(0f, this.lastFireball - (float)time.ElapsedGameTime.Milliseconds);
			if (this.withinPlayerThreshold() && this.lastFireball == 0f && Game1.random.NextDouble() < 0.01)
			{
				base.IsWalkingTowardPlayer = false;
				Vector2 vector = new Vector2(this.position.X, this.position.Y + (float)Game1.tileSize);
				this.Halt();
				switch (this.facingDirection)
				{
				case 0:
					this.sprite.CurrentFrame = 3;
					break;
				case 1:
					this.sprite.CurrentFrame = 7;
					vector.X += (float)Game1.tileSize;
					break;
				case 2:
					this.sprite.CurrentFrame = 11;
					vector.Y += (float)(Game1.tileSize / 2);
					break;
				case 3:
					this.sprite.CurrentFrame = 15;
					vector.X -= (float)(Game1.tileSize / 2);
					break;
				}
				this.sprite.UpdateSourceRect();
				Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(base.getStandingPosition()), 8f, Game1.player);
				Game1.currentLocation.projectiles.Add(new BasicProjectile(15, 10, 3, 4, 0f, velocityTowardPlayer.X, velocityTowardPlayer.Y, base.getStandingPosition(), "", "", true, false, this, false, null));
				Game1.playSound("fireball");
				this.lastFireball = (float)Game1.random.Next(1200, 3500);
				return;
			}
			if (this.lastFireball != 0f && Game1.random.NextDouble() < 0.02)
			{
				this.Halt();
				if (this.withinPlayerThreshold())
				{
					this.slipperiness = 8;
					base.setTrajectory((int)Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(base.getStandingPosition()), 8f, Game1.player).X, (int)(-(int)Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(base.getStandingPosition()), 8f, Game1.player).Y));
				}
			}
		}
	}
}
