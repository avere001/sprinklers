using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Linq;

namespace StardewValley
{
	public class FarmerSprite : AnimatedSprite
	{
		public struct AnimationFrame
		{
			public int frame;

			public int milliseconds;

			public int positionOffset;

			public int xOffset;

			public bool secondaryArm;

			public bool flip;

			public bool behaviorAtEndOfFrame;

			public AnimatedSprite.endOfAnimationBehavior frameBehavior;

			public AnimationFrame(int frame, int milliseconds, int positionOffset, bool secondaryArm, bool flip, AnimatedSprite.endOfAnimationBehavior frameBehavior = null, bool behaviorAtEndOfFrame = false, int xOffset = 0)
			{
				this.frame = frame;
				this.milliseconds = milliseconds;
				this.positionOffset = positionOffset;
				this.secondaryArm = secondaryArm;
				this.flip = flip;
				this.frameBehavior = frameBehavior;
				this.behaviorAtEndOfFrame = behaviorAtEndOfFrame;
				this.xOffset = xOffset;
			}

			public AnimationFrame(int frame, int milliseconds, bool secondaryArm, bool flip, AnimatedSprite.endOfAnimationBehavior frameBehavior = null, bool behaviorAtEndOfFrame = false)
			{
				this.frame = frame;
				this.milliseconds = milliseconds;
				this.positionOffset = 0;
				this.secondaryArm = secondaryArm;
				this.flip = flip;
				this.frameBehavior = frameBehavior;
				this.behaviorAtEndOfFrame = behaviorAtEndOfFrame;
				this.xOffset = 0;
			}

			public AnimationFrame(int frame, int milliseconds)
			{
				this = new FarmerSprite.AnimationFrame(frame, milliseconds, false, false, null, false);
			}
		}

		public const int walkDown = 0;

		public const int walkRight = 8;

		public const int walkUp = 16;

		public const int walkLeft = 24;

		public const int runDown = 32;

		public const int runRight = 40;

		public const int runUp = 48;

		public const int runLeft = 56;

		public const int grabDown = 64;

		public const int grabRight = 72;

		public const int grabUp = 80;

		public const int grabLeft = 88;

		public const int carryWalkDown = 96;

		public const int carryWalkRight = 104;

		public const int carryWalkUp = 112;

		public const int carryWalkLeft = 120;

		public const int carryRunDown = 128;

		public const int carryRunRight = 136;

		public const int carryRunUp = 144;

		public const int carryRunLeft = 152;

		public const int toolDown = 160;

		public const int toolRight = 168;

		public const int toolUp = 176;

		public const int toolLeft = 184;

		public const int toolChooseDown = 192;

		public const int toolChooseRight = 194;

		public const int toolChooseUp = 196;

		public const int toolChooseLeft = 198;

		public const int seedThrowDown = 200;

		public const int seedThrowRight = 204;

		public const int seedThrowUp = 208;

		public const int seedThrowLeft = 212;

		public const int eat = 216;

		public const int sick = 224;

		public const int swordswipeDown = 232;

		public const int swordswipeRight = 240;

		public const int swordswipeUp = 248;

		public const int swordswipeLeft = 256;

		public const int punchDown = 272;

		public const int punchRight = 274;

		public const int punchUp = 276;

		public const int punchLeft = 278;

		public const int harvestItemUp = 279;

		public const int harvestItemRight = 280;

		public const int harvestItemDown = 281;

		public const int harvestItemLeft = 282;

		public const int shearUp = 283;

		public const int shearRight = 284;

		public const int shearDown = 285;

		public const int shearLeft = 286;

		public const int milkUp = 287;

		public const int milkRight = 288;

		public const int milkDown = 289;

		public const int milkLeft = 290;

		public const int tired = 291;

		public const int tired2 = 292;

		public const int passOutTired = 293;

		public const int drink = 294;

		public const int fishingUp = 295;

		public const int fishingRight = 296;

		public const int fishingDown = 297;

		public const int fishingLeft = 298;

		public const int fishingDoneUp = 299;

		public const int fishingDoneRight = 300;

		public const int fishingDoneDown = 301;

		public const int fishingDoneLeft = 302;

		public const int pan = 303;

		public const int showHoldingEdible = 304;

		private int currentToolIndex;

		private float oldInterval;

		public bool pauseForSingleAnimation;

		public bool animateBackwards;

		public bool loopThisAnimation;

		public bool ignoreDefaultActionThisTime;

		public bool freezeUntilDialogueIsOver;

		private int currentSingleAnimation;

		private int currentAnimationFrames;

		public int indexInCurrentAnimation;

		public float currentSingleAnimationInterval = 200f;

		public float intervalModifier = 1f;

		public string currentStep = "sandyStep";

		private Farmer owner;

		public int nextOffset;

		public bool animatingBackwards;

		public const int cheer = 97;

		public FarmerSprite.AnimationFrame CurrentAnimationFrame
		{
			get
			{
				if (this.currentAnimation == null)
				{
					return new FarmerSprite.AnimationFrame(0, 100, 0, false, false, null, false, 0);
				}
				return this.currentAnimation[this.indexInCurrentAnimation % this.currentAnimation.Count];
			}
		}

		public int CurrentSingleAnimation
		{
			get
			{
				if (this.currentAnimation != null)
				{
					return this.currentAnimation[0].frame;
				}
				return -1;
			}
		}

		public override int CurrentFrame
		{
			get
			{
				return this.currentFrame;
			}
			set
			{
				if (this.currentFrame != value && !this.freezeUntilDialogueIsOver)
				{
					this.currentFrame = value;
					this.UpdateSourceRect();
				}
				if (value > FarmerRenderer.featureYOffsetPerFrame.Length - 1)
				{
					this.currentFrame = 0;
				}
			}
		}

		public bool PauseForSingleAnimation
		{
			get
			{
				return this.pauseForSingleAnimation;
			}
			set
			{
				this.pauseForSingleAnimation = value;
			}
		}

		public int CurrentToolIndex
		{
			get
			{
				return this.currentToolIndex;
			}
			set
			{
				this.currentToolIndex = value;
			}
		}

		public void setOwner(Farmer owner)
		{
			this.owner = owner;
		}

		public void setCurrentAnimation(FarmerSprite.AnimationFrame[] animation)
		{
			base.CurrentAnimation = animation.ToList<FarmerSprite.AnimationFrame>();
			this.oldFrame = this.currentFrame;
			this.currentAnimationIndex = 0;
			if (this.currentAnimation.Count > 0)
			{
				this.interval = (float)this.currentAnimation[0].milliseconds;
				this.CurrentFrame = this.currentAnimation[0].frame;
				this.currentAnimationFrames = this.currentAnimation.Count;
			}
		}

