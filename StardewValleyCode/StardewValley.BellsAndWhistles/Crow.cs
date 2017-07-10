using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Crow : Critter
	{
		public const int flyingSpeed = 6;

		public const int pecking = 0;

		public const int flyingAway = 1;

		public const int sleeping = 2;

		public const int stopped = 3;

		private int state;

		public Crow(int tileX, int tileY) : base(14, new Vector2((float)(tileX * Game1.tileSize), (float)(tileY * Game1.tileSize)))
		{
			this.flip = (Game1.random.NextDouble() < 0.5);
			this.position.X = this.position.X + (float)(Game1.tileSize / 2);
			this.position.Y = this.position.Y + (float)(Game1.tileSize / 2);
			this.startingPosition = this.position;
			this.state = 0;
		}

		public void hop(Farmer who)
		{
			this.gravityAffectedDY = -4f;
		}

		private void donePecking(Farmer who)
		{
			this.state = ((Game1.random.NextDouble() < 0.5) ? 0 : 3);
		}

		private void playFlap(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("batFlap");
			}
		}

		private void playPeck(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("shiny4");
			}
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			Farmer farmer = Utility.isThereAFarmerWithinDistance(this.position / (float)Game1.tileSize, 4);
			if (this.yJumpOffset < 0f && this.state != 1)
			{
				if (!this.flip && !environment.isCollidingPosition(this.getBoundingBox(-2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X - 2f;
				}
				else if (!environment.isCollidingPosition(this.getBoundingBox(2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X + 2f;
				}
			}
			if (farmer != null && this.state != 1)
			{
				if (Game1.random.NextDouble() < 0.85)
				{
					Game1.playSound("crow");
				}
				this.state = 1;
				if (farmer.position.X > this.position.X)
				{
					this.flip = false;
				}
				else
				{
					this.flip = true;
				}
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 6)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 10)), 40, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 40)
				});
				this.sprite.loop = true;
			}
			switch (this.state)
			{
			case 0:
				if (this.sprite.currentAnimation == null)
				{
					List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
					list.Add(new FarmerSprite.AnimationFrame((int)((short)this.baseFrame), 480));
					list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 1)), 170, false, this.flip, null, false));
					list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 170, false, this.flip, null, false));
					int num = Game1.random.Next(1, 5);
					for (int i = 0; i < num; i++)
					{
						list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 70));
						list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 100, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playPeck), false));
					}
					list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 100));
					list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 70, false, this.flip, null, false));
					list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 1)), 70, false, this.flip, null, false));
					list.Add(new FarmerSprite.AnimationFrame((int)((short)this.baseFrame), 500, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.donePecking), false));
					this.sprite.loop = false;
					this.sprite.setCurrentAnimation(list);
				}
				break;
			case 1:
				if (!this.flip)
				{
					this.position.X = this.position.X - 6f;
				}
				else
				{
					this.position.X = this.position.X + 6f;
				}
				this.yOffset -= 2f;
				break;
			case 2:
				if (this.sprite.currentAnimation == null)
				{
					this.sprite.CurrentFrame = this.baseFrame + 5;
				}
				if (Game1.random.NextDouble() < 0.003 && this.sprite.currentAnimation == null)
				{
					this.state = 3;
				}
				break;
			case 3:
				if (Game1.random.NextDouble() < 0.008 && this.sprite.currentAnimation == null && this.yJumpOffset >= 0f)
				{
					switch (Game1.random.Next(5))
					{
					case 0:
						this.state = 2;
						break;
					case 1:
						this.state = 0;
						break;
					case 2:
						this.hop(null);
						break;
					case 3:
						this.flip = !this.flip;
						this.hop(null);
						break;
					case 4:
						this.state = 1;
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 6)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 10)), 50, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 50)
						});
						this.sprite.loop = true;
						break;
					}
				}
				else if (this.sprite.currentAnimation == null)
				{
					this.sprite.CurrentFrame = this.baseFrame;
				}
				break;
			}
			return base.update(time, environment);
		}
	}
}
