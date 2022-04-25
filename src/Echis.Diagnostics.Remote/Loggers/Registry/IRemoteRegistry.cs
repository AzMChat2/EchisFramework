using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;

using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.Loggers.Registry
{
	/// <summary>
	/// Provides the service interface used for a Remote Trace Registry.
	/// </summary>
	[ServiceContract]
	public interface IRemoteRegistry 
	{
		/// <summary>
		/// Gets the machine name on which the process is executing.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "This is a WCF Service Operation, it is not possible to use a Property.")]
		[OperationContract]
		string GetMachineName();
	
		/// <summary>
		/// Gets the process name on which the thread is running.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "This is a WCF Service Operation, it is not possible to use a Property.")]
		[OperationContract]
		string GetProcessName();

		/// <summary>
		/// Gets information on all of the threads registered in this Trace Registry.
		/// </summary>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "This is a WCF Service Operation, it is not possible to use a Property.")]
		[OperationContract]
		List<string> GetThreads();

		/// <summary>
		/// Adds a Trace Listener to the Trace Listener's Collection.
		/// </summary>
		/// <param name="listenerInfo">Data object containing the information needed to create the trace listener.</param>
		/// <param name="threadName">The name of the thread to which the listener will be assigned.</param>
		[OperationContract(IsOneWay = true)]
		void AddTraceListener(string threadName, TraceListenerInfo listenerInfo);

		/// <summary>
		/// Removes a Trace Listener from the Trace Listener's Collection.
		/// </summary>
		/// <param name="listenerName">The name of the Trace Listener.</param>
		[OperationContract(IsOneWay = true)]
		void RemoveTraceListener(string listenerName);

		/// <summary>
		/// Gets the Trace Level for the specified thread.
		/// </summary>
		[OperationContract]
		TraceLevel GetTraceLevel(string contextId, string threadName);

		/// <summary>
		/// Sets the Trace Level for the specified thread.
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void SetTraceLevel(string contextId, string threadName, TraceLevel value);

		/// <summary>
		/// Retrieves information on all ThreadTraceListeners assigned to the specified thread.
		/// </summary>
		/// <param name="threadName">The Thread name to which the ThreadTraceListeners are assigned.</param>
		/// <returns>Returns a collection containing information about all ThreadTraceListeners assigned to the specified thread.</returns>
		[OperationContract]
		List<TraceListenerInfo> GetTraceListeners(string threadName);

		/// <summary>
		/// Sets the Standard Trace messages device.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly containing the Standard Message device.</param>
		/// <param name="className">The class name of the Standard Message device.</param>
		[OperationContract(IsOneWay = true)]
		void SetStandardMessages(string assemblyName, string className);

	}
}
