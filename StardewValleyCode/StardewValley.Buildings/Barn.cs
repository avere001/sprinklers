using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile;

namespace StardewValley.Buildings
{
	public class Barn : Building
	{
		public static int openAnimalDoorPosition = -Game1.tileSize - 24 + Game1.pixelZoom * 3;

		private const int closedAnimalDoorPosition = 0;

		private int yPositionOfAnimalDoor;

		private int animalDoorMotion;

		public Barn(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
		}

		public Barn()
		{
		}

		protected override GameLocation getIndoors()
		{
			if (this.indoors != null)
			{
				this.nameOfIndoorsWithoutUnique = this.indoors.name;
			}
			string nameOfIndoorsWithoutUnique = this.nameOfIndoorsWithoutUnique;
			if (!(nameOfIndoorsWithoutUnique == "Big Barn"))
			{
				if (nameOfIndoorsWithoutUnique == "Deluxe Barn")
				{
					this.nameOfIndoorsWithoutUnique = "Barn3";
				}
			}
			else
			{
				this.nameOfIndoorsWithoutUnique = "Barn2";
			}
			GameLocation gameLocation = new AnimalHouse(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
			gameLocation.IsFarm = true;
			gameLocation.isStructure = true;
			nameOfIndoorsWithoutUnique = this.nameOfIndoorsWithoutUnique;
			if (!(nameOfIndoorsWithoutUnique == "Big Barn"))
			{
				if (nameOfIndoorsWithoutUnique == "Deluxe Barn")
				{
					(gameLocation as AnimalHouse).animalLimit = 12;
				}
			}
			else
			{
				(gameLocation as AnimalHouse).animalLimit = 8;
			}
			foreach (Warp expr_E0 in gameLocation.warps)
			{
				expr_E0.TargetX = this.humanDoor.X + this.tileX;
				expr_E0.TargetY = this.humanDoor.Y + this.tileY + 1;
			}
			if (this.animalDoorOpen)
			{
				this.yPositionOfAnimalDoor = Barn.openAnimalDoorPosition;
			}
			return gameLocation;
		}

		public override Rectangle getRectForAnimalDoor()
		{
			return new Rectangle((this.animalDoor.X + this.tileX) * Game1.tileSize, (this.tileY + this.animalDoor.Y) * Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
		}

		public override bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (this.daysOfConstructionLeft <= 0 && (tileLocation.X == (float)(this.tileX + this.animalDoor.X) || tileLocation.X == (float)(this.tileX + this.animalDoor.X + 1)) && tileLocation.Y == (float)(this.tileY + this.animalDoor.Y))
			{
				if (!this.animalDoorOpen)
				{
					Game1.playSound("doorCreak");
				}
				else
				{
					Game1.playSound("doorCreakReverse");
				}
				this.animalDoorOpen = !this.animalDoorOpen;
				this.animalDoorMotion = (this.animalDoorOpen ? -3 : 2);
				return true;
			}
			return base.doAction(tileLocation, who);
		}

		public override void updateWhenFarmNotCurrentLocation(GameTime time)
		{
			base.updateWhenFarmNotCurrentLocation(time);
			((AnimalHouse)this.indoors).updateWhenNotCurrentLocation(this, time);
		}

		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height - 16);
		}

		public override void performActionOnUpgrade(GameLocation location)
		{
			(this.indoors as AnimalHouse).animalLimit += 4;
			if ((this.indoors as AnimalHouse).animalLimit == 8)
			{
				StardewValley.Object @object = new StardewValley.Object(new Vector2(1f, 3f), 104, false);
				@object.fragility = 2;
				this.indoors.objects.Add(new Vector2(1f, 3f), @object);
			}
		}

