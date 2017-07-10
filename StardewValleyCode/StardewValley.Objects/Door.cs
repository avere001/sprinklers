using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Objects
{
	public class Door : StardewValley.Object
	{
		public const int gateClosedPosition = 0;

		public const int gateOpenedPosition = 112;

		public int doorPosition;

		public int doorMotion;

		public bool locked;

		public Door()
		{
			this.name = "Door";
			this.type = "interactive";
			this.bigCraftable = true;
		}

		public Door(Vector2 tileLocation, GameLocation environment, bool locked)
		{
			this.locked = locked;
			this.bigCraftable = true;
			this.name = "Door";
			this.type = "interactive";
			this.tileLocation = tileLocation;
			this.checkForOrientation(environment);
			this.parentSheetIndex = 79;
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public void checkForOrientation(GameLocation environment)
		{
			Vector2 tileLocation = new Vector2(this.tileLocation.X, this.tileLocation.Y - 1f);
			Vector2 tileLocation2 = new Vector2(this.tileLocation.X, this.tileLocation.Y + 1f);
			if (environment.isTileOccupiedForPlacement(tileLocation, null) && environment.isTileOccupiedForPlacement(tileLocation2, null))
			{
				this.showNextIndex = true;
			}
		}

		public override void updateWhenCurrentLocation(GameTime time)
		{
			this.doorPosition += this.doorMotion;
			if ((this.doorPosition >= 112 - (this.showNextIndex ? 40 : 0) && this.doorMotion > 0) || this.doorPosition <= 0)
			{
				this.doorMotion = 0;
			}
		}

		public override bool isPassable()
		{
			return this.doorPosition >= 112 - (this.showNextIndex ? 40 : 0);
		}

		private void unlock()
		{
			this.locked = false;
			this.fragility = 1;
			Game1.playSound("openBox");
			Game1.currentLocation.debris.Add(new Debris(Game1.bigCraftableSpriteSheet, new Rectangle(88, 2606, 16, 28), 1, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize)));
		}

		public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
		{
			if (!this.locked || dropIn.parentSheetIndex != 471)
			{
				return false;
			}
			if (probe)
			{
				return true;
			}
			who.consumeObject(471, 1);
			this.unlock();
			return true;
		}

		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			if (this.locked)
			{
				if (!who.hasItemInInventory(471, 1, 0))
				{
					Game1.playSound("woodyStep");
					return false;
				}
				who.consumeObject(471, 1);
				this.unlock();
			}
			who.temporaryImpassableTile = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (this.doorMotion == 0 && this.doorPosition >= 112 - (this.showNextIndex ? 40 : 0))
			{
				this.doorMotion = -4;
			}
			else if (this.doorMotion == 0)
			{
				this.doorMotion = 4;
			}
			else
			{
				this.doorMotion = -1 * this.doorMotion;
			}
			return true;
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			Rectangle sourceRectForBigCraftable = StardewValley.Object.getSourceRectForBigCraftable((this.showNextIndex ? (base.ParentSheetIndex + 1) : base.ParentSheetIndex) + (this.locked ? 2 : 0));
			sourceRectForBigCraftable.Y += 29;
			sourceRectForBigCraftable.Height = 12;
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize + 116))), new Rectangle?(sourceRectForBigCraftable), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 32) / 9999f));
			sourceRectForBigCraftable.Y -= 29;
			sourceRectForBigCraftable.Height = 32;
			sourceRectForBigCraftable.Height -= this.doorPosition;
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize + this.doorPosition))), new Rectangle?(sourceRectForBigCraftable), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 32) / 10000f));
			if (this.showNextIndex)
			{
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize * 3 / 2 + this.doorPosition * 3 / 2))), new Rectangle?(sourceRectForBigCraftable), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 2) * Game1.tileSize - 32) / 10000f));
			}
		}
	}
}
