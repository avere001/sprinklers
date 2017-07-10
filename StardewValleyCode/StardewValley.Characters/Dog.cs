using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Characters
{
	public class Dog : Pet
	{
		public const int behavior_sit_right = 50;

		public const int behavior_sprint = 51;

		private int sprintTimer;

		private bool wagging;

		public Dog()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\dog"), 0, 32, 32);
			this.hideShadow = true;
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
		}

		public Dog(int xTile, int yTile)
		{
			this.name = "Dog";
			base.displayName = this.name;
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\dog"), 0, 32, 32);
			this.position = new Vector2((float)xTile, (float)yTile) * (float)Game1.tileSize;
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
			this.currentLocation = Game1.currentLocation;
			this.hideShadow = true;
		}

		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			this.sprintTimer = 0;
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
			if (this.sprintTimer > 0)
			{
				this.sprite.loop = true;
				this.sprintTimer -= time.ElapsedGameTime.Milliseconds;
				this.speed = 6;
				base.tryToMoveInDirection(this.facingDirection, false, -1, false);
				if (this.sprintTimer <= 0)
				{
					this.sprite.currentAnimation = null;
					this.Halt();
					this.faceDirection(this.facingDirection);
					this.speed = 2;
					base.CurrentBehavior = 0;
				}
				return;
			}
			if (Game1.timeOfDay > 2000 && this.sprite.currentAnimation == null && this.xVelocity == 0f && this.yVelocity == 0f)
			{
				base.CurrentBehavior = 1;
			}
			int currentBehavior = base.CurrentBehavior;
			switch (currentBehavior)
			{
			case 0:
				if (this.sprite.currentAnimation == null && Game1.random.NextDouble() < 0.01)
				{
					switch (Game1.random.Next(7 + ((this.currentLocation is Farm) ? 1 : 0)))
					{
					case 0:
					case 1:
					case 2:
					case 3:
						base.CurrentBehavior = 0;
						break;
					case 4:
					case 5:
						switch (this.facingDirection)
						{
						case 0:
						case 1:
						case 3:
							this.Halt();
							if (this.facingDirection == 0)
							{
								this.facingDirection = ((Game1.random.NextDouble() < 0.5) ? 3 : 1);
							}
							this.faceDirection(this.facingDirection);
							this.sprite.loop = false;
							base.CurrentBehavior = 50;
							break;
						case 2:
							this.Halt();
							this.faceDirection(2);
							this.sprite.loop = false;
							base.CurrentBehavior = 2;
							break;
						}
						break;
					case 6:
					case 7:
						base.CurrentBehavior = 51;
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
					switch (Game1.random.Next(4))
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
					{
						List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(18, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false),
							new FarmerSprite.AnimationFrame(19, 200)
						};
						int num = Game1.random.Next(7, 20);
						for (int i = 0; i < num; i++)
						{
							list.Add(new FarmerSprite.AnimationFrame(18, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false));
							list.Add(new FarmerSprite.AnimationFrame(19, 200));
						}
						this.sprite.setCurrentAnimation(list);
						break;
					}
					case 2:
					case 3:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(27, (Game1.random.NextDouble() < 0.3) ? 500 : Game1.random.Next(2000, 15000)),
							new FarmerSprite.AnimationFrame(18, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
						});
						this.sprite.loop = false;
						break;
					}
				}
				break;
			default:
				if (currentBehavior == 50)
				{
					if (base.withinPlayerThreshold(2))
					{
						if (!this.wagging)
						{
							this.wag(this.facingDirection == 3);
						}
					}
					else if (base.Sprite.currentFrame != 23 && this.sprite.currentAnimation == null)
					{
						this.sprite.CurrentFrame = 23;
					}
					else if (this.sprite.currentFrame == 23 && Game1.random.NextDouble() < 0.01)
					{
						bool flag = this.facingDirection == 3;
						switch (Game1.random.Next(7))
						{
						case 0:
							base.CurrentBehavior = 0;
							this.Halt();
							this.faceDirection(flag ? 3 : 1);
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(23, 100, false, flag, null, false),
								new FarmerSprite.AnimationFrame(22, 100, false, flag, null, false),
								new FarmerSprite.AnimationFrame(21, 100, false, flag, null, false),
								new FarmerSprite.AnimationFrame(20, 100, false, flag, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
							});
							this.sprite.loop = false;
							break;
						case 1:
							if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 10, this.currentLocation))
							{
								Game1.playSound("dog_bark");
								base.shake(500);
							}
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(26, 500, false, flag, null, false),
								new FarmerSprite.AnimationFrame(23, 1, false, flag, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
							});
							break;
						case 2:
							this.wag(flag);
							break;
						case 3:
						case 4:
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(23, Game1.random.Next(2000, 6000), false, flag, null, false),
								new FarmerSprite.AnimationFrame(23, 1, false, flag, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
							});
							break;
						default:
						{
							this.sprite.loop = false;
							List<FarmerSprite.AnimationFrame> list2 = new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(24, 200, false, flag, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false),
								new FarmerSprite.AnimationFrame(25, 200, false, flag, null, false)
							};
							int num2 = Game1.random.Next(5, 15);
							for (int j = 0; j < num2; j++)
							{
								list2.Add(new FarmerSprite.AnimationFrame(24, 200, false, flag, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false));
								list2.Add(new FarmerSprite.AnimationFrame(25, 200, false, flag, null, false));
							}
							this.sprite.setCurrentAnimation(list2);
							break;
						}
						}
					}
				}
				break;
			}
			if (this.sprite.currentAnimation != null)
			{
				this.sprite.loop = false;
			}
			else
			{
				this.wagging = false;
			}
			if (this.sprite.currentAnimation == null)
			{
				this.MovePosition(time, Game1.viewport, location);
			}
		}

		public void wag(bool localFlip)
		{
			int milliseconds = base.withinPlayerThreshold(2) ? 120 : 200;
			this.wagging = true;
			this.sprite.loop = false;
			List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(31, milliseconds, false, localFlip, null, false),
				new FarmerSprite.AnimationFrame(23, milliseconds, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false)
			};
			int num = Game1.random.Next(2, 6);
			for (int i = 0; i < num; i++)
			{
				list.Add(new FarmerSprite.AnimationFrame(31, milliseconds, false, localFlip, null, false));
				list.Add(new FarmerSprite.AnimationFrame(23, milliseconds, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false));
			}
			list.Add(new FarmerSprite.AnimationFrame(23, 2, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.doneWagging), false));
			this.sprite.setCurrentAnimation(list);
		}

		public void doneWagging(Farmer who)
		{
			this.wagging = false;
		}

		public override void initiateCurrentBehavior()
		{
			this.sprintTimer = 0;
			base.initiateCurrentBehavior();
			bool flip = this.facingDirection == 3;
			int currentBehavior = base.CurrentBehavior;
			if (currentBehavior == 50)
			{
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(20, 100, false, flip, null, false),
					new FarmerSprite.AnimationFrame(21, 100, false, flip, null, false),
					new FarmerSprite.AnimationFrame(22, 100, false, flip, null, false),
					new FarmerSprite.AnimationFrame(23, 100, false, flip, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
				});
				return;
			}
			if (currentBehavior != 51)
			{
				return;
			}
			this.faceDirection((Game1.random.NextDouble() < 0.5) ? 3 : 1);
			flip = (this.facingDirection == 3);
			this.sprintTimer = Game1.random.Next(1000, 3500);
			if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize, this.currentLocation))
			{
				Game1.playSound("dog_bark");
			}
			this.sprite.loop = true;
			this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(32, 100, false, flip, null, false),
				new FarmerSprite.AnimationFrame(33, 100, false, flip, null, false),
				new FarmerSprite.AnimationFrame(34, 100, false, flip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false),
				new FarmerSprite.AnimationFrame(33, 100, false, flip, null, false)
			});
		}

		public void hitGround(Farmer who)
		{
			if (Utility.isOnScreen(base.getTileLocationPoint(), 2 * Game1.tileSize, this.currentLocation))
			{
				this.currentLocation.playTerrainSound(base.getTileLocation(), this, false);
			}
		}

		public void pantSound(Farmer who)
		{
			if (base.withinPlayerThreshold(5))
			{
				Game1.playSound("dog_pant");
			}
		}

		public void thumpSound(Farmer who)
		{
			if (base.withinPlayerThreshold(4))
			{
				Game1.playSound("thudStep");
			}
		}

		public override void playContentSound()
		{
			if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
			{
				Game1.playSound("dog_pant");
				DelayedAction.playSoundAfterDelay("dog_pant", 400);
			}
		}
	}
}
