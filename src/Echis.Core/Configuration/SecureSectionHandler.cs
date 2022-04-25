using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

namespace System.Configuration
{
	/// <summary>
	/// The ConfigurationSectionHandler reads configurations settings from the
	/// configuration section of the Web or App config file.
	/// </summary>
	[Serializable]
	internal sealed class SecureSectionHandler : IConfigurationSectionHandler
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
					string msg = string.Format(CultureInfo.InvariantCulture,
							"The configuration section '{0}' is missing the required Filename attribute.", settingsTypeName);
					throw new ConfigurationErrorsException(msg);
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
						using (var stream = File.Open(fullFileName, FileMode.Open, FileAccess.Read, FileShare.None))
						{
							object retVal = null;
							try
							{
								var zipStream = new GZipStream(stream, CompressionMode.Decompress, true);
								retVal = serializer.Deserialize(zipStream);
							}
							catch (InvalidDataException)
							{
								stream.Position = 0;
								retVal = serializer.Deserialize(stream);
							}

							var settings = retVal as ISettings;
							if (settings != null)
							{
								settings.ConfigurationFileName = fullFileName;
								settings.ConfigurationIsSecure = true;
							}

							return retVal;
						}
					}
				}
			}
			catch (Exception ex)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "System.Configuration.SectionHandler.Create\r\nInvalid {0} Configuration.\r\n{1}", configSectionName, ex);
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
