using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Business.Resources;
using System.Data.Objects;

namespace System.Business.Rules
{
	#region StringNotNullRule
	/// <summary>
	/// Represents a property rule which validates that the string value is not null.
	/// </summary>
	public class StringNotNullRule : Rule<Property<string>>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public StringNotNullRule() : base(RuleMessages.Msg_PropertyRule_StringNotNull) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The invalidation message of the rule</param>
		public StringNotNullRule(string message) : base(message) { }
		/// <summary>
		/// Validates the new string value.
		/// </summary>
		/// <param name="item">The Property to be validated.</param>
		/// <returns>Returns true if the string is not null, otherwise returns false.</returns>
		public override bool Validate(Property<string> item)
		{
			return (item == null) || (item.Value != null);
		}
	}
	#endregion

	#region StringNotEmptyRule
	/// <summary>
	/// Represents a property rule which validates that the string value is not empty.
	/// </summary>
	public class StringNotEmptyRule : Rule<Property<string>>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public StringNotEmptyRule() : base(RuleMessages.Msg_PropertyRule_StringNotEmpty) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The invalidation message of the rule</param>
		public StringNotEmptyRule(string message) : base(message) { }

		/// <summary>
		/// Validates the new string value.
		/// </summary>
		/// <param name="item">The Property to be validated.</param>
		/// <returns>Returns true if the string is not empty, otherwise returns false.</returns>
		[SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength",
			Justification = "Unable to use string.IsNullOrEmpty in this case because we also need to check for blank (filled with spaces) strings")]
		public override bool Validate(Property<string> item)
		{
			return (item == null) || (item.Value == null) || (item.Value.Trim() != string.Empty);
		}
	}
	#endregion

	#region StringLengthRule
	/// <summary>
	/// Represents a property rule which validates that the string's length meets minumum and maximum requirements
	/// </summary>
	public class StringLengthRule : Rule<Property<string>>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public StringLengthRule() : base(string.Format(CultureInfo.CurrentCulture, RuleMessages.Msg_PropertyRule_StringLength, 0, 0)) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="minLength">The minumum length allowed.</param>
		/// <param name="maxLength">The maximum length allowed.</param>
		public StringLengthRule(int minLength, int maxLength)
			: base(string.Format(CultureInfo.CurrentCulture, RuleMessages.Msg_PropertyRule_StringLength, minLength, maxLength))
		{
			MinLength = minLength;
			MaxLength = maxLength;
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The invalidation message of the rule</param>
		/// <param name="minLength">The minimum allowable length.</param>
		/// <param name="maxLength">The maximum allowable length.</param>
		public StringLengthRule(string message, int minLength, int maxLength)
			: base(message)
		{
			MinLength = minLength;
			MaxLength = maxLength;
		}

		/// <summary>
		/// Used by the Rule Manager to set initialization parameters.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		protected internal override void SetParameter(Parameter parameter)
		{
			if (parameter != null)
			{
				if (parameter.Name.Equals("minlength", StringComparison.OrdinalIgnoreCase))
				{
					MinLength = (int)parameter;
					SetMessage();
				}
				else if (parameter.Name.Equals("maxlength", StringComparison.OrdinalIgnoreCase))
				{
					MaxLength = (int)parameter;
					SetMessage();
				}
				else if (parameter.Name.Equals("message", StringComparison.OrdinalIgnoreCase))
				{
					_messageFormat = (string)parameter;
					SetMessage();
				}
				else
				{
					base.SetParameter(parameter);
				}
			}
		}

		/// <summary>
		/// Stores the Message format.
		/// </summary>
		private string _messageFormat = RuleMessages.Msg_PropertyRule_StringLength;
		/// <summary>
		/// Sets the message using the Message Format and the Min and Max Length properties
		/// </summary>
		private void SetMessage()
		{
			RuleMessage = string.Format(CultureInfo.CurrentCulture, _messageFormat, MinLength, MaxLength);
		}

		/// <summary>
		/// Validates the new string value.
		/// </summary>
		/// <param name="item">The Property to be validated.</param>
		/// <returns>Returns true if the string meets the minumum and maximum length requirements, otherwise returns false.</returns>
		public override bool Validate(Property<string> item)
		{
			return (item == null) || (item.Value == null) || ((item.Value.Length >= MinLength) && (item.Value.Length <= MaxLength));
		}

		/// <summary>
		/// The minumum length the string may be.
		/// </summary>
		public int MinLength { get; private set; }

		/// <summary>
		/// The maximum length the string may be.
		/// </summary>
		public int MaxLength { get; private set; }
	}
	#endregion

	#region RangeRule<T>
	/// <summary>
	/// Represents a property rule which validates that the value meets minumum and maximum requirements
	/// </summary>
	/// <typeparam name="T">The property value type.</typeparam>
	public class RangeRule<T> : Rule<Property<T>> where T : IComparable<T>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public RangeRule() : base(string.Format(CultureInfo.CurrentCulture, RuleMessages.Msg_PropertyRule_Range, default(T), default(T))) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="minValue">The minimum value allowed.</param>
		/// <param name="maxValue">The maximum value allowed.</param>
		public RangeRule(T minValue, T maxValue)
			: base(string.Format(CultureInfo.CurrentCulture, RuleMessages.Msg_PropertyRule_Range, minValue, maxValue))
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The invalidation message of the rule</param>
		/// <param name="minValue">The minimum value allowed.</param>
		/// <param name="maxValue">The maximum value allowed.</param>
		public RangeRule(string message, T minValue, T maxValue)
			: base(message)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		/// <summary>
		/// Used by the Rule Manager to set initialization parameters.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		protected internal override void SetParameter(Parameter parameter)
		{
			if (parameter == null) throw new ArgumentNullException("parameter");

			if (parameter.Name.Equals("minvalue", StringComparison.OrdinalIgnoreCase))
			{
				MinValue = parameter.To<T>();
				SetMessage();
			}
			else if (parameter.Name.Equals("maxvalue", StringComparison.OrdinalIgnoreCase))
			{
				MaxValue = parameter.To<T>();
				SetMessage();
			}
			else if (parameter.Name.Equals("message", StringComparison.OrdinalIgnoreCase))
			{
				_messageFormat = (string)parameter;
				SetMessage();
			}
			else
			{
				base.SetParameter(parameter);
			}
		}

		/// <summary>
		/// Stores the Message format.
		/// </summary>
		private string _messageFormat = RuleMessages.Msg_PropertyRule_StringLength;
		/// <summary>
		/// Sets the message using the Message Format and the Min and Max Length properties
		/// </summary>
		private void SetMessage()
		{
			RuleMessage = string.Format(CultureInfo.CurrentCulture, _messageFormat, MinValue, MaxValue);
		}

		/// <summary>
		/// Validates the new value against the minimum and maximum values.
		/// </summary>
		/// <param name="item">The Property to be validated.</param>
		/// <returns>Returns true if the new value is within the specified minimum and maximum limits, otherwise returns false.</returns>
		public override bool Validate(Property<T> item)
		{
			bool retVal = true;

			if (item != null)
			{
				if (MinValue != null)
				{
					retVal = (MinValue.CompareTo(item.Value) <= 0);
				}

				if ((retVal) && (MaxValue != null))
				{
					retVal = (MaxValue.CompareTo(item.Value) >= 0);
				}
			}

			return retVal;
		}

		/// <summary>
		/// Gets the minimum value allowed.
		/// </summary>
		public T MinValue { get; private set; }

		/// <summary>
		/// Gets the maximum value allowed.
		/// </summary>
		public T MaxValue { get; private set; }
	}

	/// <summary>
	/// Represents a property rule which validates that the value meets minumum and maximum requirements
	/// </summary>
	/// <typeparam name="T">The property value type.</typeparam>
	public sealed class NullableRangeRule<T> : RangeRule<T> where T : struct, IComparable<T>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public NullableRangeRule() : base() { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="minValue">The minimum value allowed.</param>
		/// <param name="maxValue">The maximum value allowed.</param>
		public NullableRangeRule(T minValue, T maxValue) : base(minValue, maxValue) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The invalidation message of the rule</param>
		/// <param name="minValue">The minimum value allowed.</param>
		/// <param name="maxValue">The maximum value allowed.</param>
		public NullableRangeRule(string message, T minValue, T maxValue) : base(message, minValue, maxValue) { }

		/// <summary>
		/// Validates the new value against the minimum and maximum values.
		/// </summary>
		/// <param name="item">The Property to be validated.</param>
		/// <returns>Returns true if the new value is within the specified minimum and maximum limits, otherwise returns false.</returns>
		public bool Validate(Property<T?> item)
		{
			bool retVal = true;

			if ((item != null) && (item.Value.HasValue))
			{
				if (MinValue.CompareTo(default(T)) != 0) retVal = (MinValue.CompareTo(item.Value.Value) <= 0);
				if (retVal && (MaxValue.CompareTo(default(T)) != 0)) retVal = (MaxValue.CompareTo(item.Value.Value) >= 0);
			}

			return retVal;
		}
	}
	#endregion
}
