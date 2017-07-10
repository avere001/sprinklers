using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class LibraryMuseum : GameLocation
	{
		public const int dwarvenGuide = 0;

		public const int totalArtifacts = 95;

		public const int totalNotes = 21;

		public SerializableDictionary<Vector2, int> museumPieces;

		private Dictionary<int, Vector2> lostBooksLocations = new Dictionary<int, Vector2>();

		public LibraryMuseum()
		{
		}

		public LibraryMuseum(Map map, string name) : base(map, name)
		{
			this.museumPieces = new SerializableDictionary<Vector2, int>();
			for (int i = 0; i < map.Layers[0].LayerWidth; i++)
			{
				for (int j = 0; j < map.Layers[0].LayerHeight; j++)
				{
					if (base.doesTileHaveProperty(i, j, "Action", "Buildings") != null && base.doesTileHaveProperty(i, j, "Action", "Buildings").Contains("Notes"))
					{
						this.lostBooksLocations.Add(Convert.ToInt32(base.doesTileHaveProperty(i, j, "Action", "Buildings").Split(new char[]
						{
							' '
						})[1]), new Vector2((float)i, (float)j));
					}
				}
			}
		}

		public bool museumAlreadyHasArtifact(int index)
		{
			foreach (KeyValuePair<Vector2, int> current in this.museumPieces)
			{
				if (current.Value == index)
				{
					return true;
				}
			}
			return false;
		}

		public bool isItemSuitableForDonation(Item i)
		{
			if (i is StardewValley.Object && (i as StardewValley.Object).type != null && ((i as StardewValley.Object).type.Equals("Arch") || (i as StardewValley.Object).type.Equals("Minerals")))
			{
				int parentSheetIndex = (i as StardewValley.Object).parentSheetIndex;
				bool flag = false;
				foreach (KeyValuePair<Vector2, int> current in this.museumPieces)
				{
					if (current.Value == parentSheetIndex)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			return false;
		}

		public bool doesFarmerHaveAnythingToDonate(Farmer who)
		{
			for (int i = 0; i < who.maxItems; i++)
			{
				if (i < who.items.Count && who.items[i] is StardewValley.Object && (who.items[i] as StardewValley.Object).type != null && ((who.items[i] as StardewValley.Object).type.Equals("Arch") || (who.items[i] as StardewValley.Object).type.Equals("Minerals")))
				{
					int parentSheetIndex = (who.items[i] as StardewValley.Object).parentSheetIndex;
					bool flag = false;
					foreach (KeyValuePair<Vector2, int> current in this.museumPieces)
					{
						if (current.Value == parentSheetIndex)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool museumContainsTheseItems(int[] items, HashSet<int> museumItems)
		{
			for (int i = 0; i < items.Length; i++)
			{
				if (!museumItems.Contains(items[i]))
				{
					return false;
				}
			}
			return true;
		}

		private int numberOfMuseumItemsOfType(string type)
		{
			int num = 0;
			foreach (KeyValuePair<Vector2, int> current in this.museumPieces)
			{
				if (Game1.objectInformation[current.Value].Split(new char[]
				{
					'/'
				})[3].Contains(type))
				{
					num++;
				}
			}
			return num;
		}

		public override void resetForPlayerEntry()
		{
			if (!Game1.player.eventsSeen.Contains(0) && this.doesFarmerHaveAnythingToDonate(Game1.player) && !Game1.player.mailReceived.Contains("somethingToDonate"))
			{
				Game1.player.mailReceived.Add("somethingToDonate");
			}
			base.resetForPlayerEntry();
			if (!Game1.isRaining)
			{
				Game1.changeMusicTrack("libraryTheme");
			}
			int num = Game1.player.archaeologyFound.ContainsKey(102) ? Game1.player.archaeologyFound[102][0] : 0;
			for (int i = 0; i < this.lostBooksLocations.Count; i++)
			{
				if (this.lostBooksLocations.ElementAt(i).Key <= num && !Game1.player.mailReceived.Contains("lb_" + this.lostBooksLocations.ElementAt(i).Key))
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 447, 15, 15), new Vector2(this.lostBooksLocations.ElementAt(i).Value.X * (float)Game1.tileSize, this.lostBooksLocations.ElementAt(i).Value.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3 / 2) - 16f), false, 0f, Color.White)
					{
						interval = 99999f,
						animationLength = 1,
						totalNumberOfLoops = 9999,
						yPeriodic = true,
						yPeriodicLoopTime = 4000f,
						yPeriodicRange = (float)(Game1.tileSize / 4),
						layerDepth = 1f,
						scale = (float)Game1.pixelZoom,
						id = (float)this.lostBooksLocations.ElementAt(i).Key
					});
				}
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (!Game1.isRaining)
			{
				Game1.changeMusicTrack("none");
			}
		}

		public List<Item> getRewardsForPlayer(Farmer who)
		{
			List<Item> list = new List<Item>();
			HashSet<int> hashSet = new HashSet<int>(this.museumPieces.Values);
			int num = this.numberOfMuseumItemsOfType("Arch");
			int num2 = this.numberOfMuseumItemsOfType("Minerals");
			int num3 = num + num2;
			if (!who.canUnderstandDwarves && hashSet.Contains(96) && hashSet.Contains(97) && hashSet.Contains(98) && hashSet.Contains(99))
			{
				list.Add(new StardewValley.Object(326, 1, false, -1, 0));
			}
			if (!who.specialBigCraftables.Contains(1305) && hashSet.Contains(113) && num > 4)
			{
				list.Add(new Furniture(1305, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1304) && num >= 15)
			{
				list.Add(new Furniture(1304, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(139) && num >= 20)
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 139, false));
			}
			if (!who.specialBigCraftables.Contains(1545) && this.museumContainsTheseItems(new int[]
			{
				108,
				122
			}, hashSet) && num > 10)
			{
				list.Add(new Furniture(1545, Vector2.Zero));
			}
			if (!who.specialItems.Contains(464) && hashSet.Contains(119) && num > 2)
			{
				list.Add(new StardewValley.Object(464, 1, false, -1, 0));
			}
			if (!who.specialItems.Contains(463) && hashSet.Contains(123) && num > 2)
			{
				list.Add(new StardewValley.Object(463, 1, false, -1, 0));
			}
			if (!who.specialItems.Contains(499) && hashSet.Contains(114))
			{
				list.Add(new StardewValley.Object(499, 1, false, -1, 0));
				list.Add(new StardewValley.Object(499, 1, true, -1, 0));
			}
			if (!who.specialBigCraftables.Contains(1301) && this.museumContainsTheseItems(new int[]
			{
				579,
				581,
				582
			}, hashSet))
			{
				list.Add(new Furniture(1301, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1302) && this.museumContainsTheseItems(new int[]
			{
				583,
				584
			}, hashSet))
			{
				list.Add(new Furniture(1302, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1303) && this.museumContainsTheseItems(new int[]
			{
				580,
				585
			}, hashSet))
			{
				list.Add(new Furniture(1303, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1298) && num2 > 10)
			{
				list.Add(new Furniture(1298, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1299) && num2 > 30)
			{
				list.Add(new Furniture(1299, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(94) && num2 > 20)
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 94, false));
			}
			if (!who.specialBigCraftables.Contains(21) && num2 >= 50)
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 21, false));
			}
			if (!who.specialBigCraftables.Contains(131) && num2 > 40)
			{
				list.Add(new Furniture(131, Vector2.Zero));
			}
			using (List<Item>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.specialItem = true;
				}
			}
			if (!who.mailReceived.Contains("museum5") && num3 >= 5)
			{
				list.Add(new StardewValley.Object(474, 9, false, -1, 0));
			}
			if (!who.mailReceived.Contains("museum10") && num3 >= 10)
			{
				list.Add(new StardewValley.Object(479, 9, false, -1, 0));
			}
			if (!who.mailReceived.Contains("museum15") && num3 >= 15)
			{
				list.Add(new StardewValley.Object(486, 1, false, -1, 0));
			}
			if (!who.mailReceived.Contains("museum20") && num3 >= 20)
			{
				list.Add(new Furniture(1541, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museum25") && num3 >= 25)
			{
				list.Add(new Furniture(1554, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museum30") && num3 >= 30)
			{
				list.Add(new Furniture(1669, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museum40") && num3 >= 40)
			{
				list.Add(new StardewValley.Object(Vector2.Zero, 140, false));
			}
			if (!who.mailReceived.Contains("museum50") && num3 >= 50)
			{
				list.Add(new Furniture(1671, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museumComplete") && num3 >= 95)
			{
				list.Add(new StardewValley.Object(434, 1, false, -1, 0));
			}
			if (num3 >= 60)
			{
				if (!Game1.player.eventsSeen.Contains(295672))
				{
					Game1.player.eventsSeen.Add(295672);
				}
				else if (!Game1.player.hasRustyKey)
				{
					Game1.player.eventsSeen.Remove(66);
				}
			}
			return list;
		}

		public void collectedReward(Item item, Farmer who)
		{
			if (item != null && item is StardewValley.Object)
			{
				(item as StardewValley.Object).specialItem = true;
				int parentSheetIndex = (item as StardewValley.Object).ParentSheetIndex;
				if (parentSheetIndex <= 479)
				{
					if (parentSheetIndex <= 434)
					{
						if (parentSheetIndex == 140)
						{
							who.mailReceived.Add("museum40");
							return;
						}
						if (parentSheetIndex != 434)
						{
							return;
						}
						who.mailReceived.Add("museumComplete");
						return;
					}
					else
					{
						if (parentSheetIndex == 474)
						{
							who.mailReceived.Add("museum5");
							return;
						}
						if (parentSheetIndex != 479)
						{
							return;
						}
						who.mailReceived.Add("museum10");
						return;
					}
				}
				else if (parentSheetIndex <= 1541)
				{
					if (parentSheetIndex == 486)
					{
						who.mailReceived.Add("museum15");
						return;
					}
					if (parentSheetIndex != 1541)
					{
						return;
					}
					who.mailReceived.Add("museum20");
					return;
				}
				else
				{
					if (parentSheetIndex == 1554)
					{
						who.mailReceived.Add("museum25");
						return;
					}
					if (parentSheetIndex == 1669)
					{
						who.mailReceived.Add("museum30");
						return;
					}
					if (parentSheetIndex != 1671)
					{
						return;
					}
					who.mailReceived.Add("museum50");
				}
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			foreach (KeyValuePair<Vector2, int> current in this.museumPieces)
			{
				if (current.Key.X == (float)tileLocation.X && (current.Key.Y == (float)tileLocation.Y || current.Key.Y == (float)(tileLocation.Y - 1)))
				{
					string text = Game1.objectInformation[current.Value].Split(new char[]
					{
						'/'
					})[4];
					Game1.drawObjectDialogue(Game1.parseText(string.Concat(new string[]
					{
						" - ",
						text,
						" - ",
						Environment.NewLine,
						Game1.objectInformation[current.Value].Split(new char[]
						{
							'/'
						})[5]
					})));
					return true;
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public bool isTileSuitableForMuseumPiece(int x, int y)
		{
			Vector2 key = new Vector2((float)x, (float)y);
			if (!this.museumPieces.ContainsKey(key))
			{
				int tileIndexAt = base.getTileIndexAt(new Point(x, y), "Buildings");
				if (tileIndexAt == 1073 || tileIndexAt == 1074 || tileIndexAt == 1072 || tileIndexAt == 1237 || tileIndexAt == 1238)
				{
					return true;
				}
			}
			return false;
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			foreach (TemporaryAnimatedSprite current in this.temporarySprites)
			{
				if (current.layerDepth >= 1f)
				{
					current.draw(b, false, 0, 0);
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			foreach (KeyValuePair<Vector2, int> current in this.museumPieces)
			{
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, current.Key * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize - 12))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (current.Key.Y * (float)Game1.tileSize - 2f) / 10000f);
				b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, current.Key * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, current.Value, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, current.Key.Y * (float)Game1.tileSize / 10000f);
			}
		}
	}
}
