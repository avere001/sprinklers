using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
	public class Mummy : Monster
	{
		private int reviveTimer;

		public const int revivalTime = 10000;

		public Mummy()
		{
			this.sprite.spriteHeight = 32;
		}

		public Mummy(Vector2 position) : base("Mummy", position)
		{
			this.sprite.spriteHeight = 32;
			this.sprite.ignoreStopAnimation = true;
			this.sprite.UpdateSourceRect();
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Mummy"));
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
			this.sprite.ignoreStopAnimation = true;
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (this.reviveTimer <= 0)
			{
				if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
				{
					num = -1;
				}
				else
				{
					this.slipperiness = 2;
					this.health -= num;
					base.setTrajectory(xTrajectory, yTrajectory);
					Game1.playSound("shadowHit");
					Game1.playSound("skeletonStep");
					base.IsWalkingTowardPlayer = true;
					if (this.health <= 0)
					{
						this.health = this.maxHealth;
						this.deathAnimation();
					}
				}
				return num;
			}
			if (isBomb)
			{
				this.health = 0;
				Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.BlueViolet, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					holdLastFrame = true,
					alphaFade = 0.01f,
					interval = 70f
				}, Game1.currentLocation, 4, 64, 64);
				Game1.playSound("ghost");
				return 999;
			}
			return -1;
		}

		public override void deathAnimation()
		{
			Game1.playSound("monsterdead");
			this.reviveTimer = 10000;
			this.sprite.setCurrentAnimation(this.getCrumbleAnimation(false));
			this.Halt();
			this.collidesWithOtherCharacters = false;
			base.IsWalkingTowardPlayer = false;
			this.moveTowardPlayerThreshold = -1;
		}

		private List<FarmerSprite.AnimationFrame> getCrumbleAnimation(bool reverse = false)
		{
			List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
			if (!reverse)
			{
				list.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, null, false, 0));
			}
			else
			{
				list.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.behaviorAfterRevival), true, 0));
			}
			list.Add(new FarmerSprite.AnimationFrame(17, 100, 0, false, false, null, false, 0));
			list.Add(new FarmerSprite.AnimationFrame(18, 100, 0, false, false, null, false, 0));
			if (!reverse)
			{
				list.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.behaviorAfterCrumble), false, 0));
			}
			else
			{
				list.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, null, false, 0));
			}
			if (reverse)
			{
				list.Reverse();
			}
			return list;
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			if (this.sprite.currentAnimation != null && base.Sprite.animateOnce(time))
			{
				this.sprite.currentAnimation = null;
			}
			if (this.reviveTimer > 0)
			{
				this.reviveTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.reviveTimer < 2000)
				{
					base.shake(this.reviveTimer);
				}
				if (this.reviveTimer <= 0)
				{
					this.sprite.setCurrentAnimation(this.getCrumbleAnimation(true));
					Game1.playSound("skeletonDie");
					base.IsWalkingTowardPlayer = true;
				}
			}
			if (this.withinPlayerThreshold())
			{
				base.IsWalkingTowardPlayer = true;
			}
			base.behaviorAtGameTick(time);
		}

		private void behaviorAfterCrumble(Farmer who)
		{
			this.Halt();
			this.sprite.CurrentFrame = 19;
			this.sprite.currentAnimation = null;
		}

		private void behaviorAfterRevival(Farmer who)
		{
			base.IsWalkingTowardPlayer = true;
			this.collidesWithOtherCharacters = true;
			this.sprite.CurrentFrame = 0;
			this.moveTowardPlayerThreshold = 8;
			this.sprite.currentAnimation = null;
		}
	}
}
