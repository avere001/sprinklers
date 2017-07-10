using Microsoft.Xna.Framework;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class FarmHouse : DecoratableLocation
	{
		public int upgradeLevel;

		public int farmerNumberOfOwner;

		public bool fireplaceOn;

		public Chest fridge = new Chest(true);

		private int currentlyDisplayedUpgradeLevel;

		private bool displayingSpouseRoom;

		public FarmHouse()
		{
		}

		public FarmHouse(int ownerNumber = 1)
		{
			this.farmerNumberOfOwner = ownerNumber;
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
			foreach (NPC current in this.characters)
			{
				if (current.isMarried())
				{
					current.checkForMarriageDialogue(timeOfDay, this);
					if (Game1.timeOfDay == 2200)
					{
						current.controller = null;
						current.controller = new PathFindController(current, this, this.getSpouseBedSpot(), 0);
						if (current.controller.pathToEndPoint == null || !base.isTileOnMap(current.controller.pathToEndPoint.Last<Point>().X, current.controller.pathToEndPoint.Last<Point>().Y))
						{
							current.controller = null;
						}
					}
				}
				if (current is Child)
				{
					(current as Child).tenMinuteUpdate();
				}
			}
		}

		public Point getPorchStandingSpot()
		{
			int num = this.farmerNumberOfOwner;
			if (num == 0 || num == 1)
			{
				return new Point(66, 15);
			}
			return new Point(-1000, -1000);
		}

		public Point getKitchenStandingSpot()
		{
			switch (this.upgradeLevel)
			{
			case 1:
				return new Point(4, 5);
			case 2:
			case 3:
				return new Point(7, 14);
			default:
				return new Point(-1000, -1000);
			}
		}

		public Point getSpouseBedSpot()
		{
			switch (this.upgradeLevel)
			{
			case 1:
				return new Point(23, 4);
			case 2:
			case 3:
				return new Point(29, 13);
			default:
				return new Point(-1000, -1000);
			}
		}

		public Point getBedSpot()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return new Point(10, 9);
			case 1:
				return new Point(22, 4);
			case 2:
			case 3:
				return new Point(28, 13);
			default:
				return new Point(-1000, -1000);
			}
		}

		public Point getEntryLocation()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return new Point(3, 11);
			case 1:
				return new Point(9, 11);
			case 2:
			case 3:
				return new Point(12, 20);
			default:
				return new Point(-1000, -1000);
			}
		}

		public Point getChildBed(int gender)
		{
			if (gender == 0)
			{
				return new Point(23, 5);
			}
			if (gender == 1)
			{
				return new Point(27, 5);
			}
			return Point.Zero;
		}

		public Point getRandomOpenPointInHouse(Random r, int buffer = 0, int tries = 30)
		{
			Point zero = Point.Zero;
			for (int i = 0; i < tries; i++)
			{
				zero = new Point(r.Next(this.map.Layers[0].LayerWidth), r.Next(this.map.Layers[0].LayerHeight));
				Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(zero.X - buffer, zero.Y - buffer, 1 + buffer * 2, 1 + buffer * 2);
				bool flag = false;
				for (int j = rectangle.X; j < rectangle.Right; j++)
				{
					for (int k = rectangle.Y; k < rectangle.Bottom; k++)
					{
						flag = (base.getTileIndexAt(j, k, "Back") == -1 || !base.isTileLocationTotallyClearAndPlaceable(j, k) || Utility.pointInRectangles(FarmHouse.getWalls(this.upgradeLevel), j, k));
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return zero;
				}
			}
			return Point.Zero;
		}

		public void setSpouseInKitchen()
		{
			Farmer expr_0B = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner);
			NPC characterFromName = Game1.getCharacterFromName(expr_0B.spouse, false);
			if (expr_0B != null && characterFromName != null)
			{
				Game1.warpCharacter(characterFromName, this.name, this.getKitchenStandingSpot(), false, false);
				if (Game1.player.getSpouse().gender == 0)
				{
					characterFromName.spouseObstacleCheck(Game1.content.LoadString("Data\\ExtraDialogue:Spouse_KitchenBlocked", new object[0]).Split(new char[]
					{
						'/'
					}).First<string>(), this, false);
					return;
				}
				characterFromName.spouseObstacleCheck(Game1.content.LoadString("Data\\ExtraDialogue:Spouse_KitchenBlocked", new object[0]).Split(new char[]
				{
					'/'
				}).Last<string>(), this, false);
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex == 173)
				{
					this.fridge.fridge = true;
					this.fridge.checkForAction(who, false);
					return true;
				}
				switch (tileIndex)
				{
				case 794:
				case 795:
				case 796:
				case 797:
					this.fireplaceOn = !this.fireplaceOn;
					base.setFireplace(this.fireplaceOn, tileLocation.X - ((tileIndex == 795 || tileIndex == 797) ? 1 : 0), tileLocation.Y, true);
					return true;
				default:
					if (tileIndex == 2173)
					{
						if (Game1.player.eventsSeen.Contains(463391) && Game1.player.spouse != null && Game1.player.spouse.Equals("Emily"))
						{
							TemporaryAnimatedSprite temporarySpriteByID = base.getTemporarySpriteByID(5858585);
							if (temporarySpriteByID != null && temporarySpriteByID is EmilysParrot)
							{
								(temporarySpriteByID as EmilysParrot).doAction();
							}
						}
						return true;
					}
					break;
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public FarmHouse(Map m, string name) : base(m, name)
		{
			switch (Game1.whichFarm)
			{
			case 0:
				this.furniture.Add(new Furniture(1120, new Vector2(5f, 4f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1364, new Vector2(5f, 4f));
				this.furniture.Add(new Furniture(1376, new Vector2(1f, 10f)));
				this.furniture.Add(new Furniture(0, new Vector2(4f, 4f)));
				this.furniture.Add(new TV(1466, new Vector2(1f, 4f)));
				this.furniture.Add(new Furniture(1614, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1618, new Vector2(6f, 8f)));
				this.furniture.Add(new Furniture(1602, new Vector2(5f, 1f)));
				this.objects.Add(new Vector2(3f, 7f), new Chest(0, new List<Item>
				{
					new StardewValley.Object(472, 15, false, -1, 0)
				}, new Vector2(3f, 7f), true));
				return;
			case 1:
				this.setWallpaper(11, -1, true);
				this.setFloor(1, -1, true);
				this.furniture.Add(new Furniture(1122, new Vector2(1f, 6f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1367, new Vector2(1f, 6f));
				this.furniture.Add(new Furniture(3, new Vector2(1f, 5f)));
				this.furniture.Add(new TV(1680, new Vector2(5f, 4f)));
				this.furniture.Add(new Furniture(1673, new Vector2(1f, 1f)));
				this.furniture.Add(new Furniture(1673, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1676, new Vector2(5f, 1f)));
				this.furniture.Add(new Furniture(1737, new Vector2(6f, 8f)));
				this.furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
				this.furniture.Add(new Furniture(1675, new Vector2(10f, 1f)));
				this.objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new StardewValley.Object(472, 15, false, -1, 0)
				}, new Vector2(4f, 7f), true));
				return;
			case 2:
				this.setWallpaper(92, -1, true);
				this.setFloor(34, -1, true);
				this.furniture.Add(new Furniture(1134, new Vector2(1f, 7f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1748, new Vector2(1f, 7f));
				this.furniture.Add(new Furniture(3, new Vector2(1f, 6f)));
				this.furniture.Add(new TV(1680, new Vector2(6f, 4f)));
				this.furniture.Add(new Furniture(1296, new Vector2(1f, 4f)));
				this.furniture.Add(new Furniture(1682, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1777, new Vector2(6f, 5f)));
				this.furniture.Add(new Furniture(1745, new Vector2(6f, 1f)));
				this.furniture.Add(new Furniture(1747, new Vector2(5f, 4f)));
				this.furniture.Add(new Furniture(1296, new Vector2(10f, 4f)));
				this.objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new StardewValley.Object(472, 15, false, -1, 0)
				}, new Vector2(4f, 7f), true));
				return;
			case 3:
				this.setWallpaper(12, -1, true);
				this.setFloor(18, -1, true);
				this.furniture.Add(new Furniture(1218, new Vector2(1f, 6f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1368, new Vector2(1f, 6f));
				this.furniture.Add(new Furniture(1755, new Vector2(1f, 5f)));
				this.furniture.Add(new Furniture(1755, new Vector2(3f, 6f), 1));
				this.furniture.Add(new TV(1680, new Vector2(5f, 4f)));
				this.furniture.Add(new Furniture(1751, new Vector2(5f, 10f)));
				this.furniture.Add(new Furniture(1749, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1753, new Vector2(5f, 1f)));
				this.furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
				this.objects.Add(new Vector2(2f, 9f), new Chest(0, new List<Item>
				{
					new StardewValley.Object(472, 15, false, -1, 0)
				}, new Vector2(2f, 9f), true));
				return;
			case 4:
				this.setWallpaper(95, -1, true);
				this.setFloor(4, -1, true);
				this.furniture.Add(new TV(1680, new Vector2(1f, 4f)));
				this.furniture.Add(new Furniture(1628, new Vector2(1f, 5f)));
				this.furniture.Add(new Furniture(1393, new Vector2(3f, 4f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1369, new Vector2(3f, 4f));
				this.furniture.Add(new Furniture(1678, new Vector2(10f, 1f)));
				this.furniture.Add(new Furniture(1812, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1630, new Vector2(1f, 1f)));
				this.furniture.Add(new Furniture(1811, new Vector2(6f, 1f)));
				this.furniture.Add(new Furniture(1389, new Vector2(10f, 4f)));
				this.objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new StardewValley.Object(472, 15, false, -1, 0)
				}, new Vector2(4f, 7f), true));
				this.furniture.Add(new Furniture(1758, new Vector2(1f, 10f)));
				return;
			default:
				return;
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			this.fridge.updateWhenCurrentLocation(time);
			if (Game1.player.isMarried())
			{
				NPC characterFromName = base.getCharacterFromName(Game1.player.spouse);
				if (characterFromName != null && Game1.timeOfDay < 1500 && Game1.random.NextDouble() < 0.0006 && characterFromName.controller == null && characterFromName.Schedule == null && !characterFromName.getTileLocation().Equals(Utility.PointToVector2(this.getSpouseBedSpot())) && this.furniture.Count > 0)
				{
					Microsoft.Xna.Framework.Rectangle boundingBox = this.furniture[Game1.random.Next(this.furniture.Count)].boundingBox;
					Vector2 vector = new Vector2((float)(boundingBox.X / Game1.tileSize), (float)(boundingBox.Y / Game1.tileSize));
					int i = 0;
					int finalFacingDirection = -3;
					while (i < 3)
					{
						int num = Game1.random.Next(-1, 2);
						int num2 = Game1.random.Next(-1, 2);
						vector.X += (float)num;
						if (num == 0)
						{
							vector.Y += (float)num2;
						}
						if (num == -1)
						{
							finalFacingDirection = 1;
						}
						else if (num == 1)
						{
							finalFacingDirection = 3;
						}
						else if (num2 == -1)
						{
							finalFacingDirection = 2;
						}
						else if (num2 == 1)
						{
							finalFacingDirection = 0;
						}
						if (this.isTileLocationTotallyClearAndPlaceable(vector))
						{
							break;
						}
						i++;
					}
					if (i < 3)
					{
						base.getCharacterFromName(Game1.player.spouse).controller = new PathFindController(base.getCharacterFromName(Game1.player.spouse), this, new Point((int)vector.X, (int)vector.Y), finalFacingDirection);
					}
				}
				if (characterFromName != null && !characterFromName.isEmoting)
				{
					Vector2 tileLocation = characterFromName.getTileLocation();
					Vector2[] adjacentTilesOffsets = Character.AdjacentTilesOffsets;
					for (int j = 0; j < adjacentTilesOffsets.Length; j++)
					{
						Vector2 value = adjacentTilesOffsets[j];
						Vector2 vector2 = tileLocation + value;
						NPC nPC = base.isCharacterAtTile(vector2);
						if (nPC != null && nPC.IsMonster && !nPC.name.Equals("Cat"))
						{
							characterFromName.faceGeneralDirection(vector2 * new Vector2((float)Game1.tileSize, (float)Game1.tileSize), 0);
							Game1.showSwordswipeAnimation(characterFromName.facingDirection, characterFromName.position, 60f, false);
							Game1.playSound("swordswipe");
							characterFromName.shake(500);
							characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\Locations:FarmHouse_SpouseAttacked" + (Game1.random.Next(12) + 1), new object[0]), -1, 2, 3000, 0);
							((Monster)nPC).takeDamage(50, (int)Utility.getAwayFromPositionTrajectory(nPC.GetBoundingBox(), characterFromName.position).X, (int)Utility.getAwayFromPositionTrajectory(nPC.GetBoundingBox(), characterFromName.position).Y, false, 1.0);
							if (((Monster)nPC).health <= 0)
							{
								this.debris.Add(new Debris(nPC.sprite.Texture, Game1.random.Next(6, 16), new Vector2((float)nPC.getStandingX(), (float)nPC.getStandingY())));
								this.monsterDrop((Monster)nPC, nPC.getStandingX(), nPC.getStandingY());
								this.characters.Remove(nPC);
								Stats expr_377 = Game1.stats;
								uint monstersKilled = expr_377.MonstersKilled;
								expr_377.MonstersKilled = monstersKilled + 1u;
								Game1.player.changeFriendship(-10, characterFromName);
							}
							else
							{
								((Monster)nPC).shedChunks(4);
							}
							characterFromName.CurrentDialogue.Clear();
							characterFromName.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Data\\ExtraDialogue:Spouse_MonstersInHouse", new object[0]), characterFromName));
						}
					}
				}
			}
		}

		public Point getFireplacePoint()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return new Point(8, 4);
			case 1:
				return new Point(26, 4);
			case 2:
			case 3:
				return new Point(2, 13);
			default:
				return new Point(-50, -50);
			}
		}

		public bool shouldShowSpouseRoom()
		{
			bool result;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				result = Game1.player.isMarried();
			}
			else
			{
				result = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried();
			}
			return result;
		}

		public void showSpouseRoom()
		{
			int num = this.upgradeLevel;
			bool flag;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				flag = Game1.player.isMarried();
			}
			else
			{
				flag = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried();
			}
			bool arg_9F_0 = this.displayingSpouseRoom;
			this.displayingSpouseRoom = flag;
			this.map = Game1.game1.xTileContent.Load<Map>("Maps\\FarmHouse" + ((num == 0) ? "" : ((num == 3) ? "2" : string.Concat(num))) + (flag ? "_marriage" : ""));
			this.map.LoadTileSheets(Game1.mapDisplayDevice);
			if (arg_9F_0 && !this.displayingSpouseRoom)
			{
				Microsoft.Xna.Framework.Rectangle rectangle = default(Microsoft.Xna.Framework.Rectangle);
				switch (this.upgradeLevel)
				{
				case 1:
					rectangle = new Microsoft.Xna.Framework.Rectangle(29, 4, 6, 6);
					break;
				case 2:
				case 3:
					rectangle = new Microsoft.Xna.Framework.Rectangle(35, 13, 6, 6);
					break;
				}
				for (int i = rectangle.X; i <= rectangle.Right; i++)
				{
					for (int j = rectangle.Y; j <= rectangle.Bottom; j++)
					{
						Vector2 other = new Vector2((float)i, (float)j);
						for (int k = this.furniture.Count - 1; k >= 0; k--)
						{
							if (this.furniture[k].tileLocation.Equals(other))
							{
								Game1.createItemDebris(this.furniture[k], new Vector2((float)rectangle.X, (float)rectangle.Center.Y) * (float)Game1.tileSize, 3, null);
								this.furniture.RemoveAt(k);
							}
						}
					}
				}
			}
			base.loadObjects();
			if (num == 3)
			{
				base.setMapTileIndex(3, 22, 162, "Front", 0);
				base.removeTile(4, 22, "Front");
				base.removeTile(5, 22, "Front");
				base.setMapTileIndex(6, 22, 163, "Front", 0);
				base.setMapTileIndex(3, 23, 64, "Buildings", 0);
				base.setMapTileIndex(3, 24, 96, "Buildings", 0);
				base.setMapTileIndex(4, 24, 165, "Front", 0);
				base.setMapTileIndex(5, 24, 165, "Front", 0);
				base.removeTile(4, 23, "Back");
				base.removeTile(5, 23, "Back");
				base.setMapTileIndex(4, 23, 1043, "Back", 0);
				base.setMapTileIndex(5, 23, 1043, "Back", 0);
				base.setMapTileIndex(4, 24, 1075, "Back", 0);
				base.setMapTileIndex(5, 24, 1075, "Back", 0);
				base.setMapTileIndex(6, 23, 68, "Buildings", 0);
				base.setMapTileIndex(6, 24, 130, "Buildings", 0);
				base.setMapTileIndex(4, 25, 0, "Front", 0);
				base.setMapTileIndex(5, 25, 0, "Front", 0);
				base.removeTile(4, 23, "Buildings");
				base.removeTile(5, 23, "Buildings");
				this.warps.Add(new Warp(4, 25, "Cellar", 3, 2, false));
				this.warps.Add(new Warp(5, 25, "Cellar", 4, 2, false));
				if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
				{
					Game1.player.craftingRecipes.Add("Cask", 0);
				}
			}
			if (flag)
			{
				this.loadSpouseRoom();
			}
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (this.fireplaceOn)
			{
				Point fireplacePoint = this.getFireplacePoint();
				base.setFireplace(true, fireplacePoint.X, fireplacePoint.Y, false);
			}
			if (Game1.player.isMarried() && Game1.player.spouse.Equals("Emily") && Game1.player.eventsSeen.Contains(463391))
			{
				Vector2 location = new Vector2((float)(32 * Game1.tileSize + Game1.pixelZoom * 4), (float)(3 * Game1.tileSize - Game1.pixelZoom * 8));
				int num = this.upgradeLevel;
				if (num == 2 || num == 3)
				{
					location = new Vector2((float)(38 * Game1.tileSize + Game1.pixelZoom * 4), (float)(12 * Game1.tileSize - Game1.pixelZoom * 8));
				}
				this.temporarySprites.Add(new EmilysParrot(location));
			}
			if (this.currentlyDisplayedUpgradeLevel != this.upgradeLevel)
			{
				this.setMapForUpgradeLevel(this.upgradeLevel, false);
			}
			if ((!this.displayingSpouseRoom && this.shouldShowSpouseRoom()) || (this.displayingSpouseRoom && !this.shouldShowSpouseRoom()))
			{
				this.showSpouseRoom();
			}
			base.setWallpapers();
			base.setFloors();
			if (Game1.player.currentLocation == null || (!Game1.player.currentLocation.Equals(this) && !Game1.player.currentLocation.name.Equals("Cellar")))
			{
				switch (this.upgradeLevel)
				{
				case 1:
					Game1.player.position = new Vector2(9f, 11f) * (float)Game1.tileSize;
					break;
				case 2:
				case 3:
					Game1.player.position = new Vector2(12f, 20f) * (float)Game1.tileSize;
					break;
				}
				Game1.xLocationAfterWarp = Game1.player.getTileX();
				Game1.yLocationAfterWarp = Game1.player.getTileY();
				Game1.player.currentLocation = this;
			}
			if (Game1.timeOfDay >= 2200 && base.getCharacterFromName(Game1.player.spouse) != null && !Game1.player.spouse.Contains("engaged"))
			{
				NPC nPC = Game1.removeCharacterFromItsLocation(Game1.player.spouse);
				nPC.position = new Vector2((float)(this.getSpouseBedSpot().X * Game1.tileSize), (float)(this.getSpouseBedSpot().Y * Game1.tileSize));
				nPC.faceDirection(0);
				this.characters.Add(nPC);
			}
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i] is Pet && (!base.isTileOnMap(this.characters[i].getTileX(), this.characters[i].getTileY()) || base.getTileIndexAt(this.characters[i].getTileLocationPoint(), "Buildings") != -1 || base.getTileIndexAt(this.characters[i].getTileX() + 1, this.characters[i].getTileY(), "Buildings") != -1))
				{
					this.characters[i].faceDirection(2);
					Game1.warpCharacter(this.characters[i], "Farm", new Vector2(54f, 8f), false, false);
					break;
				}
			}
			Farm farm = Game1.getFarm();
			for (int j = this.characters.Count - 1; j >= 0; j--)
			{
				for (int k = j - 1; k >= 0; k--)
				{
					if (j < this.characters.Count && k < this.characters.Count && (this.characters[k].Equals(this.characters[j]) || (this.characters[k].name.Equals(this.characters[j].name) && this.characters[k].isVillager() && this.characters[j].isVillager())) && k != j)
					{
						this.characters.RemoveAt(k);
					}
				}
				for (int l = farm.characters.Count - 1; l >= 0; l--)
				{
					if (j < this.characters.Count && l < this.characters.Count && farm.characters[l].Equals(this.characters[j]))
					{
						farm.characters.RemoveAt(l);
					}
				}
			}
			if (Game1.timeOfDay >= 1800)
			{
				using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.isMarried();
					}
				}
			}
			foreach (NPC current in this.characters)
			{
				if (current is Child)
				{
					(current as Child).resetForPlayerEntry(this);
				}
				if (Game1.timeOfDay >= 2000)
				{
					current.controller = null;
					current.Halt();
				}
			}
		}

		public void moveObjectsForHouseUpgrade(int whichUpgrade)
		{
			switch (whichUpgrade)
			{
			case 0:
				if (this.upgradeLevel == 1)
				{
					this.shiftObjects(-6, 0);
					return;
				}
				break;
			case 1:
				if (this.upgradeLevel == 0)
				{
					this.shiftObjects(6, 0);
				}
				if (this.upgradeLevel == 2)
				{
					this.shiftObjects(-3, 0);
					return;
				}
				break;
			case 2:
			case 3:
				if (this.upgradeLevel == 1)
				{
					this.shiftObjects(3, 9);
					foreach (Furniture current in this.furniture)
					{
						if (current.tileLocation.X >= 10f && current.tileLocation.X <= 13f && current.tileLocation.Y >= 10f && current.tileLocation.Y <= 11f)
						{
							Furniture expr_D7_cp_0_cp_0 = current;
							expr_D7_cp_0_cp_0.tileLocation.X = expr_D7_cp_0_cp_0.tileLocation.X - 3f;
							Furniture expr_EB_cp_0_cp_0 = current;
							expr_EB_cp_0_cp_0.boundingBox.X = expr_EB_cp_0_cp_0.boundingBox.X - 3 * Game1.tileSize;
							Furniture expr_101_cp_0_cp_0 = current;
							expr_101_cp_0_cp_0.tileLocation.Y = expr_101_cp_0_cp_0.tileLocation.Y - 9f;
							Furniture expr_115_cp_0_cp_0 = current;
							expr_115_cp_0_cp_0.boundingBox.Y = expr_115_cp_0_cp_0.boundingBox.Y - 9 * Game1.tileSize;
							current.updateDrawPosition();
						}
					}
					base.moveFurniture(27, 13, 1, 4);
					base.moveFurniture(28, 13, 2, 4);
					base.moveFurniture(29, 13, 3, 4);
					base.moveFurniture(28, 14, 7, 4);
					base.moveFurniture(29, 14, 8, 4);
					base.moveFurniture(27, 14, 4, 4);
					base.moveFurniture(28, 15, 5, 4);
					base.moveFurniture(29, 16, 6, 4);
				}
				if (this.upgradeLevel == 0)
				{
					this.shiftObjects(9, 9);
				}
				break;
			default:
				return;
			}
		}

		public void setMapForUpgradeLevel(int level, bool persist = false)
		{
			if (persist)
			{
				this.upgradeLevel = level;
			}
			this.currentlyDisplayedUpgradeLevel = level;
			bool flag;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				flag = Game1.player.isMarried();
			}
			else
			{
				flag = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried();
			}
			if (this.displayingSpouseRoom && !flag)
			{
				this.displayingSpouseRoom = false;
			}
			this.map = Game1.game1.xTileContent.Load<Map>("Maps\\FarmHouse" + ((level == 0) ? "" : ((level == 3) ? "2" : string.Concat(level))) + (flag ? "_marriage" : ""));
			this.map.LoadTileSheets(Game1.mapDisplayDevice);
			if (flag)
			{
				this.showSpouseRoom();
			}
			base.loadObjects();
			if (level == 3)
			{
				base.setMapTileIndex(3, 22, 162, "Front", 0);
				base.removeTile(4, 22, "Front");
				base.removeTile(5, 22, "Front");
				base.setMapTileIndex(6, 22, 163, "Front", 0);
				base.setMapTileIndex(3, 23, 64, "Buildings", 0);
				base.setMapTileIndex(3, 24, 96, "Buildings", 0);
				base.setMapTileIndex(4, 24, 165, "Front", 0);
				base.setMapTileIndex(5, 24, 165, "Front", 0);
				base.removeTile(4, 23, "Back");
				base.removeTile(5, 23, "Back");
				base.setMapTileIndex(4, 23, 1043, "Back", 0);
				base.setMapTileIndex(5, 23, 1043, "Back", 0);
				base.setTileProperty(4, 23, "Back", "NoFurniture", "t");
				base.setTileProperty(5, 23, "Back", "NoFurniture", "t");
				base.setTileProperty(4, 23, "Back", "NPCBarrier", "t");
				base.setTileProperty(5, 23, "Back", "NPCBarrier", "t");
				base.setMapTileIndex(4, 24, 1075, "Back", 0);
				base.setMapTileIndex(5, 24, 1075, "Back", 0);
				base.setTileProperty(4, 24, "Back", "NoFurniture", "t");
				base.setTileProperty(5, 24, "Back", "NoFurniture", "t");
				base.setMapTileIndex(6, 23, 68, "Buildings", 0);
				base.setMapTileIndex(6, 24, 130, "Buildings", 0);
				base.setMapTileIndex(4, 25, 0, "Front", 0);
				base.setMapTileIndex(5, 25, 0, "Front", 0);
				base.removeTile(4, 23, "Buildings");
				base.removeTile(5, 23, "Buildings");
				this.warps.Add(new Warp(4, 25, "Cellar", 3, 2, false));
				this.warps.Add(new Warp(5, 25, "Cellar", 4, 2, false));
				if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
				{
					Game1.player.craftingRecipes.Add("Cask", 0);
				}
			}
			if (this.wallPaper.Count > 0 && this.floor.Count > 0)
			{
				List<Microsoft.Xna.Framework.Rectangle> list = FarmHouse.getWalls(this.upgradeLevel);
				if (persist)
				{
					while (this.wallPaper.Count < list.Count)
					{
						this.wallPaper.Add(0);
					}
				}
				list = FarmHouse.getFloors(this.upgradeLevel);
				if (persist)
				{
					while (this.floor.Count < list.Count)
					{
						this.floor.Add(0);
					}
				}
				if (this.upgradeLevel == 1)
				{
					this.setFloor(this.floor[0], 1, true);
					this.setFloor(this.floor[0], 2, true);
					this.setFloor(this.floor[0], 3, true);
					this.setFloor(22, 0, true);
				}
				if (this.upgradeLevel == 2)
				{
					this.setWallpaper(this.wallPaper[0], 4, true);
					this.setWallpaper(this.wallPaper[2], 6, true);
					this.setWallpaper(this.wallPaper[1], 5, true);
					this.setWallpaper(11, 0, true);
					this.setWallpaper(61, 1, true);
					this.setWallpaper(61, 2, true);
					int which = this.floor[3];
					this.setFloor(this.floor[2], 5, true);
					this.setFloor(this.floor[0], 3, true);
					this.setFloor(this.floor[1], 4, true);
					this.setFloor(which, 6, true);
					this.setFloor(1, 0, true);
					this.setFloor(31, 1, true);
					this.setFloor(31, 2, true);
				}
			}
			this.lightGlows.Clear();
		}

		public void loadSpouseRoom()
		{
			NPC spouse;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				spouse = Game1.player.getSpouse();
			}
			else
			{
				spouse = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).getSpouse();
			}
			if (spouse != null)
			{
				int num = -1;
				string name = spouse.name;
				uint num2 = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num2 <= 1866496948u)
				{
					if (num2 <= 1067922812u)
					{
						if (num2 != 161540545u)
						{
							if (num2 != 587846041u)
							{
								if (num2 == 1067922812u)
								{
									if (name == "Sam")
									{
										num = 9;
									}
								}
							}
							else if (name == "Penny")
							{
								num = 1;
							}
						}
						else if (name == "Sebastian")
						{
							num = 5;
						}
					}
					else if (num2 != 1281010426u)
					{
						if (num2 != 1708213605u)
						{
							if (num2 == 1866496948u)
							{
								if (name == "Shane")
								{
									num = 10;
								}
							}
						}
						else if (name == "Alex")
						{
							num = 6;
						}
					}
					else if (name == "Maru")
					{
						num = 4;
					}
				}
				else if (num2 <= 2571828641u)
				{
					if (num2 != 2010304804u)
					{
						if (num2 != 2434294092u)
						{
							if (num2 == 2571828641u)
							{
								if (name == "Emily")
								{
									num = 11;
								}
							}
						}
						else if (name == "Haley")
						{
							num = 3;
						}
					}
					else if (name == "Harvey")
					{
						num = 7;
					}
				}
				else if (num2 != 2732913340u)
				{
					if (num2 != 2826247323u)
					{
						if (num2 == 3066176300u)
						{
							if (name == "Elliott")
							{
								num = 8;
							}
						}
					}
					else if (name == "Leah")
					{
						num = 2;
					}
				}
				else if (name == "Abigail")
				{
					num = 0;
				}
				Microsoft.Xna.Framework.Rectangle rectangle = (this.upgradeLevel == 1) ? new Microsoft.Xna.Framework.Rectangle(29, 1, 6, 9) : new Microsoft.Xna.Framework.Rectangle(35, 10, 6, 9);
				Map map = Game1.game1.xTileContent.Load<Map>("Maps\\spouseRooms");
				Point point = new Point(num % 5 * 6, num / 5 * 9);
				this.map.Properties.Remove("DayTiles");
				this.map.Properties.Remove("NightTiles");
				for (int i = 0; i < rectangle.Width; i++)
				{
					for (int j = 0; j < rectangle.Height; j++)
					{
						if (map.GetLayer("Back").Tiles[point.X + i, point.Y + j] != null)
						{
							this.map.GetLayer("Back").Tiles[rectangle.X + i, rectangle.Y + j] = new StaticTile(this.map.GetLayer("Back"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Back").Tiles[point.X + i, point.Y + j].TileIndex);
						}
						if (map.GetLayer("Buildings").Tiles[point.X + i, point.Y + j] != null)
						{
							this.map.GetLayer("Buildings").Tiles[rectangle.X + i, rectangle.Y + j] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Buildings").Tiles[point.X + i, point.Y + j].TileIndex);
							base.adjustMapLightPropertiesForLamp(map.GetLayer("Buildings").Tiles[point.X + i, point.Y + j].TileIndex, rectangle.X + i, rectangle.Y + j, "Buildings");
						}
						else
						{
							this.map.GetLayer("Buildings").Tiles[rectangle.X + i, rectangle.Y + j] = null;
						}
						if (j < rectangle.Height - 1 && map.GetLayer("Front").Tiles[point.X + i, point.Y + j] != null)
						{
							this.map.GetLayer("Front").Tiles[rectangle.X + i, rectangle.Y + j] = new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Front").Tiles[point.X + i, point.Y + j].TileIndex);
							base.adjustMapLightPropertiesForLamp(map.GetLayer("Front").Tiles[point.X + i, point.Y + j].TileIndex, rectangle.X + i, rectangle.Y + j, "Front");
						}
						else if (j < rectangle.Height - 1)
						{
							this.map.GetLayer("Front").Tiles[rectangle.X + i, rectangle.Y + j] = null;
						}
						if (i == 4 && j == 4)
						{
							this.map.GetLayer("Back").Tiles[rectangle.X + i, rectangle.Y + j].Properties.Add(new KeyValuePair<string, PropertyValue>("NoFurniture", new PropertyValue("T")));
						}
					}
				}
			}
		}

		public void playerDivorced()
		{
			this.displayingSpouseRoom = false;
		}

		public new bool isTileOnWall(int x, int y)
		{
			foreach (Microsoft.Xna.Framework.Rectangle current in FarmHouse.getWalls(this.upgradeLevel))
			{
				if (current.Contains(x, y))
				{
					return true;
				}
			}
			return false;
		}

		public static List<Microsoft.Xna.Framework.Rectangle> getWalls(int upgradeLevel)
		{
			List<Microsoft.Xna.Framework.Rectangle> list = new List<Microsoft.Xna.Framework.Rectangle>();
			switch (upgradeLevel)
			{
			case 0:
				list.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 10, 3));
				break;
			case 1:
				list.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 17, 3));
				list.Add(new Microsoft.Xna.Framework.Rectangle(18, 6, 2, 2));
				list.Add(new Microsoft.Xna.Framework.Rectangle(20, 1, 9, 3));
				break;
			case 2:
			case 3:
				list.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 12, 3));
				list.Add(new Microsoft.Xna.Framework.Rectangle(15, 1, 13, 3));
				list.Add(new Microsoft.Xna.Framework.Rectangle(13, 3, 2, 2));
				list.Add(new Microsoft.Xna.Framework.Rectangle(1, 10, 10, 3));
				list.Add(new Microsoft.Xna.Framework.Rectangle(13, 10, 8, 3));
				list.Add(new Microsoft.Xna.Framework.Rectangle(21, 15, 2, 2));
				list.Add(new Microsoft.Xna.Framework.Rectangle(23, 10, 11, 3));
				break;
			}
			return list;
		}

		public override void setWallpaper(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = FarmHouse.getWalls(this.upgradeLevel);
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

		public new int getFloorAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = FarmHouse.getFloors(this.upgradeLevel);
			for (int i = 0; i < floors.Count; i++)
			{
				if (floors[i].Contains(p))
				{
					return i;
				}
			}
			return -1;
		}

		public new int getWallForRoomAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = FarmHouse.getWalls(this.upgradeLevel);
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

		public override void setFloor(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = FarmHouse.getFloors(this.upgradeLevel);
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

		public static List<Microsoft.Xna.Framework.Rectangle> getFloors(int upgradeLevel)
		{
			List<Microsoft.Xna.Framework.Rectangle> list = new List<Microsoft.Xna.Framework.Rectangle>();
			switch (upgradeLevel)
			{
			case 0:
				list.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 10, 9));
				break;
			case 1:
				list.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 6, 9));
				list.Add(new Microsoft.Xna.Framework.Rectangle(7, 3, 11, 9));
				list.Add(new Microsoft.Xna.Framework.Rectangle(18, 8, 2, 2));
				list.Add(new Microsoft.Xna.Framework.Rectangle(20, 3, 9, 8));
				break;
			case 2:
			case 3:
				list.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 12, 6));
				list.Add(new Microsoft.Xna.Framework.Rectangle(15, 3, 13, 6));
				list.Add(new Microsoft.Xna.Framework.Rectangle(13, 5, 2, 2));
				list.Add(new Microsoft.Xna.Framework.Rectangle(0, 12, 10, 11));
				list.Add(new Microsoft.Xna.Framework.Rectangle(10, 12, 11, 9));
				list.Add(new Microsoft.Xna.Framework.Rectangle(21, 17, 2, 2));
				list.Add(new Microsoft.Xna.Framework.Rectangle(23, 12, 11, 11));
				break;
			}
			return list;
		}
	}
}
