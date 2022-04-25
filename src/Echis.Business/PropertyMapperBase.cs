using System;
using System.Data;

namespace System.Data.Objects
{
	/// <summary>
	/// Provides a base implementation of the IPropertyMapper interface.
	/// </summary>
	/// <remarks>
	/// Xml Serialization simply uses the name of the property as the attribute or element name.
	/// Non-XmlSerializable elements are persisted as attributes.
	/// XmlSerializable elements are persisted as elements.
	/// </remarks>
	public abstract class PropertyMapperBase : IPropertyMapper
	{
		/// <summary>
		/// Gets or sets the Mapper Context which can be used by derived classes to determine the mapping configuration.
		/// </summary>
		public string Context { get; set; }

		/// <summary>
		/// Populates a collection of properties from a Data Reader.
		/// </summary>
		/// <param name="properties">The property collection to be populated.</param>
		/// <param name="reader">The Data Reader containing the data for the properties.</param>
		public void Process(PropertyCollection properties, IDataReader reader)
		{
			if (properties == null) throw new ArgumentNullException("properties");
			if (reader == null) throw new ArgumentNullException("reader");

			ProcessDataReader(properties, reader);
			properties.UpdateAll();
		}

		/// <summary>
		/// Populates a collection of properties from a Data Reader.
		/// </summary>
		/// <param name="properties">The property collection to be populated.</param>
		/// <param name="reader">The Data Reader containing the data for the properties.</param>
		protected abstract void ProcessDataReader(PropertyCollection properties, IDataReader reader);

	}
}
