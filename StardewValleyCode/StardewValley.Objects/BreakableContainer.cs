using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Tools;
using System;

namespace StardewValley.Objects
{
	public class BreakableContainer : StardewValley.Object
	{
		public const int barrel = 118;

		public const int frostBarrel = 120;

		public const int darkBarrel = 122;

		public const int desertBarrel = 124;

		private int debris;

		private new int shakeTimer;

		private new int health;

		private new int type;

		private string hitSound;

		private string breakSound;

		private Rectangle breakDebrisSource;

		private Rectangle breakDebrisSource2;

		public BreakableContainer()
		{
		}

		public BreakableContainer(Vector2 tile, int type) : base(tile, BreakableContainer.typeToIndex(type), false)
		{
			this.type = type;
			if (type == 118)
			{
				if (Game1.mine.getMineArea(-1) == 40)
				{
					this.parentSheetIndex = 120;
					this.type = 120;
				}
				if (Game1.mine.getMineArea(-1) == 80)
				{
					this.parentSheetIndex = 122;
					this.type = 122;
				}
				if (Game1.mine.getMineArea(-1) == 121)
				{
					this.parentSheetIndex = 124;
					this.type = 124;
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					this.parentSheetIndex++;
				}
				this.health = 3;
				this.debris = 12;
				this.hitSound = "woodWhack";
				this.breakSound = "barrelBreak";
				this.breakDebrisSource = new Rectangle(598, 1275, 13, 4);
				this.breakDebrisSource2 = new Rectangle(611, 1275, 10, 4);
			}
		}

		public static int typeToIndex(int type)
		{
			if (type == 118)
			{
				return type;
			}
			if (type != 120)
			{
				return 0;
			}
			return type;
		}

