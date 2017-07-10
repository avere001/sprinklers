using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Butterfly : Critter
	{
		public const float maxSpeed = 3f;

		private int flapTimer;

		private int checkForLandingSpotTimer;

		private int landedTimer;

		private int flapSpeed = 50;

		private Vector2 motion;

		private float motionMultiplier = 1f;

		private bool summerButterfly;

		public Butterfly(Vector2 position)
		{
			this.position = position * (float)Game1.tileSize;
			this.startingPosition = this.position;
			if (Game1.currentSeason.Equals("spring"))
			{
				this.baseFrame = ((Game1.random.NextDouble() < 0.5) ? (Game1.random.Next(3) * 3 + 160) : (Game1.random.Next(3) * 3 + 180));
			}
			else
			{
				this.baseFrame = ((Game1.random.NextDouble() < 0.5) ? (Game1.random.Next(3) * 4 + 128) : (Game1.random.Next(3) * 4 + 148));
				this.summerButterfly = true;
			}
			this.motion = new Vector2((float)(Game1.random.NextDouble() + 0.25) * 3f * (float)((Game1.random.NextDouble() < 0.5) ? -1 : 1) / 2f, (float)(Game1.random.NextDouble() + 0.5) * 3f * (float)((Game1.random.NextDouble() < 0.5) ? -1 : 1) / 2f);
			this.flapSpeed = Game1.random.Next(45, 80);
			this.sprite = new AnimatedSprite(Critter.critterTexture, this.baseFrame, 16, 16);
			this.sprite.loop = false;
			this.startingPosition = position;
		}

		public void doneWithFlap(Farmer who)
		{
			this.flapTimer = 200 + Game1.random.Next(-5, 6);
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			this.flapTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.flapTimer <= 0 && this.sprite.currentAnimation == null)
			{
				this.motionMultiplier = 1f;
				this.motion.X = this.motion.X + (float)Game1.random.Next(-80, 81) / 100f;
				this.motion.Y = (float)(Game1.random.NextDouble() + 0.25) * -3f / 2f;
				if (Math.Abs(this.motion.X) > 1.5f)
				{
					this.motion.X = 3f * (float)Math.Sign(this.motion.X) / 2f;
				}
				if (Math.Abs(this.motion.Y) > 3f)
				{
					this.motion.Y = 3f * (float)Math.Sign(this.motion.Y);
				}
				if (this.summerButterfly)
				{
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 3, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame, this.flapSpeed, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneWithFlap), false)
					});
				}
				else
				{
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame, this.flapSpeed, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneWithFlap), false)
					});
				}
			}
			this.position += this.motion * this.motionMultiplier;
			this.motion.Y = this.motion.Y + 0.005f * (float)time.ElapsedGameTime.Milliseconds;
			this.motionMultiplier -= 0.0005f * (float)time.ElapsedGameTime.Milliseconds;
			if (this.motionMultiplier <= 0f)
			{
				this.motionMultiplier = 0f;
			}
			return base.update(time, environment);
		}

		public override void draw(SpriteBatch b)
		{
		}

		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(-64f, -128f + this.yJumpOffset + this.yOffset)), this.position.Y / 10000f, 0, 0, Color.White, this.flip, 4f, 0f, false);
		}
	}
}
