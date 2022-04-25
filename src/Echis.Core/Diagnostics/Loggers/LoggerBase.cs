using System.Globalization;
using System.Reflection;

namespace System.Diagnostics.Loggers
{
	/// <summary>
	/// Provides a base implementation of the StandardMessages interface.
	/// </summary>
	[DebuggerStepThrough]
	public abstract class LoggerBase
	{
		/// <summary>
		/// Flushes the logging output buffer and causes all unwritten data to be written.
		/// </summary>
		/// <remarks>Logger base has no implementation, derived classes should override to flush their logs.</remarks>
		[Conditional("TRACE")]
		public virtual void Flush() { }

		/// <summary>
		/// Writes the message to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		[Conditional("TRACE")]
		public abstract void Write(string message);
		/// <summary>
		/// Writes the message and category to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public abstract void Write(string category, string message);
		/// <summary>
		/// Writes the message to the logger, if the specified condition is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the message to be written.</param>
		/// <param name="message">The message to be written.</param>
		[Conditional("TRACE")]
		public virtual void WriteIf(bool condition, string message)
		{
			if (condition) Write(message);
		}

		/// <summary>
		/// Writes the message and category to the logger, if the specified condition is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the message to be written.</param>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WriteIf(bool condition, string category, string message)
		{
			if (condition) Write(category, message);
		}

		/// <summary>
		/// Writes the message line to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		[Conditional("TRACE")]
		public abstract void WriteLine(string message);
		/// <summary>
		/// Writes the message and category line to the logger.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		protected abstract void WriteMessage(string category, string message);

		/// <summary>
		/// Writes a formatted message and category line to the logger using the default Format Provider.
		/// </summary>
		/// <param name="category">The logging category for the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An Array containing zero or more objects to format.</param>
		[Conditional("TRACE")]
		public virtual void WriteLine(string category, string format, params object[] args)
		{
			WriteLine(category, CultureInfo.InvariantCulture, format, args);
		}

		/// <summary>
		/// Writes a formatted message and category line to the logger using the specified Format Provider.
		/// </summary>
		/// <param name="provider">A Format Provider which supplies culture-specific formatting information.</param>
		/// <param name="category">The logging category for the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An Array containing zero or more objects to format.</param>
		[Conditional("TRACE")]
		public virtual void WriteLine(string category, IFormatProvider provider, string format, params object[] args)
		{
			WriteMessage(category, string.Format(provider, format, args));
		}

		/// <summary>
		/// Writes the message line to the logger, if the specified condition is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the message to be written.</param>
		/// <param name="message">The message to be written.</param>
		[Conditional("TRACE")]
		public virtual void WriteLineIf(bool condition, string message)
		{
			if (condition) WriteLine(message);
		}

		/// <summary>
		/// Writes a formatted message and category line to the logger using the default Format Provider, if the specified condition is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the message to be written.</param>
		/// <param name="category">The logging category for the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An Array containing zero or more objects to format.</param>
		[Conditional("TRACE")]
		public virtual void WriteLineIf(bool condition, string category, string format, params object[] args)
		{
			if (condition) WriteLine(category, CultureInfo.InvariantCulture, format, args);
		}

		/// <summary>
		/// Writes a formatted message and category line to the logger using the specified Format Provider, if the specified condition is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the message to be written.</param>
		/// <param name="provider">A Format Provider which supplies culture-specific formatting information.</param>
		/// <param name="category">The logging category for the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An Array containing zero or more objects to format.</param>
		[Conditional("TRACE")]
		public virtual void WriteLineIf(bool condition, string category, IFormatProvider provider, string format, params object[] args)
		{
			if (condition) WriteLine(category, provider, format, args);
		}

		/// <summary>
		/// Writes a standard method call Trace message.
		/// </summary>
		[Conditional("TRACE")]
		public virtual void WriteMethodCall()
		{
			MethodBase mb = new StackFrame(1).GetMethod();
			WriteMethodCallMessage(mb, TS.Categories.Method);
		}
		/// <summary>
		/// Writes a standard method call Trace message.
		/// </summary>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WriteMethodCall(string category)
		{
			MethodBase mb = new StackFrame(1).GetMethod();
			WriteMethodCallMessage(mb, category);
		}

