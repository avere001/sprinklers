using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Menus
{
	public class ColorPicker
	{
		public const int sliderChunks = 24;

		private int colorIndexSelection;

		private Rectangle bounds;

		public SliderBar hueBar;

		public SliderBar valueBar;

		public SliderBar saturationBar;

		public SliderBar recentSliderBar;

		public ColorPicker(int x, int y)
		{
			this.hueBar = new SliderBar(0, 0, 50);
			this.saturationBar = new SliderBar(0, 20, 50);
			this.valueBar = new SliderBar(0, 40, 50);
			this.bounds = new Rectangle(x, y, SliderBar.defaultWidth, 60);
		}

		public Color getSelectedColor()
		{
			return this.HsvToRgb((double)this.hueBar.value / 100.0 * 360.0, (double)this.saturationBar.value / 100.0, (double)this.valueBar.value / 100.0);
		}

		public Color click(int x, int y)
		{
			if (this.bounds.Contains(x, y))
			{
				x -= this.bounds.X;
				y -= this.bounds.Y;
				if (this.hueBar.bounds.Contains(x, y))
				{
					this.hueBar.click(x, y);
					this.recentSliderBar = this.hueBar;
				}
				if (this.saturationBar.bounds.Contains(x, y))
				{
					this.recentSliderBar = this.saturationBar;
					this.saturationBar.click(x, y);
				}
				if (this.valueBar.bounds.Contains(x, y))
				{
					this.recentSliderBar = this.valueBar;
					this.valueBar.click(x, y);
				}
			}
			return this.getSelectedColor();
		}

		public void changeHue(int amount)
		{
			this.hueBar.changeValueBy(amount);
			this.recentSliderBar = this.hueBar;
		}

		public void changeSaturation(int amount)
		{
			this.saturationBar.changeValueBy(amount);
			this.recentSliderBar = this.saturationBar;
		}

		public void changeValue(int amount)
		{
			this.valueBar.changeValueBy(amount);
			this.recentSliderBar = this.valueBar;
		}

		public Color clickHeld(int x, int y)
		{
			if (this.recentSliderBar != null)
			{
				x = Math.Max(x, this.bounds.X);
				x = Math.Min(x, this.bounds.Right - 1);
				y = this.recentSliderBar.bounds.Center.Y;
				x -= this.bounds.X;
				if (this.recentSliderBar.Equals(this.hueBar))
				{
					this.hueBar.click(x, y);
				}
				if (this.recentSliderBar.Equals(this.saturationBar))
				{
					this.saturationBar.click(x, y);
				}
				if (this.recentSliderBar.Equals(this.valueBar))
				{
					this.valueBar.click(x, y);
				}
			}
			return this.getSelectedColor();
		}

		public void releaseClick()
		{
			this.hueBar.release(0, 0);
			this.saturationBar.release(0, 0);
			this.valueBar.release(0, 0);
			this.recentSliderBar = null;
		}

		public void draw(SpriteBatch b)
		{
			for (int i = 0; i < 24; i++)
			{
				Color color = this.HsvToRgb((double)i / 24.0 * 360.0, 0.9, 0.9);
				b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * i, this.bounds.Y + this.hueBar.bounds.Center.Y - 2, this.hueBar.bounds.Width / 24, 4), color);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + (int)((float)this.hueBar.value / 100f * (float)this.hueBar.bounds.Width)), (float)(this.bounds.Y + this.hueBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.86f);
			Utility.drawTextWithShadow(b, string.Concat(this.hueBar.value), Game1.smallFont, new Vector2((float)(this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(this.bounds.Y + this.hueBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
			for (int j = 0; j < 24; j++)
			{
				Color color2 = this.HsvToRgb((double)this.hueBar.value / 100.0 * 360.0, (double)j / 24.0, (double)this.valueBar.value / 100.0);
				b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * j, this.bounds.Y + this.saturationBar.bounds.Center.Y - 2, this.saturationBar.bounds.Width / 24, 4), color2);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + (int)((float)this.saturationBar.value / 100f * (float)this.saturationBar.bounds.Width)), (float)(this.bounds.Y + this.saturationBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.87f);
			Utility.drawTextWithShadow(b, string.Concat(this.saturationBar.value), Game1.smallFont, new Vector2((float)(this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(this.bounds.Y + this.saturationBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
			for (int k = 0; k < 24; k++)
			{
				Color color3 = this.HsvToRgb((double)this.hueBar.value / 100.0 * 360.0, (double)this.saturationBar.value / 100.0, (double)k / 24.0);
				b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * k, this.bounds.Y + this.valueBar.bounds.Center.Y - 2, this.valueBar.bounds.Width / 24, 4), color3);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + (int)((float)this.valueBar.value / 100f * (float)this.valueBar.bounds.Width)), (float)(this.bounds.Y + this.valueBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.86f);
			Utility.drawTextWithShadow(b, string.Concat(this.valueBar.value), Game1.smallFont, new Vector2((float)(this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(this.bounds.Y + this.valueBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
		}

		public bool containsPoint(int x, int y)
		{
			return this.bounds.Contains(x, y);
		}

		public void setColor(Color color)
		{
			float num;
			float num2;
			float num3;
			this.RGBtoHSV((float)color.R, (float)color.G, (float)color.B, out num, out num2, out num3);
			this.hueBar.value = (int)(num / 360f * 100f);
			this.saturationBar.value = (int)(num2 * 100f);
			this.valueBar.value = (int)(num3 / 255f * 100f);
		}

		private void RGBtoHSV(float r, float g, float b, out float h, out float s, out float v)
		{
			float num = Math.Min(Math.Min(r, g), b);
			float num2 = Math.Max(Math.Max(r, g), b);
			v = num2;
			float num3 = num2 - num;
			if (num2 != 0f)
			{
				s = num3 / num2;
				if (r == num2)
				{
					h = (g - b) / num3;
				}
				else if (g == num2)
				{
					h = 2f + (b - r) / num3;
				}
				else
				{
					h = 4f + (r - g) / num3;
				}
				h *= 60f;
				if (h < 0f)
				{
					h += 360f;
				}
				return;
			}
			s = 0f;
			h = -1f;
		}

		private Color HsvToRgb(double h, double S, double V)
		{
			double num = h;
			while (num < 0.0)
			{
				num += 1.0;
				if (num < -1000000.0)
				{
					num = 0.0;
				}
			}
			while (num >= 360.0)
			{
				num -= 1.0;
			}
			double num4;
			double num3;
			double num2;
			if (V <= 0.0)
			{
				num2 = (num3 = (num4 = 0.0));
			}
			else if (S <= 0.0)
			{
				num4 = V;
				num2 = V;
				num3 = V;
			}
			else
			{
				double expr_8D = num / 60.0;
				int num5 = (int)Math.Floor(expr_8D);
				double num6 = expr_8D - (double)num5;
				double num7 = V * (1.0 - S);
				double num8 = V * (1.0 - S * num6);
				double num9 = V * (1.0 - S * (1.0 - num6));
				switch (num5)
				{
				case -1:
					num3 = V;
					num2 = num7;
					num4 = num8;
					break;
				case 0:
					num3 = V;
					num2 = num9;
					num4 = num7;
					break;
				case 1:
					num3 = num8;
					num2 = V;
					num4 = num7;
					break;
				case 2:
					num3 = num7;
					num2 = V;
					num4 = num9;
					break;
				case 3:
					num3 = num7;
					num2 = num8;
					num4 = V;
					break;
				case 4:
					num3 = num9;
					num2 = num7;
					num4 = V;
					break;
				case 5:
					num3 = V;
					num2 = num7;
					num4 = num8;
					break;
				case 6:
					num3 = V;
					num2 = num9;
					num4 = num7;
					break;
				default:
					num4 = V;
					num2 = V;
					num3 = V;
					break;
				}
			}
			return new Color(this.Clamp((int)(num3 * 255.0)), this.Clamp((int)(num2 * 255.0)), this.Clamp((int)(num4 * 255.0)));
		}

		private int Clamp(int i)
		{
			if (i < 0)
			{
				return 0;
			}
			if (i > 255)
			{
				return 255;
			}
			return i;
		}
	}
}
