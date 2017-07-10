using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Quests
{
	[XmlInclude(typeof(SocializeQuest)), XmlInclude(typeof(SlayMonsterQuest)), XmlInclude(typeof(ResourceCollectionQuest)), XmlInclude(typeof(ItemDeliveryQuest)), XmlInclude(typeof(ItemHarvestQuest)), XmlInclude(typeof(CraftingQuest)), XmlInclude(typeof(FishingQuest)), XmlInclude(typeof(GoSomewhereQuest)), XmlInclude(typeof(LostItemQuest)), XmlInclude(typeof(DescriptionElement))]
	public class Quest
	{
		public const int type_basic = 1;

		public const int type_crafting = 2;

		public const int type_itemDelivery = 3;

		public const int type_monster = 4;

		public const int type_socialize = 5;

		public const int type_location = 6;

		public const int type_fishing = 7;

		public const int type_building = 8;

		public const int type_harvest = 9;

		public const int type_resource = 10;

		public const int type_weeding = 11;

		public string _currentObjective = "";

		public string _questDescription = "";

		public string _questTitle = "";

		public string rewardDescription;

		public string completionString;

		protected Random random = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);

		public bool accepted;

		public bool completed;

		public bool dailyQuest;

		public bool showNew;

		public bool canBeCancelled;

		public bool destroy;

		public int id;

		public int moneyReward;

		public int questType;

		public int daysLeft;

		public List<int> nextQuests = new List<int>();

		public string questTitle
		{
			get
			{
				switch (this.questType)
				{
				case 3:
					this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13285", new object[0]);
					break;
				case 4:
					this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13696", new object[0]);
					break;
				case 5:
					this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SocializeQuest.cs.13785", new object[0]);
					break;
				case 7:
					this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingQuest.cs.13227", new object[0]);
					break;
				case 10:
					this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13640", new object[0]);
					break;
				}
				Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
				if (dictionary != null && dictionary.ContainsKey(this.id))
				{
					string[] array = dictionary[this.id].Split(new char[]
					{
						'/'
					});
					this._questTitle = array[1];
				}
				if (this._questTitle == null)
				{
					this._questTitle = "";
				}
				return this._questTitle;
			}
			set
			{
				this._questTitle = value;
			}
		}

		[XmlIgnore]
		public string questDescription
		{
			get
			{
				this.reloadDescription();
				Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
				if (dictionary != null && dictionary.ContainsKey(this.id))
				{
					string[] array = dictionary[this.id].Split(new char[]
					{
						'/'
					});
					this._questDescription = array[2];
				}
				if (this._questDescription == null)
				{
					this._questDescription = "";
				}
				return this._questDescription;
			}
			set
			{
				this._questDescription = value;
			}
		}

		[XmlIgnore]
		public string currentObjective
		{
			get
			{
				Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
				if (dictionary != null && dictionary.ContainsKey(this.id))
				{
					string[] array = dictionary[this.id].Split(new char[]
					{
						'/'
					});
					if (array[3].Length > 1)
					{
						this._currentObjective = array[3];
					}
				}
				this.reloadObjective();
				if (this._currentObjective == null)
				{
					this._currentObjective = "";
				}
				return this._currentObjective;
			}
			set
			{
				this._currentObjective = value;
			}
		}

		public static Quest getQuestFromId(int id)
		{
			Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
			if (dictionary != null && dictionary.ContainsKey(id))
			{
				string[] array = dictionary[id].Split(new char[]
				{
					'/'
				});
				string text = array[0];
				Quest quest = null;
				string[] array2 = array[4].Split(new char[]
				{
					' '
				});
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1539345862u)
				{
					if (num <= 133275711u)
					{
						if (num != 126609884u)
						{
							if (num == 133275711u)
							{
								if (text == "Crafting")
								{
									quest = new CraftingQuest(Convert.ToInt32(array2[0]), array2[1].ToLower().Equals("true"));
									quest.questType = 2;
								}
							}
						}
						else if (text == "LostItem")
						{
							quest = new LostItemQuest(array2[0], array2[2], Convert.ToInt32(array2[1]), Convert.ToInt32(array2[3]), Convert.ToInt32(array2[4]));
						}
					}
					else if (num != 1217142150u)
					{
						if (num == 1539345862u)
						{
							if (text == "Location")
							{
								quest = new GoSomewhereQuest(array2[0]);
								quest.questType = 6;
							}
						}
					}
					else if (text == "ItemDelivery")
					{
						quest = new ItemDeliveryQuest();
						(quest as ItemDeliveryQuest).target = array2[0];
						(quest as ItemDeliveryQuest).item = Convert.ToInt32(array2[1]);
						(quest as ItemDeliveryQuest).targetMessage = array[9];
						if (array2.Length > 2)
						{
							(quest as ItemDeliveryQuest).number = Convert.ToInt32(array2[2]);
						}
						quest.questType = 3;
					}
				}
				else if (num <= 2324152213u)
				{
					if (num != 1629445681u)
					{
						if (num == 2324152213u)
						{
							if (text == "Building")
							{
								quest = new Quest();
								quest.questType = 8;
								quest.completionString = array2[0];
							}
						}
					}
					else if (text == "Monster")
					{
						quest = new SlayMonsterQuest();
						(quest as SlayMonsterQuest).loadQuestInfo();
						(quest as SlayMonsterQuest).monster.name = array2[0].Replace('_', ' ');
						if ((quest as SlayMonsterQuest).monsterName == "Frost Jelly" || (quest as SlayMonsterQuest).monsterName == "Sludge")
						{
							(quest as SlayMonsterQuest).monster = new Monster();
							(quest as SlayMonsterQuest).monster.name = (quest as SlayMonsterQuest).monsterName;
						}
						else
						{
							(quest as SlayMonsterQuest).monster = new Monster((quest as SlayMonsterQuest).monsterName, Vector2.Zero);
						}
						(quest as SlayMonsterQuest).numberToKill = Convert.ToInt32(array2[1]);
						if (array2.Length > 2)
						{
							(quest as SlayMonsterQuest).target = array2[2];
						}
						else
						{
							(quest as SlayMonsterQuest).target = "null";
						}
						quest.questType = 4;
					}
				}
				else if (num != 3610215645u)
				{
					if (num != 4023868591u)
					{
						if (num == 4177547506u)
						{
							if (text == "Social")
							{
								quest = new SocializeQuest();
							}
						}
					}
					else if (text == "ItemHarvest")
					{
						quest = new ItemHarvestQuest(Convert.ToInt32(array2[0]), (array2.Length > 1) ? Convert.ToInt32(array2[1]) : 1);
					}
				}
				else if (text == "Basic")
				{
					quest = new Quest();
					quest.questType = 1;
				}
				quest.id = id;
				quest.questTitle = array[1];
				quest.questDescription = array[2];
				if (array[3].Length > 1)
				{
					quest.currentObjective = array[3];
				}
				string[] array3 = array[5].Split(new char[]
				{
					' '
				});
				for (int i = 0; i < array3.Length; i++)
				{
					quest.nextQuests.Add(Convert.ToInt32(array3[i]));
				}
				quest.showNew = true;
				quest.moneyReward = Convert.ToInt32(array[6]);
				quest.rewardDescription = (array[6].Equals("-1") ? null : array[7]);
				if (array.Length > 8)
				{
					quest.canBeCancelled = array[8].Equals("true");
				}
				return quest;
			}
			return null;
		}

		public virtual void reloadObjective()
		{
		}

		public virtual void reloadDescription()
		{
		}

		public virtual void adjustGameLocation(GameLocation location)
		{
		}

		public virtual void accept()
		{
			this.accepted = true;
		}

		public virtual bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (this.completionString != null && str != null && str.Equals(this.completionString))
			{
				this.questComplete();
				return true;
			}
			return false;
		}

		public bool hasReward()
		{
			return this.moneyReward > 0 || (this.rewardDescription != null && this.rewardDescription.Length > 2);
		}

		public void questComplete()
		{
			if (!this.completed)
			{
				if (this.dailyQuest || this.questType == 7)
				{
					Stats expr_21 = Game1.stats;
					uint questsCompleted = expr_21.QuestsCompleted;
					expr_21.QuestsCompleted = questsCompleted + 1u;
				}
				this.completed = true;
				if (this.nextQuests.Count > 0)
				{
					foreach (int current in this.nextQuests)
					{
						if (current > 0)
						{
							Game1.player.questLog.Add(Quest.getQuestFromId(current));
						}
					}
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Quest.cs.13636", new object[0]), 2));
				}
				if (this.moneyReward <= 0 && (this.rewardDescription == null || this.rewardDescription.Length <= 2))
				{
					Game1.player.questLog.Remove(this);
				}
				else
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Quest.cs.13636", new object[0]), 2));
				}
				Game1.playSound("questcomplete");
			}
		}
	}
}
