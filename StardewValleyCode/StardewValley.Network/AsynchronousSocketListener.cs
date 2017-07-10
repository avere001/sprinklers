using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace StardewValley.Network
{
	public class AsynchronousSocketListener
	{
		public static ManualResetEvent allDone = new ManualResetEvent(false);

		public static bool active = true;

		public static void StartListening()
		{
			new byte[1024];
			IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 24643);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				socket.Bind(localEP);
				socket.Listen(16);
				while (AsynchronousSocketListener.active)
				{
					AsynchronousSocketListener.allDone.Reset();
					Console.WriteLine("Waiting for a connection...");
					socket.BeginAccept(new AsyncCallback(AsynchronousSocketListener.AcceptCallback), socket);
					AsynchronousSocketListener.allDone.WaitOne();
				}
			}
			catch (Exception arg_72_0)
			{
				Console.WriteLine(arg_72_0.ToString());
			}
			Console.WriteLine("\nPress ENTER to continue...");
			Console.Read();
		}

		public static void AcceptCallback(IAsyncResult ar)
		{
			AsynchronousSocketListener.allDone.Set();
			Socket socket = ((Socket)ar.AsyncState).EndAccept(ar);
			StateObject stateObject = new StateObject();
			stateObject.workSocket = socket;
			socket.BeginReceive(stateObject.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.ReadCallback), stateObject);
		}

		public static void ReadCallback(IAsyncResult ar)
		{
			string text = string.Empty;
			StateObject stateObject = (StateObject)ar.AsyncState;
			Socket workSocket = stateObject.workSocket;
			int num = workSocket.EndReceive(ar);
			if (num > 0)
			{
				stateObject.sb.Append(Encoding.ASCII.GetString(stateObject.buffer, 0, num));
				text = stateObject.sb.ToString();
				if (text.IndexOf("<EOF>") > -1)
				{
					Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", text.Length, text);
					AsynchronousSocketListener.Send(workSocket, text);
					return;
				}
				workSocket.BeginReceive(stateObject.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.ReadCallback), stateObject);
			}
		}

		private static void Send(Socket handler, string data)
		{
			string text = data.Remove(data.IndexOf("<EOF>"));
			data = AsynchronousSocketListener.ToXml(Game1.getLocationFromName(text.Split(new char[]
			{
				'_'
			})[0], Convert.ToBoolean(text.Split(new char[]
			{
				'_'
			})[1])), typeof(GameLocation));
			byte[] bytes = Encoding.ASCII.GetBytes(data);
			byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
			handler.Send(bytes2);
			handler.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.SendCallback), handler);
		}

		public static string ToXml(object Obj, Type ObjType)
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings
			{
				CloseOutput = true
			});
			xmlWriter.WriteStartDocument();
			SaveGame.locationSerializer.Serialize(xmlWriter, Obj);
			xmlWriter.WriteEndDocument();
			xmlWriter.Flush();
			xmlWriter.Close();
			memoryStream.Close();
			string text = Encoding.UTF8.GetString(memoryStream.GetBuffer());
			text = text.Substring(text.IndexOf(Convert.ToChar(60)));
			return text.Substring(0, text.LastIndexOf(Convert.ToChar(62)) + 1);
		}

		private static void SendCallback(IAsyncResult ar)
		{
			try
			{
				Socket expr_0B = (Socket)ar.AsyncState;
				int num = expr_0B.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to client.", num);
				expr_0B.Shutdown(SocketShutdown.Both);
				expr_0B.Close();
			}
			catch (Exception arg_31_0)
			{
				Console.WriteLine(arg_31_0.ToString());
			}
		}
	}
}
