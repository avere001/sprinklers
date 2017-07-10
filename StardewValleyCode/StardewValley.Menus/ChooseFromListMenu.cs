using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class ChooseFromListMenu : IClickableMenu
	{
		public delegate void actionOnChoosingListOption(string s);

		public const int region_backButton = 101;

		public const int region_forwardButton = 102;

		public const int region_okButton = 103;

		public const int region_cancelButton = 104;

		public const int w = 640;

		public const int h = 192;

		public ClickableTextureComponent backButton;

		public ClickableTextureComponent forwardButton;

		public ClickableTextureComponent okButton;

		public ClickableTextureComponent cancelButton;

		private List<string> options = new List<string>();

		private int index;

		private ChooseFromListMenu.actionOnChoosingListOption chooseAction;

		private bool isJukebox;

		public ChooseFromListMenu(List<string> options, ChooseFromListMenu.actionOnChoosingListOption chooseAction, bool isJukebox = false) : base(Game1.viewport.Width / 2 - 320, Game1.viewport.Height - Game1.tileSize - 192, 640, 192, false)
		{
			this.chooseAction = chooseAction;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 101,
				rightNeighborID = 102
			};
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 640 + Game1.pixelZoom * 4 + Game1.tileSize, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 102,
				leftNeighborID = 101,
				rightNeighborID = 103
			};
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, new Rectangle(175, 379, 16, 15), (float)Game1.pixelZoom, false)
			{
				myID = 103,
				leftNeighborID = 102,
				rightNeighborID = 104
			};
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 3 + Game1.pixelZoom * 3, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false)
			{
				myID = 104,
				leftNeighborID = 103
			};
			Game1.playSound("bigSelect");
			this.isJukebox = isJukebox;
			if (isJukebox)
			{
				for (int i = options.Count - 1; i >= 0; i--)
				{
					if (options[i].ToLower().Contains("ambient") || options[i].ToLower().Contains("bigdrums") || options[i].ToLower().Contains("clubloop"))
					{
						options.RemoveAt(i);
					}
					else
					{
						string text = options[i];
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num <= 1611928003u)
						{
							if (num != 575982768u)
							{
								if (num != 1176080900u)
								{
									if (num == 1611928003u)
									{
										if (text == "buglevelloop")
										{
											options.RemoveAt(i);
										}
									}
								}
								else if (text == "jojaOfficeSoundscape")
								{
									options.RemoveAt(i);
								}
							}
							else if (text == "title_day")
							{
								options.RemoveAt(i);
								options.Add("MainTheme");
							}
						}
						else if (num <= 3528263180u)
						{
							if (num != 3332712824u)
							{
								if (num == 3528263180u)
								{
									if (text == "coin")
									{
										options.RemoveAt(i);
									}
								}
							}
							else if (text == "nightTime")
							{
								options.RemoveAt(i);
							}
						}
						else if (num != 3564132753u)
						{
							if (num == 3819582179u)
							{
								if (text == "communityCenter")
								{
									options.RemoveAt(i);
								}
							}
						}
						else if (text == "ocean")
						{
							options.RemoveAt(i);
						}
					}
				}
			}
			this.options = options;
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(103);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - 320;
			this.yPositionOnScreen = Game1.viewport.Height - Game1.tileSize - 192;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 640 + Game1.pixelZoom * 4 + Game1.tileSize, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, new Rectangle(175, 379, 16, 15), (float)Game1.pixelZoom, false);
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 3 + Game1.pixelZoom * 3, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
		}

		public static void playSongAction(string s)
		{
			Game1.changeMusicTrack(s);
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.okButton.tryHover(x, y, 0.1f);
			this.cancelButton.tryHover(x, y, 0.1f);
			this.backButton.tryHover(x, y, 0.1f);
			this.forwardButton.tryHover(x, y, 0.1f);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (this.okButton.containsPoint(x, y) && this.chooseAction != null)
			{
				this.chooseAction(this.options[this.index]);
				Game1.playSound("select");
			}
			if (this.cancelButton.containsPoint(x, y))
			{
				base.exitThisMenu(true);
			}
			if (this.backButton.containsPoint(x, y))
			{
				this.index--;
				if (this.index < 0)
				{
					this.index = this.options.Count - 1;
				}
				this.backButton.scale = this.backButton.baseScale - 1f;
				Game1.playSound("shwip");
			}
			if (this.forwardButton.containsPoint(x, y))
			{
				this.index++;
				this.index %= this.options.Count;
				Game1.playSound("shwip");
				this.forwardButton.scale = this.forwardButton.baseScale - 1f;
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			string text = "Summer (The Sun Can Bend An Orange Sky)";
			int num = (int)Game1.dialogueFont.MeasureString(this.isJukebox ? text : this.options[this.index]).X;
			IClickableMenu.drawTextureBox(b, this.xPositionOnScreen + this.width / 2 - num / 2 - Game1.pixelZoom * 4, this.yPositionOnScreen + Game1.tileSize - Game1.pixelZoom, num + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 4, Color.White);
			if (this.index < this.options.Count)
			{
				Utility.drawTextWithShadow(b, this.isJukebox ? Utility.getSongTitleFromCueName(this.options[this.index]) : this.options[this.index], Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.isJukebox ? Utility.getSongTitleFromCueName(this.options[this.index]) : this.options[this.index]).X / 2f, (float)(this.yPositionOnScreen + this.height / 2 - Game1.pixelZoom * 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
			}
			this.okButton.draw(b);
			this.cancelButton.draw(b);
			this.forwardButton.draw(b);
			this.backButton.draw(b);
			if (this.isJukebox)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\UI:JukeboxMenu_Title", new object[0]), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen - Game1.tileSize / 2, "", 1f, -1, 0, 0.88f, false);
			}
			base.drawMouse(b);
		}
	}
}
