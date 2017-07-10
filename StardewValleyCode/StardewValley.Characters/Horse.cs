using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Characters
{
	public class Horse : NPC
	{
		[XmlIgnore]
		public Farmer rider;

		[XmlIgnore]
		public bool mounting;

		[XmlIgnore]
		public bool dismounting;

		private Vector2 dismountTile;

		private bool squeezingThroughGate;

		public Horse()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\horse"), 0, 32, 32);
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
			this.hideShadow = true;
			this.sprite.textureUsesFlippedRightForLeft = true;
			this.sprite.loop = true;
			this.drawOffset = new Vector2((float)(-(float)Game1.tileSize / 4), 0f);
			this.faceDirection(3);
		}

		public Horse(int xTile, int yTile)
		{
			this.name = "";
			base.displayName = this.name;
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\horse"), 0, 32, 32);
			this.position = new Vector2((float)xTile, (float)yTile) * (float)Game1.tileSize;
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
			this.currentLocation = Game1.currentLocation;
			this.hideShadow = true;
			this.sprite.textureUsesFlippedRightForLeft = true;
			this.sprite.loop = true;
			this.drawOffset = new Vector2((float)(-(float)Game1.tileSize / 4), 0f);
			this.faceDirection(3);
		}

		public override void reloadSprite()
		{
		}

		public override void dayUpdate(int dayOfMonth)
		{
			this.faceDirection(3);
		}

		public override Rectangle GetBoundingBox()
		{
			Rectangle boundingBox = base.GetBoundingBox();
			if (this.squeezingThroughGate && (this.facingDirection == 0 || this.facingDirection == 2))
			{
				boundingBox.Inflate(-Game1.tileSize / 2 - Game1.pixelZoom, 0);
			}
			return boundingBox;
		}

		public override bool canPassThroughActionTiles()
		{
			return false;
		}

		public void squeezeForGate()
		{
			this.squeezingThroughGate = true;
			if (this.rider != null)
			{
				this.rider.temporaryImpassableTile = this.GetBoundingBox();
			}
		}

		public override void update(GameTime time, GameLocation location)
		{
			this.squeezingThroughGate = false;
			this.faceTowardFarmer = false;
			this.faceTowardFarmerTimer = -1;
			base.update(time, location);
			this.flip = (this.facingDirection == 3);
			if (this.mounting)
			{
				if (this.rider.position.X < (float)(this.GetBoundingBox().X + Game1.tileSize / 4 - Game1.pixelZoom))
				{
					Farmer expr_73_cp_0_cp_0 = this.rider;
					expr_73_cp_0_cp_0.position.X = expr_73_cp_0_cp_0.position.X + (float)Game1.pixelZoom;
				}
				else if (this.rider.position.X > (float)(this.GetBoundingBox().X + Game1.tileSize / 4 + Game1.pixelZoom))
				{
					Farmer expr_BB_cp_0_cp_0 = this.rider;
					expr_BB_cp_0_cp_0.position.X = expr_BB_cp_0_cp_0.position.X - (float)Game1.pixelZoom;
				}
				if (this.rider.getStandingY() < this.GetBoundingBox().Y - Game1.pixelZoom)
				{
					Farmer expr_F3_cp_0_cp_0 = this.rider;
					expr_F3_cp_0_cp_0.position.Y = expr_F3_cp_0_cp_0.position.Y + (float)Game1.pixelZoom;
				}
				else if (this.rider.getStandingY() > this.GetBoundingBox().Y + Game1.pixelZoom)
				{
					Farmer expr_12D_cp_0_cp_0 = this.rider;
					expr_12D_cp_0_cp_0.position.Y = expr_12D_cp_0_cp_0.position.Y - (float)Game1.pixelZoom;
				}
				if (this.rider.yJumpOffset >= -8 && this.rider.yJumpVelocity <= 0f)
				{
					this.sprite.loop = true;
					this.rider.mountUp(this);
					this.rider.freezePause = -1;
					this.mounting = false;
					this.rider.canMove = true;
					if (this.facingDirection == 1)
					{
						this.rider.xOffset += 8f;
						return;
					}
				}
			}
			else if (this.dismounting)
			{
				if (Math.Abs(this.rider.position.X - this.dismountTile.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(this.rider.GetBoundingBox().Width / 2)) > (float)Game1.pixelZoom)
				{
					if (this.rider.position.X < this.dismountTile.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(this.rider.GetBoundingBox().Width / 2))
					{
						Farmer expr_265_cp_0_cp_0 = this.rider;
						expr_265_cp_0_cp_0.position.X = expr_265_cp_0_cp_0.position.X + Math.Min((float)Game1.pixelZoom, this.dismountTile.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(this.rider.GetBoundingBox().Width / 2) - this.rider.position.X);
					}
					else if (this.rider.position.X > this.dismountTile.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(this.rider.GetBoundingBox().Width / 2))
					{
						Farmer expr_30A_cp_0_cp_0 = this.rider;
						expr_30A_cp_0_cp_0.position.X = expr_30A_cp_0_cp_0.position.X + Math.Max((float)(-(float)Game1.pixelZoom), this.dismountTile.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(this.rider.GetBoundingBox().Width / 2) - this.rider.position.X);
					}
				}
				if (Math.Abs(this.rider.position.Y - this.dismountTile.Y * (float)Game1.tileSize + (float)Game1.pixelZoom) > (float)Game1.pixelZoom)
				{
					if (this.rider.position.Y < this.dismountTile.Y * (float)Game1.tileSize + (float)Game1.pixelZoom)
					{
						Farmer expr_3CF_cp_0_cp_0 = this.rider;
						expr_3CF_cp_0_cp_0.position.Y = expr_3CF_cp_0_cp_0.position.Y + Math.Min((float)Game1.pixelZoom, this.dismountTile.Y * (float)Game1.tileSize + (float)Game1.pixelZoom - this.rider.position.Y);
					}
					else if (this.rider.position.Y > this.dismountTile.Y * (float)Game1.tileSize + (float)Game1.pixelZoom)
					{
						Farmer expr_445_cp_0_cp_0 = this.rider;
						expr_445_cp_0_cp_0.position.Y = expr_445_cp_0_cp_0.position.Y + Math.Max((float)(-(float)Game1.pixelZoom), this.dismountTile.Y * (float)Game1.tileSize + (float)Game1.pixelZoom - this.rider.position.Y);
					}
				}
				if (this.rider.yJumpOffset >= 0 && this.rider.yJumpVelocity <= 0f)
				{
					Farmer expr_4B5_cp_0_cp_0 = this.rider;
					expr_4B5_cp_0_cp_0.position.Y = expr_4B5_cp_0_cp_0.position.Y + (float)(Game1.pixelZoom * 2);
					int num = 0;
					while (this.rider.currentLocation.isCollidingPosition(this.rider.GetBoundingBox(), Game1.viewport, true, 0, false, this.rider) && num < 6)
					{
						num++;
						Farmer expr_4D9_cp_0_cp_0 = this.rider;
						expr_4D9_cp_0_cp_0.position.Y = expr_4D9_cp_0_cp_0.position.Y - (float)Game1.pixelZoom;
					}
					if (num == 6)
					{
						this.rider.position = this.position;
						this.dismounting = false;
						this.rider.freezePause = -1;
						this.rider.canMove = true;
						return;
					}
					this.dismount();
					return;
				}
			}
			else if (this.rider != null)
			{
				this.rider.xOffset = -6f;
				this.drawOffset = new Vector2((float)(-(float)Game1.tileSize / 4), 0f);
				switch (this.facingDirection)
				{
				case 0:
					this.rider.FarmerSprite.setCurrentSingleFrame(113, 32000, false, false);
					break;
				case 1:
					this.rider.FarmerSprite.setCurrentSingleFrame(106, 32000, false, false);
					this.rider.xOffset += 2f;
					break;
				case 2:
					this.rider.FarmerSprite.setCurrentSingleFrame(107, 32000, false, false);
					break;
				case 3:
					this.rider.FarmerSprite.setCurrentSingleFrame(106, 32000, false, true);
					this.drawOffset = Vector2.Zero;
					this.rider.xOffset = -12f;
					break;
				}
				this.rider.facingDirection = this.facingDirection;
				if (!this.rider.isMoving())
				{
					this.rider.yOffset = 0f;
					return;
				}
				switch (this.sprite.currentAnimationIndex)
				{
				case 0:
					this.rider.yOffset = 0f;
					return;
				case 1:
					this.rider.yOffset = (float)(-(float)Game1.pixelZoom);
					return;
				case 2:
					this.rider.yOffset = (float)(-(float)Game1.pixelZoom);
					return;
				case 3:
					this.rider.yOffset = 0f;
					return;
				case 4:
					this.rider.yOffset = (float)Game1.pixelZoom;
					return;
				case 5:
					this.rider.yOffset = (float)Game1.pixelZoom;
					return;
				default:
					return;
				}
			}
			else if (this.facingDirection != 2 && this.sprite.currentAnimation == null && Game1.random.NextDouble() < 0.002)
			{
				this.sprite.loop = false;
				switch (this.facingDirection)
				{
				case 0:
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(25, Game1.random.Next(250, 750)),
						new FarmerSprite.AnimationFrame(14, 10)
					});
					return;
				case 1:
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(21, 100),
						new FarmerSprite.AnimationFrame(22, 100),
						new FarmerSprite.AnimationFrame(23, 400),
						new FarmerSprite.AnimationFrame(24, 400),
						new FarmerSprite.AnimationFrame(23, 400),
						new FarmerSprite.AnimationFrame(24, 400),
						new FarmerSprite.AnimationFrame(23, 400),
						new FarmerSprite.AnimationFrame(24, 400),
						new FarmerSprite.AnimationFrame(23, 400),
						new FarmerSprite.AnimationFrame(22, 100),
						new FarmerSprite.AnimationFrame(21, 100)
					});
					return;
				case 2:
					break;
				case 3:
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(21, 100, false, true, null, false),
						new FarmerSprite.AnimationFrame(22, 100, false, true, null, false),
						new FarmerSprite.AnimationFrame(23, 100, false, true, null, false),
						new FarmerSprite.AnimationFrame(24, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(23, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(24, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(23, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(24, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(23, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(22, 100, false, true, null, false),
						new FarmerSprite.AnimationFrame(21, 100, false, true, null, false)
					});
					break;
				default:
					return;
				}
			}
		}

		public override void collisionWithFarmerBehavior()
		{
			base.collisionWithFarmerBehavior();
		}

		public void dismount()
		{
			this.rider.dismount();
			this.rider.temporaryImpassableTile = new Rectangle((int)this.dismountTile.X * Game1.tileSize, (int)this.dismountTile.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			this.rider.freezePause = -1;
			this.dismounting = false;
			this.rider.canMove = true;
			this.rider.forceCanMove();
			this.rider.xOffset = 0f;
			this.rider = null;
			this.Halt();
			this.farmerPassesThrough = false;
		}

		public void nameHorse(string name)
		{
			if (name.Length > 0)
			{
				foreach (NPC current in Utility.getAllCharacters())
				{
					if (current.isVillager() && current.name.Equals(name))
					{
						name += " ";
					}
				}
				this.name = name;
				base.displayName = name;
				Game1.exitActiveMenu();
				Game1.playSound("newArtifact");
			}
		}

		public override bool checkAction(Farmer who, GameLocation l)
		{
			if (this.rider != null)
			{
				this.dismounting = true;
				this.farmerPassesThrough = false;
				this.rider.temporaryImpassableTile = Rectangle.Empty;
				Vector2 value = Utility.recursiveFindOpenTileForCharacter(this.rider, this.rider.currentLocation, this.rider.getTileLocation(), 8);
				this.dismounting = false;
				this.Halt();
				if (!value.Equals(Vector2.Zero) && Vector2.Distance(value, this.rider.getTileLocation()) < 2f)
				{
					this.rider.yJumpVelocity = 6f;
					this.rider.yJumpOffset = -1;
					Game1.playSound("dwop");
					this.rider.freezePause = 5000;
					this.rider.Halt();
					this.rider.xOffset = 0f;
					this.dismounting = true;
					this.dismountTile = value;
					Game1.debugOutput = "dismount tile: " + value.ToString();
				}
				else
				{
					this.dismount();
				}
				return true;
			}
			if (this.name.Length <= 0)
			{
				Game1.activeClickableMenu = new NamingMenu(new NamingMenu.doneNamingBehavior(this.nameHorse), Game1.content.LoadString("Strings\\Characters:NameYourHorse", new object[0]), Game1.content.LoadString("Strings\\Characters:DefaultHorseName", new object[0]));
				return true;
			}
			this.rider = who;
			this.rider.freezePause = 5000;
			this.rider.yJumpVelocity = 6f;
			this.rider.yJumpOffset = -1;
			this.rider.faceGeneralDirection(Utility.PointToVector2(this.GetBoundingBox().Center), 0);
			this.rider.showNotCarrying();
			this.rider.Halt();
			if (this.rider.position.X < this.position.X)
			{
				this.rider.faceDirection(1);
			}
			Game1.playSound("dwop");
			this.mounting = true;
			return true;
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (this.facingDirection == 2 && this.rider != null)
			{
				b.Draw(this.sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize * 3 / 4), (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2) - this.rider.yOffset), new Rectangle?(new Rectangle(160, 96, 9, 15)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (this.position.Y + (float)Game1.tileSize) / 10000f);
			}
		}
	}
}
