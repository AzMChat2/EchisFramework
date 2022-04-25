using System;
using System.Reflection;

namespace System.Caching
{
	/// <summary>
	/// Defines methods used to provide Object Caching
	/// </summary>
	public interface ICacheProvider
	{
		/// <summary>
		/// Gets cache information, including the cached value for the specified method and arguments.
		/// </summary>
		/// <param name="method">A Method Info object representing the method whose result is being cached.</param>
		/// <param name="arguments">The arguments which are being passed into the method.</param>
		CacheInfo GetCacheInfo(MethodInfo method, object[] arguments);

		/// <summary>
		/// Adds the specified item to the in Memory Cache
		/// </summary>
		/// <param name="info">Information used to determine the caching strategy and the object to be cached.</param>
		void AddToCache(CacheInfo info);

		/// <summary>
		/// Removes all items from the object cache whose name matches the specified predicate.
		/// </summary>
		/// <param name="match">The predecate used to match the name of the Object Cache.</param>
		void RemoveAll(Predicate<string> match);
	}
}
