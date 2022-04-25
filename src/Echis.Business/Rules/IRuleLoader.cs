using System.Data.Objects;

namespace System.Business.Rules
{
	/// <summary>
	/// Loads Rules from a Data Store
	/// </summary>
	public interface IRuleLoader
	{
		/// <summary>
		/// Gets the rules for the specified property by context and domain.
		/// </summary>
		/// <typeparam name="T">The type of property.</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		/// <param name="propertyName">The name of the property.</param>
		RuleCollection<Property<T>> GetPropertyRules<T>(string contextId, string domainId, string propertyName);

		/// <summary>
		/// Gets the rules for the specified Business Object
		/// </summary>
		/// <typeparam name="T">The Business Object Type</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		RuleCollection<T> GetObjectRules<T>(string contextId, string domainId);

		/// <summary>
		/// Gets the rules for the specified Business Object List
		/// </summary>
		/// <typeparam name="T">The interface type of the Business Object Collection</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		RuleCollection<T> GetCollectionRules<T>(string contextId, string domainId);
	}
}
