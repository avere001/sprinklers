using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley.Objects
{
	public class Wallpaper : StardewValley.Object
	{
		public Rectangle sourceRect;

		public static Texture2D wallpaperTexture;

		public bool isFloor;

		private static readonly Rectangle wallpaperContainerRect = new Rectangle(193, 496, 16, 16);

		private static readonly Rectangle floorContainerRect = new Rectangle(209, 496, 16, 16);

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public Wallpaper()
		{
		}

		public Wallpaper(int which, bool isFloor = false)
		{
			if (Wallpaper.wallpaperTexture == null)
			{
				Wallpaper.wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
			}
			this.isFloor = isFloor;
			this.parentSheetIndex = which;
			this.name = (isFloor ? "Flooring" : "Wallpaper");
			this.sourceRect = (isFloor ? new Rectangle(which % 8 * 32, 336 + which / 8 * 32, 28, 26) : new Rectangle(which % 16 * 16, which / 16 * 48 + 8, 16, 28));
			this.price = 100;
		}

		protected override string loadDisplayName()
		{
			if (!this.isFloor)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13204", new object[0]);
			}
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13203", new object[0]);
		}

		public override string getDescription()
		{
			if (!this.isFloor)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13206", new object[0]);
			}
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13205", new object[0]);
		}

		public override bool performDropDownAction(Farmer who)
		{
			return true;
		}

		public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
		{
			return false;
		}

		public override bool canBePlacedHere(GameLocation l, Vector2 tile)
		{
			Vector2 vector = tile * (float)Game1.tileSize;
			vector.X += (float)(Game1.tileSize / 2);
			vector.Y += (float)(Game1.tileSize / 2);
			if (l is DecoratableLocation)
			{
				foreach (Furniture current in (l as DecoratableLocation).furniture)
				{
					if (current.furniture_type != 12 && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y))
					{
						return false;
					}
				}
			}
			return base.canBePlacedHere(l, tile);
		}

		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			if (who.currentLocation is DecoratableLocation)
			{
				Point value = new Point(x / Game1.tileSize, y / Game1.tileSize);
				DecoratableLocation decoratableLocation = who.currentLocation as DecoratableLocation;
				if (this.isFloor)
				{
					List<Rectangle> floors;
					if (decoratableLocation is FarmHouse)
					{
						floors = FarmHouse.getFloors((decoratableLocation as FarmHouse).upgradeLevel);
					}
					else
					{
						floors = DecoratableLocation.getFloors();
					}
					for (int i = 0; i < floors.Count; i++)
					{
						if (floors[i].Contains(value))
						{
							decoratableLocation.setFloor(this.parentSheetIndex, i, true);
							Game1.playSound("coin");
							return true;
						}
					}
				}
				else
				{
					List<Rectangle> walls;
					if (decoratableLocation is FarmHouse)
					{
						walls = FarmHouse.getWalls((decoratableLocation as FarmHouse).upgradeLevel);
					}
					else
					{
						walls = DecoratableLocation.getWalls();
					}
					for (int j = 0; j < walls.Count; j++)
					{
						if (walls[j].Contains(value))
						{
							decoratableLocation.setWallpaper(this.parentSheetIndex, j, true);
							Game1.playSound("coin");
							return true;
						}
					}
				}
			}
			return false;
		}

		public override bool isPlaceable()
		{
			return true;
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return this.boundingBox;
		}

		public override int salePrice()
		{
			return this.price;
		}

		public override int maximumStackSize()
		{
			return 1;
		}

		public override int getStack()
		{
			return this.stack;
		}

		public override int addToStack(int amount)
		{
			return 1;
		}

		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			base.drawInMenu(spriteBatch, objectPosition, 1f);
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			if (Wallpaper.wallpaperTexture == null)
			{
				Wallpaper.wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
			}
			if (this.isFloor)
			{
				spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(Wallpaper.floorContainerRect), Color.White * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
				spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2 - 2)), new Rectangle?(this.sourceRect), Color.White * transparency, 0f, new Vector2(14f, 13f), 2f * scaleSize, SpriteEffects.None, layerDepth + 0.001f);
				return;
			}
			spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(Wallpaper.wallpaperContainerRect), Color.White * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
			spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.sourceRect), Color.White * transparency, 0f, new Vector2(8f, 14f), 2f * scaleSize, SpriteEffects.None, layerDepth + 0.001f);
		}

		public override Item getOne()
		{
			return new Wallpaper(this.parentSheetIndex, this.isFloor);
		}
	}
}
