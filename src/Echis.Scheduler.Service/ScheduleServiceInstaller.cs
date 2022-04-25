using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System;

namespace System.Scheduler.Service
{
  /// <summary>
	/// The Scheduling Service Installer is used to install the System Scheduling Service.
  /// </summary>
  [RunInstallerAttribute(true)]
	[System.ComponentModel.DesignerCategory("Code")] // Must be fully qualified in order for the IDE to recognize this attribute
  public class SchedulingServiceInstaller : InstallerEx
  {
  }
}
