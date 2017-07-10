using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace StardewValley.Network
{
	public class GetMapClient
	{
		public static void receiveMapFromServer(GameLocation map, bool isStructure)
		{
			long num = DateTime.Now.Ticks / 10000L;
			byte[] array = new byte[4];
			IPEndPoint remoteEP = new IPEndPoint(Game1.client.serverAddress, 24643);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(remoteEP);
			Console.WriteLine("Socket connected to {0}", socket.RemoteEndPoint.ToString());
			byte[] bytes = Encoding.ASCII.GetBytes((isStructure ? map.uniqueName : map.name) + "_" + isStructure.ToString() + "_<EOF>");
			socket.Send(bytes);
			socket.Receive(array);
			array = new byte[BitConverter.ToInt32(array, 0)];
			socket.Receive(array);
			GameLocation gameLocation = (GameLocation)GetMapClient.FromXml(Encoding.ASCII.GetString(array), typeof(GameLocation));
			map.terrainFeatures = gameLocation.terrainFeatures;
			using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator = map.terrainFeatures.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.loadSprite();
				}
			}
			map.objects = gameLocation.objects;
			using (Dictionary<Vector2, StardewValley.Object>.ValueCollection.Enumerator enumerator2 = map.objects.Values.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.reloadSprite();
				}
			}
			map.characters = gameLocation.characters;
			if (gameLocation is Farm && map is Farm)
			{
				(map as Farm).buildings = (gameLocation as Farm).buildings;
				using (List<Building>.Enumerator enumerator3 = (map as Farm).buildings.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						enumerator3.Current.load();
					}
				}
				(map as Farm).animals = (gameLocation as Farm).animals;
				foreach (KeyValuePair<long, FarmAnimal> current in (map as Farm).animals)
				{
					current.Value.reload();
				}
			}
			using (List<NPC>.Enumerator enumerator5 = map.characters.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					enumerator5.Current.reloadSprite();
				}
			}
			Game1.player.remotePosition = Game1.player.position;
			Console.WriteLine("Time: " + (DateTime.Now.Ticks / 10000L - num));
			socket.Close();
		}

		public static object FromXml(string Xml, Type ObjType)
		{
			StringReader stringReader = new StringReader(Xml);
			XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
			object arg_25_0 = SaveGame.locationSerializer.Deserialize(xmlTextReader);
			xmlTextReader.Close();
			stringReader.Close();
			return arg_25_0;
		}
	}
}
