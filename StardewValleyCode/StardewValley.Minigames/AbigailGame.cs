using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Minigames
{
	internal class AbigailGame : IMinigame
	{
		public delegate void behaviorAfterMotionPause();

		public class CowboyPowerup
		{
			public int which;

			public Point position;

			public int duration;

			public float yOffset;

			public CowboyPowerup(int which, Point position, int duration)
			{
				this.which = which;
				this.position = position;
				this.duration = duration;
			}

			public void draw(SpriteBatch b)
			{
				if (this.duration > 2000 || this.duration / 200 % 2 == 0)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y + this.yOffset), new Rectangle?(new Rectangle(272 + this.which * 16, 1808, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
				}
			}
		}

		public class CowboyBullet
		{
			public Point position;

			public Point motion;

			public int damage;

			public CowboyBullet(Point position, Point motion, int damage)
			{
				this.position = position;
				this.motion = motion;
				this.damage = damage;
			}

			public CowboyBullet(Point position, int direction, int damage)
			{
				this.position = position;
				switch (direction)
				{
				case 0:
					this.motion = new Point(0, -8);
					break;
				case 1:
					this.motion = new Point(8, 0);
					break;
				case 2:
					this.motion = new Point(0, 8);
					break;
				case 3:
					this.motion = new Point(-8, 0);
					break;
				}
				this.damage = damage;
			}
		}

		public class CowboyMonster
		{
			public const int MonsterAnimationDelay = 500;

			public int health;

			public int type;

			public int speed;

			public float movementAnimationTimer;

			public Rectangle position;

			private int movementDirection;

			private bool movedLastTurn;

			private bool oppositeMotionGuy;

			private bool invisible;

			private bool special;

			private bool uninterested;

			private bool flyer;

			private Color tint = Color.White;

			private Color flashColor = Color.Red;

			public float flashColorTimer;

			public int ticksSinceLastMovement;

			public Vector2 acceleration;

			private Point targetPosition;

			public CowboyMonster(int which, int health, int speed, Point position)
			{
				this.health = health;
				this.type = which;
				this.speed = speed;
				this.position = new Rectangle(position.X, position.Y, AbigailGame.TileSize, AbigailGame.TileSize);
				this.uninterested = (Game1.random.NextDouble() < 0.25);
			}

			public CowboyMonster(int which, Point position)
			{
				this.type = which;
				this.position = new Rectangle(position.X, position.Y, AbigailGame.TileSize, AbigailGame.TileSize);
				switch (this.type)
				{
				case 0:
					this.speed = 2;
					this.health = 1;
					this.uninterested = (Game1.random.NextDouble() < 0.25);
					if (this.uninterested)
					{
						this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
					}
					break;
				case 1:
					this.speed = 2;
					this.health = 1;
					this.flyer = true;
					break;
				case 2:
					this.speed = 1;
					this.health = 3;
					break;
				case 3:
					this.health = 6;
					this.speed = 1;
					this.uninterested = (Game1.random.NextDouble() < 0.25);
					if (this.uninterested)
					{
						this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
					}
					break;
				case 4:
					this.health = 3;
					this.speed = 3;
					this.flyer = true;
					break;
				case 5:
					this.speed = 3;
					this.health = 2;
					break;
				case 6:
					this.speed = 3;
					this.health = 2;
					do
					{
						this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
					}
					while (AbigailGame.isCollidingWithMap(this.targetPosition));
					break;
				}
				this.oppositeMotionGuy = (Game1.random.NextDouble() < 0.5);
			}

			public virtual void draw(SpriteBatch b)
			{
				if (this.type != 6 || !this.special)
				{
					if (!this.invisible)
					{
						if (this.flashColorTimer > 0f)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(352 + this.type * 16, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						}
						else
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(352 + (this.type * 2 + ((this.movementAnimationTimer < 250f) ? 1 : 0)) * 16, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						}
						if (AbigailGame.monsterConfusionTimer > 0)
						{
							b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString("?").X / 2f, (float)(this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.position.Y / 10000f);
							b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString("?").X / 2f + 1f, (float)(this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.position.Y / 10000f);
							b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString("?").X / 2f - 1f, (float)(this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.position.Y / 10000f);
						}
					}
					return;
				}
				if (this.flashColorTimer > 0f)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(480, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
					return;
				}
				b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(576, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
			}

			public virtual bool takeDamage(int damage)
			{
				this.health -= damage;
				this.health = Math.Max(0, this.health);
				if (this.health <= 0)
				{
					return true;
				}
				Game1.playSound("cowboy_monsterhit");
				this.flashColor = Color.Red;
				this.flashColorTimer = 100f;
				return false;
			}

			public virtual int getLootDrop()
			{
				if (this.type == 6 && this.special)
				{
					return -1;
				}
				if (Game1.random.NextDouble() < 0.05)
				{
					if (this.type != 0 && Game1.random.NextDouble() < 0.1)
					{
						return 1;
					}
					if (Game1.random.NextDouble() < 0.01)
					{
						return 1;
					}
					return 0;
				}
				else
				{
					if (Game1.random.NextDouble() >= 0.05)
					{
						return -1;
					}
					if (Game1.random.NextDouble() < 0.15)
					{
						return Game1.random.Next(6, 8);
					}
					if (Game1.random.NextDouble() < 0.07)
					{
						return 10;
					}
					int num = Game1.random.Next(2, 10);
					if (num == 5 && Game1.random.NextDouble() < 0.4)
					{
						num = Game1.random.Next(2, 10);
					}
					return num;
				}
			}

			public virtual bool move(Vector2 playerPosition, GameTime time)
			{
				this.movementAnimationTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.movementAnimationTimer <= 0f)
				{
					this.movementAnimationTimer = (float)Math.Max(100, 500 - this.speed * 50);
				}
				if (this.flashColorTimer > 0f)
				{
					this.flashColorTimer -= (float)time.ElapsedGameTime.Milliseconds;
					return false;
				}
				if (AbigailGame.monsterConfusionTimer > 0)
				{
					return false;
				}
				if (AbigailGame.shopping)
				{
					AbigailGame.shoppingTimer -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.shoppingTimer <= 0)
					{
						AbigailGame.shoppingTimer = 100;
					}
				}
				this.ticksSinceLastMovement++;
				switch (this.type)
				{
				case 0:
				case 2:
				case 3:
				case 5:
				case 6:
				{
					if (this.type == 6)
					{
						if (this.special || this.invisible)
						{
							break;
						}
						if (this.ticksSinceLastMovement > 20)
						{
							int num = 0;
							do
							{
								num++;
								this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
								if (!AbigailGame.isCollidingWithMap(this.targetPosition))
								{
									break;
								}
							}
							while (num < 5);
						}
					}
					else if (this.ticksSinceLastMovement > 20)
					{
						int num2 = 0;
						do
						{
							this.oppositeMotionGuy = !this.oppositeMotionGuy;
							num2++;
							this.targetPosition = new Point(Game1.random.Next(this.position.X - AbigailGame.TileSize * 2, this.position.X + AbigailGame.TileSize * 2), Game1.random.Next(this.position.Y - AbigailGame.TileSize * 2, this.position.Y + AbigailGame.TileSize * 2));
						}
						while (AbigailGame.isCollidingWithMap(this.targetPosition) && num2 < 5);
					}
					Point arg_211_0 = this.targetPosition;
					Vector2 vector = (!this.targetPosition.Equals(Point.Zero)) ? new Vector2((float)this.targetPosition.X, (float)this.targetPosition.Y) : playerPosition;
					if (AbigailGame.playingWithAbigail && vector.Equals(playerPosition))
					{
						double num3 = Math.Sqrt(Math.Pow((double)((float)this.position.X - vector.X), 2.0) - Math.Pow((double)((float)this.position.Y - vector.Y), 2.0));
						if (Math.Sqrt(Math.Pow((double)((float)this.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)this.position.Y - AbigailGame.player2Position.Y), 2.0)) < num3)
						{
							vector = AbigailGame.player2Position;
						}
					}
					if (AbigailGame.gopherRunning)
					{
						vector = new Vector2((float)AbigailGame.gopherBox.X, (float)AbigailGame.gopherBox.Y);
					}
					if (Game1.random.NextDouble() < 0.001)
					{
						this.oppositeMotionGuy = !this.oppositeMotionGuy;
					}
					if ((this.type == 6 && !this.oppositeMotionGuy) || Math.Abs(vector.X - (float)this.position.X) > Math.Abs(vector.Y - (float)this.position.Y))
					{
						if (vector.X + (float)this.speed < (float)this.position.X && (this.movedLastTurn || this.movementDirection != 3))
						{
							this.movementDirection = 3;
						}
						else if (vector.X > (float)(this.position.X + this.speed) && (this.movedLastTurn || this.movementDirection != 1))
						{
							this.movementDirection = 1;
						}
						else if (vector.Y > (float)(this.position.Y + this.speed) && (this.movedLastTurn || this.movementDirection != 2))
						{
							this.movementDirection = 2;
						}
						else if (vector.Y + (float)this.speed < (float)this.position.Y && (this.movedLastTurn || this.movementDirection != 0))
						{
							this.movementDirection = 0;
						}
					}
					else if (vector.Y > (float)(this.position.Y + this.speed) && (this.movedLastTurn || this.movementDirection != 2))
					{
						this.movementDirection = 2;
					}
					else if (vector.Y + (float)this.speed < (float)this.position.Y && (this.movedLastTurn || this.movementDirection != 0))
					{
						this.movementDirection = 0;
					}
					else if (vector.X + (float)this.speed < (float)this.position.X && (this.movedLastTurn || this.movementDirection != 3))
					{
						this.movementDirection = 3;
					}
					else if (vector.X > (float)(this.position.X + this.speed) && (this.movedLastTurn || this.movementDirection != 1))
					{
						this.movementDirection = 1;
					}
					this.movedLastTurn = false;
					Rectangle rectangle = this.position;
					switch (this.movementDirection)
					{
					case 0:
						rectangle.Y -= this.speed;
						break;
					case 1:
						rectangle.X += this.speed;
						break;
					case 2:
						rectangle.Y += this.speed;
						break;
					case 3:
						rectangle.X -= this.speed;
						break;
					}
					if (AbigailGame.zombieModeTimer > 0)
					{
						rectangle.X = this.position.X - (rectangle.X - this.position.X);
						rectangle.Y = this.position.Y - (rectangle.Y - this.position.Y);
					}
					if (this.type == 2)
					{
						for (int i = AbigailGame.monsters.Count - 1; i >= 0; i--)
						{
							if (AbigailGame.monsters[i].type == 6 && AbigailGame.monsters[i].special && AbigailGame.monsters[i].position.Intersects(rectangle))
							{
								AbigailGame.addGuts(AbigailGame.monsters[i].position.Location, AbigailGame.monsters[i].type);
								Game1.playSound("Cowboy_monsterDie");
								AbigailGame.monsters.RemoveAt(i);
							}
						}
					}
					if (!AbigailGame.isCollidingWithMapForMonsters(rectangle) && !AbigailGame.isCollidingWithMonster(rectangle, this) && AbigailGame.deathTimer <= 0f)
					{
						this.ticksSinceLastMovement = 0;
						this.position = rectangle;
						this.movedLastTurn = true;
						if (this.position.Contains((int)vector.X + AbigailGame.TileSize / 2, (int)vector.Y + AbigailGame.TileSize / 2))
						{
							this.targetPosition = Point.Zero;
							if ((this.type == 0 || this.type == 3) && this.uninterested)
							{
								this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
								if (Game1.random.NextDouble() < 0.5)
								{
									this.uninterested = false;
									this.targetPosition = Point.Zero;
								}
							}
							if (this.type == 6 && !this.invisible)
							{
								AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(352, 1728, 16, 16), 60f, 3, 0, new Vector2((float)this.position.X, (float)this.position.Y) + AbigailGame.topLeftScreenCoordinate, false, false, (float)this.position.Y / 10000f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
								{
									endFunction = new TemporaryAnimatedSprite.endBehavior(this.spikeyEndBehavior)
								});
								this.invisible = true;
							}
						}
					}
					break;
				}
				case 1:
				case 4:
				{
					if (this.ticksSinceLastMovement > 20)
					{
						int num4 = 0;
						do
						{
							this.oppositeMotionGuy = !this.oppositeMotionGuy;
							num4++;
							this.targetPosition = new Point(Game1.random.Next(this.position.X - AbigailGame.TileSize * 2, this.position.X + AbigailGame.TileSize * 2), Game1.random.Next(this.position.Y - AbigailGame.TileSize * 2, this.position.Y + AbigailGame.TileSize * 2));
						}
						while (AbigailGame.isCollidingWithMap(this.targetPosition) && num4 < 5);
					}
					Point arg_914_0 = this.targetPosition;
					Vector2 vector = (!this.targetPosition.Equals(Point.Zero)) ? new Vector2((float)this.targetPosition.X, (float)this.targetPosition.Y) : playerPosition;
					Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(this.position.Location, vector + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), (float)this.speed);
					float num5 = (velocityTowardPoint.X != 0f && velocityTowardPoint.Y != 0f) ? 1.5f : 1f;
					if (velocityTowardPoint.X > this.acceleration.X)
					{
						this.acceleration.X = this.acceleration.X + 0.1f * num5;
					}
					if (velocityTowardPoint.X < this.acceleration.X)
					{
						this.acceleration.X = this.acceleration.X - 0.1f * num5;
					}
					if (velocityTowardPoint.Y > this.acceleration.Y)
					{
						this.acceleration.Y = this.acceleration.Y + 0.1f * num5;
					}
					if (velocityTowardPoint.Y < this.acceleration.Y)
					{
						this.acceleration.Y = this.acceleration.Y - 0.1f * num5;
					}
					if (!AbigailGame.isCollidingWithMonster(new Rectangle(this.position.X + (int)Math.Ceiling((double)this.acceleration.X), this.position.Y + (int)Math.Ceiling((double)this.acceleration.Y), AbigailGame.TileSize, AbigailGame.TileSize), this) && AbigailGame.deathTimer <= 0f)
					{
						this.ticksSinceLastMovement = 0;
						this.position.X = this.position.X + (int)Math.Ceiling((double)this.acceleration.X);
						this.position.Y = this.position.Y + (int)Math.Ceiling((double)this.acceleration.Y);
						if (this.position.Contains((int)vector.X + AbigailGame.TileSize / 2, (int)vector.Y + AbigailGame.TileSize / 2))
						{
							this.targetPosition = Point.Zero;
						}
					}
					break;
				}
				}
				return false;
			}

			public void spikeyEndBehavior(int extraInfo)
			{
				this.invisible = false;
				this.health += 5;
				this.special = true;
			}
		}

		public class Dracula : AbigailGame.CowboyMonster
		{
			private const int gloatingPhase = -1;

			private const int walkRandomlyAndShootPhase = 0;

			private const int spreadShotPhase = 1;

			private const int summonDemonPhase = 2;

			private const int summonMummyPhase = 3;

			private int phase = -1;

			private int phaseInternalTimer;

			private int phaseInternalCounter;

			private int shootTimer;

			private int fullHealth;

			private Point homePosition;

			public Dracula() : base(-2, new Point(8 * AbigailGame.TileSize, 8 * AbigailGame.TileSize))
			{
				this.homePosition = this.position.Location;
				this.position.Y = this.position.Y + AbigailGame.TileSize * 4;
				this.health = 350;
				this.fullHealth = this.health;
				this.phase = -1;
				this.phaseInternalTimer = 4000;
				this.speed = 2;
			}

			public override void draw(SpriteBatch b)
			{
				if (this.phase != -1)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize + 3, (int)((float)(16 * AbigailGame.TileSize) * ((float)this.health / (float)this.fullHealth)), AbigailGame.TileSize / 3), new Color(188, 51, 74));
				}
				if (this.flashColorTimer > 0f)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(464, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
					return;
				}
				switch (this.phase)
				{
				case -1:
				case 1:
				case 2:
				case 3:
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(592 + this.phaseInternalTimer / 100 % 3 * 16, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
					if (this.phase == -1)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)(this.position.Y + AbigailGame.TileSize) + (float)Math.Sin((double)((float)this.phaseInternalTimer / 1000f)) * 3f), new Rectangle?(new Rectangle(528, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X - AbigailGame.TileSize / 2), (float)(this.position.Y - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(608, 1728, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
						return;
					}
					return;
				}
				b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(592 + this.phaseInternalTimer / 100 % 2 * 16, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
			}

			public override int getLootDrop()
			{
				return -1;
			}

			public override bool takeDamage(int damage)
			{
				if (this.phase == -1)
				{
					return false;
				}
				this.health -= damage;
				if (this.health < 0)
				{
					return true;
				}
				this.flashColorTimer = 100f;
				Game1.playSound("cowboy_monsterhit");
				return false;
			}

			public override bool move(Vector2 playerPosition, GameTime time)
			{
				if (this.flashColorTimer > 0f)
				{
					this.flashColorTimer -= (float)time.ElapsedGameTime.Milliseconds;
				}
				this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
				switch (this.phase)
				{
				case -1:
					if (this.phaseInternalTimer <= 0)
					{
						this.phaseInternalCounter = 0;
						if (Game1.soundBank != null)
						{
							AbigailGame.outlawSong = Game1.soundBank.GetCue("cowboy_boss");
							AbigailGame.outlawSong.Play();
						}
						this.phase = 0;
					}
					break;
				case 0:
					if (this.phaseInternalCounter == 0)
					{
						this.phaseInternalCounter++;
						this.phaseInternalTimer = Game1.random.Next(3000, 7000);
					}
					if (this.phaseInternalTimer < 0)
					{
						this.phaseInternalCounter = 0;
						this.phase = Game1.random.Next(1, 4);
						this.phaseInternalTimer = 9999;
					}
					if (AbigailGame.deathTimer <= 0f)
					{
						int num = -1;
						if (Math.Abs(playerPosition.X - (float)this.position.X) > Math.Abs(playerPosition.Y - (float)this.position.Y))
						{
							if (playerPosition.X + (float)this.speed < (float)this.position.X)
							{
								num = 3;
							}
							else if (playerPosition.X > (float)(this.position.X + this.speed))
							{
								num = 1;
							}
							else if (playerPosition.Y > (float)(this.position.Y + this.speed))
							{
								num = 2;
							}
							else if (playerPosition.Y + (float)this.speed < (float)this.position.Y)
							{
								num = 0;
							}
						}
						else if (playerPosition.Y > (float)(this.position.Y + this.speed))
						{
							num = 2;
						}
						else if (playerPosition.Y + (float)this.speed < (float)this.position.Y)
						{
							num = 0;
						}
						else if (playerPosition.X + (float)this.speed < (float)this.position.X)
						{
							num = 3;
						}
						else if (playerPosition.X > (float)(this.position.X + this.speed))
						{
							num = 1;
						}
						Rectangle position = this.position;
						switch (num)
						{
						case 0:
							position.Y -= this.speed;
							break;
						case 1:
							position.X += this.speed;
							break;
						case 2:
							position.Y += this.speed;
							break;
						case 3:
							position.X -= this.speed;
							break;
						}
						position.X = this.position.X - (position.X - this.position.X);
						position.Y = this.position.Y - (position.Y - this.position.Y);
						if (!AbigailGame.isCollidingWithMapForMonsters(position) && !AbigailGame.isCollidingWithMonster(position, this))
						{
							this.position = position;
						}
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							Vector2 vector = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							if (AbigailGame.playerMovementDirections.Count > 0)
							{
								vector = Utility.getTranslatedVector2(vector, AbigailGame.playerMovementDirections.Last<int>(), 3f);
							}
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int)vector.X, (int)vector.Y), 1));
							this.shootTimer = 250;
							Game1.playSound("Cowboy_gunshot");
						}
					}
					break;
				case 1:
					if (this.phaseInternalCounter == 0)
					{
						Point location = this.position.Location;
						if (this.position.X > this.homePosition.X + 6)
						{
							this.position.X = this.position.X - 6;
						}
						else if (this.position.X < this.homePosition.X - 6)
						{
							this.position.X = this.position.X + 6;
						}
						if (this.position.Y > this.homePosition.Y + 6)
						{
							this.position.Y = this.position.Y - 6;
						}
						else if (this.position.Y < this.homePosition.Y - 6)
						{
							this.position.Y = this.position.Y + 6;
						}
						if (this.position.Location.Equals(location))
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 1500;
						}
					}
					else if (this.phaseInternalCounter == 1)
					{
						if (this.phaseInternalTimer < 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 2000;
							this.shootTimer = 200;
							this.fireSpread(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), 0.0);
						}
					}
					else if (this.phaseInternalCounter == 2)
					{
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							this.fireSpread(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), 0.0);
							this.shootTimer = 200;
						}
						if (this.phaseInternalTimer < 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 500;
						}
					}
					else if (this.phaseInternalCounter == 3)
					{
						if (this.phaseInternalTimer < 0)
						{
							this.phaseInternalTimer = 2000;
							this.shootTimer = 200;
							this.phaseInternalCounter++;
							Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int)velocityTowardPoint.X, (int)velocityTowardPoint.Y), 1));
							Game1.playSound("Cowboy_gunshot");
						}
					}
					else if (this.phaseInternalCounter == 4)
					{
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							Vector2 velocityTowardPoint2 = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							velocityTowardPoint2.X += (float)Game1.random.Next(-1, 2);
							velocityTowardPoint2.Y += (float)Game1.random.Next(-1, 2);
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int)velocityTowardPoint2.X, (int)velocityTowardPoint2.Y), 1));
							Game1.playSound("Cowboy_gunshot");
							this.shootTimer = 200;
						}
						if (this.phaseInternalTimer < 0)
						{
							if (Game1.random.NextDouble() < 0.4)
							{
								this.phase = 0;
								this.phaseInternalCounter = 0;
							}
							else
							{
								this.phaseInternalTimer = 500;
								this.phaseInternalCounter = 1;
							}
						}
					}
					break;
				case 2:
				case 3:
					if (this.phaseInternalCounter == 0)
					{
						Point location2 = this.position.Location;
						if (this.position.X > this.homePosition.X + 6)
						{
							this.position.X = this.position.X - 6;
						}
						else if (this.position.X < this.homePosition.X - 6)
						{
							this.position.X = this.position.X + 6;
						}
						if (this.position.Y > this.homePosition.Y + 6)
						{
							this.position.Y = this.position.Y - 6;
						}
						else if (this.position.Y < this.homePosition.Y - 6)
						{
							this.position.Y = this.position.Y + 6;
						}
						if (this.position.Location.Equals(location2))
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 1500;
						}
					}
					else if (this.phaseInternalCounter == 1 && this.phaseInternalTimer < 0)
					{
						this.summonEnemies(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), Game1.random.Next(0, 5));
						if (Game1.random.NextDouble() < 0.4)
						{
							this.phase = 0;
							this.phaseInternalCounter = 0;
						}
						else
						{
							this.phaseInternalTimer = 2000;
						}
					}
					break;
				}
				return false;
			}

			public void fireSpread(Point origin, double offsetAngle)
			{
				Vector2[] surroundingTileLocationsArray = Utility.getSurroundingTileLocationsArray(new Vector2((float)origin.X, (float)origin.Y));
				for (int i = 0; i < surroundingTileLocationsArray.Length; i++)
				{
					Vector2 vector = surroundingTileLocationsArray[i];
					Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(origin, vector, 6f);
					if (offsetAngle > 0.0)
					{
						offsetAngle /= 2.0;
						velocityTowardPoint.X = (float)(Math.Cos(offsetAngle) * (double)(vector.X - (float)origin.X) - Math.Sin(offsetAngle) * (double)(vector.Y - (float)origin.Y) + (double)origin.X);
						velocityTowardPoint.Y = (float)(Math.Sin(offsetAngle) * (double)(vector.X - (float)origin.X) + Math.Cos(offsetAngle) * (double)(vector.Y - (float)origin.Y) + (double)origin.Y);
						velocityTowardPoint = Utility.getVelocityTowardPoint(origin, velocityTowardPoint, 8f);
					}
					AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(origin, new Point((int)velocityTowardPoint.X, (int)velocityTowardPoint.Y), 1));
				}
				Game1.playSound("Cowboy_gunshot");
			}

			public void summonEnemies(Point origin, int which)
			{
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2, origin.Y, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2, origin.Y)));
				}
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2, origin.Y, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2, origin.Y)));
				}
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X, origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X, origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2)));
				}
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X, origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X, origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4)));
				}
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2), (float)origin.Y), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2), (float)origin.Y), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)origin.X, (float)(origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)origin.X, (float)(origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				Game1.playSound("Cowboy_monsterDie");
			}
		}

		public class Outlaw : AbigailGame.CowboyMonster
		{
			private const int talkingPhase = -1;

			private const int hidingPhase = 0;

			private const int dartOutAndShootPhase = 1;

			private const int runAndGunPhase = 2;

			private const int runGunAndPantPhase = 3;

			private const int shootAtPlayerPhase = 4;

			private int phase;

			private int phaseCountdown;

			private int shootTimer;

			private int phaseInternalTimer;

			private int phaseInternalCounter;

			public bool dartLeft;

			private int fullHealth;

			private Point homePosition;

			public Outlaw(Point position, int health) : base(-1, position)
			{
				this.homePosition = position;
				this.health = health;
				this.fullHealth = health;
				this.phaseCountdown = 4000;
				this.phase = -1;
			}

			public override void draw(SpriteBatch b)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize + 3, (int)((float)(16 * AbigailGame.TileSize) * ((float)this.health / (float)this.fullHealth)), AbigailGame.TileSize / 3), new Color(188, 51, 74));
				if (this.flashColorTimer > 0f)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(496, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
					return;
				}
				int num = this.phase;
				if (num == -1 || num == 0)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(560 + ((this.phaseCountdown / 250 % 2 == 0) ? 16 : 0), 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
					if (this.phase == -1 && this.phaseCountdown > 1000)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X - AbigailGame.TileSize / 2), (float)(this.position.Y - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(576 + ((AbigailGame.whichWave > 5) ? 32 : 0), 1792, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						return;
					}
				}
				else
				{
					if (this.phase == 3 && this.phaseInternalCounter == 2)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(560 + ((this.phaseCountdown / 250 % 2 == 0) ? 16 : 0), 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						return;
					}
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(592 + ((this.phaseCountdown / 80 % 2 == 0) ? 16 : 0), 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
				}
			}

			public override bool move(Vector2 playerPosition, GameTime time)
			{
				if (this.flashColorTimer > 0f)
				{
					this.flashColorTimer -= (float)time.ElapsedGameTime.Milliseconds;
				}
				this.phaseCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.position.X > 17 * AbigailGame.TileSize || this.position.X < -AbigailGame.TileSize)
				{
					this.position.X = 16 * AbigailGame.TileSize / 2;
				}
				switch (this.phase)
				{
				case -1:
				case 0:
					if (this.phaseCountdown < 0)
					{
						this.phase = Game1.random.Next(1, 5);
						this.dartLeft = (playerPosition.X < (float)this.position.X);
						if (playerPosition.X > (float)(7 * AbigailGame.TileSize) && playerPosition.X < (float)(9 * AbigailGame.TileSize))
						{
							if (Game1.random.NextDouble() < 0.66 || this.phase == 2)
							{
								this.phase = 4;
							}
						}
						else if (this.phase == 4)
						{
							this.phase = 3;
						}
						this.phaseInternalCounter = 0;
						this.phaseInternalTimer = 0;
					}
					break;
				case 1:
				{
					int num = this.dartLeft ? -3 : 3;
					if (Math.Abs(this.position.Location.X - this.homePosition.X + AbigailGame.TileSize / 2) < AbigailGame.TileSize * 2 + 12 && this.phaseInternalCounter == 0)
					{
						this.position.X = this.position.X + num;
						if (this.position.X > 256)
						{
							this.phaseInternalCounter = 2;
						}
					}
					else if (this.phaseInternalCounter == 2)
					{
						this.position.X = this.position.X - num;
						if (Math.Abs(this.position.X - this.homePosition.X) < 4)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
					}
					else
					{
						if (this.phaseInternalCounter == 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = Game1.random.Next(1000, 2000);
						}
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-2, 3), -8), 1));
							this.shootTimer = 150;
							Game1.playSound("Cowboy_gunshot");
						}
						if (this.phaseInternalTimer <= 0)
						{
							this.phaseInternalCounter++;
						}
					}
					break;
				}
				case 2:
					if (this.phaseInternalCounter == 2)
					{
						if (this.position.X < this.homePosition.X)
						{
							this.position.X = this.position.X + 4;
						}
						else
						{
							this.position.X = this.position.X - 4;
						}
						if (Math.Abs(this.position.X - this.homePosition.X) < 5)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
						return false;
					}
					if (this.phaseInternalCounter == 0)
					{
						this.phaseInternalCounter++;
						this.phaseInternalTimer = Game1.random.Next(4000, 7000);
					}
					this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
					if ((float)this.position.X > playerPosition.X && (float)this.position.X - playerPosition.X > 3f)
					{
						this.position.X = this.position.X - 2;
					}
					else if ((float)this.position.X < playerPosition.X && playerPosition.X - (float)this.position.X > 3f)
					{
						this.position.X = this.position.X + 2;
					}
					this.shootTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.shootTimer < 0)
					{
						AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-1, 2), -8), 1));
						this.shootTimer = 250;
						if (this.fullHealth > 50)
						{
							this.shootTimer -= 50;
						}
						if (Game1.random.NextDouble() < 0.2)
						{
							this.shootTimer = 150;
						}
						Game1.playSound("Cowboy_gunshot");
					}
					if (this.phaseInternalTimer <= 0)
					{
						this.phaseInternalCounter++;
					}
					break;
				case 3:
					if (this.phaseInternalCounter == 0)
					{
						this.phaseInternalCounter++;
						this.phaseInternalTimer = Game1.random.Next(3000, 6500);
					}
					else if (this.phaseInternalCounter == 2)
					{
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.phaseInternalTimer <= 0)
						{
							this.phaseInternalCounter++;
						}
					}
					else if (this.phaseInternalCounter == 3)
					{
						if (this.position.X < this.homePosition.X)
						{
							this.position.X = this.position.X + 4;
						}
						else
						{
							this.position.X = this.position.X - 4;
						}
						if (Math.Abs(this.position.X - this.homePosition.X) < 5)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
					}
					else
					{
						int num = this.dartLeft ? -3 : 3;
						this.position.X = this.position.X + num;
						if (this.position.X < AbigailGame.TileSize || this.position.X > 15 * AbigailGame.TileSize)
						{
							this.dartLeft = !this.dartLeft;
						}
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-1, 2), -8), 1));
							this.shootTimer = 250;
							if (this.fullHealth > 50)
							{
								this.shootTimer -= 50;
							}
							if (Game1.random.NextDouble() < 0.2)
							{
								this.shootTimer = 150;
							}
							Game1.playSound("Cowboy_gunshot");
						}
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.phaseInternalTimer <= 0)
						{
							if (this.phase == 2)
							{
								this.phaseInternalCounter = 3;
							}
							else
							{
								this.phaseInternalTimer = 3000;
								this.phaseInternalCounter++;
							}
						}
					}
					break;
				case 4:
				{
					int num = this.dartLeft ? -3 : 3;
					if (this.phaseInternalCounter == 0 && (playerPosition.X <= (float)(7 * AbigailGame.TileSize) || playerPosition.X >= (float)(9 * AbigailGame.TileSize)))
					{
						this.phaseInternalCounter = 1;
						this.phaseInternalTimer = Game1.random.Next(500, 1500);
					}
					else if (Math.Abs(this.position.Location.X - this.homePosition.X + AbigailGame.TileSize / 2) < AbigailGame.TileSize * 7 + 12 && this.phaseInternalCounter == 0)
					{
						this.position.X = this.position.X + num;
					}
					else if (this.phaseInternalCounter == 2)
					{
						num = (this.dartLeft ? -4 : 4);
						this.position.X = this.position.X - num;
						if (Math.Abs(this.position.X - this.homePosition.X) < 4)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
					}
					else
					{
						if (this.phaseInternalCounter == 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = Game1.random.Next(1000, 2000);
						}
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point((int)velocityTowardPoint.X, (int)velocityTowardPoint.Y), 1));
							this.shootTimer = 120;
							Game1.playSound("Cowboy_gunshot");
						}
						if (this.phaseInternalTimer <= 0)
						{
							this.phaseInternalCounter++;
						}
					}
					break;
				}
				}
				if (this.position.X <= 16 * AbigailGame.TileSize)
				{
					int arg_A90_0 = this.position.X;
				}
				return false;
			}

			public override int getLootDrop()
			{
				return 8;
			}

			public override bool takeDamage(int damage)
			{
				if (Math.Abs(this.position.X - this.homePosition.X) < 5)
				{
					return false;
				}
				this.health -= damage;
				if (this.health < 0)
				{
					return true;
				}
				this.flashColorTimer = 150f;
				Game1.playSound("cowboy_monsterhit");
				return false;
			}
		}

		public const int mapWidth = 16;

		public const int mapHeight = 16;

		public const int pixelZoom = 3;

		public const int bulletSpeed = 8;

		public const double lootChance = 0.05;

		public const double coinChance = 0.05;

		public int lootDuration = 7500;

		public int powerupDuration = 10000;

		private const int abigailPortraitDuration = 6000;

		public const float playerSpeed = 3f;

		public const int baseTileSize = 16;

		public const int orcSpeed = 2;

		public const int ogreSpeed = 1;

		public const int ghostSpeed = 3;

		public const int spikeySpeed = 3;

		public const int orcHealth = 1;

		public const int ghostHealth = 1;

		public const int ogreHealth = 3;

		public const int spikeyHealth = 2;

		public const int cactusDanceDelay = 800;

		public const int playerMotionDelay = 100;

		public const int playerFootStepDelay = 200;

		public const int deathDelay = 3000;

		public const int MAP_BARRIER1 = 0;

		public const int MAP_BARRIER2 = 1;

		public const int MAP_ROCKY1 = 2;

		public const int MAP_DESERT = 3;

		public const int MAP_GRASSY = 4;

		public const int MAP_CACTUS = 5;

		public const int MAP_FENCE = 7;

		public const int MAP_TRENCH1 = 8;

		public const int MAP_TRENCH2 = 9;

		public const int MAP_BRIDGE = 10;

		public const int orc = 0;

		public const int ghost = 1;

		public const int ogre = 2;

		public const int mummy = 3;

		public const int devil = 4;

		public const int mushroom = 5;

		public const int spikey = 6;

		public const int dracula = 7;

		public const int desert = 0;

		public const int woods = 2;

		public const int graveyard = 1;

		public const int POWERUP_LOG = -1;

		public const int POWERUP_SKULL = -2;

		public const int coin1 = 0;

		public const int coin5 = 1;

		public const int POWERUP_SPREAD = 2;

		public const int POWERUP_RAPIDFIRE = 3;

		public const int POWERUP_NUKE = 4;

		public const int POWERUP_ZOMBIE = 5;

		public const int POWERUP_SPEED = 6;

		public const int POWERUP_SHOTGUN = 7;

		public const int POWERUP_LIFE = 8;

		public const int POWERUP_TELEPORT = 9;

		public const int POWERUP_SHERRIFF = 10;

		public const int POWERUP_HEART = -3;

		public const int ITEM_FIRESPEED1 = 0;

		public const int ITEM_FIRESPEED2 = 1;

		public const int ITEM_FIRESPEED3 = 2;

		public const int ITEM_RUNSPEED1 = 3;

		public const int ITEM_RUNSPEED2 = 4;

		public const int ITEM_LIFE = 5;

		public const int ITEM_AMMO1 = 6;

		public const int ITEM_AMMO2 = 7;

		public const int ITEM_AMMO3 = 8;

		public const int ITEM_SPREADPISTOL = 9;

		public const int ITEM_STAR = 10;

		public const int ITEM_SKULL = 11;

		public const int ITEM_LOG = 12;

		public const int option_retry = 0;

		public const int option_quit = 1;

		public int runSpeedLevel;

		public int fireSpeedLevel;

		public int ammoLevel;

		public int whichRound;

		public bool spreadPistol;

		public const int waveDuration = 80000;

		public const int betweenWaveDuration = 5000;

		public static List<AbigailGame.CowboyMonster> monsters = new List<AbigailGame.CowboyMonster>();

		public Vector2 playerPosition;

		public static Vector2 player2Position = default(Vector2);

		public Rectangle playerBoundingBox;

		public Rectangle merchantBox;

		public Rectangle player2BoundingBox;

		public Rectangle noPickUpBox;

		public static List<int> playerMovementDirections = new List<int>();

		public static List<int> playerShootingDirections = new List<int>();

		public List<int> player2MovementDirections = new List<int>();

		public List<int> player2ShootingDirections = new List<int>();

		public int shootingDelay = 300;

		public int shotTimer;

		public int motionPause;

		public int bulletDamage;

		public int speedBonus;

		public int fireRateBonus;

		public int lives = 3;

		public int coins;

		public int score;

		public int player2deathtimer;

		public int player2invincibletimer;

		public List<AbigailGame.CowboyBullet> bullets = new List<AbigailGame.CowboyBullet>();

		public static List<AbigailGame.CowboyBullet> enemyBullets = new List<AbigailGame.CowboyBullet>();

		public static int[,] map = new int[16, 16];

		public static int[,] nextMap = new int[16, 16];

		public List<Point>[] spawnQueue = new List<Point>[4];

		public static Vector2 topLeftScreenCoordinate;

		public float cactusDanceTimer;

		public float playerMotionAnimationTimer;

		public float playerFootstepSoundTimer = 200f;

		public AbigailGame.behaviorAfterMotionPause behaviorAfterPause;

		public List<Vector2> monsterChances = new List<Vector2>
		{
			new Vector2(0.014f, 0.4f),
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero
		};

		public Rectangle shoppingCarpetNoPickup;

		public Dictionary<int, int> activePowerups = new Dictionary<int, int>();

		public static List<AbigailGame.CowboyPowerup> powerups = new List<AbigailGame.CowboyPowerup>();

		public string AbigailDialogue = "";

		public static List<TemporaryAnimatedSprite> temporarySprites = new List<TemporaryAnimatedSprite>();

		public AbigailGame.CowboyPowerup heldItem;

		public static int world = 0;

		public int gameOverOption;

		public int gamerestartTimer;

		public int player2TargetUpdateTimer;

		public int player2shotTimer;

		public int player2AnimationTimer;

		public int fadethenQuitTimer;

		public int abigailPortraitYposition;

		public int abigailPortraitTimer;

		public int abigailPortraitExpression;

		public static int waveTimer = 80000;

		public static int betweenWaveTimer = 5000;

		public static int whichWave;

		public static int monsterConfusionTimer;

		public static int zombieModeTimer;

		public static int shoppingTimer;

		public static int holdItemTimer;

		public static int itemToHold;

		public static int newMapPosition;

		public static int playerInvincibleTimer;

		public static int screenFlash;

		public static int gopherTrainPosition;

		public static int endCutsceneTimer;

		public static int endCutscenePhase;

		public static int startTimer;

		public static float deathTimer;

		public static bool onStartMenu;

		public static bool shopping;

		public static bool gopherRunning;

		public static bool store;

		public static bool merchantLeaving;

		public static bool merchantArriving;

		public static bool merchantShopOpen;

		public static bool waitingForPlayerToMoveDownAMap;

		public static bool scrollingMap;

		public static bool hasGopherAppeared;

		public static bool shootoutLevel;

		public static bool gopherTrain;

		public static bool playerJumped;

		public static bool endCutscene;

		public static bool gameOver;

		public static bool playingWithAbigail;

		public static bool beatLevelWithAbigail;

		private Dictionary<Rectangle, int> storeItems = new Dictionary<Rectangle, int>();

		private bool quit;

		private bool died;

		public static Rectangle gopherBox;

		public Point gopherMotion;

		private static Cue overworldSong;

		private static Cue outlawSong;

		private int player2FootstepSoundTimer;

		private AbigailGame.CowboyMonster targetMonster;

		public static int TileSize
		{
			get
			{
				return 48;
			}
		}

		public AbigailGame(bool playingWithAbby = false)
		{
			this.reset(playingWithAbby);
		}

		public AbigailGame(int coins, int ammoLevel, int bulletDamage, int fireSpeedLevel, int runSpeedLevel, int lives, bool spreadPistol, int whichRound)
		{
			this.reset(false);
			this.coins = coins;
			this.ammoLevel = ammoLevel;
			this.bulletDamage = bulletDamage;
			this.fireSpeedLevel = fireSpeedLevel;
			this.runSpeedLevel = runSpeedLevel;
			this.lives = lives;
			this.spreadPistol = spreadPistol;
			this.whichRound = whichRound;
			this.monsterChances[0] = new Vector2(0.014f + (float)whichRound * 0.005f, 0.41f + (float)whichRound * 0.05f);
			this.monsterChances[4] = new Vector2(0.002f, 0.1f);
			AbigailGame.onStartMenu = false;
		}

		public void reset(bool playingWithAbby)
		{
			this.died = false;
			AbigailGame.topLeftScreenCoordinate = new Vector2((float)(Game1.viewport.Width / 2 - 384), (float)(Game1.viewport.Height / 2 - 384));
			AbigailGame.enemyBullets.Clear();
			AbigailGame.holdItemTimer = 0;
			AbigailGame.itemToHold = -1;
			AbigailGame.merchantArriving = false;
			AbigailGame.merchantLeaving = false;
			AbigailGame.merchantShopOpen = false;
			AbigailGame.monsterConfusionTimer = 0;
			AbigailGame.monsters.Clear();
			AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
			AbigailGame.scrollingMap = false;
			AbigailGame.shopping = false;
			AbigailGame.store = false;
			AbigailGame.temporarySprites.Clear();
			AbigailGame.waitingForPlayerToMoveDownAMap = false;
			AbigailGame.waveTimer = 80000;
			AbigailGame.whichWave = 0;
			AbigailGame.zombieModeTimer = 0;
			this.bulletDamage = 1;
			AbigailGame.deathTimer = 0f;
			AbigailGame.shootoutLevel = false;
			AbigailGame.betweenWaveTimer = 5000;
			AbigailGame.gopherRunning = false;
			AbigailGame.hasGopherAppeared = false;
			AbigailGame.playerMovementDirections.Clear();
			AbigailGame.outlawSong = null;
			AbigailGame.overworldSong = null;
			AbigailGame.endCutscene = false;
			AbigailGame.endCutscenePhase = 0;
			AbigailGame.endCutsceneTimer = 0;
			AbigailGame.gameOver = false;
			AbigailGame.deathTimer = 0f;
			AbigailGame.playerInvincibleTimer = 0;
			AbigailGame.playingWithAbigail = playingWithAbby;
			AbigailGame.beatLevelWithAbigail = false;
			AbigailGame.onStartMenu = true;
			AbigailGame.startTimer = 0;
			AbigailGame.powerups.Clear();
			AbigailGame.world = 0;
			Game1.changeMusicTrack("none");
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if ((i == 0 || i == 15 || j == 0 || j == 15) && (i <= 6 || i >= 10) && (j <= 6 || j >= 10))
					{
						AbigailGame.map[i, j] = 5;
					}
					else if (i == 0 || i == 15 || j == 0 || j == 15)
					{
						AbigailGame.map[i, j] = ((Game1.random.NextDouble() < 0.15) ? 1 : 0);
					}
					else if (i == 1 || i == 14 || j == 1 || j == 14)
					{
						AbigailGame.map[i, j] = 2;
					}
					else
					{
						AbigailGame.map[i, j] = ((Game1.random.NextDouble() < 0.1) ? 4 : 3);
					}
				}
			}
			this.playerPosition = new Vector2(384f, 384f);
			this.playerBoundingBox.X = (int)this.playerPosition.X + AbigailGame.TileSize / 4;
			this.playerBoundingBox.Y = (int)this.playerPosition.Y + AbigailGame.TileSize / 4;
			this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
			this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
			if (AbigailGame.playingWithAbigail)
			{
				AbigailGame.onStartMenu = false;
				AbigailGame.player2Position = new Vector2(432f, 384f);
				this.player2BoundingBox = new Rectangle(9 * AbigailGame.TileSize, 8 * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize);
				AbigailGame.betweenWaveTimer += 1500;
			}
			for (int k = 0; k < 4; k++)
			{
				this.spawnQueue[k] = new List<Point>();
			}
			this.noPickUpBox = new Rectangle(0, 0, AbigailGame.TileSize, AbigailGame.TileSize);
			this.merchantBox = new Rectangle(8 * AbigailGame.TileSize, 0, AbigailGame.TileSize, AbigailGame.TileSize);
			AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
		}

		public float getMovementSpeed(float speed, int directions)
		{
			float num = speed;
			if (directions > 1)
			{
				num = (float)Math.Max(1, (int)Math.Sqrt((double)(2f * (num * num))) / 2);
			}
			return num;
		}

		public bool getPowerUp(AbigailGame.CowboyPowerup c)
		{
			int which = c.which;
			switch (which)
			{
			case -3:
				this.usePowerup(-3);
				break;
			case -2:
				this.usePowerup(-2);
				break;
			case -1:
				this.usePowerup(-1);
				break;
			case 0:
				this.coins++;
				Game1.playSound("Pickup_Coin15");
				break;
			case 1:
				this.coins += 5;
				Game1.playSound("Pickup_Coin15");
				break;
			default:
				if (which != 8)
				{
					if (this.heldItem != null)
					{
						AbigailGame.CowboyPowerup cowboyPowerup = this.heldItem;
						this.heldItem = c;
						this.noPickUpBox.Location = c.position;
						cowboyPowerup.position = c.position;
						AbigailGame.powerups.Add(cowboyPowerup);
						Game1.playSound("cowboy_powerup");
						return true;
					}
					this.heldItem = c;
					Game1.playSound("cowboy_powerup");
				}
				else
				{
					this.lives++;
					Game1.playSound("cowboy_powerup");
				}
				break;
			}
			return true;
		}

		public bool overrideFreeMouseMovement()
		{
			return false;
		}

		public void usePowerup(int which)
		{
			if (this.activePowerups.ContainsKey(which))
			{
				this.activePowerups[which] = this.powerupDuration + 2000;
				return;
			}
			switch (which)
			{
			case -3:
				AbigailGame.itemToHold = 13;
				AbigailGame.holdItemTimer = 4000;
				Game1.playSound("Cowboy_Secret");
				AbigailGame.endCutscene = true;
				AbigailGame.endCutsceneTimer = 4000;
				AbigailGame.world = 0;
				if (!Game1.player.hasOrWillReceiveMail("Beat_PK"))
				{
					Game1.addMailForTomorrow("Beat_PK", false, false);
					goto IL_846;
				}
				goto IL_846;
			case -2:
			case -1:
				AbigailGame.itemToHold = ((which == -1) ? 12 : 11);
				AbigailGame.holdItemTimer = 2000;
				Game1.playSound("Cowboy_Secret");
				AbigailGame.gopherTrain = true;
				AbigailGame.gopherTrainPosition = -AbigailGame.TileSize * 2;
				goto IL_846;
			case 0:
				this.coins++;
				Game1.playSound("Pickup_Coin15");
				goto IL_846;
			case 1:
				this.coins += 5;
				Game1.playSound("Pickup_Coin15");
				Game1.playSound("Pickup_Coin15");
				goto IL_846;
			case 2:
			case 3:
			case 7:
				this.shotTimer = 0;
				Game1.playSound("cowboy_gunload");
				this.activePowerups.Add(which, this.powerupDuration + 2000);
				goto IL_846;
			case 4:
				Game1.playSound("cowboy_explosion");
				if (!AbigailGame.shootoutLevel)
				{
					foreach (AbigailGame.CowboyMonster current in AbigailGame.monsters)
					{
						AbigailGame.addGuts(current.position.Location, current.type);
					}
					AbigailGame.monsters.Clear();
				}
				else
				{
					foreach (AbigailGame.CowboyMonster current2 in AbigailGame.monsters)
					{
						current2.takeDamage(30);
						this.bullets.Add(new AbigailGame.CowboyBullet(current2.position.Center, 2, 1));
					}
				}
				for (int i = 0; i < 30; i++)
				{
					AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)Game1.random.Next(1, 16), (float)Game1.random.Next(1, 16)) * (float)AbigailGame.TileSize + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
					{
						delayBeforeAnimationStart = Game1.random.Next(800)
					});
				}
				goto IL_846;
			case 5:
				if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
				{
					AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
				}
				Game1.playSound("Cowboy_undead");
				this.motionPause = 1800;
				AbigailGame.zombieModeTimer = 10000;
				goto IL_846;
			case 8:
				this.lives++;
				Game1.playSound("cowboy_powerup");
				goto IL_846;
			case 9:
			{
				Point zero = Point.Zero;
				while (Math.Abs((float)zero.X - this.playerPosition.X) < 8f || Math.Abs((float)zero.Y - this.playerPosition.Y) < 8f || AbigailGame.isCollidingWithMap(zero) || AbigailGame.isCollidingWithMonster(new Rectangle(zero.X, zero.Y, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					zero = new Point(Game1.random.Next(AbigailGame.TileSize, 16 * AbigailGame.TileSize - AbigailGame.TileSize), Game1.random.Next(AbigailGame.TileSize, 16 * AbigailGame.TileSize - AbigailGame.TileSize));
				}
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)zero.X, (float)zero.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)(zero.X - AbigailGame.TileSize / 2), (float)zero.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 200
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)(zero.X + AbigailGame.TileSize / 2), (float)zero.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 400
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)zero.X, (float)(zero.Y - AbigailGame.TileSize / 2)) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 600
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)zero.X, (float)(zero.Y + AbigailGame.TileSize / 2)) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 800
				});
				this.playerPosition = new Vector2((float)zero.X, (float)zero.Y);
				AbigailGame.monsterConfusionTimer = 4000;
				AbigailGame.playerInvincibleTimer = 4000;
				Game1.playSound("cowboy_powerup");
				goto IL_846;
			}
			case 10:
				this.usePowerup(7);
				this.usePowerup(3);
				this.usePowerup(6);
				for (int j = 0; j < this.activePowerups.Count; j++)
				{
					Dictionary<int, int> dictionary = this.activePowerups;
					int key = this.activePowerups.ElementAt(j).Key;
					dictionary[key] *= 2;
				}
				goto IL_846;
			}
			this.activePowerups.Add(which, this.powerupDuration);
			Game1.playSound("cowboy_powerup");
			IL_846:
			if (this.whichRound > 0 && this.activePowerups.ContainsKey(which))
			{
				Dictionary<int, int> dictionary = this.activePowerups;
				dictionary[which] /= 2;
			}
		}

		public static void addGuts(Point position, int whichGuts)
		{
			switch (whichGuts)
			{
			case 0:
			case 2:
			case 5:
			case 6:
			case 7:
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(512, 1696, 16, 16), 80f, 6, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 1696, 16, 16), 10000f, 1, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 480
				});
				return;
			case 1:
			case 4:
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(544, 1728, 16, 16), 80f, 4, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				return;
			case 3:
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				return;
			default:
				return;
			}
		}

		public void endOfGopherAnimationBehavior2(int extraInfo)
		{
			Game1.playSound("cowboy_gopher");
			if (Math.Abs(AbigailGame.gopherBox.X - 8 * AbigailGame.TileSize) > Math.Abs(AbigailGame.gopherBox.Y - 8 * AbigailGame.TileSize))
			{
				if (AbigailGame.gopherBox.X > 8 * AbigailGame.TileSize)
				{
					this.gopherMotion = new Point(-2, 0);
				}
				else
				{
					this.gopherMotion = new Point(2, 0);
				}
			}
			else if (AbigailGame.gopherBox.Y > 8 * AbigailGame.TileSize)
			{
				this.gopherMotion = new Point(0, -2);
			}
			else
			{
				this.gopherMotion = new Point(0, 2);
			}
			AbigailGame.gopherRunning = true;
		}

		public void endOfGopherAnimationBehavior(int extrainfo)
		{
			AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(384, 1792, 16, 16), 120f, 4, 2, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.gopherBox.X + AbigailGame.TileSize / 2), (float)(AbigailGame.gopherBox.Y + AbigailGame.TileSize / 2)), false, false, (float)AbigailGame.gopherBox.Y / 10000f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
			{
				endFunction = new TemporaryAnimatedSprite.endBehavior(this.endOfGopherAnimationBehavior2)
			});
			Game1.playSound("cowboy_gopher");
		}

		public static void killOutlaw()
		{
			AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup((AbigailGame.world == 0) ? -1 : -2, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
			if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
			{
				AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
			}
			AbigailGame.map[8, 8] = 10;
			AbigailGame.screenFlash = 200;
			Game1.playSound("Cowboy_monsterDie");
			for (int i = 0; i < 15; i++)
			{
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)(AbigailGame.monsters[0].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[0].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = i * 75
				});
			}
			AbigailGame.monsters.Clear();
		}

		public void updateBullets(GameTime time)
		{
			for (int i = this.bullets.Count - 1; i >= 0; i--)
			{
				AbigailGame.CowboyBullet expr_29_cp_0_cp_0 = this.bullets[i];
				expr_29_cp_0_cp_0.position.X = expr_29_cp_0_cp_0.position.X + this.bullets[i].motion.X;
				AbigailGame.CowboyBullet expr_59_cp_0_cp_0 = this.bullets[i];
				expr_59_cp_0_cp_0.position.Y = expr_59_cp_0_cp_0.position.Y + this.bullets[i].motion.Y;
				if (this.bullets[i].position.X <= 0 || this.bullets[i].position.Y <= 0 || this.bullets[i].position.X >= 768 || this.bullets[i].position.Y >= 768)
				{
					this.bullets.RemoveAt(i);
				}
				else if (AbigailGame.map[this.bullets[i].position.X / 16 / 3, this.bullets[i].position.Y / 16 / 3] == 7)
				{
					this.bullets.RemoveAt(i);
				}
				else
				{
					int j = AbigailGame.monsters.Count - 1;
					while (j >= 0)
					{
						if (AbigailGame.monsters[j].position.Intersects(new Rectangle(this.bullets[i].position.X, this.bullets[i].position.Y, 12, 12)))
						{
							int health = AbigailGame.monsters[j].health;
							int health2;
							if (AbigailGame.monsters[j].takeDamage(this.bullets[i].damage))
							{
								health2 = AbigailGame.monsters[j].health;
								AbigailGame.addGuts(AbigailGame.monsters[j].position.Location, AbigailGame.monsters[j].type);
								int num = AbigailGame.monsters[j].getLootDrop();
								if (this.whichRound == 1 && Game1.random.NextDouble() < 0.5)
								{
									num = -1;
								}
								if (this.whichRound > 0 && (num == 5 || num == 8) && Game1.random.NextDouble() < 0.4)
								{
									num = -1;
								}
								if (num != -1 && AbigailGame.whichWave != 12)
								{
									AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(num, AbigailGame.monsters[j].position.Location, this.lootDuration));
								}
								if (AbigailGame.shootoutLevel)
								{
									if (AbigailGame.whichWave == 12 && AbigailGame.monsters[j].type == -2)
									{
										Game1.playSound("cowboy_explosion");
										AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(-3, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
										this.noPickUpBox = new Rectangle(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize);
										if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
										{
											AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
										}
										AbigailGame.screenFlash = 200;
										for (int k = 0; k < 30; k++)
										{
											AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(512, 1696, 16, 16), 70f, 6, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
											{
												delayBeforeAnimationStart = k * 75
											});
											if (k % 4 == 0)
											{
												AbigailGame.addGuts(new Point(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize), AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), 7);
											}
											if (k % 4 == 0)
											{
												AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
												{
													delayBeforeAnimationStart = k * 75
												});
											}
											if (k % 3 == 0)
											{
												AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(544, 1728, 16, 16), 100f, 4, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
												{
													delayBeforeAnimationStart = k * 75
												});
											}
										}
									}
									else if (AbigailGame.whichWave != 12)
									{
										AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup((AbigailGame.world == 0) ? -1 : -2, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
										if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
										{
											AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
										}
										AbigailGame.map[8, 8] = 10;
										AbigailGame.screenFlash = 200;
										for (int l = 0; l < 15; l++)
										{
											AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
											{
												delayBeforeAnimationStart = l * 75
											});
										}
									}
								}
								AbigailGame.monsters.RemoveAt(j);
								Game1.playSound("Cowboy_monsterDie");
							}
							else
							{
								health2 = AbigailGame.monsters[j].health;
							}
							this.bullets[i].damage -= health - health2;
							if (this.bullets[i].damage <= 0)
							{
								this.bullets.RemoveAt(i);
								break;
							}
							break;
						}
						else
						{
							j--;
						}
					}
				}
			}
			for (int m = AbigailGame.enemyBullets.Count - 1; m >= 0; m--)
			{
				AbigailGame.CowboyBullet expr_8A4_cp_0_cp_0 = AbigailGame.enemyBullets[m];
				expr_8A4_cp_0_cp_0.position.X = expr_8A4_cp_0_cp_0.position.X + AbigailGame.enemyBullets[m].motion.X;
				AbigailGame.CowboyBullet expr_8D4_cp_0_cp_0 = AbigailGame.enemyBullets[m];
				expr_8D4_cp_0_cp_0.position.Y = expr_8D4_cp_0_cp_0.position.Y + AbigailGame.enemyBullets[m].motion.Y;
				if (AbigailGame.enemyBullets[m].position.X <= 0 || AbigailGame.enemyBullets[m].position.Y <= 0 || AbigailGame.enemyBullets[m].position.X >= 762 || AbigailGame.enemyBullets[m].position.Y >= 762)
				{
					AbigailGame.enemyBullets.RemoveAt(m);
				}
				else if (AbigailGame.map[(AbigailGame.enemyBullets[m].position.X + 6) / 16 / 3, (AbigailGame.enemyBullets[m].position.Y + 6) / 16 / 3] == 7)
				{
					AbigailGame.enemyBullets.RemoveAt(m);
				}
				else if (AbigailGame.playerInvincibleTimer <= 0 && AbigailGame.deathTimer <= 0f && this.playerBoundingBox.Intersects(new Rectangle(AbigailGame.enemyBullets[m].position.X, AbigailGame.enemyBullets[m].position.Y, 15, 15)))
				{
					this.playerDie();
					return;
				}
			}
		}

		public void playerDie()
		{
			AbigailGame.gopherRunning = false;
			AbigailGame.hasGopherAppeared = false;
			this.spawnQueue = new List<Point>[4];
			for (int i = 0; i < 4; i++)
			{
				this.spawnQueue[i] = new List<Point>();
			}
			AbigailGame.enemyBullets.Clear();
			if (!AbigailGame.shootoutLevel)
			{
				AbigailGame.powerups.Clear();
				AbigailGame.monsters.Clear();
			}
			this.died = true;
			this.activePowerups.Clear();
			AbigailGame.deathTimer = 3000f;
			if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
			{
				AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
			}
			AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 120f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate, false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
			AbigailGame.waveTimer = Math.Min(80000, AbigailGame.waveTimer + 10000);
			AbigailGame.betweenWaveTimer = 4000;
			this.lives--;
			AbigailGame.playerInvincibleTimer = 5000;
			if (AbigailGame.shootoutLevel)
			{
				this.playerPosition = new Vector2((float)(8 * AbigailGame.TileSize), (float)(3 * AbigailGame.TileSize));
				Game1.playSound("Cowboy_monsterDie");
			}
			else
			{
				this.playerPosition = new Vector2((float)(8 * AbigailGame.TileSize - AbigailGame.TileSize), (float)(8 * AbigailGame.TileSize));
				this.playerBoundingBox.X = (int)this.playerPosition.X;
				this.playerBoundingBox.Y = (int)this.playerPosition.Y;
				if (this.playerBoundingBox.Intersects(this.player2BoundingBox))
				{
					this.playerPosition.X = this.playerPosition.X - (float)(AbigailGame.TileSize * 3 / 2);
					this.player2deathtimer = (int)AbigailGame.deathTimer;
				}
				Game1.playSound("cowboy_dead");
			}
			if (this.lives < 0)
			{
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 550f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate, false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					alpha = 0.001f,
					endFunction = new TemporaryAnimatedSprite.endBehavior(this.afterPlayerDeathFunction)
				});
				AbigailGame.deathTimer *= 3f;
			}
		}

		public void afterPlayerDeathFunction(int extra)
		{
			if (this.lives < 0)
			{
				AbigailGame.gameOver = true;
				if (AbigailGame.overworldSong != null && !AbigailGame.overworldSong.IsPlaying)
				{
					AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
				}
				if (AbigailGame.outlawSong != null && !AbigailGame.outlawSong.IsPlaying)
				{
					AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
				}
				AbigailGame.monsters.Clear();
				AbigailGame.powerups.Clear();
				this.died = false;
				Game1.playSound("Cowboy_monsterDie");
				if (AbigailGame.playingWithAbigail && Game1.currentLocation.currentEvent != null)
				{
					this.unload();
					Game1.currentMinigame = null;
					Event expr_9C = Game1.currentLocation.currentEvent;
					int currentCommand = expr_9C.CurrentCommand;
					expr_9C.CurrentCommand = currentCommand + 1;
				}
			}
		}

		public void startAbigailPortrait(int whichExpression, string sayWhat)
		{
			if (this.abigailPortraitTimer <= 0)
			{
				this.abigailPortraitTimer = 6000;
				this.AbigailDialogue = sayWhat;
				this.abigailPortraitExpression = whichExpression;
				this.abigailPortraitYposition = Game1.graphics.GraphicsDevice.Viewport.Height;
				Game1.playSound("dwop");
			}
		}

		public void startNewRound()
		{
			this.gamerestartTimer = 2000;
			Game1.playSound("Cowboy_monsterDie");
			this.whichRound++;
		}

		public bool tick(GameTime time)
		{
			if (this.quit)
			{
				if (Game1.currentLocation != null && Game1.currentLocation.name.Equals("Saloon") && Game1.timeOfDay >= 1700)
				{
					Game1.changeMusicTrack("Saloon1");
				}
				return true;
			}
			if (AbigailGame.gameOver || AbigailGame.onStartMenu)
			{
				if (AbigailGame.startTimer > 0)
				{
					AbigailGame.startTimer -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.startTimer <= 0)
					{
						AbigailGame.onStartMenu = false;
					}
				}
				else
				{
					AbigailGame.startTimer = 1500;
				}
				return false;
			}
			if (this.gamerestartTimer > 0)
			{
				this.gamerestartTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gamerestartTimer <= 0)
				{
					this.unload();
					if (this.whichRound == 0 || !AbigailGame.endCutscene)
					{
						Game1.currentMinigame = new AbigailGame(false);
					}
					else
					{
						Game1.currentMinigame = new AbigailGame(this.coins, this.ammoLevel, this.bulletDamage, this.fireSpeedLevel, this.runSpeedLevel, this.lives, this.spreadPistol, this.whichRound);
					}
				}
			}
			if (this.fadethenQuitTimer > 0 && (float)this.abigailPortraitTimer <= 0f)
			{
				this.fadethenQuitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.fadethenQuitTimer <= 0)
				{
					if (Game1.currentLocation.currentEvent != null)
					{
						Event expr_165 = Game1.currentLocation.currentEvent;
						int num = expr_165.CurrentCommand;
						expr_165.CurrentCommand = num + 1;
						if (AbigailGame.beatLevelWithAbigail)
						{
							Game1.currentLocation.currentEvent.specialEventVariable1 = true;
						}
					}
					return true;
				}
			}
			if (this.abigailPortraitTimer > 0)
			{
				this.abigailPortraitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.abigailPortraitTimer > 1000 && this.abigailPortraitYposition > Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 4)
				{
					this.abigailPortraitYposition -= 16;
				}
				else if (this.abigailPortraitTimer <= 1000)
				{
					this.abigailPortraitYposition += 16;
				}
			}
			if (AbigailGame.endCutscene)
			{
				AbigailGame.endCutsceneTimer -= time.ElapsedGameTime.Milliseconds;
				if (AbigailGame.endCutsceneTimer < 0)
				{
					AbigailGame.endCutscenePhase++;
					if (AbigailGame.endCutscenePhase > 5)
					{
						AbigailGame.endCutscenePhase = 5;
					}
					switch (AbigailGame.endCutscenePhase)
					{
					case 1:
						Game1.getSteamAchievement("Achievement_PrairieKing");
						if (!this.died)
						{
							Game1.getSteamAchievement("Achievement_FectorsChallenge");
						}
						AbigailGame.endCutsceneTimer = 15500;
						Game1.playSound("Cowboy_singing");
						AbigailGame.map = this.getMap(-1);
						break;
					case 2:
						this.playerPosition = new Vector2(0f, (float)(8 * AbigailGame.TileSize));
						AbigailGame.endCutsceneTimer = 12000;
						break;
					case 3:
						AbigailGame.endCutsceneTimer = 5000;
						break;
					case 4:
						AbigailGame.endCutsceneTimer = 1000;
						break;
					case 5:
						if (Game1.oldKBState.GetPressedKeys().Length == 0)
						{
							GamePadState arg_310_0 = Game1.oldPadState;
							if (Game1.oldPadState.Buttons.X != ButtonState.Pressed && Game1.oldPadState.Buttons.Start != ButtonState.Pressed && Game1.oldPadState.Buttons.A != ButtonState.Pressed)
							{
								break;
							}
						}
						if (this.gamerestartTimer <= 0)
						{
							this.startNewRound();
						}
						break;
					}
				}
				if (AbigailGame.endCutscenePhase == 2 && this.playerPosition.X < (float)(9 * AbigailGame.TileSize))
				{
					this.playerPosition.X = this.playerPosition.X + 1f;
					this.playerMotionAnimationTimer += (float)time.ElapsedGameTime.Milliseconds;
					this.playerMotionAnimationTimer %= 400f;
				}
				return false;
			}
			if (this.motionPause > 0)
			{
				this.motionPause -= time.ElapsedGameTime.Milliseconds;
				if (this.motionPause <= 0 && this.behaviorAfterPause != null)
				{
					this.behaviorAfterPause();
					this.behaviorAfterPause = null;
				}
			}
			else if (AbigailGame.monsterConfusionTimer > 0)
			{
				AbigailGame.monsterConfusionTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.zombieModeTimer > 0)
			{
				AbigailGame.zombieModeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.holdItemTimer > 0)
			{
				AbigailGame.holdItemTimer -= time.ElapsedGameTime.Milliseconds;
				return false;
			}
			if (AbigailGame.screenFlash > 0)
			{
				AbigailGame.screenFlash -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.gopherTrain)
			{
				AbigailGame.gopherTrainPosition += 3;
				if (AbigailGame.gopherTrainPosition % 30 == 0)
				{
					Game1.playSound("Cowboy_Footstep");
				}
				if (AbigailGame.playerJumped)
				{
					this.playerPosition.Y = this.playerPosition.Y + 3f;
				}
				if (Math.Abs(this.playerPosition.Y - (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize)) <= 16f)
				{
					AbigailGame.playerJumped = true;
					this.playerPosition.Y = (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize);
				}
				if (AbigailGame.gopherTrainPosition > 16 * AbigailGame.TileSize + AbigailGame.TileSize)
				{
					AbigailGame.gopherTrain = false;
					AbigailGame.playerJumped = false;
					AbigailGame.whichWave++;
					AbigailGame.map = this.getMap(AbigailGame.whichWave);
					this.playerPosition = new Vector2((float)(8 * AbigailGame.TileSize), (float)(8 * AbigailGame.TileSize));
					AbigailGame.world = ((AbigailGame.world == 0) ? 2 : 1);
					AbigailGame.waveTimer = 80000;
					AbigailGame.betweenWaveTimer = 5000;
					AbigailGame.waitingForPlayerToMoveDownAMap = false;
					AbigailGame.shootoutLevel = false;
				}
			}
			if ((AbigailGame.shopping || AbigailGame.merchantArriving || AbigailGame.merchantLeaving || AbigailGame.waitingForPlayerToMoveDownAMap) && AbigailGame.holdItemTimer <= 0)
			{
				int num2 = AbigailGame.shoppingTimer;
				AbigailGame.shoppingTimer += time.ElapsedGameTime.Milliseconds;
				AbigailGame.shoppingTimer %= 500;
				if (!AbigailGame.merchantShopOpen && AbigailGame.shopping && ((num2 < 250 && AbigailGame.shoppingTimer >= 250) || num2 > AbigailGame.shoppingTimer))
				{
					Game1.playSound("Cowboy_Footstep");
				}
			}
			if (AbigailGame.playerInvincibleTimer > 0)
			{
				AbigailGame.playerInvincibleTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.scrollingMap)
			{
				AbigailGame.newMapPosition -= AbigailGame.TileSize / 8;
				this.playerPosition.Y = this.playerPosition.Y - (float)(AbigailGame.TileSize / 8);
				this.playerPosition.Y = this.playerPosition.Y + 3f;
				this.playerBoundingBox.X = (int)this.playerPosition.X + AbigailGame.TileSize / 4;
				this.playerBoundingBox.Y = (int)this.playerPosition.Y + AbigailGame.TileSize / 4;
				this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
				this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
				AbigailGame.playerMovementDirections = new List<int>
				{
					2
				};
				this.playerMotionAnimationTimer += (float)time.ElapsedGameTime.Milliseconds;
				this.playerMotionAnimationTimer %= 400f;
				if (AbigailGame.newMapPosition <= 0)
				{
					AbigailGame.scrollingMap = false;
					AbigailGame.map = AbigailGame.nextMap;
					AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
					AbigailGame.shopping = false;
					AbigailGame.betweenWaveTimer = 5000;
					AbigailGame.waitingForPlayerToMoveDownAMap = false;
					AbigailGame.playerMovementDirections.Clear();
					if (AbigailGame.whichWave == 12)
					{
						AbigailGame.shootoutLevel = true;
						AbigailGame.monsters.Add(new AbigailGame.Dracula());
						if (this.whichRound > 0)
						{
							AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health *= 2;
						}
					}
					else if (AbigailGame.whichWave % 4 == 0)
					{
						AbigailGame.shootoutLevel = true;
						AbigailGame.monsters.Add(new AbigailGame.Outlaw(new Point(8 * AbigailGame.TileSize, 13 * AbigailGame.TileSize), (AbigailGame.world == 0) ? 50 : 100));
						if (Game1.soundBank != null)
						{
							AbigailGame.outlawSong = Game1.soundBank.GetCue("cowboy_outlawsong");
							AbigailGame.outlawSong.Play();
						}
					}
				}
			}
			if (AbigailGame.gopherRunning)
			{
				AbigailGame.gopherBox.X = AbigailGame.gopherBox.X + this.gopherMotion.X;
				AbigailGame.gopherBox.Y = AbigailGame.gopherBox.Y + this.gopherMotion.Y;
				for (int i = AbigailGame.monsters.Count - 1; i >= 0; i--)
				{
					if (AbigailGame.gopherBox.Intersects(AbigailGame.monsters[i].position))
					{
						AbigailGame.addGuts(AbigailGame.monsters[i].position.Location, AbigailGame.monsters[i].type);
						AbigailGame.monsters.RemoveAt(i);
						Game1.playSound("Cowboy_monsterDie");
					}
				}
				if (AbigailGame.gopherBox.X < 0 || AbigailGame.gopherBox.Y < 0 || AbigailGame.gopherBox.X > 16 * AbigailGame.TileSize || AbigailGame.gopherBox.Y > 16 * AbigailGame.TileSize)
				{
					AbigailGame.gopherRunning = false;
				}
			}
			for (int j = AbigailGame.temporarySprites.Count - 1; j >= 0; j--)
			{
				if (AbigailGame.temporarySprites[j].update(time))
				{
					AbigailGame.temporarySprites.RemoveAt(j);
				}
			}
			if (this.motionPause <= 0)
			{
				for (int k = AbigailGame.powerups.Count - 1; k >= 0; k--)
				{
					if (Utility.distance((float)this.playerBoundingBox.Center.X, (float)(AbigailGame.powerups[k].position.X + AbigailGame.TileSize / 2), (float)this.playerBoundingBox.Center.Y, (float)(AbigailGame.powerups[k].position.Y + AbigailGame.TileSize / 2)) <= (float)(AbigailGame.TileSize + 3) && (AbigailGame.powerups[k].position.X < AbigailGame.TileSize || AbigailGame.powerups[k].position.X >= 16 * AbigailGame.TileSize - AbigailGame.TileSize || AbigailGame.powerups[k].position.Y < AbigailGame.TileSize || AbigailGame.powerups[k].position.Y >= 16 * AbigailGame.TileSize - AbigailGame.TileSize))
					{
						if (AbigailGame.powerups[k].position.X + AbigailGame.TileSize / 2 < this.playerBoundingBox.Center.X)
						{
							AbigailGame.CowboyPowerup expr_AB6_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_AB6_cp_0_cp_0.position.X = expr_AB6_cp_0_cp_0.position.X + 1;
						}
						if (AbigailGame.powerups[k].position.X + AbigailGame.TileSize / 2 > this.playerBoundingBox.Center.X)
						{
							AbigailGame.CowboyPowerup expr_B01_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_B01_cp_0_cp_0.position.X = expr_B01_cp_0_cp_0.position.X - 1;
						}
						if (AbigailGame.powerups[k].position.Y + AbigailGame.TileSize / 2 < this.playerBoundingBox.Center.Y)
						{
							AbigailGame.CowboyPowerup expr_B4C_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_B4C_cp_0_cp_0.position.Y = expr_B4C_cp_0_cp_0.position.Y + 1;
						}
						if (AbigailGame.powerups[k].position.Y + AbigailGame.TileSize / 2 > this.playerBoundingBox.Center.Y)
						{
							AbigailGame.CowboyPowerup expr_B97_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_B97_cp_0_cp_0.position.Y = expr_B97_cp_0_cp_0.position.Y - 1;
						}
					}
					AbigailGame.powerups[k].duration -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.powerups[k].duration <= 0)
					{
						AbigailGame.powerups.RemoveAt(k);
					}
				}
				for (int l = this.activePowerups.Count - 1; l >= 0; l--)
				{
					Dictionary<int, int> dictionary = this.activePowerups;
					int num = this.activePowerups.ElementAt(l).Key;
					dictionary[num] -= time.ElapsedGameTime.Milliseconds;
					if (this.activePowerups[this.activePowerups.ElementAt(l).Key] <= 0)
					{
						this.activePowerups.Remove(this.activePowerups.ElementAt(l).Key);
					}
				}
				if (AbigailGame.deathTimer <= 0f && AbigailGame.playerMovementDirections.Count > 0 && !AbigailGame.scrollingMap)
				{
					int num3 = AbigailGame.playerMovementDirections.Count;
					if (num3 >= 2 && AbigailGame.playerMovementDirections.Last<int>() == (AbigailGame.playerMovementDirections.ElementAt(AbigailGame.playerMovementDirections.Count - 2) + 2) % 4)
					{
						num3 = 1;
					}
					float num4 = this.getMovementSpeed(3f, num3);
					if (this.activePowerups.Keys.Contains(6))
					{
						num4 *= 1.5f;
					}
					if (AbigailGame.zombieModeTimer > 0)
					{
						num4 *= 1.5f;
					}
					for (int m = 0; m < this.runSpeedLevel; m++)
					{
						num4 *= 1.25f;
					}
					for (int n = Math.Max(0, AbigailGame.playerMovementDirections.Count - 2); n < AbigailGame.playerMovementDirections.Count; n++)
					{
						if (n != 0 || AbigailGame.playerMovementDirections.Count < 2 || AbigailGame.playerMovementDirections.Last<int>() != (AbigailGame.playerMovementDirections.ElementAt(AbigailGame.playerMovementDirections.Count - 2) + 2) % 4)
						{
							Vector2 vector = this.playerPosition;
							switch (AbigailGame.playerMovementDirections.ElementAt(n))
							{
							case 0:
								vector.Y -= num4;
								break;
							case 1:
								vector.X += num4;
								break;
							case 2:
								vector.Y += num4;
								break;
							case 3:
								vector.X -= num4;
								break;
							}
							Rectangle rectangle = new Rectangle((int)vector.X + AbigailGame.TileSize / 4, (int)vector.Y + AbigailGame.TileSize / 4, AbigailGame.TileSize / 2, AbigailGame.TileSize / 2);
							if (!AbigailGame.isCollidingWithMap(rectangle) && (!this.merchantBox.Intersects(rectangle) || this.merchantBox.Intersects(this.playerBoundingBox)) && (!AbigailGame.playingWithAbigail || !rectangle.Intersects(this.player2BoundingBox)))
							{
								this.playerPosition = vector;
							}
						}
					}
					this.playerBoundingBox.X = (int)this.playerPosition.X + AbigailGame.TileSize / 4;
					this.playerBoundingBox.Y = (int)this.playerPosition.Y + AbigailGame.TileSize / 4;
					this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
					this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
					this.playerMotionAnimationTimer += (float)time.ElapsedGameTime.Milliseconds;
					this.playerMotionAnimationTimer %= 400f;
					this.playerFootstepSoundTimer -= (float)time.ElapsedGameTime.Milliseconds;
					if (this.playerFootstepSoundTimer <= 0f)
					{
						Game1.playSound("Cowboy_Footstep");
						this.playerFootstepSoundTimer = 200f;
					}
					for (int num5 = AbigailGame.powerups.Count - 1; num5 >= 0; num5--)
					{
						if (this.playerBoundingBox.Intersects(new Rectangle(AbigailGame.powerups[num5].position.X, AbigailGame.powerups[num5].position.Y, AbigailGame.TileSize, AbigailGame.TileSize)) && !this.playerBoundingBox.Intersects(this.noPickUpBox))
						{
							if (this.heldItem != null)
							{
								this.usePowerup(AbigailGame.powerups[num5].which);
								AbigailGame.powerups.RemoveAt(num5);
							}
							else if (this.getPowerUp(AbigailGame.powerups[num5]))
							{
								AbigailGame.powerups.RemoveAt(num5);
							}
						}
					}
					if (!this.playerBoundingBox.Intersects(this.noPickUpBox))
					{
						this.noPickUpBox.Location = new Point(0, 0);
					}
					if (AbigailGame.waitingForPlayerToMoveDownAMap && this.playerBoundingBox.Bottom >= 16 * AbigailGame.TileSize - AbigailGame.TileSize / 2)
					{
						AbigailGame.shopping = false;
						AbigailGame.merchantArriving = false;
						AbigailGame.merchantLeaving = false;
						AbigailGame.merchantShopOpen = false;
						this.merchantBox.Y = -AbigailGame.TileSize;
						AbigailGame.scrollingMap = true;
						AbigailGame.nextMap = this.getMap(AbigailGame.whichWave);
						AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
						AbigailGame.temporarySprites.Clear();
						AbigailGame.powerups.Clear();
					}
					if (!this.shoppingCarpetNoPickup.Intersects(this.playerBoundingBox))
					{
						this.shoppingCarpetNoPickup.X = -1000;
					}
				}
				if (AbigailGame.shopping)
				{
					if (this.merchantBox.Y < 8 * AbigailGame.TileSize - AbigailGame.TileSize * 3 && AbigailGame.merchantArriving)
					{
						this.merchantBox.Y = this.merchantBox.Y + 2;
						if (this.merchantBox.Y >= 8 * AbigailGame.TileSize - AbigailGame.TileSize * 3)
						{
							AbigailGame.merchantShopOpen = true;
							Game1.playSound("cowboy_monsterhit");
							AbigailGame.map[8, 15] = 3;
							AbigailGame.map[7, 15] = 3;
							AbigailGame.map[7, 15] = 3;
							AbigailGame.map[8, 14] = 3;
							AbigailGame.map[7, 14] = 3;
							AbigailGame.map[7, 14] = 3;
							this.shoppingCarpetNoPickup = new Rectangle(this.merchantBox.X - AbigailGame.TileSize, this.merchantBox.Y + AbigailGame.TileSize, AbigailGame.TileSize * 3, AbigailGame.TileSize * 2);
						}
					}
					else if (AbigailGame.merchantLeaving)
					{
						this.merchantBox.Y = this.merchantBox.Y - 2;
						if (this.merchantBox.Y <= -AbigailGame.TileSize)
						{
							AbigailGame.shopping = false;
							AbigailGame.merchantLeaving = false;
							AbigailGame.merchantArriving = true;
						}
					}
					else if (AbigailGame.merchantShopOpen)
					{
						for (int num6 = this.storeItems.Count - 1; num6 >= 0; num6--)
						{
							if (!this.playerBoundingBox.Intersects(this.shoppingCarpetNoPickup) && this.playerBoundingBox.Intersects(this.storeItems.ElementAt(num6).Key) && this.coins >= this.getPriceForItem(this.storeItems.ElementAt(num6).Value))
							{
								Game1.playSound("Cowboy_Secret");
								AbigailGame.holdItemTimer = 2500;
								this.motionPause = 2500;
								AbigailGame.itemToHold = this.storeItems.ElementAt(num6).Value;
								this.storeItems.Remove(this.storeItems.ElementAt(num6).Key);
								AbigailGame.merchantLeaving = true;
								AbigailGame.merchantArriving = false;
								AbigailGame.merchantShopOpen = false;
								this.coins -= this.getPriceForItem(AbigailGame.itemToHold);
								switch (AbigailGame.itemToHold)
								{
								case 0:
								case 1:
								case 2:
									this.fireSpeedLevel++;
									break;
								case 3:
								case 4:
									this.runSpeedLevel++;
									break;
								case 5:
									this.lives++;
									break;
								case 6:
								case 7:
								case 8:
									this.ammoLevel++;
									this.bulletDamage++;
									break;
								case 9:
									this.spreadPistol = true;
									break;
								case 10:
									this.heldItem = new AbigailGame.CowboyPowerup(10, Point.Zero, 9999);
									break;
								}
							}
						}
					}
				}
				this.cactusDanceTimer += (float)time.ElapsedGameTime.Milliseconds;
				this.cactusDanceTimer %= 1600f;
				if (this.shotTimer > 0)
				{
					this.shotTimer -= time.ElapsedGameTime.Milliseconds;
				}
				if (AbigailGame.deathTimer <= 0f && AbigailGame.playerShootingDirections.Count > 0 && this.shotTimer <= 0)
				{
					if (this.activePowerups.ContainsKey(2))
					{
						this.spawnBullets(new int[1], this.playerPosition);
						this.spawnBullets(new int[]
						{
							1
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							2
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							3
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							0,
							1
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							1,
							2
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							2,
							3
						}, this.playerPosition);
						int[] expr_1542 = new int[2];
						expr_1542[0] = 3;
						this.spawnBullets(expr_1542, this.playerPosition);
					}
					else if (AbigailGame.playerShootingDirections.Count == 1 || AbigailGame.playerShootingDirections.Last<int>() == (AbigailGame.playerShootingDirections.ElementAt(AbigailGame.playerShootingDirections.Count - 2) + 2) % 4)
					{
						this.spawnBullets(new int[]
						{
							(AbigailGame.playerShootingDirections.Count == 2 && AbigailGame.playerShootingDirections.Last<int>() == (AbigailGame.playerShootingDirections.ElementAt(AbigailGame.playerShootingDirections.Count - 2) + 2) % 4) ? AbigailGame.playerShootingDirections.ElementAt(1) : AbigailGame.playerShootingDirections.ElementAt(0)
						}, this.playerPosition);
					}
					else
					{
						this.spawnBullets(AbigailGame.playerShootingDirections.ToArray(), this.playerPosition);
					}
					Game1.playSound("Cowboy_gunshot");
					this.shotTimer = this.shootingDelay;
					if (this.activePowerups.ContainsKey(3))
					{
						this.shotTimer /= 4;
					}
					for (int num7 = 0; num7 < this.fireSpeedLevel; num7++)
					{
						this.shotTimer = this.shotTimer * 3 / 4;
					}
					if (this.activePowerups.ContainsKey(7))
					{
						this.shotTimer = this.shotTimer * 3 / 2;
					}
					this.shotTimer = Math.Max(this.shotTimer, 20);
				}
				this.updateBullets(time);
				if (AbigailGame.waveTimer > 0 && AbigailGame.betweenWaveTimer <= 0 && AbigailGame.zombieModeTimer <= 0 && !AbigailGame.shootoutLevel && (AbigailGame.overworldSong == null || !AbigailGame.overworldSong.IsPlaying) && Game1.soundBank != null)
				{
					AbigailGame.overworldSong = Game1.soundBank.GetCue("Cowboy_OVERWORLD");
					AbigailGame.overworldSong.Play();
					Game1.musicPlayerVolume = Game1.options.musicVolumeLevel;
					Game1.musicCategory.SetVolume(Game1.musicPlayerVolume);
				}
				if (AbigailGame.deathTimer > 0f)
				{
					AbigailGame.deathTimer -= (float)time.ElapsedGameTime.Milliseconds;
				}
				if (AbigailGame.betweenWaveTimer > 0 && AbigailGame.monsters.Count == 0 && this.isSpawnQueueEmpty() && !AbigailGame.shopping && !AbigailGame.waitingForPlayerToMoveDownAMap)
				{
					AbigailGame.betweenWaveTimer -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.betweenWaveTimer <= 0 && AbigailGame.playingWithAbigail)
					{
						this.startAbigailPortrait(7, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11896", new object[0]));
					}
				}
				else if (AbigailGame.deathTimer <= 0f && !AbigailGame.waitingForPlayerToMoveDownAMap && !AbigailGame.shopping && !AbigailGame.shootoutLevel)
				{
					if (AbigailGame.waveTimer > 0)
					{
						int num8 = AbigailGame.waveTimer;
						AbigailGame.waveTimer -= time.ElapsedGameTime.Milliseconds;
						if (AbigailGame.playingWithAbigail && num8 > 40000 && AbigailGame.waveTimer <= 40000)
						{
							this.startAbigailPortrait(0, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11897", new object[0]));
						}
						int num9 = 0;
						foreach (Vector2 current in this.monsterChances)
						{
							if (Game1.random.NextDouble() < (double)(current.X * (float)((AbigailGame.monsters.Count == 0) ? 2 : 1)))
							{
								int num10 = 1;
								while (Game1.random.NextDouble() < (double)current.Y && num10 < 15)
								{
									num10++;
								}
								this.spawnQueue[(AbigailGame.whichWave == 11) ? (Game1.random.Next(1, 3) * 2 - 1) : Game1.random.Next(4)].Add(new Point(num9, num10));
							}
							num9++;
						}
						if (!AbigailGame.hasGopherAppeared && AbigailGame.monsters.Count > 6 && Game1.random.NextDouble() < 0.0004 && AbigailGame.waveTimer > 7000 && AbigailGame.waveTimer < 50000)
						{
							AbigailGame.hasGopherAppeared = true;
							AbigailGame.gopherBox = new Rectangle(Game1.random.Next(16 * AbigailGame.TileSize), Game1.random.Next(16 * AbigailGame.TileSize), AbigailGame.TileSize, AbigailGame.TileSize);
							while (AbigailGame.isCollidingWithMap(AbigailGame.gopherBox) || AbigailGame.isCollidingWithMonster(AbigailGame.gopherBox, null) || Math.Abs((float)AbigailGame.gopherBox.X - this.playerPosition.X) < (float)(AbigailGame.TileSize * 6) || Math.Abs((float)AbigailGame.gopherBox.Y - this.playerPosition.Y) < (float)(AbigailGame.TileSize * 6) || Math.Abs(AbigailGame.gopherBox.X - 8 * AbigailGame.TileSize) < AbigailGame.TileSize * 4 || Math.Abs(AbigailGame.gopherBox.Y - 8 * AbigailGame.TileSize) < AbigailGame.TileSize * 4)
							{
								AbigailGame.gopherBox.X = Game1.random.Next(16 * AbigailGame.TileSize);
								AbigailGame.gopherBox.Y = Game1.random.Next(16 * AbigailGame.TileSize);
							}
							AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(256, 1664, 16, 32), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.gopherBox.X + AbigailGame.TileSize / 2), (float)(AbigailGame.gopherBox.Y - AbigailGame.TileSize + AbigailGame.TileSize / 2)), false, false, (float)AbigailGame.gopherBox.Y / 10000f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
							{
								endFunction = new TemporaryAnimatedSprite.endBehavior(this.endOfGopherAnimationBehavior)
							});
						}
					}
					for (int num11 = 0; num11 < 4; num11++)
					{
						if (this.spawnQueue[num11].Count > 0)
						{
							if (this.spawnQueue[num11][0].X == 1 || this.spawnQueue[num11][0].X == 4)
							{
								List<Vector2> borderOfThisRectangle = Utility.getBorderOfThisRectangle(new Rectangle(0, 0, 16, 16));
								Vector2 vector2 = borderOfThisRectangle.ElementAt(Game1.random.Next(borderOfThisRectangle.Count));
								while (AbigailGame.isCollidingWithMonster(new Rectangle((int)vector2.X * AbigailGame.TileSize, (int)vector2.Y * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize), null))
								{
									vector2 = borderOfThisRectangle.ElementAt(Game1.random.Next(borderOfThisRectangle.Count));
								}
								AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[num11][0].X, new Point((int)vector2.X * AbigailGame.TileSize, (int)vector2.Y * AbigailGame.TileSize)));
								if (this.whichRound > 0)
								{
									AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
								}
								this.spawnQueue[num11][0] = new Point(this.spawnQueue[num11][0].X, this.spawnQueue[num11][0].Y - 1);
								if (this.spawnQueue[num11][0].Y <= 0)
								{
									this.spawnQueue[num11].RemoveAt(0);
								}
							}
							else
							{
								switch (num11)
								{
								case 0:
								{
									int num12 = 7;
									while (num12 < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(num12 * 16 * 3, 0, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[num11].First<Point>().X, new Point(num12 * AbigailGame.TileSize, 0)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[num11][0] = new Point(this.spawnQueue[num11][0].X, this.spawnQueue[num11][0].Y - 1);
											if (this.spawnQueue[num11][0].Y <= 0)
											{
												this.spawnQueue[num11].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											num12++;
										}
									}
									break;
								}
								case 1:
								{
									int num13 = 7;
									while (num13 < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(720, num13 * AbigailGame.TileSize, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[num11].First<Point>().X, new Point(15 * AbigailGame.TileSize, num13 * AbigailGame.TileSize)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[num11][0] = new Point(this.spawnQueue[num11][0].X, this.spawnQueue[num11][0].Y - 1);
											if (this.spawnQueue[num11][0].Y <= 0)
											{
												this.spawnQueue[num11].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											num13++;
										}
									}
									break;
								}
								case 2:
								{
									int num14 = 7;
									while (num14 < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(num14 * 16 * 3, 15 * AbigailGame.TileSize, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[num11].First<Point>().X, new Point(num14 * AbigailGame.TileSize, 15 * AbigailGame.TileSize)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[num11][0] = new Point(this.spawnQueue[num11][0].X, this.spawnQueue[num11][0].Y - 1);
											if (this.spawnQueue[num11][0].Y <= 0)
											{
												this.spawnQueue[num11].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											num14++;
										}
									}
									break;
								}
								case 3:
								{
									int num15 = 7;
									while (num15 < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(0, num15 * AbigailGame.TileSize, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[num11].First<Point>().X, new Point(0, num15 * AbigailGame.TileSize)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[num11][0] = new Point(this.spawnQueue[num11][0].X, this.spawnQueue[num11][0].Y - 1);
											if (this.spawnQueue[num11][0].Y <= 0)
											{
												this.spawnQueue[num11].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											num15++;
										}
									}
									break;
								}
								}
							}
						}
					}
					if (AbigailGame.waveTimer <= 0 && AbigailGame.monsters.Count > 0 && this.isSpawnQueueEmpty())
					{
						bool flag = true;
						using (List<AbigailGame.CowboyMonster>.Enumerator enumerator2 = AbigailGame.monsters.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.type != 6)
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							using (List<AbigailGame.CowboyMonster>.Enumerator enumerator2 = AbigailGame.monsters.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									enumerator2.Current.health = 1;
								}
							}
						}
					}
					if (AbigailGame.waveTimer <= 0 && AbigailGame.monsters.Count == 0 && this.isSpawnQueueEmpty())
					{
						AbigailGame.hasGopherAppeared = false;
						if (AbigailGame.playingWithAbigail)
						{
							this.startAbigailPortrait(1, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11898", new object[0]));
						}
						AbigailGame.waveTimer = 80000;
						AbigailGame.betweenWaveTimer = 3333;
						AbigailGame.whichWave++;
						if (AbigailGame.playingWithAbigail)
						{
							AbigailGame.beatLevelWithAbigail = true;
							this.fadethenQuitTimer = 2000;
						}
						switch (AbigailGame.whichWave)
						{
						case 1:
						case 2:
						case 3:
							this.monsterChances[0] = new Vector2(this.monsterChances[0].X + 0.001f, this.monsterChances[0].Y + 0.02f);
							if (AbigailGame.whichWave > 1)
							{
								this.monsterChances[2] = new Vector2(this.monsterChances[2].X + 0.001f, this.monsterChances[2].Y + 0.01f);
							}
							this.monsterChances[6] = new Vector2(this.monsterChances[6].X + 0.001f, this.monsterChances[6].Y + 0.01f);
							if (this.whichRound > 0)
							{
								this.monsterChances[4] = new Vector2(0.002f, 0.1f);
							}
							break;
						case 4:
						case 5:
						case 6:
						case 7:
							if (this.monsterChances[5].Equals(Vector2.Zero))
							{
								this.monsterChances[5] = new Vector2(0.01f, 0.15f);
								if (this.whichRound > 0)
								{
									this.monsterChances[5] = new Vector2(0.01f + (float)this.whichRound * 0.004f, 0.15f + (float)this.whichRound * 0.04f);
								}
							}
							this.monsterChances[0] = Vector2.Zero;
							this.monsterChances[6] = Vector2.Zero;
							this.monsterChances[2] = new Vector2(this.monsterChances[2].X + 0.002f, this.monsterChances[2].Y + 0.02f);
							this.monsterChances[5] = new Vector2(this.monsterChances[5].X + 0.001f, this.monsterChances[5].Y + 0.02f);
							this.monsterChances[1] = new Vector2(this.monsterChances[1].X + 0.0018f, this.monsterChances[1].Y + 0.08f);
							if (this.whichRound > 0)
							{
								this.monsterChances[4] = new Vector2(0.001f, 0.1f);
							}
							break;
						case 8:
						case 9:
						case 10:
						case 11:
							this.monsterChances[5] = Vector2.Zero;
							this.monsterChances[1] = Vector2.Zero;
							this.monsterChances[2] = Vector2.Zero;
							if (this.monsterChances[3].Equals(Vector2.Zero))
							{
								this.monsterChances[3] = new Vector2(0.012f, 0.4f);
								if (this.whichRound > 0)
								{
									this.monsterChances[3] = new Vector2(0.012f + (float)this.whichRound * 0.005f, 0.4f + (float)this.whichRound * 0.075f);
								}
							}
							if (this.monsterChances[4].Equals(Vector2.Zero))
							{
								this.monsterChances[4] = new Vector2(0.003f, 0.1f);
							}
							this.monsterChances[3] = new Vector2(this.monsterChances[3].X + 0.002f, this.monsterChances[3].Y + 0.05f);
							this.monsterChances[4] = new Vector2(this.monsterChances[4].X + 0.0015f, this.monsterChances[4].Y + 0.04f);
							if (AbigailGame.whichWave == 11)
							{
								this.monsterChances[4] = new Vector2(this.monsterChances[4].X + 0.01f, this.monsterChances[4].Y + 0.04f);
								this.monsterChances[3] = new Vector2(this.monsterChances[3].X - 0.01f, this.monsterChances[3].Y + 0.04f);
							}
							break;
						}
						if (this.whichRound > 0)
						{
							for (int num16 = 0; num16 < this.monsterChances.Count<Vector2>(); num16++)
							{
								Vector2 arg_2711_0 = this.monsterChances[num16];
								List<Vector2> list = this.monsterChances;
								int num = num16;
								list[num] *= 1.1f;
							}
						}
						if (AbigailGame.whichWave > 0 && AbigailGame.whichWave % 2 == 0)
						{
							this.startShoppingLevel();
						}
						else if (AbigailGame.whichWave > 0)
						{
							AbigailGame.waitingForPlayerToMoveDownAMap = true;
							if (!AbigailGame.playingWithAbigail)
							{
								AbigailGame.map[8, 15] = 3;
								AbigailGame.map[7, 15] = 3;
								AbigailGame.map[9, 15] = 3;
							}
						}
					}
				}
				if (AbigailGame.playingWithAbigail)
				{
					this.updateAbigail(time);
				}
				for (int num17 = AbigailGame.monsters.Count - 1; num17 >= 0; num17--)
				{
					AbigailGame.monsters[num17].move(this.playerPosition, time);
					if (num17 < AbigailGame.monsters.Count && AbigailGame.monsters[num17].position.Intersects(this.playerBoundingBox) && AbigailGame.playerInvincibleTimer <= 0)
					{
						if (AbigailGame.zombieModeTimer <= 0)
						{
							this.playerDie();
							break;
						}
						AbigailGame.addGuts(AbigailGame.monsters[num17].position.Location, AbigailGame.monsters[num17].type);
						AbigailGame.monsters.RemoveAt(num17);
						Game1.playSound("Cowboy_monsterDie");
					}
					if (AbigailGame.playingWithAbigail && num17 < AbigailGame.monsters.Count && AbigailGame.monsters[num17].position.Intersects(this.player2BoundingBox) && this.player2invincibletimer <= 0)
					{
						Game1.playSound("Cowboy_monsterDie");
						this.player2deathtimer = 3000;
						AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 120f, 5, 0, AbigailGame.player2Position + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
						this.player2invincibletimer = 4000;
						AbigailGame.player2Position = new Vector2(8f, 8f) * (float)AbigailGame.TileSize;
						this.startAbigailPortrait(5, (Game1.random.NextDouble() < 0.5) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11901", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11902", new object[0]));
					}
				}
			}
			return false;
		}

		public void updateAbigail(GameTime time)
		{
			this.player2TargetUpdateTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.player2deathtimer > 0)
			{
				this.player2deathtimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.player2invincibletimer > 0)
			{
				this.player2invincibletimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.player2deathtimer <= 0)
			{
				if (this.player2TargetUpdateTimer < 0)
				{
					this.player2TargetUpdateTimer = 500;
					AbigailGame.CowboyMonster cowboyMonster = null;
					double num = 99999.0;
					foreach (AbigailGame.CowboyMonster current in AbigailGame.monsters)
					{
						double num2 = Math.Sqrt(Math.Pow((double)((float)current.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)current.position.Y - AbigailGame.player2Position.Y), 2.0));
						if (cowboyMonster == null || num2 < num)
						{
							cowboyMonster = current;
							num = Math.Sqrt(Math.Pow((double)((float)cowboyMonster.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)cowboyMonster.position.Y - AbigailGame.player2Position.Y), 2.0));
						}
					}
					this.targetMonster = cowboyMonster;
				}
				this.player2ShootingDirections.Clear();
				this.player2MovementDirections.Clear();
				if (this.targetMonster != null)
				{
					if (Math.Sqrt(Math.Pow((double)((float)this.targetMonster.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)this.targetMonster.position.Y - AbigailGame.player2Position.Y), 2.0)) < (double)(AbigailGame.TileSize * 3))
					{
						if ((float)this.targetMonster.position.X > AbigailGame.player2Position.X)
						{
							this.addPlayer2MovementDirection(3);
						}
						else if ((float)this.targetMonster.position.X < AbigailGame.player2Position.X)
						{
							this.addPlayer2MovementDirection(1);
						}
						if ((float)this.targetMonster.position.Y > AbigailGame.player2Position.Y)
						{
							this.addPlayer2MovementDirection(0);
						}
						else if ((float)this.targetMonster.position.Y < AbigailGame.player2Position.Y)
						{
							this.addPlayer2MovementDirection(2);
						}
						using (List<int>.Enumerator enumerator2 = this.player2MovementDirections.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								int current2 = enumerator2.Current;
								this.player2ShootingDirections.Add((current2 + 2) % 4);
							}
							goto IL_4DC;
						}
					}
					if (Math.Abs((float)this.targetMonster.position.X - AbigailGame.player2Position.X) > Math.Abs((float)this.targetMonster.position.Y - AbigailGame.player2Position.Y) && Math.Abs((float)this.targetMonster.position.Y - AbigailGame.player2Position.Y) > 4f)
					{
						if ((float)this.targetMonster.position.Y > AbigailGame.player2Position.Y + 3f)
						{
							this.addPlayer2MovementDirection(2);
						}
						else if ((float)this.targetMonster.position.Y < AbigailGame.player2Position.Y - 3f)
						{
							this.addPlayer2MovementDirection(0);
						}
					}
					else if (Math.Abs((float)this.targetMonster.position.X - AbigailGame.player2Position.X) > 4f)
					{
						if ((float)this.targetMonster.position.X > AbigailGame.player2Position.X + 3f)
						{
							this.addPlayer2MovementDirection(1);
						}
						else if ((float)this.targetMonster.position.X < AbigailGame.player2Position.X - 3f)
						{
							this.addPlayer2MovementDirection(3);
						}
					}
					if ((float)this.targetMonster.position.X > AbigailGame.player2Position.X + 3f)
					{
						this.addPlayer2ShootingDirection(1);
					}
					else if ((float)this.targetMonster.position.X < AbigailGame.player2Position.X - 3f)
					{
						this.addPlayer2ShootingDirection(3);
					}
					if ((float)this.targetMonster.position.Y > AbigailGame.player2Position.Y + 3f)
					{
						this.addPlayer2ShootingDirection(2);
					}
					else if ((float)this.targetMonster.position.Y < AbigailGame.player2Position.Y - 3f)
					{
						this.addPlayer2ShootingDirection(0);
					}
				}
				IL_4DC:
				if (this.player2MovementDirections.Count > 0)
				{
					float movementSpeed = this.getMovementSpeed(3f, this.player2MovementDirections.Count);
					for (int i = 0; i < this.player2MovementDirections.Count; i++)
					{
						Vector2 vector = AbigailGame.player2Position;
						switch (this.player2MovementDirections[i])
						{
						case 0:
							vector.Y -= movementSpeed;
							break;
						case 1:
							vector.X += movementSpeed;
							break;
						case 2:
							vector.Y += movementSpeed;
							break;
						case 3:
							vector.X -= movementSpeed;
							break;
						}
						Rectangle rectangle = new Rectangle((int)vector.X + AbigailGame.TileSize / 4, (int)vector.Y + AbigailGame.TileSize / 4, AbigailGame.TileSize / 2, AbigailGame.TileSize / 2);
						if (!AbigailGame.isCollidingWithMap(rectangle) && (!this.merchantBox.Intersects(rectangle) || this.merchantBox.Intersects(this.player2BoundingBox)) && !rectangle.Intersects(this.playerBoundingBox))
						{
							AbigailGame.player2Position = vector;
						}
					}
					this.player2BoundingBox.X = (int)AbigailGame.player2Position.X + AbigailGame.TileSize / 4;
					this.player2BoundingBox.Y = (int)AbigailGame.player2Position.Y + AbigailGame.TileSize / 4;
					this.player2BoundingBox.Width = AbigailGame.TileSize / 2;
					this.player2BoundingBox.Height = AbigailGame.TileSize / 2;
					this.player2AnimationTimer += time.ElapsedGameTime.Milliseconds;
					this.player2AnimationTimer %= 400;
					this.player2FootstepSoundTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.player2FootstepSoundTimer <= 0)
					{
						Game1.playSound("Cowboy_Footstep");
						this.player2FootstepSoundTimer = 200;
					}
					for (int j = AbigailGame.powerups.Count - 1; j >= 0; j--)
					{
						if (this.player2BoundingBox.Intersects(new Rectangle(AbigailGame.powerups[j].position.X, AbigailGame.powerups[j].position.Y, AbigailGame.TileSize, AbigailGame.TileSize)) && !this.player2BoundingBox.Intersects(this.noPickUpBox))
						{
							AbigailGame.powerups.RemoveAt(j);
						}
					}
				}
				this.player2shotTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.player2ShootingDirections.Count > 0 && this.player2shotTimer <= 0)
				{
					if (this.player2ShootingDirections.Count == 1)
					{
						this.spawnBullets(new int[]
						{
							this.player2ShootingDirections[0]
						}, AbigailGame.player2Position);
					}
					else
					{
						this.spawnBullets(this.player2ShootingDirections.ToArray(), AbigailGame.player2Position);
					}
					Game1.playSound("Cowboy_gunshot");
					this.player2shotTimer = this.shootingDelay;
				}
			}
		}

		public int[,] getMap(int wave)
		{
			int[,] array = new int[16, 16];
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if ((i == 0 || i == 15 || j == 0 || j == 15) && (i <= 6 || i >= 10) && (j <= 6 || j >= 10))
					{
						array[i, j] = 5;
					}
					else if (i == 0 || i == 15 || j == 0 || j == 15)
					{
						array[i, j] = ((Game1.random.NextDouble() < 0.15) ? 1 : 0);
					}
					else if (i == 1 || i == 14 || j == 1 || j == 14)
					{
						array[i, j] = 2;
					}
					else
					{
						array[i, j] = ((Game1.random.NextDouble() < 0.1) ? 4 : 3);
					}
				}
			}
			switch (wave)
			{
			case -1:
				for (int k = 0; k < 16; k++)
				{
					for (int l = 0; l < 16; l++)
					{
						if (array[k, l] == 0 || array[k, l] == 1 || array[k, l] == 2 || array[k, l] == 5)
						{
							array[k, l] = 3;
						}
					}
				}
				array[3, 1] = 5;
				array[8, 2] = 5;
				array[13, 1] = 5;
				array[5, 0] = 0;
				array[10, 2] = 2;
				array[15, 2] = 1;
				array[14, 12] = 5;
				array[10, 6] = 7;
				array[11, 6] = 7;
				array[12, 6] = 7;
				array[13, 6] = 7;
				array[14, 6] = 7;
				array[14, 7] = 7;
				array[14, 8] = 7;
				array[14, 9] = 7;
				array[14, 10] = 7;
				array[14, 11] = 7;
				array[14, 12] = 7;
				array[14, 13] = 7;
				for (int m = 0; m < 16; m++)
				{
					array[m, 3] = ((m % 2 == 0) ? 9 : 8);
				}
				array[3, 3] = 10;
				array[7, 8] = 2;
				array[8, 8] = 2;
				array[4, 11] = 2;
				array[11, 12] = 2;
				array[9, 11] = 2;
				array[3, 9] = 2;
				array[2, 12] = 5;
				array[8, 13] = 5;
				array[12, 11] = 5;
				array[7, 14] = 0;
				array[6, 14] = 2;
				array[8, 14] = 2;
				array[7, 13] = 2;
				array[7, 15] = 2;
				return array;
			case 1:
				array[4, 4] = 7;
				array[4, 5] = 7;
				array[5, 4] = 7;
				array[12, 4] = 7;
				array[11, 4] = 7;
				array[12, 5] = 7;
				array[4, 12] = 7;
				array[5, 12] = 7;
				array[4, 11] = 7;
				array[12, 12] = 7;
				array[11, 12] = 7;
				array[12, 11] = 7;
				return array;
			case 2:
				array[8, 4] = 7;
				array[12, 8] = 7;
				array[8, 12] = 7;
				array[4, 8] = 7;
				array[1, 1] = 5;
				array[14, 1] = 5;
				array[14, 14] = 5;
				array[1, 14] = 5;
				array[2, 1] = 5;
				array[13, 1] = 5;
				array[13, 14] = 5;
				array[2, 14] = 5;
				array[1, 2] = 5;
				array[14, 2] = 5;
				array[14, 13] = 5;
				array[1, 13] = 5;
				return array;
			case 3:
				array[5, 5] = 7;
				array[6, 5] = 7;
				array[7, 5] = 7;
				array[9, 5] = 7;
				array[10, 5] = 7;
				array[11, 5] = 7;
				array[5, 11] = 7;
				array[6, 11] = 7;
				array[7, 11] = 7;
				array[9, 11] = 7;
				array[10, 11] = 7;
				array[11, 11] = 7;
				array[5, 6] = 7;
				array[5, 7] = 7;
				array[5, 9] = 7;
				array[5, 10] = 7;
				array[11, 6] = 7;
				array[11, 7] = 7;
				array[11, 9] = 7;
				array[11, 10] = 7;
				return array;
			case 4:
			case 8:
				for (int n = 0; n < 16; n++)
				{
					for (int num = 0; num < 16; num++)
					{
						if (array[n, num] == 5)
						{
							array[n, num] = ((Game1.random.NextDouble() < 0.5) ? 0 : 1);
						}
					}
				}
				for (int num2 = 0; num2 < 16; num2++)
				{
					array[num2, 8] = ((Game1.random.NextDouble() < 0.5) ? 8 : 9);
				}
				array[8, 4] = 7;
				array[8, 12] = 7;
				array[9, 12] = 7;
				array[7, 12] = 7;
				array[5, 6] = 5;
				array[10, 6] = 5;
				return array;
			case 5:
				array[1, 1] = 5;
				array[14, 1] = 5;
				array[14, 14] = 5;
				array[1, 14] = 5;
				array[2, 1] = 5;
				array[13, 1] = 5;
				array[13, 14] = 5;
				array[2, 14] = 5;
				array[1, 2] = 5;
				array[14, 2] = 5;
				array[14, 13] = 5;
				array[1, 13] = 5;
				array[3, 1] = 5;
				array[13, 1] = 5;
				array[13, 13] = 5;
				array[1, 13] = 5;
				array[1, 3] = 5;
				array[13, 3] = 5;
				array[12, 13] = 5;
				array[3, 14] = 5;
				array[3, 3] = 5;
				array[13, 12] = 5;
				array[13, 12] = 5;
				array[3, 12] = 5;
				return array;
			case 6:
				array[4, 5] = 2;
				array[12, 10] = 5;
				array[10, 9] = 5;
				array[5, 12] = 2;
				array[5, 9] = 5;
				array[12, 12] = 5;
				array[3, 4] = 5;
				array[2, 3] = 5;
				array[11, 3] = 5;
				array[10, 6] = 5;
				array[5, 9] = 7;
				array[10, 12] = 7;
				array[3, 12] = 7;
				array[10, 8] = 7;
				return array;
			case 7:
				for (int num3 = 0; num3 < 16; num3++)
				{
					array[num3, 5] = ((num3 % 2 == 0) ? 9 : 8);
					array[num3, 10] = ((num3 % 2 == 0) ? 9 : 8);
				}
				array[4, 5] = 10;
				array[8, 5] = 10;
				array[12, 5] = 10;
				array[4, 10] = 10;
				array[8, 10] = 10;
				array[12, 10] = 10;
				return array;
			case 9:
				array[4, 4] = 5;
				array[5, 4] = 5;
				array[10, 4] = 5;
				array[12, 4] = 5;
				array[4, 5] = 5;
				array[5, 5] = 5;
				array[10, 5] = 5;
				array[12, 5] = 5;
				array[4, 10] = 5;
				array[5, 10] = 5;
				array[10, 10] = 5;
				array[12, 10] = 5;
				array[4, 12] = 5;
				array[5, 12] = 5;
				array[10, 12] = 5;
				array[12, 12] = 5;
				return array;
			case 10:
				for (int num4 = 0; num4 < 16; num4++)
				{
					array[num4, 1] = ((num4 % 2 == 0) ? 9 : 8);
					array[num4, 14] = ((num4 % 2 == 0) ? 9 : 8);
				}
				array[8, 1] = 10;
				array[7, 1] = 10;
				array[9, 1] = 10;
				array[8, 14] = 10;
				array[7, 14] = 10;
				array[9, 14] = 10;
				array[6, 8] = 5;
				array[10, 8] = 5;
				array[8, 6] = 5;
				array[8, 9] = 5;
				return array;
			case 11:
				for (int num5 = 0; num5 < 16; num5++)
				{
					array[num5, 0] = 7;
					array[num5, 15] = 7;
					if (num5 % 2 == 0)
					{
						array[num5, 1] = 5;
						array[num5, 14] = 5;
					}
				}
				return array;
			case 12:
			{
				for (int num6 = 0; num6 < 16; num6++)
				{
					for (int num7 = 0; num7 < 16; num7++)
					{
						if (array[num6, num7] == 0 || array[num6, num7] == 1)
						{
							array[num6, num7] = 5;
						}
					}
				}
				for (int num8 = 0; num8 < 16; num8++)
				{
					array[num8, 0] = ((num8 % 2 == 0) ? 9 : 8);
					array[num8, 15] = ((num8 % 2 == 0) ? 9 : 8);
				}
				Rectangle r = new Rectangle(1, 1, 14, 14);
				foreach (Vector2 current in Utility.getBorderOfThisRectangle(r))
				{
					array[(int)current.X, (int)current.Y] = 10;
				}
				r.Inflate(-1, -1);
				using (List<Vector2>.Enumerator enumerator = Utility.getBorderOfThisRectangle(r).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Vector2 current2 = enumerator.Current;
						array[(int)current2.X, (int)current2.Y] = 2;
					}
					return array;
				}
				break;
			}
			}
			array[4, 4] = 5;
			array[12, 4] = 5;
			array[4, 12] = 5;
			array[12, 12] = 5;
			return array;
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public void leftClickHeld(int x, int y)
		{
		}

		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public void releaseLeftClick(int x, int y)
		{
		}

		public void releaseRightClick(int x, int y)
		{
		}

		public void spawnBullets(int[] directions, Vector2 spawn)
		{
			Point point = new Point((int)spawn.X + 24, (int)spawn.Y + 24 - 6);
			int num = (int)this.getMovementSpeed(8f, 2);
			if (directions.Length == 1)
			{
				int direction = directions[0];
				switch (direction)
				{
				case 0:
					point.Y -= 22;
					break;
				case 1:
					point.X += 16;
					point.Y -= 6;
					break;
				case 2:
					point.Y += 10;
					break;
				case 3:
					point.X -= 16;
					point.Y -= 6;
					break;
				}
				this.bullets.Add(new AbigailGame.CowboyBullet(point, direction, this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					switch (direction)
					{
					case 0:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(-2, -8), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(2, -8), this.bulletDamage));
						return;
					case 1:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(8, -2), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(8, 2), this.bulletDamage));
						return;
					case 2:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(-2, 8), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(2, 8), this.bulletDamage));
						return;
					case 3:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(-8, -2), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(point.X, point.Y), new Point(-8, 2), this.bulletDamage));
						return;
					default:
						return;
					}
				}
			}
			else if (directions.Contains(0) && directions.Contains(1))
			{
				point.X += AbigailGame.TileSize / 2;
				point.Y -= AbigailGame.TileSize / 2;
				this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(num, -num), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int num2 = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(num + num2, -num + num2), this.bulletDamage));
					num2 = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(num + num2, -num + num2), this.bulletDamage));
					return;
				}
			}
			else if (directions.Contains(0) && directions.Contains(3))
			{
				point.X -= AbigailGame.TileSize / 2;
				point.Y -= AbigailGame.TileSize / 2;
				this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(-num, -num), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int num3 = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(-num - num3, -num + num3), this.bulletDamage));
					num3 = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(-num - num3, -num + num3), this.bulletDamage));
					return;
				}
			}
			else if (directions.Contains(2) && directions.Contains(1))
			{
				point.X += AbigailGame.TileSize / 2;
				point.Y += AbigailGame.TileSize / 4;
				this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(num, num), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int num4 = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(num - num4, num + num4), this.bulletDamage));
					num4 = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(num - num4, num + num4), this.bulletDamage));
					return;
				}
			}
			else if (directions.Contains(2) && directions.Contains(3))
			{
				point.X -= AbigailGame.TileSize / 2;
				point.Y += AbigailGame.TileSize / 4;
				this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(-num, num), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int num5 = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(-num + num5, num + num5), this.bulletDamage));
					num5 = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(point, new Point(-num + num5, num + num5), this.bulletDamage));
				}
			}
		}

		public bool isSpawnQueueEmpty()
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.spawnQueue[i].Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		public static bool isMapTilePassable(int tileType)
		{
			switch (tileType)
			{
			case 0:
			case 1:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				return false;
			}
			return true;
		}

		public static bool isMapTilePassableForMonsters(int tileType)
		{
			switch (tileType)
			{
			case 5:
			case 7:
			case 8:
			case 9:
				return false;
			}
			return true;
		}

		public static bool isCollidingWithMonster(Rectangle r, AbigailGame.CowboyMonster subject)
		{
			foreach (AbigailGame.CowboyMonster current in AbigailGame.monsters)
			{
				if ((subject == null || !subject.Equals(current)) && Math.Abs(current.position.X - r.X) < 48 && Math.Abs(current.position.Y - r.Y) < 48 && r.Intersects(new Rectangle(current.position.X, current.position.Y, 48, 48)))
				{
					return true;
				}
			}
			return false;
		}

		public static bool isCollidingWithMapForMonsters(Rectangle positionToCheck)
		{
			for (int i = 0; i < 4; i++)
			{
				Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref positionToCheck, i);
				if (cornersOfThisRectangle.X < 0f || cornersOfThisRectangle.Y < 0f || cornersOfThisRectangle.X >= 768f || cornersOfThisRectangle.Y >= 768f || !AbigailGame.isMapTilePassableForMonsters(AbigailGame.map[(int)cornersOfThisRectangle.X / 16 / 3, (int)cornersOfThisRectangle.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool isCollidingWithMap(Rectangle positionToCheck)
		{
			for (int i = 0; i < 4; i++)
			{
				Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref positionToCheck, i);
				if (cornersOfThisRectangle.X < 0f || cornersOfThisRectangle.Y < 0f || cornersOfThisRectangle.X >= 768f || cornersOfThisRectangle.Y >= 768f || !AbigailGame.isMapTilePassable(AbigailGame.map[(int)cornersOfThisRectangle.X / 16 / 3, (int)cornersOfThisRectangle.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool isCollidingWithMap(Point position)
		{
			Rectangle rectangle = new Rectangle(position.X, position.Y, 48, 48);
			for (int i = 0; i < 4; i++)
			{
				Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref rectangle, i);
				if (cornersOfThisRectangle.X < 0f || cornersOfThisRectangle.Y < 0f || cornersOfThisRectangle.X >= 768f || cornersOfThisRectangle.Y >= 768f || !AbigailGame.isMapTilePassable(AbigailGame.map[(int)cornersOfThisRectangle.X / 16 / 3, (int)cornersOfThisRectangle.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool isCollidingWithMap(Vector2 position)
		{
			Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, 48, 48);
			for (int i = 0; i < 4; i++)
			{
				Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref rectangle, i);
				if (cornersOfThisRectangle.X < 0f || cornersOfThisRectangle.Y < 0f || cornersOfThisRectangle.X >= 768f || cornersOfThisRectangle.Y >= 768f || !AbigailGame.isMapTilePassable(AbigailGame.map[(int)cornersOfThisRectangle.X / 16 / 3, (int)cornersOfThisRectangle.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		private void addPlayer2MovementDirection(int direction)
		{
			if (!this.player2MovementDirections.Contains(direction))
			{
				if (this.player2MovementDirections.Count == 1 && direction == (this.player2MovementDirections[0] + 2) % 4)
				{
					this.player2MovementDirections.Clear();
				}
				this.player2MovementDirections.Add(direction);
				if (this.player2MovementDirections.Count > 2)
				{
					this.player2MovementDirections.RemoveAt(0);
				}
			}
		}

		private void addPlayerMovementDirection(int direction)
		{
			if (!AbigailGame.playerMovementDirections.Contains(direction))
			{
				if (AbigailGame.playerMovementDirections.Count == 1)
				{
					int arg_2A_0 = (AbigailGame.playerMovementDirections.ElementAt(0) + 2) % 4;
				}
				AbigailGame.playerMovementDirections.Add(direction);
			}
		}

		private void addPlayer2ShootingDirection(int direction)
		{
			if (!this.player2ShootingDirections.Contains(direction))
			{
				if (this.player2ShootingDirections.Count == 1 && direction == (this.player2ShootingDirections[0] + 2) % 4)
				{
					this.player2ShootingDirections.Clear();
				}
				this.player2ShootingDirections.Add(direction);
				if (this.player2ShootingDirections.Count > 2)
				{
					this.player2ShootingDirections.RemoveAt(0);
				}
			}
		}

		private void addPlayerShootingDirection(int direction)
		{
			if (!AbigailGame.playerShootingDirections.Contains(direction))
			{
				AbigailGame.playerShootingDirections.Add(direction);
			}
		}

		public void startShoppingLevel()
		{
			this.merchantBox.Y = -AbigailGame.TileSize;
			AbigailGame.shopping = true;
			AbigailGame.merchantArriving = true;
			AbigailGame.merchantLeaving = false;
			AbigailGame.merchantShopOpen = false;
			if (AbigailGame.overworldSong != null)
			{
				AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
			}
			AbigailGame.monsters.Clear();
			AbigailGame.waitingForPlayerToMoveDownAMap = true;
			this.storeItems.Clear();
			int num = AbigailGame.whichWave;
			if (num == 2)
			{
				this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 3);
				this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 0);
				this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 6);
			}
			else
			{
				this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.runSpeedLevel >= 2) ? 5 : (3 + this.runSpeedLevel));
				this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.fireSpeedLevel >= 3) ? ((this.ammoLevel >= 3 && !this.spreadPistol) ? 9 : 10) : this.fireSpeedLevel);
				this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.ammoLevel < 3) ? (6 + this.ammoLevel) : 10);
			}
			if (this.whichRound > 0)
			{
				this.storeItems.Clear();
				this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.runSpeedLevel >= 2) ? 5 : (3 + this.runSpeedLevel));
				this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.fireSpeedLevel >= 3) ? ((this.ammoLevel >= 3 && !this.spreadPistol) ? 9 : 10) : this.fireSpeedLevel);
				this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.ammoLevel < 3) ? (6 + this.ammoLevel) : 10);
			}
		}

		public void receiveKeyPress(Keys k)
		{
			Vector2 arg_06_0 = this.playerPosition;
			if (Game1.options.gamepadControls && (Game1.isAnyGamePadButtonBeingPressed() || Game1.isGamePadThumbstickInMotion(0.2)))
			{
				Game1.thumbstickMotionMargin = 0;
				if (Game1.isGamePadThumbstickInMotion(0.2) && (Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up) || Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right) || Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down) || Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left)))
				{
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					k = Keys.W;
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
				{
					k = Keys.D;
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					k = Keys.S;
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
				{
					k = Keys.A;
				}
				else if (k != Keys.None && Game1.options.doesInputListContain(Game1.options.actionButton, k))
				{
					k = (AbigailGame.gameOver ? Keys.X : Keys.Down);
				}
				else if (k != Keys.None && Game1.options.doesInputListContain(Game1.options.useToolButton, k))
				{
					k = Keys.Left;
				}
				else if (Game1.options.doesInputListContain(Game1.options.toolSwapButton, k) || (GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.Start) && !Game1.oldPadState.IsButtonDown(Buttons.Start)) || (GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.RightTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.RightTrigger)))
				{
					k = Keys.Space;
				}
				else if (Game1.options.doesInputListContain(Game1.options.journalButton, k))
				{
					k = Keys.Escape;
				}
				else if (Game1.options.doesInputListContain(Game1.options.menuButton, k))
				{
					k = ((GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.Y) && !Game1.oldPadState.IsButtonDown(Buttons.Y)) ? Keys.Up : ((GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.B) && !Game1.oldPadState.IsButtonDown(Buttons.B)) ? Keys.Right : k));
				}
			}
			else
			{
				bool flag = false;
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k) && !Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up))
				{
					this.addPlayerMovementDirection(0);
					if (AbigailGame.gameOver)
					{
						this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
						Game1.playSound("Cowboy_gunshot");
					}
					flag = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k) && !Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))
				{
					this.addPlayerMovementDirection(3);
					flag = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k) && !Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down))
				{
					this.addPlayerMovementDirection(2);
					if (AbigailGame.gameOver)
					{
						this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
						Game1.playSound("Cowboy_gunshot");
					}
					flag = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k) && !Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right))
				{
					this.addPlayerMovementDirection(1);
					flag = true;
				}
				if (flag)
				{
					return;
				}
			}
			if (AbigailGame.onStartMenu)
			{
				AbigailGame.startTimer = 1;
				Game1.playSound("Pickup_Coin15");
				return;
			}
			if (k <= Keys.A)
			{
				if (k <= Keys.Escape)
				{
					if (k != Keys.Enter)
					{
						if (k != Keys.Escape)
						{
							return;
						}
						if (AbigailGame.playingWithAbigail)
						{
							return;
						}
						Game1.currentMinigame = null;
						if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
						{
							AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
						}
						if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
						{
							AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
						}
						if (Game1.currentLocation != null && Game1.currentLocation.name.Equals("Saloon") && Game1.timeOfDay >= 1700)
						{
							Game1.changeMusicTrack("Saloon1");
							return;
						}
						return;
					}
				}
				else
				{
					switch (k)
					{
					case Keys.Space:
						break;
					case Keys.PageUp:
					case Keys.PageDown:
					case Keys.End:
					case Keys.Home:
						return;
					case Keys.Left:
						this.addPlayerShootingDirection(3);
						return;
					case Keys.Up:
						this.addPlayerShootingDirection(0);
						if (AbigailGame.gameOver)
						{
							this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
							Game1.playSound("Cowboy_gunshot");
							return;
						}
						return;
					case Keys.Right:
						this.addPlayerShootingDirection(1);
						return;
					case Keys.Down:
						this.addPlayerShootingDirection(2);
						if (AbigailGame.gameOver)
						{
							this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
							Game1.playSound("Cowboy_gunshot");
							return;
						}
						return;
					default:
						if (k != Keys.A)
						{
							return;
						}
						this.addPlayerMovementDirection(3);
						return;
					}
				}
			}
			else if (k <= Keys.S)
			{
				if (k == Keys.D)
				{
					this.addPlayerMovementDirection(1);
					return;
				}
				if (k != Keys.S)
				{
					return;
				}
				this.addPlayerMovementDirection(2);
				if (AbigailGame.gameOver)
				{
					this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
					Game1.playSound("Cowboy_gunshot");
					return;
				}
				return;
			}
			else if (k != Keys.W)
			{
				if (k != Keys.X)
				{
					return;
				}
			}
			else
			{
				this.addPlayerMovementDirection(0);
				if (AbigailGame.gameOver)
				{
					this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
					Game1.playSound("Cowboy_gunshot");
					return;
				}
				return;
			}
			if (AbigailGame.gameOver)
			{
				if (this.gameOverOption == 1)
				{
					this.quit = true;
					return;
				}
				this.gamerestartTimer = 1500;
				AbigailGame.gameOver = false;
				this.gameOverOption = 0;
				Game1.playSound("Pickup_Coin15");
				return;
			}
			else if (this.heldItem != null && AbigailGame.deathTimer <= 0f && AbigailGame.zombieModeTimer <= 0)
			{
				this.usePowerup(this.heldItem.which);
				this.heldItem = null;
				return;
			}
		}

		public void receiveKeyRelease(Keys k)
		{
			if (Game1.options.gamepadControls && Utility.getPressedButtons(Game1.oldPadState, GamePad.GetState(Game1.playerOneIndex)).Count > 0)
			{
				if (k != Keys.None)
				{
					if (Game1.isGamePadThumbstickInMotion(0.2) && (Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up) || Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right) || Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down) || Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left)))
					{
						return;
					}
					if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
					{
						k = Keys.W;
					}
					else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
					{
						k = Keys.D;
					}
					else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
					{
						k = Keys.S;
					}
					else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
					{
						k = Keys.A;
					}
					else if (Game1.options.doesInputListContain(Game1.options.actionButton, k))
					{
						k = Keys.Down;
					}
					else if (Game1.options.doesInputListContain(Game1.options.useToolButton, k))
					{
						k = Keys.Left;
					}
					else if (Game1.options.doesInputListContain(Game1.options.toolSwapButton, k))
					{
						k = Keys.Space;
					}
					else if (Game1.options.doesInputListContain(Game1.options.menuButton, k))
					{
						k = ((Game1.oldPadState.IsButtonDown(Buttons.Y) && !GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.Y)) ? Keys.Up : ((Game1.oldPadState.IsButtonDown(Buttons.B) && !GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.B)) ? Keys.Right : k));
					}
				}
			}
			else
			{
				bool flag = false;
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k) && !Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up))
				{
					if (AbigailGame.playerMovementDirections.Contains(0))
					{
						AbigailGame.playerMovementDirections.Remove(0);
					}
					flag = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k) && !Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))
				{
					if (AbigailGame.playerMovementDirections.Contains(3))
					{
						AbigailGame.playerMovementDirections.Remove(3);
					}
					flag = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k) && !Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down))
				{
					if (AbigailGame.playerMovementDirections.Contains(2))
					{
						AbigailGame.playerMovementDirections.Remove(2);
					}
					flag = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k) && !Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right))
				{
					if (AbigailGame.playerMovementDirections.Contains(1))
					{
						AbigailGame.playerMovementDirections.Remove(1);
					}
					flag = true;
				}
				if (flag)
				{
					return;
				}
			}
			if (k <= Keys.A)
			{
				switch (k)
				{
				case Keys.Left:
					if (AbigailGame.playerShootingDirections.Contains(3))
					{
						AbigailGame.playerShootingDirections.Remove(3);
					}
					break;
				case Keys.Up:
					if (AbigailGame.playerShootingDirections.Contains(0))
					{
						AbigailGame.playerShootingDirections.Remove(0);
						return;
					}
					break;
				case Keys.Right:
					if (AbigailGame.playerShootingDirections.Contains(1))
					{
						AbigailGame.playerShootingDirections.Remove(1);
						return;
					}
					break;
				case Keys.Down:
					if (AbigailGame.playerShootingDirections.Contains(2))
					{
						AbigailGame.playerShootingDirections.Remove(2);
						return;
					}
					break;
				default:
					if (k != Keys.A)
					{
						return;
					}
					if (AbigailGame.playerMovementDirections.Contains(3))
					{
						AbigailGame.playerMovementDirections.Remove(3);
						return;
					}
					break;
				}
			}
			else if (k != Keys.D)
			{
				if (k != Keys.S)
				{
					if (k == Keys.W && AbigailGame.playerMovementDirections.Contains(0))
					{
						AbigailGame.playerMovementDirections.Remove(0);
						return;
					}
				}
				else if (AbigailGame.playerMovementDirections.Contains(2))
				{
					AbigailGame.playerMovementDirections.Remove(2);
					return;
				}
			}
			else if (AbigailGame.playerMovementDirections.Contains(1))
			{
				AbigailGame.playerMovementDirections.Remove(1);
				return;
			}
		}

		public int getPriceForItem(int whichItem)
		{
			switch (whichItem)
			{
			case 0:
				return 10;
			case 1:
				return 20;
			case 2:
				return 30;
			case 3:
				return 8;
			case 4:
				return 20;
			case 5:
				return 10;
			case 6:
				return 15;
			case 7:
				return 30;
			case 8:
				return 45;
			case 9:
				return 99;
			case 10:
				return 10;
			default:
				return 5;
			}
		}

		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (AbigailGame.onStartMenu)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.97f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - 3 * AbigailGame.TileSize), AbigailGame.topLeftScreenCoordinate.Y + (float)(5 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(128, 1744, 96, 72 - ((AbigailGame.startTimer > 0) ? 16 : 0))), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
			}
			else if ((AbigailGame.gameOver || this.gamerestartTimer > 0) && !AbigailGame.endCutscene)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11914", new object[0]), AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float)AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11914", new object[0]), AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float)AbigailGame.TileSize + new Vector2(-1f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11914", new object[0]), AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float)AbigailGame.TileSize + new Vector2(1f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				string text = Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11917", new object[0]);
				if (this.gameOverOption == 0)
				{
					text = "> " + text;
				}
				string text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11919", new object[0]);
				if (this.gameOverOption == 1)
				{
					text2 = "> " + text2;
				}
				if (this.gamerestartTimer <= 0 || this.gamerestartTimer / 500 % 2 == 0)
				{
					b.DrawString(Game1.smallFont, text, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * (float)AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				b.DrawString(Game1.smallFont, text2, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * (float)AbigailGame.TileSize + new Vector2(0f, (float)(AbigailGame.TileSize * 2 / 3)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
			}
			else if (AbigailGame.endCutscene)
			{
				switch (AbigailGame.endCutscenePhase)
				{
				case 0:
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White * ((AbigailGame.endCutsceneTimer < 2000) ? (1f * ((float)AbigailGame.endCutsceneTimer / 2000f)) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3)) + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White * ((AbigailGame.endCutsceneTimer < 2000) ? (1f * ((float)AbigailGame.endCutsceneTimer / 2000f)) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f);
					break;
				case 1:
				case 2:
				case 3:
					for (int i = 0; i < 16; i++)
					{
						for (int j = 0; j < 16; j++)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)i, (float)j) * 16f * 3f + new Vector2(0f, (float)(AbigailGame.newMapPosition - 16 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.map[i, j] + ((AbigailGame.map[i, j] == 5 && this.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
						}
					}
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(6 * AbigailGame.TileSize), (float)(3 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(288, 1697, 64, 80)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
					if (AbigailGame.endCutscenePhase == 3)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(9 * AbigailGame.TileSize), (float)(7 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(544, 1792, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.05f);
						if (AbigailGame.endCutsceneTimer < 3000)
						{
							b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * (1f - (float)AbigailGame.endCutsceneTimer / 3000f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
						}
					}
					else
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(10 * AbigailGame.TileSize), (float)(8 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(272 - AbigailGame.endCutsceneTimer / 300 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.02f);
						if (AbigailGame.endCutscenePhase == 2)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(484, 1760 + (int)(this.playerMotionAnimationTimer / 100f) * 3, 8, 3)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f + 0.001f);
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition, new Rectangle?(new Rectangle(384, 1760, 16, 13)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f + 0.001f);
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3 - AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.005f);
						}
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * ((AbigailGame.endCutscenePhase == 1 && AbigailGame.endCutsceneTimer > 12500) ? ((float)((AbigailGame.endCutsceneTimer - 12500) / 3000)) : 0f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
					}
					break;
				case 4:
				case 5:
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.97f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(6 * AbigailGame.TileSize), (float)(3 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(224, 1744, 64, 48)), Color.White * ((AbigailGame.endCutsceneTimer > 0) ? (1f - ((float)AbigailGame.endCutsceneTimer - 2000f) / 2000f) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					if (AbigailGame.endCutscenePhase == 5 && this.gamerestartTimer <= 0)
					{
						b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_PK_NewGame+", new object[0]), AbigailGame.topLeftScreenCoordinate + new Vector2(3f, 10f) * (float)AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
					}
					break;
				}
			}
			else
			{
				if (AbigailGame.zombieModeTimer > 8200)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition, new Rectangle?(new Rectangle(384 + ((AbigailGame.zombieModeTimer / 200 % 2 == 0) ? 16 : 0), 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					for (int k = (int)(this.playerPosition.Y - (float)AbigailGame.TileSize); k > -AbigailGame.TileSize; k -= AbigailGame.TileSize)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X, (float)k), new Rectangle?(new Rectangle(368 + ((k / AbigailGame.TileSize % 3 == 0) ? 16 : 0), 1744, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					}
					b.End();
					return;
				}
				for (int l = 0; l < 16; l++)
				{
					for (int m = 0; m < 16; m++)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)l, (float)m) * 16f * 3f + new Vector2(0f, (float)(AbigailGame.newMapPosition - 16 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.map[l, m] + ((AbigailGame.map[l, m] == 5 && this.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
					}
				}
				if (AbigailGame.scrollingMap)
				{
					for (int n = 0; n < 16; n++)
					{
						for (int num = 0; num < 16; num++)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)n, (float)num) * 16f * 3f + new Vector2(0f, (float)AbigailGame.newMapPosition), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.nextMap[n, num] + ((AbigailGame.nextMap[n, num] == 5 && this.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
						}
					}
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, -1, 16 * AbigailGame.TileSize, (int)AbigailGame.topLeftScreenCoordinate.Y), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize, (int)AbigailGame.topLeftScreenCoordinate.Y + 2), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);
				}
				if (AbigailGame.deathTimer <= 0f && (AbigailGame.playerInvincibleTimer <= 0 || AbigailGame.playerInvincibleTimer / 100 % 2 == 0))
				{
					if (AbigailGame.holdItemTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3)) + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f);
					}
					else if (AbigailGame.zombieModeTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(352 + ((AbigailGame.zombieModeTimer / 50 % 2 == 0) ? 16 : 0), 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					}
					else if (AbigailGame.playerMovementDirections.Count == 0 && AbigailGame.playerShootingDirections.Count == 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(496, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					}
					else
					{
						int num2 = (AbigailGame.playerShootingDirections.Count == 0) ? AbigailGame.playerMovementDirections.ElementAt(0) : AbigailGame.playerShootingDirections.Last<int>();
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)) + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(483, 1760 + (int)(this.playerMotionAnimationTimer / 100f) * 3, 10, 3)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(3f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(464 + num2 * 16, 1744, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f + 0.001f);
					}
				}
				if (AbigailGame.playingWithAbigail && this.player2deathtimer <= 0 && (this.player2invincibletimer <= 0 || this.player2invincibletimer / 100 % 2 == 0))
				{
					if (this.player2MovementDirections.Count == 0 && this.player2ShootingDirections.Count == 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(256, 1728, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					}
					else
					{
						int num3 = (this.player2ShootingDirections.Count == 0) ? this.player2MovementDirections[0] : this.player2ShootingDirections[0];
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)) + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(243, 1728 + this.player2AnimationTimer / 100 * 3, 10, 3)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, AbigailGame.player2Position.Y / 10000f + 0.001f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(224 + num3 * 16, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, AbigailGame.player2Position.Y / 10000f + 0.002f + 0.001f);
					}
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = AbigailGame.temporarySprites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
				using (List<AbigailGame.CowboyPowerup>.Enumerator enumerator2 = AbigailGame.powerups.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b);
					}
				}
				foreach (AbigailGame.CowboyBullet current in this.bullets)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)current.position.X, (float)current.position.Y), new Rectangle?(new Rectangle(518, 1760 + (this.bulletDamage - 1) * 4, 4, 4)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
				}
				foreach (AbigailGame.CowboyBullet current2 in AbigailGame.enemyBullets)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)current2.position.X, (float)current2.position.Y), new Rectangle?(new Rectangle(523, 1760, 5, 5)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
				}
				if (AbigailGame.shopping)
				{
					if ((AbigailGame.merchantArriving || AbigailGame.merchantLeaving) && !AbigailGame.merchantShopOpen)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.merchantBox.Location.X, (float)this.merchantBox.Location.Y), new Rectangle?(new Rectangle(464 + ((AbigailGame.shoppingTimer / 100 % 2 == 0) ? 16 : 0), 1728, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.merchantBox.Y / 10000f + 0.001f);
					}
					else
					{
						int num4 = (this.playerBoundingBox.X - this.merchantBox.X > AbigailGame.TileSize) ? 2 : ((this.merchantBox.X - this.playerBoundingBox.X > AbigailGame.TileSize) ? 1 : 0);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.merchantBox.Location.X, (float)this.merchantBox.Location.Y), new Rectangle?(new Rectangle(496 + num4 * 16, 1728, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.merchantBox.Y / 10000f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.merchantBox.Location.X - AbigailGame.TileSize), (float)(this.merchantBox.Location.Y + AbigailGame.TileSize)), new Rectangle?(new Rectangle(529, 1744, 63, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.merchantBox.Y / 10000f + 0.001f);
						foreach (KeyValuePair<Rectangle, int> current3 in this.storeItems)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)current3.Key.Location.X, (float)current3.Key.Location.Y), new Rectangle?(new Rectangle(320 + current3.Value * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)current3.Key.Location.Y / 10000f);
							b.DrawString(Game1.smallFont, string.Concat(this.getPriceForItem(current3.Value)), AbigailGame.topLeftScreenCoordinate + new Vector2((float)(current3.Key.Location.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString(string.Concat(this.getPriceForItem(current3.Value))).X / 2f, (float)(current3.Key.Location.Y + AbigailGame.TileSize + 3)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)current3.Key.Location.Y / 10000f + 0.002f);
							b.DrawString(Game1.smallFont, string.Concat(this.getPriceForItem(current3.Value)), AbigailGame.topLeftScreenCoordinate + new Vector2((float)(current3.Key.Location.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString(string.Concat(this.getPriceForItem(current3.Value))).X / 2f - 1f, (float)(current3.Key.Location.Y + AbigailGame.TileSize + 3)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)current3.Key.Location.Y / 10000f + 0.002f);
							b.DrawString(Game1.smallFont, string.Concat(this.getPriceForItem(current3.Value)), AbigailGame.topLeftScreenCoordinate + new Vector2((float)(current3.Key.Location.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString(string.Concat(this.getPriceForItem(current3.Value))).X / 2f + 1f, (float)(current3.Key.Location.Y + AbigailGame.TileSize + 3)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)current3.Key.Location.Y / 10000f + 0.002f);
						}
					}
				}
				if (AbigailGame.waitingForPlayerToMoveDownAMap && (AbigailGame.merchantShopOpen || AbigailGame.merchantLeaving || !AbigailGame.shopping) && AbigailGame.shoppingTimer < 250)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(7f, 15f) * (float)AbigailGame.TileSize, new Rectangle?(new Rectangle(355, 1750, 8, 8)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.001f);
				}
				using (List<AbigailGame.CowboyMonster>.Enumerator enumerator5 = AbigailGame.monsters.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						enumerator5.Current.draw(b);
					}
				}
				if (AbigailGame.gopherRunning)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)AbigailGame.gopherBox.X, (float)AbigailGame.gopherBox.Y), new Rectangle?(new Rectangle(320 + AbigailGame.waveTimer / 100 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)AbigailGame.gopherBox.Y / 10000f + 0.001f);
				}
				if (AbigailGame.gopherTrain && AbigailGame.gopherTrainPosition > -AbigailGame.TileSize)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.95f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X - (float)(AbigailGame.TileSize / 2), (float)AbigailGame.gopherTrainPosition), new Rectangle?(new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X + (float)(AbigailGame.TileSize / 2), (float)AbigailGame.gopherTrainPosition), new Rectangle?(new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X, (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize * 3)), new Rectangle?(new Rectangle(320 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X - (float)(AbigailGame.TileSize / 2), (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize)), new Rectangle?(new Rectangle(400, 1728, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.97f);
					if (AbigailGame.holdItemTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3)) + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.99f);
					}
					else
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(464, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
					}
				}
				else
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize + 27), 0f), new Rectangle?(new Rectangle(294, 1782, 22, 22)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.25f);
					if (this.heldItem != null)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize + 18), -9f), new Rectangle?(new Rectangle(272 + this.heldItem.which * 16, 1808, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize * 2), (float)(-(float)AbigailGame.TileSize - 18)), new Rectangle?(new Rectangle(400, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					b.DrawString(Game1.smallFont, "x" + this.lives, AbigailGame.topLeftScreenCoordinate - new Vector2((float)AbigailGame.TileSize, (float)(-(float)AbigailGame.TileSize - AbigailGame.TileSize / 4 - 18)), Color.White);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize * 2), (float)(-(float)AbigailGame.TileSize * 2 - 18)), new Rectangle?(new Rectangle(272, 1808, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					b.DrawString(Game1.smallFont, "x" + this.coins, AbigailGame.topLeftScreenCoordinate - new Vector2((float)AbigailGame.TileSize, (float)(-(float)AbigailGame.TileSize * 2 - AbigailGame.TileSize / 4 - 18)), Color.White);
					for (int num5 = 0; num5 < AbigailGame.whichWave + this.whichRound * 12; num5++)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize * 16 + 3), (float)(num5 * 3 * 6)), new Rectangle?(new Rectangle(512, 1760, 5, 5)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					b.Draw(Game1.mouseCursors, new Vector2((float)((int)AbigailGame.topLeftScreenCoordinate.X), (float)((int)AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 - 12)), new Rectangle?(new Rectangle(595, 1748, 9, 11)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					if (!AbigailGame.shootoutLevel)
					{
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X + 30, (int)AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 + 3, (int)((float)(16 * AbigailGame.TileSize - 30) * ((float)AbigailGame.waveTimer / 80000f)), AbigailGame.TileSize / 4), (AbigailGame.waveTimer < 8000) ? new Color(188, 51, 74) : new Color(147, 177, 38));
					}
					if (AbigailGame.betweenWaveTimer > 0 && AbigailGame.whichWave == 0 && !AbigailGame.scrollingMap)
					{
						Vector2 position = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 120), (float)(Game1.graphics.GraphicsDevice.Viewport.Height - 144 - 3));
						if (!Game1.options.gamepadControls)
						{
							b.Draw(Game1.mouseCursors, position, new Rectangle?(new Rectangle(352, 1648, 80, 48)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.99f);
						}
						else
						{
							b.Draw(Game1.controllerMaps, position, new Rectangle?(Utility.controllerMapSourceRect(new Rectangle(681, 157, 160, 96))), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.99f);
						}
					}
					if (this.bulletDamage > 1)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize)), new Rectangle?(new Rectangle(416 + (this.ammoLevel - 1) * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (this.fireSpeedLevel > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(320 + (this.fireSpeedLevel - 1) * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (this.runSpeedLevel > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize * 3)), new Rectangle?(new Rectangle(368 + (this.runSpeedLevel - 1) * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (this.spreadPistol)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize * 4)), new Rectangle?(new Rectangle(464, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
				}
				if (AbigailGame.screenFlash > 0)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), new Color(255, 214, 168), 0f, Vector2.Zero, SpriteEffects.None, 1f);
				}
			}
			if (this.fadethenQuitTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * (1f - (float)this.fadethenQuitTimer / 2000f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
			}
			if (this.abigailPortraitTimer > 0)
			{
				b.Draw(Game1.getCharacterFromName("Abigail", false).Portrait, new Vector2(AbigailGame.topLeftScreenCoordinate.X + (float)(16 * AbigailGame.TileSize), (float)this.abigailPortraitYposition), new Rectangle?(new Rectangle(64 * (this.abigailPortraitExpression % 2), 64 * (this.abigailPortraitExpression / 2), 64, 64)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				if (this.abigailPortraitTimer < 5500 && this.abigailPortraitTimer > 500)
				{
					int widthOfString = SpriteText.getWidthOfString("0" + this.AbigailDialogue + "0");
					int x = (int)(AbigailGame.topLeftScreenCoordinate.X + (float)(16 * AbigailGame.TileSize) + (float)(64 * Game1.pixelZoom / 2) - (float)(widthOfString / 2));
					if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru)
					{
						x = (int)(AbigailGame.topLeftScreenCoordinate.X + (float)(16 * AbigailGame.TileSize)) + widthOfString / 4;
					}
					else
					{
						x = (int)(AbigailGame.topLeftScreenCoordinate.X + (float)(16 * AbigailGame.TileSize));
					}
					SpriteText.drawString(b, this.AbigailDialogue, x, (int)((double)this.abigailPortraitYposition - (double)Game1.tileSize * 1.25), 999999, widthOfString, 999999, 1f, 0.88f, false, -1, "", 3);
				}
			}
			b.End();
		}

		public void changeScreenSize()
		{
			AbigailGame.topLeftScreenCoordinate = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 384), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 384));
		}

		public void unload()
		{
			if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
			{
				AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
			}
			if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
			{
				AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
			}
			this.lives = 3;
		}

		public void receiveEventPoke(int data)
		{
		}

		public string minigameId()
		{
			return "PrairieKing";
		}
	}
}
