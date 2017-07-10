using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class BarnDweller : Character
	{
		public const double chancePerUpdateToChangeDirection = 0.007;

		public new const double chanceForSound = 0.002;

		public const int pushAccumulatorTimeTillPush = 40;

		public int daysToLay;

		public int daysSinceLastLay;

		public int defaultProduceIndex;

		public int friendshipTowardFarmer;

		public int daysSinceLastFed;

		public int pushAccumulator;

		public int age;

		public int ageWhenMature;

		public bool hasProduce;

		public bool wasPet;

		public bool wasFed;

		public string sound;

		public string type;

		public BarnDweller()
		{
		}

		public BarnDweller(string type, string name) : base(new AnimatedSprite(null, 0, Game1.tileSize, Game1.tileSize), new Vector2((float)(Game1.tileSize * Game1.random.Next(6, 16)), (float)(Game1.tileSize * Game1.random.Next(6, 13))), 2, name)
		{
			this.type = type;
			if (type == "WhiteBlackCow")
			{
				this.defaultProduceIndex = 184;
				this.sound = "cow";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyWhiteBlackCow"), 0);
				this.ageWhenMature = 1;
				this.daysToLay = 1;
				return;
			}
			if (type == "Pig")
			{
				this.defaultProduceIndex = 430;
				this.sound = "pig";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyPig"), 0);
				this.ageWhenMature = 1;
				this.daysToLay = 1;
				return;
			}
			if (type == "Goat")
			{
				this.defaultProduceIndex = 436;
				this.sound = "goat";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyGoat"), 0);
				this.ageWhenMature = 1;
				this.daysToLay = 2;
				return;
			}
			if (!(type == "Sheep"))
			{
				return;
			}
			this.defaultProduceIndex = 440;
			this.sound = "goat";
			this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabySheep"), 0);
			this.ageWhenMature = 1;
			this.daysToLay = 3;
		}

		public BarnDweller(string type, int tileX, int tileY) : base(new AnimatedSprite(null, 0, Game1.tileSize, Game1.tileSize), new Vector2((float)(Game1.tileSize * tileX), (float)(Game1.tileSize * tileY)), 2, "Missingno")
		{
			if (type == "Cow")
			{
				this.sound = "cow";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\Cow"), 0);
			}
		}

		public void reload()
		{
			string str = this.type;
			if (this.age < this.ageWhenMature)
			{
				str = "Baby" + this.type;
			}
			this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\" + str), 0);
		}

		public void farmerPushing()
		{
			this.pushAccumulator++;
			if (this.pushAccumulator > 40)
			{
				switch (Game1.player.facingDirection)
				{
				case 0:
					this.Halt();
					this.SetMovingUp(true);
					break;
				case 1:
					this.Halt();
					this.SetMovingRight(true);
					break;
				case 2:
					this.Halt();
					this.SetMovingDown(true);
					break;
				case 3:
					this.Halt();
					if (!Game1.currentLocation.isCollidingPosition(new Rectangle((int)this.position.X - Game1.tileSize, (int)this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, Game1.tileSize * 2 - Game1.tileSize / 4, Game1.tileSize / 2), Game1.viewport, this))
					{
						this.SetMovingLeft(true);
						if (this.facingDirection != 3)
						{
							this.position.X = this.position.X - (float)Game1.tileSize;
						}
					}
					else
					{
						this.SetMovingUp(true);
						this.faceDirection(0);
					}
					break;
				}
				this.faceDirection(Game1.player.facingDirection);
				this.sprite.UpdateSourceRect();
				this.pushAccumulator = 0;
			}
		}

		public override Rectangle GetBoundingBox()
		{
			int width = (this.facingDirection == 3 || this.facingDirection == 1) ? (Game1.tileSize * 2 - Game1.tileSize / 4) : (Game1.tileSize * 3 / 4);
			return new Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, width, Game1.tileSize / 2);
		}

		public void dayUpdate()
		{
			this.age++;
			this.daysSinceLastLay++;
			if (this.age == this.ageWhenMature)
			{
				this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\" + this.type);
			}
			if (!this.wasPet)
			{
				this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - (10 - this.friendshipTowardFarmer / 200));
				if (this.friendshipTowardFarmer <= 990)
				{
					this.daysToLay = 3;
				}
			}
			if (this.wasFed)
			{
				int arg_A8_0 = 0;
				int num = this.daysSinceLastFed;
				this.daysSinceLastFed = num - 1;
				this.daysSinceLastFed = Math.Max(arg_A8_0, num);
			}
			else
			{
				this.daysSinceLastFed++;
				this.hasProduce = false;
			}
			if (this.daysSinceLastFed <= 0 && this.daysSinceLastLay >= this.daysToLay && this.age >= this.ageWhenMature)
			{
				this.hasProduce = true;
				if (this.type.Equals("Pig") && Game1.random.NextDouble() < 0.75 - (double)Game1.player.LuckLevel * 0.015 - Game1.dailyLuck - (double)this.friendshipTowardFarmer / 10000.0)
				{
					this.hasProduce = false;
				}
				else if (this.type.Equals("Sheep"))
				{
					this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheep");
				}
			}
			this.wasFed = false;
			this.wasPet = false;
		}

		public Object getProduce()
		{
			if (this.hasProduce)
			{
				this.hasProduce = false;
				this.daysSinceLastLay = 0;
				if (this.type.Equals("Sheep"))
				{
					this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\ShearedSheep");
				}
				int num = this.defaultProduceIndex;
				if (num <= 430)
				{
					if (num != 184)
					{
						if (num == 430)
						{
							Stats expr_93 = Game1.stats;
							uint num2 = expr_93.TrufflesFound;
							expr_93.TrufflesFound = num2 + 1u;
						}
					}
					else
					{
						Stats expr_7D = Game1.stats;
						uint num2 = expr_7D.CowMilkProduced;
						expr_7D.CowMilkProduced = num2 + 1u;
					}
				}
				else if (num != 436)
				{
					if (num == 440)
					{
						Stats expr_BF = Game1.stats;
						uint num2 = expr_BF.SheepWoolProduced;
						expr_BF.SheepWoolProduced = num2 + 1u;
					}
				}
				else
				{
					Stats expr_A9 = Game1.stats;
					uint num2 = expr_A9.GoatMilkProduced;
					expr_A9.GoatMilkProduced = num2 + 1u;
				}
				int num3 = this.defaultProduceIndex;
				if (Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 10.0 && (num3 == 184 || num3 == 436))
				{
					num3 += 2;
				}
				return new Object(Vector2.Zero, num3, null, false, false, false, false);
			}
			return null;
		}

		public void pet()
		{
			if (Game1.timeOfDay >= 1900)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:BarnDweller.cs.386", new object[]
				{
					base.displayName
				}));
				return;
			}
			if (!this.hasProduce)
			{
				this.Halt();
				this.sprite.StopAnimation();
				switch (Game1.player.FacingDirection)
				{
				case 0:
					this.sprite.currentFrame = 0;
					break;
				case 1:
					this.sprite.currentFrame = 12;
					break;
				case 2:
					this.sprite.currentFrame = 8;
					break;
				}
				this.sprite.UpdateSourceRect();
				if (!this.wasPet)
				{
					this.wasPet = true;
					this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 10);
					base.doEmote(20, true);
					return;
				}
				if (this.daysSinceLastFed == 0)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:BarnDweller.cs.387", new object[]
					{
						base.displayName
					}));
					return;
				}
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:BarnDweller.cs.388", new object[]
				{
					base.displayName
				}));
			}
		}

		public void setRandomPosition()
		{
			string[] expr_24 = Game1.getLocationFromName("Barn").getMapProperty("ProduceArea").Split(new char[]
			{
				' '
			});
			int num = Convert.ToInt32(expr_24[0]);
			int num2 = Convert.ToInt32(expr_24[1]);
			int num3 = Convert.ToInt32(expr_24[2]);
			int num4 = Convert.ToInt32(expr_24[3]);
			this.position = new Vector2((float)(Game1.random.Next(num, num + num3) * Game1.tileSize), (float)(Game1.random.Next(num2, num2 + num4) * Game1.tileSize));
			int num5 = 0;
			while (this.doesIntersectAnotherBarnDweller())
			{
				this.faceDirection(Game1.random.Next(4));
				this.position = new Vector2((float)(Game1.random.Next(num, num + num3) * Game1.tileSize), (float)(Game1.random.Next(num2, num2 + num4) * Game1.tileSize));
				num5++;
				if (num5 > 5)
				{
					break;
				}
			}
		}

		public bool doesIntersectAnotherBarnDweller()
		{
			for (int i = 0; i < Game1.player.barnDwellers.Count; i++)
			{
				if (!Game1.player.barnDwellers[i].name.Equals(this.name) && this.GetBoundingBox().Intersects(Game1.player.barnDwellers[i].GetBoundingBox()))
				{
					return true;
				}
			}
			return false;
		}

		public new void update(GameTime time, GameLocation location)
		{
			if (this.isEmoting)
			{
				base.updateEmote(time);
			}
			if (Game1.timeOfDay >= 1900)
			{
				this.facingDirection = 2;
				this.sprite.SourceRect = new Rectangle(Game1.tileSize * 4, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
				if (!this.isEmoting && Game1.random.NextDouble() < 0.002)
				{
					base.doEmote(24, true);
					return;
				}
			}
			else
			{
				if (Game1.random.NextDouble() < 0.002 && this.age >= this.ageWhenMature && !Game1.eventUp && (!Game1.currentLocation.name.Equals("Forest") || Game1.random.NextDouble() < 0.001))
				{
					Game1.playSound(this.sound);
				}
				if (Game1.random.NextDouble() < 0.007)
				{
					int num = Game1.random.Next(5);
					if (num != (this.facingDirection + 2) % 4)
					{
						if (num < 4)
						{
							int facingDirection = this.facingDirection;
							this.faceDirection(num);
							if (location.isCollidingPosition(this.nextPosition(num), Game1.viewport, this))
							{
								this.faceDirection(facingDirection);
								return;
							}
						}
						switch (num)
						{
						case 0:
							this.SetMovingUp(true);
							break;
						case 1:
							this.SetMovingRight(true);
							break;
						case 2:
							this.SetMovingDown(true);
							break;
						case 3:
							this.SetMovingLeft(true);
							break;
						default:
							this.Halt();
							this.sprite.StopAnimation();
							break;
						}
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
					}
				}
				if (this.isMoving() && Game1.random.NextDouble() < 0.014)
				{
					this.Halt();
					this.sprite.StopAnimation();
				}
				if (this.moveUp)
				{
					if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, this))
					{
						this.position.Y = this.position.Y - (float)this.speed;
						this.sprite.AnimateUp(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingDown(true);
						}
					}
					this.facingDirection = 0;
				}
				else if (this.moveRight)
				{
					if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, this))
					{
						this.position.X = this.position.X + (float)this.speed;
						this.sprite.AnimateRight(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingLeft(true);
						}
					}
					this.facingDirection = 1;
				}
				else if (this.moveDown)
				{
					if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, this))
					{
						this.position.Y = this.position.Y + (float)this.speed;
						this.sprite.AnimateDown(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingUp(true);
						}
					}
					this.facingDirection = 2;
				}
				else if (this.moveLeft)
				{
					if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, this))
					{
						this.position.X = this.position.X - (float)this.speed;
						this.sprite.AnimateLeft(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingRight(true);
						}
					}
					this.facingDirection = 3;
				}
				this.sprite.UpdateSourceRect();
			}
		}
	}
}
