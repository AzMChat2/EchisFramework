using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace System.Business.Rules
{
	/// <summary>
	/// The RuleException is the base class for all Rule Exceptions.
	/// </summary>
	[Serializable]
	public class RuleException : Exception
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public RuleException() { }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">The exception message.</param>
		public RuleException(string message) : base(message) { }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">The exception message</param>
		/// <param name="inner">The exception which caused this exception.</param>
		public RuleException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">Serialization Information.</param>
		/// <param name="context">Serialization Streaming Context</param>
		protected RuleException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="inner">The exception which caused this exception.</param>
		/// <param name="format">A message format used to generate the message.</param>
		/// <param name="args">The message format parameters.</param>
		public RuleException(Exception inner, string format, params object[] args) : base(string.Format(CultureInfo.InvariantCulture, format, args), inner) { }

	}
}
