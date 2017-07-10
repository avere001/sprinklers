using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley.Quests
{
	public class WeedingQuest : Quest
	{
		public NPC target;

		public string targetMessage;

		public bool complete;

		public int totalWeeds;

		public List<DescriptionElement> parts = new List<DescriptionElement>();

		public DescriptionElement dialogue = new DescriptionElement();

		public DescriptionElement objective;

		public WeedingQuest()
		{
			this.questType = 11;
		}

		public void loadQuestInfo()
		{
			GameLocation locationFromName = Game1.getLocationFromName("Town");
			for (int i = 0; i < 10; i++)
			{
				locationFromName.spawnWeeds(true);
			}
			this.target = Game1.getCharacterFromName("Lewis", false);
			this.parts.Clear();
			this.parts.Add("Strings\\StringsFromCSFiles:WeedingQuest.cs.13816");
			this.parts.Add("Strings\\StringsFromCSFiles:WeedingQuest.cs.13817");
			this.parts.Add("Strings\\StringsFromCSFiles:SocializeQuest.cs.13791");
			this.dialogue = "Strings\\StringsFromCSFiles:WeedingQuest.cs.13819";
			base.currentObjective = "";
		}

		public override void accept()
		{
			base.accept();
			using (Dictionary<Vector2, StardewValley.Object>.ValueCollection.Enumerator enumerator = Game1.getLocationFromName("Town").Objects.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name.Contains("Weed"))
					{
						this.totalWeeds++;
					}
				}
			}
			this.checkIfComplete(null, -1, -1, null, null);
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
			this.targetMessage = this.dialogue.loadDescriptionElement();
		}

		private int weedsLeft()
		{
			int num = 0;
			using (Dictionary<Vector2, StardewValley.Object>.ValueCollection.Enumerator enumerator = Game1.getLocationFromName("Town").Objects.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name.Contains("Weed"))
					{
						num++;
					}
				}
			}
			return num;
		}

		public override void reloadObjective()
		{
			if (this.weedsLeft() > 0)
			{
				this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:WeedingQuest.cs.13826", this.totalWeeds - this.weedsLeft(), this.totalWeeds);
			}
			if (this.objective != null)
			{
				base.currentObjective = this.objective.loadDescriptionElement();
			}
		}

		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			if (n == null && !this.complete)
			{
				if (this.weedsLeft() == 0)
				{
					this.complete = true;
					this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:WeedingQuest.cs.13824");
					Game1.playSound("jingle1");
				}
				if (Game1.currentLocation.Name.Equals("Town"))
				{
					Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(220f, 260f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f, false)
					{
						scaleChangeChange = -0.015f
					});
				}
			}
			else if (n != null && n.Equals(this.target) && this.complete)
			{
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				this.completed = true;
				Game1.player.Money += 300;
				foreach (string current in Game1.player.friendships.Keys)
				{
					if (Game1.player.friendships[current][0] < 2729)
					{
						Game1.player.friendships[current][0] += 20;
					}
				}
				base.questComplete();
				return true;
			}
			return false;
		}
	}
}
