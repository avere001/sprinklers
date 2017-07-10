using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Train
	{
		public const int minCars = 8;

		public const int maxCars = 24;

		public const double chanceForLongTrain = 0.1;

		public const int randomTrain = 0;

		public const int jojaTrain = 1;

		public const int coalTrain = 2;

		public const int passengerTrain = 3;

		public const int uniformColorPlainTrain = 4;

		public const int prisonTrain = 5;

		public const int christmasTrain = 6;

		public List<TrainCar> cars = new List<TrainCar>();

		public int type;

		public float position;

		public float speed;

		public float wheelRotation;

		public float smokeTimer;

		private TemporaryAnimatedSprite whistleSteam;

		public Train()
		{
			Random random = new Random();
			if (random.NextDouble() < 0.1)
			{
				this.type = 3;
			}
			else if (random.NextDouble() < 0.1)
			{
				this.type = 1;
			}
			else if (random.NextDouble() < 0.1)
			{
				this.type = 2;
			}
			else if (random.NextDouble() < 0.05)
			{
				this.type = 5;
			}
			else if (Game1.currentSeason.ToLower().Equals("winter") && random.NextDouble() < 0.2)
			{
				this.type = 6;
			}
			else
			{
				this.type = 0;
			}
			int num = random.Next(8, 25);
			if (random.NextDouble() < 0.1)
			{
				num *= 2;
			}
			this.speed = 0.2f;
			this.smokeTimer = this.speed * 2000f;
			Color color = Color.White;
			double num2 = 1.0;
			double num3 = 1.0;
			switch (this.type)
			{
			case 0:
				num2 = 0.2;
				num3 = 0.2;
				break;
			case 1:
				num2 = 0.0;
				num3 = 0.0;
				color = Color.DimGray;
				break;
			case 2:
				num2 = 0.0;
				num3 = 0.7;
				break;
			case 3:
				num2 = 1.0;
				num3 = 0.0;
				this.speed = 0.4f;
				break;
			case 5:
				num3 = 0.0;
				num2 = 0.0;
				color = Color.MediumBlue;
				this.speed = 0.4f;
				break;
			case 6:
				num2 = 0.0;
				num3 = 1.0;
				color = Color.Red;
				break;
			}
			this.cars.Add(new TrainCar(random, 3, -1, Color.White, 0, 0));
			for (int i = 1; i < num; i++)
			{
				int num4 = 0;
				if (random.NextDouble() < num2)
				{
					num4 = 2;
				}
				else if (random.NextDouble() < num3)
				{
					num4 = 1;
				}
				Color color2 = color;
				if (color.Equals(Color.White))
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					switch (random.Next(3))
					{
					case 0:
						flag = true;
						break;
					case 1:
						flag2 = true;
						break;
					case 2:
						flag3 = true;
						break;
					}
					color2 = new Color(random.Next(flag ? 0 : 100, 250), random.Next(flag2 ? 0 : 100, 250), random.Next(flag3 ? 0 : 100, 250));
				}
				int frontDecal = -1;
				if (this.type == 1)
				{
					frontDecal = 2;
				}
				else if (this.type == 5)
				{
					frontDecal = 1;
				}
				else if (this.type == 6)
				{
					frontDecal = -1;
				}
				else if (random.NextDouble() < 0.3)
				{
					frontDecal = random.Next(35);
				}
				int resourceType = 0;
				if (num4 == 1)
				{
					resourceType = random.Next(9);
					if (this.type == 6)
					{
						resourceType = 9;
					}
				}
				this.cars.Add(new TrainCar(random, num4, frontDecal, color2, resourceType, random.Next(1, 3)));
			}
		}

		public Rectangle getBoundingBox()
		{
			return new Rectangle(-this.cars.Count * 128 * 4 + (int)this.position, 45 * Game1.tileSize - Game1.tileSize * 2 - 32, this.cars.Count * 128 * 4, Game1.tileSize * 2);
		}

		public bool Update(GameTime time, GameLocation location)
		{
			this.position += (float)time.ElapsedGameTime.Milliseconds * this.speed;
			this.wheelRotation += (float)time.ElapsedGameTime.Milliseconds * 0.0122718466f;
			this.wheelRotation %= 6.28318548f;
			foreach (Farmer current in location.getFarmers())
			{
				if (current.GetBoundingBox().Intersects(this.getBoundingBox()))
				{
					current.xVelocity = 8f;
					current.yVelocity = (float)(this.getBoundingBox().Center.Y - current.GetBoundingBox().Center.Y) / 4f;
					Game1.farmerTakeDamage(20, true, null);
					if (current.usingTool)
					{
						Game1.playSound("clank");
					}
				}
			}
			if (Game1.random.NextDouble() < 0.001 && location.Equals(Game1.currentLocation))
			{
				Game1.playSound("trainWhistle");
				this.whistleSteam = new TemporaryAnimatedSprite(27, new Vector2(this.position - 250f, (float)(45 * Game1.tileSize - Game1.tileSize * 4)), Color.White, 8, false, 100f, 0, Game1.tileSize, 1f, Game1.tileSize, 0);
			}
			if (this.whistleSteam != null)
			{
				this.whistleSteam.Position = new Vector2(this.position - 258f, (float)(45 * Game1.tileSize - Game1.tileSize * 4 - 32));
				if (this.whistleSteam.update(time))
				{
					this.whistleSteam = null;
				}
			}
			this.smokeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			if (this.smokeTimer <= 0f)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2(this.position - 170f, (float)(45 * Game1.tileSize - Game1.tileSize * 6)), Color.White, 8, false, 100f, 0, Game1.tileSize, 1f, Game1.tileSize * 2, 0));
				this.smokeTimer = this.speed * 2000f;
			}
			return this.position > (float)(this.cars.Count * 128 * 4 + 70 * Game1.tileSize);
		}

		public void draw(SpriteBatch b)
		{
			for (int i = 0; i < this.cars.Count; i++)
			{
				this.cars[i].draw(b, new Vector2(this.position - (float)((i + 1) * 512), (float)(45 * Game1.tileSize - Game1.tileSize * 4 - 32)), this.wheelRotation);
			}
			if (this.whistleSteam != null)
			{
				this.whistleSteam.draw(b, false, 0, 0);
			}
		}
	}
}
