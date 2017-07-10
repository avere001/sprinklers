using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile.Dimensions;

namespace StardewValley
{
	public class Background
	{
		public int defaultChunkIndex;

		public int numChunksInSheet;

		public double chanceForDeviationFromDefault;

		private Texture2D backgroundImage;

		private Vector2 position = Vector2.Zero;

		private int chunksWide;

		private int chunksHigh;

		private int chunkWidth;

		private int chunkHeight;

		private int[] chunks;

		private float zoom;

		public Color c;

		private bool summitBG;

		public int yOffset;

		public Background()
		{
			this.summitBG = true;
			this.c = Color.White;
		}

		public Background(Texture2D bgImage, int seedValue, int chunksWide, int chunksHigh, int chunkWidth, int chunkHeight, float zoom, int defaultChunkIndex, int numChunksInSheet, double chanceForDeviation, Color c)
		{
			this.backgroundImage = bgImage;
			this.chunksWide = chunksWide;
			this.chunksHigh = chunksHigh;
			this.zoom = zoom;
			this.chunkWidth = chunkWidth;
			this.chunkHeight = chunkHeight;
			this.defaultChunkIndex = defaultChunkIndex;
			this.numChunksInSheet = numChunksInSheet;
			this.chanceForDeviationFromDefault = chanceForDeviation;
			this.c = c;
			Random random = new Random(seedValue);
			this.chunks = new int[chunksWide * chunksHigh];
			for (int i = 0; i < chunksHigh * chunksWide; i++)
			{
				if (random.NextDouble() < this.chanceForDeviationFromDefault)
				{
					this.chunks[i] = random.Next(numChunksInSheet);
				}
				else
				{
					this.chunks[i] = defaultChunkIndex;
				}
			}
		}

		public void update(xTile.Dimensions.Rectangle viewport)
		{
			this.position.X = -((float)(viewport.X + viewport.Width / 2) / ((float)Game1.currentLocation.map.GetLayer("Back").LayerWidth * (float)Game1.tileSize) * ((float)(this.chunksWide * this.chunkWidth) * this.zoom - (float)viewport.Width));
			this.position.Y = -((float)(viewport.Y + viewport.Height / 2) / ((float)Game1.currentLocation.map.GetLayer("Back").LayerHeight * (float)Game1.tileSize) * ((float)(this.chunksHigh * this.chunkHeight) * this.zoom - (float)viewport.Height));
		}

		public void draw(SpriteBatch b)
		{
			if (this.summitBG)
			{
				int num = 0;
				string currentSeason = Game1.currentSeason;
				if (!(currentSeason == "summer"))
				{
					if (!(currentSeason == "fall"))
					{
						if (currentSeason == "winter")
						{
							num = 2;
						}
					}
					else
					{
						num = 1;
					}
				}
				else
				{
					num = 0;
				}
				float num2 = 1f;
				Color value = Color.White;
				if (Game1.timeOfDay >= 1800)
				{
					int num3 = (int)((float)(Game1.timeOfDay - Game1.timeOfDay % 100) + (float)(Game1.timeOfDay % 100 / 10) * 16.66f);
					this.c = new Color(255f, 255f - Math.Max(100f, (float)num3 + (float)Game1.gameTimeInterval / 7000f * 16.6f - 1800f), 255f - Math.Max(100f, ((float)num3 + (float)Game1.gameTimeInterval / 7000f * 16.6f - 1800f) / 2f));
					value = Color.Blue * 0.5f;
					num2 = Math.Max(0f, Math.Min(1f, (2000f - ((float)num3 + (float)Game1.gameTimeInterval / 7000f * 16.6f)) / 200f));
				}
				b.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height * 3 / 4), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(639 + (num + 1), 1051, 1, 400)), this.c * num2, 0f, Vector2.Zero, SpriteEffects.None, 1E-07f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - 596)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 736 + num * 149, 639, 149)), Color.White * Math.Max((float)this.c.A, 0.5f), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - 596)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 736 + num * 149, 639, 149)), value * (0.75f - num2), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
				return;
			}
			Vector2 zero = Vector2.Zero;
			Microsoft.Xna.Framework.Rectangle value2 = new Microsoft.Xna.Framework.Rectangle(0, 0, this.chunkWidth, this.chunkHeight);
			for (int i = 0; i < this.chunks.Length; i++)
			{
				zero.X = this.position.X + (float)(i * this.chunkWidth % (this.chunksWide * this.chunkWidth)) * this.zoom;
				zero.Y = this.position.Y + (float)(i * this.chunkWidth / (this.chunksWide * this.chunkWidth) * this.chunkHeight) * this.zoom;
				if (this.backgroundImage == null)
				{
					b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)zero.X, (int)zero.Y, Game1.viewport.Width, Game1.viewport.Height), new Microsoft.Xna.Framework.Rectangle?(value2), this.c, 0f, Vector2.Zero, SpriteEffects.None, 0f);
				}
				else
				{
					value2.X = this.chunks[i] * this.chunkWidth % this.backgroundImage.Width;
					value2.Y = this.chunks[i] * this.chunkWidth / this.backgroundImage.Width * this.chunkHeight;
					b.Draw(this.backgroundImage, zero, new Microsoft.Xna.Framework.Rectangle?(value2), this.c, 0f, Vector2.Zero, this.zoom, SpriteEffects.None, 0f);
				}
			}
		}
	}
}
