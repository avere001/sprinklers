using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
	public class Hat : Item
	{
		public const int widthOfTileSheetSquare = 20;

		public const int heightOfTileSheetSquare = 20;

		public int which;

		[XmlIgnore]
		public string displayName;

		[XmlIgnore]
		public string description;

		public string name;

		public bool skipHairDraw;

		public bool ignoreHairstyleOffset;

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

		public Hat()
		{
			this.load(this.which);
		}

		public void load(int which)
		{
			Dictionary<int, string> expr_0F = Game1.content.Load<Dictionary<int, string>>("Data\\hats");
			if (!expr_0F.ContainsKey(which))
			{
				which = 0;
			}
			string[] array = expr_0F[which].Split(new char[]
			{
				'/'
			});
			this.name = array[0];
			this.skipHairDraw = Convert.ToBoolean(array[2]);
			this.ignoreHairstyleOffset = Convert.ToBoolean(array[3]);
			this.category = -95;
		}

		public Hat(int which)
		{
			this.which = which;
			this.load(which);
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(FarmerRenderer.hatsTexture, location + new Vector2(10f, 10f), new Rectangle?(new Rectangle(this.which * 20 % FarmerRenderer.hatsTexture.Width, this.which * 20 / FarmerRenderer.hatsTexture.Width * 20 * 4, 20, 20)), Color.White * transparency, 0f, new Vector2(3f, 3f), 3f * scaleSize, SpriteEffects.None, layerDepth);
		}

		public override string getDescription()
		{
			if (this.description == null)
			{
				this.loadDisplayFields();
			}
			return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
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
			return 1;
		}

		public override bool isPlaceable()
		{
			return false;
		}

		public override Item getOne()
		{
			return new Hat(this.which);
		}

		private bool loadDisplayFields()
		{
			if (this.name != null)
			{
				foreach (KeyValuePair<int, string> current in Game1.content.Load<Dictionary<int, string>>("Data\\hats"))
				{
					string[] array = current.Value.Split(new char[]
					{
						'/'
					});
					if (array[0] == this.name)
					{
						this.displayName = this.name;
						if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
						{
							this.displayName = array[array.Length - 1];
						}
						this.description = array[1];
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}
}
