using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
	public class ResourceClump : TerrainFeature
	{
		public const int stumpIndex = 600;

		public const int hollowLogIndex = 602;

		public const int meteoriteIndex = 622;

		public const int boulderIndex = 672;

		public const int mineRock1Index = 752;

		public const int mineRock2Index = 754;

		public const int mineRock3Index = 756;

		public const int mineRock4Index = 758;

		public int width;

		public int height;

		public int parentSheetIndex;

		public float health;

		public Vector2 tile;

		protected float shakeTimer;

		public ResourceClump()
		{
		}

		public ResourceClump(int parentSheetIndex, int width, int height, Vector2 tile)
		{
			this.width = width;
			this.height = height;
			this.parentSheetIndex = parentSheetIndex;
			this.tile = tile;
			if (parentSheetIndex <= 602)
			{
				if (parentSheetIndex == 600)
				{
					this.health = 10f;
					return;
				}
				if (parentSheetIndex != 602)
				{
					return;
				}
				this.health = 20f;
				return;
			}
			else
			{
				if (parentSheetIndex != 622)
				{
					if (parentSheetIndex != 672)
					{
						switch (parentSheetIndex)
						{
						case 752:
						case 754:
						case 756:
						case 758:
							this.health = 8f;
							return;
						case 753:
						case 755:
						case 757:
							break;
						default:
							return;
						}
					}
					else
					{
						this.health = 10f;
					}
					return;
				}
				this.health = 20f;
				return;
			}
		}

		public override bool isPassable(Character c = null)
		{
			return false;
		}

		public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
		{
			if (t == null)
			{
				return false;
			}
			int debrisType = 12;
			int num = this.parentSheetIndex;
			if (num <= 602)
			{
				if (num != 600)
				{
					if (num == 602)
					{
						if (t is Axe && t.upgradeLevel < 2)
						{
							Game1.playSound("axe");
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceClump.cs.13948", new object[0]));
							Game1.player.jitterStrength = 1f;
							return false;
						}
						if (!(t is Axe))
						{
							return false;
						}
						Game1.playSound("axchop");
					}
				}
				else
				{
					if (t is Axe && t.upgradeLevel < 1)
					{
						Game1.playSound("axe");
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceClump.cs.13945", new object[0]));
						Game1.player.jitterStrength = 1f;
						return false;
					}
					if (!(t is Axe))
					{
						return false;
					}
					Game1.playSound("axchop");
				}
			}
			else if (num != 622)
			{
				if (num != 672)
				{
					switch (num)
					{
					case 752:
					case 754:
					case 756:
					case 758:
						if (!(t is Pickaxe))
						{
							return false;
						}
						Game1.playSound("hammer");
						debrisType = 14;
						this.shakeTimer = 500f;
						break;
					}
				}
				else
				{
					if (t is Pickaxe && t.upgradeLevel < 2)
					{
						Game1.playSound("clubhit");
						Game1.playSound("clank");
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceClump.cs.13956", new object[0]));
						Game1.player.jitterStrength = 1f;
						return false;
					}
					if (!(t is Pickaxe))
					{
						return false;
					}
					Game1.playSound("hammer");
					debrisType = 14;
				}
			}
			else
			{
				if (t is Pickaxe && t.upgradeLevel < 3)
				{
					Game1.playSound("clubhit");
					Game1.playSound("clank");
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceClump.cs.13952", new object[0]));
					Game1.player.jitterStrength = 1f;
					return false;
				}
				if (!(t is Pickaxe))
				{
					return false;
				}
				Game1.playSound("hammer");
				debrisType = 14;
			}
			float num2 = Math.Max(1f, (float)(t.upgradeLevel + 1) * 0.75f);
			this.health -= num2;
			Game1.createRadialDebris(Game1.currentLocation, debrisType, (int)tileLocation.X + Game1.random.Next(this.width / 2 + 1), (int)tileLocation.Y + Game1.random.Next(this.height / 2 + 1), Game1.random.Next(4, 9), false, -1, false, -1);
			if (this.health <= 0f)
			{
				if (Game1.IsMultiplayer)
				{
					Random arg_2BF_0 = Game1.recentMultiplayerRandom;
				}
				else
				{
					new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + Game1.stats.DaysPlayed + this.health));
				}
				num = this.parentSheetIndex;
				if (num <= 602)
				{
					if (num == 600 || num == 602)
					{
						t.getLastFarmerToUse().gainExperience(2, 25);
						int num3 = (this.parentSheetIndex == 602) ? 8 : 2;
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
							Random arg_3B0_0 = Game1.recentMultiplayerRandom;
						}
						else
						{
							new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(709, (int)tileLocation.X, (int)tileLocation.Y, num3, t.getLastFarmerToUse().uniqueMultiplayerID);
						}
						else
						{
							Game1.createMultipleObjectDebris(709, (int)tileLocation.X, (int)tileLocation.Y, num3);
						}
						Game1.playSound("stumpCrack");
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(23, tileLocation * (float)Game1.tileSize, Color.White, 4, false, 140f, 0, Game1.tileSize * 2, -1f, Game1.tileSize * 2, 0));
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(385, 1522, 127, 79), 2000f, 1, 1, tileLocation * (float)Game1.tileSize + new Vector2(0f, 49f), false, false, 1E-05f, 0.016f, Color.White, 1f, 0f, 0f, 0f, false));
						Game1.createRadialDebris(Game1.currentLocation, 34, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(4, 9), false, -1, false, -1);
						return true;
					}
				}
				else
				{
					int num3;
					if (num != 622)
					{
						if (num != 672)
						{
							switch (num)
							{
							case 752:
							case 754:
							case 756:
							case 758:
								break;
							case 753:
							case 755:
							case 757:
								return false;
							default:
								return false;
							}
						}
						num3 = ((this.parentSheetIndex == 672) ? 15 : 10);
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
							Random arg_557_0 = Game1.recentMultiplayerRandom;
						}
						else
						{
							new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(390, (int)tileLocation.X, (int)tileLocation.Y, num3, t.getLastFarmerToUse().uniqueMultiplayerID);
						}
						else
						{
							Game1.createRadialDebris(Game1.currentLocation, 390, (int)tileLocation.X, (int)tileLocation.Y, num3, false, -1, true, -1);
						}
						Game1.playSound("boulderBreak");
						Game1.createRadialDebris(Game1.currentLocation, 32, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(6, 12), false, -1, false, -1);
						Color white = Color.White;
						switch (this.parentSheetIndex)
						{
						case 752:
							white = new Color(188, 119, 98);
							break;
						case 754:
							white = new Color(168, 120, 95);
							break;
						case 756:
						case 758:
							white = new Color(67, 189, 238);
							break;
						}
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(48, tileLocation * (float)Game1.tileSize, white, 5, false, 180f, 0, Game1.tileSize * 2, -1f, Game1.tileSize * 2, 0)
						{
							alphaFade = 0.01f
						});
						return true;
					}
					num3 = 6;
					if (Game1.IsMultiplayer)
					{
						Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
						Random arg_6F3_0 = Game1.recentMultiplayerRandom;
					}
					else
					{
						new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
					}
					if (Game1.IsMultiplayer)
					{
						Game1.createMultipleObjectDebris(386, (int)tileLocation.X, (int)tileLocation.Y, num3, t.getLastFarmerToUse().uniqueMultiplayerID);
						Game1.createMultipleObjectDebris(390, (int)tileLocation.X, (int)tileLocation.Y, num3, t.getLastFarmerToUse().uniqueMultiplayerID);
						Game1.createMultipleObjectDebris(535, (int)tileLocation.X, (int)tileLocation.Y, 2, t.getLastFarmerToUse().uniqueMultiplayerID);
					}
					else
					{
						Game1.createMultipleObjectDebris(386, (int)tileLocation.X, (int)tileLocation.Y, num3);
						Game1.createMultipleObjectDebris(390, (int)tileLocation.X, (int)tileLocation.Y, num3);
						Game1.createMultipleObjectDebris(535, (int)tileLocation.X, (int)tileLocation.Y, 2);
					}
					Game1.playSound("boulderBreak");
					Game1.createRadialDebris(Game1.currentLocation, 32, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(6, 12), false, -1, false, -1);
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 0f)) * (float)Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 1f)) * (float)Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(0f, 1f)) * (float)Game1.tileSize, Color.White, 8, false, 90f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), Color.White, 8, false, 70f, 0, -1, -1f, -1, 0));
					return true;
				}
			}
			else
			{
				this.shakeTimer = 100f;
			}
			return false;
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, this.width * Game1.tileSize, this.height * Game1.tileSize);
		}

		public bool occupiesTile(int x, int y)
		{
			return (float)x >= this.tile.X && (float)x - this.tile.X < (float)this.width && (float)y >= this.tile.Y && (float)y - this.tile.Y < (float)this.height;
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			Rectangle sourceRectForStandardTileSheet = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex, 16, 16);
			sourceRectForStandardTileSheet.Width = this.width * 16;
			sourceRectForStandardTileSheet.Height = this.height * 16;
			Vector2 globalPosition = this.tile * (float)Game1.tileSize;
			if (this.shakeTimer > 0f)
			{
				globalPosition.X += (float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 4f;
			}
			spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(sourceRectForStandardTileSheet), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (this.tile.Y + 1f) * (float)Game1.tileSize / 10000f + this.tile.X / 100000f);
		}

		public override void loadSprite()
		{
		}

		public override bool performUseAction(Vector2 tileLocation)
		{
			int num = this.parentSheetIndex;
			if (num != 602)
			{
				if (num != 622)
				{
					if (num == 672)
					{
						Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceClump.cs.13963", new object[0])));
					}
				}
				else
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceClump.cs.13964", new object[0])));
				}
			}
			else
			{
				Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceClump.cs.13962", new object[0])));
			}
			return true;
		}

		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.shakeTimer > 0f)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			}
			return false;
		}

		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}
	}
}
