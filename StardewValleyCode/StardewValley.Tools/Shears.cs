using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Tools
{
	public class Shears : Tool
	{
		private FarmAnimal animal;

		public Shears() : base("Shears", -1, 7, 7, false, 0)
		{
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14240", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14241", new object[0]);
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
			who.Halt();
			int currentFrame = who.FarmerSprite.currentFrame;
			who.FarmerSprite.animateOnce(283 + who.FacingDirection, 50f, 4);
			who.FarmerSprite.oldFrame = currentFrame;
			who.UsingTool = true;
			who.CanMove = false;
			return true;
		}

		public static void playSnip(Farmer who)
		{
			Game1.playSound("scissors");
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			who.Stamina -= 4f;
			Shears.playSnip(who);
			this.currentParentTileIndex = 7;
			this.indexOfMenuItemView = 7;
			if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
			{
				if (who.addItemToInventoryBool(new StardewValley.Object(Vector2.Zero, this.animal.currentProduce, null, false, true, false, false)
				{
					quality = this.animal.produceQuality
				}, false))
				{
					this.animal.currentProduce = -1;
					Game1.playSound("coin");
					this.animal.friendshipTowardFarmer = Math.Min(1000, this.animal.friendshipTowardFarmer + 5);
					if (this.animal.showDifferentTextureWhenReadyForHarvest)
					{
						this.animal.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheared" + this.animal.type);
					}
					who.gainExperience(0, 5);
				}
			}
			else
			{
				string text = "";
				if (this.animal != null && !this.animal.toolUsedForHarvest.Equals(this.name))
				{
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14245", new object[]
					{
						this.animal.displayName
					});
				}
				if (this.animal != null && this.animal.isBaby() && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14246", new object[]
					{
						this.animal.displayName
					});
				}
				if (this.animal != null && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14247", new object[]
					{
						this.animal.displayName
					});
				}
				if (text.Length > 0)
				{
					Game1.drawObjectDialogue(text);
				}
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
