using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

namespace System.Collections.Generic
{
	/// <summary>
	/// Object Cache base class from which all Object Caches will be derived.
	/// </summary>
	public abstract class CacheBase
	{
		/// <summary>
		/// Gets or sets the Name of the Object Cache
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Date and Time the Object Cache will expire.
		/// </summary>
		[XmlAttribute]
		public DateTime Expiration { get; set; }

		/// <summary>
		/// Gets a flag which indicates if the Object Cache has expired.
		/// </summary>
		[XmlIgnore]
		public bool IsExpired
		{
			get { return DateTime.Now >= Expiration; }
		}
	}

	/// <summary>
	/// Represents a Generic Object Cache which stores a singe object of the Type T
	/// </summary>
	/// <typeparam name="T">The Type of object to be stored in the Cache.</typeparam>
	public class ObjectCache<T> : CacheBase
	{
		/// <summary>
		/// Gets item which is stored in the Cache.
		/// </summary>
		[XmlElement]
		public T Item { get; set; }
	}

	/// <summary>
	/// Represents a Generic Object Cache which stores lists of objects of the Type T
	/// </summary>
	/// <typeparam name="T">The Type of objects to be stored in the Cache.</typeparam>
	public class ListCache<T> : CacheBase
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public ListCache()
		{
			Items = new List<T>();
		}

		/// <summary>
		/// Gets the list of items which are stored in the Cache.
		/// </summary>
		[XmlElement("Item")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer")]
		public List<T> Items { get; set; }
	}
}
