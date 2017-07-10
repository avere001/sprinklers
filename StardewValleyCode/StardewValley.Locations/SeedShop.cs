using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using xTile;

namespace StardewValley.Locations
{
	public class SeedShop : GameLocation
	{
		public const int maxItemsToSellFromPlayer = 11;

		public List<Item> itemsFromPlayerToSell = new List<Item>();

		public List<Item> itemsToStartSellingTomorrow = new List<Item>();

		public SeedShop()
		{
		}

		public SeedShop(Map map, string name) : base(map, name)
		{
		}

		public string getPurchasedItemDialogueForNPC(StardewValley.Object i, NPC n)
		{
			string result = "...";
			string[] array = Game1.content.LoadString("Strings\\Lexicon:GenericPlayerTerm", new object[0]).Split(new char[]
			{
				'^'
			});
			string text = array[0];
			if (array.Length > 1 && !Game1.player.isMale)
			{
				text = array[1];
			}
			string text2 = (Game1.random.NextDouble() < (double)(Game1.player.getFriendshipLevelForNPC(n.name) / 1250)) ? Game1.player.name : text;
			if (n.age != 0)
			{
				text2 = Game1.player.name;
			}
			string text3 = (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en) ? Game1.getProperArticleForWord(i.name) : "";
			if ((i.category == -4 || i.category == -75 || i.category == -79) && Game1.random.NextDouble() < 0.5)
			{
				text3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:SeedShop.cs.9701", new object[0]);
			}
			int num = Game1.random.Next(5);
			if (n.manners == 2)
			{
				num = 2;
			}
			switch (num)
			{
			case 0:
				if (Game1.random.NextDouble() < (double)i.quality * 0.5 + 0.2)
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_1_QualityHigh", new object[]
					{
						text2,
						text3,
						i.DisplayName,
						Lexicon.getRandomDeliciousAdjective(n)
					});
				}
				else
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_1_QualityLow", new object[]
					{
						text2,
						text3,
						i.DisplayName,
						Lexicon.getRandomNegativeFoodAdjective(n)
					});
				}
				break;
			case 1:
				if (i.quality == 0)
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityLow", new object[]
					{
						text2,
						text3,
						i.DisplayName
					});
				}
				else if (n.name.Equals("Jodi"))
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityHigh_Jodi", new object[]
					{
						text2,
						text3,
						i.DisplayName
					});
				}
				else
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityHigh", new object[]
					{
						text2,
						text3,
						i.DisplayName
					});
				}
				break;
			case 2:
				if (n.manners == 2)
				{
					if (i.quality != 2)
					{
						result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_QualityLow_Rude", new object[]
						{
							text2,
							text3,
							i.DisplayName,
							i.salePrice() / 2,
							Lexicon.getRandomNegativeFoodAdjective(n),
							Lexicon.getRandomNegativeItemSlanderNoun()
						});
					}
					else
					{
						Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_QualityHigh_Rude", new object[]
						{
							text2,
							text3,
							i.DisplayName,
							i.salePrice() / 2,
							Lexicon.getRandomSlightlyPositiveAdjectiveForEdibleNoun(n)
						});
					}
				}
				else
				{
					Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_NonRude", new object[]
					{
						text2,
						text3,
						i.DisplayName,
						i.salePrice() / 2
					});
				}
				break;
			case 3:
				result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_4", new object[]
				{
					text2,
					text3,
					i.DisplayName
				});
				break;
			case 4:
				if (i.category == -75 || i.category == -79)
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_VegetableOrFruit", new object[]
					{
						text2,
						text3,
						i.DisplayName
					});
				}
				else if (i.category == -7)
				{
					string randomPositiveAdjectiveForEventOrPerson = Lexicon.getRandomPositiveAdjectiveForEventOrPerson(n);
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_Cooking", new object[]
					{
						text2,
						text3,
						i.DisplayName,
						Game1.getProperArticleForWord(randomPositiveAdjectiveForEventOrPerson),
						randomPositiveAdjectiveForEventOrPerson
					});
				}
				else
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_Foraged", new object[]
					{
						text2,
						text3,
						i.DisplayName
					});
				}
				break;
			}
			if (n.age == 1 && Game1.random.NextDouble() < 0.6)
			{
				result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Teen", new object[]
				{
					text2,
					text3,
					i.DisplayName
				});
			}
			string name = n.name;
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num2 <= 1708213605u)
			{
				if (num2 != 208794864u)
				{
					if (num2 != 786557384u)
					{
						if (num2 == 1708213605u)
						{
							if (name == "Alex")
							{
								result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Alex", new object[]
								{
									text2,
									text3,
									i.DisplayName
								});
							}
						}
					}
					else if (name == "Caroline")
					{
						if (i.quality == 0)
						{
							result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Caroline_QualityLow", new object[]
							{
								text2,
								text3,
								i.DisplayName
							});
						}
						else
						{
							result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Caroline_QualityHigh", new object[]
							{
								text2,
								text3,
								i.DisplayName
							});
						}
					}
				}
				else if (name == "Pierre")
				{
					if (i.quality == 0)
					{
						result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Pierre_QualityLow", new object[]
						{
							text2,
							text3,
							i.DisplayName
						});
					}
					else
					{
						result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Pierre_QualityHigh", new object[]
						{
							text2,
							text3,
							i.DisplayName
						});
					}
				}
			}
			else if (num2 <= 2732913340u)
			{
				if (num2 != 2434294092u)
				{
					if (num2 == 2732913340u)
					{
						if (name == "Abigail")
						{
							if (i.quality == 0)
							{
								result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Abigail_QualityLow", new object[]
								{
									text2,
									text3,
									i.DisplayName,
									Lexicon.getRandomNegativeItemSlanderNoun()
								});
							}
							else
							{
								result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Abigail_QualityHigh", new object[]
								{
									text2,
									text3,
									i.DisplayName
								});
							}
						}
					}
				}
				else if (name == "Haley")
				{
					result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Haley", new object[]
					{
						text2,
						text3,
						i.DisplayName
					});
				}
			}
			else if (num2 != 2826247323u)
			{
				if (num2 == 3066176300u)
				{
					if (name == "Elliott")
					{
						result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Elliott", new object[]
						{
							text2,
							text3,
							i.DisplayName
						});
					}
				}
			}
			else if (name == "Leah")
			{
				result = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Leah", new object[]
				{
					text2,
					text3,
					i.DisplayName
				});
			}
			return result;
		}

		public override void DayUpdate(int dayOfMonth)
		{
			for (int i = this.itemsToStartSellingTomorrow.Count - 1; i >= 0; i--)
			{
				if (this.itemsFromPlayerToSell.Count < 11)
				{
					bool flag = false;
					foreach (Item current in this.itemsFromPlayerToSell)
					{
						if (current.Name.Equals(this.itemsToStartSellingTomorrow[i].Name) && (current as StardewValley.Object).quality == (this.itemsToStartSellingTomorrow[i] as StardewValley.Object).quality)
						{
							Item expr_7F = current;
							int stack = expr_7F.Stack;
							expr_7F.Stack = stack + 1;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.itemsFromPlayerToSell.Add(this.itemsToStartSellingTomorrow[i]);
					}
					this.itemsToStartSellingTomorrow.RemoveAt(i);
				}
			}
			base.DayUpdate(dayOfMonth);
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (Game1.player.maxItems == 12)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(7 * Game1.tileSize + Game1.pixelZoom * 2), (float)(17 * Game1.tileSize))), new Rectangle?(new Rectangle(255, 1436, 12, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 19.25f * (float)Game1.tileSize / 10000f);
				return;
			}
			if (Game1.player.maxItems < 36)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(7 * Game1.tileSize + Game1.pixelZoom * 2), (float)(17 * Game1.tileSize))), new Rectangle?(new Rectangle(267, 1436, 12, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 19.25f * (float)Game1.tileSize / 10000f);
				return;
			}
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Rectangle(7 * Game1.tileSize + Game1.pixelZoom, 18 * Game1.tileSize + Game1.tileSize / 2, Game1.tileSize * 3 / 2 + Game1.pixelZoom * 4, Game1.tileSize / 4 + Game1.pixelZoom)), new Rectangle?(new Rectangle(258, 1449, 1, 1)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 19.25f * (float)Game1.tileSize / 10000f);
		}

		public List<Item> shopStock()
		{
			List<Item> list = new List<Item>();
			if (Game1.currentSeason.Equals("spring"))
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 472, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 473, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 474, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 475, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 427, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 477, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 429, 2147483647));
				if (Game1.year > 1)
				{
					list.Add(new StardewValley.Object(Vector2.Zero, 476, 2147483647));
				}
			}
			if (Game1.currentSeason.Equals("summer"))
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 479, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 480, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 481, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 482, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 483, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 484, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 453, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 455, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 302, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 487, 2147483647));
				list.Add(new StardewValley.Object(431, 2147483647, false, 100, 0));
				if (Game1.year > 1)
				{
					list.Add(new StardewValley.Object(Vector2.Zero, 485, 2147483647));
				}
			}
			if (Game1.currentSeason.Equals("fall"))
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 490, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 487, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 488, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 491, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 492, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 493, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 483, 2147483647));
				list.Add(new StardewValley.Object(431, 2147483647, false, 100, 0));
				list.Add(new StardewValley.Object(Vector2.Zero, 425, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 299, 2147483647));
				list.Add(new StardewValley.Object(Vector2.Zero, 301, 2147483647));
				if (Game1.year > 1)
				{
					list.Add(new StardewValley.Object(Vector2.Zero, 489, 2147483647));
				}
			}
			list.Add(new StardewValley.Object(Vector2.Zero, 297, 2147483647));
			list.Add(new StardewValley.Object(Vector2.Zero, 245, 2147483647));
			list.Add(new StardewValley.Object(Vector2.Zero, 246, 2147483647));
			list.Add(new StardewValley.Object(Vector2.Zero, 423, 2147483647));
			list.Add(new StardewValley.Object(Vector2.Zero, 247, 2147483647));
			list.Add(new StardewValley.Object(Vector2.Zero, 419, 2147483647));
			if (Game1.stats.DaysPlayed >= 15u)
			{
				list.Add(new StardewValley.Object(368, 2147483647, false, 50, 0));
				list.Add(new StardewValley.Object(370, 2147483647, false, 50, 0));
				list.Add(new StardewValley.Object(465, 2147483647, false, 50, 0));
			}
			if (Game1.year > 1)
			{
				list.Add(new StardewValley.Object(369, 2147483647, false, 75, 0));
				list.Add(new StardewValley.Object(371, 2147483647, false, 75, 0));
				list.Add(new StardewValley.Object(466, 2147483647, false, 75, 0));
			}
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			int num = random.Next(112);
			if (num == 21)
			{
				num = 36;
			}
			list.Add(new Wallpaper(num, false)
			{
				stack = 2147483647
			});
			list.Add(new Wallpaper(random.Next(40), true)
			{
				stack = 2147483647
			});
			list.Add(new Furniture(1308, Vector2.Zero)
			{
				stack = 2147483647
			});
			list.Add(new StardewValley.Object(628, 2147483647, false, 1700, 0));
			list.Add(new StardewValley.Object(629, 2147483647, false, 1000, 0));
			list.Add(new StardewValley.Object(630, 2147483647, false, 2000, 0));
			list.Add(new StardewValley.Object(631, 2147483647, false, 3000, 0));
			list.Add(new StardewValley.Object(632, 2147483647, false, 3000, 0));
			list.Add(new StardewValley.Object(633, 2147483647, false, 2000, 0));
			foreach (Item current in this.itemsFromPlayerToSell)
			{
				list.Add(current);
			}
			if (Game1.player.hasAFriendWithHeartLevel(8, true))
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 458, 2147483647));
			}
			return list;
		}
	}
}
