using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/// <summary>
	/// Stores connection string information.
	/// </summary>
	internal sealed class ConnectionStringInfo
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="encryptedConnectionString">The raw (encrypted) connection string.</param>
		public ConnectionStringInfo(string encryptedConnectionString)
		{
			ConnectionString = encryptedConnectionString;
			IsEncrypted = true;
		}

		/// <summary>
		/// Gets the Connection String.
		/// </summary>
		/// <remarks>Use the IsEncrypted property to determine if the connection string has been decrypted.</remarks>
		public string ConnectionString { get; private set; }
		/// <summary>
		/// Gets a valud indicating if the connection string is still encrypted.
		/// </summary>
		public bool IsEncrypted { get; private set; }

		/// <summary>
		/// Decrypts the connection string using the configured IDecryptionProvider (or the DefaultDecryptionProvider if none is configured).
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) the DecryptionProvider implementation will possibly throw")]
		public void Decrypt()
		{
			try
			{
				ConnectionString = Decryptor.Instance.DecryptString(ConnectionString);
			}
			catch (Exception ex)
			{
				TS.Logger.WriteLineIf(TS.Info, TS.Categories.Info, "Unable to decrypt Connection String: {0}", ex.GetExceptionMessage());
			}
			IsEncrypted = false;
		}
	}
}