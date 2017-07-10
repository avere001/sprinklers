using System;

namespace StardewValley.BellsAndWhistles
{
	public class Lexicon
	{
		public static string getRandomNegativeItemSlanderNoun()
		{
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeItemNoun", new object[0]).Split(new char[]
			{
				'#'
			});
			return array[random.Next(array.Length)];
		}

		public static string appendArticle(string word)
		{
			return Game1.getProperArticleForWord(word) + " " + word;
		}

		public static string getRandomPositiveAdjectiveForEventOrPerson(NPC n = null)
		{
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] array;
			if (n != null && n.age != 0)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 0)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultMale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 1)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultFemale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_PlaceOrEvent", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return array[random.Next(array.Length)];
		}

		public static string getRandomNegativeAdjectiveForEventOrPerson(NPC n = null)
		{
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] array;
			if (n != null && n.age != 0)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 0)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultMale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 1)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultFemale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_PlaceOrEvent", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return array[random.Next(array.Length)];
		}

		public static string getRandomDeliciousAdjective(NPC n = null)
		{
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] array;
			if (n != null && n.age == 2)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return array[random.Next(array.Length)];
		}

		public static string getRandomNegativeFoodAdjective(NPC n = null)
		{
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] array;
			if (n != null && n.age == 2)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.manners == 1)
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Polite", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				array = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return array[random.Next(array.Length)];
		}

		public static string getRandomSlightlyPositiveAdjectiveForEdibleNoun(NPC n = null)
		{
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] array = Game1.content.LoadString("Strings\\Lexicon:RandomSlightlyPositiveFoodAdjective", new object[0]).Split(new char[]
			{
				'#'
			});
			return array[random.Next(array.Length)];
		}

		public static string getGenderedChildTerm(bool isMale)
		{
			if (isMale)
			{
				return Game1.content.LoadString("Strings\\Lexicon:ChildTerm_Male", new object[0]);
			}
			return Game1.content.LoadString("Strings\\Lexicon:ChildTerm_Female", new object[0]);
		}
	}
}
