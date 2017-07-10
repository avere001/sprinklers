using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
	public class SlayMonsterQuest : Quest
	{
		public string targetMessage;

		public string monsterName;

		public string target;

		public Monster monster;

		public NPC actualTarget;

		public int numberToKill;

		public int reward;

		public int numberKilled;

		public List<DescriptionElement> parts = new List<DescriptionElement>();

		public List<DescriptionElement> dialogueparts = new List<DescriptionElement>();

		public DescriptionElement objective;

		public SlayMonsterQuest()
		{
			this.questType = 4;
		}

		public void loadQuestInfo()
		{
			if (this.target != null && this.monster != null)
			{
				return;
			}
			base.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13696", new object[0]);
			List<string> list = new List<string>();
			int deepestMineLevel = Game1.player.deepestMineLevel;
			if (deepestMineLevel < 39)
			{
				list.Add("Green Slime");
				if (deepestMineLevel > 10)
				{
					list.Add("Rock Crab");
				}
				if (deepestMineLevel > 30)
				{
					list.Add("Duggy");
				}
			}
			else if (deepestMineLevel < 79)
			{
				list.Add("Frost Jelly");
				list.Add("Skeleton");
				list.Add("Dust Spirit");
			}
			else
			{
				list.Add("Sludge");
				list.Add("Ghost");
				list.Add("Lava Crab");
				list.Add("Squid Kid");
			}
			bool expr_CC = this.monsterName == null;
			if (expr_CC)
			{
				this.monsterName = list.ElementAt(this.random.Next(list.Count));
			}
			if (this.monsterName == "Frost Jelly" || this.monsterName == "Sludge")
			{
				this.monster = new Monster("Green Slime", Vector2.Zero);
				this.monster.name = this.monsterName;
			}
			else
			{
				this.monster = new Monster(this.monsterName, Vector2.Zero);
			}
			if (expr_CC)
			{
				string text = this.monsterName;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 703662834u)
				{
					if (num <= 503018864u)
					{
						if (num != 165007071u)
						{
							if (num == 503018864u)
							{
								if (text == "Ghost")
								{
									this.numberToKill = this.random.Next(1, 3);
									this.reward = this.numberToKill * 250;
								}
							}
						}
						else if (text == "Lava Crab")
						{
							this.numberToKill = this.random.Next(2, 6);
							this.reward = this.numberToKill * 180;
						}
					}
					else if (num != 510600819u)
					{
						if (num == 703662834u)
						{
							if (text == "Rock Crab")
							{
								this.numberToKill = this.random.Next(2, 6);
								this.reward = this.numberToKill * 75;
							}
						}
					}
					else if (text == "Duggy")
					{
						this.parts.Clear();
						this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13711", this.numberToKill));
						this.target = "Clint";
						this.numberToKill = this.random.Next(2, 4);
						this.reward = this.numberToKill * 150;
					}
				}
				else if (num <= 1114282268u)
				{
					if (num != 1104688147u)
					{
						if (num == 1114282268u)
						{
							if (text == "Green Slime")
							{
								this.numberToKill = this.random.Next(4, 9);
								this.numberToKill -= this.numberToKill % 2;
								this.reward = this.numberToKill * 60;
							}
						}
					}
					else if (text == "Sludge")
					{
						this.numberToKill = this.random.Next(4, 9);
						this.numberToKill -= this.numberToKill % 2;
						this.reward = this.numberToKill * 125;
					}
				}
				else if (num != 2124830350u)
				{
					if (num != 2223526605u)
					{
						if (num == 3125849181u)
						{
							if (text == "Frost Jelly")
							{
								this.numberToKill = this.random.Next(4, 9);
								this.numberToKill -= this.numberToKill % 2;
								this.reward = this.numberToKill * 85;
							}
						}
					}
					else if (text == "Squid Kid")
					{
						this.numberToKill = this.random.Next(1, 3);
						this.reward = this.numberToKill * 350;
					}
				}
				else if (text == "Skeleton")
				{
					this.numberToKill = this.random.Next(1, 4);
					this.reward = this.numberToKill * 120;
				}
			}
			if (this.monsterName.Equals("Green Slime") || this.monsterName.Equals("Frost Jelly") || this.monsterName.Equals("Sludge"))
			{
				this.parts.Clear();
				this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13723", this.numberToKill, this.monsterName.Equals("Frost Jelly") ? new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13725") : (this.monsterName.Equals("Sludge") ? new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13727") : new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13728"))));
				this.target = "Lewis";
				this.dialogueparts.Clear();
				this.dialogueparts.Add("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13730");
				if (this.random.NextDouble() < 0.5)
				{
					this.dialogueparts.Add("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13731");
					this.dialogueparts.Add((this.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13732" : "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13733");
					DescriptionElement param = new DescriptionElement[]
					{
						"Strings\\StringsFromCSFiles:Dialogue.cs.795",
						"Strings\\StringsFromCSFiles:Dialogue.cs.796",
						"Strings\\StringsFromCSFiles:Dialogue.cs.797",
						"Strings\\StringsFromCSFiles:Dialogue.cs.798",
						"Strings\\StringsFromCSFiles:Dialogue.cs.799",
						"Strings\\StringsFromCSFiles:Dialogue.cs.800",
						"Strings\\StringsFromCSFiles:Dialogue.cs.801",
						"Strings\\StringsFromCSFiles:Dialogue.cs.802",
						"Strings\\StringsFromCSFiles:Dialogue.cs.803",
						"Strings\\StringsFromCSFiles:Dialogue.cs.804",
						"Strings\\StringsFromCSFiles:Dialogue.cs.805",
						"Strings\\StringsFromCSFiles:Dialogue.cs.806",
						"Strings\\StringsFromCSFiles:Dialogue.cs.807",
						"Strings\\StringsFromCSFiles:Dialogue.cs.808",
						"Strings\\StringsFromCSFiles:Dialogue.cs.809",
						"Strings\\StringsFromCSFiles:Dialogue.cs.810"
					}.ElementAt(this.random.Next(16));
					this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13734", (this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13735") : new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13736"), param, (this.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13740") : ((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13741") : new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13742"))));
				}
				else
				{
					this.dialogueparts.Add("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13744");
				}
			}
			else if (this.monsterName.Equals("Rock Crab") || this.monsterName.Equals("Lava Crab"))
			{
				this.parts.Clear();
				this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13747", this.numberToKill));
				this.target = "Demetrius";
				this.dialogueparts.Clear();
				this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13750", this.monster));
			}
			else
			{
				this.parts.Clear();
				this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13752", this.monster, this.numberToKill, (this.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13755") : ((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13756") : new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13757"))));
				this.target = "Wizard";
				this.dialogueparts.Clear();
				this.dialogueparts.Add("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13760");
			}
			if (this.target.Equals("Wizard") && !Game1.player.mailReceived.Contains("wizardJunimoNote") && !Game1.player.mailReceived.Contains("JojaMember"))
			{
				this.parts.Clear();
				this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13764", this.numberToKill, this.monster));
				this.target = "Lewis";
				this.dialogueparts.Clear();
				this.dialogueparts.Add("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13767");
			}
			this.actualTarget = Game1.getCharacterFromName(this.target, false);
			this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13274", this.reward));
			this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13770", "0", this.numberToKill, this.monster);
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
			if (this.numberKilled == 0 && this.id != 0)
			{
				return;
			}
			if (this.numberKilled < this.numberToKill)
			{
				this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13770", this.numberKilled, this.numberToKill, this.monster);
			}
			if (this.objective != null)
			{
				base.currentObjective = this.objective.loadDescriptionElement();
			}
		}

		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			if (this.completed)
			{
				return false;
			}
			if (monsterName == null)
			{
				monsterName = "Green Slime";
			}
			if (n == null && monsterName != null && monsterName.Contains(this.monsterName) && this.numberKilled < this.numberToKill)
			{
				this.numberKilled = Math.Min(this.numberToKill, this.numberKilled + 1);
				if (this.numberKilled >= this.numberToKill)
				{
					if (this.target == null || this.target.Equals("null"))
					{
						base.questComplete();
					}
					else
					{
						if (this.actualTarget == null)
						{
							this.actualTarget = Game1.getCharacterFromName(this.target, false);
						}
						this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13277", this.actualTarget);
						Game1.playSound("jingle1");
					}
				}
				else if (this.monster == null)
				{
					if (monsterName == "Frost Jelly" || monsterName == "Sludge")
					{
						this.monster = new Monster("Green Slime", Vector2.Zero);
						this.monster.name = monsterName;
					}
					else
					{
						this.monster = new Monster(monsterName, Vector2.Zero);
					}
				}
				Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f, false)
				{
					scaleChangeChange = -0.012f
				});
			}
			else if (n != null && this.target != null && !this.target.Equals("null") && this.numberKilled >= this.numberToKill && n.name.Equals(this.target) && n.isVillager())
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
