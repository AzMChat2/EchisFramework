using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;

using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.Loggers.Service
{
	/// <summary>
	/// Service contract for the Logger Service
	/// </summary>
	[ServiceContract]
	public interface IRegistryService
	{
		/// <summary>
		/// Gets a string array containing the names of all machines currently connected to the trace registry.
		/// </summary>
		/// <returns>String Array containing machine names.</returns>
		[OperationContract]
		string[] GetMachineNames();

		/// <summary>
		/// Gets a string array containing the names of all processes currently connected to the trace registry for the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine for which to retrieve the processes connected.</param>
		/// <returns>A string array contianing process names.</returns>
		[OperationContract]
		string[] GetProcessNames(string machineName);

		/// <summary>
		/// Gets a string array containing the names of all threads for the specified process currently connected to the trace registry for the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine for which to retrieve the processes connected to the trace registry.</param>
		/// <param name="processName">The name of the process for which to retrieve the threads connected to the trace registry.</param>
		/// <returns>A string array containing thread names.</returns>
		[OperationContract]
		string[] GetThreadNames(string machineName, string processName);

		/// <summary>
		/// Sets the Tracing level for the specified thread on the specified process running on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The name of the process in which the thread is running.</param>
		/// <param name="contextId">The Diagnostics Context for which the trace level will be set.</param>
		/// <param name="threadName">The name of the thread for which the trace level will be set.</param>
		/// <param name="level">The new trace level for the specified thread.</param>
		[OperationContract]
		void SetTraceLevel(string machineName, string processName, string contextId, string threadName, TraceLevel level);

		/// <summary>
		/// Gets the Tracing level for the specified thread on the specified process running on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The name of the process in which the thread is running.</param>
		/// <param name="contextId">The Diagnostics Context.</param>
		/// <param name="threadName">The name of the thread.</param>
		/// <returns>Returns the current trace level of the specified machine and process.</returns>
		[OperationContract]
		TraceLevel GetTraceLevel(string machineName, string processName, string contextId, string threadName);

		/// <summary>
		/// Get a list of the names of all trace listeners for the specified thread.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The process to which a trace listener will be added.</param>
		/// <param name="threadName">The name of the thread used to access the Trace Monitor.</param>
		/// <returns>A string array containing the names of all trace listeners.  Returns null if there are no trace listeners.</returns>
		[OperationContract]
		List<TraceListenerInfo> GetTraceListeners(string machineName, string processName, string threadName);

		/// <summary>
		/// Removes a Trace Listener from the specified process on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The process to which a trace listener will be added.</param>
		/// <param name="threadName">The name of the thread used to access the Trace Monitor.</param>
		/// <param name="listenerName">The name of the Trace Listener.</param>
		[OperationContract]
		void RemoveTraceListener(string machineName, string processName, string threadName, string listenerName);

		/// <summary>
		/// Adds a Trace Listener to the specified process on the specified machine.
		/// </summary>
		/// <param name="machineName">The name of the machine on which the process is running.</param>
		/// <param name="processName">The process to which a trace listener will be added.</param>
		/// <param name="threadName">The name of the thread used to access the Trace Monitor.</param>
		/// <param name="listenerInfo">Data object containing the information needed to create the trace listener.</param>
		[OperationContract]
		void AddTraceListener(string machineName, string processName, string threadName, TraceListenerInfo listenerInfo);

	}
}
