using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/// <summary>
	/// Attribute indicating that Updateable method parameters should not be updated.
	/// </summary>
  [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Code Analysis false message.")]
  [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class SkipUpdateAttribute : Attribute, IParameterNames
  {
		/// <summary>
		/// Creates a new instance of the Skip Update Attribute
		/// </summary>
		public SkipUpdateAttribute() { }

		/// <summary>
		/// Creates a new instance of the Skip Update Attribute
		/// </summary>
		/// <param name="parameterNames">(Optional) The names of the parameters to be skipped.</param>
    public SkipUpdateAttribute(params string[] parameterNames)
    {
			ParameterNames = parameterNames.IsNullOrEmpty() ? null : new List<string>(parameterNames);
    }

		/// <summary>
		/// Gets the name of the parameters to be skipped.
		/// </summary>
		public List<string> ParameterNames { get; private set; }
  }
}
