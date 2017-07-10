using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class MineShaft : GameLocation
	{
		public const int mineFrostLevel = 40;

		public const int mineLavaLevel = 80;

		public const int upperArea = 0;

		public const int jungleArea = 10;

		public const int frostArea = 40;

		public const int lavaArea = 80;

		public const int desertArea = 121;

		public const int bottomOfMineLevel = 120;

		public const int numberOfLevelsPerArea = 40;

		public const int mineFeature_barrels = 0;

		public const int mineFeature_chests = 1;

		public const int mineFeature_coalCart = 2;

		public const int mineFeature_elevator = 3;

		public const double chanceForColoredGemstone = 0.008;

		public const double chanceForDiamond = 0.0005;

		public const double chanceForPrismaticShard = 0.0005;

		public const int monsterLimit = 30;

		public SerializableDictionary<int, MineInfo> permanentMineChanges;

		public List<ResourceClump> resourceClumps = new List<ResourceClump>();

		private Random mineRandom;

		public int mineLevel;

		public int nextLevel;

		public int lowestLevelReached;

		private LocalizedContentManager mineLoader = Game1.content.CreateTemporary();

		private Vector2 tileBeneathLadder;

		private Vector2 tileBeneathElevator;

		private int stonesLeftOnThisLevel;

		private int timeSinceLastMusic = 200000;

		private int timeUntilElevatorLightUp;

		private int fogTime;

		private Point ElevatorLightSpot;

		private bool ladderHasSpawned;

		private bool ghostAdded;

		private bool loadedDarkArea;

		private bool isSlimeArea;

		private bool isMonsterArea;

		private bool isFallingDownShaft;

		private bool ambientFog;

		private Vector2 fogPos;

		private Color lighting = Color.White;

		private Color fogColor;

		private Point mostRecentLadder;

		private float fogAlpha;

		[XmlIgnore]
		public Cue bugLevelLoop;

		private bool rainbowLights;

		private bool isLightingDark;

		private int lastLevelsDownFallen;

		private Microsoft.Xna.Framework.Rectangle fogSource = new Microsoft.Xna.Framework.Rectangle(640, 0, 64, 64);

		public MineShaft()
		{
			this.name = "UndergroundMine";
			this.permanentMineChanges = new SerializableDictionary<int, MineInfo>();
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			foreach (ResourceClump current in this.resourceClumps)
			{
				current.tickUpdate(time, current.tile);
			}
			if (Game1.currentSong != null && (!Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("Ambient")) && Game1.random.NextDouble() < 0.00195)
			{
				Game1.playSound("cavedrip");
			}
			if (this.timeUntilElevatorLightUp > 0)
			{
				this.timeUntilElevatorLightUp -= time.ElapsedGameTime.Milliseconds;
				if (this.timeUntilElevatorLightUp <= 0)
				{
					Game1.playSound("crystal");
					base.setMapTileIndex(this.ElevatorLightSpot.X, this.ElevatorLightSpot.Y, 48, "Buildings", 0);
					Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)this.ElevatorLightSpot.X, (float)this.ElevatorLightSpot.Y) * (float)Game1.tileSize, 2f, Color.Black, this.ElevatorLightSpot.X + this.ElevatorLightSpot.Y * 1000));
				}
			}
			if (this.fogTime > 0 && Game1.shouldTimePass())
			{
				if (Game1.soundBank != null && (this.bugLevelLoop == null || this.bugLevelLoop.IsStopped))
				{
					this.bugLevelLoop = Game1.soundBank.GetCue("bugLevelLoop");
					this.bugLevelLoop.Play();
				}
				if (this.fogAlpha < 1f)
				{
					this.fogAlpha += 0.01f;
					if (this.bugLevelLoop != null && Game1.soundBank != null)
					{
						this.bugLevelLoop.SetVariable("Volume", this.fogAlpha * 100f);
						this.bugLevelLoop.SetVariable("Frequency", this.fogAlpha * 25f);
					}
				}
				else if (this.bugLevelLoop != null && Game1.soundBank != null)
				{
					float num = (float)Math.Max(0.0, Math.Min(100.0, Math.Sin((double)((float)this.fogTime / 10000f) % 628.31853071795865)));
					this.bugLevelLoop.SetVariable("Frequency", Math.Max(0f, Math.Min(100f, this.fogAlpha * 25f + num * 10f)));
				}
				int num2 = this.fogTime;
				this.fogTime -= (int)time.ElapsedGameTime.TotalMilliseconds;
				if (this.fogTime > 5000 && num2 % 4000 < this.fogTime % 4000)
				{
					this.spawnFlyingMonsterOffScreen();
				}
				Vector2 current2 = new Vector2((float)Game1.viewport.X, (float)Game1.viewport.Y);
				this.fogPos = Game1.updateFloatingObjectPositionForMovement(this.fogPos, current2, Game1.previousViewportPosition, -1f);
				this.fogPos.X = (this.fogPos.X + 0.5f) % (float)(64 * Game1.pixelZoom);
				this.fogPos.Y = (this.fogPos.Y + 0.5f) % (float)(64 * Game1.pixelZoom);
			}
			else if (this.fogAlpha > 0f)
			{
				this.fogAlpha -= 0.01f;
				if (this.bugLevelLoop != null)
				{
					this.bugLevelLoop.SetVariable("Volume", this.fogAlpha * 100f);
					this.bugLevelLoop.SetVariable("Frequency", Math.Max(0f, this.bugLevelLoop.GetVariable("Frequency") - 0.01f));
					if (this.fogAlpha <= 0f)
					{
						this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
						this.bugLevelLoop = null;
					}
				}
			}
			else if (this.ambientFog)
			{
				Vector2 current3 = new Vector2((float)Game1.viewport.X, (float)Game1.viewport.Y);
				this.fogPos = Game1.updateFloatingObjectPositionForMovement(this.fogPos, current3, Game1.previousViewportPosition, -1f);
				this.fogPos.X = (this.fogPos.X + 0.5f) % (float)(64 * Game1.pixelZoom);
				this.fogPos.Y = (this.fogPos.Y + 0.5f) % (float)(64 * Game1.pixelZoom);
			}
			base.UpdateWhenCurrentLocation(time);
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (this.bugLevelLoop != null)
			{
				this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
				this.bugLevelLoop = null;
			}
		}

		public void setNextLevel(int level)
		{
			this.nextLevel = level;
		}

		public override int getExtraMillisecondsPerInGameMinuteForThisLocation()
		{
			if (this.getMineArea(-1) != 121)
			{
				return 0;
			}
			return 2000;
		}

		public Vector2 enterMine(Farmer who, int mineLevel, bool ridingElevator)
		{
			this.mineRandom = new Random();
			this.ladderHasSpawned = false;
			this.loadLevel(this.nextLevel);
			this.chooseLevelType();
			this.findLadder();
			this.populateLevel();
			if (!who.ridingMineElevator || this.tileBeneathElevator.Equals(Vector2.Zero))
			{
				return this.tileBeneathLadder;
			}
			return this.tileBeneathElevator;
		}

		public void chooseLevelType()
		{
			this.fogTime = 0;
			if (this.bugLevelLoop != null)
			{
				this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
				this.bugLevelLoop = null;
			}
			this.ambientFog = false;
			this.rainbowLights = false;
			this.isLightingDark = false;
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)this.mineLevel + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			this.lighting = new Color(80, 80, 40);
			if (this.getMineArea(-1) == 80)
			{
				this.lighting = new Color(100, 100, 50);
			}
			if (random.NextDouble() < 0.3 && this.mineLevel > 2)
			{
				this.isLightingDark = true;
				this.lighting = new Color(120, 120, 60);
				if (random.NextDouble() < 0.3)
				{
					this.lighting = new Color(150, 150, 120);
				}
			}
			if (random.NextDouble() < 0.15 && this.mineLevel > 5 && this.mineLevel != 120)
			{
				this.isLightingDark = true;
				int mineArea = this.getMineArea(-1);
				if (mineArea <= 10)
				{
					if (mineArea == 0 || mineArea == 10)
					{
						this.lighting = new Color(110, 110, 70);
					}
				}
				else if (mineArea != 40)
				{
					if (mineArea == 80)
					{
						this.lighting = new Color(90, 130, 70);
					}
				}
				else
				{
					this.lighting = Color.Black;
				}
			}
			if (random.NextDouble() < 0.035 && this.getMineArea(-1) == 80 && this.mineLevel % 5 != 0)
			{
				this.rainbowLights = true;
			}
			if (this.isDarkArea() && this.mineLevel < 120)
			{
				this.isLightingDark = true;
				this.lighting = ((this.getMineArea(-1) == 80) ? new Color(70, 100, 100) : new Color(150, 150, 120));
				if (this.getMineArea(-1) == 0)
				{
					this.ambientFog = true;
				}
			}
			if (this.mineLevel == 100)
			{
				this.lighting = new Color(140, 140, 80);
			}
			if (this.getMineArea(-1) == 121)
			{
				this.lighting = new Color(110, 110, 40);
				if (random.NextDouble() < 0.05)
				{
					this.lighting = ((random.NextDouble() < 0.5) ? new Color(30, 30, 0) : new Color(150, 150, 50));
				}
			}
			if (this.mineLevel > 1 && (this.mineLevel == 2 || (this.mineLevel % 5 != 0 && this.timeSinceLastMusic > 150000 && Game1.random.NextDouble() < 0.5)))
			{
				this.playMineSong();
			}
		}

		private bool canAdd(int typeOfFeature, int numberSoFar)
		{
			if (this.permanentMineChanges.ContainsKey(this.mineLevel))
			{
				switch (typeOfFeature)
				{
				case 0:
					return this.permanentMineChanges[this.mineLevel].platformContainersLeft > numberSoFar;
				case 1:
					return this.permanentMineChanges[this.mineLevel].chestsLeft > numberSoFar;
				case 2:
					return this.permanentMineChanges[this.mineLevel].coalCartsLeft > numberSoFar;
				case 3:
					return this.permanentMineChanges[this.mineLevel].elevator == 0;
				}
			}
			return true;
		}

		public void updateMineLevelData(int feature, int amount = 1)
		{
			if (!this.permanentMineChanges.ContainsKey(this.mineLevel))
			{
				this.permanentMineChanges.Add(this.mineLevel, new MineInfo());
			}
			switch (feature)
			{
			case 0:
				this.permanentMineChanges[this.mineLevel].platformContainersLeft += amount;
				return;
			case 1:
				this.permanentMineChanges[this.mineLevel].chestsLeft += amount;
				return;
			case 2:
				this.permanentMineChanges[this.mineLevel].coalCartsLeft += amount;
				return;
			case 3:
				this.permanentMineChanges[this.mineLevel].elevator += amount;
				return;
			default:
				return;
			}
		}

		public bool isLevelSlimeArea()
		{
			return this.isSlimeArea;
		}

		public void checkForMapAlterations(int x, int y)
		{
			Tile tile = this.map.GetLayer("Buildings").Tiles[x, y];
			if (tile != null)
			{
				int tileIndex = tile.TileIndex;
				if (tileIndex == 194 && !this.canAdd(2, 0))
				{
					base.setMapTileIndex(x, y, 195, "Buildings", 0);
					base.setMapTileIndex(x, y - 1, 179, "Front", 0);
				}
			}
		}

		public void findLadder()
		{
			int num = 0;
			this.tileBeneathElevator = Vector2.Zero;
			bool flag = this.mineLevel % 20 == 0;
			if (flag)
			{
				this.waterTiles = new bool[this.map.Layers[0].LayerWidth, this.map.Layers[0].LayerHeight];
				this.waterColor = ((this.getMineArea(-1) == 80) ? (Color.Red * 0.8f) : (new Color(50, 100, 200) * 0.5f));
			}
			bool flag2 = false;
			this.lightGlows.Clear();
			for (int i = 0; i < this.map.GetLayer("Buildings").LayerHeight; i++)
			{
				for (int j = 0; j < this.map.GetLayer("Buildings").LayerWidth; j++)
				{
					if (this.map.GetLayer("Buildings").Tiles[j, i] != null)
					{
						int tileIndex = this.map.GetLayer("Buildings").Tiles[j, i].TileIndex;
						if (tileIndex == 115)
						{
							this.tileBeneathLadder = new Vector2((float)j, (float)(i + 1));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)j, (float)(i - 2)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 0.25f, new Color(0, 20, 50), j + i * 999));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)j, (float)(i - 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 0.5f, new Color(0, 20, 50), j + i * 998));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)j, (float)i) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 0.75f, new Color(0, 20, 50), j + i * 997));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)j, (float)(i + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 1f, new Color(0, 20, 50), j + i * 1000));
							num++;
						}
						else if (tileIndex == 112)
						{
							this.tileBeneathElevator = new Vector2((float)j, (float)(i + 1));
							num++;
						}
						if (this.lighting.Equals(Color.White) && num == 2 && !flag)
						{
							return;
						}
						if (!this.lighting.Equals(Color.White) && (tileIndex == 97 || tileIndex == 113 || tileIndex == 65 || tileIndex == 66 || tileIndex == 81 || tileIndex == 82 || tileIndex == 48))
						{
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)j, (float)i) * (float)Game1.tileSize, 2.5f, new Color(0, 50, 100), j + i * 1000));
							if (tileIndex == 66)
							{
								this.lightGlows.Add(new Vector2((float)j, (float)i) * (float)Game1.tileSize + new Vector2(0f, (float)Game1.tileSize));
							}
							else if (tileIndex == 97 || tileIndex == 113)
							{
								this.lightGlows.Add(new Vector2((float)j, (float)i) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
							}
						}
					}
					if (base.doesTileHaveProperty(j, i, "Water", "Back") != null)
					{
						flag2 = true;
						this.waterTiles[j, i] = true;
						if (this.getMineArea(-1) == 80 && Game1.random.NextDouble() < 0.1)
						{
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)j, (float)i) * (float)Game1.tileSize, 2f, new Color(0, 220, 220), j + i * 1000));
						}
					}
				}
			}
			if (!flag2)
			{
				this.waterTiles = null;
			}
			if (this.isFallingDownShaft)
			{
				Vector2 v = default(Vector2);
				while (!this.isTileClearForMineObjects(v))
				{
					v.X = (float)Game1.random.Next(1, this.map.Layers[0].LayerWidth);
					v.Y = (float)Game1.random.Next(1, this.map.Layers[0].LayerHeight);
				}
				this.tileBeneathLadder = v;
				Game1.player.showFrame(5, false);
			}
			this.isFallingDownShaft = false;
		}

		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			if (this.mustKillAllMonstersToAdvance() && this.characters.Count == 0)
			{
				Vector2 vector = new Vector2((float)((int)this.tileBeneathLadder.X), (float)((int)this.tileBeneathLadder.Y));
				if (base.getTileIndexAt(Utility.Vector2ToPoint(vector), "Buildings") == -1)
				{
					this.createLadderAt(vector, "newArtifact");
					if (this.mustKillAllMonstersToAdvance())
					{
						Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9484", new object[0]));
					}
				}
			}
			while (this.map != null && this.Equals(Game1.currentLocation) && this.mineLevel % 5 != 0 && Game1.random.NextDouble() < 0.1 && !Game1.player.hasBuff(23))
			{
				if (this.mineLevel > 10 && !this.mustKillAllMonstersToAdvance() && Game1.random.NextDouble() < 0.1)
				{
					this.fogTime = 35000 + Game1.random.Next(-5, 6) * 1000;
					Game1.changeMusicTrack("none");
					int mineArea = this.getMineArea(-1);
					if (mineArea <= 10)
					{
						if (mineArea == 0 || mineArea == 10)
						{
							this.fogColor = (this.isDarkArea() ? Color.Khaki : (Color.Green * 0.75f));
						}
					}
					else if (mineArea != 40)
					{
						if (mineArea != 80)
						{
							if (mineArea == 121)
							{
								this.fogColor = Color.BlueViolet * 1f;
							}
						}
						else
						{
							this.fogColor = Color.Red * 0.5f;
						}
					}
					else
					{
						this.fogColor = Color.Blue * 0.75f;
					}
				}
				else
				{
					this.spawnFlyingMonsterOffScreen();
				}
			}
		}

		public void spawnFlyingMonsterOffScreen()
		{
			Vector2 zero = Vector2.Zero;
			switch (Game1.random.Next(4))
			{
			case 0:
				zero.X = (float)Game1.random.Next(this.map.Layers[0].LayerWidth);
				break;
			case 1:
				zero.X = (float)(this.map.Layers[0].LayerWidth - 1);
				zero.Y = (float)Game1.random.Next(this.map.Layers[0].LayerHeight);
				break;
			case 2:
				zero.Y = (float)(this.map.Layers[0].LayerHeight - 1);
				zero.X = (float)Game1.random.Next(this.map.Layers[0].LayerWidth);
				break;
			case 3:
				zero.Y = (float)Game1.random.Next(this.map.Layers[0].LayerHeight);
				break;
			}
			if (Utility.isOnScreen(zero * (float)Game1.tileSize, Game1.tileSize))
			{
				zero.X -= (float)(Game1.viewport.Width / Game1.tileSize);
			}
			int mineArea = this.getMineArea(-1);
			if (mineArea <= 10)
			{
				if (mineArea != 0)
				{
					if (mineArea != 10)
					{
						return;
					}
					this.characters.Add(new Fly(zero * (float)Game1.tileSize, false)
					{
						focusedOnFarmers = true
					});
					return;
				}
				else if (this.mineLevel > 10 && this.isDarkArea())
				{
					this.characters.Add(new Bat(zero * (float)Game1.tileSize, this.mineLevel)
					{
						focusedOnFarmers = true
					});
					Game1.playSound("batScreech");
					return;
				}
			}
			else
			{
				if (mineArea == 40)
				{
					this.characters.Add(new Bat(zero * (float)Game1.tileSize, this.mineLevel)
					{
						focusedOnFarmers = true
					});
					Game1.playSound("batScreech");
					return;
				}
				if (mineArea == 80)
				{
					this.characters.Add(new Bat(zero * (float)Game1.tileSize, this.mineLevel)
					{
						focusedOnFarmers = true
					});
					Game1.playSound("batScreech");
					return;
				}
				if (mineArea != 121)
				{
					return;
				}
				this.characters.Add(new Serpent(zero * (float)Game1.tileSize)
				{
					focusedOnFarmers = true
				});
				Game1.playSound("serpentDie");
			}
		}

		public override void drawLightGlows(SpriteBatch b)
		{
			int mineArea = this.getMineArea(-1);
			Color color;
			if (mineArea <= 40)
			{
				if (mineArea == 0)
				{
					color = (this.isDarkArea() ? (Color.PaleGoldenrod * 0.5f) : (Color.PaleGoldenrod * 0.33f));
					goto IL_B0;
				}
				if (mineArea == 40)
				{
					color = Color.White * 0.65f;
					goto IL_B0;
				}
			}
			else
			{
				if (mineArea == 80)
				{
					color = (this.isDarkArea() ? (Color.Pink * 0.4f) : (Color.Red * 0.33f));
					goto IL_B0;
				}
				if (mineArea == 121)
				{
					color = Color.White * 0.8f;
					goto IL_B0;
				}
			}
			color = Color.PaleGoldenrod * 0.33f;
			IL_B0:
			foreach (Vector2 current in this.lightGlows)
			{
				if (this.rainbowLights)
				{
					switch ((int)(current.X / (float)Game1.tileSize + current.Y / (float)Game1.tileSize) % 4)
					{
					case 0:
						color = Color.Red * 0.5f;
						break;
					case 1:
						color = Color.Yellow * 0.5f;
						break;
					case 2:
						color = Color.Cyan * 0.33f;
						break;
					case 3:
						color = Color.Lime * 0.45f;
						break;
					}
				}
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, current), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 30, 30)), color, 0f, new Vector2(15f, 15f), (float)(Game1.pixelZoom * 2) + (float)((double)(Game1.tileSize * 3 / 2) * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(current.X * 777f) + (double)(current.Y * 9746f)) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
			}
		}

		public override StardewValley.Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			int num = -1;
			double num2 = 1.0;
			num2 += 0.4 * (double)who.FishingLevel;
			num2 += (double)waterDepth * 0.1;
			int mineArea = this.getMineArea(-1);
			if (mineArea <= 10)
			{
				if (mineArea == 0 || mineArea == 10)
				{
					num2 += (double)((bait == 689) ? 3 : 0);
					if (Game1.random.NextDouble() < 0.02 + 0.01 * num2)
					{
						num = 158;
					}
				}
			}
			else if (mineArea != 40)
			{
				if (mineArea == 80)
				{
					num2 += (double)((bait == 684) ? 3 : 0);
					if (Game1.random.NextDouble() < 0.01 + 0.008 * num2)
					{
						num = 162;
					}
				}
			}
			else
			{
				num2 += (double)((bait == 682) ? 3 : 0);
				if (Game1.random.NextDouble() < 0.015 + 0.009 * num2)
				{
					num = 161;
				}
			}
			int quality = 0;
			if (Game1.random.NextDouble() < (double)((float)who.FishingLevel / 10f))
			{
				quality = 1;
			}
			if (Game1.random.NextDouble() < (double)((float)who.FishingLevel / 50f + (float)who.LuckLevel / 100f))
			{
				quality = 2;
			}
			if (num != -1)
			{
				return new StardewValley.Object(num, 1, false, -1, quality);
			}
			if (this.getMineArea(-1) == 80)
			{
				return new StardewValley.Object(Game1.random.Next(167, 173), 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		private void adjustLevelChances(ref double stoneChance, ref double monsterChance, ref double itemChance, ref double gemStoneChance)
		{
			if (this.mineLevel == 1)
			{
				monsterChance = 0.0;
				itemChance = 0.0;
				gemStoneChance = 0.0;
			}
			else if (this.mineLevel % 5 == 0 && this.getMineArea(-1) != 121)
			{
				itemChance = 0.0;
				gemStoneChance = 0.0;
				if (this.mineLevel % 10 == 0)
				{
					monsterChance = 0.0;
				}
			}
			if (this.mustKillAllMonstersToAdvance())
			{
				monsterChance = 0.025;
				itemChance = 0.001;
				stoneChance = 0.0;
				gemStoneChance = 0.0;
			}
			if (Game1.player.hasBuff(23) && this.getMineArea(-1) != 121)
			{
				monsterChance = 0.0;
			}
			gemStoneChance /= 2.0;
		}

		private void populateLevel()
		{
			this.objects.Clear();
			this.terrainFeatures.Clear();
			this.resourceClumps.Clear();
			this.debris.Clear();
			this.characters.Clear();
			this.ghostAdded = false;
			this.stonesLeftOnThisLevel = 0;
			double num = (double)this.mineRandom.Next(10, 30) / 100.0;
			double num2 = 0.002 + (double)this.mineRandom.Next(200) / 10000.0;
			double num3 = 0.0025;
			double gemStoneChance = 0.003;
			this.adjustLevelChances(ref num, ref num2, ref num3, ref gemStoneChance);
			int num4 = 0;
			bool flag = !this.permanentMineChanges.ContainsKey(this.mineLevel);
			if (this.mineLevel > 1 && this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.5 && !this.mustKillAllMonstersToAdvance())
			{
				int num5 = this.mineRandom.Next(5) + (int)(Game1.dailyLuck * 20.0);
				for (int i = 0; i < num5; i++)
				{
					Point point;
					Point point2;
					if (this.mineRandom.NextDouble() < 0.33)
					{
						point = new Point(this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), 0);
						point2 = new Point(0, 1);
					}
					else if (this.mineRandom.NextDouble() < 0.5)
					{
						point = new Point(0, this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
						point2 = new Point(1, 0);
					}
					else
					{
						point = new Point(this.map.GetLayer("Back").LayerWidth - 1, this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
						point2 = new Point(-1, 0);
					}
					while (base.isTileOnMap(point.X, point.Y))
					{
						point.X += point2.X;
						point.Y += point2.Y;
						if (this.isTileClearForMineObjects(point.X, point.Y))
						{
							Vector2 vector = new Vector2((float)point.X, (float)point.Y);
							this.objects.Add(vector, new BreakableContainer(vector, 118));
							break;
						}
					}
				}
			}
			this.addLevelUnique(flag);
			if (this.mineLevel % 10 != 0 || this.getMineArea(-1) == 121)
			{
				for (int j = 0; j < this.map.GetLayer("Back").LayerWidth; j++)
				{
					for (int k = 0; k < this.map.GetLayer("Back").LayerHeight; k++)
					{
						this.checkForMapAlterations(j, k);
						if (this.isTileClearForMineObjects(j, k))
						{
							if (this.mineRandom.NextDouble() <= num)
							{
								Vector2 vector2 = new Vector2((float)j, (float)k);
								if (!base.Objects.ContainsKey(vector2))
								{
									if (this.getMineArea(-1) == 40 && this.mineRandom.NextDouble() < 0.15)
									{
										base.Objects.Add(vector2, new StardewValley.Object(vector2, this.mineRandom.Next(319, 322), "Weeds", true, false, false, false)
										{
											fragility = 2,
											canBeGrabbed = true
										});
									}
									else if (this.rainbowLights && this.mineRandom.NextDouble() < 0.55)
									{
										if (this.mineRandom.NextDouble() < 0.25)
										{
											int parentSheetIndex = 404;
											switch (this.mineRandom.Next(5))
											{
											case 0:
												parentSheetIndex = 422;
												break;
											case 1:
												parentSheetIndex = 420;
												break;
											case 2:
												parentSheetIndex = 420;
												break;
											case 3:
												parentSheetIndex = 420;
												break;
											case 4:
												parentSheetIndex = 420;
												break;
											}
											base.Objects.Add(vector2, new StardewValley.Object(parentSheetIndex, 1, false, -1, 0)
											{
												isSpawnedObject = true
											});
										}
									}
									else
									{
										base.Objects.Add(vector2, this.chooseStoneType(0.001, 5E-05, gemStoneChance, vector2));
										this.stonesLeftOnThisLevel++;
									}
								}
							}
							else if (this.mineRandom.NextDouble() <= num2 && Utility.distance(this.tileBeneathLadder.X, (float)j, this.tileBeneathLadder.Y, (float)k) > 5f)
							{
								Monster monsterForThisLevel = this.getMonsterForThisLevel(this.mineLevel, j, k);
								if (monsterForThisLevel is Grub)
								{
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j - 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j + 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j, k - 1);
									}
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j, k + 1);
									}
								}
								else if (monsterForThisLevel is DustSpirit)
								{
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j - 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j + 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j, k - 1);
									}
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j, k + 1);
									}
								}
								if (this.mineRandom.NextDouble() < 0.00175)
								{
									monsterForThisLevel.hasSpecialItem = true;
								}
								if (monsterForThisLevel.GetBoundingBox().Width <= Game1.tileSize || this.isTileClearForMineObjects(j + 1, k))
								{
									this.characters.Add(monsterForThisLevel);
								}
							}
							else if (this.mineRandom.NextDouble() <= num3)
							{
								Vector2 key = new Vector2((float)j, (float)k);
								base.Objects.Add(key, this.getRandomItemForThisLevel(this.mineLevel));
							}
							else if (this.mineRandom.NextDouble() <= 0.005 && !this.isDarkArea() && !this.mustKillAllMonstersToAdvance() && this.isTileClearForMineObjects(j + 1, k) && this.isTileClearForMineObjects(j, k + 1) && this.isTileClearForMineObjects(j + 1, k + 1))
							{
								Vector2 tile = new Vector2((float)j, (float)k);
								int parentSheetIndex2 = (this.mineRandom.NextDouble() < 0.5) ? 752 : 754;
								int mineArea = this.getMineArea(-1);
								if (mineArea == 40)
								{
									parentSheetIndex2 = ((this.mineRandom.NextDouble() < 0.5) ? 756 : 758);
								}
								this.resourceClumps.Add(new ResourceClump(parentSheetIndex2, 2, 2, tile));
							}
						}
						else if (this.isContainerPlatform(j, k) && base.isTileLocationTotallyClearAndPlaceable(j, k) && this.mineRandom.NextDouble() < 0.4 && (flag || this.canAdd(0, num4)))
						{
							Vector2 vector3 = new Vector2((float)j, (float)k);
							this.objects.Add(vector3, new BreakableContainer(vector3, 118));
							num4++;
							if (flag)
							{
								this.updateMineLevelData(0, 1);
							}
						}
						else if (this.mineRandom.NextDouble() <= num2 && base.isTileLocationTotallyClearAndPlaceable(j, k) && this.isTileOnClearAndSolidGround(j, k) && (!Game1.player.hasBuff(23) || this.getMineArea(-1) == 121))
						{
							Monster monsterForThisLevel2 = this.getMonsterForThisLevel(this.mineLevel, j, k);
							if (this.mineRandom.NextDouble() < 0.01)
							{
								monsterForThisLevel2.hasSpecialItem = true;
							}
							this.characters.Add(monsterForThisLevel2);
						}
					}
				}
				if (this.stonesLeftOnThisLevel > 35)
				{
					int num6 = this.stonesLeftOnThisLevel / 35;
					for (int l = 0; l < num6; l++)
					{
						Vector2 vector4 = this.objects.Keys.ElementAt(this.mineRandom.Next(this.objects.Count));
						if (this.objects[vector4].name.Equals("Stone"))
						{
							int num7 = this.mineRandom.Next(3, 8);
							bool flag2 = this.mineRandom.NextDouble() < 0.1;
							int num8 = (int)vector4.X - num7 / 2;
							while ((float)num8 < vector4.X + (float)(num7 / 2))
							{
								int num9 = (int)vector4.Y - num7 / 2;
								while ((float)num9 < vector4.Y + (float)(num7 / 2))
								{
									Vector2 key2 = new Vector2((float)num8, (float)num9);
									if (this.objects.ContainsKey(key2) && this.objects[key2].name.Equals("Stone"))
									{
										this.objects.Remove(key2);
										this.stonesLeftOnThisLevel--;
										if (flag2 && this.mineRandom.NextDouble() < 0.12)
										{
											this.characters.Add(this.getMonsterForThisLevel(this.mineLevel, num8, num9));
										}
									}
									num9++;
								}
								num8++;
							}
						}
					}
				}
				this.tryToAddAreaUniques();
				if (this.mineRandom.NextDouble() < 0.95 && !this.mustKillAllMonstersToAdvance() && this.mineLevel > 1 && this.mineLevel % 5 != 0)
				{
					Vector2 vector5 = new Vector2((float)this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), (float)this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
					if (this.isTileClearForMineObjects(vector5))
					{
						this.createLadderDown((int)vector5.X, (int)vector5.Y);
					}
				}
				if (this.mustKillAllMonstersToAdvance() && this.characters.Count <= 1)
				{
					this.characters.Add(new Bat(this.tileBeneathLadder * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 4), (float)(Game1.tileSize * 4))));
				}
			}
			if (!this.mustKillAllMonstersToAdvance() && this.mineLevel % 5 != 0 && this.mineLevel > 2)
			{
				this.tryToAddOreClumps();
				if (this.isLightingDark)
				{
					this.tryToAddOldMinerPath();
				}
			}
		}

		public void placeAppropriateOreAt(Vector2 tile)
		{
			if (this.isTileLocationTotallyClearAndPlaceable(tile))
			{
				this.objects.Add(tile, this.getAppropriateOre(tile));
			}
		}

		public StardewValley.Object getAppropriateOre(Vector2 tile)
		{
			StardewValley.Object result = new StardewValley.Object(tile, 751, "Stone", true, false, false, false)
			{
				minutesUntilReady = 3
			};
			int mineArea = this.getMineArea(-1);
			if (mineArea != 40)
			{
				if (mineArea != 80)
				{
					if (mineArea == 121)
					{
						result = new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
						{
							minutesUntilReady = 8
						};
						if (this.mineRandom.NextDouble() < 0.02)
						{
							return new StardewValley.Object(tile, 765, "Stone", true, false, false, false)
							{
								minutesUntilReady = 16
							};
						}
					}
				}
				else if (this.mineRandom.NextDouble() < 0.8)
				{
					result = new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
					{
						minutesUntilReady = 8
					};
				}
			}
			else if (this.mineRandom.NextDouble() < 0.8)
			{
				result = new StardewValley.Object(tile, 290, "Stone", true, false, false, false)
				{
					minutesUntilReady = 4
				};
			}
			if (this.mineRandom.NextDouble() < 0.25 && this.getMineArea(-1) != 40)
			{
				result = new StardewValley.Object(tile, (this.mineRandom.NextDouble() < 0.5) ? 668 : 670, "Stone", true, false, false, false)
				{
					minutesUntilReady = 2
				};
			}
			return result;
		}

		public void tryToAddOreClumps()
		{
			if (this.mineRandom.NextDouble() < 0.55 + Game1.dailyLuck)
			{
				Vector2 randomTile = base.getRandomTile();
				int num = 0;
				while (num < 1 || this.mineRandom.NextDouble() < 0.25 + Game1.dailyLuck)
				{
					if (this.isTileLocationTotallyClearAndPlaceable(randomTile) && this.isTileOnClearAndSolidGround(randomTile) && base.doesTileHaveProperty((int)randomTile.X, (int)randomTile.Y, "Diggable", "Back") == null)
					{
						StardewValley.Object appropriateOre = this.getAppropriateOre(randomTile);
						if (appropriateOre.parentSheetIndex == 670)
						{
							appropriateOre.parentSheetIndex = 668;
						}
						Utility.recursiveObjectPlacement(appropriateOre, (int)randomTile.X, (int)randomTile.Y, 0.949999988079071, 0.30000001192092896, this, "Dirt", (appropriateOre.parentSheetIndex == 668) ? 1 : 0, 0.05000000074505806, (appropriateOre.parentSheetIndex == 668) ? 2 : 1);
					}
					randomTile = base.getRandomTile();
					num++;
				}
			}
		}

		public void tryToAddOldMinerPath()
		{
			Vector2 randomTile = base.getRandomTile();
			int num = 0;
			while (!this.isTileOnClearAndSolidGround(randomTile) && num < 8)
			{
				randomTile = base.getRandomTile();
				num++;
			}
			if (this.isTileOnClearAndSolidGround(randomTile))
			{
				Stack<Point> stack = PathFindController.findPath(Utility.Vector2ToPoint(this.tileBeneathLadder), Utility.Vector2ToPoint(randomTile), new PathFindController.isAtEnd(PathFindController.isAtEndPoint), this, Game1.player, 500);
				if (stack != null)
				{
					while (stack.Count > 0)
					{
						Point point = stack.Pop();
						this.removeEverythingExceptCharactersFromThisTile(point.X, point.Y);
						if (stack.Count > 0 && this.mineRandom.NextDouble() < 0.2)
						{
							Vector2 zero = Vector2.Zero;
							if (stack.Peek().X == point.X)
							{
								zero = new Vector2((float)(point.X + ((this.mineRandom.NextDouble() < 0.5) ? -1 : 1)), (float)point.Y);
							}
							else
							{
								zero = new Vector2((float)point.X, (float)(point.Y + ((this.mineRandom.NextDouble() < 0.5) ? -1 : 1)));
							}
							if (!zero.Equals(Vector2.Zero) && this.isTileLocationTotallyClearAndPlaceable(zero) && this.isTileOnClearAndSolidGround(zero))
							{
								if (this.mineRandom.NextDouble() < 0.5)
								{
									new Torch(zero, 1).placementAction(this, (int)zero.X * Game1.tileSize, (int)zero.Y * Game1.tileSize, null);
								}
								else
								{
									this.placeAppropriateOreAt(zero);
								}
							}
						}
					}
				}
			}
		}

		public void tryToAddAreaUniques()
		{
			if ((this.getMineArea(-1) == 10 || this.getMineArea(-1) == 80 || (this.getMineArea(-1) == 40 && this.mineRandom.NextDouble() < 0.1)) && !this.isDarkArea() && !this.mustKillAllMonstersToAdvance())
			{
				int num = this.mineRandom.Next(7, 24);
				int parentSheetIndex = (this.getMineArea(-1) == 80) ? 316 : ((this.getMineArea(-1) == 40) ? 319 : 313);
				for (int i = 0; i < num; i++)
				{
					Vector2 vector = new Vector2((float)this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), (float)this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
					Utility.recursiveObjectPlacement(new StardewValley.Object(vector, parentSheetIndex, "Weeds", true, false, false, false)
					{
						fragility = 2,
						canBeGrabbed = true
					}, (int)vector.X, (int)vector.Y, 1.0, (double)((float)this.mineRandom.Next(10, 40) / 100f), this, "Dirt", 2, 0.29, 1);
				}
			}
		}

		public void tryToAddMonster(Monster m, int tileX, int tileY)
		{
			if (this.isTileClearForMineObjects(tileX, tileY) && !this.isTileOccupied(new Vector2((float)tileX, (float)tileY), ""))
			{
				m.setTilePosition(tileX, tileY);
				this.characters.Add(m);
			}
		}

		public bool isContainerPlatform(int x, int y)
		{
			return this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Back").Tiles[x, y].TileIndex == 257;
		}

		public bool mustKillAllMonstersToAdvance()
		{
			return this.isSlimeArea || this.isMonsterArea;
		}

		public void createLadderAt(Vector2 p, string sound = "hoeHit")
		{
			base.setMapTileIndex((int)p.X, (int)p.Y, 173, "Buildings", 0);
			Game1.playSound(sound);
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize, Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				interval = 80f
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize - new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 150,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 300,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize - new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 450,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize - new Vector2((float)(-(float)Game1.tileSize / 4), (float)(Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 600,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle((int)p.X * Game1.tileSize, (int)p.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public bool recursiveTryToCreateLadderDown(Vector2 centerTile, string sound = "hoeHit", int maxIterations = 16)
		{
			int num = 0;
			Queue<Vector2> queue = new Queue<Vector2>();
			queue.Enqueue(centerTile);
			List<Vector2> list = new List<Vector2>();
			while (num < maxIterations && queue.Count > 0)
			{
				Vector2 vector = queue.Dequeue();
				list.Add(vector);
				if (!this.isTileOccupied(vector, "ignoreMe") && this.isTileOnClearAndSolidGround(vector) && base.isTileOccupiedByFarmer(vector) == null && base.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Type", "Back") != null && base.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Type", "Back").Equals("Stone"))
				{
					this.createLadderAt(vector, "hoeHit");
					return true;
				}
				Vector2[] directionsTileVectors = Utility.DirectionsTileVectors;
				for (int i = 0; i < directionsTileVectors.Length; i++)
				{
					Vector2 value = directionsTileVectors[i];
					if (!list.Contains(vector + value))
					{
						queue.Enqueue(vector + value);
					}
				}
				num++;
			}
			return false;
		}

		public override void monsterDrop(Monster monster, int x, int y)
		{
			if (monster.hasSpecialItem)
			{
				Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(this.mineLevel, x / Game1.tileSize, y / Game1.tileSize), monster.position, Game1.random.Next(4), null);
			}
			else
			{
				base.monsterDrop(monster, x, y);
			}
			if ((!this.mustKillAllMonstersToAdvance() && Game1.random.NextDouble() < 0.15) || (this.mustKillAllMonstersToAdvance() && this.characters.Count <= 1))
			{
				Vector2 vector = new Vector2((float)x, (float)y) / (float)Game1.tileSize;
				vector.X = (float)((int)vector.X);
				vector.Y = (float)((int)vector.Y);
				monster.name = "ignoreMe";
				Microsoft.Xna.Framework.Rectangle value = new Microsoft.Xna.Framework.Rectangle((int)vector.X * Game1.tileSize, (int)vector.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
				if (!this.isTileOccupied(vector, "ignoreMe") && this.isTileOnClearAndSolidGround(vector) && !Game1.player.GetBoundingBox().Intersects(value) && base.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Type", "Back") != null && base.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Type", "Back").Equals("Stone"))
				{
					this.createLadderAt(vector, "hoeHit");
					return;
				}
				if (this.mustKillAllMonstersToAdvance() && this.characters.Count <= 1)
				{
					vector = new Vector2((float)((int)this.tileBeneathLadder.X), (float)((int)this.tileBeneathLadder.Y));
					this.createLadderAt(vector, "newArtifact");
					if (this.mustKillAllMonstersToAdvance())
					{
						Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9484", new object[0]));
					}
				}
			}
		}

		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			for (int i = this.resourceClumps.Count - 1; i >= 0; i--)
			{
				if (this.resourceClumps[i] != null && this.resourceClumps[i].getBoundingBox(this.resourceClumps[i].tile).Contains(tileX * Game1.tileSize, tileY * Game1.tileSize))
				{
					if (this.resourceClumps[i].performToolAction(t, 1, this.resourceClumps[i].tile, null))
					{
						this.resourceClumps.RemoveAt(i);
					}
					return true;
				}
			}
			return base.performToolAction(t, tileX, tileY);
		}

		private void addLevelUnique(bool firstTime)
		{
			List<Item> list = new List<Item>();
			Vector2 vector = new Vector2(9f, 9f);
			Color tint = Color.White;
			if (this.mineLevel % 20 == 0 && this.mineLevel % 40 != 0)
			{
				vector.Y += 4f;
			}
			int num = this.mineLevel;
			if (num <= 60)
			{
				if (num <= 20)
				{
					if (num != 5)
					{
						if (num != 10)
						{
							if (num == 20)
							{
								list.Add(new MeleeWeapon(11));
							}
						}
						else
						{
							list.Add(new Boots(506));
						}
					}
					else
					{
						Game1.player.completeQuest(14);
						if (!Game1.player.hasOrWillReceiveMail("guildQuest"))
						{
							Game1.addMailForTomorrow("guildQuest", false, false);
						}
					}
				}
				else if (num != 40)
				{
					if (num != 50)
					{
						if (num == 60)
						{
							list.Add(new MeleeWeapon(21));
						}
					}
					else
					{
						list.Add(new Boots(509));
					}
				}
				else
				{
					Game1.player.completeQuest(17);
					list.Add(new Slingshot());
				}
			}
			else if (num <= 90)
			{
				if (num != 70)
				{
					if (num != 80)
					{
						if (num == 90)
						{
							list.Add(new MeleeWeapon(8));
						}
					}
					else
					{
						list.Add(new Boots(512));
					}
				}
				else
				{
					list.Add(new Slingshot(33));
				}
			}
			else if (num != 100)
			{
				if (num != 110)
				{
					if (num == 120)
					{
						Game1.player.completeQuest(18);
						Game1.getSteamAchievement("Achievement_TheBottom");
						if (!Game1.player.hasSkullKey)
						{
							list.Add(new SpecialItem(1, 4, ""));
						}
						tint = Color.Pink;
					}
				}
				else
				{
					list.Add(new Boots(514));
				}
			}
			else
			{
				list.Add(new StardewValley.Object(434, 1, false, -1, 0));
			}
			if (list.Count > 0 && this.canAdd(1, 0))
			{
				this.objects.Add(vector, new Chest(0, list, vector, false)
				{
					tint = tint
				});
				if (firstTime)
				{
					this.updateMineLevelData(1, 1);
				}
			}
		}

		public static Item getSpecialItemForThisMineLevel(int level, int x, int y)
		{
			Random random = new Random(level + (int)Game1.stats.DaysPlayed + x + y * 10000);
			if (level < 20)
			{
				switch (random.Next(6))
				{
				case 0:
					return new MeleeWeapon(16);
				case 1:
					return new MeleeWeapon(24);
				case 2:
					return new Boots(504);
				case 3:
					return new Boots(505);
				case 4:
					return new Ring(516);
				case 5:
					return new Ring(518);
				}
			}
			else if (level < 40)
			{
				switch (random.Next(7))
				{
				case 0:
					return new MeleeWeapon(22);
				case 1:
					return new MeleeWeapon(24);
				case 2:
					return new Boots(504);
				case 3:
					return new Boots(505);
				case 4:
					return new Ring(516);
				case 5:
					return new Ring(518);
				case 6:
					return new MeleeWeapon(15);
				}
			}
			else if (level < 60)
			{
				switch (random.Next(7))
				{
				case 0:
					return new MeleeWeapon(6);
				case 1:
					return new MeleeWeapon(26);
				case 2:
					return new MeleeWeapon(15);
				case 3:
					return new Boots(510);
				case 4:
					return new Ring(517);
				case 5:
					return new Ring(519);
				case 6:
					return new MeleeWeapon(27);
				}
			}
			else if (level < 160)
			{
				switch (random.Next(7))
				{
				case 0:
					return new MeleeWeapon(26);
				case 1:
					return new MeleeWeapon(26);
				case 2:
					return new Boots(508);
				case 3:
					return new Boots(510);
				case 4:
					return new Ring(517);
				case 5:
					return new Ring(519);
				case 6:
					return new MeleeWeapon(26);
				}
			}
			else if (level < 100)
			{
				switch (random.Next(7))
				{
				case 0:
					return new MeleeWeapon(48);
				case 1:
					return new MeleeWeapon(48);
				case 2:
					return new Boots(511);
				case 3:
					return new Boots(513);
				case 4:
					return new MeleeWeapon(18);
				case 5:
					return new MeleeWeapon(28);
				case 6:
					return new MeleeWeapon(52);
				}
			}
			else if (level < 120)
			{
				switch (random.Next(6))
				{
				case 0:
					return new MeleeWeapon(19);
				case 1:
					return new MeleeWeapon(50);
				case 2:
					return new Boots(511);
				case 3:
					return new Boots(513);
				case 4:
					return new MeleeWeapon(18);
				case 5:
					return new MeleeWeapon(46);
				}
			}
			else
			{
				switch (random.Next(8))
				{
				case 0:
					return new MeleeWeapon(45);
				case 1:
					return new MeleeWeapon(50);
				case 2:
					return new Boots(511);
				case 3:
					return new Boots(513);
				case 4:
					return new MeleeWeapon(18);
				case 5:
					return new MeleeWeapon(28);
				case 6:
					return new MeleeWeapon(52);
				case 7:
					return new StardewValley.Object(787, 1, false, -1, 0);
				}
			}
			return new StardewValley.Object(78, 1, false, -1, 0);
		}

		public override bool isTileOccupied(Vector2 tileLocation, string characterToIgnore = "")
		{
			using (List<ResourceClump>.Enumerator enumerator = this.resourceClumps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.occupiesTile((int)tileLocation.X, (int)tileLocation.Y))
					{
						return true;
					}
				}
			}
			return this.tileBeneathLadder.Equals(tileLocation) || base.isTileOccupied(tileLocation, characterToIgnore);
		}

		public bool isDarkArea()
		{
			return (this.loadedDarkArea || this.mineLevel % 40 > 30) && this.getMineArea(-1) != 40;
		}

		public bool isTileClearForMineObjects(Vector2 v)
		{
			if (this.tileBeneathLadder.Equals(v) || this.tileBeneathElevator.Equals(v))
			{
				return false;
			}
			if (!this.isTileLocationTotallyClearAndPlaceable(v))
			{
				return false;
			}
			string text = base.doesTileHaveProperty((int)v.X, (int)v.Y, "Type", "Back");
			return text != null && text.Equals("Stone") && this.isTileOnClearAndSolidGround(v) && !this.objects.ContainsKey(v);
		}

		public bool isTileOnClearAndSolidGround(Vector2 v)
		{
			return this.map.GetLayer("Back").Tiles[(int)v.X, (int)v.Y] != null && this.map.GetLayer("Front").Tiles[(int)v.X, (int)v.Y] == null && this.map.GetLayer("Buildings").Tiles[(int)v.X, (int)v.Y] == null && base.getTileIndexAt((int)v.X, (int)v.Y, "Back") != 77;
		}

		public bool isTileOnClearAndSolidGround(int x, int y)
		{
			return this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Front").Tiles[x, y] == null && base.getTileIndexAt(x, y, "Back") != 77;
		}

		public bool isTileClearForMineObjects(int x, int y)
		{
			return this.isTileClearForMineObjects(new Vector2((float)x, (float)y));
		}

		public void loadLevel(int level)
		{
			this.isMonsterArea = false;
			this.isSlimeArea = false;
			this.loadedDarkArea = false;
			this.mineLoader.Unload();
			this.mineLoader.Dispose();
			this.mineLoader = Game1.content.CreateTemporary();
			int num = (level % 40 % 20 == 0 && level % 40 != 0) ? 20 : ((level % 10 == 0) ? 10 : level);
			num %= 40;
			if (level == 120)
			{
				num = 120;
			}
			if (this.getMineArea(level) == 121)
			{
				num = this.mineRandom.Next(40);
				while (num % 5 == 0)
				{
					num = this.mineRandom.Next(40);
				}
			}
			this.map = this.mineLoader.Load<Map>("Maps\\Mines\\" + num);
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)level + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			if ((!Game1.player.hasBuff(23) || this.getMineArea(-1) == 121) && random.NextDouble() < 0.05 && num % 5 != 0 && num % 40 > 5 && num % 40 < 30 && num % 40 != 19)
			{
				if (random.NextDouble() < 0.5)
				{
					this.isMonsterArea = true;
				}
				else
				{
					this.isSlimeArea = true;
				}
				Game1.showGlobalMessage(Game1.content.LoadString((random.NextDouble() < 0.5) ? "Strings\\Locations:Mines_Infested" : "Strings\\Locations:Mines_Overrun", new object[0]));
			}
			if (this.getMineArea(this.nextLevel) != this.getMineArea(this.mineLevel) || this.mineLevel == 120)
			{
				Game1.changeMusicTrack("none");
			}
			if (this.isSlimeArea)
			{
				this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_slime";
				this.map.LoadTileSheets(Game1.mapDisplayDevice);
			}
			else if (this.getMineArea(-1) == 0 || this.getMineArea(-1) == 10 || (this.getMineArea(this.nextLevel) != 0 && this.getMineArea(this.nextLevel) != 10))
			{
				if (this.getMineArea(this.nextLevel) == 40)
				{
					this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_frost";
					if (this.nextLevel >= 70)
					{
						TileSheet expr_251 = this.map.TileSheets[0];
						expr_251.ImageSource += "_dark";
						this.loadedDarkArea = true;
					}
					this.map.LoadTileSheets(Game1.mapDisplayDevice);
				}
				else if (this.getMineArea(this.nextLevel) == 80)
				{
					this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_lava";
					if (this.nextLevel >= 110 && this.nextLevel != 120)
					{
						TileSheet expr_2D2 = this.map.TileSheets[0];
						expr_2D2.ImageSource += "_dark";
						this.loadedDarkArea = true;
					}
					this.map.LoadTileSheets(Game1.mapDisplayDevice);
				}
				else if (this.getMineArea(this.nextLevel) == 121)
				{
					this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_desert";
					if (num % 40 >= 30)
					{
						TileSheet expr_34A = this.map.TileSheets[0];
						expr_34A.ImageSource += "_dark";
						this.loadedDarkArea = true;
					}
					this.map.LoadTileSheets(Game1.mapDisplayDevice);
					if (this.nextLevel >= 145 && Game1.player.hasQuest(20) && !Game1.player.hasOrWillReceiveMail("QiChallengeComplete"))
					{
						Game1.player.completeQuest(20);
						Game1.addMailForTomorrow("QiChallengeComplete", false, false);
					}
				}
			}
			if (!this.map.TileSheets[0].TileIndexProperties[165].ContainsKey("Diggable"))
			{
				this.map.TileSheets[0].TileIndexProperties[165].Add("Diggable", new PropertyValue("true"));
			}
			if (!this.map.TileSheets[0].TileIndexProperties[181].ContainsKey("Diggable"))
			{
				this.map.TileSheets[0].TileIndexProperties[181].Add("Diggable", new PropertyValue("true"));
			}
			if (!this.map.TileSheets[0].TileIndexProperties[183].ContainsKey("Diggable"))
			{
				this.map.TileSheets[0].TileIndexProperties[183].Add("Diggable", new PropertyValue("true"));
			}
			this.mineLevel = this.nextLevel;
			if (this.nextLevel > this.lowestLevelReached)
			{
				this.lowestLevelReached = this.nextLevel;
				Game1.player.deepestMineLevel = this.nextLevel;
			}
			if (this.mineLevel % 5 == 0 && this.getMineArea(-1) != 121)
			{
				this.prepareElevator();
			}
			Utility.CollectGarbage("", 0);
		}

		private void prepareElevator()
		{
			Point point = Utility.findTile(this, 80, "Buildings");
			this.ElevatorLightSpot = point;
			if (point.X >= 0)
			{
				if (this.canAdd(3, 0))
				{
					this.timeUntilElevatorLightUp = 1500;
					this.updateMineLevelData(3, 1);
					return;
				}
				base.setMapTileIndex(point.X, point.Y, 48, "Buildings", 0);
			}
		}

		public void enterMineShaft()
		{
			DelayedAction.playSoundAfterDelay("fallDown", 1200);
			DelayedAction.playSoundAfterDelay("clubSmash", 2200);
			int num = this.mineRandom.Next(3, 9);
			if (this.mineRandom.NextDouble() < 0.1)
			{
				num = num * 2 - 1;
			}
			this.lastLevelsDownFallen = num;
			Game1.player.health = Math.Max(1, Game1.player.health - num * 3);
			this.isFallingDownShaft = true;
			Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFall), 0.025f);
			Game1.player.CanMove = false;
			Game1.player.jump();
		}

		private void afterFall()
		{
			Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9578", new object[]
			{
				this.lastLevelsDownFallen
			}) + ((this.lastLevelsDownFallen > 7) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9580", new object[0]) : ""));
			Game1.drawObjectDialogue(Game1.content.LoadString((this.lastLevelsDownFallen > 7) ? "Strings\\Locations:Mines_FallenFar" : "Strings\\Locations:Mines_Fallen", new object[]
			{
				this.lastLevelsDownFallen
			}));
			this.setNextLevel(this.mineLevel + this.lastLevelsDownFallen);
			Game1.messagePause = true;
			Game1.warpFarmer("UndergroundMine", 0, 0, 2);
			Game1.player.faceDirection(2);
			Game1.player.showFrame(5, false);
			Game1.globalFadeToClear(null, 0.01f);
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tile != null && who.IsMainPlayer)
			{
				int tileIndex = tile.TileIndex;
				if (tileIndex <= 115)
				{
					if (tileIndex != 112)
					{
						if (tileIndex == 115)
						{
							Response[] answerChoices = new Response[]
							{
								new Response("Leave", Game1.content.LoadString("Strings\\Locations:Mines_LeaveMine", new object[0])),
								new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing", new object[0]))
							};
							base.createQuestionDialogue(" ", answerChoices, "ExitMine");
						}
					}
					else
					{
						Game1.activeClickableMenu = new MineElevatorMenu();
					}
				}
				else if (tileIndex != 173)
				{
					if (tileIndex != 174)
					{
						if (tileIndex == 194)
						{
							Game1.playSound("openBox");
							Game1.playSound("Ship");
							Tile expr_1B9 = this.map.GetLayer("Buildings").Tiles[tileLocation];
							tileIndex = expr_1B9.TileIndex;
							expr_1B9.TileIndex = tileIndex + 1;
							Tile expr_1F0 = this.map.GetLayer("Front").Tiles[tileLocation.X, tileLocation.Y - 1];
							tileIndex = expr_1F0.TileIndex;
							expr_1F0.TileIndex = tileIndex + 1;
							Game1.createRadialDebris(this, 382, tileLocation.X, tileLocation.Y, 6, false, -1, true, -1);
							this.updateMineLevelData(2, -1);
						}
					}
					else
					{
						Response[] answerChoices2 = new Response[]
						{
							new Response("Jump", Game1.content.LoadString("Strings\\Locations:Mines_ShaftJumpIn", new object[0])),
							new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing", new object[0]))
						};
						base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Mines_Shaft", new object[0]), answerChoices2, "Shaft");
					}
				}
				else
				{
					Game1.enterMine(false, this.mineLevel + 1, null);
					Game1.playSound("stairsdown");
				}
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public override string checkForBuriedItem(int xLocation, int yLocation, bool explosion, bool detectOnly)
		{
			if (Game1.random.NextDouble() < 0.15)
			{
				int objectIndex = 330;
				if (Game1.random.NextDouble() < 0.07)
				{
					if (Game1.random.NextDouble() < 0.75)
					{
						switch (Game1.random.Next(5))
						{
						case 0:
							objectIndex = 96;
							break;
						case 1:
							objectIndex = (Game1.player.archaeologyFound.ContainsKey(102) ? ((Game1.player.archaeologyFound[102][0] < 21) ? 102 : 770) : 770);
							break;
						case 2:
							objectIndex = 110;
							break;
						case 3:
							objectIndex = 112;
							break;
						case 4:
							objectIndex = 585;
							break;
						}
					}
					else if (Game1.random.NextDouble() < 0.75)
					{
						int mineArea = this.getMineArea(-1);
						if (mineArea != 0)
						{
							if (mineArea != 40)
							{
								if (mineArea == 80)
								{
									objectIndex = 99;
								}
							}
							else
							{
								objectIndex = ((Game1.random.NextDouble() < 0.5) ? 122 : 336);
							}
						}
						else
						{
							objectIndex = ((Game1.random.NextDouble() < 0.5) ? 121 : 97);
						}
					}
					else
					{
						objectIndex = ((Game1.random.NextDouble() < 0.5) ? 126 : 127);
					}
				}
				else if (Game1.random.NextDouble() < 0.19)
				{
					objectIndex = ((Game1.random.NextDouble() < 0.5) ? 390 : this.getOreIndexForLevel(this.mineLevel, Game1.random));
				}
				else
				{
					if (Game1.random.NextDouble() < 0.08)
					{
						Game1.createRadialDebris(this, 8, xLocation, yLocation, Game1.random.Next(1, 5), true, -1, false, -1);
						return "";
					}
					if (Game1.random.NextDouble() < 0.45)
					{
						objectIndex = 330;
					}
					else if (Game1.random.NextDouble() < 0.12)
					{
						if (Game1.random.NextDouble() < 0.25)
						{
							objectIndex = 749;
						}
						else
						{
							int mineArea = this.getMineArea(-1);
							if (mineArea != 0)
							{
								if (mineArea != 40)
								{
									if (mineArea == 80)
									{
										objectIndex = 537;
									}
								}
								else
								{
									objectIndex = 536;
								}
							}
							else
							{
								objectIndex = 535;
							}
						}
					}
					else
					{
						objectIndex = 78;
					}
				}
				Game1.createObjectDebris(objectIndex, xLocation, yLocation, Game1.player.uniqueMultiplayerID, this);
				return "";
			}
			return "";
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			foreach (ResourceClump current in this.resourceClumps)
			{
				if (!glider && current.getBoundingBox(current.tile).Intersects(position))
				{
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			foreach (NPC current in this.characters)
			{
				if (current is Monster)
				{
					(current as Monster).drawAboveAllLayers(b);
				}
			}
			if (this.fogAlpha > 0f || this.ambientFog)
			{
				Vector2 position = default(Vector2);
				for (float num = (float)(-64 * Game1.pixelZoom + (int)(this.fogPos.X % (float)(64 * Game1.pixelZoom))); num < (float)Game1.graphics.GraphicsDevice.Viewport.Width; num += (float)(64 * Game1.pixelZoom))
				{
					for (float num2 = (float)(-64 * Game1.pixelZoom + (int)(this.fogPos.Y % (float)(64 * Game1.pixelZoom))); num2 < (float)Game1.graphics.GraphicsDevice.Viewport.Height; num2 += (float)(64 * Game1.pixelZoom))
					{
						position.X = (float)((int)num);
						position.Y = (float)((int)num2);
						b.Draw(Game1.mouseCursors, position, new Microsoft.Xna.Framework.Rectangle?(this.fogSource), (this.fogAlpha > 0f) ? (this.fogColor * this.fogAlpha) : (Color.Black * 0.95f), 0f, Vector2.Zero, (float)Game1.pixelZoom + 0.001f, SpriteEffects.None, 1f);
					}
				}
			}
			if (this.isMonsterArea)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(193, 324, 7, 10)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 25f, SpriteEffects.None, 1f);
				return;
			}
			SpriteText.drawString(b, string.Concat(this.mineLevel + ((this.getMineArea(-1) == 121) ? -120 : 0)), Game1.tileSize / 4, Game1.tileSize / 4, 999999, -1, 999999, 1f, 1f, false, 2, "", (this.getMineArea(-1) == 0 || this.isDarkArea()) ? 4 : ((this.getMineArea(-1) == 10) ? 6 : ((this.getMineArea(-1) == 40) ? 7 : ((this.getMineArea(-1) == 80) ? 2 : 3))));
		}

		public override void checkForMusic(GameTime time)
		{
			if (Game1.player.freezePause > 0 || this.fogTime > 0)
			{
				return;
			}
			if (this.mineLevel == 120)
			{
				return;
			}
			if (Game1.currentSong == null || !Game1.currentSong.IsPlaying)
			{
				string text = "";
				int mineArea = this.getMineArea(-1);
				if (mineArea <= 10)
				{
					if (mineArea != 0 && mineArea != 10)
					{
						goto IL_77;
					}
				}
				else
				{
					if (mineArea == 40)
					{
						text = "Frost";
						goto IL_77;
					}
					if (mineArea == 80)
					{
						text = "Lava";
						goto IL_77;
					}
					if (mineArea != 121)
					{
						goto IL_77;
					}
				}
				text = "Upper";
				IL_77:
				text += "_Ambient";
				Game1.changeMusicTrack(text);
			}
			this.timeSinceLastMusic = Math.Min(335000, this.timeSinceLastMusic + time.ElapsedGameTime.Milliseconds);
		}

		public void playMineSong()
		{
			if ((Game1.currentSong == null || !Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("Ambient")) && !this.isDarkArea())
			{
				this.timeSinceLastMusic = 0;
				if (Game1.player.isWearingRing(528))
				{
					Game1.changeMusicTrack(Utility.getRandomNonLoopingSong());
					return;
				}
				if (this.mineLevel < 40)
				{
					Game1.changeMusicTrack("EarthMine");
					return;
				}
				if (this.mineLevel < 80)
				{
					Game1.changeMusicTrack("FrostMine");
					return;
				}
				Game1.changeMusicTrack("LavaMine");
			}
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.forceViewportPlayerFollow = true;
		}

		public void createLadderDown(int x, int y)
		{
			if (this.getMineArea(-1) == 121 && !this.mustKillAllMonstersToAdvance() && this.mineRandom.NextDouble() < 0.2)
			{
				this.map.GetLayer("Buildings").Tiles[x, y] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 174);
			}
			else
			{
				this.ladderHasSpawned = true;
				this.map.GetLayer("Buildings").Tiles[x, y] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 173);
			}
			Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public void checkStoneForItems(int tileIndexOfStone, int x, int y, Farmer who)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			double num = Game1.dailyLuck / 2.0 + (double)who.MiningLevel * 0.005 + (double)who.LuckLevel * 0.001;
			Random random = new Random(x * 1000 + y + this.mineLevel + (int)Game1.uniqueIDForThisGame / 2);
			random.NextDouble();
			double num2 = (tileIndexOfStone == 40 || tileIndexOfStone == 42) ? 1.2 : 0.8;
			if (tileIndexOfStone == 34 || tileIndexOfStone == 36 || tileIndexOfStone != 50)
			{
			}
			this.stonesLeftOnThisLevel--;
			double num3 = 0.02 + 1.0 / (double)Math.Max(1, this.stonesLeftOnThisLevel) + (double)who.LuckLevel / 100.0 + Game1.dailyLuck / 5.0;
			if (this.characters.Count == 0)
			{
				num3 += 0.04;
			}
			if (!this.ladderHasSpawned && (this.stonesLeftOnThisLevel == 0 || random.NextDouble() < num3))
			{
				this.createLadderDown(x, y);
			}
			if (base.breakStone(tileIndexOfStone, x, y, who, random))
			{
				return;
			}
			if (tileIndexOfStone == 44)
			{
				int num4 = random.Next(59, 70);
				num4 += num4 % 2;
				if (who.timesReachedMineBottom == 0)
				{
					if (this.mineLevel < 40 && num4 != 66 && num4 != 68)
					{
						num4 = ((random.NextDouble() < 0.5) ? 66 : 68);
					}
					else if (this.mineLevel < 80 && (num4 == 64 || num4 == 60))
					{
						num4 = ((random.NextDouble() < 0.5) ? ((random.NextDouble() < 0.5) ? 66 : 70) : ((random.NextDouble() < 0.5) ? 68 : 62));
					}
				}
				Game1.createObjectDebris(num4, x, y, who.uniqueMultiplayerID, this);
				Stats expr_1FF = Game1.stats;
				uint otherPreciousGemsFound = expr_1FF.OtherPreciousGemsFound;
				expr_1FF.OtherPreciousGemsFound = otherPreciousGemsFound + 1u;
				return;
			}
			if (random.NextDouble() < 0.022 * (1.0 + num) * (double)(who.professions.Contains(22) ? 2 : 1))
			{
				int objectIndex = 535 + ((this.getMineArea(-1) == 40) ? 1 : ((this.getMineArea(-1) == 80) ? 2 : 0));
				if (this.getMineArea(-1) == 121)
				{
					objectIndex = 749;
				}
				if (who.professions.Contains(19) && random.NextDouble() < 0.5)
				{
					Game1.createObjectDebris(objectIndex, x, y, who.uniqueMultiplayerID, this);
				}
				Game1.createObjectDebris(objectIndex, x, y, who.uniqueMultiplayerID, this);
				who.gainExperience(5, 20 * this.getMineArea(-1));
			}
			if (this.mineLevel > 20 && random.NextDouble() < 0.005 * (1.0 + num) * (double)(who.professions.Contains(22) ? 2 : 1))
			{
				if (who.professions.Contains(19) && random.NextDouble() < 0.5)
				{
					Game1.createObjectDebris(749, x, y, who.uniqueMultiplayerID, this);
				}
				Game1.createObjectDebris(749, x, y, who.uniqueMultiplayerID, this);
				who.gainExperience(5, 40 * this.getMineArea(-1));
			}
			if (random.NextDouble() < 0.05 * (1.0 + num) * num2)
			{
				random.Next(1, 3);
				random.NextDouble();
				double arg_3B4_0 = 0.1 * (1.0 + num);
				if (random.NextDouble() < 0.25)
				{
					Game1.createObjectDebris(382, x, y, who.uniqueMultiplayerID, this);
					this.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)(Game1.tileSize * x), (float)(Game1.tileSize * y)), Color.White, 8, Game1.random.NextDouble() < 0.5, 80f, 0, -1, -1f, Game1.tileSize * 2, 0));
				}
				else
				{
					Game1.createObjectDebris(this.getOreIndexForLevel(this.mineLevel, random), x, y, who.uniqueMultiplayerID, this);
				}
				who.gainExperience(3, 5);
				return;
			}
			if (random.NextDouble() < 0.5)
			{
				Game1.createDebris(14, x, y, 1, this);
			}
		}

		public int getOreIndexForLevel(int mineLevel, Random r)
		{
			if (mineLevel < 40)
			{
				if (mineLevel >= 20 && r.NextDouble() < 0.1)
				{
					return 380;
				}
				return 378;
			}
			else if (mineLevel < 80)
			{
				if (mineLevel >= 60 && r.NextDouble() < 0.1)
				{
					return 384;
				}
				if (r.NextDouble() >= 0.75)
				{
					return 378;
				}
				return 380;
			}
			else if (mineLevel < 120)
			{
				if (r.NextDouble() < 0.75)
				{
					return 384;
				}
				if (r.NextDouble() >= 0.75)
				{
					return 378;
				}
				return 380;
			}
			else
			{
				if (r.NextDouble() < 0.01 + (double)((float)(mineLevel - 120) / 2000f))
				{
					return 386;
				}
				if (r.NextDouble() < 0.75)
				{
					return 384;
				}
				if (r.NextDouble() >= 0.75)
				{
					return 378;
				}
				return 380;
			}
		}

		public int getMineArea(int level = -1)
		{
			if (level == -1)
			{
				level = this.mineLevel;
			}
			if (level >= 80 && level <= 120)
			{
				return 80;
			}
			if (level > 120)
			{
				return 121;
			}
			if (level >= 40)
			{
				return 40;
			}
			if (level > 10 && this.mineLevel < 30)
			{
				return 10;
			}
			return 0;
		}

		public byte getWallAt(int x, int y)
		{
			return 255;
		}

		public Color getLightingColor(GameTime time)
		{
			return this.lighting;
		}

		public StardewValley.Object getRandomItemForThisLevel(int level)
		{
			int parentSheetIndex = 0;
			if (this.mineRandom.NextDouble() < 0.05 && level > 80)
			{
				parentSheetIndex = 422;
			}
			else if (this.mineRandom.NextDouble() < 0.1 && level > 20 && this.getMineArea(-1) != 40)
			{
				parentSheetIndex = 420;
			}
			else if (this.mineRandom.NextDouble() < 0.25)
			{
				int mineArea = this.getMineArea(-1);
				if (mineArea <= 10)
				{
					if (mineArea == 0 || mineArea == 10)
					{
						parentSheetIndex = 86;
					}
				}
				else if (mineArea != 40)
				{
					if (mineArea != 80)
					{
						if (mineArea == 121)
						{
							parentSheetIndex = ((this.mineRandom.NextDouble() < 0.3) ? 86 : ((this.mineRandom.NextDouble() < 0.3) ? 84 : 82));
						}
					}
					else
					{
						parentSheetIndex = 82;
					}
				}
				else
				{
					parentSheetIndex = 84;
				}
			}
			else
			{
				parentSheetIndex = 80;
			}
			return new StardewValley.Object(parentSheetIndex, 1, false, -1, 0)
			{
				isSpawnedObject = true
			};
		}

		public int getRandomGemRichStoneForThisLevel(int level)
		{
			int num = this.mineRandom.Next(59, 70);
			num += num % 2;
			if (Game1.player.timesReachedMineBottom == 0)
			{
				if (level < 40 && num != 66 && num != 68)
				{
					num = ((this.mineRandom.NextDouble() < 0.5) ? 66 : 68);
				}
				else if (level < 80 && (num == 64 || num == 60))
				{
					num = ((this.mineRandom.NextDouble() < 0.5) ? ((this.mineRandom.NextDouble() < 0.5) ? 66 : 70) : ((this.mineRandom.NextDouble() < 0.5) ? 68 : 62));
				}
			}
			switch (num)
			{
			case 60:
				return 12;
			case 62:
				return 14;
			case 64:
				return 4;
			case 66:
				return 8;
			case 68:
				return 10;
			case 70:
				return 6;
			}
			return 40;
		}

		public Monster getMonsterForThisLevel(int level, int xTile, int yTile)
		{
			Vector2 vector = new Vector2((float)xTile, (float)yTile) * (float)Game1.tileSize;
			float num = Utility.distance((float)xTile, this.tileBeneathLadder.X, (float)yTile, this.tileBeneathLadder.Y);
			if (!this.isSlimeArea)
			{
				if (level < 40)
				{
					if (this.mineRandom.NextDouble() < 0.25 && !this.mustKillAllMonstersToAdvance())
					{
						return new Bug(vector, this.mineRandom.Next(4));
					}
					if (level < 15)
					{
						if (base.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null)
						{
							return new Duggy(vector);
						}
						if (this.mineRandom.NextDouble() < 0.15)
						{
							return new RockCrab(vector);
						}
						return new GreenSlime(vector, level);
					}
					else if (level <= 30)
					{
						if (base.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null)
						{
							return new Duggy(vector);
						}
						if (this.mineRandom.NextDouble() < 0.15)
						{
							return new RockCrab(vector);
						}
						if (this.mineRandom.NextDouble() < 0.05 && num > 10f)
						{
							return new Fly(vector, false);
						}
						if (this.mineRandom.NextDouble() < 0.45)
						{
							return new GreenSlime(vector, level);
						}
						return new Grub(vector, false);
					}
					else if (level <= 40)
					{
						if (this.mineRandom.NextDouble() < 0.1 && num > 10f)
						{
							return new Bat(vector, level);
						}
						return new RockGolem(vector);
					}
				}
				else if (this.getMineArea(-1) == 40)
				{
					if (this.mineLevel >= 70 && this.mineRandom.NextDouble() < 0.75)
					{
						return new Skeleton(vector);
					}
					if (this.mineRandom.NextDouble() < 0.3)
					{
						return new DustSpirit(vector, this.mineRandom.NextDouble() < 0.8);
					}
					if (this.mineRandom.NextDouble() < 0.3 && num > 10f)
					{
						return new Bat(vector, this.mineLevel);
					}
					if (!this.ghostAdded && this.mineLevel > 50 && this.mineRandom.NextDouble() < 0.3 && num > 10f)
					{
						this.ghostAdded = true;
						return new Ghost(vector);
					}
				}
				else if (this.getMineArea(-1) == 80)
				{
					if (this.isDarkArea() && this.mineRandom.NextDouble() < 0.25)
					{
						return new Bat(vector, this.mineLevel);
					}
					if (this.mineRandom.NextDouble() < 0.15)
					{
						return new GreenSlime(vector, this.getMineArea(-1));
					}
					if (this.mineRandom.NextDouble() < 0.15)
					{
						return new MetalHead(vector, this.getMineArea(-1));
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new ShadowBrute(vector);
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new ShadowShaman(vector);
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new RockCrab(vector, "Lava Crab");
					}
					if (this.mineRandom.NextDouble() < 0.2 && num > 8f && this.mineLevel >= 90)
					{
						return new SquidKid(vector);
					}
				}
				else if (this.getMineArea(-1) == 121)
				{
					if (this.loadedDarkArea)
					{
						return new Mummy(vector);
					}
					if (this.mineLevel % 20 == 0 && num > 10f)
					{
						return new Bat(vector, this.mineLevel);
					}
					if (this.mineLevel % 16 == 0 && !this.mustKillAllMonstersToAdvance())
					{
						return new Bug(vector, this.mineRandom.Next(4));
					}
					if (this.mineRandom.NextDouble() < 0.33 && num > 10f)
					{
						return new Serpent(vector);
					}
					if (this.mineRandom.NextDouble() < 0.33 && !this.mustKillAllMonstersToAdvance())
					{
						return new Bug(vector, this.mineRandom.Next(4));
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new GreenSlime(vector, level);
					}
					return new BigSlime(vector);
				}
				return new GreenSlime(vector, level);
			}
			if (this.mineRandom.NextDouble() < 0.2)
			{
				return new BigSlime(vector, this.getMineArea(-1));
			}
			return new GreenSlime(vector, this.mineLevel);
		}

		public Color getCrystalColorForThisLevel()
		{
			Random random = new Random(this.mineLevel + Game1.player.timesReachedMineBottom);
			if (random.NextDouble() < 0.04 && this.mineLevel < 80)
			{
				Color result = new Color(this.mineRandom.Next(256), this.mineRandom.Next(256), this.mineRandom.Next(256));
				while ((int)(result.R + result.G + result.B) < 500)
				{
					result.R = (byte)Math.Min(255, (int)(result.R + 10));
					result.G = (byte)Math.Min(255, (int)(result.G + 10));
					result.B = (byte)Math.Min(255, (int)(result.B + 10));
				}
				return result;
			}
			if (random.NextDouble() < 0.07)
			{
				return new Color(255 - this.mineRandom.Next(20), 255 - this.mineRandom.Next(20), 255 - this.mineRandom.Next(20));
			}
			if (this.mineLevel < 40)
			{
				int num = this.mineRandom.Next(2);
				if (num == 0)
				{
					return new Color(58, 145, 72);
				}
				if (num == 1)
				{
					return new Color(255, 255, 255);
				}
			}
			else if (this.mineLevel < 80)
			{
				switch (this.mineRandom.Next(4))
				{
				case 0:
					return new Color(120, 0, 210);
				case 1:
					return new Color(0, 100, 170);
				case 2:
					return new Color(0, 220, 255);
				case 3:
					return new Color(0, 255, 220);
				}
			}
			else
			{
				int num = this.mineRandom.Next(2);
				if (num == 0)
				{
					return new Color(200, 100, 0);
				}
				if (num == 1)
				{
					return new Color(220, 60, 0);
				}
			}
			return Color.White;
		}

		private StardewValley.Object chooseStoneType(double chanceForPurpleStone, double chanceForMysticStone, double gemStoneChance, Vector2 tile)
		{
			int minutesUntilReady = 1;
			int num;
			if (this.mineLevel < 40)
			{
				num = this.mineRandom.Next(31, 42);
				if (this.mineLevel % 40 < 30 && num >= 33 && num < 38)
				{
					num = ((this.mineRandom.NextDouble() < 0.5) ? 32 : 38);
				}
				else if (this.mineLevel % 40 >= 30)
				{
					num = ((this.mineRandom.NextDouble() < 0.5) ? 34 : 36);
				}
				if (this.mineLevel != 1 && this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
				{
					return new StardewValley.Object(tile, 751, "Stone", true, false, false, false)
					{
						minutesUntilReady = 3
					};
				}
			}
			else if (this.mineLevel < 80)
			{
				num = this.mineRandom.Next(47, 54);
				minutesUntilReady = 3;
				if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
				{
					return new StardewValley.Object(tile, 290, "Stone", true, false, false, false)
					{
						minutesUntilReady = 4
					};
				}
			}
			else if (this.mineLevel < 120)
			{
				minutesUntilReady = 4;
				if (this.mineRandom.NextDouble() < 0.3 && !this.isDarkArea())
				{
					if (this.mineRandom.NextDouble() < 0.5)
					{
						num = 38;
					}
					else
					{
						num = 32;
					}
				}
				else if (this.mineRandom.NextDouble() < 0.3)
				{
					num = this.mineRandom.Next(55, 58);
				}
				else if (this.mineRandom.NextDouble() < 0.5)
				{
					num = 760;
				}
				else
				{
					num = 762;
				}
				if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
				{
					return new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
					{
						minutesUntilReady = 8
					};
				}
			}
			else
			{
				minutesUntilReady = 5;
				if (this.mineRandom.NextDouble() < 0.5)
				{
					if (this.mineRandom.NextDouble() < 0.5)
					{
						num = 38;
					}
					else
					{
						num = 32;
					}
				}
				else if (this.mineRandom.NextDouble() < 0.5)
				{
					num = 40;
				}
				else
				{
					num = 42;
				}
				double num2 = 0.02 + (double)(this.mineLevel - 120) * 0.0005;
				if (this.mineLevel >= 130)
				{
					num2 += 0.01 * (double)((float)(this.mineLevel - 120 - 10) / 10f);
				}
				double num3 = 0.0;
				if (this.mineLevel >= 130)
				{
					num3 += 0.001 * (double)((float)(this.mineLevel - 120 - 10) / 10f);
				}
				num3 = Math.Min(num3, 0.004);
				if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < num2)
				{
					double num4 = (double)(this.mineLevel - 120) * (0.0003 + num3);
					double num5 = 0.01 + (double)(this.mineLevel - 120) * 0.0005;
					double num6 = Math.Min(0.5, 0.1 + (double)(this.mineLevel - 120) * 0.005);
					if (this.mineRandom.NextDouble() < num4)
					{
						return new StardewValley.Object(tile, 765, "Stone", true, false, false, false)
						{
							minutesUntilReady = 16
						};
					}
					if (this.mineRandom.NextDouble() < num5)
					{
						return new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
						{
							minutesUntilReady = 8
						};
					}
					if (this.mineRandom.NextDouble() < num6)
					{
						return new StardewValley.Object(tile, 290, "Stone", true, false, false, false)
						{
							minutesUntilReady = 4
						};
					}
					return new StardewValley.Object(tile, 751, "Stone", true, false, false, false)
					{
						minutesUntilReady = 2
					};
				}
			}
			double num7 = Game1.dailyLuck / 2.0 + (double)Game1.player.MiningLevel * 0.005;
			if (this.mineLevel > 50 && this.mineRandom.NextDouble() < 0.00025 + (double)this.mineLevel / 120000.0 + 0.0005 * num7 / 2.0)
			{
				num = 2;
				minutesUntilReady = 10;
			}
			else if (gemStoneChance != 0.0 && this.mineRandom.NextDouble() < gemStoneChance + gemStoneChance * num7 + (double)this.mineLevel / 24000.0)
			{
				return new StardewValley.Object(tile, this.getRandomGemRichStoneForThisLevel(this.mineLevel), "Stone", true, false, false, false)
				{
					minutesUntilReady = 5
				};
			}
			if (this.mineRandom.NextDouble() < chanceForPurpleStone / 2.0 + chanceForPurpleStone * (double)Game1.player.MiningLevel * 0.008 + chanceForPurpleStone * (Game1.dailyLuck / 2.0))
			{
				num = 44;
			}
			if (this.mineLevel > 100 && this.mineRandom.NextDouble() < chanceForMysticStone + chanceForMysticStone * (double)Game1.player.MiningLevel * 0.008 + chanceForMysticStone * (Game1.dailyLuck / 2.0))
			{
				num = 46;
			}
			num += num % 2;
			if (this.mineRandom.NextDouble() < 0.1 && this.getMineArea(-1) != 40)
			{
				return new StardewValley.Object(tile, (this.mineRandom.NextDouble() < 0.5) ? 668 : 670, "Stone", true, false, false, false)
				{
					minutesUntilReady = 2,
					flipped = (this.mineRandom.NextDouble() < 0.5)
				};
			}
			return new StardewValley.Object(tile, num, "Stone", true, false, false, false)
			{
				minutesUntilReady = minutesUntilReady
			};
		}

		public override void draw(SpriteBatch b)
		{
			foreach (ResourceClump current in this.resourceClumps)
			{
				current.draw(b, current.tile);
			}
			base.draw(b);
		}
	}
}
