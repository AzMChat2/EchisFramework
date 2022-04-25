using System;
using System.Collections.Generic;
using System.Text;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// The TextTraceListener builds a text message from the trace output which may be read at any time.
	/// </summary>
	public class TextTraceListener : FilteredTraceListener
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public TextTraceListener()
		{
			Message = new StringBuilder();
		}

		/// <summary>
		/// The stringbuilder used to build and store the trace messages.
		/// </summary>
		protected StringBuilder Message { get; private set; }

		/// <summary>
		/// Clears all messages currently written to the listener.
		/// </summary>
		public void Clear()
		{
			Message = new StringBuilder();
		}

		/// <summary>
		/// Gets all trace messages sent to the listener since it's creation or the last time it was cleared.
		/// </summary>
		public string Text
		{
			get { return Message.ToString(); }
		}

		/// <summary>
		/// Flag indicating that we are currently building a line of text.
		/// </summary>
		private bool buildingLine;

		/// <summary>
		/// Appends a message to the Listener's Text value.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The category of the message to be written.</param>
		protected override void WriteMessage(string message, string category)
		{
			if (!buildingLine)
			{
				Message.Append(category);
				Message.Append(": ");
				buildingLine = true;
			}
			Message.Append(message);
		}

		/// <summary>
		/// Appends a message and new line to the Listener's Text value.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The category of the message to be written.</param>
		protected override void WriteMessageLine(string message, string category)
		{
			if (!buildingLine)
			{
				Message.Append(category);
				Message.Append(": ");
			}
			Message.AppendLine(message);
			buildingLine = false;
		}
	}
}
