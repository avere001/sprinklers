using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;
using System;

namespace StardewValley.Monsters
{
	public class Skeleton : Monster
	{
		private bool spottedPlayer;

		private bool throwing;

		private int controllerAttemptTimer;

		public Skeleton()
		{
		}

		public Skeleton(Vector2 position) : base("Skeleton", position, Game1.random.Next(4))
		{
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
			base.IsWalkingTowardPlayer = false;
			this.jitteriness = 0.0;
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Skeleton"));
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			Game1.playSound("skeletonHit");
			this.slipperiness = 3;
			if (this.throwing)
			{
				this.throwing = false;
				this.Halt();
			}
			if (this.health - damage <= 0)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.White, 10, false, 70f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = 100
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(Game1.tileSize / 4), 0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = 200
				});
			}
			return base.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision);
		}

		public override void shedChunks(int number)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 128, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, (float)Game1.pixelZoom);
		}

		public override void deathAnimation()
		{
			Game1.playSound("skeletonDie");
			Game1.random.Next(5, 13);
			this.shedChunks(20);
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(3, (Game1.random.NextDouble() < 0.5) ? 3 : 35, 10, 10), 11, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, 1, (int)base.getTileLocation().Y, Color.White, (float)Game1.pixelZoom);
		}

		public override void update(GameTime time, GameLocation location)
		{
			if (!this.throwing)
			{
				base.update(time, location);
				return;
			}
			this.behaviorAtGameTick(time);
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			if (!this.throwing)
			{
				base.behaviorAtGameTick(time);
			}
			if (!this.spottedPlayer && !this.wildernessFarmMonster && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.controller = new PathFindController(this, Game1.currentLocation, new Point(Game1.player.getStandingX() / Game1.tileSize, Game1.player.getStandingY() / Game1.tileSize), Game1.random.Next(4), null, 200);
				this.spottedPlayer = true;
				this.Halt();
				base.facePlayer(Game1.player);
				Game1.playSound("skeletonStep");
				base.IsWalkingTowardPlayer = true;
			}
			else if (this.throwing)
			{
				if (this.invincibleCountdown > 0)
				{
					this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
					if (this.invincibleCountdown <= 0)
					{
						base.stopGlowing();
					}
				}
				this.sprite.Animate(time, 20, 5, 150f);
				if (this.sprite.currentFrame == 24)
				{
					this.throwing = false;
					this.sprite.CurrentFrame = 0;
					this.faceDirection(2);
					Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(new Point((int)this.position.X, (int)this.position.Y), 8f, Game1.player);
					Game1.currentLocation.projectiles.Add(new BasicProjectile(this.damageToFarmer, 4, 0, 0, 0.196349546f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2(this.position.X, this.position.Y), "skeletonHit", "skeletonStep", false, false, this, false, null));
				}
			}
			else if (this.spottedPlayer && this.controller == null && Game1.random.NextDouble() < 0.002 && !this.wildernessFarmMonster && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.throwing = true;
				this.Halt();
				this.sprite.CurrentFrame = 20;
				base.shake(750);
			}
			else if (base.withinPlayerThreshold(2))
			{
				this.controller = null;
			}
			else if (this.spottedPlayer && this.controller == null && this.controllerAttemptTimer <= 0)
			{
				this.controller = new PathFindController(this, Game1.currentLocation, new Point(Game1.player.getStandingX() / Game1.tileSize, Game1.player.getStandingY() / Game1.tileSize), Game1.random.Next(4), null, 200);
				this.Halt();
				base.facePlayer(Game1.player);
				this.controllerAttemptTimer = (this.wildernessFarmMonster ? 2000 : 1000);
			}
			else if (this.wildernessFarmMonster)
			{
				this.spottedPlayer = true;
				base.IsWalkingTowardPlayer = true;
			}
			this.controllerAttemptTimer -= time.ElapsedGameTime.Milliseconds;
		}
	}
}
