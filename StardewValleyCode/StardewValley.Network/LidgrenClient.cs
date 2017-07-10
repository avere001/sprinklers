using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net;

namespace StardewValley.Network
{
	public class LidgrenClient : Client
	{
		private NetClient client;

		public override bool isConnected
		{
			get
			{
				return this.client != null && this.client.ConnectionStatus == NetConnectionStatus.Connected;
			}
		}

		public override float averageRoundtripTime
		{
			get
			{
				return this.client.ServerConnection.AverageRoundtripTime;
			}
		}

		public override IPAddress serverAddress
		{
			get
			{
				return this.client.ServerConnection.RemoteEndPoint.Address;
			}
		}

		public override void initializeConnection(string address)
		{
			NetPeerConfiguration netPeerConfiguration = new NetPeerConfiguration("StardewValley");
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			netPeerConfiguration.ConnectionTimeout = 120f;
			netPeerConfiguration.PingInterval = 5f;
			this.client = new NetClient(netPeerConfiguration);
			this.client.Start();
			int serverPort = 24642;
			if (address.Contains(":"))
			{
				string[] expr_64 = address.Split(new char[]
				{
					':'
				});
				address = expr_64[0];
				serverPort = Convert.ToInt32(expr_64[1]);
			}
			this.client.DiscoverKnownPeer(address, serverPort);
		}

		public override void receiveMessages()
		{
			NetIncomingMessage netIncomingMessage;
			while ((netIncomingMessage = this.client.ReadMessage()) != null)
			{
				NetIncomingMessageType messageType = netIncomingMessage.MessageType;
				if (messageType <= NetIncomingMessageType.DiscoveryResponse)
				{
					if (messageType != NetIncomingMessageType.Data)
					{
						if (messageType == NetIncomingMessageType.DiscoveryResponse)
						{
							Console.WriteLine("Found server at " + netIncomingMessage.SenderEndPoint);
							this.serverName = netIncomingMessage.ReadString();
							this.receiveHandshake(netIncomingMessage);
						}
					}
					else
					{
						LidgrenClient.parseDataMessageFromServer(netIncomingMessage);
					}
				}
				else if (messageType != NetIncomingMessageType.WarningMessage)
				{
					if (messageType != NetIncomingMessageType.ErrorMessage)
					{
						if (messageType != NetIncomingMessageType.ConnectionLatencyUpdated)
						{
						}
					}
					else
					{
						Game1.debugOutput = netIncomingMessage.ReadString();
					}
				}
				else
				{
					Game1.debugOutput = netIncomingMessage.ReadString();
				}
			}
			if (this.client.ServerConnection != null && DateTime.Now.Second % 2 == 0)
			{
				Game1.debugOutput = "Ping: " + this.client.ServerConnection.AverageRoundtripTime * 1000f + "ms";
			}
		}

		private void receiveHandshake(NetIncomingMessage msg)
		{
			while ((long)msg.LengthBits - msg.Position >= 8L)
			{
				byte b = msg.ReadByte();
				if (b == 2)
				{
					long num = msg.ReadInt64();
					Farmer farmer = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2((float)(Game1.tileSize * 10), (float)(Game1.tileSize * 15)), 2, msg.ReadString(), new List<Item>(), true);
					farmer.FarmerSprite.setOwner(farmer);
					Game1.serverHost = farmer;
					Game1.otherFarmers.Add(num, farmer);
					Game1.otherFarmers[num]._tmpLocationName = msg.ReadString();
					Game1.otherFarmers[num].uniqueMultiplayerID = num;
				}
			}
			this.client.Connect(msg.SenderEndPoint.Address.ToString(), msg.SenderEndPoint.Port);
			this.hasHandshaked = true;
			if (!Game1.otherFarmers.ContainsKey(this.client.UniqueIdentifier))
			{
				Game1.otherFarmers.Add(this.client.UniqueIdentifier, Game1.player);
				Game1.player.uniqueMultiplayerID = this.client.UniqueIdentifier;
				Game1.player._tmpLocationName = "BusStop";
			}
		}

		public override void sendMessage(byte which, object[] data)
		{
			NetOutgoingMessage netOutgoingMessage = this.client.CreateMessage();
			MultiplayerUtility.writeData(netOutgoingMessage, which, data);
			this.client.SendMessage(netOutgoingMessage, NetDeliveryMethod.ReliableOrdered);
		}

		private static void parseDataMessageFromServer(NetIncomingMessage msg)
		{
			while ((long)msg.LengthBits - msg.Position >= 8L)
			{
				switch (msg.ReadByte())
				{
				case 0:
					Game1.otherFarmers[msg.ReadInt64()].setMoving(msg.ReadByte());
					break;
				case 1:
					Game1.otherFarmers[msg.ReadInt64()].updatePositionFromServer(msg.ReadVector2());
					break;
				case 2:
					MultiplayerUtility.receivePlayerIntroduction(msg.ReadInt64(), msg.ReadString());
					break;
				case 3:
				{
					long key = msg.ReadInt64();
					Game1.otherFarmers[key].FarmerSprite.CurrentToolIndex = msg.ReadInt32();
					if (msg.ReadByte() == 1)
					{
						((FarmerSprite)Game1.otherFarmers[key].sprite).animateBackwardsOnce(msg.ReadInt32(), msg.ReadFloat());
						msg.ReadByte();
					}
					else
					{
						((FarmerSprite)Game1.otherFarmers[key].sprite).animateOnce(msg.ReadInt32(), msg.ReadFloat(), (int)msg.ReadByte());
					}
					break;
				}
				case 4:
					MultiplayerUtility.performObjectAlteration(msg.ReadInt16(), msg.ReadInt16(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt32());
					break;
				case 5:
					MultiplayerUtility.warpCharacter(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt64());
					break;
				case 6:
					MultiplayerUtility.performSwitchHeldItem(msg.ReadInt64(), msg.ReadByte(), (int)msg.ReadInt16());
					break;
				case 7:
					MultiplayerUtility.performToolAction(msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt64());
					break;
				case 8:
					MultiplayerUtility.performDebrisPickup(msg.ReadInt32(), msg.ReadString(), msg.ReadInt64());
					break;
				case 9:
					MultiplayerUtility.performCheckAction(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadInt64());
					break;
				case 10:
					MultiplayerUtility.receiveChatMessage(msg.ReadString(), msg.ReadInt64());
					break;
				case 11:
					MultiplayerUtility.receiveNameChange(msg.ReadString(), msg.ReadInt64());
					break;
				case 12:
					MultiplayerUtility.receiveTenMinuteSync(msg.ReadInt16());
					break;
				case 13:
					MultiplayerUtility.receiveBuildingChange(msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadInt64(), msg.ReadInt64());
					break;
				case 14:
					MultiplayerUtility.performDebrisCreate(msg.ReadInt16(), msg.ReadInt32(), msg.ReadInt32(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), 0L);
					break;
				case 15:
					MultiplayerUtility.performNPCMove(msg.ReadInt32(), msg.ReadInt32(), msg.ReadInt64());
					break;
				case 16:
					MultiplayerUtility.performNPCBehavior(msg.ReadInt64(), msg.ReadByte());
					break;
				case 17:
					MultiplayerUtility.allFarmersReadyCheck();
					break;
				case 18:
					MultiplayerUtility.parseServerToClientsMessage(msg.ReadString());
					break;
				case 19:
					MultiplayerUtility.interpretMessageToEveryone(msg.ReadInt32(), msg.ReadString(), msg.ReadInt64());
					break;
				}
			}
		}
	}
}
