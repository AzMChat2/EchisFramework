using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace System.Business
{
	/// <summary>
	/// Stores the Business Rule settings
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// The Container Object Id or the Type for the RuleManager
		/// </summary>
		[XmlAttribute]
		public string RuleManager { get; set; }

		/// <summary>
		/// The Container Object Id or the Type for the RuleLoader
		/// </summary>
		[XmlAttribute]
		public string RuleLoader { get; set; }

		/// <summary>
		/// The Container Object Id or the Type for the PropertyMapper
		/// </summary>
		[XmlAttribute]
		public string PropertyMapper { get; set; }

		/// <summary>
		/// The default context Id to use during validation.
		/// </summary>
		[XmlAttribute]
		public string DefaultContextId { get; set; }

		/// <summary>
		/// Gets the primary RuleManifest.
		/// </summary>
		[XmlElement]
		public RuleManifest PrimaryManifest { get; set; }

	}
}
