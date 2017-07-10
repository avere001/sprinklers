using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
	public class SkeletonWarrior : Monster
	{
		public SkeletonWarrior()
		{
		}

		public SkeletonWarrior(Vector2 position) : base("Skeleton Warrior", position)
		{
			this.slipperiness = 1;
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Skeleton Warrior"));
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				if ((this.sprite.CurrentFrame == 16 && Game1.player.position.Y > this.position.Y + (float)(Game1.tileSize / 2)) || (this.sprite.CurrentFrame == 17 && Game1.player.position.X > this.position.X + (float)(Game1.tileSize / 2)) || (this.sprite.CurrentFrame == 18 && Game1.player.position.Y < this.position.Y - (float)Game1.tileSize) || (this.sprite.CurrentFrame == 19 && Game1.player.position.X < this.position.X - (float)(Game1.tileSize / 2)))
				{
					num = 0;
					Game1.playSound("crafting");
				}
				if (Game1.random.NextDouble() < 0.25)
				{
					this.Halt();
					num = 0;
					Game1.playSound("crafting");
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
					this.timeBeforeAIMovementAgain = 400f;
				}
				this.health -= num;
				if (num > 0 && Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
				{
					this.health -= damage * 3 / 4;
					Game1.currentLocation.debris.Add(new Debris(string.Concat(damage * 3 / 4), 1, new Vector2((float)base.getStandingX(), (float)base.getStandingY()), Color.LightBlue, 1f, 0f));
				}
				if (num > 0)
				{
					base.setTrajectory(xTrajectory, yTrajectory);
				}
			}
			if (this.health <= 0)
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
			return num;
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.withinPlayerThreshold())
			{
				if (Game1.random.NextDouble() < 0.005)
				{
					this.willDestroyObjectsUnderfoot = true;
				}
				if (Game1.random.NextDouble() < 0.01)
				{
					this.willDestroyObjectsUnderfoot = false;
				}
				if (base.withinPlayerThreshold(2) && Game1.random.NextDouble() < 0.01)
				{
					Game1.playSound("swordswipe");
					switch (this.facingDirection)
					{
					case 0:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float)(Game1.tileSize / 2), this.position.Y - (float)(Game1.tileSize / 4)), false, false, true, -1.57079637f));
						break;
					case 1:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float)Game1.tileSize, this.position.Y + (float)(Game1.tileSize * 3 / 4)), false, false));
						break;
					case 2:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float)(Game1.tileSize / 2), this.position.Y + (float)(Game1.tileSize * 3 / 2)), false, false, true, 1.57079637f));
						break;
					case 3:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X - (float)(Game1.tileSize / 4), this.position.Y + (float)(Game1.tileSize * 3 / 4)), false, true));
						break;
					}
					int num = (int)this.GetToolLocation(false).X;
					int num2 = (int)this.GetToolLocation(false).Y;
					Rectangle empty = Rectangle.Empty;
					Rectangle boundingBox = this.GetBoundingBox();
					switch (this.facingDirection)
					{
					case 0:
						empty = new Rectangle(num - Game1.tileSize, boundingBox.Y - Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
						break;
					case 1:
						empty = new Rectangle(boundingBox.Right, num2 - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
						break;
					case 2:
						empty = new Rectangle(num - Game1.tileSize, boundingBox.Bottom, Game1.tileSize * 2, Game1.tileSize);
						break;
					case 3:
						empty = new Rectangle(boundingBox.Left - Game1.tileSize, num2 - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
						break;
					}
					Game1.currentLocation.isCollidingPosition(empty, Game1.viewport, false, Game1.random.Next(2, 10), true);
				}
			}
		}
	}
}
