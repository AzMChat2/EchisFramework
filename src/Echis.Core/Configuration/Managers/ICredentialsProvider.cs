
using System.Diagnostics.CodeAnalysis;
namespace System.Configuration.Managers
{
	/// <summary>
	/// Interface for a Configuration Credentials Provider
	/// </summary>
	public interface ICredentialsProvider
	{
		/// <summary>
		/// Gets a string representing credentials used to retrieve configuration sections.
		/// </summary>
		/// <returns>Returns a string representing credentials used to retrieve configuration sections.</returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "A property is not desireable here.")]
		string GetCredentials();
	}
}
