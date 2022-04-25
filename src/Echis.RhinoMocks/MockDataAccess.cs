using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Data;

namespace System.RhinoMocks
{
	/// <summary>
	/// Mock DataAccess Object - to be used for unit testing.
	/// </summary>
	public class MockDataAccess : IDataAccess
	{
		/// <summary>
		/// Gets the IDataAccess Mock object.
		/// </summary>
		public static IDataAccess Mock { get; private set; }
		/// <summary>
		/// Gets the IDbConnection Mock object.
		/// </summary>
		public static IDbConnection MockConnection { get; private set; }
		/// <summary>
		/// Gets the IDbCommand Mock object.
		/// </summary>
		public static IDbCommand MockCommand { get; private set; }
		/// <summary>
		/// Gets the IDataParameter Mock object.
		/// </summary>
		public static IDataParameter MockDataParameter { get; private set; }
		/// <summary>
		/// Gets the IDbTransaction Mock object.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
			MessageId = "Db",
		Justification = "'Db' is preferred over 'DB' to match the casing to that of Microsoft ADO.Net objects")]
		public static IDbTransaction MockDbTransaction { get; private set; }
		/// <summary>
		/// Gets the IDbDataAdapter Mock object.
		/// </summary>
		public static IDbDataAdapter MockDataAdapter { get; private set; }

		static MockDataAccess()
		{
			Initialize();
		}

		internal static void Initialize()
		{
			Mock = Repository.I.DynamicMock<IDataAccess>();
			MockConnection = Repository.I.DynamicMock<IDbConnection>();
			MockCommand = Repository.I.DynamicMock<IDbCommand>();
			MockDataParameter = Repository.I.DynamicMock<IDataParameter>();
			MockDbTransaction = Repository.I.DynamicMock<IDbTransaction>();
			MockDataAdapter = Repository.I.DynamicMock<IDbDataAdapter>();
		}

		/// <summary>
		/// Executes an IDbCommand and fills a DataSet with data.
		/// </summary>
		/// <param name="command">An object containing the IDbCommand and Dataset</param>
		public void ExecuteDataSet(IDataSetCommand command)
		{
			Mock.ExecuteDataSet(command);
		}

		/// <summary>
		/// Updates a database using the contents of a Dataset
		/// </summary>
		/// <param name="command">An object containing the DataSet.</param>
		public void UpdateDataSet(IDataSetCommand command)
		{
			Mock.UpdateDataSet(command);
		}

		/// <summary>
		/// Executes an IDbCommand and returns the number of rows effected.
		/// </summary>
		/// <param name="command">The IDbCommand to be executed.</param>
		/// <returns>The number of rows effected.</returns>
		public int ExecuteNonQuery(IDataCommand command)
		{
			return Mock.ExecuteNonQuery(command);
		}

		/// <summary>
		/// Execute a Scalar Command and returns the result.
		/// </summary>
		/// <param name="command">The Command to execute.</param>
		/// <returns>The result of the Command.</returns>
		public object ExecuteScalar(IDataCommand command)
		{
			return Mock.ExecuteScalar(command);
		}

		/// <summary>
		/// Executes an IDbCommand and uses the resulting Data Reader to fill the object.
		/// </summary>
		/// <param name="command">The Command to execute.</param>
		public void ExecuteDataLoader(IDataLoaderCommand command)
		{
			Mock.ExecuteDataLoader(command);
		}

		/// <summary>
		/// Executes a IDbCommand and uses the resulting Xml Reader to fill the object.
		/// </summary>
		/// <param name="command">The IDbCommand to execute.</param>
		public void ExecuteDataXml(IXmlLoaderCommand command)
		{
			Mock.ExecuteDataXml(command);
		}

		/// <summary>
		/// The name of this IDataAccess object.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The connection string used to connect to the database. (Not used by MockDataAccess)
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Get an IDbConnection object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbConnection object for the specific Database which this client represents.</returns>
		public IDbConnection CreateConnection()
		{
			return MockConnection;
		}

		/// <summary>
		/// Get an IDbCommand object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Return an IDbCommand object for the specific Database which this client represents</returns>
		public IDbCommand CreateCommand()
		{
			return MockCommand;
		}

		/// <summary>
		/// Get an IDbDataParameter object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbDataParameter object for the specific Database which this client represents.</returns>
		public IDataParameter CreateDataParameter()
		{
			return MockDataParameter;
		}

		/// <summary>
		/// Get an IDbTransaction object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbTransaction object for the specific Database which this client represents.</returns>
		public IDbTransaction GetTransaction()
		{
			return MockDbTransaction;
		}

		/// <summary>
		/// Get an IDbDataAdapter object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbDataAdapter object for the specific Database which this client represents.</returns>
		public IDbDataAdapter CreateDataAdapter()
		{
			return MockDataAdapter;
		}
	}
}
