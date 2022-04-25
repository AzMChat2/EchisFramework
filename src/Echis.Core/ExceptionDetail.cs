using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	/// <summary>
	/// Stores details about an exception.
	/// </summary>
	public class ExceptionDetail<T>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="source">The object which caused the exception.</param>
		/// <param name="exception">The exception which was thrown.</param>
		public ExceptionDetail(T source, Exception exception)
		{
			Source = source;
			Exception = exception;
		}

		/// <summary>
		/// Gets the object which caused the exception.
		/// </summary>
		public T Source { get; private set; }

		/// <summary>
		/// Gets the exception which was thrown.
		/// </summary>
		public Exception Exception { get; private set; }
	}
}
