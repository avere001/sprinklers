using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Objects
{
	public class ColoredObject : StardewValley.Object
	{
		public Color color;

		public ColoredObject()
		{
		}

		public ColoredObject(int parentSheetIndex, int stack, Color color) : base(parentSheetIndex, stack, false, -1, 0)
		{
			this.color = color;
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			if (this.isRecipe)
			{
				transparency = 0.5f;
				scaleSize *= 0.75f;
			}
			if (this.bigCraftable)
			{
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.parentSheetIndex)), Color.White * transparency, 0f, new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize) * scaleSize, (scaleSize < 0.2f) ? scaleSize : (scaleSize / 2f), SpriteEffects.None, layerDepth);
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.parentSheetIndex + 1)), this.color * transparency, 0f, new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize) * scaleSize, (scaleSize < 0.2f) ? scaleSize : (scaleSize / 2f), SpriteEffects.None, layerDepth + 2E-05f);
			}
			else
			{
				spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f) * scaleSize, (float)Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth);
				spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex + 1, 16, 16)), this.color * transparency, 0f, new Vector2(8f, 8f) * scaleSize, (float)Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth + 2E-05f);
				if (drawStackNumber && this.maximumStackSize() > 1 && (double)scaleSize > 0.3 && this.Stack != 2147483647 && this.Stack > 1)
				{
					Utility.drawTinyDigits(this.stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.stack, 3f * scaleSize)) + 3f * scaleSize, (float)Game1.tileSize - 18f * scaleSize + 2f), 3f * scaleSize, 1f, Color.White);
				}
				if (drawStackNumber && this.quality > 0)
				{
					float num = (this.quality < 2) ? 0f : (((float)Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) + 1f) * 0.05f);
					spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float)(Game1.tileSize - 12) + num), new Rectangle?(new Rectangle(338 + (this.quality - 1) * 8, 400, 8, 8)), Color.White * transparency, 0f, new Vector2(4f, 4f), 3f * scaleSize * (1f + num), SpriteEffects.None, layerDepth);
				}
			}
			if (this.isRecipe)
			{
				spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 451, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom * 3f / 4f, SpriteEffects.None, layerDepth + 0.0001f);
			}
		}

		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			base.drawWhenHeld(spriteBatch, objectPosition, f);
			spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition, new Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex + 1)), this.color, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 3) / 10000f));
		}

		public override Item getOne()
		{
			return new ColoredObject(this.parentSheetIndex, 1, this.color)
			{
				quality = this.quality,
				price = this.price,
				hasBeenInInventory = this.hasBeenInInventory,
				hasBeenPickedUpByFarmer = this.hasBeenPickedUpByFarmer,
				specialVariable = this.specialVariable
			};
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			if (this.bigCraftable)
			{
				Vector2 scale = base.getScale();
				Vector2 vector = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
				Rectangle destinationRectangle = new Rectangle((int)(vector.X - scale.X / 2f), (int)(vector.Y - scale.Y / 2f), (int)((float)Game1.tileSize + scale.X), (int)((float)(Game1.tileSize * 2) + scale.Y / 2f));
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.showNextIndex ? (base.ParentSheetIndex + 1) : base.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 1) / 10000f));
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(base.ParentSheetIndex + 1)), this.color, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 1) / 10000f));
				if (this.Name.Equals("Loom") && this.minutesUntilReady > 0)
				{
					spriteBatch.Draw(Game1.objectSpriteSheet, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), 0f), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, 16, 16)), Color.White, this.scale.X, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 1) / 10000f));
				}
			}
			else if (!Game1.eventUp || Game1.currentLocation.IsFarm)
			{
				if (this.parentSheetIndex != 590)
				{
					spriteBatch.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 1E-07f);
				}
				Texture2D arg_34C_1 = Game1.objectSpriteSheet;
				Vector2 arg_34C_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize + Game1.tileSize / 2)));
				Rectangle? arg_34C_3 = new Rectangle?(Game1.currentLocation.getSourceRectForObject(base.ParentSheetIndex));
				Color arg_34C_4 = Color.White;
				float arg_34C_5 = 0f;
				Vector2 arg_34C_6 = new Vector2(8f, 8f);
				Vector2 arg_2FC_0 = this.scale;
				spriteBatch.Draw(arg_34C_1, arg_34C_2, arg_34C_3, arg_34C_4, arg_34C_5, arg_34C_6, (this.scale.Y > 1f) ? base.getScale().Y : ((float)Game1.pixelZoom), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 10000f);
				Texture2D arg_40D_1 = Game1.objectSpriteSheet;
				Vector2 arg_40D_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize + Game1.tileSize / 2)));
				Rectangle? arg_40D_3 = new Rectangle?(Game1.currentLocation.getSourceRectForObject(base.ParentSheetIndex + 1));
				Color arg_40D_4 = this.color;
				float arg_40D_5 = 0f;
				Vector2 arg_40D_6 = new Vector2(8f, 8f);
				Vector2 arg_3BD_0 = this.scale;
				spriteBatch.Draw(arg_40D_1, arg_40D_2, arg_40D_3, arg_40D_4, arg_40D_5, arg_40D_6, (this.scale.Y > 1f) ? base.getScale().Y : ((float)Game1.pixelZoom), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 10000f);
			}
			if (this.Name != null && this.Name.Contains("Table") && this.heldObject != null)
			{
				spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - (this.bigCraftable ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 3))))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, 1f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize + 1) / 10000f);
			}
		}
	}
}
