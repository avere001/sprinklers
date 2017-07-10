using System;

namespace StardewValley.Network
{
	public interface IGetMapService
	{
		GameLocation getMapFromName(string name);
	}
}
