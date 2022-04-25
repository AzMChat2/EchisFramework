using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Collections.Generic;

namespace System.Container
{
	/// <summary>
	/// Provides a base implementation of the IInversionOfControl interface.
	/// </summary>
	public abstract class ContainerBase : IContainer
	{
		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <typeparam name="T">The Type of the object to find.</typeparam>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public abstract bool ContainsObject<T>();

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public abstract bool ContainsObject(string objectId);

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <param name="contextId">The Id of the application context in which to search for the object.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public abstract bool ContainsObject(string contextId, string objectId);

		/// <summary>
		/// Determines if the specified context exists.
		/// </summary>
		/// <param name="contextId">The Id of the application context to find.</param>
		/// <returns>Returns true if the context exists in the IOC Container.</returns>
		public abstract bool ContainsContext(string contextId);

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public abstract T GetObject<T>();

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInject<T>()
		{
			T retVal = GetObject<T>();
			IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
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
		public abstract T GetObject<T>(string contextId, string objectId);

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInject<T>(string contextId, string objectId)
		{
			T retVal = GetObject<T>(contextId, objectId);
			IOC.Injector.InjectObjectDependencies(contextId, retVal);
			return retVal;
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
		public abstract T GetObject<T>(string objectId);

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInject<T>(string objectId)
		{
			T retVal = GetObject<T>(objectId);
			IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here:  Any exception thrown needs to be handled the same way: return null (default(T)).")]
		public virtual T GetObjectUnsafe<T>()
		{
			try
			{
				return GetObject<T>();
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here:  Any exception thrown needs to be handled the same way: return null (default(T)).")]
		public virtual T GetObjectAndInjectUnsafe<T>()
		{
			try
			{
				return GetObjectAndInject<T>();
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here:  Any exception thrown needs to be handled the same way: return null (default(T)).")]
		public virtual T GetObjectUnsafe<T>(string contextId, string objectId)
		{
			try
			{
				return GetObject<T>(contextId, objectId);
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here:  Any exception thrown needs to be handled the same way: return null (default(T)).")]
		public virtual T GetObjectAndInjectUnsafe<T>(string contextId, string objectId)
		{
			try
			{
				return GetObjectAndInject<T>(contextId, objectId);
			}
			catch
			{
				return default(T);
			}
		}
	
		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here:  Any exception thrown needs to be handled the same way: return null (default(T)).")]
		public virtual T GetObjectUnsafe<T>(string objectId)
		{
			try
			{
				return GetObject<T>(objectId);
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here:  Any exception thrown needs to be handled the same way: return null (default(T)).")]
		public virtual T GetObjectAndInjectUnsafe<T>(string objectId)
		{
			try
			{
				return GetObjectAndInject<T>(objectId);
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObject<T>(Type defaultType)
		{
			T retVal = GetObject<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObject<T>(defaultType);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObject<T>(string objectId, Type defaultType)
		{
			T retVal = GetObject<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObject<T>(defaultType);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObject<T>(string contextId, string objectId, Type defaultType)
		{
			T retVal = GetObject<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObject<T>(defaultType);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectUnsafe<T>(Type defaultType)
		{
			T retVal = GetObjectUnsafe<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectUnsafe<T>(string objectId, Type defaultType)
		{
			T retVal = GetObjectUnsafe<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectUnsafe<T>(string contextId, string objectId, Type defaultType)
		{
			T retVal = GetObjectUnsafe<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInject<T>(Type defaultType)
		{
			T retVal = GetObject<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObject<T>(defaultType);
			IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInject<T>(string objectId, Type defaultType)
		{
			T retVal = GetObject<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObject<T>(defaultType);
			IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInject<T>(string contextId, string objectId, Type defaultType)
		{
			T retVal = GetObject<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObject<T>(defaultType);
			IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInjectUnsafe<T>(Type defaultType)
		{
			T retVal = GetObjectUnsafe<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
			if (retVal == null) IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInjectUnsafe<T>(string objectId, Type defaultType)
		{
			T retVal = GetObjectUnsafe<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
			if (retVal == null) IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
		}

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public virtual T GetObjectAndInjectUnsafe<T>(string contextId, string objectId, Type defaultType)
		{
			T retVal = GetObjectUnsafe<T>();
			if (retVal == null) retVal = ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
			if (retVal == null) IOC.Injector.InjectObjectDependencies(retVal);
			return retVal;
		}
	}
}
