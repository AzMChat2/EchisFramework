using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Business.Configuration
{
	/// <summary>
	/// Defines an instruction for a Rule.
	/// </summary>
	public abstract class RuleInfo
	{
		/// <summary>
		/// Gets the unique Id of the Rule.
		/// </summary>
		[XmlAttribute]
		public string RuleId { get; set; }
	}

	/// <summary>
	/// Defines an Add rule instruction.
	/// </summary>
	public sealed class AddRuleInfo : RuleInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public AddRuleInfo()
		{
			Parameters = new ParameterCollection();
		}

		/// <summary>
		/// Gets the type name of the rule to be added.
		/// </summary>
		[XmlAttribute("Type")]
		public string RuleType { get; set; }

		/// <summary>
		/// Defines Initialization parameters for the rule.
		/// </summary>
		[XmlElement("Parameter")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public ParameterCollection Parameters { get; set; }
	}

	/// <summary>
	/// Defines a Remove rule instruction.
	/// </summary>
	public sealed class RemoveRuleInfo : RuleInfo
	{
	}

	/// <summary>
	/// Represents a collection of Add or Remove rule instructions
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class RuleInfoCollection<T> : List<T> where T : RuleInfo
	{
	}

	/// <summary>
	/// Represents a set of Add and Remove rule instruction collections.
	/// </summary>
	public sealed class RuleInfoSet
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public RuleInfoSet()
		{
			AddRules = new RuleInfoCollection<AddRuleInfo>();
			RemoveRules = new RuleInfoCollection<RemoveRuleInfo>();
		}

		/// <summary>
		/// Gets or sets the collection of Rules to be added to the ruleset.
		/// </summary>
		[XmlElement("Add")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public RuleInfoCollection<AddRuleInfo> AddRules { get; set; }

		/// <summary>
		/// Gets or sets the collection of Rules to be removed to the Ruleset.
		/// </summary>
		[XmlElement("Remove")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public RuleInfoCollection<RemoveRuleInfo> RemoveRules { get; set; }

		/// <summary>
		/// Removes rules from the AddRules collection matching a RemoveRules entry.
		/// </summary>
		internal void ProcessRemoves()
		{
			RemoveRules.ForEach(remove => AddRules.RemoveAll(add => add.RuleId.Equals(remove.RuleId, StringComparison.OrdinalIgnoreCase)));
			RemoveRules.Clear();
		}
	}
}
