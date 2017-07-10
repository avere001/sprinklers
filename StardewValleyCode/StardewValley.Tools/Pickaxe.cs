using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Tools
{
	public class Pickaxe : Tool
	{
		public const int hitMargin = 8;

		public const int BoulderStrength = 4;

		private int boulderTileX;

		private int boulderTileY;

		private int hitsToBoulder;

		public Pickaxe() : base("Pickaxe", 0, 105, 131, false, 0)
		{
			this.upgradeLevel = 0;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pickaxe.cs.14184", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pickaxe.cs.14185", new object[0]);
		}

		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			base.Update(who.facingDirection, 0, who);
			if (who.IsMainPlayer)
			{
				Game1.releaseUseToolButton();
				return true;
			}
			switch (who.FacingDirection)
			{
			case 0:
				who.FarmerSprite.setCurrentFrame(176);
				who.CurrentTool.Update(0, 0);
				break;
			case 1:
				who.FarmerSprite.setCurrentFrame(168);
				who.CurrentTool.Update(1, 0);
				break;
			case 2:
				who.FarmerSprite.setCurrentFrame(160);
				who.CurrentTool.Update(2, 0);
				break;
			case 3:
				who.FarmerSprite.setCurrentFrame(184);
				who.CurrentTool.Update(3, 0);
				break;
			}
			return true;
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			power = who.toolPower;
			who.Stamina -= (float)(2 * (power + 1)) - (float)who.MiningLevel * 0.1f;
			Utility.clampToTile(new Vector2((float)x, (float)y));
			int num = x / Game1.tileSize;
			int num2 = y / Game1.tileSize;
			Vector2 vector = new Vector2((float)num, (float)num2);
			if (location.performToolAction(this, num, num2))
			{
				return;
			}
			StardewValley.Object @object = null;
			location.Objects.TryGetValue(vector, out @object);
			if (@object == null)
			{
				if (who.FacingDirection == 0 || who.FacingDirection == 2)
				{
					num = (x - 8) / Game1.tileSize;
					location.Objects.TryGetValue(new Vector2((float)num, (float)num2), out @object);
					if (@object == null)
					{
						num = (x + 8) / Game1.tileSize;
						location.Objects.TryGetValue(new Vector2((float)num, (float)num2), out @object);
					}
				}
				else
				{
					num2 = (y + 8) / Game1.tileSize;
					location.Objects.TryGetValue(new Vector2((float)num, (float)num2), out @object);
					if (@object == null)
					{
						num2 = (y - 8) / Game1.tileSize;
						location.Objects.TryGetValue(new Vector2((float)num, (float)num2), out @object);
					}
				}
				x = num * Game1.tileSize;
				y = num2 * Game1.tileSize;
				if (location.terrainFeatures.ContainsKey(vector) && location.terrainFeatures[vector].performToolAction(this, 0, vector, null))
				{
					location.terrainFeatures.Remove(vector);
				}
			}
			vector = new Vector2((float)num, (float)num2);
			if (@object != null)
			{
				if (@object.Name.Equals("Stone"))
				{
					Game1.playSound("hammer");
					if (@object.minutesUntilReady > 0)
					{
						int num3 = Math.Max(1, this.upgradeLevel + 1);
						@object.minutesUntilReady -= num3;
						@object.shakeTimer = 200;
						if (@object.minutesUntilReady > 0)
						{
							Game1.createRadialDebris(Game1.currentLocation, 14, num, num2, Game1.random.Next(2, 5), false, -1, false, -1);
							return;
						}
					}
					if (@object.ParentSheetIndex < 200 && !Game1.objectInformation.ContainsKey(@object.ParentSheetIndex + 1))
					{
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(@object.ParentSheetIndex + 1, 300f, 1, 2, new Vector2((float)(x - x % Game1.tileSize), (float)(y - y % Game1.tileSize)), true, @object.flipped)
						{
							alphaFade = 0.01f
						});
					}
					else
					{
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(47, new Vector2((float)(num * Game1.tileSize), (float)(num2 * Game1.tileSize)), Color.Gray, 10, false, 80f, 0, -1, -1f, -1, 0));
					}
					Game1.createRadialDebris(location, 14, num, num2, Game1.random.Next(2, 5), false, -1, false, -1);
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2((float)(num * Game1.tileSize), (float)(num2 * Game1.tileSize)), Color.White, 10, false, 80f, 0, -1, -1f, -1, 0)
					{
						motion = new Vector2(0f, -0.6f),
						acceleration = new Vector2(0f, 0.002f),
						alphaFade = 0.015f
					});
					if (!location.Name.Equals("UndergroundMine"))
					{
						if (@object.parentSheetIndex == 343 || @object.parentSheetIndex == 450)
						{
							Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)(num * 2000) + (uint)num2));
							if (random.NextDouble() < 0.035 && Game1.stats.DaysPlayed > 1u)
							{
								Game1.createObjectDebris(535 + ((Game1.stats.DaysPlayed > 60u && random.NextDouble() < 0.2) ? 1 : ((Game1.stats.DaysPlayed > 120u && random.NextDouble() < 0.2) ? 2 : 0)), num, num2, base.getLastFarmerToUse().uniqueMultiplayerID);
							}
							if (random.NextDouble() < 0.035 * (double)(who.professions.Contains(21) ? 2 : 1) && Game1.stats.DaysPlayed > 1u)
							{
								Game1.createObjectDebris(382, num, num2, base.getLastFarmerToUse().uniqueMultiplayerID);
							}
							if (random.NextDouble() < 0.01 && Game1.stats.DaysPlayed > 1u)
							{
								Game1.createObjectDebris(390, num, num2, base.getLastFarmerToUse().uniqueMultiplayerID);
							}
						}
						location.breakStone(@object.parentSheetIndex, num, num2, who, new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)(num * 4000) + (uint)num2)));
					}
					else
					{
						Game1.mine.checkStoneForItems(@object.ParentSheetIndex, num, num2, who);
					}
					if (@object.minutesUntilReady <= 0)
					{
						location.Objects.Remove(new Vector2((float)num, (float)num2));
						Game1.playSound("stoneCrack");
						Stats expr_4F8 = Game1.stats;
						uint num4 = expr_4F8.RocksCrushed;
						expr_4F8.RocksCrushed = num4 + 1u;
					}
					return;
				}
				if (@object.Name.Contains("Boulder"))
				{
					Game1.playSound("hammer");
					if (this.UpgradeLevel < 2)
					{
						Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Pickaxe.cs.14194", new object[0])));
						return;
					}
					if (num == this.boulderTileX && num2 == this.boulderTileY)
					{
						this.hitsToBoulder += power + 1;
						@object.shakeTimer = 190;
					}
					else
					{
						this.hitsToBoulder = 0;
						this.boulderTileX = num;
						this.boulderTileY = num2;
					}
					if (this.hitsToBoulder >= 4)
					{
						location.removeObject(vector, false);
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * vector.X - (float)(Game1.tileSize / 2), (float)Game1.tileSize * (vector.Y - 1f)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 0
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * vector.X + (float)(Game1.tileSize / 2), (float)Game1.tileSize * (vector.Y - 1f)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 200
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * vector.X, (float)Game1.tileSize * (vector.Y - 1f) - (float)(Game1.tileSize / 2)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 400
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * vector.X, (float)Game1.tileSize * vector.Y - (float)(Game1.tileSize / 2)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 600
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)Game1.tileSize * vector.X, (float)Game1.tileSize * vector.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0));
						location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)Game1.tileSize * vector.X + (float)(Game1.tileSize / 2), (float)Game1.tileSize * vector.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0)
						{
							delayBeforeAnimationStart = 250
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)Game1.tileSize * vector.X - (float)(Game1.tileSize / 2), (float)Game1.tileSize * vector.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0)
						{
							delayBeforeAnimationStart = 500
						});
						Game1.playSound("boulderBreak");
						Stats expr_8C5 = Game1.stats;
						uint num4 = expr_8C5.BouldersCracked;
						expr_8C5.BouldersCracked = num4 + 1u;
						return;
					}
				}
				else if (@object.performToolAction(this))
				{
					@object.performRemoveAction(vector, location);
					if (@object.type.Equals("Crafting") && @object.fragility != 2)
					{
						Game1.currentLocation.debris.Add(new Debris(@object.bigCraftable ? (-@object.ParentSheetIndex) : @object.ParentSheetIndex, who.GetToolLocation(false), new Vector2((float)who.GetBoundingBox().Center.X, (float)who.GetBoundingBox().Center.Y)));
					}
					Game1.currentLocation.Objects.Remove(vector);
					return;
				}
			}
			else
			{
				Game1.playSound("woodyHit");
				if (location.doesTileHaveProperty(num, num2, "Diggable", "Back") != null)
				{
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2((float)(num * Game1.tileSize), (float)(num2 * Game1.tileSize)), Color.White, 8, false, 80f, 0, -1, -1f, -1, 0)
					{
						alphaFade = 0.015f
					});
				}
			}
		}
	}
}
