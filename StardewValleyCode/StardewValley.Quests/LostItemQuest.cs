using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley.Quests
{
	public class LostItemQuest : Quest
	{
		public string npcName;

		public string locationOfItem;

		public int itemIndex;

		public int tileX;

		public int tileY;

		public bool itemFound;

		public DescriptionElement objective;

		public LostItemQuest()
		{
		}

		public LostItemQuest(string npcName, string locationOfItem, int itemIndex, int tileX, int tileY)
		{
			this.npcName = npcName;
			this.locationOfItem = locationOfItem;
			this.itemIndex = itemIndex;
			this.tileX = tileX;
			this.tileY = tileY;
			this.questType = 9;
		}

		public override void adjustGameLocation(GameLocation location)
		{
			if (!this.itemFound && location.name.Equals(this.locationOfItem))
			{
				Vector2 vector = new Vector2((float)this.tileX, (float)this.tileY);
				if (location.objects.ContainsKey(vector))
				{
					location.objects.Remove(vector);
				}
				StardewValley.Object @object = new StardewValley.Object(vector, this.itemIndex, 1);
				@object.questItem = true;
				@object.isSpawnedObject = true;
				location.objects.Add(vector, @object);
			}
		}

		public new void reloadObjective()
		{
			if (this.objective != null)
			{
				base.currentObjective = this.objective.loadDescriptionElement();
			}
		}

		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (this.completed)
			{
				return false;
			}
			if (item != null && item is StardewValley.Object && (item as StardewValley.Object).parentSheetIndex == this.itemIndex && !this.itemFound)
			{
				this.itemFound = true;
				string displayName = this.npcName;
				NPC characterFromName = Game1.getCharacterFromName(this.npcName, false);
				if (characterFromName != null)
				{
					displayName = characterFromName.displayName;
				}
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Quests:MessageFoundLostItem", new object[]
				{
					item.DisplayName,
					displayName
				}));
				this.objective = new DescriptionElement("Strings\\Quests:ObjectiveReturnToNPC", characterFromName);
				Game1.playSound("jingle1");
			}
			else if (n != null && n.name.Equals(this.npcName) && n.isVillager() && this.itemFound && Game1.player.hasItemInInventory(this.itemIndex, 1, 0))
			{
				base.questComplete();
				Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
				string s = (dictionary[this.id].Length > 9) ? dictionary[this.id].Split(new char[]
				{
					'/'
				})[9] : Game1.content.LoadString("Data\\ExtraDialogue:LostItemQuest_DefaultThankYou", new object[0]);
				n.setNewDialogue(s, false, false);
				Game1.drawDialogue(n);
				Game1.player.changeFriendship(250, n);
				Game1.player.removeFirstOfThisItemFromInventory(this.itemIndex);
				return true;
			}
			return false;
		}
	}
}
