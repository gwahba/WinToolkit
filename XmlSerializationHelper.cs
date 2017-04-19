using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WinToolkit
{
	public class XmlSerializationHelper
	{
		public static TBaseXmlType LoadSerializableXmlObject<TBaseXmlType>(String fileName) where TBaseXmlType : new()
		{
			if (!File.Exists(fileName)) return default(TBaseXmlType);
			return LoadSerializableXmlObjectStream<TBaseXmlType>(new StreamReader(fileName));
		}

		public static bool SaveSerializableXmlObject<TBaseXmlType>(TBaseXmlType objToWrite, string fileName)
		{
			try
			{
				XmlWriterSettings logSettings = new XmlWriterSettings { IndentChars = "\t", Indent = true, OmitXmlDeclaration = true };
				XmlSerializer serializer = new XmlSerializer(typeof(TBaseXmlType));

				XmlWriter writer = XmlWriter.Create(fileName, logSettings);
				serializer.Serialize(writer, objToWrite, GetXmlSerializerNamespaces());
				writer.Close();
			}
			catch (Exception ex)
			{
				return false;
			}

			return true;
		}

		private static XmlSerializerNamespaces GetXmlSerializerNamespaces()
		{
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");
			return ns;
		}

		public static string GetSerializableXmlString<TBaseXmlType>(TBaseXmlType objToWrite)
		{
			try
			{
				XmlWriterSettings logSettings = new XmlWriterSettings { IndentChars = "\t", Indent = true, OmitXmlDeclaration = true };
				XmlSerializer serializer = new XmlSerializer(typeof(TBaseXmlType));
				using (StringWriter textWriter = new StringWriter())
				{
					using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, logSettings))
					{
						serializer.Serialize(xmlWriter, objToWrite, GetXmlSerializerNamespaces());
					}
					return textWriter.ToString();
				}

			}
			catch (Exception ex)
			{
				return null;
			}
		}
		private static TBaseXmlType LoadSerializableXmlObjectStream<TBaseXmlType>(StreamReader stream) where TBaseXmlType : new()
		{
			TBaseXmlType loadedObj = default(TBaseXmlType);
			XmlSerializer deserializer = new XmlSerializer(typeof(TBaseXmlType));
			System.Threading.Thread.Sleep(10);
			//Stream stream =stream
			StreamReader reader = stream;

			try
			{
				loadedObj = (TBaseXmlType)deserializer.Deserialize(reader);
			}
			catch (Exception e)
			{
			}

			reader.Close();
			return loadedObj;
		}
		public static TBaseXmlType LoadSerializableXmlObjectFromString<TBaseXmlType>(String xmlString) where TBaseXmlType : new()
		{

			Stream stream = GenerateStreamFromString(xmlString.Replace("<xml>", "").Replace("</xml>", ""));
			return LoadSerializableXmlObjectStream<TBaseXmlType>(new StreamReader(stream));

		}
		public static Stream GenerateStreamFromString(string s)
		{
			//if (s == null) return null;
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
		public static string GenerateStringFromStream(Stream s)
		{
			MemoryStream stream = new MemoryStream();
			StreamReader reader = new StreamReader(s, Encoding.UTF8);
			return reader.ReadToEnd();
		}


	}
}
