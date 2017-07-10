using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.TerrainFeatures
{
	public class Quartz : TerrainFeature
	{
		public const float shakeRate = 0.0157079641f;

		public const float shakeDecayRate = 0.00306796166f;

		public const double chanceForDiamond = 0.02;

		public const double chanceForPrismaticShard = 0.005;

		public const double chanceForIridium = 0.007;

		public const double chanceForLevelUnique = 0.03;

		public const double chanceForRefinedQuartz = 0.04;

		public const int startingHealth = 10;

		public const int large = 3;

		public const int medium = 2;

		public const int small = 1;

		public const int tiny = 0;

		public const int pointingLeft = 0;

		public const int pointingUp = 1;

		public const int pointingRight = 2;

		private Texture2D texture;

		public float health;

		public bool flipped;

		private bool shakeLeft;

		private bool falling;

		private float shakeRotation;

		private float maxShake;

		private float glow;

		public int bigness;

		private int identifier;

		private Color color;

		public Quartz()
		{
		}

		public Quartz(int bigness, Color color)
		{
			this.loadSprite();
			this.health = (float)(10 - (3 - bigness) * 2);
			this.bigness = bigness;
			if (bigness >= 3)
			{
				this.bigness = 2;
			}
			this.color = color;
		}

		public override void loadSprite()
		{
			try
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Quartz");
			}
			catch (Exception)
			{
			}
			this.identifier = Game1.random.Next(-999999, 999999);
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			int arg_06_0 = this.bigness;
			return new Rectangle((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.Y * (float)Game1.tileSize), Game1.tileSize, Game1.tileSize);
		}

		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.glow > 0f)
			{
				this.glow -= 0.01f;
			}
			if (this.maxShake > 0f)
			{
				if (this.shakeLeft)
				{
					this.shakeRotation -= 0.0157079641f;
					if (this.shakeRotation <= -this.maxShake)
					{
						this.shakeLeft = false;
					}
				}
				else
				{
					this.shakeRotation += 0.0157079641f;
					if (this.shakeRotation >= this.maxShake)
					{
						this.shakeLeft = true;
					}
				}
			}
			if (this.maxShake > 0f)
			{
				this.maxShake = Math.Max(0f, this.maxShake - 0.00306796166f);
			}
			return this.health <= 0f;
		}

		public override void performPlayerEntryAction(Vector2 tileLocation)
		{
			Color value = (this.glow > 0f) ? new Color((float)this.color.R + this.glow * 50f, (float)this.color.G + this.glow * 50f, (float)this.color.B + this.glow * 50f) : this.color;
			value *= 0.3f + this.glow;
			if (this.bigness < 2)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), 1f, Utility.getOppositeColor(value), (int)(tileLocation.X * 1000f + tileLocation.Y)));
				return;
			}
			if (this.bigness == 2)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), 1f, Utility.getOppositeColor(value), (int)(tileLocation.X * 1000f + tileLocation.Y)));
				Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2)), 1f, Utility.getOppositeColor(value), (int)(tileLocation.X * 1000f + tileLocation.Y)));
			}
		}

		private void shake(Vector2 tileLocation)
		{
			if (this.maxShake == 0f)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = 0.0245436933f;
			}
		}

		public override bool performUseAction(Vector2 tileLocation)
		{
			if (Game1.soundBank != null)
			{
				Random arg_4F_0 = new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + (float)Game1.mine.mineLevel));
				Cue cue = Game1.soundBank.GetCue("crystal");
				int num = arg_4F_0.Next(2400);
				num -= num % 100;
				cue.SetVariable("Pitch", (float)num);
				cue.Play();
			}
			this.glow = 0.7f;
			return false;
		}

		public override bool isPassable(Character c = null)
		{
			return this.health <= 0f;
		}

		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		private Rectangle getSourceRect(int size)
		{
			switch (size)
			{
			case 0:
				return new Rectangle(Game1.tileSize, 0, Game1.tileSize, Game1.tileSize);
			case 1:
				return new Rectangle(4 * Game1.tileSize + ((this.health <= 3f) ? Game1.tileSize : 0), Game1.tileSize, Game1.tileSize, Game1.tileSize);
			case 2:
				return new Rectangle((int)((8f - this.health) / 2f) * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 2);
			default:
				return Rectangle.Empty;
			}
		}

		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (this.health > 0f)
			{
				float num = 0f;
				if (t == null && explosion > 0)
				{
					num = (float)explosion;
				}
				else if (t.name.Contains("Pickaxe"))
				{
					switch (t.upgradeLevel)
					{
					case 0:
						num = 2f;
						break;
					case 1:
						num = 2.5f;
						break;
					case 2:
						num = 3.34f;
						break;
					case 3:
						num = 5f;
						break;
					case 4:
						num = 10f;
						break;
					}
					Game1.playSound("hammer");
				}
				if (num > 0f)
				{
					this.glow = 0.7f;
					this.shake(tileLocation);
					this.health -= num;
					if (this.health <= 0f)
					{
						Random random = new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + (float)Game1.mine.mineLevel + (float)Game1.player.timesReachedMineBottom));
						double num2 = 1.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel / 100.0 + (double)Game1.player.miningLevel / 50.0;
						if (random.NextDouble() < 0.005 * num2)
						{
							Game1.createObjectDebris(74, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						else if (random.NextDouble() < 0.007 * num2)
						{
							Game1.createDebris(10, (int)tileLocation.X, (int)tileLocation.Y, 2, null);
						}
						else if (random.NextDouble() < 0.02 * num2)
						{
							Game1.createObjectDebris(72, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						else if (random.NextDouble() < 0.03 * num2)
						{
							Game1.createObjectDebris((Game1.mine.mineLevel < 40) ? 86 : ((Game1.mine.mineLevel < 80) ? 84 : 82), (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						else if (random.NextDouble() < 0.04 * num2)
						{
							Game1.createObjectDebris(338, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						for (int i = 0; i < this.bigness * 3; i++)
						{
							int num3 = Game1.random.Next(this.getBoundingBox(tileLocation).X, this.getBoundingBox(tileLocation).Right);
							int num4 = Game1.random.Next(this.getBoundingBox(tileLocation).Y, this.getBoundingBox(tileLocation).Bottom);
							Game1.currentLocation.TemporarySprites.Add(new CosmeticDebris(this.texture, new Vector2((float)num3, (float)num4), (float)Game1.random.Next(-25, 25) / 100f, (float)(num3 - this.getBoundingBox(tileLocation).Center.X) / 30f, (float)Game1.random.Next(-800, -100) / 100f, (int)tileLocation.Y * Game1.tileSize + Game1.tileSize, new Rectangle(Game1.random.Next(4, 8) * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize), this.color, (Game1.soundBank != null) ? Game1.soundBank.GetCue("boulderCrack") : null, new LightSource(4, Vector2.Zero, 0.1f, Utility.getOppositeColor(this.color)), 24, 1000));
						}
						Utility.removeLightSource((int)(tileLocation.X * 1000f + tileLocation.Y));
					}
				}
			}
			return false;
		}

		private Vector2 getPivot()
		{
			switch (this.bigness)
			{
			case 1:
				return new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize);
			case 2:
				return new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 2));
			case 3:
				return new Vector2((float)Game1.tileSize, (float)(Game1.tileSize * 3));
			default:
				return Vector2.Zero;
			}
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.health > 0f)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)this.getBoundingBox(tileLocation).Center.X, (float)this.getBoundingBox(tileLocation).Bottom)), new Rectangle?(this.getSourceRect(this.bigness)), this.color, this.shakeRotation, this.getPivot(), 1f, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize) / 10000f);
			}
		}
	}
}
