using BmFont;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.BellsAndWhistles
{
	public class SpriteText
	{
		public const int scrollStyle_scroll = 0;

		public const int scrollStyle_scrollleftjustified = 0;

		public const int scrollStyle_speechBubble = 1;

		public const int scrollStyle_darkMetal = 2;

		public const int maxCharacter = 999999;

		public const int maxHeight = 999999;

		public const int characterWidth = 8;

		public const int characterHeight = 16;

		public const int horizontalSpaceBetweenCharacters = 0;

		public const int verticalSpaceBetweenCharacters = 2;

		public const char newLine = '^';

		public static float fontPixelZoom = 3f;

		public static float shadowAlpha = 0.15f;

		private static Dictionary<char, FontChar> _characterMap;

		private static FontFile FontFile = null;

		private static List<Texture2D> fontPages = null;

		public static Texture2D spriteTexture;

		public static Texture2D coloredTexture;

		public const int color_Black = 0;

		public const int color_Blue = 1;

		public const int color_Red = 2;

		public const int color_Purple = 3;

		public const int color_White = 4;

		public const int color_Orange = 5;

		public const int color_Green = 6;

		public const int color_Cyan = 7;

		public static void drawStringHorizontallyCenteredAt(SpriteBatch b, string s, int x, int y, int characterPosition = 999999, int width = -1, int height = 999999, float alpha = 1f, float layerDepth = 0.88f, bool junimoText = false, int color = -1)
		{
			SpriteText.drawString(b, s, x - SpriteText.getWidthOfString(s) / 2, y, characterPosition, width, height, alpha, layerDepth, junimoText, -1, "", color);
		}

		public static int getWidthOfString(string s)
		{
			SpriteText.setUpCharacterMap();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (!LocalizedContentManager.CurrentLanguageLatin)
				{
					FontChar fontChar;
					if (SpriteText._characterMap.TryGetValue(s[i], out fontChar))
					{
						num += fontChar.XAdvance;
					}
					num2 = Math.Max(num, num2);
					if (s[i] == '^')
					{
						num = 0;
					}
				}
				else
				{
					num += 8 + SpriteText.getWidthOffsetForChar(s[i]);
					num2 = Math.Max(num, num2);
					if (s[i] == '^')
					{
						num = 0;
					}
				}
			}
			return (int)((float)num2 * SpriteText.fontPixelZoom);
		}

		public static int getHeightOfString(string s, int widthConstraint = 999999)
		{
			if (s.Length == 0)
			{
				return 0;
			}
			Vector2 vector = default(Vector2);
			int num = 0;
			s = s.Replace(Environment.NewLine, "");
			SpriteText.setUpCharacterMap();
			if (!LocalizedContentManager.CurrentLanguageLatin)
			{
				for (int i = 0; i < s.Length; i++)
				{
					if (s[i] == '^')
					{
						vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
						vector.X = 0f;
					}
					else
					{
						if (SpriteText.positionOfNextSpace(s, i, (int)vector.X, num) >= widthConstraint)
						{
							vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
							num = 0;
							vector.X = 0f;
						}
						FontChar fontChar;
						if (SpriteText._characterMap.TryGetValue(s[i], out fontChar))
						{
							vector.X += (float)fontChar.XAdvance * SpriteText.fontPixelZoom;
						}
					}
				}
				return (int)(vector.Y + (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom);
			}
			for (int j = 0; j < s.Length; j++)
			{
				if (s[j] == '^')
				{
					vector.Y += 18f * SpriteText.fontPixelZoom;
					vector.X = 0f;
					num = 0;
				}
				else
				{
					if (j > 0)
					{
						vector.X += 8f * SpriteText.fontPixelZoom + (float)num + (float)(SpriteText.getWidthOffsetForChar(s[j]) + SpriteText.getWidthOffsetForChar(s[j - 1])) * SpriteText.fontPixelZoom;
					}
					num = (int)(0f * SpriteText.fontPixelZoom);
					if (SpriteText.positionOfNextSpace(s, j, (int)vector.X, num) >= widthConstraint)
					{
						vector.Y += 18f * SpriteText.fontPixelZoom;
						num = 0;
						vector.X = 0f;
					}
				}
			}
			return (int)(vector.Y + 16f * SpriteText.fontPixelZoom);
		}

		public static Color getColorFromIndex(int index)
		{
			switch (index)
			{
			case -1:
				if (LocalizedContentManager.CurrentLanguageLatin)
				{
					return Color.White;
				}
				return new Color(86, 22, 12);
			case 1:
				return Color.SkyBlue;
			case 2:
				return Color.Red;
			case 3:
				return new Color(110, 43, 255);
			case 4:
				return Color.White;
			case 5:
				return Color.OrangeRed;
			case 6:
				return Color.LimeGreen;
			case 7:
				return Color.Cyan;
			}
			return Color.Black;
		}

		public static string getSubstringBeyondHeight(string s, int width, int height)
		{
			Vector2 vector = default(Vector2);
			int num = 0;
			s = s.Replace(Environment.NewLine, "");
			SpriteText.setUpCharacterMap();
			if (!LocalizedContentManager.CurrentLanguageLatin)
			{
				for (int i = 0; i < s.Length; i++)
				{
					if (s[i] == '^')
					{
						vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
						vector.X = 0f;
						num = 0;
					}
					else
					{
						FontChar fontChar;
						if (SpriteText._characterMap.TryGetValue(s[i], out fontChar))
						{
							if (i > 0)
							{
								vector.X += (float)fontChar.XAdvance * SpriteText.fontPixelZoom;
							}
							if (SpriteText.positionOfNextSpace(s, i, (int)vector.X, num) >= width)
							{
								vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
								num = 0;
								vector.X = 0f;
							}
						}
						if (vector.Y >= (float)height - (float)SpriteText.FontFile.Common.LineHeight * SpriteText.fontPixelZoom * 2f)
						{
							return s.Substring(SpriteText.getLastSpace(s, i));
						}
					}
				}
				return "";
			}
			for (int j = 0; j < s.Length; j++)
			{
				if (s[j] == '^')
				{
					vector.Y += 18f * SpriteText.fontPixelZoom;
					vector.X = 0f;
					num = 0;
				}
				else
				{
					if (j > 0)
					{
						vector.X += 8f * SpriteText.fontPixelZoom + (float)num + (float)(SpriteText.getWidthOffsetForChar(s[j]) + SpriteText.getWidthOffsetForChar(s[j - 1])) * SpriteText.fontPixelZoom;
					}
					num = (int)(0f * SpriteText.fontPixelZoom);
					if (SpriteText.positionOfNextSpace(s, j, (int)vector.X, num) >= width)
					{
						vector.Y += 18f * SpriteText.fontPixelZoom;
						num = 0;
						vector.X = 0f;
					}
					if (vector.Y >= (float)height - 16f * SpriteText.fontPixelZoom * 2f)
					{
						return s.Substring(SpriteText.getLastSpace(s, j));
					}
				}
			}
			return "";
		}

		public static int getIndexOfSubstringBeyondHeight(string s, int width, int height)
		{
			Vector2 vector = default(Vector2);
			int num = 0;
			s = s.Replace(Environment.NewLine, "");
			SpriteText.setUpCharacterMap();
			if (!LocalizedContentManager.CurrentLanguageLatin)
			{
				for (int i = 0; i < s.Length; i++)
				{
					if (s[i] == '^')
					{
						vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
						vector.X = 0f;
						num = 0;
					}
					else
					{
						FontChar fontChar;
						if (SpriteText._characterMap.TryGetValue(s[i], out fontChar))
						{
							if (i > 0)
							{
								vector.X += (float)fontChar.XAdvance * SpriteText.fontPixelZoom;
							}
							if (SpriteText.positionOfNextSpace(s, i, (int)vector.X, num) >= width)
							{
								vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
								num = 0;
								vector.X = 0f;
							}
						}
						if (vector.Y >= (float)height - (float)SpriteText.FontFile.Common.LineHeight * SpriteText.fontPixelZoom * 2f)
						{
							return i - 1;
						}
					}
				}
				return s.Length - 1;
			}
			for (int j = 0; j < s.Length; j++)
			{
				if (s[j] == '^')
				{
					vector.Y += 18f * SpriteText.fontPixelZoom;
					vector.X = 0f;
					num = 0;
				}
				else
				{
					if (j > 0)
					{
						vector.X += 8f * SpriteText.fontPixelZoom + (float)num + (float)(SpriteText.getWidthOffsetForChar(s[j]) + SpriteText.getWidthOffsetForChar(s[j - 1])) * SpriteText.fontPixelZoom;
					}
					num = (int)(0f * SpriteText.fontPixelZoom);
					if (SpriteText.positionOfNextSpace(s, j, (int)vector.X, num) >= width)
					{
						vector.Y += 18f * SpriteText.fontPixelZoom;
						num = 0;
						vector.X = 0f;
					}
					if (vector.Y >= (float)height - 16f * SpriteText.fontPixelZoom)
					{
						return j - 1;
					}
				}
			}
			return s.Length - 1;
		}

		public static List<string> getStringBrokenIntoSectionsOfHeight(string s, int width, int height)
		{
			List<string> list = new List<string>();
			while (s.Length > 0)
			{
				string stringPreviousToThisHeightCutoff = SpriteText.getStringPreviousToThisHeightCutoff(s, width, height);
				if (stringPreviousToThisHeightCutoff.Length <= 0)
				{
					break;
				}
				list.Add(stringPreviousToThisHeightCutoff);
				s = s.Substring(list.Last<string>().Length);
			}
			return list;
		}

		public static string getStringPreviousToThisHeightCutoff(string s, int width, int height)
		{
			return s.Substring(0, SpriteText.getIndexOfSubstringBeyondHeight(s, width, height) + 1);
		}

		private static int getLastSpace(string s, int startIndex)
		{
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th)
			{
				return startIndex;
			}
			for (int i = startIndex; i >= 0; i--)
			{
				if (s[i] == ' ')
				{
					return i;
				}
			}
			return startIndex;
		}

		public static int getWidthOffsetForChar(char c)
		{
			if (c > '.')
			{
				if (c <= 'l')
				{
					if (c == '^')
					{
						return -8;
					}
					switch (c)
					{
					case 'i':
						break;
					case 'j':
					case 'l':
						return -1;
					case 'k':
						return 0;
					default:
						return 0;
					}
				}
				else
				{
					if (c == '¡')
					{
						return -1;
					}
					switch (c)
					{
					case 'ì':
					case 'í':
					case 'î':
					case 'ï':
						break;
					default:
						return 0;
					}
				}
				return -1;
			}
			if (c <= '$')
			{
				if (c != '!')
				{
					if (c != '$')
					{
						return 0;
					}
					return 1;
				}
			}
			else
			{
				if (c != ',' && c != '.')
				{
					return 0;
				}
				return -2;
			}
			return -1;
		}

		public static void drawStringWithScrollCenteredAt(SpriteBatch b, string s, int x, int y, string placeHolderWidthText = "", float alpha = 1f, int color = -1, int scrollType = 0, float layerDepth = 0.88f, bool junimoText = false)
		{
			SpriteText.drawString(b, s, x - SpriteText.getWidthOfString((placeHolderWidthText.Length > 0) ? placeHolderWidthText : s) / 2, y, 999999, -1, 999999, alpha, layerDepth, junimoText, scrollType, placeHolderWidthText, color);
		}

		public static void drawStringWithScrollBackground(SpriteBatch b, string s, int x, int y, string placeHolderWidthText = "", float alpha = 1f, int color = -1)
		{
			SpriteText.drawString(b, s, x, y, 999999, -1, 999999, alpha, 0.88f, false, 0, placeHolderWidthText, color);
		}

		private static FontFile loadFont(string assetName)
		{
			return FontLoader.Parse(Game1.content.Load<XmlSource>(assetName).Source);
		}

		private static void setUpCharacterMap()
		{
			if (!LocalizedContentManager.CurrentLanguageLatin && SpriteText._characterMap == null)
			{
				SpriteText._characterMap = new Dictionary<char, FontChar>();
				SpriteText.fontPages = new List<Texture2D>();
				switch (LocalizedContentManager.CurrentLanguageCode)
				{
				case LocalizedContentManager.LanguageCode.ja:
					SpriteText.FontFile = SpriteText.loadFont("Fonts\\Japanese");
					SpriteText.fontPixelZoom = 1.75f;
					break;
				case LocalizedContentManager.LanguageCode.ru:
					SpriteText.FontFile = SpriteText.loadFont("Fonts\\Russian");
					SpriteText.fontPixelZoom = 3f;
					break;
				case LocalizedContentManager.LanguageCode.zh:
					SpriteText.FontFile = SpriteText.loadFont("Fonts\\Chinese");
					SpriteText.fontPixelZoom = 1.5f;
					break;
				case LocalizedContentManager.LanguageCode.th:
					SpriteText.FontFile = SpriteText.loadFont("Fonts\\Thai");
					SpriteText.fontPixelZoom = 1.5f;
					break;
				}
				foreach (FontChar current in SpriteText.FontFile.Chars)
				{
					char key = (char)current.ID;
					SpriteText._characterMap.Add(key, current);
				}
				foreach (FontPage current2 in SpriteText.FontFile.Pages)
				{
					SpriteText.fontPages.Add(Game1.content.Load<Texture2D>("Fonts\\" + current2.File));
				}
				LocalizedContentManager.OnLanguageChange += new LocalizedContentManager.LanguageChangedHandler(SpriteText.OnLanguageChange);
				return;
			}
			if (LocalizedContentManager.CurrentLanguageLatin && SpriteText.fontPixelZoom < 3f)
			{
				SpriteText.fontPixelZoom = 3f;
			}
		}

		public static void drawString(SpriteBatch b, string s, int x, int y, int characterPosition = 999999, int width = -1, int height = 999999, float alpha = 1f, float layerDepth = 0.88f, bool junimoText = false, int drawBGScroll = -1, string placeHolderScrollWidthText = "", int color = -1)
		{
			SpriteText.setUpCharacterMap();
			if (width == -1)
			{
				width = Game1.graphics.GraphicsDevice.Viewport.Width - x;
				if (drawBGScroll == 1)
				{
					width = SpriteText.getWidthOfString(s) * 2;
				}
			}
			if (SpriteText.fontPixelZoom < 4f)
			{
				y += (int)((4f - SpriteText.fontPixelZoom) * (float)Game1.pixelZoom);
			}
			Vector2 vector = new Vector2((float)x, (float)y);
			int num = 0;
			if (drawBGScroll != 1)
			{
				if (vector.X + (float)width > (float)(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.pixelZoom))
				{
					vector.X = (float)(Game1.graphics.GraphicsDevice.Viewport.Width - width - Game1.pixelZoom);
				}
				if (vector.X < 0f)
				{
					vector.X = 0f;
				}
			}
			if (drawBGScroll == 0 || drawBGScroll == 0)
			{
				b.Draw(Game1.mouseCursors, vector + new Vector2(-12f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(325, 318, 12, 18)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, vector + new Vector2(0f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(337, 318, 1, 18)), Color.White * alpha, 0f, Vector2.Zero, new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)Game1.pixelZoom), SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, vector + new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)(-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(338, 318, 12, 18)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				if (placeHolderScrollWidthText.Length > 0)
				{
					if (drawBGScroll != 0)
					{
						x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
					}
					vector.X = (float)x;
				}
				vector.Y += (4f - SpriteText.fontPixelZoom) * (float)Game1.pixelZoom;
			}
			else if (drawBGScroll == 1)
			{
				b.Draw(Game1.mouseCursors, vector + new Vector2(-7f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(324, 299, 7, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, vector + new Vector2(0f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(331, 299, 1, 17)), Color.White * alpha, 0f, Vector2.Zero, new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)Game1.pixelZoom), SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, vector + new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)(-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(332, 299, 7, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, vector + new Vector2((float)(SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s) / 2), (float)(13 * Game1.pixelZoom)), new Rectangle?(new Rectangle(341, 308, 6, 5)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.0001f);
				if (placeHolderScrollWidthText.Length > 0)
				{
					x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
					vector.X = (float)x;
				}
				vector.Y += (4f - SpriteText.fontPixelZoom) * (float)Game1.pixelZoom;
			}
			else if (drawBGScroll == 2)
			{
				b.Draw(Game1.mouseCursors, vector + new Vector2(-3f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(327, 281, 3, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, vector + new Vector2(0f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(330, 281, 1, 17)), Color.White * alpha, 0f, Vector2.Zero, new Vector2((float)(SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s) + Game1.pixelZoom), (float)Game1.pixelZoom), SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, vector + new Vector2((float)(SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s) + Game1.pixelZoom), (float)(-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(333, 281, 3, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				if (placeHolderScrollWidthText.Length > 0)
				{
					x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
					vector.X = (float)x;
				}
				vector.Y += (4f - SpriteText.fontPixelZoom) * (float)Game1.pixelZoom;
			}
			s = s.Replace(Environment.NewLine, "");
			if (!junimoText && (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th))
			{
				vector.Y -= (4f - SpriteText.fontPixelZoom) * (float)Game1.pixelZoom;
			}
			s = s.Replace('♡', '<');
			for (int i = 0; i < Math.Min(s.Length, characterPosition); i++)
			{
				if ((LocalizedContentManager.CurrentLanguageLatin || SpriteText.IsSpecialCharacter(s[i])) | junimoText)
				{
					float num2 = SpriteText.fontPixelZoom;
					if (SpriteText.IsSpecialCharacter(s[i]) | junimoText)
					{
						SpriteText.fontPixelZoom = 3f;
					}
					if (s[i] == '^')
					{
						vector.Y += 18f * SpriteText.fontPixelZoom;
						vector.X = (float)x;
						num = 0;
					}
					else
					{
						if (i > 0)
						{
							vector.X += 8f * SpriteText.fontPixelZoom + (float)num + (float)(SpriteText.getWidthOffsetForChar(s[i]) + SpriteText.getWidthOffsetForChar(s[i - 1])) * SpriteText.fontPixelZoom;
						}
						num = (int)(0f * SpriteText.fontPixelZoom);
						if (SpriteText.positionOfNextSpace(s, i, (int)vector.X, num) >= x + width - Game1.pixelZoom)
						{
							vector.Y += 18f * SpriteText.fontPixelZoom;
							num = 0;
							vector.X = (float)x;
						}
						bool flag = char.IsUpper(s[i]) || s[i] == 'ß';
						Vector2 value = new Vector2(0f, (float)(-1 + ((!junimoText & flag) ? -3 : 0)));
						if (s[i] == 'Ç')
						{
							value.Y += 2f;
						}
						b.Draw((color != -1) ? SpriteText.coloredTexture : SpriteText.spriteTexture, vector + value * SpriteText.fontPixelZoom, new Rectangle?(SpriteText.getSourceRectForChar(s[i], junimoText)), ((SpriteText.IsSpecialCharacter(s[i]) | junimoText) ? Color.White : SpriteText.getColorFromIndex(color)) * alpha, 0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
						SpriteText.fontPixelZoom = num2;
					}
				}
				else if (s[i] == '^')
				{
					vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
					vector.X = (float)x;
					num = 0;
				}
				else
				{
					if (i > 0 && SpriteText.IsSpecialCharacter(s[i - 1]))
					{
						vector.X += 24f;
					}
					FontChar fontChar;
					if (SpriteText._characterMap.TryGetValue(s[i], out fontChar))
					{
						Rectangle value2 = new Rectangle(fontChar.X, fontChar.Y, fontChar.Width, fontChar.Height);
						Texture2D texture = SpriteText.fontPages[fontChar.Page];
						if (SpriteText.positionOfNextSpace(s, i, (int)vector.X, num) >= x + width - Game1.pixelZoom)
						{
							vector.Y += (float)(SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
							num = 0;
							vector.X = (float)x;
						}
						Vector2 vector2 = new Vector2(vector.X + (float)fontChar.XOffset * SpriteText.fontPixelZoom, vector.Y + (float)fontChar.YOffset * SpriteText.fontPixelZoom);
						if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru)
						{
							Vector2 vector3 = new Vector2(-1f, 1f) * SpriteText.fontPixelZoom;
							b.Draw(texture, vector2 + vector3, new Rectangle?(value2), SpriteText.getColorFromIndex(color) * alpha * SpriteText.shadowAlpha, 0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
							b.Draw(texture, vector2 + new Vector2(0f, vector3.Y), new Rectangle?(value2), SpriteText.getColorFromIndex(color) * alpha * SpriteText.shadowAlpha, 0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
							b.Draw(texture, vector2 + new Vector2(vector3.X, 0f), new Rectangle?(value2), SpriteText.getColorFromIndex(color) * alpha * SpriteText.shadowAlpha, 0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
						}
						b.Draw(texture, vector2, new Rectangle?(value2), SpriteText.getColorFromIndex(color) * alpha, 0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
						vector.X += (float)fontChar.XAdvance * SpriteText.fontPixelZoom;
					}
				}
			}
		}

		private static bool IsSpecialCharacter(char c)
		{
			return c.Equals('<') || c.Equals('=') || c.Equals('>') || c.Equals('@') || c.Equals('$') || c.Equals('`') || c.Equals('+');
		}

		private static void OnLanguageChange(LocalizedContentManager.LanguageCode code)
		{
			if (SpriteText._characterMap != null)
			{
				SpriteText._characterMap.Clear();
			}
			else
			{
				SpriteText._characterMap = new Dictionary<char, FontChar>();
			}
			if (SpriteText.fontPages != null)
			{
				SpriteText.fontPages.Clear();
			}
			else
			{
				SpriteText.fontPages = new List<Texture2D>();
			}
			switch (code)
			{
			case LocalizedContentManager.LanguageCode.ja:
				SpriteText.FontFile = SpriteText.loadFont("Fonts\\Japanese");
				SpriteText.fontPixelZoom = 1.75f;
				break;
			case LocalizedContentManager.LanguageCode.ru:
				SpriteText.FontFile = SpriteText.loadFont("Fonts\\Russian");
				SpriteText.fontPixelZoom = 3f;
				break;
			case LocalizedContentManager.LanguageCode.zh:
				SpriteText.FontFile = SpriteText.loadFont("Fonts\\Chinese");
				SpriteText.fontPixelZoom = 1.5f;
				break;
			case LocalizedContentManager.LanguageCode.th:
				SpriteText.FontFile = SpriteText.loadFont("Fonts\\Thai");
				SpriteText.fontPixelZoom = 1.5f;
				break;
			}
			foreach (FontChar current in SpriteText.FontFile.Chars)
			{
				char key = (char)current.ID;
				SpriteText._characterMap.Add(key, current);
			}
			foreach (FontPage current2 in SpriteText.FontFile.Pages)
			{
				SpriteText.fontPages.Add(Game1.content.Load<Texture2D>("Fonts\\" + current2.File));
			}
		}

		public static int positionOfNextSpace(string s, int index, int currentXPosition, int accumulatedHorizontalSpaceBetweenCharacters)
		{
			SpriteText.setUpCharacterMap();
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th)
			{
				FontChar fontChar;
				if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
				{
					return currentXPosition + (int)((float)fontChar.XAdvance * SpriteText.fontPixelZoom);
				}
				return currentXPosition + (int)((float)SpriteText.FontFile.Common.LineHeight * SpriteText.fontPixelZoom);
			}
			else
			{
				if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.ja)
				{
					for (int i = index; i < s.Length; i++)
					{
						if (!LocalizedContentManager.CurrentLanguageLatin)
						{
							if (s[i] == ' ')
							{
								return currentXPosition;
							}
							FontChar fontChar2;
							if (SpriteText._characterMap.TryGetValue(s[i], out fontChar2))
							{
								currentXPosition += (int)((float)fontChar2.XAdvance * SpriteText.fontPixelZoom);
							}
							else
							{
								currentXPosition += (int)((float)SpriteText.FontFile.Common.LineHeight * SpriteText.fontPixelZoom);
							}
						}
						else
						{
							if (s[i] == ' ')
							{
								return currentXPosition;
							}
							currentXPosition += (int)(8f * SpriteText.fontPixelZoom + (float)accumulatedHorizontalSpaceBetweenCharacters + (float)(SpriteText.getWidthOffsetForChar(s[i]) + SpriteText.getWidthOffsetForChar(s[Math.Max(0, i - 1)])) * SpriteText.fontPixelZoom);
							accumulatedHorizontalSpaceBetweenCharacters = (int)(0f * SpriteText.fontPixelZoom);
						}
					}
					return currentXPosition;
				}
				FontChar fontChar3;
				if (SpriteText._characterMap.TryGetValue(s[index], out fontChar3))
				{
					return currentXPosition + (int)((float)fontChar3.XAdvance * SpriteText.fontPixelZoom);
				}
				return currentXPosition + (int)((float)SpriteText.FontFile.Common.LineHeight * SpriteText.fontPixelZoom);
			}
		}

		private static Rectangle getSourceRectForChar(char c, bool junimoText)
		{
			int num = (int)(c - ' ');
			return new Rectangle(num * 8 % SpriteText.spriteTexture.Width, num * 8 / SpriteText.spriteTexture.Width * 16 + (junimoText ? 224 : 0), 8, 16);
		}
	}
}
