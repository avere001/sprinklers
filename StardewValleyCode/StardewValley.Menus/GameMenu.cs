using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class GameMenu : IClickableMenu
	{
		public const int inventoryTab = 0;

		public const int skillsTab = 1;

		public const int socialTab = 2;

		public const int mapTab = 3;

		public const int craftingTab = 4;

		public const int collectionsTab = 5;

		public const int optionsTab = 6;

		public const int exitTab = 7;

		public const int region_inventoryTab = 12340;

		public const int region_skillsTab = 12341;

		public const int region_socialTab = 12342;

		public const int region_mapTab = 12343;

		public const int region_craftingTab = 12344;

		public const int region_collectionsTab = 12345;

		public const int region_optionsTab = 12346;

		public const int region_exitTab = 12347;

		public const int numberOfTabs = 7;

		public int currentTab;

		private string hoverText = "";

		private string descriptionText = "";

		private List<ClickableComponent> tabs = new List<ClickableComponent>();

		private List<IClickableMenu> pages = new List<IClickableMenu>();

		public bool invisible;

		public static bool forcePreventClose;

		public ClickableTextureComponent junimoNoteIcon;

		public GameMenu() : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
		{
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "inventory", Game1.content.LoadString("Strings\\UI:GameMenu_Inventory", new object[0]))
			{
				myID = 12340,
				downNeighborID = 0,
				rightNeighborID = 12341,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new InventoryPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "skills", Game1.content.LoadString("Strings\\UI:GameMenu_Skills", new object[0]))
			{
				myID = 12341,
				downNeighborID = 1,
				rightNeighborID = 12342,
				leftNeighborID = 12340,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new SkillsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width + ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? Game1.tileSize : 0), this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "social", Game1.content.LoadString("Strings\\UI:GameMenu_Social", new object[0]))
			{
				myID = 12342,
				downNeighborID = 2,
				rightNeighborID = 12343,
				leftNeighborID = 12341,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new SocialPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 4, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "map", Game1.content.LoadString("Strings\\UI:GameMenu_Map", new object[0]))
			{
				myID = 12343,
				downNeighborID = 3,
				rightNeighborID = 12344,
				leftNeighborID = 12342,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new MapPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 5, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "crafting", Game1.content.LoadString("Strings\\UI:GameMenu_Crafting", new object[0]))
			{
				myID = 12344,
				downNeighborID = 4,
				rightNeighborID = 12345,
				leftNeighborID = 12343,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new CraftingPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 6, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "collections", Game1.content.LoadString("Strings\\UI:GameMenu_Collections", new object[0]))
			{
				myID = 12345,
				downNeighborID = 5,
				rightNeighborID = 12346,
				leftNeighborID = 12344,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new CollectionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 7, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "options", Game1.content.LoadString("Strings\\UI:GameMenu_Options", new object[0]))
			{
				myID = 12346,
				downNeighborID = 6,
				rightNeighborID = 12347,
				leftNeighborID = 12345,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new OptionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width + ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? (Game1.tileSize * 3 / 2) : (Game1.tileSize / 2)), this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 8, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "exit", Game1.content.LoadString("Strings\\UI:GameMenu_Exit", new object[0]))
			{
				myID = 12347,
				downNeighborID = 7,
				leftNeighborID = 12346,
				tryDefaultIfNoDownNeighborExists = true,
				fullyImmutable = true
			});
			this.pages.Add(new ExitPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
			if (Game1.activeClickableMenu == null)
			{
				Game1.playSound("bigSelect");
			}
			if (Game1.player.hasOrWillReceiveMail("canReadJunimoText") && !Game1.player.hasOrWillReceiveMail("JojaMember") && !Game1.player.hasCompletedCommunityCenter())
			{
				this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false)
				{
					myID = 898,
					leftNeighborID = 11,
					downNeighborID = 106
				};
			}
			GameMenu.forcePreventClose = false;
			if (Game1.options.SnappyMenus)
			{
				this.pages[this.currentTab].populateClickableComponentList();
				this.pages[this.currentTab].allClickableComponents.AddRange(this.tabs);
				this.snapToDefaultClickableComponent();
			}
		}

		public GameMenu(int startingTab, int extra = -1) : this()
		{
			this.changeTab(startingTab);
			if (startingTab == 6 && extra != -1)
			{
				(this.pages[6] as OptionsPage).currentItemIndex = extra;
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			if (this.currentTab < this.pages.Count)
			{
				this.pages[this.currentTab].snapToDefaultClickableComponent();
			}
			if (this.junimoNoteIcon != null && this.currentTab < this.pages.Count && !this.pages[this.currentTab].allClickableComponents.Contains(this.junimoNoteIcon))
			{
				this.pages[this.currentTab].allClickableComponents.Add(this.junimoNoteIcon);
			}
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (b == Buttons.RightTrigger)
			{
				if (this.currentTab == 3)
				{
					Game1.activeClickableMenu = new GameMenu(4, -1);
					return;
				}
				if (this.currentTab < 7 && this.pages[this.currentTab].readyToClose())
				{
					this.changeTab(this.currentTab + 1);
					return;
				}
			}
			else if (b == Buttons.LeftTrigger)
			{
				if (this.currentTab == 3)
				{
					Game1.activeClickableMenu = new GameMenu(2, -1);
					return;
				}
				if (this.currentTab > 0 && this.pages[this.currentTab].readyToClose())
				{
					this.changeTab(this.currentTab - 1);
					return;
				}
			}
			else if (b == Buttons.Back && this.currentTab == 0)
			{
				this.pages[this.currentTab].receiveGamePadButton(b);
			}
		}

		public override void setUpForGamePadMode()
		{
			base.setUpForGamePadMode();
			if (this.pages.Count > this.currentTab)
			{
				this.pages[this.currentTab].setUpForGamePadMode();
			}
		}

		public override ClickableComponent getCurrentlySnappedComponent()
		{
			return this.pages[this.currentTab].getCurrentlySnappedComponent();
		}

		public override void setCurrentlySnappedComponentTo(int id)
		{
			this.pages[this.currentTab].setCurrentlySnappedComponentTo(id);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (!this.invisible && !GameMenu.forcePreventClose)
			{
				for (int i = 0; i < this.tabs.Count; i++)
				{
					if (this.tabs[i].containsPoint(x, y) && this.currentTab != i && this.pages[this.currentTab].readyToClose())
					{
						this.changeTab(this.getTabNumberFromName(this.tabs[i].name));
						return;
					}
				}
				if (this.junimoNoteIcon != null && this.junimoNoteIcon.containsPoint(x, y) && this.pages[this.currentTab].readyToClose())
				{
					Game1.activeClickableMenu = new JunimoNoteMenu(true, 1, false);
				}
			}
			this.pages[this.currentTab].receiveLeftClick(x, y, true);
		}

		public static string getLabelOfTabFromIndex(int index)
		{
			switch (index)
			{
			case 0:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Inventory", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Skills", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Social", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Map", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Crafting", new object[0]);
			case 5:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Collections", new object[0]);
			case 6:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Options", new object[0]);
			case 7:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Exit", new object[0]);
			default:
				return "";
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.pages[this.currentTab].receiveRightClick(x, y, true);
		}

		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			this.pages[this.currentTab].receiveScrollWheelAction(direction);
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.hoverText = "";
			this.pages[this.currentTab].performHoverAction(x, y);
			foreach (ClickableComponent current in this.tabs)
			{
				if (current.containsPoint(x, y))
				{
					this.hoverText = current.label;
					return;
				}
			}
			if (this.junimoNoteIcon != null)
			{
				this.junimoNoteIcon.tryHover(x, y, 0.1f);
				if (this.junimoNoteIcon.containsPoint(x, y))
				{
					this.hoverText = this.junimoNoteIcon.hoverText;
				}
			}
		}

		public int getTabNumberFromName(string name)
		{
			int result = -1;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 3454868101u)
			{
				if (num <= 3001865938u)
				{
					if (num != 1700191391u)
					{
						if (num == 3001865938u)
						{
							if (name == "social")
							{
								result = 2;
							}
						}
					}
					else if (name == "skills")
					{
						result = 1;
					}
				}
				else if (num != 3048072735u)
				{
					if (num == 3454868101u)
					{
						if (name == "exit")
						{
							result = 7;
						}
					}
				}
				else if (name == "crafting")
				{
					result = 4;
				}
			}
			else if (num <= 3760730054u)
			{
				if (num != 3751997361u)
				{
					if (num == 3760730054u)
					{
						if (name == "collections")
						{
							result = 5;
						}
					}
				}
				else if (name == "map")
				{
					result = 3;
				}
			}
			else if (num != 4012403877u)
			{
				if (num == 4244489279u)
				{
					if (name == "inventory")
					{
						result = 0;
					}
				}
			}
			else if (name == "options")
			{
				result = 6;
			}
			return result;
		}

		public override void releaseLeftClick(int x, int y)
		{
			base.releaseLeftClick(x, y);
			this.pages[this.currentTab].releaseLeftClick(x, y);
		}

		public override void leftClickHeld(int x, int y)
		{
			base.leftClickHeld(x, y);
			this.pages[this.currentTab].leftClickHeld(x, y);
		}

		public override bool readyToClose()
		{
			return !GameMenu.forcePreventClose && this.pages[this.currentTab].readyToClose();
		}

		public void changeTab(int whichTab)
		{
			if (this.currentTab == 2)
			{
				if (this.junimoNoteIcon != null)
				{
					this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false);
				}
			}
			else if (whichTab == 2 && this.junimoNoteIcon != null)
			{
				ClickableTextureComponent expr_AA_cp_0_cp_0 = this.junimoNoteIcon;
				expr_AA_cp_0_cp_0.bounds.X = expr_AA_cp_0_cp_0.bounds.X + Game1.tileSize;
			}
			this.currentTab = this.getTabNumberFromName(this.tabs[whichTab].name);
			if (this.currentTab == 3)
			{
				this.invisible = true;
				this.width += Game1.tileSize * 2;
				base.initializeUpperRightCloseButton();
			}
			else
			{
				this.width = 800 + IClickableMenu.borderWidth * 2;
				base.initializeUpperRightCloseButton();
				this.invisible = false;
			}
			Game1.playSound("smallSelect");
			if (Game1.options.SnappyMenus)
			{
				this.pages[this.currentTab].populateClickableComponentList();
				this.pages[this.currentTab].allClickableComponents.AddRange(this.tabs);
				this.setTabNeighborsForCurrentPage();
				this.snapToDefaultClickableComponent();
				if (whichTab == 2)
				{
					this.pages[this.currentTab].currentlySnappedComponent = this.tabs[2];
					this.snapCursorToCurrentSnappedComponent();
				}
				if (this.currentTab == 0 && this.junimoNoteIcon != null)
				{
					this.junimoNoteIcon.leftNeighborID = 11;
					this.junimoNoteIcon.downNeighborID = 105;
				}
			}
		}

		public void setTabNeighborsForCurrentPage()
		{
			int num = this.currentTab;
			if (num == 0)
			{
				for (int i = 0; i < this.tabs.Count; i++)
				{
					this.tabs[i].downNeighborID = i;
				}
				return;
			}
			if (num != 7)
			{
				for (int j = 0; j < this.tabs.Count; j++)
				{
					this.tabs[j].downNeighborID = -99999;
				}
				return;
			}
			for (int k = 0; k < this.tabs.Count; k++)
			{
				this.tabs[k].downNeighborID = 535;
			}
		}

		public override void draw(SpriteBatch b)
		{
			if (!this.invisible)
			{
				if (!Game1.options.showMenuBackground)
				{
					b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
				}
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.pages[this.currentTab].width, this.pages[this.currentTab].height, false, true, null, false);
				this.pages[this.currentTab].draw(b);
				b.End();
				b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
				if (!GameMenu.forcePreventClose)
				{
					foreach (ClickableComponent current in this.tabs)
					{
						int num = 0;
						string name = current.name;
						uint num2 = <PrivateImplementationDetails>.ComputeStringHash(name);
						if (num2 <= 3048072735u)
						{
							if (num2 <= 2237694710u)
							{
								if (num2 != 1700191391u)
								{
									if (num2 == 2237694710u)
									{
										if (name == "catalogue")
										{
											num = 7;
										}
									}
								}
								else if (name == "skills")
								{
									num = 1;
								}
							}
							else if (num2 != 3001865938u)
							{
								if (num2 == 3048072735u)
								{
									if (name == "crafting")
									{
										num = 4;
									}
								}
							}
							else if (name == "social")
							{
								num = 2;
							}
						}
						else if (num2 <= 3751997361u)
						{
							if (num2 != 3454868101u)
							{
								if (num2 == 3751997361u)
								{
									if (name == "map")
									{
										num = 3;
									}
								}
							}
							else if (name == "exit")
							{
								num = 7;
							}
						}
						else if (num2 != 3760730054u)
						{
							if (num2 != 4012403877u)
							{
								if (num2 == 4244489279u)
								{
									if (name == "inventory")
									{
										num = 0;
									}
								}
							}
							else if (name == "options")
							{
								num = 6;
							}
						}
						else if (name == "collections")
						{
							num = 5;
						}
						b.Draw(Game1.mouseCursors, new Vector2((float)current.bounds.X, (float)(current.bounds.Y + ((this.currentTab == this.getTabNumberFromName(current.name)) ? 8 : 0))), new Rectangle?(new Rectangle(num * 16, 368, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
						if (current.name.Equals("skills"))
						{
							Game1.player.FarmerRenderer.drawMiniPortrat(b, new Vector2((float)(current.bounds.X + 8), (float)(current.bounds.Y + 12 + ((this.currentTab == this.getTabNumberFromName(current.name)) ? 8 : 0))), 0.00011f, 3f, 2, Game1.player);
						}
					}
					b.End();
					b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
					if (this.junimoNoteIcon != null)
					{
						this.junimoNoteIcon.draw(b);
					}
					if (!this.hoverText.Equals(""))
					{
						IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
					}
				}
			}
			else
			{
				this.pages[this.currentTab].draw(b);
			}
			if (!GameMenu.forcePreventClose)
			{
				base.draw(b);
			}
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		public override bool areGamePadControlsImplemented()
		{
			return false;
		}

		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.menuButton.Contains(new InputButton(key)) && this.readyToClose())
			{
				Game1.exitActiveMenu();
				Game1.playSound("bigDeSelect");
			}
			this.pages[this.currentTab].receiveKeyPress(key);
		}
	}
}
