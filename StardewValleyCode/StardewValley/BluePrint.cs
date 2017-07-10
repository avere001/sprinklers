using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class BluePrint
	{
		public string name;

		public int woodRequired;

		public int stoneRequired;

		public int copperRequired;

		public int IronRequired;

		public int GoldRequired;

		public int IridiumRequired;

		public int tilesWidth;

		public int tilesHeight;

		public int maxOccupants;

		public int moneyRequired;

		public Point humanDoor;

		public Point animalDoor;

		public string mapToWarpTo;

		public string displayName;

		public string description;

		public string blueprintType;

		public string nameOfBuildingToUpgrade;

		public string actionBehavior;

		public Texture2D texture;

		public List<string> namesOfOkayBuildingLocations = new List<string>();

		public Rectangle sourceRectForMenuView;

		public Dictionary<int, int> itemsRequired = new Dictionary<int, int>();

		public bool canBuildOnCurrentMap;

		public bool magical;

		public BluePrint(string name)
		{
			this.name = name;
			if (name.Equals("Info Tool"))
			{
				this.texture = Game1.content.Load<Texture2D>("LooseSprites\\Cursors");
				this.displayName = name;
				this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:BluePrint.cs.1", new object[0]);
				this.sourceRectForMenuView = new Rectangle(9 * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			Dictionary<string, string> arg_9A_0 = Game1.content.Load<Dictionary<string, string>>("Data\\Blueprints");
			string text = null;
			arg_9A_0.TryGetValue(name, out text);
			if (text != null)
			{
				string[] array = text.Split(new char[]
				{
					'/'
				});
				if (array[0].Equals("animal"))
				{
					try
					{
						this.texture = Game1.content.Load<Texture2D>("Animals\\" + (name.Equals("Chicken") ? "White Chicken" : name));
					}
					catch (Exception)
					{
						Game1.debugOutput = "Blueprint loaded with no texture!";
					}
					this.moneyRequired = Convert.ToInt32(array[1]);
					this.sourceRectForMenuView = new Rectangle(0, 0, Convert.ToInt32(array[2]), Convert.ToInt32(array[3]));
					this.blueprintType = "Animals";
					this.tilesWidth = 1;
					this.tilesHeight = 1;
					this.displayName = array[4];
					this.description = array[5];
					this.humanDoor = new Point(-1, -1);
					this.animalDoor = new Point(-1, -1);
					return;
				}
				try
				{
					this.texture = Game1.content.Load<Texture2D>("Buildings\\" + name);
				}
				catch (Exception)
				{
				}
				string[] array2 = array[0].Split(new char[]
				{
					' '
				});
				for (int i = 0; i < array2.Length; i += 2)
				{
					if (!array2[i].Equals(""))
					{
						this.itemsRequired.Add(Convert.ToInt32(array2[i]), Convert.ToInt32(array2[i + 1]));
					}
				}
				this.tilesWidth = Convert.ToInt32(array[1]);
				this.tilesHeight = Convert.ToInt32(array[2]);
				this.humanDoor = new Point(Convert.ToInt32(array[3]), Convert.ToInt32(array[4]));
				this.animalDoor = new Point(Convert.ToInt32(array[5]), Convert.ToInt32(array[6]));
				this.mapToWarpTo = array[7];
				this.displayName = array[8];
				this.description = array[9];
				this.blueprintType = array[10];
				if (this.blueprintType.Equals("Upgrades"))
				{
					this.nameOfBuildingToUpgrade = array[11];
				}
				this.sourceRectForMenuView = new Rectangle(0, 0, Convert.ToInt32(array[12]), Convert.ToInt32(array[13]));
				this.maxOccupants = Convert.ToInt32(array[14]);
				this.actionBehavior = array[15];
				string[] array3 = array[16].Split(new char[]
				{
					' '
				});
				for (int j = 0; j < array3.Length; j++)
				{
					string item = array3[j];
					this.namesOfOkayBuildingLocations.Add(item);
				}
				int num = 17;
				if (array.Length > num)
				{
					this.moneyRequired = Convert.ToInt32(array[17]);
				}
				if (array.Length > num + 1)
				{
					this.magical = Convert.ToBoolean(array[18]);
				}
			}
		}

		public void consumeResources()
		{
			foreach (KeyValuePair<int, int> current in this.itemsRequired)
			{
				Game1.player.consumeObject(current.Key, current.Value);
			}
			Game1.player.Money -= this.moneyRequired;
		}

		public int getTileSheetIndexForStructurePlacementTile(int x, int y)
		{
			if (x == this.humanDoor.X && y == this.humanDoor.Y)
			{
				return 2;
			}
			if (x == this.animalDoor.X && y == this.animalDoor.Y)
			{
				return 4;
			}
			return 0;
		}

		public bool isUpgrade()
		{
			return this.nameOfBuildingToUpgrade != null && this.nameOfBuildingToUpgrade.Length > 0;
		}

		public bool doesFarmerHaveEnoughResourcesToBuild()
		{
			foreach (KeyValuePair<int, int> current in this.itemsRequired)
			{
				if (!Game1.player.hasItemInInventory(current.Key, current.Value, 0))
				{
					return false;
				}
			}
			return Game1.player.Money >= this.moneyRequired;
		}

		public void drawDescription(SpriteBatch b, int x, int y, int width)
		{
			b.DrawString(Game1.smallFont, this.name, new Vector2((float)x, (float)y), Game1.textColor);
			string text = Game1.parseText(this.description, Game1.smallFont, width);
			b.DrawString(Game1.smallFont, text, new Vector2((float)x, (float)y + Game1.smallFont.MeasureString(this.name).Y), Game1.textColor * 0.75f);
			int num = (int)((float)y + Game1.smallFont.MeasureString(this.name).Y + Game1.smallFont.MeasureString(text).Y);
			foreach (KeyValuePair<int, int> current in this.itemsRequired)
			{
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(x + Game1.tileSize / 8), (float)num), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, current.Key, 16, 16)), Color.White, 0f, new Vector2(6f, 3f), (float)Game1.pixelZoom * 0.5f, SpriteEffects.None, 0.999f);
				Color color = Game1.player.hasItemInInventory(current.Key, current.Value, 0) ? Color.DarkGreen : Color.DarkRed;
				Utility.drawTinyDigits(current.Value, b, new Vector2((float)(x + Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat(current.Value)).X, (float)(num + Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat(current.Value)).Y), 1f, 0.9f, Color.AntiqueWhite);
				b.DrawString(Game1.smallFont, Game1.objectInformation[current.Key].Split(new char[]
				{
					'/'
				})[4], new Vector2((float)(x + Game1.tileSize / 2 + Game1.tileSize / 4), (float)num), color);
				num += (int)Game1.smallFont.MeasureString("P").Y;
			}
			if (this.moneyRequired > 0)
			{
				b.Draw(Game1.debrisSpriteSheet, new Vector2((float)x, (float)num), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 8), (float)(Game1.tileSize / 2 - Game1.tileSize / 3)), 0.5f, SpriteEffects.None, 0.999f);
				Color color2 = (Game1.player.money >= this.moneyRequired) ? Color.DarkGreen : Color.DarkRed;
				b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", new object[]
				{
					this.moneyRequired
				}), new Vector2((float)(x + Game1.tileSize / 4 + Game1.tileSize / 8), (float)num), color2);
				num += (int)Game1.smallFont.MeasureString(string.Concat(this.moneyRequired)).Y;
			}
		}
	}
}
