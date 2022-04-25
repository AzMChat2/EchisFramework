using System.Diagnostics.CodeAnalysis;
using System.Configuration.Managers.Remote;

namespace System.Configuration.Managers
{
	/// <summary>
	/// The Remote Configuration Manager class provides access to a Remote Configuration Service.
	/// </summary>
	public class RemoteConfigurationManager : SecureConfigurationClient
	{
		/// <summary>
		/// Gets a new Remote Configuration Service Client object.
		/// </summary>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		protected override IConfigurationManager GetConfigurationService()
		{
			return new RemoteConfigurationClient();
		}
	}
}
