using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace StardewValley.Buildings
{
	public class JunimoHut : Building
	{
		public const int cropHarvestRadius = 8;

		public Chest output;

		public bool noHarvest;

		public Rectangle sourceRect;

		private int junimoSendOutTimer;

		[XmlIgnore]
		public List<JunimoHarvester> myJunimos = new List<JunimoHarvester>();

		[XmlIgnore]
		public Point lastKnownCropLocation = Point.Zero;

		private bool wasLit;

		private Rectangle lightInteriorRect = new Rectangle(195, 0, 18, 17);

		private Rectangle bagRect = new Rectangle(208, 51, 15, 13);

		public JunimoHut(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
			this.sourceRect = this.getSourceRectForMenu();
			this.output = new Chest(true);
		}

		public JunimoHut()
		{
			this.sourceRect = this.getSourceRectForMenu();
		}

		public override Rectangle getRectForAnimalDoor()
		{
			return new Rectangle((1 + this.tileX) * Game1.tileSize, (this.tileY + 1) * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(Utility.getSeasonNumber(Game1.currentSeason) * 48, 0, 48, 64);
		}

		public override void load()
		{
			base.load();
			this.sourceRect = this.getSourceRectForMenu();
		}

		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			int arg_0F_0 = this.daysOfConstructionLeft;
			this.sourceRect = this.getSourceRectForMenu();
			this.myJunimos.Clear();
			this.wasLit = false;
		}

		public void sendOutJunimos()
		{
			this.junimoSendOutTimer = 1000;
		}

		public override void performActionOnConstruction(GameLocation location)
		{
			base.performActionOnConstruction(location);
			this.sendOutJunimos();
		}

		public override void performActionOnPlayerLocationEntry()
		{
			base.performActionOnPlayerLocationEntry();
			if (Game1.timeOfDay >= 2000 && Game1.timeOfDay < 2400 && !Game1.IsWinter)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 0.5f)
				{
					identifier = this.tileX + this.tileY * 777
				});
				AmbientLocationSounds.addSound(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)), 1);
				this.wasLit = true;
			}
		}

		public int getUnusedJunimoNumber()
		{
			for (int i = 0; i < 3; i++)
			{
				if (i >= this.myJunimos.Count<JunimoHarvester>())
				{
					return i;
				}
				bool flag = false;
				using (List<JunimoHarvester>.Enumerator enumerator = this.myJunimos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.whichJunimoFromThisHut == i)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return i;
				}
			}
			return 2;
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
			if (this.junimoSendOutTimer > 0)
			{
				this.junimoSendOutTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.junimoSendOutTimer <= 0 && this.myJunimos.Count<JunimoHarvester>() < 3 && !Game1.IsWinter && !Game1.isRaining && this.areThereMatureCropsWithinRadius() && Game1.farmEvent == null)
				{
					JunimoHarvester item = new JunimoHarvester(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize / 2)), this, this.getUnusedJunimoNumber());
					Game1.getFarm().characters.Add(item);
					this.myJunimos.Add(item);
					this.junimoSendOutTimer = 1000;
					if (Utility.isOnScreen(Utility.Vector2ToPoint(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1))), Game1.tileSize, Game1.getFarm()))
					{
						try
						{
							Game1.playSound("junimoMeep1");
						}
						catch (Exception)
						{
						}
					}
				}
			}
		}

		public bool areThereMatureCropsWithinRadius()
		{
			Farm farm = Game1.getFarm();
			for (int i = this.tileX + 1 - 8; i < this.tileX + 2 + 8; i++)
			{
				for (int j = this.tileY - 8 + 1; j < this.tileY + 2 + 8; j++)
				{
					if (farm.isCropAtTile(i, j) && (farm.terrainFeatures[new Vector2((float)i, (float)j)] as HoeDirt).readyForHarvest())
					{
						this.lastKnownCropLocation = new Point(i, j);
						return true;
					}
				}
			}
			this.lastKnownCropLocation = Point.Zero;
			return false;
		}

		public override void performTenMinuteAction(int timeElapsed)
		{
			base.performTenMinuteAction(timeElapsed);
			for (int i = this.myJunimos.Count - 1; i >= 0; i--)
			{
				if (!Game1.getFarm().characters.Contains(this.myJunimos[i]))
				{
					this.myJunimos.RemoveAt(i);
				}
				else
				{
					this.myJunimos[i].pokeToHarvest();
				}
			}
			if (this.myJunimos.Count<JunimoHarvester>() < 3 && Game1.timeOfDay < 1900)
			{
				this.junimoSendOutTimer = 1;
			}
			if (Game1.timeOfDay >= 2000 && Game1.timeOfDay < 2400 && !Game1.IsWinter && Utility.getLightSource(this.tileX + this.tileY * 777) == null && Game1.random.NextDouble() < 0.2)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 0.5f)
				{
					identifier = this.tileX + this.tileY * 777
				});
				AmbientLocationSounds.addSound(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)), 1);
				this.wasLit = true;
				return;
			}
			if (Game1.timeOfDay == 2400 && !Game1.IsWinter)
			{
				Utility.removeLightSource(this.tileX + this.tileY * 777);
				AmbientLocationSounds.removeSound(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)));
			}
		}

		public override bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (who.IsMainPlayer && tileLocation.X >= (float)this.tileX && tileLocation.X < (float)(this.tileX + this.tilesWide) && tileLocation.Y >= (float)this.tileY && tileLocation.Y < (float)(this.tileY + this.tilesHigh) && this.output != null)
			{
				Game1.activeClickableMenu = new ItemGrabMenu(this.output.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromChest), false, true, true, true, true, 1, null, 1, this);
				return true;
			}
			return base.doAction(tileLocation, who);
		}

		public override void drawInMenu(SpriteBatch b, int x, int y)
		{
			this.drawShadow(b, x, y);
			b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(0, 0, 48, 64)), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
		}

		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.sourceRect), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
			if (!this.output.isEmpty())
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize * 2 + Game1.pixelZoom * 3), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize - Game1.tileSize / 2))), new Rectangle?(this.bagRect), this.color * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize + 1) / 10000f);
			}
			if (Game1.timeOfDay >= 2000 && Game1.timeOfDay < 2400 && !Game1.IsWinter && this.wasLit)
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize - Game1.tileSize))), new Rectangle?(this.lightInteriorRect), this.color * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize + 1) / 10000f);
			}
		}
	}
}