		public override bool performToolAction(Tool t)
		{
			if (t.isHeavyHitter())
			{
				this.health--;
				if (t is MeleeWeapon && (t as MeleeWeapon).type == 2)
				{
					this.health--;
				}
				if (this.health <= 0)
				{
					if (this.breakSound != null)
					{
						Game1.playSound(this.breakSound);
					}
					this.releaseContents(t.getLastFarmerToUse().currentLocation, t.getLastFarmerToUse());
					t.getLastFarmerToUse().currentLocation.objects.Remove(this.tileLocation);
					int num = Game1.random.Next(4, 12);
					Color color = (this.type == 120) ? Color.White : ((this.type == 122) ? new Color(109, 122, 80) : ((this.type == 124) ? new Color(229, 171, 84) : new Color(130, 80, 30)));
					for (int i = 0; i < num; i++)
					{
						t.getLastFarmerToUse().currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, (Game1.random.NextDouble() < 0.5) ? this.breakDebrisSource : this.breakDebrisSource2, 999f, 1, 0, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, (this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)) / 10000f, 0.01f, color, (float)Game1.pixelZoom, 0f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 8f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 64f, false)
						{
							motion = new Vector2((float)Game1.random.Next(-30, 31) / 10f, (float)Game1.random.Next(-10, -7)),
							acceleration = new Vector2(0f, 0.3f)
						});
					}
				}
				else if (this.hitSound != null)
				{
					this.shakeTimer = 300;
					Game1.playSound(this.hitSound);
					Game1.createRadialDebris(t.getLastFarmerToUse().currentLocation, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(4, 7), false, -1, false, (this.type == 120) ? 10000 : -1);
				}
			}
			return false;
		}

		public override bool onExplosion(Farmer who, GameLocation location)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			this.releaseContents(location, who);
			int num = Game1.random.Next(4, 12);
			Color color = (this.type == 120) ? Color.White : ((this.type == 122) ? new Color(109, 122, 80) : new Color(130, 80, 30));
			for (int i = 0; i < num; i++)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, (Game1.random.NextDouble() < 0.5) ? this.breakDebrisSource : this.breakDebrisSource2, 999f, 1, 0, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, (this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)) / 10000f, 0.01f, color, (float)Game1.pixelZoom, 0f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 8f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 64f, false)
				{
					motion = new Vector2((float)Game1.random.Next(-30, 31) / 10f, (float)Game1.random.Next(-10, -7)),
					acceleration = new Vector2(0f, 0.3f)
				});
			}
			return true;
		}

		public void releaseContents(GameLocation location, Farmer who)
		{
			Random random = new Random((int)this.tileLocation.X + (int)this.tileLocation.Y * 10000 + (int)Game1.stats.DaysPlayed);
			int num = (int)this.tileLocation.X;
			int num2 = (int)this.tileLocation.Y;
			int level = -1;
			if (location is MineShaft)
			{
				level = ((MineShaft)location).mineLevel;
				if (((MineShaft)location).isContainerPlatform(num, num2))
				{
					((MineShaft)location).updateMineLevelData(0, -1);
				}
			}
			if (random.NextDouble() < 0.2)
			{
				return;
			}
			switch (this.type)
			{
			case 118:
				if (random.NextDouble() < 0.65)
				{
					if (random.NextDouble() < 0.8)
					{
						switch (random.Next(8))
						{
						case 0:
							Game1.createMultipleObjectDebris(382, num, num2, random.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(378, num, num2, random.Next(1, 4));
							return;
						case 2:
							break;
						case 3:
							Game1.createMultipleObjectDebris(390, num, num2, random.Next(2, 6));
							return;
						case 4:
							Game1.createMultipleObjectDebris(388, num, num2, random.Next(2, 3));
							return;
						case 5:
							Game1.createMultipleObjectDebris(92, num, num2, random.Next(2, 4));
							return;
						case 6:
							Game1.createMultipleObjectDebris(388, num, num2, random.Next(2, 6));
							return;
						case 7:
							Game1.createMultipleObjectDebris(390, num, num2, random.Next(2, 6));
							return;
						default:
							return;
						}
					}
					else
					{
						switch (random.Next(4))
						{
						case 0:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						case 2:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						case 3:
							Game1.createMultipleObjectDebris(535, num, num2, random.Next(1, 3));
							return;
						default:
							return;
						}
					}
				}
				else if (random.NextDouble() < 0.4)
				{
					switch (random.Next(5))
					{
					case 0:
						Game1.createMultipleObjectDebris(66, num, num2, 1);
						return;
					case 1:
						Game1.createMultipleObjectDebris(68, num, num2, 1);
						return;
					case 2:
						Game1.createMultipleObjectDebris(709, num, num2, 1);
						return;
					case 3:
						Game1.createMultipleObjectDebris(535, num, num2, 1);
						return;
					case 4:
						Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(level, num, num2), new Vector2((float)num, (float)num2) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), random.Next(4), null);
						return;
					default:
						return;
					}
				}
				break;
			case 119:
			case 121:
			case 123:
				break;
			case 120:
				if (random.NextDouble() < 0.65)
				{
					if (random.NextDouble() < 0.8)
					{
						switch (random.Next(8))
						{
						case 0:
							Game1.createMultipleObjectDebris(382, num, num2, random.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(380, num, num2, random.Next(1, 4));
							return;
						case 2:
							break;
						case 3:
							Game1.createMultipleObjectDebris(378, num, num2, random.Next(2, 6));
							return;
						case 4:
							Game1.createMultipleObjectDebris(388, num, num2, random.Next(2, 6));
							return;
						case 5:
							Game1.createMultipleObjectDebris(92, num, num2, random.Next(2, 4));
							return;
						case 6:
							Game1.createMultipleObjectDebris(390, num, num2, random.Next(2, 4));
							return;
						case 7:
							Game1.createMultipleObjectDebris(390, num, num2, random.Next(2, 6));
							return;
						default:
							return;
						}
					}
					else
					{
						switch (random.Next(4))
						{
						case 0:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(536, num, num2, random.Next(1, 3));
							return;
						case 2:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						case 3:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						default:
							return;
						}
					}
				}
				else if (random.NextDouble() < 4.0)
				{
					switch (random.Next(5))
					{
					case 0:
						Game1.createMultipleObjectDebris(62, num, num2, 1);
						return;
					case 1:
						Game1.createMultipleObjectDebris(70, num, num2, 1);
						return;
					case 2:
						Game1.createMultipleObjectDebris(709, num, num2, random.Next(1, 4));
						return;
					case 3:
						Game1.createMultipleObjectDebris(536, num, num2, 1);
						return;
					case 4:
						Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(level, num, num2), new Vector2((float)num, (float)num2) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), random.Next(4), null);
						return;
					default:
						return;
					}
				}
				break;
			case 122:
			case 124:
				if (random.NextDouble() < 0.65)
				{
					if (random.NextDouble() < 0.8)
					{
						switch (random.Next(8))
						{
						case 0:
							Game1.createMultipleObjectDebris(382, num, num2, random.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(384, num, num2, random.Next(1, 4));
							return;
						case 2:
							break;
						case 3:
							Game1.createMultipleObjectDebris(380, num, num2, random.Next(2, 6));
							return;
						case 4:
							Game1.createMultipleObjectDebris(378, num, num2, random.Next(2, 6));
							return;
						case 5:
							Game1.createMultipleObjectDebris(390, num, num2, random.Next(2, 6));
							return;
						case 6:
							Game1.createMultipleObjectDebris(388, num, num2, random.Next(2, 6));
							return;
						case 7:
							Game1.createMultipleObjectDebris(92, num, num2, random.Next(2, 6));
							return;
						default:
							return;
						}
					}
					else
					{
						switch (random.Next(4))
						{
						case 0:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(537, num, num2, random.Next(1, 3));
							return;
						case 2:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						case 3:
							Game1.createMultipleObjectDebris(78, num, num2, random.Next(1, 3));
							return;
						default:
							return;
						}
					}
				}
				else if (random.NextDouble() < 4.0)
				{
					switch (random.Next(5))
					{
					case 0:
						Game1.createMultipleObjectDebris(60, num, num2, 1);
						return;
					case 1:
						Game1.createMultipleObjectDebris(64, num, num2, 1);
						return;
					case 2:
						Game1.createMultipleObjectDebris(709, num, num2, random.Next(1, 4));
						return;
					case 3:
						Game1.createMultipleObjectDebris(749, num, num2, 1);
						return;
					case 4:
						Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(level, num, num2), new Vector2((float)num, (float)num2) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), random.Next(4), null);
						break;
					default:
						return;
					}
				}
				break;
			default:
				return;
			}
		}

		public override void updateWhenCurrentLocation(GameTime time)
		{
			if (this.shakeTimer > 0)
			{
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			Vector2 vector = base.getScale();
			vector *= (float)Game1.pixelZoom;
			Vector2 vector2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
			Rectangle destinationRectangle = new Rectangle((int)(vector2.X - vector.X / 2f), (int)(vector2.Y - vector.Y / 2f), (int)((float)Game1.tileSize + vector.X), (int)((float)(Game1.tileSize * 2) + vector.Y / 2f));
			if (this.shakeTimer > 0)
			{
				int num = this.shakeTimer / 100 + 1;
				destinationRectangle.X += Game1.random.Next(-num, num + 1);
				destinationRectangle.Y += Game1.random.Next(-num, num + 1);
			}
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.showNextIndex ? (base.ParentSheetIndex + 1) : base.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 1) / 10000f) + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
		}
	}
}
