using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace System.Security
{
  /// <summary>
  /// Provides base functionality for a Symmetric Algorithm Cryptography Provider
  /// </summary>
  public abstract class SymmetricAlgorithmCryptographyProvider : ICryptographyProvider
	{

		#region Static Helper methods
		/// <summary>
		/// Extracts the IV from the input data.
		/// </summary>
		/// <param name="data">The input Data containing the IV and Encrypted Value</param>
		protected static byte[] ExtractIV(byte[] data)
		{
			if (data.IsNullOrEmpty()) throw new ArgumentNullException("data");

			byte[] retVal = new byte[16];
			Array.Copy(data, retVal, 16);
			return retVal;
		}

		/// <summary>
		/// Extracts the Data from the input data.
		/// </summary>
		/// <param name="data">The input Data containing the IV and Encrypted Value</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods",
			Justification = "False positive. The parameter 'data' is being validated.")]
		protected static byte[] ExtractData(byte[] data)
		{
			if (data.IsNullOrEmpty()) throw new ArgumentNullException("data");

			byte[] retVal = new byte[data.Length - 16];
			Array.Copy(data, 16, retVal, 0, retVal.Length);
			return retVal;
		}
		#endregion
  
		/// <summary>
    /// Gets the Symmetric Algorithm Settings used to intialize the Symmetric Algorithm
    /// </summary>
    public ISymmetricAlgorithmSettings Settings { get; set; }

    /// <summary>
    /// Gets the Symmetric Algorithm Provider
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
      Justification = "A property is not appropriate here")]
    protected abstract SymmetricAlgorithm GetProvider();

    /// <summary>
    /// Encrypts a string value.
    /// </summary>
    /// <param name="value">The string to be encrypted.</param>
    public virtual string EncryptString(string value)
    {
			if (Settings == null) throw new InvalidOperationException("Cryptography Settings has not been set.");

			byte[] data = Encoding.ASCII.GetBytes(value);

			using (SymmetricAlgorithm provider = GetProvider())
      {
        provider.Key = Settings.CryptoKey;
				if (Settings.CryptoIV.IsNullOrEmpty())
				{
					provider.GenerateIV();
				}
				else
				{
					provider.IV = Settings.CryptoIV;
				}

        using (MemoryStream stream = new MemoryStream())
        {
          using (ICryptoTransform transform = provider.CreateEncryptor())
          {
            CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
						if (Settings.CryptoIV.IsNullOrEmpty())
						{
							cryptoStream.Write(provider.IV, 0, provider.IV.Length);
						}
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
          }

          return Convert.ToBase64String(stream.ToArray());
        }
      }
    }

		/// <summary>
    /// Decrypts a string from an ecrypted string.
    /// </summary>
    /// <param name="encryptedValue">The string containing the encrypted data.</param>
    public virtual string DecryptString(string encryptedValue)
    {
			if (Settings == null) throw new InvalidOperationException("Cryptography Settings has not been set.");

			byte[] data = Convert.FromBase64String(encryptedValue);

			using (SymmetricAlgorithm provider = GetProvider())
      {
        provider.Key = Settings.CryptoKey;

				if (Settings.CryptoIV.IsNullOrEmpty())
				{
					provider.IV = ExtractIV(data);
					data = ExtractData(data);
				}
				else
				{
					provider.IV = Settings.CryptoIV;
				}

        using (MemoryStream stream = new MemoryStream())
        {
          using (ICryptoTransform transformer = provider.CreateDecryptor())
          {
            CryptoStream cryptoStream = new CryptoStream(stream, transformer, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
          }

          return Encoding.ASCII.GetString(stream.ToArray());
        }
      }
    }
  }

  /// <summary>
  /// Provides base functionality for a Rijndael Symmetric Algorithm Cryptography Provider
  /// </summary>
  public class RijndaelCryptographyProvider : SymmetricAlgorithmCryptographyProvider
  {
    /// <summary>
    /// Gets the Symmetric Algorithm Provider
    /// </summary>
    protected override SymmetricAlgorithm GetProvider()
    {
      return string.IsNullOrWhiteSpace(Settings.AlgorithmName) ? Rijndael.Create() : Rijndael.Create(Settings.AlgorithmName);
    }
  }

  /// <summary>
  /// Provides base functionality for a DES Symmetric Algorithm Cryptography Provider
  /// </summary>
  [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DES",
    Justification = "Class is named to match Microsoft.NET Framework name.")]
  public class DESCryptographyProvider : SymmetricAlgorithmCryptographyProvider
  {
    /// <summary>
    /// Gets the Symmetric Algorithm Provider
    /// </summary>
    protected override SymmetricAlgorithm GetProvider()
    {
      return string.IsNullOrWhiteSpace(Settings.AlgorithmName) ? DES.Create() : DES.Create(Settings.AlgorithmName);
    }
  }

  /// <summary>
  /// Provides base functionality for a Triple DES Symmetric Algorithm Cryptography Provider
  /// </summary>
  [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DES",
    Justification = "Class is named to match Microsoft.NET Framework name.")]
  public class TripleDESCryptographyProvider : SymmetricAlgorithmCryptographyProvider
  {
    /// <summary>
    /// Gets the Symmetric Algorithm Provider
    /// </summary>
    protected override SymmetricAlgorithm GetProvider()
    {
      return string.IsNullOrWhiteSpace(Settings.AlgorithmName) ? TripleDES.Create() : TripleDES.Create(Settings.AlgorithmName);
    }
  }

  /// <summary>
  /// Provides base functionality for an Aes Symmetric Algorithm Cryptography Provider
  /// </summary>
  public class AesCryptographyProvider : SymmetricAlgorithmCryptographyProvider
  {
    /// <summary>
    /// Gets the Symmetric Algorithm Provider
    /// </summary>
    protected override SymmetricAlgorithm GetProvider()
    {
      return string.IsNullOrWhiteSpace(Settings.AlgorithmName) ? Aes.Create() : Aes.Create(Settings.AlgorithmName);
    }
  }

  /// <summary>
  /// Provides base functionality for an RC2 Symmetric Algorithm Cryptography Provider
  /// </summary>
  public class RC2CryptographyProvider : SymmetricAlgorithmCryptographyProvider
  {
    /// <summary>
    /// Gets the Symmetric Algorithm Provider
    /// </summary>
    protected override SymmetricAlgorithm GetProvider()
    {
      return string.IsNullOrWhiteSpace(Settings.AlgorithmName) ? RC2.Create() : RC2.Create(Settings.AlgorithmName);
    }
  }
}
