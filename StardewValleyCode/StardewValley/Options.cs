using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StardewValley
{
	public class Options
	{
		public const float minZoom = 0.75f;

		public const float maxZoom = 1.25f;

		public const int toggleAutoRun = 0;

		public const int musicVolume = 1;

		public const int soundVolume = 2;

		public const int toggleDialogueTypingSounds = 3;

		public const int toggleFullscreen = 4;

		public const int toggleWindowedOrTrueFullscreen = 5;

		public const int screenResolution = 6;

		public const int showPortraitsToggle = 7;

		public const int showMerchantPortraitsToggle = 8;

		public const int menuBG = 9;

		public const int toggleFootsteps = 10;

		public const int alwaysShowToolHitLocationToggle = 11;

		public const int hideToolHitLocationWhenInMotionToggle = 12;

		public const int windowMode = 13;

		public const int pauseWhenUnfocused = 14;

		public const int pinToolbar = 15;

		public const int toggleRumble = 16;

		public const int ambientOnly = 17;

		public const int zoom = 18;

		public const int zoomButtonsToggle = 19;

		public const int ambientVolume = 20;

		public const int footstepVolume = 21;

		public const int invertScrollDirectionToggle = 22;

		public const int snowTransparencyToggle = 23;

		public const int screenFlashToggle = 24;

		public const int lightingQualityToggle = 25;

		public const int toggleHardwareCursor = 26;

		public const int toggleShowPlacementTileGamepad = 27;

		public const int toggleSnappyMenus = 29;

		public const int input_actionButton = 7;

		public const int input_toolSwapButton = 8;

		public const int input_cancelButton = 9;

		public const int input_useToolButton = 10;

		public const int input_moveUpButton = 11;

		public const int input_moveRightButton = 12;

		public const int input_moveDownButton = 13;

		public const int input_moveLeftButton = 14;

		public const int input_menuButton = 15;

		public const int input_runButton = 16;

		public const int input_chatButton = 17;

		public const int input_journalButton = 18;

		public const int input_mapButton = 19;

		public const int input_slot1 = 20;

		public const int input_slot2 = 21;

		public const int input_slot3 = 22;

		public const int input_slot4 = 23;

		public const int input_slot5 = 24;

		public const int input_slot6 = 25;

		public const int input_slot7 = 26;

		public const int input_slot8 = 27;

		public const int input_slot9 = 28;

		public const int input_slot10 = 29;

		public const int input_slot11 = 30;

		public const int input_slot12 = 31;

		public const int checkBoxOption = 1;

		public const int sliderOption = 2;

		public const int dropDownOption = 3;

		public const float defaultZoomLevel = 1f;

		public const int defaultLightingQuality = 32;

		public bool autoRun;

		public bool dialogueTyping;

		public bool fullscreen;

		public bool windowedBorderlessFullscreen;

		public bool showPortraits;

		public bool showMerchantPortraits;

		public bool showMenuBackground;

		public bool playFootstepSounds;

		public bool alwaysShowToolHitLocation;

		public bool hideToolHitLocationWhenInMotion;

		public bool pauseWhenOutOfFocus;

		public bool pinToolbarToggle;

		public bool mouseControls;

		public bool keyboardControls;

		public bool gamepadControls;

		public bool rumble;

		public bool ambientOnlyToggle;

		public bool zoomButtons;

		public bool invertScrollDirection;

		public bool screenFlash;

		public bool hardwareCursor;

		public bool showPlacementTileForGamepad;

		public bool snappyMenus;

		public float musicVolumeLevel;

		public float soundVolumeLevel;

		public float zoomLevel;

		public float footstepVolumeLevel;

		public float ambientVolumeLevel;

		public float snowTransparency;

		public int preferredResolutionX;

		public int preferredResolutionY;

		public int lightingQuality;

		public InputButton[] actionButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.X),
			new InputButton(false)
		};

		public InputButton[] toolSwapButton = new InputButton[0];

		public InputButton[] cancelButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.V)
		};

		public InputButton[] useToolButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.C),
			new InputButton(true)
		};

		public InputButton[] moveUpButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.W)
		};

		public InputButton[] moveRightButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D)
		};

		public InputButton[] moveDownButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.S)
		};

		public InputButton[] moveLeftButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.A)
		};

		public InputButton[] menuButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.E),
			new InputButton(Microsoft.Xna.Framework.Input.Keys.Escape)
		};

		public InputButton[] runButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.LeftShift)
		};

		public InputButton[] tmpKeyToReplace = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.None)
		};

		public InputButton[] chatButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.T),
			new InputButton(Microsoft.Xna.Framework.Input.Keys.OemQuestion)
		};

		public InputButton[] mapButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.M)
		};

		public InputButton[] journalButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.F)
		};

		public InputButton[] inventorySlot1 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D1)
		};

		public InputButton[] inventorySlot2 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D2)
		};

		public InputButton[] inventorySlot3 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D3)
		};

		public InputButton[] inventorySlot4 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D4)
		};

		public InputButton[] inventorySlot5 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D5)
		};

		public InputButton[] inventorySlot6 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D6)
		};

		public InputButton[] inventorySlot7 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D7)
		};

		public InputButton[] inventorySlot8 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D8)
		};

		public InputButton[] inventorySlot9 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D9)
		};

		public InputButton[] inventorySlot10 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D0)
		};

		public InputButton[] inventorySlot11 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.OemMinus)
		};

		public InputButton[] inventorySlot12 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.OemPlus)
		};

		private float appliedZoomLevel = 1f;

		private int appliedLightingQuality = 32;

		public bool SnappyMenus
		{
			get
			{
				return this.snappyMenus && this.gamepadControls && Mouse.GetState().LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed && Mouse.GetState().RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed;
			}
		}

		public Options()
		{
			this.setToDefaults();
		}

		public Microsoft.Xna.Framework.Input.Keys getFirstKeyboardKeyFromInputButtonList(InputButton[] inputButton)
		{
			for (int i = 0; i < inputButton.Length; i++)
			{
				if (inputButton[i].key != Microsoft.Xna.Framework.Input.Keys.None)
				{
					return inputButton[i].key;
				}
			}
			return Microsoft.Xna.Framework.Input.Keys.None;
		}

		public void reApplySetOptions()
		{
			if (this.zoomLevel != this.appliedZoomLevel || this.lightingQuality != this.appliedLightingQuality)
			{
				Program.gamePtr.refreshWindowSettings();
				this.appliedZoomLevel = this.zoomLevel;
				this.appliedLightingQuality = this.lightingQuality;
			}
			Program.gamePtr.IsMouseVisible = this.hardwareCursor;
		}

		public void setToDefaults()
		{
			this.playFootstepSounds = true;
			this.showMenuBackground = false;
			this.showMerchantPortraits = true;
			this.showPortraits = true;
			this.autoRun = true;
			this.alwaysShowToolHitLocation = false;
			this.hideToolHitLocationWhenInMotion = true;
			this.dialogueTyping = true;
			this.rumble = true;
			this.fullscreen = false;
			this.pinToolbarToggle = false;
			this.zoomLevel = 1f;
			this.zoomButtons = false;
			this.pauseWhenOutOfFocus = true;
			this.screenFlash = true;
			this.snowTransparency = 1f;
			this.invertScrollDirection = false;
			this.ambientOnlyToggle = false;
			this.windowedBorderlessFullscreen = true;
			this.showPlacementTileForGamepad = true;
			this.lightingQuality = 32;
			this.hardwareCursor = false;
			this.musicVolumeLevel = 0.75f;
			this.ambientVolumeLevel = 0.75f;
			this.footstepVolumeLevel = 0.9f;
			this.soundVolumeLevel = 1f;
			this.preferredResolutionX = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last<DisplayMode>().Width;
			this.preferredResolutionY = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last<DisplayMode>().Height;
			this.snappyMenus = true;
		}

		public void setControlsToDefault()
		{
			this.actionButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.X),
				new InputButton(false)
			};
			this.toolSwapButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.Z)
			};
			this.cancelButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.V)
			};
			this.useToolButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.C),
				new InputButton(true)
			};
			this.moveUpButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.W)
			};
			this.moveRightButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D)
			};
			this.moveDownButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.S)
			};
			this.moveLeftButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.A)
			};
			this.menuButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.E),
				new InputButton(Microsoft.Xna.Framework.Input.Keys.Escape)
			};
			this.runButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.LeftShift)
			};
			this.tmpKeyToReplace = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.None)
			};
			this.chatButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.T),
				new InputButton(Microsoft.Xna.Framework.Input.Keys.OemQuestion)
			};
			this.mapButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.M)
			};
			this.journalButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.F)
			};
			this.inventorySlot1 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D1)
			};
			this.inventorySlot2 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D2)
			};
			this.inventorySlot3 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D3)
			};
			this.inventorySlot4 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D4)
			};
			this.inventorySlot5 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D5)
			};
			this.inventorySlot6 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D6)
			};
			this.inventorySlot7 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D7)
			};
			this.inventorySlot8 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D8)
			};
			this.inventorySlot9 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D9)
			};
			this.inventorySlot10 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D0)
			};
			this.inventorySlot11 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.OemMinus)
			};
			this.inventorySlot12 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.OemPlus)
			};
		}

		public string getNameOfOptionFromIndex(int index)
		{
			switch (index)
			{
			case 0:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4556", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4557", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4558", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4559", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4560", new object[0]);
			case 5:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4561", new object[0]);
			case 6:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4562", new object[0]);
			default:
				return "";
			}
		}

		public int whatTypeOfOption(int index)
		{
			if (index == 1 || index == 2)
			{
				return 2;
			}
			if (index != 6)
			{
				return 1;
			}
			return 3;
		}

		public void changeCheckBoxOption(int which, bool value)
		{
			switch (which)
			{
			case 0:
				this.autoRun = value;
				Game1.player.setRunning(this.autoRun, false);
				return;
			case 1:
			case 2:
			case 4:
			case 5:
			case 6:
			case 13:
			case 18:
			case 20:
			case 21:
			case 23:
			case 25:
			case 28:
				break;
			case 3:
				this.dialogueTyping = value;
				return;
			case 7:
				this.showPortraits = value;
				return;
			case 8:
				this.showMerchantPortraits = value;
				return;
			case 9:
				this.showMenuBackground = value;
				return;
			case 10:
				this.playFootstepSounds = value;
				return;
			case 11:
				this.alwaysShowToolHitLocation = value;
				return;
			case 12:
				this.hideToolHitLocationWhenInMotion = value;
				return;
			case 14:
				this.pauseWhenOutOfFocus = value;
				return;
			case 15:
				this.pinToolbarToggle = value;
				return;
			case 16:
				this.rumble = value;
				return;
			case 17:
				this.ambientOnlyToggle = value;
				return;
			case 19:
				this.zoomButtons = value;
				return;
			case 22:
				this.invertScrollDirection = value;
				return;
			case 24:
				this.screenFlash = value;
				return;
			case 26:
				this.hardwareCursor = value;
				Program.gamePtr.IsMouseVisible = this.hardwareCursor;
				return;
			case 27:
				this.showPlacementTileForGamepad = value;
				return;
			case 29:
				this.snappyMenus = value;
				break;
			default:
				return;
			}
		}

		public void changeSliderOption(int which, int value)
		{
			if (which == 1)
			{
				this.musicVolumeLevel = (float)value / 100f;
				Game1.musicCategory.SetVolume(this.musicVolumeLevel);
				return;
			}
			if (which != 2)
			{
				switch (which)
				{
				case 18:
				{
					int num = (int)(this.zoomLevel * 100f);
					int num2 = num;
					int num3 = (int)((float)value * 100f);
					if (num3 >= num + 10 || num3 >= 100)
					{
						num += 10;
						num = Math.Min(100, num);
					}
					else if (num3 <= num - 10 || num3 <= 50)
					{
						num -= 10;
						num = Math.Max(50, num);
					}
					if (num != num2)
					{
						this.zoomLevel = (float)num / 100f;
						Game1.overrideGameMenuReset = true;
						Program.gamePtr.refreshWindowSettings();
						Game1.overrideGameMenuReset = false;
						Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4563", new object[0]) + this.zoomLevel);
					}
					break;
				}
				case 19:
				case 22:
					break;
				case 20:
					this.ambientVolumeLevel = (float)value / 100f;
					Game1.ambientCategory.SetVolume(this.ambientVolumeLevel);
					return;
				case 21:
					this.footstepVolumeLevel = (float)value / 100f;
					Game1.footstepCategory.SetVolume(this.footstepVolumeLevel);
					return;
				case 23:
					this.snowTransparency = (float)value / 100f;
					return;
				default:
					return;
				}
				return;
			}
			this.soundVolumeLevel = (float)value / 100f;
			Game1.soundCategory.SetVolume(this.soundVolumeLevel);
		}

		public void setWindowedOption(string setting)
		{
			if (setting == "Windowed")
			{
				this.setWindowedOption(1);
				return;
			}
			if (setting == "Fullscreen")
			{
				this.setWindowedOption(2);
				return;
			}
			if (!(setting == "Windowed Borderless"))
			{
				return;
			}
			this.setWindowedOption(0);
		}

		public void setWindowedOption(int setting)
		{
			this.windowedBorderlessFullscreen = this.isCurrentlyWindowedBorderless();
			this.fullscreen = (!this.windowedBorderlessFullscreen && Game1.graphics.IsFullScreen);
			int num = -1;
			switch (setting)
			{
			case 0:
				if (!this.windowedBorderlessFullscreen)
				{
					this.windowedBorderlessFullscreen = true;
					Game1.toggleFullscreen();
					this.fullscreen = false;
				}
				num = 0;
				break;
			case 1:
				if (Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen)
				{
					Game1.toggleNonBorderlessWindowedFullscreen();
					this.fullscreen = false;
					this.windowedBorderlessFullscreen = false;
				}
				else if (this.windowedBorderlessFullscreen)
				{
					this.fullscreen = false;
					this.windowedBorderlessFullscreen = false;
					Game1.toggleFullscreen();
				}
				num = 1;
				break;
			case 2:
				if (this.windowedBorderlessFullscreen)
				{
					this.fullscreen = true;
					this.windowedBorderlessFullscreen = false;
					Game1.toggleFullscreen();
				}
				else if (!Game1.graphics.IsFullScreen)
				{
					Game1.toggleNonBorderlessWindowedFullscreen();
					this.fullscreen = true;
					this.windowedBorderlessFullscreen = false;
					this.hardwareCursor = false;
					Program.gamePtr.IsMouseVisible = false;
				}
				num = 2;
				break;
			}
			if (Game1.gameMode == 3)
			{
				Game1.exitActiveMenu();
				Game1.activeClickableMenu = new GameMenu(6, 6);
			}
			try
			{
				StartupPreferences expr_116 = new StartupPreferences();
				expr_116.loadPreferences();
				expr_116.windowMode = num;
				expr_116.savePreferences();
			}
			catch (Exception)
			{
			}
		}

		public void changeDropDownOption(int which, int selection, List<string> options)
		{
			if (which <= 13)
			{
				if (which == 6)
				{
					Rectangle oldBounds = new Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height);
					string text = options[selection];
					int num = Convert.ToInt32(text.Split(new char[]
					{
						' '
					})[0]);
					int num2 = Convert.ToInt32(text.Split(new char[]
					{
						' '
					})[2]);
					this.preferredResolutionX = num;
					this.preferredResolutionY = num2;
					Game1.graphics.PreferredBackBufferWidth = num;
					Game1.graphics.PreferredBackBufferHeight = num2;
					Game1.graphics.ApplyChanges();
					Game1.updateViewportForScreenSizeChange(true, num, num2);
					using (List<IClickableMenu>.Enumerator enumerator = Game1.onScreenMenus.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.gameWindowSizeChanged(oldBounds, new Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
						}
					}
					if (Game1.currentMinigame != null)
					{
						Game1.currentMinigame.changeScreenSize();
					}
					Game1.exitActiveMenu();
					Game1.activeClickableMenu = new GameMenu(6, 6);
					return;
				}
				if (which != 13)
				{
					return;
				}
				this.setWindowedOption(options[selection]);
			}
			else if (which != 18)
			{
				if (which == 25)
				{
					string a = options[selection];
					if (!(a == "Lowest"))
					{
						if (!(a == "Low"))
						{
							if (!(a == "Med."))
							{
								if (!(a == "High"))
								{
									if (a == "Ultra")
									{
										this.lightingQuality = 8;
									}
								}
								else
								{
									this.lightingQuality = 16;
								}
							}
							else
							{
								this.lightingQuality = 32;
							}
						}
						else
						{
							this.lightingQuality = 64;
						}
					}
					else
					{
						this.lightingQuality = 128;
					}
					int currentlySnappedComponentTo = (Game1.activeClickableMenu.getCurrentlySnappedComponent() != null) ? Game1.activeClickableMenu.getCurrentlySnappedComponent().myID : -1;
					Game1.overrideGameMenuReset = true;
					Program.gamePtr.refreshWindowSettings();
					Game1.overrideGameMenuReset = false;
					Game1.activeClickableMenu = new GameMenu(6, 19);
					if (this.snappyMenus)
					{
						Game1.activeClickableMenu.setCurrentlySnappedComponentTo(currentlySnappedComponentTo);
						return;
					}
				}
			}
			else
			{
				int num3 = Convert.ToInt32(options[selection].Replace("%", ""));
				this.zoomLevel = (float)num3 / 100f;
				Game1.overrideGameMenuReset = true;
				Program.gamePtr.refreshWindowSettings();
				Game1.overrideGameMenuReset = false;
				Game1.activeClickableMenu = new GameMenu(6, 14);
				if (Game1.debrisWeather != null)
				{
					Game1.randomizeDebrisWeatherPositions(Game1.debrisWeather);
					return;
				}
			}
		}

		public bool isKeyInUse(Microsoft.Xna.Framework.Input.Keys key)
		{
			using (List<InputButton>.Enumerator enumerator = this.getAllUsedInputButtons().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.key == key)
					{
						return true;
					}
				}
			}
			return false;
		}

		public List<InputButton> getAllUsedInputButtons()
		{
			List<InputButton> expr_05 = new List<InputButton>();
			expr_05.AddRange(this.useToolButton);
			expr_05.AddRange(this.actionButton);
			expr_05.AddRange(this.moveUpButton);
			expr_05.AddRange(this.moveRightButton);
			expr_05.AddRange(this.moveDownButton);
			expr_05.AddRange(this.moveLeftButton);
			expr_05.AddRange(this.runButton);
			expr_05.AddRange(this.menuButton);
			expr_05.AddRange(this.journalButton);
			expr_05.AddRange(this.mapButton);
			expr_05.AddRange(this.toolSwapButton);
			expr_05.AddRange(this.chatButton);
			expr_05.AddRange(this.inventorySlot1);
			expr_05.AddRange(this.inventorySlot2);
			expr_05.AddRange(this.inventorySlot3);
			expr_05.AddRange(this.inventorySlot4);
			expr_05.AddRange(this.inventorySlot5);
			expr_05.AddRange(this.inventorySlot6);
			expr_05.AddRange(this.inventorySlot7);
			expr_05.AddRange(this.inventorySlot8);
			expr_05.AddRange(this.inventorySlot9);
			expr_05.AddRange(this.inventorySlot10);
			expr_05.AddRange(this.inventorySlot11);
			expr_05.AddRange(this.inventorySlot12);
			return expr_05;
		}

		public void setCheckBoxToProperValue(OptionsCheckbox checkbox)
		{
			switch (checkbox.whichOption)
			{
			case 0:
				checkbox.isChecked = this.autoRun;
				return;
			case 1:
			case 2:
			case 6:
			case 13:
			case 18:
			case 20:
			case 21:
			case 23:
			case 25:
			case 28:
				break;
			case 3:
				checkbox.isChecked = this.dialogueTyping;
				return;
			case 4:
			{
				Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
				this.windowedBorderlessFullscreen = (form.FormBorderStyle == FormBorderStyle.None);
				this.fullscreen = (Game1.graphics.IsFullScreen || this.windowedBorderlessFullscreen);
				checkbox.isChecked = this.fullscreen;
				return;
			}
			case 5:
				checkbox.isChecked = this.windowedBorderlessFullscreen;
				checkbox.greyedOut = !this.fullscreen;
				return;
			case 7:
				checkbox.isChecked = this.showPortraits;
				return;
			case 8:
				checkbox.isChecked = this.showMerchantPortraits;
				return;
			case 9:
				checkbox.isChecked = this.showMenuBackground;
				return;
			case 10:
				checkbox.isChecked = this.playFootstepSounds;
				return;
			case 11:
				checkbox.isChecked = this.alwaysShowToolHitLocation;
				return;
			case 12:
				checkbox.isChecked = this.hideToolHitLocationWhenInMotion;
				return;
			case 14:
				checkbox.isChecked = this.pauseWhenOutOfFocus;
				return;
			case 15:
				checkbox.isChecked = this.pinToolbarToggle;
				return;
			case 16:
				checkbox.isChecked = this.rumble;
				checkbox.greyedOut = !this.gamepadControls;
				return;
			case 17:
				checkbox.isChecked = this.ambientOnlyToggle;
				return;
			case 19:
				checkbox.isChecked = this.zoomButtons;
				return;
			case 22:
				checkbox.isChecked = this.invertScrollDirection;
				return;
			case 24:
				checkbox.isChecked = this.screenFlash;
				return;
			case 26:
				checkbox.isChecked = this.hardwareCursor;
				checkbox.greyedOut = this.fullscreen;
				return;
			case 27:
				checkbox.isChecked = this.showPlacementTileForGamepad;
				checkbox.greyedOut = !this.gamepadControls;
				return;
			case 29:
				checkbox.isChecked = this.snappyMenus;
				break;
			default:
				return;
			}
		}

		public void setPlusMinusToProperValue(OptionsPlusMinus plusMinus)
		{
			int whichOption = plusMinus.whichOption;
			if (whichOption == 18)
			{
				string value = Math.Round((double)(this.zoomLevel * 100f)) + "%";
				for (int i = 0; i < plusMinus.options.Count; i++)
				{
					if (plusMinus.options[i].Equals(value))
					{
						plusMinus.selected = i;
						return;
					}
				}
				return;
			}
			if (whichOption != 25)
			{
				return;
			}
			string value2 = "";
			whichOption = this.lightingQuality;
			if (whichOption <= 16)
			{
				if (whichOption != 8)
				{
					if (whichOption == 16)
					{
						value2 = "High";
					}
				}
				else
				{
					value2 = "Ultra";
				}
			}
			else if (whichOption != 32)
			{
				if (whichOption != 64)
				{
					if (whichOption == 128)
					{
						value2 = "Lowest";
					}
				}
				else
				{
					value2 = "Low";
				}
			}
			else
			{
				value2 = "Med.";
			}
			for (int j = 0; j < plusMinus.options.Count; j++)
			{
				if (plusMinus.options[j].Equals(value2))
				{
					plusMinus.selected = j;
					return;
				}
			}
		}

		public void setSliderToProperValue(OptionsSlider slider)
		{
			int whichOption = slider.whichOption;
			if (whichOption == 1)
			{
				slider.value = (int)(this.musicVolumeLevel * 100f);
				return;
			}
			if (whichOption != 2)
			{
				switch (whichOption)
				{
				case 18:
					slider.value = (int)(this.zoomLevel * 100f);
					break;
				case 19:
				case 22:
					break;
				case 20:
					slider.value = (int)(this.ambientVolumeLevel * 100f);
					return;
				case 21:
					slider.value = (int)(this.footstepVolumeLevel * 100f);
					return;
				case 23:
					slider.value = (int)(this.snowTransparency * 100f);
					return;
				default:
					return;
				}
				return;
			}
			slider.value = (int)(this.soundVolumeLevel * 100f);
		}

		public bool doesInputListContain(InputButton[] list, Microsoft.Xna.Framework.Input.Keys key)
		{
			for (int i = 0; i < list.Length; i++)
			{
				if (list[i].key == key)
				{
					return true;
				}
			}
			return false;
		}

		public void changeInputListenerValue(int whichListener, Microsoft.Xna.Framework.Input.Keys key)
		{
			switch (whichListener)
			{
			case 7:
				this.actionButton[0] = new InputButton(key);
				return;
			case 8:
				this.toolSwapButton[0] = new InputButton(key);
				return;
			case 9:
				break;
			case 10:
				this.useToolButton[0] = new InputButton(key);
				return;
			case 11:
				this.moveUpButton[0] = new InputButton(key);
				return;
			case 12:
				this.moveRightButton[0] = new InputButton(key);
				return;
			case 13:
				this.moveDownButton[0] = new InputButton(key);
				return;
			case 14:
				this.moveLeftButton[0] = new InputButton(key);
				return;
			case 15:
				this.menuButton[0] = new InputButton(key);
				return;
			case 16:
				this.runButton[0] = new InputButton(key);
				return;
			case 17:
				this.chatButton[0] = new InputButton(key);
				return;
			case 18:
				this.journalButton[0] = new InputButton(key);
				return;
			case 19:
				this.mapButton[0] = new InputButton(key);
				return;
			case 20:
				this.inventorySlot1[0] = new InputButton(key);
				return;
			case 21:
				this.inventorySlot2[0] = new InputButton(key);
				return;
			case 22:
				this.inventorySlot3[0] = new InputButton(key);
				return;
			case 23:
				this.inventorySlot4[0] = new InputButton(key);
				return;
			case 24:
				this.inventorySlot5[0] = new InputButton(key);
				return;
			case 25:
				this.inventorySlot6[0] = new InputButton(key);
				return;
			case 26:
				this.inventorySlot7[0] = new InputButton(key);
				return;
			case 27:
				this.inventorySlot8[0] = new InputButton(key);
				return;
			case 28:
				this.inventorySlot9[0] = new InputButton(key);
				return;
			case 29:
				this.inventorySlot10[0] = new InputButton(key);
				return;
			case 30:
				this.inventorySlot11[0] = new InputButton(key);
				return;
			case 31:
				this.inventorySlot12[0] = new InputButton(key);
				break;
			default:
				return;
			}
		}

		public void setInputListenerToProperValue(OptionsInputListener inputListener)
		{
			inputListener.buttonNames.Clear();
			switch (inputListener.whichOption)
			{
			case 7:
			{
				InputButton[] array = this.actionButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton = array[i];
					inputListener.buttonNames.Add(inputButton.ToString());
				}
				return;
			}
			case 8:
			{
				InputButton[] array = this.toolSwapButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton2 = array[i];
					inputListener.buttonNames.Add(inputButton2.ToString());
				}
				return;
			}
			case 9:
				break;
			case 10:
			{
				InputButton[] array = this.useToolButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton3 = array[i];
					inputListener.buttonNames.Add(inputButton3.ToString());
				}
				return;
			}
			case 11:
			{
				InputButton[] array = this.moveUpButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton4 = array[i];
					inputListener.buttonNames.Add(inputButton4.ToString());
				}
				return;
			}
			case 12:
			{
				InputButton[] array = this.moveRightButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton5 = array[i];
					inputListener.buttonNames.Add(inputButton5.ToString());
				}
				return;
			}
			case 13:
			{
				InputButton[] array = this.moveDownButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton6 = array[i];
					inputListener.buttonNames.Add(inputButton6.ToString());
				}
				return;
			}
			case 14:
			{
				InputButton[] array = this.moveLeftButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton7 = array[i];
					inputListener.buttonNames.Add(inputButton7.ToString());
				}
				return;
			}
			case 15:
			{
				InputButton[] array = this.menuButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton8 = array[i];
					inputListener.buttonNames.Add(inputButton8.ToString());
				}
				return;
			}
			case 16:
			{
				InputButton[] array = this.runButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton9 = array[i];
					inputListener.buttonNames.Add(inputButton9.ToString());
				}
				return;
			}
			case 17:
			{
				InputButton[] array = this.chatButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton10 = array[i];
					inputListener.buttonNames.Add(inputButton10.ToString());
				}
				return;
			}
			case 18:
			{
				InputButton[] array = this.journalButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton11 = array[i];
					inputListener.buttonNames.Add(inputButton11.ToString());
				}
				return;
			}
			case 19:
			{
				InputButton[] array = this.mapButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton12 = array[i];
					inputListener.buttonNames.Add(inputButton12.ToString());
				}
				return;
			}
			case 20:
			{
				InputButton[] array = this.inventorySlot1;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton13 = array[i];
					inputListener.buttonNames.Add(inputButton13.ToString());
				}
				return;
			}
			case 21:
			{
				InputButton[] array = this.inventorySlot2;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton14 = array[i];
					inputListener.buttonNames.Add(inputButton14.ToString());
				}
				return;
			}
			case 22:
			{
				InputButton[] array = this.inventorySlot3;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton15 = array[i];
					inputListener.buttonNames.Add(inputButton15.ToString());
				}
				return;
			}
			case 23:
			{
				InputButton[] array = this.inventorySlot4;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton16 = array[i];
					inputListener.buttonNames.Add(inputButton16.ToString());
				}
				return;
			}
			case 24:
			{
				InputButton[] array = this.inventorySlot5;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton17 = array[i];
					inputListener.buttonNames.Add(inputButton17.ToString());
				}
				return;
			}
			case 25:
			{
				InputButton[] array = this.inventorySlot6;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton18 = array[i];
					inputListener.buttonNames.Add(inputButton18.ToString());
				}
				return;
			}
			case 26:
			{
				InputButton[] array = this.inventorySlot7;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton19 = array[i];
					inputListener.buttonNames.Add(inputButton19.ToString());
				}
				return;
			}
			case 27:
			{
				InputButton[] array = this.inventorySlot8;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton20 = array[i];
					inputListener.buttonNames.Add(inputButton20.ToString());
				}
				return;
			}
			case 28:
			{
				InputButton[] array = this.inventorySlot9;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton21 = array[i];
					inputListener.buttonNames.Add(inputButton21.ToString());
				}
				return;
			}
			case 29:
			{
				InputButton[] array = this.inventorySlot10;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton22 = array[i];
					inputListener.buttonNames.Add(inputButton22.ToString());
				}
				return;
			}
			case 30:
			{
				InputButton[] array = this.inventorySlot11;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton23 = array[i];
					inputListener.buttonNames.Add(inputButton23.ToString());
				}
				return;
			}
			case 31:
			{
				InputButton[] array = this.inventorySlot12;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton inputButton24 = array[i];
					inputListener.buttonNames.Add(inputButton24.ToString());
				}
				break;
			}
			default:
				return;
			}
		}

		public void setDropDownToProperValue(OptionsDropDown dropDown)
		{
			int whichOption = dropDown.whichOption;
			if (whichOption == 6)
			{
				int num = 0;
				foreach (DisplayMode current in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
				{
					if (current.Width >= 1280)
					{
						dropDown.dropDownOptions.Add(current.Width + " x " + current.Height);
						dropDown.dropDownDisplayOptions.Add(current.Width + " x " + current.Height);
						if (current.Width == this.preferredResolutionX && current.Height == this.preferredResolutionY)
						{
							dropDown.selectedOption = num;
						}
						num++;
					}
				}
				dropDown.greyedOut = (!this.fullscreen || this.windowedBorderlessFullscreen);
				return;
			}
			if (whichOption != 13)
			{
				return;
			}
			this.windowedBorderlessFullscreen = this.isCurrentlyWindowedBorderless();
			this.fullscreen = (Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen);
			dropDown.dropDownOptions.Add("Windowed");
			if (!this.windowedBorderlessFullscreen)
			{
				dropDown.dropDownOptions.Add("Fullscreen");
			}
			if (!this.fullscreen)
			{
				dropDown.dropDownOptions.Add("Windowed Borderless");
			}
			dropDown.dropDownDisplayOptions.Add(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4564", new object[0]));
			if (!this.windowedBorderlessFullscreen)
			{
				dropDown.dropDownDisplayOptions.Add(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4560", new object[0]));
			}
			if (!this.fullscreen)
			{
				dropDown.dropDownDisplayOptions.Add(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4561", new object[0]));
			}
			if (Game1.graphics.IsFullScreen || this.windowedBorderlessFullscreen)
			{
				dropDown.selectedOption = 1;
				return;
			}
			dropDown.selectedOption = 0;
		}

		public bool isCurrentlyWindowedBorderless()
		{
			Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
			return !Game1.graphics.IsFullScreen && form.FormBorderStyle == FormBorderStyle.None;
		}

		public bool isCurrentlyFullscreen()
		{
			return Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen;
		}

		public bool isCurrentlyWindowed()
		{
			return !this.isCurrentlyWindowedBorderless() && !this.isCurrentlyFullscreen();
		}
	}
}
