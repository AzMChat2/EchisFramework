
namespace System.Security
{
	/// <summary>
	/// The default Cryptography Provider.
	/// </summary>
	/// <remarks>This class performs no actual encryption or decryption. And can be used when values are stored in plain text.</remarks>
	public class DefaultCryptographyProvider : ICryptographyProvider
	{
		/// <summary>
		/// Decrypts a string from an ecrypted string.
		/// </summary>
		/// <param name="encryptedValue">The string containing the encrypted data.</param>
		/// <returns>Returns the decrypted string.</returns>
		public string DecryptString(string encryptedValue)
		{
			return encryptedValue;
		}

		/// <summary>
		/// Encrypts a string value.
		/// </summary>
		/// <param name="value">The string to be encrypted.</param>
		/// <returns>Returns the encrypted string.</returns>
		public string EncryptString(string value)
		{
			return value;
		}
	}
}
