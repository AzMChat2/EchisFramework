using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// The Event Log Trace Listener class represents a Filtered Trace Listener which writes trace messages to the Windows Event Log.
	/// </summary>
	public class EventLogTraceListener : FilteredTraceListener
	{
		private StringBuilder _msgBuilder;
		private string _lastCategory;
		private bool _sourceChecked;

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public EventLogTraceListener()
		{
			LogName = "Application";
			LogSource = Assembly.GetEntryAssembly().FullName;
		}

		/// <summary>
		/// Gets or sets the name of the Windows Event Log to which entries will be written.
		/// </summary>
		/// <remarks>Default is the Application log.</remarks>
		public string LogName { get; set; }
		/// <summary>
		/// Gets or sets the name of the Windows Event Log Source used to write entries.
		/// </summary>
		public string LogSource { get; set; }

		/// <summary>
		/// Sets the specified initialization parameter to the specified value.
		/// </summary>
		/// <param name="paramName">The name of the specified initialization parameter.</param>
		/// <param name="paramValue">The new value for the the initialization parameter.</param>
		protected override void SetParameter(string paramName, string paramValue)
		{
			if (paramName == null) throw new ArgumentNullException("paramName");

			base.SetParameter(paramName, paramValue);

			if (paramName.Equals("LogName", StringComparison.OrdinalIgnoreCase))
			{
				LogName = paramValue;
			}
			else if (paramName.Equals("LogSource", StringComparison.OrdinalIgnoreCase))
			{
				LogSource = paramValue;
			}
		}

		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected override void WriteMessage(string message, string category)
		{
			if (category == null) category = string.Empty;

			if (_msgBuilder == null)
			{
				_msgBuilder = new StringBuilder();
				_msgBuilder.Append(message);
			}
			else if (category.Equals(_lastCategory, StringComparison.OrdinalIgnoreCase))
			{
				_msgBuilder.Append(message);
			}
			else
			{
				WriteLogEntry(_msgBuilder.ToString(), _lastCategory);
				_msgBuilder = new StringBuilder();
				_msgBuilder.Append(message);
			}

			_lastCategory = category;
		}

		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected override void WriteMessageLine(string message, string category)
		{
			if (category == null) category = string.Empty;

			if (_msgBuilder == null)
			{
				WriteLogEntry(message, category);
			}
			else if (category.Equals(_lastCategory, StringComparison.OrdinalIgnoreCase))
			{
				_msgBuilder.Append(message);
				WriteLogEntry(_msgBuilder.ToString(), category);
			}
			else
			{
				WriteLogEntry(_msgBuilder.ToString(), _lastCategory);
				WriteLogEntry(message, category);
			}

			_msgBuilder = null;
			category = null;
		}

		/// <summary>
		/// Writes a trace message to the Windows EventLog.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification="Multiple exceptions may be caught, no action required for any of them.")]
		protected virtual void WriteLogEntry(string message, string category)
		{
			if (message == null) throw new ArgumentNullException("message");
			if (category == null) category = string.Empty;

			try
			{
				if (!_sourceChecked)
				{
					if (!EventLog.SourceExists(LogSource))
					{
						EventLog.CreateEventSource(LogSource, LogName);
					}
					_sourceChecked = true;
				}

				EventLogEntryType entryType = EventLogEntryType.Information;
				if (category.Equals(TS.Categories.Error, StringComparison.OrdinalIgnoreCase))
				{
					entryType = EventLogEntryType.Error;
				}
				else if (category.Equals(TS.Categories.Warning, StringComparison.OrdinalIgnoreCase))
				{
					entryType = EventLogEntryType.Warning;
				}

				EventLog.WriteEntry(LogSource, message, entryType);
			}
			catch { }
		}

		/// <summary>
		/// Writes any remaining messages to the EventLog.
		/// </summary>
		public override void Flush()
		{
			base.Flush();

			if (_msgBuilder != null)
			{
				WriteLogEntry(_msgBuilder.ToString(), _lastCategory);
				_msgBuilder = null;
			}
		}
	}
}
