using System;
using System.Data;

namespace System.Data.Objects
{
	/// <summary>
	/// Default implementation of the PropertyMapper interface
	/// </summary>
	/// <remarks>
	/// Attempts to match Property names to Column names in the Data Reader.
	/// </remarks>
	public class DefaultPropertyMapper : PropertyMapperBase
	{
		/// <summary>
		/// Populates a collection of properties from a Data Reader.
		/// </summary>
		/// <param name="properties">The property collection to be populated.</param>
		/// <param name="reader">The Data Reader containing the data for the properties.</param>
		protected override void ProcessDataReader(PropertyCollection properties, IDataReader reader)
		{
			if (properties == null) throw new ArgumentNullException("properties");
			if (reader == null) throw new ArgumentNullException("reader");

			for (int idx = 0; idx < reader.FieldCount; idx++)
			{
				PropertyBase property = properties.Find(item => item.Name.Equals(reader.GetName(idx), StringComparison.CurrentCultureIgnoreCase));
				if (property != null)
				{
					if (reader.IsDBNull(idx))
					{
						property.PropertyValue = null;
					}
					else
					{
						property.PropertyValue = reader.GetValue(idx);
					}
				}
			}
		}
	}
}
