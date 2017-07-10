using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Seagull : Critter
	{
		public const int walkingSpeed = 2;

		public const int flyingSpeed = 4;

		public const int walking = 0;

		public const int flyingAway = 1;

		public const int flyingToLand = 4;

		public const int swimming = 2;

		public const int stopped = 3;

		private int state;

		private int characterCheckTimer = 200;

		private bool moveLeft;

		public Seagull(Vector2 position, int startingState) : base(0, position)
		{
			this.moveLeft = (Game1.random.NextDouble() < 0.5);
			this.startingPosition = position;
			this.state = startingState;
		}

		public void hop(Farmer who)
		{
			this.gravityAffectedDY = -4f;
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.characterCheckTimer < 0)
			{
				Character character = Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float)Game1.tileSize, 4, environment);
				this.characterCheckTimer = 200;
				if (character != null && this.state != 1)
				{
					if (Game1.random.NextDouble() < 0.25)
					{
						Game1.playSound("seagulls");
					}
					this.state = 1;
					if (character.position.X > this.position.X)
					{
						this.moveLeft = true;
					}
					else
					{
						this.moveLeft = false;
					}
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 10)), 80),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 11)), 80),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 12)), 80),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 13)), 100)
					});
					this.sprite.loop = true;
				}
			}
			switch (this.state)
			{
			case 0:
				if (this.moveLeft && !environment.isCollidingPosition(this.getBoundingBox(-2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X - 2f;
				}
				else if (!this.moveLeft && !environment.isCollidingPosition(this.getBoundingBox(2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X + 2f;
				}
				if (Game1.random.NextDouble() < 0.005)
				{
					this.state = 3;
					this.sprite.loop = false;
					this.sprite.currentAnimation = null;
					this.sprite.CurrentFrame = 0;
				}
				break;
			case 1:
				if (this.moveLeft)
				{
					this.position.X = this.position.X - 4f;
				}
				else
				{
					this.position.X = this.position.X + 4f;
				}
				this.yOffset -= 2f;
				break;
			case 2:
			{
				this.sprite.CurrentFrame = this.baseFrame + 9;
				float yOffset = this.yOffset;
				if ((time.TotalGameTime.TotalMilliseconds + (double)((int)this.position.X * 4)) % 2000.0 < 1000.0)
				{
					this.yOffset = 2f;
				}
				else
				{
					this.yOffset = 0f;
				}
				if (this.yOffset > yOffset)
				{
					environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f, 8, 0, new Vector2(this.position.X - (float)(Game1.tileSize / 2), this.position.Y - (float)(Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
				}
				break;
			}
			case 3:
				if (Game1.random.NextDouble() < 0.003 && this.sprite.currentAnimation == null)
				{
					this.sprite.loop = false;
					switch (Game1.random.Next(4))
					{
					case 0:
					{
						List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 100),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 100),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 200),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 5)), 200)
						};
						int num = Game1.random.Next(5);
						for (int i = 0; i < num; i++)
						{
							list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 200));
							list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 5)), 200));
						}
						this.sprite.setCurrentAnimation(list);
						break;
					}
					case 1:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(6, (int)((short)Game1.random.Next(500, 4000)))
						});
						break;
					case 2:
					{
						List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 6)), 500),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 100, false, false, new AnimatedSprite.endOfAnimationBehavior(this.hop), false),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 100)
						};
						int num = Game1.random.Next(3);
						for (int j = 0; j < num; j++)
						{
							list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 100));
							list.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 100));
						}
						this.sprite.setCurrentAnimation(list);
						break;
					}
					case 3:
						this.state = 0;
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)this.baseFrame), 200),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 1)), 200)
						});
						this.sprite.loop = true;
						this.moveLeft = (Game1.random.NextDouble() < 0.5);
						if (Game1.random.NextDouble() < 0.33)
						{
							if (this.position.X > this.startingPosition.X)
							{
								this.moveLeft = true;
							}
							else
							{
								this.moveLeft = false;
							}
						}
						break;
					}
				}
				else if (this.sprite.currentAnimation == null)
				{
					this.sprite.CurrentFrame = this.baseFrame;
				}
				break;
			}
			this.flip = !this.moveLeft;
			return base.update(time, environment);
		}
	}
}
