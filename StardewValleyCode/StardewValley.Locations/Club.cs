using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using xTile;

namespace StardewValley.Locations
{
	public class Club : GameLocation
	{
		public static int timesPlayedCalicoJack;

		public static int timesPlayedSlots;

		private string coinBuffer;

		public Club()
		{
		}

		public Club(Map map, string name) : base(map, name)
		{
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.lightGlows.Clear();
			this.coinBuffer = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? "     " : ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh) ? "\u3000\u3000" : "  "));
			if (!Game1.player.hasClubCard)
			{
				Game1.currentLocation = Game1.getLocationFromName("SandyHouse");
				Game1.changeMusicTrack("none");
				Game1.currentLocation.resetForPlayerEntry();
				NPC characterFromName = Game1.currentLocation.getCharacterFromName("Bouncer");
				if (characterFromName != null)
				{
					Vector2 value = new Vector2(17f, 4f);
					characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\Locations:Club_Bouncer_TextAboveHead" + (Game1.random.Next(2) + 1), new object[0]), -1, 2, 3000, 0);
					int num = Game1.random.Next();
					Game1.playSound("thudStep");
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(288, 100f, 1, 24, value * (float)Game1.tileSize, true, false, Game1.currentLocation, Game1.player)
					{
						shakeIntensity = 0.5f,
						shakeIntensityChange = 0.002f,
						extraInfoForEndBehavior = num,
						endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
					});
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, value * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(4 * Game1.tileSize + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						id = (float)num
					});
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, value * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, true, (float)(4 * Game1.tileSize + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = 100,
						id = (float)num
					});
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, value * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(4 * Game1.tileSize + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = 200,
						id = (float)num
					});
					if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
					{
						Game1.fuseSound = Game1.soundBank.GetCue("fuse");
						Game1.fuseSound.Play();
					}
				}
				Game1.player.position = new Vector2(17f, 4f) * (float)Game1.tileSize;
			}
		}

		public override void checkForMusic(GameTime time)
		{
			if (Game1.random.NextDouble() < 0.002)
			{
				Game1.playSound("boop");
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			Game1.changeMusicTrack("none");
			base.cleanupBeforePlayerExit();
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			SpriteText.drawStringWithScrollBackground(b, this.coinBuffer + Game1.player.clubCoins, Game1.tileSize, Game1.tileSize / 4, "", 1f, -1);
			Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.tileSize + Game1.pixelZoom), (float)(Game1.tileSize / 4 + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
		}
	}
}
