using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml.Serialization;

namespace StardewValley
{
	public class BuildingUpgrade
	{
		public int daysLeftTillUpgradeDone = 4;

		public string whichBuilding;

		public Vector2 positionOfCarpenter;

		[XmlIgnore]
		public Texture2D workerTexture;

		private int currentFrame = 2;

		private int numberOfHammers;

		private int currentHammer;

		private int pauseTime = 1000;

		private int hammerPause = 500;

		private float timeAccumulator;

		public BuildingUpgrade(string whichBuilding, Vector2 positionOfCarpenter)
		{
			this.whichBuilding = whichBuilding;
			this.positionOfCarpenter = positionOfCarpenter;
			this.positionOfCarpenter.X = this.positionOfCarpenter.X * (float)Game1.tileSize;
			this.positionOfCarpenter.X = this.positionOfCarpenter.X - (float)(Game1.tileSize / 10);
			this.positionOfCarpenter.Y = this.positionOfCarpenter.Y * (float)Game1.tileSize;
			this.positionOfCarpenter.Y = this.positionOfCarpenter.Y - (float)(Game1.tileSize / 2);
			this.workerTexture = Game1.content.Load<Texture2D>("LooseSprites\\robinAtWork");
		}

		public BuildingUpgrade()
		{
			this.workerTexture = Game1.content.Load<Texture2D>("LooseSprites\\robinAtWork");
		}

		public Rectangle getSourceRectangle()
		{
			return new Rectangle(this.currentFrame * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
		}

		public void update(float milliseconds)
		{
			this.timeAccumulator += milliseconds;
			if (this.numberOfHammers > 0 && (this.timeAccumulator > (float)this.hammerPause || (this.timeAccumulator > (float)(this.hammerPause / 3) && this.currentFrame == 3)))
			{
				this.timeAccumulator = 0f;
				switch (this.currentFrame)
				{
				case 0:
					this.currentFrame = 1;
					Game1.playSound("woodyStep");
					break;
				case 1:
				case 2:
				case 3:
				case 4:
					this.currentFrame = 0;
					break;
				}
				this.currentHammer++;
				if (this.currentHammer >= this.numberOfHammers && this.currentFrame == 0)
				{
					this.currentHammer = 0;
					this.numberOfHammers = 0;
					this.pauseTime = Game1.random.Next(800, 3000);
					this.currentFrame = ((Game1.random.NextDouble() < 0.2) ? 4 : 2);
					return;
				}
			}
			else if (this.timeAccumulator > (float)this.pauseTime)
			{
				this.timeAccumulator = 0f;
				this.numberOfHammers = Game1.random.Next(2, 14);
				this.currentFrame = 3;
				this.hammerPause = Game1.random.Next(400, 700);
			}
		}
	}
}
