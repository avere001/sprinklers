using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Tools;
using System;

namespace StardewValley.Objects
{
	public class Cask : StardewValley.Object
	{
		public const int defaultDaysToMature = 56;

		public float agingRate;

		public float daysToMature;

		public Cask()
		{
		}

		public Cask(Vector2 v) : base(v, 163, false)
		{
		}

		public override bool performToolAction(Tool t)
		{
			if (t == null || !t.isHeavyHitter() || t is MeleeWeapon)
			{
				return base.performToolAction(t);
			}
			if (this.heldObject != null)
			{
				Game1.createItemDebris(this.heldObject, this.tileLocation * (float)Game1.tileSize, -1, null);
			}
			Game1.playSound("woodWhack");
			if (this.heldObject == null)
			{
				return true;
			}
			this.heldObject = null;
			this.minutesUntilReady = -1;
			return false;
		}

		public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
		{
			if (dropIn != null && dropIn.bigCraftable)
			{
				return false;
			}
			if (this.heldObject != null)
			{
				return false;
			}
			if (!probe && (who == null || !(who.currentLocation is Cellar)))
			{
				Game1.showRedMessageUsingLoadString("Strings\\Objects:CaskNoCellar");
				return false;
			}
			if (this.quality >= 4)
			{
				return false;
			}
			bool flag = false;
			float num = 1f;
			int parentSheetIndex = dropIn.parentSheetIndex;
			if (parentSheetIndex <= 348)
			{
				if (parentSheetIndex != 303)
				{
					if (parentSheetIndex != 346)
					{
						if (parentSheetIndex == 348)
						{
							flag = true;
							num = 1f;
						}
					}
					else
					{
						flag = true;
						num = 2f;
					}
				}
				else
				{
					flag = true;
					num = 1.66f;
				}
			}
			else if (parentSheetIndex != 424)
			{
				if (parentSheetIndex != 426)
				{
					if (parentSheetIndex == 459)
					{
						flag = true;
						num = 2f;
					}
				}
				else
				{
					flag = true;
					num = 4f;
				}
			}
			else
			{
				flag = true;
				num = 4f;
			}
			if (flag)
			{
				this.heldObject = (dropIn.getOne() as StardewValley.Object);
				if (!probe)
				{
					this.agingRate = num;
					this.daysToMature = 56f;
					this.minutesUntilReady = 999999;
					if (this.heldObject.quality == 1)
					{
						this.daysToMature = 42f;
					}
					else if (this.heldObject.quality == 2)
					{
						this.daysToMature = 28f;
					}
					else if (this.heldObject.quality == 4)
					{
						this.daysToMature = 0f;
						this.minutesUntilReady = 1;
					}
					Game1.playSound("Ship");
					Game1.playSound("bubbles");
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
					{
						alphaFade = 0.005f
					});
				}
				return true;
			}
			return false;
		}

		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			return base.checkForAction(who, justCheckingForActivity);
		}

		public override void DayUpdate(GameLocation location)
		{
			base.DayUpdate(location);
			if (this.heldObject != null)
			{
				this.minutesUntilReady = 999999;
				this.daysToMature -= this.agingRate;
				this.checkForMaturity();
			}
		}

		public void checkForMaturity()
		{
			if (this.daysToMature <= 0f)
			{
				this.minutesUntilReady = 1;
				this.heldObject.quality = 4;
				return;
			}
			if (this.daysToMature <= 28f)
			{
				this.heldObject.quality = 2;
				return;
			}
			if (this.daysToMature <= 42f)
			{
				this.heldObject.quality = 1;
			}
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			base.draw(spriteBatch, x, y, alpha);
			if (this.heldObject != null && this.heldObject.quality > 0)
			{
				Vector2 vector = (this.minutesUntilReady > 0) ? new Vector2(Math.Abs(this.scale.X - 5f), Math.Abs(this.scale.Y - 5f)) : Vector2.Zero;
				vector *= (float)Game1.pixelZoom;
				Vector2 vector2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
				Rectangle destinationRectangle = new Rectangle((int)(vector2.X + (float)(Game1.tileSize / 2) - (float)(Game1.pixelZoom * 2) - vector.X / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)(vector2.Y + (float)Game1.tileSize + (float)(Game1.pixelZoom * 2) - vector.Y / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)((float)(Game1.pixelZoom * 4) + vector.X), (int)((float)(Game1.pixelZoom * 4) + vector.Y / 2f));
				spriteBatch.Draw(Game1.mouseCursors, destinationRectangle, new Rectangle?((this.heldObject.quality < 4) ? new Rectangle(338 + (this.heldObject.quality - 1) * 8, 400, 8, 8) : new Rectangle(346, 392, 8, 8)), Color.White * 0.95f, 0f, Vector2.Zero, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f);
			}
		}
	}
}
