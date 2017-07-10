using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
	public class GiantCrop : ResourceClump
	{
		public const int cauliflower = 0;

		public const int melon = 1;

		public const int pumpkin = 2;

		public int which;

		public bool forSale;

		public GiantCrop()
		{
		}

		public GiantCrop(int indexOfSmallerVersion, Vector2 tile)
		{
			this.tile = tile;
			this.parentSheetIndex = indexOfSmallerVersion;
			if (indexOfSmallerVersion != 190)
			{
				if (indexOfSmallerVersion != 254)
				{
					if (indexOfSmallerVersion == 276)
					{
						this.which = 2;
					}
				}
				else
				{
					this.which = 1;
				}
			}
			else
			{
				this.which = 0;
			}
			this.width = 3;
			this.height = 3;
			this.health = 3f;
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			spriteBatch.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, tileLocation * (float)Game1.tileSize - new Vector2((this.shakeTimer > 0f) ? ((float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 2f) : 0f, (float)Game1.tileSize)), new Rectangle?(new Rectangle(112 + this.which * 48, 512, 48, 63)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y + 2f) * (float)Game1.tileSize / 10000f);
		}

		public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
		{
			if (t == null || !(t is Axe))
			{
				return false;
			}
			Game1.playSound("axchop");
			int num = t.getLastFarmerToUse().toolPower + 1;
			this.health -= (float)num;
			Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(4, 9), false, -1, false, -1);
			if (this.shakeTimer <= 0f)
			{
				this.shakeTimer = 100f;
			}
			if (this.health <= 0f)
			{
				t.getLastFarmerToUse().gainExperience(5, 50 * ((t.getLastFarmerToUse().luckLevel + 1) / 2));
				Random random;
				if (Game1.IsMultiplayer)
				{
					Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
					random = Game1.recentMultiplayerRandom;
				}
				else
				{
					random = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
				}
				int num2 = random.Next(15, 22);
				if (Game1.IsMultiplayer)
				{
					Game1.createMultipleObjectDebris(this.parentSheetIndex, (int)tileLocation.X + 1, (int)tileLocation.Y + 1, num2, t.getLastFarmerToUse().uniqueMultiplayerID);
				}
				else
				{
					Game1.createRadialDebris(Game1.currentLocation, this.parentSheetIndex, (int)tileLocation.X, (int)tileLocation.Y, num2, false, -1, true, -1);
				}
				StardewValley.Object @object = new StardewValley.Object(Vector2.Zero, this.parentSheetIndex, 1);
				Game1.setRichPresence("giantcrop", @object.Name);
				Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(4, 9), false, -1, false, -1);
				Game1.playSound("stumpCrack");
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 0f)) * (float)Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 1f)) * (float)Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(0f, 1f)) * (float)Game1.tileSize, Color.White, 8, false, 90f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), Color.White, 8, false, 70f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(2f, 0f)) * (float)Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(2f, 1f)) * (float)Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(2f, 2f)) * (float)Game1.tileSize, Color.White, 8, false, 90f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 3 / 2), (float)(Game1.tileSize * 3 / 2)), Color.White, 8, false, 70f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(0f, 2f)) * (float)Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 2f)) * (float)Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
				return true;
			}
			return false;
		}
	}
}
