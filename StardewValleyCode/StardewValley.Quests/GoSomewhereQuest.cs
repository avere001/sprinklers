using System;

namespace StardewValley.Quests
{
	public class GoSomewhereQuest : Quest
	{
		public string whereToGo;

		public GoSomewhereQuest()
		{
		}

		public GoSomewhereQuest(string where)
		{
			this.whereToGo = where;
		}

		public override void adjustGameLocation(GameLocation location)
		{
			this.checkIfComplete(null, -1, -2, null, location.name);
		}

		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (str != null && str.Equals(this.whereToGo))
			{
				base.questComplete();
				return true;
			}
			return false;
		}
	}
}
