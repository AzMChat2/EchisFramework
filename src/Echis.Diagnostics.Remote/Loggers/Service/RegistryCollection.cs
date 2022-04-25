using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Diagnostics.Loggers.Registry;

namespace System.Diagnostics.Loggers.Service
{
	internal class LoggerRegistryInfo
	{
		public LoggerRegistryInfo(IRemoteRegistry registry)
		{
			MachineName = registry.GetMachineName();
			ProcessName = registry.GetProcessName();
			Registry = registry;
		}

		public string MachineName { get; private set;}
		public string ProcessName { get; private set; }
		public IRemoteRegistry Registry { get; private set; }

	}

	internal class RegistryCollection : List<LoggerRegistryInfo>
	{
		public static RegistryCollection Instance { get; private set; }

		static RegistryCollection()
		{
			Instance = new RegistryCollection();
		}


		private RegistryCollection()
		{
		}

		public void Add(IRemoteRegistry registry)
		{
			Add(new LoggerRegistryInfo(registry));
		}

		public void Remove(IRemoteRegistry registry)
		{
			string machineName = registry.GetMachineName();
			string processName = registry.GetProcessName();

			List<LoggerRegistryInfo> list = GetLoggerRegistries(machineName, processName);
			LoggerRegistryInfo registryInfo = list.Find(item => (item.Registry == registry));

			if (registryInfo != null)
			{
				Remove(registryInfo);
			}
		}

		public string[] GetMachineNames()
		{
			return (from item in this
							select item.MachineName).Distinct().ToArray();
		}

		public string[] GetProcessNames(string machineName)
		{
			return (from item in this
							where item.MachineName == machineName 
							select item.ProcessName).Distinct().ToArray();
		}

		public string[] GetThreadNames(string machineName, string processName)
		{
			List<string> retVal = new List<string>();

			List<LoggerRegistryInfo> list = GetLoggerRegistries(machineName, processName);
			list.ForEach(item =>
			{
				try
				{
					retVal.AddRange(item.Registry.GetThreads());
				}
				catch (CommunicationException ex)
				{
					TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
					Remove(item);
					string msg = string.Format(CultureInfo.InvariantCulture, RegistryService.Constants.MsgClientCommsFailed, processName, machineName, ex.Message);
					throw new FaultException(msg);
				}
			});

			return retVal.ToArray();
		}

		public List<LoggerRegistryInfo> GetLoggerRegistries(string machineName, string processName)
		{
			return (from item in this
							where ((item.MachineName == machineName) && (item.ProcessName == processName))
							select item).ToList();
		}

		public IRemoteRegistry GetLoggerRegistry(string machineName, string processName, string threadName)
		{
			List<LoggerRegistryInfo> list = GetLoggerRegistries(machineName, processName);

			LoggerRegistryInfo registryInfo = list.Find(item =>
			{
				return item.Registry.GetThreads().Contains(threadName);
			});

			if (registryInfo == null)
			{
				return null;
			}
			else
			{
				return registryInfo.Registry;
			}

		}
	}
}
