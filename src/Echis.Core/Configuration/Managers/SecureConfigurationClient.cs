using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Configuration.Managers
{
	/// <summary>
	/// A Configuration Client base class used to provide secure (encrypted) external Configuration Management.
	/// </summary>
	public abstract class SecureConfigurationClient : IConfigurationManager
	{
		/// <summary>
		/// Gets the Configuration Service instance.
		/// </summary>
		/// <remarks>If the returned object is IDisposable then it will be disposed at the end of the current call.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "A property is not desireable here.")]
		protected abstract IConfigurationManager GetConfigurationService();

		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		public string GetConfigurationSection(string configSectionName, string credentials)
		{
			IConfigurationManager service = null;

			try
			{
				service = GetConfigurationService();

				string encryptedCredentials = ConfigurationEncryptor.EncryptString(credentials);
				string configSectionData = service.GetConfigurationSection(configSectionName, encryptedCredentials);

				return ConfigurationEncryptor.DecryptString(configSectionData);
			}
			finally
			{
				IDisposable disposable = service as IDisposable;
				if (disposable != null) disposable.Dispose();
			}

		}
	}
}
