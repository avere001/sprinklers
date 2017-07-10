using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Monsters;
using System;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
	public class Ring : Item
	{
		public const int ringLowerIndexRange = 516;

		public const int slimeCharmer = 520;

		public const int yobaRing = 524;

		public const int sturdyRing = 525;

		public const int burglarsRing = 526;

		public const int jukeboxRing = 528;

		public const int ringUpperIndexRange = 534;

		[XmlIgnore]
		public string description;

		[XmlIgnore]
		public string displayName;

		public string name;

		public int price;

		public int indexInTileSheet;

		public int uniqueID;

		public override int parentSheetIndex
		{
			get
			{
				return this.indexInTileSheet;
			}
		}

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

		public Ring()
		{
		}

		public Ring(int which)
		{
			string[] array = Game1.objectInformation[which].Split(new char[]
			{
				'/'
			});
			this.category = -96;
			this.name = array[0];
			this.price = Convert.ToInt32(array[1]);
			this.indexInTileSheet = which;
			this.uniqueID = Game1.year + Game1.dayOfMonth + Game1.timeOfDay + this.indexInTileSheet + Game1.player.getTileX() + (int)Game1.stats.MonstersKilled + (int)Game1.stats.itemsCrafted;
			this.loadDisplayFields();
		}

		public void onEquip(Farmer who)
		{
			switch (this.indexInTileSheet)
			{
			case 516:
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 5f, new Color(0, 50, 170), this.uniqueID));
				return;
			case 517:
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 10f, new Color(0, 30, 150), this.uniqueID));
				return;
			case 518:
				who.magneticRadius += 64;
				return;
			case 519:
				who.magneticRadius += 128;
				return;
			case 520:
			case 521:
			case 522:
			case 523:
			case 524:
			case 525:
			case 526:
			case 528:
				break;
			case 527:
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 10f, new Color(0, 80, 0), this.uniqueID));
				who.magneticRadius += 128;
				who.attackIncreaseModifier += 0.1f;
				return;
			case 529:
				who.knockbackModifier += 0.1f;
				return;
			case 530:
				who.weaponPrecisionModifier += 0.1f;
				return;
			case 531:
				who.critChanceModifier += 0.1f;
				return;
			case 532:
				who.critPowerModifier += 0.1f;
				return;
			case 533:
				who.weaponSpeedModifier += 0.1f;
				return;
			case 534:
				who.attackIncreaseModifier += 0.1f;
				break;
			default:
				return;
			}
		}

		public void onUnequip(Farmer who)
		{
			switch (this.indexInTileSheet)
			{
			case 516:
			case 517:
				Utility.removeLightSource(this.uniqueID);
				return;
			case 518:
				who.magneticRadius -= 64;
				return;
			case 519:
				who.magneticRadius -= 128;
				return;
			case 520:
			case 521:
			case 522:
			case 523:
			case 524:
			case 525:
			case 526:
			case 528:
				break;
			case 527:
				who.magneticRadius -= 128;
				Utility.removeLightSource(this.uniqueID);
				who.attackIncreaseModifier -= 0.1f;
				return;
			case 529:
				who.knockbackModifier -= 0.1f;
				return;
			case 530:
				who.weaponPrecisionModifier -= 0.1f;
				return;
			case 531:
				who.critChanceModifier -= 0.1f;
				return;
			case 532:
				who.critPowerModifier -= 0.1f;
				return;
			case 533:
				who.weaponSpeedModifier -= 0.1f;
				return;
			case 534:
				who.attackIncreaseModifier -= 0.1f;
				break;
			default:
				return;
			}
		}

		public override string getCategoryName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Ring.cs.1", new object[0]);
		}

		public void onNewLocation(Farmer who, GameLocation environment)
		{
			int num = this.indexInTileSheet;
			if (num == 516 || num == 517)
			{
				this.onEquip(who);
				return;
			}
			if (num != 527)
			{
				return;
			}
			Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 10f, new Color(0, 30, 150), this.uniqueID));
		}

		public void onLeaveLocation(Farmer who, GameLocation environment)
		{
			int num = this.indexInTileSheet;
			if (num == 516 || num == 517)
			{
				this.onUnequip(who);
				return;
			}
			if (num != 527)
			{
				return;
			}
			Utility.removeLightSource(this.uniqueID);
		}

		public override int salePrice()
		{
			return this.price;
		}

		public void onMonsterSlay(Monster m)
		{
			switch (this.indexInTileSheet)
			{
			case 521:
				if (Game1.random.NextDouble() < 0.1 + (double)((float)Game1.player.LuckLevel / 100f))
				{
					Game1.buffsDisplay.addOtherBuff(new Buff(20));
					Game1.playSound("warrior");
					return;
				}
				break;
			case 522:
				Game1.player.health = Math.Min(Game1.player.maxHealth, Game1.player.health + 2);
				return;
			case 523:
				Game1.buffsDisplay.addOtherBuff(new Buff(22));
				break;
			default:
				return;
			}
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.indexInTileSheet, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f) * scaleSize, scaleSize * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
		}

		public void update(GameTime time, GameLocation environment, Farmer who)
		{
			int num = this.indexInTileSheet;
			if (num <= 517)
			{
				if (num != 516 && num != 517)
				{
					return;
				}
			}
			else if (num != 527)
			{
				return;
			}
			Utility.repositionLightSource(this.uniqueID, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y));
			if (!environment.isOutdoors && !(environment is MineShaft))
			{
				LightSource lightSource = Utility.getLightSource(this.uniqueID);
				if (lightSource != null)
				{
					lightSource.radius = 3f;
				}
			}
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

		public override string getDescription()
		{
			if (this.description == null)
			{
				this.loadDisplayFields();
			}
			return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
		}

		public override bool isPlaceable()
		{
			return false;
		}

		public override Item getOne()
		{
			return new Ring(this.indexInTileSheet);
		}

		private bool loadDisplayFields()
		{
			if (Game1.objectInformation != null)
			{
				int arg_0D_0 = this.indexInTileSheet;
				string[] array = Game1.objectInformation[this.indexInTileSheet].Split(new char[]
				{
					'/'
				});
				this.displayName = array[4];
				this.description = array[5];
				return true;
			}
			return false;
		}
	}
}
