using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace System.Business.Rules
{
	/// <summary>
	/// The RuleConfigurationException is used to indicate that a Rule Configuration is invalid.
	/// </summary>
	[Serializable]
	public class RuleConfigurationException : RuleException
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public RuleConfigurationException() { }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">The exception message.</param>
		public RuleConfigurationException(string message) : base(message) { }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">The exception message</param>
		/// <param name="inner">The exception which caused this exception.</param>
		public RuleConfigurationException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">Serialization Information.</param>
		/// <param name="context">Serialization Streaming Context</param>
		protected RuleConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="inner">The exception which caused this exception.</param>
		/// <param name="format">A message format used to generate the message.</param>
		/// <param name="args">The message format parameters.</param>
		public RuleConfigurationException(Exception inner, string format, params object[] args) : base(inner, format, args) { }

	}
}