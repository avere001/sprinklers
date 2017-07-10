using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
	public class StrengthGame : IClickableMenu
	{
		private float power;

		private float changeSpeed;

		private float endTimer;

		private float transparency = 1f;

		private Color barColor;

		private bool victorySound;

		private bool clicked;

		private bool showedResult;

		public StrengthGame() : base(31 * Game1.tileSize + 6 * Game1.pixelZoom, 56 * Game1.tileSize + 10 * Game1.pixelZoom, 5 * Game1.pixelZoom, 34 * Game1.pixelZoom, false)
		{
			this.power = 0f;
			this.changeSpeed = (float)(3 + Game1.random.Next(2));
			this.barColor = Color.Red;
			Game1.playSound("cowboy_monsterhit");
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.clicked)
			{
				Game1.player.faceDirection(1);
				Game1.player.CurrentToolIndex = 107;
				Game1.player.FarmerSprite.animateOnce(168, 80f, 8);
				Game1.player.toolOverrideFunction = new AnimatedSprite.endOfAnimationBehavior(this.afterSwingAnimation);
				Game1.player.FarmerSprite.ignoreDefaultActionThisTime = false;
				this.clicked = true;
			}
			if (this.showedResult && Game1.dialogueTyping)
			{
				Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
			}
			if (this.showedResult && !Game1.dialogueTyping)
			{
				Game1.player.toolOverrideFunction = null;
				Game1.exitActiveMenu();
				Game1.afterDialogues = null;
				Game1.pressActionButton(Game1.oldKBState, Game1.oldMouseState, Game1.oldPadState);
			}
		}

		public void afterSwingAnimation(Farmer who)
		{
			if (!Game1.isFestival())
			{
				who.toolOverrideFunction = null;
				return;
			}
			this.changeSpeed = 0f;
			Game1.playSound("hammer");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2(30f, 56f) * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
			if (this.power >= 99f)
			{
				this.endTimer = 2000f;
				return;
			}
			this.endTimer = 1000f;
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.changeSpeed == 0f)
			{
				this.endTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.power >= 99f)
				{
					if (this.endTimer < 1500f)
					{
						if (!this.victorySound)
						{
							this.victorySound = true;
							Game1.playSound("getNewSpecialItem");
							this.barColor = Color.Orange;
						}
						if (!this.showedResult && Game1.random.NextDouble() < 0.08)
						{
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(10 + Game1.random.Next(2), new Vector2(31f, 55f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize, Game1.tileSize), (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize)), Color.Yellow, 8, false, 100f, 0, -1, -1f, -1, 0)
							{
								layerDepth = 1f
							});
						}
					}
				}
				else
				{
					this.transparency = Math.Max(0f, this.transparency - 0.02f);
				}
				if (this.endTimer <= 0f && !this.showedResult)
				{
					this.showedResult = true;
					if (this.power >= 99f)
					{
						Game1.player.festivalScore++;
						Game1.playSound("purchase");
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11660", new object[0]));
					}
					else if (this.power >= 2f)
					{
						string text = "";
						switch ((int)this.power)
						{
						case 2:
						case 3:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11701", new object[0]);
							break;
						case 4:
						case 5:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11700", new object[0]);
							break;
						case 6:
						case 7:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11699", new object[0]);
							break;
						case 8:
						case 9:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11698", new object[0]);
							break;
						case 10:
						case 11:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11697", new object[0]);
							break;
						case 12:
						case 13:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11696", new object[0]);
							break;
						case 14:
						case 15:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11695", new object[0]);
							break;
						case 16:
						case 17:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11694", new object[0]);
							break;
						case 18:
						case 19:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11693", new object[0]);
							break;
						case 20:
						case 21:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11692", new object[0]);
							break;
						case 22:
						case 23:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11691", new object[0]);
							break;
						case 24:
						case 25:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11690", new object[0]);
							break;
						case 26:
						case 27:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11689", new object[0]);
							break;
						case 28:
						case 29:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11688", new object[0]);
							break;
						case 30:
						case 31:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11687", new object[0]);
							break;
						case 32:
						case 33:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11686", new object[0]);
							break;
						case 34:
						case 35:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11685", new object[0]);
							break;
						case 36:
						case 37:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11684", new object[0]);
							break;
						case 38:
						case 39:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11683", new object[0]);
							break;
						case 40:
						case 41:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11682", new object[0]);
							break;
						case 42:
						case 43:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11681", new object[0]);
							break;
						case 44:
						case 45:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11680", new object[0]);
							break;
						case 46:
						case 47:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11679", new object[0]);
							break;
						case 48:
						case 49:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11678", new object[0]);
							break;
						case 50:
						case 51:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11677", new object[0]);
							break;
						case 52:
						case 53:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11676", new object[0]);
							break;
						case 54:
						case 55:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11675", new object[0]);
							break;
						case 56:
						case 57:
						case 58:
						case 59:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11674", new object[0]);
							break;
						case 60:
						case 61:
						case 62:
						case 63:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11673", new object[0]);
							break;
						case 64:
						case 65:
						case 66:
						case 67:
						case 68:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11672", new object[0]);
							break;
						case 69:
						case 70:
						case 71:
						case 72:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11671", new object[0]);
							break;
						case 73:
						case 74:
						case 75:
						case 76:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11670", new object[0]);
							break;
						case 77:
						case 78:
						case 79:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11669", new object[0]);
							break;
						case 80:
						case 81:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11668", new object[0]);
							break;
						case 82:
						case 83:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11667", new object[0]);
							break;
						case 84:
						case 85:
						case 86:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11666", new object[0]);
							break;
						case 87:
						case 89:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11665", new object[0]);
							break;
						case 88:
						case 90:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11664", new object[0]);
							break;
						case 91:
						case 92:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11663", new object[0]);
							break;
						case 93:
						case 94:
						case 95:
						case 96:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11662", new object[0]);
							break;
						case 97:
						case 98:
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11661", new object[0]);
							break;
						}
						Game1.playSound("dwop");
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11703", new object[]
						{
							text
						}));
					}
					else
					{
						Game1.player.festivalScore++;
						Game1.playSound("purchase");
						Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11705", new object[0])));
					}
					Game1.afterDialogues = new Game1.afterFadeFunction(base.exitThisMenuNoSound);
					return;
				}
			}
			else
			{
				this.power += this.changeSpeed;
				if (this.power > 100f)
				{
					this.power = 100f;
					this.changeSpeed = -this.changeSpeed;
					return;
				}
				if (this.power < 0f)
				{
					this.power = 0f;
					this.changeSpeed = -this.changeSpeed;
				}
			}
		}

		public override void performHoverAction(int x, int y)
		{
		}

		public override void draw(SpriteBatch b)
		{
			if (!Game1.dialogueUp)
			{
				b.Draw(Game1.staminaRect, Game1.GlobalToLocal(Game1.viewport, new Rectangle(this.xPositionOnScreen, (int)((float)this.yPositionOnScreen - this.power / 100f * (float)this.height), this.width, (int)(this.power / 100f * (float)this.height))), new Rectangle?(Game1.staminaRect.Bounds), this.barColor * this.transparency, 0f, Vector2.Zero, SpriteEffects.None, 1E-05f);
			}
			if (Game1.player.FarmerSprite.isOnToolAnimation())
			{
				Game1.drawTool(Game1.player, Game1.player.CurrentToolIndex);
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}
	}
}
