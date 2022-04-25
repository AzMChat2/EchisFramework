using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Serialization;

namespace System.Collections
{
	/// <summary>
	/// Represents a generic Parameter.
	/// </summary>
	[Serializable]
	public sealed class Parameter
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Parameter() { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the Parameter</param>
		/// <param name="value">The value of the Parameter</param>
		public Parameter(string name, string value)
		{
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Gets or sets the name of the Parameter.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the value of the Parameter.
		/// </summary>
		[XmlAttribute]
		public string Value { get; set; }

		/// <summary>
		/// Gets the parameter Value as a string.
		/// </summary>
		public static explicit operator string(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.Value;
		}

		/// <summary>
		/// Gets the parameter Value as a boolean.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator bool(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToBoolean();
		}

		/// <summary>
		/// Gets the parameter Value as a byte.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator byte(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToByte();
		}

		/// <summary>
		/// Gets the parameter Value as an Int16.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator short(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToShort();
		}

		/// <summary>
		/// Gets the parameter Value as an Int32.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator int(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToInt();
		}

		/// <summary>
		/// Gets the parameter Value as an Int64.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator long(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToLong();
		}

		/// <summary>
		/// Gets the parameter Value as a decimal.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator decimal(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToDecimal();
		}

		/// <summary>
		/// Gets the parameter Value as a single.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator float(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToFloat();
		}

		/// <summary>
		/// Gets the parameter Value as a double.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator double(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToDouble();
		}

		/// <summary>
		/// Gets the parameter Value as a TimeSpan.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator TimeSpan(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToTimeSpan();
		}

		/// <summary>
		/// Gets the parameter Value as a DateTime.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		public static explicit operator DateTime(Parameter value)
		{
			if (value == null) throw new ArgumentNullException("value");
			return value.ToDateTime();
		}


		/// <summary>
		/// Gets the parameter Value as a boolean.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private bool ToBoolean()
		{
			try
			{
				return bool.Parse(Value);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; valid values are 'true' and 'false'.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; valid values are 'true' and 'false'.", Name), aex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as a byte.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private byte ToByte()
		{
			try
			{
				return byte.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a byte.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a byte.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a byte.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as an Int16.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private short ToShort()
		{
			try
			{
				return short.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a 16 bit integer.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a 16 bit integer.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a 16 bit integer.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as an Int32.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private int ToInt()
		{
			try
			{
				return int.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a 32 bit integer.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a 32 bit integer.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a 32 bit integer.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as an Int64.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private long ToLong()
		{
			try
			{
				return long.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a 64 bit integer.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a 64 bit integer.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a 64 bit integer.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as a decimal.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private decimal ToDecimal()
		{
			try
			{
				return decimal.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a decimal number.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a decimal number.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a decimal number.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as a single.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private float ToFloat()
		{
			try
			{
				return float.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a single floating point number.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a single floating point number.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a single floating point number.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as a double.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private double ToDouble()
		{
			try
			{
				return double.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a double floating point number.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a double floating point number.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a double floating point number.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as a TimeSpan.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private TimeSpan ToTimeSpan()
		{
			try
			{
				return TimeSpan.Parse(Value, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a valid time.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a valid time.", Name), aex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a valid time.", Name), oex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as a DateTime.
		/// </summary>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		private DateTime ToDateTime()
		{
			try
			{
				return DateTime.Parse(Value, CultureInfo.CurrentCulture);
			}
			catch (FormatException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; value must be a valid Date-Time.", Name), ex);
			}
			catch (ArgumentNullException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'; value must be a valid Date-Time.", Name), aex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as an Enumeration.
		/// </summary>
		/// <typeparam name="T">The type of the parameter.</typeparam>
		/// <returns>Returns the enumeration value from the string value of the parameter.</returns>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public T ToEnumeration<T>()
		{
			try
			{
				return (T)Enum.Parse(typeof(T), Value, true);
			}
			catch (ArgumentNullException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'.", Name), ex);
			}
			catch (ArgumentException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; {1}", Name, aex.Message), aex);
			}
		}

		/// <summary>
		/// Gets the parameter Value as the type specified..
		/// </summary>
		/// <typeparam name="T">The type of the parameter.</typeparam>
		/// <returns>Returns the parameter value as the type provided.</returns>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">ConfigurationErrorsException</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public T To<T>()
		{
			try
			{
				return (T)Convert.ChangeType(Value, typeof(T), CultureInfo.InvariantCulture);
			}
			catch (ArgumentNullException ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Parameter value not specified for '{0}'.", Name), ex);
			}
			catch (FormatException aex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; {1}", Name, aex.Message), aex);
			}
			catch (InvalidCastException iex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'; {1}", Name, iex.Message), iex);
			}
			catch (OverflowException oex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Invalid parameter value specified for '{0}'.", Name), oex);
			}
		}
	}

	/// <summary>
	/// Represents a collection of generic parameters.
	/// </summary>
	[Serializable]
	public class ParameterCollection : List<Parameter>
	{
		/// <summary>
		/// Creates and adds a new Parameter to the collection
		/// </summary>
		/// <param name="name">The name of the Parameter</param>
		/// <param name="value">The value of the Parameter</param>
		public void Add(string name, string value)
		{
			Add(new Parameter(name, value));
		}

		/// <summary>
		/// Gets the Paramter for the given name.
		/// </summary>
		/// <param name="name">The name of the parameter to retrieve.</param>
		/// <returns>Returns the paramter for the given name or null if no paramter exists with the given name.</returns>
		public Parameter this[string name]
		{
			get { return Find(item => item.Name == name); }
		}

		/// <summary>
		/// Determines if a Parameter with a given name exists in the collection.
		/// </summary>
		/// <param name="name">The name of the parameter to find.</param>
		/// <returns>Returns true if a parameter is found with the given name, returns false otherwise.</returns>
		public bool Contains(string name)
		{
			return Exists(item => item.Name == name);
		}
	}
}
