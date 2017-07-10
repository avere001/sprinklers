using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class InventoryMenu : IClickableMenu
	{
		public delegate bool highlightThisItem(Item i);

		public const int region_inventorySlot0 = 0;

		public const int region_inventorySlot1 = 1;

		public const int region_inventorySlot2 = 2;

		public const int region_inventorySlot3 = 3;

		public const int region_inventorySlot4 = 4;

		public const int region_inventorySlot5 = 5;

		public const int region_inventorySlot6 = 6;

		public const int region_inventorySlot7 = 7;

		public const int region_inventorySlot8 = 8;

		public const int region_inventorySlot9 = 9;

		public const int region_inventorySlot10 = 10;

		public const int region_inventorySlot11 = 11;

		public const int region_inventorySlot12 = 12;

		public const int region_inventorySlot13 = 13;

		public const int region_inventorySlot14 = 14;

		public const int region_inventorySlot15 = 15;

		public const int region_inventorySlot16 = 16;

		public const int region_inventorySlot17 = 17;

		public const int region_inventorySlot18 = 18;

		public const int region_inventorySlot19 = 19;

		public const int region_inventorySlot20 = 20;

		public const int region_inventorySlot21 = 21;

		public const int region_inventorySlot22 = 22;

		public const int region_inventorySlot23 = 23;

		public const int region_inventorySlot24 = 24;

		public const int region_inventorySlot25 = 25;

		public const int region_inventorySlot26 = 26;

		public const int region_inventorySlot27 = 27;

		public const int region_inventorySlot28 = 28;

		public const int region_inventorySlot29 = 29;

		public const int region_inventorySlot30 = 30;

		public const int region_inventorySlot31 = 31;

		public const int region_inventorySlot32 = 32;

		public const int region_inventorySlot33 = 33;

		public const int region_inventorySlot34 = 34;

		public const int region_inventorySlot35 = 35;

		public const int region_inventoryArea = 9000;

		public string hoverText = "";

		public string hoverTitle = "";

		public string descriptionTitle = "";

		public string descriptionText = "";

		public List<ClickableComponent> inventory = new List<ClickableComponent>();

		public List<Item> actualInventory;

		public InventoryMenu.highlightThisItem highlightMethod;

		public ItemGrabMenu.behaviorOnItemSelect onAddItem;

		public bool playerInventory;

		public bool drawSlots;

		public bool showGrayedOutSlots;

		public int capacity;

		public int rows;

		public int horizontalGap;

		public int verticalGap;

		public InventoryMenu(int xPosition, int yPosition, bool playerInventory, List<Item> actualInventory = null, InventoryMenu.highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true) : base(xPosition, yPosition, Game1.tileSize * (((capacity == -1) ? 36 : capacity) / rows), Game1.tileSize * rows + Game1.tileSize / 4, false)
		{
			this.drawSlots = drawSlots;
			this.horizontalGap = horizontalGap;
			this.verticalGap = verticalGap;
			this.rows = rows;
			this.capacity = ((capacity == -1) ? 36 : capacity);
			this.playerInventory = playerInventory;
			this.actualInventory = actualInventory;
			if (actualInventory == null)
			{
				this.actualInventory = Game1.player.items;
			}
			for (int i = 0; i < Game1.player.maxItems; i++)
			{
				if (Game1.player.items.Count <= i)
				{
					Game1.player.items.Add(null);
				}
			}
			for (int j = 0; j < this.actualInventory.Count; j++)
			{
				this.inventory.Add(new ClickableComponent(new Rectangle(xPosition + j % (this.capacity / rows) * Game1.tileSize + horizontalGap * (j % (this.capacity / rows)), this.yPositionOnScreen + j / (this.capacity / rows) * (Game1.tileSize + verticalGap) + (j / (this.capacity / rows) - 1) * Game1.pixelZoom - ((j > this.capacity / rows || !playerInventory || verticalGap != 0) ? 0 : (Game1.tileSize / 5)), Game1.tileSize, Game1.tileSize), string.Concat(j))
				{
					myID = j,
					leftNeighborID = ((j % (this.capacity / rows) != 0) ? (j - 1) : -1),
					rightNeighborID = (((j + 1) % (this.capacity / rows) != 0) ? (j + 1) : 106),
					downNeighborID = ((j >= this.actualInventory.Count - this.capacity / rows) ? 101 : (j + this.capacity / rows)),
					upNeighborID = ((j < this.capacity / rows) ? (12340 + j) : (j - this.capacity / rows)),
					region = 9000
				});
			}
			this.highlightMethod = highlightMethod;
			if (highlightMethod == null)
			{
				this.highlightMethod = new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems);
			}
		}

		public static bool highlightAllItems(Item i)
		{
			return true;
		}

		public void movePosition(int x, int y)
		{
			this.xPositionOnScreen += x;
			this.yPositionOnScreen += y;
			foreach (ClickableComponent expr_31 in this.inventory)
			{
				expr_31.bounds.X = expr_31.bounds.X + x;
				expr_31.bounds.Y = expr_31.bounds.Y + y;
			}
		}

		public Item tryToAddItem(Item toPlace, string sound = "coin")
		{
			if (toPlace == null)
			{
				return null;
			}
			int stack = toPlace.Stack;
			using (List<ClickableComponent>.Enumerator enumerator = this.inventory.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = Convert.ToInt32(enumerator.Current.name);
					if (num < this.actualInventory.Count && this.actualInventory[num] != null && this.highlightMethod(this.actualInventory[num]) && this.actualInventory[num].canStackWith(toPlace))
					{
						toPlace.Stack = this.actualInventory[num].addToStack(toPlace.Stack);
						if (toPlace.Stack <= 0)
						{
							try
							{
								Game1.playSound(sound);
								if (this.onAddItem != null)
								{
									this.onAddItem(toPlace, this.playerInventory ? Game1.player : null);
								}
							}
							catch (Exception)
							{
							}
							Item result = null;
							return result;
						}
					}
				}
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.inventory.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num2 = Convert.ToInt32(enumerator.Current.name);
					if (num2 < this.actualInventory.Count && (this.actualInventory[num2] == null || this.highlightMethod(this.actualInventory[num2])) && this.actualInventory[num2] == null)
					{
						try
						{
							Game1.playSound(sound);
						}
						catch (Exception)
						{
						}
						Item result = Utility.addItemToInventory(toPlace, num2, this.actualInventory, this.onAddItem);
						return result;
					}
				}
			}
			if (toPlace.Stack < stack)
			{
				Game1.playSound(sound);
			}
			return toPlace;
		}

		public int getInventoryPositionOfClick(int x, int y)
		{
			for (int i = 0; i < this.inventory.Count; i++)
			{
				if (this.inventory[i] != null && this.inventory[i].bounds.Contains(x, y))
				{
					return Convert.ToInt32(this.inventory[i].name);
				}
			}
			return -1;
		}

		public Item leftClick(int x, int y, Item toPlace, bool playSound = true)
		{
			foreach (ClickableComponent current in this.inventory)
			{
				if (current.containsPoint(x, y))
				{
					int num = Convert.ToInt32(current.name);
					if (num < this.actualInventory.Count && (this.actualInventory[num] == null || this.highlightMethod(this.actualInventory[num]) || this.actualInventory[num].canStackWith(toPlace)))
					{
						if (this.actualInventory[num] != null)
						{
							Item result;
							if (toPlace != null)
							{
								if (playSound)
								{
									Game1.playSound("stoneStep");
								}
								result = Utility.addItemToInventory(toPlace, num, this.actualInventory, this.onAddItem);
								return result;
							}
							if (playSound)
							{
								Game1.playSound("dwop");
							}
							result = Utility.removeItemFromInventory(num, this.actualInventory);
							return result;
						}
						else if (toPlace != null)
						{
							if (playSound)
							{
								Game1.playSound("stoneStep");
							}
							Item result = Utility.addItemToInventory(toPlace, num, this.actualInventory, this.onAddItem);
							return result;
						}
					}
				}
			}
			return toPlace;
		}

		public Vector2 snapToClickableComponent(int x, int y)
		{
			foreach (ClickableComponent current in this.inventory)
			{
				if (current.containsPoint(x, y))
				{
					return new Vector2((float)current.bounds.X, (float)current.bounds.Y);
				}
			}
			return new Vector2((float)x, (float)y);
		}

		public Item getItemAt(int x, int y)
		{
			foreach (ClickableComponent current in this.inventory)
			{
				if (current.containsPoint(x, y))
				{
					return this.getItemFromClickableComponent(current);
				}
			}
			return null;
		}

		public Item getItemFromClickableComponent(ClickableComponent c)
		{
			if (c != null)
			{
				int num = Convert.ToInt32(c.name);
				if (num < this.actualInventory.Count)
				{
					return this.actualInventory[num];
				}
			}
			return null;
		}

		public Item rightClick(int x, int y, Item toAddTo, bool playSound = true)
		{
			foreach (ClickableComponent expr_18 in this.inventory)
			{
				int num = Convert.ToInt32(expr_18.name);
				if (expr_18.containsPoint(x, y) && (this.actualInventory[num] == null || this.highlightMethod(this.actualInventory[num])) && num < this.actualInventory.Count && this.actualInventory[num] != null)
				{
					if (this.actualInventory[num] is Tool && (toAddTo == null || toAddTo is StardewValley.Object) && (this.actualInventory[num] as Tool).canThisBeAttached((StardewValley.Object)toAddTo))
					{
						Item result = (this.actualInventory[num] as Tool).attach((toAddTo == null) ? null : ((StardewValley.Object)toAddTo));
						return result;
					}
					if (toAddTo == null)
					{
						if (this.actualInventory[num].maximumStackSize() != -1)
						{
							if (num == Game1.player.CurrentToolIndex && this.actualInventory[num] != null && this.actualInventory[num].Stack == 1)
							{
								this.actualInventory[num].actionWhenStopBeingHeld(Game1.player);
							}
							Item one = this.actualInventory[num].getOne();
							if (this.actualInventory[num].Stack > 1 && Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
							{
								new InputButton(Keys.LeftShift)
							}))
							{
								one.Stack = (int)Math.Ceiling((double)this.actualInventory[num].Stack / 2.0);
								this.actualInventory[num].Stack = this.actualInventory[num].Stack / 2;
							}
							else if (this.actualInventory[num].Stack == 1)
							{
								this.actualInventory[num] = null;
							}
							else
							{
								Item expr_208 = this.actualInventory[num];
								int stack = expr_208.Stack;
								expr_208.Stack = stack - 1;
							}
							if (this.actualInventory[num] != null && this.actualInventory[num].Stack <= 0)
							{
								this.actualInventory[num] = null;
							}
							if (playSound)
							{
								Game1.playSound("dwop");
							}
							Item result = one;
							return result;
						}
					}
					else if (this.actualInventory[num].canStackWith(toAddTo) && toAddTo.Stack < toAddTo.maximumStackSize())
					{
						if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
						{
							new InputButton(Keys.LeftShift)
						}))
						{
							toAddTo.Stack += (int)Math.Ceiling((double)this.actualInventory[num].Stack / 2.0);
							this.actualInventory[num].Stack = this.actualInventory[num].Stack / 2;
						}
						else
						{
							int stack = toAddTo.Stack;
							toAddTo.Stack = stack + 1;
							Item expr_31B = this.actualInventory[num];
							stack = expr_31B.Stack;
							expr_31B.Stack = stack - 1;
						}
						if (playSound)
						{
							Game1.playSound("dwop");
						}
						if (this.actualInventory[num].Stack <= 0)
						{
							if (num == Game1.player.CurrentToolIndex)
							{
								this.actualInventory[num].actionWhenStopBeingHeld(Game1.player);
							}
							this.actualInventory[num] = null;
						}
						Item result = toAddTo;
						return result;
					}
				}
			}
			return toAddTo;
		}

		public Item hover(int x, int y, Item heldItem)
		{
			this.descriptionText = "";
			this.descriptionTitle = "";
			this.hoverText = "";
			this.hoverTitle = "";
			Item item = null;
			foreach (ClickableComponent current in this.inventory)
			{
				int num = Convert.ToInt32(current.name);
				current.scale = Math.Max(1f, current.scale - 0.025f);
				if (current.containsPoint(x, y) && (this.actualInventory[num] == null || this.highlightMethod(this.actualInventory[num])) && num < this.actualInventory.Count && this.actualInventory[num] != null)
				{
					this.descriptionTitle = this.actualInventory[num].DisplayName;
					this.descriptionText = Environment.NewLine + this.actualInventory[num].getDescription();
					current.scale = Math.Min(current.scale + 0.05f, 1.1f);
					string hoverBoxText = this.actualInventory[num].getHoverBoxText(heldItem);
					if (hoverBoxText != null)
					{
						this.hoverText = hoverBoxText;
					}
					else
					{
						this.hoverText = this.actualInventory[num].getDescription();
						this.hoverTitle = this.actualInventory[num].DisplayName;
					}
					if (item == null)
					{
						item = this.actualInventory[num];
					}
				}
			}
			return item;
		}

		public override void setUpForGamePadMode()
		{
			base.setUpForGamePadMode();
			if (this.inventory != null && this.inventory.Count > 0)
			{
				Game1.setMousePosition(this.inventory[0].bounds.Right - this.inventory[0].bounds.Width / 8, this.inventory[0].bounds.Bottom - this.inventory[0].bounds.Height / 8);
			}
		}

		public override void draw(SpriteBatch b)
		{
			if (this.drawSlots)
			{
				for (int i = 0; i < this.capacity; i++)
				{
					Vector2 vector = new Vector2((float)(this.xPositionOnScreen + i % (this.capacity / this.rows) * Game1.tileSize + this.horizontalGap * (i % (this.capacity / this.rows))), (float)(this.yPositionOnScreen + i / (this.capacity / this.rows) * (Game1.tileSize + this.verticalGap) + (i / (this.capacity / this.rows) - 1) * Game1.pixelZoom - ((i >= this.capacity / this.rows || !this.playerInventory || this.verticalGap != 0) ? 0 : (Game1.tileSize / 5))));
					b.Draw(Game1.menuTexture, vector, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
					if ((this.playerInventory || this.showGrayedOutSlots) && i >= Game1.player.maxItems)
					{
						b.Draw(Game1.menuTexture, vector, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 57, -1, -1)), Color.White * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
					}
					if (i < 12 && this.playerInventory)
					{
						string text = (i == 9) ? "0" : ((i == 10) ? "-" : ((i == 11) ? "=" : string.Concat(i + 1)));
						Vector2 vector2 = Game1.tinyFont.MeasureString(text);
						b.DrawString(Game1.tinyFont, text, vector + new Vector2((float)Game1.tileSize / 2f - vector2.X / 2f, -vector2.Y), (i == Game1.player.CurrentToolIndex) ? Color.Red : Color.DimGray);
					}
					if (this.actualInventory.Count > i && this.actualInventory.ElementAt(i) != null)
					{
						this.actualInventory[i].drawInMenu(b, vector, (this.inventory.Count > i) ? this.inventory[i].scale : 1f, (!this.highlightMethod(this.actualInventory[i])) ? 0.2f : 1f, 0.865f);
					}
				}
			}
			for (int j = 0; j < this.capacity; j++)
			{
				Vector2 location = new Vector2((float)(this.xPositionOnScreen + j % (this.capacity / this.rows) * Game1.tileSize + this.horizontalGap * (j % (this.capacity / this.rows))), (float)(this.yPositionOnScreen + j / (this.capacity / this.rows) * (Game1.tileSize + this.verticalGap) + (j / (this.capacity / this.rows) - 1) * Game1.pixelZoom - ((j >= this.capacity / this.rows || !this.playerInventory || this.verticalGap != 0) ? 0 : (Game1.tileSize / 5))));
				if (this.actualInventory.Count > j && this.actualInventory.ElementAt(j) != null)
				{
					this.actualInventory[j].drawInMenu(b, location, (this.inventory.Count > j) ? this.inventory[j].scale : 1f, (!this.highlightMethod(this.actualInventory[j])) ? 0.2f : 1f, 0.865f);
				}
			}
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
		}
	}
}
