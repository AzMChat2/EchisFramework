using System;
using System.Xml.Serialization;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Web
{
	/// <summary>
	/// Contains configuration settings for System.Web components.
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets the IOC Container Context Id for MVC Controllers.
		/// </summary>
		[XmlAttribute]
		public string ControllerContext { get; set; }

		/// <summary>
		/// Gets the list of Model Factory Definitions for teh InterfaceModelBinder
		/// </summary>
		[XmlElement("FactoryAssembly")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public List<string> FactoryAssemblies { get; set; }

		/// <summary>
		/// Configuration section is optional. Ignore any errors.
		/// </summary>
		/// <param name="exception">The exception which was caught while attempting to read the configuration section.</param>
		protected override void HandleException(Exception exception)
		{
		}
	}
}
