using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace System.Data
{
	/// <summary>
	/// Provides interface for executing a DB Generic command.
	/// </summary>
	public interface IDataCommand : IDataEvents
	{
		/// <summary>
		/// Gets the name of the DataAccess object which will be used to execute the command. (Optional)
		/// </summary>
		string DataAccessName { get; set; }
		/// <summary>
		/// Gets the name of the Database which will be used to execute the command. (Optional)
		/// </summary>
		string DatabaseName { get; set; }
		/// <summary>
		/// Gets the SQL command text to be executed.
		/// </summary>
		string CommandText { get; set; }
		/// <summary>
		/// Gets the type of command to be executed.
		/// </summary>
		CommandType CommandType { get; set; }
		/// <summary>
		/// Gets the paramters to be used during execution of command.
		/// </summary>
		IQueryParameterCollection Parameters { get; }
		/// <summary>
		/// Gets the transaction object to be used during execution of command.
		/// </summary>
		IDbTransaction Transaction { get; set; }
		/// <summary>
		/// Gets or sets the maximum amount of time allowed in which the command should complete execution.
		/// </summary>
		int CommandTimeout { get; set; }
	}

	/// <summary>
	/// Provides an interface for executing a DB Generic command for a DataLoader.
	/// </summary>
	public interface IDataLoaderCommand : IDataCommand 
	{
		/// <summary>
		/// The DataLoader object to be called with the resulting DataReader.
		/// </summary>
		IDataLoader DataLoader { get; set; }
	}

	/// <summary>
	/// Provides an interface for executing a DB Generic command for an XmlLoader.
	/// </summary>
	public interface IXmlLoaderCommand : IDataCommand 
	{
		/// <summary>
		/// The XmlLoader object to be called with the resulting XmlReader.
		/// </summary>
		IXmlLoader XmlLoader { get; set; }
	}

	/// <summary>
	/// Provides an interface for executing a DB Generic command for a DataSet.
	/// </summary>
	public interface IDataSetCommand : IDataCommand
	{
		/// <summary>
		/// The Dataset into which the resulting tables will be placed.  If null, a new DataSet will be created.
		/// </summary>
		DataSet DataSet { get; set; }

		/// <summary>
		/// The CultureInfo which will be used if a new DataSet is created.
		/// </summary>
		CultureInfo Locale { get; set; }

		/// <summary>
		/// The DB Generic command used to Insert records.
		/// </summary>
		IDataCommand InsertCommand { get; set; }
	
		/// <summary>
		/// The DB Generic command used to Update records.
		/// </summary>
		IDataCommand UpdateCommand { get; set; }

		/// <summary>
		/// The DB Generic command used to Delete records.
		/// </summary>
		IDataCommand DeleteCommand { get; set; }
	}

	/// <summary>
	/// Represents a collection of IDataCommand objects and provides methods for rolling back or commiting transactions.
	/// </summary>
	public interface IDataCommandCollection : IList<IDataCommand> { }

}
