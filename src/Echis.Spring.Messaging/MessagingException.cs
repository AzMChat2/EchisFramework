using System;
using System.Runtime.Serialization;

namespace System.Spring.Messaging
{
	/// <summary>
	/// An exception which is raised by the Messaging framework.
	/// </summary>
	[Serializable]
	public class MessagingException : ExceptionBase
	{
		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		public MessagingException() { }
		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="message">The exception message.</param>
		public MessagingException(string message) : base(message) { }
		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="inner">The exception which was thrown immediately prior to this exception.</param>
		public MessagingException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MessagingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="format">A composite format message string</param>
		/// <param name="args">The parameters with with to replace the placeholders in the format string.</param>
		public MessagingException(string format, params object[] args) : base(format, args) { }
		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="inner">The exception which was thrown immediately prior to this exception.</param>
		/// <param name="format">A composite format message string</param>
		/// <param name="args">The parameters with with to replace the placeholders in the format string.</param>
		public MessagingException(Exception inner, string format, params object[] args) : base(inner, format, args) { }
	}
}
