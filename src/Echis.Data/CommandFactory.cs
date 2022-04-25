using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/// <summary>
	/// Contains static methods for creating Command objects.
	/// </summary>
	public static class CommandFactory
	{

		#region DataCommand

		/// <summary>
		/// Creates a DataCommand object configured to execute a stored proc.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="storedProcedureName">The name of the stored proc to execute.</param>
		/// <param name="storedProcedureParams">The stored proc parameters, if any.</param>
		/// <returns>A DataCommand object configured to execute a stored proc.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or storedProcName is null or empty.</exception>
		public static IDataCommand CreateStoredProcedureCommand(string dataAccessName, string storedProcedureName, params IQueryParameter[] storedProcedureParams)
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (storedProcedureName == null) throw new ArgumentNullException("storedProcedureName");

			return new DataCommand(dataAccessName, storedProcedureName, CommandType.StoredProcedure, storedProcedureParams);
		}

		/// <summary>
		/// Creates a DataCommand object configured to execute a Sql Statement.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="sqlText">The Sql Statement to be executed.</param>
		/// <param name="sqlParams">The Sql paramters, if any.</param>
		/// <returns>A DataCommand object configured to execute a Sql Statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or sqlText is null or empty.</exception>
		public static IDataCommand CreateSqlCommand(string dataAccessName, string sqlText, params IQueryParameter[] sqlParams)
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (sqlText == null) throw new ArgumentNullException("sqlText");

			return new DataCommand(dataAccessName, sqlText, CommandType.Text, sqlParams);
		}

		/// <summary>
		/// Creates a DataCommand object configured for Table Direct access.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="tableName">The name of the table to access.</param>
		/// <returns>A DataCommand object configured for Table Direct access.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or tableName is null or empty.</exception>
		public static IDataCommand CreateTableCommand(string dataAccessName, string tableName)
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (tableName == null) throw new ArgumentNullException("tableName");

			return new DataCommand(dataAccessName, tableName, CommandType.TableDirect);
		}

		#endregion

		#region DataLoaderCommand

		/// <summary>
		/// Creates a DataLoaderCommand object configured to execute a stored proc.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="storedProcedureName">The name of the stored proc to execute.</param>
		/// <param name="storedProcedureParams">The stored proc parameters, if any.</param>
		/// <param name="dataLoader">The DataLoader object which will be called with the DataReader resulting from the execution of this command.</param>
		/// <returns>A DataLoaderCommand object configured to execute a stored proc.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or storedProcName is null or empty.</exception>
		public static DataLoaderCommand<T> CreateStoredProcedureCommand<T>(T dataLoader, string dataAccessName, string storedProcedureName, params IQueryParameter[] storedProcedureParams) where T : IDataLoader
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (storedProcedureName == null) throw new ArgumentNullException("storedProcedureName");
			if (dataLoader == null) throw new ArgumentNullException("dataLoader");

			return new DataLoaderCommand<T>(dataLoader, dataAccessName, storedProcedureName, CommandType.StoredProcedure, storedProcedureParams);
		}

		/// <summary>
		/// Creates a DataLoaderCommand object configured to execute a stored proc.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="storedProcedureName">The name of the stored proc to execute.</param>
		/// <param name="storedProcedureParams">The stored proc parameters, if any.</param>
		/// <param name="loaderMethod">The DataLoaderHandler method which will be called with the DataReader resulting from the execution of this command.</param>
		/// <returns>A DataLoaderCommand object configured to execute a stored proc.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or storedProcName is null or empty.</exception>
		public static DataLoaderCommand<DelegateDataLoader> CreateStoredProcedureCommand(DataLoaderHandler loaderMethod, string dataAccessName, string storedProcedureName, params IQueryParameter[] storedProcedureParams) 
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (storedProcedureName == null) throw new ArgumentNullException("storedProcedureName");
			if (loaderMethod == null) throw new ArgumentNullException("loaderMethod");

			return new DataLoaderCommand<DelegateDataLoader>(new DelegateDataLoader(loaderMethod), dataAccessName, storedProcedureName, CommandType.StoredProcedure, storedProcedureParams);
		}

		/// <summary>
		/// Creates a DataLoaderCommand object configured to execute a Sql Statement.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="sqlText">The Sql Statement to be executed.</param>
		/// <param name="sqlParams">The Sql paramters, if any.</param>
		/// <param name="dataLoader">The DataLoader object which will be called with the DataReader resulting from the execution of this command.</param>
		/// <returns>A DataLoaderCommand object configured to execute a Sql Statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or sqlText is null or empty.</exception>
		public static DataLoaderCommand<T> CreateSqlCommand<T>(T dataLoader, string dataAccessName, string sqlText, params IQueryParameter[] sqlParams) where T : IDataLoader
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (sqlText == null) throw new ArgumentNullException("sqlText");
			if (dataLoader == null) throw new ArgumentNullException("dataLoader");

			return new DataLoaderCommand<T>(dataLoader, dataAccessName, sqlText, CommandType.Text, sqlParams);
		}
	
		/// <summary>
		/// Creates a DataLoaderCommand object configured to execute a Sql Statement.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="sqlText">The Sql Statement to be executed.</param>
		/// <param name="sqlParams">The Sql paramters, if any.</param>
		/// <param name="loaderMethod">The DataLoaderHandler method which will be called with the DataReader resulting from the execution of this command.</param>
		/// <returns>A DataLoaderCommand object configured to execute a Sql Statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or sqlText is null or empty.</exception>
		public static DataLoaderCommand<DelegateDataLoader> CreateSqlCommand(DataLoaderHandler loaderMethod, string dataAccessName, string sqlText, params IQueryParameter[] sqlParams) 
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (sqlText == null) throw new ArgumentNullException("sqlText");
			if (loaderMethod == null) throw new ArgumentNullException("loaderMethod");

			return new DataLoaderCommand<DelegateDataLoader>(new DelegateDataLoader(loaderMethod), dataAccessName, sqlText, CommandType.Text, sqlParams);
		}

		/// <summary>
		/// Creates a DataLoaderCommand object configured for Table Direct access.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="tableName">The name of the table to access.</param>
		/// <param name="dataLoader">The DataLoader object which will be called with the DataReader resulting from the execution of this command.</param>
		/// <returns>A DataLoaderCommand object configured for Table Direct access.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or tableName is null or empty.</exception>
		public static DataLoaderCommand<T> CreateTableCommand<T>(T dataLoader, string dataAccessName, string tableName) where T : IDataLoader
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (tableName == null) throw new ArgumentNullException("tableName");
			if (dataLoader == null) throw new ArgumentNullException("dataLoader");

			return new DataLoaderCommand<T>(dataLoader, dataAccessName, tableName, CommandType.TableDirect);
		}

		/// <summary>
		/// Creates a DataLoaderCommand object configured for Table Direct access.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="tableName">The name of the table to access.</param>
		/// <param name="loaderMethod">The DataLoaderHandler method which will be called with the DataReader resulting from the execution of this command.</param>
		/// <returns>A DataLoaderCommand object configured for Table Direct access.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or tableName is null or empty.</exception>
		public static DataLoaderCommand<DelegateDataLoader> CreateTableCommand(DataLoaderHandler loaderMethod, string dataAccessName, string tableName) 
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (tableName == null) throw new ArgumentNullException("tableName");
			if (loaderMethod == null) throw new ArgumentNullException("loaderMethod");

			return new DataLoaderCommand<DelegateDataLoader>(new DelegateDataLoader(loaderMethod), dataAccessName, tableName, CommandType.TableDirect);
		}

		#endregion

		#region XmlLoaderCommand

		/// <summary>
		/// Creates a XmlLoaderCommand object configured to execute a stored proc.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="storedProcedureName">The name of the stored proc to execute.</param>
		/// <param name="storedProcedureParams">The stored proc parameters, if any.</param>
		/// <param name="xmlLoader">The XmlLoader object which will be called with the XmlReader resulting from the execution of this command.</param>
		/// <returns>A XmlLoaderCommand object configured to execute a stored proc.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or storedProcName is null or empty.</exception>
		public static XmlLoaderCommand<T> CreateXmlLoaderStoredProcedureCommand<T>(T xmlLoader, string dataAccessName, string storedProcedureName, params IQueryParameter[] storedProcedureParams) where T : IXmlLoader
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (storedProcedureName == null) throw new ArgumentNullException("storedProcedureName");
			if (xmlLoader == null) throw new ArgumentNullException("xmlLoader");

			return new XmlLoaderCommand<T>(xmlLoader, dataAccessName, storedProcedureName, CommandType.StoredProcedure, storedProcedureParams);
		}

		/// <summary>
		/// Creates a XmlLoaderCommand object configured to execute a Sql Statement.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="sqlText">The Sql Statement to be executed.</param>
		/// <param name="sqlParams">The Sql paramters, if any.</param>
		/// <param name="xmlLoader">The XmlLoader object which will be called with the XmlReader resulting from the execution of this command.</param>
		/// <returns>A XmlLoaderCommand object configured to execute a Sql Statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or sqlText is null or empty.</exception>
		public static XmlLoaderCommand<T> CreateXmlLoaderCommand<T>(T xmlLoader, string dataAccessName, string sqlText, params IQueryParameter[] sqlParams) where T : IXmlLoader
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (sqlText == null) throw new ArgumentNullException("sqlText");
			if (xmlLoader == null) throw new ArgumentNullException("xmlLoader");

			return new XmlLoaderCommand<T>(xmlLoader, dataAccessName, sqlText, CommandType.Text, sqlParams);
		}

		/// <summary>
		/// Creates a XmlLoaderCommand object configured for Table Direct access.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="tableName">The name of the table to access.</param>
		/// <param name="xmlLoader">The XmlLoader object which will be called with the XmlReader resulting from the execution of this command.</param>
		/// <returns>A XmlLoaderCommand object configured for Table Direct access.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or tableName is null or empty.</exception>
		public static XmlLoaderCommand<T> CreateXmlLoaderCommand<T>(T xmlLoader, string dataAccessName, string tableName) where T : IXmlLoader
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (tableName == null) throw new ArgumentNullException("tableName");
			if (xmlLoader == null) throw new ArgumentNullException("xmlLoader");

			return new XmlLoaderCommand<T>(xmlLoader, dataAccessName, tableName, CommandType.TableDirect);
		}

		#endregion

		#region ListLoaderCommand

		/// <summary>
		/// Creates a DataLoaderCommand object configured to execute a stored proc.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="storedProcedureName">The name of the stored proc to execute.</param>
		/// <param name="storedProcedureParams">The stored proc parameters, if any.</param>
		/// <returns>A DataLoaderCommand object configured to execute a stored proc.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or storedProcName is null or empty.</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static ListLoaderCommand<T> CreateListLoaderStoredProcedureCommand<T>(string dataAccessName, string storedProcedureName, params IQueryParameter[] storedProcedureParams)
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (storedProcedureName == null) throw new ArgumentNullException("storedProcedureName");

			return new ListLoaderCommand<T>(dataAccessName, storedProcedureName, CommandType.StoredProcedure, storedProcedureParams);
		}

		/// <summary>
		/// Creates a DataLoaderCommand object configured to execute a Sql Statement.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="sqlText">The Sql Statement to be executed.</param>
		/// <param name="sqlParams">The Sql paramters, if any.</param>
		/// <returns>A DataLoaderCommand object configured to execute a Sql Statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or sqlText is null or empty.</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static ListLoaderCommand<T> CreateListLoaderCommand<T>(string dataAccessName, string sqlText, params IQueryParameter[] sqlParams)
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (sqlText == null) throw new ArgumentNullException("sqlText");

			return new ListLoaderCommand<T>(dataAccessName, sqlText, CommandType.Text, sqlParams);
		}

		/// <summary>
		/// Creates a DataLoaderCommand object configured for Table Direct access.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="tableName">The name of the table to access.</param>
		/// <returns>A DataLoaderCommand object configured for Table Direct access.</returns>
		/// <exception cref="ArgumentNullException">Thrown if dataAccessName or tableName is null or empty.</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static ListLoaderCommand<T> CreateListLoaderCommand<T>(string dataAccessName, string tableName)
		{
			if (dataAccessName == null) throw new ArgumentNullException("dataAccessName");
			if (tableName == null) throw new ArgumentNullException("tableName");

			return new ListLoaderCommand<T>(dataAccessName, tableName, CommandType.TableDirect);
		}

		#endregion

	}
}
