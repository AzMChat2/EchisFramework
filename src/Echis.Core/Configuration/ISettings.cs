
namespace System.Configuration
{
	/// <summary>
	/// Defines basic properties of a Settings class.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Gets or sets the configuration file name to which the settings file will be persisted when calling the Save() method.
		/// </summary>
		string ConfigurationFileName { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the configuration file is created using a GZipStream.
		/// </summary>
		bool ConfigurationIsSecure { get; set; }
	}
}
