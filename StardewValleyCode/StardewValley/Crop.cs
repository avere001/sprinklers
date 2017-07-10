using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class Crop
	{
		public const int mixedSeedIndex = 770;

		public const int seedPhase = 0;

		public const int grabHarvest = 0;

		public const int sickleHarvest = 1;

		public const int rowOfWildSeeds = 23;

		public const int finalPhaseLength = 99999;

		public const int forageCrop_springOnion = 1;

		public List<int> phaseDays = new List<int>();

		public int rowInSpriteSheet;

		public int phaseToShow = -1;

		public int currentPhase;

		public int harvestMethod;

		public int indexOfHarvest;

		public int regrowAfterHarvest;

		public int dayOfCurrentPhase;

		public int minHarvest;

		public int maxHarvest;

		public int maxHarvestIncreasePerFarmingLevel;

		public int daysOfUnclutteredGrowth;

		public int whichForageCrop;

		public List<string> seasonsToGrowIn = new List<string>();

		public Color tintColor;

		public bool flip;

		public bool fullyGrown;

		public bool raisedSeeds;

		public bool programColored;

		public bool dead;

		public bool forageCrop;

		public double chanceForExtraCrops;

		public Crop()
		{
		}

		public Crop(bool forageCrop, int which, int tileX, int tileY)
		{
			this.forageCrop = forageCrop;
			this.whichForageCrop = which;
			this.fullyGrown = true;
			this.currentPhase = 5;
		}

		public Crop(int seedIndex, int tileX, int tileY)
		{
			Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Crops");
			if (seedIndex == 770)
			{
				seedIndex = Crop.getRandomLowGradeCropForThisSeason(Game1.currentSeason);
				if (seedIndex == 473)
				{
					seedIndex--;
				}
			}
			if (dictionary.ContainsKey(seedIndex))
			{
				string[] array = dictionary[seedIndex].Split(new char[]
				{
					'/'
				});
				string[] array2 = array[0].Split(new char[]
				{
					' '
				});
				for (int i = 0; i < array2.Length; i++)
				{
					this.phaseDays.Add(Convert.ToInt32(array2[i]));
				}
				this.phaseDays.Add(99999);
				string[] array3 = array[1].Split(new char[]
				{
					' '
				});
				for (int j = 0; j < array3.Length; j++)
				{
					this.seasonsToGrowIn.Add(array3[j]);
				}
				this.rowInSpriteSheet = Convert.ToInt32(array[2]);
				this.indexOfHarvest = Convert.ToInt32(array[3]);
				this.regrowAfterHarvest = Convert.ToInt32(array[4]);
				this.harvestMethod = Convert.ToInt32(array[5]);
				string[] array4 = array[6].Split(new char[]
				{
					' '
				});
				if (array4.Length != 0 && array4[0].Equals("true"))
				{
					this.minHarvest = Convert.ToInt32(array4[1]);
					this.maxHarvest = Convert.ToInt32(array4[2]);
					this.maxHarvestIncreasePerFarmingLevel = Convert.ToInt32(array4[3]);
					this.chanceForExtraCrops = Convert.ToDouble(array4[4]);
				}
				this.raisedSeeds = Convert.ToBoolean(array[7]);
				string[] array5 = array[8].Split(new char[]
				{
					' '
				});
				if (array5.Length != 0 && array5[0].Equals("true"))
				{
					List<Color> list = new List<Color>();
					for (int k = 1; k < array5.Length; k += 3)
					{
						list.Add(new Color((int)Convert.ToByte(array5[k]), (int)Convert.ToByte(array5[k + 1]), (int)Convert.ToByte(array5[k + 2])));
					}
					Random random = new Random(tileX * 1000 + tileY + Game1.dayOfMonth);
					this.tintColor = list[random.Next(list.Count)];
					this.programColored = true;
				}
				this.flip = (Game1.random.NextDouble() < 0.5);
			}
			if (this.rowInSpriteSheet == 23)
			{
				this.whichForageCrop = seedIndex;
			}
		}

		public static int getRandomLowGradeCropForThisSeason(string season)
		{
			if (season.Equals("winter"))
			{
				season = ((Game1.random.NextDouble() < 0.33) ? "spring" : ((Game1.random.NextDouble() < 0.5) ? "summer" : "fall"));
			}
			if (!(season == "spring"))
			{
				if (!(season == "summer"))
				{
					if (season == "fall")
					{
						return Game1.random.Next(487, 491);
					}
				}
				else
				{
					switch (Game1.random.Next(4))
					{
					case 0:
						return 487;
					case 1:
						return 483;
					case 2:
						return 482;
					case 3:
						return 484;
					}
				}
				return -1;
			}
			return Game1.random.Next(472, 476);
		}

		public void growCompletely()
		{
			this.currentPhase = this.phaseDays.Count - 1;
			this.dayOfCurrentPhase = 0;
			if (this.regrowAfterHarvest != -1)
			{
				this.fullyGrown = true;
			}
		}

		public bool harvest(int xTile, int yTile, HoeDirt soil, JunimoHarvester junimoHarvester = null)
		{
			if (this.dead)
			{
				return junimoHarvester != null;
			}
			if (this.forageCrop)
			{
				Object @object = null;
				int howMuch = 3;
				int fertilizer = this.whichForageCrop;
				if (fertilizer == 1)
				{
					@object = new Object(399, 1, false, -1, 0);
				}
				if (Game1.player.professions.Contains(16))
				{
					@object.quality = 4;
				}
				else if (Game1.random.NextDouble() < (double)((float)Game1.player.ForagingLevel / 30f))
				{
					@object.quality = 2;
				}
				else if (Game1.random.NextDouble() < (double)((float)Game1.player.ForagingLevel / 15f))
				{
					@object.quality = 1;
				}
				Game1.stats.ItemsForaged += (uint)@object.Stack;
				if (junimoHarvester != null)
				{
					junimoHarvester.tryToAddItemToHut(@object);
					return true;
				}
				if (Game1.player.addItemToInventoryBool(@object, false))
				{
					Vector2 vector = new Vector2((float)xTile, (float)yTile);
					Game1.player.animateOnce(279 + Game1.player.facingDirection);
					Game1.player.canMove = false;
					Game1.playSound("harvest");
					DelayedAction.playSoundAfterDelay("coin", 260);
					if (this.regrowAfterHarvest == -1)
					{
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector.X * (float)Game1.tileSize, vector.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector.X * (float)Game1.tileSize, vector.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
					}
					Game1.player.gainExperience(2, howMuch);
					return true;
				}
				Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588", new object[0]));
			}
			else if (this.currentPhase >= this.phaseDays.Count - 1 && (!this.fullyGrown || this.dayOfCurrentPhase <= 0))
			{
				int num = 1;
				int num2 = 0;
				int num3 = 0;
				if (this.indexOfHarvest == 0)
				{
					return true;
				}
				Random random = new Random(xTile * 7 + yTile * 11 + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
				int fertilizer = soil.fertilizer;
				if (fertilizer != 368)
				{
					if (fertilizer == 369)
					{
						num3 = 2;
					}
				}
				else
				{
					num3 = 1;
				}
				double num4 = 0.2 * ((double)Game1.player.FarmingLevel / 10.0) + 0.2 * (double)num3 * (((double)Game1.player.FarmingLevel + 2.0) / 12.0) + 0.01;
				double num5 = Math.Min(0.75, num4 * 2.0);
				if (random.NextDouble() < num4)
				{
					num2 = 2;
				}
				else if (random.NextDouble() < num5)
				{
					num2 = 1;
				}
				if (this.minHarvest > 1 || this.maxHarvest > 1)
				{
					num = random.Next(this.minHarvest, Math.Min(this.minHarvest + 1, this.maxHarvest + 1 + Game1.player.FarmingLevel / this.maxHarvestIncreasePerFarmingLevel));
				}
				if (this.chanceForExtraCrops > 0.0)
				{
					while (random.NextDouble() < Math.Min(0.9, this.chanceForExtraCrops))
					{
						num++;
					}
				}
				if (this.harvestMethod == 1)
				{
					if (junimoHarvester == null)
					{
						DelayedAction.playSoundAfterDelay("daggerswipe", 150);
					}
					if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
					{
						Game1.playSound("harvest");
					}
					if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
					{
						DelayedAction.playSoundAfterDelay("coin", 260);
					}
					for (int i = 0; i < num; i++)
					{
						if (junimoHarvester != null)
						{
							junimoHarvester.tryToAddItemToHut(new Object(this.indexOfHarvest, 1, false, -1, num2));
						}
						else
						{
							Game1.createObjectDebris(this.indexOfHarvest, xTile, yTile, -1, num2, 1f, null);
						}
					}
					if (this.regrowAfterHarvest == -1)
					{
						return true;
					}
					this.dayOfCurrentPhase = this.regrowAfterHarvest;
					this.fullyGrown = true;
				}
				else
				{
					if (junimoHarvester == null)
					{
						Farmer arg_4C0_0 = Game1.player;
						Object arg_4C0_1;
						if (!this.programColored)
						{
							arg_4C0_1 = new Object(this.indexOfHarvest, 1, false, -1, num2);
						}
						else
						{
							(arg_4C0_1 = new ColoredObject(this.indexOfHarvest, 1, this.tintColor)).quality = num2;
						}
						if (!arg_4C0_0.addItemToInventoryBool(arg_4C0_1, false))
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588", new object[0]));
							return false;
						}
					}
					Vector2 vector2 = new Vector2((float)xTile, (float)yTile);
					if (junimoHarvester == null)
					{
						Game1.player.animateOnce(279 + Game1.player.facingDirection);
						Game1.player.canMove = false;
					}
					else
					{
						Object arg_536_1;
						if (!this.programColored)
						{
							arg_536_1 = new Object(this.indexOfHarvest, 1, false, -1, num2);
						}
						else
						{
							(arg_536_1 = new ColoredObject(this.indexOfHarvest, 1, this.tintColor)).quality = num2;
						}
						junimoHarvester.tryToAddItemToHut(arg_536_1);
					}
					if (random.NextDouble() < (double)((float)Game1.player.LuckLevel / 1500f) + Game1.dailyLuck / 1200.0 + 9.9999997473787516E-05)
					{
						num *= 2;
						if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
						{
							Game1.playSound("dwoop");
						}
					}
					else if (this.harvestMethod == 0)
					{
						if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
						{
							Game1.playSound("harvest");
						}
						if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
						{
							DelayedAction.playSoundAfterDelay("coin", 260);
						}
						if (this.regrowAfterHarvest == -1 && (junimoHarvester == null || junimoHarvester.currentLocation.Equals(Game1.currentLocation)))
						{
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
						}
					}
					if (this.indexOfHarvest == 421)
					{
						this.indexOfHarvest = 431;
						num = random.Next(1, 4);
					}
					for (int j = 0; j < num - 1; j++)
					{
						if (junimoHarvester == null)
						{
							Game1.createObjectDebris(this.indexOfHarvest, xTile, yTile, -1, 0, 1f, null);
						}
						else
						{
							junimoHarvester.tryToAddItemToHut(new Object(this.indexOfHarvest, 1, false, -1, 0));
						}
					}
					int num6 = Convert.ToInt32(Game1.objectInformation[this.indexOfHarvest].Split(new char[]
					{
						'/'
					})[1]);
					float num7 = (float)(16.0 * Math.Log(0.018 * (double)num6 + 1.0, 2.7182818284590451));
					if (junimoHarvester == null)
					{
						Game1.player.gainExperience(0, (int)Math.Round((double)num7));
					}
					if (this.regrowAfterHarvest == -1)
					{
						return true;
					}
					this.dayOfCurrentPhase = this.regrowAfterHarvest;
					this.fullyGrown = true;
				}
			}
			return false;
		}

		public int getRandomWildCropForSeason(string season)
		{
			if (season == "spring")
			{
				return 16 + Game1.random.Next(4) * 2;
			}
			if (!(season == "summer"))
			{
				if (season == "fall")
				{
					return 404 + Game1.random.Next(4) * 2;
				}
				if (!(season == "winter"))
				{
					return 22;
				}
				return 412 + Game1.random.Next(4) * 2;
			}
			else
			{
				if (Game1.random.NextDouble() < 0.33)
				{
					return 396;
				}
				if (Game1.random.NextDouble() >= 0.5)
				{
					return 402;
				}
				return 398;
			}
		}

		private Rectangle getSourceRect(int number)
		{
			if (this.dead)
			{
				return new Rectangle(192 + number % 4 * 16, 384, 16, 32);
			}
			return new Rectangle(Math.Min(240, (this.fullyGrown ? ((this.dayOfCurrentPhase <= 0) ? 6 : 7) : (((this.phaseToShow != -1) ? this.phaseToShow : this.currentPhase) + ((((this.phaseToShow != -1) ? this.phaseToShow : this.currentPhase) == 0 && number % 2 == 0) ? -1 : 0) + 1)) * 16 + ((this.rowInSpriteSheet % 2 != 0) ? 128 : 0)), this.rowInSpriteSheet / 2 * 16 * 2, 16, 32);
		}

		public void newDay(int state, int fertilizer, int xTile, int yTile, GameLocation environment)
		{
			if (!environment.name.Equals("Greenhouse") && (this.dead || !this.seasonsToGrowIn.Contains(Game1.currentSeason)))
			{
				this.dead = true;
				return;
			}
			if (state == 1)
			{
				if (!this.fullyGrown)
				{
					this.dayOfCurrentPhase = Math.Min(this.dayOfCurrentPhase + 1, (this.phaseDays.Count > 0) ? this.phaseDays[Math.Min(this.phaseDays.Count - 1, this.currentPhase)] : 0);
				}
				else
				{
					this.dayOfCurrentPhase--;
				}
				if (this.dayOfCurrentPhase >= ((this.phaseDays.Count > 0) ? this.phaseDays[Math.Min(this.phaseDays.Count - 1, this.currentPhase)] : 0) && this.currentPhase < this.phaseDays.Count - 1)
				{
					this.currentPhase++;
					this.dayOfCurrentPhase = 0;
				}
				while (this.currentPhase < this.phaseDays.Count - 1 && this.phaseDays.Count > 0 && this.phaseDays[this.currentPhase] <= 0)
				{
					this.currentPhase++;
				}
				if (this.rowInSpriteSheet == 23 && this.phaseToShow == -1 && this.currentPhase > 0)
				{
					this.phaseToShow = Game1.random.Next(1, 7);
				}
				if (environment is Farm && this.currentPhase == this.phaseDays.Count - 1 && (this.indexOfHarvest == 276 || this.indexOfHarvest == 190 || this.indexOfHarvest == 254) && new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + xTile * 2000 + yTile).NextDouble() < 0.01)
				{
					for (int i = xTile - 1; i <= xTile + 1; i++)
					{
						for (int j = yTile - 1; j <= yTile + 1; j++)
						{
							Vector2 key = new Vector2((float)i, (float)j);
							if (!environment.terrainFeatures.ContainsKey(key) || !(environment.terrainFeatures[key] is HoeDirt) || (environment.terrainFeatures[key] as HoeDirt).crop == null || (environment.terrainFeatures[key] as HoeDirt).crop.indexOfHarvest != this.indexOfHarvest)
							{
								return;
							}
						}
					}
					for (int k = xTile - 1; k <= xTile + 1; k++)
					{
						for (int l = yTile - 1; l <= yTile + 1; l++)
						{
							Vector2 key2 = new Vector2((float)k, (float)l);
							(environment.terrainFeatures[key2] as HoeDirt).crop = null;
						}
					}
					(environment as Farm).resourceClumps.Add(new GiantCrop(this.indexOfHarvest, new Vector2((float)(xTile - 1), (float)(yTile - 1))));
				}
			}
			if ((!this.fullyGrown || this.dayOfCurrentPhase <= 0) && this.currentPhase >= this.phaseDays.Count - 1 && this.rowInSpriteSheet == 23)
			{
				Vector2 vector = new Vector2((float)xTile, (float)yTile);
				environment.objects.Remove(vector);
				string season = Game1.currentSeason;
				switch (this.whichForageCrop)
				{
				case 495:
					season = "spring";
					break;
				case 496:
					season = "summer";
					break;
				case 497:
					season = "fall";
					break;
				case 498:
					season = "winter";
					break;
				}
				environment.objects.Add(vector, new Object(vector, this.getRandomWildCropForSeason(season), 1)
				{
					isSpawnedObject = true,
					canBeGrabbed = true
				});
				if (environment.terrainFeatures[vector] != null && environment.terrainFeatures[vector] is HoeDirt)
				{
					(environment.terrainFeatures[vector] as HoeDirt).crop = null;
				}
			}
		}

		public bool isWildSeedCrop()
		{
			return this.rowInSpriteSheet == 23;
		}

		public void draw(SpriteBatch b, Vector2 tileLocation, Color toTint, float rotation)
		{
			if (this.forageCrop)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((tileLocation.X * 11f + tileLocation.Y * 7f) % 10f - 5f) + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)(tileLocation.X * 51f + tileLocation.Y * 77f) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) / 10000f);
				return;
			}
			b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.X * 11f + tileLocation.Y * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2))), new Rectangle?(this.getSourceRect((int)tileLocation.X * 7 + (int)tileLocation.Y * 11)), toTint, rotation, new Vector2(8f, 24f), (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f))) / 10000f / ((this.currentPhase == 0 && !this.raisedSeeds) ? 2f : 1f));
			if (!this.tintColor.Equals(Color.White) && this.currentPhase == this.phaseDays.Count - 1 && !this.dead)
			{
				b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.X * 11f + tileLocation.Y * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((this.fullyGrown ? ((this.dayOfCurrentPhase <= 0) ? 6 : 7) : (this.currentPhase + 1 + 1)) * 16 + ((this.rowInSpriteSheet % 2 != 0) ? 128 : 0), this.rowInSpriteSheet / 2 * 16 * 2, 16, 32)), this.tintColor, rotation, new Vector2(8f, 24f), (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) / 10000f / (float)((this.currentPhase == 0 && !this.raisedSeeds) ? 2 : 1));
			}
		}

		public void drawInMenu(SpriteBatch b, Vector2 screenPosition, Color toTint, float rotation, float scale, float layerDepth)
		{
			b.Draw(Game1.cropSpriteSheet, screenPosition, new Rectangle?(this.getSourceRect(0)), toTint, rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + Game1.tileSize / 2)), scale, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
		}
	}
}
