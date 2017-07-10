using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class PriorityQueue
	{
		private int total_size;

		private SortedDictionary<int, Queue<PathNode>> nodes;

		public PriorityQueue()
		{
			this.nodes = new SortedDictionary<int, Queue<PathNode>>();
			this.total_size = 0;
		}

		public bool IsEmpty()
		{
			return this.total_size == 0;
		}

		public bool Contains(PathNode p, int priority)
		{
			return this.nodes.ContainsKey(priority) && this.nodes[priority].Contains(p);
		}

		public PathNode Dequeue()
		{
			if (!this.IsEmpty())
			{
				foreach (Queue<PathNode> current in this.nodes.Values)
				{
					if (current.Count > 0)
					{
						this.total_size--;
						return current.Dequeue();
					}
				}
			}
			return null;
		}

		public object Peek()
		{
			if (!this.IsEmpty())
			{
				foreach (Queue<PathNode> current in this.nodes.Values)
				{
					if (current.Count > 0)
					{
						return current.Peek();
					}
				}
			}
			return null;
		}

		public object Dequeue(int priority)
		{
			this.total_size--;
			return this.nodes[priority].Dequeue();
		}

		public void Enqueue(PathNode item, int priority)
		{
			if (!this.nodes.ContainsKey(priority))
			{
				this.nodes.Add(priority, new Queue<PathNode>());
				this.Enqueue(item, priority);
				return;
			}
			this.nodes[priority].Enqueue(item);
			this.total_size++;
		}
	}
}
