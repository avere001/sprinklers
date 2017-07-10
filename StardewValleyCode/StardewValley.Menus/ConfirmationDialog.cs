using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
	public class ConfirmationDialog : IClickableMenu
	{
		public delegate void behavior(Farmer who);

		public const int region_okButton = 101;

		public const int region_cancelButton = 102;

		private string message;

		public ClickableTextureComponent okButton;

		public ClickableTextureComponent cancelButton;

		private ConfirmationDialog.behavior onConfirm;

		private ConfirmationDialog.behavior onCancel;

		private bool active = true;

		public ConfirmationDialog(string message, ConfirmationDialog.behavior onConfirm, ConfirmationDialog.behavior onCancel = null) : base(Game1.viewport.Width / 2 - (int)Game1.dialogueFont.MeasureString(message).X / 2 - IClickableMenu.borderWidth, Game1.viewport.Height / 2 - (int)Game1.dialogueFont.MeasureString(message).Y / 2, (int)Game1.dialogueFont.MeasureString(message).X + IClickableMenu.borderWidth * 2, (int)Game1.dialogueFont.MeasureString(message).Y + IClickableMenu.borderWidth * 2 + Game1.tileSize * 5 / 2, false)
		{
			if (onCancel == null)
			{
				onCancel = new ConfirmationDialog.behavior(this.closeDialog);
			}
			else
			{
				this.onCancel = onCancel;
			}
			this.onConfirm = onConfirm;
			this.message = message;
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
			{
				myID = 101,
				rightNeighborID = 102
			};
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false)
			{
				myID = 102,
				leftNeighborID = 101
			};
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public void closeDialog(Farmer who)
		{
			Game1.exitActiveMenu();
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(102);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.active)
			{
				if (this.okButton.containsPoint(x, y))
				{
					if (this.onConfirm != null)
					{
						this.onConfirm(Game1.player);
					}
					Game1.playSound("smallSelect");
					this.active = false;
				}
				if (this.cancelButton.containsPoint(x, y))
				{
					if (this.onCancel != null)
					{
						this.onCancel(Game1.player);
					}
					else
					{
						Game1.exitActiveMenu();
					}
					Game1.playSound("bigDeSelect");
				}
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (this.active && Game1.activeClickableMenu == null && this.onCancel != null)
			{
				this.onCancel(Game1.player);
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
		}

		public override void performHoverAction(int x, int y)
		{
			if (this.okButton.containsPoint(x, y))
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
				return;
			}
			this.cancelButton.scale = Math.Max(this.cancelButton.scale - 0.02f, this.cancelButton.baseScale);
		}

		public override void draw(SpriteBatch b)
		{
			if (this.active)
			{
				b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
				b.DrawString(Game1.dialogueFont, this.message, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2)), Game1.textColor);
				this.okButton.draw(b);
				this.cancelButton.draw(b);
				base.drawMouse(b);
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}
	}
}
