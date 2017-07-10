using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Frog : Critter
	{
		private bool waterLeaper;

		private bool leapingIntoWater;

		private bool splash;

		private int characterCheckTimer = 200;

		private int beforeFadeTimer;

		private float alpha = 1f;

		public Frog(Vector2 position, bool waterLeaper = false, bool forceFlip = false)
		{
			this.waterLeaper = waterLeaper;
			this.position = position * (float)Game1.tileSize;
			this.sprite = new AnimatedSprite(Critter.critterTexture, waterLeaper ? 300 : 280, 16, 16);
			this.sprite.loop = true;
			if (!this.flip & forceFlip)
			{
				this.flip = true;
			}
			if (waterLeaper)
			{
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(300, 600),
					new FarmerSprite.AnimationFrame(304, 100),
					new FarmerSprite.AnimationFrame(305, 100),
					new FarmerSprite.AnimationFrame(306, 300),
					new FarmerSprite.AnimationFrame(305, 100),
					new FarmerSprite.AnimationFrame(304, 100)
				});
			}
			else
			{
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(280, 60),
					new FarmerSprite.AnimationFrame(281, 70),
					new FarmerSprite.AnimationFrame(282, 140),
					new FarmerSprite.AnimationFrame(283, 90)
				});
				this.beforeFadeTimer = 1000;
				this.flip = (this.position.X + (float)Game1.pixelZoom < Game1.player.position.X);
			}
			this.startingPosition = position;
		}

		public void startSplash(Farmer who)
		{
			this.splash = true;
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			if (this.waterLeaper)
			{
				if (!this.leapingIntoWater)
				{
					this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.characterCheckTimer <= 0)
					{
						if (Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float)Game1.tileSize, 6, environment) != null)
						{
							this.leapingIntoWater = true;
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(300, 100),
								new FarmerSprite.AnimationFrame(301, 100),
								new FarmerSprite.AnimationFrame(302, 100),
								new FarmerSprite.AnimationFrame(303, 1500, false, false, new AnimatedSprite.endOfAnimationBehavior(this.startSplash), true)
							});
							this.sprite.loop = false;
							this.sprite.oldFrame = 303;
							this.gravityAffectedDY = -6f;
						}
						else if (Game1.random.NextDouble() < 0.01)
						{
							Game1.playSound("croak");
						}
						this.characterCheckTimer = 200;
					}
				}
				else
				{
					this.position.X = this.position.X + (float)(this.flip ? -4 : 4);
				}
			}
			else
			{
				this.position.X = this.position.X + (float)(this.flip ? -3 : 3);
				this.beforeFadeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.beforeFadeTimer <= 0)
				{
					this.alpha -= 0.001f * (float)time.ElapsedGameTime.Milliseconds;
					if (this.alpha <= 0f)
					{
						return true;
					}
				}
				if (environment.doesTileHaveProperty((int)this.position.X / Game1.tileSize, (int)this.position.Y / Game1.tileSize, "Water", "Back") != null)
				{
					this.splash = true;
				}
			}
			if (this.splash)
			{
				environment.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 50f, 2, 1, this.position, false, false));
				Game1.playSound("dropItemInWater");
				return true;
			}
			return base.update(time, environment);
		}

		public override void draw(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.pixelZoom * 5))), (this.position.Y + (float)Game1.tileSize) / 10000f, 0, 0, Color.White * this.alpha, this.flip, 4f, 0f, false);
			b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2 + Game1.pixelZoom * 2))), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * this.alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + Math.Max(-3f, (this.yJumpOffset + this.yOffset) / 16f), SpriteEffects.None, (this.position.Y - 1f) / 10000f);
		}

		public override void drawAboveFrontLayer(SpriteBatch b)
		{
		}
	}
}
