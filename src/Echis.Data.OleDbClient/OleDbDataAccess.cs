using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/// <summary>
	/// Provides a MS OleDb Server implementation of DataAccess methods.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		MessageId = "Db",
		Justification = "'Db' is preferred over 'DB' to match the casing to that of Microsoft ADO.Net objects")]
	public class OleDbDataAccess : DataAccessBase
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public OleDbDataAccess() : base() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the Data Client</param>
		/// <param name="connectionString">The connection string used to connect to the database.</param>
		public OleDbDataAccess(string name, string connectionString) : base(name, connectionString) { }

		/// <summary>
		/// Get an IDbConnection object for MS OleDb Server databases..
		/// </summary>
		/// <returns>Returns an IDbConnection object for MS OleDb Server databases..</returns>
		public override IDbConnection CreateConnection()
		{
			return new OleDbConnection();
		}

		/// <summary>
		/// Get an OleDbCommand object for MS OleDb Server databases..
		/// </summary>
		/// <returns>Return an IDbCommand object for MS OleDb Server databases.</returns>
		public override IDbCommand CreateCommand()
		{
			return new OleDbCommand();
		}

		/// <summary>
		/// Get an OleDbDataParameter object for MS OleDb Server databases..
		/// </summary>
		/// <returns>Returns an IDbDataParameter object for MS OleDb Server databases..</returns>
		public override IDataParameter CreateDataParameter()
		{
			return new OleDbParameter();
		}


		/// <summary>
		/// Get an OleDbDataAdapter object for MS OleDb Server databases..
		/// </summary>
		/// <returns>Returns an IDbDataAdapter object for MS OleDb Server databases..</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		public override IDbDataAdapter CreateDataAdapter()
		{
			return new OleDbDataAdapter();
		}

		/// <summary>
		/// Executes a IDbCommand and uses the resulting Xml Reader to fill the object.
		/// </summary>
		/// <param name="command">The DB Generic command object.</param>
		/// <param name="connection">The IDbConnection which the resulting IDbCommand will use.</param>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			Justification = "ExecuteXmlReader is the name of a method. OleDb is the name of the DataAccess type.")]
		protected override void ExecuteDataXml(IXmlLoaderCommand command, IDbConnection connection)
		{
			throw new NotImplementedException("OleDb does not support ExecuteXmlReader");
		}
	}
}
