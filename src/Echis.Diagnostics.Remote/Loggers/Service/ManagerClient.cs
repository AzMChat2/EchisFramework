using System.ServiceModel;
using System.Diagnostics.Loggers.Registry;

namespace System.Diagnostics.Loggers.Service
{
	/// <summary>
	/// Provides access to the Manager Service.
	/// </summary>
	public sealed class ManagerClient : ReliableDuplexClientBase<IManagerService, IRemoteRegistry>, IManagerService
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="client">The client object which is a consumer of the Manager Service.</param>
		public ManagerClient(IRemoteRegistry client) : base("System.Loggers.Manager", client) { }

		/// <summary>
		/// Registers a client Trace Registry with the Trace Registry Service
		/// </summary>
		public void Register()
		{
			Service.Register();
		}

		/// <summary>
		/// Unregisters a client from the Trace Registry Service
		/// </summary>
		public void Unregister()
		{
			Service.Unregister();
		}

		/// <summary>
		/// Closes the Channel Factory.
		/// </summary>
		protected override void Close()
		{
			try
			{
				if (ChannelFactory.State == CommunicationState.Opened) Unregister();
			}
			catch (CommunicationException) { }
			finally
			{
				base.Close();
			}
		}
	}
}
