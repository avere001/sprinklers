using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class JojaCDMenu : IClickableMenu
	{
		public new const int width = 1280;

		public new const int height = 576;

		public const int buttonWidth = 147;

		public const int buttonHeight = 30;

		private Texture2D noteTexture;

		public List<ClickableComponent> checkboxes = new List<ClickableComponent>();

		private string hoverText;

		private bool boughtSomething;

		private int exitTimer = -1;

		public JojaCDMenu(Texture2D noteTexture) : base(Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 288, 1280, 576, true)
		{
			Game1.player.forceCanMove();
			this.noteTexture = noteTexture;
			int num = this.xPositionOnScreen + Game1.pixelZoom;
			int num2 = this.yPositionOnScreen + 52 * Game1.pixelZoom;
			for (int i = 0; i < 5; i++)
			{
				this.checkboxes.Add(new ClickableComponent(new Rectangle(num, num2, 147 * Game1.pixelZoom, 30 * Game1.pixelZoom), string.Concat(i))
				{
					myID = i,
					rightNeighborID = ((i % 2 != 0 || i == 4) ? -1 : (i + 1)),
					leftNeighborID = ((i % 2 == 0) ? -1 : (i - 1)),
					downNeighborID = i + 2,
					upNeighborID = i - 2
				});
				num += 148 * Game1.pixelZoom;
				if (num > this.xPositionOnScreen + 148 * Game1.pixelZoom * 2)
				{
					num = this.xPositionOnScreen + Game1.pixelZoom;
					num2 += 30 * Game1.pixelZoom;
				}
			}
			if (Game1.player.hasOrWillReceiveMail("ccVault"))
			{
				this.checkboxes[0].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccBoilerRoom"))
			{
				this.checkboxes[1].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccCraftsRoom"))
			{
				this.checkboxes[2].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccPantry"))
			{
				this.checkboxes[3].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccFishTank"))
			{
				this.checkboxes[4].name = "complete";
			}
			this.exitFunction = new IClickableMenu.onExit(this.onExitFunction);
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
				Game1.mouseCursorTransparency = 1f;
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(0);
			this.snapCursorToCurrentSnappedComponent();
		}

		private void onExitFunction()
		{
			if (this.boughtSomething)
			{
				JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_JojaCDConfirm", new object[0]), false, false);
				Game1.drawDialogue(JojaMart.Morris);
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.exitTimer >= 0)
			{
				return;
			}
			base.receiveLeftClick(x, y, true);
			foreach (ClickableComponent current in this.checkboxes)
			{
				if (current.containsPoint(x, y) && !current.name.Equals("complete"))
				{
					int buttonNumber = Convert.ToInt32(current.name);
					int priceFromButtonNumber = this.getPriceFromButtonNumber(buttonNumber);
					if (Game1.player.money >= priceFromButtonNumber)
					{
						Game1.player.money -= priceFromButtonNumber;
						Game1.playSound("reward");
						current.name = "complete";
						this.boughtSomething = true;
						switch (buttonNumber)
						{
						case 0:
							Game1.addMailForTomorrow("jojaVault", true, true);
							Game1.addMailForTomorrow("ccVault", true, true);
							break;
						case 1:
							Game1.addMailForTomorrow("jojaBoilerRoom", true, true);
							Game1.addMailForTomorrow("ccBoilerRoom", true, true);
							break;
						case 2:
							Game1.addMailForTomorrow("jojaCraftsRoom", true, true);
							Game1.addMailForTomorrow("ccCraftsRoom", true, true);
							break;
						case 3:
							Game1.addMailForTomorrow("jojaPantry", true, true);
							Game1.addMailForTomorrow("ccPantry", true, true);
							break;
						case 4:
							Game1.addMailForTomorrow("jojaFishTank", true, true);
							Game1.addMailForTomorrow("ccFishTank", true, true);
							break;
						}
						this.exitTimer = 1000;
					}
					else
					{
						Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
					}
				}
			}
		}

		public override bool readyToClose()
		{
			return true;
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.exitTimer >= 0)
			{
				this.exitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.exitTimer <= 0)
				{
					base.exitThisMenu(true);
				}
			}
			Game1.mouseCursorTransparency = 1f;
		}

		public int getPriceFromButtonNumber(int buttonNumber)
		{
			switch (buttonNumber)
			{
			case 0:
				return 40000;
			case 1:
				return 15000;
			case 2:
				return 25000;
			case 3:
				return 35000;
			case 4:
				return 20000;
			default:
				return -1;
			}
		}

		public string getDescriptionFromButtonNumber(int buttonNumber)
		{
			return Game1.content.LoadString("Strings\\UI:JojaCDMenu_Hover" + buttonNumber, new object[0]);
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.hoverText = "";
			foreach (ClickableComponent current in this.checkboxes)
			{
				if (current.containsPoint(x, y))
				{
					this.hoverText = (current.name.Equals("complete") ? "" : Game1.parseText(this.getDescriptionFromButtonNumber(Convert.ToInt32(current.name)), Game1.dialogueFont, Game1.tileSize * 6));
				}
			}
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - 640;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - 288;
			int num = this.xPositionOnScreen + Game1.pixelZoom;
			int num2 = this.yPositionOnScreen + 52 * Game1.pixelZoom;
			this.checkboxes.Clear();
			for (int i = 0; i < 5; i++)
			{
				this.checkboxes.Add(new ClickableComponent(new Rectangle(num, num2, 147 * Game1.pixelZoom, 30 * Game1.pixelZoom), string.Concat(i)));
				num += 148 * Game1.pixelZoom;
				if (num > this.xPositionOnScreen + 148 * Game1.pixelZoom * 2)
				{
					num = this.xPositionOnScreen + Game1.pixelZoom;
					num2 += 30 * Game1.pixelZoom;
				}
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			b.Draw(this.noteTexture, Utility.getTopLeftPositionForCenteringOnScreen(1280, 576, 0, 0), new Rectangle?(new Rectangle(0, 0, 320, 144)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.79f);
			base.draw(b);
			foreach (ClickableComponent current in this.checkboxes)
			{
				if (current.name.Equals("complete"))
				{
					b.Draw(this.noteTexture, new Vector2((float)(current.bounds.Left + 4 * Game1.pixelZoom), (float)(current.bounds.Y + 4 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 144, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
				}
			}
			Game1.dayTimeMoneyBox.drawMoneyBox(b, Game1.viewport.Width - 300 - IClickableMenu.spaceToClearSideBorder * 2, Game1.pixelZoom);
			Game1.mouseCursorTransparency = 1f;
			base.drawMouse(b);
			if (this.hoverText != null && !this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}
	}
}
