using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
	internal class DebugInputMenu : NamingMenu
	{
		public DebugInputMenu(NamingMenu.doneNamingBehavior b) : base(b, "Debug Input:", "")
		{
			this.textBox.limitWidth = false;
			this.textBox.Width = Game1.tileSize * 8;
			this.textBox.X -= Game1.tileSize * 2;
			ClickableTextureComponent expr_58_cp_0_cp_0 = this.randomButton;
			expr_58_cp_0_cp_0.bounds.X = expr_58_cp_0_cp_0.bounds.X + Game1.tileSize * 2;
			ClickableTextureComponent expr_73_cp_0_cp_0 = this.doneNamingButton;
			expr_73_cp_0_cp_0.bounds.X = expr_73_cp_0_cp_0.bounds.X + Game1.tileSize * 2;
			this.minLength = 0;
		}

		public override void update(GameTime time)
		{
			GamePadState state = GamePad.GetState(Game1.playerOneIndex);
			KeyboardState state2 = Keyboard.GetState();
			if (Game1.IsPressEvent(ref state, Buttons.B) || Game1.IsPressEvent(ref state2, Keys.Escape))
			{
				Game1.exitActiveMenu();
				Game1.lastDebugInput = this.textBox.Text;
			}
			base.update(time);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.randomButton.containsPoint(x, y))
			{
				this.textBox.Text = Game1.lastDebugInput;
				this.randomButton.scale = this.randomButton.baseScale;
				Game1.playSound("drumkit6");
				return;
			}
			base.receiveLeftClick(x, y, playSound);
		}
	}
}
