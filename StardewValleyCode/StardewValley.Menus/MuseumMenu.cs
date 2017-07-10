using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using System;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	public class MuseumMenu : MenuWithInventory
	{
		public const int startingState = 0;

		public const int placingInMuseumState = 1;

		public const int exitingState = 2;

		public int fadeTimer;

		public int state;

		public int menuPositionOffset;

		public bool fadeIntoBlack;

		public bool menuMovingDown;

		public float blackFadeAlpha;

		public SparklingText sparkleText;

		public Vector2 globalLocationOfSparklingArtifact;

		private bool holdingMuseumPiece;

		public MuseumMenu() : base(new InventoryMenu.highlightThisItem((Game1.currentLocation as LibraryMuseum).isItemSuitableForDonation), true, false, 0, 0)
		{
			this.fadeTimer = 800;
			this.fadeIntoBlack = true;
			base.movePosition(0, Game1.viewport.Height - this.yPositionOnScreen - this.height);
			Game1.player.forceCanMove();
			if (Game1.options.SnappyMenus)
			{
				if (this.okButton != null)
				{
					this.okButton.myID = 106;
				}
				base.populateClickableComponentList();
				this.currentlySnappedComponent = base.getComponentWithID(0);
				this.snapCursorToCurrentSnappedComponent();
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			if (this.fadeTimer <= 0)
			{
				if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
				{
					this.state = 2;
					this.fadeTimer = 500;
					this.fadeIntoBlack = true;
				}
				else if (Game1.options.SnappyMenus)
				{
					base.receiveKeyPress(key);
				}
				if (!Game1.options.SnappyMenus)
				{
					if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
					{
						Game1.panScreen(0, 4);
						return;
					}
					if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
					{
						Game1.panScreen(4, 0);
						return;
					}
					if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
					{
						Game1.panScreen(0, -4);
						return;
					}
					if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
					{
						Game1.panScreen(-4, 0);
					}
				}
			}
		}

		public override bool overrideSnappyMenuCursorMovementBan()
		{
			return this.heldItem != null;
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.fadeTimer <= 0)
			{
				Item heldItem = this.heldItem;
				if (!this.holdingMuseumPiece)
				{
					this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
				}
				if (heldItem != null && this.heldItem != null && (y < Game1.viewport.Height - (this.height - (IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize)) || this.menuMovingDown))
				{
					int num = (x + Game1.viewport.X) / Game1.tileSize;
					int num2 = (y + Game1.viewport.Y) / Game1.tileSize;
					if ((Game1.currentLocation as LibraryMuseum).isTileSuitableForMuseumPiece(num, num2) && (Game1.currentLocation as LibraryMuseum).isItemSuitableForDonation(this.heldItem))
					{
						int count = (Game1.currentLocation as LibraryMuseum).getRewardsForPlayer(Game1.player).Count;
						(Game1.currentLocation as LibraryMuseum).museumPieces.Add(new Vector2((float)num, (float)num2), (this.heldItem as StardewValley.Object).parentSheetIndex);
						Game1.playSound("stoneStep");
						this.holdingMuseumPiece = false;
						if ((Game1.currentLocation as LibraryMuseum).getRewardsForPlayer(Game1.player).Count > count)
						{
							this.sparkleText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:NewReward", new object[0]), Color.MediumSpringGreen, Color.White, false, 0.1, 2500, -1, 500);
							Game1.playSound("reward");
							this.globalLocationOfSparklingArtifact = new Vector2((float)(num * Game1.tileSize + Game1.tileSize / 2) - this.sparkleText.textWidth / 2f, (float)(num2 * Game1.tileSize - Game1.tileSize * 3 / 4));
						}
						else
						{
							Game1.playSound("newArtifact");
						}
						Game1.player.completeQuest(24);
						Item expr_1F1 = this.heldItem;
						int stack = expr_1F1.Stack;
						expr_1F1.Stack = stack - 1;
						if (this.heldItem.Stack <= 0)
						{
							this.heldItem = null;
						}
						this.menuMovingDown = false;
						int count2 = (Game1.currentLocation as LibraryMuseum).museumPieces.Count;
						if (count2 >= 95)
						{
							Game1.getAchievement(5);
						}
						else if (count2 >= 40)
						{
							Game1.getAchievement(28);
						}
					}
				}
				else if (this.heldItem == null && !this.inventory.isWithinBounds(x, y))
				{
					int num3 = (x + Game1.viewport.X) / Game1.tileSize;
					int num4 = (y + Game1.viewport.Y) / Game1.tileSize;
					Vector2 key = new Vector2((float)num3, (float)num4);
					if ((Game1.currentLocation as LibraryMuseum).museumPieces.ContainsKey(key))
					{
						this.heldItem = new StardewValley.Object((Game1.currentLocation as LibraryMuseum).museumPieces[key], 1, false, -1, 0);
						(Game1.currentLocation as LibraryMuseum).museumPieces.Remove(key);
						this.holdingMuseumPiece = true;
					}
				}
				if (this.heldItem != null && heldItem == null)
				{
					this.menuMovingDown = true;
				}
				if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
				{
					this.state = 2;
					this.fadeTimer = 800;
					this.fadeIntoBlack = true;
					Game1.playSound("bigDeSelect");
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			Item heldItem = this.heldItem;
			if (this.fadeTimer <= 0)
			{
				base.receiveRightClick(x, y, true);
			}
			if (this.heldItem != null && heldItem == null)
			{
				this.menuMovingDown = true;
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.sparkleText != null && this.sparkleText.update(time))
			{
				this.sparkleText = null;
			}
			if (this.fadeTimer > 0)
			{
				this.fadeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.fadeIntoBlack)
				{
					this.blackFadeAlpha = 0f + (1500f - (float)this.fadeTimer) / 1500f;
				}
				else
				{
					this.blackFadeAlpha = 1f - (1500f - (float)this.fadeTimer) / 1500f;
				}
				if (this.fadeTimer <= 0)
				{
					switch (this.state)
					{
					case 0:
						this.state = 1;
						Game1.viewportFreeze = true;
						Game1.viewport.Location = new Location(18 * Game1.tileSize, 2 * Game1.tileSize);
						Game1.clampViewportToGameMap();
						this.fadeTimer = 800;
						this.fadeIntoBlack = false;
						break;
					case 2:
						Game1.viewportFreeze = false;
						this.fadeIntoBlack = false;
						this.fadeTimer = 800;
						this.state = 3;
						break;
					case 3:
						Game1.exitActiveMenu();
						break;
					}
				}
			}
			if (this.menuMovingDown && this.menuPositionOffset < this.height / 3)
			{
				this.menuPositionOffset += 8;
				base.movePosition(0, 8);
			}
			else if (!this.menuMovingDown && this.menuPositionOffset > 0)
			{
				this.menuPositionOffset -= 8;
				base.movePosition(0, -8);
			}
			int num = Game1.getOldMouseX() + Game1.viewport.X;
			int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
			if (num - Game1.viewport.X < Game1.tileSize)
			{
				Game1.panScreen(-4, 0);
			}
			else if (num - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
			{
				Game1.panScreen(4, 0);
			}
			if (num2 - Game1.viewport.Y < Game1.tileSize)
			{
				Game1.panScreen(0, -4);
			}
			else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
			{
				Game1.panScreen(0, 4);
				if (this.menuMovingDown)
				{
					this.menuMovingDown = false;
				}
			}
			Keys[] pressedKeys = Game1.oldKBState.GetPressedKeys();
			for (int i = 0; i < pressedKeys.Length; i++)
			{
				Keys key = pressedKeys[i];
				this.receiveKeyPress(key);
			}
		}

		public override void gameWindowSizeChanged(Microsoft.Xna.Framework.Rectangle oldBounds, Microsoft.Xna.Framework.Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			base.movePosition(0, Game1.viewport.Height - this.yPositionOnScreen - this.height);
			Game1.player.forceCanMove();
		}

		public override void draw(SpriteBatch b)
		{
			if ((this.fadeTimer <= 0 || !this.fadeIntoBlack) && this.state != 3)
			{
				if (this.heldItem != null)
				{
					for (int i = Game1.viewport.Y / Game1.tileSize - 1; i < (Game1.viewport.Y + Game1.viewport.Height) / Game1.tileSize + 2; i++)
					{
						for (int j = Game1.viewport.X / Game1.tileSize - 1; j < (Game1.viewport.X + Game1.viewport.Width) / Game1.tileSize + 1; j++)
						{
							if ((Game1.currentLocation as LibraryMuseum).isTileSuitableForMuseumPiece(j, i))
							{
								b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)i) * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1)), Color.LightGreen);
							}
						}
					}
				}
				if (!this.holdingMuseumPiece)
				{
					base.draw(b, false, false);
				}
				if (!this.hoverText.Equals(""))
				{
					IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
				}
				if (this.heldItem != null)
				{
					this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
				}
				base.drawMouse(b);
				if (this.sparkleText != null)
				{
					this.sparkleText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.globalLocationOfSparklingArtifact));
				}
			}
			b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * this.blackFadeAlpha);
		}
	}
}
