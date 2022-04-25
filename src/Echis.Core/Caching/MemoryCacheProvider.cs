using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Caching
{
	/// <summary>
	/// An implementation of the Cache Provider which stores cached objects in memory.
	/// </summary>
	public class MemoryCacheProvider : ICacheProvider
	{
		/// <summary>
		/// The Cache Store which containes the cached objects.
		/// </summary>
		private static CacheStore _cache = new CacheStore();

		#region Public methods
		/// <summary>
		/// Gets cache information, including the cached value for the specified method and arguments.
		/// </summary>
		/// <param name="method">A Method Info object representing the method whose result is being cached.</param>
		/// <param name="arguments">The arguments which are being passed into the method.</param>
		public CacheInfo GetCacheInfo(MethodInfo method, object[] arguments)
		{
			CacheResultsAttribute attribute = method.FindAttribute<CacheResultsAttribute>();

			if (attribute == null)
			{
				return null;
			}
			else
			{
				string name = GenerateName(GetCacheName(attribute.CacheName, method), arguments);
				ObjectCache<XmlWrapper> value = _cache.GetObjectCache<XmlWrapper>(name);

				return new CacheInfo()
				{
					Name = name,
					Expiration = attribute.Expiration,
					Value = value == null ? null : value.Item
				};
			}
		}

		/// <summary>
		/// Adds the specified item to the in Memory Cache
		/// </summary>
		/// <param name="info">Information used to determine the caching strategy and the object to be cached.</param>
		public void AddToCache(CacheInfo info)
		{
			if ((info != null) && (info.Value != null))
			{
				_cache.Add(new ObjectCache<XmlWrapper>() { Name = info.Name, Item = info.Value, Expiration = GetExpiration(info.Expiration) });
			}
		}

		/// <summary>
		/// Removes all items from the object cache whose name matches the specified predicate.
		/// </summary>
		/// <param name="match">The predecate used to match the name of the Object Cache.</param>
		public void RemoveAll(Predicate<string> match)
		{
			_cache.RemoveAll(match);
		}

		#endregion

		#region Static Helper Methods
		/// <summary>
		/// Generates a Cache Name from either the attribute Cached Name property or the Method's information.
		/// </summary>
		private static string GetCacheName(string attributeCacheName, MethodInfo method)
		{
			return string.IsNullOrWhiteSpace(attributeCacheName) ? method.GetMemberFullName() : attributeCacheName;
		}

		/// <summary>
		/// Generates an Object Cache name from the specified cache name base and the Method's arguments.
		/// </summary>
		private static string GenerateName(string cacheName, object[] arguments)
		{
			StringBuilder retVal = new StringBuilder();

			retVal.Append(cacheName);
			if (!arguments.IsNullOrEmpty()) arguments.ForEach(arg => retVal.AppendFormat("::{0}", arg));

			return retVal.ToString();
		}

		/// <summary>
		/// Determines the cached object expiration date based on the specified cache expiration strategy.
		/// </summary>
		private static DateTime GetExpiration(CacheExpiration expiration)
		{
			DateTime retVal = DateTime.MaxValue;

			switch (expiration)
			{
				case CacheExpiration.Hourly:
					retVal = DateTime.Now.AddHours(1);
					break;
				case CacheExpiration.Daily:
					retVal = DateTime.Today.AddDays(1);
					break;
				case CacheExpiration.Weekly:
					retVal = DateTime.Today.AddDays(7);
					break;
			}

			return retVal;
		}
		#endregion
	}
}
