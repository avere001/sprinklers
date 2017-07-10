using System;
using System.Net.Sockets;
using System.Text;

namespace StardewValley.Network
{
	public class StateObject
	{
		public Socket workSocket;

		public const int BufferSize = 1024;

		public byte[] buffer = new byte[1024];

		public StringBuilder sb = new StringBuilder();
	}
}
