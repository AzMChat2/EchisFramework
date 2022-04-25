using System.Xml.Serialization;
using System;

namespace System.Security
{
  /// <summary>
  /// Defines values used to initialize a Symmetric Algorithm Cryptography Provider
  /// </summary>
  public class SymmetricAlgorithmSettings : ISymmetricAlgorithmSettings
  {
    /// <summary>
    /// Default Constructor.
    /// </summary>
    public SymmetricAlgorithmSettings() { }
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iv">A Base64 string containing the bytes for the Crypto IV</param>
    /// <param name="key">A Base64 string containing the bytes for the Crypto Key</param>
    public SymmetricAlgorithmSettings(string iv, string key)
    {
      if (!string.IsNullOrWhiteSpace(iv)) CryptoIV = Convert.FromBase64String(iv);
			if (!string.IsNullOrWhiteSpace(key)) CryptoKey = Convert.FromBase64String(key);
    }

    /// <summary>
    /// Gets the Algorithm Name used to encrypt or decrypt data.
    /// </summary>
    [XmlAttribute]
    public string AlgorithmName { get; set; }

    /// <summary>
    /// Gets the Cryptography Key for the Symmetric Algorithm
    /// </summary>
    [XmlAttribute]
    public byte[] CryptoKey { get; set; }

    /// <summary>
    /// Gets the Cryptography IV for the Symmetric Algorithm
    /// </summary>
    [XmlAttribute]
    public byte[] CryptoIV { get; set; }
  }
}
