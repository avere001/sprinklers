using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.BellsAndWhistles
{
	public abstract class Critter
	{
		public const int spriteWidth = 32;

		public const int spriteHeight = 32;

		public const float gravity = 0.25f;

		public static Texture2D critterTexture;

		public Vector2 position;

		public Vector2 startingPosition;

		public int baseFrame;

		public AnimatedSprite sprite;

		public bool flip;

		public float gravityAffectedDY;

		public float yOffset;

		public float yJumpOffset;

		public static void InitShared()
		{
			Critter.critterTexture = Game1.content.Load<Texture2D>("TileSheets\\critters");
		}

		public Critter()
		{
		}

		public Critter(int baseFrame, Vector2 position)
		{
			this.baseFrame = baseFrame;
			this.position = position;
			this.sprite = new AnimatedSprite(Critter.critterTexture, baseFrame, 32, 32);
			this.startingPosition = position;
		}

		public virtual Rectangle getBoundingBox(int xOffset, int yOffset)
		{
			return new Rectangle((int)this.position.X - Game1.tileSize / 2 + xOffset, (int)this.position.Y - Game1.tileSize / 4 + yOffset, Game1.tileSize, Game1.tileSize / 2);
		}

		public virtual bool update(GameTime time, GameLocation environment)
		{
			this.sprite.animateOnce(time);
			if (this.gravityAffectedDY < 0f || this.yJumpOffset < 0f)
			{
				this.yJumpOffset += this.gravityAffectedDY;
				this.gravityAffectedDY += 0.25f;
			}
			return this.position.X < (float)(-(float)Game1.tileSize * 2) || this.position.Y < (float)(-(float)Game1.tileSize * 2) || this.position.X > (float)environment.map.DisplayWidth || this.position.Y > (float)environment.map.DisplayHeight;
		}

		public virtual void draw(SpriteBatch b)
		{
			if (this.sprite != null)
			{
				this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(-64f, -128f + this.yJumpOffset + this.yOffset)), this.position.Y / 10000f + this.position.X / 100000f, 0, 0, Color.White, this.flip, 4f, 0f, false);
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, -4f)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + Math.Max(-3f, (this.yJumpOffset + this.yOffset) / 64f), SpriteEffects.None, (this.position.Y - 1f) / 10000f);
			}
		}

		public virtual void drawAboveFrontLayer(SpriteBatch b)
		{
		}
	}
}
