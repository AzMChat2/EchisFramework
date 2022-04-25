using System;
using System.Configuration;
using System.Runtime.Serialization;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Exception class for Credentials Validation errors.
	/// </summary>
	[Serializable]
	public class CredentialsValidationException : ConfigurationErrorsException
	{
		private const string ExceptionMessage = "The specified configuration credentials are not valid.";

		/// <summary>
		/// Constructor.
		/// </summary>
		public CredentialsValidationException() : base(ExceptionMessage) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="innerException"></param>
		public CredentialsValidationException(Exception innerException) : base(ExceptionMessage, innerException) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="exceptionMessage"></param>
		public CredentialsValidationException(string exceptionMessage) : base(exceptionMessage) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="innerException"></param>
		/// <param name="exceptionMessage"></param>
		public CredentialsValidationException(string exceptionMessage, Exception innerException) : base(exceptionMessage, innerException) { }

		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected CredentialsValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}
}
