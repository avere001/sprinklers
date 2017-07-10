using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StardewValley.Menus
{
	public abstract class IClickableMenu
	{
		public delegate void onExit();

		public const int currency_g = 0;

		public const int currency_starTokens = 1;

		public const int currency_qiCoins = 2;

		public const int greyedOutSpotIndex = 57;

		public const int outerBorderWithUpArrow = 61;

		public const int lvlMarkerRedIndex = 54;

		public const int lvlMarkerGreyIndex = 55;

		public const int borderWithDownArrowIndex = 46;

		public const int borderWithUpArrowIndex = 47;

		public const int littleHeartIndex = 49;

		public const int uncheckedBoxIndex = 50;

		public const int checkedBoxIndex = 51;

		public const int presentIconIndex = 58;

		public const int itemSpotIndex = 10;

		public static int borderWidth = Game1.tileSize / 2 + Game1.tileSize / 8;

		public static int tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;

		public static int spaceToClearTopBorder = Game1.tileSize * 3 / 2;

		public static int spaceToClearSideBorder = Game1.tileSize / 4;

		public const int spaceBetweenTabs = 4;

		public int width;

		public int height;

		public int xPositionOnScreen;

		public int yPositionOnScreen;

		public int currentRegion;

		public IClickableMenu.onExit exitFunction;

		public ClickableTextureComponent upperRightCloseButton;

		public bool destroy;

		public bool gamePadControlsImplemented;

		public List<ClickableComponent> allClickableComponents;

		public ClickableComponent currentlySnappedComponent;

		public IClickableMenu()
		{
		}

		public IClickableMenu(int x, int y, int width, int height, bool showUpperRightCloseButton = false)
		{
			this.initialize(x, y, width, height, showUpperRightCloseButton);
			if (Game1.gameMode == 3 && Game1.player != null && !Game1.eventUp)
			{
				Game1.player.Halt();
			}
		}

		public void initialize(int x, int y, int width, int height, bool showUpperRightCloseButton = false)
		{
			if (Game1.player != null && !Game1.player.UsingTool && !Game1.eventUp)
			{
				Game1.player.forceCanMove();
			}
			this.xPositionOnScreen = x;
			this.yPositionOnScreen = y;
			this.width = width;
			this.height = height;
			if (showUpperRightCloseButton)
			{
				this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width - 9 * Game1.pixelZoom, this.yPositionOnScreen - Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
			}
			for (int i = 0; i < 4; i++)
			{
				Game1.directionKeyPolling[i] = 250;
			}
		}

		public virtual bool areGamePadControlsImplemented()
		{
			return false;
		}

		public ClickableComponent getLastClickableComponentInThisListThatContainsThisXCoord(List<ClickableComponent> ccList, int xCoord)
		{
			for (int i = ccList.Count - 1; i >= 0; i--)
			{
				if (ccList[i].bounds.Contains(xCoord, ccList[i].bounds.Center.Y))
				{
					return ccList[i];
				}
			}
			return null;
		}

		public ClickableComponent getFirstClickableComponentInThisListThatContainsThisXCoord(List<ClickableComponent> ccList, int xCoord)
		{
			for (int i = 0; i < ccList.Count; i++)
			{
				if (ccList[i].bounds.Contains(xCoord, ccList[i].bounds.Center.Y))
				{
					return ccList[i];
				}
			}
			return null;
		}

		public ClickableComponent getLastClickableComponentInThisListThatContainsThisYCoord(List<ClickableComponent> ccList, int yCoord)
		{
			for (int i = ccList.Count - 1; i >= 0; i--)
			{
				if (ccList[i].bounds.Contains(ccList[i].bounds.Center.X, yCoord))
				{
					return ccList[i];
				}
			}
			return null;
		}

		public ClickableComponent getFirstClickableComponentInThisListThatContainsThisYCoord(List<ClickableComponent> ccList, int yCoord)
		{
			for (int i = 0; i < ccList.Count; i++)
			{
				if (ccList[i].bounds.Contains(ccList[i].bounds.Center.X, yCoord))
				{
					return ccList[i];
				}
			}
			return null;
		}

		public virtual void receiveGamePadButton(Buttons b)
		{
			if (!Game1.options.snappyMenus || !Game1.options.gamepadControls || b != Buttons.A)
			{
			}
		}

		public void drawMouse(SpriteBatch b)
		{
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.SnappyMenus ? 44 : 0, 16, 16)), Color.White * Game1.mouseCursorTransparency, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		public void populateClickableComponentList()
		{
			this.allClickableComponents = new List<ClickableComponent>();
			FieldInfo[] fields = base.GetType().GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				if (fieldInfo.FieldType.IsSubclassOf(typeof(ClickableComponent)) || fieldInfo.FieldType == typeof(ClickableComponent))
				{
					if (fieldInfo.GetValue(this) != null)
					{
						this.allClickableComponents.Add((ClickableComponent)fieldInfo.GetValue(this));
					}
				}
				else if (fieldInfo.FieldType == typeof(List<ClickableComponent>))
				{
					List<ClickableComponent> list = (List<ClickableComponent>)fieldInfo.GetValue(this);
					if (list != null)
					{
						for (int j = list.Count - 1; j >= 0; j--)
						{
							if (list[j] != null)
							{
								this.allClickableComponents.Add(list[j]);
							}
						}
					}
				}
				else if (fieldInfo.FieldType == typeof(List<ClickableTextureComponent>))
				{
					List<ClickableTextureComponent> list2 = (List<ClickableTextureComponent>)fieldInfo.GetValue(this);
					if (list2 != null)
					{
						for (int k = list2.Count - 1; k >= 0; k--)
						{
							if (list2[k] != null)
							{
								this.allClickableComponents.Add(list2[k]);
							}
						}
					}
				}
				else if (fieldInfo.FieldType == typeof(List<ClickableAnimatedComponent>))
				{
					List<ClickableAnimatedComponent> list3 = (List<ClickableAnimatedComponent>)fieldInfo.GetValue(this);
					for (int l = list3.Count - 1; l >= 0; l--)
					{
						if (list3[l] != null)
						{
							this.allClickableComponents.Add(list3[l]);
						}
					}
				}
				else if (fieldInfo.FieldType == typeof(List<Bundle>))
				{
					List<Bundle> list4 = (List<Bundle>)fieldInfo.GetValue(this);
					for (int m = list4.Count - 1; m >= 0; m--)
					{
						if (list4[m] != null)
						{
							this.allClickableComponents.Add(list4[m]);
						}
					}
				}
				else if (fieldInfo.FieldType == typeof(InventoryMenu))
				{
					this.allClickableComponents.AddRange(((InventoryMenu)fieldInfo.GetValue(this)).inventory);
				}
				else
				{
					if (fieldInfo.FieldType == typeof(List<Dictionary<ClickableTextureComponent, CraftingRecipe>>))
					{
						using (List<Dictionary<ClickableTextureComponent, CraftingRecipe>>.Enumerator enumerator = ((List<Dictionary<ClickableTextureComponent, CraftingRecipe>>)fieldInfo.GetValue(this)).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Dictionary<ClickableTextureComponent, CraftingRecipe> current = enumerator.Current;
								this.allClickableComponents.AddRange(current.Keys);
							}
							goto IL_329;
						}
					}
					if (fieldInfo.FieldType == typeof(Dictionary<int, List<List<ClickableTextureComponent>>>))
					{
						using (Dictionary<int, List<List<ClickableTextureComponent>>>.ValueCollection.Enumerator enumerator2 = ((Dictionary<int, List<List<ClickableTextureComponent>>>)fieldInfo.GetValue(this)).Values.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								foreach (List<ClickableTextureComponent> current2 in enumerator2.Current)
								{
									this.allClickableComponents.AddRange(current2);
								}
							}
						}
					}
				}
				IL_329:;
			}
		}

		public virtual void applyMovementKey(int direction)
		{
			if (this.allClickableComponents == null)
			{
				this.populateClickableComponentList();
			}
			this.moveCursorInDirection(direction);
		}

		public virtual void snapToDefaultClickableComponent()
		{
		}

		public void applyMovementKey(Keys key)
		{
			if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
			{
				this.applyMovementKey(0);
				return;
			}
			if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
			{
				this.applyMovementKey(1);
				return;
			}
			if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
			{
				this.applyMovementKey(2);
				return;
			}
			if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
			{
				this.applyMovementKey(3);
			}
		}

		public virtual void setCurrentlySnappedComponentTo(int id)
		{
			this.currentlySnappedComponent = this.getComponentWithID(id);
		}

		public void moveCursorInDirection(int direction)
		{
			if (this.currentlySnappedComponent == null && this.allClickableComponents != null && this.allClickableComponents.Count<ClickableComponent>() > 0)
			{
				this.snapToDefaultClickableComponent();
				if (this.currentlySnappedComponent == null)
				{
					this.currentlySnappedComponent = this.allClickableComponents.First<ClickableComponent>();
				}
			}
			if (this.currentlySnappedComponent != null)
			{
				ClickableComponent clickableComponent = this.currentlySnappedComponent;
				switch (direction)
				{
				case 0:
					if (this.currentlySnappedComponent.upNeighborID == -99999)
					{
						this.snapToDefaultClickableComponent();
					}
					else if (this.currentlySnappedComponent.upNeighborID == -7777)
					{
						this.customSnapBehavior(0, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
					}
					else
					{
						this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.upNeighborID);
					}
					if (this.currentlySnappedComponent != null && (clickableComponent == null || clickableComponent.upNeighborID != -7777) && !this.currentlySnappedComponent.downNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable)
					{
						this.currentlySnappedComponent.downNeighborID = clickableComponent.myID;
					}
					break;
				case 1:
					if (this.currentlySnappedComponent.rightNeighborID == -99999)
					{
						this.snapToDefaultClickableComponent();
					}
					else if (this.currentlySnappedComponent.rightNeighborID == -7777)
					{
						this.customSnapBehavior(1, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
					}
					else
					{
						this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.rightNeighborID);
					}
					if (this.currentlySnappedComponent != null && (clickableComponent == null || clickableComponent.rightNeighborID != -7777) && !this.currentlySnappedComponent.leftNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable)
					{
						this.currentlySnappedComponent.leftNeighborID = clickableComponent.myID;
					}
					if (this.currentlySnappedComponent == null && clickableComponent.tryDefaultIfNoRightNeighborExists)
					{
						this.snapToDefaultClickableComponent();
					}
					break;
				case 2:
					if (this.currentlySnappedComponent.downNeighborID == -99999)
					{
						this.snapToDefaultClickableComponent();
					}
					else if (this.currentlySnappedComponent.downNeighborID == -7777)
					{
						this.customSnapBehavior(2, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
					}
					else
					{
						this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.downNeighborID);
					}
					if (this.currentlySnappedComponent != null && (clickableComponent == null || clickableComponent.downNeighborID != -7777) && !this.currentlySnappedComponent.upNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable)
					{
						this.currentlySnappedComponent.upNeighborID = clickableComponent.myID;
					}
					if (this.currentlySnappedComponent == null && clickableComponent.tryDefaultIfNoDownNeighborExists)
					{
						this.snapToDefaultClickableComponent();
					}
					break;
				case 3:
					if (this.currentlySnappedComponent.leftNeighborID == -99999)
					{
						this.snapToDefaultClickableComponent();
					}
					else if (this.currentlySnappedComponent.leftNeighborID == -7777)
					{
						this.customSnapBehavior(3, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
					}
					else
					{
						this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.leftNeighborID);
					}
					if (this.currentlySnappedComponent != null && (clickableComponent == null || clickableComponent.leftNeighborID != -7777) && !this.currentlySnappedComponent.rightNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable)
					{
						this.currentlySnappedComponent.rightNeighborID = clickableComponent.myID;
					}
					break;
				}
				if (this.currentlySnappedComponent != null && clickableComponent != null && this.currentlySnappedComponent.region != clickableComponent.region)
				{
					this.actionOnRegionChange(clickableComponent.region, this.currentlySnappedComponent.region);
				}
				if (this.currentlySnappedComponent == null)
				{
					this.currentlySnappedComponent = clickableComponent;
				}
				this.snapCursorToCurrentSnappedComponent();
				Game1.playSound("shiny4");
				Game1.debugOutput = (("snapped Component ID: " + this.currentlySnappedComponent.myID) ?? "");
			}
		}

		public virtual void snapCursorToCurrentSnappedComponent()
		{
			if (this.currentlySnappedComponent != null)
			{
				Game1.setMousePosition(this.currentlySnappedComponent.bounds.Right - this.currentlySnappedComponent.bounds.Width / 4, this.currentlySnappedComponent.bounds.Bottom - this.currentlySnappedComponent.bounds.Height / 4);
			}
		}

		protected virtual void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
		}

		protected virtual void actionOnRegionChange(int oldRegion, int newRegion)
		{
		}

		public ClickableComponent getComponentWithID(int id)
		{
			if (this.allClickableComponents != null)
			{
				for (int i = 0; i < this.allClickableComponents.Count; i++)
				{
					if (this.allClickableComponents[i] != null && this.allClickableComponents[i].myID == id && this.allClickableComponents[i].visible)
					{
						return this.allClickableComponents[i];
					}
				}
				for (int j = 0; j < this.allClickableComponents.Count; j++)
				{
					if (this.allClickableComponents[j] != null && this.allClickableComponents[j].myAlternateID == id && this.allClickableComponents[j].visible)
					{
						return this.allClickableComponents[j];
					}
				}
			}
			return null;
		}

		public void initializeUpperRightCloseButton()
		{
			this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 9 * Game1.pixelZoom, this.yPositionOnScreen - Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
		}

		public virtual void drawBackground(SpriteBatch b)
		{
			if (this is ShopMenu)
			{
				for (int i = 0; i < Game1.viewport.Width; i += 100 * Game1.pixelZoom)
				{
					for (int j = 0; j < Game1.viewport.Height; j += 96 * Game1.pixelZoom)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)i, (float)j), new Rectangle?(new Rectangle(527, 0, 100, 96)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.08f);
					}
				}
				return;
			}
			if (Game1.isDarkOut())
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 144)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
			}
			else if (Game1.isRaining)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(640, 858, 1, 184)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
			}
			else
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639 + Utility.getSeasonNumber(Game1.currentSeason), 1051, 1, 400)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(-30 * Game1.pixelZoom), (float)(Game1.viewport.Height - 148 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1035 : ((Game1.isRaining || Game1.isDarkOut()) ? 886 : 737), 639, 148)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.08f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(-30 * Game1.pixelZoom + 639 * Game1.pixelZoom), (float)(Game1.viewport.Height - 148 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1035 : ((Game1.isRaining || Game1.isDarkOut()) ? 886 : 737), 639, 148)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.08f);
			if (Game1.isRaining)
			{
				b.Draw(Game1.staminaRect, Utility.xTileToMicrosoftRectangle(Game1.viewport), Color.Blue * 0.2f);
			}
		}

		public virtual bool showWithoutTransparencyIfOptionIsSet()
		{
			return this is GameMenu || this is ShopMenu || this is WheelSpinGame || this is ItemGrabMenu;
		}

		public virtual void clickAway()
		{
		}

		public virtual void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = (int)((float)newBounds.Width * ((float)this.xPositionOnScreen / (float)oldBounds.Width));
			this.yPositionOnScreen = (int)((float)newBounds.Height * ((float)this.yPositionOnScreen / (float)oldBounds.Height));
		}

		public virtual void setUpForGamePadMode()
		{
		}

		public virtual void releaseLeftClick(int x, int y)
		{
		}

		public virtual void leftClickHeld(int x, int y)
		{
		}

		public virtual void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.upperRightCloseButton != null && this.readyToClose() && this.upperRightCloseButton.containsPoint(x, y))
			{
				if (playSound)
				{
					Game1.playSound("bigDeSelect");
				}
				this.exitThisMenu(true);
			}
		}

		public virtual bool overrideSnappyMenuCursorMovementBan()
		{
			return false;
		}

		public abstract void receiveRightClick(int x, int y, bool playSound = true);

		public virtual void receiveKeyPress(Keys key)
		{
			if (key == Keys.None)
			{
				return;
			}
			if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
			{
				this.exitThisMenu(true);
				return;
			}
			if (Game1.options.snappyMenus && Game1.options.gamepadControls && !this.overrideSnappyMenuCursorMovementBan())
			{
				this.applyMovementKey(key);
			}
		}

		public virtual void gamePadButtonHeld(Buttons b)
		{
		}

		public virtual ClickableComponent getCurrentlySnappedComponent()
		{
			return this.currentlySnappedComponent;
		}

		public virtual void receiveScrollWheelAction(int direction)
		{
		}

		public virtual void performHoverAction(int x, int y)
		{
			if (this.upperRightCloseButton != null)
			{
				this.upperRightCloseButton.tryHover(x, y, 0.5f);
			}
		}

		public virtual void draw(SpriteBatch b)
		{
			if (this.upperRightCloseButton != null)
			{
				this.upperRightCloseButton.draw(b);
			}
		}

		public virtual bool isWithinBounds(int x, int y)
		{
			return x - this.xPositionOnScreen < this.width && x - this.xPositionOnScreen >= 0 && y - this.yPositionOnScreen < this.height && y - this.yPositionOnScreen >= 0;
		}

		public virtual void update(GameTime time)
		{
		}

		public void exitThisMenuNoSound()
		{
			Game1.exitActiveMenu();
			if (this.exitFunction != null)
			{
				this.exitFunction();
			}
		}

		public void exitThisMenu(bool playSound = true)
		{
			if (playSound)
			{
				Game1.playSound("bigDeSelect");
			}
			Game1.exitActiveMenu();
			if (this.exitFunction != null)
			{
				this.exitFunction();
			}
		}

		public virtual bool autoCenterMouseCursorForGamepad()
		{
			return true;
		}

		public virtual void emergencyShutDown()
		{
		}

		public virtual bool readyToClose()
		{
			return true;
		}

		protected void drawHorizontalPartition(SpriteBatch b, int yPosition, bool small = false)
		{
			if (small)
			{
				b.Draw(Game1.menuTexture, new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, yPosition, this.width - Game1.tileSize, Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 25, -1, -1)), Color.White);
				return;
			}
			b.Draw(Game1.menuTexture, new Vector2((float)this.xPositionOnScreen, (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 4, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(this.xPositionOnScreen + Game1.tileSize, yPosition, this.width - Game1.tileSize * 2, Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 6, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)(this.xPositionOnScreen + this.width - Game1.tileSize), (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 7, -1, -1)), Color.White);
		}

		protected void drawVerticalPartition(SpriteBatch b, int xPosition, bool small = false)
		{
			if (small)
			{
				b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize + Game1.tileSize / 2, Game1.tileSize, this.height - Game1.tileSize * 2), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 26, -1, -1)), Color.White);
				return;
			}
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 1, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, this.height - Game1.tileSize * 3), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 5, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + this.height - Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 13, -1, -1)), Color.White);
		}

		protected void drawVerticalIntersectingPartition(SpriteBatch b, int xPosition, int yPosition)
		{
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 59, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(xPosition, yPosition + Game1.tileSize, Game1.tileSize, this.yPositionOnScreen + this.height - Game1.tileSize - yPosition - Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 63, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + this.height - Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 62, -1, -1)), Color.White);
		}

		protected void drawVerticalUpperIntersectingPartition(SpriteBatch b, int xPosition, int partitionHeight)
		{
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 44, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, partitionHeight - Game1.tileSize / 2), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 63, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + partitionHeight + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 39, -1, -1)), Color.White);
		}

		public static void drawTextureBox(SpriteBatch b, int x, int y, int width, int height, Color color)
		{
			IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, color, 1f, true);
		}

		public static void drawTextureBox(SpriteBatch b, Texture2D texture, Rectangle sourceRect, int x, int y, int width, int height, Color color, float scale = 1f, bool drawShadow = true)
		{
			int num = sourceRect.Width / 3;
			if (drawShadow)
			{
				b.Draw(texture, new Vector2((float)(x + width - (int)((float)num * scale) - Game1.pixelZoom * 2), (float)(y + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X + num * 2, sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Vector2((float)(x - Game1.pixelZoom * 2), (float)(y + height - (int)((float)num * scale) + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X, num * 2 + sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Vector2((float)(x + width - (int)((float)num * scale) - Game1.pixelZoom * 2), (float)(y + height - (int)((float)num * scale) + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num * 2 + sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x + (int)((float)num * scale) - Game1.pixelZoom * 2, y + Game1.pixelZoom * 2, width - (int)((float)num * scale) * 2, (int)((float)num * scale)), new Rectangle?(new Rectangle(sourceRect.X + num, sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x + (int)((float)num * scale) - Game1.pixelZoom * 2, y + height - (int)((float)num * scale) + Game1.pixelZoom * 2, width - (int)((float)num * scale) * 2, (int)((float)num * scale)), new Rectangle?(new Rectangle(sourceRect.X + num, num * 2 + sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x - Game1.pixelZoom * 2, y + (int)((float)num * scale) + Game1.pixelZoom * 2, (int)((float)num * scale), height - (int)((float)num * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X, num + sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x + width - (int)((float)num * scale) - Game1.pixelZoom * 2, y + (int)((float)num * scale) + Game1.pixelZoom * 2, (int)((float)num * scale), height - (int)((float)num * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num + sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle((int)((float)num * scale / 2f) + x - Game1.pixelZoom * 2, (int)((float)num * scale / 2f) + y + Game1.pixelZoom * 2, width - (int)((float)num * scale), height - (int)((float)num * scale)), new Rectangle?(new Rectangle(num + sourceRect.X, num + sourceRect.Y, num, num)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
			}
			b.Draw(texture, new Rectangle((int)((float)num * scale) + x, (int)((float)num * scale) + y, width - (int)((float)num * scale * 2f), height - (int)((float)num * scale * 2f)), new Rectangle?(new Rectangle(num + sourceRect.X, num + sourceRect.Y, num, num)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(sourceRect.X, sourceRect.Y, num, num)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)(x + width - (int)((float)num * scale)), (float)y), new Rectangle?(new Rectangle(sourceRect.X + num * 2, sourceRect.Y, num, num)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)x, (float)(y + height - (int)((float)num * scale))), new Rectangle?(new Rectangle(sourceRect.X, num * 2 + sourceRect.Y, num, num)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)(x + width - (int)((float)num * scale)), (float)(y + height - (int)((float)num * scale))), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num * 2 + sourceRect.Y, num, num)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x + (int)((float)num * scale), y, width - (int)((float)num * scale) * 2, (int)((float)num * scale)), new Rectangle?(new Rectangle(sourceRect.X + num, sourceRect.Y, num, num)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x + (int)((float)num * scale), y + height - (int)((float)num * scale), width - (int)((float)num * scale) * 2, (int)((float)num * scale)), new Rectangle?(new Rectangle(sourceRect.X + num, num * 2 + sourceRect.Y, num, num)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x, y + (int)((float)num * scale), (int)((float)num * scale), height - (int)((float)num * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X, num + sourceRect.Y, num, num)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x + width - (int)((float)num * scale), y + (int)((float)num * scale), (int)((float)num * scale), height - (int)((float)num * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num + sourceRect.Y, num, num)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
		}

		public void drawBorderLabel(SpriteBatch b, string text, SpriteFont font, int x, int y)
		{
			int num = (int)font.MeasureString(text).X;
			y += Game1.tileSize - Game1.pixelZoom * 3;
			b.Draw(Game1.mouseCursors, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(256, 267, 6, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(x + 6 * Game1.pixelZoom), (float)y), new Rectangle?(new Rectangle(262, 267, 1, 16)), Color.White, 0f, Vector2.Zero, new Vector2((float)num, (float)Game1.pixelZoom), SpriteEffects.None, 0.87f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(x + 6 * Game1.pixelZoom + num), (float)y), new Rectangle?(new Rectangle(263, 267, 6, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
			Utility.drawTextWithShadow(b, text, font, new Vector2((float)(x + 6 * Game1.pixelZoom), (float)(y + Game1.pixelZoom * 5)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
		}

		public static void drawToolTip(SpriteBatch b, string hoverText, string hoverTitle, Item hoveredItem, bool heldItem = false, int healAmountToDisplay = -1, int currencySymbol = 0, int extraItemToShowIndex = -1, int extraItemToShowAmount = -1, CraftingRecipe craftingIngredients = null, int moneyAmountToShowAtBottom = -1)
		{
			bool flag = hoveredItem != null && hoveredItem is StardewValley.Object && (hoveredItem as StardewValley.Object).edibility != -300;
			IClickableMenu.drawHoverText(b, hoverText, Game1.smallFont, heldItem ? (Game1.tileSize / 2 + 8) : 0, heldItem ? (Game1.tileSize / 2 + 8) : 0, moneyAmountToShowAtBottom, hoverTitle, flag ? (hoveredItem as StardewValley.Object).edibility : -1, (flag && Game1.objectInformation[(hoveredItem as StardewValley.Object).parentSheetIndex].Split(new char[]
			{
				'/'
			}).Length > 7) ? Game1.objectInformation[(hoveredItem as StardewValley.Object).parentSheetIndex].Split(new char[]
			{
				'/'
			})[7].Split(new char[]
			{
				' '
			}) : null, hoveredItem, currencySymbol, extraItemToShowIndex, extraItemToShowAmount, -1, -1, 1f, craftingIngredients);
		}

		public static void drawHoverText(SpriteBatch b, string text, SpriteFont font, int xOffset = 0, int yOffset = 0, int moneyAmountToDisplayAtBottom = -1, string boldTitleText = null, int healAmountToDisplay = -1, string[] buffIconsToDisplay = null, Item hoveredItem = null, int currencySymbol = 0, int extraItemToShowIndex = -1, int extraItemToShowAmount = -1, int overrideX = -1, int overrideY = -1, float alpha = 1f, CraftingRecipe craftingIngredients = null)
		{
			if (text == null || text.Length == 0)
			{
				return;
			}
			if (boldTitleText != null && boldTitleText.Length == 0)
			{
				boldTitleText = null;
			}
			int num = 20;
			int num2 = Math.Max((healAmountToDisplay != -1) ? ((int)font.MeasureString(healAmountToDisplay + "+ Energy" + Game1.tileSize / 2).X) : 0, Math.Max((int)font.MeasureString(text).X, (boldTitleText != null) ? ((int)Game1.dialogueFont.MeasureString(boldTitleText).X) : 0)) + Game1.tileSize / 2;
			int num3 = Math.Max(num * 3, (int)font.MeasureString(text).Y + Game1.tileSize / 2 + (int)((moneyAmountToDisplayAtBottom > -1) ? (font.MeasureString(string.Concat(moneyAmountToDisplayAtBottom)).Y + 4f) : 0f) + (int)((boldTitleText != null) ? (Game1.dialogueFont.MeasureString(boldTitleText).Y + (float)(Game1.tileSize / 4)) : 0f) + ((healAmountToDisplay != -1) ? 38 : 0));
			if (extraItemToShowIndex != -1)
			{
				string[] array = Game1.objectInformation[extraItemToShowIndex].Split(new char[]
				{
					'/'
				});
				string text2 = array[0];
				if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
				{
					text2 = array[array.Length - 1];
				}
				string text3 = Game1.content.LoadString("Strings\\UI:ItemHover_Requirements", new object[]
				{
					extraItemToShowAmount,
					text2
				});
				int num4 = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, extraItemToShowIndex, 16, 16).Width * 2 * Game1.pixelZoom;
				num2 = Math.Max(num2, num4 + (int)font.MeasureString(text3).X);
			}
			if (buffIconsToDisplay != null)
			{
				for (int i = 0; i < buffIconsToDisplay.Length; i++)
				{
					if (!buffIconsToDisplay[i].Equals("0"))
					{
						num3 += 34;
					}
				}
				num3 += 4;
			}
			string text4 = null;
			if (hoveredItem != null)
			{
				num3 += (Game1.tileSize + 4) * hoveredItem.attachmentSlots();
				text4 = hoveredItem.getCategoryName();
				if (text4.Length > 0)
				{
					num2 = Math.Max(num2, (int)font.MeasureString(text4).X + Game1.tileSize / 2);
					num3 += (int)font.MeasureString("T").Y;
				}
				int num5 = 9999;
				int num6 = 15 * Game1.pixelZoom + Game1.tileSize / 2;
				if (hoveredItem is MeleeWeapon)
				{
					num3 = Math.Max(num * 3, (int)((boldTitleText != null) ? (Game1.dialogueFont.MeasureString(boldTitleText).Y + (float)(Game1.tileSize / 4)) : 0f) + Game1.tileSize / 2) + (int)font.MeasureString("T").Y + (int)((moneyAmountToDisplayAtBottom > -1) ? (font.MeasureString(string.Concat(moneyAmountToDisplayAtBottom)).Y + 4f) : 0f);
					num3 += ((hoveredItem.Name == "Scythe") ? 0 : ((hoveredItem as MeleeWeapon).getNumberOfDescriptionCategories() * Game1.pixelZoom * 12));
					num3 += (int)font.MeasureString(Game1.parseText((hoveredItem as MeleeWeapon).description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
					num2 = (int)Math.Max((float)num2, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Damage", new object[]
					{
						num5,
						num5
					})).X + (float)num6, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Speed", new object[]
					{
						num5
					})).X + (float)num6, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", new object[]
					{
						num5
					})).X + (float)num6, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_CritChanceBonus", new object[]
					{
						num5
					})).X + (float)num6, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_CritPowerBonus", new object[]
					{
						num5
					})).X + (float)num6, font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Weight", new object[]
					{
						num5
					})).X + (float)num6))))));
				}
				else if (hoveredItem is Boots)
				{
					num3 -= (int)font.MeasureString(text).Y;
					num3 += (int)((float)((hoveredItem as Boots).getNumberOfDescriptionCategories() * Game1.pixelZoom * 12) + font.MeasureString(Game1.parseText((hoveredItem as Boots).description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y);
					num2 = (int)Math.Max((float)num2, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", new object[]
					{
						num5
					})).X + (float)num6, font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_ImmunityBonus", new object[]
					{
						num5
					})).X + (float)num6));
				}
				else if (hoveredItem is StardewValley.Object && (hoveredItem as StardewValley.Object).edibility != -300)
				{
					if (healAmountToDisplay == -1)
					{
						num3 += (Game1.tileSize / 2 + Game1.pixelZoom * 2) * ((healAmountToDisplay > 0) ? 2 : 1);
					}
					else
					{
						num3 += Game1.tileSize / 2 + Game1.pixelZoom * 2;
					}
					healAmountToDisplay = (int)Math.Ceiling((double)(hoveredItem as StardewValley.Object).Edibility * 2.5) + (hoveredItem as StardewValley.Object).quality * (hoveredItem as StardewValley.Object).Edibility;
					num2 = (int)Math.Max((float)num2, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Energy", new object[]
					{
						num5
					})).X + (float)num6, font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Health", new object[]
					{
						num5
					})).X + (float)num6));
				}
				if (buffIconsToDisplay != null)
				{
					for (int j = 0; j < buffIconsToDisplay.Length; j++)
					{
						if (!buffIconsToDisplay[j].Equals("0") && j <= 11)
						{
							num2 = (int)Math.Max((float)num2, font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Buff" + j, new object[]
							{
								num5
							})).X + (float)num6);
						}
					}
				}
			}
			if (craftingIngredients != null)
			{
				num2 = Math.Max((int)Game1.dialogueFont.MeasureString(boldTitleText).X + Game1.pixelZoom * 3, Game1.tileSize * 6);
				num3 += craftingIngredients.getDescriptionHeight(num2 - Game1.pixelZoom * 2) + ((healAmountToDisplay == -1) ? (-Game1.tileSize / 2) : 0) + Game1.pixelZoom * 3;
			}
			if (hoveredItem is FishingRod && moneyAmountToDisplayAtBottom > -1)
			{
				num3 += (int)font.MeasureString("T").Y;
			}
			int num7 = Game1.getOldMouseX() + Game1.tileSize / 2 + xOffset;
			int num8 = Game1.getOldMouseY() + Game1.tileSize / 2 + yOffset;
			if (overrideX != -1)
			{
				num7 = overrideX;
			}
			if (overrideY != -1)
			{
				num8 = overrideY;
			}
			if (num7 + num2 > Utility.getSafeArea().Right)
			{
				num7 = Utility.getSafeArea().Right - num2;
				num8 += Game1.tileSize / 4;
			}
			if (num8 + num3 > Utility.getSafeArea().Bottom)
			{
				num7 += Game1.tileSize / 4;
				if (num7 + num2 > Utility.getSafeArea().Right)
				{
					num7 = Utility.getSafeArea().Right - num2;
				}
				num8 = Utility.getSafeArea().Bottom - num3;
			}
			IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), num7, num8, num2 + ((craftingIngredients != null) ? (Game1.tileSize / 3) : 0), num3, Color.White * alpha, 1f, true);
			if (boldTitleText != null)
			{
				IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), num7, num8, num2 + ((craftingIngredients != null) ? (Game1.tileSize / 3) : 0), (int)Game1.dialogueFont.MeasureString(boldTitleText).Y + Game1.tileSize / 2 + (int)((hoveredItem != null && text4.Length > 0) ? font.MeasureString("asd").Y : 0f) - Game1.pixelZoom, Color.White * alpha, 1f, false);
				b.Draw(Game1.menuTexture, new Rectangle(num7 + Game1.pixelZoom * 3, num8 + (int)Game1.dialogueFont.MeasureString(boldTitleText).Y + Game1.tileSize / 2 + (int)((hoveredItem != null && text4.Length > 0) ? font.MeasureString("asd").Y : 0f) - Game1.pixelZoom, num2 - Game1.pixelZoom * ((craftingIngredients == null) ? 6 : 1), Game1.pixelZoom), new Rectangle?(new Rectangle(44, 300, 4, 4)), Color.White);
				b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor);
				b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), Game1.textShadowColor);
				b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)), Game1.textColor);
				num8 += (int)Game1.dialogueFont.MeasureString(boldTitleText).Y;
			}
			if (hoveredItem != null && text4.Length > 0)
			{
				num8 -= 4;
				Utility.drawTextWithShadow(b, text4, font, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)), hoveredItem.getCategoryColor(), 1f, -1f, 2, 2, 1f, 3);
				num8 += (int)font.MeasureString("T").Y + ((boldTitleText != null) ? (Game1.tileSize / 4) : 0) + Game1.pixelZoom;
			}
			else
			{
				num8 += ((boldTitleText != null) ? (Game1.tileSize / 4) : 0);
			}
			if (hoveredItem != null && hoveredItem is Boots)
			{
				Boots boots = hoveredItem as Boots;
				Utility.drawTextWithShadow(b, Game1.parseText(boots.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4), font, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				num8 += (int)font.MeasureString(Game1.parseText(boots.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
				if (boots.defenseBonus > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(110, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", new object[]
					{
						boots.defenseBonus
					}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
					num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
				}
				if (boots.immunityBonus > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(150, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_ImmunityBonus", new object[]
					{
						boots.immunityBonus
					}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
					num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
				}
			}
			else if (hoveredItem != null && hoveredItem is MeleeWeapon)
			{
				MeleeWeapon meleeWeapon = hoveredItem as MeleeWeapon;
				Utility.drawTextWithShadow(b, Game1.parseText(meleeWeapon.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4), font, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				num8 += (int)font.MeasureString(Game1.parseText(meleeWeapon.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
				if (meleeWeapon.indexOfMenuItemView != 47)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(120, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Damage", new object[]
					{
						meleeWeapon.minDamage,
						meleeWeapon.maxDamage
					}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
					num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					if (meleeWeapon.speed != ((meleeWeapon.type == 2) ? -8 : 0))
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(130, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						bool flag = (meleeWeapon.type == 2 && meleeWeapon.speed < -8) || (meleeWeapon.type != 2 && meleeWeapon.speed < 0);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Speed", new object[]
						{
							((((meleeWeapon.type == 2) ? (meleeWeapon.speed - -8) : meleeWeapon.speed) > 0) ? "+" : "") + ((meleeWeapon.type == 2) ? (meleeWeapon.speed - -8) : meleeWeapon.speed) / 2
						}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), flag ? Color.DarkRed : (Game1.textColor * 0.9f * alpha), 1f, -1f, -1, -1, 1f, 3);
						num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if (meleeWeapon.addedDefense > 0)
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(110, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", new object[]
						{
							meleeWeapon.addedDefense
						}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if ((double)meleeWeapon.critChance / 0.02 >= 2.0)
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(40, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_CritChanceBonus", new object[]
						{
							(int)((double)meleeWeapon.critChance / 0.02)
						}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if ((double)(meleeWeapon.critMultiplier - 3f) / 0.02 >= 1.0)
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(160, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_CritPowerBonus", new object[]
						{
							(int)((double)(meleeWeapon.critMultiplier - 3f) / 0.02)
						}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 11), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if (meleeWeapon.knockback != meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type))
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 4)), new Rectangle(70, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Weight", new object[]
						{
							(((float)((int)Math.Ceiling((double)(Math.Abs(meleeWeapon.knockback - meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type)) * 10f))) > meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type)) ? "+" : "") + (int)Math.Ceiling((double)(Math.Abs(meleeWeapon.knockback - meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type)) * 10f))
						}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(num8 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						num8 += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
				}
			}
			else if (!string.IsNullOrEmpty(text) && text != " ")
			{
				b.DrawString(font, text, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor * alpha);
				b.DrawString(font, text, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), Game1.textShadowColor * alpha);
				b.DrawString(font, text, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0f), Game1.textShadowColor * alpha);
				b.DrawString(font, text, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)), Game1.textColor * 0.9f * alpha);
				num8 += (int)font.MeasureString(text).Y + 4;
			}
			if (craftingIngredients != null)
			{
				craftingIngredients.drawRecipeDescription(b, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 - Game1.pixelZoom * 2)), num2);
				num8 += craftingIngredients.getDescriptionHeight(num2);
			}
			if (healAmountToDisplay != -1)
			{
				if (healAmountToDisplay > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4)), new Rectangle((healAmountToDisplay < 0) ? 140 : 0, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Energy", new object[]
					{
						((healAmountToDisplay > 0) ? "+" : "") + healAmountToDisplay
					}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					num8 += 34;
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4)), new Rectangle(0, 438, 10, 10), Color.White, 0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Health", new object[]
					{
						((healAmountToDisplay > 0) ? "+" : "") + (int)((float)healAmountToDisplay * 0.4f)
					}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					num8 += 34;
				}
				else if (healAmountToDisplay != -300)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4)), new Rectangle(140, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Energy", new object[]
					{
						string.Concat(healAmountToDisplay)
					}), font, new Vector2((float)(num7 + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					num8 += 34;
				}
			}
			if (buffIconsToDisplay != null)
			{
				for (int k = 0; k < buffIconsToDisplay.Length; k++)
				{
					if (!buffIconsToDisplay[k].Equals("0"))
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 4 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4)), new Rectangle(10 + k * 10, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
						string text5 = ((Convert.ToInt32(buffIconsToDisplay[k]) > 0) ? "+" : "") + buffIconsToDisplay[k] + " ";
						if (k <= 11)
						{
							text5 = Game1.content.LoadString("Strings\\UI:ItemHover_Buff" + k, new object[]
							{
								text5
							});
						}
						Utility.drawTextWithShadow(b, text5, font, new Vector2((float)(num7 + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float)(num8 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
						num8 += 34;
					}
				}
			}
			if (hoveredItem != null && hoveredItem.attachmentSlots() > 0)
			{
				num8 += 16;
				hoveredItem.drawAttachments(b, num7 + Game1.tileSize / 4, num8);
				if (moneyAmountToDisplayAtBottom > -1)
				{
					num8 += Game1.tileSize * hoveredItem.attachmentSlots();
				}
			}
			if (moneyAmountToDisplayAtBottom > -1)
			{
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor);
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), Game1.textShadowColor);
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0f), Game1.textShadowColor);
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.tileSize / 4 + 4)), Game1.textColor);
				if (currencySymbol == 0)
				{
					b.Draw(Game1.debrisSpriteSheet, new Vector2((float)(num7 + Game1.tileSize / 4) + font.MeasureString(string.Concat(moneyAmountToDisplayAtBottom)).X + 20f, (float)(num8 + Game1.tileSize / 4 + 16)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 0.95f);
				}
				else if (currencySymbol == 1)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 8) + font.MeasureString(string.Concat(moneyAmountToDisplayAtBottom)).X + 20f, (float)(num8 + Game1.tileSize / 4 - 5)), new Rectangle?(new Rectangle(338, 400, 8, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				else if (currencySymbol == 2)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(num7 + Game1.tileSize / 8) + font.MeasureString(string.Concat(moneyAmountToDisplayAtBottom)).X + 20f, (float)(num8 + Game1.tileSize / 4 - 7)), new Rectangle?(new Rectangle(211, 373, 9, 10)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				num8 += Game1.tileSize * 3 / 4;
			}
			if (extraItemToShowIndex != -1)
			{
				IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), num7, num8 + Game1.pixelZoom, num2, Game1.tileSize * 3 / 2, Color.White, 1f, true);
				num8 += Game1.pixelZoom * 5;
				string text6 = Game1.objectInformation[extraItemToShowIndex].Split(new char[]
				{
					'/'
				})[4];
				string text7 = Game1.content.LoadString("Strings\\UI:ItemHover_Requirements", new object[]
				{
					extraItemToShowAmount,
					text6
				});
				b.DrawString(font, text7, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.pixelZoom)) + new Vector2(2f, 2f), Game1.textShadowColor);
				b.DrawString(font, text7, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.pixelZoom)) + new Vector2(0f, 2f), Game1.textShadowColor);
				b.DrawString(font, text7, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.pixelZoom)) + new Vector2(2f, 0f), Game1.textShadowColor);
				b.DrawString(Game1.smallFont, text7, new Vector2((float)(num7 + Game1.tileSize / 4), (float)(num8 + Game1.pixelZoom)), Game1.textColor);
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(num7 + Game1.tileSize / 4 + (int)font.MeasureString(text7).X + Game1.tileSize / 3), (float)num8), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, extraItemToShowIndex, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
		}
	}
}
