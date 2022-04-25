using System;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Xml;
using System.Xml.Serialization;

namespace System.Configuration.Install
{
	/// <summary>
	/// Settings used to install a service using the InstallerEx class.
	/// </summary>
	public class Settings 
	{
		/// <summary>
		/// The Configuration Section containing the Install Settings.
		/// </summary>
		private const string ConfigSectionName = "System.Configuration.Install.Settings";

		/// <summary>
		/// The serializer used to deserialize the Install Settings.
		/// </summary>
		private static XmlSerializer serializer = new XmlSerializer(typeof(Settings));

		/// <summary>
		/// Stores the global instace of the Settings object.
		/// </summary>
		private static Settings _values;
		/// <summary>
		/// Gets the global instance of the Settings object.
		/// </summary>
		public static Settings Values
		{
			get
			{
				if (_values == null)
				{
					string fileName = null;
					int index = 0;
					while (string.IsNullOrEmpty(fileName))
					{
						MethodBase mb = new StackFrame(index++).GetMethod();
						if (mb.ReflectedType.Assembly != typeof(Settings).Assembly)
						{
							fileName = string.Format(CultureInfo.InvariantCulture, "{0}.config", mb.ReflectedType.Assembly.Location);
						}
					}

					ReadSettings(fileName);
				}
				return _values;
			}
		}

		/// <summary>
		/// Reads the settings from the configuration file.
		/// </summary>
		/// <param name="fileName">The filename containing the configuration section.</param>
		private static void ReadSettings(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("fileName");
			}
			else if (File.Exists(fileName))
			{
				using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					XmlTextReader reader = new XmlTextReader(stream);
					if (reader.ReadToFollowing(ConfigSectionName))
					{
						string configFileName = reader.GetAttribute("Filename");

						if (string.IsNullOrEmpty(configFileName) || fileName.Equals(configFileName, StringComparison.OrdinalIgnoreCase))
						{
							reader.ReadToFollowing(typeof(Settings).Name);
							_values = (Settings)serializer.Deserialize(reader);
						}
						else
						{
							if (string.IsNullOrEmpty(Path.GetPathRoot(configFileName)))
							{
								configFileName = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", Path.GetDirectoryName(fileName), configFileName);
							}
							ReadSettings(configFileName);
						}
					}
					else
					{
						string exMsg = string.Format(CultureInfo.InvariantCulture, "The specified config section ('{0}') was not found in the specified configuration file ('{1}').", ConfigSectionName, fileName);
						throw new ConfigurationErrorsException(exMsg);
					}
				}
			}
			else
			{
				string exMsg = string.Format(CultureInfo.InvariantCulture, "The specified configuration file ('{0}') was not found.", fileName);
				throw new FileNotFoundException(exMsg);
			}
		}

		/// <summary>
		/// Gets or sets the Windows Account under which the Service will initially be configured to run.
		/// </summary>
		[XmlAttribute]
		public ServiceAccount ServiceAccount { get; set; }

		/// <summary>
		/// Gets or sets the initial Windows Service Start type for the Service.
		/// </summary>
		[XmlAttribute]
		public ServiceStartMode StartType { get; set; }

		/// <summary>
		/// Gets or sets the Service name for the Service.
		/// </summary>
		[XmlAttribute]
		public string ServiceName { get; set; }

		/// <summary>
		/// Gets or sets the Display name for the Service.
		/// </summary>
		[XmlAttribute]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the Service description for the Service.
		/// </summary>
		[XmlAttribute]
		public string Description { get; set; }
	}
}
