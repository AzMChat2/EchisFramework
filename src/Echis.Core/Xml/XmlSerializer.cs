using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace System.Xml.Serialization
{
	/// <summary>
	/// Provides static methods for serializing and deserializing objects using Xml Serialization.
	/// </summary>
	/// <typeparam name="T">The type of object to be serialized or deserialized.</typeparam>
	public static class XmlSerializer<T>
	{
		/// <summary>
		/// The XmlSerializer object used to serialize and deserialize the specified type.
		/// </summary>
		/// <remarks>Access to the serializer is provided in order to attach listeners to the deserialization events.
		/// NOTE: Events must be attached after any of the SetSerializerAttributes methods are used.
		///       If none of the SetSerializerAttributes methods are used then the listeners may be attached at any time.</remarks>
		public static XmlSerializer Serializer { get; private set; }

		#region Initialization

		static XmlSerializer()
		{
			Serializer = new XmlSerializer(typeof(T));
		}

		/// <summary>
		/// Sets the Attributes of the Serializer
		/// </summary>
		public static void SetSerializerAttributes()
		{
			Serializer = new XmlSerializer(typeof(T));
		}

		/// <summary>
		/// Sets the Attributes of the Serializer
		/// </summary>
		/// <param name="defaultNamespace">The name to use for the default namespace.</param>
		public static void SetSerializerAttributes(string defaultNamespace)
		{
			Serializer = new XmlSerializer(typeof(T), defaultNamespace);
		}

		/// <summary>
		/// Sets the Attributes of the Serializer
		/// </summary>
		/// <param name="root">An XmlRootAttribute that represents the Xml Root Element.</param>
		public static void SetSerializerAttributes(XmlRootAttribute root)
		{
			Serializer = new XmlSerializer(typeof(T), root);
		}

		/// <summary>
		/// Sets the Attributes of the Serializer
		/// </summary>
		/// <param name="extraTypes">Type array of additional types to serialize</param>
		public static void SetSerializerAttributes(Type[] extraTypes)
		{
			Serializer = new XmlSerializer(typeof(T), extraTypes);
		}

		/// <summary>
		/// Sets the Attributes of the Serializer
		/// </summary>
		/// <param name="overrides">An XmlAttributesOverides object.</param>
		public static void SetSerializerAttributes(XmlAttributeOverrides overrides)
		{
			Serializer = new XmlSerializer(typeof(T), overrides);
		}

		/// <summary>
		/// Sets the Attributes of the Serializer
		/// </summary>
		/// <param name="overrides">An XmlAttributesOverides object.</param>
		/// <param name="extraTypes">Type array of additional types to serialize</param>
		/// <param name="root">An XmlRootAttribute that represents the Xml Root Element.</param>
		/// <param name="defaultNamespace">The name to use for the default namespace.</param>
		public static void SetSerializerAttributes(XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace)
		{
			Serializer = new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace);
		}

		/// <summary>
		/// Sets the Attributes of the Serializer
		/// </summary>
		/// <param name="overrides">An XmlAttributesOverides object.</param>
		/// <param name="extraTypes">Type array of additional types to serialize</param>
		/// <param name="root">An XmlRootAttribute that represents the Xml Root Element.</param>
		/// <param name="defaultNamespace">The name to use for the default namespace.</param>
		/// <param name="location">The location of the types.</param>
		public static void SetSerializerAttributes(XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace, string location)
		{
			Serializer = new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace, location);
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified Stream.
		/// </summary>
		/// <param name="stream">The Stream used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		public static void Serialize(Stream stream, T objToSerialize)
		{
			Serializer.Serialize(stream, objToSerialize);
		}

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		public static void Serialize(TextWriter writer, T objToSerialize)
		{
			Serializer.Serialize(writer, objToSerialize);
		}

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified XmlWriter.
		/// </summary>
		/// <param name="writer">The XmlWriter used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		public static void Serialize(XmlWriter writer, T objToSerialize)
		{
			Serializer.Serialize(writer, objToSerialize);
		}

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified Stream.
		/// </summary>
		/// <param name="stream">The Stream used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		/// <param name="namespaces">The XmlSerializerNamespaces refrenced by the object.</param>
		public static void Serialize(Stream stream, T objToSerialize, XmlSerializerNamespaces namespaces)
		{
			Serializer.Serialize(stream, objToSerialize, namespaces);
		}

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		/// <param name="namespaces">The XmlSerializerNamespaces refrenced by the object.</param>
		public static void Serialize(TextWriter writer, T objToSerialize, XmlSerializerNamespaces namespaces)
		{
			Serializer.Serialize(writer, objToSerialize, namespaces);
		}

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified XmlWriter.
		/// </summary>
		/// <param name="writer">The XmlWriter used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		/// <param name="namespaces">The XmlSerializerNamespaces refrenced by the object.</param>
		public static void Serialize(XmlWriter writer, T objToSerialize, XmlSerializerNamespaces namespaces)
		{
			Serializer.Serialize(writer, objToSerialize, namespaces);
		}

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified XmlWriter.
		/// </summary>
		/// <param name="writer">The XmlWriter used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		/// <param name="namespaces">The XmlSerializerNamespaces refrenced by the object.</param>
		/// <param name="encodingStyle">The encoding style of the serialized Xml.</param>
		public static void Serialize(XmlWriter writer, T objToSerialize, XmlSerializerNamespaces namespaces, string encodingStyle)
		{
			Serializer.Serialize(writer, objToSerialize, namespaces, encodingStyle);
		}

		/// <summary>
		/// Serializes the specified object and writes the Xml Document to the specified XmlWriter.
		/// </summary>
		/// <param name="writer">The XmlWriter used to write the Xml Document.</param>
		/// <param name="objToSerialize">The object to serialize</param>
		/// <param name="namespaces">The XmlSerializerNamespaces refrenced by the object.</param>
		/// <param name="encodingStyle">The encoding style of the serialized Xml.</param>
		/// <param name="id">For SOAP encoded messages, the base used to generate Id attributes.</param>
		public static void Serialize(XmlWriter writer, T objToSerialize, XmlSerializerNamespaces namespaces, string encodingStyle, string id)
		{
			Serializer.Serialize(writer, objToSerialize, namespaces, encodingStyle, id);
		}

		/// <summary>
		/// Serializes an object using Xml Serialization to an Xml string.
		/// </summary>
		/// <param name="objToSerialize">The object to be serialized.</param>
		public static string SerializeToXml(T objToSerialize)
		{
			return SerializeToXml(objToSerialize, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Serializes an object using Xml Serialization to an Xml string.
		/// </summary>
		/// <param name="objToSerialize">The object to be serialized.</param>
		/// <param name="formatProvider">The format provider object that provides formatting</param>
		public static string SerializeToXml(T objToSerialize, IFormatProvider formatProvider)
		{
			using (StringWriter writer = new StringWriter(formatProvider))
			{
				Serializer.Serialize(writer, objToSerialize);
				return writer.ToString();
			}
		}

		/// <summary>
		/// Serializes an object using Xml Serialization to an Xml file.
		/// </summary>
		/// <param name="fileName">The name of the file to be written.</param>
		/// <param name="objToSerialize">The object to be serialized.</param>
		public static void SerializeToXmlFile(string fileName, T objToSerialize)
		{
			using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Serialize(stream, objToSerialize);
			}
		}

		/// <summary>
		/// Serializes an object using Xml Serialization and a Deflation algorythm to a file.
		/// </summary>
		/// <param name="fileName">The name of the file to be written.</param>
		/// <param name="objToSerialize">The object to be serialized.</param>
		public static void SerializeToDeflatedXmlFile(string fileName, T objToSerialize)
		{
			using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Serialize(new DeflateStream(stream, CompressionMode.Compress), objToSerialize);
			}
		}

		/// <summary>
		/// Serializes an object using Xml Serialization and a Compression algorythm to a file.
		/// </summary>
		/// <param name="fileName">The name of the file to be written.</param>
		/// <param name="objToSerialize">The object to be serialized.</param>
		public static void SerializeToCompressedXmlFile(string fileName, T objToSerialize)
		{
			using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Serialize(new GZipStream(stream, CompressionMode.Compress), objToSerialize);
			}
		}

