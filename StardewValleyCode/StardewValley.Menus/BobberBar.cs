using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class BobberBar : IClickableMenu
	{
		public const int timePerFishSizeReduction = 800;

		public const int bobberTrackHeight = 548;

		public const int bobberBarTrackHeight = 568;

		public const int xOffsetToBobberTrack = 64;

		public const int yOffsetToBobberTrack = 12;

		public const int mixed = 0;

		public const int dart = 1;

		public const int smooth = 2;

		public const int sink = 3;

		public const int floater = 4;

		private float difficulty;

		private int motionType;

		private int whichFish;

		private float bobberPosition = 548f;

		private float bobberSpeed;

		private float bobberAcceleration;

		private float bobberTargetPosition;

		private float scale;

		private float everythingShakeTimer;

		private float floaterSinkerAcceleration;

		private float treasurePosition;

		private float treasureCatchLevel;

		private float treasureAppearTimer;

		private float treasureScale;

		private bool bobberInBar;

		private bool buttonPressed;

		private bool flipBubble;

		private bool fadeIn;

		private bool fadeOut;

		private bool treasure;

		private bool treasureCaught;

		private bool perfect;

		private bool bossFish;

		private int bobberBarHeight;

		private int fishSize;

		private int fishQuality;

		private int minFishSize;

		private int maxFishSize;

		private int fishSizeReductionTimer;

		private int whichBobber;

		private Vector2 barShake;

		private Vector2 fishShake;

		private Vector2 everythingShake;

		private Vector2 treasureShake;

		private float reelRotation;

		private SparklingText sparkleText;

		private float bobberBarPos;

		private float bobberBarSpeed;

		private float bobberBarAcceleration;

		private float distanceFromCatching = 0.3f;

		public static Cue reelSound;

		public static Cue unReelSound;

		public BobberBar(int whichFish, float fishSize, bool treasure, int bobber) : base(0, 0, 96, 636, false)
		{
			this.treasure = treasure;
			this.treasureAppearTimer = (float)Game1.random.Next(1000, 3000);
			this.fadeIn = true;
			this.scale = 0f;
			this.whichFish = whichFish;
			Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
			this.bobberBarHeight = Game1.tileSize * 3 / 2 + Game1.player.FishingLevel * 8;
			this.bossFish = FishingRod.isFishBossFish(whichFish);
			if (Game1.player.fishCaught != null && Game1.player.fishCaught.Count == 0)
			{
				this.distanceFromCatching = 0.1f;
			}
			if (dictionary.ContainsKey(whichFish))
			{
				string[] array = dictionary[whichFish].Split(new char[]
				{
					'/'
				});
				this.difficulty = (float)Convert.ToInt32(array[1]);
				string a = array[2].ToLower();
				if (!(a == "mixed"))
				{
					if (!(a == "dart"))
					{
						if (!(a == "smooth"))
						{
							if (!(a == "floater"))
							{
								if (a == "sinker")
								{
									this.motionType = 3;
								}
							}
							else
							{
								this.motionType = 4;
							}
						}
						else
						{
							this.motionType = 2;
						}
					}
					else
					{
						this.motionType = 1;
					}
				}
				else
				{
					this.motionType = 0;
				}
				this.minFishSize = Convert.ToInt32(array[3]);
				this.maxFishSize = Convert.ToInt32(array[4]);
				this.fishSize = (int)((float)this.minFishSize + (float)(this.maxFishSize - this.minFishSize) * fishSize);
				this.fishSize++;
				this.perfect = true;
				this.fishQuality = (((double)fishSize < 0.33) ? 0 : (((double)fishSize < 0.66) ? 1 : 2));
				this.fishSizeReductionTimer = 800;
			}
			switch (Game1.player.FacingDirection)
			{
			case 0:
				this.xPositionOnScreen = (int)Game1.player.position.X - Game1.tileSize - 132;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				break;
			case 1:
				this.xPositionOnScreen = (int)Game1.player.position.X - Game1.tileSize - 132;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				break;
			case 2:
				this.xPositionOnScreen = (int)Game1.player.position.X - Game1.tileSize - 132;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				break;
			case 3:
				this.xPositionOnScreen = (int)Game1.player.position.X + Game1.tileSize * 2;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				this.flipBubble = true;
				break;
			}
			this.xPositionOnScreen -= Game1.viewport.X;
			this.yPositionOnScreen -= Game1.viewport.Y + Game1.tileSize;
			if (this.xPositionOnScreen + 96 > Game1.viewport.Width)
			{
				this.xPositionOnScreen = Game1.viewport.Width - 96;
			}
			else if (this.xPositionOnScreen < 0)
			{
				this.xPositionOnScreen = 0;
			}
			if (this.yPositionOnScreen < 0)
			{
				this.yPositionOnScreen = 0;
			}
			else if (this.yPositionOnScreen + 636 > Game1.viewport.Height)
			{
				this.yPositionOnScreen = Game1.viewport.Height - 636;
			}
			if (bobber == 695)
			{
				this.bobberBarHeight += 24;
			}
			this.bobberBarPos = (float)(568 - this.bobberBarHeight);
			this.bobberPosition = 508f;
			this.bobberTargetPosition = (100f - this.difficulty) / 100f * 548f;
			if (Game1.soundBank != null)
			{
				BobberBar.reelSound = Game1.soundBank.GetCue("fastReel");
			}
			this.whichBobber = bobber;
			Game1.setRichPresence("fishing", Game1.currentLocation.Name);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
		}

		public override void update(GameTime time)
		{
			if (this.sparkleText != null && this.sparkleText.update(time))
			{
				this.sparkleText = null;
			}
			if (this.everythingShakeTimer > 0f)
			{
				this.everythingShakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
				this.everythingShake = new Vector2((float)Game1.random.Next(-10, 11) / 10f, (float)Game1.random.Next(-10, 11) / 10f);
				if (this.everythingShakeTimer <= 0f)
				{
					this.everythingShake = Vector2.Zero;
				}
			}
			if (this.fadeIn)
			{
				this.scale += 0.05f;
				if (this.scale >= 1f)
				{
					this.scale = 1f;
					this.fadeIn = false;
				}
			}
			else if (this.fadeOut)
			{
				if (this.everythingShakeTimer > 0f || this.sparkleText != null)
				{
					return;
				}
				this.scale -= 0.05f;
				if (this.scale <= 0f)
				{
					this.scale = 0f;
					this.fadeOut = false;
					if (this.distanceFromCatching > 0.9f && Game1.player.CurrentTool is FishingRod)
					{
						(Game1.player.CurrentTool as FishingRod).pullFishFromWater(this.whichFish, this.fishSize, this.fishQuality, (int)this.difficulty, this.treasureCaught, this.perfect);
					}
					else
					{
						if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod)
						{
							(Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, true);
						}
						Game1.player.completelyStopAnimatingOrDoingAction();
					}
					Game1.exitActiveMenu();
					Game1.setRichPresence("location", Game1.currentLocation.Name);
				}
			}
			else
			{
				if (Game1.random.NextDouble() < (double)(this.difficulty * (float)((this.motionType == 2) ? 20 : 1) / 4000f) && (this.motionType != 2 || this.bobberTargetPosition == -1f))
				{
					float num = 548f - this.bobberPosition;
					float num2 = this.bobberPosition;
					float num3 = Math.Min(99f, this.difficulty + (float)Game1.random.Next(10, 45)) / 100f;
					this.bobberTargetPosition = this.bobberPosition + (float)Game1.random.Next(-(int)num2, (int)num) * num3;
				}
				if (this.motionType == 4)
				{
					this.floaterSinkerAcceleration = Math.Max(this.floaterSinkerAcceleration - 0.01f, -1.5f);
				}
				else if (this.motionType == 3)
				{
					this.floaterSinkerAcceleration = Math.Min(this.floaterSinkerAcceleration + 0.01f, 1.5f);
				}
				if (Math.Abs(this.bobberPosition - this.bobberTargetPosition) > 3f && this.bobberTargetPosition != -1f)
				{
					this.bobberAcceleration = (this.bobberTargetPosition - this.bobberPosition) / ((float)Game1.random.Next(10, 30) + (100f - Math.Min(100f, this.difficulty)));
					this.bobberSpeed += (this.bobberAcceleration - this.bobberSpeed) / 5f;
				}
				else if (this.motionType != 2 && Game1.random.NextDouble() < (double)(this.difficulty / 2000f))
				{
					this.bobberTargetPosition = this.bobberPosition + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-100, -51) : Game1.random.Next(50, 101));
				}
				else
				{
					this.bobberTargetPosition = -1f;
				}
				if (this.motionType == 1 && Game1.random.NextDouble() < (double)(this.difficulty / 1000f))
				{
					this.bobberTargetPosition = this.bobberPosition + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-100 - (int)this.difficulty * 2, -51) : Game1.random.Next(50, 101 + (int)this.difficulty * 2));
				}
				this.bobberTargetPosition = Math.Max(-1f, Math.Min(this.bobberTargetPosition, 548f));
				this.bobberPosition += this.bobberSpeed + this.floaterSinkerAcceleration;
				if (this.bobberPosition > 532f)
				{
					this.bobberPosition = 532f;
				}
				else if (this.bobberPosition < 0f)
				{
					this.bobberPosition = 0f;
				}
				this.bobberInBar = (this.bobberPosition + 16f <= this.bobberBarPos - 32f + (float)this.bobberBarHeight && this.bobberPosition - 16f >= this.bobberBarPos - 32f);
				if (this.bobberPosition >= (float)(548 - this.bobberBarHeight) && this.bobberBarPos >= (float)(568 - this.bobberBarHeight - 4))
				{
					this.bobberInBar = true;
				}
				bool arg_580_0 = this.buttonPressed;
				this.buttonPressed = (Game1.oldMouseState.LeftButton == ButtonState.Pressed || Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton) || (Game1.options.gamepadControls && (Game1.oldPadState.IsButtonDown(Buttons.X) || Game1.oldPadState.IsButtonDown(Buttons.A))));
				if (!arg_580_0 && this.buttonPressed)
				{
					Game1.playSound("fishingRodBend");
				}
				float num4 = this.buttonPressed ? -0.25f : 0.25f;
				if (this.buttonPressed && num4 < 0f && (this.bobberBarPos == 0f || this.bobberBarPos == (float)(568 - this.bobberBarHeight)))
				{
					this.bobberBarSpeed = 0f;
				}
				if (this.bobberInBar)
				{
					num4 *= ((this.whichBobber == 691) ? 0.3f : 0.6f);
					if (this.whichBobber == 691)
					{
						if (this.bobberPosition + 16f < this.bobberBarPos + (float)(this.bobberBarHeight / 2))
						{
							this.bobberBarSpeed -= 0.2f;
						}
						else
						{
							this.bobberBarSpeed += 0.2f;
						}
					}
				}
				float num5 = this.bobberBarPos;
				this.bobberBarSpeed += num4;
				this.bobberBarPos += this.bobberBarSpeed;
				if (this.bobberBarPos + (float)this.bobberBarHeight > 568f)
				{
					this.bobberBarPos = (float)(568 - this.bobberBarHeight);
					this.bobberBarSpeed = -this.bobberBarSpeed * 2f / 3f * ((this.whichBobber == 692) ? 0.1f : 1f);
					if (num5 + (float)this.bobberBarHeight < 568f)
					{
						Game1.playSound("shiny4");
					}
				}
				else if (this.bobberBarPos < 0f)
				{
					this.bobberBarPos = 0f;
					this.bobberBarSpeed = -this.bobberBarSpeed * 2f / 3f;
					if (num5 > 0f)
					{
						Game1.playSound("shiny4");
					}
				}
				bool flag = false;
				if (this.treasure)
				{
					float num6 = this.treasureAppearTimer;
					this.treasureAppearTimer -= (float)time.ElapsedGameTime.Milliseconds;
					if (this.treasureAppearTimer <= 0f)
					{
						if (this.treasureScale < 1f && !this.treasureCaught)
						{
							if (num6 > 0f)
							{
								this.treasurePosition = (float)((this.bobberBarPos > 274f) ? Game1.random.Next(8, (int)this.bobberBarPos - 20) : Game1.random.Next(Math.Min(528, (int)this.bobberBarPos + this.bobberBarHeight), 500));
								Game1.playSound("dwop");
							}
							this.treasureScale = Math.Min(1f, this.treasureScale + 0.1f);
						}
						flag = (this.treasurePosition + 16f <= this.bobberBarPos - 32f + (float)this.bobberBarHeight && this.treasurePosition - 16f >= this.bobberBarPos - 32f);
						if (flag && !this.treasureCaught)
						{
							this.treasureCatchLevel += 0.0135f;
							this.treasureShake = new Vector2((float)Game1.random.Next(-2, 3), (float)Game1.random.Next(-2, 3));
							if (this.treasureCatchLevel >= 1f)
							{
								Game1.playSound("newArtifact");
								this.treasureCaught = true;
							}
						}
						else if (this.treasureCaught)
						{
							this.treasureScale = Math.Max(0f, this.treasureScale - 0.1f);
						}
						else
						{
							this.treasureShake = Vector2.Zero;
							this.treasureCatchLevel = Math.Max(0f, this.treasureCatchLevel - 0.01f);
						}
					}
				}
				if (this.bobberInBar)
				{
					this.distanceFromCatching += 0.002f;
					this.reelRotation += 0.3926991f;
					this.fishShake.X = (float)Game1.random.Next(-10, 11) / 10f;
					this.fishShake.Y = (float)Game1.random.Next(-10, 11) / 10f;
					this.barShake = Vector2.Zero;
					Rumble.rumble(0.1f, 1000f);
					if (BobberBar.unReelSound != null)
					{
						BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
					}
					if (Game1.soundBank != null && (BobberBar.reelSound == null || BobberBar.reelSound.IsStopped || BobberBar.reelSound.IsStopping))
					{
						BobberBar.reelSound = Game1.soundBank.GetCue("fastReel");
					}
					if (BobberBar.reelSound != null && !BobberBar.reelSound.IsPlaying && !BobberBar.reelSound.IsStopping)
					{
						BobberBar.reelSound.Play();
					}
				}
				else if (!flag || this.treasureCaught || this.whichBobber != 693)
				{
					if (!this.fishShake.Equals(Vector2.Zero))
					{
						Game1.playSound("tinyWhip");
						this.perfect = false;
						Rumble.stopRumbling();
					}
					this.fishSizeReductionTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.fishSizeReductionTimer <= 0)
					{
						this.fishSize = Math.Max(this.minFishSize, this.fishSize - 1);
						this.fishSizeReductionTimer = 800;
					}
					if ((Game1.player.fishCaught != null && Game1.player.fishCaught.Count != 0) || Game1.currentMinigame != null)
					{
						this.distanceFromCatching -= ((this.whichBobber == 694) ? 0.002f : 0.003f);
					}
					float num7 = Math.Abs(this.bobberPosition - (this.bobberBarPos + (float)(this.bobberBarHeight / 2)));
					this.reelRotation -= 3.14159274f / Math.Max(10f, 200f - num7);
					this.barShake.X = (float)Game1.random.Next(-10, 11) / 10f;
					this.barShake.Y = (float)Game1.random.Next(-10, 11) / 10f;
					this.fishShake = Vector2.Zero;
					if (BobberBar.reelSound != null)
					{
						BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
					}
					if (Game1.soundBank != null && (BobberBar.unReelSound == null || BobberBar.unReelSound.IsStopped))
					{
						BobberBar.unReelSound = Game1.soundBank.GetCue("slowReel");
						BobberBar.unReelSound.SetVariable("Pitch", 600f);
					}
					if (BobberBar.unReelSound != null && !BobberBar.unReelSound.IsPlaying && !BobberBar.unReelSound.IsStopping)
					{
						BobberBar.unReelSound.Play();
					}
				}
				this.distanceFromCatching = Math.Max(0f, Math.Min(1f, this.distanceFromCatching));
				if (Game1.player.CurrentTool != null)
				{
					Game1.player.CurrentTool.tickUpdate(time, Game1.player);
				}
				if (this.distanceFromCatching <= 0f)
				{
					this.fadeOut = true;
					this.everythingShakeTimer = 500f;
					Game1.playSound("fishEscape");
					if (BobberBar.unReelSound != null)
					{
						BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
					}
					if (BobberBar.reelSound != null)
					{
						BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
					}
				}
				else if (this.distanceFromCatching >= 1f)
				{
					this.everythingShakeTimer = 500f;
					Game1.playSound("jingle1");
					this.fadeOut = true;
					if (BobberBar.unReelSound != null)
					{
						BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
					}
					if (BobberBar.reelSound != null)
					{
						BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
					}
					if (this.perfect)
					{
						this.sparkleText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:BobberBar_Perfect", new object[0]), Color.Yellow, Color.White, false, 0.1, 1500, -1, 500);
						if (Game1.isFestival())
						{
							Game1.CurrentEvent.perfectFishing();
						}
					}
					else if (this.fishSize == this.maxFishSize)
					{
						this.fishSize--;
					}
				}
			}
			if (this.bobberPosition < 0f)
			{
				this.bobberPosition = 0f;
			}
			if (this.bobberPosition > 548f)
			{
				this.bobberPosition = 548f;
			}
		}

		public override bool readyToClose()
		{
			return false;
		}

		public override void emergencyShutDown()
		{
			base.emergencyShutDown();
			if (BobberBar.unReelSound != null)
			{
				BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
			}
			if (BobberBar.reelSound != null)
			{
				BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
			}
			this.fadeOut = true;
			this.everythingShakeTimer = 500f;
			this.distanceFromCatching = -1f;
			Game1.playSound("fishEscape");
		}

		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.menuButton.Contains(new InputButton(key)))
			{
				this.emergencyShutDown();
			}
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - (this.flipBubble ? 44 : 20) + 104), (float)(this.yPositionOnScreen - 16 + 314)) + this.everythingShake, new Rectangle?(new Rectangle(652, 1685, 52, 157)), Color.White * 0.6f * this.scale, 0f, new Vector2(26f, 78.5f) * this.scale, (float)Game1.pixelZoom * this.scale, this.flipBubble ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.001f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 70), (float)(this.yPositionOnScreen + 296)) + this.everythingShake, new Rectangle?(new Rectangle(644, 1999, 37, 150)), Color.White * this.scale, 0f, new Vector2(18.5f, 74f) * this.scale, (float)Game1.pixelZoom * this.scale, SpriteEffects.None, 0.01f);
			if (this.scale == 1f)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64), (float)(this.yPositionOnScreen + 12 + (int)this.bobberBarPos)) + this.barShake + this.everythingShake, new Rectangle?(new Rectangle(682, 2078, 9, 2)), this.bobberInBar ? Color.White : (Color.White * 0.25f * ((float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 100.0), 2) + 2f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64), (float)(this.yPositionOnScreen + 12 + (int)this.bobberBarPos + 8)) + this.barShake + this.everythingShake, new Rectangle?(new Rectangle(682, 2081, 9, 1)), this.bobberInBar ? Color.White : (Color.White * 0.25f * ((float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 100.0), 2) + 2f)), 0f, Vector2.Zero, new Vector2(4f, (float)(this.bobberBarHeight - 16)), SpriteEffects.None, 0.89f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64), (float)(this.yPositionOnScreen + 12 + (int)this.bobberBarPos + this.bobberBarHeight - 8)) + this.barShake + this.everythingShake, new Rectangle?(new Rectangle(682, 2085, 9, 2)), this.bobberInBar ? Color.White : (Color.White * 0.25f * ((float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 100.0), 2) + 2f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
				b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 124, this.yPositionOnScreen + 4 + (int)(580f * (1f - this.distanceFromCatching)), 16, (int)(580f * this.distanceFromCatching)), Utility.getRedToGreenLerpColor(this.distanceFromCatching));
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 18), (float)(this.yPositionOnScreen + 514)) + this.everythingShake, new Rectangle?(new Rectangle(257, 1990, 5, 10)), Color.White, this.reelRotation, new Vector2(2f, 10f), 4f, SpriteEffects.None, 0.9f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64 + 18), (float)(this.yPositionOnScreen + 12 + 24) + this.treasurePosition) + this.treasureShake + this.everythingShake, new Rectangle?(new Rectangle(638, 1865, 20, 24)), Color.White, 0f, new Vector2(10f, 10f), 2f * this.treasureScale, SpriteEffects.None, 0.85f);
				if (this.treasureCatchLevel > 0f && !this.treasureCaught)
				{
					b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 64, this.yPositionOnScreen + 12 + (int)this.treasurePosition, 40, 8), Color.DimGray * 0.5f);
					b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 64, this.yPositionOnScreen + 12 + (int)this.treasurePosition, (int)(this.treasureCatchLevel * 40f), 8), Color.Orange);
				}
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64 + 18), (float)(this.yPositionOnScreen + 12 + 24) + this.bobberPosition) + this.fishShake + this.everythingShake, new Rectangle?(new Rectangle(614 + (this.bossFish ? 20 : 0), 1840, 20, 20)), Color.White, 0f, new Vector2(10f, 10f), 2f, SpriteEffects.None, 0.88f);
				if (this.sparkleText != null)
				{
					this.sparkleText.draw(b, new Vector2((float)(this.xPositionOnScreen - Game1.tileSize / 4), (float)(this.yPositionOnScreen - Game1.tileSize)));
				}
			}
			if (Game1.player.fishCaught != null && Game1.player.fishCaught.Count == 0)
			{
				Vector2 position = new Vector2((float)(this.xPositionOnScreen + (this.flipBubble ? (this.width + Game1.tileSize + Game1.pixelZoom * 2) : (-Game1.pixelZoom * 2 - 48 * Game1.pixelZoom))), (float)(this.yPositionOnScreen + Game1.tileSize * 3));
				if (!Game1.options.gamepadControls)
				{
					b.Draw(Game1.mouseCursors, position, new Rectangle?(new Rectangle(644, 1330, 48, 69)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
					return;
				}
				b.Draw(Game1.controllerMaps, position, new Rectangle?(Utility.controllerMapSourceRect(new Rectangle(681, 0, 96, 138))), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom / 2f, SpriteEffects.None, 0.88f);
			}
		}
	}
}
