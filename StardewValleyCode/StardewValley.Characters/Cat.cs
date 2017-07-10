using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Characters
{
	public class Cat : Pet
	{
		public Cat()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\cat"), 0, 32, 32);
			this.hideShadow = true;
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
		}

		public Cat(int xTile, int yTile)
		{
			this.name = "Cat";
			base.displayName = this.name;
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\cat"), 0, 32, 32);
			this.position = new Vector2((float)xTile, (float)yTile) * (float)Game1.tileSize;
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
			this.currentLocation = Game1.currentLocation;
			this.hideShadow = true;
		}

		public override void initiateCurrentBehavior()
		{
			base.initiateCurrentBehavior();
		}

		public override void update(GameTime time, GameLocation location)
		{
			base.update(time, location);
			if (this.currentLocation == null)
			{
				this.currentLocation = location;
			}
			if (Game1.eventUp)
			{
				return;
			}
			if (Game1.timeOfDay > 2000 && this.sprite.currentAnimation == null && this.xVelocity == 0f && this.yVelocity == 0f)
			{
				base.CurrentBehavior = 1;
			}
			switch (base.CurrentBehavior)
			{
			case 0:
				if (this.sprite.currentAnimation == null && Game1.random.NextDouble() < 0.01)
				{
					switch (Game1.random.Next(4))
					{
					case 0:
					case 1:
					case 2:
						base.CurrentBehavior = 0;
						break;
					case 3:
						switch (this.facingDirection)
						{
						case 0:
						case 2:
							this.Halt();
							this.faceDirection(2);
							this.sprite.loop = false;
							base.CurrentBehavior = 2;
							break;
						case 1:
							if (Game1.random.NextDouble() < 0.85)
							{
								this.sprite.loop = false;
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(24, 100),
									new FarmerSprite.AnimationFrame(25, 100),
									new FarmerSprite.AnimationFrame(26, 100),
									new FarmerSprite.AnimationFrame(27, Game1.random.Next(8000, 30000), false, false, new AnimatedSprite.endOfAnimationBehavior(this.flopSound), false)
								});
							}
							else
							{
								this.sprite.loop = false;
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(30, 300),
									new FarmerSprite.AnimationFrame(31, 300),
									new FarmerSprite.AnimationFrame(30, 300),
									new FarmerSprite.AnimationFrame(31, 300),
									new FarmerSprite.AnimationFrame(30, 300),
									new FarmerSprite.AnimationFrame(31, 500),
									new FarmerSprite.AnimationFrame(24, 800, false, false, new AnimatedSprite.endOfAnimationBehavior(this.leap), false),
									new FarmerSprite.AnimationFrame(4, 1)
								});
							}
							break;
						case 3:
							if (Game1.random.NextDouble() < 0.85)
							{
								this.sprite.loop = false;
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(24, 100, false, true, null, false),
									new FarmerSprite.AnimationFrame(25, 100, false, true, null, false),
									new FarmerSprite.AnimationFrame(26, 100, false, true, null, false),
									new FarmerSprite.AnimationFrame(27, Game1.random.Next(8000, 30000), false, true, new AnimatedSprite.endOfAnimationBehavior(this.flopSound), false),
									new FarmerSprite.AnimationFrame(12, 1)
								});
							}
							else
							{
								this.sprite.loop = false;
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(30, 300, false, true, null, false),
									new FarmerSprite.AnimationFrame(31, 300, false, true, null, false),
									new FarmerSprite.AnimationFrame(30, 300, false, true, null, false),
									new FarmerSprite.AnimationFrame(31, 300, false, true, null, false),
									new FarmerSprite.AnimationFrame(30, 300, false, true, null, false),
									new FarmerSprite.AnimationFrame(31, 500, false, true, null, false),
									new FarmerSprite.AnimationFrame(24, 800, false, true, new AnimatedSprite.endOfAnimationBehavior(this.leap), false),
									new FarmerSprite.AnimationFrame(12, 1)
								});
							}
							break;
						}
						break;
					}
				}
				break;
			case 1:
				if (Game1.timeOfDay < 2000 && Game1.random.NextDouble() < 0.001)
				{
					base.CurrentBehavior = 0;
					return;
				}
				if (Game1.random.NextDouble() < 0.002)
				{
					base.doEmote(24, true);
				}
				return;
			case 2:
				if (base.Sprite.currentFrame != 18 && this.sprite.currentAnimation == null)
				{
					base.CurrentBehavior = 2;
				}
				else if (base.Sprite.currentFrame == 18 && Game1.random.NextDouble() < 0.01)
				{
					switch (Game1.random.Next(10))
					{
					case 0:
						base.CurrentBehavior = 0;
						this.Halt();
						this.faceDirection(2);
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(17, 200),
							new FarmerSprite.AnimationFrame(16, 200),
							new FarmerSprite.AnimationFrame(0, 200)
						});
						this.sprite.loop = false;
						break;
					case 1:
					case 2:
					case 3:
					{
						List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(19, 300),
							new FarmerSprite.AnimationFrame(20, 200),
							new FarmerSprite.AnimationFrame(21, 200),
							new FarmerSprite.AnimationFrame(22, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.lickSound), false),
							new FarmerSprite.AnimationFrame(23, 200)
						};
						int num = Game1.random.Next(1, 6);
						for (int i = 0; i < num; i++)
						{
							list.Add(new FarmerSprite.AnimationFrame(21, 150));
							list.Add(new FarmerSprite.AnimationFrame(22, 150, false, false, new AnimatedSprite.endOfAnimationBehavior(this.lickSound), false));
							list.Add(new FarmerSprite.AnimationFrame(23, 150));
						}
						list.Add(new FarmerSprite.AnimationFrame(18, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(base.hold), false));
						this.sprite.loop = false;
						this.sprite.setCurrentAnimation(list);
						break;
					}
					default:
					{
						bool flag = Game1.random.NextDouble() < 0.45;
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(19, flag ? 200 : Game1.random.Next(1000, 9000)),
							new FarmerSprite.AnimationFrame(18, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
						});
						this.sprite.loop = false;
						if (flag && Game1.random.NextDouble() < 0.2)
						{
							this.playContentSound();
							base.shake(200);
						}
						break;
					}
					}
				}
				break;
			}
			if (this.sprite.currentAnimation != null)
			{
				this.sprite.loop = false;
			}
			if (this.sprite.currentAnimation == null)
			{
				this.MovePosition(time, Game1.viewport, location);
				return;
			}
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				Rectangle boundingBox = this.GetBoundingBox();
				boundingBox.X += (int)this.xVelocity;
				boundingBox.Y -= (int)this.yVelocity;
				if (this.currentLocation == null || !this.currentLocation.isCollidingPosition(boundingBox, Game1.viewport, false, 0, false, this))
				{
					this.position.X = this.position.X + (float)((int)this.xVelocity);
					this.position.Y = this.position.Y - (float)((int)this.yVelocity);
				}
				this.xVelocity = (float)((int)(this.xVelocity - this.xVelocity / 4f));
				this.yVelocity = (float)((int)(this.yVelocity - this.yVelocity / 4f));
			}
		}

		public void lickSound(Farmer who)
		{
			if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
			{
				Game1.playSound("Cowboy_Footstep");
			}
		}

		public void leap(Farmer who)
		{
			if (this.currentLocation.Equals(Game1.currentLocation))
			{
				this.jump();
			}
			if (this.facingDirection == 1)
			{
				this.xVelocity = 8f;
				return;
			}
			if (this.facingDirection == 3)
			{
				this.xVelocity = -8f;
			}
		}

		public void flopSound(Farmer who)
		{
			if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
			{
				Game1.playSound("thudStep");
			}
		}

		public override void playContentSound()
		{
			if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
			{
				Game1.playSound("cat");
			}
		}
	}
}
