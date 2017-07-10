using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.TerrainFeatures
{
	public class Stalagmite : TerrainFeature
	{
		public const float shakeRate = 0.0122718466f;

		public const float shakeDecayRate = 0.00613592332f;

		public const int minWoodDebrisForFallenTree = 8;

		public const int minWoodDebrisForStump = 4;

		public const int startingHealth = 10;

		public const int leafFallRate = 3;

		public const int bushyTree = 1;

		public const int leafyTree = 2;

		public const int pineTree = 3;

		public const int winterTree1 = 4;

		public const int winterTree2 = 5;

		public const int palmTree = 6;

		public const int seedStage = 0;

		public const int sproutStage = 1;

		public const int saplingStage = 2;

		public const int bushStage = 3;

		public const int treeStage = 5;

		private Texture2D texture;

		public float health;

		public bool stump;

		private bool shakeLeft;

		private bool falling;

		private bool tall;

		private bool drop;

		private float shakeRotation;

		private float maxShake;

		private float dropY;

		private List<Leaf> leaves = new List<Leaf>();

		public Stalagmite()
		{
		}

		public Stalagmite(bool tall)
		{
			this.loadSprite();
			this.health = 10f;
			this.tall = tall;
		}

		public override void loadSprite()
		{
			try
			{
				string str = (Game1.mine.mineLevel >= 40 && Game1.mine.mineLevel < 80) ? "_Frost" : ((Game1.mine.mineLevel < 40) ? "" : "_Lava");
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Stalagmite" + str);
			}
			catch (Exception)
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Stalagmite");
			}
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			if (this.health > 0f)
			{
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			}
			return Rectangle.Empty;
		}

		public override bool performUseAction(Vector2 tileLocation)
		{
			this.shake(tileLocation);
			return false;
		}

		private int extraStoneCalculator(Vector2 tileLocation)
		{
			Random arg_2D_0 = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
			int num = 0;
			if (arg_2D_0.NextDouble() < Game1.dailyLuck)
			{
				num++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.MiningLevel / 12.5)
			{
				num++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.MiningLevel / 12.5)
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
			if (!this.falling)
			{
				if (this.maxShake > 0f)
				{
					if (this.shakeLeft)
					{
						this.shakeRotation -= 0.0122718466f;
						if (this.shakeRotation <= -this.maxShake)
						{
							this.shakeLeft = false;
						}
					}
					else
					{
						this.shakeRotation += 0.0122718466f;
						if (this.shakeRotation >= this.maxShake)
						{
							this.shakeLeft = true;
						}
					}
				}
				if (this.maxShake > 0f)
				{
					this.maxShake = Math.Max(0f, this.maxShake - 0.00613592332f);
				}
				if (this.drop)
				{
					this.dropY += 10f;
					if (this.dropY >= tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 2))
					{
						this.drop = false;
						Game1.playSound("cavedrip");
						Game1.createWaterDroplets(this.texture, new Rectangle(Game1.tileSize, 0, 4, 4), (int)tileLocation.X * Game1.tileSize + Game1.tileSize, (int)(tileLocation.Y - 2f) * Game1.tileSize, Game1.random.Next(4, 5), (int)(tileLocation.Y + 1f));
					}
				}
				if (!this.drop && Game1.random.NextDouble() < 0.005)
				{
					this.drop = true;
					this.dropY = tileLocation.Y * (float)Game1.tileSize - (float)Game1.viewport.Height;
				}
			}
			else
			{
				this.shakeRotation += (this.shakeLeft ? (-(this.maxShake * this.maxShake)) : (this.maxShake * this.maxShake));
				this.maxShake += 0.00204530777f;
				if ((double)Math.Abs(this.shakeRotation) > 1.5707963267948966)
				{
					this.falling = false;
					this.maxShake = 0f;
					Game1.playSound("stoneCrack");
					Game1.createRadialDebris(Game1.currentLocation, 14, (int)tileLocation.X + (this.shakeLeft ? -2 : 2), (int)tileLocation.Y, 8 + this.extraStoneCalculator(tileLocation), true, -1, false, -1);
					Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(Game1.tileSize / 4, Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4), (int)tileLocation.X + (this.shakeLeft ? -2 : 2), (int)tileLocation.Y, Game1.random.Next(40, 60));
					if (this.health <= 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void shake(Vector2 tileLocation)
		{
			if (this.maxShake == 0f && !this.stump)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = 0.0490873866f;
			}
		}

		public override bool isPassable(Character c = null)
		{
			return this.health <= -99f;
		}

		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (this.health <= -99f)
			{
				return false;
			}
			if (t != null && t.name.Contains("Pickaxe"))
			{
				Game1.playSound("hammer");
				Game1.currentLocation.debris.Add(new Debris(this.texture, new Rectangle(Game1.tileSize / 4, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), Game1.player.GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f)));
			}
			else if (explosion <= 0)
			{
				return false;
			}
			this.shake(tileLocation);
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
					Game1.playSound("treecrack");
					this.maxShake = 0f;
					this.stump = true;
					this.health = 1f;
					this.falling = true;
					this.shakeLeft = (Game1.player.getTileLocation().X >= tileLocation.X && (Game1.player.getTileLocation().X != tileLocation.X || Game1.random.NextDouble() >= 0.5));
				}
				else
				{
					this.health = -100f;
					Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(2 * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 7, Game1.tileSize / 2, Game1.tileSize / 2), (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(30, 40));
					Game1.createRadialDebris(Game1.currentLocation, 14, (int)tileLocation.X, (int)tileLocation.Y, 1, true, -1, false, -1);
					if (!this.falling)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (!this.stump || this.falling)
			{
				if (this.tall)
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(Game1.tileSize, Game1.tileSize, Game1.tileSize, Game1.tileSize * 3)), Color.White, this.shakeRotation, new Vector2((float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), (float)(Game1.tileSize * 2)), 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 1E-06f);
				}
				else
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(0, 0, Game1.tileSize, Game1.tileSize * 3)), Color.White, this.shakeRotation, new Vector2((float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), (float)(Game1.tileSize * 2)), 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 1E-06f);
				}
			}
			if (this.health > 0f || !this.falling)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(0, 3 * Game1.tileSize, Game1.tileSize, Game1.tileSize)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f);
			}
			if (this.drop)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)Game1.tileSize * tileLocation.X + (float)(Game1.tileSize / 2), this.dropY)), new Rectangle?(new Rectangle(Game1.tileSize, 0, 4, 8)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
			}
		}
	}
}
