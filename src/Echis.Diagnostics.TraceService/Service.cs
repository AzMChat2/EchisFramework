using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceModel;
using System.ServiceProcess;
using System.Diagnostics.Loggers.Service;

namespace System.Diagnostics.LoggerService
{
	public sealed partial class Service : ServiceBase
	{
		public Service()
		{
			InitializeComponent();
		}

		private IRegistryService _registryService;
		private IManagerService _managerService;
		private ServiceHost _registryHost;
		private ServiceHost _managerHost;

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification="Service is starting, any exception should be recorded in the event log, and startup should abort.")]
		protected override void OnStart(string[] args)
		{
			try
			{
				_registryService = new RegistryService();
				_registryHost = new ServiceHost(_registryService);
				_registryHost.Open();
			}
			catch (Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "Failed to start the Trace Registry Service\r\n{0}",	ex);
				EventLog.WriteEntry("System.Diagnostics.TraceService.Service", msg, EventLogEntryType.Error);
				Stop();
			}

			try
			{
				_managerService = new ManagerService();
				_managerHost = new ServiceHost(_managerService);
				_managerHost.Open();
			}
			catch (Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "Failed to start the Manager Service\r\n{0}", ex);
				EventLog.WriteEntry("System.Diagnostics.TraceService.Service", msg, EventLogEntryType.Error);
				Stop();
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Service is stopping, any exception should be recorded in the event log.")]
		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
			Justification = "Service is stopping, call to GC.Collect() insures resources are released before stopping.")]
		protected override void OnStop()
		{
			try
			{
				if (_registryHost != null)
				{
					_registryHost.Close();
					Dispose(_registryHost);
					_registryHost = null;
				}
				_registryService = null;
			}
			catch (Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "An error occurred while stopping the Trace Registry Service\r\n{0}", ex);
				EventLog.WriteEntry("System.Diagnostics.TraceService.Service", msg, EventLogEntryType.Error);
			}

			try
			{
				if (_managerHost != null)
				{
					_managerHost.Close();
					Dispose(_managerHost);
					_managerHost = null;
				}
				_managerService = null;
			}
			catch (Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "An error occurred while stopping the Manager Service\r\n{0}", ex);
				EventLog.WriteEntry("System.Diagnostics.TraceService.Service", msg, EventLogEntryType.Error);
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();

		}

		private static void Dispose(IDisposable disposableObject)
		{
			if (disposableObject != null)
			{
				disposableObject.Dispose();
			}
		}
	}
}
