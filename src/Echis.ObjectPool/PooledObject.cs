using System;
using System.Diagnostics.CodeAnalysis;

namespace System.ObjectPools
{
	/// <summary>
	/// Defines a poolable object.
	/// </summary>
	public interface IPoolableObject
	{
		/// <summary>
		/// Method called when an object is released back into the object pool.
		/// Implementing class can use this method to perform "clean-up" and return the Pooled object to a neutral/intial state.
		/// </summary>
		void Reset();
	}

	/// <summary>
	/// A wrapper class for the Pooled Object.
	/// </summary>
	/// <typeparam name="T">The type of the Pooled Object.</typeparam>
	/// <remarks>
	/// The wrapper class implements the IDisposable interface so that the "using" statement can be used to automatically return an object to the pool.
	/// </remarks>
	[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
		Justification = "Not appropriate here because we are simply using IDisposable in order to return the object to the pool.")]
	public class PooledObject<T> : IDisposable where T : IPoolableObject
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		internal PooledObject() { }

		/// <summary>
		/// Gets or sets a flag indicating if the Pooled Object is currently "checked-out" of the Object Pool.
		/// </summary>
		internal bool InUse { get; set; }

		/// <summary>
		/// Gets instance of the Pooled Object.
		/// </summary>
		public T Value { get; internal set; }

		/// <summary>
		/// Releases an object back into the Object Pool.
		/// </summary>
		public void Release()
		{
			Value.Reset();
			InUse = false;
		}

		/// <summary>
		/// The wrapper class implements the IDisposable interface so that the "using" statement can be used to automatically return an object to the pool.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly",
			Justification = "Not appropriate here because we are simply using IDisposable in order to return the object to the pool.")]
		[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
			Justification = "Not appropriate here because we are simply using IDisposable in order to return the object to the pool.")]
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "Not appropriate here because we are simply using IDisposable in order to return the object to the pool.")]
		void IDisposable.Dispose()
		{
			Release();
		}
	}
}
