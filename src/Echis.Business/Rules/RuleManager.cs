using System.Collections.Generic;
using System.Data.Objects;

namespace System.Business.Rules
{
	/// <summary>
	/// The RuleManager class loads and stored information used to dynamically create rules for properties, business objects and business object lists.
	/// </summary>
	public sealed class RuleManager : RuleManagerBase
	{
		#region Property Rules
		/// <summary>
		/// Gets the rules for the specified property by context and domain.
		/// </summary>
		/// <typeparam name="T">The type of property.</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		/// <param name="propertyName">The name of the property.</param>
		protected override RuleCollection<Property<T>> GetPropertyRules<T>(string contextId, string domainId, string propertyName)
		{
			RuleCollection<Property<T>> retVal;

			DomainRuleStore ruleStore = _ruleStore.GetRuleStore(contextId, domainId);
			if (!ruleStore.PropertyRules.ContainsKey(propertyName)) ruleStore.PropertyRules.Add(propertyName, new Rules());
			Rules rules = ruleStore.PropertyRules[propertyName];

			if (rules.IsLoaded)
			{
				retVal = rules.Collection as RuleCollection<Property<T>>;
			}
			else
			{
				retVal = Services.RuleLoader.GetPropertyRules<T>(contextId, domainId, propertyName);
				rules.Collection = retVal;
			}
			
			return retVal;
		}
		#endregion

		#region Object Rules
		/// <summary>
		/// Gets the rules for the specified Business Object
		/// </summary>
		/// <typeparam name="T">The Business Object Type</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		protected override RuleCollection<T> GetBusinessObjectRules<T>(string contextId, string domainId)
		{
			RuleCollection<T> retVal;

			DomainRuleStore ruleStore = _ruleStore.GetRuleStore(contextId, domainId);

			if (ruleStore.ObjectRules.IsLoaded)
			{
				retVal = ruleStore.ObjectRules.Collection as RuleCollection<T>;
			}
			else
			{
				retVal = Services.RuleLoader.GetObjectRules<T>(contextId, domainId);
				ruleStore.ObjectRules.Collection = retVal;
			}

			return retVal;
		}
		#endregion

		#region Collection Rules
		/// <summary>
		/// Gets the rules for the specified Business Object List
		/// </summary>
		/// <typeparam name="T">The Business Object Collection Type</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		protected override RuleCollection<T> GetBusinessObjectCollectionRules<T>(string contextId, string domainId)
		{
			RuleCollection<T> retVal;

			DomainRuleStore ruleStore = _ruleStore.GetRuleStore(contextId, domainId);

			if (ruleStore.ObjectRules.IsLoaded)
			{
				retVal = ruleStore.CollectionRules.Collection as RuleCollection<T>;
			}
			else
			{
				retVal = Services.RuleLoader.GetCollectionRules<T>(contextId, domainId);
				ruleStore.CollectionRules.Collection = retVal;
			}

			return retVal;
		}
		#endregion

		#region Cached Rule Storage
		private static RuleStore _ruleStore = new RuleStore();

		private sealed class RuleStore : Dictionary<string, ContextRuleStore>
		{
			public DomainRuleStore GetRuleStore(string contextId, string domainId)
			{
				if (!ContainsKey(contextId)) Add(contextId, new ContextRuleStore());
				return this[contextId].GetRuleStore(domainId);
			}
		}

		private sealed class ContextRuleStore : Dictionary<string, DomainRuleStore>
		{
			public DomainRuleStore GetRuleStore(string domainId)
			{
				if (!ContainsKey(domainId)) Add(domainId, new DomainRuleStore());
				return this[domainId];
			}
		}

		private sealed class DomainRuleStore
		{
			public DomainRuleStore()
			{
				PropertyRules = new Dictionary<string, Rules>();
				CollectionRules = new Rules();
				ObjectRules = new Rules();
			}

			public Rules CollectionRules { get; set; }
			public Rules ObjectRules { get; set; }
			public Dictionary<string, Rules> PropertyRules;
		}

		private sealed class Rules
		{
			public object Collection { get; set; }
			public bool IsLoaded { get { return Collection != null; } }
		}
		#endregion
	}
}
