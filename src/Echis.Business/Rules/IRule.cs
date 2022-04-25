using System.Collections;

namespace System.Business.Rules
{
	/// <summary>
	/// Defines the basic Business Rule
	/// </summary>
	public interface IRule<in T>
	{
		/// <summary>
		/// Gets or sets the invalidation message of the rule.
		/// </summary>
		string GetMessage(T item);
		/// <summary>
		/// Validates the specified object.
		/// </summary>
		/// <param name="item">The object to be validated.</param>
		/// <returns>Return true if the object is valid. Returns false if the object is not valid.</returns>
		bool Validate(T item);
	}
}
