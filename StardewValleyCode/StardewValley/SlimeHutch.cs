using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Monsters;
using StardewValley.Tools;
using System;
using xTile;

namespace StardewValley
{
	public class SlimeHutch : GameLocation
	{
		private int slimeMatingsLeft;

		public bool[] waterSpots = new bool[4];

		public SlimeHutch()
		{
		}

		public SlimeHutch(Map m, string name) : base(m, name)
		{
		}

		public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
		{
		}

		public bool isFull()
		{
			return this.characters.Count >= 20;
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			for (int i = 0; i < this.waterSpots.Length; i++)
			{
				if (this.waterSpots[i])
				{
					base.setMapTileIndex(16, 6 + i, 2135, "Buildings", 0);
				}
				else
				{
					base.setMapTileIndex(16, 6 + i, 2134, "Buildings", 0);
				}
			}
		}

		public Building getBuilding()
		{
			foreach (Building current in Game1.getFarm().buildings)
			{
				if (current.indoors != null && current.indoors.Equals(this))
				{
					return current;
				}
			}
			return null;
		}

		public override bool canSlimeMateHere()
		{
			int num = this.slimeMatingsLeft;
			this.slimeMatingsLeft--;
			return !this.isFull() && num > 0;
		}

		public override bool canSlimeHatchHere()
		{
			return !this.isFull();
		}

		public override void DayUpdate(int dayOfMonth)
		{
			int num = 0;
			for (int i = 0; i < this.waterSpots.Length; i++)
			{
				if (this.waterSpots[i] && num * 5 < this.characters.Count)
				{
					num++;
					this.waterSpots[i] = false;
					base.setMapTileIndex(16, 6 + i, 2134, "Buildings", 0);
				}
			}
			for (int j = Math.Min(this.characters.Count / 5, num); j > 0; j--)
			{
				int num2 = 50;
				Vector2 randomTile = base.getRandomTile();
				while ((!this.isTileLocationTotallyClearAndPlaceable(randomTile) || base.doesTileHaveProperty((int)randomTile.X, (int)randomTile.Y, "NPCBarrier", "Back") != null || randomTile.Y >= 12f) && num2 > 0)
				{
					randomTile = base.getRandomTile();
					num2--;
				}
				if (num2 > 0)
				{
					this.objects.Add(randomTile, new Object(randomTile, 56, false));
				}
			}
			while (this.slimeMatingsLeft > 0)
			{
				if (this.characters.Count > 1 && !this.isFull())
				{
					NPC nPC = this.characters[Game1.random.Next(this.characters.Count)];
					if (nPC is GreenSlime)
					{
						GreenSlime greenSlime = nPC as GreenSlime;
						if (greenSlime.ageUntilFullGrown <= 0)
						{
							for (int k = 1; k < 10; k++)
							{
								GreenSlime greenSlime2 = (GreenSlime)Utility.checkForCharacterWithinArea(greenSlime.GetType(), nPC.position, this, new Rectangle((int)greenSlime.position.X - Game1.tileSize * k, (int)greenSlime.position.Y - Game1.tileSize * k, Game1.tileSize * (k * 2 + 1), Game1.tileSize * (k * 2 + 1)));
								if (greenSlime2 != null && greenSlime2.cute != greenSlime.cute && greenSlime2.ageUntilFullGrown <= 0)
								{
									greenSlime.mateWith(greenSlime2, this);
									break;
								}
							}
						}
					}
				}
				this.slimeMatingsLeft--;
			}
			this.slimeMatingsLeft = this.characters.Count / 5 + 1;
			base.DayUpdate(dayOfMonth);
		}

		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			if (t is WateringCan && tileX == 16)
			{
				switch (tileY)
				{
				case 6:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[0] = true;
					break;
				case 7:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[1] = true;
					break;
				case 8:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[2] = true;
					break;
				case 9:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[3] = true;
					break;
				}
			}
			return false;
		}
	}
}
