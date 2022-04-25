using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceProcess;
using System.Configuration.Install;

namespace System.Configuration.Install
{
	/// <summary>
	/// Installs a service using configurable parameters.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")] // Must be fully qualified in order for the IDE to recognize this attribute
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix",
		Justification = "InstallerEx is an Extended version of Installer")]
	public class InstallerEx : Installer
	{
		/// <summary>
		/// Gets the ServiceInstaller instance.
		/// </summary>
		protected ServiceInstaller ServiceInstaller { get; private set; }
		/// <summary>
		/// Gets the ServiceProcessInstaller instance.
		/// </summary>
		protected ServiceProcessInstaller ProcessInstaller { get; private set; }

		/// <summary>
		/// Default Constructor
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
			Justification = "Reviewed.  Call to Initialize is intended to be virtual to allow derived classes to extend.")]
		public InstallerEx()
		{
			ServiceInstaller = new ServiceInstaller();
			ProcessInstaller = new ServiceProcessInstaller();
			Initialize();
		}

		/// <summary>
		/// Initializes the ServiceInstaller and ServiceProcessInstaller properties using values contained in the configuration settings.
		/// </summary>
		protected virtual void Initialize()
		{
			// NOTE: We use Trace here instead of TS.Logger because the Service Installer doesn't load the configuration.

			Trace.Listeners.Add(new ConsoleTraceListener());
			Trace.WriteLine("Loading service installer info from settings.", TS.Categories.Event);

			ProcessInstaller.Account = ServiceAccount;
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Process Installer Account: {0}", ProcessInstaller.Account), TS.Categories.Info);

			ServiceInstaller.StartType = StartType;
			Trace.WriteLineIf(TS.Info, string.Format(CultureInfo.InvariantCulture, "Service Installer StartType: {0}", ServiceInstaller.StartType), TS.Categories.Info);

			ServiceInstaller.ServiceName = ServiceName;
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Service Installer ServiceName: {0}", ServiceInstaller.ServiceName), TS.Categories.Info);

			ServiceInstaller.DisplayName = DisplayName;
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Service Installer DisplayName: {0}", ServiceInstaller.DisplayName), TS.Categories.Info);

			ServiceInstaller.Description = Description;
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Service Installer Description: {0}", ServiceInstaller.Description), TS.Categories.Info);

			base.Installers.Add(ServiceInstaller);
			base.Installers.Add(ProcessInstaller);

			Trace.WriteLine("Service Installer initialization complete.", TS.Categories.Event);
		}

		/// <summary>
		/// Disposes installation objects.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			ServiceInstaller.Dispose();
			ProcessInstaller.Dispose();
			base.Dispose(disposing);
		}

		/// <summary>
		/// Gets the parameter value from the installation Context, or if the installation context is null, returns null.
		/// </summary>
		/// <param name="paramName">The name or key of the parameter to retrieve.</param>
		/// <returns>Returns the parameter value from the installation Context, or if the installation context is null, returns null.</returns>
		protected string GetParameterFromContext(string paramName)
		{
			return (Context == null) ? null : Context.Parameters[paramName];
		}

		/// <summary>
		/// Gets the Windows Account under which the Scheduler Service will initially be configured to run.
		/// </summary>
		protected ServiceAccount ServiceAccount
		{
			get
			{
				string parameter = GetParameterFromContext("ServiceAccount");
				if (string.IsNullOrEmpty(parameter))
				{
					return Settings.Values.ServiceAccount;
				}
				else
				{
					return (ServiceAccount)Enum.Parse(typeof(ServiceAccount), parameter);
				}
			}
		}

		/// <summary>
		/// Gets the initial Windows Service Start type for the Scheduler Service.
		/// </summary>
		protected ServiceStartMode StartType
		{
			get
			{
				string parameter = GetParameterFromContext("StartType");
				if (string.IsNullOrEmpty(parameter))
				{
					return Settings.Values.StartType;
				}
				else
				{
					return (ServiceStartMode)Enum.Parse(typeof(ServiceStartMode), parameter);
				}
			}
		}

		/// <summary>
		/// Gets the Service name for the Scheduler Service.
		/// </summary>
		protected string ServiceName
		{
			get
			{
				string retVal = GetParameterFromContext("ServiceName");
				if (string.IsNullOrEmpty(retVal))
				{
					retVal = Settings.Values.ServiceName;
				}
				return retVal;
			}
		}

		/// <summary>
		/// Gets the Display name for the Scheduler Service.
		/// </summary>
		protected string DisplayName
		{
			get
			{
				string retVal = GetParameterFromContext("DisplayName");
				if (string.IsNullOrEmpty(retVal))
				{
					retVal = Settings.Values.DisplayName;
				}
				return retVal;
			}
		}

		/// <summary>
		/// Gets the Service description for the Scheduler Service.
		/// </summary>
		protected string Description
		{
			get
			{
				string retVal = GetParameterFromContext("Description");
				if (string.IsNullOrEmpty(retVal))
				{
					retVal = Settings.Values.Description;
				}
				return retVal;
			}
		}
	}
}
