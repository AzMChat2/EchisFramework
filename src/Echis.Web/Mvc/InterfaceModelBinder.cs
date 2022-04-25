using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Web;

namespace System.Web.Mvc
{
	/// <summary>
	/// Provides concrete instances of Interfaces for System.Web.Mvc applications.
	/// </summary>
	/// <remarks>
	/// This class allows the use of interfaces as Models, providing a suitable factory method can be found in the defined factories.
	/// </remarks>
	public class InterfaceModelBinder : DefaultModelBinder
	{
		/// <summary>
		/// Stores factory information for a particular Type.
		/// </summary>
		private Dictionary<Type, InterfaceFactoryInfo> _factoryInfo = new Dictionary<Type, InterfaceFactoryInfo>();

		/// <summary>
		/// Constructor; loads Model Factory Definitions from the configuration.
		/// </summary>
		public InterfaceModelBinder()
		{
      Settings.Values.FactoryAssemblies.ForEach(AddAssembly);
    }

		/// <summary>
		/// Adds Factories from an assembly to the internal factory store.
		/// </summary>
		/// <param name="assemblyName">The assembly containing factories.</param>
    public void AddAssembly(string assemblyName)
		{
      if (assemblyName == null) throw new ArgumentNullException("assemblyName");

      AddAssembly(Assembly.Load(assemblyName));
		}

    /// <summary>
    /// Adds Factories from an assembly to the internal factory store.
    /// </summary>
    /// <param name="assembly">The assembly containing factories.</param>
    public void AddAssembly(Assembly assembly)
    {
      if (assembly == null) throw new ArgumentNullException("assembly");

      assembly.GetTypes().ForEach(AddIfFactory);
    }

    /// <summary>
    /// Determines if the specified method is a Factory.
    /// </summary>
    /// <param name="info">The method info.</param>
    protected virtual bool IsFactory(MethodInfo info)
    {
      if (info == null) throw new ArgumentNullException("info");

      return (info.FindAttribute<FactoryAttribute>() != null);
    }

    /// <summary>
    /// Adds factory methods from the specified class if it is a factory class.
    /// </summary>
    /// <param name="factoryType">The factory class type.</param>
    protected virtual void AddIfFactory(Type factoryType)
    {
      if (factoryType == null) throw new ArgumentNullException("factoryType");

      FactoryAttribute attribute = factoryType.FindAttribute<FactoryAttribute>();

      if (attribute != null)
      {
        object factory = string.IsNullOrEmpty(attribute.ObjectId) ?
          ReflectionExtensions.CreateObject(factoryType) :
          IOC.Instance.GetObjectAndInject<object>(attribute.ContextId, attribute.ObjectId);

        factoryType.GetMethods().ForEachIf(IsFactory, method => AddFactory(factory, method));
      }
    }

    /// <summary>
    /// Adds a factory method to the Factory Info collection.
    /// </summary>
    /// <param name="method">The Factory Method to be added to the Factory Info collection.</param>
    /// <param name="factory">The Factory object to which the Factory Method belongs.</param>
    public virtual void AddFactory(object factory, MethodInfo method)
    {
      if (method == null) throw new ArgumentNullException("method");

			if (!_factoryInfo.ContainsKey(method.ReturnType))
			{
				lock (_factoryInfo)
				{
					if (!_factoryInfo.ContainsKey(method.ReturnType)) _factoryInfo.Add(method.ReturnType, new InterfaceFactoryInfo());
				}
			}

      ParameterInfo[] parameters = method.GetParameters();

      if (parameters.IsNullOrEmpty())
      {
        _factoryInfo[method.ReturnType].NewModel = new FactoryInfo() { Factory = factory, Method = method };
      }
      else if (IsMatch(parameters, GetPrimaryKeys(method.ReturnType)))
      {
        _factoryInfo[method.ReturnType].ExistingModel = new FactoryInfo() { Factory = factory, Method = method };
      }
      // else: ignore Factory decoration, the method is unusable as a Factory Method because we don't know what to pass in as parameters.
    }

		/// <summary>
		/// Creates the specified model type by using the specified controller context and binding context.
		/// </summary>
		/// <param name="controllerContext">The context within which the controller operates.</param>
		/// <param name="bindingContext">The context to which the model is bound.</param>
		/// <param name="modelType">The type or interface of the Model object to return.</param>
		/// <returns>Returns an instance of the specified model type by using the specified controller context and binding context.</returns>
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			if (controllerContext == null) throw new ArgumentNullException("controllerContext");
			if (bindingContext == null) throw new ArgumentNullException("bindingContext");
			if (modelType == null) throw new ArgumentNullException("modelType");

			object retVal = null;

			if ((modelType.IsInterface) || (modelType.IsAbstract))
			{
				InterfaceFactoryInfo factoryInfo = GetFactoryInfo(modelType);
				if (factoryInfo != null)
				{
					if ((factoryInfo.ExistingModel != null) && (factoryInfo.ExistingModel.Factory != null) && (factoryInfo.ExistingModel.Method != null))
					{
						object[] pkValues = GetPrimaryKeyValues(controllerContext, bindingContext, factoryInfo.ExistingModel.Method);
						if (!pkValues.IsNullOrEmpty()) retVal = factoryInfo.ExistingModel.Method.Invoke(factoryInfo.ExistingModel.Factory, pkValues);
					}

					if ((retVal == null) && (factoryInfo.NewModel != null) && (factoryInfo.NewModel.Factory != null) && (factoryInfo.NewModel.Method != null))
					{
						retVal = factoryInfo.NewModel.Method.Invoke(factoryInfo.NewModel.Factory, null);
					}
				}
			}

