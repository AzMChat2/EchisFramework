using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Diagnostics.LoggerService
{
	#region AssemblyInfo
	/// <summary>
	/// The Assembly Information object contains information about an assembly.
	/// </summary>
	[Serializable]
	public sealed class AssemblyInfo : IXmlSerializable
	{

		/// <summary>
		/// Contains constants used by the AssemblyInfo class
		/// </summary>
		internal static class Constants
		{
			/// <summary>
			/// The Xml node name for the AssemblyInfo class
			/// </summary>
			public const string XmlNode = "AssemblyInfo";
			/// <summary>
			///  The Xml attribute name for the Name property.
			/// </summary>
			public const string XmlAttribName = "Name";
			/// <summary>
			/// The Xml attribute name for the Display property.
			/// </summary>
			public const string XmlAttribDisplay = "Display";
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public AssemblyInfo()
		{
			Classes = new ClassInfoDictionary();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The full name of the assembly.</param>
		/// <param name="displayName">The name to be displayed in the interface.</param>
		public AssemblyInfo(string name, string displayName)
		{
			Name = name;
			Display = displayName;
		}

		/// <summary>
		/// Gets the Trace Listner Class Information Collection.
		/// </summary>
		public ClassInfoDictionary Classes { get; private set; }

		/// <summary>
		/// Gets the name of the assembly.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the display name of the assembly.
		/// </summary>
		public string Display { get; private set; }

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
			Classes.WriteXml(writer);
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

			Name = reader.GetAttribute(Constants.XmlAttribName);
			Display = reader.GetAttribute(Constants.XmlAttribDisplay);
			Classes.ReadXml(reader);
		}

		#endregion
	}
	#endregion

	#region AssemblyInfoDictionary
	/// <summary>
	/// The Assembley Info Collection class represents a collection of Assembly Info objects.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Usage", "CA2229:ImplementSerializationConstructors",
		Justification = "Serialization constructor is a protected constructor; this is a sealed class with no possible derived classes.")]
	[SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly",
		Justification = "Base class (Dictionary<TKey, TValue>) correctly implements ISerializable, no further work is necessary.")]
	public sealed class AssemblyInfoDictionary : Dictionary<string, AssemblyInfo>, IXmlSerializable
	{
		/// <summary>
		/// Contains constants used by the AssemblyInfoDictionary class.
		/// </summary>
		private static class Constants
		{
			/// <summary>
			/// The configuration setting name in the App.Config file which contains the name of the Trace Listener Information file.
			/// </summary>
			public const string FilePathSetting = "TraceListenersFile";
		}

		private static string FilePath = ConfigurationManager.AppSettings[Constants.FilePathSetting];

		/// <summary>
		/// Serializes the Trace Listener Information to a file.
		/// </summary>
		/// <param name="infoObj">The object to be serialized.</param>
		public static void Serialize(AssemblyInfoDictionary infoObj)
		{
			XmlSerializer<AssemblyInfoDictionary>.SerializeToXmlFile(FilePath, infoObj);
		}

		/// <summary>
		/// Reads the Trace Listener Information File and generates an Assembly Information Collection.
		/// </summary>
		/// <returns></returns>
		public static AssemblyInfoDictionary Deserialize()
		{
			return XmlSerializer<AssemblyInfoDictionary>.DeserializeFromXmlFile(FilePath);
		}

		/// <summary>
		/// Adds an item to the dictionary.
		/// </summary>
		/// <param name="item">The item to be added to the dictionary.</param>
		public void Add(AssemblyInfo item)
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

			while (reader.Read())
			{
				if (reader.Name == AssemblyInfo.Constants.XmlNode)
				{
					AssemblyInfo info = new AssemblyInfo();
					info.ReadXml(reader);
					Add(info);
				}
			}
		}

		#endregion
	}
	#endregion
}
