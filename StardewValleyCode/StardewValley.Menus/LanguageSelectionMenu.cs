using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class LanguageSelectionMenu : IClickableMenu
	{
		public new const int width = 500;

		public new const int height = 650;

		private Texture2D texture;

		public List<ClickableComponent> languages = new List<ClickableComponent>();

		private bool isReadyToClose;

		public LanguageSelectionMenu()
		{
			this.texture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\LanguageButtons");
			Vector2 topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(500, 650, 0, 0);
			this.languages.Clear();
			int num = (int)((double)Game1.tileSize * 1.3);
			this.languages.Add(new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize, (int)topLeftPositionForCenteringOnScreen.Y + 650 - 30 - num * 7 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, num), "English", null)
			{
				myID = 0,
				downNeighborID = 1
			});
			this.languages.Add(new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize, (int)topLeftPositionForCenteringOnScreen.Y + 650 - 30 - num * 6 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, num), "German", null)
			{
				myID = 1,
				upNeighborID = 0,
				downNeighborID = 2
			});
			this.languages.Add(new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize, (int)topLeftPositionForCenteringOnScreen.Y + 650 - 30 - num * 4 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, num), "Russian", null)
			{
				myID = 3,
				upNeighborID = 2,
				downNeighborID = 4
			});
			this.languages.Add(new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize, (int)topLeftPositionForCenteringOnScreen.Y + 650 - 30 - num - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, num), "Chinese", null)
			{
				myID = 6,
				upNeighborID = 5
			});
			this.languages.Add(new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize, (int)topLeftPositionForCenteringOnScreen.Y + 650 - 30 - num * 2 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, num), "Japanese", null)
			{
				myID = 5,
				upNeighborID = 4,
				downNeighborID = 6
			});
			this.languages.Add(new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize, (int)topLeftPositionForCenteringOnScreen.Y + 650 - 30 - num * 5 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, num), "Spanish", null)
			{
				myID = 2,
				upNeighborID = 1,
				downNeighborID = 3
			});
			this.languages.Add(new ClickableComponent(new Rectangle((int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize, (int)topLeftPositionForCenteringOnScreen.Y + 650 - 30 - num * 3 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, num), "Portuguese", null)
			{
				myID = 4,
				upNeighborID = 3,
				downNeighborID = 5
			});
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(0);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			foreach (ClickableComponent current in this.languages)
			{
				if (current.containsPoint(x, y))
				{
					Game1.playSound("select");
					string name = current.name;
					uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
					if (num <= 1197024134u)
					{
						if (num != 286263347u)
						{
							if (num != 463134907u)
							{
								if (num != 1197024134u)
								{
									goto IL_145;
								}
								if (!(name == "Russian"))
								{
									goto IL_145;
								}
								LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.ru;
							}
							else
							{
								if (!(name == "English"))
								{
									goto IL_145;
								}
								LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
							}
						}
						else
						{
							if (!(name == "German"))
							{
								goto IL_145;
							}
							LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.de;
						}
					}
					else if (num <= 2483826186u)
					{
						if (num != 2115103848u)
						{
							if (num != 2483826186u)
							{
								goto IL_145;
							}
							if (!(name == "Japanese"))
							{
								goto IL_145;
							}
							LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.ja;
						}
						else
						{
							if (!(name == "Chinese"))
							{
								goto IL_145;
							}
							LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.zh;
						}
					}
					else if (num != 3088679515u)
					{
						if (num != 3872816476u)
						{
							goto IL_145;
						}
						if (!(name == "Portuguese"))
						{
							goto IL_145;
						}
						LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.pt;
					}
					else
					{
						if (!(name == "Spanish"))
						{
							goto IL_145;
						}
						LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.es;
					}
					IL_14B:
					this.isReadyToClose = true;
					if (Game1.options.SnappyMenus)
					{
						Game1.activeClickableMenu.setCurrentlySnappedComponentTo(81118);
						Game1.activeClickableMenu.snapCursorToCurrentSnappedComponent();
						continue;
					}
					continue;
					IL_145:
					LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
					goto IL_14B;
				}
			}
			this.isWithinBounds(x, y);
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			foreach (ClickableComponent current in this.languages)
			{
				if (current.containsPoint(x, y))
				{
					if (current.label == null)
					{
						Game1.playSound("Cowboy_Footstep");
						current.label = "hovered";
					}
				}
				else
				{
					current.label = null;
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void draw(SpriteBatch b)
		{
			Vector2 topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(500, 550, 0, 0);
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.6f);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(473, 36, 24, 24), (int)topLeftPositionForCenteringOnScreen.X + Game1.tileSize / 2, (int)topLeftPositionForCenteringOnScreen.Y - 55, 500 - Game1.tileSize, 640, Color.White, (float)Game1.pixelZoom, true);
			foreach (ClickableComponent current in this.languages)
			{
				int num = 0;
				string name = current.name;
				uint num2 = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num2 <= 1197024134u)
				{
					if (num2 != 286263347u)
					{
						if (num2 != 463134907u)
						{
							if (num2 == 1197024134u)
							{
								if (name == "Russian")
								{
									num = 3;
								}
							}
						}
						else if (name == "English")
						{
							num = 0;
						}
					}
					else if (name == "German")
					{
						num = 6;
					}
				}
				else if (num2 <= 2483826186u)
				{
					if (num2 != 2115103848u)
					{
						if (num2 == 2483826186u)
						{
							if (name == "Japanese")
							{
								num = 5;
							}
						}
					}
					else if (name == "Chinese")
					{
						num = 4;
					}
				}
				else if (num2 != 3088679515u)
				{
					if (num2 == 3872816476u)
					{
						if (name == "Portuguese")
						{
							num = 2;
						}
					}
				}
				else if (name == "Spanish")
				{
					num = 1;
				}
				int num3 = num * 78;
				num3 += ((current.label != null) ? 39 : 0);
				b.Draw(this.texture, current.bounds, new Rectangle?(new Rectangle(0, num3, 174, 40)), Color.White, 0f, new Vector2(0f, 0f), SpriteEffects.None, 0f);
			}
		}

		public override bool readyToClose()
		{
			return this.isReadyToClose;
		}
	}
}
