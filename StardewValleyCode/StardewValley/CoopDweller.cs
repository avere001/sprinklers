using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class CoopDweller : Character
	{
		public const double chancePerUpdateToChangeDirection = 0.007;

		public new const double chanceForSound = 0.002;

		public const int uniqueDownFrame = 16;

		public const int uniqueRightFrame = 18;

		public const int uniqueUpFrame = 20;

		public const int uniqueLeftFrame = 22;

		public const int pushAccumulatorTimeTillPush = 40;

		public const int timePerUniqueFrame = 500;

		public int daysToLay;

		public int daysSinceLastLay;

		public int defaultProduceIndex;

		public int friendshipTowardFarmer;

		public int daysSinceLastFed;

		public int pushAccumulator;

		public int uniqueFrameAccumulator = -1;

		public int age;

		public int ageWhenMature;

		public bool wasFed;

		public bool wasPet;

		public string sound;

		public string type;

		public CoopDweller()
		{
		}

		public CoopDweller(string type, string name) : base(null, new Vector2((float)(Game1.tileSize * Game1.random.Next(2, 9)), (float)(Game1.tileSize * Game1.random.Next(5, 9))), 2, name)
		{
			this.type = type;
			if (type == "WhiteChicken")
			{
				this.daysToLay = 1;
				this.ageWhenMature = 1;
				this.defaultProduceIndex = 176;
				this.sound = "cluck";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyWhiteChicken"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (type == "BrownChicken")
			{
				this.daysToLay = 1;
				this.ageWhenMature = 1;
				this.defaultProduceIndex = 180;
				this.sound = "cluck";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyBrownChicken"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (type == "Duck")
			{
				this.daysToLay = 2;
				this.ageWhenMature = 1;
				this.defaultProduceIndex = 442;
				this.sound = "cluck";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyBrownChicken"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (type == "Rabbit")
			{
				this.daysToLay = 4;
				this.ageWhenMature = 3;
				this.defaultProduceIndex = 440;
				this.sound = "rabbit";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyRabbit"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (!(type == "Dinosaur"))
			{
				return;
			}
			this.daysToLay = 7;
			this.ageWhenMature = 0;
			this.defaultProduceIndex = 107;
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\Dinosaur"), 0, Game1.tileSize, Game1.tileSize);
		}

		public void reload()
		{
			string str = this.type;
			if (this.age < this.ageWhenMature)
			{
				str = "Baby" + this.type;
			}
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\" + str), 0, Game1.tileSize, Game1.tileSize);
		}

		public void pet()
		{
			if (Game1.timeOfDay >= 1900)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:CoopDweller.cs.550", new object[]
				{
					base.displayName
				}));
				return;
			}
			this.Halt();
			this.sprite.StopAnimation();
			this.uniqueFrameAccumulator = -1;
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
			case 3:
				this.sprite.currentFrame = 4;
				break;
			}
			if (!this.wasPet)
			{
				this.wasPet = true;
				this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 10);
				base.doEmote(20, true);
				Game1.playSound(this.sound);
				return;
			}
			if (this.daysSinceLastFed == 0)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:CoopDweller.cs.551", new object[]
				{
					base.displayName
				}));
				return;
			}
			Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:CoopDweller.cs.552", new object[]
			{
				base.displayName
			}));
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
					this.SetMovingLeft(true);
					break;
				}
				this.pushAccumulator = 0;
			}
		}

		public void setRandomPosition()
		{
			GameLocation locationFromName = Game1.getLocationFromName("Coop");
			string[] expr_26 = locationFromName.getMapProperty("ProduceArea").Split(new char[]
			{
				' '
			});
			int num = Convert.ToInt32(expr_26[0]);
			int num2 = Convert.ToInt32(expr_26[1]);
			int num3 = Convert.ToInt32(expr_26[2]);
			int num4 = Convert.ToInt32(expr_26[3]);
			this.position = new Vector2((float)Game1.random.Next(num, num + num3), (float)Game1.random.Next(num2, num2 + num4));
			int num5 = 0;
			while (locationFromName.Objects.ContainsKey(this.position))
			{
				this.position = new Vector2((float)Game1.random.Next(num, num + num3), (float)Game1.random.Next(num2, num2 + num4));
				num5++;
				if (num5 > 2)
				{
					break;
				}
			}
			this.position.X = this.position.X * (float)Game1.tileSize;
			this.position.Y = this.position.Y * (float)Game1.tileSize;
		}

		public int dayUpdate()
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
			}
			this.wasPet = false;
			int num;
			if (!this.wasFed || this.age < this.ageWhenMature || this.daysSinceLastFed > 0 || this.daysSinceLastLay < this.daysToLay)
			{
				num = -1;
			}
			else
			{
				num = this.defaultProduceIndex;
				if (this.type.Equals("Duck") && Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.01)
				{
					num = 444;
				}
				else if (this.type.Equals("Rabbit") && Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.02)
				{
					num = 446;
				}
				this.daysSinceLastLay = 0;
				if (num <= 180)
				{
					if (num != 176)
					{
						if (num == 180)
						{
							Stats expr_1AD = Game1.stats;
							uint num2 = expr_1AD.ChickenEggsLayed;
							expr_1AD.ChickenEggsLayed = num2 + 1u;
						}
					}
					else
					{
						Stats expr_197 = Game1.stats;
						uint num2 = expr_197.ChickenEggsLayed;
						expr_197.ChickenEggsLayed = num2 + 1u;
					}
				}
				else if (num != 440)
				{
					if (num == 442)
					{
						Stats expr_1C3 = Game1.stats;
						uint num2 = expr_1C3.DuckEggsLayed;
						expr_1C3.DuckEggsLayed = num2 + 1u;
					}
				}
				else
				{
					Stats expr_1D9 = Game1.stats;
					uint num2 = expr_1D9.RabbitWoolProduced;
					expr_1D9.RabbitWoolProduced = num2 + 1u;
				}
				if (Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 1200.0)
				{
					if (num != 176)
					{
						if (num == 180)
						{
							num += 2;
						}
					}
					else
					{
						num -= 2;
					}
				}
			}
			if (!this.wasFed)
			{
				this.daysSinceLastFed++;
			}
			else
			{
				this.daysSinceLastFed = Math.Max(0, this.daysSinceLastFed - 1);
			}
			this.wasFed = false;
			return num;
		}

		public new void update(GameTime time, GameLocation location)
		{
			if (this.isEmoting)
			{
				base.updateEmote(time);
			}
			if (Game1.timeOfDay >= 1900)
			{
				this.sprite.currentFrame = 16;
				this.sprite.UpdateSourceRect();
				if (!this.isEmoting && Game1.random.NextDouble() < 0.002)
				{
					base.doEmote(24, true);
					return;
				}
			}
			else
			{
				if (Game1.random.NextDouble() < 0.002 && this.age >= this.ageWhenMature && this.sound != null)
				{
					Game1.playSound(this.sound);
				}
				if (Game1.random.NextDouble() < 0.007 && this.uniqueFrameAccumulator == -1)
				{
					int num = Game1.random.Next(5);
					if (num != (this.facingDirection + 2) % 4)
					{
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
				if (this.isMoving() && Game1.random.NextDouble() < 0.014 && this.uniqueFrameAccumulator == -1)
				{
					this.Halt();
					this.sprite.StopAnimation();
					if (Game1.random.NextDouble() < 0.75)
					{
						this.uniqueFrameAccumulator = 0;
						switch (this.facingDirection)
						{
						case 0:
							this.sprite.currentFrame = 20;
							break;
						case 1:
							this.sprite.currentFrame = 18;
							break;
						case 2:
							this.sprite.currentFrame = 16;
							break;
						case 3:
							this.sprite.currentFrame = 22;
							break;
						}
					}
				}
				if (this.uniqueFrameAccumulator != -1)
				{
					this.uniqueFrameAccumulator += time.ElapsedGameTime.Milliseconds;
					if (this.uniqueFrameAccumulator > 500)
					{
						this.sprite.CurrentFrame = this.sprite.CurrentFrame + 1 - this.sprite.CurrentFrame % 2 * 2;
						this.uniqueFrameAccumulator = 0;
						if (Game1.random.NextDouble() < 0.4)
						{
							this.uniqueFrameAccumulator = -1;
							return;
						}
					}
				}
				else
				{
					if (this.moveUp)
					{
						if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, false))
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
						return;
					}
					if (this.moveRight)
					{
						if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false))
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
						return;
					}
					if (this.moveDown)
					{
						if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, false))
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
						return;
					}
					if (this.moveLeft)
					{
						if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false))
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
				}
			}
		}
	}
}
