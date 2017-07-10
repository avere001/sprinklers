using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	internal class ColorChanger
	{
		private static bool IsSimilar(Color original, Color test, int redDelta, int blueDelta, int greenDelta)
		{
			return Math.Abs((int)(original.R - test.R)) < redDelta && Math.Abs((int)(original.G - test.G)) < greenDelta && Math.Abs((int)(original.B - test.B)) < blueDelta;
		}

		public static Texture2D changeColor(Texture2D texture, int targetColorIndex, int redtint, int greentint, int bluetint, int range)
		{
			Color[] array = new Color[texture.Width * texture.Height];
			texture.GetData<Color>(array);
			Color test = array[targetColorIndex];
			for (int i = 0; i < array.Length; i++)
			{
				if (ColorChanger.IsSimilar(array[i], test, range, range, range))
				{
					array[i] = new Color((int)array[i].R + redtint, (int)array[i].G + greentint, (int)array[i].B + bluetint);
				}
			}
			texture.SetData<Color>(array);
			return texture;
		}

		public static Texture2D changeColor(Texture2D texture, int targetColorIndex, Color baseColor, int range)
		{
			Color[] array = new Color[texture.Width * texture.Height];
			texture.GetData<Color>(array);
			Color test = array[targetColorIndex];
			for (int i = 0; i < array.Length; i++)
			{
				if (ColorChanger.IsSimilar(array[i], test, range, range, range))
				{
					array[i] = new Color((int)(baseColor.R + (array[i].R - test.R)), (int)(baseColor.G + (array[i].G - test.G)), (int)(baseColor.B + (array[i].B - test.B)));
				}
			}
			texture.SetData<Color>(array);
			return texture;
		}

		public static Texture2D swapColor(Texture2D texture, int targetColorIndex, int red, int green, int blue)
		{
			return ColorChanger.swapColor(texture, targetColorIndex, red, green, blue, 0, texture.Width * texture.Height);
		}

		public static Texture2D swapColor(Texture2D texture, int targetColorIndex, int red, int green, int blue, int startPixel, int endPixel)
		{
			red = Math.Min(Math.Max(1, red), 255);
			green = Math.Min(Math.Max(1, green), 255);
			blue = Math.Min(Math.Max(1, blue), 255);
			Color[] array = new Color[texture.Width * texture.Height];
			texture.GetData<Color>(array);
			Color color = array[targetColorIndex];
			for (int i = 0; i < array.Length; i++)
			{
				if (i >= startPixel && i < endPixel && array[i].R == color.R && array[i].G == color.G && array[i].B == color.B)
				{
					array[i] = new Color(red, green, blue);
				}
			}
			texture.SetData<Color>(array);
			return texture;
		}
	}
}
