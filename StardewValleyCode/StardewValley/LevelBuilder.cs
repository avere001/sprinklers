using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using System;
using xTile.Dimensions;

namespace StardewValley
{
	public class LevelBuilder
	{
		public static bool tryToAddObject(int index, bool bigCraftable, Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				if (bigCraftable)
				{
					Game1.mine.objects.Add(position, new Object(position, index, false));
				}
				else
				{
					Game1.mine.Objects.Add(position, new Object(position, index, null, false, false, false, false));
				}
				return true;
			}
			return false;
		}

		public static bool tryToAddMonster(Monster m, Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null) && Game1.mine.isTileLocationOpen(new Location((int)position.X, (int)position.Y)) && Game1.mine.isTileOnMap(position) && Game1.mine.isTileOnClearAndSolidGround(position))
			{
				m.position = new Vector2(position.X * (float)Game1.tileSize, position.Y * (float)Game1.tileSize - (float)(m.sprite.spriteHeight - Game1.tileSize));
				Game1.mine.characters.Add(m);
				return true;
			}
			return false;
		}

		public static bool tryToAddFence(int which, Vector2 position, bool gate)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				Game1.mine.objects.Add(position, new Fence(position, which, gate));
			}
			return false;
		}

		public static bool addTorch(Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				Game1.mine.Objects.Add(position, new Torch(position, 1));
				return true;
			}
			return false;
		}

		public static bool tryToAddObject(Object obj, Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				Game1.mine.Objects.Add(position, obj);
				return true;
			}
			return false;
		}

		public static bool tryToAddObject(int index, bool bigCraftable, Vector2 position, int heldItem)
		{
			if (LevelBuilder.tryToAddObject(index, bigCraftable, position))
			{
				Game1.mine.objects[position].heldObject = new Object(position, heldItem, null, false, false, false, false);
			}
			return false;
		}
	}
}
