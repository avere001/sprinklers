using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

namespace StardewValley.Tools
{
	public class Hoe : Tool
	{
		public Hoe() : base("Hoe", 0, 21, 47, false, 0)
		{
			this.upgradeLevel = 0;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Hoe.cs.14101", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Hoe.cs.14102", new object[0]);
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			if (location.Name.Equals("UndergroundMine"))
			{
				power = 1;
			}
			who.Stamina -= (float)(2 * power) - (float)who.FarmingLevel * 0.1f;
			power = who.toolPower;
			who.stopJittering();
			Game1.playSound("woodyHit");
			Vector2 vector = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			List<Vector2> list = base.tilesAffected(vector, power, who);
			foreach (Vector2 current in list)
			{
				current.Equals(vector);
				if (location.terrainFeatures.ContainsKey(current))
				{
					if (location.terrainFeatures[current].performToolAction(this, 0, current, null))
					{
						location.terrainFeatures.Remove(current);
					}
				}
				else
				{
					if (location.objects.ContainsKey(current) && location.Objects[current].performToolAction(this))
					{
						if (location.Objects[current].type.Equals("Crafting") && location.Objects[current].fragility != 2)
						{
							location.debris.Add(new Debris(location.Objects[current].bigCraftable ? (-location.Objects[current].ParentSheetIndex) : location.Objects[current].ParentSheetIndex, who.GetToolLocation(false), new Vector2((float)who.GetBoundingBox().Center.X, (float)who.GetBoundingBox().Center.Y)));
						}
						location.Objects[current].performRemoveAction(current, location);
						location.Objects.Remove(current);
					}
					if (location.doesTileHaveProperty((int)current.X, (int)current.Y, "Diggable", "Back") != null)
					{
						if (location.Name.Equals("UndergroundMine") && !location.isTileOccupied(current, ""))
						{
							if (Game1.mine.mineLevel < 40 || Game1.mine.mineLevel >= 80)
							{
								location.terrainFeatures.Add(current, new HoeDirt());
								Game1.playSound("hoeHit");
							}
							else if (Game1.mine.mineLevel < 80)
							{
								location.terrainFeatures.Add(current, new HoeDirt());
								Game1.playSound("hoeHit");
							}
							Game1.removeSquareDebrisFromTile((int)current.X, (int)current.Y);
							location.checkForBuriedItem((int)current.X, (int)current.Y, false, false);
							location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(vector.X * (float)Game1.tileSize, vector.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
							if (list.Count > 2)
							{
								location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(current.X * (float)Game1.tileSize, current.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(vector, current) * 30f, 0, -1, -1f, -1, 0));
							}
						}
						else if (!location.isTileOccupied(current, "") && location.isTilePassable(new Location((int)current.X, (int)current.Y), Game1.viewport))
						{
							location.makeHoeDirt(current);
							Game1.playSound("hoeHit");
							Game1.removeSquareDebrisFromTile((int)current.X, (int)current.Y);
							location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(current.X * (float)Game1.tileSize, current.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
							if (list.Count > 2)
							{
								location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(current.X * (float)Game1.tileSize, current.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(vector, current) * 30f, 0, -1, -1f, -1, 0));
							}
							location.checkForBuriedItem((int)current.X, (int)current.Y, false, false);
						}
						Stats expr_4B2 = Game1.stats;
						uint dirtHoed = expr_4B2.DirtHoed;
						expr_4B2.DirtHoed = dirtHoed + 1u;
					}
				}
			}
		}
	}
}
