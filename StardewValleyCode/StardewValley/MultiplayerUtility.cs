using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

namespace StardewValley
{
	public class MultiplayerUtility
	{
		public const byte movement = 0;

		public const byte position = 1;

		public const byte playerIntroduction = 2;

		public const byte animation = 3;

		public const byte objectAlteration = 4;

		public const byte warpFarmer = 5;

		public const byte switchHeldItem = 6;

		public const byte toolAction = 7;

		public const byte debrisPickup = 8;

		public const byte checkAction = 9;

		public const byte chatMessage = 10;

		public const byte nameChange = 11;

		public const byte tenMinSync = 12;

		public const byte building = 13;

		public const byte debrisCreate = 14;

		public const byte npcMove = 15;

		public const byte npcBehavior = 16;

		public const byte readyConfirmation = 17;

		public const byte serverToClientsMessage = 18;

		public const byte messageToEveryone = 19;

		public const byte addObject = 0;

		public const byte removeObject = 1;

		public const byte addTerrainFeature = 2;

		public const byte removeTerrainFeature = 3;

		public const byte addBuilding = 0;

		public const byte removeBuilding = 1;

		public const byte upgradeBuilding = 2;

		public static long recentMultiplayerEntityID;

		public static long latestID = -9223372036854775808L + (long)Game1.random.Next(1000);

		public const string MSG_START_FESTIVAL_EVENT = "festivalEvent";

		public const string MSG_END_FESTIVAL = "endFest";

		public const string MSG_PLACEHOLDER = "[replace me]";

		public const int DANCE_PARTNER = 0;

		public const int LUAU_ITEM = 1;

		public const int GRANGE_DISPLAY_USER = 2;

		public const int GRANGE_DISPLAY_CHANGE = 3;

		public const int MSGE_GRANGE_SCORE = 4;

		public const int MSGE_ADD_MAIL_RECEIVED = 5;

		public const int MSGE_BUNDLE_COMPLETE = 6;

		public const int MSGE_ADD_MAIL_FOR_TOMORROW = 7;

		public static long getNewID()
		{
			long expr_05 = MultiplayerUtility.latestID;
			MultiplayerUtility.latestID = expr_05 + 1L;
			return expr_05;
		}

