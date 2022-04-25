using System.Xml.Serialization;

namespace System.Configuration.Managers.Remote
{
	/// <summary>
	/// Stores system settings for the Remote Configuration Manager
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the Type used to retrieve the actual Configuration Sections.
		/// </summary>
		[XmlAttribute]
		public string ConfigurationManager { get; set; }
	}
}
