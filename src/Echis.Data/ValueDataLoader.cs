using System;
using System.Data;
using System.Globalization;

namespace System.Data
{
	/// <summary>
	/// Reads a value from the specified column from a Data Reader
	/// </summary>
	/// <typeparam name="TValue">The value type of the column.</typeparam>
	public sealed class ValueLoader<TValue> : IDataLoader
	{
		/// <summary>
		/// Stores the name of the column containing the data to be read.
		/// </summary>
		private string _columnName;
		/// <summary>
		/// Gets or sets the name of the column containing the data to be read.
		/// </summary>
		public string ColumnName
		{
			get { return _columnName; }
			set
			{
				if (!string.IsNullOrEmpty(value)) _columnIndex = null;
				_columnName = value;
			}
		}

		/// <summary>
		/// Stores the Column index of the column containing the data to be read.
		/// </summary>
		private int? _columnIndex;
		/// <summary>
		/// Gets or sets the Column index of the column containing the data to be read.
		/// </summary>
		public int ColumnIndex
		{
			get { return _columnIndex.HasValue ? _columnIndex.Value : -1; }
			set
			{
				if (value < 0)
				{
					_columnIndex = null;
				}
				else
				{
					_columnIndex = value;
				}
			}
		}

		/// <summary>
		/// Gets the value from the specified column of the Data Reader.
		/// </summary>
		public TValue Value { get; private set; }

		/// <summary>
		/// Reads the Value from the specified Data Reader.
		/// </summary>
		/// <param name="reader">The Data Reader containing the value to be read.</param>
		public void ReadData(IDataReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			if (reader.Read())
			{
				if (!_columnIndex.HasValue) _columnIndex = reader.GetOrdinal(ColumnName);

				if (reader.IsDBNull(_columnIndex.Value))
				{
					Value = default(TValue);
				}
				else
				{
					Value = (TValue)Convert.ChangeType(reader.GetValue(_columnIndex.Value), typeof(TValue), CultureInfo.InvariantCulture);
				}
			}
		}
	}

	/// <summary>
	/// Represents a command from which a value will be read.
	/// </summary>
	/// <typeparam name="TValue">The type of value to be read.</typeparam>
	public sealed class ValueLoaderCommand<TValue> : DataLoaderCommand<ValueLoader<TValue>>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public ValueLoaderCommand() : base(new ValueLoader<TValue>()) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="commandText">The command text to be executed.</param>
		/// <param name="commandType">The type of command to be executed.</param>
		/// <param name="queryParams">Parameters (if any) of the command to be executed.</param>
		public ValueLoaderCommand(string dataAccessName, string commandText, CommandType commandType, params IQueryParameter[] queryParams)
			: base(new ValueLoader<TValue>(), dataAccessName, commandText, commandType, queryParams) { }
	}
}
