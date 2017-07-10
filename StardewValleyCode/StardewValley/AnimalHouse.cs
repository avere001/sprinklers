using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Events;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley
{
	public class AnimalHouse : GameLocation
	{
		public SerializableDictionary<long, FarmAnimal> animals = new SerializableDictionary<long, FarmAnimal>();

		public int animalLimit = 4;

		public List<long> animalsThatLiveHere = new List<long>();

		public Point incubatingEgg;

		private List<long> animalsToRemove = new List<long>();

		public AnimalHouse()
		{
		}

		public AnimalHouse(Map m, string name) : base(m, name)
		{
		}

		public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
		{
			if (!Game1.currentLocation.Equals(this))
			{
				for (int i = this.animals.Count - 1; i >= 0; i--)
				{
					this.animals.ElementAt(i).Value.updateWhenNotCurrentLocation(parentBuilding, time, this);
				}
			}
		}

		public void incubator()
		{
			if (this.incubatingEgg.Y <= 0 && Game1.player.ActiveObject != null && Game1.player.ActiveObject.Category == -5)
			{
				this.incubatingEgg.X = 2;
				this.incubatingEgg.Y = Game1.player.ActiveObject.ParentSheetIndex;
				this.map.GetLayer("Front").Tiles[1, 2].TileIndex += ((Game1.player.ActiveObject.ParentSheetIndex == 180 || Game1.player.ActiveObject.ParentSheetIndex == 182) ? 2 : 1);
				Game1.throwActiveObjectDown();
				return;
			}
			if (Game1.player.ActiveObject == null && this.incubatingEgg.Y > 0)
			{
				base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_RemoveEgg_Question", new object[0]), base.createYesNoResponses(), "RemoveIncubatingEgg");
			}
		}

		public bool isFull()
		{
			return this.animalsThatLiveHere.Count >= this.animalLimit;
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Microsoft.Xna.Framework.Rectangle value = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (who.ActiveObject != null && who.ActiveObject.Name.Equals("Hay") && base.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Trough", "Back") != null && !this.objects.ContainsKey(new Vector2((float)tileLocation.X, (float)tileLocation.Y)))
			{
				this.objects.Add(new Vector2((float)tileLocation.X, (float)tileLocation.Y), (Object)who.ActiveObject.getOne());
				who.reduceActiveItemByOne();
				Game1.playSound("coin");
				return false;
			}
			bool flag = base.checkAction(tileLocation, viewport, who);
			if (!flag)
			{
				foreach (KeyValuePair<long, FarmAnimal> current in this.animals)
				{
					if (current.Value.GetBoundingBox().Intersects(value) && !current.Value.wasPet)
					{
						current.Value.pet(who);
						bool result = true;
						return result;
					}
				}
				foreach (KeyValuePair<long, FarmAnimal> current2 in this.animals)
				{
					if (current2.Value.GetBoundingBox().Intersects(value))
					{
						current2.Value.pet(who);
						bool result = true;
						return result;
					}
				}
				return flag;
			}
			return flag;
		}

		public override bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
		{
			foreach (KeyValuePair<long, FarmAnimal> current in this.animals)
			{
				if (current.Value.getTileLocation().Equals(tileLocation))
				{
					return true;
				}
			}
			return base.isTileOccupiedForPlacement(tileLocation, toPlace);
		}

		public override void resetForPlayerEntry()
		{
			this.resetPositionsOfAllAnimals();
			foreach (Object current in this.objects.Values)
			{
				if (current.bigCraftable && current.Name.Contains("Incubator") && current.heldObject != null && current.minutesUntilReady <= 0 && !this.isFull())
				{
					string str = "??";
					int parentSheetIndex = current.heldObject.ParentSheetIndex;
					if (parentSheetIndex <= 176)
					{
						if (parentSheetIndex == 107)
						{
							str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_DinosaurEgg", new object[0]);
							goto IL_127;
						}
						if (parentSheetIndex != 174 && parentSheetIndex != 176)
						{
							goto IL_127;
						}
					}
					else if (parentSheetIndex <= 182)
					{
						if (parentSheetIndex != 180 && parentSheetIndex != 182)
						{
							goto IL_127;
						}
					}
					else
					{
						if (parentSheetIndex == 305)
						{
							str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_VoidEgg", new object[0]);
							goto IL_127;
						}
						if (parentSheetIndex != 442)
						{
							goto IL_127;
						}
						str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_DuckEgg", new object[0]);
						goto IL_127;
					}
					str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_RegularEgg", new object[0]);
					IL_127:
					this.currentEvent = new Event("none/-1000 -1000/farmer 2 9 0/pause 250/message \"" + str + "\"/pause 500/animalNaming/pause 500/end", -1);
					break;
				}
			}
			base.resetForPlayerEntry();
		}

		public Building getBuilding()
		{
			foreach (Building current in Game1.getFarm().buildings)
			{
				if (current.indoors != null && current.indoors.Equals(this))
				{
					return current;
				}
			}
			return null;
		}

		public void addNewHatchedAnimal(string name)
		{
			if (this.getBuilding() is Coop)
			{
				using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator = this.objects.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Object current = enumerator.Current;
						if (current.bigCraftable && current.Name.Contains("Incubator") && current.heldObject != null && current.minutesUntilReady <= 0 && !this.isFull())
						{
							string type = "??";
							if (current.heldObject == null)
							{
								type = "White Chicken";
							}
							else
							{
								int num = current.heldObject.ParentSheetIndex;
								if (num <= 176)
								{
									if (num != 107)
									{
										if (num == 174 || num == 176)
										{
											type = "White Chicken";
										}
									}
									else
									{
										type = "Dinosaur";
									}
								}
								else if (num <= 182)
								{
									if (num == 180 || num == 182)
									{
										type = "Brown Chicken";
									}
								}
								else if (num != 305)
								{
									if (num == 442)
									{
										type = "Duck";
									}
								}
								else
								{
									type = "Void Chicken";
								}
							}
							FarmAnimal farmAnimal = new FarmAnimal(type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
							farmAnimal.name = name;
							farmAnimal.displayName = name;
							Building building = this.getBuilding();
							farmAnimal.home = building;
							farmAnimal.homeLocation = new Vector2((float)building.tileX, (float)building.tileY);
							farmAnimal.setRandomPosition(farmAnimal.home.indoors);
							(building.indoors as AnimalHouse).animals.Add(farmAnimal.myID, farmAnimal);
							(building.indoors as AnimalHouse).animalsThatLiveHere.Add(farmAnimal.myID);
							current.heldObject = null;
							current.ParentSheetIndex = 101;
							break;
						}
					}
					goto IL_2D6;
				}
			}
			if (Game1.farmEvent != null && Game1.farmEvent is QuestionEvent)
			{
				FarmAnimal farmAnimal2 = new FarmAnimal((Game1.farmEvent as QuestionEvent).animal.type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
				farmAnimal2.name = name;
				farmAnimal2.displayName = name;
				farmAnimal2.parentId = (Game1.farmEvent as QuestionEvent).animal.myID;
				Building building2 = this.getBuilding();
				farmAnimal2.home = building2;
				farmAnimal2.homeLocation = new Vector2((float)building2.tileX, (float)building2.tileY);
				(Game1.farmEvent as QuestionEvent).forceProceed = true;
				farmAnimal2.setRandomPosition(farmAnimal2.home.indoors);
				(building2.indoors as AnimalHouse).animals.Add(farmAnimal2.myID, farmAnimal2);
				(building2.indoors as AnimalHouse).animalsThatLiveHere.Add(farmAnimal2.myID);
			}
			IL_2D6:
			if (Game1.currentLocation.currentEvent != null)
			{
				Event expr_2EC = Game1.currentLocation.currentEvent;
				int num = expr_2EC.CurrentCommand;
				expr_2EC.CurrentCommand = num + 1;
			}
			Game1.exitActiveMenu();
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			foreach (KeyValuePair<long, FarmAnimal> current in this.animals)
			{
				if (character != null && !character.Equals(current.Value) && position.Intersects(current.Value.GetBoundingBox()))
				{
					current.Value.farmerPushing();
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, false, false);
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			foreach (KeyValuePair<long, FarmAnimal> current in this.animals)
			{
				if (current.Value.updateWhenCurrentLocation(time, this))
				{
					this.animalsToRemove.Add(current.Key);
				}
			}
			for (int i = 0; i < this.animalsToRemove.Count; i++)
			{
				this.animals.Remove(this.animalsToRemove[i]);
			}
			this.animalsToRemove.Clear();
		}

		public void resetPositionsOfAllAnimals()
		{
			foreach (KeyValuePair<long, FarmAnimal> current in this.animals)
			{
				current.Value.setRandomPosition(this);
			}
		}

		public override bool dropObject(Object obj, Vector2 location, xTile.Dimensions.Rectangle viewport, bool initialPlacement, Farmer who = null)
		{
			Vector2 vector = new Vector2((float)((int)(location.X / (float)Game1.tileSize)), (float)((int)(location.Y / (float)Game1.tileSize)));
			if (!obj.Name.Equals("Hay") || base.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Trough", "Back") == null)
			{
				return base.dropObject(obj, location, viewport, initialPlacement, null);
			}
			if (!this.objects.ContainsKey(vector))
			{
				this.objects.Add(vector, obj);
				return true;
			}
			return false;
		}

		public void feedAllAnimals()
		{
			int num = 0;
			for (int i = 0; i < this.map.Layers[0].LayerWidth; i++)
			{
				for (int j = 0; j < this.map.Layers[0].LayerHeight; j++)
				{
					if (base.doesTileHaveProperty(i, j, "Trough", "Back") != null)
					{
						Vector2 key = new Vector2((float)i, (float)j);
						if (!this.objects.ContainsKey(key) && Game1.getFarm().piecesOfHay > 0)
						{
							this.objects.Add(key, new Object(178, 1, false, -1, 0));
							num++;
							Game1.getFarm().piecesOfHay--;
						}
						if (num >= this.animalLimit)
						{
							return;
						}
					}
				}
			}
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			foreach (KeyValuePair<long, FarmAnimal> current in this.animals)
			{
				current.Value.dayUpdate(this);
			}
		}

		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			if (t is MeleeWeapon)
			{
				foreach (FarmAnimal current in this.animals.Values)
				{
					if (current.GetBoundingBox().Intersects((t as MeleeWeapon).mostRecentArea))
					{
						current.hitWithWeapon(t as MeleeWeapon);
					}
				}
			}
			return false;
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			foreach (KeyValuePair<long, FarmAnimal> current in this.animals)
			{
				current.Value.draw(b);
			}
		}
	}
}
