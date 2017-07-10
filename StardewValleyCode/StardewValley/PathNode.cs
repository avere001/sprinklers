using System;

namespace StardewValley
{
	public class PathNode
	{
		public int x;

		public int y;

		public byte g;

		public PathNode parent;

		public PathNode(int x, int y, PathNode parent)
		{
			this.x = x;
			this.y = y;
			this.parent = parent;
		}

		public PathNode(int x, int y, byte g, PathNode parent)
		{
			this.x = x;
			this.y = y;
			this.g = g;
			this.parent = parent;
		}

		public override bool Equals(object obj)
		{
			return this.x == ((PathNode)obj).x && this.y == ((PathNode)obj).y;
		}

		public override int GetHashCode()
		{
			return 100000 * this.x + this.y;
		}
	}
}
