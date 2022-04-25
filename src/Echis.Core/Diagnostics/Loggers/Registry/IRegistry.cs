using System.Collections.Generic;
using System.Diagnostics;

using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.Loggers.Registry
{
	/// <summary>
	/// Provides methods used to register and unregister the current thread from a Logger Registry.
	/// </summary>
	public interface IRegistry
	{
		/// <summary>
		/// Gets the machine name on which the process is executing.
		/// </summary>
		string MachineName { get; }
		/// <summary>
		/// Gets the process name on which the thread is running.
		/// </summary>
		string ProcessName { get; }

		/// <summary>
		/// Called during application startup to initialize the Logger Registry.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Called during application shutdown to close the Logger Registry.
		/// </summary>
		void Shutdown();

		/// <summary>
		/// Registers the current Thread in the Logger Registry
		/// </summary>
		void Register(string threadName);

		/// <summary>
		/// Removes the specified Thread from the Logger Registry
		/// </summary>
		/// <param name="threadName">The name of the Thread to be removed from the Logger Registry.</param>
		void Unregister(string threadName);

		/// <summary>
		/// Gets the contexts for the specified thread.
		/// </summary>
		/// <param name="threadName">The name of the Thread to which the contexts belong.</param>
		/// <returns>Returns an array of context Id's for the specified thread.</returns>
		string[] GetContexts(string threadName);

		/// <summary>
		/// Gets the Trace Level for the specified thread.
		/// </summary>
		/// <param name="contextId">The Diagnostics Context Id.</param>
		/// <param name="threadName">The Thread Name.</param>
		TraceLevel GetTraceLevel(string contextId, string threadName);

		/// <summary>
		/// Sets the Trace Level for the specified thread.
		/// </summary>
		/// <param name="contextId">The Diagnostics Context Id.</param>
		/// <param name="threadName">The Thread Name.</param>
		/// <param name="value">The new TraceLevel.</param>
		void SetTraceLevel(string contextId, string threadName, TraceLevel value);

		/// <summary>
		/// Retrieves information on all ThreadTraceListeners assigned to the specified thread.
		/// </summary>
		/// <param name="threadName">The Thread name to which the ThreadTraceListeners are assigned.</param>
		/// <returns>Returns a collection containing information about all ThreadTraceListeners assigned to the specified thread.</returns>
		List<TraceListenerInfo> GetTraceListeners(string threadName);
	}
}
