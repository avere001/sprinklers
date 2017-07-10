using Microsoft.Xna.Framework;
using StardewValley.Tools;
using System;

namespace StardewValley.Monsters
{
	public class RockCrab : Monster
	{
		private bool leftDrift;

		private bool shellGone;

		private bool waiter;

		private int shellHealth = 5;

		public RockCrab()
		{
		}

		public RockCrab(Vector2 position) : base("Rock Crab", position)
		{
			this.waiter = (Game1.random.NextDouble() < 0.4);
			bool arg_34_0 = this.waiter;
			this.moveTowardPlayerThreshold = 3;
		}

		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.UpdateSourceRect();
		}

		public RockCrab(Vector2 position, string name) : base(name, position)
		{
			this.waiter = (Game1.random.NextDouble() < 0.4);
			this.moveTowardPlayerThreshold = 3;
		}

		public override bool hitWithTool(Tool t)
		{
			base.hitWithTool(t);
			if (t is Pickaxe)
			{
				Game1.playSound("hammer");
				this.shellHealth--;
				base.shake(500);
				this.waiter = false;
				this.moveTowardPlayerThreshold = 3;
				base.setTrajectory(Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox(), t.getLastFarmerToUse()));
				if (this.shellHealth <= 0)
				{
					this.shellGone = true;
					base.moveTowardPlayer(-1);
					Game1.playSound("stoneCrack");
					Game1.createRadialDebris(Game1.currentLocation, 14, base.getTileX(), base.getTileY(), Game1.random.Next(2, 7), false, -1, false, -1);
					Game1.createRadialDebris(Game1.currentLocation, 14, base.getTileX(), base.getTileY(), Game1.random.Next(2, 7), false, -1, false, -1);
				}
				return true;
			}
			return false;
		}

		public override void shedChunks(int number)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 120, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, 1f * (float)Game1.pixelZoom * this.scale);
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (isBomb)
			{
				this.shellGone = true;
				this.waiter = false;
				base.moveTowardPlayer(-1);
			}
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else if (this.sprite.CurrentFrame % 4 == 0 && !this.shellGone)
			{
				num = 0;
				Game1.playSound("crafting");
			}
			else
			{
				this.health -= num;
				this.slipperiness = 3;
				base.setTrajectory(xTrajectory, yTrajectory);
				Game1.playSound("hitEnemy");
				this.glowingColor = Color.Cyan;
				if (this.health <= 0)
				{
					Game1.playSound("monsterdead");
					this.deathAnimation();
					Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.Red, 10, false, 100f, 0, -1, -1f, -1, 0)
					{
						holdLastFrame = true,
						alphaFade = 0.01f
					}, Game1.currentLocation, 4, 64, 64);
				}
			}
			return num;
		}

		public override void update(GameTime time, GameLocation location)
		{
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			if (!this.shellGone && !Game1.player.isRafting)
			{
				base.update(time, location);
				return;
			}
			if (!Game1.player.isRafting)
			{
				this.behaviorAtGameTick(time);
			}
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			if (this.waiter && this.shellHealth > 4)
			{
				this.moveTowardPlayerThreshold = 0;
				return;
			}
			base.behaviorAtGameTick(time);
			if (this.isMoving() && this.sprite.CurrentFrame % 4 == 0)
			{
				AnimatedSprite expr_3D = this.sprite;
				int currentFrame = expr_3D.CurrentFrame;
				expr_3D.CurrentFrame = currentFrame + 1;
				this.sprite.UpdateSourceRect();
			}
			if (!this.withinPlayerThreshold() && !this.shellGone)
			{
				this.Halt();
				return;
			}
			if (this.shellGone)
			{
				base.updateGlow();
				if (this.invincibleCountdown > 0)
				{
					this.glowingColor = Color.Cyan;
					this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
					if (this.invincibleCountdown <= 0)
					{
						base.stopGlowing();
					}
				}
				if (Math.Abs(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y) > Game1.tileSize * 3)
				{
					if (Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X > 0)
					{
						this.SetMovingLeft(true);
					}
					else
					{
						this.SetMovingRight(true);
					}
				}
				else if (Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y > 0)
				{
					this.SetMovingUp(true);
				}
				else
				{
					this.SetMovingDown(true);
				}
				this.MovePosition(time, Game1.viewport, Game1.currentLocation);
				this.sprite.CurrentFrame = 16 + this.sprite.CurrentFrame % 4;
			}
		}
	}
}
