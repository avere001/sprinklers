using Microsoft.Xna.Framework;
using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class PathFindController
	{
		public delegate bool isAtEnd(PathNode currentNode, Point endPoint, GameLocation location, Character c);

		public delegate void endBehavior(Character c, GameLocation location);

		public const byte impassable = 255;

		public const int timeToWaitBeforeCancelling = 5000;

		private Character character;

		public GameLocation location;

		public Stack<Point> pathToEndPoint;

		public Point endPoint;

		public int finalFacingDirection;

		public int pausedTimer;

		public int limit;

		private PathFindController.isAtEnd endFunction;

		public PathFindController.endBehavior endBehaviorFunction;

		public bool NPCSchedule;

		public int timerSinceLastCheckPoint;

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, null, 10000, endPoint)
		{
		}

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, PathFindController.endBehavior endBehaviorFunction) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, null, 10000, endPoint)
		{
			this.endPoint = endPoint;
			this.endBehaviorFunction = endBehaviorFunction;
		}

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, PathFindController.endBehavior endBehaviorFunction, int limit) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, null, limit, endPoint)
		{
			this.endPoint = endPoint;
			this.endBehaviorFunction = endBehaviorFunction;
		}

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, bool eraseOldPathController) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, eraseOldPathController, null, 10000, endPoint)
		{
		}

		public static bool isAtEndPoint(PathNode currentNode, Point endPoint, GameLocation location, Character c)
		{
			return currentNode.x == endPoint.X && currentNode.y == endPoint.Y;
		}

		public PathFindController(Stack<Point> pathToEndPoint, GameLocation location, Character c, Point endPoint)
		{
			this.pathToEndPoint = pathToEndPoint;
			this.location = location;
			this.character = c;
			this.endPoint = endPoint;
		}

		public PathFindController(Stack<Point> pathToEndPoint, Character c, GameLocation l)
		{
			this.pathToEndPoint = pathToEndPoint;
			this.character = c;
			this.location = l;
			this.NPCSchedule = true;
		}

		public PathFindController(Character c, GameLocation location, PathFindController.isAtEnd endFunction, int finalFacingDirection, bool eraseOldPathController, PathFindController.endBehavior endBehaviorFunction, int limit, Point endPoint)
		{
			this.limit = limit;
			this.character = c;
			if (c is NPC && (c as NPC).CurrentDialogue.Count > 0 && (c as NPC).CurrentDialogue.Peek().removeOnNextMove)
			{
				(c as NPC).CurrentDialogue.Pop();
			}
			this.location = location;
			this.endFunction = ((endFunction == null) ? new PathFindController.isAtEnd(PathFindController.isAtEndPoint) : endFunction);
			this.endBehaviorFunction = endBehaviorFunction;
			if (endPoint == Point.Zero)
			{
				endPoint = new Point((int)c.getTileLocation().X, (int)c.getTileLocation().Y);
			}
			this.finalFacingDirection = finalFacingDirection;
			if (!(this.character is NPC) && !Game1.currentLocation.Name.Equals(location.Name) && endFunction == new PathFindController.isAtEnd(PathFindController.isAtEndPoint) && endPoint.X > 0 && endPoint.Y > 0)
			{
				this.character.position = new Vector2((float)(endPoint.X * Game1.tileSize), (float)(endPoint.Y * Game1.tileSize - Game1.tileSize / 2));
				return;
			}
			this.pathToEndPoint = PathFindController.findPath(new Point((int)c.getTileLocation().X, (int)c.getTileLocation().Y), endPoint, endFunction, location, this.character, limit);
			if (this.pathToEndPoint == null)
			{
				FarmHouse arg_177_0 = location as FarmHouse;
			}
		}

		public bool update(GameTime time)
		{
			if (this.pathToEndPoint == null || this.pathToEndPoint.Count == 0)
			{
				return true;
			}
			if (!this.NPCSchedule && !Game1.currentLocation.Name.Equals(this.location.Name) && this.endPoint.X > 0 && this.endPoint.Y > 0)
			{
				this.character.position = new Vector2((float)(this.endPoint.X * Game1.tileSize), (float)(this.endPoint.Y * Game1.tileSize - Game1.tileSize / 2));
				return true;
			}
			if (Game1.activeClickableMenu == null)
			{
				this.timerSinceLastCheckPoint += time.ElapsedGameTime.Milliseconds;
				Vector2 position = this.character.position;
				this.moveCharacter(time);
				if (this.character.position.Equals(position))
				{
					this.pausedTimer += time.ElapsedGameTime.Milliseconds;
				}
				else
				{
					this.pausedTimer = 0;
				}
				if (!this.NPCSchedule && this.pausedTimer > 5000)
				{
					return true;
				}
			}
			return false;
		}

		public static Stack<Point> findPath(Point startPoint, Point endPoint, PathFindController.isAtEnd endPointFunction, GameLocation location, Character character, int limit)
		{
			sbyte[,] array = new sbyte[,]
			{
				{
					-1,
					0
				},
				{
					1,
					0
				},
				{
					0,
					1
				},
				{
					0,
					-1
				}
			};
			PriorityQueue priorityQueue = new PriorityQueue();
			Dictionary<PathNode, PathNode> dictionary = new Dictionary<PathNode, PathNode>();
			int num = 0;
			priorityQueue.Enqueue(new PathNode(startPoint.X, startPoint.Y, 0, null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
			while (!priorityQueue.IsEmpty())
			{
				PathNode pathNode = priorityQueue.Dequeue();
				if (endPointFunction(pathNode, endPoint, location, character))
				{
					return PathFindController.reconstructPath(pathNode, dictionary);
				}
				if (!dictionary.ContainsKey(pathNode))
				{
					dictionary.Add(pathNode, pathNode.parent);
				}
				for (int i = 0; i < 4; i++)
				{
					PathNode pathNode2 = new PathNode(pathNode.x + (int)array[i, 0], pathNode.y + (int)array[i, 1], pathNode);
					pathNode2.g = pathNode.g + 1;
					if (!dictionary.ContainsKey(pathNode2) && ((pathNode2.x == endPoint.X && pathNode2.y == endPoint.Y) || (pathNode2.x >= 0 && pathNode2.y >= 0 && pathNode2.x < location.map.Layers[0].LayerWidth && pathNode2.y < location.map.Layers[0].LayerHeight)) && !location.isCollidingPosition(new Rectangle(pathNode2.x * Game1.tileSize + 1, pathNode2.y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2), Game1.viewport, false, 0, false, character, true, false, false))
					{
						int priority = (int)pathNode2.g + (Math.Abs(endPoint.X - pathNode2.x) + Math.Abs(endPoint.Y - pathNode2.y));
						if (!priorityQueue.Contains(pathNode2, priority))
						{
							priorityQueue.Enqueue(pathNode2, priority);
						}
					}
				}
				num++;
				if (num >= limit)
				{
					return null;
				}
			}
			return null;
		}

		public static Stack<Point> reconstructPath(PathNode finalNode, Dictionary<PathNode, PathNode> closedList)
		{
			Stack<Point> stack = new Stack<Point>();
			stack.Push(new Point(finalNode.x, finalNode.y));
			for (PathNode pathNode = finalNode.parent; pathNode != null; pathNode = closedList[pathNode])
			{
				stack.Push(new Point(pathNode.x, pathNode.y));
			}
			return stack;
		}

		private byte[,] createMapGrid(GameLocation location, Point endPoint)
		{
			byte[,] array = new byte[location.map.Layers[0].LayerWidth, location.map.Layers[0].LayerHeight];
			for (int i = 0; i < location.map.Layers[0].LayerWidth; i++)
			{
				for (int j = 0; j < location.map.Layers[0].LayerHeight; j++)
				{
					if (!location.isCollidingPosition(new Rectangle(i * Game1.tileSize + 1, j * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2), Game1.viewport, false, 0, false, this.character, true, false, false))
					{
						array[i, j] = (byte)(Math.Abs(endPoint.X - i) + Math.Abs(endPoint.Y - j));
					}
					else
					{
						array[i, j] = 255;
					}
				}
			}
			return array;
		}

		private void moveCharacter(GameTime time)
		{
			Rectangle rectangle = new Rectangle(this.pathToEndPoint.Peek().X * Game1.tileSize, this.pathToEndPoint.Peek().Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			rectangle.Inflate(-2, 0);
			Rectangle boundingBox = this.character.GetBoundingBox();
			if ((rectangle.Contains(boundingBox) || (boundingBox.Width > rectangle.Width && rectangle.Contains(boundingBox.Center))) && rectangle.Bottom - boundingBox.Bottom >= 2)
			{
				this.timerSinceLastCheckPoint = 0;
				this.pathToEndPoint.Pop();
				this.character.stopWithoutChangingFrame();
				if (this.pathToEndPoint.Count == 0)
				{
					this.character.Halt();
					if (this.finalFacingDirection != -1)
					{
						this.character.faceDirection(this.finalFacingDirection);
					}
					if (this.NPCSchedule)
					{
						(this.character as NPC).DirectionsToNewLocation = null;
						(this.character as NPC).endOfRouteMessage = (this.character as NPC).nextEndOfRouteMessage;
					}
					if (this.endBehaviorFunction != null)
					{
						this.endBehaviorFunction(this.character, this.location);
						return;
					}
				}
			}
			else
			{
				if (this.character is Farmer)
				{
					(this.character as Farmer).movementDirections.Clear();
				}
				else
				{
					foreach (NPC current in this.location.characters)
					{
						if (!current.Equals(this.character) && current.GetBoundingBox().Intersects(boundingBox) && current.isMoving() && string.Compare(current.name, this.character.name) < 0)
						{
							this.character.Halt();
							return;
						}
					}
				}
				if (boundingBox.Left < rectangle.Left && boundingBox.Right < rectangle.Right)
				{
					this.character.SetMovingRight(true);
				}
				else if (boundingBox.Right > rectangle.Right && boundingBox.Left > rectangle.Left)
				{
					this.character.SetMovingLeft(true);
				}
				else if (boundingBox.Top <= rectangle.Top)
				{
					this.character.SetMovingDown(true);
				}
				else if (boundingBox.Bottom >= rectangle.Bottom - 2)
				{
					this.character.SetMovingUp(true);
				}
				this.character.MovePosition(time, Game1.viewport, this.location);
				if (this.NPCSchedule)
				{
					Warp warp = this.location.isCollidingWithWarpOrDoor(this.character.nextPosition(this.character.getDirection()));
					if (warp != null)
					{
						if (this.character is NPC && (this.character as NPC).isMarried() && (this.character as NPC).followSchedule)
						{
							NPC nPC = this.character as NPC;
							if (this.location is FarmHouse)
							{
								warp = new Warp(warp.X, warp.Y, "BusStop", 0, 23, false);
							}
							if (this.location is BusStop && warp.X <= 0)
							{
								warp = new Warp(warp.X, warp.Y, nPC.getHome().name, (nPC.getHome() as FarmHouse).getEntryLocation().X, (nPC.getHome() as FarmHouse).getEntryLocation().Y, false);
							}
							if (nPC.temporaryController != null && nPC.controller != null)
							{
								nPC.controller.location = Game1.getLocationFromName(warp.TargetName);
							}
						}
						Game1.warpCharacter(this.character as NPC, warp.TargetName, new Vector2((float)warp.TargetX, (float)warp.TargetY), false, this.location.isOutdoors);
						this.location.characters.Remove(this.character as NPC);
						if (this.location.Equals(Game1.currentLocation) && Utility.isOnScreen(new Vector2((float)(warp.X * Game1.tileSize), (float)(warp.Y * Game1.tileSize)), Game1.tileSize * 6) && this.location.doors.ContainsKey(new Point(warp.X, warp.Y)))
						{
							Game1.playSound("doorClose");
						}
						this.location = Game1.getLocationFromName(warp.TargetName);
						if (this.location.Equals(Game1.currentLocation) && Utility.isOnScreen(new Vector2((float)(warp.TargetX * Game1.tileSize), (float)(warp.TargetY * Game1.tileSize)), Game1.tileSize * 6) && this.location.doors.ContainsKey(new Point(warp.TargetX, warp.TargetY - 1)))
						{
							Game1.playSound("doorClose");
						}
						if (this.pathToEndPoint.Count > 0)
						{
							this.pathToEndPoint.Pop();
						}
						while (this.pathToEndPoint.Count > 0 && (Math.Abs(this.pathToEndPoint.Peek().X - this.character.getTileX()) > 1 || Math.Abs(this.pathToEndPoint.Peek().Y - this.character.getTileY()) > 1))
						{
							this.pathToEndPoint.Pop();
						}
					}
				}
			}
		}

		public static Stack<Point> findPathForNPCSchedules(Point startPoint, Point endPoint, GameLocation location, int limit)
		{
			sbyte[,] array = new sbyte[,]
			{
				{
					-1,
					0
				},
				{
					1,
					0
				},
				{
					0,
					1
				},
				{
					0,
					-1
				}
			};
			PriorityQueue priorityQueue = new PriorityQueue();
			Dictionary<PathNode, PathNode> dictionary = new Dictionary<PathNode, PathNode>();
			int num = 0;
			priorityQueue.Enqueue(new PathNode(startPoint.X, startPoint.Y, 0, null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
			PathNode pathNode = (PathNode)priorityQueue.Peek();
			while (!priorityQueue.IsEmpty())
			{
				PathNode pathNode2 = priorityQueue.Dequeue();
				if (pathNode2.x == endPoint.X && pathNode2.y == endPoint.Y)
				{
					return PathFindController.reconstructPath(pathNode2, dictionary);
				}
				if (pathNode2.x == 79)
				{
					int arg_B2_0 = pathNode2.y;
				}
				if (!dictionary.ContainsKey(pathNode2))
				{
					dictionary.Add(pathNode2, pathNode2.parent);
				}
				for (int i = 0; i < 4; i++)
				{
					PathNode pathNode3 = new PathNode(pathNode2.x + (int)array[i, 0], pathNode2.y + (int)array[i, 1], pathNode2);
					pathNode3.g = pathNode2.g + 1;
					if (!dictionary.ContainsKey(pathNode3) && ((pathNode3.x == endPoint.X && pathNode3.y == endPoint.Y) || (pathNode3.x >= 0 && pathNode3.y >= 0 && pathNode3.x < location.map.Layers[0].LayerWidth && pathNode3.y < location.map.Layers[0].LayerHeight && !PathFindController.isPositionImpassableForNPCSchedule(location, pathNode3.x, pathNode3.y))))
					{
						int priority = (int)pathNode3.g + PathFindController.getPreferenceValueForTerrainType(location, pathNode3.x, pathNode3.y) + (Math.Abs(endPoint.X - pathNode3.x) + Math.Abs(endPoint.Y - pathNode3.y) + (((pathNode3.x == pathNode2.x && pathNode3.x == pathNode.x) || (pathNode3.y == pathNode2.y && pathNode3.y == pathNode.y)) ? -2 : 0));
						if (!priorityQueue.Contains(pathNode3, priority))
						{
							priorityQueue.Enqueue(pathNode3, priority);
						}
					}
				}
				pathNode = pathNode2;
				num++;
				if (num >= limit)
				{
					return null;
				}
			}
			return null;
		}

		private static bool isPositionImpassableForNPCSchedule(GameLocation l, int x, int y)
		{
			return (l.getTileIndexAt(x, y, "Buildings") != -1 && (l.doesTileHaveProperty(x, y, "Action", "Buildings") == null || (!l.doesTileHaveProperty(x, y, "Action", "Buildings").Contains("Door") && !l.doesTileHaveProperty(x, y, "Action", "Buildings").Contains("Passable")))) || l.isTerrainFeatureAt(x, y);
		}

		private static int getPreferenceValueForTerrainType(GameLocation l, int x, int y)
		{
			string text = l.doesTileHaveProperty(x, y, "Type", "Back");
			if (text != null)
			{
				string a = text.ToLower();
				if (a == "stone")
				{
					return -7;
				}
				if (a == "wood")
				{
					return -4;
				}
				if (a == "dirt")
				{
					return -2;
				}
				if (a == "grass")
				{
					return -1;
				}
			}
			return 0;
		}
	}
}
