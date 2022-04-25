using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;

namespace System.Scheduler.Processors
{
	/// <summary>
	/// Executes an External Process and waits for the process to end.
	/// </summary>
	public class ExternalProcessProcessor : Processor<ExternalProcessSettings>
	{
		/// <summary>
		/// Stores the Date/Time of the Last Execution Start.
		/// </summary>
		private DateTime _lastStart;
		/// <summary>
		/// Stores the Date/Time of the Last Execution End.
		/// </summary>
		private DateTime _lastEnd;
		/// <summary>
		/// Stores the result of the Last Execution.
		/// </summary>
		private int _lastResult;

		/// <summary>
		/// Executes the external process.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification="Multiple exceptions are possible, handling is the same: Log and continue.")]
		protected override void Execute()
		{
			MethodBase __mb = new StackFrame(0).GetMethod();
			DateTime __methodStart = DateTime.Now;
			if (TS.Verbose) TS.Logger.WriteMethodCallMessage(__mb);

			try
			{
				_lastStart = __methodStart;
				using (Process process = new Process())
				{
					process.StartInfo.FileName = ProcessorSettings.Executable;
					process.StartInfo.UseShellExecute = ProcessorSettings.UseShellExecute;
					process.StartInfo.WindowStyle = ProcessorSettings.WindowStyle;

					if (!string.IsNullOrEmpty(ProcessorSettings.Arguments)) process.StartInfo.Arguments = ProcessorSettings.Arguments;
					if (!string.IsNullOrEmpty(ProcessorSettings.WorkingDirectory)) process.StartInfo.WorkingDirectory = ProcessorSettings.WorkingDirectory;

					if (process.Start()) process.WaitForExit();
					_lastResult = process.ExitCode;
				}
			}
			catch (Exception ex)
			{
				LogException(__mb, ex, null);
			}
			finally
			{
				if (TS.Info) TS.Logger.WritePerformanceMessage(__mb, DateTime.Now.Subtract(__methodStart));
				TS.Logger.WritePerformanceIf(TS.Info, __methodStart);
				_lastEnd = DateTime.Now;
			}
		}

		/// <summary>
		/// Logs Exceptions raised while processing File Mover Jobs
		/// </summary>
		/// <param name="method">The method in which the exception was caught.</param>
		/// <param name="ex">The exception which was caught.</param>
		/// <param name="additionalInfo">Additional information (if any) about what caused the exception.</param>
		/// <remarks>Derived classes may override to handle Processing Exceptions</remarks>
		protected virtual void LogException(MethodBase method, Exception ex, string additionalInfo)
		{
			if (TS.Error) TS.Logger.WriteExceptionMessage(method, ex);
		}

		/// <summary>
		/// Writes the Processor run status to an XmlWriter.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteStatusXml(XmlWriter writer)
		{
			writer.WriteAttribute("NextStart", NextRun);
			writer.WriteAttribute("LastStart", _lastStart);
			writer.WriteAttribute("LastEnd", _lastEnd);
			writer.WriteAttribute("LastProcessTime", _lastEnd.Subtract(_lastStart));
			writer.WriteAttribute("LastExitCode", _lastResult);
		}
	}
}
