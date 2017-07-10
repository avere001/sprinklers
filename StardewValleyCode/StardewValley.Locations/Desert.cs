using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class Desert : GameLocation
	{
		public const int busDefaultXTile = 17;

		public const int busDefaultYTile = 24;

		private TemporaryAnimatedSprite busDoor;

		private Vector2 busPosition;

		private Vector2 busMotion;

		private bool drivingOff;

		private bool drivingBack;

		private Microsoft.Xna.Framework.Rectangle busSource = new Microsoft.Xna.Framework.Rectangle(288, 1247, 128, 64);

		private Microsoft.Xna.Framework.Rectangle pamSource = new Microsoft.Xna.Framework.Rectangle(384, 1311, 15, 19);

		private Vector2 pamOffset = new Vector2(0f, 29f);

		public Desert()
		{
		}

		public Desert(Map map, string name) : base(map, name)
		{
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int arg_3D_0 = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				return base.checkAction(tileLocation, viewport, who);
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		private void pamReachedBusDoor(Character c, GameLocation l)
		{
			Game1.changeMusicTrack("none");
			c.position.X = -10000f;
			Game1.playSound("stoneStep");
		}

		private void playerReachedBusDoor(Character c, GameLocation l)
		{
			Game1.player.position.X = -10000f;
			this.busDriveOff();
			Game1.playSound("stoneStep");
		}

		public override bool answerDialogue(Response answer)
		{
			string a = this.lastQuestionKey.Split(new char[]
			{
				' '
			})[0] + "_" + answer.responseKey;
			if (a == "DesertBus_Yes")
			{
				this.playerReachedBusDoor(Game1.player, this);
				return true;
			}
			return base.answerDialogue(answer);
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			Game1.ambientLight = Color.White;
			if (Game1.player.getTileY() > 40 || Game1.player.getTileY() < 10)
			{
				this.drivingOff = false;
				this.drivingBack = false;
				this.busMotion = Vector2.Zero;
				this.busPosition = new Vector2(17f, 24f) * (float)Game1.tileSize;
				this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
				{
					interval = 999999f,
					animationLength = 6,
					holdLastFrame = true,
					layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
					scale = (float)Game1.pixelZoom
				};
				Game1.changeMusicTrack("wavy");
				return;
			}
			if (Game1.isRaining)
			{
				Game1.changeMusicTrack("none");
			}
			this.busPosition = new Vector2(17f, 24f) * (float)Game1.tileSize;
			this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 999999f,
				animationLength = 1,
				holdLastFrame = true,
				layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
				scale = (float)Game1.pixelZoom
			};
			Game1.displayFarmer = false;
			this.busDriveBack();
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.busDoor = null;
		}

		public void busDriveOff()
		{
			this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 999999f,
				animationLength = 6,
				holdLastFrame = true,
				layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
				scale = (float)Game1.pixelZoom
			};
			this.busDoor.timer = 0f;
			this.busDoor.interval = 70f;
			this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.busStartMovingOff);
			Game1.playSound("trashcanlid");
			this.drivingBack = false;
			this.busDoor.paused = false;
			Game1.changeMusicTrack("none");
		}

		public void busDriveBack()
		{
			this.busPosition.X = (float)this.map.GetLayer("Back").DisplayWidth;
			this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom;
			this.drivingBack = true;
			this.drivingOff = false;
			Game1.playSound("busDriveOff");
			this.busMotion = new Vector2(-6f, 0f);
		}

		private void busStartMovingOff(int extraInfo)
		{
			Game1.playSound("batFlap");
			this.drivingOff = true;
			Game1.playSound("busDriveOff");
		}

		public override void performTouchAction(string fullActionString, Vector2 playerStandingPosition)
		{
			string a = fullActionString.Split(new char[]
			{
				' '
			})[0];
			if (a == "DesertBus")
			{
				Response[] answerChoices = new Response[]
				{
					new Response("Yes", Game1.content.LoadString("Strings\\Locations:Desert_Return_Yes", new object[0])),
					new Response("Not", Game1.content.LoadString("Strings\\Locations:Desert_Return_No", new object[0]))
				};
				base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Desert_Return_Question", new object[0]), answerChoices, "DesertBus");
				return;
			}
			base.performTouchAction(fullActionString, playerStandingPosition);
		}

		private void doorOpenAfterReturn(int extraInfo)
		{
			Game1.playSound("batFlap");
			this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 999999f,
				animationLength = 6,
				holdLastFrame = true,
				layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
				scale = (float)Game1.pixelZoom
			};
			Game1.player.position = new Vector2(18f, 27f) * (float)Game1.tileSize;
			this.lastTouchActionLocation = Game1.player.getTileLocation();
			Game1.displayFarmer = true;
			Game1.player.forceCanMove();
			Game1.player.faceDirection(2);
			Game1.changeMusicTrack("wavy");
		}

		private void busLeftToValley()
		{
			Game1.viewport.Y = -100000;
			Game1.viewportFreeze = true;
			Game1.warpFarmer("BusStop", 12, 10, true);
			Game1.freezeControls = false;
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.drivingOff)
			{
				this.busMotion.X = this.busMotion.X - 0.075f;
				if (this.busPosition.X + (float)(8 * Game1.tileSize) < 0f)
				{
					this.drivingOff = false;
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.busLeftToValley), 0.01f);
				}
			}
			if (this.drivingBack)
			{
				Game1.player.position = this.busDoor.position;
				Game1.player.freezePause = 100;
				if (this.busPosition.X - (float)(17 * Game1.tileSize) < (float)(Game1.tileSize * 4))
				{
					this.busMotion.X = Math.Min(-1f, this.busMotion.X * 0.98f);
				}
				if (Math.Abs(this.busPosition.X - (float)(17 * Game1.tileSize)) <= Math.Abs(this.busMotion.X * 1.5f))
				{
					this.busPosition.X = (float)(17 * Game1.tileSize);
					this.busMotion = Vector2.Zero;
					this.drivingBack = false;
					this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom;
					this.busDoor.pingPong = true;
					this.busDoor.interval = 70f;
					this.busDoor.currentParentTileIndex = 5;
					this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.doorOpenAfterReturn);
					Game1.playSound("trashcanlid");
				}
			}
			if (!this.busMotion.Equals(Vector2.Zero))
			{
				this.busPosition += this.busMotion;
				this.busDoor.Position += this.busMotion;
			}
			if (this.busDoor != null)
			{
				this.busDoor.update(time);
			}
		}

		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition), new Microsoft.Xna.Framework.Rectangle?(this.busSource), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f);
			if (this.busDoor != null)
			{
				this.busDoor.draw(spriteBatch, false, 0, 0);
			}
			if (this.drivingOff || this.drivingBack)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition + this.pamOffset * (float)Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(this.pamSource), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (this.busPosition.Y + (float)(3 * Game1.tileSize) + (float)Game1.pixelZoom) / 10000f);
			}
		}
	}
}
