using System.Xml.Serialization;

namespace System.Configuration.Managers.FileSystem
{
	/// <summary>
	/// Represents settings for External Configuration Management
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the path and filename used to load configuration sections.
		/// </summary>
		/// <remarks>
		/// {0} is the placeholder for the Environment Name.
		/// {1} is the placeholder for the Application Name.
		/// </remarks>
		[XmlAttribute]
		public string ConfigurationFilePath { get; set; }

		/// <summary>
		/// Gets or sets the location of the User Groups configuration file.
		/// </summary>
		[XmlAttribute]
		public string UserGroupsFilePath { get; set; }

		/// <summary>
		/// Gets or sets the location of the Machine Groups configuration file.
		/// </summary>
		[XmlAttribute]
		public string MachineGroupsFilePath { get; set; }


		/// <summary>
		/// Gets or sets the XPath used to locate the User Groups section of the Application Configuration file.
		/// </summary>
		[XmlAttribute]
		public string UserGroupsXPath { get; set; }

		/// <summary>
		/// Gets or sets the XPath used to locate the Machine Groups section of the Application Configuration file.
		/// </summary>
		[XmlAttribute]
		public string MachineGroupsXPath { get; set; }

	}
}
