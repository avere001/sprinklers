using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class Sewer : GameLocation
	{
		private NPC Krobus = new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Krobus"), 0, Game1.tileSize / 4, 24), new Vector2(31f, 17f) * (float)Game1.tileSize, "Sewer", 2, "Krobus", false, null, Game1.content.Load<Texture2D>("Portraits\\Krobus"));

		private Dictionary<Item, int[]> dailyShadowStock = new Dictionary<Item, int[]>();

		public const float steamZoom = 4f;

		public const float steamYMotionPerMillisecond = 0.1f;

		public const float millisecondsPerSteamFrame = 50f;

		private Texture2D steamAnimation;

		private Vector2 steamPosition;

		private Color steamColor = new Color(200, 255, 200);

		public Sewer()
		{
		}

		public Sewer(Map map, string name) : base(map, name)
		{
			this.waterColor = Color.LimeGreen;
		}

		public Dictionary<Item, int[]> getShadowShopStock()
		{
			return this.dailyShadowStock;
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			this.populateShopStock(dayOfMonth);
		}

		public void populateShopStock(int dayOfMonth)
		{
			this.dailyShadowStock.Clear();
			this.dailyShadowStock.Add(new StardewValley.Object(769, 1, false, -1, 0), new int[]
			{
				100,
				10
			});
			this.dailyShadowStock.Add(new StardewValley.Object(768, 1, false, -1, 0), new int[]
			{
				80,
				10
			});
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			switch (dayOfMonth % 7)
			{
			case 0:
				this.dailyShadowStock.Add(new StardewValley.Object(767, 1, false, -1, 0), new int[]
				{
					30,
					10
				});
				break;
			case 1:
				this.dailyShadowStock.Add(new StardewValley.Object(766, 1, false, -1, 0), new int[]
				{
					10,
					50
				});
				break;
			case 2:
				this.dailyShadowStock.Add(new StardewValley.Object(749, 1, false, -1, 0), new int[]
				{
					300,
					1
				});
				break;
			case 3:
				this.dailyShadowStock.Add(new StardewValley.Object(random.Next(698, 709), 1, false, -1, 0), new int[]
				{
					200,
					5
				});
				break;
			case 4:
				this.dailyShadowStock.Add(new StardewValley.Object(770, 1, false, -1, 0), new int[]
				{
					30,
					10
				});
				break;
			case 5:
				this.dailyShadowStock.Add(new StardewValley.Object(645, 1, false, -1, 0), new int[]
				{
					10000,
					1
				});
				break;
			case 6:
			{
				int num = random.Next(194, 245);
				if (num == 217)
				{
					num = 216;
				}
				this.dailyShadowStock.Add(new StardewValley.Object(num, 1, false, -1, 0), new int[]
				{
					random.Next(5, 51) * 10,
					5
				});
				break;
			}
			}
			this.dailyShadowStock.Add(new StardewValley.Object(305, 1, false, -1, 0), new int[]
			{
				5000,
				2147483647
			});
			if (!Game1.player.hasOrWillReceiveMail("CF_Sewer"))
			{
				this.dailyShadowStock.Add(new StardewValley.Object(434, 1, false, -1, 0), new int[]
				{
					20000,
					1
				});
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Crystal Floor"))
			{
				this.dailyShadowStock.Add(new StardewValley.Object(333, 1, true, -1, 0), new int[]
				{
					500,
					1
				});
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Wicked Statue"))
			{
				this.dailyShadowStock.Add(new StardewValley.Object(Vector2.Zero, 83, true), new int[]
				{
					1000,
					1
				});
			}
			if (!Game1.player.hasOrWillReceiveMail("ReturnScepter"))
			{
				this.dailyShadowStock.Add(new Wand(), new int[]
				{
					2000000,
					1
				});
			}
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			for (float num = -512f + this.steamPosition.X; num < (float)Game1.graphics.GraphicsDevice.Viewport.Width + 256f; num += 256f)
			{
				for (float num2 = -256f + this.steamPosition.Y; num2 < (float)(Game1.graphics.GraphicsDevice.Viewport.Height + 128); num2 += 256f)
				{
					b.Draw(this.steamAnimation, new Vector2(num, num2), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64)), this.steamColor * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			this.steamPosition.Y = this.steamPosition.Y - (float)time.ElapsedGameTime.Milliseconds * 0.1f;
			this.steamPosition.Y = this.steamPosition.Y % -256f;
			this.steamPosition -= Game1.getMostRecentViewportMotion();
			if (Game1.random.NextDouble() < 0.001)
			{
				Game1.playSound("cavedrip");
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int num = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (num == 21)
			{
				Game1.warpFarmer("Town", 35, 97, 2);
				DelayedAction.playSoundAfterDelay("stairsdown", 250);
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.waterColor = Color.LimeGreen * 0.75f;
			this.characters.Clear();
			this.characters.Add(this.Krobus);
			this.steamPosition = new Vector2(0f, 0f);
			this.steamAnimation = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\steamAnimation");
			Game1.ambientLight = new Color(250, 140, 160);
		}

		public override StardewValley.Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (!who.fishCaught.ContainsKey(682) && Game1.random.NextDouble() < 0.1 + ((who.getTileX() > 14 && who.getTileY() > 42) ? 0.08 : 0.0))
			{
				return new StardewValley.Object(682, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.changeMusicTrack("none");
		}
	}
}
