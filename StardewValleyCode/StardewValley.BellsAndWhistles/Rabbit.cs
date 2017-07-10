using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Rabbit : Critter
	{
		private int characterCheckTimer = 200;

		private bool running;

		public Rabbit(Vector2 position, bool flip)
		{
			this.position = position * (float)Game1.tileSize;
			position.Y += (float)(Game1.tileSize * 3 / 4);
			this.flip = flip;
			this.baseFrame = (Game1.currentSeason.Equals("winter") ? 74 : 54);
			this.sprite = new AnimatedSprite(Critter.critterTexture, Game1.currentSeason.Equals("winter") ? 69 : 68, 32, 32);
			this.sprite.loop = true;
			this.startingPosition = position;
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.characterCheckTimer <= 0 && !this.running)
			{
				if (Utility.isOnScreen(this.position, -Game1.tileSize / 2))
				{
					this.running = true;
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(this.baseFrame, 40),
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, 40),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, 40),
						new FarmerSprite.AnimationFrame(this.baseFrame + 3, 100),
						new FarmerSprite.AnimationFrame(this.baseFrame + 5, 70),
						new FarmerSprite.AnimationFrame(this.baseFrame + 5, 40)
					});
					this.sprite.loop = true;
				}
				this.characterCheckTimer = 200;
			}
			if (this.running)
			{
				this.position.X = this.position.X + (float)(this.flip ? -6 : 6);
			}
			if (this.running && this.characterCheckTimer <= 0)
			{
				this.characterCheckTimer = 200;
				if (environment.largeTerrainFeatures != null)
				{
					Rectangle value = new Rectangle((int)this.position.X + Game1.tileSize / 2, (int)this.position.Y - Game1.tileSize / 2, Game1.pixelZoom, Game1.tileSize * 3);
					foreach (LargeTerrainFeature current in environment.largeTerrainFeatures)
					{
						if (current is Bush && current.getBoundingBox().Intersects(value))
						{
							(current as Bush).performUseAction(current.tilePosition);
							return true;
						}
					}
				}
			}
			return base.update(time, environment);
		}
	}
}
