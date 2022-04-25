using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/// <summary>
	/// Interface used to define DataAccess objects.
	/// </summary>
	public interface IDataAccess : IDataClient
	{
		/// <summary>
		/// Get an IDbTransaction object.
		/// </summary>
		/// <returns>Returns an IDbTransaction object for MS Sql Server databases..</returns>
		/// <remarks>If the Connection provided is not an open state, then it will be opened.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "A property is not desireable here.")]
		IDbTransaction GetTransaction();

		/// <summary>
		/// Executes an IDbCommand and fills a DataSet with data.
		/// </summary>
		/// <param name="command">An object containing the IDbCommand and Dataset</param>
		void ExecuteDataSet(IDataSetCommand command);

		/// <summary>
		/// Updates a database using the contents of a Dataset
		/// </summary>
		/// <param name="command">An object containing the DataSet.</param>
		void UpdateDataSet(IDataSetCommand command);

		/// <summary>
		/// Executes an IDbCommand and returns the number of rows effected.
		/// </summary>
		/// <param name="command">The IDbCommand to be executed.</param>
		/// <returns>The number of rows effected.</returns>
		int ExecuteNonQuery(IDataCommand command);

		/// <summary>
		/// Execute a Scalar Command and returns the result.
		/// </summary>
		/// <param name="command">The Command to execute.</param>
		/// <returns>The result of the Command.</returns>
		object ExecuteScalar(IDataCommand command);

		/// <summary>
		/// Executes an IDbCommand and uses the resulting Data Reader to fill the object.
		/// </summary>
		/// <param name="command">The Command to execute.</param>
		void ExecuteDataLoader(IDataLoaderCommand command);

		/// <summary>
		/// Executes a IDbCommand and uses the resulting Xml Reader to fill the object.
		/// </summary>
		/// <param name="command">The IDbCommand to execute.</param>
		void ExecuteDataXml(IXmlLoaderCommand command);
	}
}
