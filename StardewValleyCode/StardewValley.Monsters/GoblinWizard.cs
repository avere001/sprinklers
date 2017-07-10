using Microsoft.Xna.Framework;
using StardewValley.Projectiles;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	public class GoblinWizard : Monster
	{
		public const int visionDistance = 8;

		public const int spellCooldown = 1500;

		private bool spottedPlayer;

		private bool casting;

		private bool teleporting;

		private int coolDown = 1500;

		private IEnumerator<Point> teleportationPath;

		public GoblinWizard()
		{
		}

		public GoblinWizard(Vector2 position) : this(position, 2)
		{
		}

		public GoblinWizard(Vector2 position, int facingDirection) : base("Goblin Wizard", position, facingDirection)
		{
			this.facingDirection = facingDirection;
			this.faceDirection(facingDirection);
			this.sprite.faceDirection(facingDirection);
			this.moveTowardPlayerThreshold = 8;
			base.IsWalkingTowardPlayer = false;
		}

		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteHeight = Game1.tileSize * 3 / 2;
			this.sprite.UpdateSourceRect();
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - addedPrecision * this.missChance || this.teleporting)
			{
				num = -1;
			}
			else
			{
				this.health -= num;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					this.deathAnimation();
					Game1.playSound("goblinDie");
				}
				else
				{
					if (!this.spottedPlayer)
					{
						this.controller = null;
						this.spottedPlayer = true;
						this.Halt();
						base.facePlayer(Game1.player);
						Game1.playSound("goblinSpot");
						num *= 3;
						Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:GoblinPeasant.cs.12264", new object[0]), Color.Yellow, 3500f));
					}
					Game1.playSound("goblinHurt");
					if (this.casting)
					{
						this.coolDown += 200;
					}
					if (Game1.random.NextDouble() < 0.25)
					{
						this.castTeleport();
					}
				}
			}
			return num;
		}

		public void castTeleport()
		{
			int num = 0;
			Vector2 vector = new Vector2(base.getTileLocation().X + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)), base.getTileLocation().Y + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)));
			while (num < 6 && (!Game1.currentLocation.isTileOnMap(vector) || !Game1.currentLocation.isTileLocationOpen(new Location((int)vector.X, (int)vector.Y)) || Game1.currentLocation.isTileOccupiedForPlacement(vector, null)))
			{
				vector = new Vector2(base.getTileLocation().X + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)), base.getTileLocation().Y + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)));
				num++;
			}
			if (num < 6)
			{
				this.teleporting = true;
				this.teleportationPath = Utility.GetPointsOnLine((int)base.getTileLocation().X, (int)base.getTileLocation().Y, (int)vector.X, (int)vector.Y, true).GetEnumerator();
				this.coolDown = 200;
				Game1.playSound("leafrustle");
			}
		}

		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, Game1.tileSize * 3 / 2 * 5, Game1.tileSize, Game1.tileSize * 3 / 2), (float)Game1.random.Next(150, 200), 4, 0, this.position, false, false));
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.teleporting)
			{
				this.coolDown -= time.ElapsedGameTime.Milliseconds;
				if (this.coolDown <= 0)
				{
					if (this.teleportationPath.MoveNext())
					{
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, this.sprite.SourceRect, this.position, false, 0.05f, Color.Violet));
						this.position = new Vector2((float)(this.teleportationPath.Current.X * Game1.tileSize + 4), (float)(this.teleportationPath.Current.Y * Game1.tileSize - Game1.tileSize / 2 - 4));
					}
					else
					{
						this.teleporting = false;
						this.coolDown = 500;
					}
				}
			}
			else if (!this.spottedPlayer && this.controller == null && Game1.random.NextDouble() < 0.005)
			{
				this.faceDirection(Game1.random.Next(4));
			}
			if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision(this) && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.controller = null;
				this.spottedPlayer = true;
				this.Halt();
				base.facePlayer(Game1.player);
				Game1.playSound("goblinSpot");
				return;
			}
			if (this.casting)
			{
				this.scale = 1f + (float)(1500 - this.coolDown) / 8000f;
				this.coolDown -= time.ElapsedGameTime.Milliseconds;
				if (this.coolDown <= 0)
				{
					this.scale = 1f;
					Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(this.GetBoundingBox().Center, 15f, Game1.player);
					if (Game1.player.addedSpeed >= 0 && Game1.random.NextDouble() < 0.6)
					{
						Game1.currentLocation.projectiles.Add(new DebuffingProjectile(new Buff(12), 0, 2, 2, 0.09817477f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float)this.GetBoundingBox().X, (float)this.GetBoundingBox().Y), null));
					}
					else
					{
						Game1.playSound("fireball");
						Game1.currentLocation.projectiles.Add(new BasicProjectile(8, 1, 0, 5, 0f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float)this.GetBoundingBox().X, (float)this.GetBoundingBox().Y)));
					}
					this.casting = false;
					this.coolDown = 1500;
					return;
				}
			}
			else
			{
				if (this.spottedPlayer && base.withinPlayerThreshold(8))
				{
					if (this.coolDown <= 0 && Game1.random.NextDouble() < 0.02)
					{
						this.casting = true;
						this.Halt();
						this.coolDown = 500;
						if (this.sprite.CurrentFrame < 16)
						{
							this.sprite.CurrentFrame = 16 + this.sprite.CurrentFrame / 4;
						}
					}
					if (this.health < 8)
					{
						if (Math.Abs(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y) > Game1.tileSize * 3)
						{
							if (Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X > 0)
							{
								this.SetMovingLeft(true);
							}
							else
							{
								this.SetMovingRight(true);
							}
						}
						else if (Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y > 0)
						{
							this.SetMovingUp(true);
						}
						else
						{
							this.SetMovingDown(true);
						}
					}
					this.coolDown -= time.ElapsedGameTime.Milliseconds;
					return;
				}
				if (this.spottedPlayer)
				{
					base.IsWalkingTowardPlayer = false;
					this.spottedPlayer = false;
					this.controller = null;
					this.addedSpeed = 0;
				}
			}
		}
	}
}
