using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace System.RhinoMocks
{
	/// <summary>
	/// Exception thrown when the Mock Repository has not been properly configured for a test.
	/// </summary>
	[Serializable]
	public class MockException : Exception
	{
		/// <summary>
		/// Default Constructor. Creates a new MockException object.
		/// </summary>
		public MockException() { }
		/// <summary>
		/// Constructor. Creates a new MockException object.
		/// </summary>
		/// <param name="message">The exception message.</param>
		public MockException(string message) : base(message) { }
		/// <summary>
		/// Constructor. Creates a new MockException object.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="inner"></param>
		public MockException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Constructor. Creates a new MockException object.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MockException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
