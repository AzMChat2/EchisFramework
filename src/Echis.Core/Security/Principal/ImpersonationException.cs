using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace System.Security.Principal
{
	/// <summary>
	/// Exception thrown by the WindowsIdentityImpersionation class when impersonation fails.
	/// </summary>
	[Serializable]
	public class ImpersonationException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public ImpersonationException() { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The exception message</param>
		public ImpersonationException(string message) : base(message) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The exception message</param>
		/// <param name="inner">The exception which cause this exception to be thrown.</param>
		public ImpersonationException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">Serialization information.</param>
		/// <param name="context">Serialization context.</param>
		protected ImpersonationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}