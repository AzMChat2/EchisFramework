using System.Data;

namespace System.Data.Objects
{
	/// <summary>
	/// Defines methods used to populate properties from a Data Reader and serialize or deserialize properties during Xml Serialization.
	/// </summary>
	public interface IPropertyMapper
	{
		/// <summary>
		/// Populates a collection of properties from a Data Reader.
		/// </summary>
		/// <param name="properties">The property collection to be populated.</param>
		/// <param name="reader">The Data Reader containing the data for the properties.</param>
		void Process(PropertyCollection properties, IDataReader reader);

		/// <summary>
		/// Gets or sets the Mapper Context which can be used to determine the mapping configuration.
		/// </summary>
		string Context { get; set; }
	}
}
