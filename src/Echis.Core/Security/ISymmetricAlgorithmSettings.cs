
using System.Diagnostics.CodeAnalysis;
namespace System.Security
{
  /// <summary>
  /// Defines values used to initialize a Symmetric Algorithm Cryptography Provider
  /// </summary>
  public interface ISymmetricAlgorithmSettings
  {
    /// <summary>
    /// Gets the Algorithm Name used to encrypt or decrypt data.
    /// </summary>
    string AlgorithmName { get; }
    /// <summary>
    /// Gets the Cryptography Key for the Symmetric Algorithm
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
      Justification = "Symmetric Algorithm Key is byte array.")]
    byte[] CryptoKey { get; }
    /// <summary>
    /// Gets the Cryptography IV for the Symmetric Algorithm
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
      Justification = "Symmetric Algorithm IV is byte array.")]
    byte[] CryptoIV { get; }
  }
}
