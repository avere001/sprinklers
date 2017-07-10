using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
	public class SpecialItem : Item
	{
		public const int permanentChangeItem = 1;

		public const int skullKey = 4;

		public const int clubCard = 2;

		public const int backpack = 99;

		public const int darkTalisman = 6;

		public const int magicInk = 7;

		public string name;

		[XmlIgnore]
		private string _displayName;

		public int which;

		public new int category;

		[XmlIgnore]
		private string displayName
		{
			get
			{
				if (string.IsNullOrEmpty(this._displayName))
				{
					int num = this.which;
					switch (num)
					{
					case 2:
						this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13089", new object[0]);
						break;
					case 3:
					case 5:
						break;
					case 4:
						this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13088", new object[0]);
						break;
					case 6:
						this._displayName = Game1.content.LoadString("Strings\\Objects:DarkTalisman", new object[0]);
						break;
					case 7:
						this._displayName = Game1.content.LoadString("Strings\\Objects:MagicInk", new object[0]);
						break;
					default:
						if (num == 99)
						{
							if (Game1.player.maxItems == 36)
							{
								this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8709", new object[0]);
							}
							else
							{
								this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8708", new object[0]);
							}
						}
						break;
					}
				}
				return this._displayName;
			}
			set
			{
				if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(this._displayName))
				{
					int num = this.which;
					switch (num)
					{
					case 2:
						this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13089", new object[0]);
						return;
					case 3:
					case 5:
						break;
					case 4:
						this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13088", new object[0]);
						return;
					case 6:
						this._displayName = Game1.content.LoadString("Strings\\Objects:DarkTalisman", new object[0]);
						return;
					case 7:
						this._displayName = Game1.content.LoadString("Strings\\Objects:MagicInk", new object[0]);
						return;
					default:
						if (num != 99)
						{
							return;
						}
						if (Game1.player.maxItems == 36)
						{
							this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8709", new object[0]);
							return;
						}
						this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8708", new object[0]);
						return;
					}
				}
				else
				{
					this._displayName = value;
				}
			}
		}

		[XmlIgnore]
		public override string DisplayName
		{
			get
			{
				return this.displayName;
			}
			set
			{
				this.displayName = value;
			}
		}

		public override string Name
		{
			get
			{
				if (this.name.Length < 1)
				{
					switch (this.which)
					{
					case 2:
						return "Club Card";
					case 4:
						return "Skull Key";
					case 6:
						return Game1.content.LoadString("Strings\\Objects:DarkTalisman", new object[0]);
					case 7:
						return Game1.content.LoadString("Strings\\Objects:MagicInk", new object[0]);
					}
				}
				return this.name;
			}
		}

		public override int Stack
		{
			get
			{
				return 1;
			}
			set
			{
			}
		}

		public SpecialItem(int category, int which, string name = "")
		{
			this.category = category;
			this.which = which;
			this.name = name;
			this.displayName = name;
			if (name.Length < 1)
			{
				switch (which)
				{
				case 2:
					this.displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13089", new object[0]);
					return;
				case 3:
				case 5:
					break;
				case 4:
					this.displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13088", new object[0]);
					return;
				case 6:
					this.displayName = Game1.content.LoadString("Strings\\Objects:DarkTalisman", new object[0]);
					return;
				case 7:
					this.displayName = Game1.content.LoadString("Strings\\Objects:MagicInk", new object[0]);
					break;
				default:
					return;
				}
			}
		}

		public SpecialItem()
		{
		}

		public void actionWhenReceived(Farmer who)
		{
			switch (this.which)
			{
			case 4:
				who.hasSkullKey = true;
				who.addQuest(19);
				return;
			case 5:
				break;
			case 6:
				who.hasDarkTalisman = true;
				return;
			case 7:
				who.hasMagicInk = true;
				break;
			default:
				return;
			}
		}

		public TemporaryAnimatedSprite getTemporarySpriteForHoldingUp(Vector2 position)
		{
			if (this.category == 1)
			{
				return new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(129 + 16 * this.which, 320, 16, 16), position, false, 0f, Color.White)
				{
					layerDepth = 1f
				};
			}
			if (this.which == 99)
			{
				return new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle((Game1.player.maxItems == 36) ? 268 : 257, 1436, (Game1.player.maxItems == 36) ? 11 : 9, 13), position + new Vector2((float)(Game1.tileSize / 4), 0f), false, 0f, Color.White)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				};
			}
			return null;
		}

		public override string checkForSpecialItemHoldUpMeessage()
		{
			if (this.category == 1)
			{
				switch (this.which)
				{
				case 2:
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13090", new object[]
					{
						this.displayName
					});
				case 4:
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13092", new object[]
					{
						this.displayName
					});
				case 6:
					return Game1.content.LoadString("Strings\\Objects:DarkTalismanDescription", new object[]
					{
						this.displayName
					});
				case 7:
					return Game1.content.LoadString("Strings\\Objects:MagicInkDescription", new object[]
					{
						this.displayName
					});
				}
			}
			if (this.which == 99)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13094", new object[]
				{
					this.displayName,
					Game1.player.maxItems
				});
			}
			return base.checkForSpecialItemHoldUpMeessage();
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
		}

		public override int maximumStackSize()
		{
			return 1;
		}

		public override int getStack()
		{
			return 1;
		}

		public override int addToStack(int amount)
		{
			return -1;
		}

		public override string getDescription()
		{
			return null;
		}

		public override bool isPlaceable()
		{
			return false;
		}

		public override Item getOne()
		{
			throw new NotImplementedException();
		}
	}
}
