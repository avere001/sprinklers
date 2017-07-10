using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xTile;

namespace StardewValley.Buildings
{
	[XmlInclude(typeof(Coop)), XmlInclude(typeof(Barn)), XmlInclude(typeof(Stable)), XmlInclude(typeof(Mill)), XmlInclude(typeof(JunimoHut))]
	public class Building
	{
		public GameLocation indoors;

		[XmlIgnore]
		public Texture2D texture;

		public int tileX;

		public int tileY;

		public int tilesWide;

		public int tilesHigh;

		public int maxOccupants;

		public int currentOccupants;

		public int daysOfConstructionLeft;

		public int daysUntilUpgrade;

		public string buildingType;

		public string nameOfIndoors;

		public string baseNameOfIndoors;

		public string nameOfIndoorsWithoutUnique;

		public Point humanDoor;

		public Point animalDoor;

		public Color color = Color.White;

		public bool animalDoorOpen;

		public bool magical;

		public long owner;

		private int newConstructionTimer;

		protected float alpha;

		public static Rectangle leftShadow = new Rectangle(656, 394, 16, 16);

		public static Rectangle middleShadow = new Rectangle(672, 394, 16, 16);

		public static Rectangle rightShadow = new Rectangle(688, 394, 16, 16);

		public Building()
		{
		}

