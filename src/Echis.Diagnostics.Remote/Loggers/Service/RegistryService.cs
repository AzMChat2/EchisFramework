using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.Diagnostics.Loggers.Registry;
using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.Loggers.Service
{

	/// <summary>
	/// Provides access to information contained in the registered TraceRegistries.
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class RegistryService : IRegistryService
	{
		/// <summary>
		/// Contains constants used by the LoggerRegistryService
		/// </summary>
		internal static class Constants
		{
			/// <summary>
			/// Message indicating a communications failure.
			/// </summary>
			public const string MsgClientCommsFailed = "Communication with process '{0}' on machine '{1}' failed.\r\n{2}";
		}

		/// <summary>
		/// Gets a string array containing the names of all machines currently connected to the trace registry.
		/// </summary>
		/// <returns>String Array containing machine names.</returns>
		public string[] GetMachineNames()
		{
			return RegistryCollection.Instance.GetMachineNames();
		}

		/// <summary>
		/// Gets a string array containing the names of all processes currently connected to the trace registry for the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine for which to retrieve the processes connected.</param>
		/// <returns>A string array contianing process names.</returns>
		public string[] GetProcessNames(string machineName)
		{
			return RegistryCollection.Instance.GetProcessNames(machineName);
		}

		/// <summary>
		/// Gets a string array containing the names of all threads for the specified process currently connected to the trace registry for the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine for which to retrieve the processes connected to the trace registry.</param>
		/// <param name="processName">The name of the process for which to retrieve the threads connected to the trace registry.</param>
		/// <returns>A string array containing thread names.</returns>
		public string[] GetThreadNames(string machineName, string processName)
		{
			return RegistryCollection.Instance.GetThreadNames(machineName, processName);
		}

		/// <summary>
		/// Sets the Tracing level for the specified thread on the specified process running on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The name of the process in which the thread is running.</param>
		/// <param name="contextId">The Diagnostics Context for which the trace level will be set.</param>
		/// <param name="threadName">The name of the thread for which the trace level will be set.</param>
		/// <param name="level">The new trace level for the specified thread.</param>
		public void SetTraceLevel(string machineName, string processName, string contextId, string threadName, TraceLevel level)
		{
			IRemoteRegistry registry = RegistryCollection.Instance.GetLoggerRegistry(machineName, processName, threadName);

			try
			{
				if (registry != null)
				{
					registry.SetTraceLevel(contextId, threadName, level);
				}
			}
			catch (CommunicationException ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				RegistryCollection.Instance.Remove(registry);
				string msg = string.Format(CultureInfo.InvariantCulture, Constants.MsgClientCommsFailed, processName, machineName, ex.Message);
				throw new FaultException(msg);
			}
		}

		/// <summary>
		/// Gets the Tracing level for the specified thread on the specified process running on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The name of the process in which the thread is running.</param>
		/// <param name="threadName">The name of the thread.</param>
		/// <param name="contextId">The Diagnostics Context.</param>
		/// <returns>Returns the current trace level of the specified machine and process.</returns>
		public TraceLevel GetTraceLevel(string machineName, string processName, string contextId, string threadName)
		{
			TraceLevel retVal = TraceLevel.Off;

			IRemoteRegistry registry = RegistryCollection.Instance.GetLoggerRegistry(machineName, processName, threadName);

			try
			{
				if (registry != null)
				{
					retVal = registry.GetTraceLevel(contextId, threadName);
				}
			}
			catch (CommunicationException ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				RegistryCollection.Instance.Remove(registry);
				string msg = string.Format(CultureInfo.InvariantCulture, Constants.MsgClientCommsFailed, processName, machineName, ex.Message);
				throw new FaultException(msg);
			}

			return retVal;
		}

		/// <summary>
		/// Get a list of the names of all trace listeners for the specified thread.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The process to which a trace listener will be added.</param>
		/// <param name="threadName">The name of the thread used to access the Trace Monitor.</param>
		/// <returns>A string array containing the names of all trace listeners.  Returns null if there are no trace listeners.</returns>
		public List<TraceListenerInfo> GetTraceListeners(string machineName, string processName, string threadName)
		{
			List<TraceListenerInfo> retVal = null;

			IRemoteRegistry registry = RegistryCollection.Instance.GetLoggerRegistry(machineName, processName, threadName);

			try
			{
				if (registry != null)
				{
					retVal = registry.GetTraceListeners(threadName);
				}

			}
			catch (CommunicationException ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				RegistryCollection.Instance.Remove(registry);
				string msg = string.Format(CultureInfo.InvariantCulture, Constants.MsgClientCommsFailed, processName, machineName, ex.Message);
				throw new FaultException(msg);
			}

			return retVal;
		}

		/// <summary>
		/// Removes a Trace Listener from the specified process on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The process to which a trace listener will be added.</param>
		/// <param name="threadName">The name of the thread used to access the Trace Monitor.</param>
		/// <param name="listenerName">The name of the Trace Listener.</param>
		public void RemoveTraceListener(string machineName, string processName, string threadName, string listenerName)
		{
			IRemoteRegistry registry = RegistryCollection.Instance.GetLoggerRegistry(machineName, processName, threadName);

			try
			{
				if (registry != null)
				{
					registry.RemoveTraceListener(listenerName);
				}
			}
			catch (CommunicationException ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				RegistryCollection.Instance.Remove(registry);
				string msg = string.Format(CultureInfo.InvariantCulture, Constants.MsgClientCommsFailed, processName, machineName, ex.Message);
				throw new FaultException(msg);
			}
		}

		/// <summary>
		/// Adds a Trace Listener to the specified process on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The process to which a trace listener will be added.</param>
		/// <param name="threadName">The name of the thread used to access the Trace Monitor.</param>
		/// <param name="listenerInfo">Data object containing the information needed to create the trace listener.</param>
		public void AddTraceListener(string machineName, string processName, string threadName, TraceListenerInfo listenerInfo)
		{
			IRemoteRegistry registry = RegistryCollection.Instance.GetLoggerRegistry(machineName, processName, threadName);

			try
			{
				if (registry != null)
				{
					registry.AddTraceListener(threadName, listenerInfo);
				}
			}
			catch (CommunicationException ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				RegistryCollection.Instance.Remove(registry);
				string msg = string.Format(CultureInfo.InvariantCulture, Constants.MsgClientCommsFailed, processName, machineName, ex.Message);
				throw new FaultException(msg);
			}
		}
	}
}
