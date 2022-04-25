using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Container;
using ContainerSettings = System.Container.Settings;
using DiagnosticsSettings = System.Diagnostics.Settings;

namespace System
{
	/// <summary>
	/// Provides access to the configured Inversion Of Control container.
	/// </summary>
	public static class IOC
	{
		/// <summary>
		/// Constants used by the IOC Class.
		/// </summary>
		private static class Constants
		{
			public const string DependencyInjectorObjectId = "System.Container.DependencyInjector";
		}

		/// <summary>
		/// Stores the instance of the IOC container.
		/// </summary>
		private static IContainer _instance;

		/// <summary>
		/// Gets the instantiated Inversion Of Control container.
		/// </summary>
		public static IContainer Instance
		{
			get
			{
				if (_instance == null)
				{
					// Don't bubble up any configuration exceptions
					if (!ContainerSettings.IsLoaded) ContainerSettings.Load();

					string typeName = ContainerSettings.Values.ContainerType;

					if (!string.IsNullOrEmpty(typeName))
					{
						_instance = ReflectionExtensions.CreateObjectUnsafe<IContainer>(typeName);
					}

					if (_instance == null)
					{
						_instance = new DefaultContainer();
						if (!string.IsNullOrEmpty(typeName))
						{
							string message = string.Format(CultureInfo.InvariantCulture, "Unable to create IOC container from type '{0}'.", typeName);
							TS.Logger.WriteLine(TS.Categories.Warning, message);
						}
					}
				}

				return _instance;
			}
		}

		/// <summary>
		/// Stores the singleton instance of the Dependency Injector
		/// </summary>
		private static IDependencyInjector _injector;
		/// <summary>
		/// Gets the singleton instance of the Dependency Injector
		/// </summary>
		public static IDependencyInjector Injector
		{
			get
			{
				if (_injector == null)
				{
					_injector = GetFrameworkObject<IDependencyInjector>(ContainerSettings.Values.Injector, Constants.DependencyInjectorObjectId, false);
					if (_injector == null)
					{
						// Use Default injector
						_injector = new DefaultDependencyInjector();
					}
				}
				return _injector;
			}
		}

		/// <summary>
		/// Used by the System Framework to get instances of Framework Components.
		/// </summary>
		/// <typeparam name="T">The type of object to retrieve.</typeparam>
		/// <param name="settingName">The name of the object from configuration settings.</param>
		/// <param name="defaultName">The default name of the object to use if the setting value is null,
		/// or the object specified by the configurationsetting cannot be found.</param>
		/// <param name="defaultType">The type of object to be created if no object is configured.</param>
		/// <returns>Returns a Framework object or an instance of the default type if the object is not configured.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T GetFrameworkObject<T>(string settingName, string defaultName, Type defaultType) where T : class
		{
			return GetFrameworkObject<T>(settingName, defaultName, false) ?? ReflectionExtensions.CreateObject<T>(defaultType);
		}

		/// <summary>
		/// Used by the System Framework to get instances of Framework Components.
		/// </summary>
		/// <typeparam name="T">The type of object to retrieve.</typeparam>
		/// <param name="settingName">The name of the object from configuration settings.</param>
		/// <param name="defaultName">The default name of the object to use if the setting value is null,
		/// or the object specified by the configurationsetting cannot be found.</param>
		/// <param name="bubbleExceptions">Flag indicating if any exceptions should bubble to the caller.</param>
		/// <returns>Returns a Framework object or null if the object is not configured.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Multiple exception types possible and, all are handled the same way.")]
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T GetFrameworkObject<T>(string settingName, string defaultName, bool bubbleExceptions)
		{
			T retVal = default(T);

			MultipleErrorException exception = null;

			try
			{
				if (!string.IsNullOrEmpty(settingName))
				{
					// Try getting from IOC based on setting name.
					retVal = GetFrameworkObject<T>(settingName);
				}

				if ((retVal == null) && (!string.IsNullOrEmpty(defaultName)))
				{
					// Try getting from IOC based on default name.
					retVal = GetFrameworkObject<T>(defaultName);
				}
			}
			catch(Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "Unable to create instance of framework object '{0}' ('{1}').", settingName, defaultName);
				exception = new MultipleErrorException(msg, ex);
			}

			try
			{
				if ((retVal == null) && (!string.IsNullOrEmpty(settingName)))
				{
					// Not in IOC, check if Setting is actually the name of a Type
					Type objectType = Type.GetType(settingName);
					if (objectType != null)
					{
						retVal = ReflectionExtensions.CreateObject<T>(objectType);
					}
				}
			}
			catch (Exception ex)
			{
				if (exception == null)
				{
					string msg = string.Format(CultureInfo.InvariantCulture, "Unable to create instance of framework object '{0}' ('{1}').", settingName, defaultName);
					exception = new MultipleErrorException(msg, ex);
				}
				else
				{
					exception.Exceptions.Add(ex);
				}
			}

			if (bubbleExceptions && (retVal == null) && (exception != null)) throw exception;

			return retVal;
		}

		/// <summary>
		/// Attempts to get the Framework Object from the IOC using first the System Framework Context then the Default Context
		/// </summary>
		/// <typeparam name="T">The type of object to retrieve.</typeparam>
		/// <param name="objectId">The objectId of the object to retrieve from the IOC.</param>
		/// <returns>Returns a Framework object or null if the object is not configured.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		private static T GetFrameworkObject<T>(string objectId)
		{
			T retVal = default(T);

			if (Instance.ContainsContext(DiagnosticsSettings.Values.SystemFrameworkContext) &&
				Instance.ContainsObject(DiagnosticsSettings.Values.SystemFrameworkContext, objectId))
			{
				retVal = Instance.GetObjectUnsafe<T>(DiagnosticsSettings.Values.SystemFrameworkContext, objectId);
			}

			if ((retVal == null) && Instance.ContainsObject(objectId))
			{
				retVal = Instance.GetObjectUnsafe<T>(objectId);
			}

			return retVal;
		}

	}
}
