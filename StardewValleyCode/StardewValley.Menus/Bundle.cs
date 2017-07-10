using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class Bundle : ClickableComponent
	{
		public const float shakeRate = 0.0157079641f;

		public const float shakeDecayRate = 0.00306796166f;

		public const int Color_Green = 0;

		public const int Color_Purple = 1;

		public const int Color_Orange = 2;

		public const int Color_Yellow = 3;

		public const int Color_Red = 4;

		public const int Color_Blue = 5;

		public const int Color_Teal = 6;

		public const float DefaultShakeForce = 0.07363108f;

		public string rewardDescription;

		public List<BundleIngredientDescription> ingredients;

		public int bundleColor;

		public int numberOfIngredientSlots;

		public int bundleIndex;

		public int completionTimer;

		public bool complete;

		public bool depositsAllowed = true;

		public TemporaryAnimatedSprite sprite;

		private float maxShake;

		private bool shakeLeft;

		public Bundle(int bundleIndex, string rawBundleInfo, bool[] completedIngredientsList, Point position, Texture2D texture, JunimoNoteMenu menu) : base(new Rectangle(position.X, position.Y, Game1.tileSize, Game1.tileSize), "")
		{
			if (menu.fromGameMenu)
			{
				this.depositsAllowed = false;
			}
			this.bundleIndex = bundleIndex;
			string[] array = rawBundleInfo.Split(new char[]
			{
				'/'
			});
			this.name = array[0];
			this.label = array[0];
			if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
			{
				this.label = array[array.Length - 1];
			}
			this.rewardDescription = array[1];
			string[] array2 = array[2].Split(new char[]
			{
				' '
			});
			this.complete = true;
			this.ingredients = new List<BundleIngredientDescription>();
			int num = 0;
			for (int i = 0; i < array2.Length; i += 3)
			{
				this.ingredients.Add(new BundleIngredientDescription(Convert.ToInt32(array2[i]), Convert.ToInt32(array2[i + 1]), Convert.ToInt32(array2[i + 2]), completedIngredientsList[i / 3]));
				if (!completedIngredientsList[i / 3])
				{
					this.complete = false;
				}
				else
				{
					num++;
				}
			}
			this.bundleColor = Convert.ToInt32(array[3]);
			int num2 = 4;
			if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
			{
				num2 = 5;
			}
			this.numberOfIngredientSlots = ((array.Length > num2) ? Convert.ToInt32(array[4]) : this.ingredients.Count<BundleIngredientDescription>());
			if (num >= this.numberOfIngredientSlots)
			{
				this.complete = true;
			}
			this.sprite = new TemporaryAnimatedSprite(texture, new Rectangle(this.bundleColor * 256 % 512, 244 + this.bundleColor * 256 / 512 * 16, 16, 16), 70f, 3, 99999, new Vector2((float)this.bounds.X, (float)this.bounds.Y), false, false, 0.8f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
			{
				pingPong = true
			};
			this.sprite.paused = true;
			TemporaryAnimatedSprite expr_20A_cp_0_cp_0 = this.sprite;
			expr_20A_cp_0_cp_0.sourceRect.X = expr_20A_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width;
			if (this.name.ToLower().Contains(Game1.currentSeason) && !this.complete)
			{
				this.shake(0.07363108f);
			}
			if (this.complete)
			{
				this.completionAnimation(menu, false, 0);
			}
		}

		public Item getReward()
		{
			return Utility.getItemFromStandardTextDescription(this.rewardDescription, Game1.player, ' ');
		}

		public void shake(float force = 0.07363108f)
		{
			if (this.sprite.paused)
			{
				this.maxShake = force;
			}
		}

		public void shake(int extraInfo)
		{
			this.maxShake = 0.07363108f;
			if (extraInfo == 1)
			{
				Game1.playSound("leafrustle");
				JunimoNoteMenu.tempSprites.Add(new TemporaryAnimatedSprite(50, this.sprite.position, Bundle.getColorFromColorIndex(this.bundleColor), 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					motion = new Vector2(-1f, 0.5f),
					acceleration = new Vector2(0f, 0.02f)
				});
				JunimoNoteMenu.tempSprites.Add(new TemporaryAnimatedSprite(50, this.sprite.position, Bundle.getColorFromColorIndex(this.bundleColor), 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					motion = new Vector2(1f, 0.5f),
					acceleration = new Vector2(0f, 0.02f),
					flipped = true,
					delayBeforeAnimationStart = 50
				});
			}
		}

		public void shakeAndAllowClicking(int extraInfo)
		{
			this.maxShake = 0.07363108f;
			JunimoNoteMenu.canClick = true;
		}

		public void tryHoverAction(int x, int y)
		{
			if (this.bounds.Contains(x, y) && !this.complete)
			{
				this.sprite.paused = false;
				JunimoNoteMenu.hoverText = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
				{
					this.label
				});
				return;
			}
			if (!this.complete)
			{
				this.sprite.reset();
				TemporaryAnimatedSprite expr_6A_cp_0_cp_0 = this.sprite;
				expr_6A_cp_0_cp_0.sourceRect.X = expr_6A_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width;
				this.sprite.paused = true;
			}
		}

		public bool canAcceptThisItem(Item item, ClickableTextureComponent slot)
		{
			if (!this.depositsAllowed)
			{
				return false;
			}
			if (item is StardewValley.Object)
			{
				StardewValley.Object @object = item as StardewValley.Object;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (!this.ingredients[i].completed && this.ingredients[i].index == item.parentSheetIndex && this.ingredients[i].stack <= item.Stack && this.ingredients[i].quality <= @object.quality && slot.item == null)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public Item tryToDepositThisItem(Item item, ClickableTextureComponent slot, Texture2D noteTexture)
		{
			if (!this.depositsAllowed)
			{
				Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:JunimoNote_MustBeAtCC", new object[0]));
				return item;
			}
			if (!(item is StardewValley.Object))
			{
				return item;
			}
			StardewValley.Object @object = item as StardewValley.Object;
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (!this.ingredients[i].completed && this.ingredients[i].index == item.parentSheetIndex && item.Stack >= this.ingredients[i].stack && @object.quality >= this.ingredients[i].quality && slot.item == null)
				{
					item.Stack -= this.ingredients[i].stack;
					this.ingredients[i] = new BundleIngredientDescription(this.ingredients[i].index, this.ingredients[i].stack, this.ingredients[i].quality, true);
					this.ingredientDepositAnimation(slot, noteTexture, false);
					slot.item = new StardewValley.Object(this.ingredients[i].index, this.ingredients[i].stack, false, -1, this.ingredients[i].quality);
					Game1.playSound("newArtifact");
					(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[this.bundleIndex][i] = true;
					slot.sourceRect.X = 512;
					slot.sourceRect.Y = 244;
					break;
				}
			}
			if (item.Stack > 0)
			{
				return item;
			}
			return null;
		}

		public void ingredientDepositAnimation(ClickableTextureComponent slot, Texture2D noteTexture, bool skipAnimation = false)
		{
			TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(noteTexture, new Rectangle(530, 244, 18, 18), 50f, 6, 1, new Vector2((float)slot.bounds.X, (float)slot.bounds.Y), false, false, 0.88f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true)
			{
				holdLastFrame = true,
				endSound = "cowboy_monsterhit"
			};
			if (skipAnimation)
			{
				temporaryAnimatedSprite.sourceRect.Offset(temporaryAnimatedSprite.sourceRect.Width * 5, 0);
				temporaryAnimatedSprite.sourceRectStartingPos = new Vector2((float)temporaryAnimatedSprite.sourceRect.X, (float)temporaryAnimatedSprite.sourceRect.Y);
				temporaryAnimatedSprite.animationLength = 1;
			}
			JunimoNoteMenu.tempSprites.Add(temporaryAnimatedSprite);
		}

		public bool canBeClicked()
		{
			return !this.complete;
		}

		public void completionAnimation(JunimoNoteMenu menu, bool playSound = true, int delay = 0)
		{
			if (delay <= 0)
			{
				this.completionAnimation(playSound);
				return;
			}
			this.completionTimer = delay;
		}

		private void completionAnimation(bool playSound = true)
		{
			if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is JunimoNoteMenu)
			{
				(Game1.activeClickableMenu as JunimoNoteMenu).takeDownBundleSpecificPage(null);
			}
			this.sprite.pingPong = false;
			this.sprite.paused = false;
			this.sprite.sourceRect.X = (int)this.sprite.sourceRectStartingPos.X;
			TemporaryAnimatedSprite expr_6C_cp_0_cp_0 = this.sprite;
			expr_6C_cp_0_cp_0.sourceRect.X = expr_6C_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width;
			this.sprite.animationLength = 15;
			this.sprite.interval = 50f;
			this.sprite.totalNumberOfLoops = 0;
			this.sprite.holdLastFrame = true;
			this.sprite.endFunction = new TemporaryAnimatedSprite.endBehavior(this.shake);
			this.sprite.extraInfoForEndBehavior = 1;
			if (this.complete)
			{
				TemporaryAnimatedSprite expr_F3_cp_0_cp_0 = this.sprite;
				expr_F3_cp_0_cp_0.sourceRect.X = expr_F3_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width * 14;
				this.sprite.sourceRectStartingPos = new Vector2((float)this.sprite.sourceRect.X, (float)this.sprite.sourceRect.Y);
				this.sprite.currentParentTileIndex = 14;
				this.sprite.interval = 0f;
				this.sprite.animationLength = 1;
				this.sprite.extraInfoForEndBehavior = 0;
			}
			else
			{
				if (playSound)
				{
					Game1.playSound("dwop");
				}
				this.bounds.Inflate(Game1.tileSize, Game1.tileSize);
				JunimoNoteMenu.tempSprites.AddRange(Utility.sparkleWithinArea(this.bounds, 8, Bundle.getColorFromColorIndex(this.bundleColor) * 0.5f, 100, 0, ""));
				this.bounds.Inflate(-Game1.tileSize, -Game1.tileSize);
			}
			this.complete = true;
		}

		public void update(GameTime time)
		{
			this.sprite.update(time);
			if (this.completionTimer > 0 && JunimoNoteMenu.screenSwipe == null)
			{
				this.completionTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.completionTimer <= 0)
				{
					this.completionAnimation(true);
				}
			}
			if (Game1.random.NextDouble() < 0.005 && (this.complete || this.name.ToLower().Contains(Game1.currentSeason)))
			{
				this.shake(0.07363108f);
			}
			if (this.maxShake > 0f)
			{
				if (this.shakeLeft)
				{
					this.sprite.rotation -= 0.0157079641f;
					if (this.sprite.rotation <= -this.maxShake)
					{
						this.shakeLeft = false;
					}
				}
				else
				{
					this.sprite.rotation += 0.0157079641f;
					if (this.sprite.rotation >= this.maxShake)
					{
						this.shakeLeft = true;
					}
				}
			}
			if (this.maxShake > 0f)
			{
				this.maxShake = Math.Max(0f, this.maxShake - 0.0007669904f);
			}
		}

		public void draw(SpriteBatch b)
		{
			this.sprite.draw(b, true, 0, 0);
		}

		public static Color getColorFromColorIndex(int color)
		{
			switch (color)
			{
			case 0:
				return Color.Lime;
			case 1:
				return Color.DeepPink;
			case 2:
				return Color.Orange;
			case 3:
				return Color.Orange;
			case 4:
				return Color.Red;
			case 5:
				return Color.LightBlue;
			case 6:
				return Color.Cyan;
			default:
				return Color.Lime;
			}
		}
	}
}
