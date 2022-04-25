using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace System
{
  /// <summary>
  /// Provides a base class from which to derive exceptions with Constructor overloads that allow message formatting.
  /// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "This is an abstract base class not intended for direct consumption.")]
  public abstract class ExceptionBase : Exception
  {
    /// <summary>
    /// Constructs an exception messaging using the provided message format at parameters
    /// </summary>
    /// <param name="format">A composite format message string</param>
    /// <param name="args">The parameters with with to replace the placeholders in the format string.</param>
    protected static string GetMessage(string format, params object[] args)
    {
      return string.Format(CultureInfo.InvariantCulture, format, args);
    }

    /// <summary>
    /// Serialization Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected ExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <summary>
    /// Creates a new instance of the Exception
    /// </summary>
    protected ExceptionBase() { }
    /// <summary>
    /// Creates a new instance of the Exception
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected ExceptionBase(string message) : base(message) { }
    /// <summary>
    /// Creates a new instance of the Exception
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The exception which was thrown immediately prior to this exception.</param>
    protected ExceptionBase(string message, Exception inner) : base(message, inner) { }

    /// <summary>
    /// Creates a new instance of the Exception
    /// </summary>
    /// <param name="format">A composite format message string</param>
    /// <param name="args">The parameters with with to replace the placeholders in the format string.</param>
    protected ExceptionBase(string format, params object[] args) : base(GetMessage(format, args)) { }
    /// <summary>
    /// Creates a new instance of the Exception
    /// </summary>
    /// <param name="inner">The exception which was thrown immediately prior to this exception.</param>
    /// <param name="format">A composite format message string</param>
    /// <param name="args">The parameters with with to replace the placeholders in the format string.</param>
    protected ExceptionBase(Exception inner, string format, params object[] args) : base(GetMessage(format, args), inner) { }

  }
}
