using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace System.Business.Configuration
{

	/// <summary>
	/// Represents information about a Business Object's Property rules.
	/// </summary>
	public sealed class PropertyInfo
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public PropertyInfo()
		{
			PropertyRules = new RuleInfoSet();
		}

		/// <summary>
		/// Gets or sets the name of the property which this object represents.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets the Rule Info Set for the Property
		/// </summary>
		[XmlElement("PropertyRules")]
		public RuleInfoSet PropertyRules { get; set; }
	}

	/// <summary>
	/// Represents a collection of Property Rule Info objects.
	/// </summary>
	public class PropertyInfoCollection : List<PropertyInfo>
	{
		/// <summary>
		/// Gets the PropertyRuleInfo object by property name.
		/// </summary>
		/// <param name="name">The property name of the PropertyRuleInfo object to find.</param>
		/// <returns></returns>
		public PropertyInfo this[string name]
		{
			get { return Find(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }
		}
	}
}
