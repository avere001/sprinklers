using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class OptionsDropDown : OptionsElement
	{
		public const int pixelsHigh = 11;

		public static OptionsDropDown selected;

		public List<string> dropDownOptions = new List<string>();

		public List<string> dropDownDisplayOptions = new List<string>();

		public int selectedOption;

		public int recentSlotY;

		public int startingSelected;

		private bool clicked;

		private Rectangle dropDownBounds;

		public static Rectangle dropDownBGSource = new Rectangle(433, 451, 3, 3);

		public static Rectangle dropDownButtonSource = new Rectangle(437, 450, 10, 11);

		public OptionsDropDown(string label, int whichOption, int x = -1, int y = -1) : base(label, x, y, (int)Game1.smallFont.MeasureString("Windowed Borderless Mode   ").X + Game1.pixelZoom * 12, 11 * Game1.pixelZoom, whichOption)
		{
			Game1.options.setDropDownToProperValue(this);
			this.dropDownBounds = new Rectangle(this.bounds.X, this.bounds.Y, this.bounds.Width - Game1.pixelZoom * 12, this.bounds.Height * this.dropDownOptions.Count);
		}

		public override void leftClickHeld(int x, int y)
		{
			if (!this.greyedOut)
			{
				base.leftClickHeld(x, y);
				this.clicked = true;
				this.dropDownBounds.Y = Math.Min(this.dropDownBounds.Y, Game1.viewport.Height - this.dropDownBounds.Height - this.recentSlotY);
				this.selectedOption = (int)Math.Max(Math.Min((float)(y - this.dropDownBounds.Y) / (float)this.bounds.Height, (float)(this.dropDownOptions.Count - 1)), 0f);
			}
		}

		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut)
			{
				base.receiveLeftClick(x, y);
				this.startingSelected = this.selectedOption;
				this.leftClickHeld(x, y);
				Game1.playSound("shwip");
				OptionsDropDown.selected = this;
			}
		}

		public override void leftClickReleased(int x, int y)
		{
			if (!this.greyedOut && this.dropDownOptions.Count > 0)
			{
				base.leftClickReleased(x, y);
				this.clicked = false;
				if (this.dropDownBounds.Contains(x, y))
				{
					Game1.options.changeDropDownOption(this.whichOption, this.selectedOption, this.dropDownOptions);
				}
				else
				{
					this.selectedOption = this.startingSelected;
				}
				OptionsDropDown.selected = null;
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
				{
					this.selectedOption++;
					if (this.selectedOption >= this.dropDownOptions.Count)
					{
						this.selectedOption = 0;
					}
					Game1.options.changeDropDownOption(this.whichOption, this.selectedOption, this.dropDownOptions);
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
				{
					this.selectedOption--;
					if (this.selectedOption < 0)
					{
						this.selectedOption = this.dropDownOptions.Count - 1;
					}
					Game1.options.changeDropDownOption(this.whichOption, this.selectedOption, this.dropDownOptions);
				}
			}
		}

		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			this.recentSlotY = slotY;
			base.draw(b, slotX, slotY);
			float scale = this.greyedOut ? 0.33f : 1f;
			if (this.clicked)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, slotX + this.dropDownBounds.X, slotY + this.dropDownBounds.Y, this.dropDownBounds.Width, this.dropDownBounds.Height, Color.White * scale, (float)Game1.pixelZoom, false);
				for (int i = 0; i < this.dropDownDisplayOptions.Count; i++)
				{
					if (i == this.selectedOption)
					{
						b.Draw(Game1.staminaRect, new Rectangle(slotX + this.dropDownBounds.X, slotY + this.dropDownBounds.Y + i * this.bounds.Height, this.dropDownBounds.Width, this.bounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
					}
					b.DrawString(Game1.smallFont, this.dropDownDisplayOptions[i], new Vector2((float)(slotX + this.dropDownBounds.X + Game1.pixelZoom), (float)(slotY + this.dropDownBounds.Y + Game1.pixelZoom * 2 + this.bounds.Height * i)), Game1.textColor * scale, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
				}
				b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.Wheat * scale, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.981f);
				return;
			}
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width - Game1.pixelZoom * 12, this.bounds.Height, Color.White * scale, (float)Game1.pixelZoom, false);
			if (OptionsDropDown.selected == null || OptionsDropDown.selected.Equals(this))
			{
				b.DrawString(Game1.smallFont, (this.selectedOption < this.dropDownDisplayOptions.Count && this.selectedOption >= 0) ? this.dropDownDisplayOptions[this.selectedOption] : "", new Vector2((float)(slotX + this.bounds.X + Game1.pixelZoom), (float)(slotY + this.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor * scale, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.White * scale, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
		}
	}
}
