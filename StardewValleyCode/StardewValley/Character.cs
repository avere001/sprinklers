using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Linq;
using System.Xml.Serialization;
using xTile.Dimensions;

namespace StardewValley
{
	public class Character
	{
		public const float emoteBeginInterval = 20f;

		public const float emoteNormalInterval = 250f;

		public const int emptyCanEmote = 4;

		public const int questionMarkEmote = 8;

		public const int angryEmote = 12;

		public const int exclamationEmote = 16;

		public const int heartEmote = 20;

		public const int sleepEmote = 24;

		public const int sadEmote = 28;

		public const int happyEmote = 32;

		public const int xEmote = 36;

		public const int pauseEmote = 40;

		public const int videoGameEmote = 52;

		public const int musicNoteEmote = 56;

		public const int blushEmote = 60;

		public const int blockedIntervalBeforeEmote = 3000;

		public const int blockedIntervalBeforeSprint = 5000;

		public const double chanceForSound = 0.001;

		[XmlIgnore]
		public AnimatedSprite sprite;

		[XmlIgnore]
		public Vector2 position;

		[XmlIgnore]
		public int speed;

		[XmlIgnore]
		public int addedSpeed;

		[XmlIgnore]
		public int facingDirection = 2;

		[XmlIgnore]
		public int blockedInterval;

		[XmlIgnore]
		public int faceTowardFarmerRadius;

		[XmlIgnore]
		public int faceTowardFarmerTimer;

		[XmlIgnore]
		public int forceUpdateTimer;

		[XmlIgnore]
		public int movementPause;

		public string name;

		[XmlIgnore]
		private string _displayName;

		protected bool moveUp;

		protected bool moveRight;

		protected bool moveDown;

		protected bool moveLeft;

		protected bool freezeMotion;

		public bool isEmoting;

		public bool isCharging;

		public bool willDestroyObjectsUnderfoot = true;

		public bool isGlowing;

		public bool coloredBorder;

		public bool flip;

		public bool drawOnTop;

		public bool faceTowardFarmer;

		public bool faceAwayFromFarmer;

		public bool ignoreMovementAnimation;

		protected int currentEmote;

		protected int currentEmoteFrame;

		protected int facingDirectionBeforeSpeakingToPlayer = -1;

		[XmlIgnore]
		public float emoteInterval;

		[XmlIgnore]
		public float xVelocity;

		[XmlIgnore]
		public float yVelocity;

		[XmlIgnore]
		public Vector2 lastClick = Vector2.Zero;

		[XmlIgnore]
		public Vector2 positionToLerpTo;

		public float scale = 1f;

		public float timeBeforeAIMovementAgain;

		public float glowingTransparency;

		public float glowRate;

		private bool glowUp;

		[XmlIgnore]
		public bool nextEventcommandAfterEmote;

		[XmlIgnore]
		public bool swimming;

		[XmlIgnore]
		public bool collidesWithOtherCharacters;

		[XmlIgnore]
		public bool farmerPassesThrough;

		[XmlIgnore]
		public bool ignoreMultiplayerUpdates;

		[XmlIgnore]
		public bool eventActor;

		protected bool ignoreMovementAnimations;

		[XmlIgnore]
		public int yJumpOffset;

		[XmlIgnore]
		public int ySourceRectOffset;

		[XmlIgnore]
		public float yJumpVelocity;

		private Farmer whoToFace;

		[XmlIgnore]
		public Color glowingColor;

		[XmlIgnore]
		public PathFindController controller;

		private bool emoteFading;

		private Microsoft.Xna.Framework.Rectangle originalSourceRect;

		public static readonly Vector2[] AdjacentTilesOffsets = new Vector2[]
		{
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2(0f, -1f),
			new Vector2(0f, 1f)
		};

		[XmlIgnore]
		public Vector2 drawOffset = Vector2.Zero;

		[XmlIgnore]
		public string displayName
		{
			get
			{
				string arg_1F_0;
				if ((arg_1F_0 = this._displayName) == null)
				{
					arg_1F_0 = (this._displayName = this.translateName(this.name));
				}
				return arg_1F_0;
			}
			set
			{
				this._displayName = value;
			}
		}

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

