using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;

namespace StardewValley.Characters
{
	public class Child : NPC
	{
		public const int newborn = 0;

		public const int baby = 1;

		public const int crawler = 2;

		public const int toddler = 3;

		public new int age;

		public int daysOld;

		public long idOfParent;

		public bool darkSkinned;

		private int previousState;

		public Child()
		{
		}

		public Child(string name, bool isMale, bool isDarkSkinned, Farmer parent)
		{
			this.age = 2;
			this.gender = (isMale ? 0 : 1);
			this.darkSkinned = isDarkSkinned;
			this.reloadSprite();
			this.name = name;
			base.displayName = name;
			base.DefaultMap = "FarmHouse";
			this.hideShadow = true;
			this.speed = 1;
			this.idOfParent = parent.uniqueMultiplayerID;
			this.breather = false;
		}

		public override void reloadSprite()
		{
			if (this.age >= 3)
			{
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Toddler" + ((this.gender == 0) ? "" : "_girl") + (this.darkSkinned ? "_dark" : "")), 0, 16, 32);
				this.hideShadow = false;
			}
			else
			{
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Baby" + (this.darkSkinned ? "_dark" : "")), 0, 22, (this.age == 1) ? 32 : 16);
				if (this.age == 1)
				{
					this.sprite.CurrentFrame = 4;
				}
				else if (this.age == 2)
				{
					this.sprite.CurrentFrame = 44;
				}
				this.hideShadow = true;
			}
			this.breather = false;
		}

		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (this.moveUp)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateUp(time, 0, "");
						this.facingDirection = 0;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveUp = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			else if (this.moveRight)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateRight(time, 0, "");
						this.facingDirection = 1;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveRight = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			else if (this.moveDown)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateDown(time, 0, "");
						this.facingDirection = 2;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveDown = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			else if (this.moveLeft)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateLeft(time, 0, "");
						this.facingDirection = 3;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveLeft = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			if (this.blockedInterval >= 3000 && (float)this.blockedInterval <= 3750f && !Game1.eventUp)
			{
				base.doEmote((Game1.random.NextDouble() < 0.5) ? 8 : 40, true);
				this.blockedInterval = 3750;
				return;
			}
			if (this.blockedInterval >= 5000)
			{
				this.speed = 1;
				this.isCharging = true;
				this.blockedInterval = 0;
			}
		}

		public override bool canPassThroughActionTiles()
		{
			return false;
		}

		public override void dayUpdate(int dayOfMonth)
		{
			this.daysOld++;
			this.breather = false;
			if (this.daysOld == 13)
			{
				this.age = 1;
			}
			else if (this.daysOld == 27)
			{
				this.age = 2;
			}
			else if (this.daysOld == 55)
			{
				this.age = 3;
				this.hideShadow = false;
				this.speed = 4;
				this.reloadSprite();
			}
			if (this.age == 2)
			{
				this.hideShadow = true;
				this.speed = 1;
				Point randomOpenPointInHouse = (base.getHome() as FarmHouse).getRandomOpenPointInHouse(Game1.random, 0, 30);
				if (!randomOpenPointInHouse.Equals(Point.Zero))
				{
					base.setTilePosition(randomOpenPointInHouse);
				}
				else
				{
					this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
				}
				this.sprite.CurrentFrame = 32;
			}
			if (this.age == 3)
			{
				Point randomOpenPointInHouse2 = (base.getHome() as FarmHouse).getRandomOpenPointInHouse(Game1.random, 0, 30);
				if (!randomOpenPointInHouse2.Equals(Point.Zero))
				{
					base.setTilePosition(randomOpenPointInHouse2);
				}
				else
				{
					this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
				}
				this.sprite.spriteWidth = 16;
				this.sprite.spriteHeight = 32;
				this.sprite.CurrentFrame = 0;
				this.hideShadow = false;
			}
		}

