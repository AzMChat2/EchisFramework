using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace System.ObjectPools
{
	/// <summary>
	/// Represnets an Object Pool.
	/// </summary>
	/// <typeparam name="T">The type of the objects contained in the Object Pool.</typeparam>
	public class ObjectPool<T> where T : IPoolableObject
	{
		/// <summary>
		/// Called when a new Pooled Object is Created.
		/// </summary>
		/// <param name="objectCreated">The newly created object that was added to the object pool.</param>
		/// <remarks>
		/// May be overridden by derived classes to perform initialization or preparation of the newly created object.
		/// </remarks>
		protected virtual void OnPooledObjectCreated(PooledObject<T> objectCreated)
		{
		}

		/// <summary>
		/// Stores the list of Pooled Object Wrappers (each containing a pooled object.)
		/// </summary>
		private List<PooledObject<T>> _objects = new List<PooledObject<T>>(Settings.Values.DefaultPoolSize);

		/// <summary>
		/// Binding flags used to retrieve the Constructor of the Pooled Object Type.
		/// </summary>
		private static readonly BindingFlags _flags = (BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

		/// <summary>
		/// Default constructor.  Uses the default constructor to create instances of the Pooled Object Type.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
			Justification = "Reviewed. Call to virtual property MinimumPoolSize is acceptable." )]
		public ObjectPool() : this(typeof(T).GetConstructor(_flags, null, Type.EmptyTypes, null)) { }
		/// <summary>
		/// Constructor.  Uses the supplied Constructor Information to create instances of the Pooled Object Type
		/// </summary>
		/// <param name="constructor"></param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
			Justification = "Reviewed. Call to virtual property MinimumPoolSize is acceptable.")]
		public ObjectPool(ConstructorInfo constructor) 
		{
			Constructor = constructor;
			MinimumPoolSize = Settings.Values.DefaultPoolSize;
		}

		/// <summary>
		/// Initializes the Object Pool.
		/// </summary>
		protected virtual void Initialize()
		{
			if (Constructor == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
					"Missing constructor information for Object Pool of type '{0}'", typeof(T).FullName));
			}

			for (int idx = 0; idx < MinimumPoolSize; idx++)
			{
				_objects.Add(GetNewPooledObject());
			}
		}

		/// <summary>
		/// Gets or sets the constructor used to create Pooled Object instances.
		/// </summary>
		public ConstructorInfo Constructor { get; set; }

		/// <summary>
		/// Gets the current number of objects in the Object Pool (both used and unused).
		/// </summary>
		public int Count { get { return _objects.Count; } }

		/// <summary>
		/// Gets the current number of in-use objects in the Object Pool.
		/// </summary>
		public int CountInUse
		{
			get { return _objects.Count(item => item.InUse); }
		}

		/// <summary>
		/// Gets the current number of free objects in the Object Pool.
		/// </summary>
		public int CountFree
		{
			get { return _objects.Count(item => !item.InUse); }
		}


		/// <summary>
		/// Gets or sets the minimum size of the object pool.
		/// </summary>
		/// <remarks>This value is used during initialization to set the initial size of the object pool.
		/// The default value is the value of the System.ObjectPools.Settings.DefaultPoolSize value.</remarks>
		public virtual int MinimumPoolSize { get; set; }

		/// <summary>
		/// Gets or sets the maximum size of the object pool.
		/// </summary>
		/// <remarks>Set to zero (0) for no limit. The default is zero.</remarks>
		public virtual int MaximumPoolSize { get; set; }

		/// <summary>
		/// Gets the type of the Object in the object pool.
		/// </summary>
		public Type WrappedType { get { return Constructor.DeclaringType; } }

		/// <summary>
		/// Gets an object from the pool.  Will wait for an object to become free indefinitely.
		/// </summary>
		/// <returns>Returns an object from the pool.  Use a "using" statement or call the Release method to return the object to the pool.</returns>
		public PooledObject<T> Get()
		{
			return Get(0);
		}

		/// <summary>
		/// Gets an object from the pool.  Will wait for an object to become free until the specified timeout.
		/// Returns null if the timeout expires before an object becomes available.
		/// </summary>
		/// <param name="timeout">The timeout in milleseconds to wait for an object to become available.  Set to zero to wait indefinitely.</param>
		/// <returns>Returns an object from the pool.  Use a "using" statement or call the Release method to return the object to the pool.</returns>
		public PooledObject<T> Get(int timeout)
		{
			DateTime start = DateTime.Now;
			PooledObject<T> retVal = TryGet();

			while ((retVal == null) && ((timeout == 0) || (DateTime.Now.Subtract(start).TotalMilliseconds < timeout)))
			{
				Thread.Sleep(10);
				retVal = TryGet();
			}

			return retVal;
		}

		/// <summary>
		/// Attempts to retrieve an object from the pool, if none are available returns null.
		/// </summary>
		/// <returns>Returns an object from the pool or null if none are available.  Use a "using" statement or call the Release method to return the object to the pool.</returns>
		public virtual PooledObject<T> TryGet()
		{
			lock (_objects)
			{
				if (_objects.Count == 0) Initialize();

				PooledObject<T> retVal = _objects.Find(item => !item.InUse);

				if ((retVal == null) && ((MaximumPoolSize == 0) || (_objects.Count < MaximumPoolSize)))
				{
					retVal = GetNewPooledObject();
					_objects.Add(retVal);
				}

				if (retVal != null) retVal.InUse = true;
				return retVal;
			}
		}

		/// <summary>
		/// Creates a new Pooled Object
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		private PooledObject<T> GetNewPooledObject()
		{
			PooledObject<T> retVal = new PooledObject<T>();
			retVal.Value = (T)Constructor.Invoke(null);
			IOC.Injector.InjectObjectDependencies(retVal.Value);
			OnPooledObjectCreated(retVal);
			return retVal;
		}
	}
}
