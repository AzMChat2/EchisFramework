using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Container;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory;

namespace System.Spring
{
	/// <summary>
	/// Provides access to Spring's Inversion of Control container.
	/// </summary>
	public sealed class Container : ContainerBase
	{
		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <typeparam name="T">The Type of the object to find.</typeparam>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Overridden method from base class")]
		public override bool ContainsObject<T>()
		{
			Type t = typeof(T);
			IApplicationContext context = GetContext(null); // Default Context.

			bool retVal = context.ContainsObject(t.Name) || context.ContainsObject(t.FullName);

			if (!retVal)
			{
				// Search for it.
				retVal = !(context.GetObjectNamesForType(typeof(T)).IsNullOrEmpty());
			}

			return retVal;
		}

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public override bool ContainsObject(string objectId)
		{
			return GetContext(null).ContainsObject(objectId);
		}

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <param name="contextId">The Id of the application context in which to search for the object.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public override bool ContainsObject(string contextId, string objectId)
		{
			if (string.IsNullOrWhiteSpace(contextId)) contextId = null;
			return GetContext(contextId).ContainsObject(objectId);
		}

		/// <summary>
		/// Determines if the specified context exists.
		/// </summary>
		/// <param name="contextId">The Id of the application context to find.</param>
		/// <returns>Returns true if the context exists in the IOC Container.</returns>
		public override bool ContainsContext(string contextId)
		{
			if (string.IsNullOrWhiteSpace(contextId))
			{
				// Default Context
				return true;
			}
			else
			{
				return ContextRegistry.IsContextRegistered(contextId);
			}
		}

		private static IApplicationContext GetContext(string contextId)
		{
			if (string.IsNullOrWhiteSpace(contextId))
			{
				// Default Context
				return ContextRegistry.GetContext();
			}
			else
			{
				return ContextRegistry.GetContext(contextId);
			}
		}
	
		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public override T GetObject<T>()
		{
			Type t = typeof(T);
			IApplicationContext context = GetContext(null); // Default Context.

			if (context.ContainsObject(t.Name))
			{
				// Found it by Name
				return GetObject<T>(t.Name);
			}
			else if (context.ContainsObject(t.FullName))
			{
				// Found it by FullName
				return GetObject<T>(t.FullName);
			}
			else
			{
				// Search for it.
				string[] names = context.GetObjectNamesForType(typeof(T));
				if ((names == null) || (names.Length == 0))
				{
					string msg = string.Format(CultureInfo.InvariantCulture, "Cannot find any object definitions for type '{0}'.", typeof(T).FullName);
					throw new NoSuchObjectDefinitionException(typeof(T).FullName, msg);
				}
				else
				{
					// Arbitrarily choose first name in the list.
					return GetObject<T>(names[0]);
				}
			}
		}

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public override T GetObject<T>(string contextId, string objectId)
		{
			if (string.IsNullOrWhiteSpace(contextId)) contextId = null;
			return (T)GetContext(contextId)[objectId];
		}

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public override T GetObject<T>(string objectId)
		{
			return (T)GetContext(null)[objectId];
		}
	}
}
