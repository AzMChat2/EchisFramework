using System.Diagnostics.CodeAnalysis;

namespace System
{
	/// <summary>
	/// A Wrapper for creating disposable objects from non-disposable objects at runtime.
	/// </summary>
	public sealed class Disposable : Disposable<object>
	{
		/// <summary>
		/// Creates a new Disposable&lt;T&gt; wrapper object using the specified object and action.
		/// </summary>
		/// <typeparam name="TDispose">The type of the wrapped object to be disposed.</typeparam>
		/// <param name="wrappedObject">The wrapped object to be disposed.</param>
		/// <param name="disposeAction">The action to perform on the object during disposal.</param>
		/// <returns>Returns a new Disposable&lt;T&gt; wrapper object using the specified object and action.</returns>
		public static Disposable<TDispose> Create<TDispose>(TDispose wrappedObject, Action<TDispose> disposeAction)
		{
			return new Disposable<TDispose>(wrappedObject, disposeAction);
		}

		/// <summary>
		/// Private constructor; this class is not meant to be instantiatable.
		/// </summary>
		private Disposable() { }
	}

	/// <summary>
	/// A Wrapper for creating disposable objects from non-disposable objects at runtime.
	/// </summary>
	/// <typeparam name="T">The type of the wrapped object to be disposed.</typeparam>
	public class Disposable<T> : IDisposable
	{
		/// <summary>
		/// Gets the wrapped object which will be disposed.
		/// </summary>
		public T WrappedObject { get; private set; }

		/// <summary>
		/// Gets the action which will be performed when disposed.
		/// </summary>
		public Action<T> DisposeAction { get; private set; }

		/// <summary>
		/// Creates a new Disposable&lt;T&gt; wrapper object using the specified object and action.
		/// </summary>
		/// <param name="wrappedObject">The wrapped object to be disposed.</param>
		/// <param name="disposeAction">The action to perform on the object during disposal.</param>
		protected internal Disposable(T wrappedObject, Action<T> disposeAction)
		{
			if (wrappedObject == null) throw new ArgumentNullException("wrappedObject");
			if (disposeAction == null) throw new ArgumentNullException("disposeAction");

			WrappedObject = wrappedObject;
			DisposeAction = disposeAction;
		}

		/// <summary>
		/// Internal constructor for child class Disposable.
		/// </summary>
		internal Disposable() { }

		/// <summary>
		/// Destructor, calls Dispose.
		/// </summary>
		~Disposable()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes the wrapped object by calling the dispose action.
		/// </summary>
		/// <param name="disposing">Not used, but present to satisfy Code Analysis.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Unknown what exceptions may be thrown invoking Action<T> delegate; Intention is to allow code to continue even if errors occur while disposing object.")]
		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if ((DisposeAction != null) && (WrappedObject != null))
				{
					DisposeAction.Invoke(WrappedObject);
				}
			}
			catch { }
			finally
			{
				DisposeAction = null;
				WrappedObject = default(T);
			}
		}

		/// <summary>
		/// Disposes the wrapped object by calling the dispose action.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
