using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Xml
{
  /// <summary>
  /// Wraps an object in an XmlSerializable Wrapper
  /// </summary>
  /// <remarks>
  /// This class is used to serialize derived objects whose types may not be known at design time.
  /// </remarks>
  public class XmlWrapper : IXmlSerializable
  {
    /// <summary>
    /// Creates an instance of the XmlWrapper class.
    /// </summary>
    /// <typeparam name="T">The type of object to be wrapped</typeparam>
    /// <param name="value">The object to be wrapped</param>
    public static XmlWrapper<T> Create<T>(T value)
    {
      return new XmlWrapper<T>() { Value = value };
    }

    /// <summary>
    /// Gets or sets the wrapped object value.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Not used.
    /// </summary>
    public XmlSchema GetSchema()
    {
      return null;
    }

    /// <summary>
    /// Deserializes the wrapped object from the specified reader.
    /// </summary>
    public void ReadXml(XmlReader reader)
    {
      if (reader == null) throw new ArgumentNullException("reader");

      if (reader.IsEmptyElement)
      {
        reader.Read();
      }
      else
      {
        string typeName = reader.GetAttribute("Type");

        if (reader.Read())
        {
          Type type = Type.GetType(typeName, true, true);
          XmlSerializer serializer = new XmlSerializer(type);
          Value = serializer.Deserialize(reader);
        }

        reader.ReadEndElement();
      }
    }

    /// <summary>
    /// Serializes the wrapped object to the specified writer.
    /// </summary>
    public void WriteXml(XmlWriter writer)
    {
      if (writer == null) throw new ArgumentNullException("writer");

      if (Value != null)
      {
        Type type = Value.GetType();
        writer.WriteAttribute("Type", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", type.FullName, type.Assembly.GetName().Name));

        XmlSerializer serializer = new XmlSerializer(Value.GetType());
        serializer.Serialize(writer, Value);
      }
    }
  }

  /// <summary>
  /// Wraps an object in an XmlSerializable Wrapper
  /// </summary>
  /// <typeparam name="T">The type of object to be wrapped</typeparam>
  public class XmlWrapper<T> : XmlWrapper
  {
    /// <summary>
    /// Gets or sets the wrapped object value.
    /// </summary>
    new public T Value
    {
      get { return (T)base.Value; }
      set { base.Value = value; }
    }
  }

  /// <summary>
  /// Represents a list of Wrapped object.
  /// </summary>
  /// <typeparam name="T">The type of object to be wrapped</typeparam>
  [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", 
    Justification = "List is the appropriate suffix for this class")]
	public class XmlWrapperList<T> : List<XmlWrapper<T>>
  {
  }
}
