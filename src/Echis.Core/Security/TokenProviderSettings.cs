using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;

namespace System.Security
{
	/// <summary>
	/// Defines settings used by the Token Provider
	/// </summary>
	public interface ITokenProviderSettings
	{
		/// <summary>
		/// Gets the Key used to initialize the Hashing Algorithm
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		byte[] Key { get; }
		/// <summary>
		/// Gets the encoding used to generate the input for the Hashing Algorithm
		/// </summary>
		Encoding Encoding { get; }
	}

	/// <summary>
	/// Provides settings used by the Token Provider
	/// </summary>
	public class TokenProviderSettings : ITokenProviderSettings
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TokenProviderSettings() { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="key">A Base64 string containing the intialization information</param>
		public TokenProviderSettings(string key)
		{
			Key = Convert.FromBase64String(key);
		}

		/// <summary>
		/// Gets the Key used to initialize the Hashing Algorithm
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays"), XmlAttribute]
		public byte[] Key { get; set; }

		/// <summary>
		/// Gets the encoding used to generate the input for the Hashing Algorithm
		/// </summary>
		[XmlIgnore]
		public Encoding Encoding { get { return Encoding.Unicode; } }
	}
}
