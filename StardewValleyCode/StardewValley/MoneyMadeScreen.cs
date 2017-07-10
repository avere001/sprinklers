using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley
{
	public class MoneyMadeScreen
	{
		private const int timeToDisplayEachItem = 200;

		private Dictionary<ShippedItem, int> shippingItems = new Dictionary<ShippedItem, int>();

		public bool complete;

		public bool canProceed;

		public bool throbUp;

		public bool day;

		private int currentItemIndex;

		private int timeOnCurrentItem;

		private int total;

		private float starScale = 1f;

		public MoneyMadeScreen(List<Object> shippingItems, int timeOfDay)
		{
			if (timeOfDay < 2000)
			{
				this.day = true;
			}
			int randomItemFromSeason = Utility.getRandomItemFromSeason(Game1.currentSeason, 0, false);
			int num = Game1.cropsOfTheWeek[(Game1.dayOfMonth - 1) / 7];
			foreach (Object current in shippingItems)
			{
				ShippedItem shippedItem = new ShippedItem(current.ParentSheetIndex, current.Price, current.name);
				int num2 = current.Price * current.Stack;
				if (current.ParentSheetIndex == randomItemFromSeason)
				{
					num2 = (int)((float)num2 * 1.2f);
				}
				if (current.ParentSheetIndex == num)
				{
					num2 = (int)((float)num2 * 1.1f);
				}
				if (current.Name.Contains("="))
				{
					num2 += num2 / 2;
				}
				num2 -= num2 % 5;
				if (this.shippingItems.ContainsKey(shippedItem))
				{
					Dictionary<ShippedItem, int> arg_EB_0 = this.shippingItems;
					ShippedItem key = shippedItem;
					int num3 = arg_EB_0[key];
					arg_EB_0[key] = num3 + 1;
				}
				else
				{
					this.shippingItems.Add(shippedItem, current.Stack);
				}
				this.total += num2;
			}
		}

		public void update(int milliseconds, bool keyDown)
		{
			if (!this.complete)
			{
				this.timeOnCurrentItem += (keyDown ? (milliseconds * 2) : milliseconds);
				if (this.timeOnCurrentItem >= 200)
				{
					this.currentItemIndex++;
					Game1.playSound("shiny4");
					this.timeOnCurrentItem = 0;
					if (this.currentItemIndex == this.shippingItems.Count)
					{
						this.complete = true;
					}
				}
			}
			else
			{
				this.timeOnCurrentItem += (keyDown ? (milliseconds * 2) : milliseconds);
				if (this.timeOnCurrentItem >= 1000)
				{
					this.canProceed = true;
				}
			}
			if (this.throbUp)
			{
				this.starScale += 0.01f;
			}
			else
			{
				this.starScale -= 0.01f;
			}
			if (this.starScale >= 1.2f)
			{
				this.throbUp = false;
				return;
			}
			if (this.starScale <= 1f)
			{
				this.throbUp = true;
			}
		}

		public void draw(GameTime gametime)
		{
			if (this.day)
			{
				Game1.graphics.GraphicsDevice.Clear(Utility.getSkyColorForSeason(Game1.currentSeason));
			}
			Game1.drawTitleScreenBackground(gametime, this.day ? "_day" : "_night", Utility.weatherDebrisOffsetForSeason(Game1.currentSeason));
			int num = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - Game1.tileSize * 2;
			int num2 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.X + Game1.graphics.GraphicsDevice.Viewport.Width / 2 - (int)((float)((this.shippingItems.Count / (num / Game1.tileSize - 4) + 1) * Game1.tileSize) * 3f);
			int num3 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + Game1.tileSize;
			int num4 = (int)((float)((this.shippingItems.Count / (num / Game1.tileSize - 4) + 1) * Game1.tileSize) * 6f);
			Game1.drawDialogueBox(num2, num3, num4, num, false, true, null, false);
			int num5 = num - Game1.tileSize * 3;
			Point point = new Point(num2 + Game1.tileSize, num3 + Game1.tileSize);
			for (int i = 0; i < this.currentItemIndex; i++)
			{
				Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float)(point.X + i * Game1.tileSize / (num5 - Game1.tileSize * 2) * Game1.tileSize * 4 + Game1.tileSize / 2), (float)(i * Game1.tileSize % (num5 - Game1.tileSize * 2) - i * Game1.tileSize % (num5 - Game1.tileSize * 2) % Game1.tileSize + Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + Game1.tileSize * 3 + Game1.tileSize / 2)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.shippingItems.Keys.ElementAt(i).index, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.shippingItems.Keys.ElementAt(i).name.Contains("=") ? this.starScale : 1f, SpriteEffects.None, 0.999999f);
				Game1.spriteBatch.DrawString(Game1.dialogueFont, string.Concat(new object[]
				{
					"x",
					this.shippingItems[this.shippingItems.Keys.ElementAt(i)],
					" : ",
					this.shippingItems.Keys.ElementAt(i).price * this.shippingItems[this.shippingItems.Keys.ElementAt(i)],
					"g"
				}), new Vector2((float)(point.X + i * Game1.tileSize / (num5 - Game1.tileSize * 2) * Game1.tileSize * 4 + Game1.tileSize), (float)(i * Game1.tileSize % (num5 - Game1.tileSize * 2) - i * Game1.tileSize % (num5 - Game1.tileSize * 2) % Game1.tileSize + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString("9").Y / 2f + (float)Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + (float)(Game1.tileSize * 3)), Game1.textColor);
			}
			if (this.complete)
			{
				Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:MoneyMadeScreen.cs.3854", new object[]
				{
					this.total
				}), new Vector2((float)(num2 + num4 - Game1.tileSize) - Game1.dialogueFont.MeasureString("Total: " + this.total).X, (float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 5 / 2)), Game1.textColor);
			}
		}
	}
}
