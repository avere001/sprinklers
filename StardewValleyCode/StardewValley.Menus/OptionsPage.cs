using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class OptionsPage : IClickableMenu
	{
		public const int itemsPerPage = 7;

		public const int indexOfGraphicsPage = 6;

		private string descriptionText = "";

		private string hoverText = "";

		public List<ClickableComponent> optionSlots = new List<ClickableComponent>();

		public int currentItemIndex;

		private ClickableTextureComponent upArrow;

		private ClickableTextureComponent downArrow;

		private ClickableTextureComponent scrollBar;

		private bool scrolling;

		private List<OptionsElement> options = new List<OptionsElement>();

		private Rectangle scrollBarRunner;

		private int optionsSlotHeld = -1;

		public OptionsPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upArrow.bounds.Height - Game1.pixelZoom * 2);
			for (int i = 0; i < 7; i++)
			{
				this.optionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((height - Game1.tileSize * 2) / 7), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom), string.Concat(i))
				{
					myID = i,
					downNeighborID = ((i < 6) ? (i + 1) : -7777),
					upNeighborID = ((i > 0) ? (i - 1) : -7777),
					fullyImmutable = true
				});
			}
			this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11233", new object[0])));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11234", new object[0]), 0, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11235", new object[0]), 7, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11236", new object[0]), 8, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11237", new object[0]), 11, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11238", new object[0]), 12, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11239", new object[0]), 27, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11240", new object[0]), 14, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\UI:Options_GamepadStyleMenus", new object[0]), 29, -1, -1));
			this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11241", new object[0])));
			this.options.Add(new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11242", new object[0]), 1, -1, -1));
			this.options.Add(new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11243", new object[0]), 2, -1, -1));
			this.options.Add(new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11244", new object[0]), 20, -1, -1));
			this.options.Add(new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11245", new object[0]), 21, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11246", new object[0]), 3, -1, -1));
			this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11247", new object[0])));
			if (!Game1.conventionMode)
			{
				this.options.Add(new OptionsDropDown(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11248", new object[0]), 13, -1, -1));
				this.options.Add(new OptionsDropDown(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11251", new object[0]), 6, -1, -1));
			}
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11252", new object[0]), 9, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11253", new object[0]), 15, -1, -1));
			this.options.Add(new OptionsPlusMinus(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11254", new object[0]), 18, new List<string>
			{
				"75%",
				"80%",
				"85%",
				"90%",
				"95%",
				"100%",
				"105%",
				"110%",
				"115%",
				"120%",
				"125%"
			}, new List<string>
			{
				"75%",
				"80%",
				"85%",
				"90%",
				"95%",
				"100%",
				"105%",
				"110%",
				"115%",
				"120%",
				"125%"
			}, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11266", new object[0]), 19, -1, -1));
			this.options.Add(new OptionsPlusMinus(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11267", new object[0]), 25, new List<string>
			{
				"Low",
				"Med.",
				"High"
			}, new List<string>
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11268", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11269", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11270", new object[0])
			}, -1, -1));
			this.options.Add(new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11271", new object[0]), 23, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11272", new object[0]), 24, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11273", new object[0]), 26, -1, -1));
			this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11274", new object[0])));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11275", new object[0]), 16, -1, -1));
			this.options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11276", new object[0]), 22, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11277", new object[0]), -1, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11278", new object[0]), 7, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11279", new object[0]), 10, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11280", new object[0]), 15, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11281", new object[0]), 18, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11282", new object[0]), 19, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11283", new object[0]), 11, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11284", new object[0]), 14, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11285", new object[0]), 13, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11286", new object[0]), 12, this.optionSlots[0].bounds.Width, -1, -1));
			if (Game1.IsMultiplayer)
			{
				this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11287", new object[0]), 17, this.optionSlots[0].bounds.Width, -1, -1));
			}
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11288", new object[0]), 16, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11289", new object[0]), 20, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11290", new object[0]), 21, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11291", new object[0]), 22, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11292", new object[0]), 23, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11293", new object[0]), 24, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11294", new object[0]), 25, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11295", new object[0]), 26, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11296", new object[0]), 27, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11297", new object[0]), 28, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11298", new object[0]), 29, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11299", new object[0]), 30, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11300", new object[0]), 31, this.optionSlots[0].bounds.Width, -1, -1));
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentItemIndex = 0;
			base.snapToDefaultClickableComponent();
			this.currentlySnappedComponent = base.getComponentWithID(1);
			this.snapCursorToCurrentSnappedComponent();
		}

		protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
			base.customSnapBehavior(direction, oldRegion, oldID);
			if (oldID == 6 && direction == 2 && this.currentItemIndex < Math.Max(0, this.options.Count - 7))
			{
				this.downArrowPressed();
				Game1.playSound("shiny4");
				return;
			}
			if (oldID == 0 && direction == 0)
			{
				if (this.currentItemIndex > 0)
				{
					this.upArrowPressed();
					Game1.playSound("shiny4");
					return;
				}
				this.currentlySnappedComponent = base.getComponentWithID(12346);
				if (this.currentlySnappedComponent != null)
				{
					this.currentlySnappedComponent.downNeighborID = 0;
				}
				this.snapCursorToCurrentSnappedComponent();
			}
		}

		private void setScrollBarToCurrentIndex()
		{
			if (this.options.Count > 0)
			{
				this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.options.Count - 7 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
				if (this.currentItemIndex == this.options.Count - 7)
				{
					this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
				}
			}
		}

		public override void snapCursorToCurrentSnappedComponent()
		{
			if (this.currentlySnappedComponent == null || this.currentlySnappedComponent.myID >= this.options.Count)
			{
				if (this.currentlySnappedComponent != null)
				{
					base.snapCursorToCurrentSnappedComponent();
				}
				return;
			}
			if (this.options[this.currentlySnappedComponent.myID + this.currentItemIndex] is OptionsInputListener)
			{
				Game1.setMousePosition(this.currentlySnappedComponent.bounds.Right - Game1.tileSize * 3 / 4, this.currentlySnappedComponent.bounds.Center.Y - Game1.pixelZoom * 3);
				return;
			}
			Game1.setMousePosition(this.currentlySnappedComponent.bounds.Left + Game1.tileSize * 3 / 4, this.currentlySnappedComponent.bounds.Center.Y - Game1.pixelZoom * 3);
		}

		public override void leftClickHeld(int x, int y)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			base.leftClickHeld(x, y);
			if (this.scrolling)
			{
				int arg_F0_0 = this.scrollBar.bounds.Y;
				this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
				float num = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
				this.currentItemIndex = Math.Min(this.options.Count - 7, Math.Max(0, (int)((float)this.options.Count * num)));
				this.setScrollBarToCurrentIndex();
				if (arg_F0_0 != this.scrollBar.bounds.Y)
				{
					Game1.playSound("shiny4");
					return;
				}
			}
			else if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count)
			{
				this.options[this.currentItemIndex + this.optionsSlotHeld].leftClickHeld(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
			}
		}

		public override ClickableComponent getCurrentlySnappedComponent()
		{
			return this.currentlySnappedComponent;
		}

		public override void setCurrentlySnappedComponentTo(int id)
		{
			this.currentlySnappedComponent = base.getComponentWithID(id);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void receiveKeyPress(Keys key)
		{
			if ((this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count) || (Game1.options.snappyMenus && Game1.options.gamepadControls))
			{
				if (this.currentlySnappedComponent != null && Game1.options.snappyMenus && Game1.options.gamepadControls && this.options.Count > this.currentItemIndex + this.currentlySnappedComponent.myID && this.currentItemIndex + this.currentlySnappedComponent.myID >= 0)
				{
					this.options[this.currentItemIndex + this.currentlySnappedComponent.myID].receiveKeyPress(key);
				}
				else if (this.options.Count > this.currentItemIndex + this.optionsSlotHeld && this.currentItemIndex + this.optionsSlotHeld >= 0)
				{
					this.options[this.currentItemIndex + this.optionsSlotHeld].receiveKeyPress(key);
				}
			}
			base.receiveKeyPress(key);
		}

		public override void receiveScrollWheelAction(int direction)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shiny4");
				return;
			}
			if (direction < 0 && this.currentItemIndex < Math.Max(0, this.options.Count - 7))
			{
				this.downArrowPressed();
				Game1.playSound("shiny4");
			}
		}

		public override void releaseLeftClick(int x, int y)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			base.releaseLeftClick(x, y);
			if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count)
			{
				this.options[this.currentItemIndex + this.optionsSlotHeld].leftClickReleased(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
			}
			this.optionsSlotHeld = -1;
			this.scrolling = false;
		}

		private void downArrowPressed()
		{
			this.downArrow.scale = this.downArrow.baseScale;
			this.currentItemIndex++;
			this.setScrollBarToCurrentIndex();
		}

		private void upArrowPressed()
		{
			this.upArrow.scale = this.upArrow.baseScale;
			this.currentItemIndex--;
			this.setScrollBarToCurrentIndex();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.options.Count - 7))
			{
				this.downArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.scrollBar.containsPoint(x, y))
			{
				this.scrolling = true;
			}
			else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen && y < this.yPositionOnScreen + this.height)
			{
				this.scrolling = true;
				this.leftClickHeld(x, y);
				this.releaseLeftClick(x, y);
			}
			this.currentItemIndex = Math.Max(0, Math.Min(this.options.Count - 7, this.currentItemIndex));
			for (int i = 0; i < this.optionSlots.Count; i++)
			{
				if (this.optionSlots[i].bounds.Contains(x, y) && this.currentItemIndex + i < this.options.Count && this.options[this.currentItemIndex + i].bounds.Contains(x - this.optionSlots[i].bounds.X, y - this.optionSlots[i].bounds.Y))
				{
					this.options[this.currentItemIndex + i].receiveLeftClick(x - this.optionSlots[i].bounds.X, y - this.optionSlots[i].bounds.Y);
					this.optionsSlotHeld = i;
					return;
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			for (int i = 0; i < this.optionSlots.Count; i++)
			{
				if (this.currentItemIndex >= 0 && this.currentItemIndex + i < this.options.Count && this.options[this.currentItemIndex + i].bounds.Contains(x - this.optionSlots[i].bounds.X, y - this.optionSlots[i].bounds.Y))
				{
					Game1.SetFreeCursorDrag();
					break;
				}
			}
			if (this.scrollBarRunner.Contains(x, y))
			{
				Game1.SetFreeCursorDrag();
			}
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			this.descriptionText = "";
			this.hoverText = "";
			this.upArrow.tryHover(x, y, 0.1f);
			this.downArrow.tryHover(x, y, 0.1f);
			this.scrollBar.tryHover(x, y, 0.1f);
			bool arg_FB_0 = this.scrolling;
		}

		public override void draw(SpriteBatch b)
		{
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
			for (int i = 0; i < this.optionSlots.Count; i++)
			{
				if (this.currentItemIndex >= 0 && this.currentItemIndex + i < this.options.Count)
				{
					this.options[this.currentItemIndex + i].draw(b, this.optionSlots[i].bounds.X, this.optionSlots[i].bounds.Y);
				}
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (!GameMenu.forcePreventClose)
			{
				this.upArrow.draw(b);
				this.downArrow.draw(b);
				if (this.options.Count > 7)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, false);
					this.scrollBar.draw(b);
				}
			}
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
