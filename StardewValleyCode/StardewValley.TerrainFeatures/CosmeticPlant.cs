using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
	public class CosmeticPlant : Grass
	{
		public bool flipped;

		public float scale = 1f;

		private int xOffset;

		private int yOffset;

		public CosmeticPlant()
		{
		}

		public CosmeticPlant(int which) : base(which, 1)
		{
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.scale = 1f - ((Game1.random.NextDouble() < 0.5) ? ((float)Game1.random.Next((which == 0) ? 10 : 51) / 100f) : 0f);
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 4)), (int)((tileLocation.Y + 1f) * (float)Game1.tileSize - (float)(Game1.tileSize / 8) - 4f), Game1.tileSize / 8, Game1.tileSize / 16 + 4);
		}

		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		public override void loadSprite()
		{
			try
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\upperCavePlants");
			}
			catch (Exception)
			{
			}
			this.xOffset = Game1.random.Next(-2, 3) * 4;
			this.yOffset = Game1.random.Next(-2, 1) * 4;
		}

		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if ((t != null && t is MeleeWeapon && ((MeleeWeapon)t).type != 2) || explosion > 0)
			{
				base.shake(0.2945243f, 0.07853982f, Game1.random.NextDouble() < 0.5);
				int num;
				if (explosion > 0)
				{
					num = Math.Max(1, explosion + 2 - Game1.random.Next(2));
				}
				else
				{
					num = ((t.upgradeLevel == 3) ? 3 : (t.upgradeLevel + 1));
				}
				Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(28 + (int)this.grassType * Game1.tileSize, 24, 28, 24), (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(6, 14));
				this.numberOfWeeds -= num;
				if (this.numberOfWeeds <= 0)
				{
					Random random = new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + (float)Game1.mine.mineLevel + (float)Game1.player.timesReachedMineBottom));
					if (random.NextDouble() < 0.005)
					{
						Game1.createObjectDebris(114, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
					}
					else if (random.NextDouble() < 0.01)
					{
						Game1.createDebris((random.NextDouble() < 0.5) ? 4 : 8, (int)tileLocation.X, (int)tileLocation.Y, random.Next(1, 2), null);
					}
					else if (random.NextDouble() < 0.02)
					{
						Game1.createDebris(92, (int)tileLocation.X, (int)tileLocation.Y, random.Next(2, 4), null);
					}
					return true;
				}
			}
			return false;
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize) + new Vector2((float)(Game1.tileSize / 2 + this.xOffset), (float)(Game1.tileSize - 4 + this.yOffset))), new Rectangle?(new Rectangle((int)this.grassType * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2)), Color.White, this.shakeRotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2 - 4)), this.scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, ((float)(this.getBoundingBox(tileLocation).Y - 4) + tileLocation.X / 900f + this.scale / 100f) / 10000f);
		}
	}
}
