using System;
using System.Net;

namespace StardewValley.Network
{
	public abstract class Client
	{
		public bool hasHandshaked;

		public string serverName = "???";

		public abstract bool isConnected
		{
			get;
		}

		public abstract float averageRoundtripTime
		{
			get;
		}

		public abstract IPAddress serverAddress
		{
			get;
		}

		public abstract void initializeConnection(string address);

		public abstract void receiveMessages();

		public abstract void sendMessage(byte which, object[] data);
	}
}
