using System.Collections.Generic;

namespace System
{
	/// <summary>
	/// Defines an object which contains a list of paramter names
	/// </summary>
	public interface IParameterNames
	{
		/// <summary>
		/// Gets the list of parameter names for this object.
		/// </summary>
		List<string> ParameterNames { get; }
	}
}
