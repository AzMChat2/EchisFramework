using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace System.Data
{
	/// <summary>
	/// Loads a list of the specified type from the first column of a query result.
	/// </summary>
	/// <typeparam name="TValue">The type of list to load.</typeparam>
	public sealed class ListLoader<TValue> : IDataLoader
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
		/// Gets the loaded list.
		/// </summary>
		public List<TValue> Data { get; private set; }

		/// <summary>
		/// Called by the DataAccess object after execution with an open IDataReader object.
		/// </summary>
		/// <param name="reader">The IDataReader object containing the requested data.</param>
		public void ReadData(IDataReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			Data = new List<TValue>();

			while (reader.Read())
			{
				if (!_columnIndex.HasValue) _columnIndex = reader.GetOrdinal(ColumnName);

				if (!reader.IsDBNull(_columnIndex.Value))
				{
					Data.Add((TValue)Convert.ChangeType(reader.GetValue(_columnIndex.Value), typeof(TValue), CultureInfo.InvariantCulture));
				}
			}
		}
	}

	/// <summary>
	/// Represents a command from which a list will be generated.
	/// </summary>
	/// <typeparam name="TValue">The type of list to be generated.</typeparam>
	public sealed class ListLoaderCommand<TValue> : DataLoaderCommand<ListLoader<TValue>>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public ListLoaderCommand() : base(new ListLoader<TValue>()) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="commandText">The command text to be executed.</param>
		/// <param name="commandType">The type of command to be executed.</param>
		/// <param name="queryParams">Parameters (if any) of the command to be executed.</param>
		public ListLoaderCommand(string dataAccessName, string commandText, CommandType commandType, params IQueryParameter[] queryParams)
			: base(new ListLoader<TValue>(), dataAccessName, commandText, commandType, queryParams) { }
	}
}
