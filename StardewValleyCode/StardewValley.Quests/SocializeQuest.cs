using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley.Quests
{
	public class SocializeQuest : Quest
	{
		public List<string> whoToGreet;

		public int total;

		public List<DescriptionElement> parts = new List<DescriptionElement>();

		public DescriptionElement objective;

		public SocializeQuest()
		{
			this.questType = 5;
		}

		public void loadQuestInfo()
		{
			if (this.whoToGreet != null)
			{
				return;
			}
			this.whoToGreet = new List<string>();
			base.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SocializeQuest.cs.13785", new object[0]);
			this.parts.Clear();
			this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13786", (this.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13787") : ((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13788") : new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13789"))));
			this.parts.Add("Strings\\StringsFromCSFiles:SocializeQuest.cs.13791");
			using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (NPC current in enumerator.Current.characters)
					{
						if (!current.isInvisible && !current.name.Contains("Qi") && !current.name.Contains("???") && !current.name.Equals("Sandy") && !current.name.Contains("Dwarf") && !current.name.Contains("Gunther") && !current.name.Contains("Mariner") && !current.name.Contains("Henchman") && !current.name.Contains("Marlon") && !current.name.Contains("Wizard") && !current.name.Contains("Bouncer") && !current.name.Contains("Krobus") && current.isVillager())
						{
							this.whoToGreet.Add(current.name);
						}
					}
				}
			}
			this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13802", "2", this.whoToGreet.Count);
			this.total = this.whoToGreet.Count;
			this.whoToGreet.Remove("Lewis");
			this.whoToGreet.Remove("Robin");
		}

		public override void reloadDescription()
		{
			if (this._questDescription == "")
			{
				this.loadQuestInfo();
			}
			if (this.parts.Count == 0 || this.parts == null)
			{
				return;
			}
			string text = "";
			foreach (DescriptionElement current in this.parts)
			{
				text += current.loadDescriptionElement();
			}
			base.questDescription = text;
		}

		public override void reloadObjective()
		{
			this.loadQuestInfo();
			if (this.objective == null && this.whoToGreet.Count > 0)
			{
				this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13802", this.total - this.whoToGreet.Count, this.total);
			}
			if (this.objective != null)
			{
				base.currentObjective = this.objective.loadDescriptionElement();
			}
		}

		public override bool checkIfComplete(NPC npc = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			this.loadQuestInfo();
			if (npc != null && this.whoToGreet.Remove(npc.name))
			{
				Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f, false)
				{
					scaleChangeChange = -0.012f
				});
			}
			if (this.whoToGreet.Count == 0 && !this.completed)
			{
				foreach (string current in Game1.player.friendships.Keys)
				{
					if (Game1.player.friendships[current][0] < 2729)
					{
						Game1.player.friendships[current][0] += 100;
					}
				}
				base.questComplete();
				return true;
			}
			this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13802", this.total - this.whoToGreet.Count, this.total);
			return false;
		}
	}
}
