using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Xml;
using System.Globalization;

namespace System.Scheduler
{
	/// <summary>
	/// Represents a collection of Processors
	/// </summary>
	internal sealed class ProcessorCollection : List<IProcessor>
	{
		#region Static Members
		/// <summary>
		/// Contains constants used by the ProcessorList class
		/// </summary>
		internal static class Constants
		{
			/// <summary>
			/// The name of the Processor Statuses node.
			/// </summary>
			public const string StatusXmlNode = "ProcessorStatuses";
		}

		/// <summary>
		/// Stores the full list of processors. 
		/// </summary>
		private static ProcessorCollection _processors = new ProcessorCollection();

		/// <summary>
		/// Stores the lists of processors by status file.
		/// </summary>
		private static Dictionary<string, List<IProcessor>> _statusFiles = new Dictionary<string, List<IProcessor>>();

		/// <summary>
		/// Creates a processor from a Processor Information object and adds it to the collection.
		/// </summary>
		/// <param name="info"></param>
		public static void Add(ProcessorInfo info)
		{
			TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Adding processor '{0}' ('{1}').", info.Name, info.ProcessorType);
			TS.Logger.WriteLineIf(TS.EC.TraceInfo && !info.Enabled, TS.Categories.Info, " - Processor '{0}' is disabled.", info.Name);
			TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Info, " - Processor '{0}' has {1} enabled schedules.", info.Name, info.Schedules.EnabledCount);

      if (info.Enabled)
      {
        IProcessor processor = CreateProcessor(info);
        _processors.Add(processor);

        processor.Info = info;
        processor.Executed += new EventHandler(_processors.Item_Executed);

        if (!string.IsNullOrEmpty(info.StatusFile))
        {
          if (!_statusFiles.ContainsKey(info.StatusFile)) _statusFiles.Add(info.StatusFile, new List<IProcessor>());
          _statusFiles[info.StatusFile].Add(processor);
        }
      }
    }

    /// <summary>
    /// Creates an instance of the processor from either the IOC container or using the Processor Type specified.
    /// </summary>
    /// <param name="info">The Processor Information containing initialization information.</param>
    private static IProcessor CreateProcessor(ProcessorInfo info)
    {
      IProcessor processor = null;

      if (!string.IsNullOrWhiteSpace(info.Name))
      {
        if (IOC.Instance.ContainsObject(info.ContextId, info.Name))
        {
          processor = IOC.Instance.GetObjectAndInject<IProcessor>(info.ContextId, info.Name);
        }
        else if (IOC.Instance.ContainsObject(Settings.Values.DefaultContextId, info.Name))
        {
          processor = IOC.Instance.GetObjectAndInject<IProcessor>(Settings.Values.DefaultContextId, info.Name);
        }
      }

      if ((processor == null) && !string.IsNullOrWhiteSpace(info.ProcessorType))
        processor = ReflectionExtensions.CreateObjectUnsafe<IProcessor>(info.ProcessorType);

      if (processor == null)
        throw new TypeLoadException(string.Format(CultureInfo.InvariantCulture, "The processor '{0}' of type '{1}' could not be found in the container or created using reflection.", info.Name, info.ProcessorType));

      return processor;
    }

		/// <summary>
		/// Creates multiple processors from a Processor Information list and adds them to the collection.
		/// </summary>
		/// <param name="collection"></param>
		public static void AddRange(ProcessorInfoCollection collection)
		{
			collection.ForEach(Add);
		}

		/// <summary>
		/// Saves the status of all Processors.
		/// </summary>
		public static void SaveStatusAll()
		{
			_statusFiles.Keys.ForEach(key => SaveStatus(key, _statusFiles[key]));
		}

    /// <summary>
    /// Object used to prevent multiple Processors from attempting to update a status file at the same time.
    /// </summary>
    /// <remarks>This is necessary because multiple Processors can update the same status file.</remarks>
    private static object _statusLock = new object();

		/// <summary>
		/// Saves the status of all Processors for a given Status File.
		/// </summary>
		/// <param name="fileName">The file name of the Status File.</param>
		/// <param name="processors">The list of processors which share the Status File.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) processor classes will possibly throw")]
		private static void SaveStatus(string fileName, List<IProcessor> processors)
		{
			try
			{
        lock (_statusLock)
        {
          using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
          {
            using (XmlTextWriter writer = new XmlTextWriter(stream, Settings.Values.Encoding))
            {
              writer.Formatting = Formatting.Indented;

              writer.WriteStartDocument();
              writer.WriteStartElement(Constants.StatusXmlNode);

              processors.ForEach(item => item.WriteStatus(writer));

              writer.WriteEndElement();
              writer.WriteEndDocument();
            }
          }
        }
			}
			catch (Exception ex)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "Unable to save status of processors to file '{0}'.\r\n{1}", fileName, ex);
			}
		}

		/// <summary>
		/// Starts all processors in the list (disabled processors will not start).
		/// </summary>
		public static void StartAll()
		{
			_processors.ForEach(Start);
		}

		/// <summary>
		/// Starts a processor.
		/// </summary>
		/// <param name="processor">The processor to start.</param>
    private static void Start(IProcessor processor)
    {
      processor.Info.Schedules.SetLastRun(DateTime.Today);
      processor.StartProcess();
    }

		/// <summary>
		/// Stops all processors in the list (already stopped or unstarted processors will be unaffected).
		/// </summary>
		public static void StopAll()
		{
			_processors.ForEach(item => item.StopProcess());

			Stopwatch sw = Stopwatch.StartNew();

			while (_processors.Exists(item => item.IsRunning) &&
				((Settings.Values.ShutdownTimeout == 0) || (sw.ElapsedMilliseconds < Settings.Values.ShutdownTimeout)))
			{
				Thread.Sleep(100);
			}

			List<IProcessor> stillRunning = _processors.FindAll(item => item.IsRunning);

			if (stillRunning.Count == 0)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "All processors have stopped.");
			}
			else
			{
				TS.Logger.WriteLineIf(TS.EC.TraceWarning, TS.Categories.Warning, "All processors have not stopped.  Aborting processor threads.");
				stillRunning.ForEach(item => item.AbortProcess());
				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Remaining processors have been aborted.");
			}
		}

		/// <summary>
		/// Stops all processors and then clears the list of processors.
		/// </summary>
		public static void ClearProcessors()
		{
			StopAll();
			_processors.Clear();
			_statusFiles.Clear();
			_processors = null;
			_statusFiles = null;
		}

		#endregion

		#region Instance Members
		/// <summary>
		/// Constructor.
		/// </summary>
		private ProcessorCollection() { }

		/// <summary>
		/// Handles the Processor.Executed event.  Causes the Processor to save it's configuration and last run status to file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) processor classes will possibly throw")]
		private void Item_Executed(object sender, EventArgs e)
		{
			try
			{
				IProcessor processor = sender as IProcessor;
				if ((processor != null) && (!string.IsNullOrEmpty(processor.Info.StatusFile)) && (_statusFiles.ContainsKey(processor.Info.StatusFile)))
				{
					SaveStatus(processor.Info.StatusFile, _statusFiles[processor.Info.StatusFile]);
				}
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
			}
		}
		#endregion
	}
}
