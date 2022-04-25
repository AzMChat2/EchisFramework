using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// The Trace Listener Info class contains information about a trace listener.
	/// </summary>
	[Serializable]
	public sealed class TraceListenerInfo
	{
		#region Constructors
		/// <summary>
		/// Constructor used to create an Trace Listener Info object.
		/// </summary>
		public TraceListenerInfo()
		{
			Name = string.Empty;
			Listener = string.Empty;
			_parameters = new ParameterCollection();
		}

		/// <summary>
		/// Constructor used to generate a Trace Listener Info object about an existing ThreadTraceListener.
		/// </summary>
		/// <param name="listener">The ThreadTraceListener object from which the trace listener information will be gathered.</param>
		public TraceListenerInfo(ThreadTraceListener listener)
		{
			if (listener == null) throw new ArgumentNullException("listener");

			Type type = listener.GetType();
			Name = listener.Name;
			Listener = type.FullName;
			_parameters = listener.Parameters;
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the name of the trace listener.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or Sets the trace listener type name.
		/// </summary>
		[XmlAttribute]
		public string Listener { get; set; }

		//NOTE: The XmlSerializer chokes if we use { get; private set;} on the Parameters property,
		//      but works fine if we use an underlying variable (_parameters).
		/// <summary>
		/// Stores the list of Parameters.
		/// </summary>
		private ParameterCollection _parameters;
		/// <summary>
		/// Gets the Trace Listener's Parameter list.
		/// </summary>
		[XmlElement("Parameter")]
		public ParameterCollection Parameters { get { return _parameters; } }

		#endregion
	}
}
