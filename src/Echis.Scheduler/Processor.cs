using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Scheduler.Schedules;
using ThreadingThreadState = System.Threading.ThreadState;
using System.Security.Principal;
using System.Security;

namespace System.Scheduler
{
	/// <summary>
	/// Base class from which any Processors without settings may be derived.
	/// </summary>
	public abstract class Processor : Processor<object>
	{
	}

	/// <summary>
	/// Base class from which all Processors must be derived.
	/// </summary>
	/// <typeparam name="T">The ProcessorSettings type.</typeparam>
	public abstract class Processor<T> : IProcessor
	{
		/// <summary>
		/// Contains constants used by the Processor
		/// </summary>
		internal static class Constants
		{
			public const string XmlNode = "Processor";
			public const string XmlAttribProcessorName = "Name";
			public const string XmlAttribLastRunStatus = "LastRunStatus";
			public const string XmlAttribLastRunTime = "LastRunTime";
		}

    /// <summary>
    /// Fired when the Processor is executed.
    /// </summary>
    public event EventHandler Executed;

    /// <summary>
    /// Fires the Executed event.
    /// </summary>
    protected virtual void OnExecuted()
    {
      if (Executed != null) Executed(this, new EventArgs());
    }

		/// <summary>
		/// Gets the Processor Settings for the processor.
		/// </summary>
		/// <remarks>This is set internally when the processor is instantiated using the SettingsXml value of the ProcessorInfo.</remarks>
		protected T ProcessorSettings { get; private set; }

		/// <summary>
		/// Gets the schedule which generated the current execution event.
		/// </summary>
		protected Schedule CurrentSchedule { get; private set; }

		/// <summary>
		/// Gets the next scheduled run for this processor.
		/// </summary>
		public virtual DateTime NextRun { get; protected set; }

		/// <summary>
		/// Gets the last run status of the processor.
		/// </summary>
		public virtual string LastRunStatus { get; protected set; }

    /// <summary>
    /// Stores the Processor Info which is used to configure the processor.
    /// </summary>
    private ProcessorInfo _info;
    /// <summary>
    /// Gets the Processor Info which is used to configure the processor.
		/// </summary>
		public ProcessorInfo Info
    {
      get { return _info; }
      set
      {
        _info = value;
        ProcessorSettings = ((value != null) && (value.Settings != null)) ? (T)value.Settings.Value : default(T);
      }
    }

		/// <summary>
		/// Starts the schedule for the Processor.
		/// </summary>
		public void StartProcess()
		{
			if ((Info.Schedules != null) && (Info.Enabled))
			{
				StopRequested = false;
				ProcessThread = new Thread(Run);
				ProcessThread.Name = Info.Name;
				ProcessThread.Start();
			}
		}

		/// <summary>
		/// Stops the schedule for the Processor (will wait for processor to complete current execution if necessary).
		/// </summary>
		public void StopProcess()
		{
			StopRequested = true;
		}

    /// <summary>
    /// Aborts the processor by Aborting the Process Thread.
    /// </summary>
    public void AbortProcess()
    {
      ProcessThread.Abort();
    }

		/// <summary>
		/// Determines the time of the next execution event and sets the CurrentSchedule property.
		/// </summary>
		protected virtual void DetermineNextRun()
		{
			CurrentSchedule = Info.Schedules.NextSchedule;
      NextRun = (CurrentSchedule == null) ? DateTime.MaxValue : CurrentSchedule.NextRun;
		}

		/// <summary>
		/// Thread entry point for the Schedule.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) derived classes will possibly throw")]
		private void Run()
		{
			TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "{0} processor has started.", Info.Name);
			CheckThreadPrincipal();

      if (Info.ExecuteOnStartup)
      {
        Schedule currentSchedule = CurrentSchedule;
        CurrentSchedule = null;

        TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Startup is executing {0} Processor", Info.Name);

        Runtime = DateTime.Now;

        Execute();
        OnExecuted();
        DetermineNextRun();

        CurrentSchedule = currentSchedule;
      }
      else
      {
        DetermineNextRun();
      }

			TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Startup has set next Scheduled run for {0} processor: {1:MM/dd/yyyy HH:mm:ss}", Info.Name, NextRun);

			while (!StopRequested)
			{
				if (DateTime.Now >= NextRun)
				{
					try
					{
						TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Executing {0} Processor", Info.Name);

						Runtime = DateTime.Now;
						Info.Schedules.SetLastRun(NextRun);

						Execute();
						OnExecuted();
						DetermineNextRun();

						TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Next Scheduled run for {0} processor: {1:MM/dd/yyyy HH:mm:ss}", Info.Name, NextRun);
					}
					catch (Exception ex)
					{
						TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "An error occurred while executing {0} Processor.\r\n{1}", Info.Name, ex);
					}
					finally
					{
						// Output performance stats once per day, should be at or near the end of the log.
						if (NextRun.Date > DateTime.Today) TS.Logger.OutputPerformanceStats();
						TS.Logger.Flush();
					}
				}
				Thread.Sleep(Settings.Values.ThreadSleep);
			}
		}

		/// <summary>
		/// Checks to see if the Current Principal of the Executing Thread has been set.
		/// </summary>
		protected virtual void CheckThreadPrincipal()
		{
			try
			{
				if (Thread.CurrentPrincipal is GenericPrincipal) Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
			}
			catch (SecurityException ex)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceWarning, TS.Categories.Warning, "Unable to set Thread Current Principal.");
				TS.Logger.WriteExceptionIf(TS.EC.TraceWarning, ex);
			}
		}

    /// <summary>
    /// Called by the Scheduler in order to execute the process.
    /// </summary>
    protected abstract void Execute();

		/// <summary>
		/// Gets the last run time for the Processor.
		/// </summary>
		protected DateTime Runtime { get; private set; }

		/// <summary>
		/// The thread on which the process executes.
		/// </summary>
		protected Thread ProcessThread { get; private set; }

    /// <summary>
    /// Gets a flag indicating a request to stop the Schedule.
    /// </summary>
    public bool StopRequested { get; private set; }

		/// <summary>
		/// Gets a flag which indicates if the process thread is currently running (process may be running or idle).
		/// </summary>
		public bool IsRunning
		{
			get
			{
        return (ProcessThread != null) &&
          ((ProcessThread.ThreadState == ThreadingThreadState.Running) || (ProcessThread.ThreadState == ThreadingThreadState.WaitSleepJoin));
			}
		}

		/// <summary>
		/// Writes the Processor run status to an XmlWriter.
		/// </summary>
		/// <param name="writer"></param>
		public void WriteStatus(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			writer.WriteStartElement(Constants.XmlNode);

			writer.WriteAttributeString(Constants.XmlAttribProcessorName, Info.Name);
			writer.WriteAttributeString(Constants.XmlAttribLastRunStatus, LastRunStatus);
			writer.WriteAttributeString(Constants.XmlAttribLastRunTime, Runtime.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

			WriteStatusXml(writer);

			writer.WriteEndElement();
		}

    /// <summary>
    /// Called during shutdown in order to save status.
    /// </summary>
    /// <param name="writer"></param>
    protected abstract void WriteStatusXml(XmlWriter writer);

	}
}
