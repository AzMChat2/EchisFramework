
namespace System.Collections.Generic
{
	/// <summary>
	/// Represents a Store of Object Caches.
	/// </summary>
	public class CacheStore
	{
		/// <summary>
		/// Gets the internal dictionary used to store the Object Caches.
		/// </summary>
		protected Dictionary<string, CacheBase> Store { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public CacheStore()
		{
			Store = new Dictionary<string, CacheBase>();
		}

		/// <summary>
		/// Gets the Object Cache by name.
		/// </summary>
		/// <param name="name">The name of the Object Cache to retrieve.</param>
		/// <returns>Returns the Object Cache by name, or null if the Cache does not exist or has expired.</returns>
		public CacheBase this[string name]
		{
			get
			{
				CacheBase retVal = null;

				if (Store.ContainsKey(name))
				{
					if (Store[name].IsExpired)
					{
						Store.Remove(name);
					}
					else
					{
						retVal = Store[name];
					}
				}

				return retVal;
			}
		}

		/// <summary>
		/// Gets the cache item as an ObjectCache
		/// </summary>
		/// <typeparam name="T">The Type of object stored in the ObjectCache.</typeparam>
		/// <param name="name">The name of the ObjectCache to retrieve.</param>
		/// <returns>Returns the Object Cache by name, or null if the Cache does not exist, has expired or is not an ObjectCache.</returns>
		public ObjectCache<T> GetObjectCache<T>(string name)
		{
			return this[name] as ObjectCache<T>;
		}

		/// <summary>
		/// Gets the cache item as a ListCache
		/// </summary>
		/// <typeparam name="T">The Type of objects stored in the ListCache.</typeparam>
		/// <param name="name">The name of the ListCache to retrieve.</param>
		/// <returns>Returns the ListCache by name, or null if the Cache does not exist, has expired or is not a ListCache.</returns>
		public ListCache<T> GetListCache<T>(string name)
		{
			return this[name] as ListCache<T>;
		}

		/// <summary>
		/// Gets a boolean value which indicates if the Store contains an Object Cache by the specified name.
		/// </summary>
		/// <param name="name">The name of the Object Cache to retrieve.</param>
		/// <returns>Returns true if an Object Cache with the specified name exists, otherwise returns false.</returns>
		public bool Contains(string name)
		{
			return Store.ContainsKey(name);
		}

		/// <summary>
		/// Removes the Object Cache from the Store.
		/// </summary>
		/// <param name="name">The name of the Object Cache to remove.</param>
		public void Remove(string name)
		{
			if (Store.ContainsKey(name)) Store.Remove(name);
		}

		/// <summary>
		/// Removes all of the Object Caches from the Store where the key matches the specified Predicate.
		/// </summary>
		/// <param name="match">The predecate used to match the name of the Object Cache.</param>
		public void RemoveAll(Predicate<string> match)
		{
			Store.Keys.ForEachIf(match, Remove);
		}

		/// <summary>
		/// Adds an Object Cache to the Store.
		/// </summary>
		/// <param name="cache">The Object Cache to be added.</param>
		/// <remarks>If a Cache with the same name already exists in the Store, it will be replaced.</remarks>
		/// <exception cref="System.ArgumentNullException">A System.ArgumentNullException is thrown if the cache parameter is null.</exception>
		/// <exception cref="System.ArgumentException">A System.ArgumentException is thrown if the cache's Name property is null or an empty string.</exception>
		public void Add(CacheBase cache)
		{
			if (cache == null) throw new ArgumentNullException("cache");
			if (string.IsNullOrEmpty(cache.Name)) throw new ArgumentException("Cache name may not be null or empty.");

			lock (Store)
			{
				if (Store.ContainsKey(cache.Name)) Store.Remove(cache.Name);
				Store.Add(cache.Name, cache);
			}
		}
	}
}
