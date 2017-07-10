using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class GeodeMenu : MenuWithInventory
	{
		public const int region_geodeSpot = 998;

		public ClickableComponent geodeSpot;

		public AnimatedSprite clint;

		public TemporaryAnimatedSprite geodeDestructionAnimation;

		public TemporaryAnimatedSprite sparkle;

		public int geodeAnimationTimer;

		public int yPositionOfGem;

		public int alertTimer;

		public StardewValley.Object geodeTreasure;

		public GeodeMenu() : base(null, true, true, Game1.tileSize / 5, Game1.tileSize * 2 + Game1.pixelZoom)
		{
			this.inventory.highlightMethod = new InventoryMenu.highlightThisItem(this.highlightGeodes);
			this.geodeSpot = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + 4, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 8, 560, 308), "")
			{
				myID = 998,
				downNeighborID = 0
			};
			this.clint = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Clint"), 8, 32, 48);
			if (this.inventory.inventory != null && this.inventory.inventory.Count >= 12)
			{
				for (int i = 0; i < 12; i++)
				{
					if (this.inventory.inventory[i] != null)
					{
						this.inventory.inventory[i].upNeighborID = 998;
					}
				}
			}
			if (this.trashCan != null)
			{
				this.trashCan.myID = 106;
			}
			if (this.okButton != null)
			{
				this.okButton.leftNeighborID = 11;
			}
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(0);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override bool readyToClose()
		{
			return base.readyToClose() && this.geodeAnimationTimer <= 0 && this.heldItem == null;
		}

		public bool highlightGeodes(Item i)
		{
			if (this.heldItem != null)
			{
				return true;
			}
			int parentSheetIndex = i.parentSheetIndex;
			switch (parentSheetIndex)
			{
			case 535:
			case 536:
			case 537:
				break;
			default:
				if (parentSheetIndex != 749)
				{
					return false;
				}
				break;
			}
			return true;
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, true);
			if (this.geodeSpot.containsPoint(x, y))
			{
				if (this.heldItem != null && this.heldItem.Name.Contains("Geode") && Game1.player.money >= 25 && this.geodeAnimationTimer <= 0)
				{
					if (Game1.player.freeSpotsInInventory() > 1 || (Game1.player.freeSpotsInInventory() == 1 && this.heldItem.Stack == 1))
					{
						this.geodeSpot.item = this.heldItem.getOne();
						Item expr_A7 = this.heldItem;
						int stack = expr_A7.Stack;
						expr_A7.Stack = stack - 1;
						if (this.heldItem.Stack <= 0)
						{
							this.heldItem = null;
						}
						this.geodeAnimationTimer = 2700;
						Game1.player.money -= 25;
						Game1.playSound("stoneStep");
						this.clint.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(8, 300),
							new FarmerSprite.AnimationFrame(9, 200),
							new FarmerSprite.AnimationFrame(10, 80),
							new FarmerSprite.AnimationFrame(11, 200),
							new FarmerSprite.AnimationFrame(12, 100),
							new FarmerSprite.AnimationFrame(8, 300)
						});
						this.clint.loop = false;
						return;
					}
					this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_InventoryFull", new object[0]);
					this.wiggleWordsTimer = 500;
					this.alertTimer = 1500;
					return;
				}
				else if (Game1.player.money < 25)
				{
					this.wiggleWordsTimer = 500;
					Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			base.receiveRightClick(x, y, true);
		}

		public override void performHoverAction(int x, int y)
		{
			if (this.alertTimer <= 0)
			{
				base.performHoverAction(x, y);
				if (this.descriptionText.Equals(""))
				{
					if (Game1.player.money < 25)
					{
						this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_Description_NotEnoughMoney", new object[0]);
						return;
					}
					this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_Description", new object[0]);
				}
			}
		}

		public override void emergencyShutDown()
		{
			base.emergencyShutDown();
			if (this.heldItem != null)
			{
				Game1.player.addItemToInventoryBool(this.heldItem, false);
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.alertTimer > 0)
			{
				this.alertTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.geodeAnimationTimer > 0)
			{
				Game1.changeMusicTrack("none");
				this.geodeAnimationTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.geodeAnimationTimer <= 0)
				{
					this.geodeDestructionAnimation = null;
					this.geodeSpot.item = null;
					Game1.player.addItemToInventoryBool(this.geodeTreasure, false);
					this.geodeTreasure = null;
					this.yPositionOfGem = 0;
					return;
				}
				int currentFrame = this.clint.CurrentFrame;
				this.clint.animateOnce(time);
				if (this.clint.CurrentFrame == 11 && currentFrame != 11)
				{
					Stats expr_D1 = Game1.stats;
					uint geodesCracked = expr_D1.GeodesCracked;
					expr_D1.GeodesCracked = geodesCracked + 1u;
					Game1.playSound("hammer");
					Game1.playSound("stoneCrack");
					int num = 448;
					if (this.geodeSpot.item != null)
					{
						int parentSheetIndex = (this.geodeSpot.item as StardewValley.Object).parentSheetIndex;
						if (parentSheetIndex != 536)
						{
							if (parentSheetIndex == 537)
							{
								num += Game1.tileSize * 2;
							}
						}
						else
						{
							num += Game1.tileSize;
						}
						this.geodeDestructionAnimation = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, num, Game1.tileSize, Game1.tileSize), 100f, 8, 0, new Vector2((float)(this.geodeSpot.bounds.X + 392 - Game1.tileSize / 2), (float)(this.geodeSpot.bounds.Y + 192 - Game1.tileSize / 2)), false, false);
						this.geodeTreasure = Utility.getTreasureFromGeode(this.geodeSpot.item);
						if (this.geodeTreasure.Type.Contains("Mineral"))
						{
							Game1.player.foundMineral(this.geodeTreasure.parentSheetIndex);
						}
						else if (this.geodeTreasure.Type.Contains("Arch") && !Game1.player.hasOrWillReceiveMail("artifactFound"))
						{
							this.geodeTreasure = new StardewValley.Object(390, 5, false, -1, 0);
						}
					}
				}
				if (this.geodeDestructionAnimation != null && this.geodeDestructionAnimation.currentParentTileIndex < 7)
				{
					this.geodeDestructionAnimation.update(time);
					if (this.geodeDestructionAnimation.currentParentTileIndex < 3)
					{
						this.yPositionOfGem--;
					}
					this.yPositionOfGem--;
					if (this.geodeDestructionAnimation.currentParentTileIndex == 7 && this.geodeTreasure.price > 75)
					{
						this.sparkle = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 640, Game1.tileSize, Game1.tileSize), 100f, 8, 0, new Vector2((float)(this.geodeSpot.bounds.X + 392 - Game1.tileSize / 2), (float)(this.geodeSpot.bounds.Y + 192 + this.yPositionOfGem - Game1.tileSize / 2)), false, false);
						Game1.playSound("discoverMineral");
					}
					else if (this.geodeDestructionAnimation.currentParentTileIndex == 7 && this.geodeTreasure.price <= 75)
					{
						Game1.playSound("newArtifact");
					}
				}
				if (this.sparkle != null && this.sparkle.update(time))
				{
					this.sparkle = null;
				}
			}
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.geodeSpot = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + 4, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 8, 560, 308), "Anvil");
			int yPosition = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4 + Game1.tileSize * 2 + Game1.pixelZoom;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + Game1.tileSize / 5, yPosition, false, null, this.inventory.highlightMethod, -1, 3, 0, 0, true);
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			base.draw(b, true, true);
			Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
			b.Draw(Game1.mouseCursors, new Vector2((float)this.geodeSpot.bounds.X, (float)this.geodeSpot.bounds.Y), new Rectangle?(new Rectangle(0, 512, 140, 77)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.87f);
			if (this.geodeSpot.item != null)
			{
				if (this.geodeDestructionAnimation == null)
				{
					this.geodeSpot.item.drawInMenu(b, new Vector2((float)(this.geodeSpot.bounds.X + 90 * Game1.pixelZoom), (float)(this.geodeSpot.bounds.Y + 40 * Game1.pixelZoom)), 1f);
				}
				else
				{
					this.geodeDestructionAnimation.draw(b, true, 0, 0);
				}
				if (this.geodeTreasure != null)
				{
					this.geodeTreasure.drawInMenu(b, new Vector2((float)(this.geodeSpot.bounds.X + 90 * Game1.pixelZoom), (float)(this.geodeSpot.bounds.Y + 40 * Game1.pixelZoom + this.yPositionOfGem)), 1f);
				}
				if (this.sparkle != null)
				{
					this.sparkle.draw(b, true, 0, 0);
				}
			}
			this.clint.draw(b, new Vector2((float)(this.geodeSpot.bounds.X + 96 * Game1.pixelZoom), (float)(this.geodeSpot.bounds.Y + 16 * Game1.pixelZoom)), 0.877f);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
			}
			if (!Game1.options.hardwareCursor)
			{
				base.drawMouse(b);
			}
		}
	}
}