		public override void faceDirection(int direction)
		{
			switch (direction)
			{
			case 0:
				this.setCurrentFrame(12);
				break;
			case 1:
				this.setCurrentFrame(6);
				break;
			case 2:
				this.setCurrentFrame(0);
				break;
			case 3:
				this.setCurrentFrame(6, 0, 100, 1, true, false);
				break;
			}
			this.UpdateSourceRect();
		}

		public void setCurrentSingleFrame(int which, short interval = 32000, bool secondaryArm = false, bool flip = false)
		{
			this.loopThisAnimation = false;
			base.CurrentAnimation = new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame((int)((short)which), (int)interval, secondaryArm, flip, null, false)
			}.ToList<FarmerSprite.AnimationFrame>();
			this.CurrentFrame = base.CurrentAnimation[0].frame;
		}

		public void setCurrentFrame(int which)
		{
			this.setCurrentFrame(which, 0);
		}

		public void setCurrentFrame(int which, int offset)
		{
			this.setCurrentFrame(which, offset, 100, 1, false, false);
		}

		public void setCurrentFrameBackwards(int which, int offset, int interval, int numFrames, bool secondaryArm, bool flip)
		{
			base.CurrentAnimation = FarmerSprite.getAnimationFromIndex(which, this, interval, numFrames, secondaryArm, flip).ToList<FarmerSprite.AnimationFrame>();
			base.CurrentAnimation.Reverse();
			this.CurrentFrame = base.CurrentAnimation[Math.Min(base.CurrentAnimation.Count - 1, offset)].frame;
		}

		public void setCurrentFrame(int which, int offset, int interval, int numFrames, bool flip, bool secondaryArm)
		{
			if (this.nextOffset != 0)
			{
				offset = this.nextOffset;
				this.nextOffset = 0;
			}
			base.CurrentAnimation = FarmerSprite.getAnimationFromIndex(which, this, interval, numFrames, flip, secondaryArm).ToList<FarmerSprite.AnimationFrame>();
			this.CurrentFrame = base.CurrentAnimation[Math.Min(base.CurrentAnimation.Count - 1, offset)].frame;
			this.interval = (float)this.CurrentAnimationFrame.milliseconds;
			this.timer = 0f;
		}

		public FarmerSprite(Texture2D texture) : base(texture)
		{
			this.interval /= 2f;
			this.spriteWidth = 16;
			this.spriteHeight = 32;
		}

		public void animate(int whichAnimation, GameTime time)
		{
			this.animate(whichAnimation, time.ElapsedGameTime.Milliseconds);
		}

		public void animate(int whichAnimation, int milliseconds)
		{
			if (!this.PauseForSingleAnimation)
			{
				if (whichAnimation != this.currentSingleAnimation || base.CurrentAnimation == null || base.CurrentAnimation.Count <= 1)
				{
					float timer = this.timer;
					int num = this.indexInCurrentAnimation;
					this.currentSingleAnimation = whichAnimation;
					this.setCurrentFrame(whichAnimation);
					this.timer = timer;
					this.CurrentFrame = base.CurrentAnimation[Math.Min(num, base.CurrentAnimation.Count - 1)].frame;
					this.indexInCurrentAnimation = num;
					this.currentAnimationIndex = num;
					this.UpdateSourceRect();
				}
				this.animate(milliseconds);
			}
		}

