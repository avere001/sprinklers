using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StardewValley.Characters
{
	public class JunimoHarvester : NPC
	{
		private float alpha = 1f;

		private float alphaChange;

		private Vector2 motion = Vector2.Zero;

		private new Rectangle nextPosition;

		private Color color;

		private JunimoHut home;

		private bool destroy;

		private Item lastItemHarvested;

		private Task backgroundTask;

		public int whichJunimoFromThisHut;

		private int harvestTimer;

		public JunimoHarvester()
		{
		}

		public JunimoHarvester(Vector2 position, JunimoHut myHome, int whichJunimoNumberFromThisHut) : base(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Junimo"), 0, 16, 16), position, 2, "Junimo", null)
		{
			this.home = myHome;
			this.whichJunimoFromThisHut = whichJunimoNumberFromThisHut;
			Random random = new Random(myHome.tileX + myHome.tileY * 777 + whichJunimoNumberFromThisHut);
			this.nextPosition = this.GetBoundingBox();
			this.breather = false;
			this.speed = 3;
			this.forceUpdateTimer = 9999;
			this.collidesWithOtherCharacters = true;
			this.ignoreMovementAnimation = true;
			this.farmerPassesThrough = true;
			this.scale = 0.75f;
			this.alpha = 0f;
			this.hideShadow = true;
			this.alphaChange = 0.05f;
			if (random.NextDouble() < 0.25)
			{
				switch (random.Next(8))
				{
				case 0:
					this.color = Color.Red;
					break;
				case 1:
					this.color = Color.Goldenrod;
					break;
				case 2:
					this.color = Color.Yellow;
					break;
				case 3:
					this.color = Color.Lime;
					break;
				case 4:
					this.color = new Color(0, 255, 180);
					break;
				case 5:
					this.color = new Color(0, 100, 255);
					break;
				case 6:
					this.color = Color.MediumPurple;
					break;
				case 7:
					this.color = Color.Salmon;
					break;
				}
				if (random.NextDouble() < 0.01)
				{
					this.color = Color.White;
				}
			}
			else
			{
				switch (random.Next(8))
				{
				case 0:
					this.color = Color.LimeGreen;
					break;
				case 1:
					this.color = Color.Orange;
					break;
				case 2:
					this.color = Color.LightGreen;
					break;
				case 3:
					this.color = Color.Tan;
					break;
				case 4:
					this.color = Color.GreenYellow;
					break;
				case 5:
					this.color = Color.LawnGreen;
					break;
				case 6:
					this.color = Color.PaleGreen;
					break;
				case 7:
					this.color = Color.Turquoise;
					break;
				}
			}
			this.willDestroyObjectsUnderfoot = false;
			this.currentLocation = Game1.getFarm();
			Vector2 vector = Vector2.Zero;
			switch (whichJunimoNumberFromThisHut)
			{
			case 0:
				vector = Utility.recursiveFindOpenTileForCharacter(this, this.currentLocation, new Vector2((float)(this.home.tileX + 1), (float)(this.home.tileY + this.home.tilesHigh + 1)), 30);
				break;
			case 1:
				vector = Utility.recursiveFindOpenTileForCharacter(this, this.currentLocation, new Vector2((float)(this.home.tileX - 1), (float)this.home.tileY), 30);
				break;
			case 2:
				vector = Utility.recursiveFindOpenTileForCharacter(this, this.currentLocation, new Vector2((float)(this.home.tileX + this.home.tilesWide), (float)this.home.tileY), 30);
				break;
			}
			if (vector != Vector2.Zero)
			{
				this.controller = new PathFindController(this, this.currentLocation, Utility.Vector2ToPoint(vector), -1, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100);
			}
			if (this.controller == null || this.controller.pathToEndPoint == null)
			{
				this.pathfindToRandomSpotAroundHut();
				if (this.controller == null || this.controller.pathToEndPoint == null)
				{
					this.destroy = true;
				}
			}
			this.collidesWithOtherCharacters = false;
		}

		public void reachFirstDestinationFromHut(Character c, GameLocation l)
		{
			this.tryToHarvestHere();
		}

		public void tryToHarvestHere()
		{
			if (this.currentLocation != null)
			{
				if (this.currentLocation.terrainFeatures.ContainsKey(base.getTileLocation()) && this.currentLocation.terrainFeatures[base.getTileLocation()] is HoeDirt && (this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).readyForHarvest())
				{
					this.harvestTimer = 2000;
					return;
				}
				this.pokeToHarvest();
			}
		}

		public void pokeToHarvest()
		{
			if (!this.home.isTilePassable(base.getTileLocation()))
			{
				this.destroy = true;
				return;
			}
			if (this.harvestTimer <= 0 && Game1.random.NextDouble() < 0.7)
			{
				this.pathfindToNewCrop();
			}
		}

		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return true;
		}

		public void fadeAway()
		{
			this.collidesWithOtherCharacters = false;
			this.alphaChange = -0.015f;
		}

		public void setAlpha(float a)
		{
			this.alpha = a;
		}

		public void fadeBack()
		{
			this.alpha = 0f;
			this.alphaChange = 0.02f;
			this.isInvisible = false;
		}

		public void setMoving(int xSpeed, int ySpeed)
		{
			this.motion.X = (float)xSpeed;
			this.motion.Y = (float)ySpeed;
		}

		public void setMoving(Vector2 motion)
		{
			this.motion = motion;
		}

		public override void Halt()
		{
			base.Halt();
			this.motion = Vector2.Zero;
		}

		public void junimoReachedHut(Character c, GameLocation l)
		{
			this.controller = null;
			this.motion.X = 0f;
			this.motion.Y = -1f;
			this.destroy = true;
		}

		public bool foundCropEndFunction(PathNode currentNode, Point endPoint, GameLocation location, Character c)
		{
			return location.isCropAtTile(currentNode.x, currentNode.y) && (location.terrainFeatures[new Vector2((float)currentNode.x, (float)currentNode.y)] as HoeDirt).readyForHarvest();
		}

		public void pathFindToNewCrop_doWork()
		{
			if (Game1.timeOfDay > 1900)
			{
				if (this.controller == null)
				{
					this.returnToJunimoHut(this.currentLocation);
				}
				return;
			}
			if (Game1.random.NextDouble() < 0.035 || this.home.noHarvest)
			{
				this.pathfindToRandomSpotAroundHut();
				return;
			}
			this.controller = new PathFindController(this, this.currentLocation, new PathFindController.isAtEnd(this.foundCropEndFunction), -1, false, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100, Point.Zero);
			if (this.controller.pathToEndPoint != null && Math.Abs(this.controller.pathToEndPoint.Last<Point>().X - (this.home.tileX + 1)) <= 8 && Math.Abs(this.controller.pathToEndPoint.Last<Point>().Y - (this.home.tileY + 1)) <= 8)
			{
				this.sprite.CurrentAnimation = null;
				return;
			}
			if (Game1.random.NextDouble() < 0.5 && !this.home.lastKnownCropLocation.Equals(Point.Zero))
			{
				this.controller = new PathFindController(this, this.currentLocation, this.home.lastKnownCropLocation, -1, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100);
				return;
			}
			if (Game1.random.NextDouble() < 0.25)
			{
				this.returnToJunimoHut(this.currentLocation);
				return;
			}
			this.pathfindToRandomSpotAroundHut();
		}

		public void pathfindToNewCrop()
		{
			if (this.backgroundTask == null || this.backgroundTask.IsCompleted)
			{
				this.backgroundTask = new Task(new Action(this.pathFindToNewCrop_doWork));
				this.backgroundTask.Start();
			}
		}

		public void returnToJunimoHut(GameLocation location)
		{
			if (Utility.isOnScreen(Utility.Vector2ToPoint(this.position / (float)Game1.tileSize), Game1.tileSize, this.currentLocation))
			{
				this.jump();
			}
			this.collidesWithOtherCharacters = false;
			this.controller = new PathFindController(this, location, new Point(this.home.tileX + 1, this.home.tileY + 1), 0, new PathFindController.endBehavior(this.junimoReachedHut));
			if (this.controller.pathToEndPoint == null || this.controller.pathToEndPoint.Count<Point>() == 0 || location.isCollidingPosition(this.nextPosition, Game1.viewport, false, 0, false, this))
			{
				this.destroy = true;
			}
			if (Utility.isOnScreen(Utility.Vector2ToPoint(this.position / (float)Game1.tileSize), Game1.tileSize, this.currentLocation))
			{
				Game1.playSound("junimoMeep1");
			}
		}

		public override void faceDirection(int direction)
		{
		}

		public override void update(GameTime time, GameLocation location)
		{
			if (this.backgroundTask != null && !this.backgroundTask.IsCompleted)
			{
				this.sprite.Animate(time, 8, 4, 100f);
				return;
			}
			base.update(time, location);
			this.forceUpdateTimer = 99999;
			if (this.eventActor)
			{
				return;
			}
			if (this.destroy)
			{
				this.alphaChange = -0.05f;
			}
			this.alpha += this.alphaChange;
			if (this.alpha > 1f)
			{
				this.alpha = 1f;
				this.hideShadow = false;
			}
			else if (this.alpha < 0f)
			{
				this.alpha = 0f;
				this.isInvisible = true;
				this.hideShadow = true;
				if (this.destroy)
				{
					location.characters.Remove(this);
					this.home.myJunimos.Remove(this);
				}
			}
			if (this.harvestTimer > 0)
			{
				int num = this.harvestTimer;
				this.harvestTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.harvestTimer > 1800)
				{
					this.sprite.CurrentFrame = 0;
				}
				else if (this.harvestTimer > 1600)
				{
					this.sprite.CurrentFrame = 1;
				}
				else if (this.harvestTimer > 1000)
				{
					this.sprite.CurrentFrame = 2;
					base.shake(50);
				}
				else if (num >= 1000 && this.harvestTimer < 1000)
				{
					this.sprite.CurrentFrame = 0;
					if (this.currentLocation != null && !this.home.noHarvest && this.currentLocation.terrainFeatures.ContainsKey(base.getTileLocation()) && this.currentLocation.terrainFeatures[base.getTileLocation()] is HoeDirt && (this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).readyForHarvest())
					{
						this.sprite.CurrentFrame = 44;
						this.lastItemHarvested = null;
						if ((this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).crop.harvest(base.getTileX(), base.getTileY(), this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt, this))
						{
							(this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).destroyCrop(base.getTileLocation(), Game1.currentLocation.Equals(this.currentLocation));
						}
						if (this.lastItemHarvested != null && this.currentLocation.Equals(Game1.currentLocation))
						{
							this.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.lastItemHarvested.parentSheetIndex, 16, 16), 1000f, 1, 0, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float)base.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
							{
								motion = new Vector2(0.08f, -0.25f)
							});
							if (this.lastItemHarvested is ColoredObject)
							{
								this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.lastItemHarvested.parentSheetIndex + 1, 16, 16), 1000f, 1, 0, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float)base.getStandingY() / 10000f + 0.015f, 0.02f, (this.lastItemHarvested as ColoredObject).color, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
								{
									motion = new Vector2(0.08f, -0.25f)
								});
							}
						}
					}
				}
				else if (this.harvestTimer <= 0)
				{
					this.pokeToHarvest();
				}
			}
			else if (!this.isInvisible && this.controller == null)
			{
				if (this.addedSpeed > 0 || this.speed > 2 || this.isCharging)
				{
					this.destroy = true;
				}
				this.nextPosition = this.GetBoundingBox();
				this.nextPosition.X = this.nextPosition.X + (int)this.motion.X;
				bool flag = false;
				if (!location.isCollidingPosition(this.nextPosition, Game1.viewport, this))
				{
					this.position.X = this.position.X + (float)((int)this.motion.X);
					flag = true;
				}
				this.nextPosition.X = this.nextPosition.X - (int)this.motion.X;
				this.nextPosition.Y = this.nextPosition.Y + (int)this.motion.Y;
				if (!location.isCollidingPosition(this.nextPosition, Game1.viewport, this))
				{
					this.position.Y = this.position.Y + (float)((int)this.motion.Y);
					flag = true;
				}
				if ((!this.motion.Equals(Vector2.Zero) & flag) && Game1.random.NextDouble() < 0.005)
				{
					location.temporarySprites.Add(new TemporaryAnimatedSprite((Game1.random.NextDouble() < 0.5) ? 10 : 11, this.position, this.color, 8, false, 100f, 0, -1, -1f, -1, 0)
					{
						motion = this.motion / 4f,
						alphaFade = 0.01f,
						layerDepth = 0.8f,
						scale = 0.75f,
						alpha = 0.75f
					});
				}
				if (Game1.random.NextDouble() < 0.002)
				{
					switch (Game1.random.Next(6))
					{
					case 0:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(12, 200),
							new FarmerSprite.AnimationFrame(13, 200),
							new FarmerSprite.AnimationFrame(14, 200),
							new FarmerSprite.AnimationFrame(15, 200)
						});
						break;
					case 1:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(44, 200),
							new FarmerSprite.AnimationFrame(45, 200),
							new FarmerSprite.AnimationFrame(46, 200),
							new FarmerSprite.AnimationFrame(47, 200)
						});
						break;
					case 2:
						this.sprite.CurrentAnimation = null;
						break;
					case 3:
						this.jumpWithoutSound(8f);
						this.yJumpVelocity /= 2f;
						this.sprite.CurrentAnimation = null;
						break;
					case 4:
						if (!this.home.noHarvest)
						{
							this.pathfindToNewCrop();
						}
						break;
					case 5:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(28, 100),
							new FarmerSprite.AnimationFrame(29, 100),
							new FarmerSprite.AnimationFrame(30, 100),
							new FarmerSprite.AnimationFrame(31, 100)
						});
						break;
					}
				}
			}
			if (this.controller != null || !this.motion.Equals(Vector2.Zero))
			{
				this.sprite.CurrentAnimation = null;
				if (this.moveRight || (Math.Abs(this.motion.X) > Math.Abs(this.motion.Y) && this.motion.X > 0f))
				{
					this.flip = false;
					if (this.sprite.Animate(time, 16, 8, 50f))
					{
						this.sprite.CurrentFrame = 16;
						return;
					}
				}
				else
				{
					if (this.moveLeft || (Math.Abs(this.motion.X) > Math.Abs(this.motion.Y) && this.motion.X < 0f))
					{
						if (this.sprite.Animate(time, 16, 8, 50f))
						{
							this.sprite.CurrentFrame = 16;
						}
						this.flip = true;
						return;
					}
					if (this.moveUp || (Math.Abs(this.motion.Y) > Math.Abs(this.motion.X) && this.motion.Y < 0f))
					{
						if (this.sprite.Animate(time, 32, 8, 50f))
						{
							this.sprite.CurrentFrame = 32;
							return;
						}
					}
					else if (this.moveDown)
					{
						this.sprite.Animate(time, 0, 8, 50f);
						return;
					}
				}
			}
			else if (this.sprite.CurrentAnimation == null && this.harvestTimer <= 0)
			{
				this.sprite.Animate(time, 8, 4, 100f);
			}
		}

		public void pathfindToRandomSpotAroundHut()
		{
			Vector2 v = new Vector2((float)(this.home.tileX + 1 + Game1.random.Next(-8, 9)), (float)(this.home.tileY + 1 + Game1.random.Next(-8, 9)));
			this.controller = new PathFindController(this, this.currentLocation, Utility.Vector2ToPoint(v), -1, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100);
		}

		public void tryToAddItemToHut(Item i)
		{
			this.lastItemHarvested = i;
			Item item = this.home.output.addItem(i);
			if (item != null && i is StardewValley.Object)
			{
				for (int j = 0; j < item.Stack; j++)
				{
					Game1.createObjectDebris(i.parentSheetIndex, base.getTileX(), base.getTileY(), -1, (i as StardewValley.Object).quality, 1f, this.currentLocation);
				}
			}
		}

		public override void draw(SpriteBatch b, float alpha = 1f)
		{
			if (!this.isInvisible)
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)this.sprite.spriteHeight * 3f / 4f * (float)Game1.pixelZoom / (float)Math.Pow((double)(this.sprite.spriteHeight / 16), 2.0) + (float)this.yJumpOffset - (float)(Game1.pixelZoom * 2)) + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(base.Sprite.SourceRect), this.color * this.alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.sprite.spriteHeight * Game1.pixelZoom) * 3f / 4f) / (float)Game1.pixelZoom, Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : (((float)(base.getStandingY() + this.whichJunimoFromThisHut) + (float)base.getStandingX() / 10000f) / 10000f)));
				if (!this.swimming && !this.hideShadow)
				{
					b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(Game1.tileSize * 3) / 4f - (float)Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), this.color * this.alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)this.yJumpOffset / 40f) * this.scale, SpriteEffects.None, Math.Max(0f, (float)base.getStandingY() / 10000f) - 1E-06f);
				}
			}
		}
	}
}
