using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace System.ServiceModel
{
	/// <summary>
	/// Provides a Reliable Connection to a Remote Service
	/// </summary>
	/// <typeparam name="TChannel">The Service Interface which the Remote Service implements.</typeparam>
	public abstract class ReliableClientBase<TChannel> : IDisposable where TChannel : class
	{
		/// <summary>
		/// Stores a value indicating if this object has been disposed.
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configName">The End Point Configuration name to be used to connect to the service.</param>
		protected ReliableClientBase(string configName)
		{
			ConfigName = configName;
		}
		/// <summary>
		/// Destructor.
		/// </summary>
		~ReliableClientBase()
		{
			Dispose(false);
		}

		/// <summary>
		/// Gets the End Point Configuration name to be used to connect to the service.
		/// </summary>
		protected string ConfigName { get; private set; }

		/// <summary>
		/// Stores the instantiated TChannel Service proxy object (Channel).
		/// </summary>
		private TChannel _service;
		/// <summary>
		/// Gets the TChannel Service proxy object (Channel).
		/// </summary>
		protected TChannel Service
		{
			get
			{
				CheckChannelFactory();
				if (_service == null)
				{
					_service = _channelFactory.CreateChannel();
				}
				return _service;
			}
		}

		/// <summary>
		/// Stores the instantiated Communications Channel Factory.
		/// </summary>
		private ChannelFactory<TChannel> _channelFactory;
		/// <summary>
		/// Gets the Communications Channel Factory.
		/// </summary>
		protected ChannelFactory<TChannel> ChannelFactory
		{
			get
			{
				CheckChannelFactory();
				return _channelFactory;
			}
		}

		/// <summary>
		/// Checks the Channel Factory to insure that it is instantiated and in a communications ready state.
		/// </summary>
		private void CheckChannelFactory()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("ReliableClientBase");
			}
			else
			{
				if ((_channelFactory != null) && (_channelFactory.State == CommunicationState.Faulted))
				{
					// Channel factory exists but is in a faulted state, so close it so we'll open a new one later
					Close();
				}

				if (_channelFactory == null)
				{
					// No channel factory exists, open one.
					_channelFactory = CreateChannelFactory();
				}
			}
		}

		/// <summary>
		/// Creats a new Channel Factory object.
		/// </summary>
		/// <returns></returns>
		protected virtual ChannelFactory<TChannel> CreateChannelFactory()
		{
			return new ChannelFactory<TChannel>(ConfigName);
		}

		/// <summary>
		/// Disposes all resources
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Closes the Channel Factory.
		/// </summary>
		protected virtual void Close()
		{
			if (_channelFactory != null)
			{
				try
				{
					if (_channelFactory.State == CommunicationState.Faulted)
					{
						_channelFactory.Abort();
					}
					else
					{
						_channelFactory.Close();
					}
				}
				catch (CommunicationException)
				{
					_channelFactory.Abort();
				}
				catch (TimeoutException)
				{
					_channelFactory.Abort();
				}
				catch (Exception)
				{
					_channelFactory.Abort();
					throw;
				}
				finally
				{
					_channelFactory = null;
					_service = null;
				}
			}
		}

		/// <summary>
		/// Disposes all resources.
		/// </summary>
		/// <param name="disposing"></param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Object is disposing, ignore any error")]
		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (!_disposed)
				{
					Close();
					ConfigName = null;
					_disposed = true;
				}
			}
			catch { }
		}
	}
}
