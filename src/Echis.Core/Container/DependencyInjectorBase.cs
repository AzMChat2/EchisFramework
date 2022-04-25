
namespace System.Container
{
	/// <summary>
	/// A base class for Dependency Injector classes.
	/// </summary>
	public abstract class DependencyInjectorBase : IDependencyInjector
	{
		/// <summary>
		/// Interogates the specified objects properties for the InjectObjectAttribute and uses the IOC container to inject the specified objects.
		/// </summary>
		/// <param name="obj">The object whose properties are to be interogated for injection.</param>
		public void InjectObjectDependencies(object obj)
		{
			InjectObjectDependencies(null, obj);
		}

		/// <summary>
		/// Interogates the specified objects properties for the InjectObjectAttribute and uses the IOC container to inject the specified objects.
		/// </summary>
		/// <param name="contextId">The Id of the application context in which to search for the injected object.</param>
		/// <param name="obj">The object whose properties are to be interogated for injection.</param>
		public abstract void InjectObjectDependencies(string contextId, object obj);
	}
}
