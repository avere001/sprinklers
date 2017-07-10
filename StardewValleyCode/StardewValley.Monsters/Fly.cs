using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
	public class Fly : Monster
	{
		public const float rotationIncrement = 0.0490873866f;

		public const int volumeTileRange = 16;

		public const int spawnTime = 1000;

		private int spawningCounter = 1000;

		private int wasHitCounter;

		private float targetRotation;

		public static Cue buzz;

		private bool turningRight;

		public bool hard;

		public Fly()
		{
		}

		public Fly(Vector2 position, bool hard = false) : base("Fly", position)
		{
			this.slipperiness = 24 + Game1.random.Next(-10, 10);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.hard = hard;
			if (hard)
			{
				this.damageToFarmer *= 2;
				this.maxHealth *= 3;
				this.health = this.maxHealth;
			}
			this.hideShadow = true;
		}

		public void setHard()
		{
			this.hard = true;
			if (this.hard)
			{
				this.damageToFarmer = 12;
				this.maxHealth = 66;
				this.health = this.maxHealth;
			}
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Fly"));
			if (Game1.soundBank != null)
			{
				Fly.buzz = Game1.soundBank.GetCue("flybuzzing");
			}
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
				Game1.playSound("hitEnemy");
				if (this.health <= 0)
				{
					Game1.playSound("monsterdead");
					Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.HotPink, 10, false, 100f, 0, -1, -1f, -1, 0)
					{
						interval = 70f
					}, Game1.currentLocation, 4, 64, 64);
					if (Game1.soundBank != null && Fly.buzz != null)
					{
						Fly.buzz.Stop(AudioStopOptions.AsAuthored);
					}
				}
			}
			this.addedSpeed = Game1.random.Next(-1, 1);
			return num;
		}

		public override void drawAboveAllLayers(SpriteBatch b)
		{
			if (Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), this.hard ? Color.Lime : Color.White, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 8) / 10000f)));
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
			}
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			if (Game1.currentLocation.treatAsOutdoors)
			{
				this.drawAboveAllLayers(b);
			}
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (Game1.soundBank != null && (Fly.buzz == null || !Fly.buzz.IsPlaying))
			{
				Fly.buzz = Game1.soundBank.GetCue("flybuzzing");
				Fly.buzz.SetVariable("Volume", 0f);
				Fly.buzz.Play();
			}
			if ((double)Game1.fadeToBlackAlpha > 0.8 && Game1.fadeIn && Fly.buzz != null)
			{
				Fly.buzz.Stop(AudioStopOptions.AsAuthored);
			}
			else if (Fly.buzz != null)
			{
				Fly.buzz.SetVariable("Volume", Math.Max(0f, Fly.buzz.GetVariable("Volume") - 1f));
				float num = Math.Max(0f, 100f - Vector2.Distance(this.position, Game1.player.position) / (float)Game1.tileSize / 16f * 100f);
				if (num > Fly.buzz.GetVariable("Volume"))
				{
					Fly.buzz.SetVariable("Volume", num);
				}
			}
			if (this.wasHitCounter >= 0)
			{
				this.wasHitCounter -= time.ElapsedGameTime.Milliseconds;
			}
			if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
			{
				this.health = -500;
			}
			this.sprite.Animate(time, (this.facingDirection == 0) ? 8 : ((this.facingDirection == 2) ? 0 : (this.facingDirection * 4)), 4, 75f);
			if (this.position.X <= -640f || this.position.Y <= -640f || this.position.X >= (float)(Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || this.position.Y >= (float)(Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640))
			{
				this.health = -500;
			}
			if (this.spawningCounter >= 0)
			{
				this.spawningCounter -= time.ElapsedGameTime.Milliseconds;
				this.scale = 1f - (float)this.spawningCounter / 1000f;
				return;
			}
			if ((this.withinPlayerThreshold() || Utility.isOnScreen(this.position, Game1.tileSize * 4)) && this.invincibleCountdown <= 0)
			{
				this.faceDirection(0);
				float num2 = (float)(-(float)(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X));
				float num3 = (float)(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
				float num4 = Math.Max(1f, Math.Abs(num2) + Math.Abs(num3));
				if (num4 < (float)Game1.tileSize)
				{
					this.xVelocity = Math.Max(-7f, Math.Min(7f, this.xVelocity * 1.1f));
					this.yVelocity = Math.Max(-7f, Math.Min(7f, this.yVelocity * 1.1f));
				}
				num2 /= num4;
				num3 /= num4;
				if (this.wasHitCounter <= 0)
				{
					this.targetRotation = (float)Math.Atan2((double)(-(double)num3), (double)num2) - 1.57079637f;
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
				float num5 = Math.Min(7f, Math.Max(2f, 7f - num4 / (float)Game1.tileSize / 2f));
				num2 = (float)Math.Cos((double)this.rotation + 1.5707963267948966);
				num3 = -(float)Math.Sin((double)this.rotation + 1.5707963267948966);
				this.xVelocity += -num2 * num5 / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				this.yVelocity += -num3 * num5 / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				if (Math.Abs(this.xVelocity) > Math.Abs(-num2 * 7f))
				{
					this.xVelocity -= -num2 * num5 / 6f;
				}
				if (Math.Abs(this.yVelocity) > Math.Abs(-num3 * 7f))
				{
					this.yVelocity -= -num3 * num5 / 6f;
				}
			}
		}
	}
}