			// Failsafe: if the return value is still null at this point (which it will be by design for non-interface/abstract types),
			//           fall through to the default (which will throw an exception if the modelType is not a creatable type.)
			if (retVal == null) retVal = base.CreateModel(controllerContext, bindingContext, modelType);

			return retVal;
		}

    /// <summary>
    /// Gets the Factory Info for the specified model type.
    /// </summary>
    private InterfaceFactoryInfo GetFactoryInfo(Type modelType)
    {
      return _factoryInfo.ContainsKey(modelType) ? _factoryInfo[modelType] : null;
    }

		/// <summary>
		/// Gets the values from the Request for the data elements specific to the Primary Key.
		/// </summary>
		/// <param name="controllerContext">The context within which the controller operates.</param>
		/// <param name="bindingContext">The context to which the model is bound.</param>
		/// <param name="method">The factory method which will be called to create an instance of the interface.</param>
		/// <returns>Returns an array of objects from the Request Context matching the primary key, if provided, otherwise returns null.</returns>
		protected virtual object[] GetPrimaryKeyValues(ControllerContext controllerContext, ModelBindingContext bindingContext, MethodInfo method)
		{
			if (controllerContext == null) throw new ArgumentNullException("controllerContext");
			if (bindingContext == null) throw new ArgumentNullException("bindingContext");
			if (method == null) throw new ArgumentNullException("method");

			object[] retVal = null;

			PropertyDescriptorCollection properties = GetModelProperties(controllerContext, bindingContext);
			ParameterInfo[] parameters = method.GetParameters();

			retVal = new object[parameters.Length];
			for (int idx = 0; idx < retVal.Length; idx++)
			{
				PropertyDescriptor pkProperty = properties.Find(parameters[idx].Name, true);

				if (pkProperty == null)
				{
					retVal = null;
					break;
				}

				ModelBindingContext propertyContext = new ModelBindingContext()
				{
					ModelMetadata = bindingContext.PropertyMetadata[pkProperty.Name],
					ModelName = pkProperty.Name,
					ModelState = bindingContext.ModelState,
					ValueProvider = bindingContext.ValueProvider
				};

				retVal[idx] = GetPropertyValue(controllerContext, propertyContext, pkProperty, Binders.GetBinder(pkProperty.PropertyType));
			}

			return retVal;
		}

		/// <summary>
		/// Determines if the specified method parameters match (count and name) the primary key properties.
		/// </summary>
		/// <param name="parameters">The method parameters to be compared to the Primary Key properties.</param>
		/// <param name="primaryKey">The Primary Key properties to be compared to the method parameters.</param>
		/// <returns>Returns true if all of the parameters match the primary key properties.</returns>
		protected virtual bool IsMatch(ParameterInfo[] parameters, List<PropertyInfo> primaryKey)
		{
			if (parameters == null) throw new ArgumentNullException("parameters");
			if (primaryKey == null) throw new ArgumentNullException("primaryKey");

			// To start with, the count of parameters and primary key properties must match...
			bool retVal = (parameters.Length == primaryKey.Count);

			for (int idx = 0; (retVal && (idx < parameters.Length)); idx++)
			{
				PropertyInfo property = primaryKey.Find(item => item.Name.Equals(parameters[idx].Name, StringComparison.OrdinalIgnoreCase));

				// Return true as long as a primary key property exists with the same name (ignoring case) as the parameter name...
				if (property == null)
				{
					retVal = false;
				}
				else
				{
					// ...AND the types are the same.
					retVal = (property.PropertyType == parameters[idx].ParameterType);
				}
			}

			return retVal;
		}

		/// <summary>
		/// Gets a list of properties which represent the Primary Key of the Model.
		/// </summary>
		/// <param name="modelType">The interface type of the model.</param>
		/// <returns>Returns a list of properties which represent the Primary Key of the Model.</returns>
		protected virtual List<PropertyInfo> GetPrimaryKeys(Type modelType)
		{
			if (modelType == null) throw new ArgumentNullException("modelType");

			List<PropertyInfo> retVal = new List<PropertyInfo>();
			retVal.AddRangeIf(modelType.GetProperties(), IsPrimaryKey);
			return retVal;
		}

		/// <summary>
		/// Determines if the specified property is part of the Primary Key.
		/// </summary>
		/// <param name="propertyInfo">The property to be inspected.</param>
		/// <returns>Return true if the specified property is part of the Primary Key.</returns>
		protected virtual bool IsPrimaryKey(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");

			return propertyInfo.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length != 0;
		}
	}

	/// <summary>
	/// Represents a Model Factory used to create instances of interface models.
	/// </summary>
	public class InterfaceFactoryInfo
	{
		/// <summary>
		/// Gets or sets the Factory Info used to create new instances of the interface model.
		/// </summary>
		public FactoryInfo NewModel { get; set; }
		/// <summary>
		/// Gets or sets the Factory Info used to retrieve existing instances of the interface model.
		/// </summary>
		public FactoryInfo ExistingModel { get; set; }
	}

	/// <summary>
	/// Represents a Model Factory used to create instances of interface models.
	/// </summary>
	public class FactoryInfo
	{
		/// <summary>
		/// Stores the Factory Method Information.
		/// </summary>
		public MethodInfo Method { get; set; }
		/// <summary>
		/// Stores the instance of the Factory Object upon which the Method exists.
		/// </summary>
		public object Factory { get; set; }
	}
}
