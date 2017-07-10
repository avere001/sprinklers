using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Minigames
{
	public class FantasyBoardGame : IMinigame
	{
		public int borderSourceWidth = 138;

		public int borderSourceHeight = 74;

		public int slideSourceWidth = 128;

		public int slideSourceHeight = 64;

		private LocalizedContentManager content;

		private Texture2D slides;

		private Texture2D border;

		public int whichSlide;

		public int shakeTimer;

		public int endTimer;

		private string grade = "";

		public FantasyBoardGame()
		{
			this.content = Game1.content.CreateTemporary();
			this.slides = this.content.Load<Texture2D>("LooseSprites\\boardGame");
			this.border = this.content.Load<Texture2D>("LooseSprites\\boardGameBorder");
			Game1.globalFadeToClear(null, 0.02f);
		}

		public bool overrideFreeMouseMovement()
		{
			return false;
		}

		public bool tick(GameTime time)
		{
			if (this.shakeTimer > 0)
			{
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			Game1.currentLocation.currentEvent.checkForNextCommand(Game1.currentLocation, time);
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.update(time);
			}
			if (this.endTimer > 0)
			{
				this.endTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.endTimer <= 0 && this.whichSlide == -1)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.end), 0.02f);
				}
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.performHoverAction(Game1.getOldMouseX(), Game1.getOldMouseY());
			}
			return false;
		}

		public void end()
		{
			this.unload();
			Event expr_10 = Game1.currentLocation.currentEvent;
			int currentCommand = expr_10.CurrentCommand;
			expr_10.CurrentCommand = currentCommand + 1;
			Game1.currentMinigame = null;
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.receiveLeftClick(x, y, true);
			}
		}

		public void leftClickHeld(int x, int y)
		{
		}

		public void receiveRightClick(int x, int y, bool playSound = true)
		{
			Game1.pressActionButton(Keyboard.GetState(), Mouse.GetState(), GamePad.GetState(Game1.playerOneIndex));
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.receiveRightClick(x, y, true);
			}
		}

		public void releaseLeftClick(int x, int y)
		{
		}

		public void releaseRightClick(int x, int y)
		{
		}

		public void receiveKeyPress(Keys k)
		{
			if (Game1.isQuestion)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					Game1.currentQuestionChoice = Math.Max(Game1.currentQuestionChoice - 1, 0);
					Game1.playSound("toolSwap");
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					Game1.currentQuestionChoice = Math.Min(Game1.currentQuestionChoice + 1, Game1.questionChoices.Count - 1);
					Game1.playSound("toolSwap");
				}
			}
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (this.whichSlide >= 0)
			{
				Vector2 value = default(Vector2);
				if (this.shakeTimer > 0)
				{
					value = new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
				}
				b.Draw(this.border, value + new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.borderSourceWidth * Game1.pixelZoom / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - this.borderSourceHeight * Game1.pixelZoom / 2 - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, 0, this.borderSourceWidth, this.borderSourceHeight)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				b.Draw(this.slides, value + new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.slideSourceWidth * Game1.pixelZoom / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - this.slideSourceHeight * Game1.pixelZoom / 2 - Game1.tileSize * 2)), new Rectangle?(new Rectangle(this.whichSlide % 2 * this.slideSourceWidth, this.whichSlide / 2 * this.slideSourceHeight, this.slideSourceWidth, this.slideSourceHeight)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.01f);
			}
			else
			{
				string text = Game1.content.LoadString("Strings\\StringsFromCSFiles:FantasyBoardGame.cs.11980", new object[]
				{
					this.grade
				});
				float num = (float)Math.Sin((double)(this.endTimer / 1000)) * (float)(Game1.pixelZoom * 2);
				Game1.drawWithBorder(text, Game1.textColor, Color.Purple, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) - Game1.dialogueFont.MeasureString(text).X / 2f, num + (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2)));
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.draw(b);
			}
			b.End();
		}

		public void changeScreenSize()
		{
		}

		public void unload()
		{
			this.content.Unload();
		}

		public void afterFade()
		{
			this.whichSlide = -1;
			int num = 0;
			if (Game1.player.mailReceived.Contains("savedFriends"))
			{
				num++;
			}
			if (Game1.player.mailReceived.Contains("destroyedPods"))
			{
				num++;
			}
			if (Game1.player.mailReceived.Contains("killedSkeleton"))
			{
				num++;
			}
			switch (num)
			{
			case 0:
				this.grade = "D";
				break;
			case 1:
				this.grade = "C";
				break;
			case 2:
				this.grade = "B";
				break;
			case 3:
				this.grade = "A";
				break;
			}
			Game1.playSound("newArtifact");
			this.endTimer = 5500;
		}

		public void receiveEventPoke(int data)
		{
			if (data == -1)
			{
				this.shakeTimer = 1000;
				return;
			}
			if (data == -2)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFade), 0.02f);
				return;
			}
			this.whichSlide = data;
		}

		public string minigameId()
		{
			return "FantasyBoardGame";
		}
	}
}
