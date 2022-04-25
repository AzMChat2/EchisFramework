using System.Xml.Serialization;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Represents credentials used to retrieve configuration information.
	/// </summary>
	public class Credentials
	{
		/// <summary>
		/// Gets or sets the name of the application which is requesting a configuration section.
		/// </summary>
		[XmlAttribute]
		public virtual string Application { get; set; }
		/// <summary>
		///  Gets or sets the name of the machine upon which the Process is running.
		/// </summary>
		[XmlAttribute]
		public virtual string Machine { get; set; }
		/// <summary>
		/// Gets or sets the domain under which the user exists.
		/// </summary>
		[XmlAttribute]
		public virtual string UserDomain { get; set; }
		/// <summary>
		/// Gets or sets the name of the user running the current process.
		/// </summary>
		[XmlAttribute]
		public virtual string User { get; set; }
		/// <summary>
		/// Gets or sets the name of the web user (if applicable). 
		/// </summary>
		[XmlAttribute]
		public virtual string WebUser { get; set; }
		/// <summary>
		/// Gets or sets the type of environment.
		/// </summary>
		[XmlAttribute]
		public virtual EnvironmentTypes Environment { get; set; }
	}
}
