using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
	public class GreenSlime : Monster
	{
		public const float mutationFactor = 0.25f;

		public const int matingInterval = 120000;

		public const int childhoodLength = 120000;

		public const int durationOfMating = 2000;

		public const double chanceToMate = 0.001;

		public static int matingRange = Game1.tileSize * 3;

		public bool leftDrift;

		public bool cute = true;

		private int readyToJump = -1;

		private int matingCountdown;

		private new int yOffset;

		private int wagTimer;

		public int readyToMate = 120000;

		public int ageUntilFullGrown;

		public int animateTimer;

		public int timeSinceLastJump;

		public int specialNumber;

		public bool firstGeneration;

		public Color color;

		private GreenSlime mateToPursue;

		private GreenSlime mateToAvoid;

		private Vector2 facePosition;

		private Vector2 faceTargetPosition;

		public GreenSlime()
		{
		}

		public GreenSlime(Vector2 position) : base("Green Slime", position)
		{
			if (Game1.random.NextDouble() < 0.5)
			{
				this.leftDrift = true;
			}
			this.slipperiness = 4;
			this.readyToMate = Game1.random.Next(1000, 120000);
			int num = Game1.random.Next(200, 256);
			this.color = new Color(num / Game1.random.Next(2, 10), Game1.random.Next(180, 256), (Game1.random.NextDouble() < 0.1) ? 255 : (255 - num));
			this.firstGeneration = true;
			this.flip = (Game1.random.NextDouble() < 0.5);
			this.cute = (Game1.random.NextDouble() < 0.49);
			this.hideShadow = true;
		}

		public GreenSlime(Vector2 position, int mineLevel) : base("Green Slime", position)
		{
			this.cute = (Game1.random.NextDouble() < 0.49);
			this.flip = (Game1.random.NextDouble() < 0.5);
			this.specialNumber = Game1.random.Next(100);
			if (mineLevel < 40)
			{
				base.parseMonsterInfo("Green Slime");
				int num = Game1.random.Next(200, 256);
				this.color = new Color(num / Game1.random.Next(2, 10), num, (Game1.random.NextDouble() < 0.01) ? 255 : (255 - num));
				if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
				{
					this.color = new Color(205, 255, 0) * 0.7f;
					this.hasSpecialItem = true;
					this.health *= 3;
					this.damageToFarmer *= 2;
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(680);
				}
			}
			else if (mineLevel < 80)
			{
				this.name = "Frost Jelly";
				base.parseMonsterInfo("Frost Jelly");
				int num2 = Game1.random.Next(200, 256);
				this.color = new Color((Game1.random.NextDouble() < 0.01) ? 180 : (num2 / Game1.random.Next(2, 10)), (Game1.random.NextDouble() < 0.1) ? 255 : (255 - num2 / 3), num2);
				if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
				{
					this.color = new Color(0, 0, 0) * 0.7f;
					this.hasSpecialItem = true;
					this.health *= 3;
					this.damageToFarmer *= 2;
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(413);
				}
			}
			else if (mineLevel > 120)
			{
				this.name = "Sludge";
				base.parseMonsterInfo("Sludge");
				this.color = Color.BlueViolet;
				this.health *= 2;
				int num3 = (int)this.color.R;
				int num4 = (int)this.color.G;
				int num5 = (int)this.color.B;
				num3 += Game1.random.Next(-20, 21);
				num4 += Game1.random.Next(-20, 21);
				num5 += Game1.random.Next(-20, 21);
				this.color.R = (byte)Math.Max(Math.Min(255, num3), 0);
				this.color.G = (byte)Math.Max(Math.Min(255, num4), 0);
				this.color.B = (byte)Math.Max(Math.Min(255, num5), 0);
				while (Game1.random.NextDouble() < 0.08)
				{
					this.objectsToDrop.Add(386);
				}
				if (Game1.random.NextDouble() < 0.009)
				{
					this.objectsToDrop.Add(337);
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(439);
				}
			}
			else
			{
				this.name = "Sludge";
				base.parseMonsterInfo("Sludge");
				int num6 = Game1.random.Next(200, 256);
				this.color = new Color(num6, (Game1.random.NextDouble() < 0.01) ? 255 : (255 - num6), num6 / Game1.random.Next(2, 10));
				if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
				{
					this.color = new Color(50, 10, 50) * 0.7f;
					this.hasSpecialItem = true;
					this.health *= 3;
					this.damageToFarmer *= 2;
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(437);
				}
			}
			if (this.cute)
			{
				this.health += this.health / 4;
				this.damageToFarmer++;
			}
			if (Game1.random.NextDouble() < 0.5)
			{
				this.leftDrift = true;
			}
			this.slipperiness = 3;
			this.readyToMate = Game1.random.Next(1000, 120000);
			if (Game1.random.NextDouble() < 0.001)
			{
				this.color = new Color(255, 255, 50);
				this.coinsToDrop = 10;
			}
			this.firstGeneration = true;
			this.hideShadow = true;
		}

		public GreenSlime(Vector2 position, Color color) : base("Green Slime", position)
		{
			this.color = color;
			this.firstGeneration = true;
			this.hideShadow = true;
		}

		public override void reloadSprite()
		{
			this.hideShadow = true;
			string name = this.name;
			this.name = "Green Slime";
			base.reloadSprite();
			this.name = name;
			this.sprite.spriteHeight = 24;
			this.sprite.UpdateSourceRect();
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				if (Game1.random.NextDouble() < 0.025 && this.cute)
				{
					if (!this.focusedOnFarmers)
					{
						this.damageToFarmer += this.damageToFarmer / 2;
						base.shake(1000);
					}
					this.focusedOnFarmers = true;
				}
				this.slipperiness = 3;
				this.health -= num;
				base.setTrajectory(xTrajectory, yTrajectory);
				Game1.playSound("slimeHit");
				this.readyToJump = -1;
				base.IsWalkingTowardPlayer = true;
				if (this.health <= 0)
				{
					Game1.playSound("slimedead");
					Stats expr_CE = Game1.stats;
					uint slimesKilled = expr_CE.SlimesKilled;
					expr_CE.SlimesKilled = slimesKilled + 1u;
					if (this.mateToPursue != null)
					{
						this.mateToPursue.mateToAvoid = null;
					}
					if (this.mateToAvoid != null)
					{
						this.mateToAvoid.mateToPursue = null;
					}
					if (Game1.gameMode == 3 && this.scale > 1.8f)
					{
						this.health = 10;
						int num2 = (this.scale > 1.8f) ? Game1.random.Next(3, 5) : 1;
						this.scale *= 0.6666667f;
						for (int i = 0; i < num2; i++)
						{
							Game1.currentLocation.characters.Add(new GreenSlime(this.position + new Vector2((float)(i * this.GetBoundingBox().Width), 0f), Game1.mine.mineLevel));
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].setTrajectory(xTrajectory + Game1.random.Next(-20, 20), yTrajectory + Game1.random.Next(-20, 20));
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].willDestroyObjectsUnderfoot = false;
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].moveTowardPlayer(4);
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].scale = 0.75f + (float)Game1.random.Next(-5, 10) / 100f;
						}
					}
					else
					{
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							delayBeforeAnimationStart = 0,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2(0f, (float)(Game1.tileSize / 4)), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							delayBeforeAnimationStart = 100,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)(Game1.tileSize / 4), 0f), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							delayBeforeAnimationStart = 200,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
					}
				}
			}
			return num;
		}

		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 120, 16, 16), 8, this.GetBoundingBox().Center.X + Game1.tileSize / 2, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, this.color, 4f * scale);
		}

		public override void collisionWithFarmerBehavior()
		{
			if (Game1.random.NextDouble() < 0.3 && !Game1.player.temporarilyInvincible && !Game1.player.isWearingRing(520) && Game1.buffsDisplay.addOtherBuff(new Buff(13)))
			{
				Game1.playSound("slime");
			}
			this.farmerPassesThrough = Game1.player.isWearingRing(520);
		}

		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yOffset)), new Rectangle?(base.Sprite.SourceRect), this.color, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				if (this.ageUntilFullGrown <= 0)
				{
					if (this.cute || this.hasSpecialItem)
					{
						int x = (this.isMoving() || this.wagTimer > 0) ? (16 * Math.Min(7, Math.Abs(((this.wagTimer > 0) ? (992 - this.wagTimer) : (Game1.currentGameTime.TotalGameTime.Milliseconds % 992)) - 496) / 62) % 64) : 48;
						int num = (this.isMoving() || this.wagTimer > 0) ? (24 * Math.Min(1, Math.Max(1, Math.Abs(((this.wagTimer > 0) ? (992 - this.wagTimer) : (Game1.currentGameTime.TotalGameTime.Milliseconds % 992)) - 496) / 62) / 4)) : 24;
						if (this.hasSpecialItem)
						{
							num += 48;
						}
						b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height - Game1.pixelZoom * 4 + ((this.readyToJump <= 0) ? (Game1.pixelZoom * (-2 + Math.Abs(this.sprite.currentFrame % 4 - 2))) : (Game1.pixelZoom + Game1.pixelZoom * (this.sprite.currentFrame % 4 % 3))) + this.yOffset)) * this.scale, new Rectangle?(new Rectangle(x, 168 + num, 16, 24)), this.hasSpecialItem ? Color.White : this.color, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f + 0.0001f)));
					}
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + (new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + ((this.readyToJump <= 0) ? (Game1.pixelZoom * (-2 + Math.Abs(this.sprite.currentFrame % 4 - 2))) : (Game1.pixelZoom - Game1.pixelZoom * (this.sprite.currentFrame % 4 % 3))) + this.yOffset)) + this.facePosition) * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), new Rectangle?(new Rectangle(32 + ((this.readyToJump > 0 || this.focusedOnFarmers) ? 16 : 0), 120 + ((this.readyToJump < 0 && (this.focusedOnFarmers || this.invincibleCountdown > 0)) ? 24 : 0), 16, 24)), Color.White * ((this.facingDirection == 0) ? 0.5f : 1f), 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f + 0.0001f)));
				}
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yOffset)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale), SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
				if (this.mateToPursue != null)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2 + this.yOffset)), new Rectangle?(new Rectangle(16, 120, 8, 8)), Color.White, 0f, new Vector2(3f, 3f), (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				}
				else if (this.mateToAvoid != null)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2 + this.yOffset)), new Rectangle?(new Rectangle(24, 120, 8, 8)), Color.White, 0f, new Vector2(4f, 4f), (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				}
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 * 7) / 4f + (float)this.yOffset + (float)(Game1.pixelZoom * 2) * this.scale - (float)((this.ageUntilFullGrown > 0) ? (Game1.pixelZoom * 2) : 0)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + this.scale - (float)this.ageUntilFullGrown / 120000f - ((base.Sprite.CurrentFrame % 4 % 3 != 0) ? 1f : 0f) + (float)this.yOffset / 30f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
			}
		}

		public void moveTowardOtherSlime(GreenSlime other, bool moveAway, GameTime time)
		{
			int num = Math.Abs(other.getStandingX() - base.getStandingX());
			int num2 = Math.Abs(other.getStandingY() - base.getStandingY());
			if (num > 4 || num2 > 4)
			{
				int num3 = (other.getStandingX() > base.getStandingX()) ? 1 : -1;
				int num4 = (other.getStandingY() > base.getStandingY()) ? 1 : -1;
				if (moveAway)
				{
					num3 = -num3;
					num4 = -num4;
				}
				double num5 = (double)num / (double)(num + num2);
				if (Game1.random.NextDouble() < num5)
				{
					base.tryToMoveInDirection((num3 > 0) ? 1 : 3, false, this.damageToFarmer, false);
				}
				else
				{
					base.tryToMoveInDirection((num4 > 0) ? 2 : 0, false, this.damageToFarmer, false);
				}
			}
			this.sprite.AnimateDown(time, 0, "");
			if (this.invincibleCountdown > 0)
			{
				this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.invincibleCountdown <= 0)
				{
					base.stopGlowing();
				}
			}
		}

		public void doneMating()
		{
			this.readyToMate = 120000;
			this.matingCountdown = 2000;
			this.mateToPursue = null;
			this.mateToAvoid = null;
		}

		public override void update(GameTime time, GameLocation location)
		{
			if (this.mateToPursue == null && this.mateToAvoid == null)
			{
				base.update(time, location);
				return;
			}
			if (this.currentLocation == null)
			{
				this.currentLocation = location;
			}
			this.behaviorAtGameTick(time);
		}

		public override void noMovementProgressNearPlayerBehavior()
		{
			base.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(base.getTileLocation()).getStandingPosition(), 0);
		}

		public void mateWith(GreenSlime mateToPursue, GameLocation location)
		{
			if (location.canSlimeMateHere())
			{
				GreenSlime greenSlime = new GreenSlime(Vector2.Zero);
				Utility.recursiveFindPositionForCharacter(greenSlime, location, base.getTileLocation(), 30);
				Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 10) + (uint)((int)(this.scale * 100f)) + (uint)((int)(mateToPursue.scale * 100f))));
				switch (random.Next(4))
				{
				case 0:
					greenSlime.color = new Color(Math.Min(255, Math.Max(0, (int)this.color.R + random.Next((int)((float)(-(float)this.color.R) * 0.25f), (int)((float)this.color.R * 0.25f)))), Math.Min(255, Math.Max(0, (int)this.color.G + random.Next((int)((float)(-(float)this.color.G) * 0.25f), (int)((float)this.color.G * 0.25f)))), Math.Min(255, Math.Max(0, (int)this.color.B + random.Next((int)((float)(-(float)this.color.B) * 0.25f), (int)((float)this.color.B * 0.25f)))));
					break;
				case 1:
				case 2:
					greenSlime.color = Utility.getBlendedColor(this.color, mateToPursue.color);
					break;
				case 3:
					greenSlime.color = new Color(Math.Min(255, Math.Max(0, (int)mateToPursue.color.R + random.Next((int)((float)(-(float)mateToPursue.color.R) * 0.25f), (int)((float)mateToPursue.color.R * 0.25f)))), Math.Min(255, Math.Max(0, (int)mateToPursue.color.G + random.Next((int)((float)(-(float)mateToPursue.color.G) * 0.25f), (int)((float)mateToPursue.color.G * 0.25f)))), Math.Min(255, Math.Max(0, (int)mateToPursue.color.B + random.Next((int)((float)(-(float)mateToPursue.color.B) * 0.25f), (int)((float)mateToPursue.color.B * 0.25f)))));
					break;
				}
				int r = (int)greenSlime.color.R;
				int g = (int)greenSlime.color.G;
				int b = (int)greenSlime.color.B;
				if (r > 100 && b > 100 && g < 50)
				{
					greenSlime.parseMonsterInfo("Sludge");
					while (random.NextDouble() < 0.1)
					{
						greenSlime.objectsToDrop.Add(386);
					}
					if (random.NextDouble() < 0.01)
					{
						greenSlime.objectsToDrop.Add(337);
					}
				}
				else if (r >= 200 && g < 75)
				{
					greenSlime.parseMonsterInfo("Sludge");
				}
				else if (b >= 200 && r < 100)
				{
					greenSlime.parseMonsterInfo("Frost Jelly");
				}
				greenSlime.health = ((random.NextDouble() < 0.5) ? this.health : mateToPursue.health);
				greenSlime.health = Math.Max(1, this.health + random.Next(-4, 5));
				greenSlime.damageToFarmer = ((random.NextDouble() < 0.5) ? this.damageToFarmer : mateToPursue.damageToFarmer);
				greenSlime.damageToFarmer = Math.Max(0, this.damageToFarmer + random.Next(-1, 2));
				greenSlime.resilience = ((random.NextDouble() < 0.5) ? this.resilience : mateToPursue.resilience);
				greenSlime.resilience = Math.Max(0, this.resilience + random.Next(-1, 2));
				greenSlime.missChance = ((random.NextDouble() < 0.5) ? this.missChance : mateToPursue.missChance);
				greenSlime.missChance = Math.Max(0.0, this.missChance + (double)((float)random.Next(-1, 2) / 100f));
				greenSlime.scale = ((random.NextDouble() < 0.5) ? this.scale : mateToPursue.scale);
				greenSlime.scale = Math.Max(0.6f, Math.Min(1.5f, this.scale + (float)random.Next(-2, 3) / 100f));
				greenSlime.slipperiness = 8;
				this.speed = ((random.NextDouble() < 0.5) ? this.speed : mateToPursue.speed);
				if (random.NextDouble() < 0.015)
				{
					this.speed = Math.Max(1, Math.Min(6, this.speed + random.Next(-1, 2)));
				}
				greenSlime.setTrajectory(Utility.getAwayFromPositionTrajectory(greenSlime.GetBoundingBox(), base.getStandingPosition()) / 2f);
				greenSlime.ageUntilFullGrown = 120000;
				greenSlime.Halt();
				greenSlime.firstGeneration = false;
				if (Utility.isOnScreen(this.position, 128))
				{
					Game1.playSound("slime");
				}
			}
			mateToPursue.doneMating();
			this.doneMating();
		}

		public override List<Item> getExtraDropItems()
		{
			List<Item> list = new List<Item>();
			if (this.color.R < 80 && this.color.G < 80 && this.color.B < 80)
			{
				list.Add(new StardewValley.Object(382, 1, false, -1, 0));
				Random expr_82 = new Random((int)this.position.X * 777 + (int)this.position.Y * 77 + (int)Game1.stats.DaysPlayed);
				if (expr_82.NextDouble() < 0.05)
				{
					list.Add(new StardewValley.Object(553, 1, false, -1, 0));
				}
				if (expr_82.NextDouble() < 0.05)
				{
					list.Add(new StardewValley.Object(539, 1, false, -1, 0));
				}
			}
			else if (this.color.R > 200 && this.color.G > 180 && this.color.B < 50)
			{
				list.Add(new StardewValley.Object(384, 2, false, -1, 0));
			}
			else if (this.color.R > 220 && this.color.G > 90 && this.color.G < 150 && this.color.B < 50)
			{
				list.Add(new StardewValley.Object(378, 2, false, -1, 0));
			}
			else if (this.color.R > 230 && this.color.G > 230 && this.color.B > 230)
			{
				list.Add(new StardewValley.Object(380, 1, false, -1, 0));
				if (this.color.R % 2 == 0 && this.color.G % 2 == 0 && this.color.B % 2 == 0)
				{
					list.Add(new StardewValley.Object(72, 1, false, -1, 0));
				}
			}
			else if (this.color.R > 150 && this.color.G > 150 && this.color.B > 150)
			{
				list.Add(new StardewValley.Object(390, 2, false, -1, 0));
			}
			else if (this.color.R > 150 && this.color.B > 180 && this.color.G < 50 && this.specialNumber % 4 == 0)
			{
				list.Add(new StardewValley.Object(386, 2, false, -1, 0));
			}
			if (Game1.player.mailReceived.Contains("slimeHutchBuilt") && this.specialNumber == 1)
			{
				string name = this.name;
				if (!(name == "Green Slime"))
				{
					if (name == "Frost Jelly")
					{
						list.Add(new StardewValley.Object(413, 1, false, -1, 0));
					}
				}
				else
				{
					list.Add(new StardewValley.Object(680, 1, false, -1, 0));
				}
			}
			return list;
		}

		public override void dayUpdate(int dayOfMonth)
		{
			if (this.ageUntilFullGrown > 0)
			{
				this.ageUntilFullGrown /= 2;
			}
			if (this.readyToMate > 0)
			{
				this.readyToMate /= 2;
			}
			base.dayUpdate(dayOfMonth);
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			if (this.wagTimer > 0)
			{
				this.wagTimer -= (int)time.ElapsedGameTime.TotalMilliseconds;
			}
			switch (base.FacingDirection)
			{
			case 0:
				if (this.facePosition.X > 0f)
				{
					this.facePosition.X = this.facePosition.X - 2f;
				}
				else if (this.facePosition.X < 0f)
				{
					this.facePosition.X = this.facePosition.X + 2f;
				}
				if (this.facePosition.Y > (float)(-(float)Game1.pixelZoom * 2))
				{
					this.facePosition.Y = this.facePosition.Y - 2f;
				}
				break;
			case 1:
				if (this.facePosition.X < (float)(Game1.pixelZoom * 2))
				{
					this.facePosition.X = this.facePosition.X + 2f;
				}
				if (this.facePosition.Y < 0f)
				{
					this.facePosition.Y = this.facePosition.Y + 2f;
				}
				break;
			case 2:
				if (this.facePosition.X > 0f)
				{
					this.facePosition.X = this.facePosition.X - 2f;
				}
				else if (this.facePosition.X < 0f)
				{
					this.facePosition.X = this.facePosition.X + 2f;
				}
				if (this.facePosition.Y < 0f)
				{
					this.facePosition.Y = this.facePosition.Y + 2f;
				}
				break;
			case 3:
				if (this.facePosition.X > (float)(-(float)Game1.pixelZoom * 2))
				{
					this.facePosition.X = this.facePosition.X - 2f;
				}
				if (this.facePosition.Y < 0f)
				{
					this.facePosition.Y = this.facePosition.Y + 2f;
				}
				break;
			}
			if (this.ageUntilFullGrown <= 0)
			{
				this.readyToMate -= time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.ageUntilFullGrown -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.mateToPursue != null)
			{
				if (this.readyToMate <= -35000)
				{
					this.mateToPursue.doneMating();
					this.doneMating();
					return;
				}
				this.moveTowardOtherSlime(this.mateToPursue, false, time);
				if (this.mateToPursue.mateToAvoid == null && this.mateToPursue.mateToPursue != null && !this.mateToPursue.mateToPursue.Equals(this))
				{
					this.doneMating();
					return;
				}
				if (Vector2.Distance(base.getStandingPosition(), this.mateToPursue.getStandingPosition()) < (float)(this.GetBoundingBox().Width + 4))
				{
					if (this.mateToPursue.mateToAvoid != null && this.mateToPursue.mateToAvoid.Equals(this))
					{
						this.mateToPursue.mateToAvoid = null;
						this.mateToPursue.matingCountdown = 2000;
						this.mateToPursue.mateToPursue = this;
					}
					this.matingCountdown -= time.ElapsedGameTime.Milliseconds;
					if (this.currentLocation != null && this.matingCountdown <= 0 && this.mateToPursue != null && (!this.currentLocation.isOutdoors || Utility.getNumberOfCharactersInRadius(this.currentLocation, Utility.Vector2ToPoint(this.position), 1) <= 4))
					{
						this.mateWith(this.mateToPursue, this.currentLocation);
						return;
					}
				}
				else if (Vector2.Distance(base.getStandingPosition(), this.mateToPursue.getStandingPosition()) > (float)(GreenSlime.matingRange * 2))
				{
					this.mateToPursue.mateToAvoid = null;
					this.mateToPursue = null;
					return;
				}
			}
			else
			{
				if (this.mateToAvoid != null)
				{
					this.moveTowardOtherSlime(this.mateToAvoid, true, time);
					return;
				}
				if (this.readyToMate < 0 && this.cute)
				{
					this.readyToMate = -1;
					if (Game1.random.NextDouble() < 0.001)
					{
						GreenSlime greenSlime = (GreenSlime)Utility.checkForCharacterWithinArea(base.GetType(), this.position, Game1.currentLocation, new Rectangle(base.getStandingX() - GreenSlime.matingRange, base.getStandingY() - GreenSlime.matingRange, GreenSlime.matingRange * 2, GreenSlime.matingRange * 2));
						if (greenSlime != null && greenSlime.readyToMate <= 0 && !greenSlime.cute)
						{
							this.matingCountdown = 2000;
							this.mateToPursue = greenSlime;
							greenSlime.mateToAvoid = this;
							this.addedSpeed = 1;
							this.mateToPursue.addedSpeed = 1;
							return;
						}
					}
				}
				else if (!this.isGlowing)
				{
					this.addedSpeed = 0;
				}
				this.yOffset = Math.Max(this.yOffset - (int)Math.Abs(this.xVelocity + this.yVelocity) / 2, -Game1.tileSize);
				base.behaviorAtGameTick(time);
				if (this.yOffset < 0)
				{
					this.yOffset = Math.Min(0, this.yOffset + 4 + (int)((this.yOffset <= -Game1.tileSize) ? ((float)(-(float)this.yOffset) / 8f) : ((float)(-(float)this.yOffset) / 16f)));
				}
				this.timeSinceLastJump += time.ElapsedGameTime.Milliseconds;
				if (this.readyToJump != -1)
				{
					this.Halt();
					base.IsWalkingTowardPlayer = false;
					this.readyToJump -= time.ElapsedGameTime.Milliseconds;
					this.sprite.CurrentFrame = 16 + (800 - this.readyToJump) / 200;
					if (this.readyToJump <= 0)
					{
						this.timeSinceLastJump = this.timeSinceLastJump;
						this.slipperiness = 10;
						base.IsWalkingTowardPlayer = true;
						this.readyToJump = -1;
						if (Utility.isOnScreen(this.position, 128))
						{
							Game1.playSound("slime");
						}
						Vector2 awayFromPlayerTrajectory = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox());
						awayFromPlayerTrajectory.X = -awayFromPlayerTrajectory.X / 2f;
						awayFromPlayerTrajectory.Y = -awayFromPlayerTrajectory.Y / 2f;
						base.setTrajectory((int)awayFromPlayerTrajectory.X, (int)awayFromPlayerTrajectory.Y);
						this.sprite.CurrentFrame = 1;
						this.invincibleCountdown = 0;
					}
				}
				else if (Game1.random.NextDouble() < 0.1 && !this.focusedOnFarmers)
				{
					if (this.facingDirection == 0 || this.facingDirection == 2)
					{
						if (this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, 1, false, this))
						{
							this.position.X = this.position.X - (float)this.speed;
						}
						else if (!this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, 1, false, this))
						{
							this.position.X = this.position.X + (float)this.speed;
						}
					}
					else if (this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, 1, false, this))
					{
						this.position.Y = this.position.Y - (float)this.speed;
					}
					else if (!this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, 1, false, this))
					{
						this.position.Y = this.position.Y + (float)this.speed;
					}
					if (Game1.random.NextDouble() < 0.08)
					{
						this.leftDrift = !this.leftDrift;
					}
				}
				else if (this.withinPlayerThreshold() && this.timeSinceLastJump > (this.focusedOnFarmers ? 1000 : 4000) && Game1.random.NextDouble() < 0.01)
				{
					if (this.name.Equals("Frost Jelly") && Game1.random.NextDouble() < 0.25)
					{
						this.addedSpeed = 2;
						base.startGlowing(Color.Cyan, false, 0.15f);
					}
					else
					{
						this.addedSpeed = 0;
						base.stopGlowing();
						this.readyToJump = 800;
					}
				}
				if (Game1.random.NextDouble() < 0.01 && this.wagTimer <= 0)
				{
					this.wagTimer = 992;
				}
				if (Math.Abs(this.xVelocity) >= 0.5f || Math.Abs(this.yVelocity) >= 0.5f)
				{
					this.sprite.AnimateDown(time, 0, "");
				}
				else if (!this.position.Equals(this.lastPosition))
				{
					this.animateTimer = 500;
				}
				if (this.animateTimer > 0 && this.readyToJump <= 0)
				{
					this.animateTimer -= time.ElapsedGameTime.Milliseconds;
					this.sprite.AnimateDown(time, 0, "");
				}
			}
		}
	}
}
