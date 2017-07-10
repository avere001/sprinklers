using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
	public class Bush : LargeTerrainFeature
	{
		public const float shakeRate = 0.0157079641f;

		public const float shakeDecayRate = 0.00306796166f;

		public const int smallBush = 0;

		public const int mediumBush = 1;

		public const int largeBush = 2;

		public static Texture2D texture;

		public int size;

		public int tileSheetOffset;

		public float health;

		public bool flipped;

		public bool townBush;

		public bool drawShadow = true;

		private bool shakeLeft;

		private float shakeRotation;

		private float maxShake;

		private float alpha = 1f;

		private long lastPlayerToHit;

		private float shakeTimer;

		private Rectangle sourceRect;

		public static Rectangle treeTopSourceRect = new Rectangle(0, 0, 48, 96);

		public static Rectangle stumpSourceRect = new Rectangle(32, 96, 16, 32);

		public static Rectangle shadowSourceRect = new Rectangle(663, 1011, 41, 30);

		public Bush()
		{
		}

		public Bush(Vector2 tileLocation, int size, GameLocation location)
		{
			this.tilePosition = tileLocation;
			this.size = size;
			if (size == 0)
			{
				this.tileSheetOffset = Game1.random.Next(2);
			}
			if (location is Town && tileLocation.X % 5f != 0f)
			{
				this.townBush = true;
			}
			if (location.map.GetLayer("Front").Tiles[(int)tileLocation.X, (int)tileLocation.Y] != null)
			{
				this.drawShadow = false;
			}
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
		}

		public void setUpSourceRect()
		{
			int seasonNumber = Utility.getSeasonNumber(Game1.currentSeason);
			if (this.size == 0)
			{
				this.sourceRect = new Rectangle(seasonNumber * 16 * 2 + this.tileSheetOffset * 16, 224, 16, 32);
				return;
			}
			if (this.size != 1)
			{
				if (this.size == 2)
				{
					if (this.townBush && (seasonNumber == 0 || seasonNumber == 1))
					{
						this.sourceRect = new Rectangle(48, 176, 48, 48);
						return;
					}
					switch (seasonNumber)
					{
					case 0:
					case 1:
						this.sourceRect = new Rectangle(0, 128, 48, 48);
						return;
					case 2:
						this.sourceRect = new Rectangle(48, 128, 48, 48);
						return;
					case 3:
						this.sourceRect = new Rectangle(0, 176, 48, 48);
						break;
					default:
						return;
					}
				}
				return;
			}
			if (this.townBush)
			{
				this.sourceRect = new Rectangle(seasonNumber * 16 * 2, 96, 32, 32);
				return;
			}
			this.sourceRect = new Rectangle((seasonNumber * 16 * 4 + this.tileSheetOffset * 16 * 2) % Bush.texture.Bounds.Width, (seasonNumber * 16 * 4 + this.tileSheetOffset * 16 * 2) / Bush.texture.Bounds.Width * 3 * 16, 32, 48);
		}

		public bool inBloom(string season, int dayOfMonth)
		{
			if (season.Equals("spring"))
			{
				if (dayOfMonth > 14 && dayOfMonth < 19)
				{
					return true;
				}
			}
			else if (season.Equals("fall") && dayOfMonth > 7 && dayOfMonth < 12)
			{
				return true;
			}
			return false;
		}

		public override bool isActionable()
		{
			return true;
		}

		public override void loadSprite()
		{
			if (Bush.texture == null)
			{
				try
				{
					Bush.texture = Game1.content.Load<Texture2D>("TileSheets\\bushes");
				}
				catch (Exception)
				{
				}
			}
			Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame) + (uint)((int)this.tilePosition.X) + (uint)((int)this.tilePosition.Y * 777)));
			if (this.size == 1 && this.tileSheetOffset == 0 && random.NextDouble() < 0.5 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 1;
			}
			else if (!Game1.currentSeason.Equals("summer") && !this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 0;
			}
			this.setUpSourceRect();
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			switch (this.size)
			{
			case 0:
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			case 1:
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
			case 2:
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize);
			default:
				return Rectangle.Empty;
			}
		}

		public override bool performUseAction(Vector2 tileLocation)
		{
			if (this.maxShake == 0f)
			{
				Game1.playSound("leafrustle");
			}
			this.shake(tileLocation, false);
			return true;
		}

		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.shakeTimer > 0f)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			}
			this.alpha = Math.Min(1f, this.alpha + 0.05f);
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
			return false;
		}

		private void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
		{
			if (this.maxShake == 0f | doEvenIfStillShaking)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = 0.0245436933f;
				if (!this.townBush && this.tileSheetOffset == 1 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
				{
					int num = -1;
					string currentSeason = Game1.currentSeason;
					if (!(currentSeason == "spring"))
					{
						if (currentSeason == "fall")
						{
							num = 410;
						}
					}
					else
					{
						num = 296;
					}
					if (num != -1)
					{
						this.tileSheetOffset = 0;
						this.setUpSourceRect();
						int num2 = new Random((int)tileLocation.X + (int)tileLocation.Y * 5000 + (int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed).Next(1, 2) + Game1.player.ForagingLevel / 4;
						for (int i = 0; i < num2; i++)
						{
							Game1.createItemDebris(new StardewValley.Object(num, 1, false, -1, Game1.player.professions.Contains(16) ? 4 : 0), Utility.PointToVector2(base.getBoundingBox().Center), Game1.random.Next(1, 4), null);
						}
						DelayedAction.playSoundAfterDelay("leafrustle", 100);
						return;
					}
				}
				else if (tileLocation.X == 20f && tileLocation.Y == 8f && Game1.dayOfMonth == 28 && Game1.timeOfDay == 1200 && !Game1.player.mailReceived.Contains("junimoPlush"))
				{
					Game1.player.addItemByMenuIfNecessaryElseHoldUp(new Furniture(1733, Vector2.Zero), new ItemGrabMenu.behaviorOnItemSelect(this.junimoPlushCallback));
				}
			}
		}

		public void junimoPlushCallback(Item item, Farmer who)
		{
			if (item != null && item is Furniture && (item as Furniture).parentSheetIndex == 1733 && who != null)
			{
				who.mailReceived.Add("junimoPlush");
			}
		}

		public override bool isPassable(Character c = null)
		{
			return false;
		}

		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
			if (this.size == 1 && this.tileSheetOffset == 0 && Game1.random.NextDouble() < 0.2 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 1;
				this.setUpSourceRect();
			}
			else if (!Game1.currentSeason.Equals("summer") && !this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 0;
				this.setUpSourceRect();
			}
			this.health = 0f;
		}

		public override bool seasonUpdate(bool onLoad)
		{
			if (this.size == 1 && Game1.currentSeason.Equals("summer") && Game1.random.NextDouble() < 0.5)
			{
				this.tileSheetOffset = 1;
			}
			else
			{
				this.tileSheetOffset = 0;
			}
			this.loadSprite();
			return false;
		}

		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			if (explosion > 0)
			{
				this.shake(tileLocation, true);
				return false;
			}
			if (t != null && t is Axe && this.isDestroyable(location, tileLocation))
			{
				Game1.playSound("leafrustle");
				this.shake(tileLocation, true);
				if ((t as Axe).upgradeLevel >= 1)
				{
					this.health -= (float)(t as Axe).upgradeLevel / 5f;
					if (this.health <= -1f)
					{
						Game1.playSound("treethud");
						DelayedAction.playSoundAfterDelay("leafrustle", 100);
						Color color = Color.Green;
						string currentSeason = Game1.currentSeason;
						if (!(currentSeason == "spring"))
						{
							if (!(currentSeason == "summer"))
							{
								if (!(currentSeason == "fall"))
								{
									if (currentSeason == "winter")
									{
										color = Color.Cyan;
									}
								}
								else
								{
									color = Color.IndianRed;
								}
							}
							else
							{
								color = Color.ForestGreen;
							}
						}
						else
						{
							color = Color.Green;
						}
						for (int i = 0; i <= this.size; i++)
						{
							for (int j = 0; j < 12; j++)
							{
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(355, 1200 + (Game1.IsFall ? 16 : (Game1.IsWinter ? -16 : 0)), 16, 16), Utility.getRandomPositionInThisRectangle(base.getBoundingBox(), Game1.random) - new Vector2(0f, (float)Game1.random.Next(Game1.tileSize)), false, 0.01f, Game1.IsWinter ? Color.Cyan : Color.White)
								{
									motion = new Vector2((float)Game1.random.Next(-10, 11) / 10f, (float)(-(float)Game1.random.Next(5, 7))),
									acceleration = new Vector2(0f, (float)Game1.random.Next(13, 17) / 100f),
									accelerationChange = new Vector2(0f, -0.001f),
									scale = (float)Game1.pixelZoom,
									layerDepth = tileLocation.Y * (float)Game1.tileSize / 10000f,
									animationLength = 11,
									totalNumberOfLoops = 99,
									interval = (float)Game1.random.Next(20, 90),
									delayBeforeAnimationStart = (i + 1) * j * 20
								});
								if (j % 6 == 0)
								{
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(50, Utility.getRandomPositionInThisRectangle(base.getBoundingBox(), Game1.random) - new Vector2((float)(Game1.tileSize / 2), (float)Game1.random.Next(Game1.tileSize / 2, Game1.tileSize)), color, 8, false, 100f, 0, -1, -1f, -1, 0));
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(12, Utility.getRandomPositionInThisRectangle(base.getBoundingBox(), Game1.random) - new Vector2((float)(Game1.tileSize / 2), (float)Game1.random.Next(Game1.tileSize / 2, Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
								}
							}
						}
						return true;
					}
					Game1.playSound("axchop");
				}
			}
			return false;
		}

		public bool isDestroyable(GameLocation location, Vector2 tile)
		{
			if (location != null && location is Farm)
			{
				switch (Game1.whichFarm)
				{
				case 1:
					return new Rectangle(32, 11, 11, 25).Contains((int)tile.X, (int)tile.Y);
				case 2:
					return (tile.X == 13f && tile.Y == 35f) || (tile.X == 37f && tile.Y == 9f) || new Rectangle(43, 11, 34, 50).Contains((int)tile.X, (int)tile.Y);
				case 3:
					return new Rectangle(24, 56, 10, 8).Contains((int)tile.X, (int)tile.Y);
				}
			}
			return false;
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			layerDepth += positionOnScreen.X / 100000f;
			spriteBatch.Draw(Bush.texture, positionOnScreen + new Vector2(0f, (float)(-(float)Game1.tileSize) * scale), new Rectangle?(new Rectangle(32, 96, 16, 32)), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale - 1f) / 20000f);
		}

		public override void performPlayerEntryAction(Vector2 tileLocation)
		{
			base.performPlayerEntryAction(tileLocation);
			if (!Game1.currentSeason.Equals("winter") && !Game1.isRaining && Game1.isDarkOut() && Game1.random.NextDouble() < (Game1.currentSeason.Equals("summer") ? 0.08 : 0.04))
			{
				AmbientLocationSounds.addSound(tileLocation, 3);
				Game1.debugOutput = Game1.debugOutput + "  added cricket at " + tileLocation.ToString();
			}
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.drawShadow)
			{
				if (this.size > 0)
				{
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((tileLocation.X + ((this.size == 1) ? 0.5f : 1f)) * (float)Game1.tileSize - (float)(Game1.tileSize * 4 / 5), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 4))), new Rectangle?(Bush.shadowSourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-06f);
				}
				else
				{
					spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize - (float)Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * this.alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 1E-06f);
				}
			}
			spriteBatch.Draw(Bush.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)((this.size + 1) * Game1.tileSize / 2), (tileLocation.Y + 1f) * (float)Game1.tileSize - (float)((this.size > 0 && (!this.townBush || this.size != 1)) ? Game1.tileSize : 0))), new Rectangle?(this.sourceRect), Color.White * this.alpha, this.shakeRotation, new Vector2((float)((this.size + 1) * 16 / 2), 32f), (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.getBoundingBox(tileLocation).Center.Y + 48) / 10000f - tileLocation.X / 1000000f);
		}
	}
}
