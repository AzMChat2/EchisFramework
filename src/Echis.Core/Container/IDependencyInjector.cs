using System.Diagnostics.CodeAnalysis;

namespace System.Container
{
	/// <summary>
	/// Defines methods used to Inject Objects into properties of Container Managed objects.
	/// </summary>
	public interface IDependencyInjector
	{
		/// <summary>
		/// Interogates the specified objects properties for the InjectObjectAttribute and uses the IOC container to inject the specified objects.
		/// </summary>
		/// <param name="obj">The object whose properties are to be interogated for injection.</param>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "obj is the name used by the .Net framework (see PropertyInfo.SetValue).")]
		void InjectObjectDependencies(object obj);

		/// <summary>
		/// Interogates the specified objects properties for the InjectObjectAttribute and uses the IOC container to inject the specified objects.
		/// </summary>
		/// <param name="contextId">The Id of the application context in which to search for the injected object.</param>
		/// <param name="obj">The object whose properties are to be interogated for injection.</param>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "obj is the name used by the .Net framework (see PropertyInfo.SetValue).")]
		void InjectObjectDependencies(string contextId, object obj);
	}
}
