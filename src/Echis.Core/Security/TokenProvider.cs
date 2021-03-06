using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace System.Security
{
	/// <summary>
	/// Generic Token Provider class that provides Tokens using a Keyed Hash Algorithm
	/// </summary>
	/// <typeparam name="T">The type of Keyed Hash Algorithm used to generate the Tokens</typeparam>
	public class TokenProvider<T> : ITokenProvider
		where T : KeyedHashAlgorithm, new()
	{
		/// <summary>
		/// Provides settings used by the Token Provider
		/// </summary>
		public ITokenProviderSettings Settings { get; set; }

		/// <summary>
		/// Generates a token from the supplied components.
		/// </summary>
		/// <param name="components">The components which will be used to generate the Token</param>
		/// <returns>Returns a Base64 string containing the bytes generated by the Keyed Hash Algorithm</returns>
		public string GetToken(params object[] components)
		{
			using (KeyedHashAlgorithm hasher = new T())
			{
				hasher.Key = Settings.Key;

				string input = string.Join("|", components);
				byte[] output = hasher.ComputeHash(Settings.Encoding.GetBytes(input));

				return Convert.ToBase64String(output);
			}
		}
	}

	/// <summary>
	/// Token Provider class that provides Tokens using the HMACMD5 Algorithm
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		Justification = "The case is intended to match that of the related Microsoft.Net class.")]
	public class HMACMD5TokenProvider : TokenProvider<HMACMD5> { }

	/// <summary>
	/// Token Provider class that provides Tokens using the HMACRIPEMD160 Algorithm
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		Justification = "The case is intended to match that of the related Microsoft.Net class.")]
	public class HMACRIPEMD160TokenProvider : TokenProvider<HMACRIPEMD160> { }

	/// <summary>
	/// Token Provider class that provides Tokens using the HMACSHA1 Algorithm
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		Justification = "The case is intended to match that of the related Microsoft.Net class.")]
	public class HMACSHA1TokenProvider : TokenProvider<HMACSHA1> { }

	/// <summary>
	/// Token Provider class that provides Tokens using the HMACSHA256 Algorithm
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		Justification = "The case is intended to match that of the related Microsoft.Net class.")]
	public class HMACSHA256TokenProvider : TokenProvider<HMACSHA256> { }

	/// <summary>
	/// Token Provider class that provides Tokens using the HMACSHA384 Algorithm
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		Justification = "The case is intended to match that of the related Microsoft.Net class.")]
	public class HMACSHA384TokenProvider : TokenProvider<HMACSHA384> { }

	/// <summary>
	/// Token Provider class that provides Tokens using the HMACSHA512 Algorithm
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		Justification = "The case is intended to match that of the related Microsoft.Net class.")]
	public class HMACSHA512TokenProvider : TokenProvider<HMACSHA512> { }

	/// <summary>
	/// Token Provider class that provides Tokens using the MACTripleDES Algorithm
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
		Justification = "The case is intended to match that of the related Microsoft.Net class.")]
	public class MACTripleDESTokenProvider : TokenProvider<MACTripleDES> { }
}
