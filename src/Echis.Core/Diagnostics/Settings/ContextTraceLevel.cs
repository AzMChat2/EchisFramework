using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLevels = System.Diagnostics.TraceLevel;
using System.Xml.Serialization;

namespace System.Diagnostics
{
	/// <summary>
	/// The Context Trace Level class is used to store Context specific Trace Level information.
	/// </summary>
	public sealed class ContextTraceLevel
	{
		/// <summary>
		/// Gets the Context Id.
		/// </summary>
		[XmlAttribute]
		public string ContextId { get; set; }

		/// <summary>
		/// Gets the Trace Level
		/// </summary>
		[XmlAttribute]
		public TraceLevels TraceLevel { get; set; }
	}
}
