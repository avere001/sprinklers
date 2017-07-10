using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
	public class Serpent : Monster
	{
		public const float rotationIncrement = 0.0490873866f;

		private int wasHitCounter;

		private float targetRotation;

		private bool turningRight;

		public Serpent()
		{
		}

		public Serpent(Vector2 position) : base("Serpent", position)
		{
			this.slipperiness = 24 + Game1.random.Next(10);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.sprite.spriteWidth = 32;
			this.sprite.spriteHeight = 32;
			this.scale = 0.75f;
			this.hideShadow = true;
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Serpent"));
			this.sprite.spriteWidth = 32;
			this.sprite.spriteHeight = 32;
			this.scale = 0.75f;
			this.hideShadow = true;
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
				base.setTrajectory(xTrajectory / 3, yTrajectory / 3);
				this.wasHitCounter = 500;
				Game1.playSound("serpentHit");
				if (this.health <= 0)
				{
					Rectangle boundingBox = this.GetBoundingBox();
					boundingBox.Inflate(-boundingBox.Width / 2 + 1, -boundingBox.Height / 2 + 1);
					Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(boundingBox.Center, 4f, Game1.player);
					this.deathAnimation(-(int)velocityTowardPlayer.X, -(int)velocityTowardPlayer.Y);
				}
			}
			this.addedSpeed = Game1.random.Next(-1, 1);
			return num;
		}

		public void deathAnimation(int xTrajectory, int yTrajectory)
		{
			Game1.playSound("serpentDie");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 64, 32, 32), 200f, 4, 0, this.position, false, false, 0.9f, 0.001f, Color.White, (float)Game1.pixelZoom * this.scale, 0.01f, this.rotation + 3.14159274f, (float)((double)Game1.random.Next(3, 5) * 3.1415926535897931 / 64.0), false)
			{
				motion = new Vector2((float)xTrajectory, (float)yTrajectory),
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), Color.LightGreen * 0.9f, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 50,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory),
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2((float)(Game1.tileSize / 2), 0f), Color.LightGreen * 0.8f, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 100,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.8f,
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), Color.LightGreen * 0.7f, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 150,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.6f,
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center), Color.LightGreen * 0.6f, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 200,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.4f,
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2(0f, (float)(Game1.tileSize / 2)), Color.LightGreen * 0.5f, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 250,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.2f,
				layerDepth = 1f
			});
		}

		public override void drawAboveAllLayers(SpriteBatch b)
		{
			if (Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)Game1.tileSize, (float)this.GetBoundingBox().Height), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)Game1.tileSize, (float)(this.GetBoundingBox().Height / 2)), new Rectangle?(base.Sprite.SourceRect), Color.White, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 8) / 10000f)));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)Game1.tileSize, (float)(this.GetBoundingBox().Height / 2)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 8) / 10000f + 0.0001f)));
				}
			}
		}

		public override Rectangle GetBoundingBox()
		{
			return new Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize * 2 * 3 / 4);
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.wasHitCounter >= 0)
			{
				this.wasHitCounter -= time.ElapsedGameTime.Milliseconds;
			}
			if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
			{
				this.health = -500;
			}
			if (this.position.X <= -640f || this.position.Y <= -640f || this.position.X >= (float)(Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || this.position.Y >= (float)(Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640))
			{
				this.health = -500;
			}
			this.sprite.Animate(time, 0, 9, 40f);
			if (this.withinPlayerThreshold() && this.invincibleCountdown <= 0)
			{
				this.faceDirection(2);
				float num = (float)(-(float)(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X));
				float num2 = (float)(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
				float num3 = Math.Max(1f, Math.Abs(num) + Math.Abs(num2));
				if (num3 < (float)Game1.tileSize)
				{
					this.xVelocity = Math.Max(-7f, Math.Min(7f, this.xVelocity * 1.1f));
					this.yVelocity = Math.Max(-7f, Math.Min(7f, this.yVelocity * 1.1f));
				}
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
					this.wasHitCounter = 5 + Game1.random.Next(-1, 2);
				}
				float num4 = Math.Min(7f, Math.Max(2f, 7f - num3 / (float)Game1.tileSize / 2f));
				num = (float)Math.Cos((double)this.rotation + 1.5707963267948966);
				num2 = -(float)Math.Sin((double)this.rotation + 1.5707963267948966);
				this.xVelocity += -num * num4 / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				this.yVelocity += -num2 * num4 / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				if (Math.Abs(this.xVelocity) > Math.Abs(-num * 7f))
				{
					this.xVelocity -= -num * num4 / 6f;
				}
				if (Math.Abs(this.yVelocity) > Math.Abs(-num2 * 7f))
				{
					this.yVelocity -= -num2 * num4 / 6f;
				}
			}
		}
	}
}
