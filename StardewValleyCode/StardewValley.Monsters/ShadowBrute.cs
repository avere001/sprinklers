using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
	public class ShadowBrute : Monster
	{
		public ShadowBrute()
		{
			this.sprite.spriteHeight = 32;
		}

		public ShadowBrute(Vector2 position) : base("Shadow Brute", position)
		{
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
		}

		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Shadow Brute"));
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			Game1.playSound("shadowHit");
			return base.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision);
		}

		public override void deathAnimation()
		{
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(45, this.position, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0), Game1.currentLocation, 4, 64, 64);
			for (int i = 1; i < 3; i++)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(0f, 1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(0f, -1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(1f, 0f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(-1f, 0f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
			}
			Game1.playSound("shadowDie");
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, 16, 5), 16, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White, (float)Game1.pixelZoom);
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X + 2, this.sprite.SourceRect.Y + 5, 16, 5), 10, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White, (float)Game1.pixelZoom);
		}
	}
}
