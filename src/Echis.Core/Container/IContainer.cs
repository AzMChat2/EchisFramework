using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Container
{
	/// <summary>
	/// Provides a standard interface for Inversion of Control Containers
	/// </summary>
	public interface IContainer
	{
		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <typeparam name="T">The Type of the object to find.</typeparam>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		bool ContainsObject<T>();

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		bool ContainsObject(string objectId);

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <param name="contextId">The Id of the application context in which to search for the object.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		bool ContainsObject(string contextId, string objectId);

		/// <summary>
		/// Determines if the specified context exists.
		/// </summary>
		/// <param name="contextId">The Id of the application context to find.</param>
		/// <returns>Returns true if the context exists in the IOC Container.</returns>
		bool ContainsContext(string contextId);

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObject<T>();

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObject<T>(Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObject<T>(string objectId);

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
		T GetObject<T>(string objectId, Type defaultType);

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
		T GetObject<T>(string contextId, string objectId);

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
		T GetObject<T>(string contextId, string objectId, Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectUnsafe<T>();

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectUnsafe<T>(Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectUnsafe<T>(string objectId);

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectUnsafe<T>(string objectId, Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectUnsafe<T>(string contextId, string objectId);

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectUnsafe<T>(string contextId, string objectId, Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInject<T>();

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInject<T>(Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInject<T>(string objectId);

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
		T GetObjectAndInject<T>(string objectId, Type defaultType);

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
		T GetObjectAndInject<T>(string contextId, string objectId);

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
		T GetObjectAndInject<T>(string contextId, string objectId, Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInjectUnsafe<T>();

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInjectUnsafe<T>(Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInjectUnsafe<T>(string objectId);

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInjectUnsafe<T>(string objectId, Type defaultType);

		/// <summary>
		/// Retrieves an object from the IOC container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller, instead a null object will be returned.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInjectUnsafe<T>(string contextId, string objectId);

		/// <summary>
		/// Retrieves an object from the IOC container or returns an instance of the default type if the item does not exist in the IOC Container and then injects any object dependencies identified by the InjectObject Attribute.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned.</typeparam>
		/// <param name="contextId">The Id of the application context from which the object will be retrieved.</param>
		/// <param name="objectId">The Id of the object to be generated.</param>
		/// <param name="defaultType">The default type to return if the item does not exist in the IOC Container.</param>
		/// <returns>Returns the object from the IOC container depending upon the IOC's configuration or an instance of the default type if the item does not exist in the IOC Container.</returns>
		/// <remarks>This method will not allow any exceptions generated to bubble up to the caller.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		T GetObjectAndInjectUnsafe<T>(string contextId, string objectId, Type defaultType);
	}
}
