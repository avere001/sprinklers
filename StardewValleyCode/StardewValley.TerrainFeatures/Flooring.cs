using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.TerrainFeatures
{
	public class Flooring : TerrainFeature
	{
		public const byte N = 1;

		public const byte E = 2;

		public const byte S = 4;

		public const byte W = 8;

		public const int wood = 0;

		public const int stone = 1;

		public const int ghost = 2;

		public const int iceTile = 3;

		public const int straw = 4;

		public const int gravel = 5;

		public const int boardwalk = 6;

		public const int colored_cobblestone = 7;

		public const int cobblestone = 8;

		public const int steppingStone = 9;

		public static Texture2D floorsTexture;

		public static Dictionary<byte, int> drawGuide;

		public int whichFloor;

		public int whichView;

		private bool isPathway;

		private bool isSteppingStone;

		public Flooring()
		{
			this.loadSprite();
			if (Flooring.drawGuide == null)
			{
				Flooring.populateDrawGuide();
			}
		}

		public Flooring(int which) : this()
		{
			this.whichFloor = which;
			if (this.whichFloor == 5 || this.whichFloor == 6 || this.whichFloor == 8 || this.whichFloor == 7)
			{
				this.isPathway = true;
			}
			if (this.whichFloor == 9)
			{
				this.whichView = Game1.random.Next(16);
				this.isSteppingStone = true;
				this.isPathway = true;
			}
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.Y * (float)Game1.tileSize), Game1.tileSize, Game1.tileSize);
		}

		public static void populateDrawGuide()
		{
			Flooring.drawGuide = new Dictionary<byte, int>();
			Flooring.drawGuide.Add(0, 0);
			Flooring.drawGuide.Add(6, 1);
			Flooring.drawGuide.Add(14, 2);
			Flooring.drawGuide.Add(12, 3);
			Flooring.drawGuide.Add(4, 16);
			Flooring.drawGuide.Add(7, 17);
			Flooring.drawGuide.Add(15, 18);
			Flooring.drawGuide.Add(13, 19);
			Flooring.drawGuide.Add(5, 32);
			Flooring.drawGuide.Add(3, 33);
			Flooring.drawGuide.Add(11, 34);
			Flooring.drawGuide.Add(9, 35);
			Flooring.drawGuide.Add(1, 48);
			Flooring.drawGuide.Add(2, 49);
			Flooring.drawGuide.Add(10, 50);
			Flooring.drawGuide.Add(8, 51);
		}

		public override void loadSprite()
		{
			if (Flooring.floorsTexture == null)
			{
				try
				{
					Flooring.floorsTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\Flooring");
				}
				catch (Exception)
				{
				}
			}
			if (this.whichFloor == 5 || this.whichFloor == 6 || this.whichFloor == 8 || this.whichFloor == 7 || this.whichFloor == 9)
			{
				this.isPathway = true;
			}
			if (this.whichFloor == 9)
			{
				this.isSteppingStone = true;
			}
		}

		public override void doCollisionAction(Rectangle positionOfCollider, int speedOfCollision, Vector2 tileLocation, Character who, GameLocation location)
		{
			base.doCollisionAction(positionOfCollider, speedOfCollision, tileLocation, who, location);
			if (who != null && who is Farmer && location is Farm)
			{
				(who as Farmer).temporarySpeedBuff = 0.1f;
			}
		}

		public override bool isPassable(Character c = null)
		{
			return true;
		}

		public string getFootstepSound()
		{
			switch (this.whichFloor)
			{
			case 0:
			case 2:
			case 4:
				return "woodyStep";
			case 1:
				return "stoneStep";
			case 3:
			case 6:
				return "thudStep";
			case 5:
				return "dirtyHit";
			default:
				return "stoneStep";
			}
		}

		public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			if ((t != null || damage > 0) && (damage > 0 || t.GetType() == typeof(Pickaxe) || t.GetType() == typeof(Axe)))
			{
				Game1.createRadialDebris(location, (this.whichFloor == 0) ? 12 : 14, (int)tileLocation.X, (int)tileLocation.Y, 4, false, -1, false, -1);
				int parentSheetIndex = -1;
				switch (this.whichFloor)
				{
				case 0:
					Game1.playSound("axchop");
					parentSheetIndex = 328;
					break;
				case 1:
					Game1.playSound("hammer");
					parentSheetIndex = 329;
					break;
				case 2:
					Game1.playSound("axchop");
					parentSheetIndex = 331;
					break;
				case 3:
					Game1.playSound("hammer");
					parentSheetIndex = 333;
					break;
				case 4:
					Game1.playSound("axchop");
					parentSheetIndex = 401;
					break;
				case 5:
					Game1.playSound("hammer");
					parentSheetIndex = 407;
					break;
				case 6:
					Game1.playSound("axchop");
					parentSheetIndex = 405;
					break;
				case 7:
					Game1.playSound("hammer");
					parentSheetIndex = 409;
					break;
				case 8:
					Game1.playSound("hammer");
					parentSheetIndex = 411;
					break;
				case 9:
					Game1.playSound("hammer");
					parentSheetIndex = 415;
					break;
				}
				location.debris.Add(new Debris(new StardewValley.Object(parentSheetIndex, 1, false, -1, 0), tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
				return true;
			}
			return false;
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			int num = this.whichFloor * 4 * Game1.tileSize;
			byte b = 0;
			Vector2 key = tileLocation;
			key.X += 1f;
			GameLocation locationFromName = Game1.getLocationFromName("Farm");
			if (locationFromName.terrainFeatures.ContainsKey(key) && locationFromName.terrainFeatures[key].GetType() == typeof(Flooring))
			{
				b += 2;
			}
			key.X -= 2f;
			if (locationFromName.terrainFeatures.ContainsKey(key) && Game1.currentLocation.terrainFeatures[key].GetType() == typeof(Flooring))
			{
				b += 8;
			}
			key.X += 1f;
			key.Y += 1f;
			if (Game1.currentLocation.terrainFeatures.ContainsKey(key) && locationFromName.terrainFeatures[key].GetType() == typeof(Flooring))
			{
				b += 4;
			}
			key.Y -= 2f;
			if (locationFromName.terrainFeatures.ContainsKey(key) && locationFromName.terrainFeatures[key].GetType() == typeof(Flooring))
			{
				b += 1;
			}
			int num2 = Flooring.drawGuide[b];
			spriteBatch.Draw(Flooring.floorsTexture, positionOnScreen, new Rectangle?(new Rectangle(num2 % 16 * 16, num2 / 16 * 16 + num, 16, 16)), Color.White, 0f, Vector2.Zero, scale * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth + positionOnScreen.Y / 20000f);
		}

		private bool doesTileCountForDrawing(Vector2 surroundingLocations)
		{
			TerrainFeature terrainFeature;
			Game1.currentLocation.terrainFeatures.TryGetValue(surroundingLocations, out terrainFeature);
			return terrainFeature != null && terrainFeature is Flooring && (terrainFeature as Flooring).whichFloor == this.whichFloor;
		}

		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			byte b = 0;
			Vector2 surroundingLocations = tileLocation;
			surroundingLocations.X += 1f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				b += 2;
			}
			surroundingLocations.X -= 2f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				b += 8;
			}
			surroundingLocations.X += 1f;
			surroundingLocations.Y += 1f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				b += 4;
			}
			surroundingLocations.Y -= 2f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				b += 1;
			}
			surroundingLocations.X -= 1f;
			if (!this.isPathway)
			{
				if (!this.doesTileCountForDrawing(surroundingLocations) && (b & 9) == 9)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(60 + 64 * (this.whichFloor % 4), 44 + this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f) / 20000f);
				}
				surroundingLocations.X += 2f;
				if (!this.doesTileCountForDrawing(surroundingLocations) && (b & 3) == 3)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4), tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(16 + 64 * (this.whichFloor % 4), 44 + this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f + (float)this.whichFloor) / 20000f);
				}
				surroundingLocations.Y += 2f;
				if (!this.doesTileCountForDrawing(surroundingLocations) && (b & 6) == 6)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4))), new Rectangle?(new Rectangle(16 + 64 * (this.whichFloor % 4), this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f) / 20000f);
				}
				surroundingLocations.X -= 2f;
				if (!this.doesTileCountForDrawing(surroundingLocations) && (b & 12) == 12)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4))), new Rectangle?(new Rectangle(60 + 64 * (this.whichFloor % 4), this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f) / 20000f);
				}
				spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)(tileLocation.X * (float)Game1.tileSize) - 4 - Game1.viewport.X, (int)(tileLocation.Y * (float)Game1.tileSize) + 4 - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Black * 0.33f);
			}
			int num = Flooring.drawGuide[b];
			if (this.isSteppingStone)
			{
				num = Flooring.drawGuide.ElementAt(this.whichView).Value;
			}
			spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(this.whichFloor % 4 * 64 + num * 16 % 256, num / 16 * 16 + this.whichFloor / 4 * 64, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-09f);
		}

		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			return false;
		}

		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}
	}
}
