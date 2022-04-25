using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Data.Objects;

namespace System.Business.Rules
{
	/// <summary>
	/// Base class from which RuleManagers may be derived.
	/// </summary>
	public abstract class RuleManagerBase : IRuleManager
	{
		/// <summary>
		/// Validates a Business Object Property
		/// </summary>
		/// <param name="objectProperty">The property to be validated</param>
		/// <param name="contextId">The Validation Context against which the Property will be validated</param>
		/// <param name="domainId">The Domain to which the object belongs.</param>
		/// <param name="messages">A StringBuilder object into which Rule Validation Messages will be written.</param>
		/// <returns>Returns true if the Property has passed all Rule Validations</returns>
		public bool ValidateProperty<T>(Property<T> objectProperty, string contextId, string domainId, StringBuilder messages)
		{
			if (objectProperty == null) throw new ArgumentNullException("objectProperty");

			return GetPropertyRules<T>(contextId, domainId, objectProperty.Name).Validate(objectProperty, messages);
		}

		/// <summary>
		/// Validates a Business Object.
		/// </summary>
		/// <param name="businessObject">The Business Object to be validated</param>
		/// <param name="contextId">The Validation Context against which the Property will be validated</param>
		/// <param name="domainId">The Domain to which the object belongs.</param>
		/// <param name="messages">A StringBuilder object into which Rule Validation Messages will be written.</param>
		/// <returns>Returns true if the Business Object has passed all Rule Validations</returns>
		public bool ValidateObject<T>(T businessObject, string contextId, string domainId, StringBuilder messages)
			where T : class, IBusinessObject
		{
			return GetBusinessObjectRules<T>(contextId, domainId).Validate(businessObject, messages);
		}

		/// <summary>
		/// Validates a Business Object Collection.
		/// </summary>
		/// <typeparam name="T">The interface type of the Business Object Collection</typeparam>
		/// <param name="collection">The Business Object Collection to be validated</param>
		/// <param name="contextId">The Validation Context against which the Property will be validated</param>
		/// <param name="domainId">The Domain to which the object belongs.</param>
		/// <param name="messages">A StringBuilder object into which Rule Validation Messages will be written.</param>
		/// <returns>Returns true if the Business Object Collection has passed all Rule Validations</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Type parameter is needed for subsequent call to ValidateCollectionElements")]
		public bool ValidateCollection<T>(T collection, string contextId, string domainId, StringBuilder messages)
			where T : class, IBusinessObjectCollection
		{
			return GetBusinessObjectCollectionRules<T>(contextId, domainId).Validate(collection, messages);
		}

		/// <summary>
		/// Gets the rules for the specified property by context and domain.
		/// </summary>
		/// <typeparam name="T">The type of property.</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		/// <param name="propertyName">The name of the property.</param>
		protected abstract RuleCollection<Property<T>> GetPropertyRules<T>(string contextId, string domainId, string propertyName);

		/// <summary>
		/// Gets the rules for the specified Business Object
		/// </summary>
		/// <typeparam name="T">The Business Object Type</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		protected abstract RuleCollection<T> GetBusinessObjectRules<T>(string contextId, string domainId)
			where T : IBusinessObject;

		/// <summary>
		/// Gets the rules for the specified Business Object List
		/// </summary>
		/// <typeparam name="T">The interface type of the Business Object Collection</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		protected abstract RuleCollection<T> GetBusinessObjectCollectionRules<T>(string contextId, string domainId);

	}
}
