using System;
using System.Business.Configuration;
using System.Business.Rules;
using System.Data.Objects;

namespace System.Business
{
	/// <summary>
	/// Provides Singleton Instances of Business Object Services
	/// </summary>
	internal static class Services
	{
		/// <summary>
		/// Stores Container default ObjectIds for Business Object Services.
		/// </summary>
		private static class ObjectIds
		{
			/// <summary>
			/// Object Id for the RuleLoader
			/// </summary>
			public const string RuleLoader = "System.Business.RuleLoader";
			/// <summary>
			/// Object Id for the RuleManager
			/// </summary>
			public const string RuleManager = "System.Business.RuleManager";
			/// <summary>
			/// Object Id for the PropertyMapper
			/// </summary>
			public const string PropertyMapper = "System.Business.PropertyMapper";
		}

		/// <summary>
		/// Static constructor
		/// </summary>
		static Services()
		{
			RuleLoader = IOC.GetFrameworkObject<IRuleLoader>(Settings.Values.RuleLoader, ObjectIds.RuleLoader, typeof(XmlRuleLoader));
			RuleManager = IOC.GetFrameworkObject<IRuleManager>(Settings.Values.RuleManager, ObjectIds.RuleManager, typeof(RuleManager));
			PropertyMapper = IOC.GetFrameworkObject<IPropertyMapper>(Settings.Values.PropertyMapper, ObjectIds.PropertyMapper, typeof(DefaultPropertyMapper));
		}

		/// <summary>
		/// Gets the Rule Loader instance.
		/// </summary>
		public static IRuleLoader RuleLoader { get; private set; }

		/// <summary>
		/// Gets the Rule Manager instance.
		/// </summary>
		public static IRuleManager RuleManager { get; private set; }

		/// <summary>
		/// Gets the Property Mapper instance.
		/// </summary>
		public static IPropertyMapper PropertyMapper { get; private set; }
	}
}
