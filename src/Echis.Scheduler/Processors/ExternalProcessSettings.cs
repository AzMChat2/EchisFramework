using System.Diagnostics;
using System.Xml.Serialization;

namespace System.Scheduler.Processors
{
	/// <summary>
	/// Settings for the ExternalProcessProcessor
	/// </summary>
	public class ExternalProcessSettings
	{
		/// <summary>
		/// Gets or sets the Executable path and filename to be executed.
		/// </summary>
		[XmlAttribute]
		public string Executable { get; set; }

		/// <summary>
		/// Gets or sets any command line arguments.
		/// </summary>
		[XmlAttribute]
		public string Arguments { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating whether or not to use the operating system shell to start the process.
		/// </summary>
		[XmlAttribute]
		public bool UseShellExecute { get; set; }

		/// <summary>
		/// Gets or sets the window state to use when the process is started.
		/// </summary>
		[XmlAttribute]
		public ProcessWindowStyle WindowStyle { get; set; }

		/// <summary>
		/// Gets or sets the working directory for the process.
		/// </summary>
		[XmlAttribute]
		public string WorkingDirectory { get; set; }
	}
}
