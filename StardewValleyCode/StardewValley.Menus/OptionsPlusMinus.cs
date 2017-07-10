using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class OptionsPlusMinus : OptionsElement
	{
		public const int pixelsWide = 7;

		public List<string> options = new List<string>();

		public List<string> displayOptions = new List<string>();

		public int selected;

		public bool isChecked;

		public static bool snapZoomPlus;

		public static bool snapZoomMinus;

		private Rectangle minusButton;

		private Rectangle plusButton;

		public static Rectangle minusButtonSource = new Rectangle(177, 345, 7, 8);

		public static Rectangle plusButtonSource = new Rectangle(184, 345, 7, 8);

		public OptionsPlusMinus(string label, int whichOption, List<string> options, List<string> displayOptions, int x = -1, int y = -1) : base(label, x, y, 7 * Game1.pixelZoom, 7 * Game1.pixelZoom, whichOption)
		{
			this.options = options;
			this.displayOptions = displayOptions;
			Game1.options.setPlusMinusToProperValue(this);
			if (x == -1)
			{
				x = 8 * Game1.pixelZoom;
			}
			if (y == -1)
			{
				y = 4 * Game1.pixelZoom;
			}
			int num = (int)Game1.dialogueFont.MeasureString(options[0]).X + 7 * Game1.pixelZoom;
			foreach (string current in displayOptions)
			{
				num = Math.Max((int)Game1.dialogueFont.MeasureString(current).X + 7 * Game1.pixelZoom, num);
			}
			this.bounds = new Rectangle(x, y, 7 * Game1.pixelZoom * 2 + num, 8 * Game1.pixelZoom);
			this.label = label;
			this.whichOption = whichOption;
			this.minusButton = new Rectangle(x, 4 + Game1.pixelZoom * 3, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom);
			this.plusButton = new Rectangle(this.bounds.Right - 8 * Game1.pixelZoom, 4 + Game1.pixelZoom * 3, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom);
		}

		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut && this.options.Count > 0)
			{
				int arg_D9_0 = this.selected;
				if (this.minusButton.Contains(x, y) && this.selected != 0)
				{
					this.selected--;
					OptionsPlusMinus.snapZoomMinus = true;
					Game1.playSound("drumkit6");
				}
				else if (this.plusButton.Contains(x, y) && this.selected != this.options.Count - 1)
				{
					this.selected++;
					OptionsPlusMinus.snapZoomPlus = true;
					Game1.playSound("drumkit6");
				}
				if (this.selected < 0)
				{
					this.selected = 0;
				}
				else if (this.selected >= this.options.Count)
				{
					this.selected = this.options.Count - 1;
				}
				if (arg_D9_0 != this.selected)
				{
					Game1.options.changeDropDownOption(this.whichOption, this.selected, this.options);
				}
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
				{
					this.receiveLeftClick(this.plusButton.Center.X, this.plusButton.Center.Y);
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
				{
					this.receiveLeftClick(this.minusButton.Center.X, this.minusButton.Center.Y);
				}
			}
		}

		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.minusButton.X), (float)(slotY + this.minusButton.Y)), new Rectangle?(OptionsPlusMinus.minusButtonSource), Color.White * (this.greyedOut ? 0.33f : 1f) * ((this.selected == 0) ? 0.5f : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);
			b.DrawString(Game1.dialogueFont, (this.selected < this.displayOptions.Count && this.selected != -1) ? this.displayOptions[this.selected] : "", new Vector2((float)(slotX + this.minusButton.X + this.minusButton.Width + Game1.pixelZoom), (float)(slotY + this.minusButton.Y)), Game1.textColor);
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.plusButton.X), (float)(slotY + this.plusButton.Y)), new Rectangle?(OptionsPlusMinus.plusButtonSource), Color.White * (this.greyedOut ? 0.33f : 1f) * ((this.selected == this.displayOptions.Count - 1) ? 0.5f : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);
			if (!Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				if (OptionsPlusMinus.snapZoomMinus)
				{
					Game1.setMousePosition(slotX + this.minusButton.Center.X, slotY + this.minusButton.Center.Y);
					OptionsPlusMinus.snapZoomMinus = false;
				}
				else if (OptionsPlusMinus.snapZoomPlus)
				{
					Game1.setMousePosition(slotX + this.plusButton.Center.X, slotY + this.plusButton.Center.Y);
					OptionsPlusMinus.snapZoomPlus = false;
				}
			}
			base.draw(b, slotX, slotY);
		}
	}
}
