using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class OptionsInputListener : OptionsElement
	{
		public List<string> buttonNames = new List<string>();

		private string listenerMessage;

		private bool listening;

		private Rectangle setbuttonBounds;

		public static Rectangle setButtonSource = new Rectangle(294, 428, 21, 11);

		public OptionsInputListener(string label, int whichOption, int slotWidth, int x = -1, int y = -1) : base(label, x, y, slotWidth - x, 11 * Game1.pixelZoom, whichOption)
		{
			this.setbuttonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, y + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
			if (whichOption != -1)
			{
				Game1.options.setInputListenerToProperValue(this);
			}
		}

		public override void leftClickHeld(int x, int y)
		{
			bool arg_06_0 = this.greyedOut;
		}

		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut && !this.listening && this.setbuttonBounds.Contains(x, y))
			{
				if (this.whichOption == -1)
				{
					Game1.options.setControlsToDefault();
					Game1.activeClickableMenu = new GameMenu(6, 17);
					return;
				}
				this.listening = true;
				Game1.playSound("breathin");
				GameMenu.forcePreventClose = true;
				this.listenerMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsElement.cs.11225", new object[0]);
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			if (!this.greyedOut && this.listening)
			{
				if (key == Keys.Escape)
				{
					Game1.playSound("bigDeSelect");
					this.listening = false;
					GameMenu.forcePreventClose = false;
					return;
				}
				if (!Game1.options.isKeyInUse(key) || new InputButton(key).ToString().Equals(this.buttonNames.First<string>()))
				{
					Game1.options.changeInputListenerValue(this.whichOption, key);
					this.buttonNames[0] = new InputButton(key).ToString();
					Game1.playSound("coin");
					this.listening = false;
					GameMenu.forcePreventClose = false;
					return;
				}
				this.listenerMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsElement.cs.11228", new object[0]);
			}
		}

		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			if (this.buttonNames.Count > 0 || this.whichOption == -1)
			{
				if (this.whichOption == -1)
				{
					Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float)(this.bounds.X + slotX), (float)(this.bounds.Y + slotY)), Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
				}
				else
				{
					Utility.drawTextWithShadow(b, this.label + ": " + this.buttonNames.Last<string>() + ((this.buttonNames.Count > 1) ? (", " + this.buttonNames.First<string>()) : ""), Game1.dialogueFont, new Vector2((float)(this.bounds.X + slotX), (float)(this.bounds.Y + slotY)), Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
				}
			}
			Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.setbuttonBounds.X + slotX), (float)(this.setbuttonBounds.Y + slotY)), OptionsInputListener.setButtonSource, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.15f, -1, -1, 0.35f);
			if (this.listening)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * 0.75f, 0f, Vector2.Zero, SpriteEffects.None, 0.999f);
				b.DrawString(Game1.dialogueFont, this.listenerMessage, Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 3, Game1.tileSize, 0, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
			}
		}
	}
}
