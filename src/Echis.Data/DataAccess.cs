using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/* This File contains static methods for DataAccess */

	/// <summary>
	/// Provides access to any configured Databases by name.
	/// </summary>
	public static class DataAccess
	{
		/// <summary>
		/// Gets or sets a flag which determines if Tracing messages are written to the Trace device.
		/// </summary>
		public static bool Tracing { get; set; }

		/// <summary>
		/// Stores the Data Access Objects used to communicate with databases.
		/// </summary>
		private static DataAccessDictionary Databases = new DataAccessDictionary();

		/// <summary>
		/// Gets the DataAccess instance by the specified name.
		/// </summary>
		/// <param name="dataAccessName">The name of the data access instance</param>
		/// <returns>Returns the named DataAccess instance.</returns>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="ArgumentNullException">Thrown if data access name is null.</exception>
		public static IDataAccess GetDataAccessInstance(string dataAccessName)
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");

			return Databases[dataAccessName];
		}

		/// <summary>
		/// Creates and adds a Data Access Client using the specified Data Access Information.
		/// </summary>
		/// <param name="dataAccessInfo">An object containing the Data Access Information used to create the Data Access Client.</param>
		/// <exception cref="ArgumentNullException">Thrown if data access information object is null.</exception>
		/// <exception cref="UnableToCreateDataAccessClientException">Thrown if the data access object's connection string information is not found.</exception>
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
			Justification = "Simulating an internal method call and subsequent ArgumentNullException")]
		public static void AddDataAccessClient(DataAccessObject dataAccessInfo)
		{
			if (dataAccessInfo == null) throw new ArgumentNullException("dataAccessInfo");

			if (ConnectionInfoDictionary.ConnectionStringExists(dataAccessInfo.Name))
			{
				Databases.Add(dataAccessInfo);
			}
			else
			{
				throw new UnableToCreateDataAccessClientException(dataAccessInfo.Name, new ArgumentNullException("ConnectionString"));
			}
		}
	
		/// <summary>
		/// Adds a Data Access Client object to the collection of Clients.
		/// </summary>
		/// <param name="dataAccess">The Data Access Client object to be added to the collection.</param>
		/// <exception cref="ArgumentNullException">Thrown if data access object is null.</exception>
		public static void AddDataAccessClient(IDataAccess dataAccess)
		{
			if (dataAccess == null) throw new ArgumentNullException("dataAccess");

			Databases.Add(dataAccess);
		}

		/// <summary>
		/// Starts a Transaction for each of the commands provided in the list.
		/// </summary>
		/// <param name="commands">The collection of IDataCommand objects for which transactions will be started.</param>
		/// <exception cref="ArgumentNullException">Thrown if the command collection is null.</exception>
		/// <exception cref="DatabaseTransactionException">Thrown if an exception is caught while attempting to create transactions on the specified databases.</exception>
		public static void BeginTransactions(IDataCommandCollection commands)
		{
			if (commands == null) throw new ArgumentNullException("commands");

			ExceptionDetail<IDataCommand>[] exs = commands.ForEachGuaranteed(BeginTransaction);

			if (exs.Length != 0)
				throw new DatabaseTransactionException(DatabaseTransactionException.TransactionActions.Begin, exs);
		}

		/// <summary>
		/// Starts a Transaction for the command provided.
		/// </summary>
		/// <param name="command">The IDataCommand object for which a transaction will be started.</param>
		/// <exception cref="ArgumentNullException">Thrown if the command is null.</exception>
		public static void BeginTransaction(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			command.Transaction = Databases[command.DataAccessName].GetTransaction();
		}

		/// <summary>
		/// Cancels the Transaction for each of the commands provided in the list.
		/// </summary>
		/// <param name="commands">The collection of IDataCommand objects for which transactions will be started.</param>
		/// <exception cref="ArgumentNullException">Thrown if the command collection is null.</exception>
		/// <exception cref="DatabaseTransactionException">Thrown if an exception is caught while attempting to rollback transactions on the specified databases.</exception>
		public static void RollbackTransactions(IDataCommandCollection commands)
		{
			if (commands == null) throw new ArgumentNullException("commands");

			ExceptionDetail<IDataCommand>[] exs = commands.ForEachGuaranteed(RollbackTransaction);

			if (exs.Length != 0)
				throw new DatabaseTransactionException(DatabaseTransactionException.TransactionActions.Rollback, exs);
		}

		/// <summary>
		/// Cancels the Transaction for the command provided.
		/// </summary>
		/// <param name="command">The IDataCommand object for which a transaction will be started.</param>
		/// <exception cref="ArgumentNullException">Thrown if the command is null.</exception>
		public static void RollbackTransaction(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			command.Transaction.Rollback();
			CleanupTransaction(command);
		}

		/// <summary>
		/// Completes the Transaction for each of the commands provided in the list.
		/// </summary>
		/// <param name="commands">The collection of IDataCommand objects for which transactions will be started.</param>
		/// <exception cref="ArgumentNullException">Thrown if the command collection is null.</exception>
		/// <exception cref="DatabaseTransactionException">Thrown if an exception is caught while attempting to commit transactions on the specified databases.</exception>
		public static void CommitTransactions(IDataCommandCollection commands)
		{
			if (commands == null) throw new ArgumentNullException("commands");

			ExceptionDetail<IDataCommand>[] exs = commands.ForEachGuaranteed(CommitTransaction);

			if (exs.Length != 0)
				throw new DatabaseTransactionException(DatabaseTransactionException.TransactionActions.Commit, exs);
		}

		/// <summary>
		/// Completes the Transaction for the command provided.
		/// </summary>
		/// <param name="command">The IDataCommand object for which a transaction will be started.</param>
		/// <exception cref="ArgumentNullException">Thrown if the command is null.</exception>
		public static void CommitTransaction(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			command.Transaction.Commit();
			CleanupTransaction(command);
		}

		/// <summary>
		/// Cleans up Transaction and Connection resources.
		/// </summary>
		/// <param name="command">The command whose resources need to be recoverred.</param>
		private static void CleanupTransaction(IDataCommand command)
		{
			if (command.Transaction.Connection != null)
			{
				command.Transaction.Connection.Close();
				command.Transaction.Connection.Dispose();
			}

			command.Transaction.Dispose();
			command.Transaction = null;
		}

		/// <summary>
		/// Executes an IDbCommand and fills a DataSet with data.
		/// </summary>
		/// <param name="command">An object containing the IDataSetCommand and Dataset.</param>
		/// <exception cref="ArgumentNullException">Thrown if command is null.</exception>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="SystemException">Thrown if an unhandled (via IDataEvents) exception is thrown while executing the command.</exception>
		public static void ExecuteDataSet(IDataSetCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			Databases[command.DataAccessName].ExecuteDataSet(command);
		}

		/// <summary>
		/// Updates a database using the contents of a Dataset
		/// </summary>
		/// <param name="command">An object containing the IDataSetCommand and Dataset.</param>
		/// <exception cref="ArgumentNullException">Thrown if command is null.</exception>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="SystemException">Thrown if an unhandled (via IDataEvents) exception is thrown while executing the command.</exception>
		public static void UpdateDataSet(IDataSetCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			Databases[command.DataAccessName].UpdateDataSet(command);
		}

		/// <summary>
		/// Executes an IDbCommand and returns the number of rows effected.
		/// </summary>
		/// <param name="command">The IDataCommand to be executed.</param>
		/// <returns>The number of rows effected.</returns>
		/// <exception cref="ArgumentNullException">Thrown if command is null.</exception>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="SystemException">Thrown if an unhandled (via IDataEvents) exception is thrown while executing the command.</exception>
		public static int ExecuteNonQuery(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			return Databases[command.DataAccessName].ExecuteNonQuery(command);
		}

		/// <summary>
		/// Execute a Scalar Command and returns the result.
		/// </summary>
		/// <param name="command">The IDataCommand to execute.</param>
		/// <returns>The result of the Command.</returns>
		/// <exception cref="ArgumentNullException">Thrown if command is null.</exception>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="SystemException">Thrown if an unhandled (via IDataEvents) exception is thrown while executing the command.</exception>
		public static object ExecuteScalar(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			return Databases[command.DataAccessName].ExecuteScalar(command);
		}

		/// <summary>
		/// Execute a Scalar Command and returns the result.
		/// </summary>
		/// <param name="command">The IDataCommand to execute.</param>
		/// <returns>The result of the Command.</returns>
		/// <exception cref="ArgumentNullException">Thrown if command is null.</exception>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="SystemException">Thrown if an unhandled (via IDataEvents) exception is thrown while executing the command.</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T ExecuteScalar<T>(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			return (T)Databases[command.DataAccessName].ExecuteScalar(command);
		}

		/// <summary>
		/// Executes an IDbCommand and uses the resulting Data Reader to fill the object.
		/// </summary>
		/// <param name="command">The IDataLoaderCommand to execute.</param>
		/// <exception cref="ArgumentNullException">Thrown if command is null.</exception>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="SystemException">Thrown if an unhandled (via IDataEvents) exception is thrown while executing the command.</exception>
		public static void ExecuteDataLoader(IDataLoaderCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			Databases[command.DataAccessName].ExecuteDataLoader(command);
		}

		/// <summary>
		/// Executes a IDbCommand and uses the resulting Xml Reader to fill the object.
		/// </summary>
		/// <param name="command">The IXmlLoaderCommand to execute.</param>
		/// <exception cref="ArgumentNullException">Thrown if command is null.</exception>
		/// <exception cref="DatabaseNotConfiguredException">Thrown if the data access name provided is not a configured DataAccess object.</exception>
		/// <exception cref="SystemException">Thrown if an unhandled (via IDataEvents) exception is thrown while executing the command.</exception>
		public static void ExecuteDataXml(IXmlLoaderCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			Databases[command.DataAccessName].ExecuteDataXml(command);
		}
	}

	/// <summary>
	/// Stores all IDataAccess objects used by the DataAccess class
	/// </summary>
	internal class DataAccessDictionary
	{
		/// <summary>
		/// Stores the IDataAccess objects, indexed by DbName.
		/// </summary>
		private Dictionary<string, IDataAccess> databases = new Dictionary<string, IDataAccess>();

		/// <summary>
		/// Stores the Default Data Access Object
		/// </summary>
		private IDataAccess _defaultDAO;

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public DataAccessDictionary()
		{
			Settings.Values.DataAccessObjects.ForEach(item => Add(item));
		}

		/// <summary>
		/// Creates a Data Access Client from the information contained in the Data Access Object definition.
		/// </summary>
		/// <param name="dao">The Data Access Object definition.</param>
		/// <remarks>If the DataAccessObject is marked as Default and a Default DataAccessObject already exists,
		/// it will be replaced with the new DataAccessObject being added.</remarks>
		public void Add(DataAccessObject dao)
		{
			IDataAccess item = IOC.GetFrameworkObject<IDataAccess>(dao.DataAccessClient, null, false);

			if (item == null)
			{
				throw new UnableToCreateDataAccessClientException(dao.Name);
			}
			else
			{
				item.Name = dao.Name;
				databases.Add(item.Name, item);
				if (dao.Default)
				{
					_defaultDAO = item;
				}
			}
		}

		/// <summary>
		/// Adds a DataAccess object to the collection.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IDataAccess item)
		{
			databases.Add(item.Name, item);
		}

		/// <summary>
		/// Gets the database IDataAccess object for the specified database by name.
		/// </summary>
		/// <param name="dbName">The name of the database access object.</param>
		public IDataAccess this[string dbName]
		{
			get
			{
				if (string.IsNullOrEmpty(dbName))
				{
					if (_defaultDAO == null)
					{
						throw new DatabaseNotConfiguredException("[ DEFAULT ]");
					}
					else
					{
						return _defaultDAO;
					}
				}
				else if (databases.ContainsKey(dbName))
				{
					return (databases[dbName]);
				}
				else
				{
					throw new DatabaseNotConfiguredException(dbName);
				}
			}
		}
	}
}
