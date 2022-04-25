using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace System.Data
{
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
		Justification = "This class is instantiated via reflection when used.")]
	internal sealed class Decryptor
	{
		/// <summary>
		/// The default ObjectId for the Decryption Provider.
		/// </summary>
		private const string DecryptionProviderObjectId = "System.Data.DecryptionProvider";

		/// <summary>
		/// Stores the singleton instance of the DecryptionProvider.
		/// </summary>
		private static IDecryptionProvider _instance;
		/// <summary>
		/// Gets the singleton instance of the DecryptionProvider.
		/// </summary>
		public static IDecryptionProvider Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = IOC.GetFrameworkObject<IDecryptionProvider>(Settings.Values.DecryptionProvider, DecryptionProviderObjectId, false);
					if (_instance == null)
					{
						_instance = new DefaultCryptographyProvider();
					}
				}
				return _instance;
			}
		}
	}
}
