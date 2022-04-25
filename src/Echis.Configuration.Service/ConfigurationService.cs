using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Configuration.Managers.Remote;
using System.ServiceModel;
using System.Globalization;

namespace System.Configuration.Service
{
	public partial class ConfigurationService : ServiceBase
	{
		public ConfigurationService()
		{
			InitializeComponent();
		}

		private ServiceHost _serviceHost;

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification="Service is starting, any exception should be recorded in the event log, and startup should abort.")]
		protected override void OnStart(string[] args)
		{
			try
			{
				_serviceHost = new ServiceHost(typeof(RemoteConfigurationService));
				_serviceHost.Open();
			}
			catch (Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "Failed to start the Configuration Service\r\n{0}",	ex);
				EventLog.WriteEntry("System.Configuration.Service", msg, EventLogEntryType.Error);
				Stop();
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Service is stopping, any exception should be recorded in the event log.")]
		protected override void OnStop()
		{
			try
			{
				if (_serviceHost != null)
				{
					_serviceHost.Close();
					Dispose(_serviceHost);
					_serviceHost = null;
				}
			}
			catch (Exception ex)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "An error occurred while stopping the Configuration Service\r\n{0}", ex);
				EventLog.WriteEntry("System.Configuration.Service", msg, EventLogEntryType.Error);
			}
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
