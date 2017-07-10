using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace StardewValley.Tools
{
	public class MeleeWeapon : Tool
	{
		public const int defenseCooldownTime = 1500;

		public const int attackSwordCooldownTime = 2000;

		public const int daggerCooldownTime = 6000;

		public const int clubCooldownTime = 4000;

		public const int millisecondsPerSpeedPoint = 40;

		public const int defaultSpeed = 400;

		public const int stabbingSword = 0;

		public const int dagger = 1;

		public const int club = 2;

		public const int defenseSword = 3;

		public const int baseClubSpeed = -8;

		public const int scythe = 47;

		public int minDamage;

		public int maxDamage;

		public int speed;

		public int addedPrecision;

		public int addedDefense;

		public int type;

		public int addedAreaOfEffect;

		public float knockback;

		public float critChance;

		public float critMultiplier;

		public bool isOnSpecial;

		public static int defenseCooldown;

		public static int attackSwordCooldown;

		public static int daggerCooldown;

		public static int clubCooldown;

		public static int daggerHitsLeft;

		public static int timedHitTimer;

		private static float addedSwordScale = 0f;

		private static float addedClubScale = 0f;

		private static float addedDaggerScale = 0f;

		private bool hasBegunWeaponEndPause;

		private float swipeSpeed;

		[XmlIgnore]
		public Rectangle mostRecentArea;

		[XmlIgnore]
		public List<Monster> monstersHitThisSwing = new List<Monster>();

		private bool anotherClick;

		private static Vector2 center = new Vector2(1f, 15f);

		public MeleeWeapon()
		{
			this.category = -98;
		}

		public MeleeWeapon(int spriteIndex)
		{
			this.category = -98;
			int num = (spriteIndex > -10000) ? spriteIndex : (Math.Abs(spriteIndex) - -10000);
			LocalizedContentManager.OnLanguageChange += new LocalizedContentManager.LanguageChangedHandler(this.OnLanguageChange);
			Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\weapons");
			if (dictionary.ContainsKey(num))
			{
				string[] array = dictionary[num].Split(new char[]
				{
					'/'
				});
				this.name = array[0];
				this.minDamage = Convert.ToInt32(array[2]);
				this.maxDamage = Convert.ToInt32(array[3]);
				this.knockback = (float)Convert.ToDouble(array[4], CultureInfo.InvariantCulture);
				this.speed = Convert.ToInt32(array[5]);
				this.addedPrecision = Convert.ToInt32(array[6]);
				this.addedDefense = Convert.ToInt32(array[7]);
				this.type = Convert.ToInt32(array[8]);
				if (this.type == 0)
				{
					this.type = 3;
				}
				this.addedAreaOfEffect = Convert.ToInt32(array[11]);
				this.critChance = (float)Convert.ToDouble(array[12], CultureInfo.InvariantCulture);
				this.critMultiplier = (float)Convert.ToDouble(array[13], CultureInfo.InvariantCulture);
			}
			this.Stack = 1;
			this.initialParentTileIndex = num;
			this.currentParentTileIndex = this.initialParentTileIndex;
			this.indexOfMenuItemView = this.currentParentTileIndex;
			if (spriteIndex == 47)
			{
				this.category = -99;
			}
		}

		protected override string loadDisplayName()
		{
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
			{
				return this.name;
			}
			string[] expr_38 = Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			});
			return expr_38[expr_38.Length - 1];
		}

		protected override string loadDescription()
		{
			return Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			})[1];
		}

		private void OnLanguageChange(LocalizedContentManager.LanguageCode code)
		{
			Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\weapons");
			if (dictionary.ContainsKey(this.initialParentTileIndex))
			{
				dictionary[this.initialParentTileIndex].Split(new char[]
				{
					'/'
				});
				base.description = this.loadDescription();
				this.DisplayName = this.loadDisplayName();
			}
		}

		public MeleeWeapon(int spriteIndex, int type)
		{
			this.type = type;
			this.name = "";
		}

		public override string checkForSpecialItemHoldUpMeessage()
		{
			if (this.initialParentTileIndex == 4)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14122", new object[0]);
			}
			return null;
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			float num = 0f;
			float num2 = 0f;
			switch (this.type)
			{
			case 0:
			case 3:
				if (MeleeWeapon.defenseCooldown > 0)
				{
					num = (float)MeleeWeapon.defenseCooldown / 1500f;
				}
				num2 = MeleeWeapon.addedSwordScale;
				break;
			case 1:
				if (MeleeWeapon.daggerCooldown > 0)
				{
					num = (float)MeleeWeapon.daggerCooldown / 6000f;
				}
				num2 = MeleeWeapon.addedDaggerScale;
				break;
			case 2:
				if (MeleeWeapon.clubCooldown > 0)
				{
					num = (float)MeleeWeapon.clubCooldown / 4000f;
				}
				num2 = MeleeWeapon.addedClubScale;
				break;
			}
			spriteBatch.Draw(Tool.weaponsTexture, location + ((this.type == 1) ? new Vector2((float)(Game1.tileSize * 2 / 3), (float)(Game1.tileSize / 3)) : new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, this.indexOfMenuItemView, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom * (scaleSize + num2), SpriteEffects.None, layerDepth);
			if (num > 0f)
			{
				spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)location.X, (int)location.Y + (Game1.tileSize - (int)(num * (float)Game1.tileSize)), Game1.tileSize, (int)(num * (float)Game1.tileSize)), Color.Red * 0.66f);
			}
		}

		public override int maximumStackSize()
		{
			return 1;
		}

		public override int salePrice()
		{
			return this.getItemLevel() * 100;
		}

		public static void weaponsTypeUpdate(GameTime time)
		{
			if (MeleeWeapon.addedSwordScale > 0f)
			{
				MeleeWeapon.addedSwordScale -= 0.01f;
			}
			if (MeleeWeapon.addedClubScale > 0f)
			{
				MeleeWeapon.addedClubScale -= 0.01f;
			}
			if (MeleeWeapon.addedDaggerScale > 0f)
			{
				MeleeWeapon.addedDaggerScale -= 0.01f;
			}
			if ((float)MeleeWeapon.timedHitTimer > 0f)
			{
				MeleeWeapon.timedHitTimer -= (int)time.ElapsedGameTime.TotalMilliseconds;
			}
			if (MeleeWeapon.defenseCooldown > 0)
			{
				MeleeWeapon.defenseCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.defenseCooldown <= 0)
				{
					MeleeWeapon.addedSwordScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
			if (MeleeWeapon.attackSwordCooldown > 0)
			{
				MeleeWeapon.attackSwordCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.attackSwordCooldown <= 0)
				{
					MeleeWeapon.addedSwordScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
			if (MeleeWeapon.daggerCooldown > 0)
			{
				MeleeWeapon.daggerCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.daggerCooldown <= 0)
				{
					MeleeWeapon.addedDaggerScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
			if (MeleeWeapon.clubCooldown > 0)
			{
				MeleeWeapon.clubCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.clubCooldown <= 0)
				{
					MeleeWeapon.addedClubScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
		}

		public override void tickUpdate(GameTime time, Farmer who)
		{
			base.tickUpdate(time, who);
			if (this.isOnSpecial && this.type == 1 && MeleeWeapon.daggerHitsLeft > 0 && !who.usingTool)
			{
				this.quickStab(who);
				this.doDaggerFunction(who);
			}
			if (this.anotherClick)
			{
				this.leftClick(who);
			}
		}

		public override bool doesShowTileLocationMarker()
		{
			return false;
		}

		public int getNumberOfDescriptionCategories()
		{
			int num = 1;
			if (this.speed != ((this.type == 2) ? -8 : 0))
			{
				num++;
			}
			if (this.addedDefense > 0)
			{
				num++;
			}
			if ((double)this.critChance / 0.02 >= 2.0)
			{
				num++;
			}
			if ((double)(this.critMultiplier - 3f) / 0.02 >= 1.0)
			{
				num++;
			}
			if (this.knockback != this.defaultKnockBackForThisType(this.type))
			{
				num++;
			}
			return num;
		}

		public override void leftClick(Farmer who)
		{
			if (who.health <= 0 || Game1.activeClickableMenu != null || Game1.farmEvent != null)
			{
				return;
			}
			if (this.initialParentTileIndex != 47 && who.FarmerSprite.indexInCurrentAnimation > ((this.type == 2) ? 5 : ((this.type == 1) ? 0 : 5)))
			{
				who.completelyStopAnimatingOrDoingAction();
				Game1.player.CanMove = false;
				who.UsingTool = true;
				who.canReleaseTool = true;
				this.setFarmerAnimating(Game1.player);
				return;
			}
			if (this.initialParentTileIndex != 47 && who.FarmerSprite.indexInCurrentAnimation > ((this.type == 2) ? 3 : ((this.type == 1) ? 0 : 3)))
			{
				this.anotherClick = true;
			}
		}

		public int getItemLevel()
		{
			int num = 0;
			num += (int)((double)((this.maxDamage + this.minDamage) / 2) * (1.0 + 0.1 * (double)(Math.Max(0, this.speed) + ((this.type == 1) ? 15 : 0))));
			num += (int)((double)(this.addedPrecision / 2 + this.addedDefense) + ((double)this.critChance - 0.02) * 100.0 + (double)((this.critMultiplier - 3f) * 20f));
			if (this.type == 2)
			{
				num /= 2;
			}
			return num / 5 + 1;
		}

		public override string getDescription()
		{
			if (this.indexOfMenuItemView != 47)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(Game1.parseText(base.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14132", new object[]
				{
					this.minDamage,
					this.maxDamage
				}));
				if (this.speed != 0)
				{
					stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14134", new object[]
					{
						(this.speed > 0) ? "+" : "-",
						Math.Abs(this.speed)
					}));
				}
				if (this.addedAreaOfEffect > 0)
				{
					stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14136", new object[]
					{
						this.addedAreaOfEffect
					}));
				}
				if (this.addedPrecision > 0)
				{
					stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14138", new object[]
					{
						this.addedPrecision
					}));
				}
				if (this.addedDefense > 0)
				{
					stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14140", new object[]
					{
						this.addedDefense
					}));
				}
				if ((double)this.critChance / 0.02 >= 2.0)
				{
					stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14142", new object[]
					{
						(int)((double)this.critChance / 0.02)
					}));
				}
				if ((double)(this.critMultiplier - 3f) / 0.02 >= 1.0)
				{
					stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14144", new object[]
					{
						(int)((double)(this.critMultiplier - 3f) / 0.02)
					}));
				}
				if (this.knockback != this.defaultKnockBackForThisType(this.type))
				{
					stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:MeleeWeapon.cs.14140", new object[]
					{
						(this.knockback > this.defaultKnockBackForThisType(this.type)) ? "+" : "",
						(int)Math.Ceiling((double)(Math.Abs(this.knockback - this.defaultKnockBackForThisType(this.type)) * 10f))
					}));
				}
				return stringBuilder.ToString();
			}
			return Game1.parseText(base.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
		}

		public float defaultKnockBackForThisType(int type)
		{
			switch (type)
			{
			case 0:
			case 3:
				return 1f;
			case 1:
				return 0.5f;
			case 2:
				return 1.5f;
			default:
				return -1f;
			}
		}

		public virtual Rectangle getAreaOfEffect(int x, int y, int facingDirection, ref Vector2 tileLocation1, ref Vector2 tileLocation2, Rectangle wielderBoundingBox, int indexInCurrentAnimation)
		{
			Rectangle empty = Rectangle.Empty;
			int num = this.type;
			int num2;
			int num3;
			int num4;
			int num5;
			if (num == 1)
			{
				num2 = Game1.tileSize + Game1.tileSize / 6;
				num3 = Game1.tileSize * 3 / 4;
				num4 = Game1.tileSize * 2 / 3;
				num5 = -Game1.tileSize / 2;
			}
			else
			{
				num2 = Game1.tileSize;
				num3 = Game1.tileSize;
				num5 = -Game1.tileSize / 2;
				num4 = 0;
			}
			if (this.type == 1)
			{
				switch (facingDirection)
				{
				case 0:
					empty = new Rectangle(x - num2 / 2, wielderBoundingBox.Y - num3 - num4, num2 / 2, num3 + num4);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? empty.Left : empty.Right) / Game1.tileSize), (float)(empty.Top / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Top / Game1.tileSize));
					empty.Offset(Game1.pixelZoom * 5, -Game1.tileSize / 2 + Game1.pixelZoom * 4);
					empty.Height += Game1.tileSize / 4;
					empty.Width += Game1.pixelZoom * 5;
					break;
				case 1:
					empty = new Rectangle(wielderBoundingBox.Right, y - num3 / 2 + num5, num3, num2);
					tileLocation1 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? empty.Top : empty.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					empty.Offset(-Game1.pixelZoom, 0);
					empty.Width += Game1.tileSize / 4;
					break;
				case 2:
					empty = new Rectangle(x - num2 / 2, wielderBoundingBox.Bottom, num2, num3);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? empty.Left : empty.Right) / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					empty.Offset(Game1.pixelZoom * 3, -Game1.pixelZoom * 2);
					empty.Width -= Game1.tileSize / 3;
					break;
				case 3:
					empty = new Rectangle(wielderBoundingBox.Left - num3, y - num3 / 2 + num5, num3, num2);
					tileLocation1 = new Vector2((float)(empty.Left / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? empty.Top : empty.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Left / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					empty.Offset(-Game1.pixelZoom * 3, 0);
					empty.Width += Game1.tileSize / 4;
					break;
				}
			}
			else
			{
				switch (facingDirection)
				{
				case 0:
					empty = new Rectangle(x - num2 / 2, wielderBoundingBox.Y - num3 - num4, num2, num3 + num4);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? empty.Left : empty.Right) / Game1.tileSize), (float)(empty.Top / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Top / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						empty.Offset(-Game1.tileSize + Game1.pixelZoom, -Game1.pixelZoom * 3);
						break;
					case 1:
						empty.Offset(-Game1.pixelZoom * 12, -Game1.tileSize / 2 - Game1.pixelZoom * 6);
						empty.Height += Game1.tileSize / 2;
						break;
					case 2:
						empty.Offset(-Game1.pixelZoom * 3, -Game1.tileSize - Game1.pixelZoom);
						empty.Height += Game1.tileSize * 3 / 4;
						break;
					case 3:
						empty.Offset(Game1.pixelZoom * 10, -Game1.tileSize + Game1.pixelZoom);
						empty.Height += Game1.tileSize * 3 / 4;
						break;
					case 4:
						empty.Offset(Game1.tileSize - Game1.pixelZoom * 2, -Game1.tileSize / 2);
						empty.Height += Game1.tileSize / 2;
						break;
					case 5:
						empty.Offset(Game1.tileSize + Game1.pixelZoom * 3, -Game1.tileSize + Game1.pixelZoom * 8);
						break;
					}
					break;
				case 1:
					empty = new Rectangle(wielderBoundingBox.Right, y - num3 / 2 + num5, num3, num2);
					tileLocation1 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? empty.Top : empty.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						empty.Offset(-Game1.tileSize / 2 - Game1.pixelZoom * 3, -Game1.tileSize - Game1.pixelZoom * 5);
						break;
					case 1:
						empty.Offset(-Game1.tileSize / 2 + Game1.pixelZoom * 9, -Game1.tileSize + Game1.pixelZoom * 5);
						break;
					case 2:
						empty.Offset(-Game1.tileSize / 8 + Game1.pixelZoom * 5, -Game1.tileSize / 4 + Game1.pixelZoom * 3);
						break;
					case 3:
						empty.Offset(Game1.pixelZoom * 3, Game1.tileSize / 3 + Game1.pixelZoom * 4);
						break;
					case 4:
						empty.Offset(-Game1.pixelZoom * 7, Game1.tileSize / 2 + Game1.pixelZoom * 7);
						break;
					case 5:
						empty.Offset(-Game1.tileSize / 2 - Game1.pixelZoom * 7, Game1.tileSize + Game1.pixelZoom * 2);
						break;
					}
					break;
				case 2:
					empty = new Rectangle(x - num2 / 2, wielderBoundingBox.Bottom, num2, num3);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? empty.Left : empty.Right) / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						empty.Offset(Game1.tileSize + Game1.pixelZoom * 2, -Game1.tileSize - Game1.pixelZoom * 7);
						break;
					case 1:
						empty.Offset(Game1.tileSize - Game1.pixelZoom * 2, -Game1.tileSize / 2);
						break;
					case 2:
						empty.Offset(Game1.pixelZoom * 10, -Game1.tileSize / 2 + Game1.pixelZoom);
						break;
					case 3:
						empty.Offset(-Game1.pixelZoom * 3, -Game1.pixelZoom * 2);
						break;
					case 4:
						empty.Offset(-Game1.pixelZoom * 20, -Game1.tileSize / 2 + Game1.pixelZoom * 2);
						empty.Width += Game1.tileSize / 2;
						break;
					case 5:
						empty.Offset(-Game1.tileSize - Game1.pixelZoom, -Game1.tileSize / 2 - Game1.pixelZoom * 3);
						break;
					}
					break;
				case 3:
					empty = new Rectangle(wielderBoundingBox.Left - num3, y - num3 / 2 + num5, num3, num2);
					tileLocation1 = new Vector2((float)(empty.Left / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? empty.Top : empty.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(empty.Left / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						empty.Offset(Game1.tileSize / 2 + Game1.pixelZoom * 6, -Game1.tileSize - Game1.pixelZoom * 3);
						break;
					case 1:
						empty.Offset(-Game1.tileSize / 2 + Game1.pixelZoom * 6, -Game1.tileSize + Game1.pixelZoom * 2);
						break;
					case 2:
						empty.Offset(-Game1.tileSize / 8 - Game1.pixelZoom * 2, -Game1.tileSize / 4 + Game1.pixelZoom * 3);
						break;
					case 3:
						empty.Offset(0, Game1.tileSize / 3 + Game1.pixelZoom * 4);
						break;
					case 4:
						empty.Offset(Game1.pixelZoom * 6, Game1.tileSize / 2 + Game1.pixelZoom * 7);
						break;
					case 5:
						empty.Offset(Game1.tileSize, Game1.tileSize);
						break;
					}
					break;
				}
			}
			empty.Inflate(this.addedAreaOfEffect, this.addedAreaOfEffect);
			return empty;
		}

		public void doDefenseSwordFunction(Farmer who)
		{
			this.isOnSpecial = false;
			who.UsingTool = false;
		}

		public void doStabbingSwordFunction(Farmer who)
		{
			this.isOnSpecial = false;
			who.UsingTool = false;
			who.xVelocity = 0f;
			who.yVelocity = 0f;
		}

		public void doDaggerFunction(Farmer who)
		{
			Vector2 uniformPositionAwayFromBox = who.getUniformPositionAwayFromBox(who.facingDirection, Game1.tileSize * 3 / 4);
			float num = this.knockback;
			this.knockback = 0.1f;
			this.DoDamage(Game1.currentLocation, (int)uniformPositionAwayFromBox.X, (int)uniformPositionAwayFromBox.Y, who.facingDirection, 1, who);
			this.knockback = num;
			MeleeWeapon.daggerHitsLeft--;
			this.isOnSpecial = false;
			who.UsingTool = false;
			who.CanMove = true;
			who.FarmerSprite.pauseForSingleAnimation = false;
			if (MeleeWeapon.daggerHitsLeft > 0)
			{
				this.quickStab(who);
			}
		}

		public void doClubFunction(Farmer who)
		{
			Game1.playSound("clubSmash");
			Game1.currentLocation.damageMonster(new Rectangle((int)who.position.X - Game1.tileSize * 3, who.GetBoundingBox().Y - Game1.tileSize * 3, Game1.tileSize * 6, Game1.tileSize * 6), this.minDamage, this.maxDamage, false, 1.5f, 100, 0f, 1f, false, this.lastUser);
			Game1.viewport.Y = Game1.viewport.Y - Game1.tileSize / 3;
			Game1.viewport.X = Game1.viewport.X + Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2);
			Vector2 uniformPositionAwayFromBox = who.getUniformPositionAwayFromBox(who.facingDirection, Game1.tileSize);
			switch (who.facingDirection)
			{
			case 0:
			case 2:
				uniformPositionAwayFromBox.X -= (float)(Game1.tileSize / 2);
				uniformPositionAwayFromBox.Y -= (float)(Game1.tileSize / 2);
				break;
			case 1:
				uniformPositionAwayFromBox.X -= (float)(Game1.tileSize * 2 / 3);
				uniformPositionAwayFromBox.Y -= (float)(Game1.tileSize / 2);
				break;
			case 3:
				uniformPositionAwayFromBox.Y -= (float)(Game1.tileSize / 2);
				break;
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 128, 64, 64), 40f, 4, 0, uniformPositionAwayFromBox, false, who.facingDirection == 1));
			who.jitterStrength = 2f;
		}

		private void beginSpecialMove(Farmer who)
		{
			if (!Game1.fadeToBlack)
			{
				this.isOnSpecial = true;
				who.UsingTool = true;
				who.CanMove = false;
			}
		}

		private void quickStab(Farmer who)
		{
			switch (who.FacingDirection)
			{
			case 0:
				((FarmerSprite)who.Sprite).animateOnce(276, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(0, 0);
				break;
			case 1:
				((FarmerSprite)who.Sprite).animateOnce(274, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(1, 0);
				break;
			case 2:
				((FarmerSprite)who.Sprite).animateOnce(272, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(2, 0);
				break;
			case 3:
				((FarmerSprite)who.Sprite).animateOnce(278, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(3, 0);
				break;
			}
			this.beginSpecialMove(who);
			Game1.playSound("daggerswipe");
		}

		public void animateSpecialMove(Farmer who)
		{
			if ((this.type == 3 && (this.name.Contains("Scythe") || this.parentSheetIndex == 47)) || Game1.fadeToBlack)
			{
				return;
			}
			if (this.type == 3 && MeleeWeapon.defenseCooldown <= 0)
			{
				switch (who.FacingDirection)
				{
				case 0:
					((FarmerSprite)who.Sprite).animateOnce(252, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(0, 0);
					break;
				case 1:
					((FarmerSprite)who.Sprite).animateOnce(243, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(1, 0);
					break;
				case 2:
					((FarmerSprite)who.Sprite).animateOnce(234, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(2, 0);
					break;
				case 3:
					((FarmerSprite)who.Sprite).animateOnce(259, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(3, 0);
					break;
				}
				Game1.playSound("batFlap");
				this.beginSpecialMove(who);
				MeleeWeapon.defenseCooldown = 1500;
				if (who.professions.Contains(28))
				{
					MeleeWeapon.defenseCooldown /= 2;
					return;
				}
			}
			else if (this.type == 2 && MeleeWeapon.clubCooldown <= 0)
			{
				Game1.playSound("clubswipe");
				switch (who.FacingDirection)
				{
				case 0:
					((FarmerSprite)who.Sprite).animateOnce(176, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(0, 0);
					break;
				case 1:
					((FarmerSprite)who.Sprite).animateOnce(168, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(1, 0);
					break;
				case 2:
					((FarmerSprite)who.Sprite).animateOnce(160, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(2, 0);
					break;
				case 3:
					((FarmerSprite)who.Sprite).animateOnce(184, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(3, 0);
					break;
				}
				this.beginSpecialMove(who);
				MeleeWeapon.clubCooldown = 4000;
				if (who.professions.Contains(28))
				{
					MeleeWeapon.clubCooldown /= 2;
					return;
				}
			}
			else if (this.type == 1 && MeleeWeapon.daggerCooldown <= 0)
			{
				MeleeWeapon.daggerHitsLeft = 4;
				this.quickStab(who);
				MeleeWeapon.daggerCooldown = 6000;
				if (who.professions.Contains(28))
				{
					MeleeWeapon.daggerCooldown /= 2;
				}
			}
		}

		public static void doSwipe(int type, Vector2 position, int facingDirection, float swipeSpeed, Farmer f)
		{
			if (f == null)
			{
				return;
			}
			f.temporaryImpassableTile = Rectangle.Empty;
			f.currentLocation.lastTouchActionLocation = Vector2.Zero;
			swipeSpeed *= 1.3f;
			if (type == 3)
			{
				if (f.IsMainPlayer && f.CurrentTool != null)
				{
					switch (f.FacingDirection)
					{
					case 0:
						((FarmerSprite)f.Sprite).animateOnce(248, swipeSpeed, 6);
						f.CurrentTool.Update(0, 0);
						Game1.swordswipe(0, swipeSpeed * 2.1f, true);
						break;
					case 1:
						((FarmerSprite)f.Sprite).animateOnce(240, swipeSpeed, 6);
						f.CurrentTool.Update(1, 0);
						Game1.swordswipe(1, swipeSpeed * 2.1f, true);
						break;
					case 2:
						((FarmerSprite)f.Sprite).animateOnce(232, swipeSpeed, 6);
						f.CurrentTool.Update(2, 0);
						Game1.swordswipe(2, swipeSpeed * 2.1f, false);
						break;
					case 3:
						((FarmerSprite)f.Sprite).animateOnce(256, swipeSpeed, 6);
						f.CurrentTool.Update(3, 0);
						Game1.swordswipe(3, swipeSpeed * 2.1f, true);
						break;
					}
				}
				else if (f.facingDirection != 0)
				{
					int arg_15A_0 = f.facingDirection;
				}
				Game1.playSound("swordswipe");
				return;
			}
			if (type == 2)
			{
				if (f.IsMainPlayer && f.CurrentTool != null)
				{
					switch (f.FacingDirection)
					{
					case 0:
						((FarmerSprite)f.Sprite).animateOnce(248, swipeSpeed, 8);
						f.CurrentTool.Update(0, 0);
						break;
					case 1:
						((FarmerSprite)f.Sprite).animateOnce(240, swipeSpeed, 8);
						f.CurrentTool.Update(1, 0);
						break;
					case 2:
						((FarmerSprite)f.Sprite).animateOnce(232, swipeSpeed, 8);
						f.CurrentTool.Update(2, 0);
						break;
					case 3:
						((FarmerSprite)f.Sprite).animateOnce(256, swipeSpeed, 8);
						f.CurrentTool.Update(3, 0);
						break;
					}
				}
				Game1.playSound("clubswipe");
			}
		}

		public void setFarmerAnimating(Farmer who)
		{
			this.anotherClick = false;
			who.FarmerSprite.pauseForSingleAnimation = false;
			who.FarmerSprite.StopAnimation();
			this.hasBegunWeaponEndPause = false;
			this.swipeSpeed = (float)(400 - this.speed * 40 - who.addedSpeed * 40);
			this.swipeSpeed *= 1f - who.weaponSpeedModifier;
			if (this.type != 1)
			{
				MeleeWeapon.doSwipe(this.type, who.position, who.facingDirection, this.swipeSpeed / (float)((this.type == 2) ? 5 : 8), who);
				who.lastClick = Vector2.Zero;
				Vector2 toolLocation = who.GetToolLocation(true);
				if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon)
				{
					((MeleeWeapon)who.CurrentTool).DoDamage(Game1.currentLocation, (int)toolLocation.X, (int)toolLocation.Y, who.FacingDirection, 1, who);
				}
			}
			else
			{
				Game1.playSound("daggerswipe");
				this.swipeSpeed /= 4f;
				switch (who.FacingDirection)
				{
				case 0:
					((FarmerSprite)who.Sprite).animateOnce(276, this.swipeSpeed, 2);
					who.CurrentTool.Update(0, 0);
					break;
				case 1:
					((FarmerSprite)who.Sprite).animateOnce(274, this.swipeSpeed, 2);
					who.CurrentTool.Update(1, 0);
					break;
				case 2:
					((FarmerSprite)who.Sprite).animateOnce(272, this.swipeSpeed, 2);
					who.CurrentTool.Update(2, 0);
					break;
				case 3:
					((FarmerSprite)who.Sprite).animateOnce(278, this.swipeSpeed, 2);
					who.CurrentTool.Update(3, 0);
					break;
				}
				Vector2 toolLocation2 = who.GetToolLocation(true);
				if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon)
				{
					((MeleeWeapon)who.CurrentTool).DoDamage(Game1.currentLocation, (int)toolLocation2.X, (int)toolLocation2.Y, who.FacingDirection, 1, who);
				}
			}
			if (who.CurrentTool == null)
			{
				who.completelyStopAnimatingOrDoingAction();
				who.forceCanMove();
			}
		}

		public void DoDamage(GameLocation location, int x, int y, int facingDirection, int power, Farmer who)
		{
			this.isOnSpecial = false;
			if (this.type != 2)
			{
				base.DoFunction(location, x, y, power, who);
			}
			this.lastUser = who;
			Vector2 zero = Vector2.Zero;
			Vector2 zero2 = Vector2.Zero;
			Rectangle areaOfEffect = this.getAreaOfEffect(x, y, facingDirection, ref zero, ref zero2, who.GetBoundingBox(), who.FarmerSprite.indexInCurrentAnimation);
			this.mostRecentArea = areaOfEffect;
			if (who.IsMainPlayer && location.damageMonster(areaOfEffect, (int)((float)this.minDamage * (1f + who.attackIncreaseModifier)), (int)((float)this.maxDamage * (1f + who.attackIncreaseModifier)), false, this.knockback * (1f + who.knockbackModifier), (int)((float)this.addedPrecision * (1f + who.weaponPrecisionModifier)), this.critChance * (1f + who.critChanceModifier), this.critMultiplier * (1f + who.critPowerModifier), this.type != 1 || !this.isOnSpecial, this.lastUser) && this.type == 2)
			{
				Game1.playSound("clubhit");
			}
			string text = "";
			for (int i = location.projectiles.Count - 1; i >= 0; i--)
			{
				if (areaOfEffect.Intersects(location.projectiles[i].getBoundingBox()))
				{
					location.projectiles[i].behaviorOnCollisionWithOther(location);
				}
				if (location.projectiles[i].destroyMe)
				{
					location.projectiles.RemoveAt(i);
				}
			}
			foreach (Vector2 current in Utility.removeDuplicates(Utility.getListOfTileLocationsForBordersOfNonTileRectangle(areaOfEffect)))
			{
				if (location.terrainFeatures.ContainsKey(current) && location.terrainFeatures[current].performToolAction(this, 0, current, null))
				{
					location.terrainFeatures.Remove(current);
				}
				if (location.objects.ContainsKey(current) && location.objects[current].performToolAction(this))
				{
					location.objects.Remove(current);
				}
				if (location.performToolAction(this, (int)current.X, (int)current.Y))
				{
					break;
				}
			}
			if (!text.Equals(""))
			{
				Game1.playSound(text);
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			if (who != null && who.isRidingHorse())
			{
				who.completelyStopAnimatingOrDoingAction();
			}
		}

		public static Rectangle getSourceRect(int index)
		{
			return Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, index, 16, 16);
		}

		public void drawDuringUse(int frameOfFarmerAnimation, int facingDirection, SpriteBatch spriteBatch, Vector2 playerPosition, Farmer f)
		{
			MeleeWeapon.drawDuringUse(frameOfFarmerAnimation, facingDirection, spriteBatch, playerPosition, f, MeleeWeapon.getSourceRect(this.initialParentTileIndex), this.type, this.isOnSpecial);
		}

		public static void drawDuringUse(int frameOfFarmerAnimation, int facingDirection, SpriteBatch spriteBatch, Vector2 playerPosition, Farmer f, Rectangle sourceRect, int type, bool isOnSpecial)
		{
			Tool arg_07_0 = f.CurrentTool;
			if (type != 1)
			{
				if (isOnSpecial)
				{
					if (type == 3)
					{
						switch (f.FacingDirection)
						{
						case 0:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 8f, playerPosition.Y - 44f), new Rectangle?(sourceRect), Color.White, -1.76714587f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
							return;
						case 1:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 8f, playerPosition.Y - 4f), new Rectangle?(sourceRect), Color.White, -0.5890486f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 1) / 10000f));
							return;
						case 2:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 52f, playerPosition.Y + 4f), new Rectangle?(sourceRect), Color.White, -5.105088f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
							return;
						case 3:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 56f, playerPosition.Y - 4f), new Rectangle?(sourceRect), Color.White, -0.981747746f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 1) / 10000f));
							return;
						default:
							return;
						}
					}
					else if (type == 2)
					{
						if (facingDirection == 1)
						{
							switch (frameOfFarmerAnimation)
							{
							case 0:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 2) - 12f, playerPosition.Y - (float)(Game1.tileSize * 5 / 4)), new Rectangle?(sourceRect), Color.White, -1.17809725f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 1:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize, playerPosition.Y - (float)Game1.tileSize - 48f), new Rectangle?(sourceRect), Color.White, 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 2:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 2) - (float)(Game1.pixelZoom * 4), playerPosition.Y - (float)Game1.tileSize - (float)(Game1.pixelZoom * 3)), new Rectangle?(sourceRect), Color.White, 1.17809725f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 3:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 72f, playerPosition.Y - (float)Game1.tileSize + (float)(Game1.tileSize / 4) - 32f), new Rectangle?(sourceRect), Color.White, 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 4:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2), playerPosition.Y - (float)Game1.tileSize + (float)(Game1.tileSize / 4) - 16f), new Rectangle?(sourceRect), Color.White, 0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 5:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2) - 12f, playerPosition.Y - (float)Game1.tileSize + (float)(Game1.tileSize / 4)), new Rectangle?(sourceRect), Color.White, 0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 6:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2) - 16f, playerPosition.Y - (float)Game1.tileSize + (float)Game1.tileSize * 0.625f - 8f), new Rectangle?(sourceRect), Color.White, 0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 7:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2) - 8f, playerPosition.Y + (float)Game1.tileSize * 0.625f), new Rectangle?(sourceRect), Color.White, 0.981747746f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							default:
								return;
							}
						}
						else if (facingDirection == 3)
						{
							switch (frameOfFarmerAnimation)
							{
							case 0:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 4f + 8f, playerPosition.Y - 56f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 1:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 2), playerPosition.Y - (float)(Game1.tileSize / 2)), new Rectangle?(sourceRect), Color.White, -1.96349549f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 2:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 12f, playerPosition.Y + (float)(Game1.pixelZoom * 2)), new Rectangle?(sourceRect), Color.White, -2.74889374f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 3:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 2) - 4f, playerPosition.Y + (float)(Game1.pixelZoom * 2)), new Rectangle?(sourceRect), Color.White, -2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 4:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 4) - 24f, playerPosition.Y + (float)Game1.tileSize + 12f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, 4.31969f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 5:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 20f, playerPosition.Y + (float)Game1.tileSize + 40f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, 3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 6:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y + (float)Game1.tileSize + 56f), new Rectangle?(sourceRect), Color.White, 3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 7:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 8f, playerPosition.Y + (float)Game1.tileSize + 64f), new Rectangle?(sourceRect), Color.White, 3.73064137f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							default:
								return;
							}
						}
						else
						{
							switch (frameOfFarmerAnimation)
							{
							case 0:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 24f, playerPosition.Y - (float)(Game1.tileSize / 3) - 8f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								break;
							case 1:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y - (float)(Game1.tileSize / 3) - (float)Game1.tileSize + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								break;
							case 2:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y - (float)(Game1.tileSize / 3) + 20f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								break;
							case 3:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + (float)(Game1.pixelZoom * 2), playerPosition.Y + (float)(Game1.tileSize / 2)), new Rectangle?(sourceRect), Color.White, -3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								else
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y - (float)(Game1.tileSize / 3) + 32f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 4:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + (float)(Game1.pixelZoom * 2), playerPosition.Y + (float)(Game1.tileSize / 2)), new Rectangle?(sourceRect), Color.White, -3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 5:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + 12f, playerPosition.Y + (float)Game1.tileSize - 20f), new Rectangle?(sourceRect), Color.White, 2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 6:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + 12f, playerPosition.Y + (float)Game1.tileSize + 54f), new Rectangle?(sourceRect), Color.White, 2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 7:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + 12f, playerPosition.Y + (float)Game1.tileSize + 58f), new Rectangle?(sourceRect), Color.White, 2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							}
							if (f.facingDirection == 0)
							{
								f.FarmerRenderer.draw(spriteBatch, f.FarmerSprite, f.FarmerSprite.SourceRect, f.getLocalPosition(Game1.viewport), new Vector2(0f, (f.yOffset + (float)(Game1.tileSize * 2) - (float)(f.GetBoundingBox().Height / 2)) / 4f + 4f), Math.Max(0f, (float)f.getStandingY() / 10000f + 0.0099f), Color.White, 0f, f);
								return;
							}
						}
					}
				}
				else if (facingDirection == 1)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 40f, playerPosition.Y - (float)Game1.tileSize + 8f), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 56f, playerPosition.Y - (float)Game1.tileSize + 28f), new Rectangle?(sourceRect), Color.White, 0f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)Game1.pixelZoom, playerPosition.Y - (float)(4 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)Game1.pixelZoom, playerPosition.Y - (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(7 * Game1.pixelZoom), playerPosition.Y + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 1.96349549f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(12 * Game1.pixelZoom), playerPosition.Y + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(12 * Game1.pixelZoom), playerPosition.Y + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 16f, playerPosition.Y + (float)Game1.tileSize + 12f), new Rectangle?(sourceRect), Color.White, 1.96349537f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					default:
						return;
					}
				}
				else if (facingDirection == 3)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(4 * Game1.pixelZoom), playerPosition.Y - (float)Game1.tileSize - (float)(4 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(12 * Game1.pixelZoom), playerPosition.Y - (float)Game1.tileSize + (float)(5 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, 0f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)Game1.tileSize + (float)(8 * Game1.pixelZoom), playerPosition.Y + (float)(4 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.pixelZoom, playerPosition.Y + (float)(11 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(11 * Game1.pixelZoom), playerPosition.Y + (float)(13 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -1.96349549f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(20 * Game1.pixelZoom), playerPosition.Y + (float)(10 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(20 * Game1.pixelZoom), playerPosition.Y + (float)(10 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 44f, playerPosition.Y + 96f), new Rectangle?(sourceRect), Color.White, -5.105088f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipVertically, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					default:
						return;
					}
				}
				else if (facingDirection == 0)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 32f, playerPosition.Y - 32f), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 32f, playerPosition.Y - 48f), new Rectangle?(sourceRect), Color.White, -1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 48f, playerPosition.Y - 52f), new Rectangle?(sourceRect), Color.White, -1.17809725f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 48f, playerPosition.Y - 52f), new Rectangle?(sourceRect), Color.White, -0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 8f, playerPosition.Y - 40f), new Rectangle?(sourceRect), Color.White, 0f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize, playerPosition.Y - 40f), new Rectangle?(sourceRect), Color.White, 0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize, playerPosition.Y - 40f), new Rectangle?(sourceRect), Color.White, 0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 44f, playerPosition.Y + (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -1.96349537f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					default:
						return;
					}
				}
				else if (facingDirection == 2)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 56f, playerPosition.Y - 16f), new Rectangle?(sourceRect), Color.White, 0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 52f, playerPosition.Y - 8f), new Rectangle?(sourceRect), Color.White, 1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 40f, playerPosition.Y), new Rectangle?(sourceRect), Color.White, 1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 16f, playerPosition.Y + 4f), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 8f, playerPosition.Y + 8f), new Rectangle?(sourceRect), Color.White, 3.14159274f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 12f, playerPosition.Y), new Rectangle?(sourceRect), Color.White, 3.53429174f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 12f, playerPosition.Y), new Rectangle?(sourceRect), Color.White, 3.53429174f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 44f, playerPosition.Y + (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -5.105088f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					default:
						return;
					}
				}
			}
			else
			{
				frameOfFarmerAnimation %= 2;
				if (facingDirection == 1)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(4 * Game1.pixelZoom), playerPosition.Y - 16f), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(2 * Game1.pixelZoom), playerPosition.Y - 24f), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
					return;
				}
				else if (facingDirection == 3)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(4 * Game1.pixelZoom), playerPosition.Y - 16f), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(2 * Game1.pixelZoom), playerPosition.Y - 24f), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
					return;
				}
				else if (facingDirection == 0)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 4f, playerPosition.Y - (float)(10 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 16f, playerPosition.Y - (float)(12 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2) / 10000f));
					return;
				}
				else if (facingDirection == 2)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize / 2), playerPosition.Y - 12f), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize / 3), playerPosition.Y), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
				}
			}
		}
	}
}
