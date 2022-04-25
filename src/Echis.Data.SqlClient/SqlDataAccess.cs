using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Data
{
	/// <summary>
	/// Provides a MS Sql Server implementation of DataAccess methods.
	/// </summary>
	public class SqlDataAccess : DataAccessBase
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public SqlDataAccess() : base() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the Data Client</param>
		/// <param name="connectionString">The connection string used to connect to the database.</param>
		public SqlDataAccess(string name, string connectionString) : base(name, connectionString) { }

		/// <summary>
		/// Get an IDbConnection object for MS Sql Server databases..
		/// </summary>
		/// <returns>Returns an IDbConnection object for MS Sql Server databases..</returns>
		public override IDbConnection CreateConnection()
		{
			return new SqlConnection();
		}

		/// <summary>
		/// Get an SqlCommand object for MS Sql Server databases..
		/// </summary>
		/// <returns>Return an IDbCommand object for MS Sql Server databases.</returns>
		public override IDbCommand CreateCommand()
		{
			return new SqlCommand();
		}

		/// <summary>
		/// Get an SqlDataParameter object for MS Sql Server databases..
		/// </summary>
		/// <returns>Returns an IDbDataParameter object for MS Sql Server databases..</returns>
		public override IDataParameter CreateDataParameter()
		{
			return new SqlParameter();
		}

		/// <summary>
		/// Get an SqlDataAdapter object for MS Sql Server databases..
		/// </summary>
		/// <returns>Returns an IDbDataAdapter object for MS Sql Server databases..</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		public override IDbDataAdapter CreateDataAdapter()
		{
			return new SqlDataAdapter();
		}

		/// <summary>
		/// Executes a IDbCommand and uses the resulting Xml Reader to fill the object.
		/// </summary>
		/// <param name="command">The DB Generic command object.</param>
		/// <param name="connection">The IDbConnection which the resulting IDbCommand will use.</param>
		protected override void ExecuteDataXml(IXmlLoaderCommand command, IDbConnection connection)
		{
			if (command == null) throw new ArgumentNullException("command");
			if (connection == null) throw new ArgumentNullException("connection");

			SqlConnection sqlConn = (SqlConnection)connection;
			using (SqlCommand sqlCommand = (SqlCommand)GetCommand(command, sqlConn))
			{
				using (XmlReader reader = sqlCommand.ExecuteXmlReader())
				{
					command.XmlLoader.ReadXml(reader);
				}
			}
		}
	}
}
