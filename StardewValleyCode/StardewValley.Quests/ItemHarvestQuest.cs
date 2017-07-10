using System;

namespace StardewValley.Quests
{
	public class ItemHarvestQuest : Quest
	{
		public int itemIndex;

		public int number;

		public ItemHarvestQuest()
		{
		}

		public ItemHarvestQuest(int index, int number = 1)
		{
			this.itemIndex = index;
			this.number = number;
			this.questType = 9;
		}

		public override bool checkIfComplete(NPC n = null, int itemIndex = -1, int numberHarvested = 1, Item item = null, string str = null)
		{
			if (!this.completed && itemIndex != -1 && itemIndex == this.itemIndex)
			{
				this.number -= numberHarvested;
				if (this.number <= 0)
				{
					base.questComplete();
					return true;
				}
			}
			return false;
		}
	}
}
