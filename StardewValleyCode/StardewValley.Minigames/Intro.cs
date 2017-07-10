using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Minigames
{
	public class Intro : IMinigame
	{
		public class Balloon
		{
			public Vector2 position;

			public Color color;

			public Balloon(int screenWidth, int screenHeight)
			{
				int num = Game1.random.Next(255);
				int b = 255 - num;
				int r = (Game1.random.NextDouble() < 0.5) ? 255 : 0;
				this.position = new Vector2((float)Game1.random.Next(screenWidth / 5, screenWidth), (float)screenHeight);
				this.color = new Color(r, num, b);
			}

			public void update(float speed, GameTime time)
			{
				this.position.Y = this.position.Y - speed * (float)time.ElapsedGameTime.Milliseconds / 16f;
				this.position.X = this.position.X - speed * (float)time.ElapsedGameTime.Milliseconds / 32f;
			}
		}

		public const int pixelScale = 3;

		public const int valleyLoopWidth = 160;

		public const int skyLoopWidth = 112;

		public const int cloudLoopWidth = 170;

		public const int tilesBeyondViewportToSimulate = 6;

		public const int leftFence = 0;

		public const int centerFence = 1;

		public const int rightFence = 2;

		public const int busYRest = 240;

		public const int choosingCharacterState = 0;

		public const int panningDownFromCloudsState = 1;

		public const int panningDownToRoadState = 2;

		public const int drivingState = 3;

		public const int stardewInViewState = 4;

		public float speed = 0.1f;

		private float valleyPosition;

		private float skyPosition;

		private float roadPosition;

		private float bigCloudPosition;

		private float frontCloudPosition;

		private float backCloudPosition;

		private float globalYPan;

		private float globalYPanDY;

		private float drivingTimer;

		private float fadeAlpha;

		private int screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width / 3;

		private int screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height / 3;

		private int tileSize = 16;

		private Matrix transformMatrix;

		private Texture2D texture;

		private Texture2D roadsideTexture;

		private Texture2D cloudTexture;

		private List<Point> backClouds = new List<Point>();

		private List<int> road = new List<int>();

		private List<int> sky = new List<int>();

		private List<int> roadsideObjects = new List<int>();

		private List<int> roadsideFences = new List<int>();

		private Color skyColor;

		private Color roadColor;

		private Color carColor;

		private bool cameraCenteredOnBus = true;

		private bool addedSign;

		private Vector2 busPosition;

		private Vector2 carPosition;

		private Vector2 birdPosition = Vector2.Zero;

		private CharacterCustomization characterCreateMenu;

		private List<Intro.Balloon> balloons = new List<Intro.Balloon>();

		private int birdFrame;

		private int birdTimer;

		private int birdXTimer;

		public static Cue roadNoise;

		private int fenceBuildStatus = -1;

		private int currentState;

		private bool quit;

		private bool hasQuit;

		public Intro()
		{
			new List<int>();
			new List<int>();
			new List<int>();
			this.texture = Game1.content.Load<Texture2D>("Minigames\\Intro");
			this.roadsideTexture = Game1.content.Load<Texture2D>("Maps\\spring_outdoorsTileSheet");
			this.cloudTexture = Game1.content.Load<Texture2D>("Minigames\\Clouds");
			this.transformMatrix = Matrix.CreateScale(3f);
			this.skyColor = new Color(64, 136, 248);
			this.roadColor = new Color(130, 130, 130);
			this.createBeginningOfLevel();
			Game1.player.FarmerSprite.SourceRect = new Rectangle(0, 0, 16, 32);
			this.bigCloudPosition = (float)this.cloudTexture.Width;
			if (Game1.soundBank != null)
			{
				Intro.roadNoise = Game1.soundBank.GetCue("roadnoise");
			}
			this.currentState = 1;
			Game1.changeMusicTrack("spring_day_ambient");
		}

		public Intro(int startingGameMode)
		{
			this.texture = Game1.content.Load<Texture2D>("Minigames\\Intro");
			this.roadsideTexture = Game1.content.Load<Texture2D>("Maps\\spring_outdoorsTileSheet");
			this.cloudTexture = Game1.content.Load<Texture2D>("Minigames\\Clouds");
			this.transformMatrix = Matrix.CreateScale(3f);
			this.skyColor = new Color(102, 181, 255);
			this.roadColor = new Color(130, 130, 130);
			this.createBeginningOfLevel();
			this.currentState = startingGameMode;
			if (this.currentState == 4)
			{
				this.fadeAlpha = 1f;
			}
		}

		public bool overrideFreeMouseMovement()
		{
			return false;
		}

		public void createBeginningOfLevel()
		{
			this.backClouds.Clear();
			this.road.Clear();
			this.sky.Clear();
			this.roadsideObjects.Clear();
			this.roadsideFences.Clear();
			for (int i = 0; i < this.screenWidth / this.tileSize + 6; i++)
			{
				this.road.Add((Game1.random.NextDouble() < 0.7) ? 0 : Game1.random.Next(0, 3));
				this.roadsideObjects.Add(-1);
				this.roadsideFences.Add(-1);
			}
			for (int j = 0; j < this.screenWidth / 112 + 2; j++)
			{
				this.sky.Add((Game1.random.NextDouble() < 0.5) ? 1 : Game1.random.Next(2));
			}
			for (int k = 0; k < this.screenWidth / 170 + 2; k++)
			{
				this.backClouds.Add(new Point(Game1.random.Next(3), Game1.random.Next(this.screenHeight / 2)));
			}
			this.roadsideObjects.Add(-1);
			this.roadsideObjects.Add(-1);
			this.roadsideObjects.Add(-1);
			this.busPosition = new Vector2((float)(this.tileSize * 8), 240f);
		}

		public void updateRoad(GameTime time)
		{
			this.roadPosition += (float)time.ElapsedGameTime.Milliseconds * this.speed;
			if (this.roadPosition >= (float)(this.tileSize * 3))
			{
				this.roadPosition -= (float)(this.tileSize * 3);
				for (int i = 0; i < 3; i++)
				{
					this.road.Add((Game1.random.NextDouble() < 0.7) ? 0 : Game1.random.Next(0, 3));
				}
				this.road.RemoveRange(0, 3);
				if (this.fenceBuildStatus != -1 || (this.cameraCenteredOnBus && Game1.random.NextDouble() < 0.1))
				{
					for (int j = 0; j < 3; j++)
					{
						switch (this.fenceBuildStatus)
						{
						case -1:
							this.fenceBuildStatus = 0;
							this.roadsideFences.Add(0);
							break;
						case 0:
							this.fenceBuildStatus = 1;
							this.roadsideFences.Add(Game1.random.Next(3));
							break;
						case 1:
							if (Game1.random.NextDouble() < 0.1)
							{
								this.roadsideFences.Add(2);
								this.fenceBuildStatus = 2;
							}
							else
							{
								this.fenceBuildStatus = 1;
								this.roadsideFences.Add((Game1.random.NextDouble() < 0.1) ? 3 : Game1.random.Next(3));
							}
							break;
						case 2:
							this.fenceBuildStatus = -1;
							for (int k = j; k < 3; k++)
							{
								this.roadsideFences.Add(-1);
							}
							break;
						}
						if (this.fenceBuildStatus == -1)
						{
							break;
						}
					}
				}
				else
				{
					this.roadsideFences.Add(-1);
					this.roadsideFences.Add(-1);
					this.roadsideFences.Add(-1);
				}
				this.roadsideFences.RemoveRange(0, 3);
				if (this.cameraCenteredOnBus && !this.addedSign && Game1.random.NextDouble() < 0.25)
				{
					for (int l = 0; l < 3; l++)
					{
						if (l == 0 && Game1.random.NextDouble() < 0.3)
						{
							this.roadsideObjects.Add(Game1.random.Next(2));
							for (int m = l; m < 3; m++)
							{
								this.roadsideObjects.Add(-1);
							}
							break;
						}
						if (Game1.random.NextDouble() < 0.5)
						{
							this.roadsideObjects.Add(Game1.random.Next(2, 5));
						}
						else
						{
							this.roadsideObjects.Add(-1);
						}
					}
				}
				else
				{
					this.roadsideObjects.Add(-1);
					this.roadsideObjects.Add(-1);
					this.roadsideObjects.Add(-1);
				}
				this.roadsideObjects.RemoveRange(0, 3);
			}
			this.skyPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 12f);
			if (this.skyPosition >= 112f)
			{
				this.skyPosition -= 112f;
				this.sky.Add(Game1.random.Next(2));
				this.sky.RemoveAt(0);
			}
			this.valleyPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 6f);
			if (this.carPosition.Equals(Vector2.Zero) && Game1.random.NextDouble() < 0.002 && !this.addedSign)
			{
				this.carPosition = new Vector2((float)this.screenWidth, 200f);
				this.carColor = new Color(Game1.random.Next(100, 255), Game1.random.Next(100, 255), Game1.random.Next(100, 255));
				return;
			}
			if (!this.carPosition.Equals(Vector2.Zero))
			{
				this.carPosition.X = this.carPosition.X - 0.1f * (float)time.ElapsedGameTime.Milliseconds * ((float)this.carColor.G / 60f);
				if (this.carPosition.X < -200f)
				{
					this.carPosition = Vector2.Zero;
				}
			}
		}

		public void updateUpperClouds(GameTime time)
		{
			this.bigCloudPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 24f);
			if (this.bigCloudPosition >= (float)(this.cloudTexture.Width * 3))
			{
				this.bigCloudPosition -= (float)(this.cloudTexture.Width * 3);
			}
			this.backCloudPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 36f);
			if (this.backCloudPosition > 170f)
			{
				this.backCloudPosition %= 170f;
				this.backClouds.Add(new Point(Game1.random.Next(3), Game1.random.Next(this.screenHeight / 2)));
				this.backClouds.RemoveAt(0);
			}
			if (Game1.random.NextDouble() < 0.0002)
			{
				this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
				if (Game1.random.NextDouble() < 0.1)
				{
					Vector2 vector = new Vector2((float)Game1.random.Next(this.screenWidth / 3, this.screenWidth), (float)this.screenHeight);
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(vector.X + (float)Game1.random.Next(-16, 16), vector.Y + (float)Game1.random.Next(8));
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(vector.X + (float)Game1.random.Next(-16, 16), vector.Y + (float)Game1.random.Next(8));
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(vector.X + (float)Game1.random.Next(-16, 16), vector.Y + (float)Game1.random.Next(8));
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(vector.X + (float)Game1.random.Next(-16, 16), vector.Y + (float)Game1.random.Next(8));
				}
			}
			for (int i = this.balloons.Count - 1; i >= 0; i--)
			{
				this.balloons[i].update(this.speed, time);
				if (this.balloons[i].position.X < (float)(-(float)this.tileSize) || this.balloons[i].position.Y < (float)(-(float)this.tileSize))
				{
					this.balloons.RemoveAt(i);
				}
			}
		}

		public bool tick(GameTime time)
		{
			if (this.hasQuit)
			{
				return true;
			}
			if (this.quit && !this.hasQuit)
			{
				Game1.warpFarmer("BusStop", 12, 11, false);
				if (Intro.roadNoise != null)
				{
					Intro.roadNoise.Stop(AudioStopOptions.Immediate);
				}
				Game1.exitActiveMenu();
				this.hasQuit = true;
				return true;
			}
			switch (this.currentState)
			{
			case 0:
				this.updateUpperClouds(time);
				break;
			case 1:
				this.globalYPanDY = Math.Min(4f, this.globalYPanDY + (float)time.ElapsedGameTime.Milliseconds * (this.speed / 140f));
				this.globalYPan -= this.globalYPanDY;
				this.updateUpperClouds(time);
				if (this.globalYPan < -1f)
				{
					this.globalYPan = (float)(this.screenHeight * 3);
					this.currentState = 2;
					this.transformMatrix = Matrix.CreateScale(3f);
					this.transformMatrix.Translation = new Vector3(0f, this.globalYPan, 0f);
					if (Game1.soundBank != null && Intro.roadNoise != null)
					{
						Intro.roadNoise.SetVariable("Volume", 0f);
						Intro.roadNoise.Play();
					}
					Game1.loadForNewGame(false);
				}
				break;
			case 2:
				this.globalYPanDY = Math.Max(0.5f, this.globalYPan / 100f);
				if (Game1.soundBank != null && Intro.roadNoise != null)
				{
					Intro.roadNoise.SetVariable("Volume", Math.Max(90f, 1f / (this.globalYPan / (float)(this.screenHeight * 3)) * 90f));
				}
				this.globalYPan -= this.globalYPanDY;
				this.transformMatrix = Matrix.CreateScale(3f);
				this.transformMatrix.Translation = new Vector3(0f, this.globalYPan, 0f);
				this.updateRoad(time);
				if (this.globalYPan <= (float)(0 - Math.Max(0, 900 - Game1.graphics.GraphicsDevice.Viewport.Height)))
				{
					this.globalYPan = (float)(-(float)Math.Max(0, 900 - Game1.graphics.GraphicsDevice.Viewport.Height));
					this.currentState = 3;
					if (Game1.soundBank != null && Intro.roadNoise != null)
					{
						Intro.roadNoise.SetVariable("Volume", 100f);
					}
				}
				break;
			case 3:
				this.updateRoad(time);
				this.drivingTimer += (float)time.ElapsedGameTime.Milliseconds;
				if (this.drivingTimer > 5700f)
				{
					this.drivingTimer = 0f;
					this.currentState = 4;
				}
				break;
			case 4:
				this.updateRoad(time);
				this.drivingTimer += (float)time.ElapsedGameTime.Milliseconds;
				if (this.drivingTimer > 2000f)
				{
					this.busPosition.X = this.busPosition.X + (float)time.ElapsedGameTime.Milliseconds / 8f;
					if (Game1.soundBank != null && Intro.roadNoise != null)
					{
						Intro.roadNoise.SetVariable("Volume", Math.Max(0f, Intro.roadNoise.GetVariable("Volume") - 1f));
					}
					this.speed = Math.Max(0f, this.speed - (float)time.ElapsedGameTime.Milliseconds / 70000f);
					if (!this.addedSign)
					{
						this.addedSign = true;
						this.roadsideObjects.RemoveAt(this.roadsideObjects.Count - 1);
						this.roadsideObjects.Add(5);
						Game1.playSound("busDriveOff");
					}
					if (this.speed <= 0f && this.birdPosition.Equals(Vector2.Zero))
					{
						int num = 0;
						for (int i = 0; i < this.roadsideObjects.Count; i++)
						{
							if (this.roadsideObjects[i] == 5)
							{
								num = i;
								break;
							}
						}
						this.birdPosition = new Vector2((float)(num * 16) - this.roadPosition - 32f + 16f, -16f);
						Game1.playSound("SpringBirds");
						this.fadeAlpha = 0f;
					}
					if (!this.birdPosition.Equals(Vector2.Zero) && this.birdPosition.Y < 116f)
					{
						float num2 = Math.Max(0.5f, (116f - this.birdPosition.Y) / 116f * 2f);
						this.birdPosition.Y = this.birdPosition.Y + num2;
						this.birdPosition.X = this.birdPosition.X + (float)Math.Sin((double)this.birdXTimer / 50.26548245743669) * num2 / 2f;
						this.birdTimer += time.ElapsedGameTime.Milliseconds;
						this.birdXTimer += time.ElapsedGameTime.Milliseconds;
						if (this.birdTimer >= 100)
						{
							this.birdFrame = (this.birdFrame + 1) % 4;
							this.birdTimer = 0;
						}
					}
					else if (!this.birdPosition.Equals(Vector2.Zero))
					{
						this.birdFrame = ((this.birdTimer > 1500) ? 5 : 4);
						this.birdTimer += time.ElapsedGameTime.Milliseconds;
						if (this.birdTimer > 2400 || (this.birdTimer > 1800 && Game1.random.NextDouble() < 0.006))
						{
							this.birdTimer = 0;
							if (Game1.random.NextDouble() < 0.5)
							{
								Game1.playSound("SpringBirds");
								this.birdPosition.Y = this.birdPosition.Y - 4f;
							}
						}
					}
					if (this.drivingTimer > 14000f)
					{
						this.fadeAlpha += (float)time.ElapsedGameTime.Milliseconds * 0.1f / 128f;
						if (this.fadeAlpha >= 1f)
						{
							Game1.warpFarmer("BusStop", 12, 11, false);
							if (Intro.roadNoise != null)
							{
								Intro.roadNoise.Stop(AudioStopOptions.Immediate);
							}
							Game1.exitActiveMenu();
							return true;
						}
					}
				}
				break;
			}
			return false;
		}

		public void doneCreatingCharacter()
		{
			this.characterCreateMenu = null;
			this.currentState = 1;
			Game1.changeMusicTrack("spring_day_ambient");
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.receiveLeftClick(x, y, true);
			}
			for (int i = this.balloons.Count - 1; i >= 0; i--)
			{
				if (new Rectangle((int)this.balloons[i].position.X * 4 + 16, (int)this.balloons[i].position.Y * 4 + 16, 32, 32).Contains(x, y))
				{
					this.balloons.RemoveAt(i);
					Game1.playSound("coin");
				}
			}
		}

		public void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.receiveRightClick(x, y, true);
			}
		}

		public void releaseLeftClick(int x, int y)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.releaseLeftClick(x, y);
			}
		}

		public void leftClickHeld(int x, int y)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.leftClickHeld(x, y);
			}
		}

		public void releaseRightClick(int x, int y)
		{
		}

		public void receiveKeyPress(Keys k)
		{
			if (k == Keys.Escape && this.currentState != 1)
			{
				if (!this.quit)
				{
					Game1.playSound("bigDeSelect");
				}
				this.quit = true;
			}
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		public void draw(SpriteBatch b)
		{
			switch (this.currentState)
			{
			case 0:
				break;
			case 1:
			{
				b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				b.GraphicsDevice.Clear(this.skyColor);
				int x = Game1.tileSize;
				int y = Game1.viewport.Height - Game1.tileSize;
				int width = 0;
				int height = Game1.tileSize;
				Utility.makeSafe(ref x, ref y, width, height);
				SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3689", new object[0]), x, y, 999, -1, 999, 1f, 1f, false, 0, "", -1);
				b.End();
				return;
			}
			case 2:
			case 3:
			case 4:
				this.drawRoadArea(b);
				break;
			default:
				return;
			}
		}

		public void drawChoosingCharacterArea(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.GraphicsDevice.Clear(this.skyColor);
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.draw(b);
			}
			b.End();
		}

		public void drawRoadArea(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, this.transformMatrix);
			b.GraphicsDevice.Clear(this.roadColor);
			b.Draw(Game1.staminaRect, new Rectangle(0, -this.screenHeight * 2, this.screenWidth, this.screenHeight * 3), this.skyColor);
			b.Draw(Game1.staminaRect, new Rectangle(0, this.screenHeight / 2 - 32, this.screenWidth, this.screenHeight * 4), this.roadColor);
			for (int i = 0; i < this.screenWidth / 112 + 2; i++)
			{
				if (this.sky[i] == 0)
				{
					b.Draw(this.texture, new Vector2(-this.skyPosition + (float)(i * 112), -16f), new Rectangle?(new Rectangle(128, 0, 112, 96)), Color.White);
				}
				else
				{
					b.Draw(this.texture, new Rectangle((int)(-(int)this.skyPosition) - 1 + i * 112, -16, 114, 96), new Rectangle?(new Rectangle(128, 0, 1, 96)), Color.White);
				}
			}
			for (int j = 0; j < 8; j++)
			{
				b.Draw(Game1.mouseCursors, new Vector2(-10f + -this.valleyPosition / 2f + (float)(j * 639), 70f), new Rectangle?(new Rectangle(0, 886, 639, 148)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.08f);
				b.Draw(Game1.mouseCursors, new Vector2(-this.valleyPosition + (float)(j * 639), 80f), new Rectangle?(new Rectangle(0, 737, 639, 120)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.08f);
			}
			for (int k = 0; k < this.road.Count; k++)
			{
				if (k % 3 == 0)
				{
					b.Draw(this.texture, new Vector2((float)(k * 16) - this.roadPosition, 160f), new Rectangle?(new Rectangle(0, 176, 48, 48)), Color.White);
					b.Draw(this.texture, new Vector2((float)(k * 16 + this.tileSize) - this.roadPosition, 272f), new Rectangle?(new Rectangle(0, 64, 16, 16)), Color.White);
				}
				b.Draw(this.texture, new Vector2((float)(k * 16) - this.roadPosition, 208f), new Rectangle?(new Rectangle(this.road[k] * 16, 240, 16, 16)), Color.White);
			}
			for (int l = 0; l < this.roadsideObjects.Count; l++)
			{
				switch (this.roadsideObjects[l])
				{
				case 0:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 96f), new Rectangle?(new Rectangle(48, 0, 48, 96)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 1:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 96f), new Rectangle?(new Rectangle(0, 0, 48, 64)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 16f, 160f), new Rectangle?(new Rectangle(16, 64, 16, 32)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 2:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 176f), new Rectangle?(new Rectangle(112, 144, 16, 16)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 3:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 176f), new Rectangle?(new Rectangle(112, 160, 16, 16)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 5:
					b.Draw(this.texture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 128f), new Rectangle?(new Rectangle(48, 176, 64, 64)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				}
			}
			for (int m = 0; m < this.roadsideFences.Count; m++)
			{
				if (this.roadsideFences[m] != -1)
				{
					if (this.roadsideFences[m] == 3)
					{
						b.Draw(this.roadsideTexture, new Vector2((float)(m * 16) - this.roadPosition, 176f), new Rectangle?(new Rectangle(144, 256, 16, 32)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					else
					{
						b.Draw(this.roadsideTexture, new Vector2((float)(m * 16) - this.roadPosition, 176f), new Rectangle?(new Rectangle(128 + this.roadsideFences[m] * 16, 224, 16, 32)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
			}
			if (!this.carPosition.Equals(Vector2.Zero))
			{
				b.Draw(this.texture, this.carPosition, new Rectangle?(new Rectangle(160, 112, 80, 64)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				b.Draw(this.texture, this.carPosition, new Rectangle?(new Rectangle(160, 176, 80, 64)), this.carColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			b.Draw(this.texture, this.busPosition, new Rectangle?(new Rectangle(0, 0, 128, 64)), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
			b.Draw(this.texture, this.busPosition + new Vector2(23.5f, 56.5f) * 1.5f, new Rectangle?(new Rectangle(21, 54, 5, 5)), Color.White, (float)((double)(this.roadPosition / 3f / 16f) * 3.1415926535897931 * 2.0), new Vector2(2.5f, 2.5f), 1.5f, SpriteEffects.None, 0f);
			b.Draw(this.texture, this.busPosition + new Vector2(87.5f, 56.5f) * 1.5f, new Rectangle?(new Rectangle(21, 54, 5, 5)), Color.White, (float)((double)((this.roadPosition + 4f) / 3f / 16f) * 3.1415926535897931 * 2.0), new Vector2(2.5f, 2.5f), 1.5f, SpriteEffects.None, 0f);
			if (!this.birdPosition.Equals(Vector2.Zero))
			{
				b.Draw(this.texture, this.birdPosition, new Rectangle?(new Rectangle(16 + this.birdFrame * 16, 64, 16, 16)), Color.White);
			}
			if (this.fadeAlpha > 0f)
			{
				b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, this.screenWidth + 2, this.screenHeight * 2), Color.Black * this.fadeAlpha);
			}
			b.End();
		}

		public void changeScreenSize()
		{
			this.screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width / 3;
			this.screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height / 3;
			this.createBeginningOfLevel();
		}

		public void unload()
		{
		}

		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		public string minigameId()
		{
			return null;
		}
	}
}
