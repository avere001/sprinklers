using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class Forest : GameLocation
	{
		[XmlIgnore]
		public List<FarmAnimal> marniesLivestock;

		[XmlIgnore]
		public List<Microsoft.Xna.Framework.Rectangle> travelingMerchantBounds;

		[XmlIgnore]
		public Dictionary<Item, int[]> travelingMerchantStock;

		[XmlIgnore]
		public bool travelingMerchantDay;

		public ResourceClump log;

		private int chimneyTimer = 500;

		private Microsoft.Xna.Framework.Rectangle hatterSource = new Microsoft.Xna.Framework.Rectangle(600, 1957, 64, 32);

		private Vector2 hatterPos = new Vector2((float)(32 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(94 * Game1.tileSize));

		public Forest()
		{
		}

		public Forest(Map map, string name) : base(map, name)
		{
			this.marniesLivestock = new List<FarmAnimal>();
			this.marniesLivestock.Add(new FarmAnimal("Dairy Cow", MultiplayerUtility.getNewID(), -1L));
			this.marniesLivestock.Add(new FarmAnimal("Dairy Cow", MultiplayerUtility.getNewID(), -1L));
			this.marniesLivestock[0].position = new Vector2((float)(98 * Game1.tileSize), (float)(20 * Game1.tileSize));
			this.marniesLivestock[1].position = new Vector2((float)(101 * Game1.tileSize), (float)(20 * Game1.tileSize));
			this.log = new ResourceClump(602, 2, 2, new Vector2(1f, 6f));
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			base.addFrog();
		}

		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			if (this.log != null && this.log.getBoundingBox(this.log.tile).Contains(tileX * Game1.tileSize, tileY * Game1.tileSize))
			{
				if (this.log.performToolAction(t, 1, this.log.tile, null))
				{
					this.log = null;
				}
				return true;
			}
			return base.performToolAction(t, tileX, tileY);
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int num = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (num != 901)
			{
				if (num != 1394)
				{
					if (num == 1972)
					{
						if (who.achievements.Count > 0)
						{
							Game1.activeClickableMenu = new ShopMenu(Utility.getHatStock(), 0, "HatMouse");
						}
						else
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Forest_HatMouseStore_Abandoned", new object[0]));
						}
					}
				}
				else if (who.hasRustyKey && !who.mailReceived.Contains("OpenedSewer"))
				{
					Game1.playSound("openBox");
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Forest_OpenedSewer", new object[0])));
					who.mailReceived.Add("OpenedSewer");
				}
				else if (who.mailReceived.Contains("OpenedSewer"))
				{
					Game1.warpFarmer("Sewer", 3, 48, 0);
					Game1.playSound("openChest");
				}
				else
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor", new object[0]));
				}
			}
			else if (!who.mailReceived.Contains("wizardJunimoNote") && !who.mailReceived.Contains("JojaMember"))
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Forest_WizardTower_Locked", new object[0]));
				return false;
			}
			if (this.travelingMerchantDay && Game1.timeOfDay < 2000)
			{
				if (tileLocation.X == 27 && tileLocation.Y == 11 && this.travelingMerchantStock != null)
				{
					Game1.activeClickableMenu = new ShopMenu(this.travelingMerchantStock, 0, "Traveler");
				}
				else if (tileLocation.X == 23 && tileLocation.Y == 11)
				{
					Game1.playSound("pig");
				}
			}
			Microsoft.Xna.Framework.Rectangle value = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (this.log != null && this.log.getBoundingBox(this.log.tile).Intersects(value))
			{
				this.log.performUseAction(new Vector2((float)tileLocation.X, (float)tileLocation.Y));
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			if (this.log != null && this.log.getBoundingBox(this.log.tile).Intersects(position))
			{
				return true;
			}
			if (this.travelingMerchantBounds != null)
			{
				foreach (Microsoft.Xna.Framework.Rectangle current in this.travelingMerchantBounds)
				{
					if (position.Intersects(current))
					{
						return true;
					}
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			if (dayOfMonth % 7 % 5 == 0)
			{
				this.travelingMerchantDay = true;
				this.travelingMerchantBounds = new List<Microsoft.Xna.Framework.Rectangle>();
				this.travelingMerchantBounds.Add(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize, 10 * Game1.tileSize, 123 * Game1.pixelZoom, 28 * Game1.pixelZoom));
				this.travelingMerchantBounds.Add(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize + 45 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 19 * Game1.pixelZoom, 12 * Game1.pixelZoom));
				this.travelingMerchantBounds.Add(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize + 85 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 26 * Game1.pixelZoom, 12 * Game1.pixelZoom));
				this.travelingMerchantStock = Utility.getTravelingMerchantStock();
				using (List<Microsoft.Xna.Framework.Rectangle>.Enumerator enumerator = this.travelingMerchantBounds.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Utility.clearObjectsInArea(enumerator.Current, this);
					}
					goto IL_12B;
				}
			}
			this.travelingMerchantBounds = null;
			this.travelingMerchantDay = false;
			this.travelingMerchantStock = null;
			IL_12B:
			if (Game1.currentSeason.Equals("spring"))
			{
				for (int i = 0; i < 7; i++)
				{
					Vector2 vector = new Vector2((float)Game1.random.Next(70, this.map.Layers[0].LayerWidth - 10), (float)Game1.random.Next(68, this.map.Layers[0].LayerHeight - 15));
					if (vector.Y > 30f)
					{
						foreach (Vector2 current in Utility.recursiveFindOpenTiles(this, vector, 16, 50))
						{
							string text = base.doesTileHaveProperty((int)current.X, (int)current.Y, "Diggable", "Back");
							if (!this.terrainFeatures.ContainsKey(current) && text != null && Game1.random.NextDouble() < (double)(1f - Vector2.Distance(vector, current) * 0.15f))
							{
								this.terrainFeatures.Add(current, new HoeDirt(0, new Crop(true, 1, (int)current.X, (int)current.Y)));
							}
						}
					}
				}
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			using (List<FarmAnimal>.Enumerator enumerator = this.marniesLivestock.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.updateWhenCurrentLocation(time, this);
				}
			}
			if (this.log != null)
			{
				this.log.tickUpdate(time, this.log.tile);
			}
			if (Game1.timeOfDay < 2000)
			{
				if (this.travelingMerchantDay)
				{
					if (Game1.random.NextDouble() < 0.001)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(99, 1423, 13, 19), new Vector2((float)(23 * Game1.tileSize), (float)(10 * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom)), false, 0f, Color.White)
						{
							interval = (float)Game1.random.Next(500, 1500),
							layerDepth = (float)(12 * Game1.tileSize) / 10000f + 2E-05f,
							scale = (float)Game1.pixelZoom
						});
					}
					if (Game1.random.NextDouble() < 0.001)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(51, 1444, 5, 5), new Vector2((float)(23 * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom), (float)(11 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom * 2)), false, 0f, Color.White)
						{
							interval = 500f,
							animationLength = 1,
							layerDepth = (float)(12 * Game1.tileSize) / 10000f + 2E-05f,
							scale = (float)Game1.pixelZoom
						});
					}
					if (Game1.random.NextDouble() < 0.003)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(89, 1445, 6, 3), new Vector2((float)(27 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float)(10 * Game1.tileSize + 6 * Game1.pixelZoom)), false, 0f, Color.White)
						{
							interval = 50f,
							animationLength = 3,
							pingPong = true,
							totalNumberOfLoops = 1,
							layerDepth = (float)(12 * Game1.tileSize) / 10000f + 2E-05f,
							scale = (float)Game1.pixelZoom
						});
					}
				}
				this.chimneyTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.chimneyTimer <= 0)
				{
					this.chimneyTimer = (this.travelingMerchantDay ? 500 : Game1.random.Next(200, 2000));
					Vector2 position = this.travelingMerchantDay ? new Vector2((float)(29 * Game1.tileSize + Game1.pixelZoom * 3), (float)(8 * Game1.tileSize + Game1.pixelZoom * 3)) : new Vector2((float)(87 * Game1.tileSize + Game1.pixelZoom * 6), (float)(9 * Game1.tileSize + Game1.pixelZoom * 8));
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), position, false, 0.002f, Color.Gray)
					{
						alpha = 0.75f,
						motion = new Vector2(0f, -0.5f),
						acceleration = new Vector2(0.002f, 0f),
						interval = 99999f,
						layerDepth = 1f,
						scale = (float)(Game1.pixelZoom * 3) / 4f,
						scaleChange = 0.01f,
						rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f
					});
					if (this.travelingMerchantDay)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(225, 1388, 7, 5), new Vector2((float)(29 * Game1.tileSize + Game1.pixelZoom * 3), (float)(8 * Game1.tileSize + Game1.pixelZoom * 6)), false, 0f, Color.White)
						{
							interval = (float)(this.chimneyTimer - this.chimneyTimer / 5),
							animationLength = 1,
							layerDepth = 0.99f,
							scale = (float)Game1.pixelZoom + 0.3f,
							scaleChange = -0.015f
						});
					}
				}
			}
		}

		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			if (this.travelingMerchantDay && Game1.random.NextDouble() < 0.4)
			{
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(57, 1430, 4, 12), new Vector2((float)(28 * Game1.tileSize), (float)(10 * Game1.tileSize + 4 * Game1.pixelZoom)), false, 0f, Color.White)
				{
					interval = 50f,
					animationLength = 10,
					pingPong = true,
					totalNumberOfLoops = 1,
					layerDepth = (float)(12 * Game1.tileSize) / 10000f + 2E-05f,
					scale = (float)Game1.pixelZoom
				});
				if (Game1.random.NextDouble() < 0.66)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(89, 1445, 6, 3), new Vector2((float)(27 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float)(10 * Game1.tileSize + 6 * Game1.pixelZoom)), false, 0f, Color.White)
					{
						interval = 50f,
						animationLength = 3,
						pingPong = true,
						totalNumberOfLoops = 1,
						layerDepth = (float)(12 * Game1.tileSize) / 10000f + 3E-05f,
						scale = (float)Game1.pixelZoom
					});
				}
			}
		}

		public override int getFishingLocation(Vector2 tile)
		{
			if (tile.X < 53f && tile.Y < 43f)
			{
				return 1;
			}
			return 0;
		}

		public override StardewValley.Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.currentSeason.Equals("winter") && who.getTileX() == 58 && who.getTileY() == 87 && who.FishingLevel >= 6 && !who.fishCaught.ContainsKey(775) && waterDepth >= 3 && Game1.random.NextDouble() < 0.5)
			{
				return new StardewValley.Object(775, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			using (List<FarmAnimal>.Enumerator enumerator = this.marniesLivestock.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(spriteBatch);
				}
			}
			if (this.log != null)
			{
				this.log.draw(spriteBatch, this.log.tile);
			}
			if (this.travelingMerchantDay)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(24 * Game1.tileSize), (float)(8 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(142, 1382, 109, 70)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(12 * Game1.tileSize) / 10000f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(23 * Game1.tileSize), (float)(10 * Game1.tileSize + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(112, 1424, 30, 24)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(12 * Game1.tileSize) / 10000f + 1E-05f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(24 * Game1.tileSize), (float)(11 * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(142, 1424, 16, 3)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(12 * Game1.tileSize) / 10000f + 2E-05f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(24 * Game1.tileSize + Game1.pixelZoom * 2), (float)(10 * Game1.tileSize - Game1.tileSize / 2 - Game1.pixelZoom * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(71, 1966, 18, 18)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(12 * Game1.tileSize) / 10000f - 2E-05f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(23 * Game1.tileSize), (float)(10 * Game1.tileSize - Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(167, 1966, 18, 18)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(12 * Game1.tileSize) / 10000f - 2E-05f);
				if (Game1.timeOfDay >= 2000)
				{
					spriteBatch.Draw(Game1.staminaRect, Game1.GlobalToLocal(Game1.viewport, new Microsoft.Xna.Framework.Rectangle(27 * Game1.tileSize + Game1.tileSize / 4, 10 * Game1.tileSize, Game1.tileSize, Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, (float)(12 * Game1.tileSize) / 10000f + 4E-05f);
				}
			}
			if (Game1.player.achievements.Count > 0)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(this.hatterPos), new Microsoft.Xna.Framework.Rectangle?(this.hatterSource), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(94 * Game1.tileSize) / 10000f);
			}
		}
	}
}
