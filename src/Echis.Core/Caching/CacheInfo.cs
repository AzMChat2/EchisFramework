using System.Xml;
using System.Xml.Serialization;

namespace System.Caching
{
	/// <summary>
	/// Provides caching information used to determine cache expiration strategy and the current value of a Cached item.
	/// </summary>
	public class CacheInfo
	{
		/// <summary>
		/// Gets or sets the name of the Cache Item
		/// </summary>
		[XmlAttribute]
		public string Name { get; internal set; }
		/// <summary>
		/// Gets or sets the Cache Expiration strategy
		/// </summary>
		[XmlAttribute]
		public CacheExpiration Expiration { get; internal set; }
		/// <summary>
		/// Gets or sets the Cached item value.
		/// </summary>
		[XmlElement]
		public XmlWrapper Value { get; set; }
	}

	/// <summary>
	/// Enumeration for the Cache Expiration strategy
	/// </summary>
	public enum CacheExpiration
	{
		/// <summary>
		/// Cached item never expires
		/// </summary>
		None,
		/// <summary>
		/// Cached item expires after one hour
		/// </summary>
		Hourly,
		/// <summary>
		/// Cached item expires at midnight
		/// </summary>
		Daily,
		/// <summary>
		/// Cached item expires at midnight after one week.
		/// </summary>
		Weekly
	}
}
