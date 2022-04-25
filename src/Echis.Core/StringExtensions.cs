
namespace System
{
	/// <summary>
	/// Provides extension methods for the String class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Returns a value indicating whether the specified System.String object occurs within this string.
		/// </summary>
		/// <param name="source">A System.String object in which to locate the specified value.</param>
		/// <param name="value">A System.String object to be found within this string.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
		/// <returns>Returns true indicating the specified System.String object occurs within this string or false indicating the specified System.String object does not occur within this string.</returns>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public static bool Contains(this string source, string value, StringComparison comparisonType)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (value == null) throw new ArgumentNullException("value");

			return source.IndexOf(value, comparisonType) >= 0;
		}


		/// <summary>
		/// Checks a string's length and truncates if the string is longer than the maximum value.
		/// </summary>
		/// <param name="input">The string whose length is to be validated.</param>
		/// <param name="maxLength">The maximum length of the return value.</param>
		/// <returns>Returns the input string up to the maximum length specified.</returns>
		/// <remarks>If the maximum length specified is zero or less than zero, then the input value will be returned without being truncated.</remarks>
		public static string Truncate(this string input, int maxLength)
		{
			if ((maxLength <= 0) || string.IsNullOrEmpty(input) || (input.Length <= maxLength))
			{
				return input;
			}
			else
			{
				return input.Substring(0, maxLength);
			}
		}

	}
}
