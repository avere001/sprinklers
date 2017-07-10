using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Network
{
	public abstract class Server
	{
		public const int messageSendDelay = 50;

		public const int defaultPort = 24642;

		public const int defaultMapServerPort = 24643;

		protected string serverName;

		public abstract int connectionsCount
		{
			get;
		}

		public Server(string name)
		{
			this.serverName = name;
		}

		public abstract void initializeConnection();

		public abstract void stopServer();

		public abstract void receiveMessages();

		public abstract void sendMessages(GameTime time);
	}
}
