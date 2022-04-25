using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Diagnostics.LoggerService
{
	#region ClassInfo
	/// <summary>
	/// The Trace Listener Class Information class represents information about a Trace Listener Class.
	/// </summary>
	[Serializable]
	public sealed class ClassInfo : IXmlSerializable
	{
		/// <summary>
		/// Contains constants used by the ClassInfo class
		/// </summary>
		internal static class Constants
		{
			/// <summary>
			/// The Xml node name for the ClassInfo class
			/// </summary>
			public const string XmlNode = "ClassInfo";
			/// <summary>
			///  The Xml attribute name for the Name property.
			/// </summary>
			public const string XmlAttribName = "Name";
			/// <summary>
			/// The Xml attribute name for the Display property.
			/// </summary>
			public const string XmlAttribDisplay = "Display";
			/// <summary>
			///  The Xml node name for a parameter.
			/// </summary>
			public const string XmlParamNode = "Parameter";
			/// <summary>
			/// The Xml attribute name for a parameter name.
			/// </summary>
			public const string XmlParamAttribName = "Name";
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public ClassInfo()
		{
			Parameters = new List<string>();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The full name of the assembly.</param>
		/// <param name="displayName">The name to be displayed in the interface.</param>
		/// <param name="parameters">The available parameter names.</param>
		public ClassInfo(string name, string displayName, params string[] parameters)
		{
			Name = name;
			Display = displayName;
			Parameters = new List<string>(parameters);

		}

		/// <summary>
		/// Gets the name of the Trace Listener Class.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the display name of the Trace Listener Class.
		/// </summary>
		public string Display { get; private set; }

		/// <summary>
		/// Gets the Initialization Parameters of the Trace Listener Class.
		/// </summary>
		public List<string> Parameters { get; private set; }

		#region IXmlSerializable Members

		/// <summary>
		/// Converts the members of an Assembly Information Collection to XML.
		/// </summary>
		/// <param name="writer">The writer used to write the XML.</param>
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			writer.WriteStartElement(Constants.XmlNode);
			writer.WriteAttributeString(Constants.XmlAttribName, Name);
			writer.WriteAttributeString(Constants.XmlAttribDisplay, Display);

			foreach (string param in Parameters)
			{
				writer.WriteStartElement(Constants.XmlParamNode);
				writer.WriteAttributeString(Constants.XmlParamAttribName, param);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		/// <summary>
		/// Not Used.
		/// </summary>
		/// <returns>Null.</returns>
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Converts XML into an object.
		/// </summary>
		/// <param name="reader">The reader containing the XML.</param>
		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			reader.ReadSubtree(ReadInnerXml);
		}

		private void ReadInnerXml(XmlReader reader)
		{
			if (reader.Read())
			{
				Name = reader.GetAttribute(Constants.XmlAttribName);
				Display = reader.GetAttribute(Constants.XmlAttribDisplay);

				reader.WhileReadIf(IsParamNode, AddParameter);
			}
		}

		private void AddParameter(XmlReader reader)
		{
			Parameters.Add(reader.GetAttribute(Constants.XmlParamAttribName));
		}

		private static bool IsParamNode(XmlReader reader)
		{
			return reader.IsStartElement(Constants.XmlParamNode);
		}

		#endregion
	}
	#endregion

	#region ClassInfoCollection
	/// <summary>
	/// The Trace Listener Class Information Collection represents a collection of Trace Listener Class Information objects.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Usage", "CA2229:ImplementSerializationConstructors",
		Justification = "Serialization constructor is a protected constructor; this is a sealed class with no possible derived classes.")]
	[SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly",
		Justification = "Base class (Dictionary<TKey, TValue>) correctly implements ISerializable, no further work is necessary.")]
	public sealed class ClassInfoDictionary : Dictionary<string, ClassInfo>, IXmlSerializable
	{
		/// <summary>
		/// Adds an Trace Listener Class Information object to the collection.
		/// </summary>
		/// <param name="item">The Trace Listener Class Information object to be added.</param>
		public void Add(ClassInfo item)
		{
			if (item == null) throw new ArgumentNullException("item");
			Add(item.Display, item);
		}

		#region IXmlSerializable Members

		/// <summary>
		/// Converts the members of an Assembly Information Collection to XML.
		/// </summary>
		/// <param name="writer">The writer used to write the XML.</param>
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			Values.ForEach(item => item.WriteXml(writer));
		}

		/// <summary>
		/// Not Used.
		/// </summary>
		/// <returns>Null.</returns>
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Converts XML into an object.
		/// </summary>
		/// <param name="reader">The reader containing the XML.</param>
		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			using (XmlReader innerReader = reader.ReadSubtree())
			{
				while (innerReader.Read())
				{
					if (innerReader.Name == ClassInfo.Constants.XmlNode)
					{
						ClassInfo info = new ClassInfo();
						info.ReadXml(innerReader);
						Add(info);
					}
				}
			}
		}

		#endregion
	}
	#endregion

}
