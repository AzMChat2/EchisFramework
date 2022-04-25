using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Container
{
	/// <summary>
	/// Dependency Injector which injects objects into properties of container managed objects which have been decorated with the specified Attribute.
	/// </summary>
	/// <typeparam name="TAttribute">The attribute type to search for on container managed object properties.</typeparam>
	public abstract class AttributeDependencyInjector<TAttribute> : DependencyInjectorBase where TAttribute : Attribute
	{

		/// <summary>
		/// Gets the property value from the IOC for the specified Context, Attribute and Property.
		/// </summary>
		/// <param name="contextId">The context ID of the object to retrieve from the container.</param>
		/// <param name="attribute">The attribute associated with the property.</param>
		/// <param name="propertyInfo">The property.</param>
		/// <returns>Derived classes should return an object based on the context, attribute and property provided.</returns>
		protected abstract object GetPropertyValue(string contextId, TAttribute attribute, PropertyInfo propertyInfo);

		/// <summary>
		/// Interogates the specified objects properties for the InjectObjectAttribute and uses the IOC container to inject the specified objects.
		/// </summary>
		/// <param name="contextId">The Id of the application context in which to search for the injected object.</param>
		/// <param name="obj">The object whose properties are to be interogated for injection.</param>
		[DebuggerHidden]
		public override void InjectObjectDependencies(string contextId, object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			properties.ForEach(item => InjectObjectDependencies(contextId, item, obj));
		}

		/// <summary>
		/// Checks the specified Property for the InjectObjectAttribute and injects the value if found.
		/// </summary>
		/// <param name="contextId">The Id of the application context in which to search for the injected object.</param>
		/// <param name="propertyInfo">The Property of the Object to check.</param>
		/// <param name="obj">The object whose properties are to be interogated for injection.</param>
		[DebuggerHidden]
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "obj is the name used by the .Net framework (see PropertyInfo.SetValue).")]
		protected virtual void InjectObjectDependencies(string contextId, PropertyInfo propertyInfo, object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");

			if (IsPropertyNull(propertyInfo, obj))
			{
				TAttribute attribute = propertyInfo.FindAttribute<TAttribute>();

				if (attribute != null)
				{
					// Property has Injection Attribute, Get the value for the property from the Container
					object value = GetPropertyValue(contextId, attribute, propertyInfo);
					if (value != null) propertyInfo.SetValue(obj, value, null);
				}
			}
		}

		/// <summary>
		/// Checks to see if the properties value is null.  Also ignores any exception thrown by the property getter.
		/// </summary>
		/// <param name="propertyInfo">The property to test.</param>
		/// <param name="obj">The object containing the property.</param>
		/// <returns>
		/// Returns true if the property's value is null.
		/// Returns false if the property's value is already set or if the property getter throws an exception.
		/// </returns>
		[DebuggerHidden]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here.  Any exception thrown needs to be handled the same way: ignore.")]
		private static bool IsPropertyNull(PropertyInfo propertyInfo, object obj)
		{
			bool retVal = false;

			try
			{
				retVal = (propertyInfo.GetValue(obj, null) == null);
			}
			catch { }

			return retVal;
		}
	}
}
