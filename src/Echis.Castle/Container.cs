using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

using Castle.Windsor;
using Castle.Windsor.Configuration;
using System.Container;

namespace System.Castle
{
	/// <summary>
	/// Provides access to Castle's Inversion of Control container.
	/// </summary>
	public class Container : ContainerBase
	{
		/// <summary>
		/// Stores the name (ContextId) of the default WindsorContainer
		/// </summary>
		protected const string DefaultContext = "::Default::";

		/// <summary>
		/// Stores the collection of WindsorContainers
		/// </summary>
		private Dictionary<string, IWindsorContainer> _containters;
		/// <summary>
		/// Gets the collection of WindsorContainers, accessible by Name.
		/// </summary>
		protected Dictionary<string, IWindsorContainer> Containers
		{
			get
			{
				if (_containters == null)
				{
					_containters = new Dictionary<string, IWindsorContainer>();
					_containters.Add(DefaultContext, new WindsorContainer());
					Settings.Values.Containers.ForEach(item => _containters.Add(item.Name, CreateContainer(item)));
				}
				return _containters;
			}
		}

		/// <summary>
		/// Creates a Windsor Container from the ContainerInfo object.
		/// </summary>
		/// <param name="info">The ContainerInfo object which contains the information which will be used to create a Windsor Container</param>
		/// <returns>Returns a new Windsor Container based on the information provided in the info object.</returns>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			MessageId = "IConfigurationInterpreter", Justification = "IConfigurationInterpreter is the name of an interface.")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			MessageId = "InnerException", Justification = "InnerException is the name of a property.")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			MessageId = "IWindsorContainer", Justification = "IWindsorContainer is the name of an interface.")]
		protected virtual IWindsorContainer CreateContainer(ContainerInfo info)
		{
			if (info == null) throw new ArgumentNullException("info");
			IConfigurationInterpreter interpreter = (IConfigurationInterpreter)ReflectionExtensions.CreateObjectUnsafe(info.InterpreterType);
			if (interpreter == null)
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture,
					"Unable to create Configuration Interpreter of type '{0}'.",
					info.InterpreterTypeName));

			Type[] ctorParameterTypes = new Type[] { typeof(string), typeof(IWindsorContainer), typeof(IConfigurationInterpreter) };
			ConstructorInfo constructor = info.ContainerType.GetConstructor(ctorParameterTypes);
			if (constructor == null)
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture,
					"Unable to create Windsor Container of type '{0}': missing constructor with signature of (String, IWindsorContainer, IConfigurationInterpreter).",
					info.ContainerTypeName));

			try
			{
				object[] parameters = new object[] { info.Name, Containers[DefaultContext], interpreter };
				return (IWindsorContainer)constructor.Invoke(parameters);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture,
					"Unable to create Windsor Container of type '{0}': Constructor threw an exception, see InnerException for details).",
					info.ContainerTypeName), ex);
			}
		}

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <typeparam name="T">The Type of the object to find.</typeparam>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public override bool ContainsObject<T>()
		{
			return (GetObjectUnsafe<T>() != null);
		}

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public override bool ContainsObject(string objectId)
		{
			return (GetObjectUnsafe<object>(objectId) != null);
		}

		/// <summary>
		/// Determines if the specified object exists.
		/// </summary>
		/// <param name="objectId">The Id of the object to find.</param>
		/// <param name="contextId">The Id of the application context in which to search for the object.</param>
		/// <returns>Returns true if the object is defined in the IOC Container.</returns>
		public override bool ContainsObject(string contextId, string objectId)
		{
			return (GetObjectUnsafe<object>(contextId, objectId) != null);
		}

		/// <summary>
		/// Determines if the specified context exists.
		/// </summary>
		/// <param name="contextId">The Id of the application context to find.</param>
		/// <returns>Returns true if the context exists in the IOC Container.</returns>
		public override bool ContainsContext(string contextId)
		{
			if (string.IsNullOrWhiteSpace(contextId)) contextId = DefaultContext;
			return Containers.ContainsKey(contextId);
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
			return Containers[DefaultContext].Resolve<T>();
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
			if (string.IsNullOrWhiteSpace(contextId)) contextId = DefaultContext;
			return Containers[contextId].Resolve<T>(objectId);
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
			return Containers[DefaultContext].Resolve<T>(objectId);
		}
	}
}
