using System.ServiceModel;
using System.ServiceModel.Web;

namespace System.ServiceModel
{
	/// <summary>
	/// Provides a Reliable Duplex Connection to a Remote Service
	/// </summary>
	/// <typeparam name="TChannel">The Service Interface which the Remote Service implements.</typeparam>
	public abstract class ReliableWebClientBase<TChannel> : ReliableClientBase<TChannel> where TChannel : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configName">The End Point Configuration name to be used to connect to the service.</param>
		protected ReliableWebClientBase(string configName) : base(configName) { }

		/// <summary>
		/// Creats a new Channel Factory object.
		/// </summary>
		/// <returns></returns>
		protected override ChannelFactory<TChannel> CreateChannelFactory()
		{
			return new WebChannelFactory<TChannel>(ConfigName);
		}
	}
}
