using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace System.Configuration.Service
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
			Justification = "Service is stopping, call to GC.Collect() insures resources are released before stopping.")]
		static void Main()
		{
			ServiceBase.Run(new ConfigurationService());

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}
