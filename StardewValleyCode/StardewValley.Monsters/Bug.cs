using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
	public class Bug : Monster
	{
		public bool isArmoredBug;

		public Bug()
		{
		}

		public Bug(Vector2 position, int facingDirection) : base("Bug", position, facingDirection)
		{
			this.sprite.spriteHeight = 16;
			this.sprite.UpdateSourceRect();
			this.onCollision = new Monster.collisionBehavior(this.collide);
			this.yOffset = (float)(-(float)Game1.tileSize / 2);
			base.IsWalkingTowardPlayer = false;
			base.setMovingInFacingDirection();
			this.defaultAnimationInterval = 40;
			this.collidesWithOtherCharacters = false;
			if (Game1.mine.getMineArea(-1) == 121)
			{
				this.isArmoredBug = true;
				this.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\Monsters\\Armored Bug");
				this.damageToFarmer *= 2;
				this.slipperiness = -1;
				this.health = 150;
			}
		}

		public override bool passThroughCharacters()
		{
			return true;
		}

		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteHeight = 16;
			this.sprite.UpdateSourceRect();
		}

		private void collide(GameLocation location)
		{
			Rectangle value = this.nextPosition(this.facingDirection);
			using (List<Farmer>.Enumerator enumerator = location.getFarmers().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetBoundingBox().Intersects(value))
					{
						return;
					}
				}
			}
			this.facingDirection = (this.facingDirection + 2) % 4;
			base.setMovingInFacingDirection();
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (this.isArmoredBug)
			{
				Game1.playSound("crafting");
				return 0;
			}
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				this.health -= num;
				Game1.playSound("hitEnemy");
				base.setTrajectory(xTrajectory / 3, yTrajectory / 3);
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return num;
		}

		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				Vector2 vector = default(Vector2);
				if (this.facingDirection % 2 == 0)
				{
					vector.X = (float)(Math.Sin((double)((float)Game1.currentGameTime.TotalGameTime.Milliseconds / 1000f) * 6.2831853071795862) * 10.0);
				}
				else
				{
					vector.Y = (float)(Math.Sin((double)((float)Game1.currentGameTime.TotalGameTime.Milliseconds / 1000f) * 6.2831853071795862) * 10.0);
				}
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom) / 2f + vector.X, (float)(this.GetBoundingBox().Height * 5 / 2 - Game1.tileSize * 3 / 4)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)this.yJumpOffset / 40f) * this.scale, SpriteEffects.None, Math.Max(0f, (float)base.getStandingY() / 10000f) - 1E-06f);
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)this.yJumpOffset) + vector, new Rectangle?(base.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			}
		}

		public override void deathAnimation()
		{
			base.deathAnimation();
			Game1.playSound("slimedead");
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), Color.Violet, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				holdLastFrame = true,
				alphaFade = 0.01f,
				interval = 70f
			}, Game1.currentLocation, 4, 64, 64);
		}

		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, this.sprite.getHeight() * 4, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, (float)Game1.pixelZoom);
		}
	}
}