		public void toss(Farmer who)
		{
			who.forceTimePass = true;
			who.faceDirection(2);
			who.FarmerSprite.PauseForSingleAnimation = false;
			who.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(57, 1500, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneTossing), true)
			});
			this.position = who.position + new Vector2((float)(-(float)Game1.pixelZoom * 4), (float)(-(float)Game1.tileSize * 3 / 2));
			this.yJumpVelocity = (float)Game1.random.Next(12, 19);
			this.yJumpOffset = -1;
			Game1.playSound("dwop");
			who.CanMove = false;
			who.freezePause = 1500;
			this.drawOnTop = true;
			this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(4, 100),
				new FarmerSprite.AnimationFrame(5, 100),
				new FarmerSprite.AnimationFrame(6, 100),
				new FarmerSprite.AnimationFrame(7, 100)
			});
		}

		public void doneTossing(Farmer who)
		{
			who.forceTimePass = false;
			this.resetForPlayerEntry(who.currentLocation);
			who.CanMove = true;
			who.forceCanMove();
			who.faceDirection(0);
			this.drawOnTop = false;
			base.doEmote(20, true);
			if (!who.friendships.ContainsKey(this.name))
			{
				SerializableDictionary<string, int[]> arg_64_0 = who.friendships;
				string arg_64_1 = this.name;
				int[] expr_5C = new int[6];
				expr_5C[0] = 250;
				arg_64_0.Add(arg_64_1, expr_5C);
			}
			who.talkToFriend(this, 20);
			Game1.playSound("tinyWhip");
		}

		public override Microsoft.Xna.Framework.Rectangle getMugShotSourceRect()
		{
			switch (this.age)
			{
			case 0:
				return new Microsoft.Xna.Framework.Rectangle(0, 0, 22, 16);
			case 1:
				return new Microsoft.Xna.Framework.Rectangle(0, 42, 22, 24);
			case 2:
				return new Microsoft.Xna.Framework.Rectangle(0, 112, 22, 16);
			case 3:
				return new Microsoft.Xna.Framework.Rectangle(0, 4, 16, 24);
			default:
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
		}

		private void setCrawlerInNewDirection()
		{
			this.speed = 1;
			int num = Game1.random.Next(6);
			if (this.previousState >= 4 && Game1.random.NextDouble() < 0.6)
			{
				num = this.previousState;
			}
			if (num < 4)
			{
				while (num == this.previousState)
				{
					num = Game1.random.Next(6);
				}
			}
			else if (this.previousState >= 4)
			{
				num = this.previousState;
			}
			switch (num)
			{
			case 0:
				base.SetMovingOnlyUp();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(32, 160),
					new FarmerSprite.AnimationFrame(33, 160),
					new FarmerSprite.AnimationFrame(34, 160),
					new FarmerSprite.AnimationFrame(35, 160)
				});
				break;
			case 1:
				base.SetMovingOnlyRight();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(28, 160),
					new FarmerSprite.AnimationFrame(29, 160),
					new FarmerSprite.AnimationFrame(30, 160),
					new FarmerSprite.AnimationFrame(31, 160)
				});
				break;
			case 2:
				base.SetMovingOnlyDown();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(24, 160),
					new FarmerSprite.AnimationFrame(25, 160),
					new FarmerSprite.AnimationFrame(26, 160),
					new FarmerSprite.AnimationFrame(27, 160)
				});
				break;
			case 3:
				base.SetMovingOnlyLeft();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(36, 160),
					new FarmerSprite.AnimationFrame(37, 160),
					new FarmerSprite.AnimationFrame(38, 160),
					new FarmerSprite.AnimationFrame(39, 160)
				});
				break;
			case 4:
				this.Halt();
				this.sprite.spriteHeight = 16;
				this.sprite.setCurrentAnimation(this.getRandomCrawlerAnimation(0));
				break;
			case 5:
				this.Halt();
				this.sprite.spriteHeight = 16;
				this.sprite.setCurrentAnimation(this.getRandomCrawlerAnimation(1));
				break;
			}
			this.previousState = num;
		}

		public override bool hasSpecialCollisionRules()
		{
			return true;
		}

		public override bool isColliding(GameLocation l, Vector2 tile)
		{
			return !l.isTilePlaceable(tile, null);
		}

		public void tenMinuteUpdate()
		{
			if (!Game1.IsMasterGame || this.age != 2)
			{
				if (Game1.IsMasterGame && Game1.timeOfDay % 100 == 0 && this.age == 3 && Game1.timeOfDay < 1900)
				{
					base.IsWalkingInSquare = false;
					this.Halt();
					FarmHouse farmHouse = base.getHome() as FarmHouse;
					if (farmHouse.characters.Contains(this))
					{
						this.controller = new PathFindController(this, farmHouse, farmHouse.getRandomOpenPointInHouse(Game1.random, 0, 30), -1, new PathFindController.endBehavior(this.toddlerReachedDestination));
						if (this.controller.pathToEndPoint == null || !farmHouse.isTileOnMap(this.controller.pathToEndPoint.Last<Point>().X, this.controller.pathToEndPoint.Last<Point>().Y))
						{
							this.controller = null;
							return;
						}
					}
				}
				else if (Game1.IsMasterGame && this.age == 3 && Game1.timeOfDay == 1900)
				{
					base.IsWalkingInSquare = false;
					this.Halt();
					FarmHouse farmHouse2 = base.getHome() as FarmHouse;
					if (farmHouse2.characters.Contains(this))
					{
						this.controller = new PathFindController(this, farmHouse2, farmHouse2.getChildBed(this.gender), -1, new PathFindController.endBehavior(this.toddlerReachedDestination));
						if (this.controller.pathToEndPoint == null || !farmHouse2.isTileOnMap(this.controller.pathToEndPoint.Last<Point>().X, this.controller.pathToEndPoint.Last<Point>().Y))
						{
							this.controller = null;
						}
					}
				}
				return;
			}
			if (Game1.timeOfDay < 1800)
			{
				this.setCrawlerInNewDirection();
				return;
			}
			this.Halt();
		}

		public void toddlerReachedDestination(Character c, GameLocation l)
		{
			if (Game1.random.NextDouble() < 0.8 && c.FacingDirection == 2)
			{
				List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
				list.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(17, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(18, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(19, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(18, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(17, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(0, 1000, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(17, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(18, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(18, 300, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(17, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(0, 2000, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(17, 180, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(0, 800, 0, false, false, null, false, 0));
				this.sprite.setCurrentAnimation(list);
				return;
			}
			if (Game1.random.NextDouble() < 0.8 && c.FacingDirection == 1)
			{
				List<FarmerSprite.AnimationFrame> list2 = new List<FarmerSprite.AnimationFrame>();
				list2.Add(new FarmerSprite.AnimationFrame(20, 120, 0, false, false, null, false, 0));
				list2.Add(new FarmerSprite.AnimationFrame(21, 70, 0, false, false, null, false, 0));
				list2.Add(new FarmerSprite.AnimationFrame(22, 70, 0, false, false, null, false, 0));
				list2.Add(new FarmerSprite.AnimationFrame(23, 70, 0, false, false, null, false, 0));
				list2.Add(new FarmerSprite.AnimationFrame(22, 999999, 0, false, false, null, false, 0));
				this.sprite.setCurrentAnimation(list2);
				return;
			}
			if (Game1.random.NextDouble() < 0.8 && c.FacingDirection == 3)
			{
				List<FarmerSprite.AnimationFrame> list3 = new List<FarmerSprite.AnimationFrame>();
				list3.Add(new FarmerSprite.AnimationFrame(20, 120, 0, false, true, null, false, 0));
				list3.Add(new FarmerSprite.AnimationFrame(21, 70, 0, false, true, null, false, 0));
				list3.Add(new FarmerSprite.AnimationFrame(22, 70, 0, false, true, null, false, 0));
				list3.Add(new FarmerSprite.AnimationFrame(23, 70, 0, false, true, null, false, 0));
				list3.Add(new FarmerSprite.AnimationFrame(22, 999999, 0, false, true, null, false, 0));
				this.sprite.setCurrentAnimation(list3);
				return;
			}
			if (c.FacingDirection == 0)
			{
				this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(base.getTileX() * Game1.tileSize, base.getTileY() * Game1.tileSize, Game1.tileSize, Game1.tileSize);
				this.squareMovementFacingPreference = -1;
				base.walkInSquare(4, 4, 2000);
			}
		}

		public bool isChildOf(Farmer who)
		{
			return who.uniqueMultiplayerID == this.idOfParent;
		}

		public override bool checkAction(Farmer who, GameLocation l)
		{
			if (!who.friendships.ContainsKey(this.name))
			{
				SerializableDictionary<string, int[]> arg_2D_0 = who.friendships;
				string arg_2D_1 = this.name;
				int[] expr_25 = new int[6];
				expr_25[0] = 250;
				arg_2D_0.Add(arg_2D_1, expr_25);
			}
			if (this.age >= 2 && !who.hasTalkedToFriendToday(this.name))
			{
				who.talkToFriend(this, 20);
				base.doEmote(20, true);
				if (this.age == 3)
				{
					base.faceTowardFarmerForPeriod(4000, 3, false, who);
				}
				return true;
			}
			return false;
		}

		private List<FarmerSprite.AnimationFrame> getRandomCrawlerAnimation(int which = -1)
		{
			List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
			double num = Game1.random.NextDouble();
			if (which == 0 || num < 0.5)
			{
				list.Add(new FarmerSprite.AnimationFrame(40, 500, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(43, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(40, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(43, 1900, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(40, 500, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(40, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(40, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(40, 1500, 0, false, false, null, false, 0));
			}
			else if (which == 1 || num >= 0.5)
			{
				list.Add(new FarmerSprite.AnimationFrame(44, 1500, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(45, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(46, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(45, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(46, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(45, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(46, 200, 0, false, false, null, false, 0));
			}
			return list;
		}

		private List<FarmerSprite.AnimationFrame> getRandomNewbornAnimation()
		{
			List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
			if (Game1.random.NextDouble() < 0.5)
			{
				list.Add(new FarmerSprite.AnimationFrame(0, 400, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(1, 400, 0, false, false, null, false, 0));
			}
			else
			{
				list.Add(new FarmerSprite.AnimationFrame(1, 3400, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(2, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(3, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(4, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(5, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(6, 4400, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(5, 3400, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(4, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(3, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(2, 100, 0, false, false, null, false, 0));
			}
			return list;
		}

		private List<FarmerSprite.AnimationFrame> getRandomBabyAnimation()
		{
			List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
			if (Game1.random.NextDouble() < 0.5)
			{
				list.Add(new FarmerSprite.AnimationFrame(4, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(5, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(6, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(7, 120, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(4, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(5, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(6, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(7, 100, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(4, 150, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(5, 150, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(6, 150, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(7, 150, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(4, 2000, 0, false, false, null, false, 0));
				if (Game1.random.NextDouble() < 0.5)
				{
					list.Add(new FarmerSprite.AnimationFrame(8, 1950, 0, false, false, null, false, 0));
					list.Add(new FarmerSprite.AnimationFrame(9, 1200, 0, false, false, null, false, 0));
					list.Add(new FarmerSprite.AnimationFrame(10, 180, 0, false, false, null, false, 0));
					list.Add(new FarmerSprite.AnimationFrame(11, 1500, 0, false, false, null, false, 0));
					list.Add(new FarmerSprite.AnimationFrame(8, 1500, 0, false, false, null, false, 0));
				}
			}
			else
			{
				list.Add(new FarmerSprite.AnimationFrame(8, 250, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(9, 250, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(10, 250, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(11, 250, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(8, 1950, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(9, 1200, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(10, 180, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(11, 1500, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(8, 1500, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(9, 150, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(10, 150, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(11, 150, 0, false, false, null, false, 0));
				list.Add(new FarmerSprite.AnimationFrame(8, 1500, 0, false, false, null, false, 0));
			}
			return list;
		}

		public override void update(GameTime time, GameLocation location)
		{
			base.update(time, location);
			if (this.age >= 2)
			{
				this.MovePosition(time, Game1.viewport, location);
			}
		}

		public void resetForPlayerEntry(GameLocation l)
		{
			if (this.age == 0)
			{
				this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
				if (Game1.timeOfDay >= 1800 && this.sprite != null)
				{
					this.sprite.StopAnimation();
					this.sprite.CurrentFrame = Game1.random.Next(7);
				}
				else if (this.sprite != null)
				{
					this.sprite.setCurrentAnimation(this.getRandomNewbornAnimation());
				}
			}
			else if (this.age == 1)
			{
				this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-3 * Game1.pixelZoom));
				if (Game1.timeOfDay >= 1800 && this.sprite != null)
				{
					this.sprite.StopAnimation();
					this.sprite.spriteHeight = 16;
					this.sprite.CurrentFrame = Game1.random.Next(7);
				}
				else if (this.sprite != null)
				{
					this.sprite.spriteHeight = 32;
					this.sprite.setCurrentAnimation(this.getRandomBabyAnimation());
				}
			}
			else if (this.age == 2)
			{
				if (this.sprite != null)
				{
					this.sprite.spriteHeight = 16;
				}
				if (Game1.timeOfDay >= 1800)
				{
					this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
					if (this.sprite != null)
					{
						this.sprite.StopAnimation();
						this.sprite.spriteHeight = 16;
						this.sprite.CurrentFrame = 7;
					}
				}
			}
			if (this.sprite != null)
			{
				this.sprite.loop = true;
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (base.IsEmoting && !Game1.eventUp)
			{
				Vector2 localPosition = base.getLocalPosition(Game1.viewport);
				localPosition.Y -= (float)(Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom - ((this.age == 1 || this.age == 3) ? Game1.tileSize : 0));
				localPosition.X += (float)((this.age == 1) ? (2 * Game1.pixelZoom) : 0);
				b.Draw(Game1.emoteSpriteSheet, localPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(base.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)base.getStandingY() / 10000f);
			}
		}
	}
}
