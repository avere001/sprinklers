using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System;

namespace StardewValley.Monsters
{
	public class Crow : Monster
	{
		private bool startedFlying;

		private bool flyLeft;

		private float flightRise = 0.1f;

		public Crow()
		{
		}

		public Crow(Vector2 position) : base("Crow", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.sprite.spriteWidth = Game1.tileSize;
			this.sprite.spriteHeight = Game1.tileSize;
			this.sprite.UpdateSourceRect();
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			return Math.Max(1, damage - this.resilience);
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.withinPlayerThreshold() && !this.startedFlying)
			{
				if (!this.startedFlying)
				{
					this.speed = 6;
					if (Game1.player.position.X < this.position.X - (float)Game1.tileSize)
					{
						this.flyLeft = false;
					}
					else if (Game1.player.position.X > this.position.X + (float)(Game1.tileSize * 2))
					{
						this.flyLeft = true;
					}
					else
					{
						this.flyLeft = (Game1.random.NextDouble() < 0.5);
					}
					if (!this.flyLeft)
					{
						this.flip = true;
					}
					this.startedFlying = true;
					this.drawOnTop = true;
					return;
				}
			}
			else
			{
				if (this.startedFlying)
				{
					this.sprite.Animate(time, 20, 4, 75f);
					this.position.X = this.position.X + (float)(this.flyLeft ? (-(float)this.speed) : this.speed);
					this.position.Y = this.position.Y - this.flightRise;
					this.flightRise += 0.1f;
					return;
				}
				if (Game1.currentLocation.isCropAtTile((int)base.getTileLocation().X, (int)base.getTileLocation().Y) && Game1.random.NextDouble() < 0.003)
				{
					this.Halt();
					switch (this.facingDirection)
					{
					case 0:
						this.sprite.CurrentFrame = 18;
						break;
					case 1:
						this.sprite.CurrentFrame = 17;
						break;
					case 2:
						this.sprite.CurrentFrame = 16;
						break;
					case 3:
						this.sprite.CurrentFrame = 19;
						break;
					}
					this.sprite.UpdateSourceRect();
					if (Game1.currentLocation.terrainFeatures[base.getTileLocation()] != null && Game1.currentLocation.terrainFeatures[base.getTileLocation()].GetType() == typeof(HoeDirt))
					{
						((HoeDirt)Game1.currentLocation.terrainFeatures[base.getTileLocation()]).destroyCrop(base.getTileLocation(), true);
					}
				}
				else if (Game1.random.NextDouble() < 0.01)
				{
					switch (Game1.random.Next(6))
					{
					case 0:
						base.SetMovingOnlyUp();
						break;
					case 1:
						base.SetMovingOnlyRight();
						break;
					case 2:
						base.SetMovingOnlyDown();
						break;
					case 3:
						base.SetMovingOnlyLeft();
						break;
					default:
						this.Halt();
						break;
					}
				}
				this.MovePosition(time, Game1.viewport, Game1.currentLocation);
			}
		}
	}
}
