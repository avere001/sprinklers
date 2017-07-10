using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;

namespace StardewValley.TerrainFeatures
{
	public class Tree : TerrainFeature
	{
		public const float chanceForDailySeed = 0.05f;

		public const float shakeRate = 0.0157079641f;

		public const float shakeDecayRate = 0.00306796166f;

		public const int minWoodDebrisForFallenTree = 12;

		public const int minWoodDebrisForStump = 5;

		public const int startingHealth = 10;

		public const int leafFallRate = 3;

		public const int bushyTree = 1;

		public const int leafyTree = 2;

		public const int pineTree = 3;

		public const int winterTree1 = 4;

		public const int winterTree2 = 5;

		public const int palmTree = 6;

		public const int mushroomTree = 7;

		public const int seedStage = 0;

		public const int sproutStage = 1;

		public const int saplingStage = 2;

		public const int bushStage = 3;

		public const int treeStage = 5;

		private Texture2D texture;

		public int growthStage;

		public int treeType;

		public float health;

		public bool flipped;

		public bool stump;

		public bool tapped;

		public bool hasSeed;

		private bool shakeLeft;

		private bool falling;

		private bool destroy;

		private float shakeRotation;

		private float maxShake;

		private float alpha = 1f;

		private List<Leaf> leaves = new List<Leaf>();

		private long lastPlayerToHit;

		private float shakeTimer;

		public static Microsoft.Xna.Framework.Rectangle treeTopSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96);

