using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class DecoratableLocation : GameLocation
	{
		public List<int> wallPaper = new List<int>();

		public List<int> floor = new List<int>();

		public List<Furniture> furniture = new List<Furniture>();

		public DecoratableLocation()
		{
		}

		public DecoratableLocation(Map m, string name) : base(m, name)
		{
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			foreach (Furniture current in this.furniture)
			{
				if (current.furniture_type != 12 && current.getBoundingBox(current.tileLocation).Intersects(position))
				{
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, false, false);
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			if (character == null || character.willDestroyObjectsUnderfoot)
			{
				foreach (Furniture current in this.furniture)
				{
					if (current.furniture_type != 12 && current.getBoundingBox(current.tileLocation).Intersects(position))
					{
						return true;
					}
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public override bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
		{
			Vector2 vector = v * (float)Game1.tileSize;
			vector.X += (float)(Game1.tileSize / 2);
			vector.Y += (float)(Game1.tileSize / 2);
			foreach (Furniture current in this.furniture)
			{
				if (current.furniture_type != 12 && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y))
				{
					return false;
				}
			}
			return base.isTileLocationTotallyClearAndPlaceable(v);
		}

		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.minutesElapsed(10, this);
				}
			}
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			foreach (Furniture expr_1C in this.furniture)
			{
				expr_1C.minutesElapsed(3000 - Game1.timeOfDay, this);
				expr_1C.DayUpdate(this);
			}
		}

		public override bool leftClick(int x, int y, Farmer who)
		{
			if (Game1.activeClickableMenu != null)
			{
				return false;
			}
			for (int i = this.furniture.Count - 1; i >= 0; i--)
			{
				if (this.furniture[i].boundingBox.Contains(x, y) && this.furniture[i].clicked(who))
				{
					if (this.furniture[i].flaggedForPickUp && who.couldInventoryAcceptThisItem(this.furniture[i]))
					{
						this.furniture[i].flaggedForPickUp = false;
						this.furniture[i].performRemoveAction(new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize)), this);
						bool flag = false;
						for (int j = 0; j < 12; j++)
						{
							if (who.items[j] == null)
							{
								who.items[j] = this.furniture[i];
								who.CurrentToolIndex = j;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							Item item = who.addItemToInventory(this.furniture[i], 11);
							who.addItemToInventory(item);
							who.CurrentToolIndex = 11;
						}
						this.furniture.RemoveAt(i);
						Game1.playSound("coin");
					}
					return true;
				}
			}
			return base.leftClick(x, y, who);
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (base.checkAction(tileLocation, viewport, who))
			{
				return true;
			}
			Point value = new Point(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize);
			Point value2 = new Point(tileLocation.X * Game1.tileSize, (tileLocation.Y - 1) * Game1.tileSize);
			foreach (Furniture current in this.furniture)
			{
				if (current.boundingBox.Contains(value) && current.furniture_type != 12)
				{
					bool result;
					if (who.ActiveObject == null)
					{
						result = current.checkForAction(who, false);
						return result;
					}
					result = current.performObjectDropInAction(who.ActiveObject, false, who);
					return result;
				}
				else if (current.furniture_type == 6 && current.boundingBox.Contains(value2))
				{
					bool result;
					if (who.ActiveObject == null)
					{
						result = current.checkForAction(who, false);
						return result;
					}
					result = current.performObjectDropInAction(who.ActiveObject, false, who);
					return result;
				}
			}
			return false;
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (!Game1.player.mailReceived.Contains("button_tut_1"))
			{
				Game1.player.mailReceived.Add("button_tut_1");
				Game1.onScreenMenus.Add(new ButtonTutorialMenu(0));
			}
			if (!(this is FarmHouse))
			{
				this.setWallpapers();
				this.setFloors();
			}
			if (base.getTileIndexAt(Game1.player.getTileX(), Game1.player.getTileY(), "Buildings") != -1)
			{
				Farmer expr_85_cp_0_cp_0 = Game1.player;
				expr_85_cp_0_cp_0.position.Y = expr_85_cp_0_cp_0.position.Y + (float)Game1.tileSize;
			}
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.resetOnPlayerEntry(this);
				}
			}
		}

		public override void shiftObjects(int dx, int dy)
		{
			base.shiftObjects(dx, dy);
			foreach (Furniture expr_1D in this.furniture)
			{
				expr_1D.tileLocation.X = expr_1D.tileLocation.X + (float)dx;
				expr_1D.tileLocation.Y = expr_1D.tileLocation.Y + (float)dy;
				expr_1D.boundingBox.X = expr_1D.boundingBox.X + dx * Game1.tileSize;
				expr_1D.boundingBox.Y = expr_1D.boundingBox.Y + dy * Game1.tileSize;
				expr_1D.updateDrawPosition();
			}
			SerializableDictionary<Vector2, TerrainFeature> serializableDictionary = new SerializableDictionary<Vector2, TerrainFeature>();
			foreach (Vector2 current in this.terrainFeatures.Keys)
			{
				serializableDictionary.Add(new Vector2(current.X + (float)dx, current.Y + (float)dy), this.terrainFeatures[current]);
			}
			this.terrainFeatures = serializableDictionary;
		}

		public override bool isObjectAt(int x, int y)
		{
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.boundingBox.Contains(x, y))
					{
						return true;
					}
				}
			}
			return base.isObjectAt(x, y);
		}

		public override StardewValley.Object getObjectAt(int x, int y)
		{
			foreach (Furniture current in this.furniture)
			{
				if (current.boundingBox.Contains(x, y))
				{
					return current;
				}
			}
			return base.getObjectAt(x, y);
		}

		public void moveFurniture(int oldX, int oldY, int newX, int newY)
		{
			Vector2 vector = new Vector2((float)oldX, (float)oldY);
			foreach (Furniture current in this.furniture)
			{
				if (current.tileLocation.Equals(vector))
				{
					current.tileLocation = new Vector2((float)newX, (float)newY);
					current.boundingBox.X = newX * Game1.tileSize;
					current.boundingBox.Y = newY * Game1.tileSize;
					current.updateDrawPosition();
					return;
				}
			}
			if (this.objects.ContainsKey(vector))
			{
				StardewValley.Object @object = this.objects[vector];
				this.objects.Remove(vector);
				@object.tileLocation = new Vector2((float)newX, (float)newY);
				this.objects.Add(new Vector2((float)newX, (float)newY), @object);
			}
		}

		public bool isTileOnWall(int x, int y)
		{
			foreach (Microsoft.Xna.Framework.Rectangle current in DecoratableLocation.getWalls())
			{
				if (current.Contains(x, y))
				{
					return true;
				}
			}
			return false;
		}

		public static List<Microsoft.Xna.Framework.Rectangle> getWalls()
		{
			return new List<Microsoft.Xna.Framework.Rectangle>
			{
				new Microsoft.Xna.Framework.Rectangle(1, 1, 11, 3)
			};
		}

		public void setFloors()
		{
			for (int i = 0; i < this.floor.Count; i++)
			{
				this.setFloor(this.floor[i], i, true);
			}
		}

		public void setWallpapers()
		{
			for (int i = 0; i < this.wallPaper.Count; i++)
			{
				this.setWallpaper(this.wallPaper[i], i, true);
			}
		}

		public virtual void setWallpaper(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = DecoratableLocation.getWalls();
			if (persist)
			{
				while (this.wallPaper.Count < walls.Count)
				{
					this.wallPaper.Add(0);
				}
				if (whichRoom == -1)
				{
					for (int i = 0; i < this.wallPaper.Count; i++)
					{
						this.wallPaper[i] = which;
					}
				}
				else if (whichRoom <= this.wallPaper.Count - 1)
				{
					this.wallPaper[whichRoom] = which;
				}
			}
			int num = which % 16 + which / 16 * 48;
			if (whichRoom == -1)
			{
				using (List<Microsoft.Xna.Framework.Rectangle>.Enumerator enumerator = walls.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Microsoft.Xna.Framework.Rectangle current = enumerator.Current;
						for (int j = current.X; j < current.Right; j++)
						{
							base.setMapTileIndex(j, current.Y, num, "Back", 0);
							base.setMapTileIndex(j, current.Y + 1, num + 16, "Back", 0);
							if (current.Height >= 3)
							{
								if (this.map.GetLayer("Buildings").Tiles[j, current.Y + 2].TileSheet.Equals(this.map.TileSheets[2]))
								{
									base.setMapTileIndex(j, current.Y + 2, num + 32, "Buildings", 0);
								}
								else
								{
									base.setMapTileIndex(j, current.Y + 2, num + 32, "Back", 0);
								}
							}
						}
					}
					return;
				}
			}
			Microsoft.Xna.Framework.Rectangle rectangle = walls[Math.Min(walls.Count - 1, whichRoom)];
			for (int k = rectangle.X; k < rectangle.Right; k++)
			{
				base.setMapTileIndex(k, rectangle.Y, num, "Back", 0);
				base.setMapTileIndex(k, rectangle.Y + 1, num + 16, "Back", 0);
				if (rectangle.Height >= 3)
				{
					if (this.map.GetLayer("Buildings").Tiles[k, rectangle.Y + 2].TileSheet.Equals(this.map.TileSheets[2]))
					{
						base.setMapTileIndex(k, rectangle.Y + 2, num + 32, "Buildings", 0);
					}
					else
					{
						base.setMapTileIndex(k, rectangle.Y + 2, num + 32, "Back", 0);
					}
				}
			}
		}

		public override bool shouldShadowBeDrawnAboveBuildingsLayer(Vector2 p)
		{
			return base.getTileIndexAt((int)p.X, (int)p.Y, "Front") == -1;
		}

		public int getFloorAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = DecoratableLocation.getFloors();
			for (int i = 0; i < floors.Count; i++)
			{
				if (floors[i].Contains(p))
				{
					return i;
				}
			}
			return -1;
		}

		public Furniture getRandomFurniture(Random r)
		{
			if (this.furniture.Count > 0)
			{
				return this.furniture.ElementAt(r.Next(this.furniture.Count));
			}
			return null;
		}

		public int getWallForRoomAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = DecoratableLocation.getWalls();
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < walls.Count; j++)
				{
					if (walls[j].Contains(p))
					{
						return j;
					}
				}
				p.Y--;
			}
			return -1;
		}

		public virtual void setFloor(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = DecoratableLocation.getFloors();
			if (persist)
			{
				while (this.floor.Count < floors.Count)
				{
					this.floor.Add(0);
				}
				if (whichRoom == -1)
				{
					for (int i = 0; i < this.floor.Count; i++)
					{
						this.floor[i] = which;
					}
				}
				else
				{
					if (whichRoom > this.floor.Count - 1)
					{
						return;
					}
					this.floor[whichRoom] = which;
				}
			}
			int num = 336 + which % 8 * 2 + which / 8 * 32;
			if (whichRoom == -1)
			{
				using (List<Microsoft.Xna.Framework.Rectangle>.Enumerator enumerator = floors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Microsoft.Xna.Framework.Rectangle current = enumerator.Current;
						for (int j = current.X; j < current.Right; j += 2)
						{
							for (int k = current.Y; k < current.Bottom; k += 2)
							{
								if (current.Contains(j, k))
								{
									base.setMapTileIndex(j, k, num, "Back", 0);
								}
								if (current.Contains(j + 1, k))
								{
									base.setMapTileIndex(j + 1, k, num + 1, "Back", 0);
								}
								if (current.Contains(j, k + 1))
								{
									base.setMapTileIndex(j, k + 1, num + 16, "Back", 0);
								}
								if (current.Contains(j + 1, k + 1))
								{
									base.setMapTileIndex(j + 1, k + 1, num + 17, "Back", 0);
								}
							}
						}
					}
					return;
				}
			}
			Microsoft.Xna.Framework.Rectangle rectangle = floors[whichRoom];
			for (int l = rectangle.X; l < rectangle.Right; l += 2)
			{
				for (int m = rectangle.Y; m < rectangle.Bottom; m += 2)
				{
					if (rectangle.Contains(l, m))
					{
						base.setMapTileIndex(l, m, num, "Back", 0);
					}
					if (rectangle.Contains(l + 1, m))
					{
						base.setMapTileIndex(l + 1, m, num + 1, "Back", 0);
					}
					if (rectangle.Contains(l, m + 1))
					{
						base.setMapTileIndex(l, m + 1, num + 16, "Back", 0);
					}
					if (rectangle.Contains(l + 1, m + 1))
					{
						base.setMapTileIndex(l + 1, m + 1, num + 17, "Back", 0);
					}
				}
			}
		}

		public static List<Microsoft.Xna.Framework.Rectangle> getFloors()
		{
			return new List<Microsoft.Xna.Framework.Rectangle>
			{
				new Microsoft.Xna.Framework.Rectangle(1, 3, 11, 11)
			};
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b, -1, -1, 1f);
				}
			}
		}
	}
}
