using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Diagnostics;

namespace System.Collections
{
	/// <summary>
	/// The NameValue list represents a collection of NameValue objects.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification="List is the correct suffix for this class")]
	public class NameValueList : List<NameValue>, IXmlSerializable
	{
		/// <summary>
		/// Stores constructors used to create NameValue objects.
		/// </summary>
		private static Dictionary<Type, ConstructorInfo> _constructors = new Dictionary<Type, ConstructorInfo>();

		/// <summary>
		/// Static Constructor.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
			Justification="Unable to fully intitialize (including adding elements) constructors dictionary inline")]
		static NameValueList()
		{
			// Add value types
			_constructors.Add(typeof(bool), typeof(NameValue<bool>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(short), typeof(NameValue<short>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(int), typeof(NameValue<int>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(long), typeof(NameValue<long>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(float), typeof(NameValue<float>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(double), typeof(NameValue<double>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(decimal), typeof(NameValue<decimal>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(string), typeof(NameValue<string>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(char), typeof(NameValue<char>).GetConstructor(Type.EmptyTypes));
			_constructors.Add(typeof(DateTime), typeof(NameValue<DateTime>).GetConstructor(Type.EmptyTypes));

			_genericConstructor = typeof(NameValue).GetConstructor(Type.EmptyTypes);
		}

		/// <summary>
		/// Generic constructor used to create NameValue objects for non-primitive types.
		/// </summary>
		private static ConstructorInfo _genericConstructor;

		/// <summary>
		/// Retrieves the constructor used to create NameValue object for the specified value.
		/// </summary>
		/// <param name="value">The value which the NameValue object will contain.</param>
		/// <returns>Returns the constructor used to create NameValue object for the specified value.</returns>
		private static ConstructorInfo GetConstructor(object value)
		{
			ConstructorInfo retVal = _genericConstructor;

			if (value != null)
			{
				Type type = value.GetType();
				if (_constructors.ContainsKey(type)) retVal = _constructors[type];
			}

			return retVal;
		}

		/// <summary>
		/// Initializes a new instance of the NameValueList class that is empty and has the default initial capacity.
		/// </summary>
		public NameValueList() : base() { }
		/// <summary>
		/// Initializes a new instance of the NameValueList class that contains elements copied from the specified collection
		/// and has sufficient capacity to accomodate the number of elements copied.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		public NameValueList(IEnumerable<NameValue> collection) : base(collection) { }
		/// <summary>
		/// Initializes a new instance of the NameValueList class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new list can initially store.</param>
		public NameValueList(int capacity) : base(capacity) { }
		/// <summary>
		/// Initializes a new instance of the NameValueList class that contains elements created from the specified arrays.
		/// </summary>
		/// <param name="names">An array of names for the NameValue objects</param>
		/// <param name="values">An array of values for the NameValue objects</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods",
			Justification = "Parameters are validated by AddRange method.")]
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
			Justification = "Reviewed.  Call to virtual method AddRange is acceptable.")]
		public NameValueList(string[] names, object[] values)
			: base(names.Length)
		{
			AddRange(names, values);
		}

		#region IXmlSerializable Members

		/// <summary>
		/// Not Used.
		/// </summary>
		/// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Deserializes the NameValueList from an XmlReader.
		/// </summary>
		/// <param name="reader"></param>
		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			string elementName = reader.Name;

      if (reader.IsEmptyElement)
      {
        reader.Read();
      }
      else
			{
				while (reader.Read())
				{
					if (reader.Name == elementName) break;
					if (reader.IsStartElement("Item")) Add(reader);
				}

        reader.ReadEndElement();
      }
		}

		/// <summary>
		/// Serializes the NameValueList to an XmlWriter
		/// </summary>
		/// <param name="writer"></param>
		public void WriteXml(XmlWriter writer)
		{
			ForEach(item => item.WriteXml(writer, "Item"));
		}

		#endregion

		/// <summary>
		/// Gets the NameValue object by name.
		/// </summary>
		/// <param name="name">The name of the NameValue object to retrieve.</param>
		public virtual NameValue this[string name]
		{
			get { return Find(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }
		}

		/// <summary>
		/// Creates and adds new NameValue objects using the array of names and array of values.
		/// </summary>
		/// <param name="names">An array of names for the NameValue objects</param>
		/// <param name="values">An array of values for the NameValue objects</param>
		public virtual void AddRange(string[] names, object[] values)
		{
			if (names == null) throw new ArgumentNullException("names");
			if (values == null) throw new ArgumentNullException("values");
			if (names.Length != values.Length) throw new ArgumentException("Attribute Names array and Attribute Values array lengths are different.");
			for (int idx = 0; idx < names.Length; idx++)
			{
				Add(names[idx], values[idx]);
			}
		}

		/// <summary>
		/// Adds NameValue objects from an XmlReader
		/// </summary>
		/// <param name="reader"></param>
		protected virtual void Add(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			object value = null;
			string name = reader.GetAttribute("Name");
			string typeName = reader.GetAttribute("Type");

			if (!string.IsNullOrWhiteSpace(typeName))
			{
				Type type = Type.GetType(typeName, true, true);

				if (reader.Read())
				{
						XmlSerializer serializer = new XmlSerializer(type);
						value = serializer.Deserialize(reader);
				}
			}

			Add(name, value);
		}

		/// <summary>
		/// Creates and adds a new NameValue object using the specified name and value.
		/// </summary>
		/// <param name="name">The name of the NameValue object.</param>
		/// <param name="value">The value of the NameValue object.</param>
		public virtual void Add(string name, object value)
		{
			NameValue item = GetConstructor(value).Invoke(null) as NameValue;
			item.Name = name;
			item.Value = value;
			Add(item);
		}

		/// <summary>
		/// Creates and adds a new NameValue object using the specified name and value.
		/// </summary>
		/// <param name="name">The name of the NameValue object.</param>
		/// <param name="value">The value of the NameValue object.</param>
		public virtual void Add<T>(string name, T value)
		{
			Add(new NameValue<T>() { Name = name, Value = value });
		}

		/// <summary>
		/// Determines if the NameValueList contains a NameValue object with the specified name.
		/// </summary>
		/// <param name="name">The name of the NameValue object to search for.</param>
		/// <returns>Returns true if a NameValue object exists with the specified name.</returns>
		public virtual bool Contains(string name)
		{
			return Exists(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Validates that the a NameValue object exists with the specified name and has a value of the specified type.
		/// </summary>
		/// <typeparam name="T">The type of the value expected in the NameValue object.</typeparam>
		/// <param name="name">The name of the NameValue object to search for.</param>
		/// <returns>Returns null if the NameValue object exists and the value is of the type expected.
		/// Returns a string containing a validation error message if the object does not exist or if the value is not of the expected type.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual string Validate<T>(string name)
		{
			NameValue attribute = this[name];
			string retVal = null;

			if (attribute == null)
			{
				retVal = string.Format(CultureInfo.InvariantCulture,
					"The specified attribute '{0}' was not found.", name);
			}
			else if (!(attribute is NameValue<T>))
			{
				retVal = string.Format(CultureInfo.InvariantCulture,
					"The specified attribute '{0}' is not the expected value type '{1}'.", name, typeof(T).Name);
			}

			return retVal;
		}

				/// <summary>
		/// Gets the value of the NameValue object for the specified name.
		/// </summary>
		/// <typeparam name="T">The type of the value expected in the NameValue object.</typeparam>
		/// <param name="name">The name of the NameValue object to search for.</param>
		/// <returns>Gets the value of the NameValue object for the specified name.</returns>
		/// <exception cref="System.InvalidCastException">Thrown when the value is not of the expected type.</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetValue<T>(string name)
		{
			return GetValue<T>(name, false);
		}

		/// <summary>
		/// Gets the value of the NameValue object for the specified name.
		/// </summary>
		/// <typeparam name="T">The type of the value expected in the NameValue object.</typeparam>
		/// <param name="name">The name of the NameValue object to search for.</param>
		/// <param name="autoConvert">A boolean value indicating if the value should be automatically converted to the requested return type.
		/// If false, an InvalidCastException will be thrown if the return type is not the attribute type.</param>
		/// <returns>Gets the value of the NameValue object for the specified name.</returns>
		/// <exception cref="System.InvalidCastException">Thrown when the value is not of the expected type.</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetValue<T>(string name, bool autoConvert)
		{
			NameValue attribute = this[name];
			NameValue<T> typedAttribute = attribute as NameValue<T>;

			if ((attribute == null) || (attribute.Value == null))
			{
				return default(T);
			}
			else if (typedAttribute == null)
			{
				if (autoConvert)
				{
					return (T)Convert.ChangeType(attribute.Value, typeof(T), CultureInfo.InvariantCulture);
				}
				else
				{
					throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture,
						"The specified attribute '{0}' is not the expected value type '{1}'.", name, typeof(T).Name));
				}
			}
			else
			{
				return typedAttribute.Value;
			}
		}

		/// <summary>
		/// Sets the value of the NameValue object for the specified name.
		/// </summary>
		/// <typeparam name="T">The type of the value of the NameValue object.</typeparam>
		/// <param name="name">The name of the NameValue object whose value is to be set..</param>
		/// <param name="value">The new value for the NameValue object</param>
		/// <remarks>If the NameValue object does not exist, it will be created with its initial value set to the specified value.</remarks>
		public virtual void SetValue<T>(string name, T value)
		{
			RemoveAll(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			Add(name, value);
		}
	}
}
