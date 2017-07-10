using Microsoft.Xna.Framework;
using StardewValley.Locations;
using System;

namespace StardewValley.Monsters
{
	public class RockGolem : Monster
	{
		private bool seenPlayer;

		public RockGolem()
		{
		}

		public RockGolem(Vector2 position) : base("Stone Golem", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.sprite.CurrentFrame = 16;
			this.sprite.loop = false;
			this.slipperiness = 2;
			this.jitteriness = 0.0;
			this.hideShadow = true;
			if (Game1.currentLocation is MineShaft)
			{
				int mineLevel = (Game1.currentLocation as MineShaft).mineLevel;
				if (mineLevel > 80)
				{
					this.damageToFarmer *= 2;
					this.health = (int)((float)this.health * 2.5f);
					return;
				}
				if (mineLevel > 40)
				{
					this.damageToFarmer = (int)((float)this.damageToFarmer * 1.5f);
					this.health = (int)((float)this.health * 1.75f);
				}
			}
		}

		public RockGolem(Vector2 position, int difficultyMod) : base("Wilderness Golem", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.sprite.CurrentFrame = 16;
			this.sprite.loop = false;
			this.slipperiness = 3;
			this.hideShadow = true;
			this.jitteriness = 0.0;
			this.damageToFarmer += difficultyMod;
			this.health += (int)((float)(difficultyMod * difficultyMod) * 2f);
			this.experienceGained += difficultyMod;
			if (difficultyMod >= 5 && Game1.random.NextDouble() < 0.05)
			{
				this.objectsToDrop.Add(749);
			}
			if (difficultyMod >= 5 && Game1.random.NextDouble() < 0.2)
			{
				this.objectsToDrop.Add(770);
			}
			if (difficultyMod >= 10 && Game1.random.NextDouble() < 0.01)
			{
				this.objectsToDrop.Add(386);
			}
			if (difficultyMod >= 10 && Game1.random.NextDouble() < 0.01)
			{
				this.objectsToDrop.Add(386);
			}
			if (difficultyMod >= 10 && Game1.random.NextDouble() < 0.001)
			{
				this.objectsToDrop.Add(74);
			}
		}

		public RockGolem(Vector2 position, bool alreadySpawned) : base("Stone Golem", position)
		{
			if (alreadySpawned)
			{
				base.IsWalkingTowardPlayer = true;
				this.seenPlayer = true;
				this.moveTowardPlayerThreshold = 16;
			}
			else
			{
				base.IsWalkingTowardPlayer = false;
				this.sprite.CurrentFrame = 16;
			}
			this.sprite.loop = false;
			this.slipperiness = 2;
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			this.focusedOnFarmers = true;
			base.IsWalkingTowardPlayer = true;
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				this.health -= num;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
				else
				{
					Game1.playSound("rockGolemHit");
				}
				Game1.playSound("hitEnemy");
			}
			return num;
		}

		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.DarkGray, 10, false, 100f, 0, -1, -1f, -1, 0));
			Game1.playSound("rockGolemDie");
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 576, 64, 64), 32, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, Game1.random.Next(4, 9), (int)base.getTileLocation().Y);
		}

		public override void noMovementProgressNearPlayerBehavior()
		{
			if (base.IsWalkingTowardPlayer)
			{
				this.Halt();
				base.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(base.getTileLocation()).getStandingPosition(), 0);
			}
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			if (base.IsWalkingTowardPlayer)
			{
				base.behaviorAtGameTick(time);
			}
			if (!this.seenPlayer)
			{
				if (this.withinPlayerThreshold())
				{
					Game1.playSound("rockGolemSpawn");
					this.seenPlayer = true;
					return;
				}
			}
			else if (this.sprite.CurrentFrame >= 16)
			{
				this.sprite.Animate(time, 16, 8, 75f);
				if (this.sprite.CurrentFrame >= 24)
				{
					this.sprite.loop = true;
					this.sprite.CurrentFrame = 0;
					this.moveTowardPlayerThreshold = 16;
					base.IsWalkingTowardPlayer = true;
					this.jitteriness = 0.01;
					this.hideShadow = false;
					return;
				}
			}
			else if (base.IsWalkingTowardPlayer && Game1.random.NextDouble() < 0.001 && Utility.isOnScreen(base.getStandingPosition(), 0))
			{
				this.controller = new PathFindController(this, Game1.currentLocation, new Point((int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y), -1, null, 200);
			}
		}
	}
}
