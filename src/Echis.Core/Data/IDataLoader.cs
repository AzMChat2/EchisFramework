namespace System.Data
{
	/// <summary>
	/// Interface used in ExecuteDataLoader method.
	/// </summary>
	public interface IDataLoader
	{
		/// <summary>
		/// Called by the DataAccess object after execution with an open IDataReader object.
		/// </summary>
		/// <param name="reader">The IDataReader object containing the requested data.</param>
		void ReadData(IDataReader reader);
	}
}
