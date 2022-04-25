using System;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration.Managers;
using ContainerSettings = System.Container.Settings;
using ConfigSettings = System.Configuration.Managers.Settings;

namespace System.Configuration
{
	/// <summary>
	/// The ConfigurationSectionHandler reads configurations settings from the
	/// configuration section of the Web or App config file.
	/// </summary>
	[Serializable]
	internal sealed class ExternalSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Constants used by the External Section Handler class.
		/// </summary>
		private static class Constants
		{
			/// <summary>
			/// Default IOC ObjectId for the Credentials Provider.
			/// </summary>
			public const string CredentialsProviderObjectId = "System.Configuration.CredentialsProvider";
			/// <summary>
			/// Default IOC ObjectId for the Configuration Manager.
			/// </summary>
			public const string ConfigurationManagerObjectId = "System.Configuration.ConfigurationManager";
		}

		#region Methods
		/// <summary>
		/// Used to read the settings from the config section.
		/// </summary>
		/// <param name="parent">Parent node</param>
		/// <param name="configContext">Configuration Context</param>
		/// <param name="section">Section to read.</param>
		/// <returns>Returns a Settings object populated with values from the App or Web Config file.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			if (section == null) throw new ArgumentNullException("section");

			string className = GetAttributeValue(section, "Type", section.Name);
			string assemblyName = GetAttributeValue(section, "Assembly", null);
			string managerName = GetAttributeValue(section, "ConfigurationManager", ConfigSettings.Values.ConfigurationManager);
			string credentialsProviderName = GetAttributeValue(section, "CredentialsProvider", ConfigSettings.Values.CredentialsProvider);

			string settingsTypeName = string.IsNullOrEmpty(assemblyName) ? className :
				string.Format(CultureInfo.InvariantCulture, "{0}, {1}", className, assemblyName);

			Type settingsType = Type.GetType(settingsTypeName);

			IConfigurationManager manager = null;

			try
			{
				if (settingsType == null)
				{
					string msg = string.Format(CultureInfo.InvariantCulture,
						"Unable to determine settings type. The type specified '{0}' was not found.", settingsTypeName);
					throw new ConfigurationErrorsException(msg);
				}

				ICredentialsProvider credentialsProvider = GetCredentialsProvider(credentialsProviderName);
				manager = GetConfigurationManager(managerName);

				string credentials = credentialsProvider.GetCredentials();
				string sectionData = manager.GetConfigurationSection(section.Name, credentials);

				return Deserialize(settingsType, sectionData);
			}
			catch (Exception ex)
			{
				// Special check for System.Container.Settings to prevent recursive calls.
				if (settingsType == typeof(ContainerSettings))
				{
					TS.Logger.WriteLine(TS.Categories.Error, "System.Configuration.RemoteSectionHandler.Create\r\nInvalid {0} Configuration.\r\n{1}", section.Name, ex);
				}
				else
				{
					TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "System.Configuration.RemoteSectionHandler.Create\r\nInvalid {0} Configuration.\r\n{1}", section.Name, ex);
				}
				throw;
			}
			finally
			{
				IDisposable disposable = manager as IDisposable;
				if (disposable != null) disposable.Dispose();
			}
		}

		private static ICredentialsProvider GetCredentialsProvider(string credentialsProviderName)
		{
			ICredentialsProvider retVal = IOC.GetFrameworkObject<ICredentialsProvider>(credentialsProviderName, Constants.CredentialsProviderObjectId, false);

			if (retVal == null)
			{
				// Not configured, use default.
				return new DefaultCredentialsProvider();
			}

			return retVal;
		}
		
		private static IConfigurationManager GetConfigurationManager(string managerName)
		{
			IConfigurationManager retVal = IOC.GetFrameworkObject<IConfigurationManager>(managerName, Constants.ConfigurationManagerObjectId, false);

			if (retVal == null)
			{
				// Not configured, throw exception.
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "The Configuration Manager specified '{0}' was not found.", managerName));
			}

			return retVal;
		}
		
		private static string GetAttributeValue(XmlNode section, string attributeName, string defaultValue)
		{
			XmlAttribute attribute = section.Attributes[attributeName];
			return (attribute == null) ? defaultValue : attribute.Value;
		}

		/// <summary>
		/// Uses an XmlSerializer to create an object from the XmlNode specified.
		/// </summary>
		/// <param name="settingsType">The type of settings object to create.</param>
		/// <param name="sectionData">The serialized data of the settings object.</param>
		/// <returns>Returns the deserialized settings object.</returns>
		private static object Deserialize(Type settingsType, string sectionData)
		{
			XmlSerializer serializer = new XmlSerializer(settingsType);
			using (StringReader stringReader = new StringReader(sectionData))
			{
				return serializer.Deserialize(new XmlTextReader(stringReader));
			}
		}
		#endregion
	}
}
