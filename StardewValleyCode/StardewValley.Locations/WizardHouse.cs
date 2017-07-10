using Microsoft.Xna.Framework;
using System;
using xTile;

namespace StardewValley.Locations
{
	public class WizardHouse : GameLocation
	{
		private int cauldronTimer = 250;

		public WizardHouse()
		{
		}

		public WizardHouse(Map m, string name) : base(m, name)
		{
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			this.cauldronTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.cauldronTimer <= 0)
			{
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(372, 1956, 10, 10), new Vector2(3f, 20f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float)Game1.random.Next(Game1.tileSize / 4)), false, 0.002f, Color.Lime)
				{
					alpha = 0.75f,
					motion = new Vector2(0f, -0.5f),
					acceleration = new Vector2(-0.002f, 0f),
					interval = 99999f,
					layerDepth = 22.5f * (float)Game1.tileSize / 10000f - (float)Game1.random.Next(100) / 10000f,
					scale = (float)(Game1.pixelZoom * 3) / 4f,
					scaleChange = 0.01f,
					rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f
				});
				this.cauldronTimer = 100;
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			Game1.changeMusicTrack("none");
			base.cleanupBeforePlayerExit();
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(10f, 12f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)), false, 0f, Color.White)
			{
				interval = 50f,
				totalNumberOfLoops = 99999,
				animationLength = 4,
				light = true,
				lightRadius = 2f,
				scale = (float)Game1.pixelZoom
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(2f, 21f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 4 / 5), (float)(Game1.tileSize / 2)), false, 0f, Color.White)
			{
				interval = 50f,
				totalNumberOfLoops = 99999,
				animationLength = 4,
				light = true,
				lightRadius = 1f,
				scale = (float)(Game1.pixelZoom / 2)
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(3f, 21f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 2)), false, 0f, Color.White)
			{
				interval = 50f,
				totalNumberOfLoops = 99999,
				animationLength = 4,
				light = true,
				lightRadius = 1f,
				scale = (float)(Game1.pixelZoom * 3 / 4)
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(4f, 21f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(Game1.tileSize / 2)), false, 0f, Color.White)
			{
				interval = 50f,
				totalNumberOfLoops = 99999,
				animationLength = 4,
				light = true,
				lightRadius = 1f,
				scale = (float)(Game1.pixelZoom / 2)
			});
			if (Game1.player.eventsSeen.Contains(418172))
			{
				base.setMapTileIndex(2, 12, 2143, "Front", 1);
			}
		}
	}
}
