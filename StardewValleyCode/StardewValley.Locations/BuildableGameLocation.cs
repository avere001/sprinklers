using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class BuildableGameLocation : GameLocation
	{
		public List<Building> buildings = new List<Building>();

		private Microsoft.Xna.Framework.Rectangle caveNoBuildRect = new Microsoft.Xna.Framework.Rectangle(32, 8, 5, 3);

		private Microsoft.Xna.Framework.Rectangle shippingAreaNoBuildRect = new Microsoft.Xna.Framework.Rectangle(69, 10, 4, 4);

		public BuildableGameLocation()
		{
		}

		public BuildableGameLocation(Map m, string name) : base(m, name)
		{
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.dayUpdate(dayOfMonth);
				}
			}
		}

		public virtual void timeUpdate(int timeElapsed)
		{
			foreach (Building current in this.buildings)
			{
				if (current.indoors != null && current.indoors.GetType() == typeof(AnimalHouse))
				{
					foreach (KeyValuePair<long, FarmAnimal> current2 in ((AnimalHouse)current.indoors).animals)
					{
						current2.Value.updatePerTenMinutes(Game1.timeOfDay, current.indoors);
					}
				}
			}
		}

		public Building getBuildingAt(Vector2 tile)
		{
			for (int i = this.buildings.Count - 1; i >= 0; i--)
			{
				if (!this.buildings[i].isTilePassable(tile))
				{
					return this.buildings[i];
				}
			}
			return null;
		}

		public bool destroyStructure(Vector2 tile)
		{
			for (int i = this.buildings.Count - 1; i >= 0; i--)
			{
				if (!this.buildings[i].isTilePassable(tile))
				{
					this.buildings[i].performActionOnDemolition(this);
					this.buildings.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool destroyStructure(Building b)
		{
			b.performActionOnDemolition(this);
			return this.buildings.Remove(b);
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			if (!glider)
			{
				foreach (Building current in this.buildings)
				{
					if (current.intersects(position))
					{
						if (character != null && character.GetType() == typeof(FarmAnimal))
						{
							Microsoft.Xna.Framework.Rectangle rectForAnimalDoor = current.getRectForAnimalDoor();
							rectForAnimalDoor.Height += Game1.tileSize;
							if (rectForAnimalDoor.Contains(position) && current.buildingType.Contains((character as FarmAnimal).buildingTypeILiveIn))
							{
								continue;
							}
						}
						else if (character != null && character is JunimoHarvester)
						{
							Microsoft.Xna.Framework.Rectangle rectForAnimalDoor2 = current.getRectForAnimalDoor();
							rectForAnimalDoor2.Height += Game1.tileSize;
							if (rectForAnimalDoor2.Contains(position))
							{
								continue;
							}
						}
						return true;
					}
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, projectile, ignoreCharacterRequirement);
		}

		public override bool isActionableTile(int xTile, int yTile, Farmer who)
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isActionableTile(xTile, yTile, who))
					{
						return true;
					}
				}
			}
			return base.isActionableTile(xTile, yTile, who);
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.doAction(new Vector2((float)tileLocation.X, (float)tileLocation.Y), who))
					{
						return true;
					}
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public override bool isTileOccupied(Vector2 tileLocation, string characterToIngore = "")
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.isTilePassable(tileLocation))
					{
						return true;
					}
				}
			}
			return base.isTileOccupied(tileLocation, "");
		}

		public override bool isTileOccupiedForPlacement(Vector2 tileLocation, StardewValley.Object toPlace = null)
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.isTilePassable(tileLocation))
					{
						return true;
					}
				}
			}
			return base.isTileOccupiedForPlacement(tileLocation, toPlace);
		}

		public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.updateWhenFarmNotCurrentLocation(time);
				}
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated && Game1.gameMode != 0)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Update(time);
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
		}

		public void tryToUpgrade(Building toUpgrade, BluePrint blueprint)
		{
			if (toUpgrade != null && blueprint.name != null && toUpgrade.buildingType.Equals(blueprint.nameOfBuildingToUpgrade))
			{
				if (toUpgrade.indoors.farmers.Count > 0)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\Locations:BuildableLocation_CantUpgrade_SomeoneInside", new object[0]), Color.Red, 3500f));
					return;
				}
				toUpgrade.indoors.map = Game1.game1.xTileContent.Load<Map>("Maps\\" + blueprint.mapToWarpTo);
				toUpgrade.indoors.name = blueprint.mapToWarpTo;
				toUpgrade.indoors.isStructure = true;
				toUpgrade.nameOfIndoorsWithoutUnique = blueprint.mapToWarpTo;
				toUpgrade.buildingType = blueprint.name;
				toUpgrade.texture = blueprint.texture;
				if (toUpgrade.indoors.GetType() == typeof(AnimalHouse))
				{
					((AnimalHouse)toUpgrade.indoors).resetPositionsOfAllAnimals();
				}
				Game1.playSound("axe");
				blueprint.consumeResources();
				toUpgrade.performActionOnUpgrade(this);
				toUpgrade.color = Color.White;
				Game1.exitActiveMenu();
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.broadcastBuildingChange(2, new Vector2((float)toUpgrade.tileX, (float)toUpgrade.tileY), blueprint.name, Game1.currentLocation.name, Game1.player.uniqueMultiplayerID);
					return;
				}
				return;
			}
			else
			{
				if (toUpgrade != null)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\Locations:BuildableLocation_CantUpgrade_IncorrectBuildingType", new object[0]), Color.Red, 3500f));
					return;
				}
				return;
			}
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.performActionOnPlayerLocationEntry();
				}
			}
		}

		public bool isBuildingConstructed(string name)
		{
			foreach (Building current in this.buildings)
			{
				if (current.buildingType.Equals(name) && current.daysOfConstructionLeft <= 0 && current.daysUntilUpgrade <= 0)
				{
					return true;
				}
			}
			return false;
		}

		public bool isThereABuildingUnderConstruction()
		{
			foreach (Building current in this.buildings)
			{
				if (current.daysOfConstructionLeft > 0 || current.daysUntilUpgrade > 0)
				{
					return true;
				}
			}
			return false;
		}

		public Building getBuildingUnderConstruction()
		{
			foreach (Building current in this.buildings)
			{
				if (current.daysOfConstructionLeft > 0 || current.daysUntilUpgrade > 0)
				{
					return current;
				}
			}
			return null;
		}

		public bool buildStructure(Building b, Vector2 tileLocation, bool serverMessage, Farmer who)
		{
			if (!serverMessage || !Game1.IsClient)
			{
				for (int i = 0; i < b.tilesHigh; i++)
				{
					for (int j = 0; j < b.tilesWide; j++)
					{
						Vector2 tileLocation2 = new Vector2(tileLocation.X + (float)j, tileLocation.Y + (float)i);
						if (!this.isBuildable(tileLocation2))
						{
							return false;
						}
						for (int k = 0; k < this.farmers.Count; k++)
						{
							if (this.farmers[k].GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(j * Game1.tileSize, i * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
							{
								return false;
							}
						}
					}
				}
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.broadcastBuildingChange(0, tileLocation, b.buildingType, this.name, who.uniqueMultiplayerID);
					if (Game1.IsClient)
					{
						return false;
					}
				}
			}
			string text = b.isThereAnythingtoPreventConstruction(this);
			if (text != null)
			{
				Game1.addHUDMessage(new HUDMessage(text, Color.Red, 3500f));
				return false;
			}
			b.tileX = (int)tileLocation.X;
			b.tileY = (int)tileLocation.Y;
			if (b.indoors != null && b.indoors is AnimalHouse)
			{
				foreach (long current in (b.indoors as AnimalHouse).animalsThatLiveHere)
				{
					FarmAnimal farmAnimal = Utility.getAnimal(current);
					if (farmAnimal != null)
					{
						farmAnimal.homeLocation = tileLocation;
						farmAnimal.home = b;
					}
					else if (farmAnimal == null && (b.indoors as AnimalHouse).animals.ContainsKey(current))
					{
						farmAnimal = (b.indoors as AnimalHouse).animals[current];
						farmAnimal.homeLocation = tileLocation;
						farmAnimal.home = b;
					}
				}
			}
			if (b.indoors != null)
			{
				foreach (Warp expr_1F9 in b.indoors.warps)
				{
					expr_1F9.TargetX = b.humanDoor.X + b.tileX;
					expr_1F9.TargetY = b.humanDoor.Y + b.tileY + 1;
				}
			}
			this.buildings.Add(b);
			return true;
		}

		public bool isBuildable(Vector2 tileLocation)
		{
			if ((!Game1.player.getTileLocation().Equals(tileLocation) || !Game1.player.currentLocation.Equals(this)) && !this.isTileOccupied(tileLocation, "") && base.isTilePassable(new Location((int)tileLocation.X, (int)tileLocation.Y), Game1.viewport) && base.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "NoFurniture", "Back") == null && !this.caveNoBuildRect.Contains(Utility.Vector2ToPoint(tileLocation)) && !this.shippingAreaNoBuildRect.Contains(Utility.Vector2ToPoint(tileLocation)))
			{
				if (Game1.currentLocation.doesTileHavePropertyNoNull((int)tileLocation.X, (int)tileLocation.Y, "Buildable", "Back").ToLower().Equals("t") || Game1.currentLocation.doesTileHavePropertyNoNull((int)tileLocation.X, (int)tileLocation.Y, "Buildable", "Back").ToLower().Equals("true"))
				{
					return true;
				}
				if (Game1.currentLocation.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Diggable", "Back") != null && !Game1.currentLocation.doesTileHavePropertyNoNull((int)tileLocation.X, (int)tileLocation.Y, "Buildable", "Back").ToLower().Equals("f"))
				{
					return true;
				}
			}
			return false;
		}

		public bool buildStructure(BluePrint structureForPlacement, Vector2 tileLocation, bool serverMessage, Farmer who, bool magicalConstruction = false)
		{
			if (!serverMessage || !Game1.IsClient)
			{
				for (int i = 0; i < structureForPlacement.tilesHeight; i++)
				{
					for (int j = 0; j < structureForPlacement.tilesWidth; j++)
					{
						Vector2 tileLocation2 = new Vector2(tileLocation.X + (float)j, tileLocation.Y + (float)i);
						if (!this.isBuildable(tileLocation2))
						{
							return false;
						}
						for (int k = 0; k < this.farmers.Count; k++)
						{
							if (this.farmers[k].GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(j * Game1.tileSize, i * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
							{
								return false;
							}
						}
					}
				}
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.broadcastBuildingChange(0, tileLocation, structureForPlacement.name, this.name, who.uniqueMultiplayerID);
					if (Game1.IsClient)
					{
						return false;
					}
				}
			}
			string name = structureForPlacement.name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			Building building;
			if (num <= 1972213674u)
			{
				if (num <= 846075854u)
				{
					if (num != 45101750u)
					{
						if (num != 846075854u)
						{
							goto IL_251;
						}
						if (!(name == "Big Barn"))
						{
							goto IL_251;
						}
						goto IL_233;
					}
					else
					{
						if (!(name == "Stable"))
						{
							goto IL_251;
						}
						building = new Stable(structureForPlacement, tileLocation);
						goto IL_259;
					}
				}
				else if (num != 1684694008u)
				{
					if (num != 1972213674u)
					{
						goto IL_251;
					}
					if (!(name == "Big Coop"))
					{
						goto IL_251;
					}
				}
				else if (!(name == "Coop"))
				{
					goto IL_251;
				}
			}
			else if (num <= 2601855023u)
			{
				if (num != 2575064728u)
				{
					if (num != 2601855023u)
					{
						goto IL_251;
					}
					if (!(name == "Deluxe Barn"))
					{
						goto IL_251;
					}
					goto IL_233;
				}
				else
				{
					if (!(name == "Junimo Hut"))
					{
						goto IL_251;
					}
					building = new JunimoHut(structureForPlacement, tileLocation);
					goto IL_259;
				}
			}
			else if (num != 3183088828u)
			{
				if (num != 3734277467u)
				{
					if (num != 3933183203u)
					{
						goto IL_251;
					}
					if (!(name == "Mill"))
					{
						goto IL_251;
					}
					building = new Mill(structureForPlacement, tileLocation);
					goto IL_259;
				}
				else if (!(name == "Deluxe Coop"))
				{
					goto IL_251;
				}
			}
			else
			{
				if (!(name == "Barn"))
				{
					goto IL_251;
				}
				goto IL_233;
			}
			building = new Coop(structureForPlacement, tileLocation);
			goto IL_259;
			IL_233:
			building = new Barn(structureForPlacement, tileLocation);
			goto IL_259;
			IL_251:
			building = new Building(structureForPlacement, tileLocation);
			IL_259:
			building.owner = who.uniqueMultiplayerID;
			string text = building.isThereAnythingtoPreventConstruction(this);
			if (text != null)
			{
				Game1.addHUDMessage(new HUDMessage(text, Color.Red, 3500f));
				return false;
			}
			this.buildings.Add(building);
			building.performActionOnConstruction(this);
			return true;
		}
	}
}