		public static void broadcastFarmerPosition(long f, Vector2 position, string currentLocation)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.name.Equals(currentLocation))
				{
					for (int i = 0; i < current.Value.multiplayerMessage.Count; i++)
					{
						if ((byte)current.Value.multiplayerMessage[i][0] == 1 && (long)current.Value.multiplayerMessage[i][1] == f)
						{
							current.Value.multiplayerMessage.RemoveAt(i);
							break;
						}
					}
					current.Value.multiplayerMessage.Add(new object[]
					{
						1,
						f,
						position
					});
				}
			}
		}

		public static void broadcastFarmerAnimation(long f, int startingFrame, int numberOfFrames, float animationSpeed, bool backwards, string currentLocation, int currentToolIndex)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.name.Equals(currentLocation) && current.Value.uniqueMultiplayerID != f)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						3,
						f,
						currentToolIndex,
						backwards ? 1 : 0,
						startingFrame,
						animationSpeed,
						(byte)numberOfFrames
					});
				}
			}
		}

		public static void broadcastFarmerMovement(long f, byte command, string currentLocation)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.name.Equals(currentLocation))
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						0,
						f,
						command
					});
				}
			}
		}

		public static void broadcastObjectChange(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo, string location)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.name.Equals(location))
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						4,
						x,
						y,
						command,
						terrainFeatureIndex,
						extraInfo
					});
				}
			}
		}

		public static void broadcastFarmerWarp(short x, short y, string nameOfNewLocation, bool isStructure, long id)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.uniqueMultiplayerID != id)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						5,
						x,
						y,
						nameOfNewLocation,
						isStructure ? 1 : 0,
						id
					});
				}
			}
		}

		public static void broadcastSwitchHeldItem(byte bigCraftable, short heldItem, long whichPlayer, string location)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.name.Equals(location) && whichPlayer != current.Value.uniqueMultiplayerID)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						6,
						whichPlayer,
						bigCraftable,
						heldItem
					});
				}
			}
		}

		public static void broadcastToolAction(Tool t, int tileX, int tileY, string location, byte facingDirection, short seed, long whichPlayer)
		{
			ToolDescription indexFromTool = ToolFactory.getIndexFromTool(t);
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.name.Equals(location) && whichPlayer != current.Value.uniqueMultiplayerID)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						7,
						indexFromTool.index,
						indexFromTool.upgradeLevel,
						(short)tileX,
						(short)tileY,
						location,
						facingDirection,
						seed,
						whichPlayer
					});
				}
			}
		}

		public static NetOutgoingMessage writeData(NetOutgoingMessage sendMsg, byte which, object[] data)
		{
			sendMsg.Write(which);
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i].GetType() == typeof(Vector2))
				{
					sendMsg.Write((Vector2)data[i]);
				}
				else if (data[i] is byte)
				{
					sendMsg.Write((byte)data[i]);
				}
				else if (data[i] is int)
				{
					sendMsg.Write((int)data[i]);
				}
				else if (data[i] is short)
				{
					sendMsg.Write((short)data[i]);
				}
				else if (data[i] is float)
				{
					sendMsg.Write((float)data[i]);
				}
				else if (data[i] is long)
				{
					sendMsg.Write((long)data[i]);
				}
				else if (data[i] is string)
				{
					sendMsg.Write((string)data[i]);
				}
			}
			return sendMsg;
		}

		public static void broadcastGameClock()
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				current.Value.multiplayerMessage.Add(new object[]
				{
					12,
					(short)Game1.timeOfDay
				});
			}
		}

		public static void receiveTenMinuteSync(short time)
		{
			Game1.timeOfDay = (int)time;
			Game1.performTenMinuteClockUpdate();
		}

		public static void sendAnimationMessageToServer(int startingFrame, int numberOfFrames, float animationSpeed, bool backwards, int currentToolIndex)
		{
			Game1.client.sendMessage(3, new object[]
			{
				currentToolIndex,
				backwards ? 1 : 0,
				startingFrame,
				animationSpeed,
				(byte)numberOfFrames
			});
		}

		private static int translateObjectIndex(int index)
		{
			switch (index)
			{
			case -9:
				return 325;
			case -7:
				return 324;
			case -6:
				return 323;
			case -5:
				return 322;
			}
			return index;
		}

		public static void performObjectAlteration(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo)
		{
			switch (command)
			{
			case 0:
			{
				extraInfo = MultiplayerUtility.translateObjectIndex(extraInfo);
				Object @object;
				if (terrainFeatureIndex == 0)
				{
					@object = new Object(Vector2.Zero, extraInfo, null, true, false, false, false);
				}
				else
				{
					@object = new Object(Vector2.Zero, extraInfo, false);
				}
				@object.placementAction(Game1.currentLocation, (int)x * Game1.tileSize, (int)y * Game1.tileSize, null);
				return;
			}
			case 1:
			{
				Object @object;
				Game1.currentLocation.objects.TryGetValue(new Vector2((float)x, (float)y), out @object);
				if (@object != null)
				{
					Game1.currentLocation.objects.Remove(new Vector2((float)x, (float)y));
					@object.performRemoveAction(new Vector2((float)x, (float)y), Game1.currentLocation);
					return;
				}
				break;
			}
			case 2:
				if (Game1.currentLocation.terrainFeatures.ContainsKey(new Vector2((float)x, (float)y)))
				{
					Game1.currentLocation.terrainFeatures[new Vector2((float)x, (float)y)] = TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo);
					return;
				}
				Game1.currentLocation.terrainFeatures.Add(new Vector2((float)x, (float)y), TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo));
				return;
			case 3:
				Game1.currentLocation.terrainFeatures.Remove(new Vector2((float)x, (float)y));
				break;
			default:
				return;
			}
		}

		public static void serverTryToPerformObjectAlteration(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo, Farmer actionPerformer)
		{
			switch (command)
			{
			case 0:
			{
				extraInfo = MultiplayerUtility.translateObjectIndex(extraInfo);
				Object @object;
				if (terrainFeatureIndex == 0)
				{
					@object = new Object(Vector2.Zero, extraInfo, null, true, false, false, false);
				}
				else
				{
					@object = new Object(Vector2.Zero, extraInfo, false);
				}
				if (Utility.playerCanPlaceItemHere(actionPerformer.currentLocation, @object, (int)x, (int)y, actionPerformer))
				{
					@object.placementAction(Game1.currentLocation, (int)x, (int)y, null);
					return;
				}
				break;
			}
			case 1:
			{
				Object @object = Game1.currentLocation.objects[new Vector2((float)x, (float)y)];
				Game1.currentLocation.objects.Remove(new Vector2((float)x, (float)y));
				if (@object != null)
				{
					@object.performRemoveAction(new Vector2((float)x, (float)y), Game1.currentLocation);
					return;
				}
				break;
			}
			case 2:
				Game1.currentLocation.terrainFeatures.Add(new Vector2((float)x, (float)y), TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo));
				return;
			case 3:
				Game1.currentLocation.terrainFeatures.Remove(new Vector2((float)x, (float)y));
				break;
			default:
				return;
			}
		}

		public static void performSwitchHeldItem(long id, byte bigCraftable, int index)
		{
			if (index == -1)
			{
				Game1.otherFarmers[id].showNotCarrying();
				if (Game1.otherFarmers[id].ActiveObject != null)
				{
					Game1.otherFarmers[id].ActiveObject.actionWhenStopBeingHeld(Game1.otherFarmers[id]);
				}
				Game1.otherFarmers[id].items[Game1.otherFarmers[id].CurrentToolIndex] = null;
			}
			else
			{
				Game1.otherFarmers[id].showCarrying();
				Game1.otherFarmers[id].ActiveObject = ((bigCraftable == 1) ? new Object(Vector2.Zero, index, false) : new Object(Vector2.Zero, index, 1));
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendSwitchHeldItemMessage(index, bigCraftable, id);
			}
		}

		public static void sendSwitchHeldItemMessage(int heldItemIndex, byte bigCraftable, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(6, new object[]
				{
					bigCraftable,
					(short)heldItemIndex
				});
				return;
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastSwitchHeldItem(bigCraftable, (short)heldItemIndex, whichPlayer, Game1.currentLocation.name);
			}
		}

		public static void sendMessageToEveryone(int messageCategory, string message, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(19, new object[]
				{
					messageCategory,
					message
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						19,
						messageCategory,
						message,
						whichPlayer
					});
				}
			}
		}

		public static void warpCharacter(short x, short y, string name, byte isStructure, long id)
		{
			if (Game1.otherFarmers.ContainsKey(id))
			{
				if (Game1.otherFarmers[id].currentLocation == null)
				{
					Game1.otherFarmers[id]._tmpLocationName = name;
					return;
				}
				Game1.otherFarmers[id].currentLocation.farmers.Remove(Game1.otherFarmers[id]);
				Game1.otherFarmers[id].currentLocation = Game1.getLocationFromName(name, isStructure == 1);
				Game1.otherFarmers[id].position.X = (float)((int)x * Game1.tileSize);
				Game1.otherFarmers[id].position.Y = (float)((int)y * Game1.tileSize - Game1.tileSize / 2);
				GameLocation locationFromName = Game1.getLocationFromName(name, isStructure == 1);
				locationFromName.farmers.Add(Game1.otherFarmers[id]);
				if (locationFromName.farmers.Count.Equals(Game1.numberOfPlayers() - 1))
				{
					locationFromName.checkForEvents();
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastFarmerWarp(x, y, name, isStructure == 1, id);
			}
		}

		public static void performToolAction(byte toolIndex, byte toolUpgradeLevel, short xTile, short yTile, string locationName, byte facingDirection, short seed, long who)
		{
			Tool toolFromDescription = ToolFactory.getToolFromDescription(toolIndex, (int)toolUpgradeLevel);
			GameLocation locationFromName = Game1.getLocationFromName(locationName);
			Game1.otherFarmers[who].CurrentTool = toolFromDescription;
			Game1.recentMultiplayerRandom = new Random((int)seed);
			if (locationFromName == null)
			{
				if (toolFromDescription is MeleeWeapon)
				{
					Game1.otherFarmers[who].faceDirection((int)facingDirection);
					(toolFromDescription as MeleeWeapon).DoDamage(Game1.currentLocation, (int)xTile, (int)yTile, Game1.otherFarmers[who].facingDirection, 1, Game1.otherFarmers[who]);
				}
				else
				{
					toolFromDescription.DoFunction(Game1.currentLocation, (int)xTile, (int)yTile, 1, Game1.otherFarmers[who]);
				}
			}
			else if (toolFromDescription is MeleeWeapon)
			{
				Game1.otherFarmers[who].faceDirection((int)facingDirection);
				(toolFromDescription as MeleeWeapon).DoDamage(locationFromName, (int)xTile, (int)yTile, Game1.otherFarmers[who].facingDirection, 1, Game1.otherFarmers[who]);
			}
			else
			{
				toolFromDescription.DoFunction(locationFromName, (int)xTile, (int)yTile, 1, Game1.otherFarmers[who]);
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastToolAction(toolFromDescription, (int)xTile, (int)yTile, locationName, facingDirection, seed, who);
			}
		}

		public static void broadcastBuildingChange(byte whatChange, Vector2 tileLocation, string name, string locationName, long who)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(13, new object[]
				{
					whatChange,
					(short)tileLocation.X,
					(short)tileLocation.Y,
					name
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					if (current.Value.currentLocation.name.Equals(locationName) && (whatChange != 2 || current.Value.uniqueMultiplayerID != who))
					{
						current.Value.multiplayerMessage.Add(new object[]
						{
							13,
							whatChange,
							(short)tileLocation.X,
							(short)tileLocation.Y,
							name,
							who,
							MultiplayerUtility.recentMultiplayerEntityID
						});
					}
				}
			}
		}

		public static void receiveBuildingChange(byte whatChange, short tileX, short tileY, string name, long who, long id)
		{
			if (Game1.IsClient)
			{
				MultiplayerUtility.recentMultiplayerEntityID = id;
			}
			else
			{
				MultiplayerUtility.recentMultiplayerEntityID = MultiplayerUtility.getNewID();
			}
			if (Game1.currentLocation is Farm || Game1.IsServer)
			{
				Farm farm = (Farm)Game1.currentLocation;
				if (!(Game1.currentLocation is Farm))
				{
					farm = (Farm)Game1.otherFarmers[id].currentLocation;
				}
				Farmer farmer = Game1.getFarmer(who);
				switch (whatChange)
				{
				case 0:
				{
					BluePrint bluePrint = new BluePrint(name);
					if (bluePrint.blueprintType.Equals("Animals") && farm.placeAnimal(bluePrint, new Vector2((float)tileX, (float)tileY), true, who) && farmer.IsMainPlayer)
					{
						bluePrint.consumeResources();
					}
					else if (!bluePrint.blueprintType.Equals("Animals") && farm.buildStructure(bluePrint, new Vector2((float)tileX, (float)tileY), true, farmer, false) && farmer.IsMainPlayer)
					{
						bluePrint.consumeResources();
					}
					else if (farmer.IsMainPlayer)
					{
						Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10016", new object[0]), Color.Red, 3500f));
						return;
					}
					if (!bluePrint.blueprintType.Equals("Animals"))
					{
						Game1.playSound("axe");
						return;
					}
					break;
				}
				case 1:
				{
					Building buildingAt = farm.getBuildingAt(new Vector2((float)tileX, (float)tileY));
					if (farm.destroyStructure(new Vector2((float)tileX, (float)tileY)))
					{
						int groundLevelTile = buildingAt.tileY + buildingAt.tilesHigh;
						for (int i = 0; i < buildingAt.texture.Bounds.Height / Game1.tileSize; i++)
						{
							Game1.createRadialDebris(farm, buildingAt.texture, new Microsoft.Xna.Framework.Rectangle(buildingAt.texture.Bounds.Center.X, buildingAt.texture.Bounds.Center.Y, Game1.tileSize / 16, Game1.tileSize / 16), buildingAt.tileX + Game1.random.Next(buildingAt.tilesWide), buildingAt.tileY + buildingAt.tilesHigh - i, Game1.random.Next(20, 45), groundLevelTile);
						}
						Game1.playSound("explosion");
						Utility.spreadAnimalsAround(buildingAt, farm);
						return;
					}
					break;
				}
				case 2:
				{
					BluePrint bluePrint = new BluePrint(name);
					Building buildingAt2 = farm.getBuildingAt(new Vector2((float)tileX, (float)tileY));
					farm.tryToUpgrade(buildingAt2, bluePrint);
					break;
				}
				default:
					return;
				}
			}
		}

		public static void broadcastDebrisPickup(int uniqueID, string locationName, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(8, new object[]
				{
					uniqueID,
					locationName
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					if (current.Value.currentLocation.name.Equals(locationName) && whichPlayer != current.Value.uniqueMultiplayerID)
					{
						current.Value.multiplayerMessage.Add(new object[]
						{
							8,
							uniqueID,
							locationName,
							whichPlayer
						});
					}
				}
			}
		}

		public static void receivePlayerIntroduction(long id, string name)
		{
			Farmer farmer = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\farmer")), new Vector2((float)(Game1.tileSize * 5), (float)(Game1.tileSize * 5)), 2, name, new List<Item>(), true);
			farmer.FarmerSprite.setOwner(farmer);
			farmer.currentLocation = Game1.getLocationFromName("FarmHouse");
			farmer.uniqueMultiplayerID = id;
			Game1.otherFarmers.Add(id, farmer);
		}

		public static void broadcastPlayerIntroduction(long id, string name)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (id != current.Value.uniqueMultiplayerID)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						2,
						id,
						name
					});
				}
			}
		}

		public static void performCheckAction(short x, short y, string location, long who)
		{
			if (!Utility.canGrabSomethingFromHere((int)x * Game1.tileSize, (int)y * Game1.tileSize, Game1.otherFarmers[who]) || !Game1.getLocationFromName(location).objects.ContainsKey(new Vector2((float)x, (float)y)) || !Game1.getLocationFromName(location).objects[new Vector2((float)x, (float)y)].checkForAction(Game1.otherFarmers[who], false))
			{
				if (Game1.isFestival())
				{
					Game1.currentLocation.checkAction(new Location((int)x, (int)y), Game1.viewport, Game1.otherFarmers[who]);
				}
				else
				{
					Game1.getLocationFromName(location).checkAction(new Location((int)x, (int)y), Game1.viewport, Game1.otherFarmers[who]);
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastCheckAction((int)x, (int)y, who, location);
			}
		}

		public static void broadcastCheckAction(int x, int y, long who, string location)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(9, new object[]
				{
					(short)x,
					(short)y,
					location
				});
				return;
			}
			if (Game1.IsServer)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"Server Received Check Action message @ X:",
					x,
					" Y:",
					y
				}));
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					if (current.Value.currentLocation.name.Equals(location) && who != current.Value.uniqueMultiplayerID)
					{
						current.Value.multiplayerMessage.Add(new object[]
						{
							9,
							(short)x,
							(short)y,
							location,
							who
						});
					}
				}
			}
		}

		public static void sendReadyConfirmation(long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(17, new object[]
				{
					0
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						17,
						whichPlayer
					});
				}
			}
		}

		public static void sendServerToClientsMessage(string message)
		{
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						18,
						message
					});
				}
			}
		}

		public static void sendChatMessage(string message, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(10, new object[]
				{
					message
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						10,
						message,
						whichPlayer
					});
				}
			}
		}

		public static void sendNameChange(string name, long who)
		{
			if (who == Game1.player.uniqueMultiplayerID)
			{
				Game1.player.name = name;
			}
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(11, new object[]
				{
					name
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					if (who != current.Value.uniqueMultiplayerID)
					{
						current.Value.multiplayerMessage.Add(new object[]
						{
							11,
							name,
							who
						});
					}
				}
			}
		}

		public static void receiveNameChange(string message, long who)
		{
			Game1.ChatBox.receiveChatMessage(Game1.otherFarmers[who].isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MultiplayerUtility.cs.12478", new object[]
			{
				Game1.otherFarmers[who].displayName,
				message
			}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:MultiplayerUtility.cs.12479", new object[]
			{
				Game1.otherFarmers[who].displayName,
				message
			}), -1L);
			Game1.otherFarmers[who].name = message;
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendNameChange(message, who);
			}
		}

		public static void receiveChatMessage(string message, long whichPlayer)
		{
			foreach (IClickableMenu current in Game1.onScreenMenus)
			{
				if (current is ChatBox)
				{
					((ChatBox)current).receiveChatMessage(message, whichPlayer);
					break;
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendChatMessage(message, whichPlayer);
			}
		}

		public static void allFarmersReadyCheck()
		{
			if (Game1.IsServer)
			{
				using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.readyConfirmation)
						{
							return;
						}
					}
				}
				if (Game1.player.readyConfirmation)
				{
					MultiplayerUtility.sendReadyConfirmation(Game1.player.uniqueMultiplayerID);
					using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.readyConfirmation = false;
						}
					}
					Game1.player.readyConfirmation = false;
					if (Game1.currentLocation.currentEvent != null)
					{
						Event expr_BF = Game1.currentLocation.currentEvent;
						int currentCommand = expr_BF.CurrentCommand;
						expr_BF.CurrentCommand = currentCommand + 1;
						return;
					}
				}
			}
			else
			{
				if (Game1.isFestival())
				{
					Game1.currentLocation.currentEvent.allPlayersReady = true;
				}
				using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.readyConfirmation = false;
					}
				}
			}
		}

		public static void parseServerToClientsMessage(string message)
		{
			if (Game1.IsClient)
			{
				if (!(message == "festivalEvent"))
				{
					if (!(message == "endFest"))
					{
						return;
					}
					if (Game1.CurrentEvent != null)
					{
						Game1.CurrentEvent.forceEndFestival(Game1.player);
					}
				}
				else if (Game1.currentLocation.currentEvent != null)
				{
					Game1.currentLocation.currentEvent.forceFestivalContinue();
					return;
				}
			}
		}

		public static void interpretMessageToEveryone(int messageCategory, string message, long who)
		{
			switch (messageCategory)
			{
			case 0:
				if (Game1.isFestival())
				{
					Game1.otherFarmers[who].dancePartner = Game1.currentLocation.currentEvent.getActorByName(message);
				}
				Game1.currentLocation.currentEvent.getActorByName(message).hasPartnerForDance = true;
				break;
			case 1:
				if (Game1.isFestival())
				{
					Game1.currentLocation.currentEvent.addItemToLuauSoup(new Object(Convert.ToInt32(message.Split(new char[]
					{
						' '
					})[0]), 1, false, -1, Convert.ToInt32(message.Split(new char[]
					{
						' '
					})[1])), Game1.otherFarmers[who]);
				}
				break;
			case 2:
				if (Game1.isFestival())
				{
					Game1.CurrentEvent.setGrangeDisplayUser(message.Equals("null") ? null : Game1.getFarmer(who));
				}
				break;
			case 3:
				if (Game1.isFestival())
				{
					string[] array = message.Split(new char[]
					{
						' '
					});
					int num = Convert.ToInt32(array[0]);
					if (array[1].Equals("null"))
					{
						Game1.CurrentEvent.addItemToGrangeDisplay(null, num, true);
					}
					else
					{
						Game1.CurrentEvent.addItemToGrangeDisplay(new Object(Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), false, -1, 0), num, true);
					}
				}
				break;
			case 4:
				Game1.CurrentEvent.grangeScore = Convert.ToInt32(message);
				Game1.ChatBox.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MultiplayerUtility.cs.12488", new object[0]), -1L);
				Game1.CurrentEvent.interpretGrangeResults();
				break;
			case 5:
				if (!Game1.player.mailReceived.Contains(message))
				{
					Game1.player.mailReceived.Add(message);
				}
				break;
			case 6:
				(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).completeBundle(Convert.ToInt32(message));
				break;
			case 7:
				Game1.addMailForTomorrow(message, false, false);
				break;
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendMessageToEveryone(messageCategory, message, who);
			}
		}

		public static void broadcastDebrisCreate(short seed, Vector2 position, int facingDirection, Item i, long who)
		{
			ItemDescription descriptionFromItem = ObjectFactory.getDescriptionFromItem(i);
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(14, new object[]
				{
					seed,
					(int)position.X,
					(int)position.Y,
					(byte)facingDirection,
					descriptionFromItem.type,
					(short)descriptionFromItem.index,
					(short)descriptionFromItem.stack
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
				{
					if (who != current.Value.uniqueMultiplayerID && (who == Game1.player.uniqueMultiplayerID || Game1.otherFarmers[who].currentLocation.Equals(current.Value.currentLocation)))
					{
						current.Value.multiplayerMessage.Add(new object[]
						{
							14,
							seed,
							(int)position.X,
							(int)position.Y,
							(byte)facingDirection,
							descriptionFromItem.type,
							(short)descriptionFromItem.index,
							(short)descriptionFromItem.stack
						});
					}
				}
			}
		}

		public static void performDebrisCreate(short seed, int xPosition, int yPosition, byte facingDirection, byte type, short index, short stack, long who)
		{
			Game1.recentMultiplayerRandom = new Random((int)seed);
			Vector2 targetLocation = new Vector2((float)xPosition, (float)yPosition);
			Vector2 debrisOrigin = new Vector2((float)xPosition, (float)yPosition);
			Item itemFromDescription = ObjectFactory.getItemFromDescription(type, (int)index, (int)stack);
			switch (facingDirection)
			{
			case 0:
				debrisOrigin.X -= (float)(Game1.tileSize / 2);
				debrisOrigin.Y -= (float)(Game1.tileSize * 2 + Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2));
				targetLocation.Y -= (float)(Game1.tileSize * 3);
				break;
			case 1:
				debrisOrigin.X += (float)(Game1.tileSize * 2 / 3);
				debrisOrigin.Y -= (float)(Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
				targetLocation.X += (float)(Game1.tileSize * 4);
				break;
			case 2:
				debrisOrigin.X -= (float)(Game1.tileSize / 2);
				debrisOrigin.Y += (float)Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2);
				targetLocation.Y += (float)(Game1.tileSize * 3 / 2);
				break;
			case 3:
				debrisOrigin.X -= (float)Game1.tileSize;
				debrisOrigin.Y -= (float)(Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
				targetLocation.X -= (float)(Game1.tileSize * 4);
				break;
			}
			if (Game1.IsClient)
			{
				Game1.currentLocation.debris.Add(new Debris(itemFromDescription, debrisOrigin, targetLocation));
				return;
			}
			if (Game1.IsServer)
			{
				Game1.otherFarmers[who].currentLocation.debris.Add(new Debris(itemFromDescription, debrisOrigin, targetLocation));
				MultiplayerUtility.broadcastDebrisCreate(seed, targetLocation, (int)facingDirection, itemFromDescription, who);
			}
		}

		public static void performDebrisPickup(int uniqueID, string locationName, long whichPlayer)
		{
			GameLocation locationFromName = Game1.getLocationFromName(locationName);
			for (int i = 0; i < locationFromName.debris.Count; i++)
			{
				if (locationFromName.debris[i].uniqueID == uniqueID)
				{
					locationFromName.debris.RemoveAt(i);
					break;
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastDebrisPickup(uniqueID, locationName, whichPlayer);
			}
		}

		public static void broadcastNPCMove(int x, int y, long id, GameLocation location)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.Equals(location))
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						15,
						x,
						y,
						id
					});
				}
			}
		}

		public static void broadcastNPCBehavior(long npcID, GameLocation location, byte behavior)
		{
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				if (current.Value.currentLocation.Equals(location))
				{
					current.Value.multiplayerMessage.Add(new object[]
					{
						16,
						npcID,
						behavior
					});
				}
			}
		}

		public static void performNPCBehavior(long npcID, byte behavior)
		{
			Character characterFromID = MultiplayerUtility.getCharacterFromID(npcID);
			if (characterFromID != null && !characterFromID.ignoreMultiplayerUpdates)
			{
				characterFromID.performBehavior(behavior);
			}
		}

		public static void performNPCMove(int x, int y, long id)
		{
			Character characterFromID = MultiplayerUtility.getCharacterFromID(id);
			if (characterFromID != null && !characterFromID.ignoreMultiplayerUpdates)
			{
				characterFromID.updatePositionFromServer(new Vector2((float)x, (float)y));
			}
		}

		public static Character getCharacterFromID(long id)
		{
			if (Game1.currentLocation is Farm)
			{
				if ((Game1.currentLocation as Farm).animals.ContainsKey(id))
				{
					return (Game1.currentLocation as Farm).animals[id];
				}
				foreach (Building current in (Game1.currentLocation as Farm).buildings)
				{
					if (current.indoors is AnimalHouse && (current.indoors as AnimalHouse).animals.ContainsKey(id))
					{
						return (current.indoors as AnimalHouse).animals[id];
					}
				}
			}
			return null;
		}
	}
}
