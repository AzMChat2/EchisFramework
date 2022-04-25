using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Security
{
	/// <summary>
	/// Defines methods for Encrypting and Decrypting strings.
	/// </summary>
	public interface ICryptographyProvider : IEncryptionProvider, IDecryptionProvider
	{
	}
}
