using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley
{
	public class Polygon
	{
		public class Line
		{
			public Vector2 Start;

			public Vector2 End;

			public Line(Vector2 Start, Vector2 End)
			{
				this.Start = Start;
				this.End = End;
			}
		}

		public List<Polygon.Line> lines = new List<Polygon.Line>();

		public List<Polygon.Line> Lines
		{
			get
			{
				return this.lines;
			}
			set
			{
				this.lines = value;
			}
		}

		public void addPoint(Vector2 point)
		{
			if (this.lines.Count > 0)
			{
				this.lines.Add(new Polygon.Line(this.Lines[this.Lines.Count - 1].End, point));
			}
		}

		public bool containsPoint(Vector2 point)
		{
			using (List<Polygon.Line>.Enumerator enumerator = this.Lines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.End.Equals(point))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static Polygon getGentlerBorderForLakes(Rectangle room, Random mineRandom)
		{
			return Polygon.getGentlerBorderForLakes(room, mineRandom, Rectangle.Empty);
		}

		public static Polygon getEdgeBorder(Rectangle room, Random mineRandom)
		{
			return Polygon.getEdgeBorder(room, mineRandom, new List<Rectangle>(), (room.Width - 2) / 2, (room.Height - 2) / 2);
		}

		public static Polygon getEdgeBorder(Rectangle room, Random mineRandom, List<Rectangle> smoothZone)
		{
			return Polygon.getEdgeBorder(room, mineRandom, smoothZone, (room.Width - 2) / 2, (room.Height - 2) / 2);
		}

		public static Polygon getEdgeBorder(Rectangle room, Random mineRandom, List<Rectangle> smoothZone, int horizontalInwardLimit, int verticalInwardLimit)
		{
			if (smoothZone == null)
			{
				smoothZone = new List<Rectangle>();
			}
			int num = room.Width - 2;
			int num2 = room.Height - 2;
			int num3 = room.X + 1;
			int num4 = room.Y + 1;
			new Rectangle(num3, num4, num, num2);
			Polygon polygon = new Polygon();
			Vector2 vector = new Vector2((float)mineRandom.Next(num3 + 5, num3 + 8), (float)mineRandom.Next(num4 + 5, num4 + 8));
			polygon.Lines.Add(new Polygon.Line(vector, new Vector2(vector.X + 1f, vector.Y)));
			vector.X += 1f;
			int num5 = num - 12;
			List<int> list = new List<int>
			{
				2,
				2,
				2
			};
			int i = 0;
			while (i < num5)
			{
				int num6 = mineRandom.Next(3);
				if (list.Last<int>() != num6 && list[list.Count - 2] != list.Last<int>())
				{
					num6 = list.Last<int>();
				}
				if (num6 == 0 && vector.Y > (float)num4 && !list.Contains(1))
				{
					vector.Y -= 1f;
					list.Add(0);
				}
				else if (num6 == 1 && vector.Y < (float)(num4 + verticalInwardLimit) && !list.Contains(0))
				{
					vector.Y += 1f;
					list.Add(1);
				}
				else
				{
					vector.X += 1f;
					i++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			int num7 = num2 - 4 - (int)(vector.Y - (float)room.Y);
			vector.Y += 1f;
			list = new List<int>
			{
				2,
				2,
				2
			};
			polygon.addPoint(vector);
			int j = 0;
			while (j < num7)
			{
				int num8 = mineRandom.Next(3);
				if (list.Last<int>() != num8 && list[list.Count - 2] != list.Last<int>())
				{
					num8 = list.Last<int>();
				}
				if (j > 4 && num8 == 0 && vector.X < (float)(num3 + num - 1) && !list.Contains(1) && !Utility.pointInRectangles(smoothZone, (int)vector.X, (int)vector.Y))
				{
					vector.X += 1f;
					list.Add(0);
				}
				else if (j > 4 && num8 == 1 && vector.X > (float)(num3 + num - horizontalInwardLimit + 1) && !list.Contains(0) && !Utility.pointInRectangles(smoothZone, (int)vector.X, (int)vector.Y))
				{
					vector.X -= 1f;
					list.Add(1);
				}
				else
				{
					vector.Y += 1f;
					j++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			int num9 = (int)vector.X - (int)polygon.Lines[0].Start.X + 1;
			vector.X -= 1f;
			list = new List<int>
			{
				2,
				2,
				2
			};
			polygon.addPoint(vector);
			int k = 0;
			while (k < num9)
			{
				int num10 = mineRandom.Next(3);
				if (list.Last<int>() != num10 && list[list.Count - 2] != list.Last<int>())
				{
					num10 = list.Last<int>();
				}
				if (k > 4 && num10 == 0 && vector.Y > (float)(num4 + num2 - verticalInwardLimit) && !list.Contains(1) && !polygon.containsPoint(new Vector2(vector.X, vector.Y - 1f)))
				{
					vector.Y -= 1f;
					list.Add(0);
				}
				else if (k > 4 && num10 == 1 && vector.Y < (float)(num4 + num2) && !list.Contains(0))
				{
					vector.Y += 1f;
					list.Add(1);
				}
				else
				{
					vector.X -= 1f;
					k++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			int num11 = (int)vector.Y - (int)polygon.Lines[0].Start.Y - 1;
			vector.Y -= 1f;
			list = new List<int>
			{
				2,
				2,
				2
			};
			polygon.addPoint(vector);
			int l = 0;
			while (l < num11)
			{
				int num12 = mineRandom.Next(3);
				if (list.Last<int>() != num12 && list[list.Count - 2] != list.Last<int>())
				{
					num12 = list.Last<int>();
				}
				if (l > 4 && num12 == 0 && vector.X < (float)((int)polygon.Lines[0].Start.X) && !list.Contains(1) && !polygon.containsPoint(new Vector2(vector.X + 1f, vector.Y)) && !polygon.containsPoint(new Vector2(vector.X, vector.Y - 1f)) && !Utility.pointInRectangles(smoothZone, (int)vector.X, (int)vector.Y))
				{
					vector.X += 1f;
					list.Add(0);
				}
				else if (l > 4 && num12 == 1 && vector.X > (float)(num3 + 1) && !list.Contains(0) && !Utility.pointInRectangles(smoothZone, (int)vector.X, (int)vector.Y))
				{
					vector.X -= 1f;
					list.Add(1);
				}
				else
				{
					vector.Y -= 1f;
					l++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			if (vector.X < (float)((int)polygon.Lines[0].Start.X))
			{
				int num13 = (int)polygon.Lines[0].Start.X + 1 - (int)vector.X - 1;
				for (int m = 0; m < num13; m++)
				{
					vector.X += 1f;
					polygon.addPoint(vector);
				}
			}
			return polygon;
		}

		public static Polygon getGentlerBorderForLakes(Rectangle room, Random mineRandom, Rectangle smoothZone)
		{
			int num = room.Width - 2;
			int num2 = room.Height - 2;
			int num3 = room.X + 1;
			int num4 = room.Y + 1;
			new Rectangle(num3, num4, num, num2);
			Polygon polygon = new Polygon();
			Vector2 vector = new Vector2((float)mineRandom.Next(num3 + 5, num3 + 8), (float)mineRandom.Next(num4 + 5, num4 + 8));
			polygon.Lines.Add(new Polygon.Line(vector, new Vector2(vector.X + 1f, vector.Y)));
			vector.X += 1f;
			int num5 = num - 12;
			List<int> list = new List<int>
			{
				2,
				2,
				2
			};
			int i = 0;
			while (i < num5)
			{
				int num6 = mineRandom.Next(3);
				if (num6 == 0 && vector.Y > (float)num4 && !list.Contains(1) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.Y -= 1f;
					list.Add(0);
				}
				else if (num6 == 1 && vector.Y < (float)(num4 + num2 / 2) && !list.Contains(0) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.Y += 1f;
					list.Add(1);
				}
				else
				{
					vector.X += 1f;
					i++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			int num7 = num2 - 4 - (int)(vector.Y - (float)room.Y);
			vector.Y += 1f;
			list = new List<int>
			{
				2,
				2,
				2
			};
			polygon.addPoint(vector);
			int j = 0;
			while (j < num7)
			{
				int num8 = mineRandom.Next(3);
				if (num8 == 0 && vector.X < (float)(num3 + num) && !list.Contains(1) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.X += 1f;
					list.Add(0);
				}
				else if (num8 == 1 && vector.X > (float)(num3 + num / 2 + 1) && !list.Contains(0) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.X -= 1f;
					list.Add(1);
				}
				else
				{
					vector.Y += 1f;
					j++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			int num9 = (int)vector.X - (int)polygon.Lines[0].Start.X + 1;
			vector.X -= 1f;
			list = new List<int>
			{
				2,
				2,
				2
			};
			polygon.addPoint(vector);
			int k = 0;
			while (k < num9)
			{
				int num10 = mineRandom.Next(3);
				if (num10 == 0 && vector.Y > (float)(num4 + num2 / 2) && !list.Contains(1) && !polygon.containsPoint(new Vector2(vector.X, vector.Y - 1f)) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.Y -= 1f;
					list.Add(0);
				}
				else if (num10 == 1 && vector.Y < (float)(num4 + num2) && !list.Contains(0) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.Y += 1f;
					list.Add(1);
				}
				else
				{
					vector.X -= 1f;
					k++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			int num11 = (int)vector.Y - (int)polygon.Lines[0].Start.Y - 1;
			vector.Y -= 1f;
			list = new List<int>
			{
				2,
				2,
				2
			};
			polygon.addPoint(vector);
			int l = 0;
			while (l < num11)
			{
				int num12 = mineRandom.Next(3);
				if (num12 == 0 && vector.X < (float)((int)polygon.Lines[0].Start.X) && !list.Contains(1) && !polygon.containsPoint(new Vector2(vector.X + 1f, vector.Y)) && !polygon.containsPoint(new Vector2(vector.X, vector.Y - 1f)) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.X += 1f;
					list.Add(0);
				}
				else if (num12 == 1 && vector.X > (float)(num3 + 1) && !list.Contains(0) && !smoothZone.Contains((int)vector.X, (int)vector.Y))
				{
					vector.X -= 1f;
					list.Add(1);
				}
				else
				{
					vector.Y -= 1f;
					l++;
					list.Add(2);
				}
				list.RemoveAt(0);
				polygon.addPoint(vector);
			}
			if (vector.X < (float)((int)polygon.Lines[0].Start.X))
			{
				int num13 = (int)polygon.Lines[0].Start.X + 1 - (int)vector.X - 1;
				for (int m = 0; m < num13; m++)
				{
					vector.X += 1f;
					polygon.addPoint(vector);
				}
			}
			return polygon;
		}

		public static Polygon getRandomBorderRoom(Rectangle room, Random mineRandom)
		{
			int num = room.Width - 2;
			int num2 = room.Height - 2;
			int num3 = room.X + 1;
			int num4 = room.Y + 1;
			new Rectangle(num3, num4, num, num2);
			Polygon polygon = new Polygon();
			Vector2 vector = new Vector2((float)mineRandom.Next(num3 + 5, num3 + 8), (float)mineRandom.Next(num4 + 5, num4 + 8));
			polygon.Lines.Add(new Polygon.Line(vector, new Vector2(vector.X + 1f, vector.Y)));
			vector.X += 1f;
			int num5 = room.Right - (int)vector.X - num / 8;
			int num6 = 2;
			int i = 0;
			while (i < num5)
			{
				int num7 = mineRandom.Next(3);
				if ((num7 == 0 && vector.Y > (float)room.Y && num6 != 1) || (num6 == 2 && vector.Y >= (float)(num4 + num2 / 2)))
				{
					vector.Y -= 1f;
					num6 = 0;
				}
				else if ((num7 == 1 && vector.Y < (float)(num4 + num2 / 2) && num6 != 0) || (num6 == 2 && vector.Y <= (float)room.Y))
				{
					vector.Y += 1f;
					num6 = 1;
				}
				else
				{
					vector.X += 1f;
					i++;
					num6 = 2;
				}
				polygon.addPoint(vector);
			}
			int num8 = num2 - 4 - (int)(vector.Y - (float)room.Y);
			vector.Y += 1f;
			num6 = 2;
			polygon.addPoint(vector);
			int j = 0;
			while (j < num8)
			{
				int num9 = mineRandom.Next(3);
				if ((num9 == 0 && vector.X < (float)room.Right && num6 != 1) || (num6 == 2 && vector.X <= (float)(num3 + num / 2 + 1)))
				{
					vector.X += 1f;
					num6 = 0;
				}
				else if ((num9 == 1 && vector.X > (float)(num3 + num / 2 + 1) && num6 != 0) || (num6 == 2 && vector.X >= (float)room.Right))
				{
					vector.X -= 1f;
					num6 = 1;
				}
				else
				{
					vector.Y += 1f;
					j++;
					num6 = 2;
				}
				polygon.addPoint(vector);
			}
			int num10 = (int)vector.X - (int)polygon.Lines[0].Start.X + num / 4;
			vector.X -= 1f;
			num6 = 2;
			polygon.addPoint(vector);
			int k = 0;
			while (k < num10)
			{
				int num11 = mineRandom.Next(3);
				if ((num11 == 0 && vector.Y > (float)(num4 + num2 / 2) && num6 != 1 && !polygon.containsPoint(new Vector2(vector.X, vector.Y - 1f))) || (num6 == 2 && vector.Y >= (float)room.Bottom))
				{
					vector.Y -= 1f;
					num6 = 0;
				}
				else if ((num11 == 1 && vector.Y < (float)room.Bottom && num6 != 0) || (num6 == 2 && vector.Y <= (float)(num4 + num2 / 2)))
				{
					vector.Y += 1f;
					num6 = 1;
				}
				else
				{
					vector.X -= 1f;
					k++;
					num6 = 2;
				}
				polygon.addPoint(vector);
			}
			int num12 = (int)vector.Y - (int)polygon.Lines[0].Start.Y - 1;
			vector.Y -= 1f;
			num6 = 2;
			polygon.addPoint(vector);
			int l = 0;
			while (l < num12)
			{
				int num13 = mineRandom.Next(3);
				if ((num13 == 0 && vector.X < (float)room.Center.X && !polygon.containsPoint(new Vector2(vector.X + 1f, vector.Y)) && !polygon.containsPoint(new Vector2(vector.X, vector.Y - 1f))) || (num6 == 2 && vector.X <= (float)room.X))
				{
					vector.X += 1f;
					num6 = 0;
				}
				else if ((num13 == 1 && vector.X > (float)room.X && num6 != 0) || (num6 == 2 && vector.X >= (float)room.Center.X))
				{
					vector.X -= 1f;
					num6 = 1;
				}
				else
				{
					vector.Y -= 1f;
					l++;
					num6 = 2;
				}
				polygon.addPoint(vector);
			}
			if (vector.X < (float)((int)polygon.Lines[0].Start.X))
			{
				int num14 = (int)polygon.Lines[0].Start.X + 1 - (int)vector.X - 1;
				for (int m = 0; m < num14; m++)
				{
					vector.X += 1f;
					polygon.addPoint(vector);
				}
			}
			return polygon;
		}
	}
}
