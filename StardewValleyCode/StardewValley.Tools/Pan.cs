using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Tools
{
	public class Pan : Tool
	{
		public Pan() : base("Copper Pan", -1, 12, 12, false, 0)
		{
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pan.cs.14180", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pan.cs.14181", new object[0]);
		}

		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			this.currentParentTileIndex = 12;
			this.indexOfMenuItemView = 12;
			bool flag = false;
			Rectangle value = new Rectangle(location.orePanPoint.X * Game1.tileSize - Game1.tileSize, location.orePanPoint.Y * Game1.tileSize - Game1.tileSize, Game1.tileSize * 4, Game1.tileSize * 4);
			if (value.Contains(x, y) && Utility.distance((float)who.getStandingX(), (float)value.Center.X, (float)who.getStandingY(), (float)value.Center.Y) <= (float)(3 * Game1.tileSize))
			{
				flag = true;
			}
			who.lastClick = Vector2.Zero;
			x = (int)who.GetToolLocation(false).X;
			y = (int)who.GetToolLocation(false).Y;
			who.lastClick = new Vector2((float)x, (float)y);
			Point arg_DD_0 = location.orePanPoint;
			if (!location.orePanPoint.Equals(Point.Zero))
			{
				Rectangle boundingBox = who.GetBoundingBox();
				if (flag || boundingBox.Intersects(value))
				{
					who.faceDirection(2);
					who.FarmerSprite.animateOnce(303, 50f, 4);
					return true;
				}
			}
			who.forceCanMove();
			return true;
		}

		public static void playSlosh(Farmer who)
		{
			Game1.playSound("slosh");
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			x = (int)who.GetToolLocation(false).X;
			y = (int)who.GetToolLocation(false).Y;
			this.currentParentTileIndex = 12;
			this.indexOfMenuItemView = 12;
			Game1.playSound("coin");
			who.addItemsByMenuIfNecessary(this.getPanItems(location, who), null);
			location.orePanPoint = Point.Zero;
			location.orePanAnimation = null;
			who.CanMove = true;
			who.usingTool = false;
			who.canReleaseTool = true;
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			this.indexOfMenuItemView = 12;
			base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber);
		}

		public List<Item> getPanItems(GameLocation location, Farmer who)
		{
			List<Item> list = new List<Item>();
			int parentSheetIndex = 378;
			int num = -1;
			Random random = new Random(location.orePanPoint.X + location.orePanPoint.Y * 1000 + (int)Game1.stats.DaysPlayed);
			double num2 = random.NextDouble() - (double)who.luckLevel * 0.001 - Game1.dailyLuck;
			if (num2 < 0.01)
			{
				parentSheetIndex = 386;
			}
			else if (num2 < 0.241)
			{
				parentSheetIndex = 384;
			}
			else if (num2 < 0.6)
			{
				parentSheetIndex = 380;
			}
			int initialStack = random.Next(5) + 1 + (int)((random.NextDouble() + 0.1 + (double)((float)who.luckLevel / 10f) + Game1.dailyLuck) * 2.0);
			int num3 = random.Next(5) + 1 + (int)((random.NextDouble() + 0.1 + (double)((float)who.luckLevel / 10f)) * 2.0);
			num2 = random.NextDouble() - Game1.dailyLuck;
			if (num2 < 0.4 + (double)who.LuckLevel * 0.04)
			{
				num2 = random.NextDouble() - Game1.dailyLuck;
				num = 382;
				if (num2 < 0.02 + (double)who.LuckLevel * 0.002)
				{
					num = 72;
					num3 = 1;
				}
				else if (num2 < 0.1)
				{
					num = 60 + random.Next(5) * 2;
					num3 = 1;
				}
				else if (num2 < 0.36)
				{
					num = 749;
					num3 = Math.Max(1, num3 / 2);
				}
				else if (num2 < 0.5)
				{
					num = ((random.NextDouble() < 0.3) ? 82 : ((random.NextDouble() < 0.5) ? 84 : 86));
					num3 = 1;
				}
			}
			list.Add(new StardewValley.Object(parentSheetIndex, initialStack, false, -1, 0));
			if (num != -1)
			{
				list.Add(new StardewValley.Object(num, num3, false, -1, 0));
			}
			return list;
		}
	}
}
