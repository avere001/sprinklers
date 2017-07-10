using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using xTile;

namespace StardewValley.Minigames
{
	public class GrandpaStory : IMinigame
	{
		public const int sceneWidth = 1294;

		public const int sceneHeight = 730;

		public const int scene_beforeGrandpa = 0;

		public const int scene_grandpaSpeech = 1;

		public const int scene_fadeOutFromGrandpa = 2;

		public const int scene_timePass = 3;

		public const int scene_jojaCorpOverhead = 4;

		public const int scene_jojaCorpPan = 5;

		public const int scene_desk = 6;

		private LocalizedContentManager content;

		private Texture2D texture;

		private float foregroundFade;

		private float backgroundFade;

		private float foregroundFadeChange;

		private float backgroundFadeChange;

		private float panX;

		private float letterScale = 0.5f;

		private float letterDy;

		private float letterDyDy;

		private int scene;

		private int totalMilliseconds;

		private int grandpaSpeechTimer;

		private int parallaxPan;

		private int letterOpenTimer;

		private bool drawGrandpa;

		private bool letterReceived;

		private bool mouseActive;

		private bool clickedLetter;

		private bool quit;

		private bool fadingToQuit;

		private Queue<string> grandpaSpeech;

		private Vector2 letterPosition = new Vector2(477f, 345f);

		private LetterViewerMenu letterView;

