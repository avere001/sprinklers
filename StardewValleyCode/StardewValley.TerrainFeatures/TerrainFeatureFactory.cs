using Microsoft.Xna.Framework;
using System;

namespace StardewValley.TerrainFeatures
{
	public class TerrainFeatureFactory
	{
		public const byte grass1 = 0;

		public const byte grass2 = 1;

		public const byte grass3 = 2;

		public const byte grass4 = 3;

		public const byte bushyTree = 4;

		public const byte leafyTree = 5;

		public const byte pineTree = 6;

		public const byte winterTree1 = 7;

		public const byte winterTree2 = 8;

		public const byte palmTree = 9;

		public const byte quartzSmall = 10;

		public const byte quartzMedium = 11;

		public const byte hoeDirt = 12;

		public const byte cosmeticPlant = 13;

		public const byte floorWood = 14;

		public const byte floorStone = 15;

		public const byte mushroomTree = 16;

		public const byte floorIce = 17;

		public const byte floorGhost = 18;

		public const byte floorStraw = 19;

		public const byte stump = 20;

		public static TerrainFeature getNewTerrainFeatureFromIndex(byte index, int extraInfo)
		{
			switch (index)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				return new Grass((int)(index + 1), extraInfo);
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				return new Tree((int)(index - 3), extraInfo);
			case 10:
				return new Quartz(1, (Game1.mine != null) ? Game1.mine.getCrystalColorForThisLevel() : Color.Green);
			case 11:
				return new Quartz(2, (Game1.mine != null) ? Game1.mine.getCrystalColorForThisLevel() : Color.Green);
			case 12:
				return new HoeDirt(Game1.isRaining ? 1 : 0);
			case 13:
				return new CosmeticPlant(extraInfo);
			case 14:
				return new Flooring(0);
			case 15:
				return new Flooring(1);
			case 16:
				return new Tree(7, extraInfo);
			case 17:
				return new Flooring(3);
			case 18:
				return new Flooring(2);
			case 19:
				return new Flooring(4);
			case 20:
				return new ResourceClump(600, 2, 2, Vector2.Zero);
			default:
				throw new MissingMethodException();
			}
		}

		public static TerrainFeatureDescription getIndexFromTerrainFeature(TerrainFeature f)
		{
			if (f.GetType() == typeof(CosmeticPlant))
			{
				return new TerrainFeatureDescription(13, (int)((CosmeticPlant)f).grassType);
			}
			if (f.GetType() == typeof(Grass))
			{
				switch (((Grass)f).grassType)
				{
				case 1:
					return new TerrainFeatureDescription(0, ((Grass)f).numberOfWeeds);
				case 2:
					return new TerrainFeatureDescription(1, ((Grass)f).numberOfWeeds);
				case 3:
					return new TerrainFeatureDescription(2, ((Grass)f).numberOfWeeds);
				case 4:
					return new TerrainFeatureDescription(3, ((Grass)f).numberOfWeeds);
				}
			}
			else if (f.GetType() == typeof(Tree))
			{
				switch (((Tree)f).treeType)
				{
				case 1:
					return new TerrainFeatureDescription(4, ((Tree)f).growthStage);
				case 2:
					return new TerrainFeatureDescription(5, ((Tree)f).growthStage);
				case 3:
					return new TerrainFeatureDescription(6, ((Tree)f).growthStage);
				case 4:
					return new TerrainFeatureDescription(7, ((Tree)f).growthStage);
				case 5:
					return new TerrainFeatureDescription(8, ((Tree)f).growthStage);
				case 7:
					return new TerrainFeatureDescription(16, ((Tree)f).growthStage);
				}
			}
			else if (f.GetType() == typeof(Quartz))
			{
				int num = ((Quartz)f).bigness;
				if (num == 1)
				{
					return new TerrainFeatureDescription(10, -1);
				}
				if (num == 2)
				{
					return new TerrainFeatureDescription(11, -1);
				}
			}
			else
			{
				if (f.GetType() == typeof(HoeDirt))
				{
					return new TerrainFeatureDescription(12, -1);
				}
				if (f.GetType() == typeof(Flooring))
				{
					int num = ((Flooring)f).whichFloor;
					if (num == 0)
					{
						return new TerrainFeatureDescription(14, -1);
					}
					if (num == 1)
					{
						return new TerrainFeatureDescription(15, -1);
					}
				}
				else if (f is ResourceClump)
				{
					int num = (f as ResourceClump).parentSheetIndex;
					if (num == 600)
					{
						return new TerrainFeatureDescription(20, -1);
					}
				}
			}
			throw new MissingMethodException();
		}
	}
}
