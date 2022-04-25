using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Data
{
	/// <summary>
	/// The DataHelper class provides Data Extension methods.
	/// </summary>
	public static class DataUtilities
	{
		/// <summary>
		/// Action to be executed while reader has rows.
		/// </summary>
		/// <param name="reader">The data reader containing rows to be read.</param>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "Delegate is only used by DataUtilities")]
		public delegate void ReadAction(IDataReader reader);
		/// <summary>
		/// Peforms an action on a data reader as long as the data reader has rows.
		/// </summary>
		/// <param name="reader">The data reader containing rows to be read.</param>
		/// <param name="action">The action to be executed while reader has rows.</param>
		public static void WhileRead(this IDataReader reader, ReadAction action)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (action == null) throw new ArgumentNullException("action");

			while (reader.Read())
			{
				action(reader);
			}
		}
	
		#region DataReader Utility Methods -- Nullable Types

		#region GetString
		/// <summary>
		/// Retrieves a string from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The string value of the data contained within the Data Reader at the specified column.</returns>
		public static string GetString(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetString(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves a string from a Data Reader from the specified column, by column index.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The string value of the data contained within the Data Reader at the specified column.</returns>
		public static string GetString(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			string retValue = null;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetString(index).Trim();
			}

			return retValue;
		}
		#endregion

		#region GetStringArray
		/// <summary>
		/// Retrieves a string array from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <param name="delimiter">The delimiting character of the array.</param>
		/// <returns>The string array of the data contained within the Data Reader at the specified column.</returns>
		public static string[] GetStringArray(this IDataRecord reader, string columnName, char delimiter)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetStringArray(reader, reader.GetOrdinal(columnName), delimiter);
		}
		/// <summary>
		/// Retrieves a string array from a Data Reader from the specified column, by column index.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <param name="delimiter">The delimiting character of the array.</param>
		/// <returns>The string array of the data contained within the Data Reader at the specified column.</returns>
		public static string[] GetStringArray(IDataRecord reader, int index, char delimiter)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			string[] retValue = null;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetString(index).Split(delimiter);
			}

			return retValue;
		}
		#endregion

		#region GetBoolean
		/// <summary>
		/// Retrieves a boolean from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The boolean value of the data contained within the Data Reader at the specified column.</returns>
		public static bool? GetNullableBoolean(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetNullableBoolean(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves a boolean from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The boolean value of the data contained within the Data Reader at the specified column.</returns>
		public static bool? GetNullableBoolean(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			bool? retValue = null;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetBoolean(index);
			}

			return retValue;

		}
		#endregion

		#region GetInt
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		public static int? GetNullableInt(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetNullableInt(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		public static int? GetNullableInt(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			int? retValue = null;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetInt32(index);
			}

			return retValue;
		}
		#endregion

		#region GetLong
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		public static long? GetNullableLong(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetNullableLong(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		public static long? GetNullableLong(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			long? retValue = null;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetInt64(index);
			}

			return retValue;
		}
		#endregion

		#region GetDate
				/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime? GetNullableDate(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetNullableDate(reader, reader.GetOrdinal(columnName), CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime? GetNullableDate(IDataRecord reader, int index)
		{
			return GetNullableDate(reader, index, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <param name="provider">An object that supplies culture-specific format information.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime? GetNullableDate(this IDataRecord reader, string columnName, IFormatProvider provider)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetNullableDate(reader, reader.GetOrdinal(columnName), provider);
		}
		/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <param name="provider">An object that supplies culture-specific format information.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime? GetNullableDate(IDataRecord reader, int index, IFormatProvider provider)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (provider == null) throw new ArgumentNullException("provider");

			DateTime? retValue = null;

			if (!reader.IsDBNull(index))
			{
				try
				{
					retValue = reader.GetDateTime(index);
				}
				catch (InvalidCastException)
				{
					string dateValue = reader.GetString(index);
					try
					{
						retValue = DateTime.Parse(dateValue, provider);
					}
					catch (FormatException)
					{
						int year = int.Parse(dateValue.Substring(0, 4), provider);
						int month = int.Parse(dateValue.Substring(4, 2), provider);
						int day = int.Parse(dateValue.Substring(6, 2), provider);
						retValue = new DateTime(year, month, day);
					}
				}
			}

			return retValue;
		}
		#endregion

		#region GetDouble
		/// <summary>
		/// Retrieves a double from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The value of the data contained within the Data Reader at the specified column.</returns>
		public static double? GetNullableDouble(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetNullableDouble(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves a double from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The value of the data contained within the Data Reader at the specified column.</returns>
		public static double? GetNullableDouble(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			double? retValue = null;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetDouble(index);
			}

			return retValue;
		}
		#endregion

		#endregion

		#region DataReader Utility Methods -- Non-Nullable Types

		#region GetBoolean
		/// <summary>
		/// Retrieves a boolean from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The boolean value of the data contained within the Data Reader at the specified column.</returns>
		public static bool GetBoolean(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetBoolean(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves a boolean from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The boolean value of the data contained within the Data Reader at the specified column.</returns>
		public static bool GetBoolean(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			bool retValue = false;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetBoolean(index);
			}

			return retValue;

		}
		#endregion

		#region GetInt
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "Int is a necessary part of this method name.")]
		public static int GetInt(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetInt(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "Int is a necessary part of this method name.")]
		public static int GetInt(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			int retValue = 0;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetInt32(index);
			}

			return retValue;
		}
		#endregion

		#region GetLong
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "Long is a necessary part of this method name.")]
		public static long GetLong(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetLong(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves an integer from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The integer value of the data contained within the Data Reader at the specified column.</returns>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "Long is a necessary part of this method name.")]
		public static long GetLong(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			long retValue = 0;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetInt64(index);
			}

			return retValue;
		}
		#endregion

		#region GetDate
		/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime GetDate(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetDate(reader, reader.GetOrdinal(columnName), CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime GetDate(IDataRecord reader, int index)
		{
			return GetDate(reader, index, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <param name="provider">An object that supplies culture-specific format information.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime GetDate(this IDataRecord reader, string columnName, IFormatProvider provider)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetDate(reader, reader.GetOrdinal(columnName), provider);
		}
		/// <summary>
		/// Retrieves a Date/Time from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <param name="provider">An object that supplies culture-specific format information.</param>
		/// <returns>The DateTime value of the data contained within the Data Reader at the specified column.</returns>
		public static DateTime GetDate(IDataRecord reader, int index, IFormatProvider provider)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (provider == null) throw new ArgumentNullException("provider");

			DateTime retValue = DateTime.MinValue;

			if (!reader.IsDBNull(index))
			{
				try
				{
					retValue = reader.GetDateTime(index);
				}
				catch (InvalidCastException)
				{
					string dateValue = reader.GetString(index);
					try
					{
						retValue = DateTime.Parse(dateValue, provider);
					}
					catch (FormatException)
					{
						int year = int.Parse(dateValue.Substring(0, 4), provider);
						int month = int.Parse(dateValue.Substring(4, 2), provider);
						int day = int.Parse(dateValue.Substring(6, 2), provider);
						retValue = new DateTime(year, month, day);
					}
				}
			}

			return retValue;
		}
		#endregion

		#region GetByte
		/// <summary>
		/// Retrieves a boolean from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="columnName">The name of the specified column.</param>
		/// <returns>The byte value of the data contained within the Data Reader at the specified column.</returns>
		public static byte GetByte(this IDataRecord reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			return GetByte(reader, reader.GetOrdinal(columnName));
		}
		/// <summary>
		/// Retrieves a boolean from a Data Reader from the specified column, by column name.
		/// </summary>
		/// <param name="reader">The Data Reader containing the data to retrieve.</param>
		/// <param name="index">The index of the specified column.</param>
		/// <returns>The byte value of the data contained within the Data Reader at the specified column.</returns>
		public static byte GetByte(IDataRecord reader, int index)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			byte retValue = 0;

			if (!reader.IsDBNull(index))
			{
				retValue = reader.GetByte(index);
			}

			return retValue;

		}
		#endregion

		#endregion
	}
}