		public GrandpaStory()
		{
			Game1.changeMusicTrack("none");
			this.content = Game1.content.CreateTemporary();
			this.texture = this.content.Load<Texture2D>("Minigames\\jojacorps");
			this.backgroundFadeChange = 0.0003f;
			this.grandpaSpeech = new Queue<string>();
			this.grandpaSpeech.Enqueue(Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12026", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12028", new object[0]));
			this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12029", new object[0]));
			this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12030", new object[0]));
			this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12031", new object[0]));
			this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12034", new object[0]));
			this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12035", new object[0]));
			this.grandpaSpeech.Enqueue(Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12036", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12038", new object[0]));
			this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12040", new object[0]));
			Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3000f, 376f);
			Game1.viewport.X = 0;
			Game1.viewport.Y = 0;
			Map expr_229 = this.content.Load<Map>("Maps\\FarmHouse");
			expr_229.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.currentLocation = new FarmHouse(expr_229, "FarmHouse");
			Game1.player.currentLocation = Game1.currentLocation;
		}

		public bool tick(GameTime time)
		{
			if (this.quit)
			{
				this.unload();
				Game1.currentMinigame = new Intro();
				return false;
			}
			if (this.letterView != null)
			{
				this.letterView.update(time);
			}
			this.totalMilliseconds += time.ElapsedGameTime.Milliseconds;
			this.totalMilliseconds %= 9000000;
			this.backgroundFade += this.backgroundFadeChange * (float)time.ElapsedGameTime.Milliseconds;
			this.backgroundFade = Math.Max(0f, Math.Min(1f, this.backgroundFade));
			this.foregroundFade += this.foregroundFadeChange * (float)time.ElapsedGameTime.Milliseconds;
			this.foregroundFade = Math.Max(0f, Math.Min(1f, this.foregroundFade));
			int num = this.grandpaSpeechTimer;
			if (this.foregroundFade >= 1f && this.fadingToQuit)
			{
				this.unload();
				Game1.currentMinigame = new Intro();
				return false;
			}
			switch (this.scene)
			{
			case 0:
				if (this.backgroundFade == 1f)
				{
					if (!this.drawGrandpa)
					{
						this.foregroundFade = 1f;
						this.foregroundFadeChange = -0.0005f;
						this.drawGrandpa = true;
					}
					if (this.foregroundFade == 0f)
					{
						this.scene = 1;
						Game1.changeMusicTrack("grandpas_theme");
					}
				}
				break;
			case 1:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer >= 60000)
				{
					this.foregroundFadeChange = 0.0005f;
				}
				if (this.foregroundFade >= 1f)
				{
					this.drawGrandpa = false;
					this.scene = 3;
					this.grandpaSpeechTimer = 0;
					this.foregroundFade = 0f;
					this.foregroundFadeChange = 0f;
				}
				if (num % 10000 > this.grandpaSpeechTimer % 10000 && this.grandpaSpeech.Count > 0)
				{
					this.grandpaSpeech.Dequeue();
				}
				if (num < 25000 && this.grandpaSpeechTimer > 25000 && this.grandpaSpeech.Count > 0)
				{
					this.grandpaSpeech.Dequeue();
				}
				if (num < 17000 && this.grandpaSpeechTimer >= 17000)
				{
					Game1.playSound("newRecipe");
					this.letterReceived = true;
					this.letterDy = -0.6f;
					this.letterDyDy = 0.001f;
				}
				if (this.letterReceived && this.letterPosition.Y <= (float)Game1.graphics.GraphicsDevice.Viewport.Height)
				{
					this.letterDy += this.letterDyDy * (float)time.ElapsedGameTime.Milliseconds;
					this.letterPosition.Y = this.letterPosition.Y + this.letterDy * (float)time.ElapsedGameTime.Milliseconds;
					this.letterPosition.X = this.letterPosition.X + 0.01f * (float)time.ElapsedGameTime.Milliseconds;
					this.letterScale += 0.00125f * (float)time.ElapsedGameTime.Milliseconds;
					if (this.letterPosition.Y > (float)Game1.graphics.GraphicsDevice.Viewport.Height)
					{
						Game1.playSound("coin");
					}
				}
				break;
			case 3:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer > 2600 && num <= 2600)
				{
					Game1.changeMusicTrack("jojaOfficeSoundscape");
				}
				else if (this.grandpaSpeechTimer > 4000)
				{
					this.grandpaSpeechTimer = 0;
					this.scene = 4;
				}
				break;
			case 4:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer >= 9000)
				{
					this.grandpaSpeechTimer = 0;
					this.scene = 5;
					Game1.player.faceDirection(1);
					Game1.player.currentEyes = 1;
				}
				if (this.grandpaSpeechTimer >= 7000)
				{
					Game1.viewport.X = 0;
					Game1.viewport.Y = 0;
					this.panX -= 0.2f * (float)time.ElapsedGameTime.Milliseconds;
					Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
				}
				break;
			case 5:
				if (this.panX > (float)(-1200 * Game1.pixelZoom + Math.Max(400 * Game1.pixelZoom, Game1.graphics.GraphicsDevice.Viewport.Width)))
				{
					Game1.viewport.X = 0;
					Game1.viewport.Y = 0;
					this.panX -= 0.2f * (float)time.ElapsedGameTime.Milliseconds;
					Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
				}
				else
				{
					this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
					if (num < 2000 && this.grandpaSpeechTimer >= 2000)
					{
						Game1.player.currentEyes = 4;
					}
					if (num < 3000 && this.grandpaSpeechTimer >= 3000)
					{
						Game1.player.currentEyes = 1;
						Game1.player.jitterStrength = 1f;
					}
					if (num < 3500 && this.grandpaSpeechTimer >= 3500)
					{
						Game1.player.stopJittering();
					}
					if (num < 4000 && this.grandpaSpeechTimer >= 4000)
					{
						Game1.player.currentEyes = 1;
						Game1.player.jitterStrength = 1f;
					}
					if (num < 4500 && this.grandpaSpeechTimer >= 4500)
					{
						Game1.player.stopJittering();
						Game1.player.doEmote(28);
					}
					if (num < 7000 && this.grandpaSpeechTimer >= 7000)
					{
						Game1.player.currentEyes = 4;
					}
					if (num < 8000 && this.grandpaSpeechTimer >= 8000)
					{
						Game1.player.showFrame(33, false);
					}
					if (this.grandpaSpeechTimer >= 10000)
					{
						this.scene = 6;
						this.grandpaSpeechTimer = 0;
					}
					Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
				}
				break;
			case 6:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer >= 2000)
				{
					this.parallaxPan += (int)Math.Ceiling(0.1 * (double)time.ElapsedGameTime.Milliseconds);
					if (this.parallaxPan >= 107)
					{
						this.parallaxPan = 107;
					}
				}
				if (num < 3500 && this.grandpaSpeechTimer >= 3500)
				{
					Game1.changeMusicTrack("none");
				}
				if (num < 5000 && this.grandpaSpeechTimer >= 5000)
				{
					Game1.playSound("doorCreak");
				}
				if (num < 6000 && this.grandpaSpeechTimer >= 6000)
				{
					this.mouseActive = true;
					Point center = this.clickableGrandpaLetterRect().Center;
					Mouse.SetPosition(center.X, center.Y);
					Game1.lastCursorMotionWasMouse = false;
				}
				if (this.clickedLetter)
				{
					this.letterOpenTimer += time.ElapsedGameTime.Milliseconds;
				}
				break;
			}
			Game1.player.updateEmote(time);
			if (Game1.player.jitterStrength > 0f)
			{
				Game1.player.jitter = new Vector2((float)Game1.random.Next(-(int)(Game1.player.jitterStrength * 100f), (int)((Game1.player.jitterStrength + 1f) * 100f)) / 100f, (float)Game1.random.Next(-(int)(Game1.player.jitterStrength * 100f), (int)((Game1.player.jitterStrength + 1f) * 100f)) / 100f);
			}
			return false;
		}

		public void afterFade()
		{
		}

		private Rectangle clickableGrandpaLetterRect()
		{
			return new Rectangle((int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).X + (286 - this.parallaxPan) * 4, (int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 218 + Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8)), 524, 344);
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.clickedLetter && this.mouseActive && (this.clickableGrandpaLetterRect().Contains(x, y) || Game1.options.SnappyMenus))
			{
				this.clickedLetter = true;
				Game1.playSound("newRecipe");
				Game1.changeMusicTrack("musicboxsong");
				this.letterView = new LetterViewerMenu(Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12051", new object[]
				{
					Game1.player.name,
					Game1.player.farmName
				}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12055", new object[]
				{
					Game1.player.name,
					Game1.player.farmName
				}));
				this.letterView.exitFunction = new IClickableMenu.onExit(this.onLetterExit);
			}
			if (this.letterView != null)
			{
				this.letterView.receiveLeftClick(x, y, true);
			}
		}

		public void onLetterExit()
		{
			this.mouseActive = false;
			this.foregroundFadeChange = 0.0003f;
			this.fadingToQuit = true;
			if (this.letterView != null)
			{
				this.letterView.unload();
				this.letterView = null;
			}
			Game1.playSound("newRecipe");
		}

		public void leftClickHeld(int x, int y)
		{
		}

		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public void releaseLeftClick(int x, int y)
		{
		}

		public void releaseRightClick(int x, int y)
		{
		}

		public void receiveKeyPress(Keys k)
		{
			if (k == Keys.Escape || Game1.options.doesInputListContain(Game1.options.menuButton, k))
			{
				if (!this.quit && !this.fadingToQuit)
				{
					Game1.playSound("bigDeSelect");
				}
				if (this.letterView != null)
				{
					this.letterView.unload();
					this.letterView = null;
				}
				this.quit = true;
				return;
			}
			if (this.letterView != null)
			{
				this.letterView.receiveKeyPress(k);
				if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.RightTrigger))
				{
					this.letterView.receiveGamePadButton(Buttons.RightTrigger);
				}
				if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftTrigger) && Game1.oldPadState.IsButtonUp(Buttons.LeftTrigger))
				{
					this.letterView.receiveGamePadButton(Buttons.LeftTrigger);
				}
			}
		}

		public bool overrideFreeMouseMovement()
		{
			return Game1.options.SnappyMenus;
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Color(64, 136, 248));
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Black * this.backgroundFade);
			if (this.drawGrandpa)
			{
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(427, (this.totalMilliseconds % 300 < 150) ? 240 : 0, 427, 240)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(317f, 74f) * 3f, new Rectangle?(new Rectangle(427 + 74 * (this.totalMilliseconds % 400 / 100), 480, 74, 42)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(320f, 75f) * 3f, new Rectangle?(new Rectangle(427, 522, 70, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				if (this.grandpaSpeechTimer > 8000 && this.grandpaSpeechTimer % 10000 < 5000)
				{
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(189f, 69f) * 3f, new Rectangle?(new Rectangle(497 + 18 * (this.totalMilliseconds % 400 / 200), 523, 18, 18)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				}
				if (this.grandpaSpeech.Count > 0 && this.grandpaSpeechTimer > 3000)
				{
					b.DrawString(Game1.dialogueFont, this.grandpaSpeech.Peek(), new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) - Game1.dialogueFont.MeasureString(this.grandpaSpeech.Peek()).X / 2f - 3f, (float)((int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 669) + 3f), Color.White * 0.25f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
					b.DrawString(Game1.dialogueFont, this.grandpaSpeech.Peek(), new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) - Game1.dialogueFont.MeasureString(this.grandpaSpeech.Peek()).X / 2f, (float)((int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 669)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				if (this.letterReceived)
				{
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(157f, 113f) * 3f, new Rectangle?(new Rectangle(463, 556, 37, 17)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					if (this.grandpaSpeechTimer > 8000 && this.grandpaSpeechTimer % 10000 > 7000 && this.grandpaSpeechTimer % 10000 < 9000 && this.totalMilliseconds % 600 < 300)
					{
						b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(157f, 113f) * 3f, new Rectangle?(new Rectangle(500, 556, 37, 17)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					}
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + this.letterPosition, new Rectangle?(new Rectangle(729, 524, 131, 63)), Color.White, 0f, Vector2.Zero, this.letterScale, SpriteEffects.None, 1f);
				}
			}
			else if (this.scene == 3)
			{
				SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12059", new object[0]), (int)Utility.getTopLeftPositionForCenteringOnScreen(0, 0, -200, 0).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(0, 0, 0, -50).Y, 999, -1, 999, 1f, 1f, false, -1, "", 4);
			}
			else if (this.scene == 4)
			{
				float scale = 1f - ((float)this.grandpaSpeechTimer - 7000f) / 2000f;
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(0, 0, 427, 240)), Color.White * scale, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(22f, 211f) * 3f, new Rectangle?(new Rectangle(264 + this.totalMilliseconds % 500 / 250 * 19, 581, 19, 17)), Color.White * scale, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(332f, 215f) * 3f, new Rectangle?(new Rectangle(305 + this.totalMilliseconds % 600 / 200 * 12, 581, 12, 12)), Color.White * scale, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(414f, 211f) * 3f, new Rectangle?(new Rectangle(460 + this.totalMilliseconds % 400 / 200 * 13, 581, 13, 17)), Color.White * scale, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(189f, 81f) * 3f, new Rectangle?(new Rectangle(426 + this.totalMilliseconds % 800 / 400 * 16, 581, 16, 16)), Color.White * scale, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
			}
			if ((this.scene == 4 && this.grandpaSpeechTimer >= 5000) || this.scene == 5)
			{
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)), new Rectangle?(new Rectangle(0, 600, 1200, 180)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(1080f, 524f), new Rectangle?(new Rectangle(350 + this.totalMilliseconds % 800 / 400 * 14, 581, 14, 9)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(1564f, 520f), new Rectangle?(new Rectangle(383 + this.totalMilliseconds % 400 / 200 * 9, 581, 9, 7)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(2632f, 520f), new Rectangle?(new Rectangle(403 + this.totalMilliseconds % 600 / 300 * 8, 582, 8, 8)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(2604f, 504f), new Rectangle?(new Rectangle(364 + this.totalMilliseconds % 1100 / 100 * 5, 594, 5, 3)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3116f, 492f), new Rectangle?(new Rectangle(343 + this.totalMilliseconds % 3000 / 1000 * 6, 593, 6, 5)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				if (this.scene == 5)
				{
					Game1.player.draw(b);
				}
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3580f, 540f), new Rectangle?(new Rectangle(895, 735, 29, 36)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
			}
			if (this.scene == 6)
			{
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float)(261 - this.parallaxPan), 145f) * 4f, new Rectangle?(new Rectangle(550, 540, 56 + this.parallaxPan, 35)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float)(261 - this.parallaxPan), 4f + (float)Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8))) * 4f, new Rectangle?(new Rectangle(264, 434, 56 + this.parallaxPan, 141)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				if (this.grandpaSpeechTimer > 3000)
				{
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float)(286 - this.parallaxPan), 32f + (float)Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8)) + Math.Min(30f, (float)this.letterOpenTimer / 4f)) * 4f, new Rectangle?(new Rectangle(729 + Math.Min(2, this.letterOpenTimer / 200) * 131, 508, 131, 79)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				}
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(this.parallaxPan, 240, 320, 180)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(187f - (float)this.parallaxPan * 2.5f, 8f) * 4f, new Rectangle?(new Rectangle(20, 428, 232, 172)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
			}
			if (this.letterView != null)
			{
				this.letterView.draw(b);
			}
			if (this.mouseActive)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), this.fadingToQuit ? (new Color(64, 136, 248) * this.foregroundFade) : (Color.Black * this.foregroundFade));
			b.End();
		}

		public void changeScreenSize()
		{
			Game1.viewport.X = 0;
			Game1.viewport.Y = 0;
		}

		public void unload()
		{
			this.content.Unload();
			this.content = null;
		}

		public void receiveEventPoke(int data)
		{
		}

		public string minigameId()
		{
			return null;
		}
	}
}
