using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Tools
{
	public class MilkPail : Tool
	{
		private FarmAnimal animal;

		public MilkPail() : base("Milk Pail", -1, 6, 6, false, 0)
		{
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14167", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14168", new object[0]);
		}

		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			x = (int)who.GetToolLocation(false).X;
			y = (int)who.GetToolLocation(false).Y;
			Rectangle value = new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize);
			if (location is Farm)
			{
				using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator = (location as Farm).animals.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FarmAnimal current = enumerator.Current;
						if (current.GetBoundingBox().Intersects(value))
						{
							this.animal = current;
							break;
						}
					}
					goto IL_FE;
				}
			}
			if (location is AnimalHouse)
			{
				foreach (FarmAnimal current2 in (location as AnimalHouse).animals.Values)
				{
					if (current2.GetBoundingBox().Intersects(value))
					{
						this.animal = current2;
						break;
					}
				}
			}
			IL_FE:
			if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name) && who.couldInventoryAcceptThisObject(this.animal.currentProduce, 1, 0))
			{
				this.animal.doEmote(20, true);
				this.animal.friendshipTowardFarmer = Math.Min(1000, this.animal.friendshipTowardFarmer + 5);
				Game1.playSound("Milking");
				this.animal.pauseTimer = 1500;
			}
			else if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature)
			{
				if (!this.animal.toolUsedForHarvest.Equals(this.name))
				{
					if (this.animal.toolUsedForHarvest != null && !this.animal.toolUsedForHarvest.Equals("null"))
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14167", new object[]
						{
							this.animal.toolUsedForHarvest
						}));
					}
				}
				else if (!who.couldInventoryAcceptThisObject(this.animal.currentProduce, 1, 0))
				{
					Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588", new object[0]));
				}
			}
			else
			{
				DelayedAction.playSoundAfterDelay("fishingRodBend", 300);
				DelayedAction.playSoundAfterDelay("fishingRodBend", 1200);
				string text = "";
				if (this.animal != null && !this.animal.toolUsedForHarvest.Equals(this.name))
				{
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14175", new object[]
					{
						this.animal.displayName
					});
				}
				if (this.animal != null && this.animal.isBaby() && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14176", new object[]
					{
						this.animal.displayName
					});
				}
				if (this.animal != null && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14177", new object[]
					{
						this.animal.displayName
					});
				}
				if (text.Length > 0)
				{
					DelayedAction.showDialogueAfterDelay(text, 1000);
				}
			}
			who.Halt();
			int currentFrame = who.FarmerSprite.currentFrame;
			who.FarmerSprite.animateOnce(287 + who.FacingDirection, 50f, 4);
			who.FarmerSprite.oldFrame = currentFrame;
			who.UsingTool = true;
			who.CanMove = false;
			return true;
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			who.Stamina -= 4f;
			this.currentParentTileIndex = 6;
			this.indexOfMenuItemView = 6;
			if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name) && who.addItemToInventoryBool(new StardewValley.Object(Vector2.Zero, this.animal.currentProduce, null, false, true, false, false)
			{
				quality = this.animal.produceQuality
			}, false))
			{
				Game1.playSound("coin");
				this.animal.currentProduce = -1;
				if (this.animal.showDifferentTextureWhenReadyForHarvest)
				{
					this.animal.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheared" + this.animal.type);
				}
				who.gainExperience(0, 5);
			}
			this.animal = null;
			if (Game1.activeClickableMenu == null)
			{
				who.CanMove = true;
			}
			else
			{
				who.Halt();
			}
			who.usingTool = false;
			who.canReleaseTool = true;
		}
	}
}
