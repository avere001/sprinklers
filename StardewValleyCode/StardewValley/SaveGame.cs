using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StardewValley
{
	public class SaveGame
	{
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			public static readonly SaveGame.<>c <>9 = new SaveGame.<>c();

			public static Action <>9__51_1;

			public static Action <>9__51_2;

			public static Action <>9__51_3;

			internal void <getLoadEnumerator>b__51_1()
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				Game1.loadForNewGame(true);
			}

			internal void <getLoadEnumerator>b__51_2()
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				SaveGame.loadDataToFarmer(SaveGame.loaded.player);
			}

			internal void <getLoadEnumerator>b__51_3()
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				SaveGame.loadDataToLocations(SaveGame.loaded.locations);
			}
		}

		public static XmlSerializer serializer = new XmlSerializer(typeof(SaveGame), new Type[]
		{
			typeof(Tool),
			typeof(GameLocation),
			typeof(Crow),
			typeof(Duggy),
			typeof(Bug),
			typeof(BigSlime),
			typeof(Fireball),
			typeof(Ghost),
			typeof(Child),
			typeof(Pet),
			typeof(Dog),
			typeof(StardewValley.Characters.Cat),
			typeof(Horse),
			typeof(GreenSlime),
			typeof(LavaCrab),
			typeof(RockCrab),
			typeof(ShadowGuy),
			typeof(SkeletonMage),
			typeof(SquidKid),
			typeof(Grub),
			typeof(Fly),
			typeof(DustSpirit),
			typeof(Quest),
			typeof(MetalHead),
			typeof(ShadowGirl),
			typeof(Monster),
			typeof(JunimoHarvester),
			typeof(TerrainFeature)
		});

		public static XmlSerializer farmerSerializer = new XmlSerializer(typeof(Farmer), new Type[]
		{
			typeof(Tool)
		});

		public static XmlSerializer locationSerializer = new XmlSerializer(typeof(GameLocation), new Type[]
		{
			typeof(Tool),
			typeof(Crow),
			typeof(Duggy),
			typeof(Fireball),
			typeof(Ghost),
			typeof(GreenSlime),
			typeof(LavaCrab),
			typeof(RockCrab),
			typeof(ShadowGuy),
			typeof(SkeletonWarrior),
			typeof(Child),
			typeof(Pet),
			typeof(Dog),
			typeof(StardewValley.Characters.Cat),
			typeof(Horse),
			typeof(SquidKid),
			typeof(Grub),
			typeof(Fly),
			typeof(DustSpirit),
			typeof(Bug),
			typeof(BigSlime),
			typeof(BreakableContainer),
			typeof(MetalHead),
			typeof(ShadowGirl),
			typeof(Monster),
			typeof(JunimoHarvester),
			typeof(TerrainFeature)
		});

		public static bool IsProcessing;

		public static bool CancelToTitle;

		public Farmer player;

		public List<GameLocation> locations;

		public string currentSeason;

		public string samBandName;

		public string elliottBookName;

		public List<string> mailbox;

		public int dayOfMonth;

		public int year;

		public int farmerWallpaper;

		public int FarmerFloor;

		public int countdownToWedding;

		public int currentWallpaper;

		public int currentFloor;

		public int currentSongIndex;

		public Point incubatingEgg;

		public double chanceToRainTomorrow;

		public double dailyLuck;

		public ulong uniqueIDForThisGame;

		public bool weddingToday;

		public bool isRaining;

		public bool isDebrisWeather;

		public bool shippingTax;

		public bool bloomDay;

		public bool isLightning;

		public bool isSnowing;

		public bool shouldSpawnMonsters;

		public Stats stats;

		public static SaveGame loaded;

		public float musicVolume;

		public float soundVolume;

		public int[] cropsOfTheWeek;

		public Object dishOfTheDay;

		public long latestID;

		public Options options;

		public SerializableDictionary<int, MineInfo> mine_permanentMineChanges;

		public List<ResourceClump> mine_resourceClumps = new List<ResourceClump>();

		public int mine_mineLevel;

		public int mine_nextLevel;

		public int mine_lowestLevelReached;

		public int minecartHighScore;

		public int weatherForTomorrow;

		public int whichFarm;

		public static IEnumerator<int> Save()
		{
			SaveGame.IsProcessing = true;
			yield return 1;
			IEnumerator<int> loader = SaveGame.getSaveEnumerator();
			Task task = new Task(delegate
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				if (loader != null)
				{
					while (loader.MoveNext() && loader.Current < 100)
					{
					}
					return;
				}
			});
			task.Start();
			while (!task.IsCanceled && !task.IsCompleted && !task.IsFaulted)
			{
				yield return 1;
			}
			SaveGame.IsProcessing = false;
			if (!task.IsFaulted)
			{
				yield return 100;
				yield break;
			}
			Exception baseException = task.Exception.GetBaseException();
			if (baseException is TaskCanceledException)
			{
				Game1.ExitToTitle();
				yield break;
			}
			throw baseException;
		}

		public static IEnumerator<int> getSaveEnumerator()
		{
			return new SaveGame.<getSaveEnumerator>d__48(0);
		}

		public static void ensureFolderStructureExists(string tmpString = "")
		{
			string text = Game1.player.Name;
			string text2 = text;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2[i];
				if (!char.IsLetterOrDigit(c))
				{
					text = text.Replace(c.ToString() ?? "", "");
				}
			}
			string path = string.Concat(new object[]
			{
				text,
				"_",
				Game1.uniqueIDForThisGame,
				tmpString
			});
			FileInfo fileInfo = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), path));
			if (!fileInfo.Directory.Exists)
			{
				fileInfo.Directory.Create();
			}
			fileInfo = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), path), "dummy"));
			if (!fileInfo.Directory.Exists)
			{
				fileInfo.Directory.Create();
			}
		}

		public static void Load(string filename)
		{
			Game1.gameMode = 6;
			Game1.loadingMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:SaveGame.cs.4690", new object[0]);
			Game1.currentLoader = SaveGame.getLoadEnumerator(filename);
		}

		public static IEnumerator<int> getLoadEnumerator(string file)
		{
			SaveGame.<getLoadEnumerator>d__51 expr_06 = new SaveGame.<getLoadEnumerator>d__51(0);
			expr_06.file = file;
			return expr_06;
		}

		public static void loadDataToFarmer(Farmer target)
		{
			target.items = target.items;
			target.canMove = true;
			target.sprite = new FarmerSprite(null);
			target.FarmerSprite.setOwner(target);
			target.reloadLivestockSprites();
			if (target.cookingRecipes == null || target.cookingRecipes.Count == 0)
			{
				target.cookingRecipes.Add("Fried Egg", 0);
			}
			if (target.craftingRecipes == null || target.craftingRecipes.Count == 0)
			{
				target.craftingRecipes.Add("Lumber", 0);
			}
			if (!target.songsHeard.Contains("title_day"))
			{
				target.songsHeard.Add("title_day");
			}
			if (!target.songsHeard.Contains("title_night"))
			{
				target.songsHeard.Add("title_night");
			}
			if (target.addedSpeed > 0)
			{
				target.addedSpeed = 0;
			}
			target.maxItems = target.maxItems;
			for (int i = 0; i < target.maxItems; i++)
			{
				if (target.items.Count <= i)
				{
					target.items.Add(null);
				}
			}
			if (target.FarmerRenderer == null)
			{
				target.FarmerRenderer = new FarmerRenderer(target.getTexture());
			}
			target.changeGender(target.isMale);
			target.changeAccessory(target.accessory);
			target.changeShirt(target.shirt);
			target.changePants(target.pantsColor);
			target.changeSkinColor(target.skin);
			target.changeHairColor(target.hairstyleColor);
			target.changeHairStyle(target.hair);
			if (target.boots != null)
			{
				target.changeShoeColor(target.boots.indexInColorSheet);
			}
			target.Stamina = target.Stamina;
			target.health = target.health;
			target.MaxStamina = target.MaxStamina;
			target.mostRecentBed = target.mostRecentBed;
			target.position = target.mostRecentBed;
			target.position.X = target.position.X - (float)Game1.tileSize;
			target.checkForLevelTenStatus();
			if (!target.craftingRecipes.ContainsKey("Wood Path"))
			{
				target.craftingRecipes.Add("Wood Path", 1);
			}
			if (!target.craftingRecipes.ContainsKey("Gravel Path"))
			{
				target.craftingRecipes.Add("Gravel Path", 1);
			}
			if (!target.craftingRecipes.ContainsKey("Cobblestone Path"))
			{
				target.craftingRecipes.Add("Cobblestone Path", 1);
			}
		}

		public static void loadDataToLocations(List<GameLocation> gamelocations)
		{
			foreach (GameLocation current in gamelocations)
			{
				if (current is FarmHouse)
				{
					GameLocation locationFromName = Game1.getLocationFromName(current.name);
					(Game1.getLocationFromName("FarmHouse") as FarmHouse).upgradeLevel = (current as FarmHouse).upgradeLevel;
					(locationFromName as FarmHouse).upgradeLevel = (current as FarmHouse).upgradeLevel;
					(locationFromName as FarmHouse).setMapForUpgradeLevel((locationFromName as FarmHouse).upgradeLevel, true);
					(locationFromName as FarmHouse).wallPaper = (current as FarmHouse).wallPaper;
					(locationFromName as FarmHouse).floor = (current as FarmHouse).floor;
					(locationFromName as FarmHouse).furniture = (current as FarmHouse).furniture;
					(locationFromName as FarmHouse).fireplaceOn = (current as FarmHouse).fireplaceOn;
					(locationFromName as FarmHouse).fridge = (current as FarmHouse).fridge;
					(locationFromName as FarmHouse).farmerNumberOfOwner = (current as FarmHouse).farmerNumberOfOwner;
					(locationFromName as FarmHouse).resetForPlayerEntry();
					using (List<Furniture>.Enumerator enumerator2 = (locationFromName as FarmHouse).furniture.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.Current.updateDrawPosition();
						}
					}
					locationFromName.lastTouchActionLocation = Game1.player.getTileLocation();
				}
				if (current.name.Equals("Farm"))
				{
					GameLocation locationFromName2 = Game1.getLocationFromName(current.name);
					using (List<Building>.Enumerator enumerator3 = ((Farm)current).buildings.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							enumerator3.Current.load();
						}
					}
					((Farm)locationFromName2).buildings = ((Farm)current).buildings;
					using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator4 = ((Farm)current).animals.Values.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							enumerator4.Current.reload();
						}
					}
				}
			}
			foreach (GameLocation current2 in gamelocations)
			{
				GameLocation locationFromName3 = Game1.getLocationFromName(current2.name);
				current2.name.Equals("Farm");
				for (int i = current2.characters.Count - 1; i >= 0; i--)
				{
					if (!current2.characters[i].DefaultPosition.Equals(Vector2.Zero))
					{
						current2.characters[i].position = current2.characters[i].DefaultPosition;
					}
					current2.characters[i].currentLocation = locationFromName3;
					if (i < current2.characters.Count)
					{
						current2.characters[i].reloadSprite();
					}
				}
				using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator5 = current2.terrainFeatures.Values.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						enumerator5.Current.loadSprite();
					}
				}
				foreach (KeyValuePair<Vector2, Object> current3 in current2.objects)
				{
					current3.Value.initializeLightSource(current3.Key);
					current3.Value.reloadSprite();
				}
				if (current2.name.Equals("Farm"))
				{
					((Farm)locationFromName3).buildings = ((Farm)current2).buildings;
					using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator4 = ((Farm)current2).animals.Values.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							enumerator4.Current.reload();
						}
					}
					foreach (Building current4 in Game1.getFarm().buildings)
					{
						Vector2 tile = new Vector2((float)current4.tileX, (float)current4.tileY);
						if (current4.indoors is Shed)
						{
							(current4.indoors as Shed).furniture = ((current2 as Farm).getBuildingAt(tile).indoors as Shed).furniture;
							(current4.indoors as Shed).wallPaper = ((current2 as Farm).getBuildingAt(tile).indoors as Shed).wallPaper;
							(current4.indoors as Shed).floor = ((current2 as Farm).getBuildingAt(tile).indoors as Shed).floor;
						}
						current4.load();
						if (current4.indoors is Shed)
						{
							(current4.indoors as Shed).furniture = ((current2 as Farm).getBuildingAt(tile).indoors as Shed).furniture;
							(current4.indoors as Shed).wallPaper = ((current2 as Farm).getBuildingAt(tile).indoors as Shed).wallPaper;
							(current4.indoors as Shed).floor = ((current2 as Farm).getBuildingAt(tile).indoors as Shed).floor;
						}
					}
				}
				if (locationFromName3 != null)
				{
					locationFromName3.characters = current2.characters;
					locationFromName3.objects = current2.objects;
					locationFromName3.numberOfSpawnedObjectsOnMap = current2.numberOfSpawnedObjectsOnMap;
					locationFromName3.terrainFeatures = current2.terrainFeatures;
					locationFromName3.largeTerrainFeatures = current2.largeTerrainFeatures;
					if (locationFromName3.name.Equals("Farm"))
					{
						((Farm)locationFromName3).animals = ((Farm)current2).animals;
						(locationFromName3 as Farm).piecesOfHay = (current2 as Farm).piecesOfHay;
						(locationFromName3 as Farm).resourceClumps = (current2 as Farm).resourceClumps;
						(locationFromName3 as Farm).hasSeenGrandpaNote = (current2 as Farm).hasSeenGrandpaNote;
						(locationFromName3 as Farm).grandpaScore = (current2 as Farm).grandpaScore;
					}
					if (locationFromName3 is Sewer)
					{
						(locationFromName3 as Sewer).populateShopStock(Game1.dayOfMonth);
					}
					if (locationFromName3 is Beach)
					{
						(locationFromName3 as Beach).bridgeFixed = (current2 as Beach).bridgeFixed;
					}
					if (locationFromName3 is Woods)
					{
						(locationFromName3 as Woods).stumps = (current2 as Woods).stumps;
						(locationFromName3 as Woods).hasFoundStardrop = (current2 as Woods).hasFoundStardrop;
						(locationFromName3 as Woods).hasUnlockedStatue = (current2 as Woods).hasUnlockedStatue;
					}
					if (locationFromName3 is LibraryMuseum)
					{
						(locationFromName3 as LibraryMuseum).museumPieces = (current2 as LibraryMuseum).museumPieces;
					}
					if (locationFromName3 is CommunityCenter)
					{
						(locationFromName3 as CommunityCenter).bundleRewards = (current2 as CommunityCenter).bundleRewards;
						(locationFromName3 as CommunityCenter).bundles = (current2 as CommunityCenter).bundles;
						(locationFromName3 as CommunityCenter).areasComplete = (current2 as CommunityCenter).areasComplete;
					}
					if (locationFromName3 is SeedShop)
					{
						(locationFromName3 as SeedShop).itemsFromPlayerToSell = (current2 as SeedShop).itemsFromPlayerToSell;
						(locationFromName3 as SeedShop).itemsToStartSellingTomorrow = (current2 as SeedShop).itemsToStartSellingTomorrow;
					}
					if (locationFromName3 is Forest)
					{
						if (Game1.dayOfMonth % 7 % 5 == 0)
						{
							(locationFromName3 as Forest).travelingMerchantDay = true;
							(locationFromName3 as Forest).travelingMerchantBounds = new List<Rectangle>();
							(locationFromName3 as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize, 10 * Game1.tileSize, 123 * Game1.pixelZoom, 28 * Game1.pixelZoom));
							(locationFromName3 as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize + 45 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 19 * Game1.pixelZoom, 12 * Game1.pixelZoom));
							(locationFromName3 as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize + 85 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 26 * Game1.pixelZoom, 12 * Game1.pixelZoom));
							(locationFromName3 as Forest).travelingMerchantStock = Utility.getTravelingMerchantStock();
							using (List<Rectangle>.Enumerator enumerator7 = (locationFromName3 as Forest).travelingMerchantBounds.GetEnumerator())
							{
								while (enumerator7.MoveNext())
								{
									Utility.clearObjectsInArea(enumerator7.Current, locationFromName3);
								}
							}
						}
						(locationFromName3 as Forest).log = (current2 as Forest).log;
					}
				}
			}
			Game1.player.currentLocation = Utility.getHomeOfFarmer(Game1.player);
		}
	}
}
