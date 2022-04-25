using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.Objects
{
	/// <summary>
	/// Represents a list of Properties
	/// </summary>
	public sealed class PropertyCollection : IEnumerable<PropertyBase>, IXmlSerializable
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public PropertyCollection()
		{
			InternalList = new List<PropertyBase>();
		}

		/// <summary>
		/// Stores the internal list of Properties.
		/// </summary>
		internal List<PropertyBase> InternalList { get; private set; }

		#region Business Object Collection members
		/// <summary>
		/// Gets the Names of the Properties contained in the Collection.
		/// </summary>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "A property is not appropriate here.")]
		public List<string> GetNames()
		{
			return new List<string>(from p in InternalList select p.Name);
		}

		/// <summary>
		/// Gets a flag which indicates if any property in the list has been changed (IsDirty).
		/// </summary>
		public bool IsDirty
		{
			get { return InternalList.Exists(item => item.IsDirty); }
		}

		/// <summary>
		/// Accesses the property by name.
		/// </summary>
		/// <param name="propertyName">The name of the property to access.</param>
		/// <returns>Returns the property specified, if no property of that name exists, returns null.</returns>
		public PropertyBase this[string propertyName]
		{
			get { return InternalList.Find(item => item.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)); }
		}

		/// <summary>
		/// Updates all of original values of the properties in the list to their new value.
		/// </summary>
		public void UpdateAll()
		{
			InternalList.ForEach(item => item.Update());
		}

		/// <summary>
		/// Resets all of the properties in the list to their original value.
		/// </summary>
		public void ResetAll()
		{
			InternalList.ForEach(item => item.Reset());
		}

		/// <summary>
		/// Validates all of the properties in the collection
		/// </summary>
		/// <param name="contextId">The Validation Context against which the Property will be validated</param>
		/// <returns>Returns true if all of the Properties in the Collection pass validation.</returns>
		internal bool ValidateCollectionElements(string contextId)
		{
			bool retVal = true;
			ForEach(property => retVal = retVal & property.IsValid(contextId));
			return retVal;
		}

		#endregion

		#region Collection members
		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate and returns the first occurence within the entire list.
		/// </summary>
		/// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements to search for.</param>
		/// <returns>Returns the first occurence within the entire list.</returns>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public PropertyBase Find(Predicate<PropertyBase> match)
		{
			return InternalList.Find(match);
		}

		/// <summary>
		/// Returns all of the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements to search for.</param>
		/// <returns>Returns all of the elements that match the conditions defined by the specified predicate.</returns>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public List<PropertyBase> FindAll(Predicate<PropertyBase> match)
		{
			return InternalList.FindAll(match);
		}

		/// <summary>
		/// Determines whether a Property is in the collection.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>Returns true if a property with the specified name exists in the collection.</returns>
		public bool Contains(string propertyName)
		{
			return InternalList.Exists(item => item.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)); ;
		}

		/// <summary>
		/// Counts the number of elements in the list that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements to search for.</param>
		/// <returns>Returns the number of elements that match the conditions defined by the specified predicate.</returns>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public int CountIf(Func<PropertyBase, bool> match)
		{
			return InternalList.Count(match);
		}

		/// <summary>
		/// Determines whether the list contains elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements to search for.</param>
		/// <returns>Returns true of any elements match the conditions defined by the specified predicate.</returns>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public bool Exists(Predicate<PropertyBase> match)
		{
			return InternalList.Exists(match);
		}

		/// <summary>
		/// Performs the specified action on each element of the list.
		/// </summary>
		/// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the list.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public void ForEach(Action<PropertyBase> action)
		{
			InternalList.ForEach(action);
		}

		/// <summary>
		/// Performs the specified action on each element in the list.
		/// </summary>
		/// <param name="validator">The validator to be used to test the item.</param>
		/// <param name="action">The delegate to perform on each element of the list.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public void ForEachIf(Predicate<PropertyBase> validator, Action<PropertyBase> action)
		{
			InternalList.ForEachIf(validator, action);
		}

		/// <summary>
		/// Determines whether every element in the List matches the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions to check against the elements.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public bool TrueForAll(Predicate<PropertyBase> match)
		{
			return InternalList.TrueForAll(match);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the properties in the collection.
		/// </summary>
		IEnumerator<PropertyBase> IEnumerable<PropertyBase>.GetEnumerator()
		{
			return InternalList.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the properties in the collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return InternalList.GetEnumerator();
		}
		#endregion

		#region Xml Serialization members

		/// <summary>
		/// Stores the property types of the properties found in the collection
		/// </summary>
		private List<Type> _propertyTypes;
		/// <summary>
		/// Gets the property types of the properties found in the collection
		/// </summary>
		public List<Type> PropertyTypes
		{
			get
			{
				if (_propertyTypes == null) _propertyTypes = new List<Type>((from property in InternalList select property.GetType()).Distinct());
				return _propertyTypes;
			}
		}

		/// <summary>
		/// Not Used.
		/// </summary>
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Deserializes the Property Collection from Xml
		/// </summary>
		/// <param name="reader">The XmlReader used to read the Xml Source.</param>
		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			string elementName = reader.Name;

			if (reader.IsEmptyElement)
			{
				reader.Read();
			}
			else
			{
				List<Type> propertyTypes = new List<Type>();
				while (reader.Read())
				{
					if (reader.Name == "PropertyType")
					{
						propertyTypes.AddIf(Type.GetType(reader.GetAttribute("Name")), type => type != null);
					}
					else
					{
						break;
					}
				}

				_propertyTypes = propertyTypes;
				if (reader.Name != elementName)
				{
					XmlSerializer<List<PropertyBase>>.SetSerializerAttributes(PropertyTypes.ToArray());
					InternalList = XmlSerializer<List<PropertyBase>>.Deserialize(reader);
				}

				reader.ReadEndElement();
			}
		}

		/// <summary>
		/// Writes Xml Serialization data to an Xml Store
		/// </summary>
		/// <param name="writer">The writer used to write to the Xml Store</param>
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			PropertyTypes.ForEach(type => WritePropertyTypeXml(writer, type));

			XmlSerializer<List<PropertyBase>>.SetSerializerAttributes(PropertyTypes.ToArray());
			XmlSerializer<List<PropertyBase>>.Serialize(writer, InternalList.FindAll(item => item.IsXmlSerializable));
		}

		/// <summary>
		/// Writes Xml Serialization data for a Property type to an Xml Store
		/// </summary>
		/// <param name="writer">The writer used to write to the Xml Store</param>
		/// <param name="type">The type of the property to be serialized.</param>
		private static void WritePropertyTypeXml(XmlWriter writer, Type type)
		{
			writer.WriteStartElement("PropertyType");
			writer.WriteAttribute("Name", type.GetMemberFullName());
			writer.WriteEndElement();
		}

		#endregion

	}
}
