using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley
{
	public class FarmerRenderer
	{
		public const int sleeveDarkestColorIndex = 256;

		public const int skinDarkestColorIndex = 260;

		public const int shoeDarkestColorIndex = 268;

		public const int eyeLightestColorIndex = 276;

		public const int accessoryDrawBelowHairThreshold = 8;

		public const int accessoryFacialHairThreshold = 6;

		public const int pantsOffset = 288;

		public const int armOffset = 96;

		public const int secondaryArmOffset = 192;

		public const int shirtXOffset = 16;

		public const int shirtYOffset = 56;

		public static int[] featureYOffsetPerFrame = new int[]
		{
			1,
			2,
			2,
			0,
			5,
			6,
			1,
			2,
			2,
			1,
			0,
			2,
			0,
			1,
			1,
			0,
			1,
			2,
			3,
			3,
			2,
			2,
			1,
			1,
			0,
			0,
			2,
			2,
			4,
			4,
			0,
			0,
			1,
			2,
			1,
			1,
			1,
			1,
			0,
			0,
			1,
			1,
			1,
			0,
			0,
			-2,
			-1,
			1,
			1,
			0,
			-1,
			-2,
			-1,
			-1,
			5,
			4,
			0,
			0,
			3,
			2,
			-1,
			0,
			4,
			2,
			0,
			0,
			2,
			1,
			0,
			-1,
			1,
			-2,
			0,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			1,
			-1,
			-1,
			-1,
			-1,
			1,
			1,
			0,
			0,
			0,
			0,
			4,
			1,
			0,
			1,
			2,
			1,
			0,
			1,
			0,
			1,
			2,
			-3,
			-4,
			-1,
			0,
			0,
			2,
			1,
			-4,
			-1,
			0,
			0,
			-3,
			0,
			0,
			-1,
			0,
			0,
			2,
			1,
			1
		};

		public static int[] featureXOffsetPerFrame = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			4,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		public static int[] hairstyleHatOffset = new int[]
		{
			0,
			0,
			0,
			4,
			0,
			0,
			3,
			0,
			4,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		public Texture2D baseTexture;

		public static Texture2D hairStylesTexture;

		public static Texture2D shirtsTexture;

		public static Texture2D hatsTexture;

		public static Texture2D accessoriesTexture;

		public int heightOffset;

		private Rectangle shirtSourceRect;

		private Rectangle hairstyleSourceRect;

		private Rectangle hatSourceRect;

		private Rectangle accessorySourceRect;

		private Vector2 rotationAdjustment;

		private Vector2 positionOffset;

		public FarmerRenderer(Texture2D baseTexture)
		{
			this.baseTexture = baseTexture;
		}

		public void recolorEyes(Color lightestColor)
		{
			Color[] array = new Color[23];
			this.baseTexture.GetData<Color>(0, new Rectangle?(new Rectangle(256, 0, 23, 1)), array, 0, 23);
			Color other = FarmerRenderer.changeBrightness(lightestColor, -75);
			if (lightestColor.Equals(other))
			{
				lightestColor.B += 10;
			}
			for (int i = 256; i < array.Length; i++)
			{
				if (lightestColor.Equals(array[i]))
				{
					lightestColor.G += 1;
				}
				if (other.Equals(array[i]))
				{
					other.G += 1;
				}
			}
			ColorChanger.swapColor(this.baseTexture, 276, (int)lightestColor.R, (int)lightestColor.G, (int)lightestColor.B);
			ColorChanger.swapColor(this.baseTexture, 277, (int)other.R, (int)other.G, (int)other.B);
		}

		public void recolorShoes(int which)
		{
			Texture2D texture2D = Game1.content.Load<Texture2D>("Characters\\Farmer\\shoeColors");
			Color[] array = new Color[texture2D.Width * texture2D.Height];
			texture2D.GetData<Color>(array);
			Color color = array[which * Game1.pixelZoom % (texture2D.Height * Game1.pixelZoom)];
			Color color2 = array[which * Game1.pixelZoom % (texture2D.Height * Game1.pixelZoom) + 1];
			Color color3 = array[which * Game1.pixelZoom % (texture2D.Height * Game1.pixelZoom) + 2];
			Color color4 = array[which * Game1.pixelZoom % (texture2D.Height * Game1.pixelZoom) + 3];
			ColorChanger.swapColor(this.baseTexture, 268, (int)color.R, (int)color.G, (int)color.B);
			ColorChanger.swapColor(this.baseTexture, 269, (int)color2.R, (int)color2.G, (int)color2.B);
			ColorChanger.swapColor(this.baseTexture, 270, (int)color3.R, (int)color3.G, (int)color3.B);
			ColorChanger.swapColor(this.baseTexture, 271, (int)color4.R, (int)color4.G, (int)color4.B);
		}

		public int recolorSkin(int which)
		{
			Texture2D texture2D = Game1.content.Load<Texture2D>("Characters\\Farmer\\skinColors");
			Color[] array = new Color[texture2D.Width * texture2D.Height];
			if (which < 0)
			{
				which = texture2D.Height - 1;
			}
			if (which > texture2D.Height - 1)
			{
				which = 0;
			}
			texture2D.GetData<Color>(array);
			Color color = array[which * 3 % (texture2D.Height * 3)];
			Color color2 = array[which * 3 % (texture2D.Height * 3) + 1];
			Color color3 = array[which * 3 % (texture2D.Height * 3) + 2];
			ColorChanger.swapColor(this.baseTexture, 260, (int)color.R, (int)color.G, (int)color.B);
			ColorChanger.swapColor(this.baseTexture, 261, (int)color2.R, (int)color2.G, (int)color2.B);
			ColorChanger.swapColor(this.baseTexture, 262, (int)color3.R, (int)color3.G, (int)color3.B);
			return which;
		}

		public void changeShirt(int whichShirt)
		{
			Color[] array = new Color[FarmerRenderer.shirtsTexture.Bounds.Width * FarmerRenderer.shirtsTexture.Bounds.Height];
			FarmerRenderer.shirtsTexture.GetData<Color>(array);
			int num = whichShirt * 8 / FarmerRenderer.shirtsTexture.Bounds.Width * 32 * 128 + whichShirt * 8 % FarmerRenderer.shirtsTexture.Bounds.Width + FarmerRenderer.shirtsTexture.Width * Game1.pixelZoom;
			Color color = array[num];
			ColorChanger.swapColor(this.baseTexture, 256, (int)color.R, (int)color.G, (int)color.B);
			color = array[num - FarmerRenderer.shirtsTexture.Width];
			ColorChanger.swapColor(this.baseTexture, 257, (int)color.R, (int)color.G, (int)color.B);
			color = array[num - FarmerRenderer.shirtsTexture.Width * 2];
			ColorChanger.swapColor(this.baseTexture, 258, (int)color.R, (int)color.G, (int)color.B);
		}

		public static Color changeBrightness(Color c, int brightness)
		{
			c.R = (byte)Math.Min(255, Math.Max(0, (int)c.R + brightness));
			c.G = (byte)Math.Min(255, Math.Max(0, (int)c.G + brightness));
			c.B = (byte)Math.Min(255, Math.Max(0, (int)c.B + ((brightness > 0) ? (brightness * 5 / 6) : (brightness * 8 / 7))));
			return c;
		}

		public void draw(SpriteBatch b, Farmer who, int whichFrame, Vector2 position, float layerDepth = 1f, bool flip = false)
		{
			who.FarmerSprite.setCurrentSingleFrame(whichFrame, 32000, false, flip);
			this.draw(b, who.FarmerSprite, who.FarmerSprite.SourceRect, position, Vector2.Zero, layerDepth, Color.White, 0f, who);
		}

		public void draw(SpriteBatch b, FarmerSprite farmerSprite, Rectangle sourceRect, Vector2 position, Vector2 origin, float layerDepth, Color overrideColor, float rotation, Farmer who)
		{
			this.draw(b, farmerSprite.CurrentAnimationFrame, farmerSprite.CurrentFrame, sourceRect, position, origin, layerDepth, overrideColor, rotation, 1f, who);
		}

		public void drawMiniPortrat(SpriteBatch b, Vector2 position, float layerDepth, float scale, int facingDirection, Farmer who)
		{
			facingDirection = 2;
			bool flag = false;
			int y = 0;
			position.Y += 0f;
			this.hairstyleSourceRect = Game1.getSquareSourceRectForNonStandardTileSheet(FarmerRenderer.hairStylesTexture, 16, 96, who.hair);
			this.hairstyleSourceRect.Height = 15;
			switch (facingDirection)
			{
			case 0:
				y = 64;
				this.hairstyleSourceRect.Offset(0, 64);
				break;
			case 1:
				y = 32;
				this.hairstyleSourceRect.Offset(0, 32);
				break;
			case 3:
				flag = true;
				y = 32;
				this.hairstyleSourceRect.Offset(0, 32);
				break;
			}
			b.Draw(this.baseTexture, position, new Rectangle?(new Rectangle(0, y, 16, who.isMale ? 15 : 16)), Color.White, 0f, Vector2.Zero, scale, flag ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
			b.Draw(FarmerRenderer.hairStylesTexture, position + new Vector2(0f, 4f), new Rectangle?(this.hairstyleSourceRect), who.hairstyleColor, 0f, Vector2.Zero, scale, flag ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + 1.1E-07f);
		}

		public void draw(SpriteBatch b, FarmerSprite.AnimationFrame animationFrame, int currentFrame, Rectangle sourceRect, Vector2 position, Vector2 origin, float layerDepth, Color overrideColor, float rotation, float scale, Farmer who)
		{
			this.draw(b, animationFrame, currentFrame, sourceRect, position, origin, layerDepth, who.facingDirection, overrideColor, rotation, scale, who);
		}

		public void drawHairAndAccesories(SpriteBatch b, int facingDirection, Farmer who, Vector2 position, Vector2 origin, float scale, int currentFrame, float rotation, Color overrideColor, float layerDepth)
		{
			this.shirtSourceRect = new Rectangle(who.shirt * 8 % FarmerRenderer.shirtsTexture.Width, who.shirt * 8 / FarmerRenderer.shirtsTexture.Width * 32, 8, 8);
			this.hairstyleSourceRect = new Rectangle(who.getHair() * 16 % FarmerRenderer.hairStylesTexture.Width, who.getHair() * 16 / FarmerRenderer.hairStylesTexture.Width * 96, 16, 32);
			if (who.accessory >= 0)
			{
				this.accessorySourceRect = new Rectangle(who.accessory * 16 % FarmerRenderer.accessoriesTexture.Width, who.accessory * 16 / FarmerRenderer.accessoriesTexture.Width * 32, 16, 16);
			}
			if (who.hat != null)
			{
				this.hatSourceRect = new Rectangle(20 * who.hat.which % FarmerRenderer.hatsTexture.Width, 20 * who.hat.which / FarmerRenderer.hatsTexture.Width * 20 * Game1.pixelZoom, 20, 20);
			}
			switch (facingDirection)
			{
			case 0:
				this.shirtSourceRect.Offset(0, 24);
				this.hairstyleSourceRect.Offset(0, 64);
				if (who.hat != null)
				{
					this.hatSourceRect.Offset(0, 60);
				}
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + new Vector2(16f * scale + (float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(56 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom) + (float)this.heightOffset * scale), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.8E-07f);
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + 4 + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 2.2E-07f);
				break;
			case 1:
				this.shirtSourceRect.Offset(0, 8);
				this.hairstyleSourceRect.Offset(0, 32);
				if (who.accessory >= 0)
				{
					this.accessorySourceRect.Offset(0, 16);
				}
				if (who.hat != null)
				{
					this.hatSourceRect.Offset(0, 20);
				}
				if (rotation == -0.09817477f)
				{
					this.rotationAdjustment.X = 6f;
					this.rotationAdjustment.Y = -2f;
				}
				else if (rotation == 0.09817477f)
				{
					this.rotationAdjustment.X = -6f;
					this.rotationAdjustment.Y = 1f;
				}
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2(16f * scale + (float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), 56f * scale + (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom) + (float)this.heightOffset * scale), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + 1.8E-07f);
				}
				if (who.accessory >= 0)
				{
					b.Draw(FarmerRenderer.accessoriesTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(4 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset)), new Rectangle?(this.accessorySourceRect), (overrideColor.Equals(Color.White) && who.accessory < 6) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + ((who.accessory < 8) ? 1.9E-05f : 2.9E-05f));
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 2.2E-07f);
				break;
			case 2:
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + new Vector2((float)(16 + FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(56 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom) + (float)this.heightOffset * scale - (float)(who.isMale ? 0 : 0)), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.5E-07f);
				}
				if (who.accessory >= 0)
				{
					b.Draw(FarmerRenderer.accessoriesTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(8 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset - 4)), new Rectangle?(this.accessorySourceRect), (overrideColor.Equals(Color.White) && who.accessory < 6) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + ((who.accessory < 8) ? 1.9E-05f : 2.9E-05f));
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 2.2E-05f);
				break;
			case 3:
				this.shirtSourceRect.Offset(0, 16);
				if (who.accessory >= 0)
				{
					this.accessorySourceRect.Offset(0, 16);
				}
				this.hairstyleSourceRect.Offset(0, 32);
				if (who.hat != null)
				{
					this.hatSourceRect.Offset(0, 40);
				}
				if (rotation == -0.09817477f)
				{
					this.rotationAdjustment.X = 6f;
					this.rotationAdjustment.Y = -2f;
				}
				else if (rotation == 0.09817477f)
				{
					this.rotationAdjustment.X = -5f;
					this.rotationAdjustment.Y = 1f;
				}
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(16 - FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(56 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset)), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + 1.5E-07f);
				}
				if (who.accessory >= 0)
				{
					b.Draw(FarmerRenderer.accessoriesTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(-(float)FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(4 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset)), new Rectangle?(this.accessorySourceRect), (overrideColor.Equals(Color.White) && who.accessory < 6) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.FlipHorizontally, layerDepth + ((who.accessory < 8) ? 1.9E-05f : 2.9E-05f));
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(-(float)FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.FlipHorizontally, layerDepth + 2.2E-05f);
				break;
			}
			if (who.hat != null && !who.bathingClothes)
			{
				bool flip = who.FarmerSprite.CurrentAnimationFrame.flip;
				b.Draw(FarmerRenderer.hatsTexture, position + origin + this.positionOffset + new Vector2((float)(-(float)Game1.pixelZoom * 2 + (flip ? -1 : 1) * FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(-(float)Game1.pixelZoom * 4 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + (who.hat.ignoreHairstyleOffset ? 0 : FarmerRenderer.hairstyleHatOffset[who.hair % 16]) + 4 + this.heightOffset)), new Rectangle?(this.hatSourceRect), Color.White, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 3.9E-05f);
			}
		}

		public void draw(SpriteBatch b, FarmerSprite.AnimationFrame animationFrame, int currentFrame, Rectangle sourceRect, Vector2 position, Vector2 origin, float layerDepth, int facingDirection, Color overrideColor, float rotation, float scale, Farmer who)
		{
			position = new Vector2((float)Math.Floor((double)position.X), (float)Math.Floor((double)position.Y));
			this.rotationAdjustment = Vector2.Zero;
			this.positionOffset.Y = (float)(animationFrame.positionOffset * Game1.pixelZoom);
			this.positionOffset.X = (float)(animationFrame.xOffset * Game1.pixelZoom);
			if (who.swimming)
			{
				sourceRect.Height /= 2;
				sourceRect.Height -= (int)who.yOffset / Game1.pixelZoom;
				position.Y += (float)Game1.tileSize;
			}
			b.Draw(this.baseTexture, position + origin + this.positionOffset, new Rectangle?(sourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, animationFrame.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
			if (who.swimming)
			{
				if (who.currentEyes != 0 && who.facingDirection != 0 && Game1.timeOfDay < 2600 && (!who.FarmerSprite.pauseForSingleAnimation || (who.usingTool && who.CurrentTool is FishingRod)))
				{
					b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((who.FacingDirection == 1) ? (3 * Game1.pixelZoom) : ((who.FacingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + (who.IsMale ? (9 * Game1.pixelZoom) : (10 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(5, 16, (who.FacingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 5E-08f);
					b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((who.FacingDirection == 1) ? (3 * Game1.pixelZoom) : ((who.FacingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + (who.IsMale ? (9 * Game1.pixelZoom) : (10 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(264 + ((who.FacingDirection == 3) ? 4 : 0), 2 + (who.currentEyes - 1) * 2, (who.FacingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.2E-07f);
				}
				this.drawHairAndAccesories(b, facingDirection, who, position, origin, scale, currentFrame, rotation, overrideColor, layerDepth);
				b.Draw(Game1.staminaRect, new Rectangle((int)position.X + (int)who.yOffset + Game1.pixelZoom * 2, (int)position.Y - 32 * Game1.pixelZoom + sourceRect.Height * Game1.pixelZoom + (int)origin.Y - (int)who.yOffset, sourceRect.Width * Game1.pixelZoom - (int)who.yOffset * 2 - Game1.pixelZoom * Game1.pixelZoom, Game1.pixelZoom), new Rectangle?(Game1.staminaRect.Bounds), Color.White * 0.75f, 0f, Vector2.Zero, SpriteEffects.None, layerDepth + 0.001f);
				return;
			}
			sourceRect.Offset(288, 0);
			b.Draw(this.baseTexture, position + origin + this.positionOffset, new Rectangle?(sourceRect), overrideColor.Equals(Color.White) ? who.pantsColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, animationFrame.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + ((who.FarmerSprite.CurrentAnimationFrame.frame == 5) ? 0.00092f : 9.2E-08f));
			if (who.currentEyes != 0 && facingDirection != 0 && !who.isRidingHorse() && Game1.timeOfDay < 2600 && (!who.FarmerSprite.pauseForSingleAnimation || (who.usingTool && who.CurrentTool is FishingRod)))
			{
				b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((facingDirection == 1) ? (3 * Game1.pixelZoom) : ((facingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.IsMale && who.facingDirection != 2) ? (9 * Game1.pixelZoom) : (10 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(5, 16, (facingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 5E-08f);
				b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((facingDirection == 1) ? (3 * Game1.pixelZoom) : ((facingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.facingDirection == 1 || who.facingDirection == 3) ? (10 * Game1.pixelZoom) : (11 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(264 + ((facingDirection == 3) ? 4 : 0), 2 + (who.currentEyes - 1) * 2, (facingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.2E-07f);
			}
			this.drawHairAndAccesories(b, facingDirection, who, position, origin, scale, currentFrame, rotation, overrideColor, layerDepth);
			sourceRect.Offset(-288 + (animationFrame.secondaryArm ? 192 : 96), 0);
			b.Draw(this.baseTexture, position + origin + this.positionOffset + who.armOffset, new Rectangle?(sourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, animationFrame.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 4.9E-05f : 0f));
			if (who.usingSlingshot)
			{
				int num = Game1.getOldMouseX() + Game1.viewport.X;
				int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
				if ((who.CurrentTool as Slingshot).didStartWithGamePad())
				{
					Point expr_770 = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					num = expr_770.X;
					num2 = expr_770.Y;
				}
				int num3 = Math.Min(20, (int)Vector2.Distance(who.getStandingPosition(), new Vector2((float)num, (float)num2)) / 20);
				float num4 = (float)Math.Atan2((double)((float)num2 - who.getStandingPosition().Y - (float)Game1.tileSize), (double)((float)num - who.getStandingPosition().X)) + 3.14159274f;
				switch (facingDirection)
				{
				case 0:
					b.Draw(this.baseTexture, position + new Vector2(4f + num4 * 8f, (float)(-(float)Game1.tileSize * 3 / 4 + 4)), new Rectangle?(new Rectangle(173, 238, 9, 14)), Color.White, 0f, new Vector2(4f, 11f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : -0.0005f));
					return;
				case 1:
				{
					b.Draw(this.baseTexture, position + new Vector2((float)(52 - num3), (float)(-(float)Game1.tileSize / 2)), new Rectangle?(new Rectangle(147, 237, 10, 4)), Color.White, 0f, new Vector2(8f, 3f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					b.Draw(this.baseTexture, position + new Vector2(36f, (float)(-(float)Game1.tileSize / 2 - 12)), new Rectangle?(new Rectangle(156, 244, 9, 10)), Color.White, num4, new Vector2(0f, 3f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 1E-08f : 0f));
					int num5 = (int)(Math.Cos((double)(num4 + 1.57079637f)) * (double)(20 - num3 - 8) - Math.Sin((double)(num4 + 1.57079637f)) * -68.0);
					int num6 = (int)(Math.Sin((double)(num4 + 1.57079637f)) * (double)(20 - num3 - 8) + Math.Cos((double)(num4 + 1.57079637f)) * -68.0);
					Utility.drawLineWithScreenCoordinates((int)(position.X + 52f - (float)num3), (int)(position.Y - (float)(Game1.tileSize / 2) - 4f), (int)(position.X + 32f + (float)(num5 / 2)), (int)(position.Y - (float)(Game1.tileSize / 2) - 12f + (float)(num6 / 2)), b, Color.White, 1f);
					return;
				}
				case 2:
					b.Draw(this.baseTexture, position + new Vector2(4f, (float)(-(float)Game1.tileSize / 2 - num3 / 2)), new Rectangle?(new Rectangle(148, 244, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					Utility.drawLineWithScreenCoordinates((int)(position.X + 16f), (int)(position.Y - 28f - (float)(num3 / 2)), (int)(position.X + 44f - num4 * 10f), (int)(position.Y - (float)(Game1.tileSize / 4) - 8f), b, Color.White, 1f);
					Utility.drawLineWithScreenCoordinates((int)(position.X + 16f), (int)(position.Y - 28f - (float)(num3 / 2)), (int)(position.X + 56f - num4 * 10f), (int)(position.Y - (float)(Game1.tileSize / 4) - 8f), b, Color.White, 1f);
					b.Draw(this.baseTexture, position + new Vector2(44f - num4 * 10f, (float)(-(float)Game1.tileSize / 4)), new Rectangle?(new Rectangle(167, 235, 7, 9)), Color.White, 0f, new Vector2(3f, 5f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					break;
				case 3:
				{
					b.Draw(this.baseTexture, position + new Vector2((float)(40 + num3), (float)(-(float)Game1.tileSize / 2)), new Rectangle?(new Rectangle(147, 237, 10, 4)), Color.White, 0f, new Vector2(9f, 4f), (float)Game1.pixelZoom * scale, SpriteEffects.FlipHorizontally, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					b.Draw(this.baseTexture, position + new Vector2(24f, (float)(-(float)Game1.tileSize / 2 - 8)), new Rectangle?(new Rectangle(156, 244, 9, 10)), Color.White, num4 + 3.14159274f, new Vector2(8f, 3f), (float)Game1.pixelZoom * scale, SpriteEffects.FlipHorizontally, layerDepth + ((facingDirection != 0) ? 1E-08f : 0f));
					int num5 = (int)(Math.Cos((double)(num4 + 1.2566371f)) * (double)(20 + num3 - 8) - Math.Sin((double)(num4 + 1.2566371f)) * -68.0);
					int num6 = (int)(Math.Sin((double)(num4 + 1.2566371f)) * (double)(20 + num3 - 8) + Math.Cos((double)(num4 + 1.2566371f)) * -68.0);
					Utility.drawLineWithScreenCoordinates((int)(position.X + 4f + (float)num3), (int)(position.Y - (float)(Game1.tileSize / 2) - 8f), (int)(position.X + 26f + (float)num5 * 4f / 10f), (int)(position.Y - (float)(Game1.tileSize / 2) - 8f + (float)num6 * 4f / 10f), b, Color.White, 1f);
					return;
				}
				default:
					return;
				}
			}
		}
	}
}