		public int Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		public int FacingDirection
		{
			get
			{
				return this.facingDirection;
			}
		}

		[XmlIgnore]
		public AnimatedSprite Sprite
		{
			get
			{
				return this.sprite;
			}
			set
			{
				this.sprite = value;
			}
		}

		public bool IsEmoting
		{
			get
			{
				return this.isEmoting;
			}
			set
			{
				this.isEmoting = value;
			}
		}

		public int CurrentEmote
		{
			get
			{
				return this.currentEmote;
			}
			set
			{
				this.currentEmote = value;
			}
		}

		public int CurrentEmoteIndex
		{
			get
			{
				return this.currentEmoteFrame;
			}
		}

		public virtual bool IsMonster
		{
			get
			{
				return false;
			}
		}

		public Character()
		{
		}

		public Character(AnimatedSprite sprite, Vector2 position, int speed, string name)
		{
			this.sprite = sprite;
			this.position = position;
			this.speed = speed;
			this.name = name;
			if (sprite != null)
			{
				this.originalSourceRect = sprite.SourceRect;
			}
		}

		protected virtual string translateName(string name)
		{
			return name;
		}

		public virtual void SetMovingUp(bool b)
		{
			this.moveUp = b;
			if (!b)
			{
				this.Halt();
			}
		}

		public virtual void SetMovingRight(bool b)
		{
			this.moveRight = b;
			if (!b)
			{
				this.Halt();
			}
		}

		public virtual void SetMovingDown(bool b)
		{
			this.moveDown = b;
			if (!b)
			{
				this.Halt();
			}
		}

		public virtual void SetMovingLeft(bool b)
		{
			this.moveLeft = b;
			if (!b)
			{
				this.Halt();
			}
		}

		public void setMovingInFacingDirection()
		{
			switch (this.facingDirection)
			{
			case 0:
				this.SetMovingUp(true);
				return;
			case 1:
				this.SetMovingRight(true);
				return;
			case 2:
				this.SetMovingDown(true);
				return;
			case 3:
				this.SetMovingLeft(true);
				return;
			default:
				return;
			}
		}

		public int getFacingDirection()
		{
			if (this.sprite.CurrentFrame < 4)
			{
				return 2;
			}
			if (this.sprite.CurrentFrame < 8)
			{
				return 1;
			}
			if (this.sprite.CurrentFrame < 12)
			{
				return 0;
			}
			return 3;
		}

		public void setTrajectory(int xVelocity, int yVelocity)
		{
			this.xVelocity = (float)xVelocity;
			this.yVelocity = (float)yVelocity;
		}

		public void setTrajectory(Vector2 trajectory)
		{
			this.xVelocity = trajectory.X;
			this.yVelocity = trajectory.Y;
		}

		public virtual void Halt()
		{
			this.moveUp = false;
			this.moveDown = false;
			this.moveRight = false;
			this.moveLeft = false;
			this.sprite.StopAnimation();
		}

		public void extendSourceRect(int horizontal, int vertical, bool ignoreSourceRectUpdates = true)
		{
			this.sprite.sourceRect.Inflate(Math.Abs(horizontal) / 2, Math.Abs(vertical) / 2);
			this.sprite.sourceRect.Offset(horizontal / 2, vertical / 2);
			Microsoft.Xna.Framework.Rectangle arg_3C_0 = this.originalSourceRect;
			if (this.sprite.SourceRect.Equals(this.originalSourceRect))
			{
				this.sprite.ignoreSourceRectUpdates = false;
				return;
			}
			this.sprite.ignoreSourceRectUpdates = ignoreSourceRectUpdates;
		}

		public virtual bool collideWith(Object o)
		{
			return true;
		}

		public virtual void faceDirection(int direction)
		{
			if (direction != -3)
			{
				this.facingDirection = direction;
				if (this.sprite != null)
				{
					this.sprite.faceDirection(direction);
				}
				this.faceTowardFarmer = false;
				return;
			}
			this.faceTowardFarmer = true;
		}

