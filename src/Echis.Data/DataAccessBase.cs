using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Data
{
	/* This file contains base methods for DataAccess objects */
	/// <summary>
	/// Provides a base implementation of DataAccess methods.
	/// </summary>
	public abstract class DataAccessBase : IDataAccess
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		protected DataAccessBase() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the Data Client</param>
		/// <param name="connectionString">The connection string used to connect to the database.</param>
		protected DataAccessBase(string name, string connectionString)
		{
			Name = name;
			ConnectionInfoDictionary.SetConnectionString(name, connectionString);
		}

		#region Properties
		/// <summary>
		/// The name of this IDataAccess object.
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Abstract Methods

		/// <summary>
		/// Get an IDbConnection object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbConnection object for the specific Database which this client represents.</returns>
		public abstract IDbConnection CreateConnection();

		/// <summary>
		/// Get an IDbCommand object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Return an IDbCommand object for the specific Database which this client represents</returns>
		public abstract IDbCommand CreateCommand();

		/// <summary>
		/// Get an IDbDataParameter object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbDataParameter object for the specific Database which this client represents.</returns>
		public abstract IDataParameter CreateDataParameter();

		/// <summary>
		/// Get an IDbDataAdapter object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbDataAdapter object for the specific Database which this client represents.</returns>
		public abstract IDbDataAdapter CreateDataAdapter();

		/// <summary>
		/// Executes a IDbCommand and uses the resulting Xml Reader to fill the object.
		/// </summary>
		/// <param name="command">The DB Generic command object.</param>
		/// <param name="connection">The IDbConnection which the resulting IDbCommand will use.</param>
		protected abstract void ExecuteDataXml(IXmlLoaderCommand command, IDbConnection connection);

		#endregion

		#region Data Access Object factory methods
		/// <summary>
		/// Get an IDbTransaction object.
		/// </summary>
		/// <returns>Returns an IDbTransaction object.</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		public virtual IDbTransaction GetTransaction()
		{
			IDbConnection connection = DataConnection.OpenConnection(this);
			return connection.BeginTransaction();
		}

		/// <summary>
		/// Get an IDbCommand object.
		/// </summary>
		/// <param name="command">The DB Generic command object.</param>
		/// <param name="connection">The IDbConnection which the resulting IDbCommand will use.</param>
		/// <returns>Return an IDbCommand object.</returns>
		[SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
			Justification="This method is specifically intended for consumers to execute dynamic queries.")]
		protected virtual IDbCommand GetCommand(IDataCommand command, IDbConnection connection)
		{
			if (command == null) throw new ArgumentNullException("command");
			if (connection == null) throw new ArgumentNullException("connection");

			IDbCommand retVal = CreateCommand();

			retVal.Connection = connection;
			retVal.CommandText = command.CommandText;
			retVal.CommandType = command.CommandType;
			retVal.CommandTimeout = command.CommandTimeout;
			retVal.Transaction = command.Transaction;

			command.Parameters.ForEach(item => retVal.Parameters.Add(GetDataParameter(item)));

			return retVal;
		}

		/// <summary>
		/// Get an IDataParameter object.
		/// </summary>
		/// <param name="parameter">The DB Generic parameter object.</param>
		/// <returns>Returns an IDataParameter object.</returns>
		protected virtual IDataParameter GetDataParameter(IQueryParameter parameter)
		{
			if (parameter == null) throw new ArgumentNullException("parameter");

			parameter.Parameter = CreateDataParameter();

			parameter.Parameter.ParameterName = parameter.Name;
			parameter.Parameter.Value = ((parameter.Value == null) ? DBNull.Value : parameter.Value);
			parameter.Parameter.Direction = parameter.Direction;

			return parameter.Parameter;
		}

		/// <summary>
		/// Get an IDbDataAdapter object.
		/// </summary>
		/// <param name="command">The IDataCommand object from which the adapter will be made.</param>
		/// <param name="connection">The IDbConnection which the resulting DataAdapter will use.</param>
		/// <returns>Returns an IDbDataAdapter object.</returns>
		protected virtual IDbDataAdapter GetAdapter(IDataSetCommand command, IDbConnection connection)
		{
			if (command == null) throw new ArgumentNullException("command");
			if (connection == null) throw new ArgumentNullException("connection");

			IDbDataAdapter retVal = CreateDataAdapter();

			retVal.SelectCommand = GetCommand(command, connection);
			retVal.InsertCommand = GetCommand(command.InsertCommand, connection);
			retVal.UpdateCommand = GetCommand(command.UpdateCommand, connection);
			retVal.DeleteCommand = GetCommand(command.DeleteCommand, connection);

			return retVal;
		}
		#endregion

		#region ExecuteDataSet
		/// <summary>
		/// Executes a SqlCommand and fills a DataSet with data.
		/// </summary>
		/// <param name="command">An object containing the SqlCommand and Dataset</param>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		public void ExecuteDataSet(IDataSetCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			command.OnBeforeExecute();

			Stopwatch sw = Stopwatch.StartNew();
			TS.Logger.WriteMethodCallIf(DataAccess.Tracing && TS.EC.TraceVerbose);

			try
			{
				using (DataConnection connInfo = new DataConnection(command, this))
				{
					DataSet data = new DataSet();
					data.Locale = command.Locale;

					IDbDataAdapter adapter = GetAdapter(command, connInfo.Connection);

					adapter.Fill(data);
					command.Parameters.UpdateParameterValues();

					if (command.DataSet == null)
					{
						command.DataSet = data;
					}
					else
					{
						CopyTables(data, command.DataSet);
					}
				}
			}
			catch (SystemException ex)
			{
				TS.Logger.WriteExceptionIf(DataAccess.Tracing && TS.EC.TraceError, ex);
				if (!command.OnDataException(ex))
				{
					throw;
				}
			}
			finally
			{
				sw.Stop();
				TS.Logger.WritePerformanceIf(DataAccess.Tracing && TS.EC.TraceInfo, sw.Elapsed);

				command.OnAfterExecute();
			}
		}
		#endregion

		#region UpdateDataSet
		/// <summary>
		/// Updates a database using the contents of a Dataset
		/// </summary>
		/// <param name="command">An object containing the DataSet.</param>
		public void UpdateDataSet(IDataSetCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			command.OnBeforeExecute();

			Stopwatch sw = Stopwatch.StartNew();
			TS.Logger.WriteMethodCallIf(DataAccess.Tracing && TS.EC.TraceVerbose);

			try
			{
				using (DataConnection connInfo = new DataConnection(command, this))
				{
					IDbDataAdapter adapter = GetAdapter(command, connInfo.Connection);
					adapter.Update(command.DataSet);
					command.Parameters.UpdateParameterValues();
				}
			}
			catch (SystemException ex)
			{
				TS.Logger.WriteExceptionIf(DataAccess.Tracing && TS.EC.TraceError, ex);
				if (!command.OnDataException(ex))
				{
					throw;
				}
			}
			finally
			{
				sw.Stop();
				TS.Logger.WritePerformanceIf(DataAccess.Tracing && TS.EC.TraceInfo, sw.Elapsed);

				command.OnAfterExecute();
			}
		}
		#endregion

		#region ExecuteNonQuery
		/// <summary>
		/// Executes a SqlCommand and returns the number of rows effected.
		/// </summary>
		/// <param name="command">The SqlCommand to be executed.</param>
		/// <returns>The number of rows effected.</returns>
		public int ExecuteNonQuery(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			int retVal = -1;
			command.OnBeforeExecute();

			Stopwatch sw = Stopwatch.StartNew();
			TS.Logger.WriteMethodCallIf(DataAccess.Tracing && TS.EC.TraceVerbose);

			try
			{
				using (DataConnection connInfo = new DataConnection(command, this))
				{
					using (IDbCommand dbCommand = GetCommand(command, connInfo.Connection))
					{
						retVal = dbCommand.ExecuteNonQuery();
						command.Parameters.UpdateParameterValues();
					}
				}
			}
			catch (SystemException ex)
			{
				TS.Logger.WriteExceptionIf(DataAccess.Tracing && TS.EC.TraceError, ex);
				if (!command.OnDataException(ex))
				{
					throw;
				}
			}
			finally
			{
				sw.Stop();
				TS.Logger.WritePerformanceIf(DataAccess.Tracing && TS.EC.TraceInfo, sw.Elapsed);

				command.OnAfterExecute();
			}

			return retVal;
		}
		#endregion

		#region ExecuteScalar
		/// <summary>
		/// Execute a Scalar SqlCommand and returns the result.
		/// </summary>
		/// <param name="command">The SqlCommand to execute.</param>
		/// <returns>The result of the SqlCommand.</returns>
		public object ExecuteScalar(IDataCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			object retVal = null;
			command.OnBeforeExecute();

			Stopwatch sw = Stopwatch.StartNew();
			TS.Logger.WriteMethodCallIf(DataAccess.Tracing && TS.EC.TraceVerbose);

			try
			{
				using (DataConnection connInfo = new DataConnection(command, this))
				{
					using (IDbCommand dbCommand = GetCommand(command, connInfo.Connection))
					{
						retVal = dbCommand.ExecuteScalar();
						command.Parameters.UpdateParameterValues();
					}
				}
			}
			catch (SystemException ex)
			{
				TS.Logger.WriteExceptionIf(DataAccess.Tracing && TS.EC.TraceError, ex);
				if (!command.OnDataException(ex))
				{
					throw;
				}
			}
			finally
			{
				sw.Stop();
				TS.Logger.WritePerformanceIf(DataAccess.Tracing && TS.EC.TraceInfo, sw.Elapsed);

				command.OnAfterExecute();
			}

			return retVal;
		}
		#endregion

		#region ExecuteDataLoader
		/// <summary>
		/// Executes a SqlCommand and uses the resulting Data Reader to fill the object.
		/// </summary>
		/// <param name="command">The SqlCommand to execute.</param>
		public void ExecuteDataLoader(IDataLoaderCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			command.OnBeforeExecute();

			Stopwatch sw = Stopwatch.StartNew();
			TS.Logger.WriteMethodCallIf(DataAccess.Tracing && TS.EC.TraceVerbose);

			try
			{
				using (DataConnection connInfo = new DataConnection(command, this))
				{
					using (IDbCommand dbCommand = GetCommand(command, connInfo.Connection))
					{
						using (IDataReader reader = dbCommand.ExecuteReader())
						{
							command.DataLoader.ReadData(reader);
						}
						command.Parameters.UpdateParameterValues();
					}
				}
			}
			catch (SystemException ex)
			{
				TS.Logger.WriteExceptionIf(DataAccess.Tracing && TS.EC.TraceError, ex);
				if (!command.OnDataException(ex))
				{
					throw;
				}
			}
			finally
			{
				sw.Stop();
				TS.Logger.WritePerformanceIf(DataAccess.Tracing && TS.EC.TraceInfo, sw.Elapsed);

				command.OnAfterExecute();
			}
		}
		#endregion

		#region ExecuteDataXml
		/// <summary>
		/// Executes a IDbCommand and uses the resulting Xml Reader to fill the object.
		/// </summary>
		/// <param name="command">The IDbCommand to execute.</param>
		public void ExecuteDataXml(IXmlLoaderCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			command.OnBeforeExecute();

			Stopwatch sw = Stopwatch.StartNew();
			TS.Logger.WriteMethodCallIf(DataAccess.Tracing && TS.EC.TraceVerbose);

			try
			{
				using (DataConnection connInfo = new DataConnection(command, this))
				{
					ExecuteDataXml(command, connInfo.Connection);
					command.Parameters.UpdateParameterValues();
				}
			}
			catch (SystemException ex)
			{
				TS.Logger.WriteExceptionIf(DataAccess.Tracing && TS.EC.TraceError, ex);
				if (!command.OnDataException(ex))
				{
					throw;
				}
			}
			finally
			{
				sw.Stop();
				TS.Logger.WritePerformanceIf(DataAccess.Tracing && TS.EC.TraceInfo, sw.Elapsed);

				command.OnAfterExecute();
			}
		}
		#endregion

		#region Private Static Methods
		private static void CopyTables(DataSet source, DataSet target)
		{
			List<DataTable> list = new List<DataTable>();

			foreach (DataTable table in source.Tables)
			{
				list.Add(table);
			}

			list.ForEach(item =>
			{
				item.DataSet.Tables.Remove(item);

				string tableName = item.TableName;
				int idx = 2;

				while (target.Tables.Contains(tableName))
				{
					tableName = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", item.TableName, idx++);
				}

				if (item.TableName != tableName)
				{
					item.TableName = tableName;
				}

				target.Tables.Add(item);
			});
		}
		#endregion

	}
}
