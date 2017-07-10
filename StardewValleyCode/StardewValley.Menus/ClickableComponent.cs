using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Menus
{
	public class ClickableComponent
	{
		public const int ID_ignore = -500;

		public const int CUSTOM_SNAP_BEHAVIOR = -7777;

		public const int SNAP_TO_DEFAULT = -99999;

		public Rectangle bounds;

		public string name;

		public string label;

		public float scale = 1f;

		public Item item;

		public bool visible = true;

		public bool leftNeighborImmutable;

		public bool rightNeighborImmutable;

		public bool upNeighborImmutable;

		public bool downNeighborImmutable;

		public bool tryDefaultIfNoDownNeighborExists;

		public bool tryDefaultIfNoRightNeighborExists;

		public bool fullyImmutable;

		public int myID = -500;

		public int leftNeighborID = -1;

		public int rightNeighborID = -1;

		public int upNeighborID = -1;

		public int downNeighborID = -1;

		public int myAlternateID = -500;

		public int region;

		public ClickableComponent(Rectangle bounds, string name)
		{
			this.bounds = bounds;
			this.name = name;
		}

		public ClickableComponent(Rectangle bounds, string name, string label)
		{
			this.bounds = bounds;
			this.name = name;
			this.label = label;
		}

		public ClickableComponent(Rectangle bounds, Item item)
		{
			this.bounds = bounds;
			this.item = item;
		}

		public virtual bool containsPoint(int x, int y)
		{
			if (!this.visible)
			{
				return false;
			}
			if (this.bounds.Contains(x, y))
			{
				Game1.SetFreeCursorDrag();
				return true;
			}
			return false;
		}

		public virtual void snapMouseCursor()
		{
			Game1.setMousePosition(this.bounds.Right - this.bounds.Width / 8, this.bounds.Bottom - this.bounds.Height / 8);
		}

		public void snapMouseCursorToCenter()
		{
			Game1.setMousePosition(this.bounds.Center.X, this.bounds.Center.Y);
		}
	}
}
