using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.BellsAndWhistles
{
	public class TrainCar
	{
		public const int spotsForTopFeatures = 6;

		public const double chanceForTopFeature = 0.2;

		public const int engine = 3;

		public const int passengerCar = 2;

		public const int coalCar = 1;

		public const int plainCar = 0;

		public const int coal = 0;

		public const int metal = 1;

		public const int wood = 2;

		public const int compartments = 3;

		public const int grass = 4;

		public const int hay = 5;

		public const int bricks = 6;

		public const int rocks = 7;

		public const int packages = 8;

		public const int presents = 9;

		public int frontDecal;

		public int carType;

		public int resourceType;

		public int loaded;

		public int[] topFeatures = new int[6];

		public bool alternateCar;

		public Color color;

		public TrainCar(Random random, int carType, int frontDecal, Color color, int resourceType = 0, int loaded = 0)
		{
			this.carType = carType;
			this.frontDecal = frontDecal;
			this.color = color;
			this.resourceType = resourceType;
			this.loaded = loaded;
			if (carType != 0 && carType != 1)
			{
				this.color = Color.White;
			}
			if (carType == 0 && !color.Equals(Color.DimGray))
			{
				for (int i = 0; i < this.topFeatures.Length; i++)
				{
					if (random.NextDouble() < 0.2)
					{
						this.topFeatures[i] = random.Next(2);
					}
					else
					{
						this.topFeatures[i] = -1;
					}
				}
			}
			if (carType == 2 && random.NextDouble() < 0.5)
			{
				this.alternateCar = true;
			}
		}

		public void draw(SpriteBatch b, Vector2 globalPosition, float wheelRotation)
		{
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(192 + this.carType * 128, 512 - (this.alternateCar ? 64 : 0), 128, 57)), this.color, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 256f) / 10000f);
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(0f, 228f)), new Rectangle?(new Rectangle(192 + this.carType * 128, 569, 128, 7)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 256f) / 10000f);
			if (this.carType == 1)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(448 + this.resourceType * 128 % 256, 576 + this.resourceType / 2 * 32, 128, 32)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				if (this.loaded > 0 && Game1.random.NextDouble() < 0.003 && globalPosition.X > (float)(Game1.tileSize * 4) && globalPosition.X < (float)(Game1.currentLocation.map.DisplayWidth - Game1.tileSize * 4))
				{
					this.loaded--;
					int num = -1;
					switch (this.resourceType)
					{
					case 0:
						num = 382;
						break;
					case 1:
						num = ((this.color.R > this.color.G) ? 378 : ((this.color.G > this.color.B) ? 380 : ((this.color.B > this.color.R) ? 384 : 378)));
						break;
					case 2:
						num = 388;
						break;
					case 6:
						num = 390;
						break;
					case 7:
						num = (Game1.currentSeason.Equals("winter") ? 536 : ((Game1.stats.DaysPlayed > 120u && this.color.R > this.color.G) ? 537 : 535));
						break;
					}
					if (num != -1)
					{
						Game1.createObjectDebris(num, (int)globalPosition.X / Game1.tileSize, (int)globalPosition.Y / Game1.tileSize, (int)(globalPosition.Y + (float)(Game1.tileSize * 5)), 0, 1f, null);
					}
				}
			}
			if (this.carType == 0)
			{
				for (int i = 0; i < this.topFeatures.Length; i += Game1.tileSize)
				{
					if (this.topFeatures[i] != -1)
					{
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2((float)(Game1.tileSize + i), 20f)), new Rectangle?(new Rectangle(192, 608 + this.topFeatures[i] * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
					}
				}
			}
			if (this.frontDecal != -1 && (this.carType == 0 || this.carType == 1))
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(192f, 92f)), new Rectangle?(new Rectangle(224 + this.frontDecal * 32 % 224, 576 + this.frontDecal * 32 / 224 * 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
			}
			if (this.carType == 3)
			{
				Vector2 vector = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(72f, 208f));
				Vector2 vector2 = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(316f, 208f));
				b.Draw(Game1.mouseCursors, vector, new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(228f, 208f)), new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				b.Draw(Game1.mouseCursors, vector2, new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				int num2 = (int)((double)(vector.X + 4f) + 24.0 * Math.Cos((double)wheelRotation));
				int num3 = (int)((double)(vector.Y + 4f) + 24.0 * Math.Sin((double)wheelRotation));
				int num4 = (int)((double)(vector2.X + 4f) + 24.0 * Math.Cos((double)wheelRotation));
				int num5 = (int)((double)(vector2.Y + 4f) + 24.0 * Math.Sin((double)wheelRotation));
				Utility.drawLineWithScreenCoordinates(num2, num3, num4, num5, b, new Color(112, 98, 92), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(num2, num3 + 2, num4, num5 + 2, b, new Color(112, 98, 92), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(num2, num3 + 4, num4, num5 + 4, b, new Color(53, 46, 43), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(num2, num3 + 6, num4, num5 + 6, b, new Color(53, 46, 43), (globalPosition.Y + 264f) / 10000f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(num2 - 8), (float)(num3 - 8)), new Rectangle?(new Rectangle(192, 640, 24, 24)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 268f) / 10000f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(num4 - 8), (float)(num5 - 8)), new Rectangle?(new Rectangle(192, 640, 24, 24)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 268f) / 10000f);
			}
		}
	}
}
