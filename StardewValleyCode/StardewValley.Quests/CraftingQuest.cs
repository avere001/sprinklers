using System;

namespace StardewValley.Quests
{
	public class CraftingQuest : Quest
	{
		public bool isBigCraftable;

		public int indexToCraft;

		public CraftingQuest()
		{
		}

		public CraftingQuest(int indexToCraft, bool bigCraftable)
		{
			this.indexToCraft = indexToCraft;
			this.isBigCraftable = bigCraftable;
		}

		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (item != null && item is StardewValley.Object && (item as StardewValley.Object).bigCraftable == this.isBigCraftable && (item as StardewValley.Object).parentSheetIndex == this.indexToCraft)
			{
				base.questComplete();
				return true;
			}
			return false;
		}
	}
}
