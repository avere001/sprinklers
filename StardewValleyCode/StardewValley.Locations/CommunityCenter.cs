using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class CommunityCenter : GameLocation
	{
		public const int AREA_Pantry = 0;

		public const int AREA_FishTank = 2;

		public const int AREA_CraftsRoom = 1;

		public const int AREA_BoilerRoom = 3;

		public const int AREA_Vault = 4;

		public const int AREA_Bulletin = 5;

		public const int AREA_Bulletin2 = 6;

		public const int AREA_JunimoHut = 7;

		private bool refurbishedLoaded;

		private bool warehouse;

		public SerializableDictionary<int, bool[]> bundles;

		public SerializableDictionary<int, bool> bundleRewards;

		public bool[] areasComplete = new bool[6];

		public int numberOfStarsOnPlaque;

		private float messageAlpha;

		private List<int> junimoNotesViewportTargets;

		private Dictionary<int, List<int>> areaToBundleDictionary;

		private Dictionary<int, int> bundleToAreaDictionary;

		public const int PHASE_firstPause = 0;

		public const int PHASE_junimoAppear = 1;

		public const int PHASE_junimoDance = 2;

		public const int PHASE_restore = 3;

		private int restoreAreaTimer;

		private int restoreAreaPhase;

		private int restoreAreaIndex;

		private Cue buildUpSound;

		public CommunityCenter()
		{
		}

		public CommunityCenter(string name) : base(Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Ruins"), name)
		{
			Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
			this.bundles = new SerializableDictionary<int, bool[]>();
			this.bundleRewards = new SerializableDictionary<int, bool>();
			this.areaToBundleDictionary = new Dictionary<int, List<int>>();
			this.bundleToAreaDictionary = new Dictionary<int, int>();
			for (int i = 0; i < 6; i++)
			{
				this.areaToBundleDictionary.Add(i, new List<int>());
			}
			foreach (KeyValuePair<string, string> current in dictionary)
			{
				this.bundles.Add(Convert.ToInt32(current.Key.Split(new char[]
				{
					'/'
				})[1]), new bool[current.Value.Split(new char[]
				{
					'/'
				})[2].Split(new char[]
				{
					' '
				}).Length]);
				this.bundleRewards.Add(Convert.ToInt32(current.Key.Split(new char[]
				{
					'/'
				})[1]), false);
				this.areaToBundleDictionary[this.getAreaNumberFromName(current.Key.Split(new char[]
				{
					'/'
				})[0])].Add(Convert.ToInt32(current.Key.Split(new char[]
				{
					'/'
				})[1]));
				this.bundleToAreaDictionary.Add(Convert.ToInt32(current.Key.Split(new char[]
				{
					'/'
				})[1]), this.getAreaNumberFromName(current.Key.Split(new char[]
				{
					'/'
				})[0]));
			}
		}

		private int getAreaNumberFromName(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2486580683u)
			{
				if (num <= 696049845u)
				{
					if (num != 244995591u)
					{
						if (num != 696049845u)
						{
							return -1;
						}
						if (!(name == "Pantry"))
						{
							return -1;
						}
						return 0;
					}
					else
					{
						if (!(name == "FishTank"))
						{
							return -1;
						}
						return 2;
					}
				}
				else if (num != 1618314778u)
				{
					if (num != 1881810045u)
					{
						if (num != 2486580683u)
						{
							return -1;
						}
						if (!(name == "BoilerRoom"))
						{
							return -1;
						}
						return 3;
					}
					else if (!(name == "CraftsRoom"))
					{
						return -1;
					}
				}
				else
				{
					if (!(name == "Bulletin"))
					{
						return -1;
					}
					return 5;
				}
			}
			else if (num <= 3168044721u)
			{
				if (num != 2576994461u)
				{
					if (num != 3063871366u)
					{
						if (num != 3168044721u)
						{
							return -1;
						}
						if (!(name == "Crafts Room"))
						{
							return -1;
						}
					}
					else
					{
						if (!(name == "Bulletin Board"))
						{
							return -1;
						}
						return 5;
					}
				}
				else
				{
					if (!(name == "Fish Tank"))
					{
						return -1;
					}
					return 2;
				}
			}
			else if (num != 3170708731u)
			{
				if (num != 3560083791u)
				{
					if (num != 4104466714u)
					{
						return -1;
					}
					if (!(name == "BulletinBoard"))
					{
						return -1;
					}
					return 5;
				}
				else
				{
					if (!(name == "Vault"))
					{
						return -1;
					}
					return 4;
				}
			}
			else
			{
				if (!(name == "Boiler Room"))
				{
					return -1;
				}
				return 3;
			}
			return 1;
		}

		private Point getNotePosition(int area)
		{
			switch (area)
			{
			case 0:
				return new Point(14, 5);
			case 1:
				return new Point(14, 23);
			case 2:
				return new Point(40, 10);
			case 3:
				return new Point(63, 14);
			case 4:
				return new Point(55, 6);
			case 5:
				return new Point(46, 11);
			default:
				return Point.Zero;
			}
		}

		public void addJunimoNote(int area)
		{
			Point notePosition = this.getNotePosition(area);
			if (!notePosition.Equals(Vector2.Zero))
			{
				StaticTile[] junimoNoteTileFrames = this.getJunimoNoteTileFrames(area);
				string layerId = (area == 5) ? "Front" : "Buildings";
				this.map.GetLayer(layerId).Tiles[notePosition.X, notePosition.Y] = new AnimatedTile(this.map.GetLayer(layerId), junimoNoteTileFrames, 70L);
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(notePosition.X * Game1.tileSize), (float)(notePosition.Y * Game1.tileSize)), 1f));
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(notePosition.X * Game1.tileSize), (float)(notePosition.Y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f)
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(notePosition.X * Game1.tileSize - Game1.pixelZoom * 3), (float)(notePosition.Y * Game1.tileSize - Game1.pixelZoom * 3)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					scale = 0.75f,
					layerDepth = 1f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f),
					delayBeforeAnimationStart = 50
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(notePosition.X * Game1.tileSize - Game1.pixelZoom * 3), (float)(notePosition.Y * Game1.tileSize + Game1.pixelZoom * 3)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f),
					delayBeforeAnimationStart = 100
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(notePosition.X * Game1.tileSize), (float)(notePosition.Y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					scale = 0.75f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f),
					delayBeforeAnimationStart = 150
				});
			}
		}

		public int numberOfCompleteBundles()
		{
			int num = 0;
			foreach (KeyValuePair<int, bool[]> current in this.bundles)
			{
				num++;
				for (int i = 0; i < current.Value.Length; i++)
				{
					if (!current.Value[i])
					{
						num--;
						break;
					}
				}
			}
			return num;
		}

		public void addStarToPlaque()
		{
			this.numberOfStarsOnPlaque++;
		}

		private string getMessageForAreaCompletion()
		{
			int numberOfAreasComplete = this.getNumberOfAreasComplete();
			if (numberOfAreasComplete >= 1 && numberOfAreasComplete <= 6)
			{
				return Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaCompletion" + numberOfAreasComplete, new object[]
				{
					Game1.player.name
				});
			}
			return "";
		}

		private int getNumberOfAreasComplete()
		{
			int num = 0;
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				if (this.areasComplete[i])
				{
					num++;
				}
			}
			return num;
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int num = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (num != 1799)
			{
				switch (num)
				{
				case 1824:
				case 1825:
				case 1826:
				case 1827:
				case 1828:
				case 1829:
				case 1830:
				case 1831:
				case 1832:
				case 1833:
					Game1.activeClickableMenu = new JunimoNoteMenu(this.getAreaNumberFromLocation(who.getTileLocation()), this.bundles);
					break;
				}
			}
			else if (this.numberOfCompleteBundles() > 2)
			{
				Game1.activeClickableMenu = new JunimoNoteMenu(5, this.bundles);
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public void addJunimoNoteViewportTarget(int area)
		{
			if (this.junimoNotesViewportTargets == null)
			{
				this.junimoNotesViewportTargets = new List<int>();
			}
			this.junimoNotesViewportTargets.Add(area);
		}

		public void checkForNewJunimoNotes()
		{
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				if (!this.isJunimoNoteAtArea(i) && this.shouldNoteAppearInArea(i))
				{
					this.addJunimoNoteViewportTarget(i);
				}
			}
		}

		public void removeJunimoNote(int area)
		{
			Point notePosition = this.getNotePosition(area);
			if (area == 5)
			{
				this.map.GetLayer("Front").Tiles[notePosition.X, notePosition.Y] = null;
				return;
			}
			this.map.GetLayer("Buildings").Tiles[notePosition.X, notePosition.Y] = null;
		}

		public bool isJunimoNoteAtArea(int area)
		{
			Point notePosition = this.getNotePosition(area);
			if (area == 5)
			{
				return this.map.GetLayer("Front").Tiles[notePosition.X, notePosition.Y] != null;
			}
			return this.map.GetLayer("Buildings").Tiles[notePosition.X, notePosition.Y] != null;
		}

		public bool shouldNoteAppearInArea(int area)
		{
			if (area >= 0 && this.areasComplete.Length > area && !this.areasComplete[area])
			{
				switch (area)
				{
				case 0:
				case 2:
					if (this.numberOfCompleteBundles() > 0)
					{
						return true;
					}
					break;
				case 1:
					return true;
				case 3:
					if (this.numberOfCompleteBundles() > 1)
					{
						return true;
					}
					break;
				case 4:
					if (this.numberOfCompleteBundles() > 3)
					{
						return true;
					}
					break;
				case 5:
					if (this.numberOfCompleteBundles() > 2)
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.mailReceived.Contains("JojaMember"))
			{
				this.map = Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Joja");
				this.warehouse = true;
				this.refurbishedLoaded = true;
			}
			else if (this.areAllAreasComplete() && !this.refurbishedLoaded)
			{
				this.map = Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Refurbished");
				this.refurbishedLoaded = true;
			}
			else
			{
				for (int i = 0; i < this.areasComplete.Length; i++)
				{
					if (this.shouldNoteAppearInArea(i))
					{
						this.addJunimoNote(i);
						this.characters.Add(new Junimo(new Vector2((float)this.getNotePosition(i).X, (float)(this.getNotePosition(i).Y + 2)) * (float)Game1.tileSize, i, false));
					}
					else if (this.areasComplete[i])
					{
						this.loadArea(i, false);
					}
				}
			}
			this.numberOfStarsOnPlaque = 0;
			for (int j = 0; j < this.areasComplete.Length; j++)
			{
				if (this.areasComplete[j])
				{
					this.numberOfStarsOnPlaque++;
				}
			}
			if (!Game1.eventUp && !this.areAllAreasComplete())
			{
				Game1.changeMusicTrack("communityCenter");
			}
		}

		private int getAreaNumberFromLocation(Vector2 tileLocation)
		{
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				if (this.getAreaBounds(i).Contains((int)tileLocation.X, (int)tileLocation.Y))
				{
					return i;
				}
			}
			return -1;
		}

		private Microsoft.Xna.Framework.Rectangle getAreaBounds(int area)
		{
			switch (area)
			{
			case 0:
				return new Microsoft.Xna.Framework.Rectangle(0, 0, 22, 11);
			case 1:
				return new Microsoft.Xna.Framework.Rectangle(0, 12, 21, 17);
			case 2:
				return new Microsoft.Xna.Framework.Rectangle(35, 4, 9, 9);
			case 3:
				return new Microsoft.Xna.Framework.Rectangle(52, 9, 16, 12);
			case 4:
				return new Microsoft.Xna.Framework.Rectangle(45, 0, 15, 9);
			case 5:
				return new Microsoft.Xna.Framework.Rectangle(22, 13, 28, 9);
			case 6:
				return new Microsoft.Xna.Framework.Rectangle(44, 10, 6, 3);
			case 7:
				return new Microsoft.Xna.Framework.Rectangle(22, 4, 13, 9);
			default:
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i] is Junimo)
				{
					this.characters.RemoveAt(i);
				}
			}
			Game1.changeMusicTrack("none");
		}

		public bool isBundleComplete(int bundleIndex)
		{
			for (int i = 0; i < this.bundles[bundleIndex].Length; i++)
			{
				if (!this.bundles[bundleIndex][i])
				{
					return false;
				}
			}
			return true;
		}

		public void areaCompleteReward(int whichArea)
		{
			string text = "";
			switch (whichArea)
			{
			case 0:
				text = "ccPantry";
				goto IL_A4;
			case 1:
				break;
			case 2:
				text = "ccFishTank";
				goto IL_A4;
			case 3:
				text = "ccBoilerRoom";
				goto IL_A4;
			case 4:
				text = "ccVault";
				goto IL_A4;
			case 5:
				text = "ccBulletin";
				Game1.addMailForTomorrow("ccBulletinThankYou", false, false);
				using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC current = enumerator.Current;
						if (!current.datable)
						{
							Game1.player.changeFriendship(500, current);
						}
					}
					goto IL_A4;
				}
				break;
			default:
				goto IL_A4;
			}
			text = "ccCraftsRoom";
			IL_A4:
			if (text.Length > 0 && !Game1.player.mailReceived.Contains(text))
			{
				Game1.player.mailForTomorrow.Add(text + "%&NL&%");
			}
		}

		public void completeBundle(int which)
		{
			bool flag = false;
			for (int i = 0; i < this.bundles[which].Length; i++)
			{
				if (!flag && !this.bundles[which][i])
				{
					flag = true;
				}
				this.bundles[which][i] = true;
			}
			if (flag)
			{
				this.bundleRewards[which] = true;
			}
			int num = this.bundleToAreaDictionary[which];
			if (!this.areasComplete[num])
			{
				bool flag2 = false;
				foreach (int current in this.areaToBundleDictionary[num])
				{
					if (!this.isBundleComplete(current))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					this.areasComplete[num] = true;
					this.areaCompleteReward(num);
					if (Game1.IsMultiplayer)
					{
						Game1.ChatBox.receiveChatMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaRestored", new object[]
						{
							CommunityCenter.getAreaDisplayNameFromNumber(num)
						}), -1L);
						return;
					}
					Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaRestored", new object[]
					{
						CommunityCenter.getAreaDisplayNameFromNumber(num)
					}));
				}
			}
		}

		public void loadArea(int area, bool showEffects = true)
		{
			Microsoft.Xna.Framework.Rectangle areaBounds = this.getAreaBounds(area);
			Map map = Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Refurbished");
			for (int i = areaBounds.X; i < areaBounds.Right; i++)
			{
				for (int j = areaBounds.Y; j < areaBounds.Bottom; j++)
				{
					if (map.GetLayer("Back").Tiles[i, j] != null)
					{
						this.map.GetLayer("Back").Tiles[i, j].TileIndex = map.GetLayer("Back").Tiles[i, j].TileIndex;
					}
					if (map.GetLayer("Buildings").Tiles[i, j] != null)
					{
						this.map.GetLayer("Buildings").Tiles[i, j] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Buildings").Tiles[i, j].TileIndex);
						base.adjustMapLightPropertiesForLamp(map.GetLayer("Buildings").Tiles[i, j].TileIndex, i, j, "Buildings");
					}
					else
					{
						this.map.GetLayer("Buildings").Tiles[i, j] = null;
					}
					if (map.GetLayer("Front").Tiles[i, j] != null)
					{
						this.map.GetLayer("Front").Tiles[i, j] = new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Front").Tiles[i, j].TileIndex);
						base.adjustMapLightPropertiesForLamp(map.GetLayer("Front").Tiles[i, j].TileIndex, i, j, "Front");
					}
					else
					{
						this.map.GetLayer("Front").Tiles[i, j] = null;
					}
					if (map.GetLayer("Paths").Tiles[i, j] != null && map.GetLayer("Paths").Tiles[i, j].TileIndex == 8)
					{
						Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(i * Game1.tileSize), (float)(j * Game1.tileSize)), 2f));
					}
					if (showEffects && Game1.random.NextDouble() < 0.58 && map.GetLayer("Buildings").Tiles[i, j] == null)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(i * Game1.tileSize), (float)(j * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
						{
							layerDepth = 1f,
							interval = 50f,
							motion = new Vector2((float)Game1.random.Next(17) / 10f, 0f),
							acceleration = new Vector2(-0.005f, 0f),
							delayBeforeAnimationStart = Game1.random.Next(500)
						});
					}
				}
			}
			if (area == 5)
			{
				this.loadArea(6, true);
			}
			base.addLightGlows();
		}

		public void restoreAreaCutscene(int whichArea)
		{
			Game1.freezeControls = true;
			this.restoreAreaIndex = whichArea;
			this.restoreAreaPhase = 0;
			this.restoreAreaTimer = 1000;
			Game1.changeMusicTrack("none");
			this.areasComplete[whichArea] = true;
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.restoreAreaTimer > 0)
			{
				int num = this.restoreAreaTimer;
				this.restoreAreaTimer -= time.ElapsedGameTime.Milliseconds;
				switch (this.restoreAreaPhase)
				{
				case 0:
					if (this.restoreAreaTimer <= 0)
					{
						this.restoreAreaTimer = 3000;
						this.restoreAreaPhase = 1;
						Game1.player.faceDirection(2);
						Game1.player.jump();
						Game1.player.jitterStrength = 1f;
						Game1.player.showFrame(94, false);
						return;
					}
					break;
				case 1:
					if (Game1.random.NextDouble() < 0.4)
					{
						Vector2 randomPositionInThisRectangle = Utility.getRandomPositionInThisRectangle(this.getAreaBounds(this.restoreAreaIndex), Game1.random);
						Junimo junimo = new Junimo(randomPositionInThisRectangle * (float)Game1.tileSize, this.restoreAreaIndex, true);
						if (!base.isCollidingPosition(junimo.GetBoundingBox(), Game1.viewport, junimo))
						{
							this.characters.Add(junimo);
							this.temporarySprites.Add(new TemporaryAnimatedSprite((Game1.random.NextDouble() < 0.5) ? 5 : 46, randomPositionInThisRectangle * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
							{
								layerDepth = 1f
							});
							Game1.playSound("tinyWhip");
						}
					}
					if (this.restoreAreaTimer <= 0)
					{
						this.restoreAreaTimer = 999999;
						this.restoreAreaPhase = 2;
						Game1.screenGlowOnce(Color.White, true, 0.005f, 1f);
						if (Game1.soundBank != null)
						{
							this.buildUpSound = Game1.soundBank.GetCue("wind");
							this.buildUpSound.SetVariable("Volume", 0f);
							this.buildUpSound.SetVariable("Frequency", 0f);
							this.buildUpSound.Play();
						}
						Game1.player.jitterStrength = 2f;
						Game1.player.stopShowingFrame();
					}
					Game1.drawLighting = false;
					return;
				case 2:
					if (this.buildUpSound != null)
					{
						this.buildUpSound.SetVariable("Volume", Game1.screenGlowAlpha * 150f);
						this.buildUpSound.SetVariable("Frequency", Game1.screenGlowAlpha * 150f);
					}
					if (Game1.screenGlowAlpha >= Game1.screenGlowMax)
					{
						this.messageAlpha += 0.008f;
						this.messageAlpha = Math.Min(this.messageAlpha, 1f);
					}
					if (Game1.screenGlowAlpha == Game1.screenGlowMax && this.restoreAreaTimer > 5200)
					{
						this.restoreAreaTimer = 5200;
					}
					if (this.restoreAreaTimer < 5200 && Game1.random.NextDouble() < (double)((float)(5200 - this.restoreAreaTimer) / 10000f))
					{
						Game1.playSound((Game1.random.NextDouble() < 0.5) ? "dustMeep" : "junimoMeep1");
					}
					if (this.restoreAreaTimer <= 0)
					{
						this.restoreAreaTimer = 2000;
						this.restoreAreaPhase = 3;
						Game1.screenGlowHold = false;
						this.loadArea(this.restoreAreaIndex, true);
						if (this.buildUpSound != null)
						{
							this.buildUpSound.Stop(AudioStopOptions.Immediate);
						}
						Game1.playSound("wand");
						Game1.changeMusicTrack("junimoStarSong");
						Game1.playSound("woodyHit");
						this.messageAlpha = 0f;
						Game1.flashAlpha = 1f;
						Game1.player.stopJittering();
						for (int i = this.characters.Count - 1; i >= 0; i--)
						{
							if (this.characters[i] is Junimo && (this.characters[i] as Junimo).temporaryJunimo)
							{
								this.characters.RemoveAt(i);
							}
						}
						Game1.drawLighting = true;
						return;
					}
					break;
				case 3:
					if (num > 1000 && this.restoreAreaTimer <= 1000)
					{
						Junimo junimoForArea = this.getJunimoForArea(this.restoreAreaIndex);
						if (junimoForArea != null)
						{
							junimoForArea.position = Utility.getRandomAdjacentOpenTile(Game1.player.getTileLocation()) * (float)Game1.tileSize;
							int num2 = 0;
							while (base.isCollidingPosition(junimoForArea.GetBoundingBox(), Game1.viewport, junimoForArea) && num2 < 20)
							{
								junimoForArea.position = Utility.getRandomPositionInThisRectangle(this.getAreaBounds(this.restoreAreaIndex), Game1.random);
								num2++;
							}
							if (num2 < 20)
							{
								junimoForArea.fadeBack();
								junimoForArea.returnToJunimoHutToFetchStar(this);
							}
						}
					}
					if (this.restoreAreaTimer <= 0)
					{
						Game1.freezeControls = false;
						return;
					}
					break;
				default:
					return;
				}
			}
			else if (Game1.activeClickableMenu == null && this.junimoNotesViewportTargets != null && this.junimoNotesViewportTargets.Count > 0 && !Game1.isViewportOnCustomPath())
			{
				this.setViewportToNextJunimoNoteTarget();
			}
		}

		private void setViewportToNextJunimoNoteTarget()
		{
			if (this.junimoNotesViewportTargets.Count > 0)
			{
				Game1.freezeControls = true;
				int area = this.junimoNotesViewportTargets[0];
				Point notePosition = this.getNotePosition(area);
				Game1.moveViewportTo(new Vector2((float)notePosition.X, (float)notePosition.Y) * (float)Game1.tileSize, 5f, 2000, new Game1.afterFadeFunction(this.afterViewportGetsToJunimoNotePosition), new Game1.afterFadeFunction(this.setViewportToNextJunimoNoteTarget));
				return;
			}
			Game1.viewportFreeze = true;
			Game1.viewportHold = 10000;
			Game1.globalFadeToBlack(new Game1.afterFadeFunction(Game1.afterFadeReturnViewportToPlayer), 0.02f);
			Game1.freezeControls = false;
			Game1.afterViewport = null;
		}

		private void afterViewportGetsToJunimoNotePosition()
		{
			int area = this.junimoNotesViewportTargets[0];
			this.junimoNotesViewportTargets.RemoveAt(0);
			this.addJunimoNote(area);
			Game1.playSound("reward");
		}

		public Junimo getJunimoForArea(int whichArea)
		{
			foreach (Character current in this.characters)
			{
				if (current is Junimo && (current as Junimo).whichArea == whichArea)
				{
					return current as Junimo;
				}
			}
			Junimo junimo = new Junimo(Vector2.Zero, whichArea, false);
			base.addCharacter(junimo);
			return junimo;
		}

		public bool areAllAreasComplete()
		{
			bool[] array = this.areasComplete;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					return false;
				}
			}
			return true;
		}

		public void junimoGoodbyeDance()
		{
			this.getJunimoForArea(0).position = new Vector2(23f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(1).position = new Vector2(27f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(2).position = new Vector2(24f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(4).position = new Vector2(26f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(3).position = new Vector2(28f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(5).position = new Vector2(25f, 11f) * (float)Game1.tileSize;
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				this.getJunimoForArea(i).stayStill();
				this.getJunimoForArea(i).faceDirection(1);
				this.getJunimoForArea(i).fadeBack();
				this.getJunimoForArea(i).isInvisible = false;
				this.getJunimoForArea(i).setAlpha(1f);
			}
			Game1.moveViewportTo(new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY()), 2f, 5000, new Game1.afterFadeFunction(this.startGoodbyeDance), new Game1.afterFadeFunction(this.endGoodbyeDance));
			Game1.viewportFreeze = false;
			Game1.freezeControls = true;
		}

		private void startGoodbyeDance()
		{
			Game1.freezeControls = true;
			this.getJunimoForArea(0).position = new Vector2(23f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(1).position = new Vector2(27f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(2).position = new Vector2(24f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(4).position = new Vector2(26f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(3).position = new Vector2(28f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(5).position = new Vector2(25f, 11f) * (float)Game1.tileSize;
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				this.getJunimoForArea(i).stayStill();
				this.getJunimoForArea(i).faceDirection(1);
				this.getJunimoForArea(i).fadeBack();
				this.getJunimoForArea(i).isInvisible = false;
				this.getJunimoForArea(i).setAlpha(1f);
				this.getJunimoForArea(i).sayGoodbye();
			}
		}

		private void endGoodbyeDance()
		{
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				this.getJunimoForArea(i).fadeAway();
			}
			Game1.pauseThenDoFunction(3600, new Game1.afterFadeFunction(this.loadJunimoHut));
			Game1.freezeControls = true;
		}

		private void loadJunimoHut()
		{
			this.loadArea(7, true);
			Game1.flashAlpha = 1f;
			Game1.playSound("wand");
			Game1.freezeControls = false;
			Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_JunimosReturned", new object[0]));
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			for (int i = 0; i < this.numberOfStarsOnPlaque; i++)
			{
				switch (i)
				{
				case 0:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize + 6 * Game1.pixelZoom), (float)(5 * Game1.tileSize + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 1:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize + 6 * Game1.pixelZoom), (float)(5 * Game1.tileSize + 11 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 2:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize - 4 * Game1.pixelZoom), (float)(6 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 3:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(32 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(5 * Game1.tileSize + 11 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 4:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(32 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(5 * Game1.tileSize + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 5:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize - 4 * Game1.pixelZoom), (float)(5 * Game1.tileSize - 3 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				}
			}
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			if (this.messageAlpha > 0f)
			{
				Junimo junimoForArea = this.getJunimoForArea(0);
				if (junimoForArea != null)
				{
					b.Draw(junimoForArea.Sprite.Texture, new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize / 2), (float)(Game1.viewport.Height * 2) / 3f - (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800.0) / 100 * 16, 0, 16, 16)), Color.Lime * this.messageAlpha, 0f, new Vector2((float)(junimoForArea.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(junimoForArea.sprite.spriteHeight * Game1.pixelZoom) * 3f / 4f) / (float)Game1.pixelZoom, Math.Max(0.2f, 1f) * (float)Game1.pixelZoom, junimoForArea.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
				}
				b.DrawString(Game1.dialogueFont, "\"" + Game1.parseText(this.getMessageForAreaCompletion() + "\"", Game1.dialogueFont, Game1.tileSize * 10), new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 5), (float)(Game1.viewport.Height * 2) / 3f), Game1.textColor * this.messageAlpha * 0.6f);
			}
		}

		public static string getAreaNameFromNumber(int areaNumber)
		{
			switch (areaNumber)
			{
			case 0:
				return "Pantry";
			case 1:
				return "Crafts Room";
			case 2:
				return "Fish Tank";
			case 3:
				return "Boiler Room";
			case 4:
				return "Vault";
			case 5:
				return "Bulletin Board";
			default:
				return "";
			}
		}

		public static string getAreaEnglishDisplayNameFromNumber(int areaNumber)
		{
			return Game1.content.LoadBaseString("Strings\\Locations:CommunityCenter_AreaName_" + CommunityCenter.getAreaNameFromNumber(areaNumber).Replace(" ", ""), new object[0]);
		}

		public static string getAreaDisplayNameFromNumber(int areaNumber)
		{
			return Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaName_" + CommunityCenter.getAreaNameFromNumber(areaNumber).Replace(" ", ""), new object[0]);
		}

		private StaticTile[] getJunimoNoteTileFrames(int area)
		{
			if (area == 5)
			{
				return new StaticTile[]
				{
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1773),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1805),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1805),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1773)
				};
			}
			return new StaticTile[]
			{
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1832),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1824),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1825),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1826),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1827),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1828),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1829),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1830),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1831),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1832),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833)
			};
		}
	}
}
