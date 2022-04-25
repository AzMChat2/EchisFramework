using System;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace System.Container
{
	/// <summary>
	/// Stores the settings used to determine the Inversion Of Control container to be used.
	/// </summary>
	public sealed class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the Type name of the IOC class to be used.
		/// </summary>
		[XmlAttribute]
		public string ContainerType { get; set; }

		/// <summary>
		/// Gets or sets the Type name of the IOC class to be used.
		/// </summary>
		[XmlAttribute]
		public string Injector { get; set; }

		/// <summary>
		/// Gets or sets the ContextId for System Framework components.
		/// </summary>
		[XmlAttribute]
		public string SystemFrameworkContext { get; set; }

		/// <summary>
		/// Ignore any Exceptions.
		/// </summary>
		/// <param name="exception">The exception thrown while attempting to read the settings from the configuration file.</param>
		protected override void HandleException(Exception exception)
		{
			// Ignore errors
		}
	}
}
