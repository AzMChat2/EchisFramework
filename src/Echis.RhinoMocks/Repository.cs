using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Mocks;
using System.Diagnostics.CodeAnalysis;

namespace System.RhinoMocks
{
	/// <summary>
	/// Creates and manages proxied instances of types.
	/// </summary>
	public static class Repository
	{
		/// <summary>
		/// Gets the current Mock Repository Instance.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
			Justification = "I is an abreviation for Instance, I is used to make consuming code more terse.")]
		public static MockRepository I { get; private set; }

		/// <summary>
		/// Static Constructor.
		/// </summary>
		static Repository()
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new Mock Repository.
		/// </summary>
		private static void Initialize()
		{
			I = new MockRepository();
		}

		/// <summary>
		/// Resets the Mock Repository for another unit test.
		/// </summary>
		/// <remarks>This method should be called from the TestInitialize method.</remarks>
		public static void Reset()
		{
			Initialize();
			MockDataAccess.Initialize();
		}
	}
}
