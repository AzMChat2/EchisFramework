using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Diagnostics.Loggers;
using InstallSettings = System.Configuration.Install.Settings;

namespace System.Scheduler.Service
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Object 'stream' is being disposed properly.")]
		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
			Justification = "Service is stopping, call to GC.Collect() insures resources are released before stopping.")]
		static void Main()
		{
			Thread.CurrentThread.Name = "ServiceHost";

			Stream stream = null;
			TraceListener listener = null;

			try
			{
				if (!string.IsNullOrEmpty(Settings.Values.TraceOutputFileName))
				{
					try
					{
						IOExtensions.CreateDirectoryIfNotExists(Path.GetDirectoryName(Settings.Values.TraceOutputFileName));
						stream = File.Open(Settings.Values.TraceOutputFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
						listener = new TextWriterTraceListener(stream);

						StreamWriter writer = new StreamWriter(stream);
						writer.WriteLine("Starting Scheduler Service.");

						if (TS.Logger.GetType().FullName.StartsWith("System.Diagnostics.Loggers.DefaultLogger", StringComparison.OrdinalIgnoreCase))
						{
							Trace.WriteLine("Logger is currently set to DefaultLogger. Changing logger to TraceLogger", TS.Categories.Event);
							TS.Logger = ReflectionExtensions.CreateObjectUnsafe<LoggerBase>("System.Diagnostics.Loggers.TraceLogger, System.Diagnostics");
						}
					}
					catch { }
				}

				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "{0} Scheduler Service Host has started.", InstallSettings.Values.ServiceName);
				Trace.Flush();

				ServiceBase.Run(new ServiceBase[] { new SchedulerService() });

				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "{0} Scheduler Service Host has stopped.", InstallSettings.Values.ServiceName);
				TS.Logger.OutputPerformanceStats();
				TS.Logger.Flush();
			}
			finally
			{
				if (listener != null)
				{
					Trace.Flush();
					Trace.Listeners.Remove(listener);
					listener = null;
				}

				if (stream != null)
				{
					stream.Dispose();
					stream = null;
				}

				// Force cleanup.
				GC.Collect();
				GC.WaitForPendingFinalizers();

			}
		}
	}
}
