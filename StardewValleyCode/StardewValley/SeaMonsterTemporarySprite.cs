using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class SeaMonsterTemporarySprite : TemporaryAnimatedSprite
	{
		public Texture2D texture;

		public SeaMonsterTemporarySprite(float animationInterval, int animationLength, int numberOfLoops, Vector2 position) : base(-666, animationInterval, animationLength, numberOfLoops, position, false, false)
		{
			this.texture = Game1.content.Load<Texture2D>("LooseSprites\\SeaMonster");
			Game1.playSound("pullItemFromWater");
			this.currentParentTileIndex = 0;
		}

		public override void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
		{
			spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, base.Position), new Rectangle?(new Rectangle(this.currentParentTileIndex * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (base.Position.Y + (float)(Game1.tileSize / 2)) / 10000f);
		}

		public override bool update(GameTime time)
		{
			this.timer += (float)time.ElapsedGameTime.Milliseconds;
			if (this.timer > this.interval)
			{
				this.currentParentTileIndex++;
				this.timer = 0f;
				if (this.currentParentTileIndex >= this.animationLength)
				{
					this.currentNumberOfLoops++;
					this.currentParentTileIndex = 2;
				}
			}
			if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
			{
				this.position.Y = this.position.Y + 2f;
				if (this.position.Y >= (float)Game1.currentLocation.Map.DisplayHeight)
				{
					return true;
				}
			}
			return false;
		}
	}
}
