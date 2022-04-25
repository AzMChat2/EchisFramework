using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Data.Objects;

namespace System.Business.Rules
{
	/// <summary>
	/// The IRuleManager contains methods used to dynamically create rules for properties, business objects and business object lists.
	/// </summary>
	public interface IRuleManager
	{
		/// <summary>
		/// Validates a Business Object Property
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="objectProperty">The property to be validated</param>
		/// <param name="contextId">The Validation Context against which the Property will be validated</param>
		/// <param name="domainId">The Domain to which the object belongs.</param>
		/// <param name="messages">A StringBuilder object into which Rule Validation Messages will be written.</param>
		/// <returns>Returns true if the Property has passed all Rule Validations</returns>
		bool ValidateProperty<T>(Property<T> objectProperty, string contextId, string domainId, StringBuilder messages);

		/// <summary>
		/// Validates a Business Object.
		/// </summary>
		/// <typeparam name="T">The interface type of the Business Object</typeparam>
		/// <param name="businessObject">The Business Object to be validated</param>
		/// <param name="contextId">The Validation Context against which the Property will be validated</param>
		/// <param name="domainId">The Domain to which the object belongs.</param>
		/// <param name="messages">A StringBuilder object into which Rule Validation Messages will be written.</param>
		/// <returns>Returns true if the Business Object has passed all Rule Validations</returns>
		bool ValidateObject<T>(T businessObject, string contextId, string domainId, StringBuilder messages)
			where T : class, IBusinessObject;

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
		bool ValidateCollection<T>(T collection, string contextId, string domainId, StringBuilder messages)
			where T : class, IBusinessObjectCollection;
	}
}
