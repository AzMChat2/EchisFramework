using System.ServiceModel;

namespace System.ServiceModel
{
	/// <summary>
	/// Provides a Reliable Duplex Connection to a Remote Service
	/// </summary>
	/// <typeparam name="TChannel">The Service Interface which the Remote Service implements.</typeparam>
	/// <typeparam name="TClientContext">The Client Interface which will be consuming the Remote Service.</typeparam>
	public abstract class ReliableDuplexClientBase<TChannel, TClientContext> : ReliableClientBase<TChannel> where TChannel : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configName">The End Point Configuration name to be used to connect to the service.</param>
		/// <param name="client">The Client which is consuming the Remote Service.</param>
		protected ReliableDuplexClientBase(string configName, TClientContext client) : base(configName)
		{
			ClientContext = new InstanceContext(client);
		}

		/// <summary>
		/// Creats a new Channel Factory object.
		/// </summary>
		/// <returns></returns>
		protected override ChannelFactory<TChannel> CreateChannelFactory()
		{
			return new DuplexChannelFactory<TChannel>(ClientContext, ConfigName);
		}

		/// <summary>
		/// Gets the instance context for the duplex channel operation.
		/// </summary>
		protected InstanceContext ClientContext { get; private set; }
	}
}
