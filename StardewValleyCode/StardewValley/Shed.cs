using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using xTile;

namespace StardewValley
{
	public class Shed : DecoratableLocation
	{
		public Shed()
		{
		}

		public Shed(Map m, string name) : base(m, name)
		{
			List<Rectangle> list = DecoratableLocation.getWalls();
			while (this.wallPaper.Count < list.Count)
			{
				this.wallPaper.Add(0);
			}
			list = DecoratableLocation.getFloors();
			while (this.floor.Count < list.Count)
			{
				this.floor.Add(0);
			}
		}

		public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
		{
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.isDarkOut())
			{
				Game1.ambientLight = new Color(180, 180, 0);
			}
		}

		public Building getBuilding()
		{
			foreach (Building current in Game1.getFarm().buildings)
			{
				if (current.indoors != null && current.indoors.Equals(this))
				{
					return current;
				}
			}
			return null;
		}
	}
}
