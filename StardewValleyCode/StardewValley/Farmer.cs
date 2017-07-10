using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	public class Farmer : Character, IComparable
	{
		public const int millisecondsPerSpeedUnit = 64;

		public const byte halt = 64;

		public const byte up = 1;

		public const byte right = 2;

		public const byte down = 4;

		public const byte left = 8;

		public const byte run = 16;

		public const byte release = 32;

		public const int FESTIVAL_WINNER = -9999;

		public const int farmingSkill = 0;

		public const int miningSkill = 3;

		public const int fishingSkill = 1;

		public const int foragingSkill = 2;

		public const int combatSkill = 4;

		public const int luckSkill = 5;

		public const float interpolationConstant = 0.5f;

		public const int runningSpeed = 5;

		public const int walkingSpeed = 2;

		public const int caveNothing = 0;

		public const int caveBats = 1;

		public const int caveMushrooms = 2;

		public const int millisecondsInvincibleAfterDamage = 1200;

		public const int millisecondsPerFlickerWhenInvincible = 50;

		public const int startingStamina = 270;

		public const int totalLevels = 35;

		public static int tileSlideThreshold = Game1.tileSize / 2;

		public const int maxInventorySpace = 36;

		public const int hotbarSize = 12;

		public const int eyesOpen = 0;

		public const int eyesHalfShut = 4;

		public const int eyesClosed = 1;

		public const int eyesRight = 2;

		public const int eyesLeft = 3;

		public const int eyesWide = 5;

		public const int rancher = 0;

		public const int tiller = 1;

		public const int butcher = 2;

		public const int shepherd = 3;

		public const int artisan = 4;

		public const int agriculturist = 5;

		public const int fisher = 6;

		public const int trapper = 7;

		public const int angler = 8;

		public const int pirate = 9;

		public const int baitmaster = 10;

		public const int mariner = 11;

		public const int forester = 12;

		public const int gatherer = 13;

		public const int lumberjack = 14;

		public const int tapper = 15;

		public const int botanist = 16;

		public const int tracker = 17;

		public const int miner = 18;

		public const int geologist = 19;

		public const int blacksmith = 20;

		public const int burrower = 21;

		public const int excavator = 22;

		public const int gemologist = 23;

		public const int fighter = 24;

		public const int scout = 25;

		public const int brute = 26;

		public const int defender = 27;

		public const int acrobat = 28;

		public const int desperado = 29;

		public List<Quest> questLog = new List<Quest>();

		public List<int> professions = new List<int>();

		public List<Point> newLevels = new List<Point>();

		private Queue<int> newLevelSparklingTexts = new Queue<int>();

		private SparklingText sparklingText;

		public int[] experiencePoints = new int[6];

		[XmlIgnore]
		private Item activeObject;

		public List<Item> items;

		public List<int> dialogueQuestionsAnswered = new List<int>();

		public List<string> furnitureOwned = new List<string>();

		public SerializableDictionary<string, int> cookingRecipes = new SerializableDictionary<string, int>();

		public SerializableDictionary<string, int> craftingRecipes = new SerializableDictionary<string, int>();

		public SerializableDictionary<string, int> activeDialogueEvents = new SerializableDictionary<string, int>();

		public List<int> eventsSeen = new List<int>();

		public List<string> songsHeard = new List<string>();

		public List<int> achievements = new List<int>();

		public List<int> specialItems = new List<int>();

		public List<int> specialBigCraftables = new List<int>();

		public List<string> mailReceived = new List<string>();

		public List<string> mailForTomorrow = new List<string>();

		public List<string> blueprints = new List<string>();

		public List<CoopDweller> coopDwellers = new List<CoopDweller>();

		public List<BarnDweller> barnDwellers = new List<BarnDweller>();

		public Tool[] toolBox = new Tool[30];

		public Object[] cupboard = new Object[30];

		[XmlIgnore]
		public List<int> movementDirections = new List<int>();

		public string farmName = "";

		public string favoriteThing = "";

		[XmlIgnore]
		public List<Buff> buffs = new List<Buff>();

		[XmlIgnore]
		public List<object[]> multiplayerMessage = new List<object[]>();

		[XmlIgnore]
		public GameLocation currentLocation = Game1.getLocationFromName("FarmHouse");

		[XmlIgnore]
		public long uniqueMultiplayerID = -6666666L;

		[XmlIgnore]
		public string _tmpLocationName = "FarmHouse";

		[XmlIgnore]
		public string previousLocationName = "";

		public bool catPerson = true;

		[XmlIgnore]
		public Item mostRecentlyGrabbedItem;

		[XmlIgnore]
		public Item itemToEat;

		private FarmerRenderer farmerRenderer;

		[XmlIgnore]
		public int toolPower;

		[XmlIgnore]
		public int toolHold;

		public Vector2 mostRecentBed;

		public int shirt;

		public int hair;

		public int skin;

		public int accessory = -1;

		public int facialHair = -1;

		[XmlIgnore]
		public int currentEyes;

		[XmlIgnore]
		public int blinkTimer;

		[XmlIgnore]
		public int festivalScore;

		[XmlIgnore]
		public float temporarySpeedBuff;

		public Color hairstyleColor;

		public Color pantsColor;

		public Color newEyeColor;

		public Hat hat;

		public Boots boots;

		public Ring leftRing;

		public Ring rightRing;

		[XmlIgnore]
		public NPC dancePartner;

		[XmlIgnore]
		public bool ridingMineElevator;

		[XmlIgnore]
		public bool mineMovementDirectionWasUp;

		[XmlIgnore]
		public bool cameFromDungeon;

		[XmlIgnore]
		public bool readyConfirmation;

		[XmlIgnore]
		public bool exhausted;

		[XmlIgnore]
		public bool divorceTonight;

		[XmlIgnore]
		public AnimatedSprite.endOfAnimationBehavior toolOverrideFunction;

		public int deepestMineLevel;

		private int currentToolIndex;

		public int woodPieces;

		public int stonePieces;

		public int copperPieces;

		public int ironPieces;

		public int coalPieces;

		public int goldPieces;

		public int iridiumPieces;

		public int quartzPieces;

		public int caveChoice;

		public int feed;

		public int farmingLevel;

		public int miningLevel;

		public int combatLevel;

		public int foragingLevel;

		public int fishingLevel;

		public int luckLevel;

		public int newSkillPointsToSpend;

		public int addedFarmingLevel;

		public int addedMiningLevel;

		public int addedCombatLevel;

		public int addedForagingLevel;

		public int addedFishingLevel;

		public int addedLuckLevel;

		public int maxStamina = 270;

		public int maxItems = 12;

		public float stamina = 270f;

		public int resilience;

		public int attack;

		public int immunity;

		public float attackIncreaseModifier;

		public float knockbackModifier;

		public float weaponSpeedModifier;

		public float critChanceModifier;

		public float critPowerModifier;

		public float weaponPrecisionModifier;

		public int money = 500;

		public int clubCoins;

		public uint totalMoneyEarned;

		public uint millisecondsPlayed;

		public Tool toolBeingUpgraded;

		public int daysLeftForToolUpgrade;

		private float timeOfLastPositionPacket;

		private int numUpdatesSinceLastDraw;

		public int houseUpgradeLevel;

		public int daysUntilHouseUpgrade = -1;

		public int coopUpgradeLevel;

		public int barnUpgradeLevel;

		public bool hasGreenhouse;

		public bool hasRustyKey;

		public bool hasSkullKey;

		public bool hasUnlockedSkullDoor;

		public bool hasDarkTalisman;

		public bool hasMagicInk;

		public bool showChestColorPicker = true;

		public int magneticRadius = Game1.tileSize * 2;

		public int temporaryInvincibilityTimer;

		[XmlIgnore]
		public float rotation;

		private int craftingTime = 1000;

		private int raftPuddleCounter = 250;

		private int raftBobCounter = 1000;

		public int health = 100;

		public int maxHealth = 100;

		public int timesReachedMineBottom;

		[XmlIgnore]
		public Vector2 jitter = Vector2.Zero;

		[XmlIgnore]
		public Vector2 lastPosition;

		[XmlIgnore]
		public Vector2 lastGrabTile = Vector2.Zero;

		[XmlIgnore]
		public float jitterStrength;

		[XmlIgnore]
		public float xOffset;

		public bool isMale = true;

		[XmlIgnore]
		public bool canMove = true;

		[XmlIgnore]
		public bool running;

		[XmlIgnore]
		public bool usingTool;

		[XmlIgnore]
		public bool forceTimePass;

		[XmlIgnore]
		public bool isRafting;

		[XmlIgnore]
		public bool usingSlingshot;

		[XmlIgnore]
		public bool bathingClothes;

		[XmlIgnore]
		public bool canOnlyWalk;

		[XmlIgnore]
		public bool temporarilyInvincible;

		public bool hasBusTicket;

		public bool stardewHero;

		public bool hasClubCard;

		public bool hasSpecialCharm;

		[XmlIgnore]
		public bool canReleaseTool;

		[XmlIgnore]
		public bool isCrafting;

		[XmlIgnore]
		public Microsoft.Xna.Framework.Rectangle temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;

		public bool canUnderstandDwarves;

		public SerializableDictionary<int, int> basicShipped;

		public SerializableDictionary<int, int> mineralsFound;

		public SerializableDictionary<int, int> recipesCooked;

		public SerializableDictionary<int, int[]> archaeologyFound;

		public SerializableDictionary<int, int[]> fishCaught;

		public SerializableDictionary<string, int[]> friendships;

		[XmlIgnore]
		public Vector2 positionBeforeEvent;

		[XmlIgnore]
		public Vector2 remotePosition;

		[XmlIgnore]
		public int orientationBeforeEvent;

		[XmlIgnore]
		public int swimTimer;

		[XmlIgnore]
		public int timerSinceLastMovement;

		[XmlIgnore]
		public int noMovementPause;

		[XmlIgnore]
		public int freezePause;

		[XmlIgnore]
		public float yOffset;

		public BuildingUpgrade currentUpgrade;

		public string spouse;

		public string dateStringForSaveGame;

		public int? dayOfMonthForSaveGame;

		public int? seasonForSaveGame;

		public int? yearForSaveGame;

		public int overallsColor;

		public int shirtColor;

		public int skinColor;

		public int hairColor;

		public int eyeColor;

		[XmlIgnore]
		public Vector2 armOffset;

		public string bobber = "";

		private Horse mount;

		private LocalizedContentManager farmerTextureManager;

		public int saveTime;

		public int daysMarried;

		private int toolPitchAccumulator;

		private int charactercollisionTimer;

		private NPC collisionNPC;

		public float movementMultiplier = 0.01f;

		[XmlIgnore]
		public int MaxItems
		{
			get
			{
				return this.maxItems;
			}
			set
			{
				this.maxItems = value;
			}
		}

		[XmlIgnore]
		public int Level
		{
			get
			{
				return (this.farmingLevel + this.fishingLevel + this.foragingLevel + this.combatLevel + this.miningLevel + this.luckLevel) / 2;
			}
		}

		[XmlIgnore]
		public int CraftingTime
		{
			get
			{
				return this.craftingTime;
			}
			set
			{
				this.craftingTime = value;
			}
		}

		[XmlIgnore]
		public int NewSkillPointsToSpend
		{
			get
			{
				return this.newSkillPointsToSpend;
			}
			set
			{
				this.newSkillPointsToSpend = value;
			}
		}

		[XmlIgnore]
		public int FarmingLevel
		{
			get
			{
				return this.farmingLevel + this.addedFarmingLevel;
			}
			set
			{
				this.farmingLevel = value;
			}
		}

		[XmlIgnore]
		public int MiningLevel
		{
			get
			{
				return this.miningLevel + this.addedMiningLevel;
			}
			set
			{
				this.miningLevel = value;
			}
		}

		[XmlIgnore]
		public int CombatLevel
		{
			get
			{
				return this.combatLevel + this.addedCombatLevel;
			}
			set
			{
				this.combatLevel = value;
			}
		}

		[XmlIgnore]
		public int ForagingLevel
		{
			get
			{
				return this.foragingLevel + this.addedForagingLevel;
			}
			set
			{
				this.foragingLevel = value;
			}
		}

		[XmlIgnore]
		public int FishingLevel
		{
			get
			{
				return this.fishingLevel + this.addedFishingLevel;
			}
			set
			{
				this.fishingLevel = value;
			}
		}

		[XmlIgnore]
		public int LuckLevel
		{
			get
			{
				return this.luckLevel + this.addedLuckLevel;
			}
			set
			{
				this.luckLevel = value;
			}
		}

		[XmlIgnore]
		public int HouseUpgradeLevel
		{
			get
			{
				return this.houseUpgradeLevel;
			}
			set
			{
				this.houseUpgradeLevel = value;
			}
		}

		[XmlIgnore]
		public int CoopUpgradeLevel
		{
			get
			{
				return this.coopUpgradeLevel;
			}
			set
			{
				this.coopUpgradeLevel = value;
			}
		}

		[XmlIgnore]
		public int BarnUpgradeLevel
		{
			get
			{
				return this.barnUpgradeLevel;
			}
			set
			{
				this.barnUpgradeLevel = value;
			}
		}

		[XmlIgnore]
		public Microsoft.Xna.Framework.Rectangle TemporaryImpassableTile
		{
			get
			{
				return this.temporaryImpassableTile;
			}
			set
			{
				this.temporaryImpassableTile = value;
			}
		}

		[XmlIgnore]
		public List<Item> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
			}
		}

		[XmlIgnore]
		public int MagneticRadius
		{
			get
			{
				return this.magneticRadius;
			}
			set
			{
				this.magneticRadius = value;
			}
		}

		[XmlIgnore]
		public Object ActiveObject
		{
			get
			{
				if (this.currentToolIndex < this.items.Count && this.items[this.currentToolIndex] != null && this.items[this.currentToolIndex] is Object)
				{
					return (Object)this.items[this.currentToolIndex];
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.removeItemFromInventory(this.ActiveObject);
					return;
				}
				this.addItemToInventory(value, this.CurrentToolIndex);
			}
		}

		[XmlIgnore]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		[XmlIgnore]
		public bool IsMale
		{
			get
			{
				return this.isMale;
			}
			set
			{
				this.isMale = value;
			}
		}

		[XmlIgnore]
		public List<int> DialogueQuestionsAnswered
		{
			get
			{
				return this.dialogueQuestionsAnswered;
			}
			set
			{
				this.dialogueQuestionsAnswered = value;
			}
		}

		[XmlIgnore]
		public int WoodPieces
		{
			get
			{
				return this.woodPieces;
			}
			set
			{
				this.woodPieces = value;
			}
		}

		[XmlIgnore]
		public int StonePieces
		{
			get
			{
				return this.stonePieces;
			}
			set
			{
				this.stonePieces = value;
			}
		}

		[XmlIgnore]
		public int CopperPieces
		{
			get
			{
				return this.copperPieces;
			}
			set
			{
				this.copperPieces = value;
			}
		}

		[XmlIgnore]
		public int IronPieces
		{
			get
			{
				return this.ironPieces;
			}
			set
			{
				this.ironPieces = value;
			}
		}

		[XmlIgnore]
		public int CoalPieces
		{
			get
			{
				return this.coalPieces;
			}
			set
			{
				this.coalPieces = value;
			}
		}

		[XmlIgnore]
		public int GoldPieces
		{
			get
			{
				return this.goldPieces;
			}
			set
			{
				this.goldPieces = value;
			}
		}

		[XmlIgnore]
		public int IridiumPieces
		{
			get
			{
				return this.iridiumPieces;
			}
			set
			{
				this.iridiumPieces = value;
			}
		}

		[XmlIgnore]
		public int QuartzPieces
		{
			get
			{
				return this.quartzPieces;
			}
			set
			{
				this.quartzPieces = value;
			}
		}

		[XmlIgnore]
		public int Feed
		{
			get
			{
				return this.feed;
			}
			set
			{
				this.feed = value;
			}
		}

		[XmlIgnore]
		public bool CanMove
		{
			get
			{
				return this.canMove;
			}
			set
			{
				this.canMove = value;
			}
		}

		[XmlIgnore]
		public bool UsingTool
		{
			get
			{
				return this.usingTool;
			}
			set
			{
				this.usingTool = value;
			}
		}

		[XmlIgnore]
		public Tool CurrentTool
		{
			get
			{
				if (this.CurrentItem != null && this.CurrentItem is Tool)
				{
					return (Tool)this.CurrentItem;
				}
				return null;
			}
			set
			{
				this.items[this.CurrentToolIndex] = value;
			}
		}

		[XmlIgnore]
		public Item CurrentItem
		{
			get
			{
				if (this.currentToolIndex >= this.items.Count)
				{
					return null;
				}
				return this.items[this.currentToolIndex];
			}
		}

		[XmlIgnore]
		public int CurrentToolIndex
		{
			get
			{
				return this.currentToolIndex;
			}
			set
			{
				if (this.currentToolIndex >= 0 && this.CurrentItem != null && value != this.currentToolIndex)
				{
					this.CurrentItem.actionWhenStopBeingHeld(this);
				}
				this.currentToolIndex = value;
			}
		}

		[XmlIgnore]
		public float Stamina
		{
			get
			{
				return this.stamina;
			}
			set
			{
				this.stamina = Math.Min((float)this.maxStamina, Math.Max(value, -16f));
			}
		}

		[XmlIgnore]
		public int MaxStamina
		{
			get
			{
				return this.maxStamina;
			}
			set
			{
				this.maxStamina = value;
			}
		}

		[XmlIgnore]
		public bool IsMainPlayer
		{
			get
			{
				return this.uniqueMultiplayerID == Game1.player.uniqueMultiplayerID;
			}
		}

		[XmlIgnore]
		public FarmerSprite FarmerSprite
		{
			get
			{
				return (FarmerSprite)this.sprite;
			}
			set
			{
				this.sprite = value;
			}
		}

		[XmlIgnore]
		public FarmerRenderer FarmerRenderer
		{
			get
			{
				return this.farmerRenderer;
			}
			set
			{
				this.farmerRenderer = value;
			}
		}

		[XmlIgnore]
		public int Money
		{
			get
			{
				return this.money;
			}
			set
			{
				if (value > this.money)
				{
					uint num = (uint)(value - this.money);
					this.totalMoneyEarned += num;
					Game1.stats.checkForMoneyAchievements();
				}
				else
				{
					int arg_33_0 = this.money;
				}
				this.money = value;
			}
		}

		public Farmer()
		{
			this.farmerTextureManager = Game1.content.CreateTemporary();
			this.farmerRenderer = new FarmerRenderer(this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (this.isMale ? "" : "girl_") + "base"));
			this.currentLocation = Game1.getLocationFromName("FarmHouse");
			Game1.player.sprite = new FarmerSprite(null);
		}

		public Farmer(FarmerSprite sprite, Vector2 position, int speed, string name, List<Item> initialTools, bool isMale) : base(sprite, position, speed, name)
		{
			this.farmerTextureManager = Game1.content.CreateTemporary();
			this.pantsColor = new Color(46, 85, 183);
			this.hairstyleColor = new Color(193, 90, 50);
			this.newEyeColor = new Color(122, 68, 52);
			this.name = name;
			base.displayName = name;
			this.currentToolIndex = 0;
			this.isMale = isMale;
			this.basicShipped = new SerializableDictionary<int, int>();
			this.fishCaught = new SerializableDictionary<int, int[]>();
			this.archaeologyFound = new SerializableDictionary<int, int[]>();
			this.mineralsFound = new SerializableDictionary<int, int>();
			this.recipesCooked = new SerializableDictionary<int, int>();
			this.friendships = new SerializableDictionary<string, int[]>();
			this.stamina = (float)this.maxStamina;
			this.items = initialTools;
			if (this.items == null)
			{
				this.items = new List<Item>();
			}
			for (int i = this.items.Count; i < this.maxItems; i++)
			{
				this.items.Add(null);
			}
			this.activeDialogueEvents.Add("Introduction", 6);
			name = "Cam";
			this.farmerRenderer = new FarmerRenderer(this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (isMale ? "" : "girl_") + "base"));
			this.currentLocation = Game1.getLocationFromName("FarmHouse");
			if (this.currentLocation != null)
			{
				this.mostRecentBed = Utility.PointToVector2((this.currentLocation as FarmHouse).getBedSpot()) * (float)Game1.tileSize;
				return;
			}
			this.mostRecentBed = new Vector2(9f, 9f) * (float)Game1.tileSize;
		}

		public Texture2D getTexture()
		{
			if (this.farmerTextureManager == null)
			{
				this.farmerTextureManager = Game1.content.CreateTemporary();
			}
			return this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (this.isMale ? "" : "girl_") + "base");
		}

		public void checkForLevelTenStatus()
		{
		}

		public void unload()
		{
			if (this.farmerTextureManager != null)
			{
				this.farmerTextureManager.Unload();
				this.farmerTextureManager.Dispose();
				this.farmerTextureManager = null;
			}
		}

		public void setInventory(List<Item> newInventory)
		{
			this.items = newInventory;
			if (this.items == null)
			{
				this.items = new List<Item>();
			}
			for (int i = this.items.Count; i < this.maxItems; i++)
			{
				this.items.Add(null);
			}
		}

		public void makeThisTheActiveObject(Object o)
		{
			if (this.freeSpotsInInventory() > 0)
			{
				Item currentItem = this.CurrentItem;
				this.ActiveObject = o;
				this.addItemToInventory(currentItem);
			}
		}

		public int getNumberOfChildren()
		{
			int num = 0;
			foreach (NPC current in Utility.getHomeOfFarmer(Game1.player).characters)
			{
				if (current is Child && (current as Child).isChildOf(Game1.player))
				{
					num++;
				}
			}
			foreach (NPC current2 in Game1.getLocationFromName("Farm").characters)
			{
				if (current2 is Child && (current2 as Child).isChildOf(Game1.player))
				{
					num++;
				}
			}
			return num;
		}

		public void mountUp(Horse mount)
		{
			this.mount = mount;
			this.xOffset = -11f;
			this.position = Utility.PointToVector2(mount.GetBoundingBox().Location);
			this.position.Y = this.position.Y - (float)(Game1.pixelZoom * 4);
			this.position.X = this.position.X - (float)(Game1.pixelZoom * 2);
			this.speed = 2;
			this.showNotCarrying();
		}

		public Horse getMount()
		{
			return this.mount;
		}

		public void dismount()
		{
			if (this.mount != null)
			{
				this.mount = null;
			}
			this.collisionNPC = null;
			this.running = false;
			this.speed = ((Game1.isOneOfTheseKeysDown(Keyboard.GetState(), Game1.options.runButton) && !Game1.options.autoRun) ? 5 : 2);
			bool flag = this.speed == 5;
			this.running = flag;
			if (this.running)
			{
				this.speed = 5;
			}
			else
			{
				this.speed = 2;
				this.Halt();
			}
			this.Halt();
			this.xOffset = 0f;
		}

		public bool isRidingHorse()
		{
			return this.mount != null && !Game1.eventUp;
		}

		public List<Child> getChildren()
		{
			List<Child> list = new List<Child>();
			foreach (NPC current in Utility.getHomeOfFarmer(Game1.player).characters)
			{
				if (current is Child && (current as Child).isChildOf(Game1.player))
				{
					list.Add(current as Child);
				}
			}
			foreach (NPC current2 in Game1.getLocationFromName("Farm").characters)
			{
				if (current2 is Child && (current2 as Child).isChildOf(Game1.player))
				{
					list.Add(current2 as Child);
				}
			}
			return list;
		}

		public Tool getToolFromName(string name)
		{
			foreach (Item current in this.items)
			{
				if (current != null && current is Tool && current.Name.Contains(name))
				{
					return (Tool)current;
				}
			}
			return null;
		}

		public override void SetMovingDown(bool b)
		{
			this.setMoving((byte)(4 + (b ? 0 : 32)));
		}

		public override void SetMovingRight(bool b)
		{
			this.setMoving((byte)(2 + (b ? 0 : 32)));
		}

		public override void SetMovingUp(bool b)
		{
			this.setMoving((byte)(1 + (b ? 0 : 32)));
		}

		public override void SetMovingLeft(bool b)
		{
			this.setMoving((byte)(8 + (b ? 0 : 32)));
		}

		public int? tryGetFriendshipLevelForNPC(string name)
		{
			int[] array;
			if (this.friendships.TryGetValue(name, out array))
			{
				return new int?(array[0]);
			}
			return null;
		}

		public int getFriendshipLevelForNPC(string name)
		{
			int[] array;
			if (this.friendships.TryGetValue(name, out array))
			{
				return array[0];
			}
			return 0;
		}

		public int getFriendshipHeartLevelForNPC(string name)
		{
			return this.getFriendshipLevelForNPC(name) / 250;
		}

		public bool hasAFriendWithHeartLevel(int heartLevel, bool datablesOnly)
		{
			foreach (NPC current in Utility.getAllCharacters())
			{
				if ((!datablesOnly || current.datable) && this.getFriendshipHeartLevelForNPC(current.name) >= heartLevel)
				{
					return true;
				}
			}
			return false;
		}

		public int getTallyOfObject(int index, bool bigCraftable)
		{
			int num = 0;
			foreach (Item current in this.items)
			{
				if (current is Object && (current as Object).ParentSheetIndex == index && (current as Object).bigCraftable == bigCraftable)
				{
					num += current.Stack;
				}
			}
			return num;
		}

		public bool areAllItemsNull()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		public void shipAll()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null && this.items[i] is Object)
				{
					this.shippedBasic(((Object)this.items[i]).ParentSheetIndex, this.items[i].Stack);
					for (int j = 0; j < this.items[i].Stack; j++)
					{
						Game1.shipObject((Object)this.items[i].getOne());
					}
					this.items[i] = null;
				}
			}
			Game1.playSound("Ship");
		}

		public void shippedBasic(int index, int number)
		{
			if (this.basicShipped.ContainsKey(index))
			{
				SerializableDictionary<int, int> serializableDictionary = this.basicShipped;
				serializableDictionary[index] += number;
				return;
			}
			this.basicShipped.Add(index, number);
		}

		public void shiftToolbar(bool right)
		{
			if (this.items == null || this.items.Count < 12)
			{
				return;
			}
			if (this.UsingTool || Game1.dialogueUp || (!Game1.pickingTool && !Game1.player.CanMove) || this.areAllItemsNull() || Game1.eventUp)
			{
				return;
			}
			Game1.playSound("shwip");
			if (right)
			{
				List<Item> range = this.items.GetRange(0, 12);
				this.items.RemoveRange(0, 12);
				this.items.AddRange(range);
			}
			else
			{
				List<Item> range2 = this.items.GetRange(this.items.Count - 12, 12);
				for (int i = 0; i < this.items.Count - 12; i++)
				{
					range2.Add(this.items[i]);
				}
				this.items = range2;
			}
			for (int j = 0; j < Game1.onScreenMenus.Count; j++)
			{
				if (Game1.onScreenMenus[j] is Toolbar)
				{
					(Game1.onScreenMenus[j] as Toolbar).shifted(right);
					return;
				}
			}
		}

		public void foundArtifact(int index, int number)
		{
			if (this.archaeologyFound == null)
			{
				this.archaeologyFound = new SerializableDictionary<int, int[]>();
			}
			if (this.archaeologyFound.ContainsKey(index))
			{
				this.archaeologyFound[index][0] += number;
				this.archaeologyFound[index][1] += number;
				return;
			}
			if (this.archaeologyFound.Count == 0)
			{
				if (!this.eventsSeen.Contains(0) && index != 102)
				{
					this.addQuest(23);
				}
				this.mailReceived.Add("artifactFound");
				this.holdUpItemThenMessage(new Object(index, 1, false, -1, 0), true);
			}
			this.archaeologyFound.Add(index, new int[]
			{
				number,
				number
			});
		}

		public void cookedRecipe(int index)
		{
			if (this.recipesCooked == null)
			{
				this.recipesCooked = new SerializableDictionary<int, int>();
			}
			if (this.recipesCooked.ContainsKey(index))
			{
				SerializableDictionary<int, int> expr_29 = this.recipesCooked;
				int num = expr_29[index];
				expr_29[index] = num + 1;
				return;
			}
			this.recipesCooked.Add(index, 1);
		}

		public bool caughtFish(int index, int size)
		{
			if (this.fishCaught == null)
			{
				this.fishCaught = new SerializableDictionary<int, int[]>();
			}
			if (index >= 167 && index < 173)
			{
				return false;
			}
			bool result = false;
			if (this.fishCaught.ContainsKey(index))
			{
				this.fishCaught[index][0]++;
				Game1.stats.checkForFishingAchievements();
				if (size > this.fishCaught[index][1])
				{
					this.fishCaught[index][1] = size;
					result = true;
				}
			}
			else
			{
				this.fishCaught.Add(index, new int[]
				{
					1,
					size
				});
				Game1.stats.checkForFishingAchievements();
			}
			this.checkForQuestComplete(null, index, -1, null, null, 7, -1);
			return result;
		}

		public void gainExperience(int which, int howMuch)
		{
			if (which == 5 || howMuch <= 0)
			{
				return;
			}
			int num = Farmer.checkForLevelGain(this.experiencePoints[which], this.experiencePoints[which] + howMuch);
			this.experiencePoints[which] += howMuch;
			int num2 = -1;
			if (num != -1)
			{
				switch (which)
				{
				case 0:
					num2 = this.farmingLevel;
					this.farmingLevel = num;
					break;
				case 1:
					num2 = this.fishingLevel;
					this.fishingLevel = num;
					break;
				case 2:
					num2 = this.foragingLevel;
					this.foragingLevel = num;
					break;
				case 3:
					num2 = this.miningLevel;
					this.miningLevel = num;
					break;
				case 4:
					num2 = this.combatLevel;
					this.combatLevel = num;
					break;
				case 5:
					num2 = this.luckLevel;
					this.luckLevel = num;
					break;
				}
			}
			if (num > num2)
			{
				for (int i = num2 + 1; i <= num; i++)
				{
					this.newLevels.Add(new Point(which, i));
					int arg_DF_0 = this.newLevels.Count;
				}
			}
		}

		public int getEffectiveSkillLevel(int whichSkill)
		{
			if (whichSkill < 0 || whichSkill > 5)
			{
				return -1;
			}
			int[] array = new int[]
			{
				this.farmingLevel,
				this.fishingLevel,
				this.foragingLevel,
				this.miningLevel,
				this.combatLevel,
				this.luckLevel
			};
			for (int i = 0; i < this.newLevels.Count; i++)
			{
				array[this.newLevels[i].X] -= this.newLevels[i].Y;
			}
			return array[whichSkill];
		}

		public static int checkForLevelGain(int oldXP, int newXP)
		{
			int result = -1;
			if (oldXP < 100 && newXP >= 100)
			{
				result = 1;
			}
			if (oldXP < 380 && newXP >= 380)
			{
				result = 2;
			}
			if (oldXP < 770 && newXP >= 770)
			{
				result = 3;
			}
			if (oldXP < 1300 && newXP >= 1300)
			{
				result = 4;
			}
			if (oldXP < 2150 && newXP >= 2150)
			{
				result = 5;
			}
			if (oldXP < 3300 && newXP >= 3300)
			{
				result = 6;
			}
			if (oldXP < 4800 && newXP >= 4800)
			{
				result = 7;
			}
			if (oldXP < 6900 && newXP >= 6900)
			{
				result = 8;
			}
			if (oldXP < 10000 && newXP >= 10000)
			{
				result = 9;
			}
			if (oldXP < 15000 && newXP >= 15000)
			{
				result = 10;
			}
			return result;
		}

		public void foundMineral(int index)
		{
			if (this.mineralsFound == null)
			{
				this.mineralsFound = new SerializableDictionary<int, int>();
			}
			if (this.mineralsFound.ContainsKey(index))
			{
				SerializableDictionary<int, int> expr_29 = this.mineralsFound;
				int num = expr_29[index];
				expr_29[index] = num + 1;
			}
			else
			{
				this.mineralsFound.Add(index, 1);
			}
			if (!this.hasOrWillReceiveMail("artifactFound"))
			{
				this.mailReceived.Add("artifactFound");
			}
		}

		public void increaseBackpackSize(int howMuch)
		{
			this.MaxItems += howMuch;
			for (int i = 0; i < howMuch; i++)
			{
				this.items.Add(null);
			}
		}

		public void consumeObject(int index, int quantity)
		{
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				if (this.items[i] != null && this.items[i] is Object && ((Object)this.items[i]).parentSheetIndex == index)
				{
					int num = quantity;
					quantity -= this.items[i].Stack;
					this.items[i].Stack -= num;
					if (this.items[i].Stack <= 0)
					{
						this.items[i] = null;
					}
					if (quantity <= 0)
					{
						return;
					}
				}
			}
		}

		public bool hasItemInInventory(int itemIndex, int quantity, int minPrice = 0)
		{
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null && ((this.items[i] is Object && !(this.items[i] is Furniture) && !(this.items[i] as Object).bigCraftable && ((Object)this.items[i]).ParentSheetIndex == itemIndex) || (this.items[i] is Object && ((Object)this.items[i]).Category == itemIndex)))
				{
					num += this.items[i].Stack;
				}
			}
			return num >= quantity;
		}

		public bool hasItemInList(List<Item> list, int itemIndex, int quantity, int minPrice = 0)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null && ((list[i] is Object && !(list[i] is Furniture) && !(list[i] as Object).bigCraftable && ((Object)list[i]).ParentSheetIndex == itemIndex) || (list[i] is Object && ((Object)list[i]).Category == itemIndex)))
				{
					num += list[i].Stack;
				}
			}
			return num >= quantity;
		}

		public void addItemByMenuIfNecessaryElseHoldUp(Item item, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
		{
			this.mostRecentlyGrabbedItem = item;
			this.addItemsByMenuIfNecessary(new List<Item>
			{
				item
			}, itemSelectedCallback);
			if (Game1.activeClickableMenu == null && this.mostRecentlyGrabbedItem.parentSheetIndex != 434)
			{
				this.holdUpItemThenMessage(item, true);
			}
		}

		public void addItemByMenuIfNecessary(Item item, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
		{
			this.addItemsByMenuIfNecessary(new List<Item>
			{
				item
			}, itemSelectedCallback);
		}

		public void addItemsByMenuIfNecessary(List<Item> itemsToAdd, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
		{
			if (itemsToAdd == null)
			{
				return;
			}
			if (itemsToAdd.Count > 0 && itemsToAdd[0] is Object && (itemsToAdd[0] as Object).parentSheetIndex == 434)
			{
				Game1.playerEatObject(itemsToAdd[0] as Object, true);
				if (Game1.activeClickableMenu != null)
				{
					Game1.activeClickableMenu.exitThisMenu(false);
				}
				return;
			}
			for (int i = itemsToAdd.Count - 1; i >= 0; i--)
			{
				if (this.addItemToInventoryBool(itemsToAdd[i], false))
				{
					if (itemSelectedCallback != null)
					{
						itemSelectedCallback(itemsToAdd[i], this);
					}
					itemsToAdd.Remove(itemsToAdd[i]);
				}
			}
			if (itemsToAdd.Count > 0)
			{
				Game1.activeClickableMenu = new ItemGrabMenu(itemsToAdd);
				(Game1.activeClickableMenu as ItemGrabMenu).inventory.showGrayedOutSlots = true;
				(Game1.activeClickableMenu as ItemGrabMenu).inventory.onAddItem = itemSelectedCallback;
				(Game1.activeClickableMenu as ItemGrabMenu).source = 2;
			}
		}

		public void showCarrying()
		{
			if (Game1.eventUp || this.isRidingHorse())
			{
				return;
			}
			if (this.ActiveObject != null && (this.ActiveObject is Furniture || this.ActiveObject is Wallpaper))
			{
				return;
			}
			if (!this.FarmerSprite.pauseForSingleAnimation && !this.isMoving())
			{
				int indexInCurrentAnimation = this.FarmerSprite.indexInCurrentAnimation;
				float timer = this.FarmerSprite.timer;
				switch (this.facingDirection)
				{
				case 0:
					this.FarmerSprite.setCurrentFrame(this.running ? 144 : 112);
					break;
				case 1:
					this.FarmerSprite.setCurrentFrame(this.running ? 136 : 104);
					break;
				case 2:
					this.FarmerSprite.setCurrentFrame(this.running ? 128 : 96);
					break;
				case 3:
					this.FarmerSprite.setCurrentFrame(this.running ? 152 : 120);
					break;
				}
				this.FarmerSprite.CurrentFrame = this.FarmerSprite.CurrentAnimation[indexInCurrentAnimation].frame;
				this.FarmerSprite.indexInCurrentAnimation = indexInCurrentAnimation;
				this.FarmerSprite.currentAnimationIndex = indexInCurrentAnimation;
				this.FarmerSprite.timer = timer;
				if (this.IsMainPlayer && this.ActiveObject != null)
				{
					MultiplayerUtility.sendSwitchHeldItemMessage(this.ActiveObject.ParentSheetIndex, this.ActiveObject.bigCraftable ? 1 : 0, this.uniqueMultiplayerID);
				}
			}
			if (this.ActiveObject != null)
			{
				this.mostRecentlyGrabbedItem = this.ActiveObject;
			}
			if (this.mostRecentlyGrabbedItem != null && this.mostRecentlyGrabbedItem is Object && (this.mostRecentlyGrabbedItem as Object).ParentSheetIndex == 434)
			{
				Game1.eatHeldObject();
			}
		}

		public void showNotCarrying()
		{
			if (!this.FarmerSprite.pauseForSingleAnimation && !this.isMoving())
			{
				int indexInCurrentAnimation = this.FarmerSprite.indexInCurrentAnimation;
				float timer = this.FarmerSprite.timer;
				switch (this.facingDirection)
				{
				case 0:
					this.FarmerSprite.setCurrentFrame(this.running ? 48 : 16);
					break;
				case 1:
					this.FarmerSprite.setCurrentFrame(this.running ? 40 : 8);
					break;
				case 2:
					this.FarmerSprite.setCurrentFrame(this.running ? 32 : 0);
					break;
				case 3:
					this.FarmerSprite.setCurrentFrame(this.running ? 56 : 24);
					break;
				}
				this.FarmerSprite.CurrentFrame = this.FarmerSprite.CurrentAnimation[Math.Min(indexInCurrentAnimation, this.FarmerSprite.CurrentAnimation.Count - 1)].frame;
				this.FarmerSprite.indexInCurrentAnimation = indexInCurrentAnimation;
				this.FarmerSprite.currentAnimationIndex = indexInCurrentAnimation;
				this.FarmerSprite.timer = timer;
				if (this.IsMainPlayer)
				{
					MultiplayerUtility.sendSwitchHeldItemMessage(-1, 0, this.uniqueMultiplayerID);
				}
			}
		}

		public bool isThereALostItemQuestThatTakesThisItem(int index)
		{
			foreach (Quest current in Game1.player.questLog)
			{
				if (current is LostItemQuest && (current as LostItemQuest).itemIndex == index)
				{
					return true;
				}
			}
			return false;
		}

		public bool hasDailyQuest()
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].dailyQuest)
				{
					return true;
				}
			}
			return false;
		}

		public void dayupdate()
		{
			this.attack = 0;
			this.addedSpeed = 0;
			this.dancePartner = null;
			this.festivalScore = 0;
			this.forceTimePass = false;
			if (this.daysLeftForToolUpgrade > 0)
			{
				this.daysLeftForToolUpgrade--;
			}
			if (this.daysUntilHouseUpgrade > 0)
			{
				this.daysUntilHouseUpgrade--;
				if (this.daysUntilHouseUpgrade <= 0)
				{
					this.daysUntilHouseUpgrade = -1;
					this.houseUpgradeLevel++;
					Utility.getHomeOfFarmer(this).moveObjectsForHouseUpgrade(this.houseUpgradeLevel);
					Utility.getHomeOfFarmer(this).upgradeLevel++;
					if (this.houseUpgradeLevel == 1)
					{
						this.position = new Vector2(20f, 4f) * (float)Game1.tileSize;
					}
					if (this.houseUpgradeLevel == 2)
					{
						this.position = new Vector2(29f, 13f) * (float)Game1.tileSize;
					}
					Game1.stats.checkForBuildingUpgradeAchievements();
				}
			}
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].dailyQuest)
				{
					this.questLog[i].daysLeft--;
					if (this.questLog[i].daysLeft <= 0 && !this.questLog[i].completed)
					{
						this.questLog.RemoveAt(i);
					}
				}
			}
			using (List<Buff>.Enumerator enumerator = this.buffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.removeBuff();
				}
			}
			Game1.buffsDisplay.clearAllBuffs();
			base.stopGlowing();
			this.buffs.Clear();
			this.addedCombatLevel = 0;
			this.addedFarmingLevel = 0;
			this.addedFishingLevel = 0;
			this.addedForagingLevel = 0;
			this.addedLuckLevel = 0;
			this.addedMiningLevel = 0;
			this.addedSpeed = 0;
			this.bobber = "";
			float num = this.Stamina;
			this.Stamina = (float)this.MaxStamina;
			if (this.exhausted)
			{
				this.exhausted = false;
				this.Stamina = (float)(this.MaxStamina / 2 + 1);
			}
			if (Game1.timeOfDay > 2400)
			{
				this.Stamina -= (1f - (float)(2600 - Math.Min(2600, Game1.timeOfDay)) / 200f) * (float)(this.MaxStamina / 2);
				if (Game1.timeOfDay > 2700)
				{
					this.Stamina /= 2f;
				}
			}
			if (Game1.timeOfDay < 2700 && num > this.Stamina)
			{
				this.Stamina = num;
			}
			this.health = this.maxHealth;
			List<string> list = new List<string>();
			foreach (string current in this.activeDialogueEvents.Keys.ToList<string>())
			{
				SerializableDictionary<string, int> arg_2E7_0 = this.activeDialogueEvents;
				string key = current;
				int num2 = arg_2E7_0[key];
				arg_2E7_0[key] = num2 - 1;
				if (this.activeDialogueEvents[current] < 0)
				{
					list.Add(current);
				}
			}
			foreach (string current2 in list)
			{
				this.activeDialogueEvents.Remove(current2);
			}
			if (this.isMarried())
			{
				this.daysMarried++;
			}
			if (this.isMarried() && this.divorceTonight)
			{
				NPC nPC = this.getSpouse();
				if (nPC != null)
				{
					this.spouse = null;
					string text = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[nPC.name].Split(new char[]
					{
						'/'
					})[10];
					nPC.defaultMap = text.Split(new char[]
					{
						' '
					})[0];
					nPC.DefaultPosition = new Vector2((float)Convert.ToInt32(text.Split(new char[]
					{
						' '
					})[1]), (float)Convert.ToInt32(text.Split(new char[]
					{
						' '
					})[2])) * (float)Game1.tileSize;
					nPC.datingFarmer = false;
					nPC.divorcedFromFarmer = true;
					nPC.setMarried(false);
					for (int j = this.specialItems.Count - 1; j >= 0; j--)
					{
						if (this.specialItems[j] == 460)
						{
							this.specialItems.RemoveAt(j);
						}
					}
					if (this.friendships.ContainsKey(nPC.name))
					{
						this.friendships[nPC.name][0] = 0;
					}
					Game1.warpCharacter(nPC, nPC.defaultMap, nPC.DefaultPosition, true, false);
					Utility.getHomeOfFarmer(this).showSpouseRoom();
					Game1.getFarm().addSpouseOutdoorArea("");
				}
				this.divorceTonight = false;
			}
		}

		public static void showReceiveNewItemMessage(Farmer who)
		{
			string text = who.mostRecentlyGrabbedItem.checkForSpecialItemHoldUpMeessage();
			if (text != null)
			{
				Game1.drawObjectDialogue(text);
			}
			else if (who.mostRecentlyGrabbedItem.parentSheetIndex == 472 && who.mostRecentlyGrabbedItem.Stack == 15)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1918", new object[0]));
			}
			else
			{
				Game1.drawObjectDialogue((who.mostRecentlyGrabbedItem.Stack > 1) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1922", new object[]
				{
					who.mostRecentlyGrabbedItem.Stack,
					who.mostRecentlyGrabbedItem.DisplayName
				}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1919", new object[]
				{
					who.mostRecentlyGrabbedItem.DisplayName,
					Game1.getProperArticleForWord(who.mostRecentlyGrabbedItem.DisplayName)
				}));
			}
			who.completelyStopAnimatingOrDoingAction();
		}

		public static void showEatingItem(Farmer who)
		{
			TemporaryAnimatedSprite temporaryAnimatedSprite = null;
			if (who.itemToEat == null)
			{
				return;
			}
			switch (who.FarmerSprite.indexInCurrentAnimation)
			{
			case 1:
				if (who.itemToEat != null && who.itemToEat is Object && (who.itemToEat as Object).ParentSheetIndex == 434)
				{
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 62.75f, 8, 2, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
				}
				else
				{
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16), 254f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
				}
				break;
			case 2:
				if (who.itemToEat != null && who.itemToEat is Object && (who.itemToEat as Object).ParentSheetIndex == 434)
				{
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 81.25f, 8, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + 4 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
					{
						motion = new Vector2(0.8f, -11f),
						acceleration = new Vector2(0f, 0.5f)
					};
				}
				else
				{
					Game1.playSound("dwop");
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16), 650f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + 4 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
					{
						motion = new Vector2(0.8f, -11f),
						acceleration = new Vector2(0f, 0.5f)
					};
				}
				break;
			case 3:
				who.yJumpVelocity = 6f;
				who.yJumpOffset = 1;
				break;
			case 4:
				Game1.playSound("eat");
				for (int i = 0; i < 8; i++)
				{
					Microsoft.Xna.Framework.Rectangle sourceRectForStandardTileSheet = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16);
					sourceRectForStandardTileSheet.X += 8;
					sourceRectForStandardTileSheet.Y += 8;
					sourceRectForStandardTileSheet.Width = Game1.pixelZoom;
					sourceRectForStandardTileSheet.Height = Game1.pixelZoom;
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, sourceRectForStandardTileSheet, 400f, 1, 0, who.position + new Vector2(24f, -48f), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						motion = new Vector2((float)Game1.random.Next(-30, 31) / 10f, (float)Game1.random.Next(-6, -3)),
						acceleration = new Vector2(0f, 0.5f)
					};
					who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite);
				}
				return;
			default:
				who.freezePause = 0;
				break;
			}
			if (temporaryAnimatedSprite != null)
			{
				who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite);
			}
		}

		public static void eatItem(Farmer who)
		{
		}

		public bool hasBuff(int whichBuff)
		{
			using (List<Buff>.Enumerator enumerator = this.buffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.which == whichBuff)
					{
						bool result = true;
						return result;
					}
				}
			}
			using (List<Buff>.Enumerator enumerator = Game1.buffsDisplay.otherBuffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.which == whichBuff)
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		public bool hasOrWillReceiveMail(string id)
		{
			return this.mailReceived.Contains(id) || this.mailForTomorrow.Contains(id) || Game1.mailbox.Contains(id) || this.mailForTomorrow.Contains(id + "%&NL&%");
		}

		public static void showHoldingItem(Farmer who)
		{
			if (who.mostRecentlyGrabbedItem is SpecialItem)
			{
				TemporaryAnimatedSprite temporarySpriteForHoldingUp = (who.mostRecentlyGrabbedItem as SpecialItem).getTemporarySpriteForHoldingUp(who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)));
				temporarySpriteForHoldingUp.motion = new Vector2(0f, -0.1f);
				temporarySpriteForHoldingUp.scale = (float)Game1.pixelZoom;
				temporarySpriteForHoldingUp.interval = 2500f;
				temporarySpriteForHoldingUp.totalNumberOfLoops = 0;
				temporarySpriteForHoldingUp.animationLength = 1;
				Game1.currentLocation.temporarySprites.Add(temporarySpriteForHoldingUp);
			}
			else if (who.mostRecentlyGrabbedItem is Slingshot)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Tool.weaponsTexture, Game1.getSquareSourceRectForNonStandardTileSheet(Tool.weaponsTexture, 16, 16, (who.mostRecentlyGrabbedItem as Slingshot).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is MeleeWeapon)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Tool.weaponsTexture, Game1.getSquareSourceRectForNonStandardTileSheet(Tool.weaponsTexture, 16, 16, (who.mostRecentlyGrabbedItem as MeleeWeapon).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is Boots)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSquareSourceRectForNonStandardTileSheet(Game1.objectSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, (who.mostRecentlyGrabbedItem as Boots).indexInTileSheet), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is Tool)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.toolSpriteSheet, Game1.getSquareSourceRectForNonStandardTileSheet(Game1.toolSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, (who.mostRecentlyGrabbedItem as Tool).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is Furniture)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Furniture.furnitureTexture, (who.mostRecentlyGrabbedItem as Furniture).sourceRect, 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 + 4)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
			}
			else if (who.mostRecentlyGrabbedItem is Object && !(who.mostRecentlyGrabbedItem as Object).bigCraftable)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, who.mostRecentlyGrabbedItem.parentSheetIndex, 16, 16), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				if (who.mostRecentlyGrabbedItem.parentSheetIndex == 434)
				{
					Game1.eatHeldObject();
				}
			}
			else if (who.mostRecentlyGrabbedItem is Object)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.bigCraftableSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, who.mostRecentlyGrabbedItem.parentSheetIndex, 16, 32), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 + 4)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
			}
			if (who.mostRecentlyGrabbedItem == null)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(420, 489, 25, 18), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 - Game1.pixelZoom * 6)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				return;
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(10, who.position + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 3 / 2)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				motion = new Vector2(0f, -0.1f)
			});
		}

		public void holdUpItemThenMessage(Item item, bool showMessage = true)
		{
			this.completelyStopAnimatingOrDoingAction();
			if (showMessage)
			{
				DelayedAction.playSoundAfterDelay("getNewSpecialItem", 750);
			}
			Game1.player.faceDirection(2);
			this.freezePause = 4000;
			this.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(57, 0),
				new FarmerSprite.AnimationFrame(57, 2500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showHoldingItem), false),
				showMessage ? new FarmerSprite.AnimationFrame((int)((short)this.FarmerSprite.currentFrame), 500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showReceiveNewItemMessage), true) : new FarmerSprite.AnimationFrame((int)((short)this.FarmerSprite.currentFrame), 500, false, false, null, false)
			});
			this.mostRecentlyGrabbedItem = item;
			this.canMove = false;
		}

		private void checkForLevelUp()
		{
			int num = 600;
			int num2 = 0;
			int level = this.Level;
			for (int i = 0; i <= 35; i++)
			{
				if (level <= i && (ulong)this.totalMoneyEarned >= (ulong)((long)num))
				{
					this.NewSkillPointsToSpend += 2;
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1925", new object[0]), Color.Violet, 3500f));
				}
				else if ((ulong)this.totalMoneyEarned < (ulong)((long)num))
				{
					return;
				}
				int arg_7A_0 = num;
				num += (int)((double)(num - num2) * 1.2);
				num2 = arg_7A_0;
			}
		}

		public void clearBackpack()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i] = null;
			}
		}

		public int numberOfItemsInInventory()
		{
			int num = 0;
			foreach (Item current in this.items)
			{
				if (current != null && current is Object)
				{
					num++;
				}
			}
			return num;
		}

		public void resetFriendshipsForNewDay()
		{
			string[] array = this.friendships.Keys.ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				this.friendships[text][3] = 0;
				bool flag = false;
				NPC characterFromName = Game1.getCharacterFromName(text, false);
				if (characterFromName != null && characterFromName.datable && !characterFromName.datingFarmer && !characterFromName.isMarried())
				{
					flag = true;
				}
				if (this.spouse != null && text.Equals(this.spouse) && !this.hasPlayerTalkedToNPC(text))
				{
					this.friendships[text][0] = Math.Max(this.friendships[text][0] - 20, 0);
				}
				else if (characterFromName != null && characterFromName.datingFarmer && !this.hasPlayerTalkedToNPC(text) && this.friendships[text][0] < 2500)
				{
					this.friendships[text][0] = Math.Max(this.friendships[text][0] - 8, 0);
				}
				if (this.hasPlayerTalkedToNPC(text))
				{
					this.friendships[text][2] = 0;
				}
				else if ((!flag && this.friendships[text][0] < 2500) || (flag && this.friendships[text][0] < 2000))
				{
					this.friendships[text][0] = Math.Max(this.friendships[text][0] - 2, 0);
				}
				if (Game1.dayOfMonth % 7 == 0)
				{
					if (this.friendships[text][1] == 2)
					{
						this.friendships[text][0] = Math.Min(this.friendships[text][0] + 10, 2749);
					}
					this.friendships[text][1] = 0;
				}
			}
		}

		public bool hasPlayerTalkedToNPC(string name)
		{
			if (!this.friendships.ContainsKey(name) && Game1.NPCGiftTastes.ContainsKey(name))
			{
				this.friendships.Add(name, new int[4]);
			}
			return this.friendships.ContainsKey(name) && this.friendships[name][2] == 1;
		}

		public void fuelLantern(int units)
		{
			Tool toolFromName = this.getToolFromName("Lantern");
			if (toolFromName != null)
			{
				((Lantern)toolFromName).fuelLeft = Math.Min(100, ((Lantern)toolFromName).fuelLeft + units);
			}
		}

		public bool tryToCraftItem(List<int[]> ingredients, double successRate, int itemToCraft, bool bigCraftable, string craftingOrCooking)
		{
			List<int[]> list = new List<int[]>();
			foreach (int[] current in ingredients)
			{
				if (current[0] <= -100)
				{
					int num = 0;
					switch (current[0])
					{
					case -106:
						num = this.IridiumPieces;
						break;
					case -105:
						num = this.GoldPieces;
						break;
					case -104:
						num = this.CoalPieces;
						break;
					case -103:
						num = this.IronPieces;
						break;
					case -102:
						num = this.CopperPieces;
						break;
					case -101:
						num = this.stonePieces;
						break;
					case -100:
						num = this.WoodPieces;
						break;
					}
					if (num < current[1])
					{
						bool result = false;
						return result;
					}
					list.Add(current);
				}
				else
				{
					for (int i = 0; i < current[1]; i++)
					{
						int[] array = new int[]
						{
							99999,
							-1
						};
						int j = 0;
						while (j < this.items.Count)
						{
							if (this.items[j] != null && this.items[j] is Object && ((Object)this.items[j]).ParentSheetIndex == current[0] && !Farmer.containsIndex(list, j))
							{
								list.Add(new int[]
								{
									j,
									1
								});
								break;
							}
							if (this.items[j] != null && this.items[j] is Object && ((Object)this.items[j]).Category == current[0] && !Farmer.containsIndex(list, j) && ((Object)this.items[j]).Price < array[0])
							{
								array[0] = ((Object)this.items[j]).Price;
								array[1] = j;
							}
							if (j == this.items.Count - 1)
							{
								if (array[1] != -1)
								{
									list.Add(new int[]
									{
										array[1],
										current[1]
									});
									break;
								}
								bool result = false;
								return result;
							}
							else
							{
								j++;
							}
						}
					}
				}
			}
			string text = "";
			if (itemToCraft == 291)
			{
				text = ((Object)this.items[list[0][0]]).Name;
			}
			else if (itemToCraft == 216 && Game1.random.NextDouble() < 0.5)
			{
				itemToCraft++;
			}
			Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1927", new object[]
			{
				craftingOrCooking
			}));
			this.isCrafting = true;
			Game1.playSound("crafting");
			int num2 = -1;
			string message = Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1930", new object[0]);
			if (bigCraftable)
			{
				Game1.player.ActiveObject = new Object(Vector2.Zero, itemToCraft, false);
				Game1.player.showCarrying();
			}
			else if (itemToCraft < 0)
			{
				if (!true)
				{
					message = Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1935", new object[0]);
				}
			}
			else
			{
				num2 = list[0][0];
				if (list[0][0] < 0)
				{
					for (int k = 0; k < this.items.Count; k++)
					{
						if (this.items[k] == null)
						{
							num2 = k;
							break;
						}
						if (k == this.maxItems - 1)
						{
							Game1.pauseThenMessage(this.craftingTime + ingredients.Count<int[]>() * 500, Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1936", new object[0]), true);
							return false;
						}
					}
				}
				if (!text.Equals(""))
				{
					this.items[num2] = new Object(Vector2.Zero, itemToCraft, text + " Bobber", true, true, false, false);
				}
				else
				{
					this.items[num2] = new Object(Vector2.Zero, itemToCraft, null, true, true, false, false);
				}
			}
			Game1.pauseThenMessage(this.craftingTime + ingredients.Count * 500, message, true);
			string a = craftingOrCooking.ToLower();
			if (!(a == "crafting"))
			{
				if (a == "cooking")
				{
					Stats expr_462 = Game1.stats;
					uint num3 = expr_462.ItemsCooked;
					expr_462.ItemsCooked = num3 + 1u;
				}
			}
			else
			{
				Stats expr_44A = Game1.stats;
				uint num3 = expr_44A.ItemsCrafted;
				expr_44A.ItemsCrafted = num3 + 1u;
			}
			foreach (int[] current2 in list)
			{
				if (current2[0] <= -100)
				{
					switch (current2[0])
					{
					case -106:
						this.IridiumPieces -= current2[1];
						break;
					case -105:
						this.GoldPieces -= current2[1];
						break;
					case -104:
						this.CoalPieces -= current2[1];
						break;
					case -103:
						this.IronPieces -= current2[1];
						break;
					case -102:
						this.CopperPieces -= current2[1];
						break;
					case -101:
						this.stonePieces -= current2[1];
						break;
					case -100:
						this.WoodPieces -= current2[1];
						break;
					}
				}
				else if (current2[0] != num2)
				{
					this.items[current2[0]] = null;
				}
			}
			return true;
		}

		private static bool containsIndex(List<int[]> locationOfIngredients, int index)
		{
			for (int i = 0; i < locationOfIngredients.Count; i++)
			{
				if (locationOfIngredients[i][0] == index)
				{
					return true;
				}
			}
			return false;
		}

		public override bool collideWith(Object o)
		{
			base.collideWith(o);
			if (this.isRidingHorse() && o is Fence)
			{
				this.mount.squeezeForGate();
				int facingDirection = this.facingDirection;
				if (facingDirection != 1)
				{
					if (facingDirection == 3 && o.tileLocation.X > (float)base.getTileX())
					{
						return false;
					}
				}
				else if (o.tileLocation.X < (float)base.getTileX())
				{
					return false;
				}
			}
			return true;
		}

		public void changeIntoSwimsuit()
		{
			this.bathingClothes = true;
			this.Halt();
			this.setRunning(false, false);
			this.canOnlyWalk = true;
		}

		public void changeOutOfSwimSuit()
		{
			this.bathingClothes = false;
			this.canOnlyWalk = false;
			this.Halt();
			this.FarmerSprite.StopAnimation();
			if (Game1.options.autoRun)
			{
				this.setRunning(true, false);
			}
		}

		public bool ownsFurniture(string name)
		{
			using (List<string>.Enumerator enumerator = this.furnitureOwned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Equals(name))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void showFrame(int frame, bool flip = false)
		{
			List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
			list.Add(new FarmerSprite.AnimationFrame(Convert.ToInt32(frame), 100, false, flip, null, false));
			this.FarmerSprite.setCurrentAnimation(list.ToArray());
			this.FarmerSprite.loopThisAnimation = true;
			this.FarmerSprite.PauseForSingleAnimation = true;
			this.sprite.CurrentFrame = Convert.ToInt32(frame);
		}

		public void stopShowingFrame()
		{
			this.FarmerSprite.loopThisAnimation = false;
			this.FarmerSprite.PauseForSingleAnimation = false;
			this.completelyStopAnimatingOrDoingAction();
		}

		public Item addItemToInventory(Item item)
		{
			if (item == null)
			{
				return null;
			}
			if (item is SpecialItem)
			{
				return item;
			}
			for (int i = 0; i < this.maxItems; i++)
			{
				if (i < this.items.Count && this.items[i] != null && this.items[i].maximumStackSize() != -1 && this.items[i].getStack() < this.items[i].maximumStackSize() && this.items[i].Name.Equals(item.Name) && (!(item is Object) || !(this.items[i] is Object) || ((item as Object).quality == (this.items[i] as Object).quality && (item as Object).parentSheetIndex == (this.items[i] as Object).parentSheetIndex)) && item.canStackWith(this.items[i]))
				{
					int num = this.items[i].addToStack(item.getStack());
					if (num <= 0)
					{
						return null;
					}
					item.Stack = num;
				}
			}
			for (int j = 0; j < this.maxItems; j++)
			{
				if (this.items.Count > j && this.items[j] == null)
				{
					this.items[j] = item;
					return null;
				}
			}
			return item;
		}

		public bool isInventoryFull()
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && this.items[i] == null)
				{
					return false;
				}
			}
			return true;
		}

		public bool couldInventoryAcceptThisItem(Item item)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && (this.items[i] == null || (item is Object && this.items[i] is Object && this.items[i].Stack + item.Stack <= this.items[i].maximumStackSize() && (this.items[i] as Object).canStackWith(item))))
				{
					return true;
				}
			}
			if (this.isInventoryFull() && Game1.hudMessages.Count<HUDMessage>() == 0)
			{
				Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588", new object[0]));
			}
			return false;
		}

		public bool couldInventoryAcceptThisObject(int index, int stack, int quality = 0)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && (this.items[i] == null || (this.items[i] is Object && this.items[i].Stack + stack <= this.items[i].maximumStackSize() && (this.items[i] as Object).ParentSheetIndex == index && (this.items[i] as Object).quality == quality)))
				{
					return true;
				}
			}
			if (this.isInventoryFull() && Game1.hudMessages.Count<HUDMessage>() == 0)
			{
				Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588", new object[0]));
			}
			return false;
		}

		public bool hasItemOfType(string type)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && this.items[i] is Object && (this.items[i] as Object).type.Equals(type))
				{
					return true;
				}
			}
			return false;
		}

		public NPC getSpouse()
		{
			if (this.isMarried())
			{
				return Game1.getCharacterFromName(this.spouse, false);
			}
			return null;
		}

		public int freeSpotsInInventory()
		{
			int num = 0;
			for (int i = 0; i < this.maxItems; i++)
			{
				if (i < this.items.Count && this.items[i] == null)
				{
					num++;
				}
			}
			return num;
		}

		public Item hasItemWithNameThatContains(string name)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (i < this.items.Count && this.items[i] != null && this.items[i].Name.Contains(name))
				{
					return this.items[i];
				}
			}
			return null;
		}

		public bool addItemToInventoryBool(Item item, bool makeActiveObject = false)
		{
			if (item == null)
			{
				return false;
			}
			int arg_0B_0 = item.Stack;
			Item item2 = this.IsMainPlayer ? this.addItemToInventory(item) : null;
			bool flag = item2 == null || item2.Stack != item.Stack || item is SpecialItem;
			if (item is Object)
			{
				(item as Object).reloadSprite();
			}
			if (flag && this.IsMainPlayer)
			{
				if (item != null)
				{
					if (this.IsMainPlayer && !item.hasBeenInInventory)
					{
						if (item is SpecialItem)
						{
							(item as SpecialItem).actionWhenReceived(this);
							return true;
						}
						if (item is Object && (item as Object).specialItem)
						{
							if ((item as Object).bigCraftable || item is Furniture)
							{
								if (!this.specialBigCraftables.Contains((item as Object).parentSheetIndex))
								{
									this.specialBigCraftables.Add((item as Object).parentSheetIndex);
								}
							}
							else if (!this.specialItems.Contains((item as Object).parentSheetIndex))
							{
								this.specialItems.Add((item as Object).parentSheetIndex);
							}
						}
						if (item is Object && (item as Object).Category == -2 && !(item as Object).hasBeenPickedUpByFarmer)
						{
							this.foundMineral((item as Object).parentSheetIndex);
						}
						else if (!(item is Furniture) && item is Object && (item as Object).type != null && (item as Object).type.Contains("Arch") && !(item as Object).hasBeenPickedUpByFarmer)
						{
							this.foundArtifact((item as Object).parentSheetIndex, 1);
						}
						if (item.parentSheetIndex == 102)
						{
							this.foundArtifact((item as Object).parentSheetIndex, 1);
							this.removeItemFromInventory(item);
						}
						else
						{
							int parentSheetIndex = item.parentSheetIndex;
							if (parentSheetIndex <= 380)
							{
								if (parentSheetIndex != 378)
								{
									if (parentSheetIndex == 380)
									{
										Game1.stats.IronFound += (uint)item.Stack;
									}
								}
								else
								{
									Game1.stats.CopperFound += (uint)item.Stack;
								}
							}
							else if (parentSheetIndex != 384)
							{
								if (parentSheetIndex == 386)
								{
									Game1.stats.IridiumFound += (uint)item.Stack;
								}
							}
							else
							{
								Game1.stats.GoldFound += (uint)item.Stack;
							}
						}
					}
					if (item is Object && !item.hasBeenInInventory)
					{
						if (!(item is Furniture) && !(item as Object).bigCraftable && !(item as Object).hasBeenPickedUpByFarmer)
						{
							this.checkForQuestComplete(null, (item as Object).parentSheetIndex, (item as Object).stack, item, null, 9, -1);
						}
						(item as Object).hasBeenPickedUpByFarmer = true;
						if ((item as Object).questItem)
						{
							return true;
						}
						if (Game1.activeClickableMenu == null)
						{
							int parentSheetIndex = (item as Object).parentSheetIndex;
							if (parentSheetIndex <= 378)
							{
								if (parentSheetIndex == 102)
								{
									Stats expr_382 = Game1.stats;
									uint num = expr_382.NotesFound;
									expr_382.NotesFound = num + 1u;
									Game1.playSound("newRecipe");
									this.holdUpItemThenMessage(item, true);
									return true;
								}
								if (parentSheetIndex == 378)
								{
									if (!Game1.player.hasOrWillReceiveMail("copperFound"))
									{
										Game1.addMailForTomorrow("copperFound", true, false);
									}
								}
							}
							else if (parentSheetIndex != 390)
							{
								if (parentSheetIndex == 535 && !Game1.player.hasOrWillReceiveMail("geodeFound"))
								{
									this.mailReceived.Add("geodeFound");
									this.holdUpItemThenMessage(item, true);
								}
							}
							else
							{
								Stats expr_3AC = Game1.stats;
								uint num = expr_3AC.StoneGathered;
								expr_3AC.StoneGathered = num + 1u;
								if (Game1.stats.StoneGathered >= 100u && !Game1.player.hasOrWillReceiveMail("robinWell"))
								{
									Game1.addMailForTomorrow("robinWell", false, false);
								}
							}
						}
					}
					Color color = Color.WhiteSmoke;
					string text = item.DisplayName;
					if (item is Object)
					{
						string type = (item as Object).type;
						if (!(type == "Arch"))
						{
							if (!(type == "Fish"))
							{
								if (!(type == "Mineral"))
								{
									if (!(type == "Vegetable"))
									{
										if (type == "Fruit")
										{
											color = Color.Pink;
										}
									}
									else
									{
										color = Color.PaleGreen;
									}
								}
								else
								{
									color = Color.PaleVioletRed;
								}
							}
							else
							{
								color = Color.SkyBlue;
							}
						}
						else
						{
							color = Color.Tan;
							text += Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1954", new object[0]);
						}
					}
					if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is ItemGrabMenu))
					{
						Game1.addHUDMessage(new HUDMessage(text, Math.Max(1, item.Stack), true, color, item));
					}
					this.mostRecentlyGrabbedItem = item;
					if ((item2 != null & makeActiveObject) && item.Stack <= 1)
					{
						int indexOfInventoryItem = this.getIndexOfInventoryItem(item);
						Item value = this.items[this.currentToolIndex];
						this.items[this.currentToolIndex] = this.items[indexOfInventoryItem];
						this.items[indexOfInventoryItem] = value;
					}
				}
				if (item is Object && !item.hasBeenInInventory)
				{
					this.checkForQuestComplete(null, item.parentSheetIndex, item.Stack, item, "", 10, -1);
				}
				item.hasBeenInInventory = true;
				return flag;
			}
			return false;
		}

		public int getIndexOfInventoryItem(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == item || (this.items[i] != null && item != null && item.canStackWith(this.items[i])))
				{
					return i;
				}
			}
			return -1;
		}

		public void reduceActiveItemByOne()
		{
			if (this.CurrentItem != null)
			{
				Item expr_0E = this.CurrentItem;
				int stack = expr_0E.Stack;
				expr_0E.Stack = stack - 1;
				if (this.CurrentItem.Stack <= 0)
				{
					this.removeItemFromInventory(this.CurrentItem);
					this.showNotCarrying();
				}
			}
		}

		public bool removeItemsFromInventory(int index, int stack)
		{
			if (this.hasItemInInventory(index, stack, 0))
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (this.items[i] != null && this.items[i] is Object && (this.items[i] as Object).parentSheetIndex == index)
					{
						if (this.items[i].Stack > stack)
						{
							this.items[i].Stack -= stack;
							return true;
						}
						stack -= this.items[i].Stack;
						this.items[i] = null;
					}
					if (stack <= 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public Item addItemToInventory(Item item, int position)
		{
			if (item != null && item is Object && (item as Object).specialItem)
			{
				if ((item as Object).bigCraftable)
				{
					if (!this.specialBigCraftables.Contains((item as Object).parentSheetIndex))
					{
						this.specialBigCraftables.Add((item as Object).parentSheetIndex);
					}
				}
				else if (!this.specialItems.Contains((item as Object).parentSheetIndex))
				{
					this.specialItems.Add((item as Object).parentSheetIndex);
				}
			}
			if (position < 0 || position >= this.items.Count)
			{
				return item;
			}
			if (this.items[position] == null)
			{
				this.items[position] = item;
				return null;
			}
			if (item == null || this.items[position].maximumStackSize() == -1 || !this.items[position].Name.Equals(item.Name) || (item is Object && this.items[position] is Object && (item as Object).quality != (this.items[position] as Object).quality))
			{
				Item arg_174_0 = this.items[position];
				this.items[position] = item;
				return arg_174_0;
			}
			int num = this.items[position].addToStack(item.getStack());
			if (num <= 0)
			{
				return null;
			}
			item.Stack = num;
			return item;
		}

		public void removeItemFromInventory(Item which)
		{
			int num = this.items.IndexOf(which);
			if (num >= 0 && num < this.items.Count)
			{
				this.items[this.items.IndexOf(which)] = null;
			}
		}

		public Item removeItemFromInventory(int whichItemIndex)
		{
			if (whichItemIndex >= 0 && whichItemIndex < this.items.Count && this.items[whichItemIndex] != null)
			{
				Item arg_39_0 = this.items[whichItemIndex];
				this.items[whichItemIndex] = null;
				return arg_39_0;
			}
			return null;
		}

		public bool isMarried()
		{
			return this.spouse != null && !this.spouse.Contains("engaged");
		}

		public void removeFirstOfThisItemFromInventory(int parentSheetIndexOfItem)
		{
			if (this.ActiveObject != null && this.ActiveObject.ParentSheetIndex == parentSheetIndexOfItem)
			{
				Object expr_1C = this.ActiveObject;
				int stack = expr_1C.Stack;
				expr_1C.Stack = stack - 1;
				if (this.ActiveObject.Stack <= 0)
				{
					this.ActiveObject = null;
					this.showNotCarrying();
					return;
				}
			}
			else
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (this.items[i] != null && this.items[i] is Object && ((Object)this.items[i]).ParentSheetIndex == parentSheetIndexOfItem)
					{
						Item expr_94 = this.items[i];
						int stack = expr_94.Stack;
						expr_94.Stack = stack - 1;
						if (this.items[i].Stack <= 0)
						{
							this.items[i] = null;
						}
						return;
					}
				}
			}
		}

		public bool hasCoopDweller(string type)
		{
			using (List<CoopDweller>.Enumerator enumerator = this.coopDwellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.type.Equals(type))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void changeShirt(int whichShirt)
		{
			if (whichShirt < 0)
			{
				whichShirt = FarmerRenderer.shirtsTexture.Height / 32 * (FarmerRenderer.shirtsTexture.Width / 8) - 1;
			}
			else if (whichShirt > FarmerRenderer.shirtsTexture.Height / 32 * (FarmerRenderer.shirtsTexture.Width / 8) - 1)
			{
				whichShirt = 0;
			}
			this.shirt = whichShirt;
			this.FarmerRenderer.changeShirt(whichShirt);
		}

		public void changeHairStyle(int whichHair)
		{
			if (whichHair < 0)
			{
				whichHair = FarmerRenderer.hairStylesTexture.Height / 96 * 8 - 1;
			}
			else if (whichHair > FarmerRenderer.hairStylesTexture.Height / 96 * 8 - 1)
			{
				whichHair = 0;
			}
			this.hair = whichHair;
		}

		public void changeShoeColor(int which)
		{
			this.FarmerRenderer.recolorShoes(which);
		}

		public void changeHairColor(Color c)
		{
			this.hairstyleColor = c;
		}

		public void changePants(Color color)
		{
			this.pantsColor = color;
		}

		public void changeHat(int newHat)
		{
			if (newHat < 0)
			{
				this.hat = null;
				return;
			}
			this.hat = new Hat(newHat);
		}

		public void changeAccessory(int which)
		{
			if (which < -1)
			{
				which = 18;
			}
			if (which >= -1)
			{
				if (which >= 19)
				{
					which = -1;
				}
				this.accessory = which;
			}
		}

		public void changeSkinColor(int which)
		{
			this.skin = this.FarmerRenderer.recolorSkin(which);
		}

		public bool hasDarkSkin()
		{
			return (this.skin >= 4 && this.skin <= 8) || this.skin == 14;
		}

		public void changeEyeColor(Color c)
		{
			this.newEyeColor = c;
			this.FarmerRenderer.recolorEyes(c);
		}

		public int getHair()
		{
			if (this.hat == null || this.hat.skipHairDraw || this.bathingClothes)
			{
				return this.hair;
			}
			switch (this.hair)
			{
			case 1:
			case 5:
			case 6:
			case 9:
			case 11:
				return this.hair;
			case 3:
				return 11;
			case 17:
			case 20:
			case 23:
			case 24:
			case 25:
			case 27:
			case 28:
			case 29:
			case 30:
				return this.hair;
			case 18:
			case 19:
			case 21:
			case 31:
				return 23;
			}
			if (this.hair >= 16)
			{
				return 30;
			}
			return 7;
		}

		public void changeGender(bool male)
		{
			if (male)
			{
				this.isMale = true;
				this.FarmerRenderer.baseTexture = this.getTexture();
				this.FarmerRenderer.heightOffset = 0;
			}
			else
			{
				this.isMale = false;
				this.FarmerRenderer.heightOffset = 4;
				this.FarmerRenderer.baseTexture = this.getTexture();
			}
			this.changeShirt(this.shirt);
			this.changeEyeColor(this.newEyeColor);
		}

		public bool hasBarnDweller(string type)
		{
			using (List<BarnDweller>.Enumerator enumerator = this.barnDwellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.type.Equals(type))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void changeFriendship(int amount, NPC n)
		{
			if (amount > 0 && n.name.Equals("Dwarf") && !this.canUnderstandDwarves)
			{
				return;
			}
			if (this.friendships.ContainsKey(n.name))
			{
				if (!n.datable || n.datingFarmer || (this.spouse != null && this.spouse.Equals(n.name)) || this.friendships[n.name][0] < 2000)
				{
					this.friendships[n.name][0] = Math.Max(0, Math.Min(this.friendships[n.name][0] + amount, ((this.spouse != null && n.name.Equals(this.spouse)) ? 14 : 11) * 250 - 1));
					if (n.datable && !n.datingFarmer && (this.spouse == null || !this.spouse.Equals(n.name)))
					{
						this.friendships[n.name][0] = Math.Min(2498, this.friendships[n.name][0]);
					}
				}
				if (n.datable && this.friendships[n.name][0] >= 2000 && !this.hasOrWillReceiveMail("Bouquet"))
				{
					Game1.addMailForTomorrow("Bouquet", false, false);
				}
				if (n.datable && this.friendships[n.name][0] >= 2500 && !this.hasOrWillReceiveMail("SeaAmulet"))
				{
					Game1.addMailForTomorrow("SeaAmulet", false, false);
					return;
				}
			}
			else
			{
				Game1.debugOutput = "Tried to change friendship for a friend that wasn't there.";
			}
		}

		public bool knowsRecipe(string name)
		{
			return this.craftingRecipes.Keys.Contains(name.Replace(" Recipe", "")) || this.cookingRecipes.Keys.Contains(name.Replace(" Recipe", ""));
		}

		public Vector2 getUniformPositionAwayFromBox(int direction, int distance)
		{
			switch (this.facingDirection)
			{
			case 0:
				return new Vector2((float)this.GetBoundingBox().Center.X, (float)(this.GetBoundingBox().Y - distance));
			case 1:
				return new Vector2((float)(this.GetBoundingBox().Right + distance), (float)this.GetBoundingBox().Center.Y);
			case 2:
				return new Vector2((float)this.GetBoundingBox().Center.X, (float)(this.GetBoundingBox().Bottom + distance));
			case 3:
				return new Vector2((float)(this.GetBoundingBox().X - distance), (float)this.GetBoundingBox().Center.Y);
			default:
				return Vector2.Zero;
			}
		}

		public bool hasTalkedToFriendToday(string npcName)
		{
			return this.friendships.ContainsKey(npcName) && this.friendships[npcName][2] == 1;
		}

		public void talkToFriend(NPC n, int friendshipPointChange = 20)
		{
			if (this.friendships.ContainsKey(n.name) && this.friendships[n.name][2] == 0)
			{
				this.changeFriendship(friendshipPointChange, n);
				this.friendships[n.name][2] = 1;
			}
		}

		public void moveRaft(GameLocation currentLocation, GameTime time)
		{
			float num = 0.2f;
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton))
			{
				this.yVelocity = Math.Max(this.yVelocity - num, -3f + Math.Abs(this.xVelocity) / 2f);
				this.faceDirection(0);
			}
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton))
			{
				this.xVelocity = Math.Min(this.xVelocity + num, 3f - Math.Abs(this.yVelocity) / 2f);
				this.faceDirection(1);
			}
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton))
			{
				this.yVelocity = Math.Min(this.yVelocity + num, 3f - Math.Abs(this.xVelocity) / 2f);
				this.faceDirection(2);
			}
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton))
			{
				this.xVelocity = Math.Max(this.xVelocity - num, -3f + Math.Abs(this.yVelocity) / 2f);
				this.faceDirection(3);
			}
			Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int)this.position.X, (int)(this.position.Y + (float)Game1.tileSize + (float)(Game1.tileSize / 4)), Game1.tileSize, Game1.tileSize);
			rectangle.X += (int)Math.Ceiling((double)this.xVelocity);
			if (!currentLocation.isCollidingPosition(rectangle, Game1.viewport, true))
			{
				this.position.X = this.position.X + this.xVelocity;
			}
			rectangle.X -= (int)Math.Ceiling((double)this.xVelocity);
			rectangle.Y += (int)Math.Floor((double)this.yVelocity);
			if (!currentLocation.isCollidingPosition(rectangle, Game1.viewport, true))
			{
				this.position.Y = this.position.Y + this.yVelocity;
			}
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				this.raftPuddleCounter -= time.ElapsedGameTime.Milliseconds;
				if (this.raftPuddleCounter <= 0)
				{
					this.raftPuddleCounter = 250;
					currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2((float)rectangle.X, (float)(rectangle.Y - Game1.tileSize)), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
					if (Game1.random.NextDouble() < 0.6)
					{
						Game1.playSound("wateringCan");
					}
					if (Game1.random.NextDouble() < 0.6)
					{
						this.raftBobCounter /= 2;
					}
				}
			}
			this.raftBobCounter -= time.ElapsedGameTime.Milliseconds;
			if (this.raftBobCounter <= 0)
			{
				this.raftBobCounter = Game1.random.Next(15, 28) * 100;
				if (this.yOffset <= 0f)
				{
					this.yOffset = 4f;
					currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2((float)rectangle.X, (float)(rectangle.Y - Game1.tileSize)), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
				}
				else
				{
					this.yOffset = 0f;
				}
			}
			if (this.xVelocity > 0f)
			{
				this.xVelocity = Math.Max(0f, this.xVelocity - num / 2f);
			}
			else if (this.xVelocity < 0f)
			{
				this.xVelocity = Math.Min(0f, this.xVelocity + num / 2f);
			}
			if (this.yVelocity > 0f)
			{
				this.yVelocity = Math.Max(0f, this.yVelocity - num / 2f);
				return;
			}
			if (this.yVelocity < 0f)
			{
				this.yVelocity = Math.Min(0f, this.yVelocity + num / 2f);
			}
		}

		public void warpFarmer(Warp w)
		{
			if (w != null && !Game1.eventUp)
			{
				this.Halt();
				Game1.warpFarmer(w.TargetName, w.TargetX, w.TargetY, w.flipFarmer);
				if ((Game1.currentLocation.Name.Equals("Town") || Game1.jukeboxPlaying) && Game1.getLocationFromName(w.TargetName).IsOutdoors && Game1.currentSong != null && (Game1.currentSong.Name.Contains("town") || Game1.jukeboxPlaying))
				{
					Game1.changeMusicTrack("none");
				}
			}
		}

		public static void passOutFromTired(Farmer who)
		{
			if (who.isRidingHorse())
			{
				who.getMount().dismount();
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.emergencyShutDown();
				Game1.exitActiveMenu();
			}
			Game1.warpFarmer(Utility.getHomeOfFarmer(who), (int)who.mostRecentBed.X / Game1.tileSize, (int)who.mostRecentBed.Y / Game1.tileSize, 2, false);
			Game1.newDay = true;
			who.currentLocation.lastTouchActionLocation = new Vector2((float)((int)who.mostRecentBed.X / Game1.tileSize), (float)((int)who.mostRecentBed.Y / Game1.tileSize));
			who.completelyStopAnimatingOrDoingAction();
			if (who.bathingClothes)
			{
				who.changeOutOfSwimSuit();
			}
			who.swimming = false;
			Game1.player.CanMove = false;
			Game1.changeMusicTrack("none");
			if (!(who.currentLocation is FarmHouse) && !(who.currentLocation is Cellar))
			{
				int num = Math.Min(1000, who.Money / 10);
				who.Money -= num;
				who.mailForTomorrow.Add("passedOut " + num);
			}
			who.FarmerSprite.setCurrentSingleFrame(5, 3000, false, false);
		}

		public static void doSleepEmote(Farmer who)
		{
			who.doEmote(24);
			who.yJumpVelocity = -2f;
		}

		public override Microsoft.Xna.Framework.Rectangle GetBoundingBox()
		{
			if (this.mount != null && !this.mount.dismounting)
			{
				return this.mount.GetBoundingBox();
			}
			return new Microsoft.Xna.Framework.Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, Game1.tileSize * 3 / 4, Game1.tileSize / 2);
		}

		public string getPetName()
		{
			foreach (NPC current in Game1.getFarm().characters)
			{
				if (current is Pet)
				{
					string name = current.name;
					return name;
				}
			}
			foreach (NPC current2 in Utility.getHomeOfFarmer(this).characters)
			{
				if (current2 is Pet)
				{
					string name = current2.name;
					return name;
				}
			}
			return "the Farm";
		}

		public string getPetDisplayName()
		{
			foreach (NPC current in Game1.getFarm().characters)
			{
				if (current is Pet)
				{
					string displayName = current.displayName;
					return displayName;
				}
			}
			foreach (NPC current2 in Utility.getHomeOfFarmer(this).characters)
			{
				if (current2 is Pet)
				{
					string displayName = current2.displayName;
					return displayName;
				}
			}
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1972", new object[0]);
		}

		public bool hasPet()
		{
			using (List<NPC>.Enumerator enumerator = Game1.getFarm().characters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Pet)
					{
						bool result = true;
						return result;
					}
				}
			}
			using (List<NPC>.Enumerator enumerator = Utility.getHomeOfFarmer(this).characters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Pet)
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		public bool movedDuringLastTick()
		{
			return !this.position.Equals(this.lastPosition);
		}

		public int CompareTo(object obj)
		{
			return ((Farmer)obj).saveTime - this.saveTime;
		}

		public override void draw(SpriteBatch b)
		{
			if (this.currentLocation == null || (!this.currentLocation.Equals(Game1.currentLocation) && !this.IsMainPlayer))
			{
				return;
			}
			Vector2 origin = new Vector2(this.xOffset, (this.yOffset + (float)(Game1.tileSize * 2) - (float)(this.GetBoundingBox().Height / 2)) / (float)Game1.pixelZoom + (float)Game1.pixelZoom);
			this.numUpdatesSinceLastDraw = 0;
			PropertyValue propertyValue = null;
			Tile tile = Game1.currentLocation.Map.GetLayer("Buildings").PickTile(new Location(base.getStandingX(), base.getStandingY()), Game1.viewport.Size);
			if (this.isGlowing && this.coloredBorder)
			{
				b.Draw(base.Sprite.Texture, new Vector2(base.getLocalPosition(Game1.viewport).X - (float)Game1.pixelZoom, base.getLocalPosition(Game1.viewport).Y - (float)Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, Math.Max(0f, (float)base.getStandingY() / 10000f - 0.001f));
			}
			else if (this.isGlowing && !this.coloredBorder)
			{
				this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.00011f), this.glowingColor * this.glowingTransparency, this.rotation, this);
			}
			if (tile != null)
			{
				tile.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
			}
			if (propertyValue == null)
			{
				if (!this.temporarilyInvincible || this.temporaryInvincibilityTimer % 100 < 50)
				{
					this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport) + this.jitter + new Vector2(0f, (float)this.yJumpOffset), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.0001f), Color.White, this.rotation, this);
				}
			}
			else
			{
				this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.0001f), Color.White, this.rotation, this);
				this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.0002f), Color.Black * 0.25f, this.rotation, this);
			}
			if (this.isRafting)
			{
				b.Draw(Game1.toolSpriteSheet, base.getLocalPosition(Game1.viewport) + new Vector2(0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.toolSpriteSheet, 1, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)base.getStandingY() / 10000f - 0.001f);
			}
			if (Game1.activeClickableMenu == null && !Game1.eventUp && this.IsMainPlayer && this.CurrentTool != null && (Game1.oldKBState.IsKeyDown(Keys.LeftShift) || Game1.options.alwaysShowToolHitLocation) && this.CurrentTool.doesShowTileLocationMarker() && (!Game1.options.hideToolHitLocationWhenInMotion || !this.isMoving()))
			{
				Vector2 position = Game1.GlobalToLocal(Game1.viewport, (Utility.withinRadiusOfPlayer(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 1, this) && !Game1.options.gamepadControls) ? (new Vector2((float)((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize), (float)((Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize)) * (float)Game1.tileSize) : Utility.clampToTile(this.GetToolLocation(false)));
				if (!Game1.wasMouseVisibleThisFrame || Game1.isAnyGamePadButtonBeingPressed())
				{
					position = Game1.GlobalToLocal(Game1.viewport, Utility.clampToTile(this.GetToolLocation(false)));
				}
				b.Draw(Game1.mouseCursors, position, new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (this.GetToolLocation(false).Y + (float)Game1.tileSize) / 10000f);
			}
			if (base.IsEmoting)
			{
				Vector2 localPosition = base.getLocalPosition(Game1.viewport);
				localPosition.Y -= (float)(Game1.tileSize * 2 + Game1.tileSize / 2);
				b.Draw(Game1.emoteSpriteSheet, localPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(base.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)base.getStandingY() / 10000f);
			}
			if (this.ActiveObject != null)
			{
				Game1.drawPlayerHeldObject(this);
			}
			if (!this.IsMainPlayer)
			{
				if (this.FarmerSprite.isOnToolAnimation())
				{
					Game1.drawTool(this, this.FarmerSprite.CurrentToolIndex);
				}
				if (new Microsoft.Xna.Framework.Rectangle((int)this.position.X - Game1.viewport.X, (int)this.position.Y - Game1.viewport.Y, Game1.tileSize, Game1.tileSize * 3 / 2).Contains(new Point(Game1.getOldMouseX(), Game1.getOldMouseY())))
				{
					Game1.drawWithBorder(base.displayName, Color.Black, Color.White, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(base.displayName).X / 2f, -Game1.dialogueFont.MeasureString(base.displayName).Y));
				}
			}
			if (this.sparklingText != null)
			{
				this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(Game1.tileSize / 2) - this.sparklingText.textWidth / 2f, (float)(-(float)Game1.tileSize * 2))));
			}
		}

		public static void drinkGlug(Farmer who)
		{
			Color color = Color.LightBlue;
			if (who.itemToEat != null)
			{
				string text = who.itemToEat.Name.Split(new char[]
				{
					' '
				}).Last<string>();
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 2470525844u)
				{
					if (num <= 948615682u)
					{
						if (num != 154965655u)
						{
							if (num != 948615682u)
							{
								goto IL_168;
							}
							if (!(text == "Tonic"))
							{
								goto IL_168;
							}
							color = Color.Red;
							goto IL_168;
						}
						else
						{
							if (!(text == "Beer"))
							{
								goto IL_168;
							}
							color = Color.Orange;
							goto IL_168;
						}
					}
					else if (num != 1702016080u)
					{
						if (num != 2470525844u)
						{
							goto IL_168;
						}
						if (!(text == "Wine"))
						{
							goto IL_168;
						}
						color = Color.Purple;
						goto IL_168;
					}
					else if (!(text == "Cola"))
					{
						goto IL_168;
					}
				}
				else if (num <= 3224132511u)
				{
					if (num != 2679015821u)
					{
						if (num != 3224132511u)
						{
							goto IL_168;
						}
						if (!(text == "Juice"))
						{
							goto IL_168;
						}
						color = Color.LightGreen;
						goto IL_168;
					}
					else
					{
						if (!(text == "Remedy"))
						{
							goto IL_168;
						}
						color = Color.LimeGreen;
						goto IL_168;
					}
				}
				else if (num != 3560961217u)
				{
					if (num != 4017071298u)
					{
						goto IL_168;
					}
					if (!(text == "Milk"))
					{
						goto IL_168;
					}
					color = Color.White;
					goto IL_168;
				}
				else if (!(text == "Coffee"))
				{
					goto IL_168;
				}
				color = new Color(46, 20, 0);
			}
			IL_168:
			Game1.playSound("gulp");
			who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(653, 858, 1, 1), 9999f, 1, 1, who.position + new Vector2((float)(32 + Game1.random.Next(-2, 3) * 4), -48f), false, false, (float)who.getStandingY() / 10000f + 0.001f, 0.04f, color, 5f, 0f, 0f, 0f, false)
			{
				acceleration = new Vector2(0f, 0.5f)
			});
		}

		public bool isDivorced()
		{
			using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.divorcedFromFarmer)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void wipeExMemories()
		{
			foreach (NPC current in Utility.getAllCharacters())
			{
				if (current.divorcedFromFarmer)
				{
					current.divorcedFromFarmer = false;
					current.datingFarmer = false;
					current.daysMarried = 0;
					try
					{
						this.friendships[current.name][0] = 0;
						this.friendships[current.name][1] = 0;
						this.friendships[current.name][2] = 0;
						this.friendships[current.name][3] = 0;
						this.friendships[current.name][4] = 0;
					}
					catch (Exception)
					{
					}
					current.CurrentDialogue.Clear();
					current.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:WipedMemory", new object[0]), current));
				}
			}
		}

		public void getRidOfChildren()
		{
			for (int i = Utility.getHomeOfFarmer(this).characters.Count<NPC>() - 1; i >= 0; i--)
			{
				if (Utility.getHomeOfFarmer(this).characters[i] is Child && (Utility.getHomeOfFarmer(this).characters[i] as Child).isChildOf(this))
				{
					Utility.getHomeOfFarmer(this).characters.RemoveAt(i);
				}
			}
			for (int j = Game1.getLocationFromName("Farm").characters.Count<NPC>() - 1; j >= 0; j--)
			{
				if (Game1.getLocationFromName("Farm").characters[j] is Child && (Game1.getLocationFromName("Farm").characters[j] as Child).isChildOf(this))
				{
					Game1.getLocationFromName("Farm").characters.RemoveAt(j);
				}
			}
		}

		public void animateOnce(int whichAnimation)
		{
			this.FarmerSprite.animateOnce(whichAnimation, 100f, 6);
			this.CanMove = false;
		}

		public static void showItemIntake(Farmer who)
		{
			TemporaryAnimatedSprite temporaryAnimatedSprite = null;
			Object @object = (who.mostRecentlyGrabbedItem == null || !(who.mostRecentlyGrabbedItem is Object)) ? ((who.ActiveObject == null) ? null : who.ActiveObject) : ((Object)who.mostRecentlyGrabbedItem);
			if (@object == null)
			{
				return;
			}
			switch (who.facingDirection)
			{
			case 0:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 2)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 3)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			case 1:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(Game1.tileSize / 2 - 4), (float)(-(float)Game1.tileSize)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(Game1.tileSize / 2 - 8), (float)(-(float)Game1.tileSize - 8)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(4f, (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			case 2:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 3)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			case 3:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 2 + 4), (float)(-(float)Game1.tileSize - 12)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			}
			if ((@object.Equals(who.ActiveObject) || (who.ActiveObject != null && @object != null && @object.ParentSheetIndex == who.ActiveObject.parentSheetIndex)) && who.FarmerSprite.indexInCurrentAnimation == 5)
			{
				temporaryAnimatedSprite = null;
			}
			if (temporaryAnimatedSprite != null)
			{
				who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite);
			}
			if (who.mostRecentlyGrabbedItem is ColoredObject && temporaryAnimatedSprite != null)
			{
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex + 1, 16, 16), temporaryAnimatedSprite.interval, 1, 0, temporaryAnimatedSprite.Position, false, false, temporaryAnimatedSprite.layerDepth + 0.0001f, temporaryAnimatedSprite.alphaFade, (who.mostRecentlyGrabbedItem as ColoredObject).color, (float)Game1.pixelZoom, temporaryAnimatedSprite.scaleChange, 0f, 0f, false));
			}
			if (who.FarmerSprite.indexInCurrentAnimation == 5)
			{
				who.Halt();
				who.FarmerSprite.CurrentAnimation = null;
			}
		}

		public static void showSwordSwipe(Farmer who)
		{
			TemporaryAnimatedSprite temporaryAnimatedSprite = null;
			bool flag = who.CurrentTool != null && who.CurrentTool is MeleeWeapon && (who.CurrentTool as MeleeWeapon).type == 1;
			Vector2 toolLocation = who.GetToolLocation(true);
			if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon)
			{
				(who.CurrentTool as MeleeWeapon).DoDamage(who.currentLocation, (int)toolLocation.X, (int)toolLocation.Y, who.facingDirection, 1, who);
			}
			switch (who.facingDirection)
			{
			case 0:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.yVelocity = -0.3f;
							temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(0f, -32f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								alpha = 0.5f,
								rotation = 3.926991f
							};
						}
					}
					else
					{
						who.yVelocity = (flag ? -0.5f : 0.5f);
					}
				}
				else if (flag)
				{
					who.yVelocity = 0.6f;
				}
				break;
			}
			case 1:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.xVelocity = -0.3f;
							temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(4f, -12f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								alpha = 0.5f
							};
						}
					}
					else
					{
						who.xVelocity = (flag ? -0.5f : 0.5f);
					}
				}
				else if (flag)
				{
					who.xVelocity = 0.6f;
				}
				break;
			}
			case 2:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.yVelocity = 0.3f;
							temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(503, 256, 42, 17), who.position + new Vector2(-16f, -2f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								alpha = 0.5f,
								layerDepth = (who.position.Y + (float)Game1.tileSize) / 10000f
							};
						}
					}
					else
					{
						who.yVelocity = (flag ? 0.5f : -0.5f);
					}
				}
				else if (flag)
				{
					who.yVelocity = -0.6f;
				}
				break;
			}
			case 3:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.xVelocity = 0.3f;
							temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(-15f, -12f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								flipped = true,
								alpha = 0.5f
							};
						}
					}
					else
					{
						who.xVelocity = (flag ? 0.5f : -0.5f);
					}
				}
				else if (flag)
				{
					who.xVelocity = -0.6f;
				}
				break;
			}
			}
			if (temporaryAnimatedSprite != null)
			{
				if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon && who.CurrentTool.initialParentTileIndex == 4)
				{
					temporaryAnimatedSprite.color = Color.HotPink;
				}
				who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite);
			}
		}

		public static void showToolSwipeEffect(Farmer who)
		{
			if (who.CurrentTool != null && who.CurrentTool is WateringCan)
			{
				int arg_1B_0 = who.FacingDirection;
				return;
			}
			switch (who.FacingDirection)
			{
			case 0:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(18, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 - 4)), Color.White, 4, false, (who.stamina <= 0f) ? 100f : 50f, 0, Game1.tileSize, 1f, Game1.tileSize, 0)
				{
					layerDepth = (float)(who.getStandingY() - Game1.tileSize / 7) / 10000f
				});
				return;
			case 1:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(15, who.position + new Vector2(20f, (float)(-(float)Game1.tileSize * 2 - 4)), Color.White, 4, false, (who.stamina <= 0f) ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
				{
					layerDepth = (float)(who.GetBoundingBox().Bottom + 1) / 10000f
				});
				return;
			case 2:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(19, who.position + new Vector2(-4f, (float)(-(float)Game1.tileSize * 2)), Color.White, 4, false, (who.stamina <= 0f) ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
				{
					layerDepth = (float)(who.GetBoundingBox().Bottom + 1) / 10000f
				});
				return;
			case 3:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(15, who.position + new Vector2((float)(-(float)Game1.tileSize - 28), (float)(-(float)Game1.tileSize * 2 - 4)), Color.White, 4, true, (who.stamina <= 0f) ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
				{
					layerDepth = (float)(who.GetBoundingBox().Bottom + 1) / 10000f
				});
				return;
			default:
				return;
			}
		}

		public static void canMoveNow(Farmer who)
		{
			who.CanMove = true;
			who.usingTool = false;
			who.usingSlingshot = false;
			who.FarmerSprite.pauseForSingleAnimation = false;
			who.yVelocity = 0f;
			who.xVelocity = 0f;
		}

		public static void useTool(Farmer who)
		{
			if (who.toolOverrideFunction != null)
			{
				who.toolOverrideFunction(who);
				return;
			}
			if (who.CurrentTool != null)
			{
				float oldStamina = who.stamina;
				who.CurrentTool.DoFunction(who.currentLocation, (int)who.GetToolLocation(false).X, (int)who.GetToolLocation(false).Y, 1, who);
				who.lastClick = Vector2.Zero;
				who.checkForExhaustion(oldStamina);
				Game1.toolHold = 0f;
			}
		}

		public void checkForExhaustion(float oldStamina)
		{
			if (this.stamina <= 0f && oldStamina > 0f)
			{
				if (!this.exhausted)
				{
					Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1986", new object[0]));
				}
				this.setRunning(false, false);
				this.doEmote(36);
			}
			else if (this.stamina <= 15f && oldStamina > 15f)
			{
				Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1987", new object[0]));
			}
			if (this.stamina <= 0f)
			{
				this.exhausted = true;
			}
			if (this.stamina <= -15f)
			{
				Game1.farmerShouldPassOut = true;
			}
		}

		public void setMoving(byte command)
		{
			bool flag = false;
			if (this.movementDirections.Count < 2)
			{
				if (command == 1 && !this.movementDirections.Contains(0) && !this.movementDirections.Contains(2))
				{
					this.movementDirections.Insert(0, 0);
					flag = true;
				}
				if (command == 2 && !this.movementDirections.Contains(1) && !this.movementDirections.Contains(3))
				{
					this.movementDirections.Insert(0, 1);
					flag = true;
				}
				if (command == 4 && !this.movementDirections.Contains(2) && !this.movementDirections.Contains(0))
				{
					this.movementDirections.Insert(0, 2);
					flag = true;
				}
				if (command == 8 && !this.movementDirections.Contains(3) && !this.movementDirections.Contains(1))
				{
					this.movementDirections.Insert(0, 3);
					flag = true;
				}
			}
			if (command == 33)
			{
				this.movementDirections.Remove(0);
				flag = true;
			}
			if (command == 34)
			{
				this.movementDirections.Remove(1);
				flag = true;
			}
			if (command == 36)
			{
				this.movementDirections.Remove(2);
				flag = true;
			}
			if (command == 40)
			{
				this.movementDirections.Remove(3);
				flag = true;
			}
			if (command == 16)
			{
				this.setRunning(true, false);
				flag = true;
			}
			else if (command == 48)
			{
				this.setRunning(false, false);
				flag = true;
			}
			if ((command & 64) == 64)
			{
				this.Halt();
				this.running = false;
				flag = true;
			}
			if ((Game1.IsClient & flag) && (command & 32) != 32)
			{
				this.timeOfLastPositionPacket = 60f;
			}
			if (Game1.IsServer & flag)
			{
				MultiplayerUtility.broadcastFarmerMovement(this.uniqueMultiplayerID, command, this.currentLocation.name);
			}
		}

		public void toolPowerIncrease()
		{
			if (this.toolPower == 0)
			{
				this.toolPitchAccumulator = 0;
			}
			this.toolPower++;
			if (this.CurrentTool is Pickaxe && this.toolPower == 1)
			{
				this.toolPower += 2;
			}
			Color color = Color.White;
			int num = (base.FacingDirection == 0) ? 4 : ((base.FacingDirection == 2) ? 2 : 0);
			switch (this.toolPower)
			{
			case 1:
				color = Color.Orange;
				if (!(this.CurrentTool is WateringCan))
				{
					this.FarmerSprite.CurrentFrame = 72 + num;
				}
				this.jitterStrength = 0.25f;
				break;
			case 2:
				color = Color.LightSteelBlue;
				if (!(this.CurrentTool is WateringCan))
				{
					FarmerSprite expr_CE = this.FarmerSprite;
					int currentFrame = expr_CE.CurrentFrame;
					expr_CE.CurrentFrame = currentFrame + 1;
				}
				this.jitterStrength = 0.5f;
				break;
			case 3:
				color = Color.Gold;
				this.jitterStrength = 1f;
				break;
			case 4:
				color = Color.Violet;
				this.jitterStrength = 2f;
				break;
			}
			int num2 = (base.FacingDirection == 1) ? Game1.tileSize : ((base.FacingDirection == 3) ? (-Game1.tileSize) : ((base.FacingDirection == 2) ? (Game1.tileSize / 2) : 0));
			int num3 = Game1.tileSize * 3;
			if (this.CurrentTool is WateringCan)
			{
				num2 = -num2;
				num3 = Game1.tileSize * 2;
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(21, this.position - new Vector2((float)num2, (float)num3), color, 8, false, 70f, 0, Game1.tileSize, (float)base.getStandingY() / 10000f + 0.005f, Game1.tileSize * 2, 0));
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(192, 1152, Game1.tileSize, Game1.tileSize), 50f, 4, 0, this.position - new Vector2((float)((base.FacingDirection == 1) ? 0 : (-(float)Game1.tileSize)), (float)(Game1.tileSize * 2)), false, base.FacingDirection == 1, (float)base.getStandingY() / 10000f, 0.01f, Color.White, 1f, 0f, 0f, 0f, false));
			if (Game1.soundBank != null)
			{
				Cue arg_294_0 = Game1.soundBank.GetCue("toolCharge");
				Random random = new Random(Game1.dayOfMonth + (int)this.position.X * 1000 + (int)this.position.Y);
				arg_294_0.SetVariable("Pitch", (float)(random.Next(12, 16) * 100 + this.toolPower * 100));
				arg_294_0.Play();
			}
		}

		public override void updatePositionFromServer(Vector2 position)
		{
			if (!Game1.eventUp || Game1.currentLocation.currentEvent.playerControlSequence)
			{
				this.remotePosition = position;
				if (Game1.IsClient && Game1.client.isConnected)
				{
					float num = Game1.client.averageRoundtripTime / 2f * 60f;
					if (this.movementDirections.Contains(0))
					{
						this.remotePosition.Y = this.remotePosition.Y - num / 64f * this.getMovementSpeed();
					}
					if (this.movementDirections.Contains(1))
					{
						this.remotePosition.X = this.remotePosition.X + num / 64f * this.getMovementSpeed();
					}
					if (this.movementDirections.Contains(2))
					{
						this.remotePosition.Y = this.remotePosition.Y + num / 64f * this.getMovementSpeed();
					}
					if (this.movementDirections.Contains(3))
					{
						this.remotePosition.X = this.remotePosition.X - num / 64f * this.getMovementSpeed();
					}
				}
			}
		}

		public override void lerpPosition(Vector2 target)
		{
			if (target.Equals(Vector2.Zero))
			{
				return;
			}
			int num = (int)(target.X - this.position.X);
			if (Math.Abs(num) > Game1.tileSize * 8)
			{
				this.position.X = target.X;
			}
			else
			{
				this.position.X = this.position.X + (float)num * this.getMovementSpeed() * Math.Min(0.04f, this.timeOfLastPositionPacket / 40000f);
			}
			num = (int)(target.Y - this.position.Y);
			if (Math.Abs(num) > Game1.tileSize * 8)
			{
				this.position.Y = target.Y;
				return;
			}
			this.position.Y = this.position.Y + (float)num * this.getMovementSpeed() * Math.Min(0.04f, this.timeOfLastPositionPacket / 40000f);
		}

		public void UpdateIfOtherPlayer(GameTime time)
		{
			if (this.currentLocation == null)
			{
				return;
			}
			this.FarmerSprite.setOwner(this);
			this.FarmerSprite.checkForSingleAnimation(time);
			this.timeOfLastPositionPacket += (float)time.ElapsedGameTime.Milliseconds;
			if (!Game1.eventUp || Game1.currentLocation.currentEvent.playerControlSequence)
			{
				this.lerpPosition(this.remotePosition);
			}
			Vector2 arg_67_0 = this.position;
			this.MovePosition(time, Game1.viewport, this.currentLocation);
			this.rotation = 0f;
			if (this.movementDirections.Count == 0 && !this.FarmerSprite.pauseForSingleAnimation && !this.UsingTool)
			{
				this.sprite.StopAnimation();
			}
			if (Game1.IsServer && this.movementDirections.Count > 0)
			{
				MultiplayerUtility.broadcastFarmerPosition(this.uniqueMultiplayerID, this.position, this.currentLocation.name);
			}
			if (this.movementDirections.Count > 0)
			{
				Game1.debugOutput = this.position.ToString();
			}
			else
			{
				Game1.debugOutput = "no movemement";
			}
			if (this.CurrentTool != null)
			{
				this.CurrentTool.tickUpdate(time, this);
			}
			else if (this.ActiveObject != null)
			{
				this.ActiveObject.actionWhenBeingHeld(this);
			}
			base.updateEmote(time);
			base.updateGlow();
		}

		public void forceCanMove()
		{
			this.forceTimePass = false;
			this.movementDirections.Clear();
			Game1.isEating = false;
			this.CanMove = true;
			Game1.freezeControls = false;
			this.freezePause = 0;
			this.usingTool = false;
			this.usingSlingshot = false;
			this.FarmerSprite.pauseForSingleAnimation = false;
			if (this.CurrentTool is FishingRod)
			{
				(this.CurrentTool as FishingRod).isFishing = false;
			}
		}

		public void dropItem(Item i)
		{
			if (i != null && i.canBeDropped())
			{
				Game1.createItemDebris(i.getOne(), base.getStandingPosition(), base.FacingDirection, null);
			}
		}

		public bool addEvent(string eventName, int daysActive)
		{
			if (!this.activeDialogueEvents.ContainsKey(eventName))
			{
				this.activeDialogueEvents.Add(eventName, daysActive);
				return true;
			}
			return false;
		}

		public void dropObjectFromInventory(int parentSheetIndex, int quantity)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null && this.items[i] is Object && (this.items[i] as Object).parentSheetIndex == parentSheetIndex)
				{
					while (quantity > 0)
					{
						this.dropItem(this.items[i].getOne());
						Item expr_69 = this.items[i];
						int stack = expr_69.Stack;
						expr_69.Stack = stack - 1;
						quantity--;
						if (this.items[i].Stack <= 0)
						{
							this.items[i] = null;
							break;
						}
					}
					if (quantity <= 0)
					{
						return;
					}
				}
			}
		}

		public Vector2 getMostRecentMovementVector()
		{
			return new Vector2(this.position.X - this.lastPosition.X, this.position.Y - this.lastPosition.Y);
		}

		public void dropActiveItem()
		{
			if (this.CurrentItem != null && this.CurrentItem.canBeDropped())
			{
				Game1.createItemDebris(this.CurrentItem.getOne(), base.getStandingPosition(), base.FacingDirection, null);
				this.reduceActiveItemByOne();
			}
		}

		public static string getSkillNameFromIndex(int index)
		{
			switch (index)
			{
			case 0:
				return "Farming";
			case 1:
				return "Fishing";
			case 2:
				return "Foraging";
			case 3:
				return "Mining";
			case 4:
				return "Combat";
			case 5:
				return "Luck";
			default:
				return "";
			}
		}

		public static string getSkillDisplayNameFromIndex(int index)
		{
			switch (index)
			{
			case 0:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1991", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1993", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1994", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1992", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1996", new object[0]);
			case 5:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1995", new object[0]);
			default:
				return "";
			}
		}

		public override bool isMoving()
		{
			return this.movementDirections.Count > 0;
		}

		public bool hasCompletedCommunityCenter()
		{
			return this.mailReceived.Contains("ccBoilerRoom") && this.mailReceived.Contains("ccCraftsRoom") && this.mailReceived.Contains("ccPantry") && this.mailReceived.Contains("ccFishTank") && this.mailReceived.Contains("ccVault") && this.mailReceived.Contains("ccBulletin");
		}

		public void Update(GameTime time, GameLocation location)
		{
			if (Game1.CurrentEvent == null && !this.bathingClothes)
			{
				this.canOnlyWalk = false;
			}
			if (this.exhausted && this.stamina <= 1f)
			{
				this.currentEyes = 4;
				this.blinkTimer = -1000;
			}
			if (this.noMovementPause > 0)
			{
				this.CanMove = false;
				this.noMovementPause -= time.ElapsedGameTime.Milliseconds;
				if (this.noMovementPause <= 0)
				{
					this.CanMove = true;
				}
			}
			if (this.freezePause > 0)
			{
				this.CanMove = false;
				this.freezePause -= time.ElapsedGameTime.Milliseconds;
				if (this.freezePause <= 0)
				{
					this.CanMove = true;
				}
			}
			if (this.sparklingText != null && this.sparklingText.update(time))
			{
				this.sparklingText = null;
			}
			if (this.newLevelSparklingTexts.Count > 0 && this.sparklingText == null && !this.usingTool && this.CanMove && Game1.activeClickableMenu == null)
			{
				this.sparklingText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2003", new object[]
				{
					Farmer.getSkillDisplayNameFromIndex(this.newLevelSparklingTexts.Peek())
				}), Color.White, Color.White, true, 0.1, 2500, -1, 500);
				this.newLevelSparklingTexts.Dequeue();
			}
			if (this.jitterStrength > 0f)
			{
				this.jitter = new Vector2((float)Game1.random.Next(-(int)(this.jitterStrength * 100f), (int)((this.jitterStrength + 1f) * 100f)) / 100f, (float)Game1.random.Next(-(int)(this.jitterStrength * 100f), (int)((this.jitterStrength + 1f) * 100f)) / 100f);
			}
			this.blinkTimer += time.ElapsedGameTime.Milliseconds;
			if (this.blinkTimer > 2200 && Game1.random.NextDouble() < 0.01)
			{
				this.blinkTimer = -150;
				this.currentEyes = 4;
			}
			else if (this.blinkTimer > -100)
			{
				if (this.blinkTimer < -50)
				{
					this.currentEyes = 1;
				}
				else if (this.blinkTimer < 0)
				{
					this.currentEyes = 4;
				}
				else
				{
					this.currentEyes = 0;
				}
			}
			if (this.swimming)
			{
				this.yOffset = (float)(Math.Cos(time.TotalGameTime.TotalMilliseconds / 2000.0) * (double)Game1.pixelZoom);
				int num = this.swimTimer;
				this.swimTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.timerSinceLastMovement == 0)
				{
					if (num > 400 && this.swimTimer <= 400)
					{
						this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2(this.position.X, (float)(base.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
					}
					if (this.swimTimer < 0)
					{
						this.swimTimer = 800;
						Game1.playSound("slosh");
						this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2(this.position.X, (float)(base.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
					}
				}
				else if (!Game1.eventUp && Game1.activeClickableMenu == null && !Game1.paused)
				{
					if (this.timerSinceLastMovement > 700)
					{
						this.currentEyes = 4;
					}
					if (this.timerSinceLastMovement > 800)
					{
						this.currentEyes = 1;
					}
					if (this.swimTimer < 0)
					{
						this.swimTimer = 100;
						if (this.stamina < (float)this.maxStamina)
						{
							this.stamina += 1f;
						}
						if (this.health < this.maxHealth)
						{
							this.health++;
						}
					}
				}
			}
			this.FarmerSprite.setOwner(this);
			this.FarmerSprite.checkForSingleAnimation(time);
			if (Game1.IsClient && (!Game1.eventUp || (location.currentEvent != null && location.currentEvent.playerControlSequence)))
			{
				this.lerpPosition(this.remotePosition);
				this.timeOfLastPositionPacket += (float)time.ElapsedGameTime.Milliseconds;
			}
			if (this.CanMove)
			{
				this.rotation = 0f;
				if (this.health <= 0 && !Game1.killScreen)
				{
					this.CanMove = false;
					Game1.screenGlowOnce(Color.Red, true, 0.005f, 0.3f);
					Game1.killScreen = true;
					this.FarmerSprite.setCurrentFrame(5);
					this.jitterStrength = 1f;
					Game1.pauseTime = 3000f;
					Rumble.rumbleAndFade(0.75f, 1500f);
					this.freezePause = 8000;
					if (Game1.currentSong != null && Game1.currentSong.IsPlaying)
					{
						Game1.currentSong.Stop(AudioStopOptions.Immediate);
					}
					Game1.playSound("death");
					Game1.dialogueUp = false;
					Stats expr_609 = Game1.stats;
					uint timesUnconscious = expr_609.TimesUnconscious;
					expr_609.TimesUnconscious = timesUnconscious + 1u;
				}
				switch (base.getDirection())
				{
				case 0:
					location.isCollidingWithWarp(this.nextPosition(0));
					break;
				case 1:
					location.isCollidingWithWarp(this.nextPosition(1));
					break;
				case 2:
					location.isCollidingWithWarp(this.nextPosition(2));
					break;
				case 3:
					location.isCollidingWithWarp(this.nextPosition(3));
					break;
				}
				if (this.collisionNPC != null)
				{
					this.collisionNPC.farmerPassesThrough = true;
				}
				if (this.isMoving() && !this.isRidingHorse() && location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)) != null)
				{
					this.charactercollisionTimer += time.ElapsedGameTime.Milliseconds;
					if (this.charactercollisionTimer > 400)
					{
						location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)).shake(50);
					}
					if (this.charactercollisionTimer >= 1500 && this.collisionNPC == null)
					{
						this.collisionNPC = location.isCollidingWithCharacter(this.nextPosition(this.facingDirection));
						if (this.collisionNPC.name.Equals("Bouncer") && this.currentLocation != null && this.currentLocation.name.Equals("SandyHouse"))
						{
							this.collisionNPC.showTextAboveHead(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2010", new object[0]), -1, 2, 3000, 0);
							this.collisionNPC = null;
							this.charactercollisionTimer = 0;
						}
						else if (this.collisionNPC.name.Equals("Henchman") && this.currentLocation != null && this.currentLocation.name.Equals("WitchSwamp"))
						{
							this.collisionNPC = null;
							this.charactercollisionTimer = 0;
						}
					}
				}
				else
				{
					this.charactercollisionTimer = 0;
					if (this.collisionNPC != null && location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)) == null)
					{
						this.collisionNPC.farmerPassesThrough = false;
						this.collisionNPC = null;
					}
				}
			}
			MeleeWeapon.weaponsTypeUpdate(time);
			if (!Game1.eventUp || !this.isMoving() || this.currentLocation.currentEvent == null || this.currentLocation.currentEvent.playerControlSequence)
			{
				this.lastPosition = this.position;
				if (this.controller != null)
				{
					if (this.controller.update(time))
					{
						this.controller = null;
					}
				}
				else if (this.controller == null)
				{
					this.MovePosition(time, Game1.viewport, location);
				}
			}
			if (this.lastPosition.Equals(this.position))
			{
				this.timerSinceLastMovement += time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.timerSinceLastMovement = 0;
			}
			if (Game1.IsServer && this.movementDirections.Count > 0)
			{
				MultiplayerUtility.broadcastFarmerPosition(this.uniqueMultiplayerID, this.position, this.currentLocation.name);
			}
			if (this.yJumpOffset != 0)
			{
				this.yJumpVelocity -= 0.5f;
				this.yJumpOffset -= (int)this.yJumpVelocity;
				if (this.yJumpOffset >= 0)
				{
					this.yJumpOffset = 0;
					this.yJumpVelocity = 0f;
				}
			}
			base.updateEmote(time);
			base.updateGlow();
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				if (this.items[i] != null && this.items[i] is Tool)
				{
					((Tool)this.items[i]).tickUpdate(time, this);
				}
			}
			if (this.rightRing != null)
			{
				this.rightRing.update(time, location, this);
			}
			if (this.leftRing != null)
			{
				this.leftRing.update(time, location, this);
			}
		}

		public void addQuest(int questID)
		{
			if (!this.hasQuest(questID))
			{
				this.questLog.Add(Quest.getQuestFromId(questID));
				Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2011", new object[0]), 2));
			}
		}

		public void removeQuest(int questID)
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].id == questID)
				{
					this.questLog.RemoveAt(i);
				}
			}
		}

		public void completeQuest(int questID)
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].id == questID)
				{
					this.questLog[i].questComplete();
				}
			}
		}

		public bool hasQuest(int id)
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].id == id)
				{
					return true;
				}
			}
			return false;
		}

		public bool hasNewQuestActivity()
		{
			foreach (Quest current in this.questLog)
			{
				if (current.showNew || (current.completed && !current.destroy))
				{
					return true;
				}
			}
			return false;
		}

		public float getMovementSpeed()
		{
			float num;
			if (Game1.CurrentEvent == null || Game1.CurrentEvent.playerControlSequence)
			{
				this.movementMultiplier = 0.066f;
				num = Math.Max(1f, ((float)this.speed + (Game1.eventUp ? 0f : ((float)this.addedSpeed + (this.isRidingHorse() ? 4.6f : this.temporarySpeedBuff)))) * this.movementMultiplier * (float)Game1.currentGameTime.ElapsedGameTime.Milliseconds);
				if (this.movementDirections.Count > 1)
				{
					num = 0.7f * num;
				}
			}
			else
			{
				num = Math.Max(1f, (float)this.speed + (Game1.eventUp ? ((float)Math.Max(0, Game1.CurrentEvent.farmerAddedSpeed - 2)) : ((float)this.addedSpeed + (this.isRidingHorse() ? 5f : this.temporarySpeedBuff))));
				if (this.movementDirections.Count > 1)
				{
					num = (float)Math.Max(1, (int)Math.Sqrt((double)(2f * (num * num))) / 2);
				}
			}
			return num;
		}

		public bool isWearingRing(int ringIndex)
		{
			return (this.rightRing != null && this.rightRing.indexInTileSheet == ringIndex) || (this.leftRing != null && this.leftRing.indexInTileSheet == ringIndex);
		}

		public override void Halt()
		{
			if (!this.FarmerSprite.pauseForSingleAnimation)
			{
				base.Halt();
			}
			this.movementDirections.Clear();
			this.stopJittering();
			this.armOffset = Vector2.Zero;
			if (this.isRidingHorse())
			{
				this.mount.Halt();
				this.mount.sprite.CurrentAnimation = null;
			}
		}

		public void stopJittering()
		{
			this.jitterStrength = 0f;
			this.jitter = Vector2.Zero;
		}

		public override Microsoft.Xna.Framework.Rectangle nextPosition(int direction)
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			switch (direction)
			{
			case 0:
				boundingBox.Y -= (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			case 1:
				boundingBox.X += (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			case 2:
				boundingBox.Y += (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			case 3:
				boundingBox.X -= (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			}
			return boundingBox;
		}

		public Microsoft.Xna.Framework.Rectangle nextPositionHalf(int direction)
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			switch (direction)
			{
			case 0:
				boundingBox.Y -= (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			case 1:
				boundingBox.X += (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			case 2:
				boundingBox.Y += (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			case 3:
				boundingBox.X -= (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			}
			return boundingBox;
		}

		public int getProfessionForSkill(int skillType, int skillLevel)
		{
			if (skillLevel == 5)
			{
				switch (skillType)
				{
				case 0:
					if (this.professions.Contains(0))
					{
						return 0;
					}
					if (this.professions.Contains(1))
					{
						return 1;
					}
					break;
				case 1:
					if (this.professions.Contains(6))
					{
						return 6;
					}
					if (this.professions.Contains(7))
					{
						return 7;
					}
					break;
				case 2:
					if (this.professions.Contains(12))
					{
						return 12;
					}
					if (this.professions.Contains(13))
					{
						return 13;
					}
					break;
				case 3:
					if (this.professions.Contains(18))
					{
						return 18;
					}
					if (this.professions.Contains(19))
					{
						return 19;
					}
					break;
				case 4:
					if (this.professions.Contains(24))
					{
						return 24;
					}
					if (this.professions.Contains(25))
					{
						return 25;
					}
					break;
				}
			}
			else if (skillLevel == 10)
			{
				switch (skillType)
				{
				case 0:
					if (this.professions.Contains(1))
					{
						if (this.professions.Contains(4))
						{
							return 4;
						}
						if (this.professions.Contains(5))
						{
							return 5;
						}
					}
					else
					{
						if (this.professions.Contains(2))
						{
							return 2;
						}
						if (this.professions.Contains(3))
						{
							return 3;
						}
					}
					break;
				case 1:
					if (this.professions.Contains(6))
					{
						if (this.professions.Contains(8))
						{
							return 8;
						}
						if (this.professions.Contains(9))
						{
							return 9;
						}
					}
					else
					{
						if (this.professions.Contains(10))
						{
							return 10;
						}
						if (this.professions.Contains(11))
						{
							return 11;
						}
					}
					break;
				case 2:
					if (this.professions.Contains(12))
					{
						if (this.professions.Contains(14))
						{
							return 14;
						}
						if (this.professions.Contains(15))
						{
							return 15;
						}
					}
					else
					{
						if (this.professions.Contains(16))
						{
							return 16;
						}
						if (this.professions.Contains(17))
						{
							return 17;
						}
					}
					break;
				case 3:
					if (this.professions.Contains(18))
					{
						if (this.professions.Contains(20))
						{
							return 20;
						}
						if (this.professions.Contains(21))
						{
							return 21;
						}
					}
					else
					{
						if (this.professions.Contains(23))
						{
							return 23;
						}
						if (this.professions.Contains(22))
						{
							return 22;
						}
					}
					break;
				case 4:
					if (this.professions.Contains(24))
					{
						if (this.professions.Contains(26))
						{
							return 26;
						}
						if (this.professions.Contains(27))
						{
							return 27;
						}
					}
					else
					{
						if (this.professions.Contains(28))
						{
							return 28;
						}
						if (this.professions.Contains(29))
						{
							return 29;
						}
					}
					break;
				}
			}
			return -1;
		}

		public void behaviorOnMovement(int direction)
		{
		}

		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (Game1.activeClickableMenu != null && (!Game1.eventUp || Game1.CurrentEvent.playerControlSequence))
			{
				return;
			}
			if (this.isRafting)
			{
				this.moveRaft(currentLocation, time);
				return;
			}
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
				{
					this.xVelocity = 0f;
					this.yVelocity = 0f;
				}
				Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
				boundingBox.X += (int)this.xVelocity;
				boundingBox.Y -= (int)this.yVelocity;
				if (!currentLocation.isCollidingPosition(boundingBox, viewport, true, -1, false, this))
				{
					this.position.X = this.position.X + this.xVelocity;
					this.position.Y = this.position.Y - this.yVelocity;
					this.xVelocity -= this.xVelocity / 16f;
					this.yVelocity -= this.yVelocity / 16f;
					if (Math.Abs(this.xVelocity) <= 0.05f)
					{
						this.xVelocity = 0f;
					}
					if (Math.Abs(this.yVelocity) <= 0.05f)
					{
						this.yVelocity = 0f;
					}
				}
				else
				{
					this.xVelocity -= this.xVelocity / 16f;
					this.yVelocity -= this.yVelocity / 16f;
					if (Math.Abs(this.xVelocity) <= 0.05f)
					{
						this.xVelocity = 0f;
					}
					if (Math.Abs(this.yVelocity) <= 0.05f)
					{
						this.yVelocity = 0f;
					}
				}
			}
			if (this.CanMove || Game1.eventUp || this.controller != null)
			{
				if (!this.temporaryImpassableTile.Intersects(this.GetBoundingBox()))
				{
					this.temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
				}
				float movementSpeed = this.getMovementSpeed();
				this.temporarySpeedBuff = 0f;
				if (this.movementDirections.Contains(0))
				{
					this.facingDirection = 0;
					Warp warp = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(0));
					if (warp != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp);
						return;
					}
					if (this.isRidingHorse())
					{
						currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this);
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y - movementSpeed;
						this.behaviorOnMovement(0);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(0), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y - movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle rectangle = this.nextPosition(0);
						rectangle.Width /= 4;
						bool flag = currentLocation.isCollidingPosition(rectangle, viewport, true, 0, false, this);
						rectangle.X += rectangle.Width * 3;
						bool flag2 = currentLocation.isCollidingPosition(rectangle, viewport, true, 0, false, this);
						if (flag && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (flag2 && !flag && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Count == 1)
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								((FarmerSprite)this.sprite).animate(48, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(16, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(144, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(112, time);
						}
					}
				}
				if (this.movementDirections.Contains(2))
				{
					this.facingDirection = 2;
					Warp warp2 = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(2));
					if (warp2 != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp2);
						return;
					}
					if (this.isRidingHorse())
					{
						currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this);
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y + movementSpeed;
						this.behaviorOnMovement(2);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(2), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y + movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle rectangle2 = this.nextPosition(2);
						rectangle2.Width /= 4;
						bool flag3 = currentLocation.isCollidingPosition(rectangle2, viewport, true, 0, false, this);
						rectangle2.X += rectangle2.Width * 3;
						bool flag4 = currentLocation.isCollidingPosition(rectangle2, viewport, true, 0, false, this);
						if (flag3 && !flag4 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (flag4 && !flag3 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Count == 1)
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								((FarmerSprite)this.sprite).animate(32, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(0, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(128, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(96, time);
						}
					}
				}
				if (this.movementDirections.Contains(1))
				{
					this.facingDirection = 1;
					Warp warp3 = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(1));
					if (warp3 != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp3);
						return;
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X + movementSpeed;
						this.behaviorOnMovement(1);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(1), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X + movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle rectangle3 = this.nextPosition(1);
						rectangle3.Height /= 4;
						bool flag5 = currentLocation.isCollidingPosition(rectangle3, viewport, true, 0, false, this);
						rectangle3.Y += rectangle3.Height * 3;
						bool flag6 = currentLocation.isCollidingPosition(rectangle3, viewport, true, 0, false, this);
						if (flag5 && !flag6 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (flag6 && !flag5 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Contains(1))
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								this.FarmerSprite.animate(40, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(8, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(136, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(104, time);
						}
					}
				}
				if (this.movementDirections.Contains(3))
				{
					this.facingDirection = 3;
					Warp warp4 = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(3));
					if (warp4 != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp4);
						return;
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X - movementSpeed;
						this.behaviorOnMovement(3);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(3), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X - movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle rectangle4 = this.nextPosition(3);
						rectangle4.Height /= 4;
						bool flag7 = currentLocation.isCollidingPosition(rectangle4, viewport, true, 0, false, this);
						rectangle4.Y += rectangle4.Height * 3;
						bool flag8 = currentLocation.isCollidingPosition(rectangle4, viewport, true, 0, false, this);
						if (flag7 && !flag8 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (flag8 && !flag7 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Contains(3))
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								((FarmerSprite)this.sprite).animate(56, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(24, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(152, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(120, time);
						}
					}
				}
				else if (this.moveUp && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(48, time);
				}
				else if (this.moveRight && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(40, time);
				}
				else if (this.moveDown && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(32, time);
				}
				else if (this.moveLeft && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(56, time);
				}
				else if (this.moveUp && this.running)
				{
					((FarmerSprite)this.sprite).animate(144, time);
				}
				else if (this.moveRight && this.running)
				{
					((FarmerSprite)this.sprite).animate(136, time);
				}
				else if (this.moveDown && this.running)
				{
					((FarmerSprite)this.sprite).animate(128, time);
				}
				else if (this.moveLeft && this.running)
				{
					((FarmerSprite)this.sprite).animate(152, time);
				}
				else if (this.moveUp && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(16, time);
				}
				else if (this.moveRight && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(8, time);
				}
				else if (this.moveDown && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(0, time);
				}
				else if (this.moveLeft && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(24, time);
				}
				else if (this.moveUp)
				{
					((FarmerSprite)this.sprite).animate(112, time);
				}
				else if (this.moveRight)
				{
					((FarmerSprite)this.sprite).animate(104, time);
				}
				else if (this.moveDown)
				{
					((FarmerSprite)this.sprite).animate(96, time);
				}
				else if (this.moveLeft)
				{
					((FarmerSprite)this.sprite).animate(120, time);
				}
			}
			if (this.isMoving() && !this.usingTool)
			{
				this.FarmerSprite.intervalModifier = 1f - (this.running ? 0.03f : 0.025f) * (Math.Max(1f, ((float)this.speed + (Game1.eventUp ? 0f : ((float)this.addedSpeed + (this.isRidingHorse() ? 4.6f : 0f)))) * this.movementMultiplier * (float)Game1.currentGameTime.ElapsedGameTime.Milliseconds) * 1.25f);
			}
			else
			{
				this.FarmerSprite.intervalModifier = 1f;
			}
			if (this.moveUp)
			{
				this.facingDirection = 0;
			}
			else if (this.moveRight)
			{
				this.facingDirection = 1;
			}
			else if (this.moveDown)
			{
				this.facingDirection = 2;
			}
			else if (this.moveLeft)
			{
				this.facingDirection = 3;
			}
			if (this.temporarilyInvincible)
			{
				this.temporaryInvincibilityTimer += time.ElapsedGameTime.Milliseconds;
				if (this.temporaryInvincibilityTimer > 1200)
				{
					this.temporarilyInvincible = false;
					this.temporaryInvincibilityTimer = 0;
				}
			}
			if (currentLocation != null && currentLocation.isFarmerCollidingWithAnyCharacter())
			{
				this.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle((int)base.getTileLocation().X * Game1.tileSize, (int)base.getTileLocation().Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			}
			if (this.isRidingHorse() && !this.mount.dismounting)
			{
				this.speed = 2;
				if (this.movementDirections.Count > 0 && (this.mount.facingDirection != this.movementDirections.First<int>() || (this.mount.facingDirection != 1 && this.movementDirections.Contains(1)) || (this.mount.facingDirection != 3 && this.movementDirections.Contains(3))) && (this.movementDirections.Count <= 1 || !this.movementDirections.Contains(1) || this.mount.facingDirection != 1) && (this.movementDirections.Count <= 1 || !this.movementDirections.Contains(3) || this.mount.facingDirection != 3))
				{
					this.mount.sprite.currentAnimation = null;
				}
				if (this.movementDirections.Count > 0)
				{
					if (this.movementDirections.Contains(1))
					{
						this.mount.faceDirection(1);
					}
					else if (this.movementDirections.Contains(3))
					{
						this.mount.faceDirection(3);
					}
					else
					{
						this.mount.faceDirection(this.movementDirections.First<int>());
					}
				}
				if (this.isMoving() && this.mount.sprite.currentAnimation == null)
				{
					if (this.movementDirections.Contains(1))
					{
						this.faceDirection(1);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(8, 70),
							new FarmerSprite.AnimationFrame(9, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(10, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(11, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(12, 70),
							new FarmerSprite.AnimationFrame(13, 70)
						});
					}
					else if (this.movementDirections.Contains(3))
					{
						this.faceDirection(3);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(8, 70, false, true, null, false),
							new FarmerSprite.AnimationFrame(9, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(10, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(11, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(12, 70, false, true, null, false),
							new FarmerSprite.AnimationFrame(13, 70, false, true, null, false)
						});
					}
					else if (this.movementDirections.First<int>().Equals(0))
					{
						this.faceDirection(0);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(15, 70),
							new FarmerSprite.AnimationFrame(16, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(17, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(18, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(19, 70),
							new FarmerSprite.AnimationFrame(20, 70)
						});
					}
					else if (this.movementDirections.First<int>().Equals(2))
					{
						this.faceDirection(2);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(1, 70),
							new FarmerSprite.AnimationFrame(2, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(3, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(4, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(5, 70),
							new FarmerSprite.AnimationFrame(6, 70)
						});
					}
				}
				else if (!this.isMoving())
				{
					this.mount.Halt();
					this.mount.sprite.currentAnimation = null;
				}
				this.mount.position = this.position;
			}
		}

		public bool checkForQuestComplete(NPC n, int number1, int number2, Item item, string str, int questType = -1, int questTypeToIgnore = -1)
		{
			bool result = false;
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i] != null && (questType == -1 || this.questLog[i].questType == questType) && (questTypeToIgnore == -1 || this.questLog[i].questType != questTypeToIgnore) && this.questLog[i].checkIfComplete(n, number1, number2, item, str))
				{
					result = true;
				}
			}
			return result;
		}

		public static void completelyStopAnimating(Farmer who)
		{
			who.completelyStopAnimatingOrDoingAction();
		}

		public void completelyStopAnimatingOrDoingAction()
		{
			this.CanMove = !Game1.eventUp;
			this.usingTool = false;
			this.FarmerSprite.pauseForSingleAnimation = false;
			this.usingSlingshot = false;
			this.canReleaseTool = false;
			this.Halt();
			this.sprite.StopAnimation();
			if (this.CurrentTool != null && this.CurrentTool is MeleeWeapon)
			{
				(this.CurrentTool as MeleeWeapon).isOnSpecial = false;
			}
			this.stopJittering();
		}

		public void doEmote(int whichEmote)
		{
			if (!this.isEmoting)
			{
				this.isEmoting = true;
				this.currentEmote = whichEmote;
				this.currentEmoteFrame = 0;
				this.emoteInterval = 0f;
			}
		}

		public void reloadLivestockSprites()
		{
			using (List<CoopDweller>.Enumerator enumerator = this.coopDwellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.reload();
				}
			}
			using (List<BarnDweller>.Enumerator enumerator2 = this.barnDwellers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.reload();
				}
			}
		}

		public void performTenMinuteUpdate()
		{
			if (this.addedSpeed > 0 && this.buffs.Count == 0 && Game1.buffsDisplay.otherBuffs.Count == 0 && Game1.buffsDisplay.food == null && Game1.buffsDisplay.drink == null)
			{
				this.addedSpeed = 0;
			}
		}

		public void setRunning(bool isRunning, bool force = false)
		{
			if (this.canOnlyWalk || (this.bathingClothes && !this.running) || ((Game1.CurrentEvent != null & isRunning) && !Game1.CurrentEvent.isFestival && !Game1.CurrentEvent.playerControlSequence))
			{
				return;
			}
			if (this.isRidingHorse())
			{
				this.running = true;
				return;
			}
			if (this.stamina <= 0f)
			{
				this.speed = 2;
				if (this.running)
				{
					this.Halt();
				}
				this.running = false;
				return;
			}
			if (!force && (!this.CanMove || Game1.isEating || (Game1.currentLocation.currentEvent != null && !Game1.currentLocation.currentEvent.playerControlSequence) || (!isRunning && this.usingTool) || (!isRunning && Game1.pickingTool) || (this.sprite != null && ((FarmerSprite)this.sprite).pauseForSingleAnimation)))
			{
				if (this.usingTool)
				{
					this.running = isRunning;
					if (this.running)
					{
						this.speed = 5;
						return;
					}
					this.speed = 2;
				}
				return;
			}
			this.running = isRunning;
			if (this.running)
			{
				this.speed = 5;
				return;
			}
			this.speed = 2;
		}

		public void addSeenResponse(int id)
		{
			this.dialogueQuestionsAnswered.Add(id);
		}

		public void grabObject(Object obj)
		{
			if (obj != null)
			{
				this.CanMove = false;
				this.activeObject = obj;
				switch (this.facingDirection)
				{
				case 0:
					((FarmerSprite)this.sprite).animateOnce(80, 50f, 8);
					break;
				case 1:
					((FarmerSprite)this.sprite).animateOnce(72, 50f, 8);
					break;
				case 2:
					((FarmerSprite)this.sprite).animateOnce(64, 50f, 8);
					break;
				case 3:
					((FarmerSprite)this.sprite).animateOnce(88, 50f, 8);
					break;
				}
				Game1.playSound("pickUpItem");
			}
		}

		public string getTitle()
		{
			int level = this.Level;
			if (level >= 30)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2016", new object[0]);
			}
			if (level > 28)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2017", new object[0]);
			}
			if (level > 26)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2018", new object[0]);
			}
			if (level > 24)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2019", new object[0]);
			}
			if (level > 22)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2020", new object[0]);
			}
			if (level > 20)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2021", new object[0]);
			}
			if (level > 18)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2022", new object[0]);
			}
			if (level > 16)
			{
				if (!this.isMale)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2024", new object[0]);
				}
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2023", new object[0]);
			}
			else
			{
				if (level > 14)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2025", new object[0]);
				}
				if (level > 12)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2026", new object[0]);
				}
				if (level > 10)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2027", new object[0]);
				}
				if (level > 8)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2028", new object[0]);
				}
				if (level > 6)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2029", new object[0]);
				}
				if (level > 4)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2030", new object[0]);
				}
				if (level > 2)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2031", new object[0]);
				}
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2032", new object[0]);
			}
		}
	}
}