		public Building(string buildingType, string nameOfIndoors, int tileX, int tileY, int tilesWide, int tilesTall, Point humanDoor, Point animalDoor, GameLocation indoors, Texture2D texture, bool magical, long owner)
		{
			this.tileX = tileX;
			this.tileY = tileY;
			this.tilesWide = tilesWide;
			this.tilesHigh = tilesTall;
			this.buildingType = buildingType;
			this.nameOfIndoors = nameOfIndoors + (tileX * 2000 + tileY);
			this.texture = texture;
			this.indoors = indoors;
			this.baseNameOfIndoors = indoors.name;
			this.nameOfIndoorsWithoutUnique = this.baseNameOfIndoors;
			this.humanDoor = humanDoor;
			this.animalDoor = animalDoor;
			this.daysOfConstructionLeft = 2;
			this.magical = magical;
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

		public virtual void performTenMinuteAction(int timeElapsed)
		{
		}

		public virtual void performActionOnPlayerLocationEntry()
		{
			this.color = Color.White;
		}

		public virtual bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (who.IsMainPlayer && tileLocation.X >= (float)this.tileX && tileLocation.X < (float)(this.tileX + this.tilesWide) && tileLocation.Y >= (float)this.tileY && tileLocation.Y < (float)(this.tileY + this.tilesHigh) && this.daysOfConstructionLeft > 0)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:UnderConstruction", new object[0]));
			}
			else if (who.IsMainPlayer && tileLocation.X == (float)(this.humanDoor.X + this.tileX) && tileLocation.Y == (float)(this.humanDoor.Y + this.tileY) && this.indoors != null)
			{
				if (who.getMount() != null)
				{
					Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:DismountBeforeEntering", new object[0]));
					return false;
				}
				this.indoors.isStructure = true;
				this.indoors.uniqueName = this.baseNameOfIndoors + (this.tileX * 2000 + this.tileY);
				Game1.warpFarmer(this.indoors, this.indoors.warps[0].X, this.indoors.warps[0].Y - 1, Game1.player.facingDirection, true);
				Game1.playSound("doorClose");
				return true;
			}
			else if (who.IsMainPlayer && this.buildingType.Equals("Silo") && !this.isTilePassable(tileLocation))
			{
				if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 178)
				{
					if (who.ActiveObject.Stack == 0)
					{
						who.ActiveObject.stack = 1;
					}
					int stack = who.ActiveObject.Stack;
					int stack2 = (Game1.getLocationFromName("Farm") as Farm).tryToAddHay(who.ActiveObject.Stack);
					who.ActiveObject.stack = stack2;
					if (who.ActiveObject.stack < stack)
					{
						Game1.playSound("Ship");
						DelayedAction.playSoundAfterDelay("grassyStep", 100);
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:AddedHay", new object[]
						{
							stack - who.ActiveObject.Stack
						}));
					}
					if (who.ActiveObject.Stack <= 0)
					{
						who.removeItemFromInventory(who.ActiveObject);
					}
				}
				else
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:PiecesOfHay", new object[]
					{
						(Game1.getLocationFromName("Farm") as Farm).piecesOfHay,
						Utility.numSilos() * 240
					}));
				}
			}
			else if (who.IsMainPlayer && this.buildingType.Contains("Obelisk") && !this.isTilePassable(tileLocation))
			{
				for (int i = 0; i < 12; i++)
				{
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float)Game1.random.Next(25, 75), 6, 1, new Vector2((float)Game1.random.Next((int)who.position.X - Game1.tileSize * 4, (int)who.position.X + Game1.tileSize * 3), (float)Game1.random.Next((int)who.position.Y - Game1.tileSize * 4, (int)who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
				}
				Game1.playSound("wand");
				Game1.displayFarmer = false;
				Game1.player.freezePause = 1000;
				Game1.flashAlpha = 1f;
				DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.obeliskWarpForReal), 1000);
				Rectangle rectangle = new Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize);
				rectangle.Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
				int num = 0;
				for (int j = who.getTileX() + 8; j >= who.getTileX() - 8; j--)
				{
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)j, (float)who.getTileY()) * (float)Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
					{
						layerDepth = 1f,
						delayBeforeAnimationStart = num * 25,
						motion = new Vector2(-0.25f, 0f)
					});
					num++;
				}
			}
			return false;
		}

		private void obeliskWarpForReal()
		{
			string a = this.buildingType;
			if (!(a == "Earth Obelisk"))
			{
				if (a == "Water Obelisk")
				{
					Game1.warpFarmer("Beach", 20, 4, false);
				}
			}
			else
			{
				Game1.warpFarmer("Mountain", 31, 20, false);
			}
			Game1.fadeToBlackAlpha = 0.99f;
			Game1.screenGlow = false;
			Game1.player.temporarilyInvincible = false;
			Game1.player.temporaryInvincibilityTimer = 0;
			Game1.displayFarmer = true;
		}

		public virtual bool isActionableTile(int xTile, int yTile, Farmer who)
		{
			return (this.humanDoor.X >= 0 && xTile == this.tileX + this.humanDoor.X && yTile == this.tileY + this.humanDoor.Y) || (this.animalDoor.X >= 0 && xTile == this.tileX + this.animalDoor.X && yTile == this.tileY + this.animalDoor.Y);
		}

		public virtual void performActionOnConstruction(GameLocation location)
		{
			this.daysOfConstructionLeft = 2;
			this.newConstructionTimer = (this.magical ? 2000 : 1000);
			if (!this.magical)
			{
				Game1.playSound("axchop");
				for (int i = this.tileX; i < this.tileX + this.tilesWide; i++)
				{
					for (int j = this.tileY; j < this.tileY + this.tilesHigh; j++)
					{
						for (int k = 0; k < 5; k++)
						{
							location.temporarySprites.Add(new TemporaryAnimatedSprite((Game1.random.NextDouble() < 0.5) ? 46 : 12, new Vector2((float)i, (float)j) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2)), Color.White, 10, Game1.random.NextDouble() < 0.5, 100f, 0, -1, -1f, -1, 0)
							{
								delayBeforeAnimationStart = Math.Max(0, Game1.random.Next(-200, 400)),
								motion = new Vector2(0f, -1f),
								interval = (float)Game1.random.Next(50, 80)
							});
						}
						location.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2((float)i, (float)j) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2)), Color.White, 10, Game1.random.NextDouble() < 0.5, 100f, 0, -1, -1f, -1, 0));
					}
				}
				for (int l = 0; l < 8; l++)
				{
					DelayedAction.playSoundAfterDelay("dirtyHit", 250 + l * 150);
				}
				return;
			}
			for (int m = 0; m < 8; m++)
			{
				DelayedAction.playSoundAfterDelay("dirtyHit", 100 + m * 210);
			}
			Game1.flashAlpha = 2f;
			Game1.playSound("wand");
			for (int n = 0; n < this.getSourceRectForMenu().Width / 16 * 2; n++)
			{
				for (int num = this.texture.Bounds.Height / 16 * 2; num >= 0; num--)
				{
					location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(666, 1851, 8, 8), 40f, 4, 2, new Vector2((float)this.tileX, (float)this.tileY) * (float)Game1.tileSize + new Vector2((float)(n * Game1.tileSize / 2), (float)(num * Game1.tileSize / 2 - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, false)
					{
						layerDepth = (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + (float)n / 10000f,
						pingPong = true,
						delayBeforeAnimationStart = (this.texture.Bounds.Height / 16 * 2 - num) * 100,
						scale = (float)Game1.pixelZoom,
						alphaFade = 0.01f,
						color = Color.AliceBlue
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(666, 1851, 8, 8), 40f, 4, 2, new Vector2((float)this.tileX, (float)this.tileY) * (float)Game1.tileSize + new Vector2((float)(n * Game1.tileSize / 2), (float)(num * Game1.tileSize / 2 - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, false)
					{
						layerDepth = (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + (float)n / 10000f + 0.0001f,
						pingPong = true,
						delayBeforeAnimationStart = (this.texture.Bounds.Height / 16 * 2 - num) * 100,
						scale = (float)Game1.pixelZoom,
						alphaFade = 0.01f,
						color = Color.AliceBlue
					});
				}
			}
		}

		public virtual void performActionOnDemolition(GameLocation location)
		{
		}

		public virtual void performActionOnUpgrade(GameLocation location)
		{
		}

		public virtual string isThereAnythingtoPreventConstruction(GameLocation location)
		{
			return null;
		}

		public virtual void updateWhenFarmNotCurrentLocation(GameTime time)
		{
			if (this.indoors != null)
			{
				this.indoors.updateEvenIfFarmerIsntHere(time, false);
			}
		}

		public virtual void Update(GameTime time)
		{
			if (this.newConstructionTimer > 0)
			{
				this.newConstructionTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.newConstructionTimer <= 0 && this.magical)
				{
					this.daysOfConstructionLeft = 0;
				}
			}
			this.alpha = Math.Min(1f, this.alpha + 0.05f);
			if (Game1.player.GetBoundingBox().Intersects(new Rectangle(Game1.tileSize * this.tileX, Game1.tileSize * (this.tileY + (-(this.getSourceRectForMenu().Height / 16) + this.tilesHigh)), this.tilesWide * Game1.tileSize, (this.getSourceRectForMenu().Height / 16 - this.tilesHigh) * Game1.tileSize + Game1.tileSize / 2)))
			{
				this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
			}
		}

		public void showUpgradeAnimation(GameLocation location)
		{
			this.color = Color.White;
			location.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.getUpgradeSignLocation() + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), Color.Beige, 10, Game1.random.NextDouble() < 0.5, 75f, 0, -1, -1f, -1, 0)
			{
				motion = new Vector2(0f, -0.5f),
				acceleration = new Vector2(-0.02f, 0.01f),
				delayBeforeAnimationStart = Game1.random.Next(100),
				layerDepth = 0.89f
			});
			location.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.getUpgradeSignLocation() + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), Color.Beige, 10, Game1.random.NextDouble() < 0.5, 75f, 0, -1, -1f, -1, 0)
			{
				motion = new Vector2(0f, -0.5f),
				acceleration = new Vector2(-0.02f, 0.01f),
				delayBeforeAnimationStart = Game1.random.Next(40),
				layerDepth = 0.89f
			});
		}

		public virtual Vector2 getUpgradeSignLocation()
		{
			return new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize / 2), (float)(this.tileY * Game1.tileSize - Game1.tileSize / 2));
		}

		public string getNameOfNextUpgrade()
		{
			string a = this.buildingType.ToLower();
			if (a == "coop")
			{
				return "Big Coop";
			}
			if (a == "big coop")
			{
				return "Deluxe Coop";
			}
			if (a == "barn")
			{
				return "Big Barn";
			}
			if (!(a == "big barn"))
			{
				return "well";
			}
			return "Deluxe Barn";
		}

		public void showDestroyedAnimation(GameLocation location)
		{
			for (int i = this.tileX; i < this.tileX + this.tilesWide; i++)
			{
				for (int j = this.tileY; j < this.tileY + this.tilesHigh; j++)
				{
					location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2((float)(i * Game1.tileSize), (float)(j * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5)
					{
						delayBeforeAnimationStart = Game1.random.Next(300)
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2((float)(i * Game1.tileSize), (float)(j * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5)
					{
						delayBeforeAnimationStart = 250 + Game1.random.Next(300)
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2((float)i, (float)j) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, 0f, Color.White)
					{
						interval = 30f,
						totalNumberOfLoops = 99999,
						animationLength = 4,
						scale = (float)Game1.pixelZoom,
						alphaFade = 0.01f
					});
				}
			}
		}

		public virtual void dayUpdate(int dayOfMonth)
		{
			if (this.daysOfConstructionLeft > 0 && !Utility.isFestivalDay(dayOfMonth, Game1.currentSeason))
			{
				this.daysOfConstructionLeft--;
				if (this.daysOfConstructionLeft <= 0)
				{
					Game1.player.checkForQuestComplete(null, -1, -1, null, this.buildingType, 8, -1);
					if (this.buildingType.Equals("Slime Hutch") && this.indoors != null)
					{
						this.indoors.objects.Add(new Vector2(1f, 4f), new StardewValley.Object(new Vector2(1f, 4f), 156, false)
						{
							fragility = 2
						});
						if (!Game1.player.mailReceived.Contains("slimeHutchBuilt"))
						{
							Game1.player.mailReceived.Add("slimeHutchBuilt");
						}
					}
				}
				return;
			}
			if (this.daysUntilUpgrade > 0 && !Utility.isFestivalDay(dayOfMonth, Game1.currentSeason))
			{
				this.daysUntilUpgrade--;
				if (this.daysUntilUpgrade <= 0)
				{
					Game1.player.checkForQuestComplete(null, -1, -1, null, this.getNameOfNextUpgrade(), 8, -1);
					BluePrint bluePrint = new BluePrint(this.getNameOfNextUpgrade());
					this.indoors.map = Game1.game1.xTileContent.Load<Map>("Maps\\" + bluePrint.mapToWarpTo);
					this.indoors.name = bluePrint.mapToWarpTo;
					this.buildingType = bluePrint.name;
					this.texture = bluePrint.texture;
					if (this.indoors.GetType() == typeof(AnimalHouse))
					{
						((AnimalHouse)this.indoors).resetPositionsOfAllAnimals();
						((AnimalHouse)this.indoors).animalLimit += 4;
						((AnimalHouse)this.indoors).loadLights();
					}
					this.upgrade();
				}
			}
			if (this.indoors != null)
			{
				this.indoors.DayUpdate(dayOfMonth);
			}
			if (this.buildingType.Contains("Deluxe"))
			{
				(this.indoors as AnimalHouse).feedAllAnimals();
			}
		}

		public virtual void upgrade()
		{
		}

		public virtual Rectangle getSourceRectForMenu()
		{
			return this.texture.Bounds;
		}

		public Building(BluePrint blueprint, Vector2 tileLocation)
		{
			this.tileX = (int)tileLocation.X;
			this.tileY = (int)tileLocation.Y;
			this.tilesWide = blueprint.tilesWidth;
			this.tilesHigh = blueprint.tilesHeight;
			this.buildingType = blueprint.name;
			this.texture = blueprint.texture;
			this.humanDoor = blueprint.humanDoor;
			this.animalDoor = blueprint.animalDoor;
			this.nameOfIndoors = blueprint.mapToWarpTo;
			this.baseNameOfIndoors = this.nameOfIndoors;
			this.nameOfIndoorsWithoutUnique = this.baseNameOfIndoors;
			this.indoors = this.getIndoors();
			this.nameOfIndoors += this.tileX * 2000 + this.tileY;
			this.maxOccupants = blueprint.maxOccupants;
			this.daysOfConstructionLeft = 2;
			this.magical = blueprint.magical;
		}

		protected virtual GameLocation getIndoors()
		{
			if (this.buildingType.Equals("Slime Hutch"))
			{
				if (this.indoors != null)
				{
					this.nameOfIndoorsWithoutUnique = this.indoors.name;
				}
				string a = this.nameOfIndoorsWithoutUnique;
				if (a == "Slime Hutch")
				{
					this.nameOfIndoorsWithoutUnique = "SlimeHutch";
				}
				GameLocation gameLocation = new SlimeHutch(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
				gameLocation.IsFarm = true;
				gameLocation.isStructure = true;
				foreach (Warp expr_9B in gameLocation.warps)
				{
					expr_9B.TargetX = this.humanDoor.X + this.tileX;
					expr_9B.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				return gameLocation;
			}
			if (this.buildingType.Equals("Shed"))
			{
				if (this.indoors != null)
				{
					this.nameOfIndoorsWithoutUnique = this.indoors.name;
				}
				string a = this.nameOfIndoorsWithoutUnique;
				if (a == "Shed")
				{
					this.nameOfIndoorsWithoutUnique = "Shed";
				}
				GameLocation gameLocation2 = new Shed(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
				gameLocation2.IsFarm = true;
				gameLocation2.isStructure = true;
				foreach (Warp expr_182 in gameLocation2.warps)
				{
					expr_182.TargetX = this.humanDoor.X + this.tileX;
					expr_182.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				return gameLocation2;
			}
			if (this.nameOfIndoorsWithoutUnique != null && this.nameOfIndoorsWithoutUnique.Length > 0 && !this.nameOfIndoorsWithoutUnique.Equals("null"))
			{
				GameLocation gameLocation3 = new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
				gameLocation3.IsFarm = true;
				gameLocation3.isStructure = true;
				if (gameLocation3.name.Equals("Greenhouse"))
				{
					gameLocation3.terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();
				}
				foreach (Warp expr_270 in gameLocation3.warps)
				{
					expr_270.TargetX = this.humanDoor.X + this.tileX;
					expr_270.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				if (gameLocation3 is AnimalHouse)
				{
					AnimalHouse animalHouse = gameLocation3 as AnimalHouse;
					string a = this.buildingType.Split(new char[]
					{
						' '
					})[0];
					if (!(a == "Big"))
					{
						if (!(a == "Deluxe"))
						{
							animalHouse.animalLimit = 4;
						}
						else
						{
							animalHouse.animalLimit = 12;
						}
					}
					else
					{
						animalHouse.animalLimit = 8;
					}
				}
				return gameLocation3;
			}
			return null;
		}

		public virtual Rectangle getRectForAnimalDoor()
		{
			return new Rectangle((this.animalDoor.X + this.tileX) * Game1.tileSize, (this.tileY + this.animalDoor.Y) * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public virtual void load()
		{
			this.texture = Game1.content.Load<Texture2D>("Buildings\\" + this.buildingType);
			GameLocation gameLocation = this.getIndoors();
			if (gameLocation != null)
			{
				gameLocation.characters = this.indoors.characters;
				gameLocation.objects = this.indoors.objects;
				gameLocation.terrainFeatures = this.indoors.terrainFeatures;
				gameLocation.IsFarm = true;
				gameLocation.IsOutdoors = false;
				gameLocation.isStructure = true;
				gameLocation.uniqueName = gameLocation.name + (this.tileX * 2000 + this.tileY);
				gameLocation.numberOfSpawnedObjectsOnMap = this.indoors.numberOfSpawnedObjectsOnMap;
				if (this.indoors.GetType() == typeof(AnimalHouse))
				{
					((AnimalHouse)gameLocation).animals = ((AnimalHouse)this.indoors).animals;
					((AnimalHouse)gameLocation).animalsThatLiveHere = ((AnimalHouse)this.indoors).animalsThatLiveHere;
					foreach (KeyValuePair<long, FarmAnimal> current in ((AnimalHouse)gameLocation).animals)
					{
						current.Value.reload();
					}
				}
				if (this.indoors is Shed)
				{
					((Shed)gameLocation).furniture = ((Shed)this.indoors).furniture;
					using (List<Furniture>.Enumerator enumerator2 = ((Shed)gameLocation).furniture.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.Current.updateDrawPosition();
						}
					}
					((Shed)gameLocation).wallPaper = ((Shed)this.indoors).wallPaper;
					((Shed)gameLocation).floor = ((Shed)this.indoors).floor;
				}
				this.indoors = gameLocation;
				gameLocation = null;
				foreach (Warp expr_1FE in this.indoors.warps)
				{
					expr_1FE.TargetX = this.humanDoor.X + this.tileX;
					expr_1FE.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				if (this.indoors.IsFarm && this.indoors.terrainFeatures == null)
				{
					this.indoors.terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();
				}
				using (List<NPC>.Enumerator enumerator4 = this.indoors.characters.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						enumerator4.Current.reloadSprite();
					}
				}
				using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator5 = this.indoors.terrainFeatures.Values.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						enumerator5.Current.loadSprite();
					}
				}
				foreach (KeyValuePair<Vector2, StardewValley.Object> current2 in this.indoors.objects)
				{
					current2.Value.initializeLightSource(current2.Key);
					current2.Value.reloadSprite();
				}
				if (this.indoors is AnimalHouse)
				{
					AnimalHouse animalHouse = this.indoors as AnimalHouse;
					string a = this.buildingType.Split(new char[]
					{
						' '
					})[0];
					if (a == "Big")
					{
						animalHouse.animalLimit = 8;
						return;
					}
					if (a == "Deluxe")
					{
						animalHouse.animalLimit = 12;
						return;
					}
					animalHouse.animalLimit = 4;
				}
			}
		}

		public bool isUnderConstruction()
		{
			return this.daysOfConstructionLeft > 0;
		}

		public virtual bool isTilePassable(Vector2 tile)
		{
			return tile.X < (float)this.tileX || tile.X >= (float)(this.tileX + this.tilesWide) || tile.Y < (float)this.tileY || tile.Y >= (float)(this.tileY + this.tilesHigh);
		}

		public virtual bool intersects(Rectangle boundingBox)
		{
			return new Rectangle(this.tileX * Game1.tileSize, this.tileY * Game1.tileSize, this.tilesWide * Game1.tileSize, this.tilesHigh * Game1.tileSize).Intersects(boundingBox);
		}

		public virtual void drawInMenu(SpriteBatch b, int x, int y)
		{
			if (this.tilesWide <= 8)
			{
				this.drawShadow(b, x, y);
				b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(this.texture.Bounds), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
				return;
			}
			int num = Game1.tileSize + 11 * Game1.pixelZoom;
			int num2 = Game1.tileSize / 2 - Game1.pixelZoom;
			b.Draw(this.texture, new Vector2((float)(x + num), (float)(y + num2)), new Rectangle?(new Rectangle(this.texture.Bounds.Center.X - 64, this.texture.Bounds.Bottom - 136 - 2, 122, 138)), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
		}

		public virtual void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				this.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.texture.Bounds), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
			if (this.magical && this.buildingType.Equals("Gold Clock"))
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.hourHandSource), Color.White * this.alpha, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1200) / 1200f) + (double)((float)Game1.gameTimeInterval / 7000f / 23f)), new Vector2(2.5f, 8f), (float)(Game1.pixelZoom * 3) / 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.minuteHandSource), Color.White * this.alpha, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1000 % 100 % 60) / 60f) + (double)((float)Game1.gameTimeInterval / 7000f * 1.02f)), new Vector2(2.5f, 12f), (float)(Game1.pixelZoom * 3) / 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.00011f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.clockNub), Color.White * this.alpha, 0f, new Vector2(2f, 2f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.00012f);
			}
		}

		public virtual void drawShadow(SpriteBatch b, int localX = -1, int localY = -1)
		{
			Vector2 vector = (localX == -1) ? Game1.GlobalToLocal(new Vector2((float)(this.tileX * Game1.tileSize), (float)((this.tileY + this.tilesHigh) * Game1.tileSize))) : new Vector2((float)localX, (float)(localY + this.getSourceRectForMenu().Height * Game1.pixelZoom));
			b.Draw(Game1.mouseCursors, vector, new Rectangle?(Building.leftShadow), Color.White * ((localX == -1) ? this.alpha : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
			for (int i = 1; i < this.tilesWide - 1; i++)
			{
				b.Draw(Game1.mouseCursors, vector + new Vector2((float)(i * Game1.tileSize), 0f), new Rectangle?(Building.middleShadow), Color.White * ((localX == -1) ? this.alpha : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
			}
			b.Draw(Game1.mouseCursors, vector + new Vector2((float)((this.tilesWide - 1) * Game1.tileSize), 0f), new Rectangle?(Building.rightShadow), Color.White * ((localX == -1) ? this.alpha : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
		}

		public void drawInConstruction(SpriteBatch b)
		{
			int num = Math.Min(16, Math.Max(0, (int)(16f - (float)this.newConstructionTimer / 1000f * 16f)));
			float num2 = (float)(2000 - this.newConstructionTimer) / 2000f;
			if (this.magical)
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize) + (float)(this.texture.Bounds.Height * Game1.pixelZoom) * (1f - num2))), new Rectangle?(new Rectangle(0, (int)((float)this.texture.Bounds.Bottom - num2 * (float)this.texture.Bounds.Height), this.getSourceRectForMenu().Width, (int)((float)this.texture.Bounds.Height * num2))), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
				for (int i = 0; i < this.tilesWide * 4; i++)
				{
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + i * (Game1.tileSize / 4)), (float)(this.tileY * Game1.tileSize - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize) + (float)(this.texture.Bounds.Height * Game1.pixelZoom) * (1f - num2))) + new Vector2((float)Game1.random.Next(-1, 2), (float)(Game1.random.Next(-1, 2) - ((i % 2 == 0) ? (Game1.pixelZoom * 8) : (Game1.pixelZoom * 2)))), new Rectangle?(new Rectangle(536 + (this.newConstructionTimer + i * 4) % 56 / 8 * 8, 1945, 8, 8)), (i % 2 == 1) ? (Color.Pink * this.alpha) : (Color.LightPink * this.alpha), 0f, new Vector2(0f, 0f), 4f + (float)Game1.random.Next(100) / 100f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
					if (i % 2 == 0)
					{
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + i * (Game1.tileSize / 4)), (float)(this.tileY * Game1.tileSize - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize) + (float)(this.texture.Bounds.Height * Game1.pixelZoom) * (1f - num2))) + new Vector2((float)Game1.random.Next(-1, 2), (float)(Game1.random.Next(-1, 2) + ((i % 2 == 0) ? (Game1.pixelZoom * 8) : (Game1.pixelZoom * 2)))), new Rectangle?(new Rectangle(536 + (this.newConstructionTimer + i * 4) % 56 / 8 * 8, 1945, 8, 8)), Color.White * this.alpha, 0f, new Vector2(0f, 0f), 4f + (float)Game1.random.Next(100) / 100f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
					}
				}
				return;
			}
			bool flag = this.daysOfConstructionLeft == 1;
			for (int j = this.tileX; j < this.tileX + this.tilesWide; j++)
			{
				for (int k = this.tileY; k < this.tileY + this.tilesHigh; k++)
				{
					if (j == this.tileX + this.tilesWide / 2 && k == this.tileY + this.tilesHigh - 1)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4 - Game1.pixelZoom)), new Rectangle?(new Rectangle(367, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 309, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (j == this.tileX && k == this.tileY)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 293, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (j == this.tileX + this.tilesWide - 1 && k == this.tileY)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 293, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (j == this.tileX + this.tilesWide - 1 && k == this.tileY + this.tilesHigh - 1)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 325, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize) / 10000f);
					}
					else if (j == this.tileX && k == this.tileY + this.tilesHigh - 1)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 325, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize) / 10000f);
					}
					else if (j == this.tileX + this.tilesWide - 1)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 309, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize) / 10000f);
					}
					else if (k == this.tileY + this.tilesHigh - 1)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 325, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize) / 10000f);
					}
					else if (j == this.tileX)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 309, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize) / 10000f);
					}
					else if (k == this.tileY)
					{
						if (flag)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 293, 16, num)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(k * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (flag)
					{
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)j, (float)k) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - num * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
					}
				}
			}
		}
	}
}
