using System.ServiceModel;

using System.Diagnostics.Loggers.Registry;

namespace System.Diagnostics.Loggers.Service
{
	/// <summary>
	/// Provides service methods for LoggerRegistry object to register and unregister from the Logger Service
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class ManagerService : IManagerService
	{
		/// <summary>
		/// Registers a client Trace Monitor with the Remote Trace Server.
		/// </summary>
		public void Register()
		{
			IRemoteRegistry registry = OperationContext.Current.GetCallbackChannel<IRemoteRegistry>();
			RegistryCollection.Instance.Add(registry);
		}

		/// <summary>
		/// Unregisters a Trace Monitor with the Remote Trace Server.
		/// </summary>
		public void Unregister()
		{
			IRemoteRegistry registry = OperationContext.Current.GetCallbackChannel<IRemoteRegistry>();
			RegistryCollection.Instance.Remove(registry);
		}
	}
}
