using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Security
{
	/// <summary>
	/// Defines methods used to encrypt strings
	/// </summary>
	public interface IEncryptionProvider
	{
		/// <summary>
		/// Encrypts a string value.
		/// </summary>
		/// <param name="value">The string to be encrypted.</param>
		/// <returns>Returns the encrypted string.</returns>
		string EncryptString(string value);
	}
}
