using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Minigames
{
	public class MineCart : IMinigame
	{
		private class Spark
		{
			public float x;

			public float y;

			public Color c;

			public float dx;

			public float dy;

			public Spark(float x, float y, float dx, float dy)
			{
				this.x = x;
				this.y = y;
				this.dx = dx;
				this.dy = dy;
				this.c = Color.Yellow;
			}
		}

		public const int track1 = 1;

		public const int track2 = 2;

		public const int noTrack = 0;

		public const int trackSlopeLeft = 3;

		public const int trackSlopeRight = 4;

		public const int minecartObstacle = 1;

		public const int coinObstacle = 2;

		public int pixelScale = 4;

		public const int maxTrackDeviationFromZero = 6;

		public const int tilesBeyondViewportToSimulate = 4;

		public const int bgLoopWidth = 96;

		public const int tileOfMineCart = 6;

		public const int gapsBeforeForcedTrack = 4;

		public const int tracksBeforeConsideredObstacle = 2;

		public const float gravity = 0.21f;

		public const float snapMinecartToTrackThreshold = 6f;

		public const float maxDY = 4.5f;

		public float maxJumpHeight;

		public const float jumpStrengthPerTick = 0.6f;

		public const float dyThreshAtWhichJumpIsImpossible = 1f;

		public const int frostArea = 0;

		public const int lavaArea = 1;

		public const int waterArea = 2;

		public const int darkArea = 3;

		public const int heavenlyArea = 4;

		public const int brownArea = 5;

		public const int noSlope = 0;

		public const int slopingUp = 1;

		public const int slopingDown = 2;

		public const int mineLevelMode = 0;

		public const int arcadeTitleScreenMode = 1;

		public const int infiniteMode = 2;

		public const int progressMode = 3;

		public const int highScoreMode = 4;

		public const int respawnTime = 1400;

		public const int distanceToTravelInMineMode = 350;

		public const double ceilingHeightFluctuation = 0.15;

		public const double coinOccurance = 0.01;

		private float speed;

		private float speedAccumulator;

		private float lakeSpeedAccumulator;

		private float backBGPosition;

		private float midBGPosition;

		private float waterFallPosition;

		private int noiseSeed = Game1.random.Next(0, 2147483647);

		private int currentTrackY;

		private int screenWidth;

		private int screenHeight;

		private int tileSize;

		private int waterfallWidth = 1;

		private int ytileOffset;

		private int totalMotion;

		private int movingOnSlope;

		private int levelsBeat;

		private int gameMode;

		private int livesLeft;

		private int distanceToTravel = -1;

		private int respawnCounter;

		private int currentTheme;

		private float mineCartYPosition;

		private float mineCartXOffset;

		private float minecartDY;

		private float minecartPositionBeforeJump;

		private float minecartBumpOffset;

		private double lastNoiseValue;

		private double heightChangeThreshold;

		private double obstacleOccurance;

		private double heightFluctuationsThreshold;

		private bool isJumping;

		private bool reachedJumpApex;

		private bool reachedFinish;

		private float screenDarkness;

		private Cue minecartLoop;

		private Texture2D texture;

		private List<Point> track = new List<Point>();

		private List<Point> lakeDecor = new List<Point>();

		private List<Point> ceiling = new List<Point>();

		private List<Point> obstacles = new List<Point>();

		private List<MineCart.Spark> sparkShower = new List<MineCart.Spark>();

		private Color backBGTint;

		private Color midBGTint;

		private Color caveTint;

		private Color lakeTint;

		private Color waterfallTint;

		private Color trackShadowTint;

		private Color trackTint;

		private Matrix transformMatrix;

		public MineCart(int whichTheme, int mode)
		{
			this.changeScreenSize();
			this.maxJumpHeight = (float)(Game1.tileSize / this.pixelScale) * 5f;
			this.texture = Game1.content.Load<Texture2D>("Minigames\\MineCart");
			if (Game1.soundBank != null)
			{
				this.minecartLoop = Game1.soundBank.GetCue("minecartLoop");
				this.minecartLoop.Play();
			}
			this.ytileOffset = this.screenHeight / 2 / this.tileSize;
			this.gameMode = mode;
			this.setGameModeParameters();
			this.setUpTheme(whichTheme);
			this.createBeginningOfLevel();
			this.screenDarkness = 1f;
		}

		public bool overrideFreeMouseMovement()
		{
			return false;
		}

		public bool tick(GameTime time)
		{
			if (!this.reachedFinish && (this.livesLeft > 0 || this.gameMode == 2) && this.screenDarkness > 0f)
			{
				this.screenDarkness -= (float)time.ElapsedGameTime.Milliseconds * 0.002f;
			}
			int num = (this.track.ElementAt(6).X == 0) ? 9999 : (this.track.ElementAt(6).Y * this.tileSize + (int)((this.track.ElementAt(6).X == 3) ? (-(int)this.speedAccumulator) : ((this.track.ElementAt(6).X == 4) ? (this.speedAccumulator - 16f) : 0f)));
			if (this.respawnCounter <= 0 || this.track[6].X == 0 || this.obstacles[6].X == 1 || this.obstacles[7].X == 1)
			{
				this.speedAccumulator += (float)time.ElapsedGameTime.Milliseconds * this.speed;
				num = ((this.track.ElementAt(6).X == 0) ? 9999 : (this.track.ElementAt(6).Y * this.tileSize + (int)((this.track.ElementAt(6).X == 3) ? (-(int)this.speedAccumulator) : ((this.track.ElementAt(6).X == 4) ? (this.speedAccumulator - 16f) : 0f))));
				if (this.speedAccumulator >= (float)this.tileSize)
				{
					if (!this.isJumping && this.movingOnSlope == 0 && Game1.random.NextDouble() < 0.5)
					{
						this.minecartBumpOffset = (float)Game1.random.Next(1, 3);
					}
					if ((this.totalMotion + 1) % 1000 == 0)
					{
						Game1.playSound("newArtifact");
					}
					else if ((this.totalMotion + 1) % 100 == 0)
					{
						Game1.playSound("Pickup_Coin15");
					}
					this.totalMotion++;
					if (this.totalMotion > Game1.minecartHighScore)
					{
						Game1.minecartHighScore = this.totalMotion;
					}
					if (this.distanceToTravel != -1 && this.totalMotion >= this.distanceToTravel + this.screenWidth / this.tileSize)
					{
						if (!this.reachedFinish)
						{
							Game1.playSound("reward");
						}
						this.reachedFinish = true;
					}
					this.track.RemoveAt(0);
					this.ceiling.RemoveAt(0);
					this.obstacles.RemoveAt(0);
					if (this.distanceToTravel == -1 || this.totalMotion < this.distanceToTravel)
					{
						double num2 = NoiseGenerator.Noise(this.totalMotion, this.noiseSeed);
						Point zero = Point.Zero;
						if (num2 > this.heightChangeThreshold && this.lastNoiseValue <= this.heightChangeThreshold)
						{
							this.currentTrackY = Math.Max(this.currentTrackY - Game1.random.Next(1, 2), -6);
						}
						else if (num2 < this.heightChangeThreshold && this.lastNoiseValue >= this.heightChangeThreshold)
						{
							this.currentTrackY = Math.Min(this.currentTrackY + Game1.random.Next(1, (this.currentTrackY <= -3) ? 6 : 3), 4);
						}
						else if (Math.Abs(num2 - this.lastNoiseValue) > this.heightFluctuationsThreshold)
						{
							if (this.track[this.track.Count - 1].X == 0)
							{
								this.currentTrackY = Math.Max(-6, Math.Min(6, this.currentTrackY + Game1.random.Next(1, 1)));
							}
							else
							{
								this.currentTrackY = Math.Max(Math.Min(4, this.currentTrackY + Game1.random.Next(-3, 3)), -6);
							}
						}
						if (num2 < -0.5)
						{
							bool flag = false;
							for (int i = 0; i < 4 - ((Game1.random.NextDouble() < 0.1) ? 1 : 0); i++)
							{
								if (this.track[this.track.Count - 1 - i].X != 0)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								zero = new Point(0, 999);
							}
							else
							{
								zero = new Point(Game1.random.Next(1, 3), (Game1.random.NextDouble() < 0.5 && this.currentTrackY < 6) ? this.currentTrackY : (this.currentTrackY + 1));
							}
						}
						else
						{
							zero = new Point(Game1.random.Next(1, 3), this.currentTrackY);
						}
						if (this.track[this.track.Count - 1].X == 0 && zero.X != 0)
						{
							zero.Y = Math.Min(6, zero.Y + 1);
						}
						if (zero.Y == this.track[this.track.Count - 1].Y - 1)
						{
							this.track.RemoveAt(this.track.Count - 1);
							this.track.Add(new Point(3, this.currentTrackY + 1));
						}
						else if (zero.Y == this.track[this.track.Count - 1].Y + 1)
						{
							zero.X = 4;
						}
						this.track.Add(zero);
						this.ceiling.Add(new Point(Game1.random.Next(200), Math.Min(this.currentTrackY - 5 + this.ytileOffset, Math.Max(0, this.ceiling.Last<Point>().Y + ((Game1.random.NextDouble() < 0.15) ? Game1.random.Next(-1, 2) : 0)))));
						bool flag2 = false;
						for (int j = 0; j < 2; j++)
						{
							if (this.track[this.track.Count - 1 - j].X == 0 || this.track[this.track.Count - 1 - j - 1].Y != this.track[this.track.Count - 1 - j].Y)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2 && Game1.random.NextDouble() < this.obstacleOccurance && this.currentTrackY > -2 && this.track.Last<Point>().X != 3 && this.track.Last<Point>().X != 4)
						{
							this.obstacles.Add(new Point(1, this.currentTrackY));
						}
						else
						{
							this.obstacles.Add(Point.Zero);
						}
						this.lastNoiseValue = num2;
					}
					else
					{
						this.track.Add(new Point(Game1.random.Next(1, 3), this.currentTrackY));
						this.ceiling.Add(new Point(Game1.random.Next(200), this.ceiling.Last<Point>().Y));
						this.obstacles.Add(Point.Zero);
						this.lakeDecor.Add(new Point(Game1.random.Next(2), Game1.random.Next(this.ytileOffset + 1, this.screenHeight / this.tileSize)));
					}
					this.speedAccumulator %= (float)this.tileSize;
				}
				this.lakeSpeedAccumulator += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 4f);
				if (this.lakeSpeedAccumulator >= (float)this.tileSize)
				{
					this.lakeSpeedAccumulator %= (float)this.tileSize;
					this.lakeDecor.RemoveAt(0);
					this.lakeDecor.Add(new Point(Game1.random.Next(2), Game1.random.Next(this.ytileOffset + 3, this.screenHeight / this.tileSize)));
				}
				this.backBGPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 5f);
				this.backBGPosition %= 96f;
				this.midBGPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 4f);
				this.midBGPosition %= 96f;
				this.waterFallPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed * 6f / 5f);
				if (this.waterFallPosition > (float)(this.screenWidth * 3 / 2))
				{
					this.waterFallPosition %= (float)(this.screenWidth * 3 / 2);
					this.waterfallWidth = Game1.random.Next(6);
				}
			}
			else
			{
				this.respawnCounter -= time.ElapsedGameTime.Milliseconds;
				this.mineCartYPosition = (float)num;
			}
			if (Math.Abs(this.mineCartYPosition - (float)num) <= 6f && this.minecartDY >= 0f && this.movingOnSlope == 0)
			{
				if (this.minecartDY > 0f)
				{
					this.mineCartYPosition = (float)num;
					this.minecartDY = 0f;
					if (Game1.soundBank != null)
					{
						Game1.soundBank.GetCue("parry").Play();
						this.minecartLoop = Game1.soundBank.GetCue("minecartLoop");
						this.minecartLoop.Play();
					}
					this.isJumping = false;
					this.reachedJumpApex = false;
					this.createSparkShower();
				}
				if (this.track[6].X == 3)
				{
					this.movingOnSlope = 1;
					this.createSparkShower();
				}
				else if (this.track[6].X == 4)
				{
					this.movingOnSlope = 2;
					this.createSparkShower();
				}
			}
			else if (!this.isJumping && Math.Abs(this.mineCartYPosition - (float)num) <= 6f && (this.track[6].X == 3 || this.track[6].X == 4))
			{
				this.mineCartYPosition = (float)num;
				if (this.mineCartYPosition == (float)num && this.track[6].X == 3)
				{
					this.movingOnSlope = 1;
					if (this.respawnCounter <= 0)
					{
						this.createSparkShower(Game1.random.Next(2));
					}
				}
				else if (this.mineCartYPosition == (float)num && this.track[6].X == 4)
				{
					this.movingOnSlope = 2;
					if (this.respawnCounter <= 0)
					{
						this.createSparkShower(Game1.random.Next(2));
					}
				}
				this.minecartDY = 0f;
			}
			else
			{
				this.movingOnSlope = 0;
				this.minecartDY += (((this.reachedJumpApex || !this.isJumping) && this.mineCartYPosition != (float)num) ? 0.21f : 0f);
				if (this.minecartDY > 0f)
				{
					this.minecartDY = Math.Min(this.minecartDY, 9f);
				}
				if (this.minecartDY > 0f || this.minecartPositionBeforeJump - this.mineCartYPosition <= this.maxJumpHeight)
				{
					this.mineCartYPosition += this.minecartDY;
				}
			}
			if (this.minecartDY > 0f && this.minecartLoop != null && this.minecartLoop.IsPlaying)
			{
				this.minecartLoop.Stop(AudioStopOptions.Immediate);
			}
			if (this.reachedFinish)
			{
				this.mineCartXOffset += this.speed * (float)time.ElapsedGameTime.Milliseconds;
				if (Game1.random.NextDouble() < 0.25)
				{
					this.createSparkShower();
				}
			}
			if (this.mineCartXOffset > (float)(this.screenWidth - 6 * this.tileSize + this.tileSize))
			{
				int x = this.gameMode;
				if (x != 0)
				{
					if (x == 3)
					{
						this.screenDarkness += (float)time.ElapsedGameTime.Milliseconds / 2000f;
						if (this.screenDarkness >= 1f)
						{
							this.reachedFinish = false;
							this.currentTheme = (this.currentTheme + 1) % 6;
							this.levelsBeat++;
							if (this.levelsBeat == 6)
							{
								if (!Game1.player.hasOrWillReceiveMail("JunimoKart"))
								{
									Game1.addMailForTomorrow("JunimoKart", false, false);
								}
								this.unload();
								Game1.currentMinigame = null;
								DelayedAction.playSoundAfterDelay("discoverMineral", 1000);
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineCart.cs.12106", new object[0]));
							}
							else
							{
								this.setUpTheme(this.currentTheme);
								this.restartLevel();
							}
						}
					}
				}
				else
				{
					this.screenDarkness += (float)time.ElapsedGameTime.Milliseconds / 2000f;
					if (this.screenDarkness >= 1f)
					{
						if (Game1.mine != null)
						{
							Game1.mine.mineLevel += 3;
							Game1.warpFarmer("UndergroundMine", 16, 16, false);
							Game1.fadeToBlackAlpha = 1f;
						}
						return true;
					}
				}
			}
			if (this.speedAccumulator >= (float)(this.tileSize / 2) && ((int)(this.mineCartYPosition / (float)this.tileSize) == this.obstacles[7].Y || (int)(this.mineCartYPosition / (float)this.tileSize - (float)(this.tileSize - 1)) == this.obstacles[7].Y))
			{
				int x = this.obstacles[7].X;
				if (x != 1)
				{
					if (x == 2)
					{
						Game1.playSound("money");
						this.obstacles.RemoveAt(6);
						this.obstacles.Insert(6, Point.Zero);
					}
				}
				else
				{
					Game1.playSound("woodWhack");
					this.mineCartYPosition = (float)this.screenHeight;
				}
			}
			if (this.mineCartYPosition > (float)this.screenHeight)
			{
				this.mineCartYPosition = -999999f;
				this.livesLeft--;
				Game1.playSound("fishEscape");
				if (this.gameMode == 0 && (float)this.livesLeft < 0f)
				{
					this.mineCartYPosition = 999999f;
					this.livesLeft++;
					this.screenDarkness += (float)time.ElapsedGameTime.Milliseconds * 0.001f;
					if (this.screenDarkness >= 1f)
					{
						if (Game1.player.health > 1)
						{
							Game1.player.health = 1;
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineCart.cs.12108", new object[0]));
						}
						else
						{
							Game1.player.health = 0;
						}
						return true;
					}
				}
				else if (this.gameMode == 4 || (this.gameMode == 3 && this.livesLeft < 0))
				{
					if (this.gameMode == 3)
					{
						this.levelsBeat = 0;
						this.setUpTheme(5);
					}
					this.restartLevel();
				}
				else
				{
					this.respawnCounter = 1400;
					this.minecartDY = 0f;
					this.isJumping = false;
					this.reachedJumpApex = false;
					if (this.gameMode == 2)
					{
						this.totalMotion = 0;
					}
				}
			}
			this.minecartBumpOffset = Math.Max(0f, this.minecartBumpOffset - 0.5f);
			for (int k = this.sparkShower.Count - 1; k >= 0; k--)
			{
				this.sparkShower[k].dy += 0.105f;
				this.sparkShower[k].x += this.sparkShower[k].dx;
				this.sparkShower[k].y += this.sparkShower[k].dy;
				this.sparkShower[k].c.B = (byte)(0.0 + Math.Max(0.0, Math.Sin((double)time.TotalGameTime.Milliseconds / (62.831853071795862 / (double)this.sparkShower[k].dx)) * 255.0));
				if (this.reachedFinish)
				{
					this.sparkShower[k].c.R = (byte)(0.0 + Math.Max(0.0, Math.Sin((double)(time.TotalGameTime.Milliseconds + 50) / (62.831853071795862 / (double)this.sparkShower[k].dx)) * 255.0));
					this.sparkShower[k].c.G = (byte)(0.0 + Math.Max(0.0, Math.Sin((double)(time.TotalGameTime.Milliseconds + 100) / (62.831853071795862 / (double)this.sparkShower[k].dx)) * 255.0));
					if (this.sparkShower[k].c.R == 0)
					{
						this.sparkShower[k].c.R = 255;
					}
					if (this.sparkShower[k].c.G == 0)
					{
						this.sparkShower[k].c.G = 255;
					}
				}
				if (this.sparkShower[k].y > (float)this.screenHeight)
				{
					this.sparkShower.RemoveAt(k);
				}
			}
			return false;
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			this.jump();
		}

		public void releaseLeftClick(int x, int y)
		{
			this.releaseJump();
		}

		public void releaseRightClick(int x, int y)
		{
		}

		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public void receiveKeyPress(Keys k)
		{
			if (k.Equals(Keys.Escape) || Game1.options.doesInputListContain(Game1.options.menuButton, k))
			{
				if (Game1.isAnyGamePadButtonBeingPressed() && !GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.Start))
				{
					return;
				}
				this.unload();
				Game1.playSound("bigDeSelect");
				Game1.currentMinigame = null;
			}
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		private void restartLevel()
		{
			this.track.Clear();
			this.ceiling.Clear();
			this.lakeDecor.Clear();
			this.obstacles.Clear();
			this.totalMotion = 0;
			this.speedAccumulator = 0f;
			this.currentTrackY = 0;
			this.mineCartYPosition = 0f;
			this.minecartDY = 0f;
			this.isJumping = false;
			this.reachedJumpApex = false;
			this.reachedFinish = false;
			this.movingOnSlope = 0;
			this.mineCartXOffset = 0f;
			this.createBeginningOfLevel();
			this.setGameModeParameters();
		}

		public void createSparkShower()
		{
			int num = Game1.random.Next(3, 7);
			for (int i = 0; i < num; i++)
			{
				this.sparkShower.Add(new MineCart.Spark((float)(6 * this.tileSize - 3) + this.mineCartXOffset, this.mineCartYPosition + (float)(this.ytileOffset * this.tileSize) + (float)this.tileSize - 4f, (float)Game1.random.Next(-200, 5) / 100f, (float)(-(float)Game1.random.Next(5, 150)) / 100f));
			}
		}

		public void createSparkShower(int number)
		{
			for (int i = 0; i < number; i++)
			{
				this.sparkShower.Add(new MineCart.Spark((float)(6 * this.tileSize - 3) + this.mineCartXOffset, this.mineCartYPosition + (float)(this.ytileOffset * this.tileSize) + (float)this.tileSize - 4f, (float)Game1.random.Next(-200, 5) / 100f, (float)(-(float)Game1.random.Next(5, 150)) / 100f));
			}
		}

		public void createBeginningOfLevel()
		{
			for (int i = 0; i < this.screenWidth / this.tileSize + 4; i++)
			{
				this.track.Add(new Point(Game1.random.Next(1, 3), 0));
				this.ceiling.Add(new Point(Game1.random.Next(200), 0));
				this.obstacles.Add(Point.Zero);
				this.lakeDecor.Add(new Point(Game1.random.Next(2), Game1.random.Next(this.ytileOffset + 3, this.screenHeight / this.tileSize)));
			}
		}

		public void setGameModeParameters()
		{
			int num = this.gameMode;
			if (num == 0)
			{
				this.distanceToTravel = 200;
				this.livesLeft = 3;
				return;
			}
			if (num != 3)
			{
				return;
			}
			this.distanceToTravel = 200;
			this.livesLeft = 3;
		}

		public void setUpTheme(int whichTheme)
		{
			switch (whichTheme)
			{
			case 0:
				this.backBGTint = new Color(254, 254, 254);
				this.midBGTint = new Color(254, 254, 254);
				this.caveTint = new Color(230, 244, 254);
				this.lakeTint = new Color(150, 210, 255);
				this.waterfallTint = Color.LightCyan * 0.5f;
				this.trackTint = Color.LightCyan;
				this.speed = 0.085f;
				NoiseGenerator.Amplitude = 2.8;
				NoiseGenerator.Frequency = 0.18;
				this.heightChangeThreshold = 0.85;
				this.obstacleOccurance = 0.05;
				this.heightFluctuationsThreshold = 0.35;
				this.trackShadowTint = Color.DarkSlateBlue;
				break;
			case 1:
				this.backBGTint = Color.DarkRed;
				this.midBGTint = Color.DarkSalmon;
				this.caveTint = Color.DarkRed;
				this.lakeTint = Color.DarkRed;
				this.trackTint = Color.DarkGray;
				this.waterfallTint = Color.Red * 0.9f;
				this.trackShadowTint = Color.DarkOrange;
				this.speed = 0.12f;
				this.heightChangeThreshold = 0.8;
				NoiseGenerator.Amplitude = 3.0;
				NoiseGenerator.Frequency = 0.18;
				this.obstacleOccurance = 0.05;
				this.heightFluctuationsThreshold = 0.2;
				break;
			case 2:
				this.backBGTint = new Color(50, 150, 225);
				this.midBGTint = new Color(120, 170, 225);
				this.caveTint = Color.SlateGray;
				this.lakeTint = new Color(30, 120, 215);
				this.waterfallTint = Color.White * 0.5f;
				this.trackTint = Color.Gray;
				this.speed = 0.085f;
				NoiseGenerator.Amplitude = 3.0;
				NoiseGenerator.Frequency = 0.15;
				this.heightChangeThreshold = 0.9;
				this.obstacleOccurance = 0.05;
				this.heightFluctuationsThreshold = 0.4;
				this.trackShadowTint = Color.DarkSlateBlue;
				break;
			case 3:
				this.backBGTint = new Color(60, 60, 60);
				this.midBGTint = new Color(60, 60, 60);
				this.caveTint = new Color(70, 70, 70);
				this.lakeTint = new Color(60, 70, 80);
				this.trackTint = Color.DimGray;
				this.waterfallTint = Color.Black * 0f;
				this.trackShadowTint = Color.Black;
				this.speed = 0.1f;
				this.heightChangeThreshold = 0.7;
				NoiseGenerator.Amplitude = 3.0;
				NoiseGenerator.Frequency = 0.2;
				this.obstacleOccurance = 0.0;
				this.heightFluctuationsThreshold = 0.2;
				break;
			case 4:
				this.backBGTint = Color.SeaGreen;
				this.midBGTint = Color.Green;
				this.caveTint = new Color(255, 200, 60);
				this.lakeTint = Color.Lime;
				this.trackTint = Color.LightSlateGray;
				this.waterfallTint = Color.ForestGreen * 0.5f;
				this.trackShadowTint = new Color(0, 180, 50);
				this.speed = 0.08f;
				this.heightChangeThreshold = 0.6;
				NoiseGenerator.Amplitude = 3.1;
				NoiseGenerator.Frequency = 0.24;
				this.obstacleOccurance = 0.05;
				this.heightFluctuationsThreshold = 0.15;
				break;
			case 5:
				this.backBGTint = Color.DarkKhaki;
				this.midBGTint = Color.SandyBrown;
				this.caveTint = Color.SandyBrown;
				this.lakeTint = Color.MediumAquamarine;
				this.trackTint = Color.Beige;
				this.waterfallTint = Color.MediumAquamarine * 0.9f;
				this.trackShadowTint = new Color(60, 60, 60);
				this.speed = 0.085f;
				this.heightChangeThreshold = 0.8;
				NoiseGenerator.Amplitude = 2.0;
				NoiseGenerator.Frequency = 0.12;
				this.obstacleOccurance = 0.05;
				this.heightFluctuationsThreshold = 0.25;
				break;
			}
			this.currentTheme = whichTheme;
		}

		private void jump()
		{
			if (this.minecartDY < 1f && this.respawnCounter <= 0)
			{
				if (!this.isJumping)
				{
					this.movingOnSlope = 0;
					this.minecartPositionBeforeJump = this.mineCartYPosition;
					this.isJumping = true;
					if (this.minecartLoop != null)
					{
						this.minecartLoop.Stop(AudioStopOptions.Immediate);
					}
					if (Game1.soundBank != null)
					{
						Cue expr_68 = Game1.soundBank.GetCue("pickUpItem");
						expr_68.SetVariable("Pitch", 200f);
						expr_68.Play();
					}
				}
				if (!this.reachedJumpApex)
				{
					this.minecartDY = Math.Max(-4.5f, this.minecartDY - 0.6f);
					if (this.minecartDY <= -4.5f)
					{
						this.reachedJumpApex = true;
					}
				}
			}
		}

		private void releaseJump()
		{
			if (this.isJumping)
			{
				this.reachedJumpApex = true;
			}
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, this.transformMatrix);
			for (int i = 0; i <= this.screenWidth / this.tileSize + 1; i++)
			{
				b.Draw(this.texture, new Rectangle(i * this.tileSize - (int)this.lakeSpeedAccumulator, this.tileSize * 9, this.tileSize, this.screenHeight - 96), new Rectangle?(new Rectangle(0, 80, 16, 97)), this.lakeTint);
			}
			for (int j = 0; j < this.lakeDecor.Count; j++)
			{
				b.Draw(this.texture, new Vector2((float)(j * this.tileSize) - this.lakeSpeedAccumulator, (float)(this.lakeDecor.ElementAt(j).Y * this.tileSize)), new Rectangle?(new Rectangle(32 + this.lakeDecor.ElementAt(j).X * this.tileSize, 0, 16, 16)), (this.lakeDecor.ElementAt(j).X == 0) ? this.midBGTint : this.lakeTint);
			}
			for (int k = 0; k <= this.screenWidth / 96 + 2; k++)
			{
				b.Draw(this.texture, new Vector2(-this.backBGPosition + (float)(k * 96), (float)(this.tileSize * 2)), new Rectangle?(new Rectangle(64, 162, 96, 111)), this.backBGTint);
			}
			for (int l = 0; l < this.screenWidth / 96 + 2; l++)
			{
				b.Draw(this.texture, new Vector2(-this.midBGPosition + (float)(l * 96), 0f), new Rectangle?(new Rectangle(64, 0, 96, 162)), this.midBGTint);
			}
			for (int m = 0; m < this.track.Count; m++)
			{
				if (this.track.ElementAt(m).X != 0)
				{
					b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float)(m * this.tileSize), (float)((this.track.ElementAt(m).Y + this.ytileOffset) * this.tileSize - this.tileSize)), new Rectangle?(new Rectangle((this.track.ElementAt(m).X - 1) * 16, 180, 16, 32)), this.trackTint);
					float num = 0f;
					for (int n = this.track.ElementAt(m).Y + 1; n < this.screenHeight / this.tileSize; n++)
					{
						b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float)(m * this.tileSize), (float)((n + this.ytileOffset) * this.tileSize)), new Rectangle?(new Rectangle(16 + ((n % 2 == 0) ? ((this.track.ElementAt(m).X + 1) % 2) : (this.track.ElementAt(m).X % 2)) * 16, 32, 16, 16)), this.trackTint);
						b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float)(m * this.tileSize), (float)((n + this.ytileOffset) * this.tileSize)), new Rectangle?(new Rectangle(16 + ((n % 2 == 0) ? ((this.track.ElementAt(m).X + 1) % 2) : (this.track.ElementAt(m).X % 2)) * 16, 32, 16, 16)), this.trackShadowTint * num);
						num += 0.1f;
					}
				}
			}
			for (int num2 = 0; num2 < this.obstacles.Count; num2++)
			{
				int x = this.obstacles[num2].X;
				if (x != 1)
				{
					if (x == 2)
					{
						b.Draw(Game1.debrisSpriteSheet, new Vector2(-this.speedAccumulator + (float)(num2 * this.tileSize), (float)((this.obstacles[num2].Y + this.ytileOffset) * this.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), 0f), 0.25f, SpriteEffects.None, 0f);
					}
				}
				else
				{
					b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float)(num2 * this.tileSize), (float)((this.obstacles[num2].Y + this.ytileOffset) * this.tileSize)), new Rectangle?(new Rectangle(16, 0, 16, 16)), Color.White);
				}
			}
			if (this.respawnCounter / 200 % 2 == 0)
			{
				b.Draw(this.texture, new Vector2((float)(6 * this.tileSize + this.tileSize / 2) + this.mineCartXOffset, (float)(this.ytileOffset * this.tileSize) + this.mineCartYPosition + (float)this.tileSize - this.minecartBumpOffset - 4f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White, (this.minecartDY < 0f) ? (this.minecartDY / 3.14159274f / 2f) : 0f, new Vector2(16f, 16f), 1f, SpriteEffects.None, 0f);
				Game1.player.faceDirection(1);
				b.Draw(Game1.mouseCursors, new Vector2((float)(6 * this.tileSize - 2) + this.mineCartXOffset, (float)(this.ytileOffset * this.tileSize - 22) + this.mineCartYPosition + (float)this.tileSize + (float)this.tileSize - this.minecartBumpOffset - 8f), new Rectangle?(new Rectangle(294 + (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0) / 100 * 16, 1432, 16, 16)), Color.Lime, (this.minecartDY < 0f) ? (this.minecartDY / 3.14159274f / 4f) : 0f, new Vector2(8f, 8f), 0.6666667f, SpriteEffects.None, 0.1f);
				b.Draw(this.texture, new Vector2((float)(6 * this.tileSize + this.tileSize / 2) + this.mineCartXOffset, (float)(this.ytileOffset * this.tileSize) + this.mineCartYPosition + (float)this.tileSize - this.minecartBumpOffset - 4f + 8f), new Rectangle?(new Rectangle(0, 8, 16, 8)), Color.White, (this.minecartDY < 0f) ? (this.minecartDY / 3.14159274f / 4f) : 0f, new Vector2(16f, 16f), 1f, SpriteEffects.None, 0.1f);
			}
			foreach (MineCart.Spark current in this.sparkShower)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)current.x, (int)current.y, this.pixelScale / 4, this.pixelScale / 4), current.c);
			}
			for (int num3 = 0; num3 < this.waterfallWidth; num3 += 2)
			{
				for (int num4 = -2; num4 <= this.screenHeight / this.tileSize + 1; num4++)
				{
					b.Draw(this.texture, new Vector2((float)(this.screenWidth + this.tileSize * num3) - this.waterFallPosition, (float)(num4 * this.tileSize) + this.lakeSpeedAccumulator * 2f), new Rectangle?(new Rectangle(48, 32, 16, 16)), this.waterfallTint);
				}
			}
			if (this.gameMode != 2 && this.totalMotion < this.distanceToTravel + this.screenWidth / this.tileSize)
			{
				if (this.gameMode != 4)
				{
					for (int num5 = 0; num5 < this.livesLeft; num5++)
					{
						b.Draw(this.texture, new Vector2((float)(this.screenWidth - num5 * (this.tileSize + 2) - this.tileSize), 0f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White);
					}
				}
				b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale, this.pixelScale, this.tileSize * 8, this.pixelScale), Color.LightGray);
				b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale + (int)((float)this.totalMotion / (float)(this.distanceToTravel + this.screenWidth / this.tileSize) * (float)(this.tileSize * 8 - this.pixelScale)), this.pixelScale, this.pixelScale, this.pixelScale), Color.Aquamarine);
				for (int num6 = 0; num6 < 4; num6++)
				{
					b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale + this.tileSize * 8, this.pixelScale + num6 * (this.pixelScale / 4), this.pixelScale / 4, this.pixelScale / 4), (num6 % 2 == 0) ? Color.White : Color.Black);
					b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale + this.tileSize * 8 + this.pixelScale / 4, this.pixelScale + num6 * (this.pixelScale / 4), this.pixelScale / 4, this.pixelScale / 4), (num6 % 2 == 0) ? Color.Black : Color.White);
				}
				b.DrawString(Game1.dialogueFont, string.Concat(this.levelsBeat + 1), new Vector2((float)(this.pixelScale * 2 + this.tileSize * 8), (float)this.pixelScale / 2f), Color.Orange, 0f, Vector2.Zero, 1f / (float)Game1.pixelZoom, SpriteEffects.None, 0f);
			}
			else if (this.gameMode == 2)
			{
				string arg = Game1.content.LoadString("Strings\\StringsFromCSFiles:MineCart.cs.12115", new object[0]);
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.10444", new object[]
				{
					this.totalMotion
				}), new Vector2(1f, 1f), Color.White, 0f, Vector2.Zero, 1f / (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				b.DrawString(Game1.dialogueFont, arg + Game1.minecartHighScore, new Vector2(128f, 1f), Color.White, 0f, Vector2.Zero, 1f / (float)Game1.pixelZoom, SpriteEffects.None, 0f);
			}
			if (this.screenDarkness > 0f)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.screenWidth, this.screenHeight + this.tileSize), Color.Black * this.screenDarkness);
			}
			b.End();
		}

		public void changeScreenSize()
		{
			this.pixelScale = 4;
			int num = (Game1.viewport.Height < 1000) ? 3 : 4;
			this.screenWidth = Game1.viewport.Width / num;
			this.screenHeight = Game1.viewport.Height / num;
			this.tileSize = Game1.tileSize / this.pixelScale;
			this.ytileOffset = this.screenHeight / 2 / this.tileSize;
			this.maxJumpHeight = (float)(Game1.tileSize / this.pixelScale) * 5f;
			this.transformMatrix = Matrix.CreateScale((float)num);
		}

		public void unload()
		{
			Game1.player.faceDirection(0);
			if (this.minecartLoop != null && this.minecartLoop.IsPlaying)
			{
				this.minecartLoop.Stop(AudioStopOptions.Immediate);
			}
		}

		public void leftClickHeld(int x, int y)
		{
			if (this.isJumping && !this.reachedJumpApex)
			{
				this.minecartDY = Math.Max(-4.5f, this.minecartDY - 0.6f);
				if (this.minecartDY == -4.5f)
				{
					this.reachedJumpApex = true;
				}
			}
		}

		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		public string minigameId()
		{
			return "MineCart";
		}
	}
}
