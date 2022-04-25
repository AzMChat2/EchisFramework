using System.Data;

namespace System.Data
{
	/// <summary>
	/// Represents an XmlLoader Command.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class XmlLoaderCommand<T> : DataCommand, IXmlLoaderCommand where T : IXmlLoader
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public XmlLoaderCommand(T xmlLoader) : base() 
		{
			XmlLoader = xmlLoader;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="commandText">The command text to be executed.</param>
		/// <param name="commandType">The type of command to be executed.</param>
		/// <param name="queryParams">Parameters (if any) of the command to be executed.</param>
		/// <param name="xmlLoader">The XmlLoader object which will be called with the XmlReader resulting from the execution of this command.</param>
		public XmlLoaderCommand(T xmlLoader, string dataAccessName, string commandText, CommandType commandType, params IQueryParameter[] queryParams)
			: base(dataAccessName, commandText, commandType, queryParams)
		{
			XmlLoader = xmlLoader;
		}

		/// <summary>
		/// Gets or sets the IDataLoader object.
		/// </summary>
		public T XmlLoader { get; set; }

		IXmlLoader IXmlLoaderCommand.XmlLoader
		{
			get { return XmlLoader; }
			set { XmlLoader = (T)value; }
		}
	}
}
