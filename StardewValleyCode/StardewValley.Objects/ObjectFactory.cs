using Microsoft.Xna.Framework;
using StardewValley.Tools;
using System;

namespace StardewValley.Objects
{
	public class ObjectFactory
	{
		public const byte regularObject = 0;

		public const byte bigCraftable = 1;

		public const byte weapon = 2;

		public const byte specialItem = 3;

		public const byte regularObjectRecipe = 4;

		public const byte bigCraftableRecipe = 5;

		public static ItemDescription getDescriptionFromItem(Item i)
		{
			if (i is StardewValley.Object && (i as StardewValley.Object).bigCraftable)
			{
				return new ItemDescription(1, (i as StardewValley.Object).ParentSheetIndex, i.Stack);
			}
			if (i is StardewValley.Object)
			{
				return new ItemDescription(0, (i as StardewValley.Object).ParentSheetIndex, i.Stack);
			}
			if (i is MeleeWeapon)
			{
				return new ItemDescription(2, (i as MeleeWeapon).currentParentTileIndex, i.Stack);
			}
			throw new Exception("ItemFactory trying to create item description from unknown item");
		}

		public static Item getItemFromDescription(byte type, int index, int stack)
		{
			switch (type)
			{
			case 0:
				return new StardewValley.Object(Vector2.Zero, index, stack);
			case 1:
				return new StardewValley.Object(Vector2.Zero, index, false);
			case 2:
				return new MeleeWeapon(index);
			case 4:
				return new StardewValley.Object(index, stack, true, -1, 0);
			case 5:
				return new StardewValley.Object(Vector2.Zero, index, true);
			}
			throw new Exception("ItemFactory trying to create item from unknown description");
		}
	}
}
