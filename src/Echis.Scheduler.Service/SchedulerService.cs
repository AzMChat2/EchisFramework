using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceProcess;
using InstallSettings = System.Configuration.Install.Settings;

namespace System.Scheduler.Service
{
	/// <summary>
	/// The Windows Scheduler Service
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")] // Must be fully qualified in order for the IDE to recognize this attribute
	public partial class SchedulerService : ServiceBase
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public SchedulerService()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Starts the service.
		/// </summary>
		/// <param name="args"></param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Service is starting, any exception should be recorded in the event log, and startup should abort.")]
		protected override void OnStart(string[] args)
		{
			try
			{
				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Starting {0} Scheduler service", InstallSettings.Values.ServiceName);
				ServiceManager.Start();
				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "{0} Scheduler service has started.", InstallSettings.Values.ServiceName);
			}
			catch (Exception ex)
			{
				LogException("start up", ex);
				Stop();
			}
		}

		/// <summary>
		/// Stops the service.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Service is stopping, any exception should be recorded in the event log.")]
		protected override void OnStop()
		{
			try
			{
				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Stopping Scheduler service");
				ServiceManager.Stop();
			}
			catch (Exception ex)
			{
				LogException("shutdown", ex);
			}
		}

		/// <summary>
		/// Logs any Exception thrown during start up or shutdown.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="ex"></param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Service attempting to write to Windows Event Log because of an exception during startup or shutdown, any new exception should be ignored.")]
		private void LogException(string action, Exception ex)
		{
			try
			{
				string message = string.Format(CultureInfo.InvariantCulture, "An exception was thrown during {0}: {1}", action, ex);
				EventLog.WriteEntry(message, EventLogEntryType.Error);
			}
			catch { }
		}
	}
}
