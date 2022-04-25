using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.Loggers.Registry
{
	/// <summary>
	/// Provides a default behavior for Logger Registry.
	/// </summary>
	/// <remarks>This class intentionally does nothing.  It is used only as a placeholder
	/// which is called when no other IRegistry implementation is configured.</remarks>
	internal class DefaultRegistry : IRegistry
	{
		/// <summary>
		/// Gets the machine name on which the process is executing.
		/// </summary>
		public string MachineName
		{
			get { return Environment.MachineName; }
		}

		/// <summary>
		/// Gets the process name on which the thread is running.
		/// </summary>
		public string ProcessName
		{
			get { return Process.GetCurrentProcess().ProcessName; }
		}

		/// <summary>
		/// Not Used.
		/// </summary>
		public void Initialize() { }
		/// <summary>
		/// Not Used.
		/// </summary>
		public void Shutdown() { }
		/// <summary>
		/// Not Used.
		/// </summary>
		public void Register(string threadName) { }
		/// <summary>
		/// Not Used.
		/// </summary>
		public void Unregister(string threadName) { }
		/// <summary>
		/// Not Used.
		/// </summary>
		public string[] GetContexts(string threadName) { return null; }
		/// <summary>
		/// Not Used.
		/// </summary>
		public TraceLevel GetTraceLevel(string context, string threadName) { return TraceLevel.Off; }
		/// <summary>
		/// Not Used.
		/// </summary>
		public void SetTraceLevel(string context, string threadName, TraceLevel value) { }
		/// <summary>
		/// Not Used.
		/// </summary>
		public List<TraceListenerInfo> GetTraceListeners(string threadName) { return null; }

	}
}
