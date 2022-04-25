using System;
using System.Data;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Data
{
	/// <summary>
	/// Exception thrown when a requested Data Access name was not found in the databases configuration.
	/// </summary>
	[Serializable]
	public class UnableToCreateDataAccessClientException : DataException
	{
		/// <summary>
		/// The Exception Message.
		/// </summary>
		private const string MsgFormat = "Unable to create data access client '{0}' the assembly or typename specified is invalid or the class has no public parameterless constructor.";

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the requested Data Access object.</param>
		public UnableToCreateDataAccessClientException(string name) : base(string.Format(CultureInfo.InvariantCulture, MsgFormat, name)) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		public UnableToCreateDataAccessClientException() : base() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the requested Data Access object.</param>
		/// <param name="innerException"></param>
		public UnableToCreateDataAccessClientException(string name, Exception innerException) : base(string.Format(CultureInfo.InvariantCulture, MsgFormat, name), innerException) { }

		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected UnableToCreateDataAccessClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}
}
