using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
	public class FishingQuest : Quest
	{
		public string target;

		public string targetMessage;

		public int numberToFish;

		public int reward;

		public int numberFished;

		public int whichFish;

		public StardewValley.Object fish;

		public List<DescriptionElement> parts = new List<DescriptionElement>();

		public List<DescriptionElement> dialogueparts = new List<DescriptionElement>();

		public DescriptionElement objective;

		public FishingQuest()
		{
			this.questType = 7;
		}

		public void loadQuestInfo()
		{
			if (this.target != null && this.fish != null)
			{
				return;
			}
			base.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingQuest.cs.13227", new object[0]);
			if (this.random.NextDouble() < 0.5)
			{
				string currentSeason = Game1.currentSeason;
				if (!(currentSeason == "spring"))
				{
					if (!(currentSeason == "summer"))
					{
						if (!(currentSeason == "fall"))
						{
							if (currentSeason == "winter")
							{
								int[] array = new int[]
								{
									130,
									131,
									136,
									141,
									143,
									144,
									146,
									147,
									150,
									151
								};
								this.whichFish = array[this.random.Next(array.Length)];
							}
						}
						else
						{
							int[] array = new int[]
							{
								129,
								131,
								136,
								137,
								139,
								142,
								143,
								150
							};
							this.whichFish = array[this.random.Next(array.Length)];
						}
					}
					else
					{
						int[] array = new int[]
						{
							130,
							131,
							136,
							138,
							142,
							144,
							145,
							146,
							149,
							150
						};
						this.whichFish = array[this.random.Next(array.Length)];
					}
				}
				else
				{
					int[] array = new int[]
					{
						129,
						131,
						136,
						137,
						142,
						143,
						145,
						147
					};
					this.whichFish = array[this.random.Next(array.Length)];
				}
				this.fish = new StardewValley.Object(Vector2.Zero, this.whichFish, 1);
				this.numberToFish = (int)Math.Ceiling(90.0 / (double)Math.Max(1, this.fish.price)) + Game1.player.FishingLevel / 5;
				this.reward = this.numberToFish * this.fish.price;
				this.target = "Demetrius";
				this.parts.Clear();
				this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13228", this.fish, this.numberToFish));
				this.dialogueparts.Clear();
				this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13231", this.fish, new DescriptionElement[]
				{
					"Strings\\StringsFromCSFiles:FishingQuest.cs.13233",
					"Strings\\StringsFromCSFiles:FishingQuest.cs.13234",
					"Strings\\StringsFromCSFiles:FishingQuest.cs.13235",
					new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13236", this.fish)
				}.ElementAt(this.random.Next(4))));
				this.objective = (this.fish.name.Equals("Octopus") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13243", 0, this.numberToFish) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13244", 0, this.numberToFish, this.fish));
			}
			else
			{
				string currentSeason = Game1.currentSeason;
				if (!(currentSeason == "spring"))
				{
					if (!(currentSeason == "summer"))
					{
						if (!(currentSeason == "fall"))
						{
							if (currentSeason == "winter")
							{
								int[] array2 = new int[]
								{
									130,
									131,
									136,
									141,
									143,
									144,
									146,
									147,
									150,
									151,
									699,
									702,
									705
								};
								this.whichFish = array2[this.random.Next(array2.Length)];
							}
						}
						else
						{
							int[] array2 = new int[]
							{
								129,
								131,
								136,
								137,
								139,
								142,
								143,
								150,
								699,
								702,
								705
							};
							this.whichFish = array2[this.random.Next(array2.Length)];
						}
					}
					else
					{
						int[] array2 = new int[]
						{
							128,
							130,
							131,
							136,
							138,
							142,
							144,
							145,
							146,
							149,
							150,
							702
						};
						this.whichFish = array2[this.random.Next(array2.Length)];
					}
				}
				else
				{
					int[] array2 = new int[]
					{
						129,
						131,
						136,
						137,
						142,
						143,
						145,
						147,
						702
					};
					this.whichFish = array2[this.random.Next(array2.Length)];
				}
				this.target = "Willy";
				this.fish = new StardewValley.Object(Vector2.Zero, this.whichFish, 1);
				this.numberToFish = (int)Math.Ceiling(90.0 / (double)Math.Max(1, this.fish.price)) + Game1.player.FishingLevel / 5;
				this.reward = this.numberToFish * this.fish.price;
				this.parts.Clear();
				if (Game1.player.isMale)
				{
					this.parts.Add(this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13248", this.reward, this.numberToFish, new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13253")) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13248", this.reward, this.numberToFish, this.fish));
				}
				else
				{
					this.parts.Add(this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13251", this.reward, this.numberToFish, new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13253")) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13251", this.reward, this.numberToFish, this.fish));
				}
				this.dialogueparts.Clear();
				this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13256", this.fish));
				this.dialogueparts.Add(new DescriptionElement[]
				{
					"Strings\\StringsFromCSFiles:FishingQuest.cs.13258",
					"Strings\\StringsFromCSFiles:FishingQuest.cs.13259",
					new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13260", new DescriptionElement[]
					{
						"Strings\\StringsFromCSFiles:FishingQuest.cs.13261",
						"Strings\\StringsFromCSFiles:FishingQuest.cs.13262",
						"Strings\\StringsFromCSFiles:FishingQuest.cs.13263",
						"Strings\\StringsFromCSFiles:FishingQuest.cs.13264",
						"Strings\\StringsFromCSFiles:FishingQuest.cs.13265",
						"Strings\\StringsFromCSFiles:FishingQuest.cs.13266"
					}.ElementAt(this.random.Next(6))),
					"Strings\\StringsFromCSFiles:FishingQuest.cs.13267"
				}.ElementAt(this.random.Next(4)));
				this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13268"));
				this.objective = (this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13255", 0, this.numberToFish) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13244", 0, this.numberToFish, this.fish));
			}
			this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13274", this.reward));
			this.parts.Add("Strings\\StringsFromCSFiles:FishingQuest.cs.13275");
		}

		public override void reloadDescription()
		{
			if (this._questDescription == "")
			{
				this.loadQuestInfo();
			}
			if (this.parts.Count == 0 || this.parts == null || this.dialogueparts.Count == 0 || this.dialogueparts == null)
			{
				return;
			}
			string text = "";
			string str = "";
			foreach (DescriptionElement current in this.parts)
			{
				text += current.loadDescriptionElement();
			}
			foreach (DescriptionElement current2 in this.dialogueparts)
			{
				str += current2.loadDescriptionElement();
			}
			base.questDescription = text;
			this.targetMessage = str;
		}

		public override void reloadObjective()
		{
			if (this.numberFished < this.numberToFish)
			{
				this.objective = (this.fish.name.Equals("Octopus") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13243", this.numberFished, this.numberToFish) : (this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13255", this.numberFished, this.numberToFish) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13244", this.numberFished, this.numberToFish, this.fish)));
			}
			if (this.objective != null)
			{
				base.currentObjective = this.objective.loadDescriptionElement();
			}
		}

		public override bool checkIfComplete(NPC n = null, int fishid = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			this.loadQuestInfo();
			if (n == null && fishid != -1 && fishid == this.whichFish && this.numberFished < this.numberToFish)
			{
				this.numberFished = Math.Min(this.numberToFish, this.numberFished + 1);
				if (this.numberFished >= this.numberToFish)
				{
					this.dailyQuest = false;
					if (this.target == null)
					{
						this.target = "Willy";
					}
					NPC characterFromName = Game1.getCharacterFromName(this.target, false);
					this.objective = new DescriptionElement("Strings\\Quests:ObjectiveReturnToNPC", characterFromName);
					Game1.playSound("jingle1");
				}
			}
			else if (n != null && this.numberFished >= this.numberToFish && this.target != null && n.name.Equals(this.target) && n.isVillager() && !this.completed)
			{
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				this.moneyReward = this.reward;
				base.questComplete();
				Game1.drawDialogue(n);
				return true;
			}
			return false;
		}
	}
}