		public static Microsoft.Xna.Framework.Rectangle stumpSourceRect = new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32);

		public static Microsoft.Xna.Framework.Rectangle shadowSourceRect = new Microsoft.Xna.Framework.Rectangle(663, 1011, 41, 30);

		public Tree()
		{
		}

		public Tree(int which, int growthStage)
		{
			this.growthStage = growthStage;
			this.treeType = which;
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.health = 10f;
		}

		public Tree(int which)
		{
			this.treeType = which;
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.health = 10f;
		}

		public override void loadSprite()
		{
			try
			{
				if (this.treeType == 7)
				{
					this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\mushroom_tree");
				}
				else if (this.treeType == 6)
				{
					this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\tree_palm");
				}
				else
				{
					if (this.treeType == 4)
					{
						this.treeType = 1;
					}
					if (this.treeType == 5)
					{
						this.treeType = 2;
					}
					string text = Game1.currentSeason;
					if (this.treeType == 3 && text.Equals("summer"))
					{
						text = "spring";
					}
					this.texture = Game1.content.Load<Texture2D>(string.Concat(new object[]
					{
						"TerrainFeatures\\tree",
						Math.Max(1, this.treeType),
						"_",
						text
					}));
				}
			}
			catch (Exception)
			{
			}
		}

		public override Microsoft.Xna.Framework.Rectangle getBoundingBox(Vector2 tileLocation)
		{
			switch (this.growthStage)
			{
			case 0:
			case 1:
			case 2:
				return new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize + Game1.tileSize / 5, (int)tileLocation.Y * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 3 / 5, Game1.tileSize * 3 / 4);
			}
			return new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public override bool performUseAction(Vector2 tileLocation)
		{
			if (!this.tapped)
			{
				if (this.maxShake == 0f && !this.stump && this.growthStage >= 3 && (!Game1.currentSeason.Equals("winter") || this.treeType == 3))
				{
					Game1.playSound("leafrustle");
				}
				this.shake(tileLocation, false);
			}
			return false;
		}

		private int extraWoodCalculator(Vector2 tileLocation)
		{
			Random arg_2D_0 = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
			int num = 0;
			if (arg_2D_0.NextDouble() < Game1.dailyLuck)
			{
				num++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.ForagingLevel / 12.5)
			{
				num++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.ForagingLevel / 12.5)
			{
				num++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.LuckLevel / 25.0)
			{
				num++;
			}
			return num;
		}

		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.shakeTimer > 0f)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			}
			if (this.destroy)
			{
				return true;
			}
			this.alpha = Math.Min(1f, this.alpha + 0.05f);
			if (this.growthStage >= 5 && !this.falling && !this.stump && Game1.player.GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(Game1.tileSize * ((int)tileLocation.X - 1), Game1.tileSize * ((int)tileLocation.Y - 5), 3 * Game1.tileSize, 4 * Game1.tileSize + Game1.tileSize / 2)))
			{
				this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
			}
			if (!this.falling)
			{
				if ((double)Math.Abs(this.shakeRotation) > 1.5707963267948966 && this.leaves.Count <= 0 && this.health <= 0f)
				{
					return true;
				}
				if (this.maxShake > 0f)
				{
					if (this.shakeLeft)
					{
						this.shakeRotation -= ((this.growthStage >= 5) ? 0.005235988f : 0.0157079641f);
						if (this.shakeRotation <= -this.maxShake)
						{
							this.shakeLeft = false;
						}
					}
					else
					{
						this.shakeRotation += ((this.growthStage >= 5) ? 0.005235988f : 0.0157079641f);
						if (this.shakeRotation >= this.maxShake)
						{
							this.shakeLeft = true;
						}
					}
				}
				if (this.maxShake > 0f)
				{
					this.maxShake = Math.Max(0f, this.maxShake - ((this.growthStage >= 5) ? 0.00102265389f : 0.00306796166f));
				}
			}
			else
			{
				this.shakeRotation += (this.shakeLeft ? (-(this.maxShake * this.maxShake)) : (this.maxShake * this.maxShake));
				this.maxShake += 0.00153398083f;
				if (Game1.random.NextDouble() < 0.01 && this.treeType != 7)
				{
					Game1.playSound("leafrustle");
				}
				if ((double)Math.Abs(this.shakeRotation) > 1.5707963267948966)
				{
					this.falling = false;
					this.maxShake = 0f;
					Game1.playSound("treethud");
					int num = Game1.random.Next(90, 120);
					if (Game1.currentLocation.Objects.ContainsKey(tileLocation))
					{
						Game1.currentLocation.Objects.Remove(tileLocation);
					}
					for (int i = 0; i < num; i++)
					{
						this.leaves.Add(new Leaf(new Vector2((float)(Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3))) + (this.shakeLeft ? (-Game1.tileSize * 5) : (Game1.tileSize * 4))), tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(10, 40) / 10f));
					}
					if (this.treeType != 7)
					{
						Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 12 + this.extraWoodCalculator(tileLocation), true, -1, false, -1);
						Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 12 + this.extraWoodCalculator(tileLocation), false, -1, false, -1);
						Random random;
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
							random = Game1.recentMultiplayerRandom;
						}
						else
						{
							random = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(92, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 5, this.lastPlayerToHit);
							int num2 = 0;
							if (Game1.getFarmer(this.lastPlayerToHit) != null)
							{
								while (Game1.getFarmer(this.lastPlayerToHit).professions.Contains(14) && random.NextDouble() < 0.4)
								{
									num2++;
								}
							}
							if (num2 > 0)
							{
								Game1.createMultipleObjectDebris(709, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, num2, this.lastPlayerToHit);
							}
							if (Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1 && random.NextDouble() < 0.75)
							{
								Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, random.Next(1, 3), this.lastPlayerToHit);
							}
						}
						else
						{
							Game1.createMultipleObjectDebris(92, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 5);
							int num3 = 0;
							if (Game1.getFarmer(this.lastPlayerToHit) != null)
							{
								while (Game1.getFarmer(this.lastPlayerToHit).professions.Contains(14) && random.NextDouble() < 0.4)
								{
									num3++;
								}
							}
							if (num3 > 0)
							{
								Game1.createMultipleObjectDebris(709, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, num3);
							}
							if (this.lastPlayerToHit != 0L && Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1 && random.NextDouble() < 0.75 && this.treeType < 4)
							{
								Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, random.Next(1, 3));
							}
						}
					}
					else if (!Game1.IsMultiplayer)
					{
						Game1.createMultipleObjectDebris(420, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 5);
					}
					if (this.health == -100f)
					{
						return true;
					}
					if (this.health <= 0f)
					{
						this.health = -100f;
					}
				}
			}
			for (int j = this.leaves.Count - 1; j >= 0; j--)
			{
				Leaf expr_6D9_cp_0_cp_0 = this.leaves.ElementAt(j);
				expr_6D9_cp_0_cp_0.position.Y = expr_6D9_cp_0_cp_0.position.Y - (this.leaves.ElementAt(j).yVelocity - 3f);
				this.leaves.ElementAt(j).yVelocity = Math.Max(0f, this.leaves.ElementAt(j).yVelocity - 0.01f);
				this.leaves.ElementAt(j).rotation += this.leaves.ElementAt(j).rotationRate;
				if (this.leaves.ElementAt(j).position.Y >= tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)
				{
					this.leaves.RemoveAt(j);
				}
			}
			return false;
		}

		private void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
		{
			if ((this.maxShake == 0f | doEvenIfStillShaking) && this.growthStage >= 3 && !this.stump)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = (float)((this.growthStage >= 5) ? 0.024543692606170259 : 0.049087385212340517);
				if (this.growthStage >= 5)
				{
					if (Game1.random.NextDouble() < 0.66)
					{
						int num = Game1.random.Next(1, 6);
						for (int i = 0; i < num; i++)
						{
							this.leaves.Add(new Leaf(new Vector2((float)Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize - (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 2))), (float)Game1.random.Next((int)(tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 4)), (int)(tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3)))), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(5) / 10f));
						}
					}
					if (Game1.random.NextDouble() < 0.01)
					{
						if (!Game1.currentSeason.Equals("spring"))
						{
							if (!Game1.currentSeason.Equals("summer"))
							{
								goto IL_229;
							}
						}
						while (Game1.random.NextDouble() < 0.8)
						{
							Game1.currentLocation.addCritter(new Butterfly(new Vector2(tileLocation.X + (float)Game1.random.Next(1, 3), tileLocation.Y - 2f + (float)Game1.random.Next(-1, 2))));
						}
					}
					IL_229:
					if (this.hasSeed && (Game1.IsMultiplayer || Game1.player.ForagingLevel >= 1))
					{
						int num2 = -1;
						switch (this.treeType)
						{
						case 1:
							num2 = 309;
							break;
						case 2:
							num2 = 310;
							break;
						case 3:
							num2 = 311;
							break;
						case 6:
							num2 = 88;
							break;
						}
						if (Game1.currentSeason.Equals("fall") && this.treeType == 2 && Game1.dayOfMonth >= 14)
						{
							num2 = 408;
						}
						if (num2 != -1)
						{
							Game1.createObjectDebris(num2, (int)tileLocation.X, (int)tileLocation.Y - 3, ((int)tileLocation.Y + 1) * Game1.tileSize, 0, 1f, null);
						}
						this.hasSeed = false;
						return;
					}
				}
				else if (Game1.random.NextDouble() < 0.66)
				{
					int num3 = Game1.random.Next(1, 3);
					for (int j = 0; j < num3; j++)
					{
						this.leaves.Add(new Leaf(new Vector2((float)Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4))), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2)), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(30) / 10f));
					}
					return;
				}
			}
			else if (this.stump)
			{
				this.shakeTimer = 100f;
			}
		}

		public override bool isPassable(Character c = null)
		{
			return this.health <= -99f || this.growthStage == 0;
		}

		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
			Microsoft.Xna.Framework.Rectangle value = new Microsoft.Xna.Framework.Rectangle((int)((tileLocation.X - 1f) * (float)Game1.tileSize), (int)((tileLocation.Y - 1f) * (float)Game1.tileSize), Game1.tileSize * 3, Game1.tileSize * 3);
			if (this.health <= -100f)
			{
				this.destroy = true;
			}
			if (!Game1.currentSeason.Equals("winter") || this.treeType == 6 || environment.Name.ToLower().Contains("greenhouse"))
			{
				string text = environment.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "NoSpawn", "Back");
				if (text != null && (text.Equals("All") || text.Equals("Tree") || text.Equals("True")))
				{
					return;
				}
				if (this.growthStage == 4)
				{
					using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = environment.terrainFeatures.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Vector2, TerrainFeature> current = enumerator.Current;
							if (current.Value is Tree && !current.Value.Equals(this) && ((Tree)current.Value).growthStage >= 5 && current.Value.getBoundingBox(current.Key).Intersects(value))
							{
								return;
							}
						}
						goto IL_176;
					}
				}
				if (this.growthStage == 0 && environment.objects.ContainsKey(tileLocation))
				{
					return;
				}
				IL_176:
				if (Game1.random.NextDouble() < 0.2)
				{
					this.growthStage++;
				}
			}
			if (Game1.currentSeason.Equals("winter") && this.treeType == 7)
			{
				this.stump = true;
			}
			else if (this.treeType == 7 && Game1.dayOfMonth <= 1 && Game1.currentSeason.Equals("spring"))
			{
				this.stump = false;
				this.health = 10f;
			}
			if (this.growthStage >= 5 && environment is Farm && Game1.random.NextDouble() < 0.15)
			{
				int num = Game1.random.Next(-3, 4) + (int)tileLocation.X;
				int num2 = Game1.random.Next(-3, 4) + (int)tileLocation.Y;
				Vector2 vector = new Vector2((float)num, (float)num2);
				string text2 = environment.doesTileHaveProperty(num, num2, "NoSpawn", "Back");
				if ((text2 == null || (!text2.Equals("Tree") && !text2.Equals("All") && !text2.Equals("True"))) && environment.isTileLocationOpen(new Location(num * Game1.tileSize, num2 * Game1.tileSize)) && !environment.isTileOccupied(vector, "") && environment.doesTileHaveProperty(num, num2, "Water", "Back") == null && environment.isTileOnMap(vector))
				{
					environment.terrainFeatures.Add(vector, new Tree(this.treeType, 0));
				}
			}
			this.hasSeed = false;
			if (this.growthStage >= 5 && Game1.random.NextDouble() < 0.05000000074505806)
			{
				this.hasSeed = true;
			}
		}

		public override bool seasonUpdate(bool onLoad)
		{
			this.loadSprite();
			return false;
		}

		public override bool isActionable()
		{
			return !this.tapped && this.growthStage >= 3;
		}

		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			if (explosion > 0)
			{
				this.tapped = false;
			}
			if (this.tapped)
			{
				return false;
			}
			Console.WriteLine(string.Concat(new object[]
			{
				"TREE: IsClient:",
				Game1.IsClient.ToString(),
				" randomOutput: ",
				Game1.recentMultiplayerRandom.Next(9999)
			}));
			if (this.health <= -99f)
			{
				return false;
			}
			if (this.growthStage >= 5)
			{
				if (t != null && t is Axe)
				{
					Game1.playSound("axchop");
					location.debris.Add(new Debris(12, Game1.random.Next(1, 3), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f), t.getLastFarmerToUse().position, 0, -1));
					this.lastPlayerToHit = t.getLastFarmerToUse().uniqueMultiplayerID;
				}
				else if (explosion <= 0)
				{
					return false;
				}
				this.shake(tileLocation, true);
				float num = 1f;
				if (explosion > 0)
				{
					num = (float)explosion;
				}
				else
				{
					if (t == null)
					{
						return false;
					}
					switch (t.upgradeLevel)
					{
					case 0:
						num = 1f;
						break;
					case 1:
						num = 1.25f;
						break;
					case 2:
						num = 1.67f;
						break;
					case 3:
						num = 2.5f;
						break;
					case 4:
						num = 5f;
						break;
					}
				}
				this.health -= num;
				if (this.health <= 0f)
				{
					if (!this.stump)
					{
						if ((t != null || explosion > 0) && location.Equals(Game1.currentLocation))
						{
							Game1.playSound("treecrack");
						}
						this.stump = true;
						this.health = 5f;
						this.falling = true;
						if (t != null)
						{
							t.getLastFarmerToUse().gainExperience(2, 12);
						}
						if (t == null || t.getLastFarmerToUse() == null)
						{
							this.shakeLeft = true;
						}
						else
						{
							this.shakeLeft = (t.getLastFarmerToUse().getTileLocation().X > tileLocation.X || (t.getLastFarmerToUse().getTileLocation().Y < tileLocation.Y && tileLocation.X % 2f == 0f));
						}
					}
					else
					{
						this.health = -100f;
						Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(30, 40), false, -1, false, -1);
						int index = (this.treeType == 7 && tileLocation.X % 7f == 0f) ? 422 : ((this.treeType == 7) ? 420 : 92);
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 2000 + (int)tileLocation.Y);
							Random arg_2D6_0 = Game1.recentMultiplayerRandom;
						}
						else
						{
							new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (t == null || t.getLastFarmerToUse() == null)
						{
							if (location.Equals(Game1.currentLocation))
							{
								Game1.createMultipleObjectDebris(92, (int)tileLocation.X, (int)tileLocation.Y, 2);
							}
							else
							{
								Game1.createItemDebris(new StardewValley.Object(92, 1, false, -1, 0), tileLocation * (float)Game1.tileSize, 2, location);
								Game1.createItemDebris(new StardewValley.Object(92, 1, false, -1, 0), tileLocation * (float)Game1.tileSize, 2, location);
							}
						}
						else if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(index, (int)tileLocation.X, (int)tileLocation.Y, 1, this.lastPlayerToHit);
							if (this.treeType != 7)
							{
								Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, 4, true, -1, false, -1);
							}
						}
						else
						{
							if (this.treeType != 7)
							{
								Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, 5 + this.extraWoodCalculator(tileLocation), true, -1, false, -1);
							}
							Game1.createMultipleObjectDebris(index, (int)tileLocation.X, (int)tileLocation.Y, 1);
						}
						if (location.Equals(Game1.currentLocation))
						{
							Game1.playSound("treethud");
						}
						if (!this.falling)
						{
							return true;
						}
					}
				}
			}
			else if (this.growthStage >= 3)
			{
				if (t != null && t.name.Contains("Ax"))
				{
					Game1.playSound("axchop");
					if (this.treeType != 7)
					{
						Game1.playSound("leafrustle");
					}
					location.debris.Add(new Debris(12, Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f), new Vector2((float)t.getLastFarmerToUse().GetBoundingBox().Center.X, (float)t.getLastFarmerToUse().GetBoundingBox().Center.Y), 0, -1));
				}
				else if (explosion <= 0)
				{
					return false;
				}
				this.shake(tileLocation, true);
				float num2 = 1f;
				if (Game1.IsMultiplayer)
				{
					Random arg_51E_0 = Game1.recentMultiplayerRandom;
				}
				else
				{
					new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + Game1.stats.DaysPlayed + this.health));
				}
				if (explosion > 0)
				{
					num2 = (float)explosion;
				}
				else
				{
					switch (t.upgradeLevel)
					{
					case 0:
						num2 = 2f;
						break;
					case 1:
						num2 = 2.5f;
						break;
					case 2:
						num2 = 3.34f;
						break;
					case 3:
						num2 = 5f;
						break;
					case 4:
						num2 = 10f;
						break;
					}
				}
				this.health -= num2;
				if (this.health <= 0f)
				{
					Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, 4, null);
					Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(20, 30), false, -1, false, -1);
					return true;
				}
			}
			else if (this.growthStage >= 1)
			{
				if (explosion > 0)
				{
					return true;
				}
				if (location.Equals(Game1.currentLocation))
				{
					Game1.playSound("cut");
				}
				if (t != null && t.name.Contains("Axe"))
				{
					Game1.playSound("axchop");
					Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
				}
				if (t is Axe || t is Pickaxe || t is Hoe || t is MeleeWeapon)
				{
					Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
					if (t.name.Contains("Axe") && Game1.recentMultiplayerRandom.NextDouble() < (double)((float)t.getLastFarmerToUse().ForagingLevel / 10f))
					{
						Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, 1, null);
					}
					location.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					return true;
				}
			}
			else
			{
				if (explosion > 0)
				{
					return true;
				}
				if (t.name.Contains("Axe") || t.name.Contains("Pick") || t.name.Contains("Hoe"))
				{
					Game1.playSound("woodyHit");
					Game1.playSound("axchop");
					location.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					if (this.lastPlayerToHit != 0L && Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1)
					{
						Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X, (int)tileLocation.Y, 1, t.getLastFarmerToUse().uniqueMultiplayerID, location);
					}
					else if (!Game1.IsMultiplayer && Game1.player.getEffectiveSkillLevel(2) >= 1)
					{
						Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X, (int)tileLocation.Y, 1, t.getLastFarmerToUse().uniqueMultiplayerID, location);
					}
					return true;
				}
			}
			return false;
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			layerDepth += positionOnScreen.X / 100000f;
			if (this.growthStage < 5)
			{
				Microsoft.Xna.Framework.Rectangle empty = Microsoft.Xna.Framework.Rectangle.Empty;
				switch (this.growthStage)
				{
				case 0:
					empty = new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16);
					break;
				case 1:
					empty = new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16);
					break;
				case 2:
					empty = new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16);
					break;
				default:
					empty = new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32);
					break;
				}
				spriteBatch.Draw(this.texture, positionOnScreen - new Vector2(0f, (float)empty.Height * scale), new Microsoft.Xna.Framework.Rectangle?(empty), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)empty.Height * scale) / 20000f);
				return;
			}
			if (!this.falling)
			{
				spriteBatch.Draw(this.texture, positionOnScreen + new Vector2(0f, (float)(-(float)Game1.tileSize) * scale), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32)), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale - 1f) / 20000f);
			}
			if (!this.stump || this.falling)
			{
				spriteBatch.Draw(this.texture, positionOnScreen + new Vector2((float)(-(float)Game1.tileSize) * scale, (float)(-5 * Game1.tileSize) * scale), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96)), Color.White, this.shakeRotation, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale) / 20000f);
			}
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.growthStage < 5)
			{
				Microsoft.Xna.Framework.Rectangle empty = Microsoft.Xna.Framework.Rectangle.Empty;
				switch (this.growthStage)
				{
				case 0:
					empty = new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16);
					break;
				case 1:
					empty = new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16);
					break;
				case 2:
					empty = new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16);
					break;
				default:
					empty = new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32);
					break;
				}
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize - (float)(empty.Height * Game1.pixelZoom - Game1.tileSize) + (float)((this.growthStage >= 3) ? (Game1.tileSize * 2) : Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(empty), Color.White, this.shakeRotation, new Vector2(8f, (float)((this.growthStage >= 3) ? 32 : 16)), (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.growthStage == 0) ? 0.0001f : ((float)this.getBoundingBox(tileLocation).Bottom / 10000f));
			}
			else
			{
				if (!this.stump || this.falling)
				{
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize - (float)(Game1.tileSize * 4 / 5), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 4))), new Microsoft.Xna.Framework.Rectangle?(Tree.shadowSourceRect), Color.White * (1.57079637f - Math.Abs(this.shakeRotation)), 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-06f);
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Tree.treeTopSourceRect), Color.White * this.alpha, this.shakeRotation, new Vector2(24f, 96f), (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.getBoundingBox(tileLocation).Bottom + 2) / 10000f - tileLocation.X / 1000000f);
				}
				if (this.health >= 1f || (!this.falling && this.health > -99f))
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.shakeTimer > 0f) ? ((float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 3f) : 0f), tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Tree.stumpSourceRect), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f);
				}
				if (this.stump && this.health < 4f && this.health > -99f)
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.shakeTimer > 0f) ? ((float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 3f) : 0f), tileLocation.Y * (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(Math.Min(2, (int)(3f - this.health)) * 16, 144, 16, 16)), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.getBoundingBox(tileLocation).Bottom + 1) / 10000f);
				}
			}
			foreach (Leaf current in this.leaves)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, current.position), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(16 + current.type % 2 * 8, 112 + current.type / 2 * 8, 8, 8)), Color.White, current.rotation, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 0.01f);
			}
		}
	}
}
