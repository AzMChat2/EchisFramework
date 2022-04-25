using System.Data;

namespace System.Data
{
	/// <summary>
	/// Provides an interface for Database Clients for the generation of DB Specific data objects.
	/// </summary>
	public interface IDataClient
	{
		/// <summary>
		/// The name of this IDataAccess object.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Get an IDbConnection object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbConnection object for the specific Database which this client represents.</returns>
		IDbConnection CreateConnection();
	
		/// <summary>
		/// Get an IDbCommand object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Return an IDbCommand object for the specific Database which this client represents</returns>
		IDbCommand CreateCommand();

		/// <summary>
		/// Get an IDbDataParameter object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbDataParameter object for the specific Database which this client represents.</returns>
		IDataParameter CreateDataParameter();

		/// <summary>
		/// Get an IDbDataAdapter object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an IDbDataAdapter object for the specific Database which this client represents.</returns>
		IDbDataAdapter CreateDataAdapter();
	}
}
