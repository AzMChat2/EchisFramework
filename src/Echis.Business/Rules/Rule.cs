using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Business.Rules
{
	/// <summary>
	/// Represents a basic Business Rule
	/// </summary>
	public abstract class Rule<T> : IRule<T>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		protected Rule() { }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ruleMessage">The invalidation message of the rule</param>
		protected Rule(string ruleMessage)
		{
			RuleMessage = ruleMessage;
		}

		/// <summary>
		/// Gets or sets the invalidation message of the rule.
		/// </summary>
		protected string RuleMessage { get; set; }

		/// <summary>
		/// Validates the specified object.
		/// </summary>
		/// <param name="item">The object to be validated.</param>
		/// <returns>Return true if the object is valid. Returns false if the object is not valid.</returns>
		public abstract bool Validate(T item);

		/// <summary>
		/// Gets the invalidtion message for the specified item.
		/// </summary>
		/// <param name="item">The invalidated item.</param>
		/// <remarks>Derived classes may override if a custom formatted message is required.</remarks>
		public virtual string GetMessage(T item)
		{
			return RuleMessage;
		}

		/// <summary>
		/// Used by the Rule Manager to set initialization parameters.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		/// <remarks>Derived classes need to call base implementation or provide initialization for Message and RuleOverridable</remarks>
		protected internal virtual void SetParameter(Parameter parameter)
		{
			if (parameter != null)
			{
				if (parameter.Name.Equals("Message", StringComparison.OrdinalIgnoreCase)) RuleMessage = (string)parameter;
			}
		}
	}

	/// <summary>
	/// Represents a collection of Business Rules.
	/// </summary>
	/// <typeparam name="T">The type of object which the collection of rules operates against.</typeparam>
	public class RuleCollection<T> : List<IRule<T>>
	{
		/// <summary>
		/// Executes all of the Rules in the Collection to validate the specified object. 
		/// </summary>
		/// <param name="item">The item to be validated</param>
		/// <param name="messages">The StringBuilder to which rule messages will be written.</param>
		/// <returns>Returns true if all the rules pass validation.</returns>
		public bool Validate(T item, StringBuilder messages)
		{
			bool retVal = true;

			ForEach(rule => retVal = retVal & Validate(rule, item, messages));

			return retVal;
		}

		/// <summary>
		/// Executes the rule against the specified object, adding the Rule Message if the validation fails.
		/// </summary>
		/// <param name="rule">The rule to be executed.</param>
		/// <param name="item">The object to be validated.</param>
		/// <param name="messages">The string builder to which rule messages will be written.</param>
		/// <returns>Returns true if the object passes rule validation.</returns>
		private static bool Validate(IRule<T> rule, T item, StringBuilder messages)
		{
			bool retVal = rule.Validate(item);

			if (!retVal) messages.AppendLine(rule.GetMessage(item));

			return retVal;
		}
	}
}
