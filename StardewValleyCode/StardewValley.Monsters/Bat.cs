using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
	public class Bat : Monster
	{
		public const float rotationIncrement = 0.0490873866f;

		private int wasHitCounter;

		private float targetRotation;

		private bool turningRight;

		private bool seenPlayer;

		private Cue batFlap;

		public Bat()
		{
		}

		public Bat(Vector2 position) : base("Bat", position)
		{
			this.slipperiness = 24 + Game1.random.Next(-10, 11);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.hideShadow = true;
		}

		public Bat(Vector2 position, int mineLevel) : base("Bat", position)
		{
			if (mineLevel >= 40 && mineLevel < 80)
			{
				this.name = "Frost Bat";
				base.parseMonsterInfo("Frost Bat");
				this.reloadSprite();
			}
			else if (mineLevel >= 80)
			{
				this.name = "Lava Bat";
				base.parseMonsterInfo("Lava Bat");
				this.reloadSprite();
			}
			this.slipperiness = 20 + Game1.random.Next(-5, 6);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.hideShadow = true;
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + this.name));
			this.hideShadow = true;
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			this.seenPlayer = true;
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
					this.deathAnimation();
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, Color.DarkMagenta, 10, false, 100f, 0, -1, -1f, -1, 0));
					Game1.playSound("batScreech");
				}
			}
			this.addedSpeed = Game1.random.Next(-1, 1);
			return num;
		}

		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 384, 64, 64), 32, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, scale);
		}

		public override void drawAboveAllLayers(SpriteBatch b)
		{
			if (Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), Color.White, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.92f);
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, this.wildernessFarmMonster ? 0.0001f : ((float)(base.getStandingY() - 1) / 10000f));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
			}
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.wasHitCounter >= 0)
			{
				this.wasHitCounter -= time.ElapsedGameTime.Milliseconds;
			}
			if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity) || this.position.X < -2000f || this.position.Y < -2000f)
			{
				this.health = -500;
			}
			if (this.position.X <= -640f || this.position.Y <= -640f || this.position.X >= (float)(Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || this.position.Y >= (float)(Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640))
			{
				this.health = -500;
			}
			if (this.focusedOnFarmers || base.withinPlayerThreshold(6) || this.seenPlayer)
			{
				this.seenPlayer = true;
				this.sprite.Animate(time, 0, 4, 80f);
				if (this.sprite.CurrentFrame % 3 == 0 && Utility.isOnScreen(this.position, Game1.tileSize * 8) && (this.batFlap == null || !this.batFlap.IsPlaying) && Game1.soundBank != null)
				{
					this.batFlap = Game1.soundBank.GetCue("batFlap");
					this.batFlap.Play();
				}
				if (this.invincibleCountdown > 0)
				{
					if (this.name.Equals("Lava Bat"))
					{
						this.glowingColor = Color.Cyan;
					}
					return;
				}
				float num = (float)(-(float)(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X));
				float num2 = (float)(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
				float num3 = Math.Max(1f, Math.Abs(num) + Math.Abs(num2));
				if (num3 < (float)Game1.tileSize)
				{
					this.xVelocity = Math.Max(-5f, Math.Min(5f, this.xVelocity * 1.05f));
					this.yVelocity = Math.Max(-5f, Math.Min(5f, this.yVelocity * 1.05f));
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
					this.wasHitCounter = 0;
				}
				float num4 = Math.Min(5f, Math.Max(1f, 5f - num3 / (float)Game1.tileSize / 2f));
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
					return;
				}
			}
			else
			{
				this.sprite.CurrentFrame = 4;
				this.Halt();
			}
		}
	}
}
