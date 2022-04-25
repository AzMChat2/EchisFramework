using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/// <summary>
	/// Attribute indicating that Validatable method parameters should not be validated.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Code Analysis false message.")]
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class SkipValidateAttribute : Attribute, IParameterNames
	{
		/// <summary>
		/// Creates a new instance of the Skip Validation Attribute
		/// </summary>
		public SkipValidateAttribute() { }

		/// <summary>
		/// Creates a new instance of the Skip Validation Attribute
		/// </summary>
		/// <param name="parameterNames">(Optional) The names of the parameters to be skipped.</param>
		public SkipValidateAttribute(params string[] parameterNames)
		{
			ParameterNames = parameterNames.IsNullOrEmpty() ? null : new List<string>(parameterNames);
		}

		/// <summary>
		/// Gets the name of the parameters to be skipped.
		/// </summary>
		public List<string> ParameterNames { get; private set; }
	}
}
