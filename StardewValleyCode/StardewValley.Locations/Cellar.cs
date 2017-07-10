using Microsoft.Xna.Framework;
using StardewValley.Objects;
using System;
using xTile;

namespace StardewValley.Locations
{
	public class Cellar : GameLocation
	{
		public Cellar()
		{
		}

		public Cellar(Map map, string name) : base(map, name)
		{
			this.setUpAgingBoards();
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
		}

		public void setUpAgingBoards()
		{
			for (int i = 6; i < 17; i++)
			{
				Vector2 vector = new Vector2((float)i, 8f);
				if (!this.objects.ContainsKey(vector))
				{
					this.objects.Add(vector, new Cask(vector));
				}
				vector = new Vector2((float)i, 10f);
				if (!this.objects.ContainsKey(vector))
				{
					this.objects.Add(vector, new Cask(vector));
				}
				vector = new Vector2((float)i, 12f);
				if (!this.objects.ContainsKey(vector))
				{
					this.objects.Add(vector, new Cask(vector));
				}
			}
		}
	}
}
