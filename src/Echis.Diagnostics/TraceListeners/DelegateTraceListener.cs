using System;
using System.Collections.Generic;
using System.Text;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// This trace listener calls a delegate method when an item is written to the trace device.
	/// </summary>
	public sealed class DelegateTraceListener : ThreadTraceListener
	{
		/// <summary>
		/// Stores the reference to method to call when a message is to be written.
		/// </summary>
		private WriteTraceMessage _messageWriteMethod;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="messageWriteMethod">The method which will be called when a message is written to the trace listener.</param>
		public DelegateTraceListener(WriteTraceMessage messageWriteMethod) : this(messageWriteMethod, null) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="messageWriteMethod">The method which will be called when a message is written to the trace listener.</param>
		/// <param name="name">The name of the Trace Listener</param>
		public DelegateTraceListener(WriteTraceMessage messageWriteMethod, string name) : base(name) 
		{
			if (messageWriteMethod == null) throw new ArgumentNullException("messageWriteMethod");
			_messageWriteMethod = messageWriteMethod;
		}

		/// <summary>
		/// String builder used to assemble a message line when the Write method is called.
		/// </summary>
		private StringBuilder _message;

		/// <summary>
		/// Appends a message to the end of a message line.
		/// </summary>
		/// <param name="message">The message to be appended.</param>
		public override void Write(string message)
		{
			if (_message == null)
			{
				_message = new StringBuilder();
			}
			_message.Append(message);
		}

		/// <summary>
		/// Calls the delegate method with the completed message.
		/// </summary>
		/// <param name="message">The message to be sent to the delegate.</param>
		public override void WriteLine(string message)
		{
			if (_message == null)
			{
				_messageWriteMethod.Invoke(message);
			}
			else
			{
				_message.Append(message);
				_messageWriteMethod(_message.ToString());
				_message = null;
			}
		}

		/// <summary>
		/// Flushes the trace listener, sending any incompleted message to the delegate.
		/// </summary>
		public override void Flush()
		{
			base.Flush();
			if (_message != null)
			{
				_messageWriteMethod.Invoke(_message.ToString());
			}
		}

		/// <summary>
		/// Not used.
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="paramValue"></param>
		protected override void SetParameter(string paramName, string paramValue)
		{
		}
	}

	/// <summary>
	/// Method delegate used to write trace messages.
	/// </summary>
	/// <param name="message">The message which was sent through the trace device.</param>
	public delegate void WriteTraceMessage(string message);
}
