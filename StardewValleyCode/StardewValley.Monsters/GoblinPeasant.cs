using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
	public class GoblinPeasant : Monster
	{
		public const int durationOfDaggerThrust = 200;

		public const int huntForPlayerUpdateTicks = 2000;

		public const int distanceToStopChasing = 13;

		public const int visionDistance = 8;

		public Rectangle hallway = Rectangle.Empty;

		private bool spottedPlayer;

		private bool actionGoblin;

		private bool attacking;

		private int controllerCountdown;

		private int actionCountdown;

		private MeleeWeapon weapon = new MeleeWeapon((Game1.random.NextDouble() < 0.01) ? 22 : 16);

		private Color clothesColor;

		private Color gogglesColor;

		private Color skinColor;

		public GoblinPeasant()
		{
			this.pickColors();
		}

		public GoblinPeasant(Vector2 position) : this(position, 2)
		{
		}

		public GoblinPeasant(Vector2 position, int facingDir, Rectangle hallway) : base("Goblin Warrior", position, facingDir)
		{
			this.facingDirection = facingDir;
			this.faceDirection(facingDir);
			this.sprite.faceDirection(facingDir);
			this.hallway = new Rectangle(hallway.X * Game1.tileSize, hallway.Y * Game1.tileSize, hallway.Width * Game1.tileSize, hallway.Height * Game1.tileSize);
			this.hallway.Inflate(Game1.tileSize / 2, Game1.tileSize / 2);
			this.moveTowardPlayerThreshold = 8;
			base.IsWalkingTowardPlayer = false;
			this.pickColors();
		}

		public GoblinPeasant(Vector2 position, int facingDirection) : this(position, facingDirection, false)
		{
			this.pickColors();
		}

		public GoblinPeasant(Vector2 position, int facingDir, bool actionGoblin) : base("Goblin Warrior", position, facingDir)
		{
			this.facingDirection = facingDir;
			base.IsWalkingTowardPlayer = false;
			this.faceDirection(facingDir);
			this.sprite.faceDirection(facingDir);
			this.moveTowardPlayerThreshold = 8;
			this.actionGoblin = actionGoblin;
			this.pickColors();
		}

		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteHeight = Game1.tileSize * 3 / 2;
			this.sprite.UpdateSourceRect();
		}

		private void pickColors()
		{
			this.clothesColor = new Color(Game1.random.Next(80, 256), Game1.random.Next(80, 256), Game1.random.Next(80, 256));
			this.gogglesColor = new Color(Game1.random.Next(50, 256), Game1.random.Next(50, 256), Game1.random.Next(50, 256));
			this.skinColor = new Color(Game1.random.Next(100, (Game1.random.NextDouble() < 0.08) ? 256 : 150), Game1.random.Next(100, (Game1.random.NextDouble() < 0.08) ? 256 : 190), Game1.random.Next(100, (Game1.random.NextDouble() < 0.08) ? 256 : 190));
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
				if (!this.spottedPlayer)
				{
					this.controller = null;
					this.spottedPlayer = true;
					this.Halt();
					base.facePlayer(Game1.player);
					base.doEmote(16, false);
					Game1.playSound("goblinSpot");
					base.IsWalkingTowardPlayer = true;
					num *= 3;
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:GoblinPeasant.cs.12264", new object[0]), Color.Yellow, 3500f));
				}
				this.health -= num;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					this.deathAnimation();
					Game1.playSound("goblinDie");
					if (this.weapon.indexOfMenuItemView == 22 || Game1.random.NextDouble() < 0.18)
					{
						Game1.currentLocation.debris.Add(new Debris(this.weapon, new Vector2((float)this.GetBoundingBox().Center.X, (float)this.GetBoundingBox().Center.Y)));
					}
				}
				else
				{
					Game1.playSound("goblinHurt");
				}
			}
			return num;
		}

		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, Game1.tileSize * 3 / 2 * 5, Game1.tileSize, Game1.tileSize * 3 / 2), (float)Game1.random.Next(150, 200), 4, 0, this.position, false, false));
		}

		public void meetUpWithNeighborEndFunction(Character c, GameLocation location)
		{
			List<Vector2> arg_0D_0 = Utility.getAdjacentTileLocations(base.getTileLocation());
			bool flag = false;
			foreach (Vector2 current in arg_0D_0)
			{
				foreach (Character current2 in location.characters)
				{
					if (current2.getTileLocation().Equals(current))
					{
						current2.faceGeneralDirection(base.getTileLocation(), 0);
						base.faceGeneralDirection(current, 0);
						flag = true;
					}
				}
			}
			if (flag)
			{
				base.doEmote((Game1.random.NextDouble() < 0.5) ? 20 : ((Game1.random.NextDouble() < 0.5) ? 32 : 8), true);
			}
		}

		private void tryToMeetUpWithNeighbor()
		{
			Character character = Game1.currentLocation.characters[Game1.random.Next(Game1.currentLocation.characters.Count)];
			if (!this.Equals(character) && character.controller != null && character is GoblinPeasant && !((GoblinPeasant)character).hallway.Equals(Rectangle.Empty))
			{
				Vector2 randomAdjacentOpenTile = Utility.getRandomAdjacentOpenTile(character.getTileLocation());
				if (!randomAdjacentOpenTile.Equals(Vector2.Zero))
				{
					this.controller = new PathFindController(this, Game1.currentLocation, new Point((int)randomAdjacentOpenTile.X, (int)randomAdjacentOpenTile.Y), Game1.random.Next(4), new PathFindController.endBehavior(this.meetUpWithNeighborEndFunction));
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))), (float)this.GetBoundingBox().Center.Y / 10000f, 0, 0, this.skinColor, false, (float)Game1.pixelZoom, 0f, false);
			if (this.attacking)
			{
				this.weapon.drawDuringUse((200 - this.actionCountdown) / 100, base.FacingDirection, b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))) + new Vector2((float)((base.FacingDirection == 1) ? -16 : ((base.FacingDirection == 3) ? 16 : 0)), (float)(Game1.tileSize + ((base.FacingDirection == 1 || base.FacingDirection == 3) ? (Game1.tileSize / 2) : 0))), Game1.player);
			}
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))), (float)this.GetBoundingBox().Center.Y / 10000f + 1E-05f, 0, 144, this.clothesColor, false, (float)Game1.pixelZoom, 0f, false);
			if (this.clothesColor.R % 2 == 0 || this.clothesColor.G % 2 == 0 || this.clothesColor.B % 2 == 0)
			{
				this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))), (float)this.GetBoundingBox().Center.Y / 10000f + 1E-06f, 0, 264, this.gogglesColor, false, (float)Game1.pixelZoom, 0f, false);
			}
		}

		public override void update(GameTime time, GameLocation location)
		{
			if (!this.attacking)
			{
				base.update(time, location);
				return;
			}
			this.behaviorAtGameTick(time);
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (!this.spottedPlayer && this.controller == null)
			{
				if (this.actionGoblin)
				{
					this.actionCountdown -= time.ElapsedGameTime.Milliseconds;
					if (this.actionCountdown <= 0)
					{
						if (this.sprite.CurrentFrame >= 16)
						{
							this.actionCountdown = Game1.random.Next(500, 2000);
							this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 16) * 4;
							this.sprite.UpdateSourceRect();
						}
						else
						{
							this.sprite.CurrentFrame = 16 + ((this.facingDirection % 2 == 0) ? ((this.facingDirection + 2) % 4) : this.facingDirection);
							this.sprite.UpdateSourceRect();
							this.actionCountdown = Game1.random.Next(100, 500);
							if (Utility.isOnScreen(this.position, Game1.tileSize * 4))
							{
								Game1.playSound("hammer");
								Game1.createRadialDebris(Game1.currentLocation, 14, (int)base.getTileLocation().X + ((this.facingDirection == 1) ? 1 : ((this.facingDirection == 3) ? -1 : 0)), (int)base.getTileLocation().Y + ((this.facingDirection == 0) ? -1 : ((this.facingDirection == 2) ? 1 : 0)), Game1.random.Next(4, 6), false, -1, false, -1);
							}
						}
					}
				}
				else if (!this.hallway.Equals(Rectangle.Empty))
				{
					if (this.hallway.Contains(this.GetBoundingBox().Center))
					{
						if (this.hallway.Height > this.hallway.Width && (this.facingDirection == 1 || this.facingDirection == 3))
						{
							this.facingDirection = ((Game1.random.NextDouble() < 0.5) ? 0 : 2);
						}
						else if (this.hallway.Width > this.hallway.Height && (this.facingDirection == 0 || this.facingDirection == 2))
						{
							this.facingDirection = ((Game1.random.NextDouble() < 0.5) ? 1 : 3);
						}
						base.setMovingInFacingDirection();
					}
					else
					{
						this.controller = new PathFindController(this, Game1.currentLocation, new Point(this.hallway.Center.X / Game1.tileSize, this.hallway.Center.Y / Game1.tileSize), Game1.random.Next(4));
					}
					if (Game1.currentLocation.isCollidingPosition(this.nextPosition(this.facingDirection), Game1.viewport, false, 0, false, this))
					{
						this.faceDirection((this.facingDirection + 2) % 4);
						base.setMovingInFacingDirection();
						this.MovePosition(time, Game1.viewport, Game1.currentLocation);
						this.MovePosition(time, Game1.viewport, Game1.currentLocation);
					}
				}
				else if (Game1.random.NextDouble() < 0.0025)
				{
					this.tryToMeetUpWithNeighbor();
				}
				else
				{
					Game1.random.NextDouble();
				}
			}
			if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision(this) && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.controller = null;
				this.spottedPlayer = true;
				this.Halt();
				base.facePlayer(Game1.player);
				base.doEmote(16, false);
				Game1.playSound("goblinSpot");
				base.IsWalkingTowardPlayer = true;
				this.actionGoblin = false;
				return;
			}
			if ((this.spottedPlayer && base.withinPlayerThreshold(13)) || this.attacking)
			{
				if (base.withinPlayerThreshold(2) || this.attacking)
				{
					if (!this.attacking && Game1.random.NextDouble() < 0.04)
					{
						this.actionCountdown = 200;
						this.attacking = true;
						Game1.playSound("daggerswipe");
						Vector2 zero = Vector2.Zero;
						Vector2 zero2 = Vector2.Zero;
						Rectangle areaOfEffect = this.weapon.getAreaOfEffect((int)this.GetToolLocation(false).X, (int)this.GetToolLocation(false).Y, base.getFacingDirection(), ref zero, ref zero2, this.GetBoundingBox(), 0);
						if (this.facingDirection == 1 || this.facingDirection == 3)
						{
							areaOfEffect.Inflate(-12, -12);
						}
						if (areaOfEffect.Intersects(Game1.player.GetBoundingBox()))
						{
							int health = Game1.player.health;
							Game1.farmerTakeDamage(Game1.random.Next(this.weapon.minDamage, this.weapon.maxDamage + 1), false, this);
							if (Game1.player.health == health)
							{
								base.setTrajectory(Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox()) / 2f);
								return;
							}
						}
					}
					else if (this.attacking)
					{
						this.Halt();
						int currentFrame = 16 + ((this.facingDirection == 0) ? 2 : ((this.facingDirection == 2) ? 0 : this.facingDirection));
						this.sprite.CurrentFrame = currentFrame;
						this.actionCountdown -= time.ElapsedGameTime.Milliseconds;
						if (this.actionCountdown <= 0)
						{
							this.attacking = false;
							this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 16) * 4;
							return;
						}
					}
				}
			}
			else if (this.spottedPlayer)
			{
				base.IsWalkingTowardPlayer = false;
				this.spottedPlayer = false;
				this.controllerCountdown = 0;
				this.controller = null;
				this.Halt();
				this.tryToMeetUpWithNeighbor();
				this.addedSpeed = 0;
				base.doEmote(8, false);
			}
		}
	}
}
