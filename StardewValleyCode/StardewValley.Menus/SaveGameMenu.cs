using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Text;

namespace StardewValley.Menus
{
	public class SaveGameMenu : IClickableMenu, IDisposable
	{
		private IEnumerator<int> loader;

		private int completePause = -1;

		public bool quit;

		public bool hasDrawn;

		private SparklingText saveText;

		private int margin = 500;

		private StringBuilder _stringBuilder = new StringBuilder();

		private float _ellipsisDelay = 0.5f;

		private int _ellipsisCount;

		public SaveGameMenu()
		{
			this.saveText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:SaveGameMenu.cs.11378", new object[0]), Color.LimeGreen, Color.Black * 0.001f, false, 0.1, 1500, Game1.tileSize / 2, 500);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void update(GameTime time)
		{
			if (this.quit)
			{
				return;
			}
			base.update(time);
			if (!Game1.saveOnNewDay)
			{
				this.quit = true;
				if (Game1.activeClickableMenu.Equals(this))
				{
					Game1.player.checkForLevelTenStatus();
					Game1.exitActiveMenu();
				}
				return;
			}
			if (this.loader != null)
			{
				this.loader.MoveNext();
				if (this.loader.Current >= 100)
				{
					this.margin -= time.ElapsedGameTime.Milliseconds;
					if (this.margin <= 0)
					{
						Game1.playSound("money");
						this.completePause = 1500;
						this.loader = null;
						Game1.game1.IsSaving = false;
					}
				}
				this._ellipsisDelay -= (float)time.ElapsedGameTime.TotalSeconds;
				if (this._ellipsisDelay <= 0f)
				{
					this._ellipsisDelay += 0.75f;
					this._ellipsisCount++;
					if (this._ellipsisCount > 3)
					{
						this._ellipsisCount = 1;
					}
				}
			}
			else if (this.hasDrawn && this.completePause == -1)
			{
				Game1.game1.IsSaving = true;
				this.loader = SaveGame.Save();
			}
			if (this.completePause >= 0)
			{
				this.completePause -= time.ElapsedGameTime.Milliseconds;
				this.saveText.update(time);
				if (this.completePause < 0)
				{
					this.quit = true;
					this.completePause = -9999;
					if (Game1.activeClickableMenu.Equals(this))
					{
						Game1.player.checkForLevelTenStatus();
						Game1.exitActiveMenu();
					}
					Game1.currentLocation.resetForPlayerEntry();
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			Vector2 vector = new Vector2((float)Game1.tileSize, (float)(Game1.viewport.Height - Game1.tileSize));
			Vector2 renderSize = new Vector2((float)Game1.tileSize, (float)Game1.tileSize);
			vector = Utility.makeSafe(vector, renderSize);
			if (this.completePause >= 0)
			{
				this.saveText.draw(b, vector);
			}
			else
			{
				this._stringBuilder.Clear();
				this._stringBuilder.Append(Game1.content.LoadString("Strings\\StringsFromCSFiles:SaveGameMenu.cs.11381", new object[0]));
				for (int i = 0; i < this._ellipsisCount; i++)
				{
					this._stringBuilder.Append(".");
				}
				b.DrawString(Game1.dialogueFont, this._stringBuilder, vector, Color.White);
			}
			this.hasDrawn = true;
		}

		public void Dispose()
		{
			Game1.game1.IsSaving = false;
		}
	}
}
