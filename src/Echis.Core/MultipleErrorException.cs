using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	/// <summary>
	/// Represents a collection of Exceptions that were thrown.
	/// </summary>
	[Serializable]
	public class MultipleErrorException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The name of the requested Data Access object.</param>
		public MultipleErrorException(string message) : base(message) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		public MultipleErrorException() : base() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="innerException"></param>
		public MultipleErrorException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MultipleErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Stores the underlying value for the Exceptions property.
		/// </summary>
		private List<Exception> _exceptions = new List<Exception>();
		/// <summary>
		/// Gets the collection of exceptions which lead to the current exception being thrown.
		/// </summary>
		public List<Exception> Exceptions { get { return _exceptions; } }

		/// <summary>
		/// Sets the SerializationInfo with information about the exception.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
