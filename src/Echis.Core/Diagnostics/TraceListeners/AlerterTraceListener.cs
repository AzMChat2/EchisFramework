using System;
using System.Diagnostics;

namespace System.Diagnostics.TraceListeners
{
	internal class AlerterTraceListener : TraceListener
	{
		public event EventHandler Closing;
		public override void Write(string message) { }
		public override void WriteLine(string message) { }
		public override void Close()
		{
			if (Closing != null) Closing.Invoke(this, new EventArgs());
			base.Close();
		}
	}
}