		public int getDirection()
		{
			if (this.moveUp)
			{
				return 0;
			}
			if (this.moveRight)
			{
				return 1;
			}
			if (this.moveDown)
			{
				return 2;
			}
			if (this.moveLeft)
			{
				return 3;
			}
			return -1;
		}

		public void tryToMoveInDirection(int direction, bool isFarmer, int damagesFarmer, bool glider)
		{
			if (Game1.currentLocation.isCollidingPosition(this.nextPosition(direction), Game1.viewport, isFarmer, damagesFarmer, glider, this))
			{
				return;
			}
			switch (direction)
			{
			case 0:
				this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
				return;
			case 1:
				this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
				return;
			case 2:
				this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
				return;
			case 3:
				this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
				return;
			default:
				return;
			}
		}

		public virtual bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return this.controller == null && !this.IsMonster;
		}

		public virtual void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (base.GetType() == typeof(FarmAnimal))
			{
				this.willDestroyObjectsUnderfoot = false;
			}
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
				boundingBox.X += (int)this.xVelocity;
				boundingBox.Y -= (int)this.yVelocity;
				if (currentLocation == null || !currentLocation.isCollidingPosition(boundingBox, viewport, false, 0, false, this))
				{
					this.position.X = this.position.X + this.xVelocity;
					this.position.Y = this.position.Y - this.yVelocity;
				}
				this.xVelocity = (float)((int)(this.xVelocity - this.xVelocity / 2f));
				this.yVelocity = (float)((int)(this.yVelocity - this.yVelocity / 2f));
			}
			else if (this.moveUp)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateUp(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(0);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize - 1));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(0), true))
					{
						this.doEmote(12, true);
						this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (this.moveRight)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateRight(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(1);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize + 1), (float)(this.getStandingY() / Game1.tileSize));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(1), true))
					{
						this.doEmote(12, true);
						this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (this.moveDown)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateDown(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(2);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize + 1));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(2), true))
					{
						this.doEmote(12, true);
						this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (this.moveLeft)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateLeft(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(3);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize - 1), (float)(this.getStandingY() / Game1.tileSize));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(3), true))
					{
						this.doEmote(12, true);
						this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			if (this.blockedInterval >= 3000 && (float)this.blockedInterval <= 3750f && !Game1.eventUp)
			{
				this.doEmote((Game1.random.NextDouble() < 0.5) ? 8 : 40, true);
				this.blockedInterval = 3750;
				return;
			}
			if (this.blockedInterval >= 5000)
			{
				this.speed = 4;
				this.isCharging = true;
				this.blockedInterval = 0;
			}
		}

		public virtual bool canPassThroughActionTiles()
		{
			return false;
		}

		public virtual Microsoft.Xna.Framework.Rectangle nextPosition(int direction)
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			switch (direction)
			{
			case 0:
				boundingBox.Y -= this.speed + this.addedSpeed;
				break;
			case 1:
				boundingBox.X += this.speed + this.addedSpeed;
				break;
			case 2:
				boundingBox.Y += this.speed + this.addedSpeed;
				break;
			case 3:
				boundingBox.X -= this.speed + this.addedSpeed;
				break;
			}
			return boundingBox;
		}

		public Location nextPositionPoint()
		{
			Location result = default(Location);
			switch (this.getDirection())
			{
			case 0:
				result = new Location(this.getStandingX(), this.getStandingY() - Game1.tileSize);
				break;
			case 1:
				result = new Location(this.getStandingX() + Game1.tileSize, this.getStandingY());
				break;
			case 2:
				result = new Location(this.getStandingX(), this.getStandingY() + Game1.tileSize);
				break;
			case 3:
				result = new Location(this.getStandingX() - Game1.tileSize, this.getStandingY());
				break;
			}
			return result;
		}

		public int getHorizontalMovement()
		{
			if (this.moveRight)
			{
				return this.speed + this.addedSpeed;
			}
			if (!this.moveLeft)
			{
				return 0;
			}
			return -this.speed - this.addedSpeed;
		}

		public int getVerticalMovement()
		{
			if (this.moveDown)
			{
				return this.speed + this.addedSpeed;
			}
			if (!this.moveUp)
			{
				return 0;
			}
			return -this.speed - this.addedSpeed;
		}

		public Vector2 nextPositionVector2()
		{
			return new Vector2((float)(this.getStandingX() + this.getHorizontalMovement()), (float)(this.getStandingY() + this.getVerticalMovement()));
		}

		public Location nextPositionTile()
		{
			Location result = this.nextPositionPoint();
			result.X /= Game1.tileSize;
			result.Y /= Game1.tileSize;
			return result;
		}

		public virtual void doEmote(int whichEmote, bool playSound, bool nextEventCommand = true)
		{
			if (!this.isEmoting && (!Game1.eventUp || this is Farmer || (Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.actors.Contains(this))))
			{
				this.isEmoting = true;
				this.currentEmote = whichEmote;
				this.currentEmoteFrame = 0;
				this.emoteInterval = 0f;
				this.nextEventcommandAfterEmote = nextEventCommand;
			}
		}

		public void doEmote(int whichEmote, bool nextEventCommand = true)
		{
			this.doEmote(whichEmote, true, nextEventCommand);
		}

		public void updateEmote(GameTime time)
		{
			if (this.isEmoting)
			{
				this.emoteInterval += (float)time.ElapsedGameTime.Milliseconds;
				if (this.emoteFading && this.emoteInterval > 20f)
				{
					this.emoteInterval = 0f;
					this.currentEmoteFrame--;
					if (this.currentEmoteFrame < 0)
					{
						this.emoteFading = false;
						this.isEmoting = false;
						if (this.nextEventcommandAfterEmote && Game1.currentLocation.currentEvent != null && (Game1.currentLocation.currentEvent.actors.Contains(this) || this.name.Equals(Game1.player.Name)))
						{
							Event expr_CA = Game1.currentLocation.currentEvent;
							int currentCommand = expr_CA.CurrentCommand;
							expr_CA.CurrentCommand = currentCommand + 1;
							return;
						}
					}
				}
				else if (!this.emoteFading && this.emoteInterval > 20f && this.currentEmoteFrame <= 3)
				{
					this.emoteInterval = 0f;
					this.currentEmoteFrame++;
					if (this.currentEmoteFrame == 4)
					{
						this.currentEmoteFrame = this.currentEmote;
						return;
					}
				}
				else if (!this.emoteFading && this.emoteInterval > 250f)
				{
					this.emoteInterval = 0f;
					this.currentEmoteFrame++;
					if (this.currentEmoteFrame >= this.currentEmote + 4)
					{
						this.emoteFading = true;
						this.currentEmoteFrame = 3;
					}
				}
			}
		}

		public Vector2 GetGrabTile()
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			switch (this.facingDirection)
			{
			case 0:
				return new Vector2((float)((boundingBox.X + boundingBox.Width / 2) / Game1.tileSize), (float)((boundingBox.Y - 5) / Game1.tileSize));
			case 1:
				return new Vector2((float)((boundingBox.X + boundingBox.Width + 5) / Game1.tileSize), (float)((boundingBox.Y + boundingBox.Height / 2) / Game1.tileSize));
			case 2:
				return new Vector2((float)((boundingBox.X + boundingBox.Width / 2) / Game1.tileSize), (float)((boundingBox.Y + boundingBox.Height + 5) / Game1.tileSize));
			case 3:
				return new Vector2((float)((boundingBox.X - 5) / Game1.tileSize), (float)((boundingBox.Y + boundingBox.Height / 2) / Game1.tileSize));
			default:
				return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
			}
		}

		public Vector2 GetDropLocation()
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			switch (this.facingDirection)
			{
			case 0:
				return new Vector2((float)(boundingBox.X + Game1.tileSize / 4), (float)(boundingBox.Y - Game1.tileSize));
			case 1:
				return new Vector2((float)(boundingBox.X + boundingBox.Width + Game1.tileSize), (float)(boundingBox.Y + Game1.tileSize / 4));
			case 2:
				return new Vector2((float)(boundingBox.X + Game1.tileSize / 4), (float)(boundingBox.Y + boundingBox.Height + Game1.tileSize));
			case 3:
				return new Vector2((float)(boundingBox.X - Game1.tileSize), (float)(boundingBox.Y + Game1.tileSize / 4));
			default:
				return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
			}
		}

		public virtual Vector2 GetToolLocation(bool ignoreClick = false)
		{
			if (!Game1.wasMouseVisibleThisFrame || Game1.isAnyGamePadButtonBeingHeld())
			{
				ignoreClick = true;
			}
			if ((Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is WateringCan)) && (int)(this.lastClick.X / (float)Game1.tileSize) == Game1.player.getTileX() && (int)(this.lastClick.Y / (float)Game1.tileSize) == Game1.player.getTileY())
			{
				Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
				switch (this.facingDirection)
				{
				case 0:
					return new Vector2((float)(boundingBox.X + boundingBox.Width / 2), (float)(boundingBox.Y - Game1.tileSize));
				case 1:
					return new Vector2((float)(boundingBox.X + boundingBox.Width + Game1.tileSize), (float)(boundingBox.Y + boundingBox.Height / 2));
				case 2:
					return new Vector2((float)(boundingBox.X + boundingBox.Width / 2), (float)(boundingBox.Y + boundingBox.Height + Game1.tileSize));
				case 3:
					return new Vector2((float)(boundingBox.X - Game1.tileSize), (float)(boundingBox.Y + boundingBox.Height / 2));
				}
			}
			if (!ignoreClick && !this.lastClick.Equals(Vector2.Zero) && this.name.Equals(Game1.player.name) && ((int)(this.lastClick.X / (float)Game1.tileSize) != Game1.player.getTileX() || (int)(this.lastClick.Y / (float)Game1.tileSize) != Game1.player.getTileY() || (Game1.player.CurrentTool != null && Game1.player.CurrentTool is WateringCan)) && Utility.distance(this.lastClick.X, (float)Game1.player.getStandingX(), this.lastClick.Y, (float)Game1.player.getStandingY()) <= (float)(Game1.tileSize * 2))
			{
				return this.lastClick;
			}
			Microsoft.Xna.Framework.Rectangle boundingBox2 = this.GetBoundingBox();
			if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name.Equals("Fishing Rod"))
			{
				switch (this.facingDirection)
				{
				case 0:
					return new Vector2((float)(boundingBox2.X - Game1.tileSize / 4), (float)(boundingBox2.Y - Game1.tileSize * 8 / 5));
				case 1:
					return new Vector2((float)(boundingBox2.X + boundingBox2.Width + Game1.tileSize), (float)boundingBox2.Y);
				case 2:
					return new Vector2((float)(boundingBox2.X - Game1.tileSize / 4), (float)(boundingBox2.Y + boundingBox2.Height + Game1.tileSize));
				case 3:
					return new Vector2((float)(boundingBox2.X - Game1.tileSize * 7 / 4), (float)boundingBox2.Y);
				}
			}
			else
			{
				switch (this.facingDirection)
				{
				case 0:
					return new Vector2((float)(boundingBox2.X + boundingBox2.Width / 2), (float)(boundingBox2.Y - Game1.tileSize * 3 / 4));
				case 1:
					return new Vector2((float)(boundingBox2.X + boundingBox2.Width + Game1.tileSize * 3 / 4), (float)(boundingBox2.Y + boundingBox2.Height / 2));
				case 2:
					return new Vector2((float)(boundingBox2.X + boundingBox2.Width / 2), (float)(boundingBox2.Y + boundingBox2.Height + Game1.tileSize * 3 / 4));
				case 3:
					return new Vector2((float)(boundingBox2.X - Game1.tileSize * 3 / 4), (float)(boundingBox2.Y + boundingBox2.Height / 2));
				}
			}
			return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
		}

		public void faceGeneralDirection(Vector2 target, int yBias = 0)
		{
			int tileX = this.getTileX();
			int tileY = this.getTileY();
			int num = (int)(target.X / (float)Game1.tileSize) - tileX;
			int num2 = (int)(target.Y / (float)Game1.tileSize) - tileY;
			if (num > Math.Abs(num2) + yBias)
			{
				this.faceDirection(1);
				return;
			}
			if (Math.Abs(num) > Math.Abs(num2) + yBias)
			{
				this.faceDirection(3);
				return;
			}
			if (num2 > 0 || (float)this.getStandingY() < target.Y)
			{
				this.faceDirection(2);
				return;
			}
			this.faceDirection(0);
		}

		public Vector2 getStandingPosition()
		{
			return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
		}

		public virtual void draw(SpriteBatch b)
		{
			this.draw(b, 1f);
		}

		public virtual void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
		}

		public virtual void draw(SpriteBatch b, float alpha = 1f)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position), (float)this.GetBoundingBox().Center.Y / 10000f);
			if (this.IsEmoting)
			{
				Vector2 localPosition = this.getLocalPosition(Game1.viewport);
				localPosition.Y -= (float)(Game1.tileSize * 3 / 2);
				b.Draw(Game1.emoteSpriteSheet, localPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White * alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)this.getStandingY() / 10000f);
			}
		}

		public virtual void draw(SpriteBatch b, int ySourceRectOffset, float alpha = 1f)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)), (float)this.GetBoundingBox().Center.Y / 10000f, 0, ySourceRectOffset, Color.White, false, (float)Game1.pixelZoom, 0f, true);
			if (this.IsEmoting)
			{
				Vector2 localPosition = this.getLocalPosition(Game1.viewport);
				localPosition.Y -= (float)(Game1.tileSize * 3 / 2);
				b.Draw(Game1.emoteSpriteSheet, localPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White * alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)this.getStandingY() / 10000f);
			}
		}

		public virtual Microsoft.Xna.Framework.Rectangle GetBoundingBox()
		{
			if (this.sprite == null)
			{
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
			return new Microsoft.Xna.Framework.Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y + Game1.tileSize / 4, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize / 2);
		}

		public void stopWithoutChangingFrame()
		{
			this.moveDown = false;
			this.moveLeft = false;
			this.moveRight = false;
			this.moveUp = false;
		}

		public virtual void collisionWithFarmerBehavior()
		{
		}

		public int getStandingX()
		{
			return this.GetBoundingBox().Center.X;
		}

		public int getStandingY()
		{
			return this.GetBoundingBox().Center.Y;
		}

		public Vector2 getLocalPosition(xTile.Dimensions.Rectangle viewport)
		{
			return new Vector2(this.position.X - (float)viewport.X, this.position.Y - (float)viewport.Y + (float)this.yJumpOffset) + this.drawOffset;
		}

		public virtual bool isMoving()
		{
			return this.moveUp || this.moveDown || this.moveRight || this.moveLeft;
		}

		public Point getTileLocationPoint()
		{
			return new Point(this.getStandingX() / Game1.tileSize, this.getStandingY() / Game1.tileSize);
		}

		public Point getLeftMostTileX()
		{
			return new Point(this.GetBoundingBox().X / Game1.tileSize, this.GetBoundingBox().Center.Y / Game1.tileSize);
		}

		public Point getRightMostTileX()
		{
			return new Point((this.GetBoundingBox().Right - 1) / Game1.tileSize, this.GetBoundingBox().Center.Y / Game1.tileSize);
		}

		public int getTileX()
		{
			return this.getStandingX() / Game1.tileSize;
		}

		public int getTileY()
		{
			return this.getStandingY() / Game1.tileSize;
		}

		public Vector2 getTileLocation()
		{
			return new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize));
		}

		public void startGlowing(Color glowingColor, bool border, float glowRate)
		{
			if (!this.glowingColor.Equals(glowingColor))
			{
				this.isGlowing = true;
				this.coloredBorder = border;
				this.glowingColor = glowingColor;
				this.glowUp = true;
				this.glowRate = glowRate;
				this.glowingTransparency = 0f;
			}
		}

		public void stopGlowing()
		{
			this.isGlowing = false;
			this.glowingColor = Color.White;
		}

		public virtual void jumpWithoutSound(float velocity = 8f)
		{
			this.yJumpVelocity = velocity;
			this.yJumpOffset = -1;
		}

		public virtual void jump()
		{
			this.yJumpVelocity = 8f;
			this.yJumpOffset = -1;
			Game1.playSound("dwop");
		}

		public virtual void jump(float jumpVelocity)
		{
			this.yJumpVelocity = jumpVelocity;
			this.yJumpOffset = -1;
			Game1.playSound("dwop");
		}

		public void faceTowardFarmerForPeriod(int milliseconds, int radius, bool faceAway, Farmer who)
		{
			if ((this.sprite != null && this.sprite.currentAnimation == null) || this.isMoving())
			{
				if (this.isMoving())
				{
					milliseconds /= 2;
				}
				this.Halt();
				if (this.facingDirectionBeforeSpeakingToPlayer == -1)
				{
					this.facingDirectionBeforeSpeakingToPlayer = this.facingDirection;
				}
				this.faceTowardFarmerTimer = milliseconds;
				this.faceTowardFarmerRadius = radius;
				this.movementPause = milliseconds;
				this.faceAwayFromFarmer = faceAway;
				this.whoToFace = who;
			}
		}

		public virtual void update(GameTime time, GameLocation location)
		{
			this.update(time, location, 0L, true);
		}

		public virtual void performBehavior(byte which)
		{
		}

		public virtual void update(GameTime time, GameLocation location, long id, bool move)
		{
			if (this.yJumpOffset != 0)
			{
				this.yJumpVelocity -= 0.5f;
				this.yJumpOffset -= (int)this.yJumpVelocity;
				if (this.yJumpOffset >= 0)
				{
					this.yJumpOffset = 0;
					this.yJumpVelocity = 0f;
					if (!this.IsMonster && (location == null || location.Equals(Game1.currentLocation)))
					{
						FarmerSprite.checkForFootstep(this);
					}
				}
			}
			if (this.faceTowardFarmerTimer > 0)
			{
				this.faceTowardFarmerTimer -= time.ElapsedGameTime.Milliseconds;
				if (!this.faceTowardFarmer && this.faceTowardFarmerTimer > 0 && Utility.tileWithinRadiusOfPlayer((int)this.getTileLocation().X, (int)this.getTileLocation().Y, this.faceTowardFarmerRadius, this.whoToFace))
				{
					this.faceTowardFarmer = true;
				}
				else if (!Utility.tileWithinRadiusOfPlayer((int)this.getTileLocation().X, (int)this.getTileLocation().Y, this.faceTowardFarmerRadius, this.whoToFace) || this.faceTowardFarmerTimer <= 0)
				{
					this.faceDirection(this.facingDirectionBeforeSpeakingToPlayer);
					if (this.faceTowardFarmerTimer <= 0)
					{
						this.facingDirectionBeforeSpeakingToPlayer = -1;
						this.faceTowardFarmer = false;
						this.faceAwayFromFarmer = false;
						this.faceTowardFarmerTimer = 0;
					}
				}
			}
			if (this.forceUpdateTimer > 0)
			{
				this.forceUpdateTimer -= time.ElapsedGameTime.Milliseconds;
			}
			this.updateGlow();
			this.updateEmote(time);
			if (!Game1.IsMultiplayer || Game1.IsServer || this.ignoreMultiplayerUpdates)
			{
				if (this.faceTowardFarmer && this.whoToFace != null)
				{
					this.faceGeneralDirection(this.whoToFace.getStandingPosition(), 0);
					if (this.faceAwayFromFarmer)
					{
						this.faceDirection((this.facingDirection + 2) % 4);
					}
				}
				if ((this.controller == null & move) && !this.freezeMotion)
				{
					this.updateMovement(location, time);
				}
				if (this.controller != null && !this.freezeMotion && this.controller.update(time))
				{
					this.controller = null;
				}
				if (Game1.IsServer && !Game1.isFestival() && Game1.random.NextDouble() < 0.2)
				{
					MultiplayerUtility.broadcastNPCMove((int)this.position.X, (int)this.position.Y, id, location);
					return;
				}
			}
			else if (!Game1.eventUp)
			{
				this.lerpPosition(this.positionToLerpTo);
				if (this.distanceFromLastServerPosition() >= 8f)
				{
					this.animateInFacingDirection(time);
				}
			}
		}

		public virtual bool hasSpecialCollisionRules()
		{
			return false;
		}

		public virtual bool isColliding(GameLocation l, Vector2 tile)
		{
			return false;
		}

		public float distanceFromLastServerPosition()
		{
			return Vector2.DistanceSquared(this.position, this.positionToLerpTo);
		}

		public virtual void animateInFacingDirection(GameTime time)
		{
			switch (this.FacingDirection)
			{
			case 0:
				this.Sprite.AnimateUp(time, 0, "");
				return;
			case 1:
				this.Sprite.AnimateRight(time, 0, "");
				return;
			case 2:
				this.Sprite.AnimateDown(time, 0, "");
				return;
			case 3:
				this.Sprite.AnimateLeft(time, 0, "");
				return;
			default:
				return;
			}
		}

		public virtual void updatePositionFromServer(Vector2 newPosition)
		{
			this.positionToLerpTo = newPosition;
			int num = (int)(newPosition.X - this.position.X);
			int num2 = (int)(newPosition.Y - this.position.Y);
			if (num > Math.Abs(num2))
			{
				this.faceDirection(1);
				return;
			}
			if (Math.Abs(num) > Math.Abs(num2))
			{
				this.faceDirection(3);
				return;
			}
			if (num2 > 0)
			{
				this.faceDirection(2);
				return;
			}
			if (num2 < 0)
			{
				this.faceDirection(0);
				return;
			}
			if (this.sprite.CurrentFrame < 16)
			{
				this.Halt();
			}
		}

		public virtual void lerpPosition(Vector2 target)
		{
			if (target.Equals(Vector2.Zero))
			{
				return;
			}
			int num = (int)(target.X - this.position.X);
			if (Math.Abs(num) > Game1.tileSize * 4)
			{
				this.position.X = target.X;
			}
			else
			{
				this.position.X = this.position.X + (float)(num * this.Speed) * 0.04f;
			}
			num = (int)(target.Y - this.position.Y);
			if (Math.Abs(num) > Game1.tileSize * 4)
			{
				this.position.Y = target.Y;
				return;
			}
			this.position.Y = this.position.Y + (float)(num * this.Speed) * 0.04f;
		}

		public virtual void updateMovement(GameLocation location, GameTime time)
		{
		}

		public void updateGlow()
		{
			if (this.isGlowing)
			{
				if (this.glowUp)
				{
					this.glowingTransparency += this.glowRate;
					if (this.glowingTransparency >= 1f)
					{
						this.glowingTransparency = 1f;
						this.glowUp = false;
						return;
					}
				}
				else
				{
					this.glowingTransparency -= this.glowRate;
					if (this.glowingTransparency <= 0f)
					{
						this.glowingTransparency = 0f;
						this.glowUp = true;
					}
				}
			}
		}

		public void convertEventMotionCommandToMovement(Vector2 command)
		{
			if (command.X < 0f)
			{
				this.SetMovingLeft(true);
				return;
			}
			if (command.X > 0f)
			{
				this.SetMovingRight(true);
				return;
			}
			if (command.Y < 0f)
			{
				this.SetMovingUp(true);
				return;
			}
			if (command.Y > 0f)
			{
				this.SetMovingDown(true);
			}
		}
	}
}
