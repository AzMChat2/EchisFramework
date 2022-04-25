using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Principal;
using System.Xml.Serialization;

namespace System.Data
{
	/// <summary>
	/// Represents User Credentials used to connect to a database.
	/// </summary>
	public sealed class DataAccessCredentials : ImpersonationCredentials
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public DataAccessCredentials()
		{
			IsEncrypted = true;
		}

		/// <summary>
		/// Gets or sets a value indicating if the password is still encypted.
		/// </summary>
		[XmlIgnore]
		internal bool IsEncrypted { get; set; }

		/// <summary>
		/// Decrypts the password.
		/// </summary>
		[DebuggerHidden]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) the DecryptionProvider implementation will possibly throw")]
		internal void Decrypt()
		{
			try
			{
				Password = Decryptor.Instance.DecryptString(Password);
			}
			catch (Exception ex)
			{
				TS.Logger.WriteLineIf(TS.Info, TS.Categories.Info, "Unable to decrypt password: {0}", ex.GetExceptionMessage());
			}
			IsEncrypted = false;
		}
	}
}