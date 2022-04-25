

namespace System.Diagnostics.Loggers.Registry
{
	/// <summary>
	/// Provides an implementation of the Trace Registry which is only available to the local process.
	/// </summary>
	public class LocalRegistry : RegistryBase
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public LocalRegistry() : base() { }

		/// <summary>
		/// Called during application startup to initialize the Trace Registry.
		/// </summary>
		public override void Initialize()
		{
		}

		/// <summary>
		/// Called during application shutdown to close the Trace Registry.
		/// </summary>
		public override void Shutdown()
		{
		}
	}
}
