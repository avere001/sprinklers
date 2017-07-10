using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace StardewValley.Tools
{
	public class MagnifyingGlass : Tool
	{
		public MagnifyingGlass() : base("Magnifying Glass", -1, 5, 5, false, 0)
		{
			this.instantUse = true;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:MagnifyingGlass.cs.14119", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:MagnifyingGlass.cs.14120", new object[0]);
		}

		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			who.Halt();
			who.canMove = true;
			who.usingTool = false;
			this.DoFunction(location, Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 0, who);
			return true;
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			this.currentParentTileIndex = 5;
			this.indexOfMenuItemView = 5;
			Rectangle value = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (location is Farm)
			{
				using (Dictionary<long, FarmAnimal>.Enumerator enumerator = (location as Farm).animals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<long, FarmAnimal> current = enumerator.Current;
						if (current.Value.GetBoundingBox().Intersects(value))
						{
							Game1.activeClickableMenu = new AnimalQueryMenu(current.Value);
							break;
						}
					}
					return;
				}
			}
			if (location is AnimalHouse)
			{
				foreach (KeyValuePair<long, FarmAnimal> current2 in (location as AnimalHouse).animals)
				{
					if (current2.Value.GetBoundingBox().Intersects(value))
					{
						Game1.activeClickableMenu = new AnimalQueryMenu(current2.Value);
						break;
					}
				}
			}
		}
	}
}
