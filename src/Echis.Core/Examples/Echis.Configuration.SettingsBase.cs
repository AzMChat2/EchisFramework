using System.Configuration;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MyApp.MyNamespace
{
	// See the System.Configuration.SettingsBase.config file for an example of a serialized instance of this class. 

	// - The SettingsBase class provides a static singleton instance of your settings class called Values.
	// - The SettingsBase class also handles the loading of your settings class using a Configuration Section Handler.
	// - In order to use either the System.Configuration.SectionHandler or the System.Configuration.ExternalSectionHandler
	//     your settings class needs to be XmlSerializable, this can be done using System.Xml.Serialization attributes
	//     as shown below, or for more advanced functionality, your settings class may implement IXmlSerializable.
	public class Settings : SettingsBase<Settings>
	{
		// Use XmlAttribute for simple type such as string, int, boolean, etc.
		[XmlAttribute]
		public string Name { get; set; }

		// Use XmlElement for more complext types
		[XmlElement]
		public Address Address { get; set; }

		// Use XmlElement for Lists
		[XmlElement("Alias")]
		public List<string> AliasNames { get; set; }

		// Use XmlElement for Lists
		[XmlElement("AlternateAddress")]
		public List<Address> AlternateAddressList { get; set; }
	}

	// - Your settings base class can use complex types as long as they are XmlSerializable.
	public class Address
	{
		[XmlAttribute]
		public string Street { get; set; }
		[XmlAttribute]
		public string City { get; set; }
		[XmlAttribute]
		public string State { get; set; }
		[XmlAttribute]
		public string Zip { get; set; }
	}
}
