using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Tools
{
	public class Wand : Tool
	{
		public bool charged;

		public Wand() : base("Return Scepter", 0, 2, 2, false, 0)
		{
			this.upgradeLevel = 0;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			this.instantUse = true;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14318", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14319", new object[0]);
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			if (who.bathingClothes)
			{
				return;
			}
			this.indexOfMenuItemView = 2;
			base.CurrentParentTileIndex = 2;
			if (who.IsMainPlayer)
			{
				for (int i = 0; i < 12; i++)
				{
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float)Game1.random.Next(25, 75), 6, 1, new Vector2((float)Game1.random.Next((int)who.position.X - Game1.tileSize * 4, (int)who.position.X + Game1.tileSize * 3), (float)Game1.random.Next((int)who.position.Y - Game1.tileSize * 4, (int)who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
				}
				Game1.playSound("wand");
				Game1.displayFarmer = false;
				Game1.player.Halt();
				Game1.player.faceDirection(2);
				Game1.player.freezePause = 1000;
				Game1.flashAlpha = 1f;
				DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.wandWarpForReal), 1000);
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
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}

		public override bool actionWhenPurchased()
		{
			Game1.player.mailReceived.Add("ReturnScepter");
			return base.actionWhenPurchased();
		}

		private void wandWarpForReal()
		{
			Game1.warpFarmer("Farm", 64, 15, false);
			if (!Game1.isStartingToGetDarkOut())
			{
				Game1.playMorningSong();
			}
			else
			{
				Game1.changeMusicTrack("none");
			}
			Game1.fadeToBlackAlpha = 0.99f;
			Game1.screenGlow = false;
			Game1.player.temporarilyInvincible = false;
			Game1.player.temporaryInvincibilityTimer = 0;
			Game1.displayFarmer = true;
		}
	}
}
