using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Events
{
	public class WorldChangeEvent : FarmEvent
	{
		public const int identifier = 942066;

		public const int jojaGreenhouse = 0;

		public const int junimoGreenHouse = 1;

		public const int jojaBoiler = 2;

		public const int junimoBoiler = 3;

		public const int jojaBridge = 4;

		public const int junimoBridge = 5;

		public const int jojaBus = 6;

		public const int junimoBus = 7;

		public const int jojaBoulder = 8;

		public const int junimoBoulder = 9;

		private int whichEvent;

		private int cutsceneLengthTimer;

		private int timerSinceFade;

		private int soundTimer;

		private int soundInterval = 99999;

		private GameLocation location;

		private string sound;

		private bool kill;

		private bool wasRaining;

		public WorldChangeEvent(int which)
		{
			this.whichEvent = which;
		}

		public bool setUp()
		{
			Game1.currentLightSources.Clear();
			this.location = null;
			int num = 0;
			int num2 = 0;
			this.cutsceneLengthTimer = 8000;
			this.wasRaining = Game1.isRaining;
			Game1.isRaining = false;
			switch (this.whichEvent)
			{
			case 0:
				this.location = Game1.getLocationFromName("Farm");
				num = 28;
				num2 = 13;
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1349, 19, 28), 150f, 5, 999, new Vector2((float)(25 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(12 * Game1.tileSize - Game1.tileSize / 2)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 0.0961f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 140f, 5, 999, new Vector2((float)(31 * Game1.tileSize - 4 * Game1.pixelZoom), (float)(11 * Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 0.0961f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1000f, 2, 999, new Vector2((float)(28 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(9 * Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 0.0961f
				});
				this.soundInterval = 560;
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f));
				this.sound = "axchop";
				break;
			case 1:
				this.location = Game1.getLocationFromName("Farm");
				num = 28;
				num2 = 13;
				Utility.addSprinklesToLocation(this.location, num, 12, 7, 7, 15000, 150, Color.LightCyan, null, false);
				Utility.addStarsAndSpirals(this.location, num, 12, 7, 7, 15000, 150, Color.White, null, false);
				this.sound = "junimoMeep1";
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f, Color.DarkGoldenrod));
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(28 * Game1.tileSize), (float)(12 * Game1.tileSize - Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2000f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.soundInterval = 800;
				break;
			case 2:
				this.location = Game1.getLocationFromName("Town");
				num = 105;
				num2 = 79;
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 100f, 5, 999, new Vector2((float)(104 * Game1.tileSize), (float)(79 * Game1.tileSize - Game1.tileSize / 2)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1406, 22, 26), 700f, 2, 999, new Vector2((float)(108 * Game1.tileSize - 6 * Game1.pixelZoom), (float)(79 * Game1.tileSize - Game1.tileSize * 2 / 3)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float)(106 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(76 * Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(335, 1410, 21, 21), 999f, 1, 9999, new Vector2((float)(108 * Game1.tileSize), (float)(80 * Game1.tileSize + Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.soundInterval = 500;
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f));
				this.sound = "clank";
				break;
			case 3:
				this.location = Game1.getLocationFromName("Town");
				num = 105;
				num2 = 79;
				Utility.addSprinklesToLocation(this.location, num + 1, num2, 6, 4, 15000, 350, Color.LightCyan, null, false);
				Utility.addStarsAndSpirals(this.location, num + 1, num2, 6, 4, 15000, 350, Color.White, null, false);
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(104 * Game1.tileSize), (float)(80 * Game1.tileSize - Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2000f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(108 * Game1.tileSize), (float)(80 * Game1.tileSize - Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2300f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					color = Color.HotPink,
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.sound = "junimoMeep1";
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f, Color.DarkGoldenrod));
				this.soundInterval = 800;
				break;
			case 4:
				this.location = Game1.getLocationFromName("Mountain");
				num = 95;
				num2 = 27;
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(383, 1378, 28, 27), 400f, 2, 999, new Vector2((float)(86 * Game1.tileSize), (float)(26 * Game1.tileSize - Game1.tileSize / 2)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					motion = new Vector2(0.5f, 0f)
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1406, 22, 26), 350f, 2, 999, new Vector2((float)(98 * Game1.tileSize), (float)(26 * Game1.tileSize - Game1.tileSize / 2)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(358, 1415, 31, 20), 999f, 1, 9999, new Vector2((float)(92 * Game1.tileSize), (float)(26 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(335, 1410, 21, 21), 999f, 1, 9999, new Vector2((float)(100 * Game1.tileSize), (float)(26 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float)(91 * Game1.tileSize), (float)(25 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 0.8f
				});
				this.soundInterval = 700;
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f));
				this.sound = "axchop";
				break;
			case 5:
				this.location = Game1.getLocationFromName("Mountain");
				num = 95;
				num2 = 27;
				Utility.addSprinklesToLocation(this.location, num, num2, 7, 4, 15000, 150, Color.LightCyan, null, false);
				Utility.addStarsAndSpirals(this.location, num + 1, num2, 7, 4, 15000, 350, Color.White, null, false);
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(91 * Game1.tileSize), (float)(26 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2000f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(99 * Game1.tileSize), (float)(26 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2300f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					color = Color.Yellow,
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.sound = "junimoMeep1";
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f, Color.DarkGoldenrod));
				this.soundInterval = 800;
				break;
			case 6:
				this.location = Game1.getLocationFromName("BusStop");
				num = 14;
				num2 = 8;
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1349, 19, 28), 150f, 5, 999, new Vector2((float)(19 * Game1.tileSize), (float)(8 * Game1.tileSize - Game1.tileSize / 2)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 140f, 5, 999, new Vector2((float)(10 * Game1.tileSize), (float)(8 * Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float)(14 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(3 * Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.soundInterval = 560;
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f));
				this.sound = "clank";
				break;
			case 7:
				this.location = Game1.getLocationFromName("BusStop");
				num = 14;
				num2 = 8;
				Utility.addSprinklesToLocation(this.location, num, num2, 9, 4, 10000, 200, Color.LightCyan, null, true);
				Utility.addStarsAndSpirals(this.location, num, num2, 9, 4, 15000, 150, Color.White, null, false);
				this.sound = "junimoMeep1";
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(10 * Game1.tileSize), (float)(11 * Game1.tileSize - Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2000f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(12 * Game1.tileSize), (float)(11 * Game1.tileSize - Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2300f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					color = Color.Pink,
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(14 * Game1.tileSize), (float)(11 * Game1.tileSize - Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2200f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					color = Color.Yellow,
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(16 * Game1.tileSize), (float)(11 * Game1.tileSize - Game1.tileSize)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2100f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					color = Color.LightBlue,
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f, Color.DarkGoldenrod));
				this.soundInterval = 500;
				break;
			case 8:
				this.location = Game1.getLocationFromName("Mountain");
				this.location.resetForPlayerEntry();
				num = 48;
				num2 = 5;
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 100f, 5, 999, new Vector2((float)(45 * Game1.tileSize), (float)(5 * Game1.tileSize - Game1.tileSize / 2)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 1340, 17, 37), 50f, 2, 99999, new Vector2((float)(47 * Game1.tileSize + Game1.tileSize / 2), (float)(3 * Game1.tileSize - Game1.tileSize / 2)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					yPeriodic = true,
					yPeriodicLoopTime = 100f,
					yPeriodicRange = (float)(Game1.pixelZoom / 2)
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(335, 1410, 21, 21), 999f, 1, 9999, new Vector2((float)(44 * Game1.tileSize), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float)(50 * Game1.tileSize), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.soundInterval = 100;
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f));
				this.sound = "thudStep";
				break;
			case 9:
				this.location = Game1.getLocationFromName("Mountain");
				this.location.resetForPlayerEntry();
				num = 48;
				num2 = 5;
				Utility.addSprinklesToLocation(this.location, num, num2, 4, 4, 15000, 350, Color.LightCyan, null, false);
				Utility.addStarsAndSpirals(this.location, num + 1, num2, 4, 4, 15000, 550, Color.White, null, false);
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(45 * Game1.tileSize), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2000f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float)(50 * Game1.tileSize), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), false, false)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					xPeriodic = true,
					xPeriodicLoopTime = 2300f,
					xPeriodicRange = (float)(Game1.tileSize / 4),
					color = Color.Yellow,
					light = true,
					lightcolor = Color.DarkGoldenrod,
					lightRadius = 1f
				});
				this.sound = "junimoMeep1";
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 1f));
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 1f, Color.DarkCyan));
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)num, (float)num2) * (float)Game1.tileSize, 4f, Color.DarkGoldenrod));
				this.soundInterval = 1000;
				break;
			}
			this.soundTimer = this.soundInterval;
			Game1.currentLocation = this.location;
			Game1.fadeClear();
			Game1.nonWarpFade = true;
			Game1.timeOfDay = 2400;
			Game1.displayHUD = false;
			Game1.viewportFreeze = true;
			Game1.player.position.X = -999999f;
			Game1.viewport.X = Math.Max(0, Math.Min(this.location.map.DisplayWidth - Game1.viewport.Width, num * Game1.tileSize - Game1.viewport.Width / 2));
			Game1.viewport.Y = Math.Max(0, Math.Min(this.location.map.DisplayHeight - Game1.viewport.Height, num2 * Game1.tileSize - Game1.viewport.Height / 2));
			Game1.changeMusicTrack("nightTime");
			return false;
		}

		public bool tickUpdate(GameTime time)
		{
			Game1.UpdateGameClock(time);
			this.cutsceneLengthTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.timerSinceFade > 0)
			{
				this.timerSinceFade -= time.ElapsedGameTime.Milliseconds;
				Game1.globalFade = true;
				Game1.fadeToBlackAlpha = 1f;
				return this.timerSinceFade <= 0;
			}
			if (this.cutsceneLengthTimer <= 0 && !Game1.globalFade)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.endEvent), 0.01f);
			}
			this.soundTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.soundTimer <= 0 && this.sound != null)
			{
				Game1.playSound(this.sound);
				this.soundTimer = this.soundInterval;
			}
			return false;
		}

		public void endEvent()
		{
			Game1.changeMusicTrack("none");
			this.timerSinceFade = 1500;
			Game1.isRaining = this.wasRaining;
			Game1.getFarm().temporarySprites.Clear();
		}

		public void draw(SpriteBatch b)
		{
		}

		public void makeChangesToLocation()
		{
		}

		public void drawAboveEverything(SpriteBatch b)
		{
		}
	}
}
