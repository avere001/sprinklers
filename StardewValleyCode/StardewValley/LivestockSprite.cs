using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class LivestockSprite : AnimatedSprite
	{
		public LivestockSprite(Texture2D texture, int currentFrame) : base(texture, 0, Game1.tileSize, Game1.tileSize * 3 / 2)
		{
		}

		public override void faceDirection(int direction)
		{
			switch (direction)
			{
			case 0:
				this.CurrentFrame = this.spriteTexture.Width / Game1.tileSize * 2 + this.CurrentFrame % (this.spriteTexture.Width / Game1.tileSize);
				break;
			case 1:
				this.CurrentFrame = this.spriteTexture.Width / (Game1.tileSize * 2) + this.CurrentFrame % (this.spriteTexture.Width / (Game1.tileSize * 2));
				break;
			case 2:
				this.CurrentFrame %= this.spriteTexture.Width / Game1.tileSize;
				break;
			case 3:
				this.CurrentFrame = this.spriteTexture.Width / (Game1.tileSize * 2) * 3 + this.CurrentFrame % (this.spriteTexture.Width / (Game1.tileSize * 2));
				break;
			}
			this.UpdateSourceRect();
		}

		public override void UpdateSourceRect()
		{
			switch (this.currentFrame)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				base.SourceRect = new Rectangle(this.currentFrame * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
				return;
			case 4:
			case 5:
			case 6:
			case 7:
				base.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize * 2, Game1.tileSize * 3 / 2, Game1.tileSize * 2, Game1.tileSize * 3 / 2);
				return;
			case 8:
			case 9:
			case 10:
			case 11:
				base.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize, Game1.tileSize * 3 / 2);
				return;
			case 12:
			case 13:
			case 14:
			case 15:
				base.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize * 2, Game1.tileSize * 9 / 2, Game1.tileSize * 2, Game1.tileSize * 3 / 2);
				return;
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
				break;
			case 24:
			case 25:
			case 26:
			case 27:
				base.SourceRect = new Rectangle((this.currentFrame - 20) * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize, Game1.tileSize * 3 / 2);
				break;
			default:
				return;
			}
		}
	}
}
