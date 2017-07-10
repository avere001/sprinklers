using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Minigames
{
	public class CalicoJack : IMinigame
	{
		public const int cardState_flipped = -1;

		public const int cardState_up = 0;

		public const int cardState_transitioning = 400;

		public const int bet = 100;

		public const int cardWidth = 96;

		public const int dealTime = 1000;

		public const int playingTo = 21;

		public const int passNumber = 18;

		public const int dealerTurnDelay = 1000;

		public List<int[]> playerCards;

		public List<int[]> dealerCards;

		private Random r;

		private int currentBet;

		private int startTimer;

		private int dealerTurnTimer = -1;

		private int bustTimer;

		private ClickableComponent hit;

		private ClickableComponent stand;

		private ClickableComponent doubleOrNothing;

		private ClickableComponent playAgain;

		private ClickableComponent quit;

		private ClickableComponent currentlySnappedComponent;

		private bool showingResultsScreen;

		private bool playerWon;

		private bool highStakes;

		private string endMessage = "";

		private string endTitle = "";

		private string coinBuffer;

		public CalicoJack(int toBet = -1, bool highStakes = false)
		{
			this.coinBuffer = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? "     " : ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh) ? "\u3000\u3000" : "  "));
			this.highStakes = highStakes;
			this.startTimer = 1000;
			this.playerCards = new List<int[]>();
			this.dealerCards = new List<int[]>();
			if (toBet == -1)
			{
				this.currentBet = (highStakes ? 1000 : 100);
			}
			else
			{
				this.currentBet = toBet;
			}
			Club.timesPlayedCalicoJack++;
			this.r = new Random(Club.timesPlayedCalicoJack + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
			this.hit = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11924", new object[0])), Game1.graphics.GraphicsDevice.Viewport.Height / 2 - Game1.tileSize, SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11924", new object[0]) + "  "), Game1.tileSize), "", " " + Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11924", new object[0]) + " ");
			this.stand = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11927", new object[0])), Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.tileSize / 2, SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11927", new object[0]) + "  "), Game1.tileSize), "", " " + Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11927", new object[0]) + " ");
			this.doubleOrNothing = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11930", new object[0])) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2, SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11930", new object[0])) + Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11930", new object[0]));
			this.playAgain = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11933", new object[0])) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.tileSize + Game1.tileSize / 4, SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11933", new object[0])) + Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11933", new object[0]));
			this.quit = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11936", new object[0])) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.tileSize + Game1.tileSize * 3 / 2, SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11936", new object[0])) + Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11936", new object[0]));
			if (Game1.options.SnappyMenus)
			{
				this.currentlySnappedComponent = this.hit;
				this.currentlySnappedComponent.snapMouseCursorToCenter();
			}
		}

		public bool overrideFreeMouseMovement()
		{
			return Game1.options.SnappyMenus;
		}

		public bool playButtonsActive()
		{
			return this.startTimer <= 0 && this.dealerTurnTimer < 0 && !this.showingResultsScreen;
		}

		public bool tick(GameTime time)
		{
			for (int i = 0; i < this.playerCards.Count; i++)
			{
				if (this.playerCards[i][1] > 0)
				{
					this.playerCards[i][1] -= time.ElapsedGameTime.Milliseconds;
					if (this.playerCards[i][1] <= 0)
					{
						this.playerCards[i][1] = 0;
					}
				}
			}
			for (int j = 0; j < this.dealerCards.Count; j++)
			{
				if (this.dealerCards[j][1] > 0)
				{
					this.dealerCards[j][1] -= time.ElapsedGameTime.Milliseconds;
					if (this.dealerCards[j][1] <= 0)
					{
						this.dealerCards[j][1] = 0;
					}
				}
			}
			if (this.startTimer > 0)
			{
				int num = this.startTimer;
				this.startTimer -= time.ElapsedGameTime.Milliseconds;
				if (num % 250 < this.startTimer % 250)
				{
					switch (num / 250)
					{
					case 1:
						this.playerCards.Add(new int[]
						{
							this.r.Next(1, 10),
							400
						});
						break;
					case 2:
						this.playerCards.Add(new int[]
						{
							this.r.Next(1, 12),
							400
						});
						break;
					case 3:
						this.dealerCards.Add(new int[]
						{
							this.r.Next(1, 10),
							400
						});
						break;
					case 4:
						this.dealerCards.Add(new int[]
						{
							this.r.Next(1, 12),
							-1
						});
						break;
					}
					Game1.playSound("shwip");
				}
			}
			else if (this.bustTimer > 0)
			{
				this.bustTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.bustTimer <= 0)
				{
					this.endGame();
				}
			}
			else if (this.dealerTurnTimer > 0 && !this.showingResultsScreen)
			{
				this.dealerTurnTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.dealerTurnTimer <= 0)
				{
					int num2 = 0;
					foreach (int[] current in this.dealerCards)
					{
						num2 += current[0];
					}
					int num3 = 0;
					foreach (int[] current2 in this.playerCards)
					{
						num3 += current2[0];
					}
					if (this.dealerCards[0][1] == -1)
					{
						this.dealerCards[0][1] = 400;
						Game1.playSound("shwip");
					}
					else if (num2 < 18 || (num2 < num3 && num3 <= 21))
					{
						this.dealerCards.Add(new int[]
						{
							this.r.Next(1, 10),
							400
						});
						num2 += this.dealerCards.Last<int[]>()[0];
						Game1.playSound("shwip");
						if (num2 > 21)
						{
							this.bustTimer = 2000;
						}
					}
					else
					{
						this.bustTimer = 50;
					}
					this.dealerTurnTimer = 1000;
				}
			}
			if (this.playButtonsActive())
			{
				this.hit.scale = (this.hit.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
				this.stand.scale = (this.stand.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
			}
			else if (this.showingResultsScreen)
			{
				this.doubleOrNothing.scale = (this.doubleOrNothing.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
				this.playAgain.scale = (this.playAgain.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
				this.quit.scale = (this.quit.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
			}
			return false;
		}

		public void endGame()
		{
			if (Game1.options.SnappyMenus)
			{
				this.currentlySnappedComponent = this.quit;
				this.currentlySnappedComponent.snapMouseCursorToCenter();
			}
			this.showingResultsScreen = true;
			int num = 0;
			foreach (int[] current in this.playerCards)
			{
				num += current[0];
			}
			if (num == 21)
			{
				Game1.playSound("reward");
				this.playerWon = true;
				this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11943", new object[0]);
				this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11944", new object[0]);
				Game1.player.clubCoins += this.currentBet;
				return;
			}
			if (num > 21)
			{
				Game1.playSound("fishEscape");
				this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11946", new object[0]);
				this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11947", new object[0]);
				Game1.player.clubCoins -= this.currentBet;
				return;
			}
			int num2 = 0;
			foreach (int[] current2 in this.dealerCards)
			{
				num2 += current2[0];
			}
			if (num2 > 21)
			{
				Game1.playSound("reward");
				this.playerWon = true;
				this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11943", new object[0]);
				this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11950", new object[0]);
				Game1.player.clubCoins += this.currentBet;
				return;
			}
			if (num == num2)
			{
				this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11951", new object[0]);
				this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11952", new object[0]);
				return;
			}
			if (num > num2)
			{
				Game1.playSound("reward");
				this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11943", new object[0]);
				this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11955", new object[]
				{
					21
				});
				Game1.player.clubCoins += this.currentBet;
				this.playerWon = true;
				return;
			}
			Game1.playSound("fishEscape");
			this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11946", new object[0]);
			this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11958", new object[]
			{
				21
			});
			Game1.player.clubCoins -= this.currentBet;
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.playButtonsActive() && this.bustTimer <= 0)
			{
				if (this.hit.bounds.Contains(x, y))
				{
					this.playerCards.Add(new int[]
					{
						this.r.Next(1, 10),
						400
					});
					Game1.playSound("shwip");
					int num = 0;
					foreach (int[] current in this.playerCards)
					{
						num += current[0];
					}
					if (num == 21)
					{
						this.bustTimer = 1000;
					}
					else if (num > 21)
					{
						this.bustTimer = 1000;
					}
				}
				if (this.stand.bounds.Contains(x, y))
				{
					this.dealerTurnTimer = 1000;
					Game1.playSound("coin");
					return;
				}
			}
			else if (this.showingResultsScreen)
			{
				if (this.playerWon && this.doubleOrNothing.containsPoint(x, y))
				{
					Game1.currentMinigame = new CalicoJack(this.currentBet * 2, this.highStakes);
					Game1.playSound("bigSelect");
				}
				if (Game1.player.clubCoins >= this.currentBet && this.playAgain.containsPoint(x, y))
				{
					Game1.currentMinigame = new CalicoJack(-1, this.highStakes);
					Game1.playSound("smallSelect");
				}
				if (this.quit.containsPoint(x, y))
				{
					Game1.currentMinigame = null;
					Game1.playSound("bigDeSelect");
				}
			}
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
			if (Game1.options.SnappyMenus)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					if (this.currentlySnappedComponent.Equals(this.stand))
					{
						this.currentlySnappedComponent = this.hit;
					}
					else if (this.currentlySnappedComponent.Equals(this.playAgain) && this.playerWon)
					{
						this.currentlySnappedComponent = this.doubleOrNothing;
					}
					else if (this.currentlySnappedComponent.Equals(this.quit) && Game1.player.clubCoins >= this.currentBet)
					{
						this.currentlySnappedComponent = this.playAgain;
					}
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					if (this.currentlySnappedComponent.Equals(this.hit))
					{
						this.currentlySnappedComponent = this.stand;
					}
					else if (this.currentlySnappedComponent.Equals(this.doubleOrNothing))
					{
						this.currentlySnappedComponent = this.playAgain;
					}
					else if (this.currentlySnappedComponent.Equals(this.playAgain))
					{
						this.currentlySnappedComponent = this.quit;
					}
				}
				if (this.currentlySnappedComponent != null)
				{
					this.currentlySnappedComponent.snapMouseCursorToCenter();
				}
			}
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), this.highStakes ? new Color(130, 0, 82) : Color.DarkGreen);
			if (this.showingResultsScreen)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, this.endMessage, Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.tileSize * 3 / 4, "", 1f, -1, 0, 0.88f, false);
				SpriteText.drawStringWithScrollCenteredAt(b, this.endTitle, Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.tileSize * 2, "", 1f, -1, 0, 0.88f, false);
				if (!this.endTitle.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11951", new object[0])))
				{
					SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11965", new object[]
					{
						(this.playerWon ? "" : "-") + this.currentBet + "   "
					}), Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.tileSize * 4, "", 1f, -1, 0, 0.88f, false);
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - Game1.tileSize / 2 + SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11965", new object[]
					{
						(this.playerWon ? "" : "-") + this.currentBet + "   "
					})) / 2), (float)(Game1.tileSize * 4 + Game1.pixelZoom)) + new Vector2((float)(Game1.pixelZoom * 2), 0f), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
				}
				if (this.playerWon)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.doubleOrNothing.bounds.X, this.doubleOrNothing.bounds.Y, this.doubleOrNothing.bounds.Width, this.doubleOrNothing.bounds.Height, Color.White, (float)Game1.pixelZoom * this.doubleOrNothing.scale, true);
					SpriteText.drawString(b, this.doubleOrNothing.label, this.doubleOrNothing.bounds.X + Game1.pixelZoom * 8, this.doubleOrNothing.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
				}
				if (Game1.player.clubCoins >= this.currentBet)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.playAgain.bounds.X, this.playAgain.bounds.Y, this.playAgain.bounds.Width, this.playAgain.bounds.Height, Color.White, (float)Game1.pixelZoom * this.playAgain.scale, true);
					SpriteText.drawString(b, this.playAgain.label, this.playAgain.bounds.X + Game1.pixelZoom * 8, this.playAgain.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
				}
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.quit.bounds.X, this.quit.bounds.Y, this.quit.bounds.Width, this.quit.bounds.Height, Color.White, (float)Game1.pixelZoom * this.quit.scale, true);
				SpriteText.drawString(b, this.quit.label, this.quit.bounds.X + Game1.pixelZoom * 8, this.quit.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
			}
			else
			{
				Vector2 vector = new Vector2((float)(Game1.tileSize * 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 5));
				int num = 0;
				foreach (int[] current in this.playerCards)
				{
					int num2 = 144;
					if (current[1] > 0)
					{
						num2 = (int)(Math.Abs((float)current[1] - 200f) / 200f * 144f);
					}
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, (current[1] > 200 || current[1] == -1) ? new Rectangle(399, 396, 15, 15) : new Rectangle(384, 396, 15, 15), (int)vector.X, (int)vector.Y + 72 - num2 / 2, 96, num2, Color.White, (float)Game1.pixelZoom, true);
					if (current[1] == 0)
					{
						SpriteText.drawStringHorizontallyCenteredAt(b, string.Concat(current[0]), (int)vector.X + 48 - Game1.tileSize / 8 + Game1.pixelZoom, (int)vector.Y + 72 - Game1.tileSize / 4, 999999, -1, 999999, 1f, 0.88f, false, -1);
					}
					vector.X += (float)(96 + Game1.tileSize / 4);
					if (current[1] == 0)
					{
						num += current[0];
					}
				}
				SpriteText.drawStringWithScrollBackground(b, Game1.player.name + ": " + num, Game1.tileSize * 2 + Game1.tileSize / 2, (int)vector.Y + 144 + Game1.tileSize / 2, "", 1f, -1);
				vector.X = (float)(Game1.tileSize * 2);
				vector.Y = (float)(Game1.tileSize * 2);
				num = 0;
				foreach (int[] current2 in this.dealerCards)
				{
					int num3 = 144;
					if (current2[1] > 0)
					{
						num3 = (int)(Math.Abs((float)current2[1] - 200f) / 200f * 144f);
					}
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, (current2[1] > 200 || current2[1] == -1) ? new Rectangle(399, 396, 15, 15) : new Rectangle(384, 396, 15, 15), (int)vector.X, (int)vector.Y + 72 - num3 / 2, 96, num3, Color.White, (float)Game1.pixelZoom, true);
					if (current2[1] == 0)
					{
						SpriteText.drawStringHorizontallyCenteredAt(b, string.Concat(current2[0]), (int)vector.X + 48 - Game1.tileSize / 8 + Game1.pixelZoom, (int)vector.Y + 72 - Game1.tileSize / 4, 999999, -1, 999999, 1f, 0.88f, false, -1);
					}
					vector.X += (float)(96 + Game1.tileSize / 4);
					if (current2[1] == 0)
					{
						num += current2[0];
					}
					else if (current2[1] == -1)
					{
						num = -99999;
					}
				}
				SpriteText.drawStringWithScrollBackground(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11970", new object[]
				{
					(num > 0) ? string.Concat(num) : "?"
				}), Game1.tileSize * 2 + Game1.tileSize / 2, Game1.tileSize / 2, "", 1f, -1);
				SpriteText.drawStringWithScrollBackground(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11972", new object[]
				{
					this.currentBet + this.coinBuffer
				}), Game1.tileSize * 3, Game1.graphics.GraphicsDevice.Viewport.Height / 2, "", 1f, -1);
				Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 3 + Game1.pixelZoom * 3 + SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11972", new object[]
				{
					this.currentBet
				}))), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
				if (this.playButtonsActive())
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.hit.bounds.X, this.hit.bounds.Y, this.hit.bounds.Width, this.hit.bounds.Height, Color.White, (float)Game1.pixelZoom * this.hit.scale, true);
					SpriteText.drawString(b, this.hit.label, this.hit.bounds.X + Game1.pixelZoom * 2, this.hit.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.stand.bounds.X, this.stand.bounds.Y, this.stand.bounds.Width, this.stand.bounds.Height, Color.White, (float)Game1.pixelZoom * this.stand.scale, true);
					SpriteText.drawString(b, this.stand.label, this.stand.bounds.X + Game1.pixelZoom * 2, this.stand.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
				}
			}
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
			b.End();
		}

		public void changeScreenSize()
		{
		}

		public void unload()
		{
		}

		public void receiveEventPoke(int data)
		{
		}

		public string minigameId()
		{
			return "CalicoJack";
		}
	}
}
