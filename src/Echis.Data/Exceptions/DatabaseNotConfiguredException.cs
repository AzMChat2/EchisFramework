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
	public class DatabaseNotConfiguredException : DataException
	{
		/// <summary>
		/// The Exception Message.
		/// </summary>
		private const string MsgFormat = "The specified Data Access Object '{0}' was not found in the configuration.";

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dbName">The name of the requested Data Access object.</param>
		public DatabaseNotConfiguredException(string dbName) : base(string.Format(CultureInfo.InvariantCulture, MsgFormat, dbName)) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		public DatabaseNotConfiguredException() : base() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dbName">The name of the requested Data Access object.</param>
		/// <param name="innerException"></param>
		public DatabaseNotConfiguredException(string dbName, Exception innerException) : base(string.Format(CultureInfo.InvariantCulture, MsgFormat, dbName), innerException) { }

		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DatabaseNotConfiguredException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}
}
