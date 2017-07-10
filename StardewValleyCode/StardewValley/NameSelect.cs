using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class NameSelect
	{
		public const int maxNameLength = 9;

		public const int charactersPerRow = 15;

		public static string name = "";

		private static int selection = 0;

		private static List<char> namingCharacters;

		public static void load()
		{
			NameSelect.namingCharacters = new List<char>();
			for (int i = 0; i < 25; i += 5)
			{
				for (int j = 0; j < 5; j++)
				{
					NameSelect.namingCharacters.Add((char)(97 + i + j));
				}
				for (int k = 0; k < 5; k++)
				{
					NameSelect.namingCharacters.Add((char)(65 + i + k));
				}
				if (i < 10)
				{
					for (int l = 0; l < 5; l++)
					{
						NameSelect.namingCharacters.Add((char)(48 + i + l));
					}
				}
				else if (i < 15)
				{
					NameSelect.namingCharacters.Add('?');
					NameSelect.namingCharacters.Add('$');
					NameSelect.namingCharacters.Add('\'');
					NameSelect.namingCharacters.Add('#');
					NameSelect.namingCharacters.Add('[');
				}
				else if (i < 20)
				{
					NameSelect.namingCharacters.Add('-');
					NameSelect.namingCharacters.Add('=');
					NameSelect.namingCharacters.Add('~');
					NameSelect.namingCharacters.Add('&');
					NameSelect.namingCharacters.Add('!');
				}
				else
				{
					NameSelect.namingCharacters.Add('Z');
					NameSelect.namingCharacters.Add('z');
					NameSelect.namingCharacters.Add('<');
					NameSelect.namingCharacters.Add('"');
					NameSelect.namingCharacters.Add(']');
				}
			}
		}

		public static void draw()
		{
			int num = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width % Game1.tileSize, Game1.graphics.GraphicsDevice.Viewport.Width - Game1.graphics.GraphicsDevice.Viewport.Width % Game1.tileSize - Game1.tileSize * 2);
			int num2 = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height % Game1.tileSize, Game1.graphics.GraphicsDevice.Viewport.Height - Game1.graphics.GraphicsDevice.Viewport.Height % Game1.tileSize - Game1.tileSize);
			int num3 = Game1.graphics.GraphicsDevice.Viewport.Width / 2 - num / 2;
			int num4 = Game1.graphics.GraphicsDevice.Viewport.Height / 2 - num2 / 2;
			int num5 = (num - Game1.tileSize * 2) / 15;
			int num6 = (num2 - Game1.tileSize * 4) / 5;
			Game1.drawDialogueBox(num3, num4, num, num2, false, true, null, false);
			string text = "";
			string nameSelectType = Game1.nameSelectType;
			if (!(nameSelectType == "samBand"))
			{
				if (nameSelectType == "Animal" || nameSelectType == "playerName" || nameSelectType == "coopDwellerBorn")
				{
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3860", new object[0]);
				}
			}
			else
			{
				text = Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3856", new object[0]);
			}
			Game1.spriteBatch.DrawString(Game1.dialogueFont, text, new Vector2((float)(num3 + 2 * Game1.tileSize), (float)(num4 + Game1.tileSize * 2)), Game1.textColor);
			int num7 = (int)Game1.dialogueFont.MeasureString(text).X;
			string text2 = "";
			for (int i = 0; i < 9; i++)
			{
				if (NameSelect.name.Length > i)
				{
					Game1.spriteBatch.DrawString(Game1.dialogueFont, NameSelect.name[i].ToString() ?? "", new Vector2((float)(num3 + 2 * Game1.tileSize + num7) + Game1.dialogueFont.MeasureString(text2).X + (Game1.dialogueFont.MeasureString("_").X - Game1.dialogueFont.MeasureString(NameSelect.name[i].ToString() ?? "").X) / 2f - 2f, (float)(num4 + Game1.tileSize * 2 - Game1.tileSize / 10)), Game1.textColor);
				}
				text2 += "_ ";
			}
			Game1.spriteBatch.DrawString(Game1.dialogueFont, "_ _ _ _ _ _ _ _ _", new Vector2((float)(num3 + 2 * Game1.tileSize + num7), (float)(num4 + Game1.tileSize * 2)), Game1.textColor);
			Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3864", new object[0]), new Vector2((float)(num3 + num - Game1.tileSize * 3), (float)(num4 + num2 - Game1.tileSize * 3 / 2)), Game1.textColor);
			for (int j = 0; j < 5; j++)
			{
				int num8 = 0;
				for (int k = 0; k < 15; k++)
				{
					if (k != 0 && k % 5 == 0)
					{
						num8 += num5 / 3;
					}
					Game1.spriteBatch.DrawString(Game1.dialogueFont, NameSelect.namingCharacters[j * 15 + k].ToString() ?? "", new Vector2((float)(num8 + num3 + Game1.tileSize + num5 * k), (float)(num4 + Game1.tileSize * 3 + num6 * j)), Game1.textColor);
					if (NameSelect.selection == j * 15 + k)
					{
						Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float)(num8 + num3 + num5 * k - Game1.tileSize / 10), (float)(num4 + Game1.tileSize * 3 + num6 * j - Game1.tileSize / 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 26, -1, -1)), Color.White);
					}
				}
			}
			if (NameSelect.selection == -1)
			{
				Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float)(num3 + num - Game1.tileSize * 3 - Game1.tileSize - Game1.tileSize / 10), (float)(num4 + num2 - Game1.tileSize * 3 / 2 - Game1.tileSize / 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 26, -1, -1)), Color.White);
			}
		}

		public static bool select()
		{
			if (NameSelect.selection == -1)
			{
				if (NameSelect.name.Length > 0)
				{
					return true;
				}
			}
			else if (NameSelect.name.Length < 9)
			{
				NameSelect.name += NameSelect.namingCharacters[NameSelect.selection].ToString();
				Game1.playSound("smallSelect");
			}
			return false;
		}

		public static void startButton()
		{
			if (NameSelect.name.Length > 0)
			{
				NameSelect.selection = -1;
				Game1.playSound("smallSelect");
			}
		}

		public static bool isOnDone()
		{
			return NameSelect.selection == -1;
		}

		public static void backspace()
		{
			if (NameSelect.name.Length > 0)
			{
				NameSelect.name = NameSelect.name.Remove(NameSelect.name.Length - 1);
				Game1.playSound("toolSwap");
			}
		}

		public static bool cancel()
		{
			if ((Game1.nameSelectType.Equals("samBand") || Game1.nameSelectType.Equals("coopDwellerBorn")) && NameSelect.name.Length > 0)
			{
				Game1.playSound("toolSwap");
				NameSelect.name = NameSelect.name.Remove(NameSelect.name.Length - 1);
				return false;
			}
			NameSelect.selection = 0;
			NameSelect.name = "";
			return true;
		}

		public static void moveSelection(int direction)
		{
			Game1.playSound("toolSwap");
			if (!direction.Equals(0))
			{
				if (direction.Equals(1))
				{
					NameSelect.selection++;
					if (NameSelect.selection % 15 == 0)
					{
						NameSelect.selection -= 15;
						return;
					}
				}
				else if (direction.Equals(2))
				{
					if (NameSelect.selection >= NameSelect.namingCharacters.Count - 2)
					{
						NameSelect.selection = -1;
						return;
					}
					NameSelect.selection += 15;
					if (NameSelect.selection >= NameSelect.namingCharacters.Count)
					{
						NameSelect.selection -= NameSelect.namingCharacters.Count;
						return;
					}
				}
				else if (direction.Equals(3))
				{
					if (NameSelect.selection % 15 == 0)
					{
						NameSelect.selection += 14;
						return;
					}
					NameSelect.selection--;
				}
				return;
			}
			if (NameSelect.selection == -1)
			{
				NameSelect.selection = NameSelect.namingCharacters.Count - 2;
				return;
			}
			if (NameSelect.selection - 15 < 0)
			{
				NameSelect.selection = NameSelect.namingCharacters.Count - 15 + NameSelect.selection;
				return;
			}
			NameSelect.selection -= 15;
		}
	}
}
