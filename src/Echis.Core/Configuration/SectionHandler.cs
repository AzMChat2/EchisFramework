using System;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ContainerSettings = System.Container.Settings;

namespace System.Configuration
{
	/// <summary>
	/// The ConfigurationSectionHandler reads configurations settings from the
	/// configuration section of the Web or App config file.
	/// </summary>
	[Serializable]
	internal sealed class SectionHandler : IConfigurationSectionHandler
	{
		#region Methods
		/// <summary>
		/// Used to read the settings from the config section.
		/// </summary>
		/// <param name="parent">Parent node</param>
		/// <param name="configContext">Configuration Context</param>
		/// <param name="section">Section to read.</param>
		/// <returns>Returns a Settings object populated with values from the App, Web or other Config file.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			if (section == null) throw new ArgumentNullException("section");

			string configSectionName = section.Name;
			string className = (section.Attributes["Type"] == null) ? configSectionName : section.Attributes["Type"].Value;
			string assemblyName = (section.Attributes["Assembly"] == null) ? null : section.Attributes["Assembly"].Value;
			string fileName = (section.Attributes["Filename"] == null) ? null : section.Attributes["Filename"].Value;

			string settingsTypeName = string.IsNullOrEmpty(assemblyName) ? className :
				string.Format(CultureInfo.InvariantCulture, "{0}, {1}", className, assemblyName);

			Type settingsType = Type.GetType(settingsTypeName);

			try
			{
				if (settingsType == null)
				{
					string msg = string.Format(CultureInfo.InvariantCulture,
							"Unable to determine settings type. The type specified '{0}' was not found.", settingsTypeName);
					throw new ConfigurationErrorsException(msg);
				}

				XmlSerializer serializer = new XmlSerializer(settingsType);

				if (string.IsNullOrEmpty(fileName))
				{
					using (XmlReader reader = new XmlNodeReader(section))
					{
						reader.ReadToFollowing(settingsType.Name);
						return serializer.Deserialize(reader);
					}
				}
				else
				{
					string fullFileName = GetFullFileName(fileName);

					if (string.IsNullOrEmpty(fullFileName))
					{
						string exMsg = string.Format(CultureInfo.InvariantCulture, "The specified configuration file ('{0}') was not found.", fileName);
						throw new FileNotFoundException(exMsg);
					}
					else
					{
						using (FileStream stream = File.Open(fullFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							XmlTextReader reader = new XmlTextReader(stream);
							if (reader.ReadToFollowing(configSectionName))
							{
								reader.ReadToFollowing(settingsType.Name);

								object retVal = serializer.Deserialize(reader);

								var settings = retVal as ISettings;
								if (settings != null)
								{
									settings.ConfigurationFileName = fullFileName;
									settings.ConfigurationIsSecure = false;
								}

								return retVal;
							}
							else
							{
								string exMsg = string.Format(CultureInfo.InvariantCulture, "The specified config section ('{0}') was not found in the specified configuration file ('{1}').", configSectionName, fileName);
								throw new ConfigurationErrorsException(exMsg);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Special check for System.Container.Settings to prevent recursive calls.
				if (settingsType == typeof(ContainerSettings))
				{
					TS.Logger.WriteLine(TS.Categories.Error, "System.Configuration.SectionHandler.Create\r\nInvalid {0} Configuration.\r\n{1}", configSectionName, ex);
				}
				else
				{
					TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "System.Configuration.SectionHandler.Create\r\nInvalid {0} Configuration.\r\n{1}", configSectionName, ex);
				}
				throw;
			}
		}

		/// <summary>
		/// Determines the full path and filename of the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>
		/// If the file exists, returns the full path and filename of the specified file.
		/// If the file does not exist, returns null.
		/// </returns>
		private static string GetFullFileName(string fileName)
		{
			FileInfo info = null;
			string baseDirFileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;

			if (File.Exists(fileName))
			{
				info = new FileInfo(fileName);
			}
			else if (File.Exists(baseDirFileName))
			{
				info = new FileInfo(baseDirFileName);
			}

			return (info == null) ? null : info.FullName;
		}
		#endregion
	}

}