		public override void performActionOnConstruction(GameLocation location)
		{
			base.performActionOnConstruction(location);
			StardewValley.Object @object = new StardewValley.Object(new Vector2(6f, 3f), 99, false);
			@object.fragility = 2;
			this.indoors.objects.Add(new Vector2(6f, 3f), @object);
			this.daysOfConstructionLeft = 3;
		}

		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			if (this.daysOfConstructionLeft <= 0)
			{
				this.currentOccupants = ((AnimalHouse)this.indoors).animals.Count;
				if ((this.indoors as AnimalHouse).animalLimit == 16)
				{
					int arg_6C_0 = (this.indoors as AnimalHouse).animals.Count;
					int num = this.indoors.numberOfObjectsWithName("Hay");
					int num2 = Math.Min(arg_6C_0 - num, (Game1.getLocationFromName("Farm") as Farm).piecesOfHay);
					(Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= num2;
					int num3 = 0;
					while (num3 < 16 && num2 > 0)
					{
						Vector2 key = new Vector2((float)(8 + num3), 3f);
						if (!this.indoors.objects.ContainsKey(key))
						{
							this.indoors.objects.Add(key, new StardewValley.Object(178, 1, false, -1, 0));
						}
						num2--;
						num3++;
					}
				}
			}
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
			if (this.animalDoorMotion != 0)
			{
				if (this.animalDoorOpen && this.yPositionOfAnimalDoor <= Barn.openAnimalDoorPosition)
				{
					this.animalDoorMotion = 0;
					this.yPositionOfAnimalDoor = Barn.openAnimalDoorPosition;
				}
				else if (!this.animalDoorOpen && this.yPositionOfAnimalDoor >= 0)
				{
					this.animalDoorMotion = 0;
					this.yPositionOfAnimalDoor = 0;
				}
				this.yPositionOfAnimalDoor += this.animalDoorMotion;
			}
		}

		public override void upgrade()
		{
			if (this.buildingType.Equals("Big Barn"))
			{
				this.animalDoor.X = this.animalDoor.X + 1;
				this.indoors.moveObject(15, 3, 18, 13);
				this.indoors.moveObject(16, 3, 19, 13);
				this.indoors.moveObject(1, 4, 20, 3);
				for (int i = 4; i < 13; i++)
				{
					this.indoors.moveObject(16, i, 20, i);
				}
				return;
			}
			this.indoors.moveObject(20, 3, 1, 4);
			for (int j = 6; j < 12; j++)
			{
				this.indoors.moveObject(20, j, 23, j);
			}
			this.indoors.moveObject(20, 4, 20, 13);
			this.indoors.moveObject(20, 5, 21, 13);
			this.indoors.moveObject(20, 12, 22, 13);
		}

		public override Vector2 getUpgradeSignLocation()
		{
			return new Vector2((float)this.tileX, (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 3), (float)Game1.pixelZoom);
		}

		public override void drawInMenu(SpriteBatch b, int x, int y)
		{
			this.drawShadow(b, x, y);
			b.Draw(this.texture, new Vector2((float)x, (float)y) + new Vector2((float)this.animalDoor.X, (float)(this.animalDoor.Y + 3)) * (float)Game1.tileSize, new Rectangle?(new Rectangle(64, 112, 32, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.888f);
			b.Draw(this.texture, new Vector2((float)x, (float)y) + new Vector2((float)this.animalDoor.X, (float)this.animalDoor.Y + 2.25f) * (float)Game1.tileSize, new Rectangle?(new Rectangle(0, 112, 32, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f - 1E-07f);
			b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(0, 0, 112, 112)), this.color, 0f, new Vector2(0f, 0f), 4f, SpriteEffects.None, 0.89f);
		}

		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX + this.animalDoor.X), (float)(this.tileY + this.animalDoor.Y - 1)) * (float)Game1.tileSize), new Rectangle?(new Rectangle(32, 112, 32, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX + this.animalDoor.X), (float)(this.tileY + this.animalDoor.Y)) * (float)Game1.tileSize), new Rectangle?(new Rectangle(64, 112, 32, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((this.tileX + this.animalDoor.X) * Game1.tileSize), (float)((this.tileY + this.animalDoor.Y) * Game1.tileSize + this.yPositionOfAnimalDoor - Game1.tileSize * 3 / 4))), new Rectangle?(new Rectangle(0, 112, 32, 12)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 0.0001f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((this.tileX + this.animalDoor.X) * Game1.tileSize), (float)((this.tileY + this.animalDoor.Y) * Game1.tileSize + this.yPositionOfAnimalDoor))), new Rectangle?(new Rectangle(0, 112, 32, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 0.0001f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(new Rectangle(0, 0, 112, 112)), this.color * this.alpha, 0f, new Vector2(0f, 112f), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 1E-05f);
			if (this.daysUntilUpgrade > 0)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.getUpgradeSignLocation()), new Rectangle?(new Rectangle(367, 309, 16, 15)), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
			}
		}
	}
}
