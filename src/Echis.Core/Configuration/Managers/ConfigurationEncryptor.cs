using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Security;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Used to provide Crytpography services for the RemoteSectionHandler and ConfigurationManagerBase classes.
	/// </summary>
	internal static class ConfigurationEncryptor
	{
		/// <summary>
		/// Default ObjectId for the Cryptography Provider
		/// </summary>
		public const string CryptographyProviderObjectId = "System.Configuration.CryptographyProvider";

		/// <summary>
		/// Stores the singleton instance of the CryptographyProvider
		/// </summary>
		private static ICryptographyProvider _cryptographyProvider;
		/// <summary>
		/// Gets the singleton instance of the CryptographyProvider
		/// </summary>
		public static ICryptographyProvider CryptographyProvider
		{
			get
			{
				if (_cryptographyProvider == null)
				{
					_cryptographyProvider = IOC.GetFrameworkObject<ICryptographyProvider>(Settings.Values.CryptographyProvider, CryptographyProviderObjectId, false);
					if (_cryptographyProvider == null)
					{
						_cryptographyProvider = new ConfigurationCryptographyProvider();
					}
				}
				return _cryptographyProvider;
			}
		}

		/// <summary>
		/// Encrypts a string of data.
		/// </summary>
		/// <param name="data">The string containing the data to be encrypted.</param>
		/// <returns>A string representing the encrypted bytes.</returns>
		public static string EncryptString(string data)
		{
			return CryptographyProvider.EncryptString(data);
		}

		/// <summary>
		/// Decrypts a string of encoded data.
		/// </summary>
		/// <param name="data">The encoded data to be decrypted.</param>
		/// <returns>The decrypted string from the encoded data.</returns>
		public static string DecryptString(string data)
		{
			return CryptographyProvider.DecryptString(data);
		}
	}
}
