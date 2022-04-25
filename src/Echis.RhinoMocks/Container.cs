using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Container;

namespace System.RhinoMocks
{
	/// <summary>
	/// Provides a Mock Object repository and acts as an Inversion of Control Container serving the Mock Objects when requested.
	/// </summary>
	public class Container : ContainerBase
	{
		private static string DefaultContext = "::Default::";
		private Dictionary<string, Dictionary<string, object>> _registry = new Dictionary<string, Dictionary<string, object>>();

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <typeparam name="T">The Type of the object to find.</typeparam>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Overridden method from base class")]
		public override bool ContainsObject<T>()
		{
			return _registry[DefaultContext].ContainsKey(typeof(T).FullName);
		}

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public override bool ContainsObject(string objectId)
		{
			return _registry[DefaultContext].ContainsKey(objectId); ;
		}

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <param name="contextId">The Id of the application context in which to search for the object.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public override bool ContainsObject(string contextId, string objectId)
		{
			return _registry[contextId].ContainsKey(objectId);
		}

		/// <summary>
		/// Determines if the specified context exists.
		/// </summary>
		/// <param name="contextId">The Id of the application context to find.</param>
		/// <returns>Returns true if the context exists in the IOC Container.</returns>
		public override bool ContainsContext(string contextId)
		{
			if (string.IsNullOrEmpty(contextId)) contextId = DefaultContext;
			return _registry.ContainsKey(contextId);
		}

		/// <summary>
		/// Gets the Singleton instance of the MockIOC.
		/// </summary>
		public static Container Instance
		{
			get { return IOC.Instance as Container; }
		}

		/// <summary>
		/// Registers a Mock Object within the IOC Container.
		/// </summary>
		/// <param name="interfaceType">The interface type of the Mock Object</param>
		/// <param name="mockObject">The Mock Object to be returned when the specified contextId and objectId are requested.</param>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "mockObject is a generic term, yet specific within the context.")]
		public void Register(Type interfaceType, object mockObject)
		{
			if (interfaceType == null) throw new ArgumentNullException("interfaceType");
			Register(DefaultContext, interfaceType.FullName, mockObject);
		}

		/// <summary>
		/// Registers a Mock Object within the IOC Container.
		/// </summary>
		/// <param name="objectId">The objectId used to retrieve the Mock Object from the IOC Container.</param>
		/// <param name="mockObject">The Mock Object to be returned when the specified contextId and objectId are requested.</param>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification="mockObject is a generic term, yet specific within the context.")]
		public void Register(string objectId, object mockObject)
		{
			Register(DefaultContext, objectId, mockObject);
		}

		/// <summary>
		/// Registers a Mock Object within the IOC Container.
		/// </summary>
		/// <param name="contextId">The contextId used to retrieve the Mock Object from the IOC Container.</param>
		/// <param name="interfaceType">The interface type of the Mock Object</param>
		/// <param name="mockObject">The Mock Object to be returned when the specified contextId and objectId are requested.</param>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "mockObject is a generic term, yet specific within the context.")]
		public void Register(string contextId, Type interfaceType, object mockObject)
		{
			if (interfaceType == null) throw new ArgumentNullException("interfaceType");
			Register(contextId, interfaceType.FullName, mockObject);
		}

		/// <summary>
		/// Registers a Mock Object within the IOC Container.
		/// </summary>
		/// <param name="contextId">The contextId used to retrieve the Mock Object from the IOC Container.</param>
		/// <param name="objectId">The objectId used to retrieve the Mock Object from the IOC Container.</param>
		/// <param name="mockObject">The Mock Object to be returned when the specified contextId and objectId are requested.</param>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames",
			Justification = "mockObject is a generic term, yet specific within the context.")]
		public void Register(string contextId, string objectId, object mockObject)
		{
			Dictionary<string, object> context;
			if (_registry.ContainsKey(contextId))
			{
				context = _registry[contextId];
			}
			else
			{
				context = new Dictionary<string, object>();
				_registry.Add(contextId, context);
			}

			context.Add(objectId, mockObject);
		}

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Overridden method from base class")]
		public override T GetObject<T>()
		{
			return GetObject<T>(DefaultContext, typeof(T).FullName);
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
			Justification = "Overridden method from base class")]
		public override T GetObject<T>(string contextId, string objectId)
		{
			if (_registry.ContainsKey(contextId))
			{
				Dictionary<string, object> context = _registry[contextId];
				if (context.ContainsKey(objectId))
				{
					return (T)context[objectId];
				}
				else
				{
					throw new MockException(string.Format(CultureInfo.InvariantCulture, "No Mock Object registered as '{0} in context '{1}'.", objectId, contextId));
				}
			}
			else
			{
				throw new MockException(string.Format(CultureInfo.InvariantCulture, "Context '{0}' does not exist in the Mock Registry.", contextId));
			}
		}

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Overridden method from base class")]
		public override T GetObject<T>(string objectId)
		{
			return GetObject<T>(DefaultContext, objectId);
		}
	}
}
