using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace System.Diagnostics.LoggerService
{
  /// <summary>
  /// The Logger Service Installer is used to install the System Diagnostics Logger Service.
  /// </summary>
  [RunInstallerAttribute(true)]
  public class ServiceInstaller : InstallerEx
  {
  }
}

