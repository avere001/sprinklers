using Microsoft.Xna.Framework;
using System;

namespace StardewValley.BellsAndWhistles
{
	public class EmilysParrot : TemporaryAnimatedSprite
	{
		public const int flappingPhase = 1;

		public const int hoppingPhase = 0;

		public const int lookingSidewaysPhase = 2;

		public const int nappingPhase = 3;

		public const int headBobbingPhase = 4;

		private int currentFrame;

		private int currentFrameTimer;

		private int currentPhaseTimer;

		private int currentPhase;

		private int shakeTimer;

		public EmilysParrot(Vector2 location)
		{
			base.Texture = Game1.mouseCursors;
			this.sourceRect = new Rectangle(92, 148, 9, 16);
			this.sourceRectStartingPos = new Vector2(92f, 149f);
			this.position = location;
			this.initialPosition = this.position;
			this.scale = (float)Game1.pixelZoom;
			this.id = 5858585f;
		}

		public void doAction()
		{
			Game1.playSound("parrot");
			this.shakeTimer = 800;
		}

		public override bool update(GameTime time)
		{
			this.currentPhaseTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.currentPhaseTimer <= 0)
			{
				this.currentPhase = Game1.random.Next(5);
				this.currentPhaseTimer = Game1.random.Next(4000, 16000);
				if (this.currentPhase == 1)
				{
					this.currentPhaseTimer /= 2;
					if (this.currentFrame == 0)
					{
						this.position.X = this.initialPosition.X;
					}
					else
					{
						this.position.X = this.initialPosition.X - (float)(Game1.pixelZoom * 2);
					}
				}
				else
				{
					this.position = this.initialPosition;
				}
			}
			if (this.shakeTimer > 0)
			{
				this.shakeIntensity = 1f;
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.shakeIntensity = 0f;
			}
			this.currentFrameTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.currentFrameTimer <= 0)
			{
				switch (this.currentPhase)
				{
				case 0:
					if (this.currentFrame == 7)
					{
						this.currentFrame = 0;
						this.currentFrameTimer = 600;
					}
					else if (Game1.random.NextDouble() < 0.5)
					{
						this.currentFrame = 7;
						this.currentFrameTimer = 300;
					}
					break;
				case 1:
					this.currentFrame = 6 - this.currentPhaseTimer % 1000 / 166;
					this.currentFrame = 3 - Math.Abs(this.currentFrame - 3);
					this.currentFrameTimer = 0;
					this.position.Y = this.initialPosition.Y - (float)(Game1.pixelZoom * (3 - this.currentFrame));
					if (this.currentFrame == 0)
					{
						this.position.X = this.initialPosition.X;
					}
					else
					{
						this.position.X = this.initialPosition.X - (float)(Game1.pixelZoom * 2);
					}
					break;
				case 2:
					this.currentFrame = Game1.random.Next(3, 5);
					this.currentFrameTimer = 1000;
					break;
				case 3:
					if (this.currentFrame == 5)
					{
						this.currentFrame = 6;
					}
					else
					{
						this.currentFrame = 5;
					}
					this.currentFrameTimer = 1000;
					break;
				case 4:
					if (this.currentFrame == 1 && Game1.random.NextDouble() < 0.1)
					{
						this.currentFrame = 2;
					}
					else if (this.currentFrame == 2)
					{
						this.currentFrame = 1;
					}
					else
					{
						this.currentFrame = Game1.random.Next(2);
					}
					this.currentFrameTimer = 500;
					break;
				}
			}
			if (this.currentPhase == 1 && this.currentFrame != 0)
			{
				this.sourceRect.X = 38 + this.currentFrame * 13;
				this.sourceRect.Width = 13;
			}
			else
			{
				this.sourceRect.X = 92 + this.currentFrame * 9;
				this.sourceRect.Width = 9;
			}
			return false;
		}
	}
}
