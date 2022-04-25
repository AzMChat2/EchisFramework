using System.Configuration.Managers.FileSystem;
using System;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Configuration Manager which retrieves configuration sections from a file system.
	/// </summary>
	public class FileSystemConfigurationManager : ConfigurationManagerBase<Credentials>
	{
		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		protected override string GetConfigurationSection(string configSectionName, Credentials credentials)
		{
			if (credentials == null) throw new ArgumentNullException("credentials");
			return ApplicationEnvironments.GetConfigSection(credentials.Application, credentials.Environment.ToString(), configSectionName);
		}
	}
}