		/// <summary>
		/// Writes a standard exception Trace message.
		/// </summary>
		/// <param name="ex">The exception which has been caught.</param>
		[Conditional("TRACE")]
		public virtual void WriteException(Exception ex)
		{
			MethodBase mb = new StackFrame(1).GetMethod();
			WriteExceptionMessage(mb, ex, TS.Categories.Error);
		}
		/// <summary>
		/// Writes a standard exception Trace message.
		/// </summary>
		/// <param name="ex">The exception which has been caught.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WriteException(Exception ex, string category)
		{
			MethodBase mb = new StackFrame(1).GetMethod();
			WriteExceptionMessage(mb, ex, category);
		}

		/// <summary>
		/// Writes a standard performance Trace message and records performance information.
		/// </summary>
		/// <param name="elapsed">The elapsed time to be recorded.</param>
		[Conditional("TRACE")]
		public virtual void WritePerformance(TimeSpan elapsed)
		{
			MethodBase mb = new StackFrame(1).GetMethod();
			RecordPerformance(mb, elapsed);
			WritePerformanceMessage(mb, elapsed, TS.Categories.Performance);
		}
		/// <summary>
		/// Writes a standard performance Trace message and records performance information.
		/// </summary>
		/// <param name="elapsed">The elapsed time to be recorded.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WritePerformance(TimeSpan elapsed, string category)
		{
			MethodBase mb = new StackFrame(1).GetMethod();
			RecordPerformance(mb, elapsed);
			WritePerformanceMessage(mb, elapsed, category);
		}

		/// <summary>
		/// Records performance information.
		/// </summary>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		[Conditional("TRACE")]
		public virtual void RecordPerformance(TimeSpan elapsed)
		{
			MethodBase mb = new StackFrame(1).GetMethod();
			RecordPerformance(mb, elapsed);
		}

