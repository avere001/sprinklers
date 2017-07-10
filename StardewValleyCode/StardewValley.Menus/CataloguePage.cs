using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	public class CataloguePage : IClickableMenu
	{
		public static int widthToMoveActiveTab = Game1.tileSize / 8;

		public static int blueprintButtonMargin = Game1.tileSize / 2;

		public const int buildingsTab = 0;

		public const int upgradesTab = 1;

		public const int animalsTab = 2;

		public const int demolishTab = 3;

		public const int numberOfTabs = 4;

		private string descriptionText = "";

		private string hoverText = "";

		private InventoryMenu inventory;

		private Item heldItem;

		private int currentTab;

		private BluePrint hoveredItem;

		private List<ClickableTextureComponent> sideTabs = new List<ClickableTextureComponent>();

		private List<Dictionary<ClickableComponent, BluePrint>> blueprintButtons = new List<Dictionary<ClickableComponent, BluePrint>>();

		private bool demolishing;

		private bool upgrading;

		private bool placingStructure;

		private BluePrint structureForPlacement;

		private GameMenu parent;

		private Texture2D buildingPlacementTiles;

		public CataloguePage(int x, int y, int width, int height, GameMenu parent) : base(x, y, width, height, false)
		{
			this.parent = parent;
			this.buildingPlacementTiles = Game1.content.Load<Texture2D>("LooseSprites\\buildingPlacementTiles");
			CataloguePage.widthToMoveActiveTab = Game1.tileSize / 8;
			CataloguePage.blueprintButtonMargin = Game1.tileSize / 2;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, false, null, null, -1, 3, 0, 0, true);
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4 + CataloguePage.widthToMoveActiveTab, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), "", "Buildings", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 4, -1, -1), 1f, false));
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CataloguePage.cs.10138", new object[0]), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 5, -1, -1), 1f, false));
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 4, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CataloguePage.cs.10139", new object[0]), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 8, -1, -1), 1f, false));
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 5, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CataloguePage.cs.10140", new object[0]), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 6, -1, -1), 1f, false));
			for (int i = 0; i < 4; i++)
			{
				this.blueprintButtons.Add(new Dictionary<ClickableComponent, BluePrint>());
			}
			int num = Game1.tileSize * 8;
			int[] array = new int[4];
			for (int j = 0; j < Game1.player.blueprints.Count; j++)
			{
				BluePrint bluePrint = new BluePrint(Game1.player.blueprints[j]);
				if (CataloguePage.canPlaceThisBuildingOnTheCurrentMap(bluePrint, Game1.currentLocation))
				{
					bluePrint.canBuildOnCurrentMap = true;
				}
				int tabNumberFromName = this.getTabNumberFromName(bluePrint.blueprintType);
				if (bluePrint.blueprintType != null)
				{
					int num2 = (int)((float)Math.Max(bluePrint.tilesWidth, 4) / 4f * (float)Game1.tileSize) + CataloguePage.blueprintButtonMargin;
					if (array[tabNumberFromName] % (num - IClickableMenu.borderWidth * 2) + num2 > num - IClickableMenu.borderWidth * 2)
					{
						array[tabNumberFromName] += num - IClickableMenu.borderWidth * 2 - array[tabNumberFromName] % (num - IClickableMenu.borderWidth * 2);
					}
					this.blueprintButtons[Math.Min(3, tabNumberFromName)].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + array[tabNumberFromName] % (num - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + array[tabNumberFromName] / (num - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, num2, Game1.tileSize * 2), bluePrint.name), bluePrint);
					array[tabNumberFromName] += num2;
				}
			}
		}

		public int getTabNumberFromName(string name)
		{
			int result = -1;
			if (!(name == "Buildings"))
			{
				if (!(name == "Upgrades"))
				{
					if (!(name == "Demolish"))
					{
						if (name == "Animals")
						{
							result = 2;
						}
					}
					else
					{
						result = 3;
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

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.placingStructure)
			{
				this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
				for (int i = 0; i < this.sideTabs.Count; i++)
				{
					if (this.sideTabs[i].containsPoint(x, y) && this.currentTab != i)
					{
						Game1.playSound("smallSelect");
						if (i == 3)
						{
							this.placingStructure = true;
							this.demolishing = true;
							this.parent.invisible = true;
						}
						else
						{
							ClickableTextureComponent expr_8F_cp_0_cp_0 = this.sideTabs[this.currentTab];
							expr_8F_cp_0_cp_0.bounds.X = expr_8F_cp_0_cp_0.bounds.X - CataloguePage.widthToMoveActiveTab;
							this.currentTab = i;
							ClickableTextureComponent expr_B5_cp_0_cp_0 = this.sideTabs[i];
							expr_B5_cp_0_cp_0.bounds.X = expr_B5_cp_0_cp_0.bounds.X + CataloguePage.widthToMoveActiveTab;
						}
					}
				}
				using (Dictionary<ClickableComponent, BluePrint>.KeyCollection.Enumerator enumerator = this.blueprintButtons[this.currentTab].Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableComponent current = enumerator.Current;
						if (current.containsPoint(x, y))
						{
							if (this.blueprintButtons[this.currentTab][current].doesFarmerHaveEnoughResourcesToBuild())
							{
								this.structureForPlacement = this.blueprintButtons[this.currentTab][current];
								this.placingStructure = true;
								this.parent.invisible = true;
								if (this.currentTab == 1)
								{
									this.upgrading = true;
								}
								Game1.playSound("smallSelect");
								break;
							}
							Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10002", new object[0]), Color.Red, 3500f));
							break;
						}
					}
					return;
				}
			}
			if (this.demolishing)
			{
				if (!(Game1.currentLocation is Farm))
				{
					return;
				}
				if (Game1.IsClient)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:CataloguePage.cs.10148", new object[0]), Color.Red, 3500f));
					return;
				}
				Vector2 vector = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
				Building buildingAt = ((Farm)Game1.currentLocation).getBuildingAt(vector);
				if (Game1.IsMultiplayer && buildingAt != null && buildingAt.indoors.farmers.Count > 0)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:CataloguePage.cs.10149", new object[0]), Color.Red, 3500f));
					return;
				}
				if (buildingAt == null || !((Farm)Game1.currentLocation).destroyStructure(buildingAt))
				{
					this.parent.invisible = false;
					this.placingStructure = false;
					this.demolishing = false;
					return;
				}
				int groundLevelTile = buildingAt.tileY + buildingAt.tilesHigh;
				for (int j = 0; j < buildingAt.texture.Bounds.Height / Game1.tileSize; j++)
				{
					Game1.createRadialDebris(Game1.currentLocation, buildingAt.texture, new Microsoft.Xna.Framework.Rectangle(buildingAt.texture.Bounds.Center.X, buildingAt.texture.Bounds.Center.Y, Game1.tileSize / 16, Game1.tileSize / 16), buildingAt.tileX + Game1.random.Next(buildingAt.tilesWide), buildingAt.tileY + buildingAt.tilesHigh - j, Game1.random.Next(20, 45), groundLevelTile);
				}
				Game1.playSound("explosion");
				Utility.spreadAnimalsAround(buildingAt, (Farm)Game1.currentLocation);
				if (Game1.IsServer)
				{
					MultiplayerUtility.broadcastBuildingChange(1, vector, "", Game1.currentLocation.name, Game1.player.uniqueMultiplayerID);
					return;
				}
			}
			else
			{
				if (this.upgrading && Game1.currentLocation.GetType() == typeof(Farm))
				{
					(Game1.currentLocation as Farm).tryToUpgrade(((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize))), this.structureForPlacement);
					return;
				}
				if (!CataloguePage.canPlaceThisBuildingOnTheCurrentMap(this.structureForPlacement, Game1.currentLocation))
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:CataloguePage.cs.10152", new object[0]), Color.Red, 3500f));
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
				else if (!Game1.IsClient)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10016", new object[0]), Color.Red, 3500f));
				}
			}
		}

		public static bool canPlaceThisBuildingOnTheCurrentMap(BluePrint structureToPlace, GameLocation map)
		{
			return true;
		}

		private bool tryToBuild()
		{
			if (this.structureForPlacement.blueprintType.Equals("Animals"))
			{
				return ((Farm)Game1.getLocationFromName("Farm")).placeAnimal(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player.uniqueMultiplayerID);
			}
			return (Game1.currentLocation as BuildableGameLocation).buildStructure(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player, false);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.placingStructure)
			{
				this.placingStructure = false;
				this.upgrading = false;
				this.demolishing = false;
				this.parent.invisible = false;
				return;
			}
			this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
		}

		public override bool readyToClose()
		{
			return this.heldItem == null && !this.placingStructure;
		}

		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			foreach (ClickableTextureComponent current in this.sideTabs)
			{
				if (current.containsPoint(x, y))
				{
					this.hoverText = current.hoverText;
					return;
				}
			}
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
			if (this.demolishing)
			{
				using (List<Building>.Enumerator enumerator3 = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						enumerator3.Current.color = Color.White;
					}
				}
				Building buildingAt = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (buildingAt != null)
				{
					buildingAt.color = Color.Red * 0.8f;
				}
			}
			else if (this.upgrading)
			{
				using (List<Building>.Enumerator enumerator3 = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						enumerator3.Current.color = Color.White;
					}
				}
				Building buildingAt2 = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (buildingAt2 != null && this.structureForPlacement.nameOfBuildingToUpgrade != null && this.structureForPlacement.nameOfBuildingToUpgrade.Equals(buildingAt2.buildingType))
				{
					buildingAt2.color = Color.Green * 0.8f;
				}
				else if (buildingAt2 != null)
				{
					buildingAt2.color = Color.Red * 0.8f;
				}
			}
			if (!flag)
			{
				this.hoveredItem = null;
			}
		}

		private int getTileSheetIndexForStructurePlacementTile(int x, int y)
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

		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.placingStructure)
			{
				this.placingStructure = false;
				this.upgrading = false;
				this.demolishing = false;
				this.parent.invisible = false;
			}
		}

		public override void draw(SpriteBatch b)
		{
			if (!this.placingStructure)
			{
				using (List<ClickableTextureComponent>.Enumerator enumerator = this.sideTabs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize, false);
				base.drawVerticalUpperIntersectingPartition(b, this.xPositionOnScreen + Game1.tileSize * 9, 5 * Game1.tileSize + Game1.tileSize / 8);
				this.inventory.draw(b);
				foreach (ClickableComponent current in this.blueprintButtons[this.currentTab].Keys)
				{
					Texture2D texture = this.blueprintButtons[this.currentTab][current].texture;
					Vector2 origin = new Vector2((float)this.blueprintButtons[this.currentTab][current].sourceRectForMenuView.Center.X, (float)this.blueprintButtons[this.currentTab][current].sourceRectForMenuView.Center.Y);
					b.Draw(texture, new Vector2((float)current.bounds.Center.X, (float)current.bounds.Center.Y), new Microsoft.Xna.Framework.Rectangle?(this.blueprintButtons[this.currentTab][current].sourceRectForMenuView), this.blueprintButtons[this.currentTab][current].canBuildOnCurrentMap ? Color.White : (Color.Gray * 0.8f), 0f, origin, 1f * current.scale + ((this.currentTab == 2) ? 0.75f : 0f), SpriteEffects.None, 0.9f);
				}
				if (this.hoveredItem != null)
				{
					this.hoveredItem.drawDescription(b, this.xPositionOnScreen + Game1.tileSize * 9 + Game1.tileSize * 2 / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 2, Game1.tileSize * 3 + Game1.tileSize / 2);
				}
				if (this.heldItem != null)
				{
					this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
				}
				if (!this.hoverText.Equals(""))
				{
					IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
					return;
				}
			}
			else if (!this.demolishing && !this.upgrading)
			{
				Vector2 vector = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
				for (int i = 0; i < this.structureForPlacement.tilesHeight; i++)
				{
					for (int j = 0; j < this.structureForPlacement.tilesWidth; j++)
					{
						int num = this.getTileSheetIndexForStructurePlacementTile(j, i);
						Vector2 vector2 = new Vector2(vector.X + (float)j, vector.Y + (float)i);
						if (Game1.player.getTileLocation().Equals(vector2) || Game1.currentLocation.isTileOccupied(vector2, "") || !Game1.currentLocation.isTilePassable(new Location((int)vector2.X, (int)vector2.Y), Game1.viewport))
						{
							num++;
						}
						b.Draw(this.buildingPlacementTiles, Game1.GlobalToLocal(Game1.viewport, vector2 * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(this.buildingPlacementTiles, num, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
					}
				}
			}
		}
	}
}
