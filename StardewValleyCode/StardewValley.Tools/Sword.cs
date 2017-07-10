using Microsoft.Xna.Framework;
using StardewValley.Quests;
using System;

namespace StardewValley.Tools
{
	public class Sword : Tool
	{
		public const double baseCritChance = 0.02;

		public int whichUpgrade;

		public Sword()
		{
		}

		public Sword(string name, int spriteIndex) : base(name, 0, spriteIndex, spriteIndex, false, 0)
		{
		}

		public void DoFunction(GameLocation location, int x, int y, int facingDirection, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			Vector2 zero = Vector2.Zero;
			Vector2 zero2 = Vector2.Zero;
			Rectangle empty = Rectangle.Empty;
			Rectangle boundingBox = who.GetBoundingBox();
			switch (facingDirection)
			{
			case 0:
				empty = new Rectangle(x - Game1.tileSize, boundingBox.Y - Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
				zero = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? empty.Left : empty.Right) / Game1.tileSize), (float)(empty.Top / Game1.tileSize));
				zero2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Top / Game1.tileSize));
				break;
			case 1:
				empty = new Rectangle(boundingBox.Right, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
				zero = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? empty.Top : empty.Bottom) / Game1.tileSize));
				zero2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
				break;
			case 2:
				empty = new Rectangle(x - Game1.tileSize, boundingBox.Bottom, Game1.tileSize * 2, Game1.tileSize);
				zero = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? empty.Left : empty.Right) / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
				zero2 = new Vector2((float)(empty.Center.X / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
				break;
			case 3:
				empty = new Rectangle(boundingBox.Left - Game1.tileSize, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
				zero = new Vector2((float)(empty.Left / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? empty.Top : empty.Bottom) / Game1.tileSize));
				zero2 = new Vector2((float)(empty.Left / Game1.tileSize), (float)(empty.Center.Y / Game1.tileSize));
				break;
			}
			int minDamage = ((this.whichUpgrade == 2) ? 3 : ((this.whichUpgrade == 4) ? 6 : this.whichUpgrade)) + 1;
			int maxDamage = 4 * (((this.whichUpgrade == 2) ? 3 : ((this.whichUpgrade == 4) ? 5 : this.whichUpgrade)) + 1);
			bool flag = location.damageMonster(empty, minDamage, maxDamage, false, who);
			if (this.whichUpgrade == 4 && !flag)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float)Game1.random.Next(50, 120), 2, 1, new Vector2((float)(empty.Center.X - Game1.tileSize / 2), (float)(empty.Center.Y - Game1.tileSize / 2)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5));
			}
			string text = "";
			if (!flag)
			{
				if (location.objects.ContainsKey(zero) && !location.Objects[zero].Name.Contains("Stone") && !location.Objects[zero].Name.Contains("Stick") && !location.Objects[zero].Name.Contains("Stump") && !location.Objects[zero].Name.Contains("Boulder") && !location.Objects[zero].Name.Contains("Lumber") && !location.Objects[zero].IsHoeDirt)
				{
					if (location.Objects[zero].Name.Contains("Weed"))
					{
						if (who.Stamina <= 0f)
						{
							return;
						}
						Stats expr_4B7 = Game1.stats;
						uint weedsEliminated = expr_4B7.WeedsEliminated;
						expr_4B7.WeedsEliminated = weedsEliminated + 1u;
						if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest"))
						{
							((WeedingQuest)Game1.questOfTheDay).checkIfComplete(null, -1, -1, null, null);
						}
						this.checkWeedForTreasure(zero, who);
						int category = location.Objects[zero].Category;
						if (category == -2)
						{
							text = "stoneCrack";
						}
						else
						{
							text = "cut";
						}
						location.removeObject(zero, true);
					}
					else
					{
						location.objects[zero].performToolAction(this);
					}
				}
				if (location.objects.ContainsKey(zero2) && !location.Objects[zero2].Name.Contains("Stone") && !location.Objects[zero2].Name.Contains("Stick") && !location.Objects[zero2].Name.Contains("Stump") && !location.Objects[zero2].Name.Contains("Boulder") && !location.Objects[zero2].Name.Contains("Lumber") && !location.Objects[zero2].IsHoeDirt)
				{
					if (location.Objects[zero2].Name.Contains("Weed"))
					{
						if (who.Stamina <= 0f)
						{
							return;
						}
						Stats expr_661 = Game1.stats;
						uint weedsEliminated = expr_661.WeedsEliminated;
						expr_661.WeedsEliminated = weedsEliminated + 1u;
						if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest"))
						{
							((WeedingQuest)Game1.questOfTheDay).checkIfComplete(null, -1, -1, null, null);
						}
						this.checkWeedForTreasure(zero2, who);
					}
					else
					{
						location.objects[zero2].performToolAction(this);
					}
				}
			}
			foreach (Vector2 current in Utility.getListOfTileLocationsForBordersOfNonTileRectangle(empty))
			{
				if (location.terrainFeatures.ContainsKey(current) && location.terrainFeatures[current].performToolAction(this, 0, current, null))
				{
					location.terrainFeatures.Remove(current);
				}
			}
			if (!text.Equals(""))
			{
				Game1.playSound(text);
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}

		public void checkWeedForTreasure(Vector2 tileLocation, Farmer who)
		{
			Random random = new Random((int)(Game1.uniqueIDForThisGame + (ulong)Game1.stats.DaysPlayed + tileLocation.X * 13f + tileLocation.Y * 29f));
			if (random.NextDouble() < 0.07)
			{
				Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, random.Next(1, 3), null);
				return;
			}
			if (random.NextDouble() < 0.02 + (double)who.LuckLevel / 10.0)
			{
				Game1.createDebris((random.NextDouble() < 0.5) ? 4 : 8, (int)tileLocation.X, (int)tileLocation.Y, random.Next(1, 4), null);
				return;
			}
			if (random.NextDouble() < 0.006 + (double)who.LuckLevel / 20.0)
			{
				Game1.createObjectDebris(114, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
			}
		}

		protected override string loadDisplayName()
		{
			if (this.name.Equals("Battered Sword"))
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1205", new object[0]);
			}
			switch (this.whichUpgrade)
			{
			case 1:
				IL_47:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14290", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14292", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14294", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14296", new object[0]);
			}
			goto IL_47;
		}

		protected override string loadDescription()
		{
			switch (this.whichUpgrade)
			{
			case 1:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14291", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14293", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14295", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14297", new object[0]);
			default:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1206", new object[0]);
			}
		}

		public void upgrade(int which)
		{
			if (which > this.whichUpgrade)
			{
				this.whichUpgrade = which;
				switch (which)
				{
				case 1:
					this.name = "Hero's Sword";
					this.indexOfMenuItemView = 68;
					break;
				case 2:
					this.name = "Holy Sword";
					this.indexOfMenuItemView = 70;
					break;
				case 3:
					this.name = "Dark Sword";
					this.indexOfMenuItemView = 69;
					break;
				case 4:
					this.name = "Galaxy Sword";
					this.indexOfMenuItemView = 71;
					break;
				}
				this.displayName = null;
				base.description = null;
				this.upgradeLevel = which;
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}
	}
}