		/// <summary>
		/// Writes a standard method call Trace message, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		[Conditional("TRACE")]
		public virtual void WriteMethodCallIf(bool condition)
		{
			if (condition)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				WriteMethodCallMessage(mb, TS.Categories.Method);
			}
		}
		/// <summary>
		/// Writes a standard method call Trace message, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WriteMethodCallIf(bool condition, string category)
		{
			if (condition)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				WriteMethodCallMessage(mb, category);
			}
		}

		/// <summary>
		/// Writes a standard exception Trace message, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="ex">The exception which has been caught.</param>
		[Conditional("TRACE")]
		public virtual void WriteExceptionIf(bool condition, Exception ex)
		{
			if (condition)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				WriteExceptionMessage(mb, ex, TS.Categories.Error);
			}
		}
		/// <summary>
		/// Writes a standard exception Trace message, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="ex">The exception which has been caught.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WriteExceptionIf(bool condition, Exception ex, string category)
		{
			if (condition)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				WriteExceptionMessage(mb, ex, category);
			}
		}

		/// <summary>
		/// Writes a standard performance Trace message and records performance information, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, TimeSpan elapsed)
		{
			if (condition)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				WritePerformanceMessage(mb, elapsed, TS.Categories.Performance);
			}
		}
		/// <summary>
		/// Writes a standard performance Trace message and records performance information, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, TimeSpan elapsed, string category)
		{
			if (condition)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				WritePerformanceMessage(mb, elapsed, category);
			}
		}

		/// <summary>
		/// Writes a standard performance Trace message and records performance information, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="methodStart">The time the method call started</param>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, DateTime methodStart)
		{
			if (condition)
			{
				TimeSpan elapsed = DateTime.Now.Subtract(methodStart);
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				WritePerformanceMessage(mb, elapsed, TS.Categories.Performance);
			}
		}
		/// <summary>
		/// Writes a standard performance Trace message and records performance information, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="methodStart">The time the method call started</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, DateTime methodStart, string category)
		{
			if (condition)
			{
				TimeSpan elapsed = DateTime.Now.Subtract(methodStart);
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				WritePerformanceMessage(mb, elapsed, category);
			}
		}

		/// <summary>
		/// Writes a standard performance Trace message, if the condition is met and records performance information if the condition is met or alwaysRecord is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		/// <param name="alwaysRecord">A flag indicating if the performance information should be recorded, even if the condition is false</param>
		/// <remarks>If condition is false and alwaysRecord is true then the performance information will be recorded, but no trace message will be written.</remarks>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, TimeSpan elapsed, bool alwaysRecord)
		{
			if ((condition) || (alwaysRecord))
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				if (condition)
				{
					WritePerformanceMessage(mb, elapsed, TS.Categories.Performance);
				}
			}
		}
		/// <summary>
		/// Writes a standard performance Trace message, if the condition is met and records performance information if the condition is met or alwaysRecord is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		/// <param name="alwaysRecord">A flag indicating if the performance information should be recorded, even if the condition is false</param>
		/// <remarks>If condition is false and alwaysRecord is true then the performance information will be recorded, but no trace message will be written.</remarks>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, TimeSpan elapsed, bool alwaysRecord, string category)
		{
			if ((condition) || (alwaysRecord))
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				if (condition)
				{
					WritePerformanceMessage(mb, elapsed, category);
				}
			}
		}

		/// <summary>
		/// Writes a standard performance Trace message, if the condition is met and records performance information if the condition is met or alwaysRecord is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="methodStart">The time the method call started</param>
		/// <param name="alwaysRecord">A flag indicating if the performance information should be recorded, even if the condition is false</param>
		/// <remarks>If condition is false and alwaysRecord is true then the performance information will be recorded, but no trace message will be written.</remarks>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, DateTime methodStart, bool alwaysRecord)
		{
			if ((condition) || (alwaysRecord))
			{
				TimeSpan elapsed = DateTime.Now.Subtract(methodStart);
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				if (condition)
				{
					WritePerformanceMessage(mb, elapsed, TS.Categories.Performance);
				}
			}
		}
		/// <summary>
		/// Writes a standard performance Trace message, if the condition is met and records performance information if the condition is met or alwaysRecord is true.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="methodStart">The time the method call started</param>
		/// <param name="alwaysRecord">A flag indicating if the performance information should be recorded, even if the condition is false</param>
		/// <remarks>If condition is false and alwaysRecord is true then the performance information will be recorded, but no trace message will be written.</remarks>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public virtual void WritePerformanceIf(bool condition, DateTime methodStart, bool alwaysRecord, string category)
		{
			if ((condition) || (alwaysRecord))
			{
				TimeSpan elapsed = DateTime.Now.Subtract(methodStart);
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
				if (condition)
				{
					WritePerformanceMessage(mb, elapsed, category);
				}
			}
		}

		/// <summary>
		/// Records performance information, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		[Conditional("TRACE")]
		public virtual void RecordPerformanceIf(bool condition, TimeSpan elapsed)
		{
			if (condition)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
			}
		}

		/// <summary>
		/// Records performance information, if the condition is met.
		/// </summary>
		/// <param name="condition">A condition, which if true, will cause the Trace message to be written.</param>
		/// <param name="methodStart">The time the method call started</param>
		[Conditional("TRACE")]
		public virtual void RecordPerformanceIf(bool condition, DateTime methodStart)
		{
			if (condition)
			{
				TimeSpan elapsed = DateTime.Now.Subtract(methodStart);
				MethodBase mb = new StackFrame(1).GetMethod();
				RecordPerformance(mb, elapsed);
			}
		}

		/// <summary>
		/// Writes a standard method call Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public abstract void WriteMethodCallMessage(MethodBase mb, string category);
		/// <summary>
		/// Writes a standard exception Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="ex">The exception which has been caught.</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public abstract void WriteExceptionMessage(MethodBase mb, Exception ex, string category);
		/// <summary>
		/// Writes a standard performance Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		/// <param name="category">The logging category for the message.</param>
		[Conditional("TRACE")]
		public abstract void WritePerformanceMessage(MethodBase mb, TimeSpan elapsed, string category);
		/// <summary>
		/// Records performance information for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		[Conditional("TRACE")]
		public abstract void RecordPerformance(MethodBase mb, TimeSpan elapsed);
		/// <summary>
		/// Writes the collection of Performance data to the Trace output.
		/// </summary>
		[Conditional("TRACE")]
		public abstract void OutputPerformanceStats();

		/// <summary>
		/// Writes a standard method call Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		[Conditional("TRACE")]
		public void WriteMethodCallMessage(MethodBase mb)
		{
			WriteMethodCallMessage(mb, TS.Categories.Method);
		}

		/// <summary>
		/// Writes a standard exception Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="ex">The exception which has been caught.</param>
		[Conditional("TRACE")]
		public void WriteExceptionMessage(MethodBase mb, Exception ex)
		{
			WriteExceptionMessage(mb, ex, TS.Categories.Error);
		}

		/// <summary>
		/// Writes a standard performance Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		[Conditional("TRACE")]
		public void WritePerformanceMessage(MethodBase mb, TimeSpan elapsed)
		{
			WritePerformanceMessage(mb, elapsed, TS.Categories.Performance);
		}
	}
}
