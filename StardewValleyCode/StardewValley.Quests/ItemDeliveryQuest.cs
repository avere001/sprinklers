using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
	public class ItemDeliveryQuest : Quest
	{
		public string targetMessage;

		public string target;

		public int item;

		public int number = 1;

		public NPC actualTarget;

		public StardewValley.Object deliveryItem;

		public List<DescriptionElement> parts = new List<DescriptionElement>();

		public List<DescriptionElement> dialogueparts = new List<DescriptionElement>();

		public DescriptionElement objective;

		public ItemDeliveryQuest()
		{
			this.questType = 3;
		}

		public void loadQuestInfo()
		{
			if (this.target != null)
			{
				return;
			}
			base.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13285", new object[0]);
			if (Game1.player.friendships == null || Game1.player.friendships.Count <= 0)
			{
				return;
			}
			this.target = Game1.player.friendships.Keys.ElementAt(this.random.Next(Game1.player.friendships.Count));
			int num = 0;
			this.actualTarget = Game1.getCharacterFromName(this.target, false);
			if (this.actualTarget == null)
			{
				return;
			}
			while (num < 30 && (this.target == null || this.actualTarget == null || this.actualTarget.isInvisible || this.actualTarget.name.Equals(Game1.player.spouse) || this.actualTarget.name.Equals("Krobus") || this.actualTarget.name.Contains("Qi") || this.actualTarget.name.Contains("Dwarf") || this.actualTarget.name.Contains("Gunther") || this.actualTarget.age == 2 || this.actualTarget.name.Contains("Bouncer") || this.actualTarget.name.Contains("Henchman") || this.actualTarget.name.Contains("Marlon") || this.actualTarget.name.Contains("Mariner") || !this.actualTarget.isVillager() || (this.actualTarget.name.Equals("Sandy") && !Game1.player.eventsSeen.Contains(67))))
			{
				num++;
				this.target = Game1.player.friendships.Keys.ElementAt(this.random.Next(Game1.player.friendships.Count));
				this.actualTarget = Game1.getCharacterFromName(this.target, false);
			}
			if (this.actualTarget == null)
			{
				return;
			}
			if (num >= 30 || (this.target.Equals("Wizard") && !Game1.player.mailReceived.Contains("wizardJunimoNote") && !Game1.player.mailReceived.Contains("JojaMember")))
			{
				this.target = "Demetrius";
				this.actualTarget = Game1.getCharacterFromName(this.target, false);
			}
			if (!Game1.currentSeason.Equals("winter") && this.random.NextDouble() < 0.15)
			{
				List<int> list = Utility.possibleCropsAtThisTime(Game1.currentSeason, Game1.dayOfMonth <= 7);
				this.item = list.ElementAt(this.random.Next(list.Count));
				this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.item, 1);
				this.parts.Clear();
				this.parts.Add((this.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13299" : ((this.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13300" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13301"));
				this.parts.Add((this.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13302", this.deliveryItem) : ((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13303", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13304", this.deliveryItem)));
				this.parts.Add((this.random.NextDouble() < 0.25) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13306" : ((this.random.NextDouble() < 0.33) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13307" : ((this.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13308")));
				this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget));
				if (this.target.Equals("Demetrius"))
				{
					this.parts.Clear();
					this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13311", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13314", this.deliveryItem));
				}
				if (this.target.Equals("Marnie"))
				{
					this.parts.Clear();
					this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13317", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13320", this.deliveryItem));
				}
				if (this.target.Equals("Sebastian"))
				{
					this.parts.Clear();
					this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13324", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13327", this.deliveryItem));
				}
			}
			else
			{
				this.item = Utility.getRandomItemFromSeason(Game1.currentSeason, 1000, true);
				if (this.item == -5)
				{
					this.item = 176;
				}
				if (this.item == -6)
				{
					this.item = 184;
				}
				this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.item, 1);
				DescriptionElement[] array = null;
				DescriptionElement[] array2 = null;
				DescriptionElement[] array3 = null;
				if (Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[3].Split(new char[]
				{
					' '
				})[0].Equals("Cooking") && !this.target.Equals("Wizard"))
				{
					if (this.random.NextDouble() < 0.33)
					{
						DescriptionElement[] source = new DescriptionElement[]
						{
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13336",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13337",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13338",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13339",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13340",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13341",
							Game1.samBandName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156", new object[0])) ? ((!Game1.elliottBookName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157", new object[0]))) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13342", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2157")) : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13346") : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13347", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2156")),
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13349",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13350",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13351",
							Game1.currentSeason.Equals("winter") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13353" : (Game1.currentSeason.Equals("summer") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13355" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13356"),
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13357"
						};
						this.parts.Clear();
						this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13333", this.deliveryItem, source.ElementAt(this.random.Next(12))) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13334", this.deliveryItem, source.ElementAt(this.random.Next(12))));
						this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget));
					}
					else
					{
						DescriptionElement param = new DescriptionElement();
						switch (Game1.dayOfMonth % 7)
						{
						case 0:
							param = "Strings\\StringsFromCSFiles:Game1.cs.3042";
							break;
						case 1:
							param = "Strings\\StringsFromCSFiles:Game1.cs.3043";
							break;
						case 2:
							param = "Strings\\StringsFromCSFiles:Game1.cs.3044";
							break;
						case 3:
							param = "Strings\\StringsFromCSFiles:Game1.cs.3045";
							break;
						case 4:
							param = "Strings\\StringsFromCSFiles:Game1.cs.3046";
							break;
						case 5:
							param = "Strings\\StringsFromCSFiles:Game1.cs.3047";
							break;
						case 6:
							param = "Strings\\StringsFromCSFiles:Game1.cs.3048";
							break;
						}
						array = new DescriptionElement[]
						{
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13360", this.deliveryItem),
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13364", this.deliveryItem),
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13367", this.deliveryItem),
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13370", this.deliveryItem),
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13373", param, this.deliveryItem, this.actualTarget)
						};
						array2 = new DescriptionElement[]
						{
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
							new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
							""
						};
						array3 = new DescriptionElement[]
						{
							"",
							"",
							"",
							"",
							""
						};
					}
					this.parts.Clear();
					int num2 = this.random.Next(array.Count<DescriptionElement>());
					this.parts.Add(array[num2]);
					this.parts.Add(array2[num2]);
					this.parts.Add(array3[num2]);
					if (this.target.Equals("Sebastian"))
					{
						this.parts.Clear();
						this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13378", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13381", this.deliveryItem));
					}
				}
				else if (this.random.NextDouble() < 0.5 && Convert.ToInt32(Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[2]) > 0)
				{
					array = new DescriptionElement[]
					{
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13383", this.deliveryItem, new DescriptionElement[]
						{
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13385",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13386",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13387",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13388",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13389",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13390",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13391",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13392",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13393",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13394",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13395",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13396"
						}.ElementAt(this.random.Next(12))),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13400", this.deliveryItem)
					};
					array2 = new DescriptionElement[]
					{
						new DescriptionElement((this.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13398"),
						new DescriptionElement((this.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13402")
					};
					array3 = new DescriptionElement[]
					{
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget)
					};
					if (this.random.NextDouble() < 0.33)
					{
						DescriptionElement[] source2 = new DescriptionElement[]
						{
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13336",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13337",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13338",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13339",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13340",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13341",
							Game1.samBandName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156", new object[0])) ? ((!Game1.elliottBookName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157", new object[0]))) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13342", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2157")) : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13346") : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13347", new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2156")),
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13420",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13421",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13422",
							Game1.currentSeason.Equals("winter") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13424" : (Game1.currentSeason.Equals("summer") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13426" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13427"),
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13357"
						};
						this.parts.Clear();
						this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13333", this.deliveryItem, source2.ElementAt(this.random.Next(12))) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13334", this.deliveryItem, source2.ElementAt(this.random.Next(12))));
						this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget));
					}
					else
					{
						this.parts.Clear();
						int num3 = this.random.Next(array.Count<DescriptionElement>());
						this.parts.Add(array[num3]);
						this.parts.Add(array2[num3]);
						this.parts.Add(array3[num3]);
					}
					if (this.target.Equals("Demetrius"))
					{
						this.parts.Clear();
						this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13311", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13314", this.deliveryItem));
					}
					if (this.target.Equals("Marnie"))
					{
						this.parts.Clear();
						this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13317", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13320", this.deliveryItem));
					}
					if (this.target.Equals("Harvey"))
					{
						DescriptionElement[] source3 = new DescriptionElement[]
						{
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13448",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13449",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13450",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13451",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13452",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13453",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13454",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13455",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13456",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13457",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13458",
							"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13459"
						};
						this.parts.Clear();
						this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13446", this.deliveryItem, source3.ElementAt(this.random.Next(12))));
					}
					if (this.target.Equals("Gus") && this.random.NextDouble() < 0.6)
					{
						this.parts.Clear();
						this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13462", this.deliveryItem));
					}
				}
				else if (this.random.NextDouble() < 0.5 && Convert.ToInt32(Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[2]) < 0)
				{
					this.parts.Clear();
					this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13464", this.deliveryItem, new DescriptionElement[]
					{
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13465",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13466",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13467",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13468",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13469"
					}.ElementAt(this.random.Next(5))));
					this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget.displayName));
					if (this.target.Equals("Emily"))
					{
						this.parts.Clear();
						this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13473", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13476", this.deliveryItem));
					}
				}
				else
				{
					DescriptionElement[] source4 = new DescriptionElement[]
					{
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13502",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13503",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13504",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13505",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13506",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13507",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13508",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13509",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13510",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13511",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13512",
						"Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13513"
					};
					array = new DescriptionElement[]
					{
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13480", this.actualTarget, this.deliveryItem),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13481", this.deliveryItem),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13485", this.deliveryItem),
						(this.random.NextDouble() < 0.4) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13491", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13492", this.deliveryItem),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13494", this.deliveryItem),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13497", this.deliveryItem),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13500", this.deliveryItem, source4.ElementAt(this.random.Next(12))),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13518", this.actualTarget, this.deliveryItem),
						(this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13520", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13523", this.deliveryItem)
					};
					array2 = new DescriptionElement[]
					{
						"",
						(this.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13482" : ((this.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13483"),
						(this.random.NextDouble() < 0.25) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13487" : ((this.random.NextDouble() < 0.33) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13488" : ((this.random.NextDouble() < 0.5) ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13489")),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
						(this.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13514" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13516",
						"",
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget)
					};
					array3 = new DescriptionElement[]
					{
						"",
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
						"",
						"",
						"",
						new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", this.actualTarget),
						"",
						""
					};
					this.parts.Clear();
					int num4 = this.random.Next(array.Count<DescriptionElement>());
					this.parts.Add(array[num4]);
					this.parts.Add(array2[num4]);
					this.parts.Add(array3[num4]);
				}
			}
			this.dialogueparts.Clear();
			this.dialogueparts.Add((this.random.NextDouble() < 0.3 || this.target.Equals("Evelyn")) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13526" : ((this.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13527" : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13528", Game1.player.name)));
			this.dialogueparts.Add((this.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13530", this.deliveryItem) : ((this.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13532" : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13533", (this.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13534") : ((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13535") : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13536")))));
			this.dialogueparts.Add((this.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13538" : ((this.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13539" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13540"));
			this.dialogueparts.Add((this.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13542" : ((this.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13543" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13544"));
			if (this.target.Equals("Wizard"))
			{
				this.parts.Clear();
				if (this.random.NextDouble() < 0.5)
				{
					this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13546", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13548", this.deliveryItem));
				}
				else
				{
					this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13551", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13553", this.deliveryItem));
				}
				this.dialogueparts.Clear();
				this.dialogueparts.Add("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13555");
			}
			if (this.target.Equals("Haley"))
			{
				this.parts.Clear();
				this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13557", this.deliveryItem) : (Game1.player.isMale ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13560", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13563", this.deliveryItem)));
				this.dialogueparts.Clear();
				this.dialogueparts.Add("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13566");
			}
			if (this.target.Equals("Sam"))
			{
				this.parts.Clear();
				this.parts.Add((this.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13568", this.deliveryItem) : (Game1.player.isMale ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13571", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13574", this.deliveryItem)));
				this.dialogueparts.Clear();
				this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13577", Game1.player.name));
			}
			if (this.target.Equals("Maru"))
			{
				this.parts.Clear();
				double num5 = this.random.NextDouble();
				this.parts.Add((num5 < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13580", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13583", this.deliveryItem));
				this.dialogueparts.Clear();
				this.dialogueparts.Add((num5 < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13585", Game1.player.name) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13587", Game1.player.name));
			}
			if (this.target.Equals("Abigail"))
			{
				this.parts.Clear();
				double num6 = this.random.NextDouble();
				this.parts.Add((num6 < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13590", this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13593", this.deliveryItem));
				this.dialogueparts.Add((num6 < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13597", Game1.player.name) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13599", Game1.player.name));
			}
			if (this.target.Equals("Sebastian"))
			{
				this.dialogueparts.Clear();
				this.dialogueparts.Add("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13602");
			}
			if (this.target.Equals("Elliott"))
			{
				this.dialogueparts.Clear();
				this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13604", this.deliveryItem, Game1.player.name));
			}
			DescriptionElement descriptionElement;
			if (this.random.NextDouble() < 0.3)
			{
				descriptionElement = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13608", this.actualTarget);
			}
			else if (this.random.NextDouble() < 0.5)
			{
				descriptionElement = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13610", this.actualTarget);
			}
			else
			{
				descriptionElement = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13612", this.actualTarget);
			}
			this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13607", this.deliveryItem.price * 3));
			this.parts.Add(descriptionElement);
			this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13614", this.actualTarget, this.deliveryItem);
		}

		public override void reloadDescription()
		{
			if (this._questDescription == "" && this.target != null)
			{
				return;
			}
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
			if (item == null || !(item is StardewValley.Object) || n == null || !n.isVillager() || !n.name.Equals(this.target) || ((item as StardewValley.Object).ParentSheetIndex != this.item && (item as StardewValley.Object).Category != this.item))
			{
				return false;
			}
			if (item.Stack >= this.number)
			{
				Game1.player.ActiveObject.Stack -= this.number - 1;
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				Game1.drawDialogue(n);
				Game1.player.reduceActiveItemByOne();
				if (this.dailyQuest)
				{
					Game1.player.changeFriendship(150, n);
					if (this.deliveryItem == null)
					{
						this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.item, 1);
					}
					this.moneyReward = this.deliveryItem.price * 3;
				}
				else
				{
					Game1.player.changeFriendship(255, n);
				}
				base.questComplete();
				return true;
			}
			n.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13615", new object[]
			{
				this.number
			}), n));
			Game1.drawDialogue(n);
			return false;
		}
	}
}
