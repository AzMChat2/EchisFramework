using System.Diagnostics;

namespace System.Diagnostics.Loggers
{
	/// <summary>
	/// Bases class for all System.Diagnostics.Trace based loggers
	/// </summary>
	public abstract class TraceLoggerBase : LoggerBase
	{

		/// <summary>
		/// Flushes the logging output buffer and causes all unwritten data to be written.
		/// </summary>
		public override void Flush()
		{
			Trace.Flush();
		}

		/// <summary>
		/// Writes the message to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		public override void Write(string message)
		{
			Trace.Write(message);
		}

		/// <summary>
		/// Writes the message and category to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The logging category for the message.</param>
		public override void Write(string category, string message)
		{
			Trace.Write(message, category);
		}

		/// <summary>
		/// Writes the message line to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		public override void WriteLine(string message)
		{
			Trace.WriteLine(message);
		}

		/// <summary>
		/// Writes the message and category line to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The logging category for the message.</param>
		protected override void WriteMessage(string category, string message)
		{
			Trace.WriteLine(message, category);
		}
	}
}
