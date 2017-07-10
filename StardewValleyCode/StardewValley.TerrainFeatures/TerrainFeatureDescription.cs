using System;

namespace StardewValley.TerrainFeatures
{
	public struct TerrainFeatureDescription
	{
		public byte index;

		public int extraInfo;

		public TerrainFeatureDescription(byte index, int extraInfo)
		{
			this.index = index;
			this.extraInfo = extraInfo;
		}
	}
}
