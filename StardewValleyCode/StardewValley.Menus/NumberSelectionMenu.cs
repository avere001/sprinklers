using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
	public class NumberSelectionMenu : IClickableMenu
	{
		public delegate void behaviorOnNumberSelect(int number, int price, Farmer who);

		public const int region_leftButton = 101;

		public const int region_rightButton = 102;

		public const int region_okButton = 103;

		public const int region_cancelButton = 104;

		private string message;

		private int price;

		private int minValue;

		private int maxValue;

		private int currentValue;

		private int priceShake;

		private int heldTimer;

		private NumberSelectionMenu.behaviorOnNumberSelect behaviorFunction;

		private TextBox numberSelectedBox;

		public ClickableTextureComponent leftButton;

		public ClickableTextureComponent rightButton;

		public ClickableTextureComponent okButton;

		public ClickableTextureComponent cancelButton;

		public NumberSelectionMenu(string message, NumberSelectionMenu.behaviorOnNumberSelect behaviorOnSelection, int price = -1, int minValue = 0, int maxValue = 99, int defaultNumber = 0)
		{
			Vector2 expr_11 = Game1.dialogueFont.MeasureString(message);
			int num = Math.Max((int)expr_11.X, 600) + IClickableMenu.borderWidth * 2;
			int num2 = (int)expr_11.Y + IClickableMenu.borderWidth * 2 + Game1.tileSize * 5 / 2;
			int x = Game1.viewport.Width / 2 - num / 2;
			int y = Game1.viewport.Height / 2 - num2 / 2;
			base.initialize(x, y, num, num2, false);
			this.message = message;
			this.price = price;
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.currentValue = defaultNumber;
			this.behaviorFunction = behaviorOnSelection;
			this.numberSelectedBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + IClickableMenu.borderWidth + 14 * Game1.pixelZoom,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2,
				Text = string.Concat(this.currentValue),
				numbersOnly = true,
				textLimit = string.Concat(maxValue).Length
			};
			this.numberSelectedBox.SelectMe();
			this.leftButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 101,
				rightNeighborID = 102
			};
			this.rightButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + 16 * Game1.pixelZoom + this.numberSelectedBox.Width, this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 102,
				leftNeighborID = 101,
				rightNeighborID = 103
			};
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
			{
				myID = 103,
				leftNeighborID = 102,
				rightNeighborID = 104
			};
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false)
			{
				myID = 104,
				leftNeighborID = 103
			};
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(102);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void gamePadButtonHeld(Buttons b)
		{
			base.gamePadButtonHeld(b);
			if (b == Buttons.A && this.currentlySnappedComponent != null)
			{
				this.heldTimer += Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (this.heldTimer > 300)
				{
					if (this.currentlySnappedComponent.myID == 102)
					{
						int num = this.currentValue + 1;
						if (num <= this.maxValue && (this.price == -1 || num * this.price <= Game1.player.Money))
						{
							this.rightButton.scale = this.rightButton.baseScale;
							this.currentValue = num;
							this.numberSelectedBox.Text = string.Concat(this.currentValue);
							return;
						}
					}
					else if (this.currentlySnappedComponent.myID == 101)
					{
						int num2 = this.currentValue - 1;
						if (num2 >= this.minValue)
						{
							this.leftButton.scale = this.leftButton.baseScale;
							this.currentValue = num2;
							this.numberSelectedBox.Text = string.Concat(this.currentValue);
						}
					}
				}
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.leftButton.containsPoint(x, y))
			{
				int num = this.currentValue - 1;
				if (num >= this.minValue)
				{
					this.leftButton.scale = this.leftButton.baseScale;
					this.currentValue = num;
					this.numberSelectedBox.Text = string.Concat(this.currentValue);
					Game1.playSound("smallSelect");
				}
			}
			if (this.rightButton.containsPoint(x, y))
			{
				int num2 = this.currentValue + 1;
				if (num2 <= this.maxValue && (this.price == -1 || num2 * this.price <= Game1.player.Money))
				{
					this.rightButton.scale = this.rightButton.baseScale;
					this.currentValue = num2;
					this.numberSelectedBox.Text = string.Concat(this.currentValue);
					Game1.playSound("smallSelect");
				}
			}
			if (this.okButton.containsPoint(x, y))
			{
				if (this.currentValue > this.maxValue || this.currentValue < this.minValue)
				{
					this.currentValue = Math.Max(this.minValue, Math.Min(this.maxValue, this.currentValue));
					this.numberSelectedBox.Text = string.Concat(this.currentValue);
				}
				else
				{
					this.behaviorFunction(this.currentValue, this.price, Game1.player);
				}
				Game1.playSound("smallSelect");
			}
			if (this.cancelButton.containsPoint(x, y))
			{
				Game1.exitActiveMenu();
				Game1.playSound("bigDeSelect");
				Game1.player.canMove = true;
			}
			this.numberSelectedBox.Update();
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (key == Keys.Enter)
			{
				this.receiveLeftClick(this.okButton.bounds.Center.X, this.okButton.bounds.Center.Y, true);
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			this.currentValue = 0;
			if (this.numberSelectedBox.Text != null)
			{
				int.TryParse(this.numberSelectedBox.Text, out this.currentValue);
			}
			if (this.priceShake > 0)
			{
				this.priceShake -= time.ElapsedGameTime.Milliseconds;
			}
			if (Game1.options.SnappyMenus)
			{
				GamePadState arg_67_0 = Game1.oldPadState;
				if (!Game1.oldPadState.IsButtonDown(Buttons.A))
				{
					this.heldTimer = 0;
				}
			}
		}

		public override void performHoverAction(int x, int y)
		{
			if (this.okButton.containsPoint(x, y) && (this.price == -1 || this.currentValue > this.minValue))
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.2f);
			}
			else
			{
				this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
			}
			if (this.cancelButton.containsPoint(x, y))
			{
				this.cancelButton.scale = Math.Min(this.cancelButton.scale + 0.02f, this.cancelButton.baseScale + 0.2f);
			}
			else
			{
				this.cancelButton.scale = Math.Max(this.cancelButton.scale - 0.02f, this.cancelButton.baseScale);
			}
			if (this.leftButton.containsPoint(x, y))
			{
				this.leftButton.scale = Math.Min(this.leftButton.scale + 0.02f, this.leftButton.baseScale + 0.2f);
			}
			else
			{
				this.leftButton.scale = Math.Max(this.leftButton.scale - 0.02f, this.leftButton.baseScale);
			}
			if (this.rightButton.containsPoint(x, y))
			{
				this.rightButton.scale = Math.Min(this.rightButton.scale + 0.02f, this.rightButton.baseScale + 0.2f);
				return;
			}
			this.rightButton.scale = Math.Max(this.rightButton.scale - 0.02f, this.rightButton.baseScale);
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			b.DrawString(Game1.dialogueFont, this.message, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2)), Game1.textColor);
			this.okButton.draw(b);
			this.cancelButton.draw(b);
			this.leftButton.draw(b);
			this.rightButton.draw(b);
			if (this.price != -1)
			{
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
				{
					this.price * this.currentValue
				}), new Vector2((float)(this.rightButton.bounds.Right + Game1.tileSize / 2 + ((this.priceShake > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(this.rightButton.bounds.Y + ((this.priceShake > 0) ? Game1.random.Next(-1, 2) : 0))), (this.currentValue * this.price > Game1.player.Money) ? Color.Red : Game1.textColor);
			}
			this.numberSelectedBox.Draw(b);
			base.drawMouse(b);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}
	}
}
