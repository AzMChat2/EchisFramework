using System;
using System.ServiceModel;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.Loggers.Service;

namespace System.Diagnostics.LoggerService
{
	class Program
	{
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Service is stopping, any exception should be recorded in the event log.")]
		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
			Justification = "Service is stopping, call to GC.Collect() insures resources are released before stopping.")]
		static void Main()
		{
			try
			{
				Thread.CurrentThread.Name = "TraceService.MainThread";
				Trace.Listeners.Add(new ConsoleTraceListener());

				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Logger Service Test Host is starting...");

				TS.Logger.WriteLine("Creating instance of RegistryService...");
				RegistryService registryService = new RegistryService();

				TS.Logger.WriteLine("Creating instance of ManagerService...");
				ManagerService managerService = new ManagerService();

				TS.Logger.WriteLine("Creating WCF Service Hosts...");
				using (ServiceHost registryHost = new ServiceHost(registryService))
				{
					using (ServiceHost managerHost = new ServiceHost(managerService))
					{
						TS.Logger.WriteLine("Opening WCF Service Host...");
						registryHost.Open();
						managerHost.Open();

						TS.Logger.WriteLine("WCF Service is active, press enter when ready to stop and close");
						Console.ReadLine();

						TS.Logger.WriteLine("Closing WCF Service Hosts...");
						registryHost.Close();
						managerHost.Close();

						TS.Logger.WriteLine("Releasing resources...");
					}
				}

				registryService = null;
				managerService = null;

				TS.Logger.WriteLine("Forcing Garbage Collection...");
				TS.Logger.Flush();
				GC.Collect();
				GC.WaitForPendingFinalizers();

			}
			catch (Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture,
					"\r\n\r\n\r\n***************************** ERROR *****************************\r\n\r\nFailed to start the Trace Registry Service\r\n\r\n{0}", ex);
				TS.Logger.WriteLine(msg);
			}

#if DEBUG
			TS.Logger.WriteLine("Services have been stopped, press enter to exit.");
			Console.ReadLine();
#endif
		}
	}
}
