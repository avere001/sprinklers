using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StardewValley
{
	[XmlRoot("dictionary")]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable, INotifyCollectionChanged
	{
		[method: CompilerGenerated]
		[CompilerGenerated]
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public XmlSchema GetSchema()
		{
			return null;
		}

		public new void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key, 0));
		}

		public new bool Remove(TKey key)
		{
			bool arg_1A_0 = base.Remove(key);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key, 0));
			return arg_1A_0;
		}

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(null, e);
			}
		}

		public void ReadXml(XmlReader reader)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			bool arg_2D_0 = reader.IsEmptyElement;
			reader.Read();
			if (arg_2D_0)
			{
				return;
			}
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				reader.ReadStartElement("key");
				TKey key = (TKey)((object)xmlSerializer.Deserialize(reader));
				reader.ReadEndElement();
				reader.ReadStartElement("value");
				TValue value = (TValue)((object)xmlSerializer2.Deserialize(reader));
				reader.ReadEndElement();
				this.Add(key, value);
				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			foreach (TKey current in base.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				xmlSerializer.Serialize(writer, current);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				TValue tValue = base[current];
				xmlSerializer2.Serialize(writer, tValue);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
	}
}
