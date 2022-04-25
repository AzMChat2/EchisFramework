using System.Data;

namespace System.Data
{
	/// <summary>
	/// Represents a DataLoader Command.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DataLoaderCommand<T> : DataCommand, IDataLoaderCommand where T : IDataLoader
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public DataLoaderCommand(T dataLoader) : base()
		{
			DataLoader = dataLoader;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="commandText">The command text to be executed.</param>
		/// <param name="commandType">The type of command to be executed.</param>
		/// <param name="queryParams">Parameters (if any) of the command to be executed.</param>
		/// <param name="dataLoader">The DataLoader object which will be called with the DataReader resulting from the execution of this command.</param>
		public DataLoaderCommand(T dataLoader, string dataAccessName, string commandText, CommandType commandType, params IQueryParameter[] queryParams)
			: base(dataAccessName, commandText, commandType, queryParams)
		{
			DataLoader = dataLoader;
		}

		/// <summary>
		/// Gets or sets the IDataLoader object.
		/// </summary>
		public T DataLoader { get; set; }

		IDataLoader IDataLoaderCommand.DataLoader
		{
			get { return DataLoader; }
			set { DataLoader = (T)value; }
		}
	}
}
