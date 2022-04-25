using System.Xml.Serialization;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Represents settings for External Configuration Management
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the Full Type name or ObjectId of the Default Configuration Manager
		/// </summary>
		[XmlAttribute]
		public string ConfigurationManager { get; set; }

		/// <summary>
		/// Gets or sets the Full Type name or ObjectId of the CredentialsValidator used to validate configuration credentials.
		/// </summary>
		[XmlAttribute]
		public string CredentialsValidator { get; set; }

		/// <summary>
		/// Gets or sets the Full Type name or ObjectId of the Default Credentials Provider
		/// </summary>
		[XmlAttribute]
		public string CredentialsProvider { get; set; }

		/// <summary>
		/// Gets or sets the Full Type name or ObjectId of the Cryptography Provider.
		/// </summary>
		[XmlAttribute]
		public string CryptographyProvider { get; set; }

		/// <summary>
		/// Gets or sets the Environment Type
		/// </summary>
		[XmlAttribute]
		public EnvironmentTypes Environment { get; set; }

	}
}
