using System.Collections.Generic;
using System.Diagnostics;

using System.Diagnostics.TraceListeners;
using System.ServiceModel;

namespace System.Diagnostics.Loggers.Service
{
	/// <summary>
	/// Provides access to the Trace Registry Service
	/// </summary>
	public class RegistryClient : ReliableClientBase<IRegistryService>, IRegistryService
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public RegistryClient() : base("System.Loggers.Registry") { }

		/// <summary>
		/// Gets a string array containing the names of all machines currently connected to the trace registry.
		/// </summary>
		/// <returns>String Array containing machine names.</returns>
		public string[] GetMachineNames()
		{
			return Service.GetMachineNames();
		}

		/// <summary>
		/// Gets a string array containing the names of all processes currently connected to the trace registry for the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine for which to retrieve the processes connected.</param>
		/// <returns>A string array contianing process names.</returns>
		public string[] GetProcessNames(string machineName)
		{
			return Service.GetProcessNames(machineName);
		}

		/// <summary>
		/// Gets a string array containing the names of all threads for the specified process currently connected to the trace registry for the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine for which to retrieve the processes connected to the trace registry.</param>
		/// <param name="processName">The name of the process for which to retrieve the threads connected to the trace registry.</param>
		/// <returns>A string array containing thread names.</returns>
		public string[] GetThreadNames(string machineName, string processName)
		{
			return Service.GetThreadNames(machineName, processName);
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
			Service.SetTraceLevel(machineName, processName, contextId, threadName, level);
		}

		/// <summary>
		/// Gets the Tracing level for the specified thread on the specified process running on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The name of the process in which the thread is running.</param>
		/// <param name="contextId">The Diagnostics Context.</param>
		/// <param name="threadName">The name of the thread.</param>
		/// <returns>Returns the current trace level of the specified machine and process.</returns>
		public TraceLevel GetTraceLevel(string machineName, string processName, string contextId, string threadName)
		{
			return Service.GetTraceLevel(machineName, processName, contextId, threadName);
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
			return Service.GetTraceListeners(machineName, processName, threadName);
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
			Service.RemoveTraceListener(machineName, processName, threadName, listenerName);
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
			Service.AddTraceListener(machineName, processName, threadName, listenerInfo);
		}
	}
}
