
namespace System.Configuration.Managers
{
	/// <summary>
	/// A Configuration Service base class used to provide secure (encrypted) external Configuration Management.
	/// </summary>
	public abstract class SecureConfigurationService : IConfigurationManager
	{
		/// <summary>
		/// In derived classes, gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>In derived classes, returns a string of Data containing the specified Configuration Section</returns>
		protected abstract string GetConfiguration(string configSectionName, string credentials);

		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		public string GetConfigurationSection(string configSectionName, string credentials)
		{
			string decryptedCredentials = ConfigurationEncryptor.DecryptString(credentials);
			string configSectionData = GetConfiguration(configSectionName, decryptedCredentials);
			return ConfigurationEncryptor.EncryptString(configSectionData);
		}
	}
}
