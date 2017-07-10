using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
	public class Boots : Item
	{
		public int defenseBonus;

		public int immunityBonus;

		public int indexInTileSheet;

		public int price;

		public int indexInColorSheet;

		[XmlIgnore]
		public string displayName;

		[XmlIgnore]
		public string description;

		public string name;

		[XmlIgnore]
		public override string DisplayName
		{
			get
			{
				if (this.displayName == null)
				{
					this.loadDisplayFields();
				}
				return this.displayName;
			}
			set
			{
				this.displayName = value;
			}
		}

		[XmlIgnore]
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		[XmlIgnore]
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

		public Boots()
		{
			this.category = -97;
		}

		public Boots(int which)
		{
			string[] array = Game1.content.Load<Dictionary<int, string>>("Data\\Boots")[which].Split(new char[]
			{
				'/'
			});
			this.name = array[0];
			this.price = Convert.ToInt32(array[2]);
			this.defenseBonus = Convert.ToInt32(array[3]);
			this.immunityBonus = Convert.ToInt32(array[4]);
			this.indexInColorSheet = Convert.ToInt32(array[5]);
			this.indexInTileSheet = which;
			this.category = -97;
		}

		public override int salePrice()
		{
			return this.defenseBonus * 100 + this.immunityBonus * 100;
		}

		public void onEquip()
		{
			Game1.player.resilience += this.defenseBonus;
			Game1.player.immunity += this.immunityBonus;
			Game1.player.changeShoeColor(this.indexInColorSheet);
		}

		public void onUnequip()
		{
			Game1.player.resilience -= this.defenseBonus;
			Game1.player.immunity -= this.immunityBonus;
			Game1.player.changeShoeColor(12);
		}

		public int getNumberOfDescriptionCategories()
		{
			if (this.immunityBonus > 0 && this.defenseBonus > 0)
			{
				return 2;
			}
			return 1;
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.indexInTileSheet, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f) * scaleSize, scaleSize * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
		}

		public override int maximumStackSize()
		{
			return -1;
		}

		public override int getStack()
		{
			return 1;
		}

		public override int addToStack(int amount)
		{
			return 1;
		}

		public override string getCategoryName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Boots.cs.12501", new object[0]);
		}

		public override string getDescription()
		{
			if (this.description == null)
			{
				this.loadDisplayFields();
			}
			return Game1.parseText(this.description + Environment.NewLine + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Boots.cs.12500", new object[]
			{
				this.immunityBonus + this.defenseBonus
			}), Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
		}

		public override bool isPlaceable()
		{
			return false;
		}

		public override Item getOne()
		{
			return new Boots(this.indexInTileSheet);
		}

		private bool loadDisplayFields()
		{
			int arg_06_0 = this.indexInTileSheet;
			string[] array = Game1.content.Load<Dictionary<int, string>>("Data\\Boots")[this.indexInTileSheet].Split(new char[]
			{
				'/'
			});
			this.displayName = this.Name;
			if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
			{
				this.displayName = array[array.Length - 1];
			}
			this.description = array[1];
			return true;
		}
	}
}
