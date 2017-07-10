using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Projectiles;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Tools
{
	public class Slingshot : Tool
	{
		public const int basicDamage = 5;

		public const int basicSlingshot = 32;

		public const int masterSlingshot = 33;

		public const int galaxySlingshot = 34;

		public const int drawBackSoundThreshold = 8;

		[XmlIgnore]
		public int recentClickX;

		[XmlIgnore]
		public int recentClickY;

		[XmlIgnore]
		public int lastClickX;

		[XmlIgnore]
		public int lastClickY;

		[XmlIgnore]
		public int mouseDragAmount;

		private bool canPlaySound;

		private bool startedWithGamePad;

		public Slingshot()
		{
			this.initialParentTileIndex = 32;
			this.currentParentTileIndex = this.initialParentTileIndex;
			this.indexOfMenuItemView = this.currentParentTileIndex;
			string[] array = Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			});
			this.name = array[0];
			this.numAttachmentSlots = 1;
			this.attachments = new StardewValley.Object[1];
		}

		protected override string loadDisplayName()
		{
			string[] array = Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			});
			if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
			{
				return array[array.Length - 1];
			}
			return this.name;
		}

		protected override string loadDescription()
		{
			return Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			})[1];
		}

		public override bool doesShowTileLocationMarker()
		{
			return false;
		}

		public Slingshot(int which = 32)
		{
			this.initialParentTileIndex = which;
			this.currentParentTileIndex = this.initialParentTileIndex;
			this.indexOfMenuItemView = this.currentParentTileIndex;
			string[] array = Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			});
			this.name = array[0];
			this.numAttachmentSlots = 1;
			this.attachments = new StardewValley.Object[1];
		}

		public bool didStartWithGamePad()
		{
			return this.startedWithGamePad;
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			this.indexOfMenuItemView = this.initialParentTileIndex;
			who.usingSlingshot = false;
			who.canReleaseTool = true;
			who.usingTool = false;
			who.canMove = true;
			if (this.attachments[0] != null)
			{
				StardewValley.Object @object = (StardewValley.Object)this.attachments[0].getOne();
				StardewValley.Object expr_54 = this.attachments[0];
				int num = expr_54.Stack;
				expr_54.Stack = num - 1;
				if (this.attachments[0].Stack <= 0)
				{
					this.attachments[0] = null;
				}
				int num2 = Game1.getOldMouseX() + Game1.viewport.X;
				int num3 = Game1.getOldMouseY() + Game1.viewport.Y;
				if (this.startedWithGamePad)
				{
					Point expr_107 = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					num2 = expr_107.X;
					num3 = expr_107.Y;
				}
				int arg_195_0 = Math.Min(20, (int)Vector2.Distance(new Vector2((float)who.getStandingX(), (float)(who.getStandingY() - Game1.tileSize)), new Vector2((float)num2, (float)num3)) / 20);
				Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(who.getStandingX(), who.getStandingY() + Game1.tileSize), new Vector2((float)num2, (float)(num3 + Game1.tileSize)), (float)(15 + Game1.random.Next(4, 6)) * (1f + who.weaponSpeedModifier));
				if (arg_195_0 > 4 && !this.canPlaySound)
				{
					int num4 = 1;
					BasicProjectile.onCollisionBehavior collisionBehavior = null;
					string collisionSound = "hammer";
					float num5 = 1f;
					if (this.initialParentTileIndex == 33)
					{
						num5 = 2f;
					}
					else if (this.initialParentTileIndex == 34)
					{
						num5 = 4f;
					}
					num = @object.ParentSheetIndex;
					switch (num)
					{
					case 378:
					{
						num4 = 10;
						StardewValley.Object expr_270 = @object;
						num = expr_270.ParentSheetIndex;
						expr_270.ParentSheetIndex = num + 1;
						break;
					}
					case 379:
					case 381:
					case 383:
					case 385:
					case 387:
					case 389:
						break;
					case 380:
					{
						num4 = 20;
						StardewValley.Object expr_288 = @object;
						num = expr_288.ParentSheetIndex;
						expr_288.ParentSheetIndex = num + 1;
						break;
					}
					case 382:
					{
						num4 = 15;
						StardewValley.Object expr_2B8 = @object;
						num = expr_2B8.ParentSheetIndex;
						expr_2B8.ParentSheetIndex = num + 1;
						break;
					}
					case 384:
					{
						num4 = 30;
						StardewValley.Object expr_2A0 = @object;
						num = expr_2A0.ParentSheetIndex;
						expr_2A0.ParentSheetIndex = num + 1;
						break;
					}
					case 386:
					{
						num4 = 50;
						StardewValley.Object expr_2D0 = @object;
						num = expr_2D0.ParentSheetIndex;
						expr_2D0.ParentSheetIndex = num + 1;
						break;
					}
					case 388:
					{
						num4 = 2;
						StardewValley.Object expr_23B = @object;
						num = expr_23B.ParentSheetIndex;
						expr_23B.ParentSheetIndex = num + 1;
						break;
					}
					case 390:
					{
						num4 = 5;
						StardewValley.Object expr_255 = @object;
						num = expr_255.ParentSheetIndex;
						expr_255.ParentSheetIndex = num + 1;
						break;
					}
					default:
						if (num == 441)
						{
							num4 = 20;
							collisionBehavior = new BasicProjectile.onCollisionBehavior(BasicProjectile.explodeOnImpact);
							collisionSound = "explosion";
						}
						break;
					}
					num = @object.category;
					if (num == -5)
					{
						collisionSound = "slimedead";
					}
					location.projectiles.Add(new BasicProjectile((int)(num5 * (float)(num4 + Game1.random.Next(-(num4 / 2), num4 + 2)) * (1f + who.attackIncreaseModifier)), @object.ParentSheetIndex, 0, 0, (float)(3.1415926535897931 / (double)(64f + (float)Game1.random.Next(-63, 64))), -velocityTowardPoint.X, -velocityTowardPoint.Y, new Vector2((float)(who.getStandingX() - 16), (float)(who.getStandingY() - Game1.tileSize - 8)), collisionSound, "", false, true, who, true, collisionBehavior)
					{
						ignoreLocationCollision = (Game1.currentLocation.currentEvent != null)
					});
				}
			}
			else
			{
				Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Slingshot.cs.14254", new object[0]));
			}
			this.canPlaySound = true;
			who.Halt();
		}

		public override bool canThisBeAttached(StardewValley.Object o)
		{
			return o == null || (!o.bigCraftable && ((o.parentSheetIndex >= 378 && o.parentSheetIndex <= 390) || o.category == -5 || o.category == -79 || o.category == -75 || o.parentSheetIndex == 441));
		}

		public override StardewValley.Object attach(StardewValley.Object o)
		{
			StardewValley.Object arg_1B_0 = this.attachments[0];
			this.attachments[0] = o;
			Game1.playSound("button1");
			return arg_1B_0;
		}

		public override string getHoverBoxText(Item hoveredItem)
		{
			if (hoveredItem != null && hoveredItem is StardewValley.Object && this.canThisBeAttached(hoveredItem as StardewValley.Object))
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Slingshot.cs.14256", new object[]
				{
					this.DisplayName,
					hoveredItem.DisplayName
				});
			}
			if (hoveredItem == null && this.attachments != null && this.attachments[0] != null)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Slingshot.cs.14258", new object[]
				{
					this.attachments[0].DisplayName
				});
			}
			return null;
		}

		public override bool onRelease(GameLocation location, int x, int y, Farmer who)
		{
			this.DoFunction(location, x, y, 1, who);
			return true;
		}

		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			who.usingSlingshot = true;
			who.canReleaseTool = false;
			this.mouseDragAmount = 0;
			int num = (who.FacingDirection == 3 || who.FacingDirection == 1) ? 1 : ((who.FacingDirection == 0) ? 2 : 0);
			who.FarmerSprite.setCurrentFrame(42 + num);
			double num2 = (double)(Game1.getOldMouseX() + Game1.viewport.X - who.getStandingX());
			double num3 = (double)(Game1.getOldMouseY() + Game1.viewport.Y - who.getStandingY());
			if (Math.Abs(num2) > Math.Abs(num3))
			{
				num2 /= Math.Abs(num2);
				num3 = 0.5;
			}
			else
			{
				num3 /= Math.Abs(num3);
				num2 = 0.0;
			}
			num2 *= 16.0;
			num3 *= 16.0;
			if (this.didStartWithGamePad())
			{
				Mouse.SetPosition(who.getStandingX() - Game1.viewport.X + (int)num2, who.getStandingY() - Game1.viewport.Y + (int)num3);
				Game1.lastCursorMotionWasMouse = false;
			}
			Game1.oldMouseState = Mouse.GetState();
			Game1.lastMousePositionBeforeFade = Game1.getMousePosition();
			this.lastClickX = Game1.getOldMouseX() + Game1.viewport.X;
			this.lastClickY = Game1.getOldMouseY() + Game1.viewport.Y;
			this.startedWithGamePad = false;
			if (Game1.options.gamepadControls && GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.X))
			{
				this.startedWithGamePad = true;
			}
			return true;
		}

		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (who.usingSlingshot)
			{
				Point point = Game1.getMousePosition();
				if (this.startedWithGamePad)
				{
					point = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					point.X -= Game1.viewport.X;
					point.Y -= Game1.viewport.Y;
				}
				int num = point.X + Game1.viewport.X;
				int num2 = point.Y + Game1.viewport.Y;
				Game1.debugOutput = string.Concat(new object[]
				{
					"playerPos: ",
					Game1.player.getStandingPosition().ToString(),
					", mousePos: ",
					num,
					", ",
					num2
				});
				this.mouseDragAmount++;
				who.faceGeneralDirection(new Vector2((float)num, (float)num2), 0);
				who.faceDirection((who.FacingDirection + 2) % 4);
				int num3 = (who.FacingDirection == 3 || who.FacingDirection == 1) ? 1 : ((who.FacingDirection == 0) ? 2 : 0);
				who.FarmerSprite.setCurrentFrame(42 + num3);
				if (this.canPlaySound && (Math.Abs(num - this.lastClickX) > 8 || Math.Abs(num2 - this.lastClickY) > 8) && this.mouseDragAmount > 4)
				{
					Game1.playSound("slingshot");
					this.canPlaySound = false;
				}
				this.lastClickX = num;
				this.lastClickY = num2;
				Game1.mouseCursor = -1;
			}
		}

		public override void drawAttachments(SpriteBatch b, int x, int y)
		{
			if (this.attachments[0] == null)
			{
				b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 43, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
				return;
			}
			b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
			this.attachments[0].drawInMenu(b, new Vector2((float)x, (float)y), 1f);
		}

		public override void draw(SpriteBatch b)
		{
			if (Game1.player.usingSlingshot)
			{
				int num = Game1.getOldMouseX() + Game1.viewport.X;
				int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
				if (this.startedWithGamePad)
				{
					Point expr_98 = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					num = expr_98.X;
					num2 = expr_98.Y;
				}
				Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(Game1.player.getStandingX(), Game1.player.getStandingY() + Game1.tileSize / 2), new Vector2((float)num, (float)num2), 256f);
				if (Math.Abs(velocityTowardPoint.X) < 1f)
				{
					int arg_F5_0 = this.mouseDragAmount;
				}
				double num3 = Math.Sqrt((double)(velocityTowardPoint.X * velocityTowardPoint.X + velocityTowardPoint.Y * velocityTowardPoint.Y)) - 181.0;
				double num4 = (double)(velocityTowardPoint.X / 256f);
				double num5 = (double)(velocityTowardPoint.Y / 256f);
				int num6 = (int)((double)velocityTowardPoint.X - num3 * num4);
				int num7 = (int)((double)velocityTowardPoint.Y - num3 * num5);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(Game1.player.getStandingX() - num6), (float)(Game1.player.getStandingY() - Game1.tileSize - 8 - num7))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 43, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 1f, SpriteEffects.None, 0.999999f);
			}
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			if (this.indexOfMenuItemView == 0 || this.indexOfMenuItemView == 21 || this.indexOfMenuItemView == 47 || this.currentParentTileIndex == 47)
			{
				string name = this.name;
				if (!(name == "Slingshot"))
				{
					if (!(name == "Master Slingshot"))
					{
						if (name == "Galaxy Slingshot")
						{
							this.currentParentTileIndex = 34;
						}
					}
					else
					{
						this.currentParentTileIndex = 33;
					}
				}
				else
				{
					this.currentParentTileIndex = 32;
				}
				this.indexOfMenuItemView = this.currentParentTileIndex;
			}
			spriteBatch.Draw(Tool.weaponsTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 3 + 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, this.indexOfMenuItemView, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), scaleSize * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
			if (drawStackNumber && this.attachments != null && this.attachments[0] != null)
			{
				Utility.drawTinyDigits(this.attachments[0].Stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.attachments[0].Stack, 3f * scaleSize)) + 3f * scaleSize, (float)Game1.tileSize - 18f * scaleSize + 2f), 3f * scaleSize, 1f, Color.White);
			}
		}
	}
}
