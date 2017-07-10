using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class TemporaryAnimatedSprite
	{
		public delegate void endBehavior(int extraInfo);

		public float timer;

		public float interval = 200f;

		public int currentParentTileIndex;

		public int oldCurrentParentTileIndex;

		public int initialParentTileIndex;

		public int totalNumberOfLoops;

		public int currentNumberOfLoops;

		public int xStopCoordinate = -1;

		public int yStopCoordinate = -1;

		public int animationLength;

		public int bombRadius;

		public int pingPongMotion = 1;

		public bool flicker;

		public bool timeBasedMotion;

		public bool overrideLocationDestroy;

		public bool pingPong;

		public bool holdLastFrame;

		public bool pulse;

		public int extraInfoForEndBehavior;

		public int lightID;

		public bool bigCraftable;

		public bool swordswipe;

		public bool flash;

		public bool flipped;

		public bool verticalFlipped;

		public bool local;

		public bool light;

		public bool hasLit;

		public bool xPeriodic;

		public bool yPeriodic;

		public bool destroyable = true;

		public bool paused;

		public float rotation;

		public float alpha = 1f;

		public float alphaFade;

		public float layerDepth = -1f;

		public float scale = 1f;

		public float scaleChange;

		public float scaleChangeChange;

		public float rotationChange;

		public float id;

		public float lightRadius;

		public float xPeriodicRange;

		public float yPeriodicRange;

		public float xPeriodicLoopTime;

		public float yPeriodicLoopTime;

		public float shakeIntensityChange;

		public float shakeIntensity;

		public float pulseTime;

		public float pulseAmount = 1.1f;

		public Vector2 position;

		public Vector2 sourceRectStartingPos;

		protected GameLocation parent;

		private Texture2D texture;

		public Rectangle sourceRect;

		public Color color = Color.White;

		public Color lightcolor = Color.White;

		protected Farmer owner;

		public Vector2 motion = Vector2.Zero;

		public Vector2 acceleration = Vector2.Zero;

		public Vector2 accelerationChange = Vector2.Zero;

		public Vector2 initialPosition;

		public int delayBeforeAnimationStart;

		public string startSound;

		public string endSound;

		public string text;

		public TemporaryAnimatedSprite.endBehavior endFunction;

		public TemporaryAnimatedSprite.endBehavior reachedStopCoordinate;

		public TemporaryAnimatedSprite parentSprite;

		public Character attachedCharacter;

		private float pulseTimer;

		private float originalScale;

		private float totalTimer;

		public Vector2 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
			set
			{
				this.texture = value;
			}
		}

		public TemporaryAnimatedSprite getClone()
		{
			return new TemporaryAnimatedSprite
			{
				texture = this.texture,
				interval = this.interval,
				currentParentTileIndex = this.currentParentTileIndex,
				oldCurrentParentTileIndex = this.oldCurrentParentTileIndex,
				initialParentTileIndex = this.initialParentTileIndex,
				totalNumberOfLoops = this.totalNumberOfLoops,
				currentNumberOfLoops = this.currentNumberOfLoops,
				xStopCoordinate = this.xStopCoordinate,
				yStopCoordinate = this.yStopCoordinate,
				animationLength = this.animationLength,
				bombRadius = this.bombRadius,
				pingPongMotion = this.pingPongMotion,
				flicker = this.flicker,
				timeBasedMotion = this.timeBasedMotion,
				overrideLocationDestroy = this.overrideLocationDestroy,
				pingPong = this.pingPong,
				holdLastFrame = this.holdLastFrame,
				extraInfoForEndBehavior = this.extraInfoForEndBehavior,
				lightID = this.lightID,
				acceleration = this.acceleration,
				accelerationChange = this.accelerationChange,
				alpha = this.alpha,
				alphaFade = this.alphaFade,
				attachedCharacter = this.attachedCharacter,
				bigCraftable = this.bigCraftable,
				color = this.color,
				delayBeforeAnimationStart = this.delayBeforeAnimationStart,
				destroyable = this.destroyable,
				endFunction = this.endFunction,
				endSound = this.endSound,
				flash = this.flash,
				flipped = this.flipped,
				hasLit = this.hasLit,
				id = this.id,
				initialPosition = this.initialPosition,
				light = this.light,
				local = this.local,
				motion = this.motion,
				owner = this.owner,
				parent = this.parent,
				parentSprite = this.parentSprite,
				position = this.position,
				rotation = this.rotation,
				rotationChange = this.rotationChange,
				scale = this.scale,
				scaleChange = this.scaleChange,
				scaleChangeChange = this.scaleChangeChange,
				shakeIntensity = this.shakeIntensity,
				shakeIntensityChange = this.shakeIntensityChange,
				sourceRect = this.sourceRect,
				sourceRectStartingPos = this.sourceRectStartingPos,
				startSound = this.startSound,
				timeBasedMotion = this.timeBasedMotion,
				verticalFlipped = this.verticalFlipped,
				xPeriodic = this.xPeriodic,
				xPeriodicLoopTime = this.xPeriodicLoopTime,
				xPeriodicRange = this.xPeriodicRange,
				yPeriodic = this.yPeriodic,
				yPeriodicLoopTime = this.yPeriodicLoopTime,
				yPeriodicRange = this.yPeriodicRange,
				yStopCoordinate = this.yStopCoordinate,
				totalNumberOfLoops = this.totalNumberOfLoops
			};
		}

		public static void addMoneyToFarmerEndBehavior(int extraInfo)
		{
			Game1.player.money += extraInfo;
		}

		public TemporaryAnimatedSprite()
		{
		}

		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped)
		{
			if (initialParentTileIndex == -1)
			{
				this.swordswipe = true;
				this.currentParentTileIndex = 0;
			}
			else
			{
				this.currentParentTileIndex = initialParentTileIndex;
			}
			this.initialParentTileIndex = initialParentTileIndex;
			this.interval = animationInterval;
			this.totalNumberOfLoops = numberOfLoops;
			this.position = position;
			this.animationLength = animationLength;
			this.flicker = flicker;
			this.flipped = flipped;
		}

		public TemporaryAnimatedSprite(int rowInAnimationTexture, Vector2 position, Color color, int animationLength = 8, bool flipped = false, float animationInterval = 100f, int numberOfLoops = 0, int sourceRectWidth = -1, float layerDepth = -1f, int sourceRectHeight = -1, int delay = 0) : this(Game1.animations, new Rectangle(0, rowInAnimationTexture * Game1.tileSize, sourceRectWidth, sourceRectHeight), animationInterval, animationLength, numberOfLoops, position, false, flipped, layerDepth, 0f, color, 1f, 0f, 0f, 0f, false)
		{
			if (sourceRectWidth == -1)
			{
				sourceRectWidth = Game1.tileSize;
				this.sourceRect.Width = Game1.tileSize;
			}
			if (sourceRectHeight == -1)
			{
				sourceRectHeight = Game1.tileSize;
				this.sourceRect.Height = Game1.tileSize;
			}
			if (layerDepth == -1f)
			{
				layerDepth = (position.Y + (float)(Game1.tileSize / 2)) / 10000f;
			}
			this.delayBeforeAnimationStart = delay;
		}

		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, bool verticalFlipped, float rotation) : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.rotation = rotation;
			this.verticalFlipped = verticalFlipped;
		}

		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool bigCraftable, bool flipped) : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.bigCraftable = bigCraftable;
			if (bigCraftable)
			{
				this.position.Y = this.position.Y - (float)Game1.tileSize;
			}
		}

		public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped) : this(0, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.Texture = texture;
			this.sourceRect = sourceRect;
			this.sourceRectStartingPos = new Vector2((float)sourceRect.X, (float)sourceRect.Y);
			this.initialPosition = position;
		}

		public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, float layerDepth, float alphaFade, Color color, float scale, float scaleChange, float rotation, float rotationChange, bool local = false) : this(0, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.Texture = texture;
			this.sourceRect = sourceRect;
			this.sourceRectStartingPos = new Vector2((float)sourceRect.X, (float)sourceRect.Y);
			this.layerDepth = layerDepth;
			this.alphaFade = Math.Max(0f, alphaFade);
			this.color = color;
			this.scale = scale;
			this.scaleChange = scaleChange;
			this.rotation = rotation;
			this.rotationChange = rotationChange;
			this.local = local;
			this.initialPosition = position;
		}

		public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, Vector2 position, bool flipped, float alphaFade, Color color) : this(0, 999999f, 1, 0, position, false, flipped)
		{
			this.Texture = texture;
			this.sourceRect = sourceRect;
			this.sourceRectStartingPos = new Vector2((float)sourceRect.X, (float)sourceRect.Y);
			this.initialPosition = position;
			this.alphaFade = Math.Max(0f, alphaFade);
			this.color = color;
		}

		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, GameLocation parent, Farmer owner) : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.position.X = (float)((int)this.position.X);
			this.position.Y = (float)((int)this.position.Y);
			this.parent = parent;
			switch (initialParentTileIndex)
			{
			case 286:
				this.bombRadius = 3;
				break;
			case 287:
				this.bombRadius = 5;
				break;
			case 288:
				this.bombRadius = 7;
				break;
			}
			this.owner = owner;
		}

		public virtual void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
		{
			if (this.local)
			{
				localPosition = true;
			}
			if (this.currentParentTileIndex >= 0 && this.delayBeforeAnimationStart <= 0)
			{
				if (this.text != null)
				{
					spriteBatch.DrawString(Game1.dialogueFont, this.text, localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, this.Position), this.color * this.alpha, this.rotation, Vector2.Zero, this.scale, SpriteEffects.None, this.layerDepth);
					return;
				}
				if (this.Texture != null)
				{
					spriteBatch.Draw(this.Texture, (localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((int)this.Position.X + xOffset), (float)((int)this.Position.Y + yOffset)))) + new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)) * this.scale + new Vector2((float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0), (float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0)), new Rectangle?(this.sourceRect), this.color * this.alpha, this.rotation, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, this.flipped ? SpriteEffects.FlipHorizontally : (this.verticalFlipped ? SpriteEffects.FlipVertically : SpriteEffects.None), (this.layerDepth >= 0f) ? this.layerDepth : ((this.Position.Y + (float)this.sourceRect.Height) / 10000f));
					return;
				}
				if (this.bigCraftable)
				{
					spriteBatch.Draw(Game1.bigCraftableSpriteSheet, localPosition ? this.Position : (Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((int)this.Position.X + xOffset), (float)((int)this.Position.Y + yOffset))) + new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2))), new Rectangle?(Object.getSourceRectForBigCraftable(this.currentParentTileIndex)), Color.White, 0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, SpriteEffects.None, (this.Position.Y + (float)(Game1.tileSize / 2)) / 10000f);
					return;
				}
				if (!this.swordswipe)
				{
					spriteBatch.Draw(Game1.objectSpriteSheet, localPosition ? this.Position : (Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((int)this.Position.X + xOffset), (float)((int)this.Position.Y + yOffset))) + new Vector2(8f, 8f) * (float)Game1.pixelZoom + new Vector2((float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0), (float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.currentParentTileIndex)), (this.flash ? (Color.LightBlue * 0.85f) : Color.White) * this.alpha, this.rotation, new Vector2(8f, 8f), (float)Game1.pixelZoom * this.scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.layerDepth >= 0f) ? this.layerDepth : ((this.Position.Y + (float)(Game1.tileSize / 2)) / 10000f));
				}
			}
		}

		public void unload()
		{
			if (this.endSound != null)
			{
				Game1.playSound(this.endSound);
			}
			if (this.endFunction != null)
			{
				this.endFunction(this.extraInfoForEndBehavior);
			}
			if (this.hasLit)
			{
				Utility.removeLightSource(this.lightID);
			}
		}

		public void reset()
		{
			this.sourceRect.X = (int)this.sourceRectStartingPos.X;
			this.sourceRect.Y = (int)this.sourceRectStartingPos.Y;
			this.currentParentTileIndex = 0;
			this.oldCurrentParentTileIndex = 0;
			this.timer = 0f;
			this.totalTimer = 0f;
			this.currentNumberOfLoops = 0;
			this.pingPongMotion = 1;
		}

		public virtual bool update(GameTime time)
		{
			if (this.paused)
			{
				return false;
			}
			if (this.bombRadius > 0 && Game1.activeClickableMenu != null)
			{
				return false;
			}
			if (this.delayBeforeAnimationStart > 0)
			{
				this.delayBeforeAnimationStart -= time.ElapsedGameTime.Milliseconds;
				if (this.delayBeforeAnimationStart <= 0 && this.startSound != null)
				{
					Game1.playSound(this.startSound);
				}
				if (this.delayBeforeAnimationStart <= 0 && this.parentSprite != null)
				{
					this.position = this.parentSprite.position + this.position;
				}
				return false;
			}
			this.timer += (float)time.ElapsedGameTime.Milliseconds;
			this.totalTimer += (float)time.ElapsedGameTime.Milliseconds;
			this.alpha -= this.alphaFade * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			if (this.alphaFade > 0f && this.light && this.alpha < 1f && this.alpha >= 0f)
			{
				try
				{
					Utility.getLightSource(this.lightID).color.A = (byte)(255f * this.alpha);
				}
				catch (Exception)
				{
				}
			}
			this.shakeIntensity += this.shakeIntensityChange * (float)time.ElapsedGameTime.Milliseconds;
			this.scale += this.scaleChange * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.scaleChange += this.scaleChangeChange * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.rotation += this.rotationChange;
			if (this.xPeriodic)
			{
				this.position.X = this.initialPosition.X + this.xPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.xPeriodicLoopTime * (double)this.totalTimer);
			}
			else
			{
				this.position.X = this.position.X + this.motion.X * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			}
			if (this.yPeriodic)
			{
				this.position.Y = this.initialPosition.Y + this.yPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.yPeriodicLoopTime * (double)(this.totalTimer + this.yPeriodicLoopTime / 2f));
			}
			else
			{
				this.position.Y = this.position.Y + this.motion.Y * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			}
			if (this.attachedCharacter != null)
			{
				if (this.xPeriodic)
				{
					this.attachedCharacter.position.X = this.initialPosition.X + this.xPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.xPeriodicLoopTime * (double)this.totalTimer);
				}
				else
				{
					Character expr_348_cp_0_cp_0 = this.attachedCharacter;
					expr_348_cp_0_cp_0.position.X = expr_348_cp_0_cp_0.position.X + this.motion.X * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
				}
				if (this.yPeriodic)
				{
					this.attachedCharacter.position.Y = this.initialPosition.Y + this.yPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.yPeriodicLoopTime * (double)this.totalTimer);
				}
				else
				{
					Character expr_3CF_cp_0_cp_0 = this.attachedCharacter;
					expr_3CF_cp_0_cp_0.position.Y = expr_3CF_cp_0_cp_0.position.Y + this.motion.Y * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
				}
			}
			this.motion.X = this.motion.X + this.acceleration.X * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.motion.Y = this.motion.Y + this.acceleration.Y * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.acceleration.X = this.acceleration.X + this.accelerationChange.X;
			this.acceleration.Y = this.acceleration.Y + this.accelerationChange.Y;
			if (this.xStopCoordinate != -1 || this.yStopCoordinate != -1)
			{
				if (this.xStopCoordinate != -1 && Math.Abs(this.position.X - (float)this.xStopCoordinate) <= Math.Abs(this.motion.X))
				{
					this.motion.X = 0f;
					this.acceleration.X = 0f;
					this.xStopCoordinate = -1;
				}
				if (this.yStopCoordinate != -1 && Math.Abs(this.position.Y - (float)this.yStopCoordinate) <= Math.Abs(this.motion.Y))
				{
					this.motion.Y = 0f;
					this.acceleration.Y = 0f;
					this.yStopCoordinate = -1;
				}
				if (this.xStopCoordinate == -1 && this.yStopCoordinate == -1)
				{
					this.rotationChange = 0f;
					if (this.reachedStopCoordinate != null)
					{
						this.reachedStopCoordinate(0);
					}
				}
			}
			if (!this.pingPong)
			{
				this.pingPongMotion = 1;
			}
			if (this.pulse)
			{
				this.pulseTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.originalScale == 0f)
				{
					this.originalScale = this.scale;
				}
				if (this.pulseTimer <= 0f)
				{
					this.pulseTimer = this.pulseTime;
					this.scale = this.originalScale * this.pulseAmount;
				}
				if (this.scale > this.originalScale)
				{
					this.scale -= this.pulseAmount / 100f * (float)time.ElapsedGameTime.Milliseconds;
				}
			}
			if (this.light)
			{
				if (!this.hasLit)
				{
					this.hasLit = true;
					this.lightID = Game1.random.Next(-2147483648, 2147483647);
					Game1.currentLightSources.Add(new LightSource(4, this.position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.lightRadius, this.lightcolor.Equals(Color.White) ? new Color(0, 131, 255) : this.lightcolor, this.lightID));
				}
				else
				{
					Utility.repositionLightSource(this.lightID, this.position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
				}
			}
			if (this.alpha <= 0f || (this.position.X < -2000f && !this.overrideLocationDestroy) || this.scale <= 0f)
			{
				this.unload();
				return this.destroyable;
			}
			if (this.timer > this.interval)
			{
				this.currentParentTileIndex += this.pingPongMotion;
				this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width * this.pingPongMotion;
				if (this.Texture != null)
				{
					if (!this.pingPong && this.sourceRect.X >= this.Texture.Width)
					{
						this.sourceRect.Y = this.sourceRect.Y + this.sourceRect.Height;
					}
					if (!this.pingPong)
					{
						this.sourceRect.X = this.sourceRect.X % this.Texture.Width;
					}
					if (this.pingPong)
					{
						if ((float)this.sourceRect.X + ((float)this.sourceRect.Y - this.sourceRectStartingPos.Y) / (float)this.sourceRect.Height * (float)this.Texture.Width >= this.sourceRectStartingPos.X + (float)(this.sourceRect.Width * this.animationLength))
						{
							this.pingPongMotion = -1;
							this.sourceRect.X = this.sourceRect.X - this.sourceRect.Width * 2;
							this.currentParentTileIndex--;
							if (this.sourceRect.X < 0)
							{
								this.sourceRect.X = this.Texture.Width + this.sourceRect.X;
							}
						}
						else if ((float)this.sourceRect.X < this.sourceRectStartingPos.X && (float)this.sourceRect.Y == this.sourceRectStartingPos.Y)
						{
							this.pingPongMotion = 1;
							this.sourceRect.X = (int)this.sourceRectStartingPos.X + this.sourceRect.Width;
							this.currentParentTileIndex++;
							this.currentNumberOfLoops++;
							if (this.endFunction != null)
							{
								this.endFunction(this.extraInfoForEndBehavior);
								this.endFunction = null;
							}
							if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
							{
								this.unload();
								return this.destroyable;
							}
						}
					}
					else if (this.totalNumberOfLoops >= 1 && (float)this.sourceRect.X + ((float)this.sourceRect.Y - this.sourceRectStartingPos.Y) / (float)this.sourceRect.Height * (float)this.Texture.Width >= this.sourceRectStartingPos.X + (float)(this.sourceRect.Width * this.animationLength))
					{
						this.sourceRect.X = (int)this.sourceRectStartingPos.X;
						this.sourceRect.Y = (int)this.sourceRectStartingPos.Y;
					}
				}
				this.timer = 0f;
				if (this.flicker)
				{
					if (this.currentParentTileIndex < 0 || this.flash)
					{
						this.currentParentTileIndex = this.oldCurrentParentTileIndex;
						this.flash = false;
					}
					else
					{
						this.oldCurrentParentTileIndex = this.currentParentTileIndex;
						if (this.bombRadius > 0)
						{
							this.flash = true;
						}
						else
						{
							this.currentParentTileIndex = -100;
						}
					}
				}
				if (this.currentParentTileIndex - this.initialParentTileIndex >= this.animationLength)
				{
					this.currentNumberOfLoops++;
					if (this.holdLastFrame)
					{
						this.currentParentTileIndex = this.initialParentTileIndex + this.animationLength - 1;
						this.setSourceRectToCurrentTileIndex();
						if (this.endFunction != null)
						{
							this.endFunction(this.extraInfoForEndBehavior);
							this.endFunction = null;
						}
						return false;
					}
					this.currentParentTileIndex = this.initialParentTileIndex;
					if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
					{
						if (this.bombRadius > 0)
						{
							if (Game1.fuseSound != null)
							{
								Game1.fuseSound.Stop(AudioStopOptions.AsAuthored);
								Game1.fuseSound = Game1.soundBank.GetCue("fuse");
							}
							Game1.playSound("explosion");
							Game1.flashAlpha = 1f;
							this.parent.explode(new Vector2((float)((int)(this.position.X / (float)Game1.tileSize)), (float)((int)(this.position.Y / (float)Game1.tileSize))), this.bombRadius, this.owner);
						}
						this.unload();
						return this.destroyable;
					}
					if (this.bombRadius > 0 && this.currentNumberOfLoops == this.totalNumberOfLoops - 5)
					{
						this.interval -= this.interval / 3f;
					}
				}
			}
			return false;
		}

		private void setSourceRectToCurrentTileIndex()
		{
			this.sourceRect.X = (int)(this.sourceRectStartingPos.X + (float)(this.currentParentTileIndex * this.sourceRect.Width)) % this.texture.Width;
			if (this.sourceRect.X < 0)
			{
				this.sourceRect.X = 0;
			}
			this.sourceRect.Y = (int)this.sourceRectStartingPos.Y;
		}
	}
}
