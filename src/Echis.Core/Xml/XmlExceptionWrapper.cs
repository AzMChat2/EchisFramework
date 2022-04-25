using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Xml
{
	/// <summary>
	/// Wraps an Exception in an Xml Serializable container
	/// </summary>
	public class XmlExceptionWrapper : IXmlSerializable
	{
		/// <summary>
		/// The Exception to be serialized
		/// </summary>
		public Exception Value { get; set; }

		/// <summary>
		/// Not Used.
		/// </summary>
		public XmlSchema GetSchema() { return null; }

		/// <summary>
		/// Reads serialization data to reconstruct the Exception.
		/// </summary>
		/// <param name="reader">The reader containing the serialized exception.</param>
		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			if (reader.IsEmptyElement)
			{
				reader.Read();
			}
			else
			{
				string data = reader.ReadString();
				if (!string.IsNullOrWhiteSpace(data))
				{
					IFormatter serializer = new BinaryFormatter();
					using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(data)))
					{
						Value = serializer.Deserialize(stream) as Exception;
					}
				}

				reader.ReadEndElement();
			}
		}

		/// <summary>
		/// Writes serialization data to the specified Xml Writer
		/// </summary>
		/// <param name="writer">The writer to which the serialized exception data will be written.</param>
		[SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes",
			Justification = "Using System.Exception only to serialize a non Serializable Exception's information.")]
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			if (Value != null)
			{
				ISerializable exception = Value as ISerializable;

				if (exception == null) exception = new Exception(Value.Message, Value.InnerException);

				IFormatter serializer = new BinaryFormatter();
				using (MemoryStream stream = new MemoryStream())
				{
					serializer.Serialize(stream, exception);
					writer.WriteString(Convert.ToBase64String(stream.ToArray()));
				}
			}
		}
	}

}
