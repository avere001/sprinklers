using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace StardewValley.Minigames
{
	public class Slots : IMinigame
	{
		public const float slotTurnRate = 0.008f;

		public const int numberOfIcons = 8;

		public const int defaultBet = 10;

		private string coinBuffer;

		private List<float> slots;

		private List<float> slotResults;

		private ClickableComponent spinButton10;

		private ClickableComponent spinButton100;

		private ClickableComponent doneButton;

		private Random r;

		private bool spinning;

		private bool showResult;

		private float payoutModifier;

		private int currentBet;

		private int spinsCount;

		private int slotsFinished;

		private int endTimer;

		public ClickableComponent currentlySnappedComponent;

		public Slots(int toBet = -1, bool highStakes = false)
		{
			this.coinBuffer = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? "     " : ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh) ? "\u3000\u3000" : "  "));
			this.currentBet = toBet;
			if (this.currentBet == -1)
			{
				this.currentBet = 10;
			}
			this.slots = new List<float>();
			this.slots.Add(0f);
			this.slots.Add(0f);
			this.slots.Add(0f);
			this.slotResults = new List<float>();
			this.slotResults.Add(0f);
			this.slotResults.Add(0f);
			this.slotResults.Add(0f);
			Game1.playSound("newArtifact");
			this.r = new Random(Club.timesPlayedSlots + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
			this.setSlotResults(this.slots);
			Vector2 topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(26 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize / 2);
			this.spinButton10 = new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X, (int)topLeftPositionForCenteringOnScreen.Y, 26 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.content.LoadString("Strings\\StringsFromCSFiles:Slots.cs.12117", new object[0]));
			topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(31 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize * 3 / 2);
			this.spinButton100 = new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X, (int)topLeftPositionForCenteringOnScreen.Y, 31 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.content.LoadString("Strings\\StringsFromCSFiles:Slots.cs.12118", new object[0]));
			topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(24 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize * 5 / 2);
			this.doneButton = new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X, (int)topLeftPositionForCenteringOnScreen.Y, 24 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3864", new object[0]));
			if (Game1.isAnyGamePadButtonBeingPressed())
			{
				Game1.setMousePosition(this.spinButton10.bounds.Center);
				if (Game1.options.SnappyMenus)
				{
					this.currentlySnappedComponent = this.spinButton10;
				}
			}
		}

		public void setSlotResults(List<float> toSet)
		{
			double num = this.r.NextDouble();
			double num2 = 0.858 + Game1.dailyLuck * 2.0 + (double)Game1.player.LuckLevel * 0.08;
			if (num < 5E-05 * num2)
			{
				this.set(toSet, 5);
				this.payoutModifier = 2500f;
				return;
			}
			if (num < 0.0005 * num2)
			{
				this.set(toSet, 6);
				this.payoutModifier = 1000f;
				return;
			}
			if (num < 0.001 * num2)
			{
				this.set(toSet, 7);
				this.payoutModifier = 500f;
				return;
			}
			if (num < 0.002 * num2)
			{
				this.set(toSet, 4);
				this.payoutModifier = 200f;
				return;
			}
			if (num < 0.004 * num2)
			{
				this.set(toSet, 3);
				this.payoutModifier = 120f;
				return;
			}
			if (num < 0.006 * num2)
			{
				this.set(toSet, 2);
				this.payoutModifier = 80f;
				return;
			}
			if (num < 0.01 * num2)
			{
				this.set(toSet, 1);
				this.payoutModifier = 30f;
				return;
			}
			if (num < 0.03 * num2)
			{
				int num3 = this.r.Next(3);
				for (int i = 0; i < 3; i++)
				{
					toSet[i] = (float)((i == num3) ? this.r.Next(7) : 7);
				}
				this.payoutModifier = 3f;
				return;
			}
			if (num < 0.08 * num2)
			{
				this.set(toSet, 0);
				this.payoutModifier = 5f;
				return;
			}
			if (num < 0.2 * num2)
			{
				int num4 = this.r.Next(3);
				for (int j = 0; j < 3; j++)
				{
					toSet[j] = (float)((j == num4) ? 7 : this.r.Next(7));
				}
				this.payoutModifier = 2f;
				return;
			}
			this.payoutModifier = 0f;
			int[] array = new int[8];
			for (int k = 0; k < 3; k++)
			{
				int num5 = this.r.Next(6);
				while (array[num5] > 1)
				{
					num5 = this.r.Next(6);
				}
				toSet[k] = (float)num5;
				array[num5]++;
			}
		}

		private void set(List<float> toSet, int number)
		{
			toSet[0] = (float)number;
			toSet[1] = (float)number;
			toSet[2] = (float)number;
		}

		public bool tick(GameTime time)
		{
			if (this.spinning && this.endTimer <= 0)
			{
				for (int i = this.slotsFinished; i < this.slots.Count; i++)
				{
					float num = this.slots[i];
					List<float> list = this.slots;
					int index = i;
					list[index] += (float)time.ElapsedGameTime.Milliseconds * 0.008f * (1f - (float)i * 0.05f);
					list = this.slots;
					index = i;
					list[index] %= 8f;
					if (i == 2)
					{
						if (num % (0.25f + (float)this.slotsFinished * 0.5f) > this.slots[i] % (0.25f + (float)this.slotsFinished * 0.5f))
						{
							Game1.playSound("shiny4");
						}
						if (num > this.slots[i])
						{
							this.spinsCount++;
						}
					}
					if (this.spinsCount > 0 && i == this.slotsFinished && Math.Abs(this.slots[i] - this.slotResults[i]) <= (float)time.ElapsedGameTime.Milliseconds * 0.008f)
					{
						this.slots[i] = this.slotResults[i];
						this.slotsFinished++;
						this.spinsCount--;
						Game1.playSound("Cowboy_gunshot");
					}
				}
				if (this.slotsFinished >= 3)
				{
					this.endTimer = ((this.payoutModifier == 0f) ? 600 : 1000);
				}
			}
			if (this.endTimer > 0)
			{
				this.endTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.endTimer <= 0)
				{
					this.spinning = false;
					this.spinsCount = 0;
					this.slotsFinished = 0;
					if (this.payoutModifier > 0f)
					{
						this.showResult = true;
						Game1.playSound((this.payoutModifier >= 5f) ? ((this.payoutModifier >= 10f) ? "reward" : "money") : "newArtifact");
					}
					else
					{
						Game1.playSound("breathout");
					}
					Game1.player.clubCoins += (int)((float)this.currentBet * this.payoutModifier);
				}
			}
			this.spinButton10.scale = ((!this.spinning && this.spinButton10.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY())) ? 1.05f : 1f);
			this.spinButton100.scale = ((!this.spinning && this.spinButton100.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY())) ? 1.05f : 1f);
			this.doneButton.scale = ((!this.spinning && this.doneButton.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY())) ? 1.05f : 1f);
			return false;
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.spinning && Game1.player.clubCoins >= 10 && this.spinButton10.bounds.Contains(x, y))
			{
				Club.timesPlayedSlots++;
				this.r = new Random(Club.timesPlayedSlots + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
				this.setSlotResults(this.slotResults);
				this.spinning = true;
				Game1.playSound("bigSelect");
				this.currentBet = 10;
				this.slotsFinished = 0;
				this.spinsCount = 0;
				this.showResult = false;
				Game1.player.clubCoins -= 10;
			}
			if (!this.spinning && Game1.player.clubCoins >= 100 && this.spinButton100.bounds.Contains(x, y))
			{
				Club.timesPlayedSlots++;
				this.r = new Random(Club.timesPlayedSlots + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
				this.setSlotResults(this.slotResults);
				Game1.playSound("bigSelect");
				this.spinning = true;
				this.slotsFinished = 0;
				this.spinsCount = 0;
				this.showResult = false;
				this.currentBet = 100;
				Game1.player.clubCoins -= 100;
			}
			if (!this.spinning && this.doneButton.bounds.Contains(x, y))
			{
				Game1.playSound("bigDeSelect");
				Game1.currentMinigame = null;
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

		public bool overrideFreeMouseMovement()
		{
			return Game1.options.SnappyMenus;
		}

		public void receiveKeyPress(Keys k)
		{
			if (!this.spinning && (k.Equals(Keys.Escape) || Game1.options.doesInputListContain(Game1.options.menuButton, k)))
			{
				this.unload();
				Game1.playSound("bigDeSelect");
				Game1.currentMinigame = null;
				return;
			}
			if (!this.spinning)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					if (this.currentlySnappedComponent.Equals(this.spinButton10))
					{
						this.currentlySnappedComponent = this.spinButton100;
						Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
						return;
					}
					if (this.currentlySnappedComponent.Equals(this.spinButton100))
					{
						this.currentlySnappedComponent = this.doneButton;
						Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
						return;
					}
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					if (this.currentlySnappedComponent.Equals(this.doneButton))
					{
						this.currentlySnappedComponent = this.spinButton100;
						Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
						return;
					}
					if (this.currentlySnappedComponent.Equals(this.spinButton100))
					{
						this.currentlySnappedComponent = this.spinButton10;
						Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
					}
				}
			}
		}

		public void receiveKeyRelease(Keys k)
		{
		}

		public int getIconIndex(int index)
		{
			switch (index)
			{
			case 0:
				return 24;
			case 1:
				return 186;
			case 2:
				return 138;
			case 3:
				return 392;
			case 4:
				return 254;
			case 5:
				return 434;
			case 6:
				return 72;
			case 7:
				return 638;
			default:
				return 24;
			}
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Color(38, 0, 7));
			b.Draw(Game1.mouseCursors, Utility.getTopLeftPositionForCenteringOnScreen(57 * Game1.pixelZoom, 13 * Game1.pixelZoom, 0, -4 * Game1.tileSize), new Rectangle?(new Rectangle(441, 424, 57, 13)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			for (int i = 0; i < 3; i++)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 28 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 32 * Game1.pixelZoom)), new Rectangle?(new Rectangle(306, 320, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				float num = (this.slots[i] + 1f) % 8f;
				int iconIndex = this.getIconIndex(((int)num + 8 - 1) % 8);
				int iconIndex2 = this.getIconIndex((iconIndex + 1) % 8);
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 28 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 32 * Game1.pixelZoom)) - new Vector2(0f, (float)(-(float)Game1.tileSize) * (num % 1f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, iconIndex, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 28 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 32 * Game1.pixelZoom)) - new Vector2(0f, (float)Game1.tileSize - (float)Game1.tileSize * (num % 1f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, iconIndex2, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 33 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 48 * Game1.pixelZoom)), new Rectangle?(new Rectangle(415, 385, 26, 48)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			}
			if (this.showResult)
			{
				SpriteText.drawString(b, "+" + this.payoutModifier * (float)this.currentBet, Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 93 * Game1.pixelZoom, this.spinButton10.bounds.Y - Game1.tileSize + Game1.pixelZoom * 2, 9999, -1, 9999, 1f, 1f, false, -1, "", 4);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)this.spinButton10.bounds.X, (float)this.spinButton10.bounds.Y), new Rectangle?(new Rectangle(441, 385, 26, 13)), Color.White * ((!this.spinning && Game1.player.clubCoins >= 10) ? 1f : 0.5f), 0f, Vector2.Zero, (float)Game1.pixelZoom * this.spinButton10.scale, SpriteEffects.None, 0.99f);
			b.Draw(Game1.mouseCursors, new Vector2((float)this.spinButton100.bounds.X, (float)this.spinButton100.bounds.Y), new Rectangle?(new Rectangle(441, 398, 31, 13)), Color.White * ((!this.spinning && Game1.player.clubCoins >= 100) ? 1f : 0.5f), 0f, Vector2.Zero, (float)Game1.pixelZoom * this.spinButton100.scale, SpriteEffects.None, 0.99f);
			b.Draw(Game1.mouseCursors, new Vector2((float)this.doneButton.bounds.X, (float)this.doneButton.bounds.Y), new Rectangle?(new Rectangle(441, 411, 24, 13)), Color.White * ((!this.spinning) ? 1f : 0.5f), 0f, Vector2.Zero, (float)Game1.pixelZoom * this.doneButton.scale, SpriteEffects.None, 0.99f);
			SpriteText.drawStringWithScrollBackground(b, this.coinBuffer + Game1.player.clubCoins, Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 94 * Game1.pixelZoom, Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 30 * Game1.pixelZoom, "", 1f, -1);
			Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 94 * Game1.pixelZoom + Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 30 * Game1.pixelZoom + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
			Vector2 vector = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 + 50 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 88 * Game1.pixelZoom));
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), (int)vector.X, (int)vector.Y, Game1.tileSize * 6, Game1.tileSize * 11, Color.White, (float)Game1.pixelZoom, true);
			b.Draw(Game1.objectSpriteSheet, vector + new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.pixelZoom * 2)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			SpriteText.drawString(b, "x2", (int)vector.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int)vector.Y + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
			b.Draw(Game1.objectSpriteSheet, vector + new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			b.Draw(Game1.objectSpriteSheet, vector + new Vector2((float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom)), (float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			SpriteText.drawString(b, "x3", (int)vector.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int)vector.Y + (Game1.tileSize + Game1.pixelZoom) + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
			for (int j = 0; j < 8; j++)
			{
				int index = j;
				if (j == 5)
				{
					index = 7;
				}
				else if (j == 7)
				{
					index = 5;
				}
				b.Draw(Game1.objectSpriteSheet, vector + new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.pixelZoom * 2 + (j + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(index), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.objectSpriteSheet, vector + new Vector2((float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom)), (float)(Game1.pixelZoom * 2 + (j + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(index), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.objectSpriteSheet, vector + new Vector2((float)(Game1.pixelZoom * 2 + 2 * (Game1.tileSize + Game1.pixelZoom)), (float)(Game1.pixelZoom * 2 + (j + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(index), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				int num2 = 0;
				switch (j)
				{
				case 0:
					num2 = 5;
					break;
				case 1:
					num2 = 30;
					break;
				case 2:
					num2 = 80;
					break;
				case 3:
					num2 = 120;
					break;
				case 4:
					num2 = 200;
					break;
				case 5:
					num2 = 500;
					break;
				case 6:
					num2 = 1000;
					break;
				case 7:
					num2 = 2500;
					break;
				}
				SpriteText.drawString(b, "x" + num2, (int)vector.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int)vector.Y + (j + 2) * (Game1.tileSize + Game1.pixelZoom) + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
			}
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), (int)vector.X - Game1.tileSize * 10, (int)vector.Y, Game1.tileSize * 16, Game1.tileSize * 11, Color.Red, (float)Game1.pixelZoom, false);
			for (int k = 1; k < 8; k++)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), (int)vector.X - Game1.tileSize * 10 - Game1.pixelZoom * k, (int)vector.Y - Game1.pixelZoom * k, Game1.tileSize * 16 + Game1.pixelZoom * 2 * k, Game1.tileSize * 11 + Game1.pixelZoom * 2 * k, Color.Red * (1f - (float)k * 0.15f), (float)Game1.pixelZoom, false);
			}
			for (int l = 0; l < 17; l++)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(147, 472, 3, 3), (int)vector.X - Game1.tileSize * 10 + Game1.pixelZoom * 2, (int)vector.Y + l * Game1.pixelZoom * 3 + Game1.pixelZoom * 3, (int)((float)Game1.tileSize * 9.5f - (float)(l * Game1.tileSize) * 1.2f + (float)(l * l * Game1.pixelZoom) * 0.7f), Game1.pixelZoom, new Color(l * 25, (l > 8) ? (l * 10) : 0, 255 - l * 25), (float)Game1.pixelZoom, false);
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
			return "Slots";
		}
	}
}
