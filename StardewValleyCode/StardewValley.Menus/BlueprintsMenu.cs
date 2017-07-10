using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	public class BlueprintsMenu : IClickableMenu
	{
		public static int heightOfDescriptionBox = Game1.tileSize * 6;

		public static int blueprintButtonMargin = Game1.tileSize / 2;

		public new static int tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;

		public const int buildingsTab = 0;

		public const int upgradesTab = 1;

		public const int decorationsTab = 2;

		public const int demolishTab = 3;

		public const int animalsTab = 4;

		public const int numberOfTabs = 5;

		private bool placingStructure;

		private bool demolishing;

		private bool upgrading;

		private bool queryingAnimals;

		private int currentTab;

		private Vector2 positionOfAnimalWhenClicked;

		private string hoverText = "";

		private List<Dictionary<ClickableComponent, BluePrint>> blueprintButtons = new List<Dictionary<ClickableComponent, BluePrint>>();

		private List<ClickableComponent> tabs = new List<ClickableComponent>();

		private BluePrint hoveredItem;

		private BluePrint structureForPlacement;

		private FarmAnimal currentAnimal;

		private Texture2D buildingPlacementTiles;

		public BlueprintsMenu(int x, int y) : base(x, y, Game1.viewport.Width / 2 + Game1.tileSize * 3 / 2, 0, false)
		{
			BlueprintsMenu.tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;
			BlueprintsMenu.blueprintButtonMargin = Game1.tileSize / 2;
			BlueprintsMenu.heightOfDescriptionBox = Game1.tileSize * 6;
			for (int i = 0; i < 5; i++)
			{
				this.blueprintButtons.Add(new Dictionary<ClickableComponent, BluePrint>());
			}
			this.xPositionOnScreen = x;
			this.yPositionOnScreen = y;
			int[] array = new int[5];
			for (int j = 0; j < Game1.player.blueprints.Count; j++)
			{
				BluePrint bluePrint = new BluePrint(Game1.player.blueprints[j]);
				int tabNumberFromName = this.getTabNumberFromName(bluePrint.blueprintType);
				if (bluePrint.blueprintType != null)
				{
					int num = (int)((float)Math.Max(bluePrint.tilesWidth, 4) / 4f * (float)Game1.tileSize) + BlueprintsMenu.blueprintButtonMargin;
					if (array[tabNumberFromName] % (this.width - IClickableMenu.borderWidth * 2) + num > this.width - IClickableMenu.borderWidth * 2)
					{
						array[tabNumberFromName] += this.width - IClickableMenu.borderWidth * 2 - array[tabNumberFromName] % (this.width - IClickableMenu.borderWidth * 2);
					}
					this.blueprintButtons[Math.Min(4, tabNumberFromName)].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + array[tabNumberFromName] % (this.width - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + array[tabNumberFromName] / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, num, Game1.tileSize * 2), bluePrint.name), bluePrint);
					array[tabNumberFromName] += num;
				}
			}
			this.blueprintButtons[4].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + array[4] % (this.width - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + array[4] / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, Game1.tileSize + BlueprintsMenu.blueprintButtonMargin, Game1.tileSize * 2), "Info Tool"), new BluePrint("Info Tool"));
			int num2 = 0;
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] > num2)
				{
					num2 = array[k];
				}
			}
			this.height = Game1.tileSize * 2 + num2 / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + IClickableMenu.borderWidth * 4 + BlueprintsMenu.heightOfDescriptionBox;
			this.buildingPlacementTiles = Game1.content.Load<Texture2D>("LooseSprites\\buildingPlacementTiles");
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Buildings"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + Game1.tileSize + 4, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Upgrades"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 2, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Decorations"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 3, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Demolish"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 4, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Animals"));
		}

		public int getTabNumberFromName(string name)
		{
			int result = -1;
			if (!(name == "Buildings"))
			{
				if (!(name == "Upgrades"))
				{
					if (!(name == "Decorations"))
					{
						if (!(name == "Demolish"))
						{
							if (name == "Animals")
							{
								result = 4;
							}
						}
						else
						{
							result = 3;
						}
					}
					else
					{
						result = 2;
					}
				}
				else
				{
					result = 1;
				}
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public void changePosition(int x, int y)
		{
			int num = this.xPositionOnScreen - x;
			int num2 = this.yPositionOnScreen - y;
			this.xPositionOnScreen = x;
			this.yPositionOnScreen = y;
			using (List<Dictionary<ClickableComponent, BluePrint>>.Enumerator enumerator = this.blueprintButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (ClickableComponent expr_49 in enumerator.Current.Keys)
					{
						expr_49.bounds.X = expr_49.bounds.X + num;
						expr_49.bounds.Y = expr_49.bounds.Y - num2;
					}
				}
			}
			foreach (ClickableComponent expr_B0 in this.tabs)
			{
				expr_B0.bounds.X = expr_B0.bounds.X + num;
				expr_B0.bounds.Y = expr_B0.bounds.Y - num2;
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.currentAnimal != null)
			{
				this.currentAnimal = null;
				this.placingStructure = true;
				this.queryingAnimals = true;
			}
			if (!this.placingStructure)
			{
				Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height);
				foreach (ClickableComponent current in this.blueprintButtons[this.currentTab].Keys)
				{
					if (current.containsPoint(x, y))
					{
						if (current.name.Equals("Info Tool"))
						{
							this.placingStructure = true;
							this.queryingAnimals = true;
							Game1.playSound("smallSelect");
							return;
						}
						if (this.blueprintButtons[this.currentTab][current].doesFarmerHaveEnoughResourcesToBuild())
						{
							this.structureForPlacement = this.blueprintButtons[this.currentTab][current];
							this.placingStructure = true;
							if (this.currentTab == 1)
							{
								this.upgrading = true;
							}
							Game1.playSound("smallSelect");
							return;
						}
						Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10002", new object[0]), Color.Red, 3500f));
						return;
					}
				}
				foreach (ClickableComponent current2 in this.tabs)
				{
					if (current2.containsPoint(x, y))
					{
						this.currentTab = this.getTabNumberFromName(current2.name);
						Game1.playSound("smallSelect");
						if (this.currentTab == 3)
						{
							this.placingStructure = true;
							this.demolishing = true;
						}
						return;
					}
				}
				if (!rectangle.Contains(x, y))
				{
					Game1.exitActiveMenu();
					return;
				}
			}
			else if (this.demolishing)
			{
				Building buildingAt = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (buildingAt != null && ((Farm)Game1.getLocationFromName("Farm")).destroyStructure(buildingAt))
				{
					int groundLevelTile = buildingAt.tileY + buildingAt.tilesHigh;
					for (int i = 0; i < buildingAt.texture.Bounds.Height / Game1.tileSize; i++)
					{
						Game1.createRadialDebris(Game1.currentLocation, buildingAt.texture, new Microsoft.Xna.Framework.Rectangle(buildingAt.texture.Bounds.Center.X, buildingAt.texture.Bounds.Center.Y, Game1.tileSize / 16, Game1.tileSize / 16), buildingAt.tileX + Game1.random.Next(buildingAt.tilesWide), buildingAt.tileY + buildingAt.tilesHigh - i, Game1.random.Next(20, 45), groundLevelTile);
					}
					Game1.playSound("explosion");
					Utility.spreadAnimalsAround(buildingAt, (Farm)Game1.getLocationFromName("Farm"));
					return;
				}
				Game1.exitActiveMenu();
				return;
			}
			else if (this.upgrading && Game1.currentLocation.GetType() == typeof(Farm))
			{
				Building buildingAt2 = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (buildingAt2 != null && this.structureForPlacement.name != null && buildingAt2.buildingType.Equals(this.structureForPlacement.nameOfBuildingToUpgrade))
				{
					buildingAt2.indoors.map = Game1.game1.xTileContent.Load<Map>("Maps\\" + this.structureForPlacement.mapToWarpTo);
					buildingAt2.indoors.name = this.structureForPlacement.mapToWarpTo;
					buildingAt2.buildingType = this.structureForPlacement.name;
					buildingAt2.texture = this.structureForPlacement.texture;
					if (buildingAt2.indoors.GetType() == typeof(AnimalHouse))
					{
						((AnimalHouse)buildingAt2.indoors).resetPositionsOfAllAnimals();
					}
					Game1.playSound("axe");
					this.structureForPlacement.consumeResources();
					buildingAt2.color = Color.White;
					Game1.exitActiveMenu();
					return;
				}
				if (buildingAt2 != null)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10011", new object[0]), Color.Red, 3500f));
					return;
				}
				Game1.exitActiveMenu();
				return;
			}
			else
			{
				if (this.queryingAnimals)
				{
					if (!(Game1.currentLocation.GetType() == typeof(Farm)) && !(Game1.currentLocation.GetType() == typeof(AnimalHouse)))
					{
						return;
					}
					using (List<FarmAnimal>.Enumerator enumerator3 = ((Game1.currentLocation.GetType() == typeof(Farm)) ? ((Farm)Game1.currentLocation).animals.Values.ToList<FarmAnimal>() : ((AnimalHouse)Game1.currentLocation).animals.Values.ToList<FarmAnimal>()).GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							FarmAnimal current3 = enumerator3.Current;
							if (new Microsoft.Xna.Framework.Rectangle((int)current3.position.X, (int)current3.position.Y, current3.sprite.SourceRect.Width, current3.sprite.SourceRect.Height).Contains(Game1.viewport.X + Game1.getOldMouseX(), Game1.viewport.Y + Game1.getOldMouseY()))
							{
								this.positionOfAnimalWhenClicked = Game1.GlobalToLocal(Game1.viewport, current3.position);
								this.currentAnimal = current3;
								this.queryingAnimals = false;
								this.placingStructure = false;
								if (current3.sound != null && !current3.sound.Equals(""))
								{
									Game1.playSound(current3.sound);
								}
								break;
							}
						}
						return;
					}
				}
				if (Game1.currentLocation.GetType() != typeof(Farm))
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10012", new object[0]), Color.Red, 3500f));
					return;
				}
				if (!this.structureForPlacement.doesFarmerHaveEnoughResourcesToBuild())
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10002", new object[0]), Color.Red, 3500f));
					return;
				}
				if (this.tryToBuild())
				{
					this.structureForPlacement.consumeResources();
					if (!this.structureForPlacement.blueprintType.Equals("Animals"))
					{
						Game1.playSound("axe");
						return;
					}
				}
				else
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10016", new object[0]), Color.Red, 3500f));
				}
			}
		}

		public bool tryToBuild()
		{
			if (this.structureForPlacement.blueprintType.Equals("Animals"))
			{
				return ((Farm)Game1.getLocationFromName("Farm")).placeAnimal(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player.uniqueMultiplayerID);
			}
			return ((Farm)Game1.getLocationFromName("Farm")).buildStructure(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player, false);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.currentAnimal != null)
			{
				this.currentAnimal = null;
				this.queryingAnimals = true;
				this.placingStructure = true;
				return;
			}
			if (this.placingStructure)
			{
				this.placingStructure = false;
				this.queryingAnimals = false;
				this.upgrading = false;
				this.demolishing = false;
				return;
			}
			Game1.exitActiveMenu();
		}

		public override void performHoverAction(int x, int y)
		{
			if (this.demolishing)
			{
				using (List<Building>.Enumerator enumerator = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.color = Color.White;
					}
				}
				Building buildingAt = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (buildingAt != null)
				{
					buildingAt.color = Color.Red * 0.8f;
					return;
				}
			}
			else if (this.upgrading)
			{
				using (List<Building>.Enumerator enumerator = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.color = Color.White;
					}
				}
				Building buildingAt2 = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (buildingAt2 != null && this.structureForPlacement.nameOfBuildingToUpgrade != null && this.structureForPlacement.nameOfBuildingToUpgrade.Equals(buildingAt2.buildingType))
				{
					buildingAt2.color = Color.Green * 0.8f;
					return;
				}
				if (buildingAt2 != null)
				{
					buildingAt2.color = Color.Red * 0.8f;
					return;
				}
			}
			else if (!this.placingStructure)
			{
				foreach (ClickableComponent current in this.tabs)
				{
					if (current.containsPoint(x, y))
					{
						this.hoverText = current.name;
						return;
					}
				}
				this.hoverText = "";
				bool flag = false;
				foreach (ClickableComponent current2 in this.blueprintButtons[this.currentTab].Keys)
				{
					if (current2.containsPoint(x, y))
					{
						current2.scale = Math.Min(current2.scale + 0.01f, 1.1f);
						this.hoveredItem = this.blueprintButtons[this.currentTab][current2];
						flag = true;
					}
					else
					{
						current2.scale = Math.Max(current2.scale - 0.01f, 1f);
					}
				}
				if (!flag)
				{
					this.hoveredItem = null;
				}
			}
		}

		public int getTileSheetIndexForStructurePlacementTile(int x, int y)
		{
			if (x == this.structureForPlacement.humanDoor.X && y == this.structureForPlacement.humanDoor.Y)
			{
				return 2;
			}
			if (x == this.structureForPlacement.animalDoor.X && y == this.structureForPlacement.animalDoor.Y)
			{
				return 4;
			}
			return 0;
		}

		public override void draw(SpriteBatch b)
		{
			if (this.currentAnimal != null)
			{
				int num = (int)Math.Max(0f, Math.Min(this.positionOfAnimalWhenClicked.X - (float)(Game1.tileSize * 4) + (float)(Game1.tileSize / 2), (float)(Game1.viewport.Width - Game1.tileSize * 8)));
				int num2 = (int)Math.Max(0f, Math.Min((float)(Game1.viewport.Height - Game1.tileSize * 4 - this.currentAnimal.frontBackSourceRect.Height), this.positionOfAnimalWhenClicked.Y - (float)(Game1.tileSize * 4) - (float)this.currentAnimal.frontBackSourceRect.Height));
				Game1.drawDialogueBox(num, num2, Game1.tileSize * 8, Game1.tileSize * 5 + Game1.tileSize / 2, false, true, null, false);
				b.Draw(this.currentAnimal.sprite.Texture, new Vector2((float)(num + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 - this.currentAnimal.frontBackSourceRect.Width / 2), (float)(num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, this.currentAnimal.frontBackSourceRect.Width, this.currentAnimal.frontBackSourceRect.Height)), Color.White);
				float num3 = (float)this.currentAnimal.fullness / 255f;
				float num4 = (float)this.currentAnimal.happiness / 255f;
				string text = Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10026", new object[0]);
				string text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10027", new object[0]);
				b.DrawString(Game1.dialogueFont, this.currentAnimal.displayName, new Vector2((float)(num + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2) - Game1.dialogueFont.MeasureString(this.currentAnimal.name).X / 2f, (float)(num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + this.currentAnimal.frontBackSourceRect.Height + Game1.tileSize / 8)), Game1.textColor);
				b.DrawString(Game1.dialogueFont, text, new Vector2((float)(num + IClickableMenu.borderWidth + Game1.tileSize * 3), (float)(num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2)), Game1.textColor);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(num + IClickableMenu.borderWidth + Game1.tileSize * 3, num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString(text).Y + Game1.tileSize / 8, Game1.tileSize * 3, Game1.tileSize / 4), Color.Gray);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(num + IClickableMenu.borderWidth + Game1.tileSize * 3, num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString(text).Y + Game1.tileSize / 8, (int)((float)(Game1.tileSize * 3) * num3), Game1.tileSize / 4), ((double)num3 > 0.33) ? (((double)num3 > 0.66) ? Color.Green : Color.Goldenrod) : Color.Red);
				b.DrawString(Game1.dialogueFont, text2, new Vector2((float)(num + IClickableMenu.borderWidth + Game1.tileSize * 3), (float)(num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2) + Game1.dialogueFont.MeasureString(text).Y + (float)(Game1.tileSize / 2)), Game1.textColor);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(num + IClickableMenu.borderWidth + Game1.tileSize * 3, num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString(text).Y + (int)Game1.dialogueFont.MeasureString(text2).Y + Game1.tileSize / 2, Game1.tileSize * 3, Game1.tileSize / 4), Color.Gray);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(num + IClickableMenu.borderWidth + Game1.tileSize * 3, num2 + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString(text).Y + (int)Game1.dialogueFont.MeasureString(text2).Y + Game1.tileSize / 2, (int)((float)(Game1.tileSize * 3) * num4), Game1.tileSize / 4), ((double)num4 > 0.33) ? (((double)num4 > 0.66) ? Color.Green : Color.Goldenrod) : Color.Red);
			}
			else if (!this.placingStructure)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height - BlueprintsMenu.heightOfDescriptionBox, false, true, null, false);
				foreach (ClickableComponent current in this.tabs)
				{
					int tilePosition = 0;
					string name = current.name;
					if (!(name == "Buildings"))
					{
						if (!(name == "Upgrades"))
						{
							if (!(name == "Decorations"))
							{
								if (!(name == "Demolish"))
								{
									if (name == "Animals")
									{
										tilePosition = 8;
									}
								}
								else
								{
									tilePosition = 6;
								}
							}
							else
							{
								tilePosition = 7;
							}
						}
						else
						{
							tilePosition = 5;
						}
					}
					else
					{
						tilePosition = 4;
					}
					b.Draw(Game1.mouseCursors, new Vector2((float)current.bounds.X, (float)(current.bounds.Y + ((this.currentTab == this.getTabNumberFromName(current.name)) ? 8 : 0))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, tilePosition, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0001f);
				}
				foreach (ClickableComponent current2 in this.blueprintButtons[this.currentTab].Keys)
				{
					Texture2D texture = this.blueprintButtons[this.currentTab][current2].texture;
					Vector2 origin = current2.name.Equals("Info Tool") ? new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) : new Vector2((float)this.blueprintButtons[this.currentTab][current2].sourceRectForMenuView.Center.X, (float)this.blueprintButtons[this.currentTab][current2].sourceRectForMenuView.Center.Y);
					b.Draw(texture, new Vector2((float)current2.bounds.Center.X, (float)current2.bounds.Center.Y), new Microsoft.Xna.Framework.Rectangle?(this.blueprintButtons[this.currentTab][current2].sourceRectForMenuView), Color.White, 0f, origin, 0.25f * current2.scale + ((this.currentTab == 4) ? 0.75f : 0f), SpriteEffects.None, 0.9f);
				}
				Game1.drawWithBorder(this.hoverText, Color.Black, Color.White, new Vector2((float)(Game1.getOldMouseX() + Game1.tileSize), (float)(Game1.getOldMouseY() + Game1.tileSize)), 0f, 1f, 1f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen + (this.height - BlueprintsMenu.heightOfDescriptionBox) - IClickableMenu.borderWidth * 2, this.width, BlueprintsMenu.heightOfDescriptionBox, false, true, null, false);
				if (this.hoveredItem != null)
				{
				}
			}
			else if (!this.demolishing && !this.upgrading && !this.queryingAnimals)
			{
				Vector2 vector = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
				for (int i = 0; i < this.structureForPlacement.tilesHeight; i++)
				{
					for (int j = 0; j < this.structureForPlacement.tilesWidth; j++)
					{
						int num5 = this.getTileSheetIndexForStructurePlacementTile(j, i);
						Vector2 vector2 = new Vector2(vector.X + (float)j, vector.Y + (float)i);
						if (Game1.player.getTileLocation().Equals(vector2) || Game1.currentLocation.isTileOccupied(vector2, "") || !Game1.currentLocation.isTilePassable(new Location((int)vector2.X, (int)vector2.Y), Game1.viewport))
						{
							num5++;
						}
						b.Draw(this.buildingPlacementTiles, Game1.GlobalToLocal(Game1.viewport, vector2 * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(this.buildingPlacementTiles, num5, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
					}
				}
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, (this.queryingAnimals || this.currentAnimal != null) ? 9 : 0, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
		}
	}
}
