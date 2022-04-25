using System;

namespace System.Caching
{
	/// <summary>
	/// Attribute used to define Caching strategy for a given service method.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class CacheResultsAttribute : Attribute
	{
		/// <summary>
		/// Creates a new instance of the Cache Results Attribute
		/// </summary>
		/// <param name="expiration">The expiration strategy of the cached object.</param>
		public CacheResultsAttribute(CacheExpiration expiration) : this(null, expiration) { }
		/// <summary>
		/// Creates a new instance of the Cache Results Attribute
		/// </summary>
		/// <param name="cacheName">The base name of the cached object</param>
		/// <param name="expiration">The expiration strategy of the cached object.</param>
		public CacheResultsAttribute(string cacheName, CacheExpiration expiration)
		{
			CacheName = cacheName;
			Expiration = expiration;
		}

		/// <summary>
		/// Gets the base name of the cached object.
		/// </summary>
		public string CacheName { get; private set; }
		/// <summary>
		/// Gets the expiration strategy of the cached object.
		/// </summary>
		public CacheExpiration Expiration { get; private set; }
	}
}
