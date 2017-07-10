using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Menus
{
	public class AddNewItemMenu : IClickableMenu
	{
		private InventoryMenu playerInventory;

		private ClickableComponent garbage;

		public AddNewItemMenu() : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (300 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 300 + IClickableMenu.borderWidth * 2, false)
		{
			this.playerInventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder, true, null, null, -1, 3, 0, 0, true);
			this.garbage = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + this.height - Game1.tileSize, Game1.tileSize, Game1.tileSize), "Garbage");
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

		public override void draw(SpriteBatch b)
		{
		}
	}
}
