using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace System
{
	/// <summary>
	/// Provides a singlton instance for the type specified.
	/// </summary>
	/// <typeparam name="T">The type of singleton.</typeparam>
	[SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors",
		Justification = "This class is intended to be derived, the protected constructor is necessary for derived classes.")]
	public class Singleton<T> where T : class
	{
		/// <summary>
		/// Used for preventing multiple threads from causing multiple intializations of the Singleton.
		/// </summary>
		private static object lockObject = new object();
		/// <summary>
		/// Stores the singleton instance of the type specified.
		/// </summary>
		private static T _instance = null;
		/// <summary>
		/// Gets the singleton instance of the type specified.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
			Justification = "I is an abreviation for Instance, I is used to make consuming code more terse.")]
		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
			Justification = "The idea behind a singleton is that there can be only one.  Singleton objects may only have a private constructor.")]
		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
			Justification = "The static member is necessary in order to expose the singleton instance of T.")]
		public static T I
		{
			get
			{
				if (_instance == null)
				{
					lock (lockObject)
					{
						// Recheck in case another thread caused the singleton to be intialized while this thread was waiting for the lock.
						if (_instance == null)
						{
							Type t = typeof(T);
							BindingFlags flags = (BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
							_instance = t.InvokeMember(t.Name, flags, null, null, null, CultureInfo.InvariantCulture) as T;
						}
					}
				}
				return _instance;
			}
		}

		/// <summary>
		/// Default Constructor.
		/// </summary>
		protected Singleton() { }
	}
}
