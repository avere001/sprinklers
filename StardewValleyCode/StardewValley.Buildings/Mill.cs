using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using System;

namespace StardewValley.Buildings
{
	public class Mill : Building
	{
		public Chest input;

		public Chest output;

		private bool hasLoadedToday;

		private Rectangle baseSourceRect = new Rectangle(0, 0, 64, 128);

		public Mill(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
			this.input = new Chest(true);
			this.output = new Chest(true);
		}

		public Mill()
		{
		}

		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, 64, this.texture.Bounds.Height);
		}

		public override void load()
		{
			base.load();
		}

		public override bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (this.daysOfConstructionLeft <= 0)
			{
				if (tileLocation.X == (float)(this.tileX + 1) && tileLocation.Y == (float)(this.tileY + 1))
				{
					if (who != null && who.ActiveObject != null)
					{
						bool flag = false;
						int parentSheetIndex = who.ActiveObject.parentSheetIndex;
						if (parentSheetIndex == 262 || parentSheetIndex == 284)
						{
							flag = true;
						}
						if (!flag)
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:CantMill", new object[0]));
							return false;
						}
						Item item = Utility.addItemToThisInventoryList(who.ActiveObject, this.input.items, 36);
						if (item != null)
						{
							who.ActiveObject = null;
							who.ActiveObject = (StardewValley.Object)item;
						}
						else
						{
							who.ActiveObject = null;
						}
						this.hasLoadedToday = true;
						Game1.playSound("Ship");
						if (who.ActiveObject != null)
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:MillFull", new object[0]));
						}
					}
				}
				else if (tileLocation.X == (float)(this.tileX + 3) && tileLocation.Y == (float)(this.tileY + 1))
				{
					Game1.activeClickableMenu = new ItemGrabMenu(this.output.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromChest), false, true, true, true, true, 1, null, -1, null);
				}
			}
			return base.doAction(tileLocation, who);
		}

		public override void dayUpdate(int dayOfMonth)
		{
			this.hasLoadedToday = false;
			for (int i = this.input.items.Count - 1; i >= 0; i--)
			{
				if (this.input.items[i] != null)
				{
					Item item = null;
					int parentSheetIndex = this.input.items[i].parentSheetIndex;
					if (parentSheetIndex <= 246)
					{
						if (parentSheetIndex == 245 || parentSheetIndex == 246)
						{
							item = this.input.items[i];
						}
					}
					else if (parentSheetIndex != 262)
					{
						if (parentSheetIndex == 284)
						{
							item = new StardewValley.Object(245, 3 * this.input.items[i].Stack, false, -1, 0);
						}
					}
					else
					{
						item = new StardewValley.Object(246, this.input.items[i].Stack, false, -1, 0);
					}
					if (item != null && Utility.canItemBeAddedToThisInventoryList(item, this.output.items, 36))
					{
						this.input.items[i] = Utility.addItemToThisInventoryList(item, this.output.items, 36);
					}
				}
			}
			base.dayUpdate(dayOfMonth);
		}

		public override void drawInMenu(SpriteBatch b, int x, int y)
		{
			this.drawShadow(b, x, y);
			b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(this.getSourceRectForMenu()), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
			b.Draw(this.texture, new Vector2((float)(x + Game1.tileSize / 2), (float)(y + Game1.pixelZoom)), new Rectangle?(new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 % 160, (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 / 160 * 32, 32, 32)), this.color, 0f, new Vector2(0f, 0f), 4f, SpriteEffects.None, 0.9f);
		}

		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.baseSourceRect), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize / 2), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize + Game1.pixelZoom))), new Rectangle?(new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 % 160, (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 / 160 * 32, 32, 32)), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
			if (this.hasLoadedToday)
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 13 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize + Game1.pixelZoom * 69))), new Rectangle?(new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 700 / 100 * 21, 72, 21, 8)), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
			}
			if (this.output.items.Count > 0 && this.output.items[0] != null && (this.output.items[0].parentSheetIndex == 245 || this.output.items[0].parentSheetIndex == 246))
			{
				float num = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize * 3), (float)(this.tileY * Game1.tileSize - Game1.tileSize * 3 / 2) + num)), new Rectangle?(new Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + 1) * Game1.tileSize) / 10000f + 1E-06f + (float)this.tileX / 10000f);
				b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize * 3 + Game1.tileSize / 2 + Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - Game1.tileSize + Game1.tileSize / 8) + num)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.output.items[0].parentSheetIndex, 16, 16)), Color.White * 0.75f, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((this.tileY + 1) * Game1.tileSize) / 10000f + 1E-05f + (float)this.tileX / 10000f);
			}
		}
	}
}
