using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class Stats
	{
		public uint seedsSown;

		public uint itemsShipped;

		public uint itemsCooked;

		public uint itemsCrafted;

		public uint chickenEggsLayed;

		public uint duckEggsLayed;

		public uint cowMilkProduced;

		public uint goatMilkProduced;

		public uint rabbitWoolProduced;

		public uint sheepWoolProduced;

		public uint cheeseMade;

		public uint goatCheeseMade;

		public uint trufflesFound;

		public uint stoneGathered;

		public uint rocksCrushed;

		public uint dirtHoed;

		public uint giftsGiven;

		public uint timesUnconscious;

		public uint averageBedtime;

		public uint timesFished;

		public uint fishCaught;

		public uint bouldersCracked;

		public uint stumpsChopped;

		public uint stepsTaken;

		public uint monstersKilled;

		public uint diamondsFound;

		public uint prismaticShardsFound;

		public uint otherPreciousGemsFound;

		public uint caveCarrotsFound;

		public uint copperFound;

		public uint ironFound;

		public uint coalFound;

		public uint coinsFound;

		public uint goldFound;

		public uint iridiumFound;

		public uint barsSmelted;

		public uint beveragesMade;

		public uint preservesMade;

		public uint piecesOfTrashRecycled;

		public uint mysticStonesCrushed;

		public uint daysPlayed;

		public uint weedsEliminated;

		public uint sticksChopped;

		public uint notesFound;

		public uint questsCompleted;

		public uint starLevelCropsShipped;

		public uint cropsShipped;

		public uint itemsForaged;

		public uint slimesKilled;

		public uint geodesCracked;

		public uint goodFriends;

		public SerializableDictionary<string, int> specificMonstersKilled = new SerializableDictionary<string, int>();

		public uint GoodFriends
		{
			get
			{
				return this.goodFriends;
			}
			set
			{
				this.goodFriends = value;
			}
		}

		public uint CropsShipped
		{
			get
			{
				return this.cropsShipped;
			}
			set
			{
				this.cropsShipped = value;
			}
		}

		public uint ItemsForaged
		{
			get
			{
				return this.itemsForaged;
			}
			set
			{
				this.itemsForaged = value;
			}
		}

		public uint GeodesCracked
		{
			get
			{
				return this.geodesCracked;
			}
			set
			{
				this.geodesCracked = value;
			}
		}

		public uint SlimesKilled
		{
			get
			{
				return this.slimesKilled;
			}
			set
			{
				this.slimesKilled = value;
			}
		}

		public uint StarLevelCropsShipped
		{
			get
			{
				return this.starLevelCropsShipped;
			}
			set
			{
				this.starLevelCropsShipped = value;
				this.checkForStarCropsAchievements();
			}
		}

		public uint StoneGathered
		{
			get
			{
				return this.stoneGathered;
			}
			set
			{
				this.stoneGathered = value;
				this.checkForStoneAchievements();
			}
		}

		public uint QuestsCompleted
		{
			get
			{
				return this.questsCompleted;
			}
			set
			{
				this.questsCompleted = value;
				this.checkForQuestAchievements();
			}
		}

		public uint FishCaught
		{
			get
			{
				return this.fishCaught;
			}
			set
			{
				this.fishCaught = value;
			}
		}

		public uint NotesFound
		{
			get
			{
				return this.notesFound;
			}
			set
			{
				this.notesFound = value;
			}
		}

		public uint SticksChopped
		{
			get
			{
				return this.sticksChopped;
			}
			set
			{
				this.sticksChopped = value;
				this.checkForWoodAchievements();
			}
		}

		public uint WeedsEliminated
		{
			get
			{
				return this.weedsEliminated;
			}
			set
			{
				this.weedsEliminated = value;
			}
		}

		public uint DaysPlayed
		{
			get
			{
				return this.daysPlayed;
			}
			set
			{
				this.daysPlayed = value;
			}
		}

		public uint BouldersCracked
		{
			get
			{
				return this.bouldersCracked;
			}
			set
			{
				this.bouldersCracked = value;
			}
		}

		public uint MysticStonesCrushed
		{
			get
			{
				return this.mysticStonesCrushed;
			}
			set
			{
				this.mysticStonesCrushed = value;
			}
		}

		public uint GoatCheeseMade
		{
			get
			{
				return this.goatCheeseMade;
			}
			set
			{
				this.goatCheeseMade = value;
				this.checkForCheeseAchievements();
			}
		}

		public uint CheeseMade
		{
			get
			{
				return this.cheeseMade;
			}
			set
			{
				this.cheeseMade = value;
				this.checkForCheeseAchievements();
			}
		}

		public uint PiecesOfTrashRecycled
		{
			get
			{
				return this.piecesOfTrashRecycled;
			}
			set
			{
				this.piecesOfTrashRecycled = value;
			}
		}

		public uint PreservesMade
		{
			get
			{
				return this.preservesMade;
			}
			set
			{
				this.preservesMade = value;
			}
		}

		public uint BeveragesMade
		{
			get
			{
				return this.beveragesMade;
			}
			set
			{
				this.beveragesMade = value;
			}
		}

		public uint BarsSmelted
		{
			get
			{
				return this.barsSmelted;
			}
			set
			{
				this.barsSmelted = value;
			}
		}

		public uint IridiumFound
		{
			get
			{
				return this.iridiumFound;
			}
			set
			{
				this.iridiumFound = value;
				this.checkForIridiumOreAchievements();
			}
		}

		public uint GoldFound
		{
			get
			{
				return this.goldFound;
			}
			set
			{
				this.goldFound = value;
				this.checkForGoldOreAchievements();
			}
		}

		public uint CoinsFound
		{
			get
			{
				return this.coinsFound;
			}
			set
			{
				this.coinsFound = value;
			}
		}

		public uint CoalFound
		{
			get
			{
				return this.coalFound;
			}
			set
			{
				this.coalFound = value;
				this.checkForCoalOreAchievements();
			}
		}

		public uint IronFound
		{
			get
			{
				return this.ironFound;
			}
			set
			{
				this.ironFound = value;
				this.checkForIronOreAchievements();
			}
		}

		public uint CopperFound
		{
			get
			{
				return this.copperFound;
			}
			set
			{
				this.copperFound = value;
				this.checkForCopperOreAchievements();
			}
		}

		public uint CaveCarrotsFound
		{
			get
			{
				return this.caveCarrotsFound;
			}
			set
			{
				this.caveCarrotsFound = value;
			}
		}

		public uint OtherPreciousGemsFound
		{
			get
			{
				return this.otherPreciousGemsFound;
			}
			set
			{
				this.otherPreciousGemsFound = value;
			}
		}

		public uint PrismaticShardsFound
		{
			get
			{
				return this.prismaticShardsFound;
			}
			set
			{
				this.prismaticShardsFound = value;
			}
		}

		public uint DiamondsFound
		{
			get
			{
				return this.diamondsFound;
			}
			set
			{
				this.diamondsFound = value;
			}
		}

		public uint MonstersKilled
		{
			get
			{
				return this.monstersKilled;
			}
			set
			{
				this.monstersKilled = value;
			}
		}

		public uint StepsTaken
		{
			get
			{
				return this.stepsTaken;
			}
			set
			{
				this.stepsTaken = value;
			}
		}

		public uint StumpsChopped
		{
			get
			{
				return this.stumpsChopped;
			}
			set
			{
				this.stumpsChopped = value;
				this.checkForWoodAchievements();
			}
		}

		public uint TimesFished
		{
			get
			{
				return this.timesFished;
			}
			set
			{
				this.timesFished = value;
			}
		}

		public uint AverageBedtime
		{
			get
			{
				return this.averageBedtime;
			}
			set
			{
				this.averageBedtime = (this.averageBedtime * (this.daysPlayed - 1u) + value) / this.daysPlayed;
			}
		}

		public uint TimesUnconscious
		{
			get
			{
				return this.timesUnconscious;
			}
			set
			{
				this.timesUnconscious = value;
			}
		}

		public uint GiftsGiven
		{
			get
			{
				return this.giftsGiven;
			}
			set
			{
				this.giftsGiven = value;
			}
		}

		public uint DirtHoed
		{
			get
			{
				return this.dirtHoed;
			}
			set
			{
				this.dirtHoed = value;
			}
		}

		public uint RocksCrushed
		{
			get
			{
				return this.rocksCrushed;
			}
			set
			{
				this.rocksCrushed = value;
				this.checkForStoneBreakAchievements();
			}
		}

		public uint TrufflesFound
		{
			get
			{
				return this.trufflesFound;
			}
			set
			{
				this.trufflesFound = value;
			}
		}

		public uint SheepWoolProduced
		{
			get
			{
				return this.sheepWoolProduced;
			}
			set
			{
				this.sheepWoolProduced = value;
				this.checkForWoolAchievements();
			}
		}

		public uint RabbitWoolProduced
		{
			get
			{
				return this.rabbitWoolProduced;
			}
			set
			{
				this.rabbitWoolProduced = value;
				this.checkForWoolAchievements();
			}
		}

		public uint GoatMilkProduced
		{
			get
			{
				return this.goatMilkProduced;
			}
			set
			{
				this.goatMilkProduced = value;
				this.checkForGoatMilkAchievements();
			}
		}

		public uint CowMilkProduced
		{
			get
			{
				return this.cowMilkProduced;
			}
			set
			{
				this.cowMilkProduced = value;
				this.checkForCowMilkAchievements();
			}
		}

		public uint DuckEggsLayed
		{
			get
			{
				return this.duckEggsLayed;
			}
			set
			{
				this.duckEggsLayed = value;
				this.checkForDuckEggAchievements();
			}
		}

		public uint ItemsCrafted
		{
			get
			{
				return this.itemsCrafted;
			}
			set
			{
				this.itemsCrafted = value;
				this.checkForCraftingAchievements();
			}
		}

		public uint ChickenEggsLayed
		{
			get
			{
				return this.chickenEggsLayed;
			}
			set
			{
				this.chickenEggsLayed = value;
				this.checkForChickenEggAchievements();
			}
		}

		public uint ItemsCooked
		{
			get
			{
				return this.itemsCooked;
			}
			set
			{
				this.itemsCooked = value;
			}
		}

		public uint ItemsShipped
		{
			get
			{
				return this.itemsShipped;
			}
			set
			{
				this.itemsShipped = value;
			}
		}

		public uint SeedsSown
		{
			get
			{
				return this.seedsSown;
			}
			set
			{
				this.seedsSown = value;
			}
		}

		public void monsterKilled(string name)
		{
			if (this.specificMonstersKilled.ContainsKey(name))
			{
				int num;
				if (!AdventureGuild.willThisKillCompleteAMonsterSlayerQuest(name))
				{
					SerializableDictionary<string, int> expr_63 = this.specificMonstersKilled;
					num = expr_63[name];
					expr_63[name] = num + 1;
					return;
				}
				Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Stats.cs.5129", new object[0]));
				SerializableDictionary<string, int> expr_38 = this.specificMonstersKilled;
				num = expr_38[name];
				expr_38[name] = num + 1;
				if (AdventureGuild.areAllMonsterSlayerQuestsComplete())
				{
					Game1.getSteamAchievement("Achievement_KeeperOfTheMysticRings");
					return;
				}
			}
			else
			{
				this.specificMonstersKilled.Add(name, 1);
			}
		}

		public int getMonstersKilled(string name)
		{
			if (this.specificMonstersKilled.ContainsKey(name))
			{
				return this.specificMonstersKilled[name];
			}
			return 0;
		}

		public void checkForWoodAchievements()
		{
			uint num = this.SticksChopped + this.StumpsChopped * 4u;
			if (num >= 5000u || num < 1500u)
			{
			}
		}

		public void checkForStoneAchievements()
		{
			uint num = this.RocksCrushed + this.BouldersCracked * 4u;
			if (num >= 5000u || num < 1500u)
			{
			}
		}

		public void checkForStoneBreakAchievements()
		{
		}

		public void checkForCookingAchievements()
		{
			Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, string> current in dictionary)
			{
				if (Game1.player.cookingRecipes.ContainsKey(current.Key))
				{
					int key = Convert.ToInt32(current.Value.Split(new char[]
					{
						'/'
					})[2].Split(new char[]
					{
						' '
					})[0]);
					if (Game1.player.recipesCooked.ContainsKey(key))
					{
						num2 += Game1.player.recipesCooked[key];
						num++;
					}
				}
			}
			this.itemsCooked = (uint)num2;
			if (num == dictionary.Count)
			{
				Game1.getAchievement(17);
			}
			if (num >= 25)
			{
				Game1.getAchievement(16);
			}
			if (num >= 10)
			{
				Game1.getAchievement(15);
			}
		}

		public void checkForCraftingAchievements()
		{
			Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CraftingRecipes");
			int num = 0;
			int num2 = 0;
			foreach (string current in dictionary.Keys)
			{
				if (Game1.player.craftingRecipes.ContainsKey(current))
				{
					num2 += Game1.player.craftingRecipes[current];
					if (Game1.player.craftingRecipes[current] > 0)
					{
						num++;
					}
				}
			}
			this.itemsCrafted = (uint)num2;
			if (num >= dictionary.Count)
			{
				Game1.getAchievement(22);
			}
			if (num >= 30)
			{
				Game1.getAchievement(21);
			}
			if (num >= 15)
			{
				Game1.getAchievement(20);
			}
		}

		public void checkForShippingAchievements()
		{
			if (this.farmerShipped(24, 15) && this.farmerShipped(188, 15) && this.farmerShipped(190, 15) && this.farmerShipped(192, 15) && this.farmerShipped(248, 15) && this.farmerShipped(250, 15) && this.farmerShipped(252, 15) && this.farmerShipped(254, 15) && this.farmerShipped(256, 15) && this.farmerShipped(258, 15) && this.farmerShipped(260, 15) && this.farmerShipped(262, 15) && this.farmerShipped(264, 15) && this.farmerShipped(266, 15) && this.farmerShipped(268, 15) && this.farmerShipped(270, 15) && this.farmerShipped(272, 15) && this.farmerShipped(274, 15) && this.farmerShipped(276, 15) && this.farmerShipped(278, 15) && this.farmerShipped(280, 15) && this.farmerShipped(282, 15) && this.farmerShipped(284, 15) && this.farmerShipped(300, 15) && this.farmerShipped(304, 15) && this.farmerShipped(398, 15) && this.farmerShipped(400, 15) && this.farmerShipped(433, 15))
			{
				Game1.getAchievement(31);
			}
			if (this.farmerShipped(24, 300) || this.farmerShipped(188, 300) || this.farmerShipped(190, 300) || this.farmerShipped(192, 300) || this.farmerShipped(248, 300) || this.farmerShipped(250, 300) || this.farmerShipped(252, 300) || this.farmerShipped(254, 300) || this.farmerShipped(256, 300) || this.farmerShipped(258, 300) || this.farmerShipped(260, 300) || this.farmerShipped(262, 300) || this.farmerShipped(264, 300) || this.farmerShipped(266, 300) || this.farmerShipped(268, 300) || this.farmerShipped(270, 300) || this.farmerShipped(272, 300) || this.farmerShipped(274, 300) || this.farmerShipped(276, 300) || this.farmerShipped(278, 300) || this.farmerShipped(280, 300) || this.farmerShipped(282, 300) || this.farmerShipped(284, 300) || this.farmerShipped(454, 300) || this.farmerShipped(300, 300) || this.farmerShipped(304, 300) || (this.farmerShipped(398, 300) | this.farmerShipped(433, 300)) || this.farmerShipped(400, 300) || this.farmerShipped(591, 300) || this.farmerShipped(593, 300) || this.farmerShipped(595, 300) || this.farmerShipped(597, 300))
			{
				Game1.getAchievement(32);
			}
		}

		public void checkForStarCropsAchievements()
		{
			if (this.StarLevelCropsShipped >= 100u)
			{
				Game1.getAchievement(77);
			}
		}

		private bool farmerShipped(int index, int number)
		{
			return Game1.player.basicShipped.ContainsKey(index) && Game1.player.basicShipped[index] >= number;
		}

		public void checkForFishingAchievements()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 59;
			foreach (KeyValuePair<int, string> current in Game1.objectInformation)
			{
				if (current.Value.Split(new char[]
				{
					'/'
				})[3].Contains("Fish") && (current.Key < 167 || current.Key >= 173) && Game1.player.fishCaught.ContainsKey(current.Key))
				{
					num += Game1.player.fishCaught[current.Key][0];
					num2++;
				}
			}
			this.fishCaught = (uint)num;
			if (num >= 100)
			{
				Game1.getAchievement(27);
			}
			if (num2 == num3)
			{
				Game1.getAchievement(26);
				if (!Game1.player.hasOrWillReceiveMail("CF_Fish"))
				{
					Game1.addMailForTomorrow("CF_Fish", false, false);
				}
			}
			if (num2 >= 24)
			{
				Game1.getAchievement(25);
			}
			if (num2 >= 10)
			{
				Game1.getAchievement(24);
			}
		}

		public void checkForChickenEggAchievements()
		{
			if (this.ChickenEggsLayed < 800u && this.ChickenEggsLayed < 350u)
			{
				uint arg_26_0 = this.ChickenEggsLayed;
			}
			uint arg_30_0 = this.chickenEggsLayed;
			uint arg_46_0 = this.chickenEggsLayed + this.duckEggsLayed * 2u;
		}

		public void checkForDuckEggAchievements()
		{
			if (this.DuckEggsLayed < 200u)
			{
				uint arg_16_0 = this.DuckEggsLayed;
			}
			uint arg_2C_0 = this.chickenEggsLayed + this.duckEggsLayed * 2u;
		}

		public void checkForCowMilkAchievements()
		{
			if (this.CowMilkProduced < 500u)
			{
				uint arg_16_0 = this.CowMilkProduced;
			}
			uint arg_20_0 = this.cowMilkProduced;
			uint arg_36_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u;
			uint arg_55_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u + this.GoatMilkProduced * 2u;
		}

		public void checkForGoatMilkAchievements()
		{
			if (this.GoatMilkProduced < 300u)
			{
				uint arg_16_0 = this.GoatMilkProduced;
			}
			uint arg_35_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u + this.GoatMilkProduced * 2u;
		}

		public void checkForWoolAchievements()
		{
			uint num = this.RabbitWoolProduced + this.SheepWoolProduced;
			if (num < 200u)
			{
			}
			uint arg_30_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u;
			uint arg_4F_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u + this.GoatMilkProduced * 2u;
		}

		public void checkForCheeseAchievements()
		{
			uint arg_10_0 = this.GoatCheeseMade + this.CheeseMade;
		}

		public void checkForArchaeologyAchievements()
		{
		}

		public void checkForCopperOreAchievements()
		{
			if (this.CopperFound < 2500u)
			{
				uint arg_19_0 = this.CopperFound;
			}
		}

		public void checkForIronOreAchievements()
		{
			if (this.IronFound < 1000u)
			{
				uint arg_19_0 = this.IronFound;
			}
		}

		public void checkForCoalOreAchievements()
		{
			if (this.CoalFound < 750u)
			{
				uint arg_19_0 = this.CoalFound;
			}
		}

		public void checkForGoldOreAchievements()
		{
			if (this.GoldFound < 500u)
			{
				uint arg_16_0 = this.GoldFound;
			}
		}

		public void checkForIridiumOreAchievements()
		{
			if (this.IridiumFound < 30u)
			{
				uint arg_12_0 = this.IridiumFound;
			}
		}

		public void checkForMoneyAchievements()
		{
			if (Game1.player.totalMoneyEarned >= 10000000u)
			{
				Game1.getAchievement(4);
			}
			if (Game1.player.totalMoneyEarned >= 1000000u)
			{
				Game1.getAchievement(3);
			}
			if (Game1.player.totalMoneyEarned >= 250000u)
			{
				Game1.getAchievement(2);
			}
			if (Game1.player.totalMoneyEarned >= 50000u)
			{
				Game1.getAchievement(1);
			}
			if (Game1.player.totalMoneyEarned >= 15000u)
			{
				Game1.getAchievement(0);
			}
		}

		public void checkForBuildingUpgradeAchievements()
		{
			if (Game1.player.HouseUpgradeLevel == 2)
			{
				Game1.getAchievement(19);
			}
			if (Game1.player.HouseUpgradeLevel == 1)
			{
				Game1.getAchievement(18);
			}
		}

		public void checkForQuestAchievements()
		{
			if (this.QuestsCompleted >= 40u)
			{
				Game1.getAchievement(30);
				Game1.addMailForTomorrow("quest35", false, false);
			}
			if (this.QuestsCompleted >= 10u)
			{
				Game1.getAchievement(29);
				Game1.addMailForTomorrow("quest10", false, false);
			}
		}

		public void checkForFriendshipAchievements()
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			foreach (int[] expr_25 in Game1.player.friendships.Values)
			{
				if (expr_25[0] >= 2500)
				{
					num3 += 1u;
				}
				if (expr_25[0] >= 2000)
				{
					num2 += 1u;
				}
				if (expr_25[0] >= 1250)
				{
					num += 1u;
				}
			}
			this.GoodFriends = num2;
			if (num >= 20u)
			{
				Game1.getAchievement(13);
			}
			if (num >= 10u)
			{
				Game1.getAchievement(12);
			}
			if (num >= 4u)
			{
				Game1.getAchievement(11);
			}
			if (num >= 1u)
			{
				Game1.getAchievement(6);
			}
			if (num3 >= 8u)
			{
				Game1.getAchievement(9);
			}
			if (num3 >= 1u)
			{
				Game1.getAchievement(7);
			}
			Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
			foreach (string current in dictionary.Keys)
			{
				string[] array = dictionary[current].Split(new char[]
				{
					'/'
				})[3].Split(new char[]
				{
					' '
				});
				if (array[0].Equals("f") && Game1.player.friendships.ContainsKey(array[1]) && Game1.player.friendships[array[1]][0] >= Convert.ToInt32(array[2]) * 250 && !Game1.player.cookingRecipes.ContainsKey(current))
				{
					Game1.addMailForTomorrow(array[1] + "Cooking", false, false);
				}
			}
		}
	}
}
