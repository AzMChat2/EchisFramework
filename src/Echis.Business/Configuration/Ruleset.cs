using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

namespace System.Business.Configuration
{
	/// <summary>
	/// The Ruleset class reprents a collection of rules for multiple domains (business objects).
	/// </summary>
	[Serializable]
	public sealed class Ruleset
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Ruleset()
		{
			Domains = new DomainInfoCollection();
		}

		/// <summary>
		/// Gets or sets the Id of the Ruleset
		/// </summary>
		[XmlAttribute]
		public string ContextId { get; set; }

		/// <summary>
		/// Gets or sets the Ruleset from which the current ruleset is derived.
		/// </summary>
		/// <remarks>This is for loading purposes, once inheritance has been processed, this value will be null.</remarks>
		[XmlAttribute]
		public string Inherits { get; set; }

		/// <summary>
		/// Gets the list of Domain informtion objects supported by this Ruleset.
		/// </summary>
		[XmlElement("Domain")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public DomainInfoCollection Domains { get; set; }

	}

	/// <summary>
	/// Stores a collection of Ruleset objects.
	/// </summary>
	[Serializable]
	[XmlType("Rulesets")]
	public sealed class RulesetCollection : List<Ruleset>
	{
		/// <summary>
		/// Gets the Ruleset object by ruleset Id.
		/// </summary>
		/// <param name="rulesetId">The ruleset Id of the Ruleset object to find.</param>
		/// <returns></returns>
		public Ruleset this[string rulesetId]
		{
			get { return Find(item => item.ContextId == rulesetId); }
		}
	}
}
