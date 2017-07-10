using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace StardewValley.Tools
{
	public class FishingRod : Tool
	{
		public const int sizeOfLandCheckRectangle = 11;

		private Vector2 bobber;

		public static int minFishingBiteTime = 600;

		public static int maxFishingBiteTime = 30000;

		public static int minTimeToNibble = 340;

		public static int maxTimeToNibble = 800;

		public static double baseChanceForTreasure = 0.15;

		private int bobberBob;

		[XmlIgnore]
		public float bobberTimeAccumulator;

		[XmlIgnore]
		public float timePerBobberBob = 2000f;

		[XmlIgnore]
		public float timeUntilFishingBite = -1f;

		[XmlIgnore]
		public float fishingBiteAccumulator;

		[XmlIgnore]
		public float fishingNibbleAccumulator;

		[XmlIgnore]
		public float timeUntilFishingNibbleDone = -1f;

		[XmlIgnore]
		public float castingPower;

		[XmlIgnore]
		public float castingChosenCountdown;

		[XmlIgnore]
		public float castingTimerSpeed = 0.001f;

		[XmlIgnore]
		public float fishWiggle;

		[XmlIgnore]
		public float fishWiggleIntensity;

		[XmlIgnore]
		public bool isFishing;

		[XmlIgnore]
		public bool hit;

		[XmlIgnore]
		public bool isNibbling;

		[XmlIgnore]
		public bool favBait;

		[XmlIgnore]
		public bool isTimingCast;

		[XmlIgnore]
		public bool isCasting;

		[XmlIgnore]
		public bool castedButBobberStillInAir;

		[XmlIgnore]
		public bool doneWithAnimation;

		[XmlIgnore]
		public bool hasDoneFucntionYet;

		[XmlIgnore]
		public bool pullingOutOfWater;

		[XmlIgnore]
		public bool isReeling;

		[XmlIgnore]
		public bool fishCaught;

		[XmlIgnore]
		public bool recordSize;

		[XmlIgnore]
		public bool treasureCaught;

		[XmlIgnore]
		public bool showingTreasure;

		[XmlIgnore]
		public bool hadBobber;

		[XmlIgnore]
		public bool bossFish;

		[XmlIgnore]
		public List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();

		[XmlIgnore]
		public SparklingText sparklingText;

		[XmlIgnore]
		private int fishSize;

		[XmlIgnore]
		private int whichFish;

		[XmlIgnore]
		private int fishQuality;

		[XmlIgnore]
		private int clearWaterDistance;

		[XmlIgnore]
		private int originalFacingDirection;

		public static Cue chargeSound;

		public static Cue reelSound;

		private bool usedGamePadToCast;

		public override string DisplayName
		{
			get
			{
				switch (this.upgradeLevel)
				{
				case 0:
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14045", new object[0]);
				case 1:
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14046", new object[0]);
				case 2:
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14047", new object[0]);
				case 3:
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14048", new object[0]);
				default:
					return this.displayName;
				}
			}
		}

		public override string Name
		{
			get
			{
				switch (this.upgradeLevel)
				{
				case 0:
					return "Bamboo Pole";
				case 1:
					return "Yew Rod";
				case 2:
					return "Fiberglass Rod";
				case 3:
					return "Iridium Rod";
				default:
					return this.name;
				}
			}
		}

		public FishingRod() : base("Fishing Rod", 0, 189, 8, false, 0)
		{
			this.numAttachmentSlots = 2;
			this.attachments = new StardewValley.Object[this.numAttachmentSlots];
			this.indexOfMenuItemView = 8 + this.upgradeLevel;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14041", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14042", new object[0]);
		}

		public override int salePrice()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return 500;
			case 1:
				return 2000;
			case 2:
				return 5000;
			case 3:
				return 15000;
			default:
				return 500;
			}
		}

		public override int attachmentSlots()
		{
			if (this.upgradeLevel > 2)
			{
				return 2;
			}
			if (this.upgradeLevel <= 0)
			{
				return 0;
			}
			return 1;
		}

		public FishingRod(int upgradeLevel) : base("Fishing Rod", upgradeLevel, 189, 8, false, 0)
		{
			this.numAttachmentSlots = 2;
			this.attachments = new StardewValley.Object[this.numAttachmentSlots];
			this.indexOfMenuItemView = 8 + upgradeLevel;
			this.upgradeLevel = upgradeLevel;
		}

		private int getAddedDistance(Farmer who)
		{
			if (who.FishingLevel >= 8)
			{
				return 3;
			}
			if (who.FishingLevel >= 4)
			{
				return 2;
			}
			if (who.FishingLevel >= 1)
			{
				return 1;
			}
			return 0;
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			if (this.fishCaught)
			{
				return;
			}
			this.hasDoneFucntionYet = true;
			int num = (int)(this.bobber.X / (float)Game1.tileSize);
			int num2 = (int)((this.bobber.Y - (float)(Game1.tileSize / 2)) / (float)Game1.tileSize);
			base.DoFunction(location, x, y, power, who);
			if (this.doneWithAnimation && who.IsMainPlayer)
			{
				who.canReleaseTool = true;
			}
			if (Game1.isAnyGamePadButtonBeingPressed())
			{
				Game1.lastCursorMotionWasMouse = false;
			}
			if (!this.isFishing && !this.castedButBobberStillInAir && !this.pullingOutOfWater && !this.isNibbling && !this.hit)
			{
				if (!Game1.eventUp)
				{
					float stamina = who.Stamina;
					who.Stamina -= 8f - (float)who.FishingLevel * 0.1f;
					who.checkForExhaustion(stamina);
				}
				if ((location.doesTileHaveProperty(num, num2, "Water", "Back") != null && location.doesTileHaveProperty(num, num2, "NoFishing", "Back") == null && location.getTileIndexAt(num, num2, "Buildings") == -1) || location.doesTileHaveProperty(num, num2, "Water", "Buildings") != null)
				{
					this.isFishing = true;
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 100f, 2, 1, new Vector2(this.bobber.X - (float)Game1.tileSize - 16f, this.bobber.Y - (float)Game1.tileSize - 16f), false, false));
					Game1.playSound("dropItemInWater");
					this.timeUntilFishingBite = (float)Game1.random.Next(FishingRod.minFishingBiteTime, FishingRod.maxFishingBiteTime - 250 * who.FishingLevel - ((this.attachments[1] != null && this.attachments[1].ParentSheetIndex == 686) ? 5000 : ((this.attachments[1] != null && this.attachments[1].ParentSheetIndex == 687) ? 10000 : 0)));
					this.timeUntilFishingBite *= 0.75f;
					if (this.attachments[0] != null)
					{
						this.timeUntilFishingBite *= 0.5f;
						if (this.attachments[0].parentSheetIndex == 774)
						{
							this.timeUntilFishingBite *= 0.75f;
						}
					}
					this.timeUntilFishingBite = Math.Max(500f, this.timeUntilFishingBite);
					Stats expr_279 = Game1.stats;
					uint timesFished = expr_279.TimesFished;
					expr_279.TimesFished = timesFished + 1u;
					float arg_2A3_0 = (this.bobber.X - (float)(Game1.tileSize / 2)) / (float)Game1.tileSize;
					float arg_2BF_0 = (this.bobber.Y - (float)(Game1.tileSize / 2)) / (float)Game1.tileSize;
					Point arg_2C6_0 = location.fishSplashPoint;
					Rectangle value = new Rectangle(location.fishSplashPoint.X * Game1.tileSize, location.fishSplashPoint.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					Rectangle rectangle = new Rectangle((int)this.bobber.X - Game1.tileSize * 5 / 4, (int)this.bobber.Y - Game1.tileSize * 5 / 4, Game1.tileSize, Game1.tileSize);
					if (rectangle.Intersects(value))
					{
						this.timeUntilFishingBite /= 4f;
						location.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.bobber - new Vector2((float)Game1.tileSize, (float)(Game1.tileSize * 2)), Color.Cyan, 8, false, 100f, 0, -1, -1f, -1, 0));
					}
					if (!who.IsMainPlayer)
					{
						who.Halt();
						who.FarmerSprite.PauseForSingleAnimation = false;
					}
					who.UsingTool = true;
					if (who.IsMainPlayer)
					{
						who.canMove = false;
						return;
					}
				}
				else
				{
					if (this.doneWithAnimation && who.IsMainPlayer)
					{
						who.usingTool = false;
					}
					if (this.doneWithAnimation && who.IsMainPlayer)
					{
						who.canMove = true;
						return;
					}
				}
			}
			else if (!this.isCasting && !this.pullingOutOfWater)
			{
				who.FarmerSprite.pauseForSingleAnimation = false;
				switch (who.FacingDirection)
				{
				case 0:
					who.FarmerSprite.animateBackwardsOnce(299, 35f);
					break;
				case 1:
					who.FarmerSprite.animateBackwardsOnce(300, 35f);
					break;
				case 2:
					who.FarmerSprite.animateBackwardsOnce(301, 35f);
					break;
				case 3:
					who.FarmerSprite.animateBackwardsOnce(302, 35f);
					break;
				}
				if (this.isNibbling)
				{
					double num3 = (double)((this.attachments[0] != null) ? ((float)this.attachments[0].Price / 10f) : 0f);
					Point arg_4EB_0 = location.fishSplashPoint;
					Rectangle rectangle2 = new Rectangle(location.fishSplashPoint.X * Game1.tileSize, location.fishSplashPoint.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					Rectangle value2 = new Rectangle((int)this.bobber.X - Game1.tileSize * 5 / 4, (int)this.bobber.Y - Game1.tileSize * 5 / 4, Game1.tileSize, Game1.tileSize);
					bool flag = rectangle2.Intersects(value2);
					StardewValley.Object @object = location.getFish(this.fishingNibbleAccumulator, (this.attachments[0] != null) ? this.attachments[0].ParentSheetIndex : -1, this.clearWaterDistance + (flag ? 1 : 0), this.lastUser, num3 + (flag ? 0.4 : 0.0));
					if (@object == null || @object.ParentSheetIndex <= 0)
					{
						@object = new StardewValley.Object(Game1.random.Next(167, 173), 1, false, -1, 0);
					}
					if (@object.scale.X == 1f)
					{
						this.favBait = true;
					}
					if (@object.Category == -20 || @object.ParentSheetIndex == 152 || @object.ParentSheetIndex == 153 || @object.parentSheetIndex == 157)
					{
						this.pullFishFromWater(@object.ParentSheetIndex, -1, 0, 0, false, false);
						return;
					}
					if (!this.hit)
					{
						this.hit = true;
						Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(612, 1913, 74, 30), 1500f, 1, 0, Game1.GlobalToLocal(Game1.viewport, this.bobber + new Vector2(-140f, (float)(-(float)Game1.tileSize * 5 / 2))), false, false, 1f, 0.005f, Color.White, 4f, 0.075f, 0f, 0f, true)
						{
							scaleChangeChange = -0.005f,
							motion = new Vector2(0f, -0.1f),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.startMinigameEndFunction),
							extraInfoForEndBehavior = @object.ParentSheetIndex
						});
						Game1.playSound("FishHit");
					}
					return;
				}
				else
				{
					Game1.playSound("pullItemFromWater");
					this.isFishing = false;
					this.pullingOutOfWater = true;
					if (this.lastUser.FacingDirection == 1 || this.lastUser.FacingDirection == 3)
					{
						double arg_785_0 = (double)Math.Abs(this.bobber.X - (float)this.lastUser.getStandingX());
						float num4 = 0.005f;
						float num5 = -(float)Math.Sqrt(arg_785_0 * (double)num4 / (double)2f);
						float num6 = 2f * (Math.Abs(num5 - 0.5f) / num4);
						num6 *= 1.2f;
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), num6, 1, 0, this.bobber + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 3 / 4)), false, false, (float)who.getStandingY() / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							motion = new Vector2((float)((who.FacingDirection == 3) ? -1 : 1) * (num5 + 0.2f), num5 - 0.8f),
							acceleration = new Vector2(0f, num4),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.donefishingEndFunction),
							timeBasedMotion = true,
							alphaFade = 0.001f
						});
					}
					else
					{
						float num7 = this.bobber.Y - (float)this.lastUser.getStandingY();
						float num8 = Math.Abs(num7 + (float)(Game1.tileSize * 4));
						float num9 = 0.005f;
						float num10 = (float)Math.Sqrt((double)(2f * num9 * num8));
						float animationInterval = (float)(Math.Sqrt((double)(2f * (num8 - num7) / num9)) + (double)(num10 / num9));
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), animationInterval, 1, 0, this.bobber + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 3 / 4)), false, false, this.bobber.Y / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							motion = new Vector2(0f, -num10),
							acceleration = new Vector2(0f, num9),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.donefishingEndFunction),
							timeBasedMotion = true,
							alphaFade = 0.001f
						});
					}
					who.UsingTool = true;
					who.canReleaseTool = false;
				}
			}
		}

		public Color getColor()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return Color.Goldenrod;
			case 1:
				return Color.RosyBrown;
			case 2:
				return Color.White;
			case 3:
				return Color.Violet;
			default:
				return Color.White;
			}
		}

		public static int distanceToLand(int tileX, int tileY, GameLocation location)
		{
			Rectangle rectangle = new Rectangle(tileX - 1, tileY - 1, 3, 3);
			bool flag = false;
			int num = 1;
			while (!flag && rectangle.Width <= 11)
			{
				foreach (Vector2 current in Utility.getBorderOfThisRectangle(rectangle))
				{
					if (location.isTileOnMap(current) && location.doesTileHaveProperty((int)current.X, (int)current.Y, "Water", "Back") == null)
					{
						flag = true;
						num = rectangle.Width / 2;
						break;
					}
				}
				rectangle.Inflate(1, 1);
			}
			if (rectangle.Width > 11)
			{
				num = 6;
			}
			num--;
			return num;
		}

		public void startMinigameEndFunction(int extra)
		{
			this.isReeling = true;
			this.hit = false;
			int facingDirection = this.lastUser.FacingDirection;
			if (facingDirection != 1)
			{
				if (facingDirection == 3)
				{
					this.lastUser.FarmerSprite.setCurrentSingleFrame(48, 32000, false, true);
				}
			}
			else
			{
				this.lastUser.FarmerSprite.setCurrentSingleFrame(48, 32000, false, false);
			}
			this.lastUser.FarmerSprite.pauseForSingleAnimation = true;
			this.clearWaterDistance = FishingRod.distanceToLand((int)(this.bobber.X / (float)Game1.tileSize - 1f), (int)(this.bobber.Y / (float)Game1.tileSize - 1f), this.lastUser.currentLocation);
			float num = 1f;
			num *= (float)this.clearWaterDistance / 5f;
			num *= (float)Game1.random.Next(1 + Math.Min(10, this.lastUser.FishingLevel) / 2, 6) / 5f;
			if (this.favBait)
			{
				num *= 1.2f;
			}
			num *= 1f + (float)Game1.random.Next(-10, 10) / 100f;
			num = Math.Max(0f, Math.Min(1f, num));
			bool treasure = !Game1.isFestival() && this.lastUser.fishCaught != null && this.lastUser.fishCaught.Count > 1 && Game1.random.NextDouble() < FishingRod.baseChanceForTreasure + (double)this.lastUser.LuckLevel * 0.005 + ((this.getBaitAttachmentIndex() == 703) ? FishingRod.baseChanceForTreasure : 0.0) + ((this.getBobberAttachmentIndex() == 693) ? (FishingRod.baseChanceForTreasure / 3.0) : 0.0) + Game1.dailyLuck / 2.0 + (this.lastUser.professions.Contains(9) ? FishingRod.baseChanceForTreasure : 0.0);
			Game1.activeClickableMenu = new BobberBar(extra, num, treasure, (this.attachments[1] != null) ? this.attachments[1].ParentSheetIndex : -1);
		}

		public int getBobberAttachmentIndex()
		{
			if (this.attachments[1] == null)
			{
				return -1;
			}
			return this.attachments[1].ParentSheetIndex;
		}

		public int getBaitAttachmentIndex()
		{
			if (this.attachments[0] == null)
			{
				return -1;
			}
			return this.attachments[0].ParentSheetIndex;
		}

		public bool inUse()
		{
			return this.isFishing || this.isCasting || this.isTimingCast || this.isNibbling || this.isReeling || this.fishCaught;
		}

		public void donefishingEndFunction(int extra)
		{
			this.isFishing = false;
			this.isReeling = false;
			this.lastUser.canReleaseTool = true;
			this.lastUser.canMove = true;
			this.lastUser.usingTool = false;
			this.lastUser.FarmerSprite.pauseForSingleAnimation = false;
			this.pullingOutOfWater = false;
			this.doneFishing(this.lastUser, false);
		}

		public static void endOfAnimationBehavior(Farmer f)
		{
		}

		public override StardewValley.Object attach(StardewValley.Object o)
		{
			if (o != null && o.Category == -21 && this.upgradeLevel > 0)
			{
				StardewValley.Object @object = this.attachments[0];
				if (@object != null && @object.canStackWith(o))
				{
					@object.Stack = o.addToStack(@object.Stack);
					if (@object.Stack <= 0)
					{
						@object = null;
					}
				}
				this.attachments[0] = o;
				Game1.playSound("button1");
				return @object;
			}
			if (o != null && o.Category == -22 && this.upgradeLevel > 2)
			{
				StardewValley.Object arg_8E_0 = this.attachments[1];
				this.attachments[1] = o;
				Game1.playSound("button1");
				return arg_8E_0;
			}
			if (o == null)
			{
				if (this.attachments[0] != null)
				{
					StardewValley.Object arg_B7_0 = this.attachments[0];
					this.attachments[0] = null;
					Game1.playSound("dwop");
					return arg_B7_0;
				}
				if (this.attachments[1] != null)
				{
					StardewValley.Object arg_DD_0 = this.attachments[1];
					this.attachments[1] = null;
					Game1.playSound("dwop");
					return arg_DD_0;
				}
			}
			return null;
		}

		public override void drawAttachments(SpriteBatch b, int x, int y)
		{
			if (this.upgradeLevel > 0)
			{
				if (this.attachments[0] == null)
				{
					b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 36, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
				}
				else
				{
					b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
					this.attachments[0].drawInMenu(b, new Vector2((float)x, (float)y), 1f);
				}
			}
			if (this.upgradeLevel > 2)
			{
				if (this.attachments[1] == null)
				{
					b.Draw(Game1.menuTexture, new Vector2((float)x, (float)(y + Game1.tileSize + 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 37, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
					return;
				}
				b.Draw(Game1.menuTexture, new Vector2((float)x, (float)(y + Game1.tileSize + 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
				this.attachments[1].drawInMenu(b, new Vector2((float)x, (float)(y + Game1.tileSize + 4)), 1f);
			}
		}

		public override bool canThisBeAttached(StardewValley.Object o)
		{
			return o == null || (o.category == -21 && this.upgradeLevel > 0) || (o.Category == -22 && this.upgradeLevel > 2);
		}

		public void playerCaughtFishEndFunction(int extraData)
		{
			this.lastUser.Halt();
			this.lastUser.armOffset = Vector2.Zero;
			this.castedButBobberStillInAir = false;
			this.fishCaught = true;
			this.isReeling = false;
			this.isFishing = false;
			this.pullingOutOfWater = false;
			this.lastUser.canReleaseTool = false;
			if (!Game1.isFestival())
			{
				this.recordSize = this.lastUser.caughtFish(this.whichFish, this.fishSize);
				this.lastUser.faceDirection(2);
			}
			else
			{
				Game1.currentLocation.currentEvent.caughtFish(this.whichFish, this.fishSize, this.lastUser);
				this.fishCaught = false;
				this.doneFishing(Game1.player, false);
			}
			if (FishingRod.isFishBossFish(this.whichFish))
			{
				Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14068", new object[0]));
				return;
			}
			if (this.recordSize)
			{
				this.sparklingText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14069", new object[0]), Color.LimeGreen, Color.Azure, false, 0.1, 2500, -1, 500);
				Game1.playSound("newRecord");
				return;
			}
			Game1.playSound("fishSlap");
		}

		public static bool isFishBossFish(int index)
		{
			switch (index)
			{
			case 159:
			case 160:
			case 163:
				break;
			case 161:
			case 162:
				return false;
			default:
				if (index != 682 && index != 775)
				{
					return false;
				}
				break;
			}
			return true;
		}

		public void pullFishFromWater(int whichFish, int fishSize, int fishQuality, int fishDifficulty, bool treasureCaught, bool wasPerfect = false)
		{
			this.treasureCaught = treasureCaught;
			this.fishSize = fishSize;
			this.fishQuality = fishQuality;
			this.whichFish = whichFish;
			if (!Game1.isFestival())
			{
				this.bossFish = FishingRod.isFishBossFish(whichFish);
				int num = Math.Max(1, (fishQuality + 1) * 3 + fishDifficulty / 3);
				if (treasureCaught)
				{
					num += (int)((float)num * 1.2f);
				}
				if (wasPerfect)
				{
					num += (int)((float)num * 1.4f);
				}
				if (this.bossFish)
				{
					num *= 5;
				}
				this.lastUser.gainExperience(1, num);
			}
			float num7;
			if (this.lastUser.FacingDirection == 1 || this.lastUser.FacingDirection == 3)
			{
				float num2 = Vector2.Distance(this.bobber, this.lastUser.position);
				float num3 = 0.001f;
				float num4 = (float)(Game1.tileSize * 2) - (this.lastUser.position.Y - this.bobber.Y + 10f);
				double a = 1.1423973285781066;
				float num5 = (float)((double)(num2 * num3) * Math.Tan(a) / Math.Sqrt((double)(2f * num2 * num3) * Math.Tan(a) - (double)(2f * num3 * num4)));
				if (float.IsNaN(num5))
				{
					num5 = 0.6f;
				}
				float num6 = (float)((double)num5 * (1.0 / Math.Tan(a)));
				num7 = num2 / num6;
				this.animations.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, whichFish, 16, 16), num7, 1, 0, this.bobber, false, false, this.bobber.Y / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					motion = new Vector2((float)((this.lastUser.FacingDirection == 3) ? -1 : 1) * -num6, -num5),
					acceleration = new Vector2(0f, num3),
					timeBasedMotion = true,
					endFunction = new TemporaryAnimatedSprite.endBehavior(this.playerCaughtFishEndFunction),
					extraInfoForEndBehavior = whichFish,
					endSound = "tinyWhip"
				});
			}
			else
			{
				float num8 = this.bobber.Y - (float)this.lastUser.getStandingY();
				float num9 = Math.Abs(num8 + (float)(Game1.tileSize * 4) + (float)(Game1.tileSize / 2));
				if (this.lastUser.FacingDirection == 0)
				{
					num9 += (float)(Game1.tileSize * 3 / 2);
				}
				float num10 = 0.005f;
				float num11 = (float)Math.Sqrt((double)(2f * num10 * num9));
				num7 = (float)(Math.Sqrt((double)(2f * (num9 - num8) / num10)) + (double)(num11 / num10));
				float x = 0f;
				if (num7 != 0f)
				{
					x = (this.lastUser.position.X - this.bobber.X) / num7;
				}
				this.animations.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, whichFish, 16, 16), num7, 1, 0, new Vector2(this.bobber.X, this.bobber.Y), false, false, this.bobber.Y / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					motion = new Vector2(x, -num11),
					acceleration = new Vector2(0f, num10),
					timeBasedMotion = true,
					endFunction = new TemporaryAnimatedSprite.endBehavior(this.playerCaughtFishEndFunction),
					extraInfoForEndBehavior = whichFish,
					endSound = "tinyWhip"
				});
			}
			Game1.playSound("pullItemFromWater");
			Game1.playSound("dwop");
			this.castedButBobberStillInAir = false;
			this.pullingOutOfWater = true;
			this.isFishing = false;
			this.isReeling = false;
			switch (this.lastUser.FacingDirection)
			{
			case 0:
				this.lastUser.FarmerSprite.animateBackwardsOnce(299, num7);
				return;
			case 1:
				this.lastUser.FarmerSprite.animateBackwardsOnce(300, num7);
				return;
			case 2:
				this.lastUser.FarmerSprite.animateBackwardsOnce(301, num7);
				return;
			case 3:
				this.lastUser.FarmerSprite.animateBackwardsOnce(302, num7);
				return;
			default:
				return;
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (!this.bobber.Equals(Vector2.Zero) && this.isFishing)
			{
				Vector2 globalPosition = this.bobber;
				float num = 4f;
				if (this.bobberTimeAccumulator > this.timePerBobberBob)
				{
					if ((!this.isNibbling && !this.isReeling) || Game1.random.NextDouble() < 0.05)
					{
						Game1.playSound("waterSlosh");
						this.lastUser.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f, 8, 0, new Vector2(this.bobber.X - (float)Game1.tileSize - 14f, this.bobber.Y - (float)Game1.tileSize - 10f), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 0.75f, 0.003f, 0f, 0f, false));
					}
					this.timePerBobberBob = (float)((this.bobberBob == 0) ? Game1.random.Next(1500, 3500) : Game1.random.Next(350, 750));
					this.bobberTimeAccumulator = 0f;
					if (this.isNibbling || this.isReeling)
					{
						this.timePerBobberBob = (float)Game1.random.Next(25, 75);
						globalPosition.X += (float)Game1.random.Next(-5, 5);
						globalPosition.Y += (float)Game1.random.Next(-5, 5);
						if (!this.isReeling)
						{
							num += (float)Game1.random.Next(-20, 20) / 100f;
						}
					}
					else if (Game1.random.NextDouble() < 0.1)
					{
						Game1.playSound("bob");
					}
				}
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(179 + this.bobberBob * 9, 1903, 9, 9)), Color.White, 0f, new Vector2(4f, 4f) * num, num, SpriteEffects.None, 0.1f);
			}
			else if (this.isTimingCast || this.castingChosenCountdown > 0f)
			{
				int num2 = (int)(-Math.Abs(this.castingChosenCountdown / 2f - this.castingChosenCountdown) / 50f);
				float scale = (this.castingChosenCountdown > 0f && this.castingChosenCountdown < 100f) ? (this.castingChosenCountdown / 100f) : 1f;
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position + new Vector2((float)(-(float)Game1.tileSize / 2 - 16), (float)(-(float)Game1.tileSize * 2 - Game1.tileSize / 2 + num2))), new Rectangle?(new Rectangle(193, 1868, 47, 12)), Color.White * scale, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.885f);
				b.Draw(Game1.staminaRect, new Rectangle((int)Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position).X - Game1.tileSize / 2 - 4, (int)Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position).Y + num2 - Game1.tileSize * 2 - Game1.tileSize / 2 + 12, (int)(164f * this.castingPower), 25), new Rectangle?(Game1.staminaRect.Bounds), Utility.getRedToGreenLerpColor(this.castingPower) * scale, 0f, Vector2.Zero, SpriteEffects.None, 0.887f);
			}
			for (int i = this.animations.Count - 1; i >= 0; i--)
			{
				this.animations[i].draw(b, false, 0, 0);
			}
			if (this.sparklingText != null && !this.fishCaught)
			{
				this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position + new Vector2((float)(-(float)Game1.tileSize / 2 + 8), (float)(-(float)Game1.tileSize * 2 - Game1.tileSize))));
			}
			else if (this.sparklingText != null && this.fishCaught)
			{
				this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position + new Vector2((float)(-(float)Game1.tileSize / 2 - 32), (float)(-(float)Game1.tileSize * 4) - (float)Game1.tileSize * 1.5f)));
			}
			if ((!this.isFishing && !this.pullingOutOfWater && !this.castedButBobberStillInAir) || this.lastUser.FarmerSprite.CurrentFrame == 57 || (this.lastUser.FacingDirection == 0 && this.pullingOutOfWater && this.whichFish != -1))
			{
				if (this.fishCaught)
				{
					float num3 = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8), (float)(-(float)Game1.tileSize * 5 + Game1.tileSize / 2) + num3)), new Rectangle?(new Rectangle(31, 1870, 73, 49)), Color.White * 0.8f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.06f);
					b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 4), (float)(-(float)Game1.tileSize * 5 + Game1.tileSize / 2 + 4) + num3) + new Vector2(44f, 68f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.whichFish, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.0001f + 0.06f);
					b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize / 8 - 8), (float)(-(float)Game1.tileSize / 4 - Game1.tileSize / 2 - Game1.pixelZoom * 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.whichFish, 16, 16)), Color.White, (this.fishSize == -1) ? 0f : 2.3561945f, new Vector2(8f, 8f), (float)Game1.pixelZoom * 0.75f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
					string text = Game1.objectInformation[this.whichFish].Split(new char[]
					{
						'/'
					})[4];
					b.DrawString(Game1.smallFont, text, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8 + 146) - Game1.smallFont.MeasureString(text).X / 2f, (float)(-(float)Game1.tileSize * 5 + Game1.tileSize * 2 / 3) + num3)), this.bossFish ? new Color(126, 61, 237) : Game1.textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
					if (this.fishSize != -1)
					{
						b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14082", new object[0]), Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8 + 140), (float)(-(float)Game1.tileSize * 5 + Game1.tileSize * 5 / 3) + num3)), Game1.textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
						b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14083", new object[]
						{
							(LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en) ? Math.Round((double)this.fishSize * 2.54) : ((double)this.fishSize)
						}), Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8 + 205) - Game1.smallFont.MeasureString(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14083", new object[]
						{
							(LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en) ? Math.Round((double)this.fishSize * 2.54) : ((double)this.fishSize)
						})).X / 2f, (float)(-(float)Game1.tileSize * 5 + Game1.tileSize * 7 / 3 - 8) + num3)), this.recordSize ? (Color.Blue * Math.Min(1f, num3 / 8f + 1.5f)) : Game1.textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
					}
				}
				return;
			}
			Vector2 value = this.isFishing ? this.bobber : ((this.animations.Count > 0) ? (this.animations[0].position + new Vector2((float)(Game1.tileSize / 2 + 8), (float)(Game1.tileSize + 4))) : Vector2.Zero);
			if (this.whichFish != -1)
			{
				value += new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2));
			}
			Vector2 vector = Vector2.Zero;
			if (this.castedButBobberStillInAir)
			{
				switch (this.lastUser.FacingDirection)
				{
				case 0:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 1:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(32f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 2:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - (float)Game1.tileSize + 40f));
						break;
					case 3:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 16f));
						break;
					case 4:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 32f));
						break;
					case 5:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 32f));
						break;
					default:
						vector = Vector2.Zero;
						break;
					}
					break;
				case 1:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-48f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					case 1:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-16f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 2:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize + 20), this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 3:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 16), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) - 20f));
						break;
					case 4:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 8), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					case 5:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 8), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					default:
						vector = Vector2.Zero;
						break;
					}
					break;
				case 2:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(8f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 1:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 2:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - (float)Game1.tileSize + 40f));
						break;
					case 3:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - 8f));
						break;
					case 4:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + 32f));
						break;
					case 5:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + 32f));
						break;
					default:
						vector = Vector2.Zero;
						break;
					}
					break;
				case 3:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(112f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					case 1:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(80f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 2:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize + 20))), this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 3:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 16))), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) - 20f));
						break;
					case 4:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 8))), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					case 5:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 8))), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					}
					break;
				default:
					vector = Vector2.Zero;
					break;
				}
			}
			else if (this.isReeling)
			{
				if (Game1.didPlayerJustClickAtAll())
				{
					switch (this.lastUser.FacingDirection)
					{
					case 0:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(24f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 12f));
						break;
					case 1:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(20f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 12f));
						break;
					case 2:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(12f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 8f));
						break;
					case 3:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(48f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 12f));
						break;
					}
				}
				else
				{
					switch (this.lastUser.FacingDirection)
					{
					case 0:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(25f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 1:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					case 2:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(12f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 3:
						vector = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					}
				}
			}
			else
			{
				switch (this.lastUser.FacingDirection)
				{
				case 0:
					vector = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - (float)Game1.tileSize - 12f)));
					break;
				case 1:
					vector = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-48f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 8), this.lastUser.armOffset.Y - (float)Game1.tileSize + 16f)));
					break;
				case 2:
					vector = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(8f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + (float)Game1.tileSize - 12f)));
					break;
				case 3:
					vector = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(112f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 8))), this.lastUser.armOffset.Y - (float)Game1.tileSize + 16f)));
					break;
				default:
					vector = Vector2.Zero;
					break;
				}
			}
			Vector2 vector2 = Game1.GlobalToLocal(Game1.viewport, value + new Vector2(-36f, (float)(-(float)Game1.tileSize / 2 - 24 + ((this.bobberBob == 1) ? 4 : 0))));
			if (this.isReeling)
			{
				Utility.drawLineWithScreenCoordinates((int)vector.X, (int)vector.Y, (int)vector2.X, (int)vector2.Y, b, Color.White * 0.5f, 1f);
				return;
			}
			Vector2 p = vector;
			Vector2 p2 = new Vector2(vector.X + (vector2.X - vector.X) / 3f, vector.Y + (vector2.Y - vector.Y) * 2f / 3f);
			Vector2 p3 = new Vector2(vector.X + (vector2.X - vector.X) * 2f / 3f, vector.Y + (vector2.Y - vector.Y) * (float)(this.isFishing ? 6 : 2) / 5f);
			Vector2 p4 = vector2;
			for (float num4 = 0f; num4 < 1f; num4 += 0.025f)
			{
				Vector2 curvePoint = Utility.GetCurvePoint(num4, p, p2, p3, p4);
				Utility.drawLineWithScreenCoordinates((int)vector.X, (int)vector.Y, (int)curvePoint.X, (int)curvePoint.Y, b, Color.White * 0.5f, (float)this.lastUser.getStandingY() / 10000f + ((this.lastUser.FacingDirection != 0) ? 0.005f : -0.001f));
				vector = curvePoint;
			}
		}

		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			if (who.Stamina <= 1f)
			{
				if (!who.isEmoting)
				{
					who.doEmote(36);
				}
				who.CanMove = !Game1.eventUp;
				who.UsingTool = false;
				who.canReleaseTool = false;
				this.doneFishing(null, false);
				return true;
			}
			this.usedGamePadToCast = false;
			if (GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.X))
			{
				this.usedGamePadToCast = true;
			}
			this.bossFish = false;
			this.originalFacingDirection = who.FacingDirection;
			who.Halt();
			this.treasureCaught = false;
			this.showingTreasure = false;
			this.isFishing = false;
			this.hit = false;
			this.favBait = false;
			if (this.attachments != null && this.attachments.Length > 1 && this.attachments[1] != null)
			{
				this.hadBobber = true;
			}
			this.isNibbling = false;
			this.lastUser = who;
			this.isTimingCast = true;
			who.usingTool = true;
			this.whichFish = -1;
			who.canMove = false;
			this.fishCaught = false;
			this.doneWithAnimation = false;
			who.canReleaseTool = false;
			this.hasDoneFucntionYet = false;
			this.isReeling = false;
			this.pullingOutOfWater = false;
			this.castingPower = 0f;
			this.castingChosenCountdown = 0f;
			this.animations.Clear();
			this.sparklingText = null;
			switch (who.FacingDirection)
			{
			case 0:
				who.FarmerSprite.setCurrentFrame(295);
				Game1.player.CurrentTool.Update(0, 0);
				break;
			case 1:
				who.FarmerSprite.setCurrentFrame(296);
				Game1.player.CurrentTool.Update(1, 0);
				break;
			case 2:
				who.FarmerSprite.setCurrentFrame(297);
				Game1.player.CurrentTool.Update(2, 0);
				break;
			case 3:
				who.FarmerSprite.setCurrentFrame(298);
				Game1.player.CurrentTool.Update(3, 0);
				break;
			}
			return true;
		}

		public void doneFishing(Farmer who, bool consumeBaitAndTackle = false)
		{
			if (consumeBaitAndTackle)
			{
				if (this.attachments[0] != null)
				{
					StardewValley.Object expr_18 = this.attachments[0];
					int stack = expr_18.Stack;
					expr_18.Stack = stack - 1;
					if (this.attachments[0].Stack <= 0)
					{
						this.attachments[0] = null;
						Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14085", new object[0]));
					}
				}
				if (this.attachments[1] != null)
				{
					StardewValley.Object expr_76_cp_0_cp_0 = this.attachments[1];
					expr_76_cp_0_cp_0.scale.Y = expr_76_cp_0_cp_0.scale.Y - 0.05f;
					if (this.attachments[1].scale.Y <= 0f)
					{
						this.attachments[1] = null;
						Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14086", new object[0]));
					}
				}
			}
			this.bobber = Vector2.Zero;
			this.isNibbling = false;
			this.fishCaught = false;
			this.isFishing = false;
			this.isReeling = false;
			this.doneWithAnimation = false;
			this.pullingOutOfWater = false;
			this.fishingBiteAccumulator = 0f;
			this.fishingNibbleAccumulator = 0f;
			this.timeUntilFishingBite = -1f;
			this.timeUntilFishingNibbleDone = -1f;
			this.bobberTimeAccumulator = 0f;
			if (FishingRod.chargeSound != null && FishingRod.chargeSound.IsPlaying)
			{
				FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
			}
			if (FishingRod.reelSound != null && FishingRod.reelSound.IsPlaying)
			{
				FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
			}
			if (who != null)
			{
				who.UsingTool = false;
				who.Halt();
				who.faceDirection(this.originalFacingDirection);
			}
		}

		public static void doneWithCastingAnimation(Farmer who)
		{
			if (who.CurrentTool != null && who.CurrentTool is FishingRod)
			{
				(who.CurrentTool as FishingRod).doneWithAnimation = true;
				if ((who.CurrentTool as FishingRod).hasDoneFucntionYet)
				{
					who.canReleaseTool = true;
					who.usingTool = false;
					who.canMove = true;
					Farmer.canMoveNow(who);
				}
			}
		}

		public void castingEndFunction(int extraInfo)
		{
			this.castedButBobberStillInAir = false;
			if (this.lastUser != null)
			{
				float stamina = this.lastUser.Stamina;
				this.lastUser.CurrentTool.DoFunction(this.lastUser.currentLocation, (int)this.bobber.X, (int)this.bobber.Y, 1, this.lastUser);
				this.lastUser.lastClick = Vector2.Zero;
				if (FishingRod.reelSound != null)
				{
					FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
				}
				FishingRod.reelSound = null;
				if (this.lastUser.Stamina <= 0f && stamina > 0f)
				{
					this.lastUser.doEmote(36);
				}
				Game1.toolHold = 0f;
				if (!this.isFishing && this.doneWithAnimation)
				{
					Farmer.canMoveNow(this.lastUser);
				}
			}
		}

		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (Game1.paused)
			{
				return;
			}
			if (who.CurrentTool != null && who.CurrentTool.Equals(this) && who.usingTool)
			{
				who.CanMove = false;
			}
			else if (Game1.currentMinigame == null && (who.CurrentTool == null || !(who.CurrentTool is FishingRod) || !who.usingTool))
			{
				if (FishingRod.chargeSound != null && FishingRod.chargeSound.IsPlaying)
				{
					FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
					FishingRod.chargeSound = null;
				}
				return;
			}
			for (int i = this.animations.Count - 1; i >= 0; i--)
			{
				if (this.animations[i].update(time))
				{
					this.animations.RemoveAt(i);
				}
			}
			if (this.sparklingText != null && this.sparklingText.update(time))
			{
				this.sparklingText = null;
			}
			if (this.castingChosenCountdown > 0f)
			{
				this.castingChosenCountdown -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.castingChosenCountdown <= 0f)
				{
					switch (who.FacingDirection)
					{
					case 0:
						who.FarmerSprite.animateOnce(295, 1f, 1);
						Game1.player.CurrentTool.Update(0, 0);
						break;
					case 1:
						who.FarmerSprite.animateOnce(296, 1f, 1);
						Game1.player.CurrentTool.Update(1, 0);
						break;
					case 2:
						who.FarmerSprite.animateOnce(297, 1f, 1);
						Game1.player.CurrentTool.Update(2, 0);
						break;
					case 3:
						who.FarmerSprite.animateOnce(298, 1f, 1);
						Game1.player.CurrentTool.Update(3, 0);
						break;
					}
					if (who.FacingDirection == 1 || who.FacingDirection == 3)
					{
						float num = Math.Max((float)(Game1.tileSize * 2), this.castingPower * (float)(this.getAddedDistance(who) + 4) * (float)Game1.tileSize);
						if (who.FacingDirection == 3)
						{
							num = Math.Max((float)(Game1.tileSize * 2), num - (float)Game1.tileSize);
						}
						num -= 8f;
						float num2 = 0.005f;
						float num3 = (float)((double)num * Math.Sqrt((double)(num2 / (2f * (num + (float)(Game1.tileSize * 3 / 2))))));
						float animationInterval = 2f * (num3 / num2) + (float)((Math.Sqrt((double)(num3 * num3 + 2f * num2 * (float)(Game1.tileSize * 3 / 2))) - (double)num3) / (double)num2);
						this.bobber = new Vector2((float)who.getStandingX() + (float)((who.FacingDirection == 3) ? -1 : 1) * num + (float)(Game1.tileSize / 2), (float)(who.getStandingY() + Game1.tileSize / 2));
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), animationInterval, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, false, (float)who.getStandingY() / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							motion = new Vector2((float)((who.FacingDirection == 3) ? -1 : 1) * num3, -num3),
							acceleration = new Vector2(0f, num2),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.castingEndFunction),
							timeBasedMotion = true
						});
					}
					else
					{
						float num4 = -Math.Max((float)(Game1.tileSize * 2), this.castingPower * (float)(this.getAddedDistance(who) + 3) * (float)Game1.tileSize);
						float num5 = Math.Abs(num4 - (float)Game1.tileSize);
						if (this.lastUser.FacingDirection == 0)
						{
							num4 = -num4;
							num5 += (float)Game1.tileSize;
						}
						float num6 = 0.005f;
						float num7 = (float)Math.Sqrt((double)(2f * num6 * num5));
						float num8 = (float)(Math.Sqrt((double)(2f * (num5 - num4) / num6)) + (double)(num7 / num6));
						num8 *= 1.05f;
						if (this.lastUser.FacingDirection == 0)
						{
							num8 *= 1.05f;
						}
						this.bobber = new Vector2((float)(who.getStandingX() + Game1.random.Next(48)), (float)(who.getStandingY() + Game1.tileSize / 2) - num4);
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), num8, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, false, this.bobber.Y / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							alphaFade = 0.0001f,
							motion = new Vector2(0f, -num7),
							acceleration = new Vector2(0f, num6),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.castingEndFunction),
							timeBasedMotion = true
						});
					}
					this.castedButBobberStillInAir = true;
					this.isCasting = false;
					Game1.playSound("cast");
					if (Game1.soundBank != null)
					{
						FishingRod.reelSound = Game1.soundBank.GetCue("slowReel");
						FishingRod.reelSound.SetVariable("Pitch", 1600f);
						FishingRod.reelSound.Play();
					}
				}
			}
			else if (!this.isTimingCast && this.castingChosenCountdown <= 0f)
			{
				who.jitterStrength = 0f;
			}
			if (this.isTimingCast)
			{
				if (FishingRod.chargeSound == null && Game1.soundBank != null)
				{
					FishingRod.chargeSound = Game1.soundBank.GetCue("SinWave");
				}
				if (this.castingPower > 0f && FishingRod.chargeSound != null && !FishingRod.chargeSound.IsPlaying && !FishingRod.chargeSound.IsStopped)
				{
					FishingRod.chargeSound.Play();
				}
				this.castingPower = Math.Max(0f, Math.Min(1f, this.castingPower + this.castingTimerSpeed * (float)time.ElapsedGameTime.Milliseconds));
				if (FishingRod.chargeSound != null)
				{
					FishingRod.chargeSound.SetVariable("Pitch", 2400f * this.castingPower);
				}
				if (this.castingPower == 1f || this.castingPower == 0f)
				{
					this.castingTimerSpeed = -this.castingTimerSpeed;
				}
				who.armOffset.Y = 2f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				who.jitterStrength = Math.Max(0f, this.castingPower - 0.5f);
				if (((!this.usedGamePadToCast && Mouse.GetState().LeftButton == ButtonState.Released) || (this.usedGamePadToCast && Game1.options.gamepadControls && GamePad.GetState(Game1.playerOneIndex).IsButtonUp(Buttons.X))) && Game1.areAllOfTheseKeysUp(Keyboard.GetState(), Game1.options.useToolButton))
				{
					if (FishingRod.chargeSound != null)
					{
						FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
						FishingRod.chargeSound = null;
					}
					Game1.playSound("button1");
					Rumble.rumble(0.5f, 150f);
					this.isTimingCast = false;
					this.isCasting = true;
					this.castingChosenCountdown = 350f;
					who.armOffset.Y = 0f;
					if (this.castingPower > 0.99f)
					{
						Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(545, 1921, 53, 19), 800f, 1, 0, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3))), false, false, 1f, 0.01f, Color.White, 2f, 0f, 0f, 0f, true)
						{
							motion = new Vector2(0f, -4f),
							acceleration = new Vector2(0f, 0.2f),
							delayBeforeAnimationStart = 200
						});
						DelayedAction.playSoundAfterDelay("crit", 200);
						return;
					}
				}
			}
			else
			{
				if (this.isReeling)
				{
					if (Game1.didPlayerJustClickAtAll())
					{
						if (Game1.isAnyGamePadButtonBeingPressed())
						{
							Game1.lastCursorMotionWasMouse = false;
						}
						switch (who.FacingDirection)
						{
						case 0:
							who.FarmerSprite.setCurrentSingleFrame(76, 32000, false, false);
							break;
						case 1:
							who.FarmerSprite.setCurrentSingleFrame(72, 100, false, false);
							break;
						case 2:
							who.FarmerSprite.setCurrentSingleFrame(75, 32000, false, false);
							break;
						case 3:
							who.FarmerSprite.setCurrentSingleFrame(72, 100, false, true);
							break;
						}
						who.armOffset.Y = (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
						who.jitterStrength = 1f;
					}
					else
					{
						switch (who.FacingDirection)
						{
						case 0:
							who.FarmerSprite.setCurrentSingleFrame(36, 32000, false, false);
							break;
						case 1:
							who.FarmerSprite.setCurrentSingleFrame(48, 100, false, false);
							break;
						case 2:
							who.FarmerSprite.setCurrentSingleFrame(66, 32000, false, false);
							break;
						case 3:
							who.FarmerSprite.setCurrentSingleFrame(48, 100, false, true);
							break;
						}
						who.stopJittering();
					}
					who.armOffset = new Vector2((float)Game1.random.Next(-10, 11) / 10f, (float)Game1.random.Next(-10, 11) / 10f);
					this.bobberTimeAccumulator += (float)time.ElapsedGameTime.Milliseconds;
					return;
				}
				if (this.isFishing)
				{
					this.bobber.Y = this.bobber.Y + (float)(0.10000000149011612 * Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0));
					who.canReleaseTool = true;
					this.bobberTimeAccumulator += (float)time.ElapsedGameTime.Milliseconds;
					switch (who.FacingDirection)
					{
					case 0:
						who.FarmerSprite.setCurrentFrame(44);
						break;
					case 1:
						who.FarmerSprite.setCurrentFrame(89);
						break;
					case 2:
						who.FarmerSprite.setCurrentFrame(70);
						break;
					case 3:
						who.FarmerSprite.setCurrentFrame(89, 0, 10, 1, true, false);
						break;
					}
					who.armOffset.Y = (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2) + (float)((who.FacingDirection == 1 || who.FacingDirection == 3) ? 1 : -1);
					if (who.IsMainPlayer)
					{
						if (this.timeUntilFishingBite != -1f)
						{
							this.fishingBiteAccumulator += (float)time.ElapsedGameTime.Milliseconds;
							if (this.fishingBiteAccumulator > this.timeUntilFishingBite)
							{
								this.fishingBiteAccumulator = 0f;
								this.timeUntilFishingBite = -1f;
								this.isNibbling = true;
								Game1.playSound("fishBite");
								Rumble.rumble(0.75f, 250f);
								this.timeUntilFishingNibbleDone = (float)FishingRod.maxTimeToNibble;
								if (Game1.currentMinigame == null)
								{
									Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(395, 497, 3, 8), new Vector2((float)(this.lastUser.getStandingX() - Game1.viewport.X), (float)(this.lastUser.getStandingY() - Game1.tileSize * 2 - Game1.pixelZoom * 2 - Game1.viewport.Y)), false, 0.02f, Color.White)
									{
										scale = (float)(Game1.pixelZoom + 1),
										scaleChange = -0.01f,
										motion = new Vector2(0f, -0.5f),
										shakeIntensityChange = -0.005f,
										shakeIntensity = 1f
									});
								}
								this.timePerBobberBob = 1f;
							}
						}
						if (this.timeUntilFishingNibbleDone != -1f)
						{
							this.fishingNibbleAccumulator += (float)time.ElapsedGameTime.Milliseconds;
							if (this.fishingNibbleAccumulator > this.timeUntilFishingNibbleDone)
							{
								this.fishingNibbleAccumulator = 0f;
								this.timeUntilFishingNibbleDone = -1f;
								this.isNibbling = false;
								this.timeUntilFishingBite = (float)Game1.random.Next(FishingRod.minFishingBiteTime, FishingRod.maxFishingBiteTime);
								return;
							}
						}
					}
				}
				else if (who.usingTool && this.castedButBobberStillInAir)
				{
					Vector2 zero = Vector2.Zero;
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton) && who.FacingDirection != 2 && who.FacingDirection != 0)
					{
						zero.Y += 4f;
					}
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton) && who.FacingDirection != 1 && who.FacingDirection != 3)
					{
						zero.X += 2f;
					}
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton) && who.FacingDirection != 0 && who.FacingDirection != 2)
					{
						zero.Y -= 4f;
					}
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton) && who.FacingDirection != 3 && who.FacingDirection != 1)
					{
						zero.X -= 2f;
					}
					this.bobber += zero;
					if (this.animations.Count > 0)
					{
						this.animations[0].position += zero;
						return;
					}
				}
				else
				{
					if (this.showingTreasure)
					{
						who.FarmerSprite.setCurrentSingleFrame(0, 32000, false, false);
						return;
					}
					if (this.fishCaught)
					{
						if (!Game1.isFestival())
						{
							who.faceDirection(2);
							who.FarmerSprite.setCurrentFrame(84);
						}
						if (Game1.random.NextDouble() < 0.025)
						{
							who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(653, 858, 1, 1), 9999f, 1, 1, who.position + new Vector2((float)(Game1.random.Next(-3, 2) * 4), (float)(-(float)Game1.tileSize / 2)), false, false, (float)who.getStandingY() / 10000f + 0.002f, 0.04f, Color.LightBlue, 5f, 0f, 0f, 0f, false)
							{
								acceleration = new Vector2(0f, 0.25f)
							});
						}
						if (Mouse.GetState().LeftButton == ButtonState.Pressed || Game1.didPlayerJustClickAtAll() || Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton))
						{
							Game1.playSound("coin");
							if (this.treasureCaught)
							{
								this.fishCaught = false;
								this.showingTreasure = true;
								bool flag = this.lastUser.addItemToInventoryBool(new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality), false);
								this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(64, 1920, 32, 32), 500f, 1, 0, this.lastUser.position + new Vector2(-32f, -160f), false, false, (float)this.lastUser.getStandingY() / 10000f + 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -0.128f),
									timeBasedMotion = true,
									endFunction = new TemporaryAnimatedSprite.endBehavior(this.openChestEndFunction),
									extraInfoForEndBehavior = (flag ? 0 : 1),
									alpha = 0f,
									alphaFade = -0.002f
								});
								return;
							}
							this.doneFishing(this.lastUser, true);
							this.lastUser.completelyStopAnimatingOrDoingAction();
							if (!Game1.isFestival() && !this.lastUser.addItemToInventoryBool(new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality), false))
							{
								Game1.activeClickableMenu = new ItemGrabMenu(new List<Item>
								{
									new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality)
								});
								return;
							}
						}
					}
					else
					{
						if (who.usingTool && this.castedButBobberStillInAir && this.doneWithAnimation)
						{
							switch (who.FacingDirection)
							{
							case 0:
								who.FarmerSprite.setCurrentFrame(39);
								break;
							case 1:
								who.FarmerSprite.setCurrentFrame(89);
								break;
							case 2:
								who.FarmerSprite.setCurrentFrame(28);
								break;
							case 3:
								who.FarmerSprite.setCurrentFrame(89, 0, 10, 1, true, false);
								break;
							}
							who.armOffset.Y = (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
							return;
						}
						if (!this.castedButBobberStillInAir && this.whichFish != -1 && this.animations.Count > 0 && this.animations[0].timer > 500f && !Game1.eventUp)
						{
							this.lastUser.faceDirection(2);
							this.lastUser.FarmerSprite.setCurrentFrame(57);
						}
					}
				}
			}
		}

		public void openChestEndFunction(int extra)
		{
			Game1.playSound("openChest");
			this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(64, 1920, 32, 32), 200f, 4, 0, this.lastUser.position + new Vector2(-32f, -228f), false, false, (float)this.lastUser.getStandingY() / 10000f + 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
			{
				endFunction = new TemporaryAnimatedSprite.endBehavior(this.openTreasureMenuEndFunction),
				extraInfoForEndBehavior = extra
			});
			this.sparklingText = null;
		}

		public override bool doesShowTileLocationMarker()
		{
			return false;
		}

		public void openTreasureMenuEndFunction(int extra)
		{
			this.lastUser.gainExperience(5, 10 * (this.clearWaterDistance + 1));
			this.doneFishing(this.lastUser, true);
			this.lastUser.completelyStopAnimatingOrDoingAction();
			List<Item> list = new List<Item>();
			if (extra == 1)
			{
				list.Add(new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality));
			}
			float num = 1f;
			while (Game1.random.NextDouble() <= (double)num)
			{
				num *= 0.4f;
				switch (Game1.random.Next(4))
				{
				case 0:
					if (this.clearWaterDistance >= 5 && Game1.random.NextDouble() < 0.03)
					{
						list.Add(new StardewValley.Object(386, Game1.random.Next(1, 3), false, -1, 0));
					}
					else
					{
						List<int> list2 = new List<int>();
						if (this.clearWaterDistance >= 4)
						{
							list2.Add(384);
						}
						if (this.clearWaterDistance >= 3 && (list2.Count == 0 || Game1.random.NextDouble() < 0.6))
						{
							list2.Add(380);
						}
						if (list2.Count == 0 || Game1.random.NextDouble() < 0.6)
						{
							list2.Add(378);
						}
						if (list2.Count == 0 || Game1.random.NextDouble() < 0.6)
						{
							list2.Add(388);
						}
						if (list2.Count == 0 || Game1.random.NextDouble() < 0.6)
						{
							list2.Add(390);
						}
						list2.Add(382);
						list.Add(new StardewValley.Object(list2.ElementAt(Game1.random.Next(list2.Count)), Game1.random.Next(2, 7) * ((Game1.random.NextDouble() < 0.05 + (double)this.lastUser.luckLevel * 0.015) ? 2 : 1), false, -1, 0));
						if (Game1.random.NextDouble() < 0.05 + (double)this.lastUser.LuckLevel * 0.03)
						{
							list.Last<Item>().Stack *= 2;
						}
					}
					break;
				case 1:
					if (this.clearWaterDistance >= 4 && Game1.random.NextDouble() < 0.1 && this.lastUser.FishingLevel >= 6)
					{
						list.Add(new StardewValley.Object(687, 1, false, -1, 0));
					}
					else if (this.lastUser.FishingLevel >= 6)
					{
						list.Add(new StardewValley.Object(685, 1, false, -1, 0));
					}
					else
					{
						list.Add(new StardewValley.Object(685, 10, false, -1, 0));
					}
					break;
				case 2:
					if (Game1.random.NextDouble() < 0.1 && this.lastUser != null && this.lastUser.archaeologyFound != null && this.lastUser.archaeologyFound.ContainsKey(102) && this.lastUser.archaeologyFound[102][0] < 21)
					{
						list.Add(new StardewValley.Object(102, 1, false, -1, 0));
						Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14100", new object[0]));
					}
					else if (Game1.player.archaeologyFound.Count > 0)
					{
						if (Game1.random.NextDouble() < 0.25 && this.lastUser.FishingLevel > 1)
						{
							list.Add(new StardewValley.Object(Game1.random.Next(585, 588), 1, false, -1, 0));
						}
						else if (Game1.random.NextDouble() < 0.5 && this.lastUser.FishingLevel > 1)
						{
							list.Add(new StardewValley.Object(Game1.random.Next(103, 120), 1, false, -1, 0));
						}
						else
						{
							list.Add(new StardewValley.Object(535, 1, false, -1, 0));
						}
					}
					else
					{
						list.Add(new StardewValley.Object(382, Game1.random.Next(1, 3), false, -1, 0));
					}
					break;
				case 3:
					switch (Game1.random.Next(3))
					{
					case 0:
						if (this.clearWaterDistance >= 4)
						{
							list.Add(new StardewValley.Object(537 + ((Game1.random.NextDouble() < 0.4) ? Game1.random.Next(-2, 0) : 0), Game1.random.Next(1, 4), false, -1, 0));
						}
						else if (this.clearWaterDistance >= 3)
						{
							list.Add(new StardewValley.Object(536 + ((Game1.random.NextDouble() < 0.4) ? -1 : 0), Game1.random.Next(1, 4), false, -1, 0));
						}
						else
						{
							list.Add(new StardewValley.Object(535, Game1.random.Next(1, 4), false, -1, 0));
						}
						if (Game1.random.NextDouble() < 0.05 + (double)this.lastUser.LuckLevel * 0.03)
						{
							list.Last<Item>().Stack *= 2;
						}
						break;
					case 1:
						if (this.lastUser.FishingLevel < 2)
						{
							list.Add(new StardewValley.Object(382, Game1.random.Next(1, 4), false, -1, 0));
						}
						else
						{
							if (this.clearWaterDistance >= 4)
							{
								list.Add(new StardewValley.Object((Game1.random.NextDouble() < 0.3) ? 82 : ((Game1.random.NextDouble() < 0.5) ? 64 : 60), Game1.random.Next(1, 3), false, -1, 0));
							}
							else if (this.clearWaterDistance >= 3)
							{
								list.Add(new StardewValley.Object((Game1.random.NextDouble() < 0.3) ? 84 : ((Game1.random.NextDouble() < 0.5) ? 70 : 62), Game1.random.Next(1, 3), false, -1, 0));
							}
							else
							{
								list.Add(new StardewValley.Object((Game1.random.NextDouble() < 0.3) ? 86 : ((Game1.random.NextDouble() < 0.5) ? 66 : 68), Game1.random.Next(1, 3), false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.028 * (double)((float)this.clearWaterDistance / 5f))
							{
								list.Add(new StardewValley.Object(72, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.05)
							{
								list.Last<Item>().Stack *= 2;
							}
						}
						break;
					case 2:
						if (this.lastUser.FishingLevel < 2)
						{
							list.Add(new StardewValley.Object(770, Game1.random.Next(1, 4), false, -1, 0));
						}
						else
						{
							float num2 = (1f + (float)Game1.dailyLuck) * ((float)this.clearWaterDistance / 5f);
							if (Game1.random.NextDouble() < 0.05 * (double)num2 && !this.lastUser.specialItems.Contains(14))
							{
								list.Add(new MeleeWeapon(14)
								{
									specialItem = true
								});
							}
							if (Game1.random.NextDouble() < 0.05 * (double)num2 && !this.lastUser.specialItems.Contains(51))
							{
								list.Add(new MeleeWeapon(51)
								{
									specialItem = true
								});
							}
							if (Game1.random.NextDouble() < 0.07 * (double)num2)
							{
								switch (Game1.random.Next(3))
								{
								case 0:
									list.Add(new Ring(516 + ((Game1.random.NextDouble() < (double)((float)this.lastUser.LuckLevel / 11f)) ? 1 : 0)));
									break;
								case 1:
									list.Add(new Ring(518 + ((Game1.random.NextDouble() < (double)((float)this.lastUser.LuckLevel / 11f)) ? 1 : 0)));
									break;
								case 2:
									list.Add(new Ring(Game1.random.Next(529, 535)));
									break;
								}
							}
							if (Game1.random.NextDouble() < 0.02 * (double)num2)
							{
								list.Add(new StardewValley.Object(166, 1, false, -1, 0));
							}
							if (this.lastUser.FishingLevel > 5 && Game1.random.NextDouble() < 0.001 * (double)num2)
							{
								list.Add(new StardewValley.Object(74, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)num2)
							{
								list.Add(new StardewValley.Object(127, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)num2)
							{
								list.Add(new StardewValley.Object(126, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)num2)
							{
								list.Add(new Ring(527));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)num2)
							{
								list.Add(new Boots(Game1.random.Next(504, 514)));
							}
							if (list.Count == 1)
							{
								list.Add(new StardewValley.Object(72, 1, false, -1, 0));
							}
						}
						break;
					}
					break;
				}
			}
			if (list.Count == 0)
			{
				list.Add(new StardewValley.Object(685, Game1.random.Next(1, 4) * 5, false, -1, 0));
			}
			Game1.activeClickableMenu = new ItemGrabMenu(list);
			(Game1.activeClickableMenu as ItemGrabMenu).source = 3;
			this.lastUser.completelyStopAnimatingOrDoingAction();
		}
	}
}
