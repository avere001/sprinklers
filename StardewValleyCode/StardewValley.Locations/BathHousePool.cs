using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using xTile;

namespace StardewValley.Locations
{
	public class BathHousePool : GameLocation
	{
		public const float steamZoom = 4f;

		public const float steamYMotionPerMillisecond = 0.1f;

		public const float millisecondsPerSteamFrame = 50f;

		private Texture2D steamAnimation;

		private Texture2D swimShadow;

		private Vector2 steamPosition;

		private int swimShadowTimer;

		private int swimShadowFrame;

		public BathHousePool()
		{
		}

		public BathHousePool(Map map, string name) : base(map, name)
		{
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			Game1.changeMusicTrack("pool_ambient");
			this.steamPosition = new Vector2(0f, 0f);
			this.steamAnimation = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\steamAnimation");
			this.swimShadow = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\swimShadow");
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.changeMusicTrack("none");
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (this.currentEvent != null)
			{
				using (List<NPC>.Enumerator enumerator = this.currentEvent.actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC current = enumerator.Current;
						if (current.swimming)
						{
							b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, current.position + new Vector2(0f, (float)(current.sprite.spriteHeight / 3 * Game1.pixelZoom + Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
						}
					}
					goto IL_237;
				}
			}
			foreach (NPC current2 in this.characters)
			{
				if (current2.swimming)
				{
					b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, current2.position + new Vector2(0f, (float)(current2.sprite.spriteHeight / 3 * Game1.pixelZoom + Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				}
			}
			foreach (Farmer current3 in this.farmers)
			{
				if (current3.swimming)
				{
					b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, current3.position + new Vector2(0f, (float)(current3.sprite.spriteHeight / 3 * Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				}
			}
			IL_237:
			if (Game1.player.swimming)
			{
				b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(0f, (float)(Game1.player.sprite.spriteHeight / 4 * Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.Blue * 0.75f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
			}
		}

		public override void checkForMusic(GameTime time)
		{
			base.checkForMusic(time);
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			for (float num = this.steamPosition.X; num < (float)Game1.graphics.GraphicsDevice.Viewport.Width + 256f; num += 256f)
			{
				for (float num2 = this.steamPosition.Y; num2 < (float)(Game1.graphics.GraphicsDevice.Viewport.Height + 128); num2 += 256f)
				{
					b.Draw(this.steamAnimation, new Vector2(num, num2), new Rectangle?(new Rectangle(0, 0, 64, 64)), Color.White * 0.8f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			this.steamPosition.Y = this.steamPosition.Y - (float)time.ElapsedGameTime.Milliseconds * 0.1f;
			this.steamPosition.Y = this.steamPosition.Y % -256f;
			this.steamPosition -= Game1.getMostRecentViewportMotion();
			this.swimShadowTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.swimShadowTimer <= 0)
			{
				this.swimShadowTimer = 70;
				this.swimShadowFrame++;
				this.swimShadowFrame %= 10;
			}
		}
	}
}
