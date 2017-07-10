using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	public class Monster : NPC
	{
		protected delegate void collisionBehavior(GameLocation location);

		public const int defaultInvincibleCountdown = 450;

		public const int seekPlayerIterationLimit = 80;

		public int damageToFarmer;

		public int health;

		public int maxHealth;

		public int coinsToDrop;

		public int durationOfRandomMovements;

		public int resilience;

		public int slipperiness = 2;

		public int experienceGained;

		public double jitteriness;

		public double missChance;

		public bool isGlider;

		public bool mineMonster;

		public bool hasSpecialItem;

		public List<int> objectsToDrop = new List<int>();

		protected int skipHorizontal;

		protected int invincibleCountdown;

		private bool skipHorizontalUp;

		protected int defaultAnimationInterval = 175;

		[XmlIgnore]
		public bool focusedOnFarmers;

		[XmlIgnore]
		public bool wildernessFarmMonster;

		protected Monster.collisionBehavior onCollision;

		private int slideAnimationTimer;

		public override bool IsMonster
		{
			get
			{
				return true;
			}
		}

		public Monster()
		{
		}

		public Monster(string name, Vector2 position) : this(name, position, 2)
		{
			this.breather = false;
		}

		public virtual List<Item> getExtraDropItems()
		{
			return new List<Item>();
		}

		public override bool withinPlayerThreshold()
		{
			return this.focusedOnFarmers || base.withinPlayerThreshold(this.moveTowardPlayerThreshold);
		}

		public Monster(string name, Vector2 position, int facingDir) : base(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + name)), position, facingDir, name, null)
		{
			this.parseMonsterInfo(name);
			this.breather = false;
		}

		public virtual void drawAboveAllLayers(SpriteBatch b)
		{
		}

		public override void draw(SpriteBatch b)
		{
			if (!this.isGlider)
			{
				base.draw(b);
			}
		}

		public bool isInvincible()
		{
			return this.invincibleCountdown > 0;
		}

		public void setInvincibleCountdown(int time)
		{
			this.invincibleCountdown = time;
			base.startGlowing(new Color(255, 0, 0), false, 0.25f);
			this.glowingTransparency = 1f;
		}

		protected void parseMonsterInfo(string name)
		{
			string[] array = Game1.content.Load<Dictionary<string, string>>("Data\\Monsters")[name].Split(new char[]
			{
				'/'
			});
			this.health = Convert.ToInt32(array[0]);
			this.maxHealth = this.health;
			this.damageToFarmer = Convert.ToInt32(array[1]);
			this.coinsToDrop = Game1.random.Next(Convert.ToInt32(array[2]), Convert.ToInt32(array[3]) + 1);
			this.isGlider = Convert.ToBoolean(array[4]);
			this.durationOfRandomMovements = Convert.ToInt32(array[5]);
			string[] array2 = array[6].Split(new char[]
			{
				' '
			});
			this.objectsToDrop.Clear();
			for (int i = 0; i < array2.Length; i += 2)
			{
				if (Game1.random.NextDouble() < Convert.ToDouble(array2[i + 1]))
				{
					this.objectsToDrop.Add(Convert.ToInt32(array2[i]));
				}
			}
			this.resilience = Convert.ToInt32(array[7]);
			this.jitteriness = Convert.ToDouble(array[8]);
			this.willDestroyObjectsUnderfoot = false;
			base.moveTowardPlayer(Convert.ToInt32(array[9]));
			this.speed = Convert.ToInt32(array[10]);
			this.missChance = Convert.ToDouble(array[11]);
			this.mineMonster = Convert.ToBoolean(array[12]);
			if (Game1.player.timesReachedMineBottom >= 1 && this.mineMonster)
			{
				this.resilience *= 2;
				if (Game1.random.NextDouble() < 0.1)
				{
					this.addedSpeed = 1;
				}
				this.missChance *= 2.0;
				this.health += Game1.random.Next(0, this.health);
				this.damageToFarmer += Game1.random.Next(0, this.damageToFarmer);
				this.coinsToDrop += Game1.random.Next(0, this.coinsToDrop + 1);
				if (Game1.random.NextDouble() < 0.008)
				{
					this.objectsToDrop.Add((Game1.random.NextDouble() < 0.5) ? 72 : 74);
				}
			}
			try
			{
				this.experienceGained = Convert.ToInt32(array[13]);
			}
			catch (Exception)
			{
				this.experienceGained = 1;
			}
			if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
			{
				base.displayName = array[array.Length - 1];
			}
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + this.name), 0, 16, 16);
		}

		public virtual void shedChunks(int number)
		{
			this.shedChunks(number, 0.75f);
		}

		public virtual void shedChunks(int number, float scale)
		{
			if (this.sprite.Texture.Height > this.sprite.getHeight() * 4)
			{
				Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, this.sprite.getHeight() * 4 + 16, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, 1f * (float)Game1.pixelZoom * scale);
			}
		}

		public virtual void deathAnimation()
		{
			this.shedChunks(Game1.random.Next(4, 9), 0.75f);
		}

		public virtual int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			return this.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision, "hitEnemy");
		}

		public int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, string hitSound)
		{
			int num = Math.Max(1, damage - this.resilience);
			this.slideAnimationTimer = 0;
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				this.health -= num;
				Game1.playSound(hitSound);
				base.setTrajectory(xTrajectory / 3, yTrajectory / 3);
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return num;
		}

		public virtual void behaviorAtGameTick(GameTime time)
		{
			if (this.timeBeforeAIMovementAgain > 0f)
			{
				this.timeBeforeAIMovementAgain -= (float)time.ElapsedGameTime.Milliseconds;
			}
			if (Game1.player.isRafting && base.withinPlayerThreshold(4))
			{
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
			}
		}

		public virtual bool passThroughCharacters()
		{
			return false;
		}

		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return true;
		}

		public override void update(GameTime time, GameLocation location)
		{
			if (this.invincibleCountdown > 0)
			{
				this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.invincibleCountdown <= 0)
				{
					base.stopGlowing();
				}
			}
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			if (!Game1.player.isRafting || !base.withinPlayerThreshold(4))
			{
				base.update(time, location);
			}
			this.behaviorAtGameTick(time);
			if (this.controller != null && base.withinPlayerThreshold(3))
			{
				this.controller = null;
			}
			if (!this.isGlider && (this.position.X < 0f || this.position.X > (float)(location.map.GetLayer("Back").LayerWidth * Game1.tileSize) || this.position.Y < 0f || this.position.Y > (float)(location.map.GetLayer("Back").LayerHeight * Game1.tileSize)))
			{
				location.characters.Remove(this);
				return;
			}
			if (this.isGlider && this.position.X < -2000f)
			{
				this.health = -500;
			}
		}

		private bool doHorizontalMovement(GameLocation location)
		{
			bool result = false;
			if (this.position.X > Game1.player.position.X + (float)(Game1.pixelZoom * 2) || (this.skipHorizontal > 0 && Game1.player.getStandingX() < base.getStandingX() - Game1.pixelZoom * 2))
			{
				base.SetMovingOnlyLeft();
				if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					result = true;
				}
				else
				{
					this.faceDirection(3);
					if (this.durationOfRandomMovements > 0 && Game1.random.NextDouble() < this.jitteriness)
					{
						if (Game1.random.NextDouble() < 0.5)
						{
							base.tryToMoveInDirection(2, false, this.damageToFarmer, this.isGlider);
						}
						else
						{
							base.tryToMoveInDirection(0, false, this.damageToFarmer, this.isGlider);
						}
						this.timeBeforeAIMovementAgain = (float)this.durationOfRandomMovements;
					}
				}
			}
			else if (this.position.X < Game1.player.position.X - (float)(Game1.pixelZoom * 2))
			{
				base.SetMovingOnlyRight();
				if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					result = true;
				}
				else
				{
					this.faceDirection(1);
					if (this.durationOfRandomMovements > 0 && Game1.random.NextDouble() < this.jitteriness)
					{
						if (Game1.random.NextDouble() < 0.5)
						{
							base.tryToMoveInDirection(2, false, this.damageToFarmer, this.isGlider);
						}
						else
						{
							base.tryToMoveInDirection(0, false, this.damageToFarmer, this.isGlider);
						}
						this.timeBeforeAIMovementAgain = (float)this.durationOfRandomMovements;
					}
				}
			}
			else
			{
				base.faceGeneralDirection(Game1.player.getStandingPosition(), 0);
				base.setMovingInFacingDirection();
				this.skipHorizontal = 500;
			}
			return result;
		}

		private void checkHorizontalMovement(ref bool success, ref bool setMoving, ref bool scootSuccess, Farmer who, GameLocation location)
		{
			if (who.position.X > this.position.X + (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyRight();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
				}
				else
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					if (!this.position.Equals(this.lastPosition))
					{
						scootSuccess = true;
					}
				}
			}
			if (!success && who.position.X < this.position.X - (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyLeft();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
					return;
				}
				this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
				if (!this.position.Equals(this.lastPosition))
				{
					scootSuccess = true;
				}
			}
		}

		private void checkVerticalMovement(ref bool success, ref bool setMoving, ref bool scootSuccess, Farmer who, GameLocation location)
		{
			if (!success && who.position.Y < this.position.Y - (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyUp();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
				}
				else
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					if (!this.position.Equals(this.lastPosition))
					{
						scootSuccess = true;
					}
				}
			}
			if (!success && who.position.Y > this.position.Y + (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyDown();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
					return;
				}
				this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
				if (!this.position.Equals(this.lastPosition))
				{
					scootSuccess = true;
				}
			}
		}

		public override void updateMovement(GameLocation location, GameTime time)
		{
			if (base.IsWalkingTowardPlayer)
			{
				if ((this.moveTowardPlayerThreshold == -1 || this.withinPlayerThreshold()) && this.timeBeforeAIMovementAgain <= 0f && this.IsMonster && !this.isGlider && !location.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].Properties.ContainsKey("NPCBarrier"))
				{
					if (this.skipHorizontal <= 0)
					{
						Farmer nearestFarmerInCurrentLocation = Utility.getNearestFarmerInCurrentLocation(base.getTileLocation());
						if (this.lastPosition.Equals(this.position) && Game1.random.NextDouble() < 0.001)
						{
							switch (this.facingDirection)
							{
							case 0:
							case 2:
								if (Game1.random.NextDouble() < 0.5)
								{
									base.SetMovingOnlyRight();
								}
								else
								{
									base.SetMovingOnlyLeft();
								}
								break;
							case 1:
							case 3:
								if (Game1.random.NextDouble() < 0.5)
								{
									base.SetMovingOnlyUp();
								}
								else
								{
									base.SetMovingOnlyDown();
								}
								break;
							}
							this.skipHorizontal = 700;
							return;
						}
						bool flag = false;
						bool flag2 = false;
						bool flag3 = false;
						if (this.lastPosition.X == this.position.X)
						{
							this.checkHorizontalMovement(ref flag, ref flag2, ref flag3, nearestFarmerInCurrentLocation, location);
							this.checkVerticalMovement(ref flag, ref flag2, ref flag3, nearestFarmerInCurrentLocation, location);
						}
						else
						{
							this.checkVerticalMovement(ref flag, ref flag2, ref flag3, nearestFarmerInCurrentLocation, location);
							this.checkHorizontalMovement(ref flag, ref flag2, ref flag3, nearestFarmerInCurrentLocation, location);
						}
						if (!flag && !flag2)
						{
							this.Halt();
							base.faceGeneralDirection(nearestFarmerInCurrentLocation.getStandingPosition(), 0);
						}
						if (flag)
						{
							this.skipHorizontal = 500;
						}
						if (flag3)
						{
							return;
						}
					}
					else
					{
						this.skipHorizontal -= time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else
			{
				this.defaultMovementBehavior(time);
			}
			this.MovePosition(time, Game1.viewport, location);
			if (this.position.Equals(this.lastPosition) && base.IsWalkingTowardPlayer && this.withinPlayerThreshold())
			{
				this.noMovementProgressNearPlayerBehavior();
			}
		}

		public virtual void noMovementProgressNearPlayerBehavior()
		{
			this.Halt();
			base.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(base.getTileLocation()).getStandingPosition(), 0);
		}

		public virtual void defaultMovementBehavior(GameTime time)
		{
			if (Game1.random.NextDouble() < this.jitteriness * 1.8 && this.skipHorizontal <= 0)
			{
				switch (Game1.random.Next(6))
				{
				case 0:
					base.SetMovingOnlyUp();
					return;
				case 1:
					base.SetMovingOnlyRight();
					return;
				case 2:
					base.SetMovingOnlyDown();
					return;
				case 3:
					base.SetMovingOnlyLeft();
					return;
				default:
					this.Halt();
					break;
				}
			}
		}

		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			this.lastPosition = this.position;
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
				{
					this.xVelocity = 0f;
					this.yVelocity = 0f;
				}
				Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
				boundingBox.X += (int)this.xVelocity;
				boundingBox.Y -= (int)this.yVelocity;
				if (!currentLocation.isCollidingPosition(boundingBox, viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					this.position.X = this.position.X + this.xVelocity;
					this.position.Y = this.position.Y - this.yVelocity;
					if (this.slipperiness < 1000)
					{
						this.xVelocity -= this.xVelocity / (float)this.slipperiness;
						this.yVelocity -= this.yVelocity / (float)this.slipperiness;
						if (Math.Abs(this.xVelocity) <= 0.05f)
						{
							this.xVelocity = 0f;
						}
						if (Math.Abs(this.yVelocity) <= 0.05f)
						{
							this.yVelocity = 0f;
						}
					}
					if (!this.isGlider && this.invincibleCountdown > 0)
					{
						this.slideAnimationTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.slideAnimationTimer < 0 && (Math.Abs(this.xVelocity) >= 3f || Math.Abs(this.yVelocity) >= 3f))
						{
							this.slideAnimationTimer = 100 - (int)(Math.Abs(this.xVelocity) * 2f + Math.Abs(this.yVelocity) * 2f);
							currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, base.getStandingPosition() + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)), Color.White * 0.75f, 8, Game1.random.NextDouble() < 0.5, 20f, 0, -1, -1f, -1, 0)
							{
								scale = 0.75f
							});
						}
					}
				}
				else if (this.isGlider || this.slipperiness >= 8)
				{
					bool[] expr_26C = Utility.horizontalOrVerticalCollisionDirections(boundingBox, this, false);
					if (expr_26C[0])
					{
						this.xVelocity = -this.xVelocity;
						this.position.X = this.position.X + (float)Math.Sign(this.xVelocity);
						this.rotation += (float)(3.1415926535897931 + (double)Game1.random.Next(-10, 11) * 3.1415926535897931 / 500.0);
					}
					if (expr_26C[1])
					{
						this.yVelocity = -this.yVelocity;
						this.position.Y = this.position.Y - (float)Math.Sign(this.yVelocity);
						this.rotation += (float)(3.1415926535897931 + (double)Game1.random.Next(-10, 11) * 3.1415926535897931 / 500.0);
					}
					if (this.slipperiness < 1000)
					{
						this.xVelocity -= this.xVelocity / (float)this.slipperiness / 4f;
						this.yVelocity -= this.yVelocity / (float)this.slipperiness / 4f;
						if (Math.Abs(this.xVelocity) <= 0.05f)
						{
							this.xVelocity = 0f;
						}
						if (Math.Abs(this.yVelocity) <= 0.051f)
						{
							this.yVelocity = 0f;
						}
					}
				}
				else
				{
					this.xVelocity -= this.xVelocity / (float)this.slipperiness;
					this.yVelocity -= this.yVelocity / (float)this.slipperiness;
					if (Math.Abs(this.xVelocity) <= 0.05f)
					{
						this.xVelocity = 0f;
					}
					if (Math.Abs(this.yVelocity) <= 0.05f)
					{
						this.yVelocity = 0f;
					}
				}
				if (this.isGlider)
				{
					return;
				}
			}
			if (this.moveUp)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateUp(time, 0, "");
					}
					this.facingDirection = 0;
					this.faceDirection(0);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle rectangle = this.nextPosition(0);
					rectangle.Width /= 4;
					bool flag = currentLocation.isCollidingPosition(rectangle, viewport, false, this.damageToFarmer, this.isGlider, this);
					rectangle.X += rectangle.Width * 3;
					bool flag2 = currentLocation.isCollidingPosition(rectangle, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (flag && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (flag2 && !flag && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize), (float)(base.getStandingY() / Game1.tileSize - 1));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(0), true))
						{
							Game1.playSound("stoneCrack");
							this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (this.moveRight)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateRight(time, 0, "");
					}
					this.facingDirection = 1;
					this.faceDirection(1);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle rectangle2 = this.nextPosition(1);
					rectangle2.Height /= 4;
					bool flag3 = currentLocation.isCollidingPosition(rectangle2, viewport, false, this.damageToFarmer, this.isGlider, this);
					rectangle2.Y += rectangle2.Height * 3;
					bool flag4 = currentLocation.isCollidingPosition(rectangle2, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (flag3 && !flag4 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (flag4 && !flag3 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize + 1), (float)(base.getStandingY() / Game1.tileSize));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(1), true))
						{
							Game1.playSound("stoneCrack");
							this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (this.moveDown)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateDown(time, 0, "");
					}
					this.facingDirection = 2;
					this.faceDirection(2);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle rectangle3 = this.nextPosition(2);
					rectangle3.Width /= 4;
					bool flag5 = currentLocation.isCollidingPosition(rectangle3, viewport, false, this.damageToFarmer, this.isGlider, this);
					rectangle3.X += rectangle3.Width * 3;
					bool flag6 = currentLocation.isCollidingPosition(rectangle3, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (flag5 && !flag6 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (flag6 && !flag5 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize), (float)(base.getStandingY() / Game1.tileSize + 1));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(2), true))
						{
							Game1.playSound("stoneCrack");
							this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (this.moveLeft)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					this.facingDirection = 3;
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateLeft(time, 0, "");
					}
					this.faceDirection(3);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle rectangle4 = this.nextPosition(3);
					rectangle4.Height /= 4;
					bool flag7 = currentLocation.isCollidingPosition(rectangle4, viewport, false, this.damageToFarmer, this.isGlider, this);
					rectangle4.Y += rectangle4.Height * 3;
					bool flag8 = currentLocation.isCollidingPosition(rectangle4, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (flag7 && !flag8 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (flag8 && !flag7 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize - 1), (float)(base.getStandingY() / Game1.tileSize));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(3), true))
						{
							Game1.playSound("stoneCrack");
							this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (!this.ignoreMovementAnimations)
			{
				if (this.moveUp)
				{
					this.sprite.AnimateUp(time, 0, "");
				}
				else if (this.moveRight)
				{
					this.sprite.AnimateRight(time, 0, "");
				}
				else if (this.moveDown)
				{
					this.sprite.AnimateDown(time, 0, "");
				}
				else if (this.moveLeft)
				{
					this.sprite.AnimateLeft(time, 0, "");
				}
			}
			if (!this.ignoreMovementAnimations)
			{
				this.sprite.interval = (float)this.defaultAnimationInterval - (float)(this.speed + this.addedSpeed - 2) * 20f;
			}
			if ((this.blockedInterval < 3000 || (float)this.blockedInterval > 3750f) && this.blockedInterval >= 5000)
			{
				this.speed = 4;
				this.isCharging = true;
				this.blockedInterval = 0;
			}
			if (this.damageToFarmer > 0 && Game1.random.NextDouble() < 0.00033333333333333332)
			{
				if (this.name.Equals("Shadow Guy") && Game1.random.NextDouble() < 0.3)
				{
					if (Game1.random.NextDouble() < 0.5)
					{
						Game1.playSound("grunt");
						return;
					}
					Game1.playSound("shadowpeep");
					return;
				}
				else if (!this.name.Equals("Shadow Girl"))
				{
					if (this.name.Equals("Ghost"))
					{
						Game1.playSound("ghost");
						return;
					}
					if (!this.name.Contains("Slime"))
					{
						this.name.Contains("Jelly");
					}
				}
			}
		}
	}
}