#endregion

		#region Deserialization

		/// <summary>
		/// Deserializes an object from the Xml Document contained in the specified Stream.
		/// </summary>
		/// <param name="stream">The Stream that contains the Xml Document to deserialize.</param>
		/// <returns>Returns the object represented by the Xml Document contained in the Stream.</returns>
		public static T Deserialize(Stream stream)
		{
			return (T)Serializer.Deserialize(stream);
		}

		/// <summary>
		/// Deserializes an object from the Xml Document contained in the specified TextReader.
		/// </summary>
		/// <param name="reader">The TextReader that contains the Xml Document to deserialize.</param>
		/// <returns>Returns the object represented by the Xml Document contained in the TextReader.</returns>
		public static T Deserialize(TextReader reader)
		{
			return (T)Serializer.Deserialize(reader);
		}

		/// <summary>
		/// Deserializes an object from the Xml Document contained in the specified XmlReader.
		/// </summary>
		/// <param name="reader">The XmlReader that contains the Xml Document to deserialize.</param>
		/// <returns>Returns the object represented by the Xml Document contained in the XmlReader.</returns>
		public static T Deserialize(XmlReader reader)
		{
			return (T)Serializer.Deserialize(reader);
		}

		/// <summary>
		/// Deserializes an object from the Xml Document contained in the specified XmlReader.
		/// </summary>
		/// <param name="reader">The XmlReader that contains the Xml Document to deserialize.</param>
		/// <param name="encodingStyle">The encoding style of the serialized Xml.</param>
		/// <returns>Returns the object represented by the Xml Document contained in the XmlReader.</returns>
		public static T Deserialize(XmlReader reader, string encodingStyle)
		{
			return (T)Serializer.Deserialize(reader, encodingStyle);
		}

		/// <summary>
		/// Deserializes an object from the Xml Document contained in the specified XmlReader.
		/// </summary>
		/// <param name="reader">The XmlReader that contains the Xml Document to deserialize.</param>
		/// <param name="events">An instance of the XmlDeserializationEvents class.</param>
		/// <returns>Returns the object represented by the Xml Document contained in the XmlReader.</returns>
		public static T Deserialize(XmlReader reader, XmlDeserializationEvents events)
		{
			return (T)Serializer.Deserialize(reader, events);
		}

		/// <summary>
		/// Deserializes an object from the Xml Document contained in the specified XmlReader.
		/// </summary>
		/// <param name="reader">The XmlReader that contains the Xml Document to deserialize.</param>
		/// <param name="encodingStyle">The encoding style of the serialized Xml.</param>
		/// <param name="events">An instance of the XmlDeserializationEvents class.</param>
		/// <returns>Returns the object represented by the Xml Document contained in the XmlReader.</returns>
		public static T Deserialize(XmlReader reader, string encodingStyle, XmlDeserializationEvents events)
		{
			return (T)Serializer.Deserialize(reader, encodingStyle, events);
		}

		/// <summary>
		/// Deserializes an object using Xml Serialization from an Xml string.
		/// </summary>
		/// <param name="xmlData">The Xml string to be deserialized.</param>
		/// <returns>Returns the deserialized object from the Xml string.</returns>
		public static T DeserializeFromXml(string xmlData)
		{
			using (StringReader reader = new StringReader(xmlData))
			{
				return (T)Serializer.Deserialize(reader);
			}
		}

		/// <summary>
		/// Deserializes an object using Xml Serialization from an Xml file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>Returns the deserialized object from the file.</returns>
		public static T DeserializeFromXmlFile(string fileName)
		{
			using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				return Deserialize(stream);
			}
		}

		/// <summary>
		/// Deserializes an object using Xml Serialization from an embedded resource.
		/// </summary>
		/// <param name="assembly">The Assembly in which the resource is stored.</param>
		/// <param name="resourceName">The fully qualified name of the resource.</param>
		/// <returns>Returns the deserialized object from the file.</returns>
		public static T DeserializeFromResource(Assembly assembly, string resourceName)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				return Deserialize(stream);
			}
		}

		/// <summary>
		/// Deserializes an object using Xml Serialization and a Deflation algorythm from a file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>Returns the deserialized object from the file.</returns>
		public static T DeserializeFromDeflatedXmlFile(string fileName)
		{
			using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				return Deserialize(new DeflateStream(stream, CompressionMode.Decompress));
			}
		}

		/// <summary>
		/// Deserializes an object using Xml Serialization and a Deflation algorythm from an assembly resource.
		/// </summary>
		/// <param name="assembly">The Assembly in which the resource is stored.</param>
		/// <param name="resourceName">The fully qualified name of the resource.</param>
		/// <returns>Returns the deserialized object from the file.</returns>
		public static T DeserializeFromDeflatedResource(Assembly assembly, string resourceName)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				return Deserialize(new DeflateStream(stream, CompressionMode.Decompress));
			}
		}

		/// <summary>
		/// Deserializes an object using Xml Serialization and a Compression algorythm from a file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>Returns the deserialized object from the file.</returns>
		public static T DeserializeFromCompressedXmlFile(string fileName)
		{
			using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				return Deserialize(new GZipStream(stream, CompressionMode.Decompress));
			}
		}

		/// <summary>
		/// Deserializes an object using Xml Serialization and a Compression algorythm from an assembly resource.
		/// </summary>
		/// <param name="assembly">The Assembly in which the resource is stored.</param>
		/// <param name="resourceName">The fully qualified name of the resource.</param>
		/// <returns>Returns the deserialized object from the file.</returns>
		public static T DeserializeFromCompressedResource(Assembly assembly, string resourceName)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				return Deserialize(new GZipStream(stream, CompressionMode.Decompress));
			}
		}

		#endregion

		/// <summary>
		/// Gets a value that indicates if the XmlSerializer can deserialize the Xml Document contained in the specified XmlReader.
		/// </summary>
		/// <param name="reader">An XmlReader that contains the Xml Document to deserialize.</param>
		/// <returns>Returns true if the XmlSerializer can deserialize the Xml Document contained in the specified XmlReader.</returns>
		public static bool CanDeserialize(XmlReader reader)
		{
			return Serializer.CanDeserialize(reader);
		}

	}
}
