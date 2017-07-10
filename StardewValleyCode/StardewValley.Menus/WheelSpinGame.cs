using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;

namespace StardewValley.Menus
{
	public class WheelSpinGame : IClickableMenu
	{
		public new const int width = 640;

		public new const int height = 448;

		public double arrowRotation;

		public double arrowRotationVelocity;

		public double arrowRotationDeceleration;

		private int timerBeforeStart;

		private int wager;

		private SparklingText resultText;

		private bool doneSpinning;

		public WheelSpinGame(int wager) : base(Game1.viewport.Width / 2 - 320, Game1.viewport.Height / 2 - 224, 640, 448, false)
		{
			this.timerBeforeStart = 1000;
			this.arrowRotationVelocity = 0.19634954084936207;
			this.arrowRotationVelocity += (double)Game1.random.Next(0, 15) * 3.1415926535897931 / 256.0;
			this.arrowRotationDeceleration = -0.00062831853071795862;
			if (Game1.random.NextDouble() < 0.5)
			{
				this.arrowRotationVelocity += 0.049087385212340517;
			}
			this.wager = wager;
			Game1.player.Halt();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.timerBeforeStart <= 0)
			{
				double num = this.arrowRotationVelocity;
				this.arrowRotationVelocity += this.arrowRotationDeceleration;
				if (this.arrowRotationVelocity <= 0.039269908169872414 && num > 0.039269908169872414)
				{
					bool specialEventVariable = Game1.currentLocation.currentEvent.specialEventVariable2;
					if (this.arrowRotation > 1.5707963267948966 && this.arrowRotation <= 4.3196898986859651 && Game1.random.NextDouble() < (double)((float)Game1.player.LuckLevel / 15f))
					{
						if (specialEventVariable)
						{
							this.arrowRotationVelocity = 0.065449846949787352;
							Game1.playSound("dwop");
						}
					}
					else if ((this.arrowRotation + 3.1415926535897931) % 6.2831853071795862 <= 4.3196898986859651 && !specialEventVariable && Game1.random.NextDouble() < (double)((float)Game1.player.LuckLevel / 20f))
					{
						this.arrowRotationVelocity = 0.065449846949787352;
						Game1.playSound("dwop");
					}
				}
				if (this.arrowRotationVelocity <= 0.0 && !this.doneSpinning)
				{
					this.doneSpinning = true;
					this.arrowRotationDeceleration = 0.0;
					this.arrowRotationVelocity = 0.0;
					bool specialEventVariable2 = Game1.currentLocation.currentEvent.specialEventVariable2;
					bool flag = false;
					if (this.arrowRotation > 1.5707963267948966 && this.arrowRotation <= 4.71238898038469)
					{
						if (!specialEventVariable2)
						{
							flag = true;
						}
					}
					else if (specialEventVariable2)
					{
						flag = true;
					}
					if (flag)
					{
						Game1.playSound("reward");
						this.resultText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:WheelSpinGame.cs.11829", new object[0]), Color.Lime, Color.White, false, 0.1, 2500, -1, 500);
						Game1.player.festivalScore += this.wager;
					}
					else
					{
						this.resultText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:WheelSpinGame.cs.11830", new object[0]), Color.Red, Color.Transparent, false, 0.1, 2500, -1, 500);
						Game1.playSound("fishEscape");
						Game1.player.festivalScore -= this.wager;
					}
				}
				double arg_292_0 = this.arrowRotation;
				this.arrowRotation += this.arrowRotationVelocity;
				if (arg_292_0 % 1.5707963267948966 > this.arrowRotation % 1.5707963267948966)
				{
					Game1.playSound("Cowboy_gunshot");
				}
				this.arrowRotation %= 6.2831853071795862;
			}
			else
			{
				this.timerBeforeStart -= time.ElapsedGameTime.Milliseconds;
				if (this.timerBeforeStart <= 0)
				{
					Game1.playSound("cowboy_monsterhit");
				}
			}
			if (this.resultText != null && this.resultText.update(time))
			{
				this.resultText = null;
			}
			if (this.doneSpinning && this.resultText == null)
			{
				Game1.exitActiveMenu();
				Game1.player.canMove = true;
			}
		}

		public override void performHoverAction(int x, int y)
		{
		}

		public override void receiveKeyPress(Keys key)
		{
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			b.Draw(Game1.mouseCursors, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(new Rectangle(128, 1184, 160, 112)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.95f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 320), (float)(this.yPositionOnScreen + 224 + Game1.pixelZoom)), new Rectangle?(new Rectangle(120, 1234, 8, 16)), Color.White, (float)this.arrowRotation, new Vector2(4f, 15f), (float)Game1.pixelZoom, SpriteEffects.None, 0.96f);
			if (this.resultText != null)
			{
				this.resultText.draw(b, new Vector2((float)(this.xPositionOnScreen + 320) - this.resultText.textWidth, (float)(this.yPositionOnScreen - Game1.tileSize)));
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}
	}
}
