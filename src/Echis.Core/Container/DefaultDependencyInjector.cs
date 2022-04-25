using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace System.Container
{
	/// <summary>
	/// Default Dependency Injector which injects objects into properties of container managed objects which have been decorated with the Inject Object Attribute.
	/// </summary>
	public class DefaultDependencyInjector : AttributeDependencyInjector<InjectObjectAttribute>
	{
		/// <summary>
		/// Gets the property value from the IOC for the specified Context, Attribute and Property.
		/// </summary>
		/// <param name="contextId">The context ID of the object to retrieve from the container.</param>
		/// <param name="attribute">The attribute associated with the property.</param>
		/// <param name="propertyInfo">The property.</param>
		/// <returns>Returns an object based on the context, attribute and property provided.</returns>
		[DebuggerHidden]
		protected override object GetPropertyValue(string contextId, InjectObjectAttribute attribute, PropertyInfo propertyInfo)
		{
			if (attribute == null) throw new ArgumentNullException("attribute");
			if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");

			object retVal = null;

			List<string> objectIds = new List<string>();
			objectIds.AddIf(attribute.ObjectId, IsValidId);
			if (string.IsNullOrEmpty(attribute.ObjectId))
			{
				objectIds.Add(propertyInfo.PropertyType.FullName);
				objectIds.Add(propertyInfo.PropertyType.Name);
			}

			List<string> contextIds = new List<string>();
			contextIds.AddIf(attribute.ContextId, IsValidId);
			contextIds.AddIf(contextId, IsValidId);
			contextIds.Add(string.Empty);

			for (int idxContext = 0; ((retVal == null) && (idxContext < contextIds.Count)); idxContext++)
			{
				for (int idxObject = 0; ((retVal == null) && (idxObject < objectIds.Count)); idxObject++)
				{
					retVal = IOC.Instance.GetObjectAndInject<object>(contextIds[idxContext], objectIds[idxObject]);
				}
			}

			if (retVal != null) InjectObjectDependencies(contextId, retVal);

			return retVal;
		}

		/// <summary>
		/// Tests to see if a given ObjectId or ContextId is valid (not empty or null)
		/// </summary>
		/// <param name="id">The ObjectId or ContextId to test.</param>
		/// <returns>Returns true if the given ObjectId or ContextId is valid (not empty or null).</returns>
		private static bool IsValidId(string id)
		{
			return !string.IsNullOrEmpty(id);
		}
	}
}
