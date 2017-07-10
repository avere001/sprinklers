using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class Woods : GameLocation
	{
		public const int numBaubles = 25;

		private List<Vector2> baubles;

		private List<WeatherDebris> weatherDebris;

		public List<ResourceClump> stumps = new List<ResourceClump>();

		public bool hasUnlockedStatue;

		public bool hasFoundStardrop;

		private bool addedSlimesToday;

		private int statueTimer;

		public Woods()
		{
		}

		public Woods(Map map, string name) : base(map, name)
		{
			this.isOutdoors = true;
			this.ignoreDebrisWeather = true;
			this.ignoreOutdoorLighting = true;
		}

		public override void checkForMusic(GameTime time)
		{
			if ((Game1.currentSong == null || !Game1.currentSong.IsPlaying) && (Game1.nextMusicTrack == null || Game1.nextMusicTrack.Length == 0))
			{
				if (Game1.isRaining)
				{
					Game1.changeMusicTrack("rain");
					return;
				}
				Game1.changeMusicTrack(Game1.currentSeason + "_day_ambient");
			}
		}

		public void statueAnimation(Farmer who)
		{
			this.hasUnlockedStatue = true;
			who.reduceActiveItemByOne();
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 7f) * (float)Game1.tileSize, Color.White, 9, false, 50f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 7f) * (float)Game1.tileSize, Color.Orange, 9, false, 70f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 6f) * (float)Game1.tileSize, Color.White, 9, false, 60f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 6f) * (float)Game1.tileSize, Color.OrangeRed, 9, false, 120f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 5f) * (float)Game1.tileSize, Color.Red, 9, false, 100f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 5f) * (float)Game1.tileSize, Color.White, 9, false, 170f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.Orange, 9, false, 40f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(9 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.White, 9, false, 90f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize + Game1.tileSize / 4)), Color.OrangeRed, 9, false, 190f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(9 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize + Game1.tileSize / 4)), Color.White, 9, false, 80f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(5 * Game1.tileSize + Game1.tileSize / 4)), Color.Red, 9, false, 69f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(9 * Game1.tileSize + Game1.tileSize / 2), (float)(5 * Game1.tileSize + Game1.tileSize / 4)), Color.OrangeRed, 9, false, 130f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2((float)(7 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.Orange, 9, false, 40f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(10 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), Color.White, 9, false, 90f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2((float)(7 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.Red, 9, false, 30f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(10 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), Color.White, 9, false, 180f, 0, -1, -1f, -1, 0));
			Game1.playSound("secret1");
			this.map.GetLayer("Front").Tiles[8, 6].TileIndex += 2;
			this.map.GetLayer("Front").Tiles[9, 6].TileIndex += 2;
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tile != null && who.IsMainPlayer)
			{
				int tileIndex = tile.TileIndex;
				if (tileIndex == 1140 || tileIndex == 1141)
				{
					if (!this.hasUnlockedStatue)
					{
						if (who.ActiveObject != null && who.ActiveObject.ParentSheetIndex == 417)
						{
							this.statueTimer = 1000;
							who.freezePause = 1000;
							who.FarmerSprite.ignoreDefaultActionThisTime = true;
							Game1.changeMusicTrack("none");
							Game1.playSound("newArtifact");
						}
						else
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Woods_Statue", new object[0]).Replace('\n', '^'));
						}
					}
					if (this.hasUnlockedStatue && !this.hasFoundStardrop && who.freeSpotsInInventory() > 0)
					{
						who.addItemByMenuIfNecessaryElseHoldUp(new StardewValley.Object(434, 1, false, -1, 0), null);
						this.hasFoundStardrop = true;
						if (!Game1.player.mailReceived.Contains("CF_Statue"))
						{
							Game1.player.mailReceived.Add("CF_Statue");
						}
					}
					return true;
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			foreach (ResourceClump expr_15 in this.stumps)
			{
				if (expr_15.getBoundingBox(expr_15.tile).Intersects(position))
				{
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			if (t is Axe)
			{
				Point value = new Point(tileX * Game1.tileSize + Game1.tileSize / 2, tileY * Game1.tileSize + Game1.tileSize / 2);
				for (int i = this.stumps.Count - 1; i >= 0; i--)
				{
					if (this.stumps[i].getBoundingBox(this.stumps[i].tile).Contains(value))
					{
						if (this.stumps[i].performToolAction(t, 1, this.stumps[i].tile, null))
						{
							this.stumps.RemoveAt(i);
						}
						return true;
					}
				}
			}
			return false;
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			this.characters.Clear();
			this.addedSlimesToday = false;
			PropertyValue propertyValue;
			this.map.Properties.TryGetValue("Stumps", out propertyValue);
			if (propertyValue != null)
			{
				string[] array = propertyValue.ToString().Split(new char[]
				{
					' '
				});
				for (int i = 0; i < array.Length; i += 3)
				{
					int num = Convert.ToInt32(array[i]);
					int num2 = Convert.ToInt32(array[i + 1]);
					Vector2 vector = new Vector2((float)num, (float)num2);
					bool flag = false;
					using (List<ResourceClump>.Enumerator enumerator = this.stumps.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.tile.Equals(vector))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						this.stumps.Add(new ResourceClump(600, 2, 2, vector));
						base.removeObject(vector, false);
						base.removeObject(vector + new Vector2(1f, 0f), false);
						base.removeObject(vector + new Vector2(1f, 1f), false);
						base.removeObject(vector + new Vector2(0f, 1f), false);
					}
				}
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.changeMusicTrack("none");
			if (this.baubles != null)
			{
				this.baubles.Clear();
			}
			if (this.weatherDebris != null)
			{
				this.weatherDebris.Clear();
			}
		}

		public override bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
		{
			using (List<ResourceClump>.Enumerator enumerator = this.stumps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.occupiesTile((int)v.X, (int)v.Y))
					{
						return false;
					}
				}
			}
			return base.isTileLocationTotallyClearAndPlaceable(v);
		}

		public override void resetForPlayerEntry()
		{
			if (!Game1.player.mailReceived.Contains("beenToWoods"))
			{
				Game1.player.mailReceived.Add("beenToWoods");
			}
			if (!this.addedSlimesToday)
			{
				this.addedSlimesToday = true;
				Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame) + 12u));
				for (int i = 50; i > 0; i--)
				{
					Vector2 randomTile = base.getRandomTile();
					if (random.NextDouble() < 0.25 && this.isTileLocationTotallyClearAndPlaceable(randomTile))
					{
						string currentSeason = Game1.currentSeason;
						if (!(currentSeason == "spring"))
						{
							if (!(currentSeason == "summer"))
							{
								if (!(currentSeason == "fall"))
								{
									if (currentSeason == "winter")
									{
										this.characters.Add(new GreenSlime(randomTile * (float)Game1.tileSize, 40));
									}
								}
								else
								{
									this.characters.Add(new GreenSlime(randomTile * (float)Game1.tileSize, (random.NextDouble() < 0.5) ? 0 : 40));
								}
							}
							else
							{
								this.characters.Add(new GreenSlime(randomTile * (float)Game1.tileSize, 0));
							}
						}
						else
						{
							this.characters.Add(new GreenSlime(randomTile * (float)Game1.tileSize, 0));
						}
					}
				}
			}
			if (Game1.timeOfDay > 1600)
			{
				this.ignoreOutdoorLighting = false;
				this.ignoreLights = true;
			}
			else
			{
				this.ignoreOutdoorLighting = true;
				this.ignoreLights = false;
			}
			base.resetForPlayerEntry();
			Random random2 = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			int num = 25 + random2.Next(0, 75);
			if (!Game1.isRaining)
			{
				this.baubles = new List<Vector2>();
				for (int j = 0; j < num; j++)
				{
					this.baubles.Add(new Vector2((float)Game1.random.Next(0, this.map.DisplayWidth), (float)Game1.random.Next(0, this.map.DisplayHeight)));
				}
				if (!Game1.currentSeason.Equals("winter"))
				{
					this.weatherDebris = new List<WeatherDebris>();
					int num2 = Game1.tileSize * 3;
					for (int k = 0; k < num; k++)
					{
						this.weatherDebris.Add(new WeatherDebris(new Vector2((float)(k * num2 % Game1.graphics.GraphicsDevice.Viewport.Width + Game1.random.Next(num2)), (float)(k * num2 / Game1.graphics.GraphicsDevice.Viewport.Width * num2 + Game1.random.Next(num2))), 1, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
					}
				}
			}
			if (Game1.timeOfDay < 1800)
			{
				Game1.changeMusicTrack("woodsTheme");
			}
			if (this.hasUnlockedStatue && !this.hasFoundStardrop)
			{
				this.map.GetLayer("Front").Tiles[8, 6].TileIndex += 2;
				this.map.GetLayer("Front").Tiles[9, 6].TileIndex += 2;
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.statueTimer > 0)
			{
				this.statueTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.statueTimer <= 0)
				{
					this.statueAnimation(Game1.player);
				}
			}
			if (this.baubles != null)
			{
				for (int i = 0; i < this.baubles.Count; i++)
				{
					Vector2 vector = default(Vector2);
					vector.X = this.baubles[i].X - Math.Max(0.4f, Math.Min(1f, (float)i * 0.01f)) - (float)((double)((float)i * 0.01f) * Math.Sin(6.2831853071795862 * (double)time.TotalGameTime.Milliseconds / 8000.0));
					vector.Y = this.baubles[i].Y + Math.Max(0.5f, Math.Min(1.2f, (float)i * 0.02f));
					if (vector.Y > (float)this.map.DisplayHeight || vector.X < 0f)
					{
						vector.X = (float)Game1.random.Next(0, this.map.DisplayWidth);
						vector.Y = (float)(-(float)Game1.tileSize);
					}
					this.baubles[i] = vector;
				}
			}
			if (this.weatherDebris != null)
			{
				using (List<WeatherDebris>.Enumerator enumerator = this.weatherDebris.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.update();
					}
				}
				Game1.updateDebrisWeatherForMovement(this.weatherDebris);
			}
			foreach (ResourceClump current in this.stumps)
			{
				current.tickUpdate(time, current.tile);
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (!Game1.eventUp || (this.currentEvent != null && this.currentEvent.showGroundObjects))
			{
				foreach (ResourceClump current in this.stumps)
				{
					current.draw(b, current.tile);
				}
			}
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			if (this.baubles != null)
			{
				for (int i = 0; i < this.baubles.Count; i++)
				{
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.baubles[i]), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(346 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(i * 25)) % 600.0) / 150 * 5, 1971, 5, 5)), Color.White, (float)i * 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
			}
			if (this.weatherDebris != null && this.currentEvent == null)
			{
				using (List<WeatherDebris>.Enumerator enumerator = this.weatherDebris.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
			}
		}
	}
}
