using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class FarmInfoPage : IClickableMenu
	{
		private string descriptionText = "";

		private string hoverText = "";

		private ClickableTextureComponent moneyIcon;

		private ClickableTextureComponent farmMap;

		private ClickableTextureComponent mapFarmer;

		private ClickableTextureComponent farmHouse;

		private List<ClickableTextureComponent> animals = new List<ClickableTextureComponent>();

		private List<ClickableTextureComponent> mapBuildings = new List<ClickableTextureComponent>();

		private List<MiniatureTerrainFeature> mapFeatures = new List<MiniatureTerrainFeature>();

		private Farm farm;

		private int mapX;

		private int mapY;

		public FarmInfoPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.moneyIcon = new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2, (Game1.player.Money > 9999) ? 18 : 20, 16), Game1.player.Money + "g", "", Game1.debrisSpriteSheet, new Rectangle(88, 280, 16, 16), 1f, false);
			this.mapX = x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + Game1.tileSize / 2 + Game1.tileSize / 4;
			this.mapY = y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3 - 4;
			this.farmMap = new ClickableTextureComponent(new Rectangle(this.mapX, this.mapY, 20, 20), Game1.content.Load<Texture2D>("LooseSprites\\farmMap"), Rectangle.Empty, 1f, false);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			int num14 = 0;
			int num15 = 0;
			int num16 = 0;
			this.farm = (Farm)Game1.getLocationFromName("Farm");
			this.farmHouse = new ClickableTextureComponent("FarmHouse", new Rectangle(this.mapX + 443, this.mapY + 43, 80, 72), "FarmHouse", "", Game1.content.Load<Texture2D>("Buildings\\houses"), new Rectangle(0, 0, 160, 144), 0.5f, false);
			foreach (FarmAnimal current in this.farm.getAllFarmAnimals())
			{
				if (current.type.Contains("Chicken"))
				{
					num++;
					num9 += current.friendshipTowardFarmer;
				}
				else
				{
					string type = current.type;
					if (!(type == "Cow"))
					{
						if (!(type == "Duck"))
						{
							if (!(type == "Rabbit"))
							{
								if (!(type == "Sheep"))
								{
									if (!(type == "Goat"))
									{
										if (!(type == "Pig"))
										{
											num4++;
											num12 += current.friendshipTowardFarmer;
										}
										else
										{
											num8++;
											num16 += current.friendshipTowardFarmer;
										}
									}
									else
									{
										num7++;
										num14 += current.friendshipTowardFarmer;
									}
								}
								else
								{
									num6++;
									num15 += current.friendshipTowardFarmer;
								}
							}
							else
							{
								num3++;
								num10 += current.friendshipTowardFarmer;
							}
						}
						else
						{
							num2++;
							num11 += current.friendshipTowardFarmer;
						}
					}
					else
					{
						num5++;
						num13 += current.friendshipTowardFarmer;
					}
				}
			}
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize, Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num), "Chickens" + ((num > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num9 / num
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num2), "Ducks" + ((num2 > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num11 / num2
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 2 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num3), "Rabbits" + ((num3 > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num10 / num3
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 3 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num5), "Cows" + ((num5 > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num13 / num5
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 4 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num7), "Goats" + ((num7 > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num14 / num7
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 5 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num6), "Sheep" + ((num6 > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num15 / num6
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 6 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num8), "Pigs" + ((num8 > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num16 / num8
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 7 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(num4), "???" + ((num4 > 0) ? (Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", new object[]
			{
				num12 / num4
			})) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 8 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(Game1.stats.CropsShipped), Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10440", new object[0]), Game1.mouseCursors, new Rectangle(Game1.tileSize * 7 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 9 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(this.farm.buildings.Count<Building>()), Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10441", new object[0]), Game1.mouseCursors, new Rectangle(Game1.tileSize * 7, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			int num17 = 8;
			foreach (Building current2 in this.farm.buildings)
			{
				this.mapBuildings.Add(new ClickableTextureComponent("", new Rectangle(this.mapX + current2.tileX * num17, this.mapY + current2.tileY * num17 + (current2.tilesHigh + 1) * num17 - (int)((float)current2.texture.Height / 8f), current2.tilesWide * num17, (int)((float)current2.texture.Height / 8f)), "", current2.buildingType, current2.texture, current2.getSourceRectForMenu(), 0.125f, false));
			}
			foreach (KeyValuePair<Vector2, TerrainFeature> current3 in this.farm.terrainFeatures)
			{
				this.mapFeatures.Add(new MiniatureTerrainFeature(current3.Value, new Vector2(current3.Key.X * (float)num17 + (float)this.mapX, current3.Key.Y * (float)num17 + (float)this.mapY), current3.Key, 0.125f));
			}
			if (Game1.currentLocation.GetType() == typeof(Farm))
			{
				this.mapFarmer = new ClickableTextureComponent("", new Rectangle(this.mapX + (int)(Game1.player.Position.X / 8f), this.mapY + (int)(Game1.player.position.Y / 8f), 8, 12), "", Game1.player.name, null, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize * 3 / 2), 0.125f, false);
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			foreach (ClickableTextureComponent current in this.animals)
			{
				if (current.containsPoint(x, y))
				{
					this.hoverText = current.hoverText;
					return;
				}
			}
			foreach (ClickableTextureComponent current2 in this.mapBuildings)
			{
				if (current2.containsPoint(x, y))
				{
					this.hoverText = current2.hoverText;
					return;
				}
			}
			if (this.mapFarmer != null && this.mapFarmer.containsPoint(x, y))
			{
				this.hoverText = this.mapFarmer.hoverText;
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.drawVerticalPartition(b, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, false);
			this.moneyIcon.draw(b);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.animals.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			this.farmMap.draw(b);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.mapBuildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			this.farmMap.draw(b);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.mapBuildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			using (List<MiniatureTerrainFeature>.Enumerator enumerator2 = this.mapFeatures.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.draw(b);
				}
			}
			this.farmHouse.draw(b);
			if (this.mapFarmer != null)
			{
				Game1.player.FarmerRenderer.drawMiniPortrat(b, new Vector2((float)(this.mapFarmer.bounds.X - 16), (float)(this.mapFarmer.bounds.Y - 16)), 0.99f, 2f, 2, Game1.player);
			}
			foreach (KeyValuePair<long, FarmAnimal> current in this.farm.animals)
			{
				b.Draw(current.Value.sprite.Texture, new Vector2((float)(this.mapX + (int)(current.Value.position.X / 8f)), (float)(this.mapY + (int)(current.Value.position.Y / 8f))), new Rectangle?(current.Value.sprite.SourceRect), Color.White, 0f, Vector2.Zero, 0.125f, SpriteEffects.None, 0.86f + current.Value.position.Y / 8f / 20000f + 0.0125f);
			}
			foreach (KeyValuePair<Vector2, StardewValley.Object> current2 in this.farm.objects)
			{
				current2.Value.drawInMenu(b, new Vector2((float)this.mapX + current2.Key.X * 8f, (float)this.mapY + current2.Key.Y * 8f), 0.125f, 1f, 0.86f + ((float)this.mapY + current2.Key.Y * 8f - 25f) / 20000f);
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
