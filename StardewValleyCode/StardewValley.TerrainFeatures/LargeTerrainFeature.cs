using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.TerrainFeatures
{
	public class LargeTerrainFeature : TerrainFeature
	{
		public Vector2 tilePosition;

		public Rectangle getBoundingBox()
		{
			return this.getBoundingBox(this.tilePosition);
		}

		public void dayUpdate(GameLocation l)
		{
			this.dayUpdate(l, this.tilePosition);
		}

		public bool tickUpdate(GameTime time)
		{
			return this.tickUpdate(time, this.tilePosition);
		}

		public void draw(SpriteBatch b)
		{
			this.draw(b, this.tilePosition);
		}
	}
}
