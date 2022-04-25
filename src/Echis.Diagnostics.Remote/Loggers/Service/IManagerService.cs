using System.ServiceModel;

using System.Diagnostics.Loggers.Registry;

namespace System.Diagnostics.Loggers.Service
{
	/// <summary>
	/// Service contract for the Manager Service
	/// </summary>
	[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IRemoteRegistry))]
	public interface IManagerService
	{
		/// <summary>
		/// Registers a client Trace Monitor with the Remote Trace Server.
		/// </summary>
		[OperationContract]
		void Register();

		/// <summary>
		/// Unregisters a Trace Monitor with the Remote Trace Server.
		/// </summary>
		[OperationContract]
		void Unregister();
	}
}
