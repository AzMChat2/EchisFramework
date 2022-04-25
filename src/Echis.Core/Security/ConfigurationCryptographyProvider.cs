using System.Security.Cryptography;
using System.Xml.Serialization;

namespace System.Security
{
  internal sealed class ConfigurationCryptographyProvider : RijndaelCryptographyProvider
	{
    /// <summary>
    /// Constructor.
    /// </summary>
		public ConfigurationCryptographyProvider()
		{
			Settings = XmlSerializer<Key>.DeserializeFromDeflatedResource(typeof(Key).Assembly, "System.Security.Key.dat");
		}
	}

	/// <summary>
	/// Stores the Cryptography Key and IV information.
	/// </summary>
	public class Key : ISymmetricAlgorithmSettings
	{
    /// <summary>
    /// Not Used.
    /// </summary>
    [XmlIgnore]
    public string AlgorithmName { get { return null; } }

    /// <summary>
    /// Gets or sets the Key.
    /// </summary>
    [XmlElement("Key")]
    public byte[] CryptoKey { get; set; }

    /// <summary>
    /// Gets or sets the IV.
    /// </summary>
    [XmlElement("IV")]
    public byte[] CryptoIV { get; set; }
  }
}
