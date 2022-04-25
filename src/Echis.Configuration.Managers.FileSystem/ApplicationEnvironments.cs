using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace System.Configuration.Managers.FileSystem
{
	/// <summary>
	/// Represents the In Memory cache of Application Environment Configurations including Configuration Sections, User Groups and Machine Groups.
	/// </summary>
	internal class ApplicationEnvironments : Dictionary<string, ApplicationEnvironment>
	{
		/// <summary>
		/// Stores the global instance of the Application Environments dictionary.
		/// </summary>
		private static ApplicationEnvironments _store = new ApplicationEnvironments();

		/// <summary>
		/// Gets the config section for the specified application, environment and configuration section.
		/// </summary>
		/// <param name="application">The name of the Application.</param>
		/// <param name="environment">The name of the Environment.</param>
		/// <param name="configSection">The configuration section to retrieve.</param>
		/// <returns></returns>
		public static string GetConfigSection(string application, string environment, string configSection)
		{
			return GetApplicationEnvironment(application, environment).GetConfigSection(configSection);
		}

		/// <summary>
		/// Determines if the specified Application Environment has at least one of the specified User Groups.
		/// </summary>
		/// <param name="application">The name of the Application.</param>
		/// <param name="environment">The name of the Environment.</param>
		/// <param name="userGroups">The list of User Groups to check.</param>
		/// <returns>Returns true if the specified Application Environment has at least one of the specified User Groups.</returns>
		public static bool HasUserGroup(string application, string environment, List<string> userGroups)
		{
			ApplicationEnvironment appEnv = GetApplicationEnvironment(application, environment);
			return userGroups.Exists(appEnv.HasUserGroup);
		}

		/// <summary>
		/// Determines if the specified Application Environment has at least one of the specified Machine Groups.
		/// </summary>
		/// <param name="application">The name of the Application.</param>
		/// <param name="environment">The name of the Environment.</param>
		/// <param name="machineGroups">The list of Machine Groups to check.</param>
		/// <returns>Returns true if the specified Application Environment has at least one of the specified Machine Groups.</returns>
		public static bool HasMachineGroup(string application, string environment, List<string> machineGroups)
		{
			ApplicationEnvironment appEnv = GetApplicationEnvironment(application, environment);
			return machineGroups.Exists(appEnv.HasMachineGroup);
		}

		/// <summary>
		/// Gets the Application Environment object which represents the specified Application and Environment.
		/// </summary>
		/// <param name="application">The name of the Application.</param>
		/// <param name="environment">The name of the Environment.</param>
		/// <returns>Returns the Application Environment object which represents the specified Application and Environment.</returns>
		private static ApplicationEnvironment GetApplicationEnvironment(string application, string environment)
		{
			string key = string.Format(CultureInfo.InvariantCulture, "|:{0}:|:{1}:|", application, environment);

			ApplicationEnvironment appEnviron;
			
			if (_store.ContainsKey(key))
			{
				appEnviron = _store[key];
			}
			else
			{
				appEnviron = new ApplicationEnvironment()
				{
					ApplicationName = application,
					EnvironmentName = environment
				};
				_store.Add(key, appEnviron);
			}

			return appEnviron;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		private ApplicationEnvironments() { }
	}

	/// <summary>
	/// Represents an Application Environment.
	/// </summary>
	internal class ApplicationEnvironment
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public ApplicationEnvironment()
		{
			_configurationSections = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets the name of the Application.
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		/// Gets or sets the name of the Environment.
		/// </summary>
		public string EnvironmentName { get; set; }

		/// <summary>
		/// Stores the loaded Configuration Sections for this Application Environment.
		/// </summary>
		private Dictionary<string, string> _configurationSections;

		/// <summary>
		/// Stores the list of User Groups for this Application Environment.
		/// </summary>
		private UserGroupList _userGroups;

		/// <summary>
		/// Stores the list of Machine Groups for this Application Environment.
		/// </summary>
		private MachineGroupList _machineGroups;

		/// <summary>
		/// Gets the Configuration Section specified for this Application Environment.
		/// </summary>
		/// <param name="configSectionName">The configuration section to retrieve.</param>
		/// <returns>Returns the Configuration Section specified for this Application Environment.</returns>
		public string GetConfigSection(string configSectionName)
		{
			string retVal;

			if (_configurationSections.ContainsKey(configSectionName))
			{
				retVal = _configurationSections[configSectionName];
			}
			else
			{
				retVal = LoadConfiguration(configSectionName);
				_configurationSections.Add(configSectionName, retVal);
			}

			return retVal;
		}

		/// <summary>
		/// Loads the specified configuration from this Application Environments configuration file.
		/// </summary>
		/// <param name="configSectionName">The configuration section to load.</param>
		/// <returns>Returns the Configuration Section specified for this Application Environment.</returns>
		private string LoadConfiguration(string configSectionName)
		{
			string fileName = string.Format(CultureInfo.InvariantCulture, Settings.Values.ConfigurationFilePath, EnvironmentName, ApplicationName);
			string retVal = null;

			using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				XmlTextReader reader = new XmlTextReader(stream);
				if (reader.ReadToFollowing(configSectionName))
				{
					retVal = reader.ReadInnerXml();
				}
			}

			return retVal;
		}

		/// <summary>
		/// Determines if the specified User Group exists for this Application Environment.
		/// </summary>
		/// <param name="userGroup">The User Group to check.</param>
		/// <returns>Returns true if the specified User Group exists for this Application Environment.</returns>
		public bool HasUserGroup(string userGroup)
		{
			if (_userGroups == null) LoadUserGroups();

			return _userGroups.Exists(item => item.Name.Equals(userGroup, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Determines if the specified Machine Group exists for this Application Environment.
		/// </summary>
		/// <param name="machineGroup">The Machine Group to check.</param>
		/// <returns>Returns true if the specified Machine Group exists for this Application Environment.</returns>
		public bool HasMachineGroup(string machineGroup)
		{
			if (_machineGroups == null) LoadMachineGroups();

			return _machineGroups.Exists(item => item.Name.Equals(machineGroup, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Loads the User Groups for this Application Environment from the configuration file.
		/// </summary>
		private void LoadUserGroups()
		{
			string fileName = string.Format(CultureInfo.InvariantCulture, Settings.Values.ConfigurationFilePath, EnvironmentName, ApplicationName);

			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			using (XmlReader reader = new XmlNodeReader(doc.SelectSingleNode(Settings.Values.UserGroupsXPath)))
			{
				_userGroups = (UserGroupList)XmlSerializer<UserGroupList>.Serializer.Deserialize(reader);
			}
		}

		/// <summary>
		/// Loads the Machine Groups for this Application Environment from the configuration file.
		/// </summary>
		private void LoadMachineGroups()
		{
			string fileName = string.Format(CultureInfo.InvariantCulture, Settings.Values.ConfigurationFilePath, EnvironmentName, ApplicationName);

			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			using (XmlReader reader = new XmlNodeReader(doc.SelectSingleNode(Settings.Values.MachineGroupsXPath)))
			{
				_machineGroups = (MachineGroupList)XmlSerializer<MachineGroupList>.Serializer.Deserialize(reader);
			}
		}
	}
}
