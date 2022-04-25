using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.Diagnostics.Loggers.Service;
using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.Loggers.Registry
{
	/// <summary>
	/// Provides WCF remote access to Tracing objects.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
		Justification = "ManagerClient is disposing when Unregister() is called")]
	public class RemoteRegistry : RegistryBase, IRemoteRegistry, IDisposable
	{
		/// <summary>
		/// Finalizer.
		/// </summary>
		~RemoteRegistry()
		{
			Dispose(false);
		}

		/// <summary>
		/// The remote Manager from the Logger Service.
		/// </summary>
		private IManagerService _manager;

		/// <summary>
		/// Connects to the Remote Manager and registers this Trace Registry.
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification="This is an initialization method for the class. Disposing of disposable objects is handled by the Dispose method.")]
		public override void Initialize()
		{
			try
			{
				_manager = new ManagerClient(this);
				_manager.Register();
			}
			catch (CommunicationException ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
			}
		}

		/// <summary>
		/// Disconnects from the Remote Manager.
		/// </summary>
		public override void Shutdown()
		{
			try
			{
				if (_manager != null)
				{
					_manager.Unregister();
				}
			}
			catch (CommunicationException ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
			}
			finally
			{
				_manager = null;
			}
		}

		/// <summary>
		/// Gets the machine name on which the process is executing.
		/// </summary>
		public string GetMachineName()
		{
			return MachineName;
		}

		/// <summary>
		/// Gets the process name on which the thread is running.
		/// </summary>
		public string GetProcessName()
		{
			return ProcessName;
		}
	
		/// <summary>
		/// Gets information on all of the threads registered in this Trace Registry.
		/// </summary>
		/// <returns></returns>
		public List<string> GetThreads()
		{
			return ThreadList;
		}

		/// <summary>
		/// Adds a Trace Listener to the Trace Listener's Collection.
		/// </summary>
		/// <param name="listenerInfo">Data object containing the information needed to create the trace listener.</param>
		/// <param name="threadName">The name of the thread to which the listener is assigned.</param>
		public void AddTraceListener(string threadName, TraceListenerInfo listenerInfo)
		{
			TS.AddTraceListener(threadName, listenerInfo);
		}

		/// <summary>
		/// Removes a Trace Listener from the Trace Listener's Collection.
		/// </summary>
		/// <param name="listenerName">The name of the Trace Listener.</param>
		public void RemoveTraceListener(string listenerName)
		{
			Trace.Listeners.Remove(listenerName);
		}

		/// <summary>
		/// Sets the Standard Trace messages device.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly containing the Standard Message device.</param>
		/// <param name="className">The class name of the Standard Message device.</param>
		public void SetStandardMessages(string assemblyName, string className)
		{
			var sm = ReflectionExtensions.CreateObjectUnsafe<LoggerBase>(assemblyName, className);

			if (sm != null)
			{
				// Output previous performance stats so that they are not lost.
				TS.Logger.OutputPerformanceStats();
				TS.Logger = sm;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				IDisposable disposable = _manager as IDisposable;
				if (disposable != null) disposable.Dispose();
			}
		}
	}
}
