
namespace System.Security
{
	/// <summary>
	/// Defines methods used to decrypt strings
	/// </summary>
	public interface IDecryptionProvider
	{
		/// <summary>
		/// Decrypts a string from an ecrypted string.
		/// </summary>
		/// <param name="encryptedValue">The string containing the encrypted data.</param>
		/// <returns>Returns the decrypted string.</returns>
		string DecryptString(string encryptedValue);
	}
}
