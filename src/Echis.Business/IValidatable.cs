using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Echis.Business.Rules;

namespace Echis.Data.Objects
{
	/// <summary>
	/// Defines an object that is validatable
	/// </summary>
	public interface IValidatable
	{
		/// <summary>
		/// Occurs when the list has been invalidated by one or more rules.
		/// </summary>
		event EventHandler<InvalidatedEventArgs> Invalidated;
		/// <summary>
		/// Occurs when the list has been validated by all rules.
		/// </summary>
		event EventHandler<ValidatedEventArgs> Validated;

		/// <summary>
		/// Determines if the object is valid for the default context.
		/// </summary>
		bool IsValid();
		/// <summary>
		/// Determines if the object is valid for the specified context.
		/// </summary>
		/// <param name="contextId">The validation context to validate the object against.</param>
		bool IsValid(string contextId);

		/// <summary>
		/// Gets the rule messages for rules that currently do not pass validation.
		/// </summary>
		/// <remarks>If the object is valid, this will be an empty string.</remarks>
		string RuleMessages { get; }

		/// <summary>
		/// Gets the rule messages for rules that currently do not pass validation for the current object and any child objects within the current object.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "A property is not appropriate.")]
		IListEx<string> GetAllRuleMessages();
	}
}
