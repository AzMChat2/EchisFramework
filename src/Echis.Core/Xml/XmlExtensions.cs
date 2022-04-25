using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Xml
{
	/// <summary>
	/// The XmlUtilities class provides basic Xml Utilities.
	/// </summary>
	public static class XmlExtensions
	{
		/// <summary>
		/// The Constants class contains all constants used in the XmlUtilities class.
		/// </summary>
		private static class Constants
		{
			public const string DateTimeFormat = "G";
		}

		/// <summary>
		/// Uses an XmlReader to read the contents of the current Xml Node.
		/// </summary>
		/// <param name="reader">The reader containing the element to be read.</param>
		/// <param name="action">The action to be performed with the inner XmlReader.</param>
		public static void ReadSubtree(this XmlReader reader, Action<XmlReader> action)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (action == null) throw new ArgumentNullException("action");

			using (XmlReader innerReader = reader.ReadSubtree())
			{
				action(innerReader);
			}
			reader.Read();
		}

		/// <summary>
		/// Executes an action on an XmlReader for as long as the XmlReader returns results.
		/// </summary>
		/// <param name="reader">The reader containing the xml data to be read.</param>
		/// <param name="action">The action to be performed.</param>
		public static void WhileRead(this XmlReader reader, Action<XmlReader> action)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (action == null) throw new ArgumentNullException("action");

			while (reader.Read()) action(reader);
		}

		/// <summary>
		/// Executes an action on an XmlReader for as long as the XmlReader returns results.
		/// </summary>
		/// <param name="reader">The reader containing the xml data to be read.</param>
		/// <param name="validator">The validator to be used to test the Xml Data.</param>
		/// <param name="action">The action to be performed.</param>
		public static void WhileReadIf(this XmlReader reader, Predicate<XmlReader> validator, Action<XmlReader> action)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (validator == null) throw new ArgumentNullException("validator");
			if (action == null) throw new ArgumentNullException("action");

			while (reader.Read()) if (validator(reader)) action(reader);
		}

		/// <summary>
		/// Executes an action on an XmlReader for as long as the validator returns true.
		/// </summary>
		/// <param name="reader">The reader containing the xml data to be read.</param>
		/// <param name="validator">The validator to be used to test the Xml Data.</param>
		/// <param name="action">The action to be performed.</param>
		public static void WhileTrue(this XmlReader reader, Predicate<XmlReader> validator, Action<XmlReader> action)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (validator == null) throw new ArgumentNullException("validator");
			if (action == null) throw new ArgumentNullException("action");

			while (validator(reader)) action(reader);
		}

		/// <summary>
		/// Moves to the start of the next Xml Element with the specified name.
		/// </summary>
		/// <param name="reader">The reader containing the xml data to be read.</param>
		/// <param name="name">The name of the element to find.</param>
		/// <returns>Returns true if the an Element with the specified name has been found.</returns>
		/// <remarks>This is similar to XmlReader.ReadToFollowing except that this method will
		/// first check to see if the current node is a Start Element with the specified name.</remarks>
		public static bool MoveToStartElement(this XmlReader reader, string name)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			bool retVal = reader.IsStartElement(name);

			if (!retVal) retVal = reader.ReadToFollowing(name);

			return retVal;


			//bool retVal = false;

			//while(!(retVal = reader.IsStartElement(name)) && reader.Read()) {}

			//return retVal;
		}


		#region XmlWriter Utility Functions
		/// <summary>
		/// Writes an attribute or an element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="attribute">A flag indicating if an Xml Element or an Xml Attribute will be written.</param>
		/// <param name="writer">The XmlWriter to which the Element or Attribute will be written.</param>
		/// <param name="name">The name of the Element or Attribute</param>
		/// <param name="value">The value of the Element or Attribute</param>
		/// <param name="delimiter">The delimiter to be used to join the string array.</param>
		private static void WriteIfNotNull(XmlWriter writer, bool attribute, string name, string[] value, char delimiter)
		{
			if ((value != null) && (value.Length != 0))
			{
				WriteIfNotNull(writer, attribute, name, string.Join(new string(delimiter, 1), value));
			}
		}

		/// <summary>
		/// Writes an attribute or an element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="attribute">A flag indicating if an Xml Element or an Xml Attribute will be written.</param>
		/// <param name="writer">The XmlWriter to which the Element or Attribute will be written.</param>
		/// <param name="name">The name of the Element or Attribute</param>
		/// <param name="value">The value of the Element or Attribute</param>
		private static void WriteIfNotNull(XmlWriter writer, bool attribute, string name, string value)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			if (value != null)
			{
				if (attribute)
				{
					writer.WriteAttributeString(name, value);
				}
				else
				{
					writer.WriteElementString(name, value);
				}
			}
		}

		/// <summary>
		/// Writes an attribute or an element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="attribute">A flag indicating if an Xml Element or an Xml Attribute will be written.</param>
		/// <param name="writer">The XmlWriter to which the Element or Attribute will be written.</param>
		/// <param name="name">The name of the Element or Attribute</param>
		/// <param name="value">The value of the Element or Attribute</param>
		private static void WriteIfNotNull(XmlWriter writer, bool attribute, string name, bool? value)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			if (value.HasValue)
			{
				string formattedValue = value.Value ? "1" : "0";

				if (attribute)
				{
					writer.WriteAttributeString(name, formattedValue);
				}
				else
				{
					writer.WriteElementString(name, formattedValue);
				}
			}
		}

		/// <summary>
		/// Writes an attribute or an element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="attribute">A flag indicating if an Xml Element or an Xml Attribute will be written.</param>
		/// <param name="writer">The XmlWriter to which the Element or Attribute will be written.</param>
		/// <param name="name">The name of the Element or Attribute</param>
		/// <param name="value">The value of the Element or Attribute</param>
		private static void WriteIfNotNull(XmlWriter writer, bool attribute, string name, int? value)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			if (value.HasValue)
			{
				if (attribute)
				{
					writer.WriteAttributeString(name, value.Value.ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					writer.WriteElementString(name, value.Value.ToString(CultureInfo.InvariantCulture));
				}
			}
		}

		/// <summary>
		/// Writes an attribute or an element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="attribute">A flag indicating if an Xml Element or an Xml Attribute will be written.</param>
		/// <param name="writer">The XmlWriter to which the Element or Attribute will be written.</param>
		/// <param name="name">The name of the Element or Attribute</param>
		/// <param name="value">The value of the Element or Attribute</param>
		private static void WriteIfNotNull(XmlWriter writer, bool attribute, string name, DateTime? value)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			if (value.HasValue)
			{
				if (attribute)
				{
					writer.WriteAttributeString(name, value.Value.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture));
				}
				else
				{
					writer.WriteElementString(name, value.Value.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture));
				}
			}
		}

		/// <summary>
		/// Writes an attribute or an element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="attribute">A flag indicating if an Xml Element or an Xml Attribute will be written.</param>
		/// <param name="writer">The XmlWriter to which the Element or Attribute will be written.</param>
		/// <param name="name">The name of the Element or Attribute</param>
		/// <param name="value">The value of the Element or Attribute</param>
		private static void WriteIfNotNull(XmlWriter writer, bool attribute, string name, TimeSpan? value)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			if (value.HasValue)
			{
				if (attribute)
				{
					writer.WriteAttributeString(name, value.Value.ToString());
				}
				else
				{
					writer.WriteElementString(name, value.Value.ToString());
				}
			}
		}

		/// <summary>
		/// Writes an attribute or an element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="attribute">A flag indicating if an Xml Element or an Xml Attribute will be written.</param>
		/// <param name="writer">The XmlWriter to which the Element or Attribute will be written.</param>
		/// <param name="name">The name of the Element or Attribute</param>
		/// <param name="value">The value of the Element or Attribute</param>
		private static void WriteIfNotNull(XmlWriter writer, bool attribute, string name, double? value)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			if (value.HasValue)
			{
				if (attribute)
				{
					writer.WriteAttributeString(name, value.Value.ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					writer.WriteElementString(name, value.Value.ToString(CultureInfo.InvariantCulture));
				}
			}
		}

		/// <summary>
		/// Writes an Attribute to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Attribute will be written.</param>
		/// <param name="name">The name of the Attribute</param>
		/// <param name="value">The value of the Attribute</param>
		/// <param name="delimiter">The delimiter to be used to join the string array.</param>
		public static void WriteAttribute(this XmlWriter writer, string name, string[] value, char delimiter)
		{
			WriteIfNotNull(writer, true, name, value, delimiter);
		}

		/// <summary>
		/// Writes an Attribute to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Attribute will be written.</param>
		/// <param name="name">The name of the Attribute</param>
		/// <param name="value">The value of the Attribute</param>
		public static void WriteAttribute(this XmlWriter writer, string name, string value)
		{
			WriteIfNotNull(writer, true, name, value);
		}

		/// <summary>
		/// Writes an Attribute to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Attribute will be written.</param>
		/// <param name="name">The name of the Attribute</param>
		/// <param name="value">The value of the Attribute</param>
		public static void WriteAttribute(this XmlWriter writer, string name, bool? value)
		{
			WriteIfNotNull(writer, true, name, value);
		}

		/// <summary>
		/// Writes an Attribute to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Attribute will be written.</param>
		/// <param name="name">The name of the Attribute</param>
		/// <param name="value">The value of the Attribute</param>
		public static void WriteAttribute(this XmlWriter writer, string name, int? value)
		{
			WriteIfNotNull(writer, true, name, value);
		}

		/// <summary>
		/// Writes an Attribute to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Attribute will be written.</param>
		/// <param name="name">The name of the Attribute</param>
		/// <param name="value">The value of the Attribute</param>
		public static void WriteAttribute(this XmlWriter writer, string name, DateTime? value)
		{
			WriteIfNotNull(writer, true, name, value);
		}

		/// <summary>
		/// Writes an Attribute to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Attribute will be written.</param>
		/// <param name="name">The name of the Attribute</param>
		/// <param name="value">The value of the Attribute</param>
		public static void WriteAttribute(this XmlWriter writer, string name, TimeSpan? value)
		{
			WriteIfNotNull(writer, true, name, value);
		}

		/// <summary>
		/// Writes an Attribute to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Attribute will be written.</param>
		/// <param name="name">The name of the Attribute</param>
		/// <param name="value">The value of the Attribute</param>
		public static void WriteAttribute(this XmlWriter writer, string name, double? value)
		{
			WriteIfNotNull(writer, true, name, value);
		}

		/// <summary>
		/// Writes an Element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Element will be written.</param>
		/// <param name="name">The name of the Element</param>
		/// <param name="value">The value of the Element</param>
		/// <param name="delimiter">The delimiter to be used to join the string array.</param>
		public static void WriteElement(this XmlWriter writer, string name, string[] value, char delimiter)
		{
			WriteIfNotNull(writer, false, name, value, delimiter);
		}

		/// <summary>
		/// Writes an Element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Element will be written.</param>
		/// <param name="name">The name of the Element</param>
		/// <param name="value">The value of the Element</param>
		public static void WriteElement(this XmlWriter writer, string name, string value)
		{
			WriteIfNotNull(writer, false, name, value);
		}

		/// <summary>
		/// Writes an Element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Element will be written.</param>
		/// <param name="name">The name of the Element</param>
		/// <param name="value">The value of the Element</param>
		public static void WriteElement(this XmlWriter writer, string name, bool? value)
		{
			WriteIfNotNull(writer, false, name, value);
		}

		/// <summary>
		/// Writes an Element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Element will be written.</param>
		/// <param name="name">The name of the Element</param>
		/// <param name="value">The value of the Element</param>
		public static void WriteElement(this XmlWriter writer, string name, int? value)
		{
			WriteIfNotNull(writer, false, name, value);
		}

		/// <summary>
		/// Writes an Element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Element will be written.</param>
		/// <param name="name">The name of the Element</param>
		/// <param name="value">The value of the Element</param>
		public static void WriteElement(this XmlWriter writer, string name, DateTime? value)
		{
			WriteIfNotNull(writer, false, name, value);
		}

		/// <summary>
		/// Writes an Element to an XmlWriter, if the value is not null.
		/// </summary>
		/// <param name="writer">The XmlWriter to which the Element will be written.</param>
		/// <param name="name">The name of the Element</param>
		/// <param name="value">The value of the Element</param>
		public static void WriteElement(this XmlWriter writer, string name, double? value)
		{
			WriteIfNotNull(writer, false, name, value);
		}
		#endregion

		#region XmlReader Utility Methods -- Nullable Types

		#region GetStringArray
		/// <summary>
		/// Retrieves a string array from an Xml Reader from the specified attribute.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="delimiter">The delimiting character of the array.</param>
		/// <returns>The string array of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static string[] GetStringArrayAttribute(this XmlReader reader, string attributeName, char delimiter)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			string[] retValue = null;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				retValue = attributeValue.Split(delimiter);
			}

			return retValue;
		}
		#endregion

		#region GetBoolean

		/// <summary>
		/// Retrieves a boolean from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The boolean value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static bool? GetNullableBooleanAttribute(this XmlReader reader, string attributeName)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			bool? retValue = null;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				bool value;
				if (bool.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}
			return retValue;
		}
		#endregion

		#region GetInt
		/// <summary>
		/// Retrieves an integer from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The integer value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static int? GetNullableIntAttribute(this XmlReader reader, string attributeName)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			int? retValue = null;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				int value;
				if (int.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}
		#endregion

		#region GetDate
		/// <summary>
		/// Retrieves a Date/Time from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The DateTime value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static DateTime? GetNullableDateAttribute(this XmlReader reader, string attributeName)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			DateTime? retValue = null;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				DateTime value;
				if (DateTime.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}
		#endregion

		#region GetDouble
		/// <summary>
		/// Retrieves a double from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static double? GetNullableDoubleAttribute(this XmlReader reader, string attributeName)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			double? retValue = null;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				double value;
				if (double.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}
		#endregion

		#region GetByte
		/// <summary>
		/// Retrieves a boolean from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The byte value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static byte? GetNullableByteAttribute(this XmlReader reader, string attributeName)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			byte? retValue = null;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				byte value;
				if (byte.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;

		}
		#endregion
		#endregion

		#region XmlReader Utility Methods -- Non-Nullable Types

		#region GetBoolean
		/// <summary>
		/// Retrieves a boolean from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The boolean value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static bool GetBooleanAttribute(this XmlReader reader, string attributeName)
		{
			return GetBooleanAttribute(reader, attributeName, false);
		}
		/// <summary>
		/// Retrieves a boolean from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="defaultValue">The default return value, if the attribute is missing or invalid.</param>
		/// <returns>The boolean value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static bool GetBooleanAttribute(this XmlReader reader, string attributeName, bool defaultValue)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			bool retValue = defaultValue;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				bool value;
				if (bool.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}

		/// <summary>
		/// Retrieves an enumerated value from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <typeparam name="T">The type of enumeration.</typeparam>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="defaultValue">The default return value, if the attribute is missing or invalid.</param>
		/// <returns>The enumeration value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static T GetEnumAttribute<T>(this XmlReader reader, string attributeName, T defaultValue)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			T retVal = defaultValue;

			string value = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					retVal = (T)Enum.Parse(typeof(T), value);
				}
				catch (ArgumentException) { }
			}

			return retVal;
		}
		#endregion

		#region GetInt
		/// <summary>
		/// Retrieves an integer from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The integer value of the data contained within the Xml Reader at the specified Attribute.</returns>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "Int is a necessary part of this method name.")]
		public static int GetIntAttribute(this XmlReader reader, string attributeName)
		{
			return GetIntAttribute(reader, attributeName, 0);
		}
		/// <summary>
		/// Retrieves an integer from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="defaultValue">The default return value, if the attribute is missing or invalid.</param>
		/// <returns>The integer value of the data contained within the Xml Reader at the specified Attribute.</returns>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "Int is a necessary part of this method name.")]
		public static int GetIntAttribute(this XmlReader reader, string attributeName, int defaultValue)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			int retValue = defaultValue;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				int value;
				if (int.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}
		#endregion

		#region GetDate
		/// <summary>
		/// Retrieves a Date/Time from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The DateTime value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static DateTime GetDateAttribute(this XmlReader reader, string attributeName)
		{
			return GetDateAttribute(reader, attributeName, new DateTime());
		}
		/// <summary>
		/// Retrieves a Date/Time from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="defaultValue">The default return value, if the attribute is missing or invalid.</param>
		/// <returns>The DateTime value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static DateTime GetDateAttribute(this XmlReader reader, string attributeName, DateTime defaultValue)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			DateTime retValue = defaultValue;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				DateTime value;
				if (DateTime.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}
		#endregion

		#region TimeSpan
		/// <summary>
		/// Retrieves a TimeSpan from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The TimeSpan value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static TimeSpan GetTimeSpanAttribute(this XmlReader reader, string attributeName)
		{
			return GetTimeSpanAttribute(reader, attributeName, new TimeSpan());
		}
		/// <summary>
		/// Retrieves a TimeSpan from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="defaultValue">The default return value, if the attribute is missing or invalid.</param>
		/// <returns>The TimeSpan value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static TimeSpan GetTimeSpanAttribute(this XmlReader reader, string attributeName, TimeSpan defaultValue)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			TimeSpan retValue = defaultValue;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				TimeSpan value;
				if (TimeSpan.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}
		#endregion

		#region GetDouble
		/// <summary>
		/// Retrieves a double from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static double GetDoubleAttribute(this XmlReader reader, string attributeName)
		{
			return GetDoubleAttribute(reader, attributeName, 0);
		}
		/// <summary>
		/// Retrieves a double from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="defaultValue">The default return value, if the attribute is missing or invalid.</param>
		/// <returns>The value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static double GetDoubleAttribute(this XmlReader reader, string attributeName, double defaultValue)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			double retValue = defaultValue;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				double value;
				if (double.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;
		}
		#endregion

		#region GetByte
		/// <summary>
		/// Retrieves a boolean from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <returns>The byte value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static byte GetByteAttribute(this XmlReader reader, string attributeName)
		{
			return GetByteAttribute(reader, attributeName, 0);
		}
		/// <summary>
		/// Retrieves a boolean from an Xml Reader from the specified Attribute, by Attribute name.
		/// </summary>
		/// <param name="reader">The Xml Reader containing the data to retrieve.</param>
		/// <param name="attributeName">The name of the specified Attribute.</param>
		/// <param name="defaultValue">The default return value, if the attribute is missing or invalid.</param>
		/// <returns>The byte value of the data contained within the Xml Reader at the specified Attribute.</returns>
		public static byte GetByteAttribute(this XmlReader reader, string attributeName, byte defaultValue)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			byte retValue = defaultValue;
			string attributeValue = reader.GetAttribute(attributeName);

			if (!string.IsNullOrEmpty(attributeValue))
			{
				byte value;
				if (byte.TryParse(attributeValue, out value))
				{
					retValue = value;
				}
			}

			return retValue;

		}
		#endregion

		#endregion

	}
}