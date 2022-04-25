using System.ServiceModel;

namespace System.Configuration.Managers.Remote
{
	/// <summary>
	/// Used internally to connect to the Remote Configuration Service.
	/// </summary>
	internal class RemoteConfigurationClient : ReliableClientBase<IRemoteConfigurationService>, IConfigurationManager
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public RemoteConfigurationClient() : base("System.Configuration.Remote.Service") { }

		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		public string GetConfigurationSection(string configSectionName, string credentials)
		{
			return Service.GetConfigurationSection(configSectionName, credentials);
		}
	}
}
