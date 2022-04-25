using System.Xml.Serialization;

namespace System.Configuration.Managers.Database
{
	/// <summary>
	/// Represents settings for External Configuration Management
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the name of the Data Access Object used to connect to the Configuration database.
		/// </summary>
		[XmlAttribute]
		public string ConfigurationDataAccessName { get; set; }

		/// <summary>
		/// Gets or sets the Database Schema name for Configuration objects (default is 'dbo')
		/// </summary>
		[XmlAttribute]
		public string DatabaseSchemaName { get; set; }

		/// <summary>
		/// Validates the settings.
		/// </summary>
		public override void Validate()
		{
			if (string.IsNullOrEmpty(DatabaseSchemaName))
			{
				DatabaseSchemaName = "dbo";
			}
		}
	}
}
