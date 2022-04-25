using System;
using System.Configuration;
using System.Globalization;
using System.ServiceModel;

namespace System.Configuration.Managers.Remote
{
	/// <summary>
	/// Remote Configuration Service.  Provides a service end point for External Configuration Management.
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
	public sealed class RemoteConfigurationService : SecureConfigurationService, IRemoteConfigurationService
	{
		/// <summary>
		/// Constants used by the External Section Handler class.
		/// </summary>
		private static class Constants
		{
			/// <summary>
			/// Default IOC ObjectId for the Configuration Manager.
			/// </summary>
			public const string ConfigurationManagerObjectId = "System.Configuration.ConfigurationManager";
		}
	
		/// <summary>
		/// Get the actual Configuration Manager used to retrieve configuration sections.
		/// </summary>
		private static IConfigurationManager _manager = null;

		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		protected override string GetConfiguration(string configSectionName, string credentials)
		{
			if (_manager == null)
			{
				_manager = GetConfigurationManager();
			}
			return _manager.GetConfigurationSection(configSectionName, credentials);
		}

		/// <summary>
		/// Creates an instance of the configured Configuration Manager.
		/// </summary>
		/// <returns>Returns a new instance of the configured Configuration Manager.</returns>
		private static IConfigurationManager GetConfigurationManager()
		{
			IConfigurationManager retVal = IOC.GetFrameworkObject<IConfigurationManager>(Settings.Values.ConfigurationManager, Constants.ConfigurationManagerObjectId, false);

			if (retVal == null)
			{
				// Not configured, throw exception.
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "The Configuration Manager specified '{0}' was not found.", Settings.Values.ConfigurationManager));
			}

			return retVal;
		}
	}
}
