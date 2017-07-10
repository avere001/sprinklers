using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Minigames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StardewValley.Menus
{
	public class TitleMenu : IClickableMenu, IDisposable
	{
		public const int region_muteMusic = 81111;

		public const int region_windowedButton = 81112;

		public const int region_aboutButton = 81113;

		public const int region_backButton = 81114;

		public const int region_newButton = 81115;

		public const int region_loadButton = 81116;

		public const int region_exitButton = 81117;

		public const int region_languagesButton = 81118;

		public const int fadeFromWhiteDuration = 2000;

		public const int viewportFinalPosition = -1000;

		public const int logoSwipeDuration = 1000;

		public const int numberOfButtons = 3;

		public const int spaceBetweenButtons = 8;

		public const float bigCloudDX = 0.1f;

		public const float mediumCloudDX = 0.2f;

		public const float smallCloudDX = 0.3f;

		public const float bgmountainsParallaxSpeed = 0.66f;

		public const float mountainsParallaxSpeed = 1f;

		public const float foregroundJungleParallaxSpeed = 2f;

		public const float cloudsParallaxSpeed = 0.5f;

		public const int pixelZoom = 3;

		public LocalizedContentManager menuContent = Game1.content.CreateTemporary();

		private Texture2D cloudsTexture;

		private Texture2D titleButtonsTexture;

		private List<float> bigClouds = new List<float>();

		private List<float> smallClouds = new List<float>();

		private List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();

		public List<ClickableTextureComponent> buttons = new List<ClickableTextureComponent>();

		public ClickableTextureComponent backButton;

		public ClickableTextureComponent muteMusicButton;

		public ClickableTextureComponent aboutButton;

		public ClickableTextureComponent languageButton;

		public ClickableTextureComponent windowedButton;

		public ClickableComponent skipButton;

		private List<TemporaryAnimatedSprite> birds = new List<TemporaryAnimatedSprite>();

		private Rectangle eRect;

		private List<Rectangle> leafRects;

		private static IClickableMenu _subMenu;

		private StartupPreferences startupPreferences;

		private int globalXOffset;

		private float viewportY;

		private float viewportDY;

		private float logoSwipeTimer;

		private float globalCloudAlpha = 1f;

		private int numFarmsSaved = -1;

		private int fadeFromWhiteTimer;

		private int pauseBeforeViewportRiseTimer;

		private int buttonsToShow;

		private int showButtonsTimer;

		private int logoFadeTimer;

		private int logoSurprisedTimer;

		private int clicksOnE;

		private int clicksOnLeaf;

		private int buttonsDX;

		private int chuckleFishTimer;

		private bool titleInPosition;

		private bool isTransitioningButtons;

		private bool shades;

		private bool transitioningCharacterCreationMenu;

		private static int windowNumber = 3;

		public string startupMessage = "";

		public Color startupMessageColor = Color.DeepSkyBlue;

		private int bCount;

		private string whichSubMenu = "";

		private int quitTimer;

		private bool transitioningFromLoadScreen;

		private bool disposedValue;

		public static IClickableMenu subMenu
		{
			get
			{
				return TitleMenu._subMenu;
			}
			set
			{
				if (TitleMenu._subMenu != null && TitleMenu._subMenu is IDisposable)
				{
					(TitleMenu._subMenu as IDisposable).Dispose();
				}
				TitleMenu._subMenu = value;
			}
		}

		private bool HasActiveUser
		{
			get
			{
				return true;
			}
		}

		public TitleMenu() : base(0, 0, Game1.viewport.Width, Game1.viewport.Height, false)
		{
			LocalizedContentManager.OnLanguageChange += new LocalizedContentManager.LanguageChangedHandler(this.OnLanguageChange);
			this.cloudsTexture = this.menuContent.Load<Texture2D>(Path.Combine("Minigames", "Clouds"));
			this.titleButtonsTexture = this.menuContent.Load<Texture2D>(Path.Combine("Minigames", "TitleButtons"));
			this.viewportY = 0f;
			this.fadeFromWhiteTimer = 4000;
			this.logoFadeTimer = 5000;
			this.chuckleFishTimer = 4000;
			this.bigClouds.Add(-750f);
			this.bigClouds.Add((float)(this.width * 3 / 4));
			this.shades = (Game1.random.NextDouble() < 0.5);
			this.smallClouds.Add((float)(this.width / 2));
			this.smallClouds.Add((float)(this.width - 1));
			this.smallClouds.Add(1f);
			this.smallClouds.Add((float)(this.width / 3));
			this.smallClouds.Add((float)(this.width * 2 / 3));
			this.smallClouds.Add((float)(this.width * 3 / 4));
			this.smallClouds.Add((float)(this.width / 4));
			this.smallClouds.Add((float)(this.width / 2 + 300));
			this.smallClouds.Add((float)(this.width - 1 + 300));
			this.smallClouds.Add(301f);
			this.smallClouds.Add((float)(this.width / 3 + 300));
			this.smallClouds.Add((float)(this.width * 2 / 3 + 300));
			this.smallClouds.Add((float)(this.width * 3 / 4 + 300));
			this.smallClouds.Add((float)(this.width / 4 + 300));
			if (Game1.currentSong == null && Game1.nextMusicTrack != null)
			{
				int arg_2A9_0 = Game1.nextMusicTrack.Length;
			}
			this.birds.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 227, 26, 21), new Vector2((float)(this.width - 210), (float)(this.height - 390)), false, 0f, Color.White)
			{
				scale = 3f,
				pingPong = true,
				animationLength = 4,
				interval = 100f,
				totalNumberOfLoops = 9999,
				local = true,
				motion = new Vector2(-1f, 0f),
				layerDepth = 0.25f
			});
			this.birds.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 227, 26, 21), new Vector2((float)(this.width - 120), (float)(this.height - 360)), false, 0f, Color.White)
			{
				scale = 3f,
				pingPong = true,
				animationLength = 4,
				interval = 100f,
				totalNumberOfLoops = 9999,
				local = true,
				delayBeforeAnimationStart = 100,
				motion = new Vector2(-1f, 0f),
				layerDepth = 0.25f
			});
			this.setUpIcons();
			this.muteMusicButton = new ClickableTextureComponent(new Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(128, 384, 9, 9), (float)Game1.pixelZoom, false)
			{
				myID = 81111,
				downNeighborID = 81115,
				rightNeighborID = 81112
			};
			this.windowedButton = new ClickableTextureComponent(new Rectangle(Game1.viewport.Width - 9 * Game1.pixelZoom - Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle((Game1.options != null && !Game1.options.isCurrentlyWindowed()) ? 155 : 146, 384, 9, 9), (float)Game1.pixelZoom, false)
			{
				myID = 81112,
				leftNeighborID = 81111,
				downNeighborID = 81113
			};
			this.startupPreferences = new StartupPreferences();
			this.startupPreferences.loadPreferences();
			this.applyPreferences();
			int timesPlayed = this.startupPreferences.timesPlayed;
			if (timesPlayed <= 30)
			{
				switch (timesPlayed)
				{
				case 2:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11717", new object[0]);
					break;
				case 3:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11718", new object[0]);
					break;
				case 4:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11719", new object[0]);
					break;
				case 5:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11720", new object[0]);
					break;
				case 6:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11721", new object[0]);
					break;
				case 7:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11722", new object[0]);
					break;
				case 8:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11723", new object[0]);
					break;
				case 9:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11724", new object[0]);
					break;
				case 10:
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11725", new object[0]);
					break;
				case 11:
				case 12:
				case 13:
				case 14:
				case 16:
				case 17:
				case 18:
				case 19:
					break;
				case 15:
					if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
					{
						string randomNoun = Dialogue.getRandomNoun();
						string randomNoun2 = Dialogue.getRandomNoun();
						this.startupMessage = string.Concat(new string[]
						{
							Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11726", new object[0]),
							Environment.NewLine,
							"The ",
							Dialogue.getRandomAdjective(),
							" ",
							randomNoun,
							" ",
							Dialogue.getRandomVerb(),
							" ",
							Dialogue.getRandomPositional(),
							" the ",
							randomNoun.Equals(randomNoun2) ? ("other " + randomNoun2) : randomNoun2
						});
					}
					else
					{
						int num = new Random().Next(1, 15);
						this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:RandomSentence." + num, new object[0]);
					}
					break;
				case 20:
					this.startupMessage = "<";
					break;
				default:
					if (timesPlayed == 30)
					{
						this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11731", new object[0]);
					}
					break;
				}
			}
			else if (timesPlayed != 100)
			{
				if (timesPlayed != 1000)
				{
					if (timesPlayed == 10000)
					{
						this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11734", new object[0]);
					}
				}
				else
				{
					this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11733", new object[0]);
				}
			}
			else
			{
				this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11732", new object[0]);
			}
			this.startupPreferences.savePreferences();
			Game1.setRichPresence("menus", null);
			if (Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		private bool alternativeTitleGraphic()
		{
			return Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh;
		}

		public void applyPreferences()
		{
			if (this.startupPreferences.startMuted)
			{
				if (Utility.toggleMuteMusic())
				{
					this.muteMusicButton.sourceRect.X = 137;
				}
				else
				{
					this.muteMusicButton.sourceRect.X = 128;
				}
			}
			if (this.startupPreferences.skipWindowPreparation && TitleMenu.windowNumber == 3)
			{
				TitleMenu.windowNumber = -1;
			}
			if (Game1.options.gamepadControls && Game1.options.snappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		private void OnLanguageChange(LocalizedContentManager.LanguageCode code)
		{
			this.titleButtonsTexture = Game1.content.Load<Texture2D>(Path.Combine("Minigames", "TitleButtons"));
			this.setUpIcons();
			this.startupPreferences.savePreferences();
			this.tempSprites.Clear();
		}

		public void skipToTitleButtons()
		{
			this.logoFadeTimer = 0;
			this.logoSwipeTimer = 0f;
			this.titleInPosition = false;
			this.pauseBeforeViewportRiseTimer = 0;
			this.fadeFromWhiteTimer = 0;
			this.viewportY = -999f;
			this.viewportDY = -0.01f;
			this.birds.Clear();
			this.logoSwipeTimer = 1f;
			this.chuckleFishTimer = 0;
			Game1.changeMusicTrack("MainTheme");
			if (Game1.options.SnappyMenus && Game1.options.gamepadControls)
			{
				this.snapToDefaultClickableComponent();
			}
		}

		public void setUpIcons()
		{
			this.buttons.Clear();
			int num = 74;
			int num2 = num * 3 * 3;
			num2 += 48;
			int num3 = this.width / 2 - num2 / 2;
			this.buttons.Add(new ClickableTextureComponent("New", new Rectangle(num3, this.height - 174 - 24, num * 3, 174), null, "", this.titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f, false)
			{
				myID = 81115,
				rightNeighborID = 81116,
				upNeighborID = 81111
			});
			num3 += (num + 8) * 3;
			this.buttons.Add(new ClickableTextureComponent("Load", new Rectangle(num3, this.height - 174 - 24, 222, 174), null, "", this.titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f, false)
			{
				myID = 81116,
				leftNeighborID = 81115,
				rightNeighborID = -7777,
				upNeighborID = 81111
			});
			num3 += (num + 8) * 3;
			this.buttons.Add(new ClickableTextureComponent("Exit", new Rectangle(num3, this.height - 174 - 24, 222, 174), null, "", this.titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f, false)
			{
				myID = 81117,
				leftNeighborID = 81116,
				rightNeighborID = 81118,
				upNeighborID = 81111
			});
			int num4 = (this.height < 800) ? 2 : 3;
			this.eRect = new Rectangle(this.width / 2 - 200 * num4 + 251 * num4, -300 * num4 - (int)(this.viewportY / 3f) * num4 + 26 * num4, 42 * num4, 68 * num4);
			this.populateLeafRects();
			this.backButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11739", new object[0]), new Rectangle(this.width + -198 - 48, this.height - 81 - 24, 198, 81), null, "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false)
			{
				myID = 81114
			};
			this.aboutButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11740", new object[0]), new Rectangle(this.width + -66 - 48, this.height - 75 - 24, 66, 75), null, "", this.titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f, false)
			{
				myID = 81113,
				upNeighborID = 81112,
				leftNeighborID = 81118
			};
			this.languageButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11740", new object[0]), new Rectangle(this.width + -132 - 96, this.height - 75 - 24, 81, 75), null, "", this.titleButtonsTexture, new Rectangle(52, 458, 27, 25), 3f, false)
			{
				myID = 81118,
				rightNeighborID = 81113,
				leftNeighborID = -7777,
				upNeighborID = 81112
			};
			this.skipButton = new ClickableComponent(new Rectangle(this.width / 2 - 261, this.height / 2 - 102, 249, 201), Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11741", new object[0]));
			if (Game1.options.gamepadControls && Game1.options.snappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID((this.startupPreferences != null && this.startupPreferences.timesPlayed > 0) ? 81116 : 81115);
			this.snapCursorToCurrentSnappedComponent();
		}

		protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
			if (oldID != 81116 || direction != 1)
			{
				if (oldID == 81118 && direction == 3)
				{
					if (base.getComponentWithID(81117) != null)
					{
						this.setCurrentlySnappedComponentTo(81117);
						this.snapCursorToCurrentSnappedComponent();
						return;
					}
					this.setCurrentlySnappedComponentTo(81116);
					this.snapCursorToCurrentSnappedComponent();
				}
				return;
			}
			if (base.getComponentWithID(81117) != null)
			{
				this.setCurrentlySnappedComponentTo(81117);
				this.snapCursorToCurrentSnappedComponent();
				return;
			}
			this.setCurrentlySnappedComponentTo(81118);
			this.snapCursorToCurrentSnappedComponent();
		}

		public void populateLeafRects()
		{
			int num = (this.height < 800) ? 2 : 3;
			this.leafRects = new List<Rectangle>();
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num - 196 * num, -300 * num - (int)(this.viewportY / 3f) * num + 26 * num + 109 * num, 17 * num, 30 * num));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num + 91 * num, -300 * num - (int)(this.viewportY / 3f) * num + 26 * num - 26 * num, 17 * num, 31 * num));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num + 79 * num, -300 * num - (int)(this.viewportY / 3f) * num + 26 * num + 83 * num, 25 * num, 17 * num));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num - 213 * num, -300 * num - (int)(this.viewportY / 3f) * num + 26 * num - 24 * num, 14 * num, 23 * num));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num - 234 * num, -300 * num - (int)(this.viewportY / 3f) * num + 26 * num - 11 * num, 18 * num, 12 * num));
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.receiveRightClick(x, y, true);
			}
		}

		public override bool readyToClose()
		{
			return false;
		}

		public override bool overrideSnappyMenuCursorMovementBan()
		{
			return !this.titleInPosition;
		}

		public override void leftClickHeld(int x, int y)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			base.leftClickHeld(x, y);
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.leftClickHeld(x, y);
			}
		}

		public override void releaseLeftClick(int x, int y)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			base.releaseLeftClick(x, y);
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.releaseLeftClick(x, y);
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			if (!Program.releaseBuild && key == Keys.N && Game1.oldKBState.IsKeyDown(Keys.RightShift) && Game1.oldKBState.IsKeyDown(Keys.LeftControl))
			{
				Game1.loadForNewGame(false);
				Game1.saveOnNewDay = false;
				Game1.player.eventsSeen.Add(60367);
				Game1.player.currentLocation = Utility.getHomeOfFarmer(Game1.player);
				Game1.player.position = new Vector2(7f, 9f) * (float)Game1.tileSize;
				Game1.player.FarmerSprite.setOwner(Game1.player);
				Game1.NewDay(0f);
				Game1.exitActiveMenu();
				Game1.setGameMode(3);
				return;
			}
			if (this.logoFadeTimer > 0 && (key == Keys.B || key == Keys.Escape))
			{
				this.bCount++;
				if (key == Keys.Escape)
				{
					this.bCount += 3;
				}
				if (this.bCount >= 3)
				{
					Game1.playSound("bigDeSelect");
					this.logoFadeTimer = 0;
					this.fadeFromWhiteTimer = 0;
					Game1.delayedActions.Clear();
					this.pauseBeforeViewportRiseTimer = 0;
					this.fadeFromWhiteTimer = 0;
					this.viewportY = -999f;
					this.viewportDY = -0.01f;
					this.birds.Clear();
					this.logoSwipeTimer = 1f;
					this.chuckleFishTimer = 0;
					Game1.changeMusicTrack("MainTheme");
				}
			}
			if (Game1.options.doesInputListContain(Game1.options.menuButton, key))
			{
				return;
			}
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.receiveKeyPress(key);
			}
			if (Game1.options.snappyMenus && Game1.options.gamepadControls && TitleMenu.subMenu == null)
			{
				base.receiveKeyPress(key);
			}
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			bool flag = true;
			if (TitleMenu.subMenu != null)
			{
				if (TitleMenu.subMenu is LoadGameMenu && (TitleMenu.subMenu as LoadGameMenu).deleteConfirmationScreen)
				{
					flag = false;
				}
				TitleMenu.subMenu.receiveGamePadButton(b);
			}
			if (flag && b == Buttons.B)
			{
				this.backButtonPressed();
			}
		}

		public override void gamePadButtonHeld(Buttons b)
		{
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.gamePadButtonHeld(b);
			}
		}

		public void backButtonPressed()
		{
			Game1.playSound("bigDeSelect");
			this.buttonsDX = -1;
			if (TitleMenu.subMenu is AboutMenu)
			{
				TitleMenu.subMenu = null;
				this.buttonsDX = 0;
				if (Game1.options.SnappyMenus)
				{
					this.setCurrentlySnappedComponentTo(81113);
					this.snapCursorToCurrentSnappedComponent();
				}
				return;
			}
			this.isTransitioningButtons = true;
			if (TitleMenu.subMenu is LoadGameMenu)
			{
				this.transitioningFromLoadScreen = true;
			}
			TitleMenu.subMenu = null;
			Game1.changeMusicTrack("spring_day_ambient");
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.HasActiveUser && this.muteMusicButton.containsPoint(x, y))
			{
				this.startupPreferences.startMuted = Utility.toggleMuteMusic();
				if (this.muteMusicButton.sourceRect.X == 128)
				{
					this.muteMusicButton.sourceRect.X = 137;
				}
				else
				{
					this.muteMusicButton.sourceRect.X = 128;
				}
				Game1.playSound("drumkit6");
				this.startupPreferences.savePreferences();
				return;
			}
			if (this.HasActiveUser && this.windowedButton.containsPoint(x, y))
			{
				if (!Game1.options.isCurrentlyWindowed())
				{
					Game1.options.setWindowedOption("Windowed");
					this.windowedButton.sourceRect.X = 146;
					this.startupPreferences.windowMode = 1;
				}
				else
				{
					Game1.options.setWindowedOption("Windowed Borderless");
					this.windowedButton.sourceRect.X = 155;
					this.startupPreferences.windowMode = 0;
				}
				this.startupPreferences.savePreferences();
				Game1.playSound("drumkit6");
				return;
			}
			if (this.logoFadeTimer > 0 && this.skipButton.containsPoint(x, y) && this.chuckleFishTimer <= 0)
			{
				if (this.logoSurprisedTimer <= 0)
				{
					this.logoSurprisedTimer = 1500;
					string cueName = "fishSlap";
					Game1.changeMusicTrack("none");
					int num = Game1.random.Next(2);
					if (num != 0)
					{
						if (num == 1)
						{
							cueName = "fishSlap";
						}
					}
					else
					{
						cueName = "Duck";
					}
					Game1.playSound(cueName);
				}
				else if (this.logoSurprisedTimer > 1)
				{
					this.logoSurprisedTimer = Math.Max(1, this.logoSurprisedTimer - 500);
				}
			}
			if (this.chuckleFishTimer > 500)
			{
				this.chuckleFishTimer = 500;
			}
			if (this.logoFadeTimer > 0 || this.fadeFromWhiteTimer > 0)
			{
				return;
			}
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			if (TitleMenu.subMenu != null)
			{
				if (!this.isTransitioningButtons)
				{
					TitleMenu.subMenu.receiveLeftClick(x, y, true);
				}
				if (TitleMenu.subMenu != null && (this.backButton.containsPoint(x, y) || TitleMenu.subMenu is TooManyFarmsMenu || (TitleMenu.subMenu is LanguageSelectionMenu && TitleMenu.subMenu.readyToClose())))
				{
					Game1.playSound("bigDeSelect");
					this.buttonsDX = -1;
					if (TitleMenu.subMenu is AboutMenu || TitleMenu.subMenu is LanguageSelectionMenu)
					{
						TitleMenu.subMenu = null;
						this.buttonsDX = 0;
						return;
					}
					this.isTransitioningButtons = true;
					if (TitleMenu.subMenu is LoadGameMenu)
					{
						this.transitioningFromLoadScreen = true;
					}
					TitleMenu.subMenu = null;
					Game1.changeMusicTrack("spring_day_ambient");
					return;
				}
			}
			else
			{
				if (this.logoFadeTimer <= 0 && !this.titleInPosition && this.logoSwipeTimer == 0f)
				{
					this.pauseBeforeViewportRiseTimer = 0;
					this.fadeFromWhiteTimer = 0;
					this.viewportY = -999f;
					this.viewportDY = -0.01f;
					this.birds.Clear();
					this.logoSwipeTimer = 1f;
					return;
				}
				if (!this.alternativeTitleGraphic())
				{
					if (this.clicksOnLeaf >= 10 && Game1.random.NextDouble() < 0.001)
					{
						Game1.playSound("junimoMeep1");
					}
					if (this.titleInPosition && this.eRect.Contains(x, y) && this.clicksOnE < 10)
					{
						this.clicksOnE++;
						Game1.playSound("woodyStep");
						if (this.clicksOnE == 10)
						{
							int num2 = (this.height < 800) ? 2 : 3;
							Game1.playSound("openChest");
							this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(0, 491, 42, 68), new Vector2((float)(this.width / 2 - 200 * num2 + 251 * num2), (float)(-300 * num2 - (int)(this.viewportY / 3f) * num2 + 26 * num2)), false, 0f, Color.White)
							{
								scale = (float)num2,
								animationLength = 9,
								interval = 200f,
								local = true,
								holdLastFrame = true
							});
						}
					}
					else if (this.titleInPosition)
					{
						bool flag = false;
						foreach (Rectangle current in this.leafRects)
						{
							if (current.Contains(x, y))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							this.clicksOnLeaf++;
							if (this.clicksOnLeaf == 10)
							{
								int num3 = (this.height < 800) ? 2 : 3;
								Game1.playSound("discoverMineral");
								this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(264, 464, 16, 16), new Vector2((float)(this.width / 2 - 200 * num3 + 80 * num3), (float)(-300 * num3 - (int)(this.viewportY / 3f) * num3 + 10 * num3 + 2)), false, 0f, Color.White)
								{
									scale = (float)num3,
									animationLength = 8,
									interval = 80f,
									totalNumberOfLoops = 999999,
									local = true,
									holdLastFrame = false,
									delayBeforeAnimationStart = 200
								});
								this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float)(this.width / 2 - 200 * num3 + 80 * num3), (float)(-300 * num3 - (int)(this.viewportY / 3f) * num3 + 10 * num3)), false, 0f, Color.White)
								{
									scale = (float)num3,
									animationLength = 8,
									interval = 50f,
									local = true,
									holdLastFrame = false
								});
								this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(200, 464, 16, 16), new Vector2((float)(this.width / 2 - 200 * num3 + 178 * num3), (float)(-300 * num3 - (int)(this.viewportY / 3f) * num3 + 141 * num3 + 2)), false, 0f, Color.White)
								{
									scale = (float)num3,
									animationLength = 4,
									interval = 150f,
									totalNumberOfLoops = 999999,
									local = true,
									holdLastFrame = false,
									delayBeforeAnimationStart = 400
								});
								this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float)(this.width / 2 - 200 * num3 + 178 * num3), (float)(-300 * num3 - (int)(this.viewportY / 3f) * num3 + 141 * num3)), false, 0f, Color.White)
								{
									scale = (float)num3,
									animationLength = 8,
									interval = 50f,
									local = true,
									holdLastFrame = false,
									delayBeforeAnimationStart = 200
								});
								this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 464, 16, 16), new Vector2((float)(this.width / 2 - 200 * num3 + 294 * num3), (float)(-300 * num3 - (int)(this.viewportY / 3f) * num3 + 89 * num3 + 2)), false, 0f, Color.White)
								{
									scale = (float)num3,
									animationLength = 4,
									interval = 150f,
									totalNumberOfLoops = 999999,
									local = true,
									holdLastFrame = false,
									delayBeforeAnimationStart = 600
								});
								this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float)(this.width / 2 - 200 * num3 + 294 * num3), (float)(-300 * num3 - (int)(this.viewportY / 3f) * num3 + 89 * num3)), false, 0f, Color.White)
								{
									scale = (float)num3,
									animationLength = 8,
									interval = 50f,
									local = true,
									holdLastFrame = false,
									delayBeforeAnimationStart = 400
								});
							}
							else
							{
								Game1.playSound("leafrustle");
								int num4 = (this.height < 800) ? 2 : 3;
								for (int i = 0; i < 2; i++)
								{
									this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(355, 1199 + Game1.random.Next(-1, 2) * 16, 16, 16), new Vector2((float)(x + Game1.random.Next(-8, 9)), (float)(y + Game1.random.Next(-8, 9))), Game1.random.NextDouble() < 0.5, 0f, Color.White)
									{
										scale = (float)num4,
										animationLength = 11,
										interval = (float)(50 + Game1.random.Next(50)),
										totalNumberOfLoops = 999,
										motion = new Vector2((float)Game1.random.Next(-100, 101) / 100f, 1f + (float)Game1.random.Next(-100, 100) / 500f),
										xPeriodic = (Game1.random.NextDouble() < 0.5),
										xPeriodicLoopTime = (float)Game1.random.Next(6000, 16000),
										xPeriodicRange = (float)Game1.random.Next(Game1.tileSize, Game1.tileSize * 3),
										alphaFade = 0.001f,
										local = true,
										holdLastFrame = false,
										delayBeforeAnimationStart = i * 20
									});
								}
							}
						}
					}
				}
				if (!this.HasActiveUser)
				{
					return;
				}
				if ((TitleMenu.subMenu == null || TitleMenu.subMenu.readyToClose()) && !this.isTransitioningButtons)
				{
					foreach (ClickableTextureComponent current2 in this.buttons)
					{
						if (current2.containsPoint(x, y))
						{
							this.performButtonAction(current2.name);
						}
					}
					if (this.aboutButton.containsPoint(x, y))
					{
						TitleMenu.subMenu = new AboutMenu();
						Game1.playSound("newArtifact");
					}
					if (this.languageButton.visible && this.languageButton.containsPoint(x, y))
					{
						TitleMenu.subMenu = new LanguageSelectionMenu();
						Game1.playSound("newArtifact");
					}
				}
			}
		}

		public void performButtonAction(string which)
		{
			this.whichSubMenu = which;
			if (which == "New")
			{
				this.buttonsDX = 1;
				this.isTransitioningButtons = true;
				Game1.playSound("select");
				lock (this)
				{
					this.numFarmsSaved = -1;
				}
				Game1.GetNumFarmsSavedAsync(delegate(int num)
				{
					lock (this)
					{
						this.numFarmsSaved = num;
					}
				});
				return;
			}
			if (!(which == "Load"))
			{
				if (!(which == "Co-op"))
				{
					if (!(which == "Exit"))
					{
						return;
					}
					Game1.playSound("bigDeSelect");
					Game1.changeMusicTrack("none");
					this.quitTimer = 500;
				}
				return;
			}
			this.buttonsDX = 1;
			this.isTransitioningButtons = true;
			Game1.playSound("select");
		}

		private void addRightLeafGust()
		{
			if (this.isTransitioningButtons || this.tempSprites.Count<TemporaryAnimatedSprite>() > 0 || this.alternativeTitleGraphic())
			{
				return;
			}
			int num = (this.height < 800) ? 2 : 3;
			this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 187, 27, 21), new Vector2((float)(this.width / 2 - 200 * num + 327 * num), (float)(-300 * num) - this.viewportY / 3f * (float)num + (float)(107 * num)), false, 0f, Color.White)
			{
				scale = (float)num,
				pingPong = true,
				animationLength = 3,
				interval = 100f,
				totalNumberOfLoops = 3,
				local = true
			});
		}

		private void addLeftLeafGust()
		{
			if (this.isTransitioningButtons || this.tempSprites.Count<TemporaryAnimatedSprite>() > 0 || this.alternativeTitleGraphic())
			{
				return;
			}
			int num = (this.height < 800) ? 2 : 3;
			this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 208, 22, 18), new Vector2((float)(this.width / 2 - 200 * num + 16 * num), (float)(-300 * num) - this.viewportY / 3f * (float)num + (float)(16 * num)), false, 0f, Color.White)
			{
				scale = (float)num,
				pingPong = true,
				animationLength = 3,
				interval = 100f,
				totalNumberOfLoops = 3,
				local = true
			});
		}

		public void createdNewCharacter(bool skipIntro)
		{
			Game1.playSound("smallSelect");
			TitleMenu.subMenu = null;
			this.transitioningCharacterCreationMenu = true;
			if (skipIntro)
			{
				Game1.loadForNewGame(false);
				Game1.saveOnNewDay = true;
				Game1.player.eventsSeen.Add(60367);
				Game1.player.currentLocation = Utility.getHomeOfFarmer(Game1.player);
				Game1.player.position = new Vector2(7f, 9f) * (float)Game1.tileSize;
				Game1.NewDay(0f);
				Game1.exitActiveMenu();
				Game1.setGameMode(3);
			}
		}

		public override void update(GameTime time)
		{
			if (TitleMenu.windowNumber > ((this.startupPreferences.windowMode == 1) ? 0 : 1))
			{
				if (TitleMenu.windowNumber % 2 == 0)
				{
					Game1.options.setWindowedOption("Windowed Borderless");
				}
				else
				{
					Game1.options.setWindowedOption("Windowed");
				}
				TitleMenu.windowNumber--;
				if (TitleMenu.windowNumber == ((this.startupPreferences.windowMode == 1) ? 0 : 1))
				{
					Game1.options.setWindowedOption(this.startupPreferences.windowMode);
				}
			}
			base.update(time);
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.update(time);
			}
			if (this.transitioningCharacterCreationMenu)
			{
				this.globalCloudAlpha -= (float)time.ElapsedGameTime.Milliseconds * 0.001f;
				if (this.globalCloudAlpha <= 0f)
				{
					this.transitioningCharacterCreationMenu = false;
					this.globalCloudAlpha = 0f;
					TitleMenu.subMenu = null;
					Game1.currentMinigame = new GrandpaStory();
					Game1.exitActiveMenu();
					Game1.setGameMode(3);
				}
			}
			if (this.quitTimer > 0)
			{
				this.quitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.quitTimer <= 0)
				{
					Game1.quit = true;
					Game1.exitActiveMenu();
				}
			}
			if (this.chuckleFishTimer > 0)
			{
				this.chuckleFishTimer -= time.ElapsedGameTime.Milliseconds;
			}
			else if (this.logoFadeTimer > 0)
			{
				if (this.logoSurprisedTimer > 0)
				{
					this.logoSurprisedTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.logoSurprisedTimer <= 0)
					{
						this.logoFadeTimer = 1;
					}
				}
				else
				{
					int num = this.logoFadeTimer;
					this.logoFadeTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.logoFadeTimer < 4000 & num >= 4000)
					{
						Game1.playSound("mouseClick");
					}
					if (this.logoFadeTimer < 2500 & num >= 2500)
					{
						Game1.playSound("mouseClick");
					}
					if (this.logoFadeTimer < 2000 & num >= 2000)
					{
						Game1.playSound("mouseClick");
					}
					if (this.logoFadeTimer <= 0)
					{
						Game1.changeMusicTrack("MainTheme");
					}
				}
			}
			else if (this.fadeFromWhiteTimer > 0)
			{
				this.fadeFromWhiteTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.fadeFromWhiteTimer <= 0)
				{
					this.pauseBeforeViewportRiseTimer = 3500;
				}
			}
			else if (this.pauseBeforeViewportRiseTimer > 0)
			{
				this.pauseBeforeViewportRiseTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.pauseBeforeViewportRiseTimer <= 0)
				{
					this.viewportDY = -0.05f;
				}
			}
			this.viewportY += this.viewportDY;
			if (this.viewportDY < 0f)
			{
				this.viewportDY -= 0.006f;
			}
			if (this.viewportY <= -1000f)
			{
				if (this.viewportDY != 0f)
				{
					this.logoSwipeTimer = 1000f;
					this.showButtonsTimer = 250;
				}
				this.viewportDY = 0f;
			}
			if (this.logoSwipeTimer > 0f)
			{
				this.logoSwipeTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.logoSwipeTimer <= 0f)
				{
					this.addLeftLeafGust();
					this.addRightLeafGust();
					this.titleInPosition = true;
					int num2 = (this.height < 800) ? 2 : 3;
					this.eRect = new Rectangle(this.width / 2 - 200 * num2 + 251 * num2, -300 * num2 - (int)(this.viewportY / 3f) * num2 + 26 * num2, 42 * num2, 68 * num2);
					this.populateLeafRects();
				}
			}
			if (this.showButtonsTimer > 0 && this.HasActiveUser)
			{
				this.showButtonsTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.showButtonsTimer <= 0 && this.buttonsToShow < 3)
				{
					this.buttonsToShow++;
					Game1.playSound("Cowboy_gunshot");
					this.showButtonsTimer = 250;
				}
			}
			if (this.titleInPosition && !this.isTransitioningButtons && this.globalXOffset == 0 && Game1.random.NextDouble() < 0.005)
			{
				if (Game1.random.NextDouble() < 0.5)
				{
					this.addLeftLeafGust();
				}
				else
				{
					this.addRightLeafGust();
				}
			}
			if (this.titleInPosition && this.isTransitioningButtons)
			{
				int num3 = this.buttonsDX * (int)time.ElapsedGameTime.TotalMilliseconds;
				int num4 = this.globalXOffset + num3;
				int num5 = num4 - this.width;
				if (num5 > 0)
				{
					num4 -= num5;
					num3 -= num5;
				}
				this.globalXOffset = num4;
				this.moveFeatures(num3, 0);
				if (this.buttonsDX > 0 && this.globalXOffset >= this.width)
				{
					if (TitleMenu.subMenu != null)
					{
						if (TitleMenu.subMenu.readyToClose())
						{
							this.isTransitioningButtons = false;
							this.buttonsDX = 0;
						}
					}
					else if (this.whichSubMenu.Equals("Load"))
					{
						TitleMenu.subMenu = new LoadGameMenu();
						Game1.changeMusicTrack("title_night");
						this.buttonsDX = 0;
						this.isTransitioningButtons = false;
					}
					else if (this.whichSubMenu.Equals("New") && this.numFarmsSaved != -1)
					{
						if (this.numFarmsSaved >= Game1.GetMaxNumFarmsSaved())
						{
							TitleMenu.subMenu = new TooManyFarmsMenu();
							Game1.playSound("newArtifact");
							this.buttonsDX = 0;
							this.isTransitioningButtons = false;
						}
						else
						{
							Game1.resetPlayer();
							TitleMenu.subMenu = new CharacterCustomization(new List<int>
							{
								0,
								1,
								2,
								3,
								4,
								5
							}, new List<int>
							{
								0,
								1,
								2,
								3,
								4,
								5
							}, new List<int>
							{
								0,
								1,
								2,
								3,
								4,
								5
							}, false);
							Game1.playSound("select");
							Game1.changeMusicTrack("CloudCountry");
							Game1.player.favoriteThing = "";
							this.buttonsDX = 0;
							this.isTransitioningButtons = false;
						}
					}
					if (!this.isTransitioningButtons)
					{
						this.whichSubMenu = "";
					}
				}
				else if (this.buttonsDX < 0 && this.globalXOffset <= 0)
				{
					this.globalXOffset = 0;
					this.isTransitioningButtons = false;
					this.buttonsDX = 0;
					this.setUpIcons();
					this.whichSubMenu = "";
					this.transitioningFromLoadScreen = false;
				}
			}
			for (int i = this.bigClouds.Count - 1; i >= 0; i--)
			{
				List<float> list = this.bigClouds;
				int index = i;
				list[index] -= 0.1f;
				list = this.bigClouds;
				index = i;
				list[index] += (float)(this.buttonsDX * time.ElapsedGameTime.Milliseconds / 2);
				if (this.bigClouds[i] < -1536f)
				{
					this.bigClouds[i] = (float)this.width;
				}
			}
			for (int j = this.smallClouds.Count - 1; j >= 0; j--)
			{
				List<float> list = this.smallClouds;
				int index = j;
				list[index] -= 0.3f;
				list = this.smallClouds;
				index = j;
				list[index] += (float)(this.buttonsDX * time.ElapsedGameTime.Milliseconds / 2);
				if (this.smallClouds[j] < -384f)
				{
					this.smallClouds[j] = (float)this.width;
				}
			}
			for (int k = this.tempSprites.Count - 1; k >= 0; k--)
			{
				if (this.tempSprites[k].update(time))
				{
					this.tempSprites.RemoveAt(k);
				}
			}
			for (int l = this.birds.Count - 1; l >= 0; l--)
			{
				TemporaryAnimatedSprite expr_88C_cp_0_cp_0 = this.birds[l];
				expr_88C_cp_0_cp_0.position.Y = expr_88C_cp_0_cp_0.position.Y - this.viewportDY * 2f;
				if (this.birds[l].update(time))
				{
					this.birds.RemoveAt(l);
				}
			}
		}

		private void moveFeatures(int dx, int dy)
		{
			foreach (TemporaryAnimatedSprite expr_15 in this.tempSprites)
			{
				expr_15.position.X = expr_15.position.X + (float)dx;
				expr_15.position.Y = expr_15.position.Y + (float)dy;
			}
			foreach (ClickableTextureComponent expr_64 in this.buttons)
			{
				expr_64.bounds.X = expr_64.bounds.X + dx;
				expr_64.bounds.Y = expr_64.bounds.Y + dy;
			}
		}

		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.receiveScrollWheelAction(direction);
			}
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.muteMusicButton.tryHover(x, y, 0.1f);
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.performHoverAction(x, y);
				if (this.backButton.containsPoint(x, y))
				{
					if (this.backButton.sourceRect.Y == 252)
					{
						Game1.playSound("Cowboy_Footstep");
					}
					this.backButton.sourceRect.Y = 279;
				}
				else
				{
					this.backButton.sourceRect.Y = 252;
				}
				this.backButton.tryHover(x, y, 0.25f);
				return;
			}
			if (this.titleInPosition && this.HasActiveUser)
			{
				foreach (ClickableTextureComponent current in this.buttons)
				{
					if (current.containsPoint(x, y))
					{
						if (current.sourceRect.Y == 187)
						{
							Game1.playSound("Cowboy_Footstep");
						}
						current.sourceRect.Y = 245;
					}
					else
					{
						current.sourceRect.Y = 187;
					}
					current.tryHover(x, y, 0.25f);
				}
				this.aboutButton.tryHover(x, y, 0.25f);
				if (this.aboutButton.containsPoint(x, y))
				{
					if (this.aboutButton.sourceRect.X == 8)
					{
						Game1.playSound("Cowboy_Footstep");
					}
					this.aboutButton.sourceRect.X = 30;
				}
				else
				{
					this.aboutButton.sourceRect.X = 8;
				}
				if (this.languageButton.visible)
				{
					this.languageButton.tryHover(x, y, 0.25f);
					if (this.languageButton.containsPoint(x, y))
					{
						if (this.languageButton.sourceRect.X == 52)
						{
							Game1.playSound("Cowboy_Footstep");
						}
						this.languageButton.sourceRect.X = 79;
						return;
					}
					this.languageButton.sourceRect.X = 52;
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), new Color(64, 136, 248));
			b.Draw(Game1.mouseCursors, new Rectangle(0, (int)(-900f - this.viewportY * 0.66f), this.width, 900 + this.height - 360), new Rectangle?(new Rectangle(703, 1912, 1, 264)), Color.White);
			if (!this.whichSubMenu.Equals("Load"))
			{
				b.Draw(Game1.mouseCursors, new Vector2(-30f, -1080f - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * (1f - (float)this.globalXOffset / 1200f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
			}
			foreach (float current in this.bigClouds)
			{
				b.Draw(this.cloudsTexture, new Vector2(current, (float)(this.height - 750) - this.viewportY * 0.5f), new Rectangle?(new Rectangle(0, 0, 512, 337)), Color.White * this.globalCloudAlpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
			}
			b.Draw(Game1.mouseCursors, new Vector2(-90f, (float)(this.height - 474) - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 886, 639, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.08f);
			b.Draw(Game1.mouseCursors, new Vector2(1827f, (float)(this.height - 474) - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 886, 640, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.08f);
			for (int i = 0; i < this.smallClouds.Count; i++)
			{
				b.Draw(this.cloudsTexture, new Vector2(this.smallClouds[i], (float)(this.height - 900 - i * 16 * 3) - this.viewportY * 0.5f), new Rectangle?((i % 2 == 0) ? new Rectangle(152, 447, 123, 55) : new Rectangle(410, 467, 63, 37)), Color.White * this.globalCloudAlpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
			}
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.height - 444) - this.viewportY * 1f), new Rectangle?(new Rectangle(0, 737, 639, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.1f);
			b.Draw(Game1.mouseCursors, new Vector2(1917f, (float)(this.height - 444) - this.viewportY * 1f), new Rectangle?(new Rectangle(0, 737, 640, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.1f);
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = this.birds.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.draw(b, false, 0, 0);
				}
			}
			b.Draw(this.cloudsTexture, new Vector2(0f, (float)(this.height - 426) - this.viewportY * 2f), new Rectangle?(new Rectangle(0, 554, 165, 142)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
			b.Draw(this.cloudsTexture, new Vector2((float)(this.width - 366), (float)(this.height - 459) - this.viewportY * 2f), new Rectangle?(new Rectangle(390, 543, 122, 153)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
			int num = (this.height < 800) ? 2 : 3;
			if (this.whichSubMenu.Equals("Load") || (TitleMenu.subMenu != null && TitleMenu.subMenu is LoadGameMenu) || this.transitioningFromLoadScreen)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, this.width, this.height), new Rectangle?(new Rectangle(639, 858, 1, 100)), Color.White * ((float)this.globalXOffset / 1200f));
				b.Draw(Game1.mouseCursors, Vector2.Zero, new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * ((float)this.globalXOffset / 1200f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(195 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * ((float)this.globalXOffset / 1200f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 0.8f);
			}
			b.Draw(this.titleButtonsTexture, new Vector2((float)(this.globalXOffset + this.width / 2 - 200 * num), (float)(-300 * num) - this.viewportY / 3f * (float)num), new Rectangle?(new Rectangle(0, 0, 400, 187)), Color.White, 0f, Vector2.Zero, (float)num, SpriteEffects.None, 0.2f);
			if (this.logoSwipeTimer > 0f)
			{
				b.Draw(this.titleButtonsTexture, new Vector2((float)(this.globalXOffset + this.width / 2), (float)(-300 * num) - this.viewportY / 3f * (float)num + (float)(93 * num)), new Rectangle?(new Rectangle(0, 0, 400, 187)), Color.White, 0f, new Vector2(200f, 93f), (float)num + (0.5f - Math.Abs(this.logoSwipeTimer / 1000f - 0.5f)) * 0.1f, SpriteEffects.None, 0.2f);
			}
			if (!this.HasActiveUser && this.titleInPosition)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\UI:TitleMenu_PressAToStart", new object[0]), Game1.viewport.Width / 2, Game1.viewport.Height / 2 + 270, "", 1f, -1, 0, 0.88f, false);
				b.Draw(Game1.controllerMaps, new Rectangle(Game1.viewport.Width / 2 - 64, Game1.viewport.Height / 2 + 273, 52, 52), new Rectangle?(new Rectangle(542, 260, 26, 26)), Color.White);
			}
			if (TitleMenu.subMenu != null && !this.isTransitioningButtons)
			{
				this.backButton.draw(b);
				TitleMenu.subMenu.draw(b);
				if (!(TitleMenu.subMenu is CharacterCustomization))
				{
					this.backButton.draw(b);
				}
			}
			else if (TitleMenu.subMenu == null && this.isTransitioningButtons && (this.whichSubMenu.Equals("Load") || this.whichSubMenu.Equals("New")))
			{
				int x = Game1.tileSize + 20;
				int y = Game1.viewport.Height - Game1.tileSize;
				int width = 0;
				int tileSize = Game1.tileSize;
				Utility.makeSafe(ref x, ref y, width, tileSize);
				SpriteText.drawStringWithScrollBackground(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3689", new object[0]), x, y, "", 1f, -1);
			}
			else if (TitleMenu.subMenu == null && !this.isTransitioningButtons && this.titleInPosition && !this.transitioningCharacterCreationMenu && this.HasActiveUser)
			{
				this.aboutButton.draw(b);
				this.languageButton.draw(b);
			}
			for (int j = 0; j < this.buttonsToShow; j++)
			{
				if (this.buttons.Count > j)
				{
					this.buttons[j].draw(b, (TitleMenu.subMenu == null || (!(TitleMenu.subMenu is AboutMenu) && !(TitleMenu.subMenu is LanguageSelectionMenu))) ? Color.White : (Color.LightGray * 0.1f), 1f);
				}
			}
			if (TitleMenu.subMenu == null)
			{
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = this.tempSprites.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b, false, 0, 0);
					}
				}
			}
			if (this.chuckleFishTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.White);
				b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2 - 66 * Game1.pixelZoom), (float)(this.height / 2 - 48 * Game1.pixelZoom)), new Rectangle?(new Rectangle(this.chuckleFishTimer % 200 / 100 * 132, 559, 132, 96)), Color.White * Math.Min(1f, (float)this.chuckleFishTimer / 500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.2f);
			}
			else if (this.logoFadeTimer > 0 || this.fadeFromWhiteTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.White * ((float)this.fadeFromWhiteTimer / 2000f));
				b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2), (float)(this.height / 2 - 90)), new Rectangle?(new Rectangle(171 + ((this.logoFadeTimer / 100 % 2 == 0 && this.logoSurprisedTimer <= 0) ? 111 : 0), 311, 111, 60)), Color.White * ((this.logoFadeTimer < 500) ? ((float)this.logoFadeTimer / 500f) : ((this.logoFadeTimer > 4500) ? (1f - (float)(this.logoFadeTimer - 4500) / 500f) : 1f)), 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
				if (this.logoSurprisedTimer <= 0)
				{
					b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2 - 261), (float)(this.height / 2 - 102)), new Rectangle?(new Rectangle((this.logoFadeTimer / 100 % 2 == 0) ? 85 : 0, 306 + (this.shades ? 69 : 0), 85, 69)), Color.White * ((this.logoFadeTimer < 500) ? ((float)this.logoFadeTimer / 500f) : ((this.logoFadeTimer > 4500) ? (1f - (float)(this.logoFadeTimer - 4500) / 500f) : 1f)), 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
				}
				if (this.logoSurprisedTimer > 0)
				{
					b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2 - 261), (float)(this.height / 2 - 102)), new Rectangle?(new Rectangle((this.logoSurprisedTimer > 800 || this.logoSurprisedTimer < 400) ? 176 : 260, 375, 85, 69)), Color.White * ((this.logoSurprisedTimer < 200) ? ((float)this.logoSurprisedTimer / 200f) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.22f);
				}
				if (this.startupMessage.Length > 0 && this.logoFadeTimer > 0)
				{
					b.DrawString(Game1.smallFont, Game1.parseText(this.startupMessage, Game1.smallFont, Game1.tileSize * 10), new Vector2((float)(Game1.pixelZoom * 2), (float)Game1.viewport.Height - Game1.smallFont.MeasureString(Game1.parseText(this.startupMessage, Game1.smallFont, Game1.tileSize * 10)).Y - (float)Game1.pixelZoom), this.startupMessageColor * ((this.logoFadeTimer < 500) ? ((float)this.logoFadeTimer / 500f) : ((this.logoFadeTimer > 4500) ? (1f - (float)(this.logoFadeTimer - 4500) / 500f) : 1f)));
				}
			}
			if (this.logoFadeTimer > 0)
			{
				int arg_E18_0 = this.logoSurprisedTimer;
			}
			if (this.quitTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.Black * (1f - (float)this.quitTimer / 500f));
			}
			if (this.HasActiveUser)
			{
				this.muteMusicButton.draw(b);
				this.windowedButton.draw(b);
			}
			base.drawMouse(b);
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.width = Game1.viewport.Width;
			this.height = Game1.viewport.Height;
			if (!this.isTransitioningButtons && TitleMenu.subMenu == null)
			{
				this.setUpIcons();
			}
			if (TitleMenu.subMenu != null)
			{
				TitleMenu.subMenu.gameWindowSizeChanged(oldBounds, newBounds);
			}
			this.backButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11739", new object[0]), new Rectangle(this.width + -198 - 48, this.height - 81 - 24, 198, 81), null, "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false)
			{
				myID = 81114
			};
			this.tempSprites.Clear();
			if (this.birds.Count > 0 && !this.titleInPosition)
			{
				for (int i = 0; i < this.birds.Count; i++)
				{
					this.birds[i].position = ((i % 2 == 0) ? new Vector2((float)(this.width - 210), (float)(this.height - 360)) : new Vector2((float)(this.width - 120), (float)(this.height - 330)));
				}
			}
			this.windowedButton = new ClickableTextureComponent(new Rectangle(Game1.viewport.Width - 9 * Game1.pixelZoom - Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle((Game1.options != null && !Game1.options.isCurrentlyWindowed()) ? 155 : 146, 384, 9, 9), (float)Game1.pixelZoom, false)
			{
				myID = 81112,
				leftNeighborID = 81111,
				downNeighborID = 81113
			};
			if (Game1.options.SnappyMenus)
			{
				int id = (this.currentlySnappedComponent != null) ? this.currentlySnappedComponent.myID : 81115;
				base.populateClickableComponentList();
				this.currentlySnappedComponent = base.getComponentWithID(id);
				this.snapCursorToCurrentSnappedComponent();
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					if (this.tempSprites != null)
					{
						this.tempSprites.Clear();
					}
					if (this.menuContent != null)
					{
						this.menuContent.Dispose();
						this.menuContent = null;
					}
					TitleMenu.subMenu = null;
				}
				this.disposedValue = true;
			}
		}

		~TitleMenu()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
