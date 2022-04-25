using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

namespace System.Business.Configuration
{
	/// <summary>
	/// Represents information about Domain rules.
	/// </summary>
	[Serializable]
	public sealed class DomainInfo
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public DomainInfo()
		{
			CollectionRules = new RuleInfoSet();
			ObjectRules = new RuleInfoSet();
			Properties = new PropertyInfoCollection();
		}

		/// <summary>
		/// Gets or sets the collection of Business Object Collection rules.
		/// </summary>
		[XmlElement("CollectionRules")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public RuleInfoSet CollectionRules { get; set; }

		/// <summary>
		/// Gets or sets the collection of Business Object rules.
		/// </summary>
		[XmlElement("ObjectRules")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public RuleInfoSet ObjectRules { get; set; }

		/// <summary>
		/// Gets or sets the collection of Properties
		/// </summary>
		[XmlElement("Property")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public PropertyInfoCollection Properties { get; set; }

		/// <summary>
		/// Gets or sets the Id of the Domain (typically the name of the Business Object)
		/// </summary>
		[XmlAttribute]
		public string DomainId { get; set; }

		/// <summary>
		/// Gets or sets the Domain from which this Domain is derived.
		/// </summary>
		[XmlAttribute]
		public string Inherits { get; set; }
	}

	/// <summary>
	/// Represents a collection of Domain Info objects.
	/// </summary>
	[Serializable]
	public sealed class DomainInfoCollection : List<DomainInfo>
	{
		/// <summary>
		/// Gets the DomainInfo object by Domain Id
		/// </summary>
		/// <param name="domainId">The domain Id of the DomainInfo object to find.</param>
		/// <returns></returns>
		public DomainInfo this[string domainId]
		{
			get { return Find(item => item.DomainId == domainId); }
		}
	}
}

