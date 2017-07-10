using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StardewValley.Network
{
	public class LidgrenServer : Server
	{
		private NetServer server;

		private Thread mapServerThread;

		private static int messageSendCounter = 50;

		public override int connectionsCount
		{
			get
			{
				if (this.server == null)
				{
					return 0;
				}
				return this.server.ConnectionsCount;
			}
		}

		public LidgrenServer(string name) : base(name)
		{
		}

		public override void initializeConnection()
		{
			NetPeerConfiguration netPeerConfiguration = new NetPeerConfiguration("StardewValley");
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
			netPeerConfiguration.Port = 24642;
			netPeerConfiguration.ConnectionTimeout = 120f;
			netPeerConfiguration.PingInterval = 5f;
			netPeerConfiguration.MaximumConnections = 4;
			netPeerConfiguration.EnableUPnP = true;
			this.server = new NetServer(netPeerConfiguration);
			this.server.Start();
			this.server.UPnP.ForwardPort(24642, "Stardew Valley Server");
			this.mapServerThread = new Thread(new ThreadStart(AsynchronousSocketListener.StartListening));
			this.mapServerThread.Start();
			Game1.player.uniqueMultiplayerID = this.server.UniqueIdentifier;
			Game1.serverHost = Game1.player;
		}

		public override void stopServer()
		{
			this.server.Shutdown("Server shutting down...");
			AsynchronousSocketListener.allDone.Close();
			AsynchronousSocketListener.active = false;
		}

		public override void receiveMessages()
		{
			NetIncomingMessage netIncomingMessage;
			while ((netIncomingMessage = this.server.ReadMessage()) != null)
			{
				NetIncomingMessageType messageType = netIncomingMessage.MessageType;
				if (messageType <= NetIncomingMessageType.Data)
				{
					if (messageType != NetIncomingMessageType.ConnectionApproval)
					{
						if (messageType != NetIncomingMessageType.Data)
						{
							goto IL_62;
						}
						LidgrenServer.parseDataMessageFromClient(netIncomingMessage);
					}
					else
					{
						netIncomingMessage.SenderConnection.Approve();
						this.addNewFarmer(netIncomingMessage.SenderConnection.RemoteUniqueIdentifier);
					}
				}
				else if (messageType != NetIncomingMessageType.DiscoveryRequest)
				{
					if (messageType != NetIncomingMessageType.ErrorMessage)
					{
						goto IL_62;
					}
					Game1.debugOutput = netIncomingMessage.ToString();
				}
				else
				{
					this.handshakeWithPlayer(netIncomingMessage);
				}
				IL_6D:
				this.server.Recycle(netIncomingMessage);
				continue;
				IL_62:
				Game1.debugOutput = netIncomingMessage.ToString();
				goto IL_6D;
			}
		}

		private void addNewFarmer(long id)
		{
			Farmer farmer = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2((float)(Game1.tileSize * 10), (float)(Game1.tileSize * 15)), 2, "Max", new List<Item>(), true);
			farmer.FarmerSprite.setOwner(farmer);
			farmer.currentLocation = Game1.getLocationFromName("BusStop");
			farmer.uniqueMultiplayerID = id;
			Game1.getLocationFromName(farmer.currentLocation.name).farmers.Add(farmer);
			Game1.otherFarmers.Add(id, farmer);
			MultiplayerUtility.broadcastPlayerIntroduction(id, "Max");
		}

		private void handshakeWithPlayer(NetIncomingMessage message)
		{
			NetOutgoingMessage netOutgoingMessage = this.server.CreateMessage();
			netOutgoingMessage.Write(this.serverName);
			netOutgoingMessage.Write(2);
			netOutgoingMessage.Write(this.server.UniqueIdentifier);
			netOutgoingMessage.Write(Game1.player.name);
			netOutgoingMessage.Write(Game1.player.currentLocation.name);
			foreach (KeyValuePair<long, Farmer> current in Game1.otherFarmers)
			{
				netOutgoingMessage.Write(2);
				netOutgoingMessage.Write(current.Key);
				netOutgoingMessage.Write(current.Value.name);
				netOutgoingMessage.Write(current.Value.currentLocation.name);
			}
			this.server.SendDiscoveryResponse(netOutgoingMessage, message.SenderEndPoint);
		}

		private static void parseDataMessageFromClient(NetIncomingMessage msg)
		{
			switch (msg.ReadByte())
			{
			case 0:
				Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].setMoving(msg.ReadByte());
				return;
			case 1:
			case 2:
			case 12:
			case 15:
			case 16:
			case 18:
				break;
			case 3:
				((FarmerSprite)Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).CurrentToolIndex = msg.ReadInt32();
				if (msg.ReadByte() == 1)
				{
					((FarmerSprite)Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).animateBackwardsOnce(msg.ReadInt32(), msg.ReadFloat());
					msg.ReadByte();
					return;
				}
				((FarmerSprite)Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).animateOnce(msg.ReadInt32(), msg.ReadFloat(), (int)msg.ReadByte());
				return;
			case 4:
				MultiplayerUtility.serverTryToPerformObjectAlteration(msg.ReadInt16(), msg.ReadInt16(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt32(), Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier]);
				return;
			case 5:
				MultiplayerUtility.warpCharacter(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 6:
				MultiplayerUtility.performSwitchHeldItem(msg.SenderConnection.RemoteUniqueIdentifier, msg.ReadByte(), (int)msg.ReadInt16());
				return;
			case 7:
				MultiplayerUtility.performToolAction(msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt16(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 8:
				MultiplayerUtility.performDebrisPickup(msg.ReadInt32(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 9:
				MultiplayerUtility.performCheckAction(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 10:
				MultiplayerUtility.receiveChatMessage(msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 11:
				MultiplayerUtility.receiveNameChange(msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 13:
				MultiplayerUtility.receiveBuildingChange(msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier, 0L);
				return;
			case 14:
				MultiplayerUtility.performDebrisCreate(msg.ReadInt16(), msg.ReadInt32(), msg.ReadInt32(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 17:
				Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].readyConfirmation = true;
				MultiplayerUtility.allFarmersReadyCheck();
				return;
			case 19:
				MultiplayerUtility.interpretMessageToEveryone(msg.ReadInt32(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				break;
			default:
				return;
			}
		}

		public override void sendMessages(GameTime time)
		{
			LidgrenServer.messageSendCounter -= time.ElapsedGameTime.Milliseconds;
			if (LidgrenServer.messageSendCounter < 0)
			{
				LidgrenServer.messageSendCounter = 50;
				foreach (NetConnection current in this.server.Connections)
				{
					if (Game1.otherFarmers.ContainsKey(current.RemoteUniqueIdentifier) && Game1.otherFarmers[current.RemoteUniqueIdentifier].multiplayerMessage.Count > 0)
					{
						NetOutgoingMessage netOutgoingMessage = this.server.CreateMessage();
						for (int i = 0; i < Game1.otherFarmers[current.RemoteUniqueIdentifier].multiplayerMessage.Count; i++)
						{
							MultiplayerUtility.writeData(netOutgoingMessage, (byte)Game1.otherFarmers[current.RemoteUniqueIdentifier].multiplayerMessage[i][0], Game1.otherFarmers[current.RemoteUniqueIdentifier].multiplayerMessage[i].Skip(1).ToArray<object>());
						}
						this.server.SendMessage(netOutgoingMessage, current, NetDeliveryMethod.ReliableOrdered);
					}
				}
				foreach (KeyValuePair<long, Farmer> current2 in Game1.otherFarmers)
				{
					current2.Value.multiplayerMessage.Clear();
				}
			}
		}
	}
}
