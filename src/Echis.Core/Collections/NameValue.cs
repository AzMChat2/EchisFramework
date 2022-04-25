using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Globalization;
using System.IO;

namespace System.Collections
{
	/// <summary>
	/// Represents a Name and a Value, used by the NameValueList
	/// </summary>
	[Serializable]
	[XmlType("NameValue")]
	public class NameValue
	{
		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		[XmlElement]
		public virtual string Name { get; set; }
		/// <summary>
		/// Gets or sets the Value.
		/// </summary>
		[XmlElement]
		public virtual object Value { get; set; }

		/// <summary>
		/// Serializes the NameValue object to an XmlWriter
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="nodeName"></param>
		protected internal virtual void WriteXml(XmlWriter writer, string nodeName)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			writer.WriteStartElement(nodeName);
			writer.WriteAttribute("Name", Name);

			if (Value != null)
			{
				Type type = Value.GetType();
				writer.WriteAttributeString("Type", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", type.FullName, type.Assembly.GetName().Name));

				XmlSerializer serializer = new XmlSerializer(type);
				serializer.Serialize(writer, Value);
			}

			writer.WriteEndElement();
		}
	}

	/// <summary>
	/// Represents a Name and a Value, used by the NameValueList
	/// </summary>
	/// <typeparam name="T">The type of the Value.</typeparam>
	[Serializable]
	[XmlType("NameValue")]
	public class NameValue<T> : NameValue
	{
		/// <summary>
		/// Gets or sets the Value
		/// </summary>
		[XmlElement]
		new public virtual T Value
		{
			get { return (T)base.Value; }
			set { base.Value = value; }
		}
	}
}
