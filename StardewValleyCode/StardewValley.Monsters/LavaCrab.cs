using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Monsters
{
	public class LavaCrab : Monster
	{
		private bool leftDrift;

		public LavaCrab()
		{
		}

		public LavaCrab(Vector2 position) : base("Lava Crab", position)
		{
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else if (this.sprite.CurrentFrame % 4 == 0)
			{
				num = 0;
				Game1.playSound("crafting");
			}
			else
			{
				this.health -= num;
				Game1.playSound("hitEnemy");
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, Color.Purple, 10, false, 100f, 0, -1, -1f, -1, 0));
					Game1.playSound("monsterdead");
				}
			}
			return num;
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.isMoving() && this.sprite.CurrentFrame % 4 == 0)
			{
				AnimatedSprite expr_24 = this.sprite;
				int currentFrame = expr_24.CurrentFrame;
				expr_24.CurrentFrame = currentFrame + 1;
				this.sprite.UpdateSourceRect();
			}
			if (!this.withinPlayerThreshold())
			{
				this.Halt();
			}
		}
	}
}
