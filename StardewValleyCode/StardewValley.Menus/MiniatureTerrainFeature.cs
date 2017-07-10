using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System;

namespace StardewValley.Menus
{
	public class MiniatureTerrainFeature
	{
		private TerrainFeature feature;

		private Vector2 positionOnScreen;

		private Vector2 tileLocation;

		private float scale;

		public MiniatureTerrainFeature(TerrainFeature feature, Vector2 positionOnScreen, Vector2 tileLocation, float scale)
		{
			this.feature = feature;
			this.positionOnScreen = positionOnScreen;
			this.scale = scale;
			this.tileLocation = tileLocation;
		}

		public void draw(SpriteBatch b)
		{
			this.feature.drawInMenu(b, this.positionOnScreen, this.tileLocation, this.scale, 0.86f);
		}
	}
}
