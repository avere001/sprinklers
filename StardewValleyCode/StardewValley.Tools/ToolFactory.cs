using System;

namespace StardewValley.Tools
{
	public class ToolFactory
	{
		public const byte axe = 0;

		public const byte hoe = 1;

		public const byte fishingRod = 2;

		public const byte pickAxe = 3;

		public const byte wateringCan = 4;

		public const byte meleeWeapon = 5;

		public const byte slingshot = 6;

		public static ToolDescription getIndexFromTool(Tool t)
		{
			if (t is Axe)
			{
				return new ToolDescription(0, (byte)t.upgradeLevel);
			}
			if (t is Hoe)
			{
				return new ToolDescription(1, (byte)t.upgradeLevel);
			}
			if (t is FishingRod)
			{
				return new ToolDescription(2, (byte)t.upgradeLevel);
			}
			if (t is Pickaxe)
			{
				return new ToolDescription(3, (byte)t.upgradeLevel);
			}
			if (t is WateringCan)
			{
				return new ToolDescription(4, (byte)t.upgradeLevel);
			}
			if (t is MeleeWeapon)
			{
				return new ToolDescription(5, (byte)t.upgradeLevel);
			}
			if (t is Slingshot)
			{
				return new ToolDescription(6, (byte)t.upgradeLevel);
			}
			return new ToolDescription(0, 0);
		}

		public static Tool getToolFromDescription(byte index, int upgradeLevel)
		{
			Tool tool = null;
			switch (index)
			{
			case 0:
				tool = new Axe();
				break;
			case 1:
				tool = new Hoe();
				break;
			case 2:
				tool = new FishingRod();
				break;
			case 3:
				tool = new Pickaxe();
				break;
			case 4:
				tool = new WateringCan();
				break;
			case 5:
				tool = new MeleeWeapon(0, upgradeLevel);
				break;
			case 6:
				tool = new Slingshot();
				break;
			}
			tool.UpgradeLevel = upgradeLevel;
			return tool;
		}
	}
}
