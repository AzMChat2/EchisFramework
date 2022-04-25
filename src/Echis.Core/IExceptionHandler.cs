using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System
{
	/// <summary>
	/// Defines an Exception Handler to handle exceptions.
	/// </summary>
	public interface IExceptionHandler
	{
		/// <summary>
		/// Handles an exception.
		/// </summary>
		/// <param name="methodInfo">The method info for the method which threw the exception.</param>
		/// <param name="ex">The exception which was caught.</param>
		/// <param name="arguments">The arguments which were passed into the exception.</param>
		object HandleException(MethodInfo methodInfo, Exception ex, params object[] arguments);
	}
}
