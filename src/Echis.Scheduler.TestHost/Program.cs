using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Diagnostics.Loggers;
using InstallSettings = System.Configuration.Install.Settings;

namespace System.Scheduler.Service
{
	class Program
	{
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

				Trace.Listeners.Add(new ConsoleTraceListener());
				TS.Logger.WriteLineIf(TS.Warning, TS.Categories.Event, "{0} Scheduler Test Console is starting service", InstallSettings.Values.ServiceName);

				ServiceManager.Start();

				TS.Logger.WriteLine(string.Empty, "{0} Scheduler Test Console has started service.\r\nPress ENTER to stop service.", InstallSettings.Values.ServiceName);

				Console.ReadLine();

				TS.Logger.WriteLine(string.Empty, "{0} Scheduler Test Console is stopping service", InstallSettings.Values.ServiceName);
				ServiceManager.Stop();
				TS.Logger.WriteLine(string.Empty, "{0} Scheduler Test Console has stopped service", InstallSettings.Values.ServiceName);

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
