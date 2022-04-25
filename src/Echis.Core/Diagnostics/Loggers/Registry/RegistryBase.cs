using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.Loggers.Registry
{

	/// <summary>
	/// Provides a base implementation of the LoggerRegistry.
	/// </summary>
	public abstract class RegistryBase : IRegistry
	{
		/// <summary>
		/// Called during application startup to initialize the Logger Registry.
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// Called during application shutdown to close the Logger Registry.
		/// </summary>
		public abstract void Shutdown();

		/// <summary>
		/// Gets the collection of ThreadInfo objects which have been registered with the Logger Registry.
		/// </summary>
		protected List<string> ThreadList { get; private set; }

		/// <summary>
		/// Default Constructor.
		/// </summary>
		protected RegistryBase()
		{
			ThreadList = new List<string>();
			ProcessName = Process.GetCurrentProcess().ProcessName;
			MachineName = Environment.MachineName;
		}

		/// <summary>
		/// Gets the process name on which the thread is running.
		/// </summary>
		public string ProcessName { get; private set; }
		/// <summary>
		/// Gets the machine name on which the process is executing.
		/// </summary>
		public string MachineName { get; private set; }

		/// <summary>
		/// Registers the current Thread in the Logger Registry
		/// </summary>
		public virtual void Register(string threadName)
		{
			lock (ThreadList)
			{
				ThreadList.Add(threadName);
			}
		}

		/// <summary>
		/// Removes the specified Thread from the Logger Registry
		/// </summary>
		/// <param name="threadName">The name of the Thread to be removed from the Logger Registry.</param>
		public virtual void Unregister(string threadName)
		{
			lock (ThreadList)
			{
				if (ThreadList.Contains(threadName))
				{
					ThreadList.Remove(threadName);
				}
			}
		}

		/// <summary>
		/// Gets the contexts for the specified thread.
		/// </summary>
		/// <param name="threadName">The name of the Thread to which the contexts belong.</param>
		/// <returns>Returns an array of context Id's for the specified thread.</returns>
		public string[] GetContexts(string threadName)
		{
			return TS.GetContexts(threadName).ToArray();
		}

		/// <summary>
		/// Gets the Trace Level for the specified thread.
		/// </summary>
		public virtual TraceLevel GetTraceLevel(string contextId, string threadName)
		{
			return TS.GetTraceSwitch(contextId, threadName).Level;
		}

		/// <summary>
		/// Sets the Trace Level for the specified thread.
		/// </summary>
		public virtual void SetTraceLevel(string contextId, string threadName, TraceLevel value)
		{
			TS.GetTraceSwitch(contextId, threadName).Level = value;
		}

		/// <summary>
		/// Retrieves information on all ThreadTraceListeners assigned to the specified thread.
		/// </summary>
		/// <param name="threadName">The Thread name to which the ThreadTraceListeners are assigned.</param>
		/// <returns>Returns a collection containing information about all ThreadTraceListeners assigned to the specified thread.</returns>
		public virtual List<TraceListenerInfo> GetTraceListeners(string threadName)
		{
			List<TraceListenerInfo> retVal = new List<TraceListenerInfo>();

			foreach (TraceListener listener in Trace.Listeners)
			{
				ThreadTraceListener SystemListener = listener as ThreadTraceListener;
				if ((SystemListener != null) && (SystemListener.ThreadName == threadName))
				{
					retVal.Add(new TraceListenerInfo(SystemListener));
				}
			}

			return retVal;
		}
	}
}
