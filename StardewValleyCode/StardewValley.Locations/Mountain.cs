using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class Mountain : GameLocation
	{
		public const int daysBeforeLandslide = 31;

		private TemporaryAnimatedSprite minecartSteam;

		private bool bridgeRestored;

		private bool oreBoulderPresent;

		private bool railroadAreaBlocked = Game1.stats.DaysPlayed < 31u;

		private bool landslide = Game1.stats.DaysPlayed < 5u;

		private Microsoft.Xna.Framework.Rectangle landSlideRect = new Microsoft.Xna.Framework.Rectangle(50 * Game1.tileSize, 4 * Game1.tileSize, 3 * Game1.tileSize, 5 * Game1.tileSize);

		private Microsoft.Xna.Framework.Rectangle railroadBlockRect = new Microsoft.Xna.Framework.Rectangle(8 * Game1.tileSize, 0, 4 * Game1.tileSize, 5 * Game1.tileSize);

		private int oldTime;

		private Microsoft.Xna.Framework.Rectangle boulderSourceRect = new Microsoft.Xna.Framework.Rectangle(439, 1385, 39, 48);

		private Microsoft.Xna.Framework.Rectangle raildroadBlocksourceRect = new Microsoft.Xna.Framework.Rectangle(640, 2176, 64, 80);

		private Microsoft.Xna.Framework.Rectangle landSlideSourceRect = new Microsoft.Xna.Framework.Rectangle(646, 1218, 48, 80);

		private Vector2 boulderPosition = new Vector2(47f, 3f) * (float)Game1.tileSize - new Vector2(4f, 3f) * (float)Game1.pixelZoom;

		public Mountain()
		{
			if (Game1.stats.DaysPlayed >= 5u)
			{
				this.landslide = false;
			}
			if (Game1.stats.DaysPlayed >= 31u)
			{
				this.railroadAreaBlocked = false;
			}
		}

		public Mountain(Map map, string name) : base(map, name)
		{
			for (int i = 0; i < 10; i++)
			{
				this.quarryDayUpdate();
			}
			if (Game1.stats.DaysPlayed >= 5u)
			{
				this.landslide = false;
			}
			if (Game1.stats.DaysPlayed >= 31u)
			{
				this.railroadAreaBlocked = false;
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex <= 1080)
				{
					if (tileIndex != 958 && tileIndex != 1080)
					{
						goto IL_1E0;
					}
				}
				else if (tileIndex != 1081)
				{
					if (tileIndex == 1136 && !who.mailReceived.Contains("guildMember") && !who.hasQuest(16))
					{
						Game1.drawLetterMessage(Game1.content.LoadString("Strings\\Locations:Mountain_AdventurersGuildNote", new object[0]).Replace('\n', '^'));
						return true;
					}
					goto IL_1E0;
				}
				if (Game1.player.getMount() != null)
				{
					return true;
				}
				if (!Game1.player.mailReceived.Contains("ccBoilerRoom"))
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder", new object[0]));
					return true;
				}
				if (Game1.player.isRidingHorse() && Game1.player.getMount() != null)
				{
					Game1.player.getMount().checkAction(Game1.player, this);
				}
				else
				{
					Response[] answerChoices = new Response[]
					{
						new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
						new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
						new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town", new object[0])),
						new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
					};
					base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination", new object[0]), answerChoices, "Minecart");
				}
			}
			IL_1E0:
			return base.checkAction(tileLocation, viewport, who);
		}

		private void restoreBridge()
		{
			LocalizedContentManager localizedContentManager = Game1.content.CreateTemporary();
			Map map = localizedContentManager.Load<Map>("Maps\\Mountain-BridgeFixed");
			int num = 92;
			int num2 = 24;
			for (int i = 0; i < map.GetLayer("Back").LayerWidth; i++)
			{
				for (int j = 0; j < map.GetLayer("Back").LayerHeight; j++)
				{
					this.map.GetLayer("Back").Tiles[i + num, j + num2] = ((map.GetLayer("Back").Tiles[i, j] == null) ? null : new StaticTile(this.map.GetLayer("Back"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Back").Tiles[i, j].TileIndex));
					this.map.GetLayer("Buildings").Tiles[i + num, j + num2] = ((map.GetLayer("Buildings").Tiles[i, j] == null) ? null : new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Buildings").Tiles[i, j].TileIndex));
					this.map.GetLayer("Front").Tiles[i + num, j + num2] = ((map.GetLayer("Front").Tiles[i, j] == null) ? null : new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Front").Tiles[i, j].TileIndex));
				}
			}
			this.bridgeRestored = true;
			localizedContentManager.Unload();
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
			{
				this.minecartSteam = new TemporaryAnimatedSprite(27, new Vector2((float)(126 * Game1.tileSize + Game1.pixelZoom * 2), (float)(11 * Game1.tileSize) - (float)Game1.tileSize * 3f / 4f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					totalNumberOfLoops = 999999,
					interval = 60f,
					flipped = true
				};
			}
			if (!this.bridgeRestored && Game1.player.mailReceived.Contains("ccCraftsRoom"))
			{
				this.restoreBridge();
			}
			this.oreBoulderPresent = (!Game1.player.mailReceived.Contains("ccFishTank") || Game1.farmEvent != null);
			this.boulderSourceRect = new Microsoft.Xna.Framework.Rectangle(439 + (Game1.currentSeason.Equals("winter") ? 39 : 0), 1385, 39, 48);
			if (!this.objects.ContainsKey(new Vector2(29f, 9f)))
			{
				Vector2 vector = new Vector2(29f, 9f);
				this.objects.Add(vector, new Torch(vector, 146, true)
				{
					isOn = false,
					fragility = 2
				});
				this.objects[vector].checkForAction(null, false);
			}
			if (Game1.IsSpring)
			{
				this.raildroadBlocksourceRect = new Microsoft.Xna.Framework.Rectangle(640, 2176, 64, 80);
			}
			else
			{
				this.raildroadBlocksourceRect = new Microsoft.Xna.Framework.Rectangle(640, 1453, 64, 80);
			}
			base.addFrog();
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			this.quarryDayUpdate();
			if (Game1.stats.DaysPlayed >= 31u)
			{
				this.railroadAreaBlocked = false;
			}
			if (Game1.stats.DaysPlayed >= 5u)
			{
				this.landslide = false;
				if (!Game1.player.hasOrWillReceiveMail("landslideDone"))
				{
					Game1.mailbox.Enqueue("landslideDone");
				}
			}
		}

		private void quarryDayUpdate()
		{
			Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(106, 13, 21, 21);
			int num = 5;
			for (int i = 0; i < num; i++)
			{
				Vector2 randomPositionInThisRectangle = Utility.getRandomPositionInThisRectangle(r, Game1.random);
				if (this.isTileOpenForQuarryStone((int)randomPositionInThisRectangle.X, (int)randomPositionInThisRectangle.Y))
				{
					if (Game1.random.NextDouble() < 0.06)
					{
						if (this.isTileOpenForQuarryStone((int)randomPositionInThisRectangle.X + 1, (int)randomPositionInThisRectangle.Y) && this.isTileOpenForQuarryStone((int)randomPositionInThisRectangle.Y, (int)randomPositionInThisRectangle.Y + 1) && this.isTileOpenForQuarryStone((int)randomPositionInThisRectangle.X + 1, (int)randomPositionInThisRectangle.Y + 1))
						{
						}
					}
					else if (Game1.random.NextDouble() < 0.02)
					{
						if (Game1.random.NextDouble() < 0.1)
						{
							this.objects.Add(randomPositionInThisRectangle, new StardewValley.Object(randomPositionInThisRectangle, 46, "Stone", true, false, false, false)
							{
								minutesUntilReady = 12
							});
						}
						else
						{
							this.objects.Add(randomPositionInThisRectangle, new StardewValley.Object(randomPositionInThisRectangle, (Game1.random.Next(7) + 1) * 2, "Stone", true, false, false, false)
							{
								minutesUntilReady = 5
							});
						}
					}
					else if (Game1.random.NextDouble() < 0.1)
					{
						if (Game1.random.NextDouble() < 0.001)
						{
							this.objects.Add(randomPositionInThisRectangle, new StardewValley.Object(randomPositionInThisRectangle, 765, 1)
							{
								minutesUntilReady = 16
							});
						}
						else if (Game1.random.NextDouble() < 0.1)
						{
							this.objects.Add(randomPositionInThisRectangle, new StardewValley.Object(randomPositionInThisRectangle, 764, 1)
							{
								minutesUntilReady = 8
							});
						}
						else if (Game1.random.NextDouble() < 0.33)
						{
							this.objects.Add(randomPositionInThisRectangle, new StardewValley.Object(randomPositionInThisRectangle, 290, 1)
							{
								minutesUntilReady = 5
							});
						}
						else
						{
							this.objects.Add(randomPositionInThisRectangle, new StardewValley.Object(randomPositionInThisRectangle, 751, 1)
							{
								minutesUntilReady = 3
							});
						}
					}
					else
					{
						this.objects.Add(randomPositionInThisRectangle, new StardewValley.Object(randomPositionInThisRectangle, (Game1.random.NextDouble() < 0.25) ? 32 : ((Game1.random.NextDouble() < 0.33) ? 38 : ((Game1.random.NextDouble() < 0.5) ? 40 : 42)), 1)
						{
							minutesUntilReady = 2,
							name = "Stone"
						});
					}
				}
			}
		}

		private bool isTileOpenForQuarryStone(int tileX, int tileY)
		{
			return base.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null && this.isTileLocationTotallyClearAndPlaceable(new Vector2((float)tileX, (float)tileY));
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.minecartSteam = null;
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.update(time);
			}
			if (this.landslide && (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds - 400.0) / 1600.0) % 2 != 0 && Utility.isOnScreen(new Point(this.landSlideRect.X / Game1.tileSize, this.landSlideRect.Y / Game1.tileSize), Game1.tileSize * 2, null))
			{
				if (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 < (double)(this.oldTime % 400))
				{
					Game1.playSound("hammer");
				}
				this.oldTime = (int)time.TotalGameTime.TotalMilliseconds;
			}
		}

		public override StardewValley.Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.currentSeason.Equals("spring") && Game1.isRaining && who.FishingLevel >= 10 && !who.fishCaught.ContainsKey(163) && waterDepth >= 4 && Game1.random.NextDouble() < 0.1)
			{
				return new StardewValley.Object(163, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			return (this.landslide && position.Intersects(this.landSlideRect)) || (this.railroadAreaBlocked && position.Intersects(this.railroadBlockRect)) || base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.draw(spriteBatch, false, 0, 0);
			}
			if (this.oreBoulderPresent)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.boulderPosition), new Microsoft.Xna.Framework.Rectangle?(this.boulderSourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
			}
			if (this.railroadAreaBlocked)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.railroadBlockRect), new Microsoft.Xna.Framework.Rectangle?(this.raildroadBlocksourceRect), Color.White, 0f, Vector2.Zero, SpriteEffects.None, (float)(3 * Game1.tileSize) / 10000f + 0.0001f);
			}
			if (this.landslide)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.landSlideRect), new Microsoft.Xna.Framework.Rectangle?(this.landSlideSourceRect), Color.White, 0f, Vector2.Zero, SpriteEffects.None, (float)(3 * Game1.tileSize) / 10000f);
				spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(new Vector2((float)(this.landSlideRect.X + Game1.tileSize * 3 - Game1.pixelZoom * 5), (float)(this.landSlideRect.Y + Game1.tileSize * 3 + Game1.pixelZoom * 5)) + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, 3.5f * (float)Game1.tileSize / 10000f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(this.landSlideRect.X + Game1.tileSize * 3 - Game1.pixelZoom * 5), (float)(this.landSlideRect.Y + Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(288 + (((int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 1600.0 % 2.0) == 0) ? 0 : ((int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 / 100.0) * 19)), 1349, 19, 28)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(4 * Game1.tileSize) / 10000f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(this.landSlideRect.X + Game1.tileSize * 4 - Game1.pixelZoom * 5), (float)(this.landSlideRect.Y + Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(335, 1410, 21, 21)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(2 * Game1.tileSize) / 10000f);
			}
		}
	}
}
