using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

using System.Diagnostics.TraceListeners;

using TraceLevels = System.Diagnostics.TraceLevel;

namespace System.Diagnostics
{
	/// <summary>
	/// The Thread Trace Level class is used to store Thread specific Trace Level and Listener information.
	/// </summary>
	[Serializable]
	public sealed class ThreadTraceLevel
	{
		#region Properties
		/// <summary>
		/// Gets the Thread Name.
		/// </summary>
		[XmlAttribute]
		public string ThreadName { get; set; }

		/// <summary>
		/// Gets the Trace Level
		/// </summary>
		[XmlAttribute]
		public TraceLevels TraceLevel { get; set; }

		//NOTE: The XmlSerializer chokes if we use { get; private set;} on List properties,
		//      but works fine if we use an underlying variable (e.g. _traceListeners, _contextLevels).

		/// <summary>
		/// Stores the list of Trace Listeners.
		/// </summary>
		private List<TraceListenerInfo> _traceListeners = new List<TraceListenerInfo>();
		/// <summary>
		/// Gets the list of Trace Listeners.
		/// </summary>
		[XmlElement("TraceListener")]
		public List<TraceListenerInfo> TraceListeners { get { return _traceListeners; } }

		/// <summary>
		/// Stores the list of Context Trace Levels
		/// </summary>
		private List<ContextTraceLevel> _contextLevels = new List<ContextTraceLevel>();
		/// <summary>
		/// Gets the list of Context Trace Levels
		/// </summary>
		[XmlElement("ContextLevel")]
		public List<ContextTraceLevel> ContextLevels { get { return _contextLevels; } }
		#endregion

	}
}