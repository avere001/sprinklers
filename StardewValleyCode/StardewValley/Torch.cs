using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;

namespace StardewValley
{
	public class Torch : Object
	{
		public const float yVelocity = 1f;

		public const float yDissapearLevel = -100f;

		public const double ashChance = 0.015;

		private float color;

		private Vector2[] ashes = new Vector2[3];

		public Torch()
		{
		}

		public Torch(Vector2 tileLocation, int initialStack) : base(tileLocation, 93, initialStack)
		{
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4);
		}

		public Torch(Vector2 tileLocation, int initialStack, int index) : base(tileLocation, index, initialStack)
		{
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4);
		}

		public Torch(Vector2 tileLocation, int index, bool bigCraftable) : base(tileLocation, index, false)
		{
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public override Item getOne()
		{
			if (this.bigCraftable)
			{
				return new Torch(this.tileLocation, this.parentSheetIndex, true)
				{
					isRecipe = this.isRecipe
				};
			}
			return new Torch(this.tileLocation, 1)
			{
				isRecipe = this.isRecipe
			};
		}

		public override void actionOnPlayerEntry()
		{
			base.actionOnPlayerEntry();
			if (this.bigCraftable && this.isOn)
			{
				AmbientLocationSounds.addSound(this.tileLocation, 1);
			}
		}

		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (!this.bigCraftable)
			{
				return base.checkForAction(who, justCheckingForActivity);
			}
			if (justCheckingForActivity)
			{
				return true;
			}
			this.isOn = !this.isOn;
			if (this.isOn)
			{
				if (this.bigCraftable)
				{
					if (who != null)
					{
						Game1.playSound("fireball");
					}
					base.initializeLightSource(this.tileLocation);
					AmbientLocationSounds.addSound(this.tileLocation, 1);
				}
			}
			else if (this.bigCraftable)
			{
				this.performRemoveAction(this.tileLocation, Game1.currentLocation);
				if (who != null)
				{
					Game1.playSound("woodyHit");
				}
			}
			return true;
		}

		public override bool placementAction(GameLocation location, int x, int y, Farmer who)
		{
			Vector2 vector = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			Torch torch = this.bigCraftable ? new Torch(vector, this.parentSheetIndex, true) : new Torch(vector, 1, this.parentSheetIndex);
			if (this.bigCraftable)
			{
				torch.isOn = false;
			}
			torch.tileLocation = vector;
			torch.initializeLightSource(vector);
			location.objects.Add(vector, torch);
			if (who != null)
			{
				Game1.playSound("woodyStep");
			}
			return true;
		}

		public override void DayUpdate(GameLocation location)
		{
			base.DayUpdate(location);
		}

		public override bool isPassable()
		{
			return !this.bigCraftable;
		}

		public override void updateWhenCurrentLocation(GameTime time)
		{
			base.updateWhenCurrentLocation(time);
			this.updateAshes((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
		}

		public override void actionWhenBeingHeld(Farmer who)
		{
			base.actionWhenBeingHeld(who);
		}

		private void updateAshes(int identifier)
		{
			if (Utility.isOnScreen(this.tileLocation * (float)Game1.tileSize, 4 * Game1.tileSize))
			{
				for (int i = this.ashes.Length - 1; i >= 0; i--)
				{
					Vector2 vector = this.ashes[i];
					vector.Y -= 1f * ((float)(i + 1) * 0.25f);
					if (i % 2 != 0)
					{
						vector.X += (float)Math.Sin((double)this.ashes[i].Y / 6.2831853071795862) / 2f;
					}
					this.ashes[i] = vector;
					if (Game1.random.NextDouble() < 0.0075 && this.ashes[i].Y < -100f)
					{
						this.ashes[i] = new Vector2((float)(Game1.random.Next(-1, 3) * Game1.pixelZoom) * 0.75f, 0f);
					}
				}
				this.color = Math.Max(-0.8f, Math.Min(0.7f, this.color + this.ashes[0].Y / 1200f));
			}
		}

		public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
		{
			AmbientLocationSounds.removeSound(this.tileLocation);
			if (this.bigCraftable)
			{
				this.isOn = false;
			}
			base.performRemoveAction(this.tileLocation, environment);
		}

		public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
		{
			Rectangle sourceRectForObject = Game1.currentLocation.getSourceRectForObject(base.ParentSheetIndex);
			sourceRectForObject.Y += 8;
			sourceRectForObject.Height /= 2;
			spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)xNonTile, (float)(yNonTile + Game1.tileSize / 2))), new Rectangle?(sourceRectForObject), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
			sourceRectForObject.X = 276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(xNonTile * 320) + (double)(yNonTile * 49)) % 700.0 / 100.0) * 8;
			sourceRectForObject.Y = 1965;
			sourceRectForObject.Width = 8;
			sourceRectForObject.Height = 8;
			spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + Game1.tileSize / 2 + Game1.pixelZoom), (float)(yNonTile + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(sourceRectForObject), Color.White * 0.75f, 0f, new Vector2(4f, 4f), (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, layerDepth + 1E-05f);
			spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + Game1.tileSize / 2 + Game1.pixelZoom), (float)(yNonTile + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(new Rectangle(88, 1779, 30, 30)), Color.PaleGoldenrod * (Game1.currentLocation.IsOutdoors ? 0.35f : 0.43f), 0f, new Vector2(15f, 15f), (float)(Game1.pixelZoom * 2) + (float)((double)(Game1.tileSize / 2) * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(xNonTile * 777) + (double)(yNonTile * 9746)) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			if (!Game1.eventUp || Game1.currentLocation.IsFarm)
			{
				if (!this.bigCraftable)
				{
					Rectangle sourceRectForObject = Game1.currentLocation.getSourceRectForObject(base.ParentSheetIndex);
					sourceRectForObject.Y += 8;
					sourceRectForObject.Height /= 2;
					Texture2D arg_D7_1 = Game1.objectSpriteSheet;
					Vector2 arg_D7_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize + Game1.tileSize / 2)));
					Rectangle? arg_D7_3 = new Rectangle?(sourceRectForObject);
					Color arg_D7_4 = Color.White;
					float arg_D7_5 = 0f;
					Vector2 arg_D7_6 = Vector2.Zero;
					Vector2 arg_92_0 = this.scale;
					spriteBatch.Draw(arg_D7_1, arg_D7_2, arg_D7_3, arg_D7_4, arg_D7_5, arg_D7_6, (this.scale.Y > 1f) ? base.getScale().Y : ((float)Game1.pixelZoom), SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 10000f);
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom / 2), (float)(y * Game1.tileSize + Game1.tileSize / 4))), new Rectangle?(new Rectangle(88, 1779, 30, 30)), Color.PaleGoldenrod * (Game1.currentLocation.IsOutdoors ? 0.35f : 0.43f), 0f, new Vector2(15f, 15f), (float)Game1.pixelZoom + (float)((double)Game1.tileSize * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * Game1.tileSize * 777) + (double)(y * Game1.tileSize * 9746)) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
					sourceRectForObject.X = 276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 3204) + (double)(y * 49)) % 700.0 / 100.0) * 8;
					sourceRectForObject.Y = 1965;
					sourceRectForObject.Width = 8;
					sourceRectForObject.Height = 8;
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float)(y * Game1.tileSize + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(sourceRectForObject), Color.White * 0.75f, 0f, new Vector2(4f, 4f), (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom + 1) / 10000f);
					for (int i = 0; i < this.ashes.Length; i++)
					{
						spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2) + this.ashes[i].X, (float)(y * Game1.tileSize + Game1.tileSize / 2) + this.ashes[i].Y)), new Rectangle?(new Rectangle(344 + i % 3, 53, 1, 1)), Color.White * 0.5f * ((-100f - this.ashes[i].Y / 2f) / -100f), 0f, Vector2.Zero, (float)Game1.pixelZoom * 0.75f, SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 10000f);
					}
					return;
				}
				base.draw(spriteBatch, x, y, alpha);
				if (this.isOn)
				{
					if (this.parentSheetIndex == 146)
					{
						spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 4 - Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.pixelZoom * 2))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 3047) + (double)(y * 88)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 16) / 10000f);
						spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 3), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 2047) + (double)(y * 98)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 15) / 10000f);
						spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 5), (float)(y * Game1.tileSize + Game1.pixelZoom * 3))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 2077) + (double)(y * 98)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 14) / 10000f);
						return;
					}
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 4 - Game1.pixelZoom * 2), (float)(y * Game1.tileSize - Game1.tileSize + Game1.pixelZoom * 2))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 3047) + (double)(y * 88)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 16) / 10000f);
				}
			}
		}
	}
}
