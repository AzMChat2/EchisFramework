using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace System.Configuration
{
	/// <summary>
	/// Provides a base class from which all Application Settings classes may be derived.
	/// </summary>
	/// <remarks>Also provides a static property to access a singleton instance of the settings class.</remarks>
	/// <typeparam name="T">The type of settings class which derives this abstract base class.</typeparam>
	public abstract class SettingsBase<T> : ISettings where T : SettingsBase<T>, new()
	{
		/// <summary>
		/// Gets the Configuration Section Name containing the serialized Settings Data for this Settings class.
		/// </summary>
		/// <remarks>
		/// Value is the Settings Type Full Name.
		/// </remarks>
		protected static string ConfigSectionName
		{
			get { return typeof(T).FullName; }
		}

		/// <summary>
		/// Gets or sets the configuration file name to which the settings file will be persisted when calling the Save() method.
		/// </summary>
		public string ConfigurationFileName { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the configuration file is created using a GZipStream.
		/// </summary>
		public bool ConfigurationIsSecure { get; set; }

		/// <summary>
		/// Saves the current configuration to the Configuration File.
		/// </summary>
		public static void Save()
		{
			Save(_values, _values.ConfigurationFileName, _values.ConfigurationIsSecure);
		}

		/// <summary>
		/// Saves the specified configuration to the specified configuration file.
		/// </summary>
		/// <param name="settings">The configuration settings to be persisted.</param>
		/// <param name="fileName">The name of the file to which the settings will be persisted.</param>
		/// <param name="useGZip">A boolean flag indicating if the settings file should be created using a GZipStream (Compressed).</param>
		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public static void Save(T settings, string fileName, bool useGZip)
		{
			if (settings == default(T)) throw new ArgumentNullException("settings");
			if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException("fileName");

			using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				if (useGZip)
				{
					using (var zipStream = new GZipStream(stream, CompressionMode.Compress, true))
					{
						XmlSerializer<T>.Serialize(zipStream, settings);
					}
				}
				else
				{
					XmlSerializer<T>.Serialize(stream, settings);
				}
			}
		}

		/// <summary>
		/// Loads the current configuration from the Configuration File.
		/// </summary>
		public static void Load()
		{
			if (_values == null)
			{
				ReadSettings();
			}
			else
			{
				_values = Load(_values.ConfigurationFileName, _values.ConfigurationIsSecure);
			}
		}

		/// <summary>
		/// Loads the current configuration from the Configuration File.
		/// </summary>
		/// <param name="fileName">The name of the file from which the settings will be deserialized.</param>
		/// <param name="useZipStream">Determines if a Zip stream should be used to read the configuration settings.</param>
		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public static T Load(string fileName, bool useZipStream)
		{
			using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				T retVal = null;

				if (useZipStream)
				{
					try
					{
						using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
						{
							retVal = XmlSerializer<T>.Deserialize(zipStream);
						}
					}
					catch (InvalidDataException)
					{
						stream.Position = 0;
						retVal = XmlSerializer<T>.Deserialize(stream);
					}
				}
				else
				{
					retVal = XmlSerializer<T>.Deserialize(stream);
				}

				var settings = retVal as ISettings;
				if (settings != null)
				{
					settings.ConfigurationFileName = fileName;
					settings.ConfigurationIsSecure = useZipStream;
				}

				return retVal;
			}
		}

		/// <summary>
		/// Stores the global instance of the Settings object.
		/// </summary>
		private static T _values = null;

		/// <summary>
		/// Stores an object used for locking the Settings instance while loading
		/// </summary>
		private static object _lockObject = new object();

		/// <summary>
		/// Gets the global instance of the Settings object.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
				Justification = "The static method is necessary in order to expose the singleton instance of T.")]
		public static T Values
		{
			get
			{
				if (_values == null)
				{
					lock (_lockObject)
					{
						// Check again after clearing the lock in case another thread has loaded the settings.
						if (_values == null) ReadSettings();
					}
				}
				return _values;
			}
		}

		/// <summary>
		/// Gets a value indicating if the settings have been loaded from configuration.
		/// </summary>
		public static bool IsLoaded
		{
			get { return (_values != null); }
		}

		/// <summary>
		/// Flag used while settings are loading to prevent the ReadSettings from being executed multiple times.
		/// </summary>
		private static bool _isLoading;

		/// <summary>
		/// Reads the settings from the configuration file.
		/// </summary>
		/// <remarks>This can be called in order to re-read configuration settings that may have changed.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
				Justification = "Multiple exceptions are possible and each is handled exactly the same way.")]
		protected static void ReadSettings()
		{
			if (!_isLoading)
			{
				_isLoading = true;

				try
				{
					_values = (T)ConfigurationManager.GetSection(ConfigSectionName);
					if (_values == null)
					{
						string msg = string.Format(CultureInfo.InvariantCulture, "Configuration section '{0}' does not exist or configuration is invalid.", ConfigSectionName);
						throw new ConfigurationErrorsException(msg);
					}
					else
					{
						_values.Validate();
					}
				}
				catch (Exception ex)
				{
					TS.Logger.WriteLine(TS.Categories.Warning, "Unable to load {0} settings from configuration.\r\n{1}", ConfigSectionName, ex.Message);
					_values = new T(); // Use default settings.  Setting this will prevent this exception from happening each time a setting is read.
					_values.Validate();
					_values.HandleException(ex);
				}
				finally
				{
					_isLoading = false;
				}
			}
		}

		/// <summary>
		/// Overriden in derived classes to handle errors in the config file.
		/// </summary>
		/// <param name="exception">The exception which was caught while reading configuration settings from the file.</param>
		/// <remarks>Default behavior is to simply re-throw the exception.</remarks>
		protected virtual void HandleException(Exception exception)
		{
			throw exception;
		}

		/// <summary>
		/// Unimplemented. Derived classes may override if there is a need to validate any settings after reading from the configuration store.
		/// </summary>
		public virtual void Validate()
		{
		}
	}
}
