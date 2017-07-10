using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
	public class Furniture : StardewValley.Object
	{
		public const int chair = 0;

		public const int bench = 1;

		public const int couch = 2;

		public const int armchair = 3;

		public const int dresser = 4;

		public const int longTable = 5;

		public const int painting = 6;

		public const int lamp = 7;

		public const int decor = 8;

		public const int other = 9;

		public const int bookcase = 10;

		public const int table = 11;

		public const int rug = 12;

		public const int window = 13;

		public new int price;

		public int furniture_type;

		public int rotations;

		public int currentRotation;

		public new bool flipped;

		private int sourceIndexOffset;

		protected Vector2 drawPosition;

		public Rectangle sourceRect;

		public Rectangle defaultSourceRect;

		public Rectangle defaultBoundingBox;

		public static Texture2D furnitureTexture;

		public bool drawHeldObjectLow;

		[XmlIgnore]
		public bool flaggedForPickUp;

		[XmlIgnore]
		private string _description;

		private bool lightGlowAdded;

		[XmlIgnore]
		public string description
		{
			get
			{
				if (this._description == null)
				{
					this._description = this.loadDescription();
				}
				return this._description;
			}
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public Furniture()
		{
			this.updateDrawPosition();
		}

		public Furniture(int which, Vector2 tile, int initialRotations) : this(which, tile)
		{
			for (int i = 0; i < initialRotations; i++)
			{
				this.rotate();
			}
		}

		public Furniture(int which, Vector2 tile)
		{
			this.tileLocation = tile;
			this.parentSheetIndex = which;
			string[] data = this.getData();
			this.name = data[0];
			this.furniture_type = this.getTypeNumberFromName(data[1]);
			this.defaultSourceRect = new Rectangle(which * 16 % Furniture.furnitureTexture.Width, which * 16 / Furniture.furnitureTexture.Width * 16, 1, 1);
			this.drawHeldObjectLow = this.name.ToLower().Contains("tea");
			if (data[2].Equals("-1"))
			{
				this.sourceRect = this.getDefaultSourceRectForType(which, this.furniture_type);
				this.defaultSourceRect = this.sourceRect;
			}
			else
			{
				this.defaultSourceRect.Width = Convert.ToInt32(data[2].Split(new char[]
				{
					' '
				})[0]);
				this.defaultSourceRect.Height = Convert.ToInt32(data[2].Split(new char[]
				{
					' '
				})[1]);
				this.sourceRect = new Rectangle(which * 16 % Furniture.furnitureTexture.Width, which * 16 / Furniture.furnitureTexture.Width * 16, this.defaultSourceRect.Width * 16, this.defaultSourceRect.Height * 16);
				this.defaultSourceRect = this.sourceRect;
			}
			this.defaultBoundingBox = new Rectangle((int)this.tileLocation.X, (int)this.tileLocation.Y, 1, 1);
			if (data[3].Equals("-1"))
			{
				this.boundingBox = this.getDefaultBoundingBoxForType(this.furniture_type);
				this.defaultBoundingBox = this.boundingBox;
			}
			else
			{
				this.defaultBoundingBox.Width = Convert.ToInt32(data[3].Split(new char[]
				{
					' '
				})[0]);
				this.defaultBoundingBox.Height = Convert.ToInt32(data[3].Split(new char[]
				{
					' '
				})[1]);
				this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, this.defaultBoundingBox.Width * Game1.tileSize, this.defaultBoundingBox.Height * Game1.tileSize);
				this.defaultBoundingBox = this.boundingBox;
			}
			this.updateDrawPosition();
			this.rotations = Convert.ToInt32(data[4]);
			this.price = Convert.ToInt32(data[5]);
		}

		private string[] getData()
		{
			Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Furniture");
			if (!dictionary.ContainsKey(this.parentSheetIndex))
			{
				dictionary = Game1.content.LoadBase<Dictionary<int, string>>("Data\\Furniture");
			}
			return dictionary[this.parentSheetIndex].Split(new char[]
			{
				'/'
			});
		}

		protected override string loadDisplayName()
		{
			if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
			{
				string[] expr_0D = this.getData();
				return expr_0D[expr_0D.Length - 1];
			}
			return this.name;
		}

		protected string loadDescription()
		{
			if (this.parentSheetIndex == 1308)
			{
				return Game1.parseText(Game1.content.LoadString("Strings\\Objects:CatalogueDescription", new object[0]), Game1.smallFont, Game1.tileSize * 5);
			}
			if (this.parentSheetIndex == 1226)
			{
				return Game1.parseText(Game1.content.LoadString("Strings\\Objects:FurnitureCatalogueDescription", new object[0]), Game1.smallFont, Game1.tileSize * 5);
			}
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12623", new object[0]);
		}

		public override string getDescription()
		{
			return this.description;
		}

		public override bool performDropDownAction(Farmer who)
		{
			this.resetOnPlayerEntry((who == null) ? Game1.currentLocation : who.currentLocation);
			return false;
		}

		public override void hoverAction()
		{
			base.hoverAction();
			if (!Game1.player.isInventoryFull())
			{
				Game1.mouseCursor = 2;
			}
		}

		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			if (this.parentSheetIndex == 1402)
			{
				Game1.activeClickableMenu = new Billboard(false);
			}
			else if (this.parentSheetIndex == 1308)
			{
				Game1.activeClickableMenu = new ShopMenu(Utility.getAllWallpapersAndFloorsForFree(), 0, null);
			}
			else if (this.parentSheetIndex == 1226)
			{
				Game1.activeClickableMenu = new ShopMenu(Utility.getAllFurnituresForFree(), 0, null);
			}
			return this.clicked(who);
		}

		public override bool clicked(Farmer who)
		{
			Game1.haltAfterCheck = false;
			if (this.furniture_type == 11 && who.ActiveObject != null && who.ActiveObject != null && this.heldObject == null)
			{
				return false;
			}
			if (this.heldObject == null && (who.ActiveObject == null || !(who.ActiveObject is Furniture)))
			{
				this.flaggedForPickUp = true;
				return true;
			}
			if (this.heldObject != null && who.addItemToInventoryBool(this.heldObject, false))
			{
				this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
				this.heldObject = null;
				Game1.playSound("coin");
				return true;
			}
			return false;
		}

		public override void DayUpdate(GameLocation location)
		{
			base.DayUpdate(location);
			this.lightGlowAdded = false;
			if (!Game1.isDarkOut() || (Game1.newDay && !Game1.isRaining))
			{
				this.removeLights(location);
				return;
			}
			this.addLights(location);
		}

		public void resetOnPlayerEntry(GameLocation environment)
		{
			this.removeLights(environment);
			if (Game1.isDarkOut())
			{
				this.addLights(environment);
			}
		}

		public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
		{
			if ((this.furniture_type == 11 || this.furniture_type == 5) && this.heldObject == null && !dropIn.bigCraftable && (!(dropIn is Furniture) || ((dropIn as Furniture).getTilesWide() == 1 && (dropIn as Furniture).getTilesHigh() == 1)))
			{
				this.heldObject = (StardewValley.Object)dropIn.getOne();
				this.heldObject.tileLocation = this.tileLocation;
				this.heldObject.boundingBox.X = this.boundingBox.X;
				this.heldObject.boundingBox.Y = this.boundingBox.Y;
				this.heldObject.performDropDownAction(who);
				if (!probe)
				{
					Game1.playSound("woodyStep");
					if (who != null)
					{
						who.reduceActiveItemByOne();
					}
				}
				return true;
			}
			return false;
		}

		private void addLights(GameLocation environment)
		{
			if (this.furniture_type == 7)
			{
				if (this.sourceIndexOffset == 0)
				{
					this.sourceRect = this.defaultSourceRect;
					this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
				}
				this.sourceIndexOffset = 1;
				if (this.lightSource == null)
				{
					Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
					this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, Color.Black, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
					Game1.currentLightSources.Add(this.lightSource);
					return;
				}
			}
			else if (this.furniture_type == 13)
			{
				if (this.sourceIndexOffset == 0)
				{
					this.sourceRect = this.defaultSourceRect;
					this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
				}
				this.sourceIndexOffset = 1;
				if (this.lightGlowAdded)
				{
					environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
					this.lightGlowAdded = false;
				}
			}
		}

		private void removeLights(GameLocation environment)
		{
			if (this.furniture_type == 7)
			{
				if (this.sourceIndexOffset == 1)
				{
					this.sourceRect = this.defaultSourceRect;
				}
				this.sourceIndexOffset = 0;
				Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
				this.lightSource = null;
				return;
			}
			if (this.furniture_type == 13)
			{
				if (this.sourceIndexOffset == 1)
				{
					this.sourceRect = this.defaultSourceRect;
				}
				this.sourceIndexOffset = 0;
				if (Game1.isRaining)
				{
					this.sourceRect = this.defaultSourceRect;
					this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
					this.sourceIndexOffset = 1;
					return;
				}
				if (!this.lightGlowAdded && !environment.lightGlows.Contains(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize))))
				{
					environment.lightGlows.Add(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
				}
				this.lightGlowAdded = true;
			}
		}

		public override bool minutesElapsed(int minutes, GameLocation environment)
		{
			if (Game1.isDarkOut())
			{
				this.addLights(environment);
			}
			else
			{
				this.removeLights(environment);
			}
			return false;
		}

		public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
		{
			this.removeLights(environment);
			if (this.furniture_type == 13 && this.lightGlowAdded)
			{
				environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
				this.lightGlowAdded = false;
			}
			base.performRemoveAction(tileLocation, environment);
		}

		public void rotate()
		{
			if (this.rotations < 2)
			{
				return;
			}
			int arg_10_0 = this.currentRotation;
			int num = (this.rotations == 4) ? 1 : 2;
			this.currentRotation += num;
			this.currentRotation %= 4;
			this.flipped = false;
			Point point = default(Point);
			int num2 = this.furniture_type;
			switch (num2)
			{
			case 2:
				point.Y = 1;
				point.X = -1;
				break;
			case 3:
				point.X = -1;
				point.Y = 1;
				break;
			case 4:
				break;
			case 5:
				point.Y = 0;
				point.X = -1;
				break;
			default:
				if (num2 == 12)
				{
					point.X = 0;
					point.Y = 0;
				}
				break;
			}
			bool flag = this.furniture_type == 5 || this.furniture_type == 12 || this.parentSheetIndex == 724 || this.parentSheetIndex == 727;
			bool flag2 = this.defaultBoundingBox.Width != this.defaultBoundingBox.Height;
			if (flag && this.currentRotation == 2)
			{
				this.currentRotation = 1;
			}
			if (flag2)
			{
				int height = this.boundingBox.Height;
				switch (this.currentRotation)
				{
				case 0:
				case 2:
					this.boundingBox.Height = this.defaultBoundingBox.Height;
					this.boundingBox.Width = this.defaultBoundingBox.Width;
					break;
				case 1:
				case 3:
					this.boundingBox.Height = this.boundingBox.Width + point.X * Game1.tileSize;
					this.boundingBox.Width = height + point.Y * Game1.tileSize;
					break;
				}
			}
			Point point2 = default(Point);
			num2 = this.furniture_type;
			if (num2 == 12)
			{
				point2.X = 1;
				point2.Y = -1;
			}
			if (flag2)
			{
				switch (this.currentRotation)
				{
				case 0:
					this.sourceRect = this.defaultSourceRect;
					break;
				case 1:
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point.X * 16 + point2.Y * 16);
					break;
				case 2:
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width + this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
					break;
				case 3:
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point.X * 16 + point2.Y * 16);
					this.flipped = true;
					break;
				}
			}
			else
			{
				this.flipped = (this.currentRotation == 3);
				if (this.rotations == 2)
				{
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 2) ? 1 : 0) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
				}
				else
				{
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 3) ? 1 : this.currentRotation) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
				}
			}
			if (flag && this.currentRotation == 1)
			{
				this.currentRotation = 2;
			}
			this.updateDrawPosition();
		}

		public bool isGroundFurniture()
		{
			return this.furniture_type != 13 && this.furniture_type != 6 && this.furniture_type != 13;
		}

		public override bool canBeGivenAsGift()
		{
			return false;
		}

		public override bool canBePlacedHere(GameLocation l, Vector2 tile)
		{
			if (!(l is DecoratableLocation))
			{
				return false;
			}
			for (int i = 0; i < this.boundingBox.Width / Game1.tileSize; i++)
			{
				for (int j = 0; j < this.boundingBox.Height / Game1.tileSize; j++)
				{
					Vector2 vector = tile * (float)Game1.tileSize + new Vector2((float)i, (float)j) * (float)Game1.tileSize;
					vector.X += (float)(Game1.tileSize / 2);
					vector.Y += (float)(Game1.tileSize / 2);
					foreach (Furniture current in (l as DecoratableLocation).furniture)
					{
						if (current.furniture_type == 11 && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y) && current.heldObject == null && this.getTilesWide() == 1)
						{
							bool result = true;
							return result;
						}
						if ((current.furniture_type != 12 || this.furniture_type == 12) && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y))
						{
							bool result = false;
							return result;
						}
					}
					Vector2 key = tile + new Vector2((float)i, (float)j);
					if (l.Objects.ContainsKey(key))
					{
						return false;
					}
				}
			}
			return base.canBePlacedHere(l, tile);
		}

		public void updateDrawPosition()
		{
			this.drawPosition = new Vector2((float)this.boundingBox.X, (float)(this.boundingBox.Y - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)));
		}

		public int getTilesWide()
		{
			return this.boundingBox.Width / Game1.tileSize;
		}

		public int getTilesHigh()
		{
			return this.boundingBox.Height / Game1.tileSize;
		}

		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			if (location is DecoratableLocation)
			{
				Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
				List<Rectangle> walls;
				if (location is FarmHouse)
				{
					walls = FarmHouse.getWalls((location as FarmHouse).upgradeLevel);
				}
				else
				{
					walls = DecoratableLocation.getWalls();
				}
				this.tileLocation = new Vector2((float)point.X, (float)point.Y);
				bool flag = false;
				if (this.furniture_type == 6 || this.furniture_type == 13 || this.parentSheetIndex == 1293)
				{
					int num = (this.parentSheetIndex == 1293) ? 3 : 0;
					bool flag2 = false;
					foreach (Rectangle current in walls)
					{
						if ((this.furniture_type == 6 || this.furniture_type == 13 || num != 0) && current.Y + num == point.Y && current.Contains(point.X, point.Y - num))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12629", new object[0]));
						return false;
					}
					flag = true;
				}
				for (int i = point.X; i < point.X + this.getTilesWide(); i++)
				{
					for (int j = point.Y; j < point.Y + this.getTilesHigh(); j++)
					{
						if (location.doesTileHaveProperty(i, j, "NoFurniture", "Back") != null)
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12632", new object[0]));
							return false;
						}
						if (!flag && Utility.pointInRectangles(walls, i, j))
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12633", new object[0]));
							return false;
						}
						if (location.getTileIndexAt(i, j, "Buildings") != -1)
						{
							return false;
						}
					}
				}
				this.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
				foreach (Furniture current2 in (location as DecoratableLocation).furniture)
				{
					if (current2.furniture_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
					{
						current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
						bool result = true;
						return result;
					}
				}
				using (List<Farmer>.Enumerator enumerator3 = location.getFarmers().GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.GetBoundingBox().Intersects(this.boundingBox))
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12636", new object[0]));
							bool result = false;
							return result;
						}
					}
				}
				this.updateDrawPosition();
				return base.placementAction(location, x, y, who);
			}
			Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12628", new object[0]));
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

		private Rectangle getDefaultSourceRectForType(int tileIndex, int type)
		{
			int num;
			int num2;
			switch (type)
			{
			case 0:
				num = 1;
				num2 = 2;
				goto IL_92;
			case 1:
				num = 2;
				num2 = 2;
				goto IL_92;
			case 2:
				num = 3;
				num2 = 2;
				goto IL_92;
			case 3:
				num = 2;
				num2 = 2;
				goto IL_92;
			case 4:
				num = 2;
				num2 = 2;
				goto IL_92;
			case 5:
				num = 5;
				num2 = 3;
				goto IL_92;
			case 6:
				num = 2;
				num2 = 2;
				goto IL_92;
			case 7:
				num = 1;
				num2 = 3;
				goto IL_92;
			case 8:
				num = 1;
				num2 = 2;
				goto IL_92;
			case 10:
				num = 2;
				num2 = 3;
				goto IL_92;
			case 11:
				num = 2;
				num2 = 3;
				goto IL_92;
			case 12:
				num = 3;
				num2 = 2;
				goto IL_92;
			case 13:
				num = 1;
				num2 = 2;
				goto IL_92;
			}
			num = 1;
			num2 = 2;
			IL_92:
			return new Rectangle(tileIndex * 16 % Furniture.furnitureTexture.Width, tileIndex * 16 / Furniture.furnitureTexture.Width * 16, num * 16, num2 * 16);
		}

		private Rectangle getDefaultBoundingBoxForType(int type)
		{
			int num;
			int num2;
			switch (type)
			{
			case 0:
				num = 1;
				num2 = 1;
				goto IL_92;
			case 1:
				num = 2;
				num2 = 1;
				goto IL_92;
			case 2:
				num = 3;
				num2 = 1;
				goto IL_92;
			case 3:
				num = 2;
				num2 = 1;
				goto IL_92;
			case 4:
				num = 2;
				num2 = 1;
				goto IL_92;
			case 5:
				num = 5;
				num2 = 2;
				goto IL_92;
			case 6:
				num = 2;
				num2 = 2;
				goto IL_92;
			case 7:
				num = 1;
				num2 = 1;
				goto IL_92;
			case 8:
				num = 1;
				num2 = 1;
				goto IL_92;
			case 10:
				num = 2;
				num2 = 1;
				goto IL_92;
			case 11:
				num = 2;
				num2 = 2;
				goto IL_92;
			case 12:
				num = 3;
				num2 = 2;
				goto IL_92;
			case 13:
				num = 1;
				num2 = 2;
				goto IL_92;
			}
			num = 1;
			num2 = 1;
			IL_92:
			return new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, num * Game1.tileSize, num2 * Game1.tileSize);
		}

		private int getTypeNumberFromName(string typeName)
		{
			string text = typeName.ToLower();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1555340682u)
			{
				if (num <= 732630053u)
				{
					if (num != 44871939u)
					{
						if (num != 600654789u)
						{
							if (num == 732630053u)
							{
								if (text == "couch")
								{
									return 2;
								}
							}
						}
						else if (text == "rug")
						{
							return 12;
						}
					}
					else if (text == "long table")
					{
						return 5;
					}
				}
				else if (num != 1049849701u)
				{
					if (num != 1251777503u)
					{
						if (num == 1555340682u)
						{
							if (text == "chair")
							{
								return 0;
							}
						}
					}
					else if (text == "table")
					{
						return 11;
					}
				}
				else if (text == "painting")
				{
					return 6;
				}
			}
			else if (num <= 2058371002u)
			{
				if (num != 1651424953u)
				{
					if (num != 1810951995u)
					{
						if (num == 2058371002u)
						{
							if (text == "armchair")
							{
								return 3;
							}
						}
					}
					else if (text == "lamp")
					{
						return 7;
					}
				}
				else if (text == "bench")
				{
					return 1;
				}
			}
			else if (num <= 2708649949u)
			{
				if (num != 2236496455u)
				{
					if (num == 2708649949u)
					{
						if (text == "window")
						{
							return 13;
						}
					}
				}
				else if (text == "dresser")
				{
					return 4;
				}
			}
			else if (num != 3104904292u)
			{
				if (num == 3358447858u)
				{
					if (text == "decor")
					{
						return 8;
					}
				}
			}
			else if (text == "bookcase")
			{
				return 10;
			}
			return 9;
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

		private float getScaleSize()
		{
			int num = this.sourceRect.Width / 16;
			int num2 = this.sourceRect.Height / 16;
			if (num >= 5)
			{
				return 0.75f;
			}
			if (num2 >= 3)
			{
				return 1f;
			}
			if (num <= 2)
			{
				return 2f;
			}
			if (num <= 4)
			{
				return 1f;
			}
			return 0.1f;
		}

		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(Furniture.furnitureTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.defaultSourceRect), Color.White * transparency, 0f, new Vector2((float)(this.defaultSourceRect.Width / 2), (float)(this.defaultSourceRect.Height / 2)), 1f * this.getScaleSize() * scaleSize, SpriteEffects.None, layerDepth);
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			if (x == -1)
			{
				spriteBatch.Draw(Furniture.furnitureTexture, Game1.GlobalToLocal(Game1.viewport, this.drawPosition), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.furniture_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
			}
			else
			{
				spriteBatch.Draw(Furniture.furnitureTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)))), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.furniture_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
			}
			if (this.heldObject != null)
			{
				if (this.heldObject is Furniture)
				{
					(this.heldObject as Furniture).drawAtNonTileSpot(spriteBatch, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.heldObject as Furniture).sourceRect.Height * Game1.pixelZoom - (this.drawHeldObjectLow ? (-Game1.tileSize / 4) : (Game1.tileSize / 4))))), (float)(this.boundingBox.Bottom - 7) / 10000f, alpha);
					return;
				}
				spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.drawHeldObjectLow ? (Game1.tileSize / 2) : (Game1.tileSize * 4 / 3))))) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.boundingBox.Bottom / 10000f);
				spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.drawHeldObjectLow ? (Game1.tileSize / 2) : (Game1.tileSize * 4 / 3))))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.boundingBox.Bottom + 1) / 10000f);
			}
		}

		public void drawAtNonTileSpot(SpriteBatch spriteBatch, Vector2 location, float layerDepth, float alpha = 1f)
		{
			spriteBatch.Draw(Furniture.furnitureTexture, location, new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
		}

		public override Item getOne()
		{
			Furniture expr_11 = new Furniture(this.parentSheetIndex, this.tileLocation);
			expr_11.drawPosition = this.drawPosition;
			expr_11.defaultBoundingBox = this.defaultBoundingBox;
			expr_11.boundingBox = this.boundingBox;
			expr_11.currentRotation = this.currentRotation - 1;
			expr_11.rotations = this.rotations;
			expr_11.rotate();
			return expr_11;
		}
	}
}
