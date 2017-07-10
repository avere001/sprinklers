using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class Fish
	{
		public const int widthOfTrack = 1020;

		public const int msPerFrame = 65;

		public const int fishingFieldWidth = 1028;

		public const int fishingFieldHeight = 612;

		public int whichFish;

		public int indexOfAnimation;

		public int animationTimer = 65;

		public float chanceToDart;

		public float dartingRandomness;

		public float dartingIntensity;

		public float dartingDuration;

		public float dartingTimer;

		public float dartingExtraSpeed;

		public float turnFrequency;

		public float turnSpeed;

		public float turnIntensity;

		public float minSpeed;

		public float maxSpeed;

		public float speedChangeFrequency;

		public float currentSpeed;

		public float targetSpeed;

		public float positionOnTrack = 510f;

		public Vector2 position;

		public float rotation;

		public float targetRotation;

		public bool isDarting;

		public Rectangle fishingField;

		private string fishName;

		public int bobberDifficulty;

		public Fish(int whichFish)
		{
			this.whichFish = whichFish;
			this.fishingField = new Rectangle(0, 0, 1028, 612);
			Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
			if (dictionary.ContainsKey(whichFish))
			{
				string[] array = dictionary[whichFish].Split(new char[]
				{
					'/'
				});
				this.fishName = array[0];
				this.chanceToDart = (float)Convert.ToInt32(array[1]);
				this.dartingRandomness = (float)Convert.ToInt32(array[2]);
				this.dartingIntensity = (float)Convert.ToInt32(array[3]);
				this.dartingDuration = (float)Convert.ToInt32(array[4]);
				this.turnFrequency = (float)Convert.ToInt32(array[5]);
				this.turnSpeed = (float)Convert.ToInt32(array[6]);
				this.turnIntensity = (float)Convert.ToInt32(array[7]);
				this.minSpeed = (float)Convert.ToInt32(array[8]);
				this.maxSpeed = (float)Convert.ToInt32(array[9]);
				this.speedChangeFrequency = (float)Convert.ToInt32(array[10]);
				this.bobberDifficulty = Convert.ToInt32(array[11]);
			}
			this.position = new Vector2(514f, 306f);
			this.targetSpeed = this.minSpeed / 50f;
		}

		public bool isWithinRectangle(Rectangle r, int xPositionOfFishingField, int yPositionOfFishingField)
		{
			return r.Contains((int)this.position.X + xPositionOfFishingField, (int)this.position.Y + yPositionOfFishingField);
		}

		public void Update(GameTime time)
		{
			this.animationTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.animationTimer <= 0)
			{
				this.animationTimer = 65 - (int)(this.currentSpeed * 10f);
				this.indexOfAnimation = (this.indexOfAnimation + 1) % 8;
			}
			if (!this.isDarting && Game1.random.NextDouble() < (double)(this.chanceToDart / 10000f))
			{
				this.rotation += (float)((double)Game1.random.Next(-(int)this.dartingRandomness, (int)this.dartingRandomness) * 3.1415926535897931 / 100.0);
				this.targetSpeed = this.rotation;
				this.dartingExtraSpeed = this.dartingIntensity / 20f;
				this.dartingExtraSpeed *= 1f + (float)Game1.random.Next(-10, 10) / 100f;
				this.dartingTimer = this.dartingDuration * 10f + (float)Game1.random.Next(-(int)this.dartingDuration, (int)this.dartingDuration) * 0.1f;
				this.isDarting = true;
			}
			if (this.dartingTimer > 0f)
			{
				this.dartingTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.dartingTimer <= 0f && this.isDarting)
				{
					this.isDarting = false;
					this.dartingTimer = this.dartingDuration * 10f + (float)Game1.random.Next(-(int)this.dartingDuration, (int)this.dartingDuration) * 0.1f;
				}
				if (!this.isDarting)
				{
					this.dartingExtraSpeed -= this.dartingExtraSpeed * 0.0005f * (float)time.ElapsedGameTime.Milliseconds;
				}
			}
			if (Game1.random.NextDouble() < (double)(this.turnFrequency / 10000f))
			{
				this.targetRotation = (float)((double)((float)Game1.random.Next((int)(-(int)this.turnIntensity), (int)this.turnIntensity) / 100f) * 3.1415926535897931);
			}
			if (Game1.random.NextDouble() < (double)(this.speedChangeFrequency / 10000f))
			{
				this.targetSpeed = (float)((int)((float)Game1.random.Next((int)this.minSpeed, (int)this.maxSpeed) / 20f));
			}
			if (Math.Abs(this.rotation - this.targetRotation) > Math.Abs(this.targetRotation / (100f - this.turnSpeed)))
			{
				this.rotation += this.targetRotation / (100f - this.turnSpeed);
			}
			this.rotation %= 6.28318548f;
			this.currentSpeed += (this.targetSpeed - this.currentSpeed) / 10f;
			this.currentSpeed = Math.Min(this.maxSpeed / 20f, this.currentSpeed);
			this.currentSpeed = Math.Max(this.minSpeed / 20f, this.currentSpeed);
			this.position.X = this.position.X + (float)((double)this.currentSpeed * Math.Cos((double)this.rotation));
			int num = 0;
			if (!this.fishingField.Contains(new Rectangle((int)this.position.X - Game1.tileSize / 2, (int)this.position.Y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize)))
			{
				Vector2 vector = new Vector2(this.currentSpeed * (float)Math.Cos((double)this.rotation), this.currentSpeed * (float)Math.Sin((double)this.rotation));
				vector.X = -vector.X;
				this.rotation = (float)Math.Atan((double)(vector.Y / vector.X));
				if (vector.X < 0f)
				{
					this.rotation += 3.14159274f;
				}
				else if (vector.Y < 0f)
				{
					this.rotation += 1.57079637f;
				}
				this.position.X = this.position.X + (float)((double)this.currentSpeed * Math.Cos((double)this.rotation));
				num++;
			}
			this.position.Y = this.position.Y + (float)((double)this.currentSpeed * Math.Sin((double)this.rotation));
			if (!this.fishingField.Contains(new Rectangle((int)this.position.X - Game1.tileSize / 2, (int)this.position.Y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize)))
			{
				Vector2 vector2 = new Vector2(this.currentSpeed * (float)Math.Cos((double)this.rotation), this.currentSpeed * (float)Math.Sin((double)this.rotation));
				vector2.Y = -vector2.Y;
				this.rotation = (float)Math.Atan((double)(vector2.Y / vector2.X));
				if (vector2.X < 0f)
				{
					this.rotation += 3.14159274f;
				}
				else if (vector2.Y > 0f)
				{
					this.rotation += 1.57079637f;
				}
				this.position.Y = this.position.Y + (float)((double)this.currentSpeed * Math.Sin((double)this.rotation));
				num++;
			}
			if (num >= 2)
			{
				Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point((int)this.position.X, (int)this.position.Y), new Vector2(514f, 306f), this.currentSpeed);
				this.rotation = (float)Math.Atan((double)(velocityTowardPoint.Y / velocityTowardPoint.X));
				if (velocityTowardPoint.X < 0f)
				{
					this.rotation += 3.14159274f;
				}
				else if (velocityTowardPoint.Y < 0f)
				{
					this.rotation += 1.57079637f;
				}
				this.position.X = this.position.X + (float)((double)this.currentSpeed * Math.Cos((double)this.rotation));
				this.position.Y = this.position.Y + (float)((double)this.currentSpeed * Math.Sin((double)this.rotation));
				return;
			}
			if (num == 1)
			{
				this.targetRotation = this.rotation;
			}
		}

		public void draw(SpriteBatch b, Vector2 positionOfFishingField)
		{
			b.Draw(Game1.mouseCursors, this.position + positionOfFishingField, new Rectangle?(new Rectangle(561, 1846 + this.indexOfAnimation * 16, 16, 16)), Color.White, this.rotation + 1.57079637f, new Vector2(8f, 8f), 4f, SpriteEffects.None, 0.5f);
		}
	}
}
