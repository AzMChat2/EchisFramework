using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace System.Data
{

	/// <summary>
	/// Provides a DataLoader instance which utilizes a delegate.
	/// </summary>
	public sealed class DelegateDataLoader : IDataLoader
	{
		/// <summary>
		/// The delegate method to invoke when reading data.
		/// </summary>
		private DataLoaderHandler _loaderMethod;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loaderMethod">The delegate method to invoke when reading data.</param>
		public DelegateDataLoader(DataLoaderHandler loaderMethod)
		{
			if (loaderMethod == null) throw new ArgumentNullException("loaderMethod");
			_loaderMethod = loaderMethod;
		}

		/// <summary>
		/// Invokes the delegate method.
		/// </summary>
		/// <param name="reader">The reader containing the data.</param>
		public void ReadData(IDataReader reader)
		{
			_loaderMethod.Invoke(reader);
		}
	}

	/// <summary>
	/// Represents a method to be invoked for reading data from a data reader.
	/// </summary>
	/// <param name="reader">The reader containing the data.</param>
	public delegate void DataLoaderHandler(IDataReader reader);

}