		public void checkForSingleAnimation(GameTime time)
		{
			if (this.PauseForSingleAnimation)
			{
				if (!this.animateBackwards)
				{
					this.animateOnce(time);
					return;
				}
				this.animateBackwardsOnce(time);
			}
		}

		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames)
		{
			this.animateOnce(whichAnimation, animationInterval, numberOfFrames, null);
		}

		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames, AnimatedSprite.endOfAnimationBehavior endOfBehaviorFunction)
		{
			this.animateOnce(whichAnimation, animationInterval, numberOfFrames, endOfBehaviorFunction, false, false);
		}

		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames, AnimatedSprite.endOfAnimationBehavior endOfBehaviorFunction, bool flip, bool secondaryArm)
		{
			this.animateOnce(whichAnimation, animationInterval, numberOfFrames, endOfBehaviorFunction, flip, secondaryArm, false);
		}

		public void animateOnce(FarmerSprite.AnimationFrame[] animation)
		{
			if (!this.PauseForSingleAnimation)
			{
				this.currentSingleAnimation = 0;
				this.CurrentFrame = this.currentSingleAnimation;
				this.PauseForSingleAnimation = true;
				this.oldFrame = this.CurrentFrame;
				this.oldInterval = this.interval;
				this.currentSingleAnimationInterval = 100f;
				this.timer = 0f;
				base.CurrentAnimation = animation.ToList<FarmerSprite.AnimationFrame>();
				this.CurrentFrame = base.CurrentAnimation[0].frame;
				this.currentAnimationFrames = base.CurrentAnimation.Count;
				this.indexInCurrentAnimation = 0;
				this.interval = (float)this.CurrentAnimationFrame.milliseconds;
				this.loopThisAnimation = false;
			}
		}

		public void showFrameUntilDialogueOver(int whichFrame)
		{
			this.freezeUntilDialogueIsOver = true;
			this.setCurrentFrame(whichFrame);
			this.UpdateSourceRect();
		}

		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames, AnimatedSprite.endOfAnimationBehavior endOfBehaviorFunction, bool flip, bool secondaryArm, bool backwards)
		{
			if (!this.PauseForSingleAnimation && !this.freezeUntilDialogueIsOver)
			{
				if (!this.owner.IsMainPlayer)
				{
					if (whichAnimation <= 240)
					{
						if (whichAnimation == 232)
						{
							this.owner.faceDirection(2);
							goto IL_A9;
						}
						if (whichAnimation == 240)
						{
							this.owner.faceDirection(1);
							goto IL_A9;
						}
					}
					else
					{
						if (whichAnimation == 248)
						{
							this.owner.faceDirection(0);
							goto IL_A9;
						}
						if (whichAnimation == 256)
						{
							this.owner.faceDirection(3);
							goto IL_A9;
						}
					}
					int num = whichAnimation / 8 % 4;
					if (num == 0)
					{
						num = 2;
					}
					else if (num == 2)
					{
						num = 0;
					}
					this.owner.faceDirection(num);
				}
				IL_A9:
				this.currentSingleAnimation = whichAnimation;
				this.CurrentFrame = this.currentSingleAnimation;
				this.PauseForSingleAnimation = true;
				this.oldFrame = this.CurrentFrame;
				this.oldInterval = this.interval;
				this.currentSingleAnimationInterval = animationInterval;
				this.endOfAnimationFunction = endOfBehaviorFunction;
				this.timer = 0f;
				this.animatingBackwards = false;
				if (backwards)
				{
					this.animatingBackwards = true;
					this.setCurrentFrameBackwards(this.currentSingleAnimation, 0, (int)animationInterval, numberOfFrames, secondaryArm, flip);
				}
				else
				{
					this.setCurrentFrame(this.currentSingleAnimation, 0, (int)animationInterval, numberOfFrames, secondaryArm, flip);
				}
				if (base.CurrentAnimation[0].frameBehavior != null && !base.CurrentAnimation[0].behaviorAtEndOfFrame)
				{
					base.CurrentAnimation[0].frameBehavior(this.owner);
				}
				if (this.owner.Stamina <= 0f && this.owner.usingTool)
				{
					for (int i = 0; i < base.CurrentAnimation.Count; i++)
					{
						base.CurrentAnimation[i] = new FarmerSprite.AnimationFrame(base.CurrentAnimation[i].frame, base.CurrentAnimation[i].milliseconds * 2, base.CurrentAnimation[i].secondaryArm, base.CurrentAnimation[i].flip, base.CurrentAnimation[i].frameBehavior, base.CurrentAnimation[i].behaviorAtEndOfFrame);
					}
				}
				this.currentAnimationFrames = base.CurrentAnimation.Count;
				this.indexInCurrentAnimation = 0;
				this.interval = (float)this.CurrentAnimationFrame.milliseconds;
				if (Game1.IsClient && this.owner.uniqueMultiplayerID == Game1.player.uniqueMultiplayerID)
				{
					this.currentToolIndex = -1;
					if (this.owner.UsingTool)
					{
						this.currentToolIndex = this.owner.CurrentTool.CurrentParentTileIndex;
						if (this.owner.CurrentTool is FishingRod)
						{
							if (this.owner.facingDirection == 3 || this.owner.facingDirection == 1)
							{
								this.currentToolIndex = 55;
							}
							else
							{
								this.currentToolIndex = 48;
							}
						}
					}
					MultiplayerUtility.sendAnimationMessageToServer(whichAnimation, numberOfFrames, animationInterval, false, this.currentToolIndex);
				}
				else if (Game1.IsServer)
				{
					if (this.owner.IsMainPlayer && this.owner.UsingTool)
					{
						this.currentToolIndex = this.owner.CurrentTool.CurrentParentTileIndex;
						if (this.owner.CurrentTool is FishingRod)
						{
							if (this.owner.facingDirection == 3 || this.owner.facingDirection == 1)
							{
								this.currentToolIndex = 55;
							}
							else
							{
								this.currentToolIndex = 48;
							}
						}
					}
					MultiplayerUtility.broadcastFarmerAnimation(this.owner.uniqueMultiplayerID, whichAnimation, numberOfFrames, animationInterval, false, this.owner.currentLocation.name, this.currentToolIndex);
				}
				if (Game1.IsMultiplayer && this.getWeaponTypeFromAnimation() == 3)
				{
					MeleeWeapon.doSwipe(this.getWeaponTypeFromAnimation(), this.owner.position, this.owner.facingDirection, animationInterval, this.owner);
				}
			}
		}

		public void animateBackwardsOnce(int whichAnimation, float animationInterval)
		{
			this.animateOnce(whichAnimation, animationInterval, 6, null, false, false, true);
		}

		public bool isUsingWeapon()
		{
			return this.PauseForSingleAnimation && ((this.currentSingleAnimation >= 232 && this.currentSingleAnimation < 264) || (this.currentSingleAnimation >= 272 && this.currentSingleAnimation < 280));
		}

		public int getWeaponTypeFromAnimation()
		{
			if (this.currentSingleAnimation >= 272 && this.currentSingleAnimation < 280)
			{
				return 1;
			}
			if (this.currentSingleAnimation >= 232 && this.currentSingleAnimation < 264)
			{
				return 3;
			}
			return -1;
		}

		public bool isOnToolAnimation()
		{
			return (this.PauseForSingleAnimation || this.owner.UsingTool) && ((this.currentSingleAnimation >= 160 && this.currentSingleAnimation < 192) || (this.currentSingleAnimation >= 232 && this.currentSingleAnimation < 264) || (this.currentSingleAnimation >= 272 && this.currentSingleAnimation < 280));
		}

		private void doneWithAnimation()
		{
			if (this.CurrentFrame < 64 || this.CurrentFrame > 96)
			{
				this.CurrentFrame = this.oldFrame;
			}
			else
			{
				int currentFrame = this.CurrentFrame;
				this.CurrentFrame = currentFrame - 1;
			}
			this.interval = this.oldInterval;
			if (!Game1.eventUp)
			{
				this.owner.CanMove = true;
			}
			this.owner.Halt();
			this.PauseForSingleAnimation = false;
			this.animatingBackwards = false;
			this.ignoreDefaultActionThisTime = false;
		}

		private void currentAnimationTick()
		{
			if (this.indexInCurrentAnimation >= base.CurrentAnimation.Count)
			{
				return;
			}
			if (base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior != null && base.CurrentAnimation[this.indexInCurrentAnimation].behaviorAtEndOfFrame)
			{
				base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior(this.owner);
			}
			this.indexInCurrentAnimation++;
			if (this.loopThisAnimation)
			{
				this.indexInCurrentAnimation %= base.CurrentAnimation.Count;
			}
			else if (this.indexInCurrentAnimation >= base.CurrentAnimation.Count)
			{
				this.loopThisAnimation = false;
				return;
			}
			if (base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior != null && !base.CurrentAnimation[this.indexInCurrentAnimation].behaviorAtEndOfFrame)
			{
				base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior(this.owner);
			}
			if (base.CurrentAnimation != null && this.indexInCurrentAnimation < base.CurrentAnimation.Count)
			{
				this.currentSingleAnimationInterval = (float)base.CurrentAnimation[this.indexInCurrentAnimation].milliseconds;
				this.CurrentFrame = base.CurrentAnimation[this.indexInCurrentAnimation].frame;
				this.interval = (float)base.CurrentAnimation[this.indexInCurrentAnimation].milliseconds;
				return;
			}
			this.owner.completelyStopAnimatingOrDoingAction();
			this.owner.forceCanMove();
		}

		public override void UpdateSourceRect()
		{
			base.SourceRect = new Rectangle(this.CurrentFrame * this.spriteWidth % 96, this.CurrentFrame * this.spriteWidth / 96 * this.spriteHeight, this.spriteWidth, this.spriteHeight);
		}

		private new void animateOnce(GameTime time)
		{
			if (this.freezeUntilDialogueIsOver || this.owner == null)
			{
				return;
			}
			this.timer += (float)time.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval * this.intervalModifier)
			{
				this.currentAnimationTick();
				this.timer = 0f;
				int currentFrame;
				if (this.indexInCurrentAnimation > this.currentAnimationFrames - 1)
				{
					if (this.owner.IsMainPlayer)
					{
						if (this.CurrentAnimationFrame.frameBehavior != null && this.CurrentAnimationFrame.behaviorAtEndOfFrame)
						{
							this.CurrentAnimationFrame.frameBehavior(this.owner);
						}
						if (this.endOfAnimationFunction != null && !this.ignoreDefaultActionThisTime)
						{
							AnimatedSprite.endOfAnimationBehavior arg_D2_0 = this.endOfAnimationFunction;
							this.endOfAnimationFunction = null;
							arg_D2_0(this.owner);
							if (this.owner.UsingTool && this.owner.CurrentTool.Name.Equals("Fishing Rod"))
							{
								this.PauseForSingleAnimation = false;
								this.interval = this.oldInterval;
								if (this.owner.IsMainPlayer)
								{
									this.owner.CanMove = false;
									return;
								}
							}
							else if (!(this.owner.CurrentTool is MeleeWeapon) || (this.owner.CurrentTool as MeleeWeapon).type != 1)
							{
								this.doneWithAnimation();
							}
							return;
						}
						if (!this.ignoreDefaultActionThisTime && (this.currentSingleAnimation < 160 || this.currentSingleAnimation >= 192) && (this.currentSingleAnimation < 200 || this.currentSingleAnimation >= 216) && (this.currentSingleAnimation < 232 || this.currentSingleAnimation >= 264) && this.currentSingleAnimation >= 272)
						{
							int arg_1CD_0 = this.currentSingleAnimation;
						}
						this.doneWithAnimation();
						if (Game1.isEating)
						{
							Game1.doneEating();
						}
					}
					else if ((this.owner.UsingTool && this.owner.CurrentTool is FishingRod) || (this.currentToolIndex >= 48 && this.currentToolIndex <= 55))
					{
						this.PauseForSingleAnimation = false;
						this.interval = this.oldInterval;
						currentFrame = this.CurrentFrame;
						this.CurrentFrame = currentFrame - 1;
					}
					else
					{
						this.doneWithAnimation();
					}
				}
				currentFrame = this.currentSingleAnimation;
				if (currentFrame <= 168)
				{
					if (currentFrame <= 161)
					{
						if (currentFrame != 160 && currentFrame != 161)
						{
							goto IL_339;
						}
					}
					else if (currentFrame != 165)
					{
						if (currentFrame != 168)
						{
							goto IL_339;
						}
						goto IL_339;
					}
					if (this.owner.CurrentTool != null)
					{
						this.owner.CurrentTool.Update(2, this.indexInCurrentAnimation, this.owner);
					}
				}
				else if (currentFrame <= 184)
				{
					switch (currentFrame)
					{
					case 172:
					case 173:
					case 174:
					case 175:
						goto IL_339;
					case 176:
						break;
					default:
						switch (currentFrame)
						{
						case 180:
						case 181:
							break;
						case 182:
						case 183:
						case 184:
							goto IL_339;
						default:
							goto IL_339;
						}
						break;
					}
					if (this.owner.CurrentTool != null)
					{
						this.owner.CurrentTool.Update(0, this.indexInCurrentAnimation, this.owner);
					}
				}
				else if (currentFrame != 188 && currentFrame != 189)
				{
				}
				IL_339:
				if (this.CurrentFrame == 109)
				{
					Game1.playSound("eat");
				}
				if (!this.owner.IsMainPlayer && this.isOnToolAnimation() && !this.isUsingWeapon() && this.indexInCurrentAnimation == 4 && this.currentToolIndex % 2 == 0 && !(this.owner.CurrentTool is FishingRod))
				{
					this.currentToolIndex++;
				}
			}
			this.UpdateSourceRect();
		}

		private void checkForFootstep()
		{
			if (Game1.player.isRidingHorse())
			{
				return;
			}
			if (((this.currentSingleAnimation >= 32 && this.currentSingleAnimation <= 56) || (this.currentSingleAnimation >= 128 && this.currentSingleAnimation <= 152)) && this.indexInCurrentAnimation % 4 == 0)
			{
				Vector2 vector = (this.owner != null) ? this.owner.getTileLocation() : Game1.player.getTileLocation();
				if (Game1.currentLocation.IsOutdoors || Game1.currentLocation.Name.ToLower().Contains("mine") || Game1.currentLocation.Name.ToLower().Contains("cave") || Game1.currentLocation.Name.Equals("Greenhouse"))
				{
					string text = Game1.currentLocation.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Type", "Buildings");
					if (text == null || text.Length < 1)
					{
						text = Game1.currentLocation.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Type", "Back");
					}
					if (text != null)
					{
						if (!(text == "Dirt"))
						{
							if (!(text == "Stone"))
							{
								if (!(text == "Grass"))
								{
									if (text == "Wood")
									{
										this.currentStep = "woodyStep";
									}
								}
								else
								{
									this.currentStep = (Game1.currentSeason.Equals("winter") ? "snowyStep" : "grassyStep");
								}
							}
							else
							{
								this.currentStep = "stoneStep";
							}
						}
						else
						{
							this.currentStep = "sandyStep";
						}
					}
				}
				else
				{
					this.currentStep = "thudStep";
				}
				if (Game1.currentLocation.terrainFeatures.ContainsKey(vector) && Game1.currentLocation.terrainFeatures[vector].GetType() == typeof(Flooring))
				{
					this.currentStep = ((Flooring)Game1.currentLocation.terrainFeatures[vector]).getFootstepSound();
				}
				if (this.currentStep.Equals("sandyStep"))
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(128, 2948, 64, 64), 80f, 8, 0, new Vector2(this.owner.position.X + (float)(Game1.tileSize / 4) + (float)Game1.random.Next(-8, 8), this.owner.position.Y + (float)(Game1.random.Next(-3, -1) * Game1.pixelZoom)), false, Game1.random.NextDouble() < 0.5, this.owner.position.Y / 10000f, 0.03f, Color.Khaki * 0.45f, 0.75f + (float)Game1.random.Next(-3, 4) * 0.05f, 0f, 0f, 0f, false));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(128, 2948, 64, 64), 80f, 8, 0, new Vector2(this.owner.position.X + (float)(Game1.tileSize / 4) + (float)Game1.random.Next(-4, 4), this.owner.position.Y + (float)(Game1.random.Next(-3, -1) * Game1.pixelZoom)), false, Game1.random.NextDouble() < 0.5, this.owner.position.Y / 10000f, 0.03f, Color.Khaki * 0.45f, 0.55f + (float)Game1.random.Next(-3, 4) * 0.05f, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = 20
					});
				}
				else if (this.currentStep.Equals("snowyStep"))
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(247, 407, 6, 6), 2000f, 1, 10000, new Vector2(this.owner.position.X + (float)(Game1.pixelZoom * 6) + (float)(Game1.random.Next(-4, 4) * Game1.pixelZoom), this.owner.position.Y + (float)(Game1.pixelZoom * 2) + (float)(Game1.random.Next(-4, 4) * Game1.pixelZoom)), false, false, this.owner.position.Y / 1E+07f, 0.01f, Color.White, (float)(Game1.pixelZoom * 3) / 4f + (float)Game1.random.NextDouble(), 0f, (this.owner.facingDirection == 1 || this.owner.facingDirection == 3) ? -0.7853982f : 0f, 0f, false));
				}
				if (this.currentStep != null)
				{
					Game1.playSound(this.currentStep);
				}
				Stats expr_538 = Game1.stats;
				uint stepsTaken = expr_538.StepsTaken;
				expr_538.StepsTaken = stepsTaken + 1u;
				return;
			}
			if (this.CurrentFrame == 4 || this.CurrentFrame == 12 || this.CurrentFrame == 20 || this.CurrentFrame == 28 || this.CurrentFrame == 100 || this.CurrentFrame == 108 || this.CurrentFrame == 116 || this.CurrentFrame == 124)
			{
				Stats expr_59C = Game1.stats;
				uint stepsTaken = expr_59C.StepsTaken;
				expr_59C.StepsTaken = stepsTaken + 1u;
			}
		}

		public static void checkForFootstep(Character who)
		{
			if (who != null)
			{
				Game1.currentLocation.playTerrainSound(who.getTileLocation(), who, true);
				return;
			}
			string a = Game1.currentLocation.doesTileHaveProperty((int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y, "Type", "Back");
			if (a == "Stone")
			{
				Game1.playSound("stoneStep");
				Rumble.rumble(0.1f, 50f);
				return;
			}
			if (!(a == "Wood"))
			{
				Game1.playSound("thudStep");
				Rumble.rumble(0.3f, 50f);
				return;
			}
			Game1.playSound("woodyStep");
			Rumble.rumble(0.1f, 50f);
		}

		private void animateBackwardsOnce(GameTime time)
		{
			this.timer += (float)time.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.currentSingleAnimationInterval)
			{
				int currentFrame = this.CurrentFrame;
				this.CurrentFrame = currentFrame - 1;
				this.timer = 0f;
				if (this.indexInCurrentAnimation > this.currentAnimationFrames - 1)
				{
					if (this.CurrentFrame < 63 || this.CurrentFrame > 96)
					{
						this.CurrentFrame = this.oldFrame;
					}
					else
					{
						this.CurrentFrame = this.CurrentFrame % 16 + 8;
					}
					this.interval = this.oldInterval;
					this.PauseForSingleAnimation = false;
					this.animatingBackwards = false;
					if (!Game1.eventUp)
					{
						this.owner.CanMove = true;
					}
					if (this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Fishing Rod"))
					{
						this.owner.usingTool = false;
					}
					this.owner.Halt();
					if (((this.CurrentSingleAnimation >= 160 && this.CurrentSingleAnimation < 192) || (this.CurrentSingleAnimation >= 200 && this.CurrentSingleAnimation < 216) || (this.CurrentSingleAnimation >= 232 && this.CurrentSingleAnimation < 264)) && this.owner.IsMainPlayer)
					{
						Game1.toolAnimationDone();
					}
				}
				if (this.owner.UsingTool && this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Fishing Rod"))
				{
					currentFrame = this.CurrentFrame;
					if (currentFrame <= 168)
					{
						if (currentFrame != 164)
						{
							if (currentFrame == 168)
							{
								this.owner.CurrentTool.Update(1, 0);
							}
						}
						else
						{
							this.owner.CurrentTool.Update(2, 0);
						}
					}
					else if (currentFrame != 180)
					{
						if (currentFrame == 184)
						{
							this.owner.CurrentTool.Update(3, 0);
						}
					}
					else
					{
						this.owner.CurrentTool.Update(0, 0);
					}
				}
			}
			this.UpdateSourceRect();
		}

		public int frameOfCurrentSingleAnimation()
		{
			if (this.PauseForSingleAnimation)
			{
				return this.CurrentFrame - (this.currentSingleAnimation - this.currentSingleAnimation % 8);
			}
			if (!Game1.pickingTool && this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Watering Can"))
			{
				return 4;
			}
			if (!Game1.pickingTool && this.owner.UsingTool && ((this.currentToolIndex >= 48 && this.currentToolIndex <= 55) || (this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Fishing Rod"))))
			{
				return 6;
			}
			return 0;
		}

		public void setCurrentSingleAnimation(int which)
		{
			this.CurrentFrame = which;
			this.currentSingleAnimation = which;
			base.CurrentAnimation = FarmerSprite.getAnimationFromIndex(which, this, 100, 1, false, false).ToList<FarmerSprite.AnimationFrame>();
			if (base.CurrentAnimation != null && base.CurrentAnimation.Count > 0)
			{
				this.currentAnimationFrames = base.CurrentAnimation.Count;
				this.interval = (float)base.CurrentAnimation.First<FarmerSprite.AnimationFrame>().milliseconds;
				this.CurrentFrame = base.CurrentAnimation.First<FarmerSprite.AnimationFrame>().frame;
			}
			if (this.interval <= 50f)
			{
				this.interval = 800f;
			}
			this.UpdateSourceRect();
		}

		private void animate(int Milliseconds, int firstFrame, int lastFrame)
		{
			if (this.CurrentFrame > lastFrame || this.CurrentFrame < firstFrame)
			{
				this.CurrentFrame = firstFrame;
			}
			this.timer += (float)Milliseconds;
			if (this.timer > this.interval * this.intervalModifier)
			{
				int currentFrame = this.CurrentFrame;
				this.CurrentFrame = currentFrame + 1;
				this.timer = 0f;
				if (this.CurrentFrame > lastFrame)
				{
					this.CurrentFrame = firstFrame;
				}
				this.checkForFootstep();
			}
			this.UpdateSourceRect();
		}

		private void animate(int Milliseconds)
		{
			this.timer += (float)Milliseconds;
			if (this.timer > this.interval * this.intervalModifier)
			{
				this.currentAnimationTick();
				this.timer = 0f;
				this.checkForFootstep();
			}
			this.UpdateSourceRect();
		}

		public override void StopAnimation()
		{
			if (!this.pauseForSingleAnimation)
			{
				this.interval = 0f;
				if (this.CurrentFrame >= 64 && this.CurrentFrame <= 155 && this.owner != null && !this.owner.bathingClothes)
				{
					switch (this.owner.facingDirection)
					{
					case 0:
						this.CurrentFrame = 12;
						break;
					case 1:
						this.CurrentFrame = 6;
						break;
					case 2:
						this.CurrentFrame = 0;
						break;
					case 3:
						this.CurrentFrame = 6;
						break;
					}
				}
				else if (!Game1.pickingTool)
				{
					if (this.owner != null)
					{
						switch (this.owner.facingDirection)
						{
						case 0:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(112, 1);
							}
							else
							{
								this.setCurrentFrame(16, 1);
							}
							break;
						case 1:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(104, 1);
							}
							else
							{
								this.setCurrentFrame(8, 1);
							}
							break;
						case 2:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(96, 1);
							}
							else
							{
								this.setCurrentFrame(0, 1);
							}
							break;
						case 3:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(120, 1);
							}
							else
							{
								this.setCurrentFrame(24, 1);
							}
							break;
						}
					}
					this.currentSingleAnimation = -1;
				}
				this.indexInCurrentAnimation = 0;
				this.UpdateSourceRect();
			}
		}

		public static FarmerSprite.AnimationFrame[] getAnimationFromIndex(int index, FarmerSprite requester, int interval, int numberOfFrames, bool flip, bool secondaryArm)
		{
			bool secondaryArm2 = (index >= 96 && index < 160) || index == 232 || index == 248;
			if (requester.owner != null && requester.owner.ActiveObject != null && requester.owner.ActiveObject is Furniture)
			{
				secondaryArm2 = false;
			}
			requester.loopThisAnimation = true;
			int num = 0;
			if (requester.owner != null && requester.owner.bathingClothes)
			{
				num += 108;
			}
			if (index <= 88)
			{
				if (index <= 43)
				{
					if (index <= 16)
					{
						if (index <= 0)
						{
							if (index == -1)
							{
								return new FarmerSprite.AnimationFrame[]
								{
									new FarmerSprite.AnimationFrame(0, 100, secondaryArm2, false, null, false)
								};
							}
							if (index != 0)
							{
								goto IL_2425;
							}
						}
						else
						{
							if (index == 8)
							{
								goto IL_489;
							}
							if (index != 16)
							{
								goto IL_2425;
							}
							goto IL_4F0;
						}
					}
					else if (index <= 32)
					{
						if (index == 24)
						{
							goto IL_55B;
						}
						if (index != 32)
						{
							goto IL_2425;
						}
						goto IL_5C2;
					}
					else
					{
						if (index == 40)
						{
							goto IL_675;
						}
						if (index != 43)
						{
							goto IL_2425;
						}
						flip = (requester.owner.FacingDirection == 3);
						goto IL_2425;
					}
				}
				else
				{
					if (index > 71)
					{
						if (index <= 83)
						{
							if (index != 72)
							{
								switch (index)
								{
								case 79:
									break;
								case 80:
									goto IL_1606;
								case 81:
								case 82:
									goto IL_2425;
								case 83:
									requester.loopThisAnimation = false;
									return new FarmerSprite.AnimationFrame[]
									{
										new FarmerSprite.AnimationFrame(0, 0, false, false, null, false)
									};
								default:
									goto IL_2425;
								}
							}
							requester.loopThisAnimation = false;
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(6, 0, false, false, null, false)
							};
						}
						if (index != 87)
						{
							if (index != 88)
							{
								goto IL_2425;
							}
							goto IL_1667;
						}
						IL_1606:
						requester.loopThisAnimation = false;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(12, 0, false, false, null, false)
						};
					}
					if (index <= 56)
					{
						if (index == 48)
						{
							goto IL_72C;
						}
						if (index != 56)
						{
							goto IL_2425;
						}
						goto IL_7E5;
					}
					else
					{
						if (index != 64 && index != 71)
						{
							goto IL_2425;
						}
						requester.loopThisAnimation = false;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(0, 0, false, false, null, false)
						};
					}
				}
			}
			else if (index <= 152)
			{
				if (index <= 120)
				{
					if (index <= 104)
					{
						switch (index)
						{
						case 95:
							goto IL_1667;
						case 96:
							break;
						case 97:
							requester.loopThisAnimation = false;
							flip = (requester.owner.FacingDirection == 3);
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(97, 800, false, flip, null, false)
							};
						default:
							if (index != 104)
							{
								goto IL_2425;
							}
							goto IL_489;
						}
					}
					else
					{
						if (index == 112)
						{
							goto IL_4F0;
						}
						if (index != 120)
						{
							goto IL_2425;
						}
						goto IL_55B;
					}
				}
				else if (index <= 136)
				{
					if (index == 128)
					{
						goto IL_5C2;
					}
					if (index != 136)
					{
						goto IL_2425;
					}
					goto IL_675;
				}
				else
				{
					if (index == 144)
					{
						goto IL_72C;
					}
					if (index != 152)
					{
						goto IL_2425;
					}
					goto IL_7E5;
				}
			}
			else if (index <= 184)
			{
				if (index <= 168)
				{
					if (index == 160)
					{
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(66, 150, false, false, null, false),
							new FarmerSprite.AnimationFrame(67, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(68, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
							new FarmerSprite.AnimationFrame(69, (int)((short)(170 + requester.owner.toolPower * 30)), false, false, null, false),
							new FarmerSprite.AnimationFrame(70, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					}
					switch (index)
					{
					case 164:
					case 166:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(54, 0, false, false, null, false),
							new FarmerSprite.AnimationFrame(54, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(55, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
							new FarmerSprite.AnimationFrame(25, 500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					case 165:
					case 167:
						goto IL_2425;
					case 168:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(48, 100, false, false, null, false),
							new FarmerSprite.AnimationFrame(49, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(50, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
							new FarmerSprite.AnimationFrame(51, (int)((short)(220 + requester.owner.toolPower * 30)), false, false, null, false),
							new FarmerSprite.AnimationFrame(52, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					default:
						goto IL_2425;
					}
				}
				else
				{
					switch (index)
					{
					case 172:
					case 174:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(58, 0, false, false, null, false),
							new FarmerSprite.AnimationFrame(58, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(59, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
							new FarmerSprite.AnimationFrame(45, 500, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					case 173:
					case 175:
						goto IL_2425;
					case 176:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(36, 100, false, false, null, false),
							new FarmerSprite.AnimationFrame(37, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(38, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
							new FarmerSprite.AnimationFrame(63, (int)((short)(220 + requester.owner.toolPower * 30)), false, false, null, false),
							new FarmerSprite.AnimationFrame(62, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					default:
						switch (index)
						{
						case 180:
						case 182:
							requester.loopThisAnimation = false;
							requester.ignoreDefaultActionThisTime = true;
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(62, 0, false, false, null, false),
								new FarmerSprite.AnimationFrame(62, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
								new FarmerSprite.AnimationFrame(63, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
								new FarmerSprite.AnimationFrame(46, 500, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
							};
						case 181:
						case 183:
							goto IL_2425;
						case 184:
							requester.loopThisAnimation = false;
							requester.ignoreDefaultActionThisTime = true;
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(48, 100, false, true, null, false),
								new FarmerSprite.AnimationFrame(49, 40, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
								new FarmerSprite.AnimationFrame(50, 40, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
								new FarmerSprite.AnimationFrame(51, (int)((short)(220 + requester.owner.toolPower * 30)), false, true, null, false),
								new FarmerSprite.AnimationFrame(52, 75, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
							};
						default:
							goto IL_2425;
						}
						break;
					}
				}
			}
			else if (index <= 216)
			{
				switch (index)
				{
				case 188:
				case 190:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 0, false, true, null, false),
						new FarmerSprite.AnimationFrame(58, 75, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(59, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
						new FarmerSprite.AnimationFrame(45, 500, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 189:
				case 191:
				case 193:
				case 195:
				case 197:
					goto IL_2425;
				case 192:
					index = 3;
					interval = 500;
					goto IL_2425;
				case 194:
					index = 9;
					interval = 500;
					goto IL_2425;
				case 196:
					index = 15;
					interval = 500;
					goto IL_2425;
				case 198:
					index = 9;
					flip = true;
					interval = 500;
					goto IL_2425;
				default:
					if (index != 216)
					{
						goto IL_2425;
					}
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(0, 0),
						new FarmerSprite.AnimationFrame(84, (requester.owner.mostRecentlyGrabbedItem != null && requester.owner.mostRecentlyGrabbedItem is Object && (requester.owner.mostRecentlyGrabbedItem as Object).ParentSheetIndex == 434) ? 1000 : 250, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false),
						new FarmerSprite.AnimationFrame(85, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false),
						new FarmerSprite.AnimationFrame(86, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), true),
						new FarmerSprite.AnimationFrame(86, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), true),
						new FarmerSprite.AnimationFrame(87, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(88, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(87, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(88, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(87, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(0, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false)
					};
				}
			}
			else
			{
				switch (index)
				{
				case 224:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false)
					};
				case 225:
				case 226:
				case 227:
				case 228:
				case 229:
				case 230:
				case 231:
				case 233:
				case 235:
				case 236:
				case 237:
				case 238:
				case 239:
				case 241:
				case 244:
				case 245:
				case 246:
				case 247:
				case 249:
				case 250:
				case 251:
				case 253:
				case 254:
				case 255:
				case 257:
				case 260:
				case 261:
				case 262:
				case 263:
				case 264:
				case 265:
				case 266:
				case 267:
				case 268:
				case 269:
				case 270:
				case 271:
				case 273:
				case 275:
				case 277:
					goto IL_2425;
				case 232:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(24, 55, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(25, 45, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(26, 25, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(27, 25, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(28, 25, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(29, (int)((short)interval * 2), secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(29, 0, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 234:
					index = 28;
					secondaryArm = true;
					goto IL_2425;
				case 240:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(30, 55, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(31, 45, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(32, 25, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 25, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(34, 25, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, (int)((short)interval * 2), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, 0, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 242:
				case 243:
					index = 34;
					goto IL_2425;
				case 248:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(36, 55, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(37, 45, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(38, 25, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(39, 25, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(40, 25, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(41, (int)((short)interval * 2), secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(41, 0, secondaryArm2, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 252:
					index = 40;
					secondaryArm = true;
					goto IL_2425;
				case 256:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(30, 55, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(31, 45, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(32, 25, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 25, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(34, 25, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, (int)((short)interval * 2), true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, 0, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 258:
				case 259:
					index = 34;
					flip = true;
					goto IL_2425;
				case 272:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(25, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(27, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(27, 0, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 274:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(34, (int)((short)interval), false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, (int)((short)interval), false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 276:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(40, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(38, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(38, 0, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 278:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(34, (int)((short)interval), false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, (int)((short)interval), false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 0, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 279:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(62, 0, false, false, null, false),
						new FarmerSprite.AnimationFrame(62, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(63, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(64, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(65, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(65, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 280:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 0, false, false, null, false),
						new FarmerSprite.AnimationFrame(58, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(59, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(60, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 281:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(54, 0, false, false, null, false),
						new FarmerSprite.AnimationFrame(54, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(55, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(56, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(57, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(57, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 282:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 0, false, true, null, false),
						new FarmerSprite.AnimationFrame(58, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(59, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(60, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 200, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 0, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 283:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(82, 400),
						new FarmerSprite.AnimationFrame(83, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(82, 400),
						new FarmerSprite.AnimationFrame(83, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 284:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(80, 400),
						new FarmerSprite.AnimationFrame(81, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(80, 400),
						new FarmerSprite.AnimationFrame(81, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 285:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(78, 400),
						new FarmerSprite.AnimationFrame(79, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(78, 400),
						new FarmerSprite.AnimationFrame(79, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 286:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(80, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(81, 400, false, true, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(80, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(81, 400, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 287:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(62, 400),
						new FarmerSprite.AnimationFrame(63, 400),
						new FarmerSprite.AnimationFrame(62, 400),
						new FarmerSprite.AnimationFrame(63, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 288:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 400),
						new FarmerSprite.AnimationFrame(59, 400),
						new FarmerSprite.AnimationFrame(58, 400),
						new FarmerSprite.AnimationFrame(59, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 289:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(54, 400),
						new FarmerSprite.AnimationFrame(55, 400),
						new FarmerSprite.AnimationFrame(54, 400),
						new FarmerSprite.AnimationFrame(55, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 290:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(59, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(58, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(59, 400, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 291:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(16, 1500),
						new FarmerSprite.AnimationFrame(16, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.completelyStopAnimating), false)
					};
				case 292:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(16, 500),
						new FarmerSprite.AnimationFrame(0, 500),
						new FarmerSprite.AnimationFrame(16, 500),
						new FarmerSprite.AnimationFrame(0, 500),
						new FarmerSprite.AnimationFrame(0, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.completelyStopAnimating), false)
					};
				case 293:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(16, 1000),
						new FarmerSprite.AnimationFrame(0, 500),
						new FarmerSprite.AnimationFrame(16, 1000),
						new FarmerSprite.AnimationFrame(4, 200),
						new FarmerSprite.AnimationFrame(5, 2000, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.doSleepEmote), false),
						new FarmerSprite.AnimationFrame(5, 2000, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.passOutFromTired), false),
						new FarmerSprite.AnimationFrame(5, 2000)
					};
				case 294:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(0, 1),
						new FarmerSprite.AnimationFrame(90, 250),
						new FarmerSprite.AnimationFrame(91, 150),
						new FarmerSprite.AnimationFrame(92, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(93, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.drinkGlug), false),
						new FarmerSprite.AnimationFrame(92, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(93, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.drinkGlug), false),
						new FarmerSprite.AnimationFrame(92, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(93, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.drinkGlug), false),
						new FarmerSprite.AnimationFrame(91, 250),
						new FarmerSprite.AnimationFrame(90, 50)
					};
				case 295:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(76, 100, false, false, null, false),
						new FarmerSprite.AnimationFrame(38, 40, false, false, null, false),
						new FarmerSprite.AnimationFrame(63, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(62, 80, false, false, null, false),
						new FarmerSprite.AnimationFrame(63, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 296:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(48, 100, false, false, null, false),
						new FarmerSprite.AnimationFrame(49, 40, false, false, null, false),
						new FarmerSprite.AnimationFrame(50, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(51, 80, false, false, null, false),
						new FarmerSprite.AnimationFrame(52, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 297:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(66, 100, false, false, null, false),
						new FarmerSprite.AnimationFrame(67, 40, false, false, null, false),
						new FarmerSprite.AnimationFrame(68, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(69, 80, false, false, null, false),
						new FarmerSprite.AnimationFrame(70, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 298:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(48, 100, false, true, null, false),
						new FarmerSprite.AnimationFrame(49, 40, false, true, null, false),
						new FarmerSprite.AnimationFrame(50, 40, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(51, 80, false, true, null, false),
						new FarmerSprite.AnimationFrame(52, 200, false, true, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 299:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(76, 5000, false, false, null, false)
					};
				case 300:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(72, 5000, false, false, null, false)
					};
				case 301:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(74, 5000, false, false, null, false)
					};
				case 302:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(72, 5000, false, true, null, false)
					};
				case 303:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(125, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(125, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(125, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 500, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 304:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(84, 99999999, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false)
					};
				default:
					if (index != 999996)
					{
						goto IL_2425;
					}
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(96, 800, false, false, null, false)
					};
				}
			}
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(1 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(2 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(num, 200, secondaryArm2, false, null, false)
			};
			IL_489:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(7 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(6 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(8 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(6 + num, 200, secondaryArm2, false, null, false)
			};
			IL_4F0:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(13 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(12 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(14 + num, 200, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(12 + num, 200, secondaryArm2, false, null, false)
			};
			IL_55B:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(7 + num, 200, secondaryArm2, true, null, false),
				new FarmerSprite.AnimationFrame(6 + num, 200, secondaryArm2, true, null, false),
				new FarmerSprite.AnimationFrame(8 + num, 200, secondaryArm2, true, null, false),
				new FarmerSprite.AnimationFrame(6 + num, 200, secondaryArm2, true, null, false)
			};
			IL_5C2:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(0, 90, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(1, 60, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(18, 120, -4, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(1, 60, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(0, 90, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(2, 60, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(19, 120, -4, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(2, 60, -2, secondaryArm2, false, null, false, 0)
			};
			IL_675:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(6, 80, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(20, 140, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(11, 100, 0, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(6, 80, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(21, 140, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(17, 100, 0, secondaryArm2, false, null, false, 0)
			};
			IL_72C:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(12, 90, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(13, 60, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(22, 120, -3, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(13, 60, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(12, 90, secondaryArm2, false, null, false),
				new FarmerSprite.AnimationFrame(14, 60, -2, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(23, 120, -3, secondaryArm2, false, null, false, 0),
				new FarmerSprite.AnimationFrame(14, 60, -2, secondaryArm2, false, null, false, 0)
			};
			IL_7E5:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(6, 80, secondaryArm2, true, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, secondaryArm2, true, null, false, 0),
				new FarmerSprite.AnimationFrame(20, 140, -2, secondaryArm2, true, null, false, 0),
				new FarmerSprite.AnimationFrame(11, 100, 0, secondaryArm2, true, null, false, 0),
				new FarmerSprite.AnimationFrame(6, 80, secondaryArm2, true, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, secondaryArm2, true, null, false, 0),
				new FarmerSprite.AnimationFrame(21, 140, -2, secondaryArm2, true, null, false, 0),
				new FarmerSprite.AnimationFrame(17, 100, 0, secondaryArm2, true, null, false, 0)
			};
			IL_1667:
			requester.loopThisAnimation = false;
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(6, 0, false, true, null, false)
			};
			IL_2425:
			if (index > FarmerRenderer.featureYOffsetPerFrame.Length - 1)
			{
				index = 0;
			}
			requester.loopThisAnimation = false;
			FarmerSprite.AnimationFrame[] array = new FarmerSprite.AnimationFrame[numberOfFrames];
			for (int i = 0; i < numberOfFrames; i++)
			{
				array[i] = new FarmerSprite.AnimationFrame((int)((short)(i + index)), (int)((short)interval), secondaryArm, flip, null, false);
			}
			return array;
		}
	}
}
