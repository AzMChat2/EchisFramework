using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using System.Configuration;

namespace System.Scheduler
{
	/// <summary>
	/// The service settings class contains settings used by the Scheduler Service
	/// </summary>
	[Serializable]
	public class Settings : SettingsBase<Settings>
	{
    /// <summary>
    /// Gets or sets the Default Container ContextId used to lookup Processor objects in the container.
    /// </summary>
    [XmlAttribute]
    public string DefaultContextId { get; set; }

		/// <summary>
		/// Gets or sets the amount of time threads should sleep before checking schedule.
		/// </summary>
		[XmlAttribute]
		public int ThreadSleep { get; set; }

		/// <summary>
		/// Gets or sets the amount of time the service should wait during shutdown before aborting Processor Threads.
		/// </summary>
		[XmlAttribute]
		public int ShutdownTimeout { get; set; }

		/// <summary>
		/// Gets or sets the filename of an optional Trace Output file. (Used for service startup debugging).
		/// </summary>
		[XmlAttribute]
		public string TraceOutputFileName { get; set; }

		/// <summary>
		/// Gets or sets the Encoding (by name) used for writing Xml
		/// </summary>
		[XmlAttribute("Encoding")]
		public string EncodingName
		{
			get { return Encoding.EncodingName; }
			set
			{
				try
				{
					Encoding = System.Text.Encoding.GetEncoding(value);
				}
				catch (ArgumentException ex)
				{
					TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "Invalid setting for Encoding, defaulting to ASCII-US.\r\n{0}", ex);
					Encoding = System.Text.Encoding.ASCII;
				}
			}
		}

		/// <summary>
		/// Gets the Text Encoding to use for all written Xml Files.
		/// </summary>
		[XmlIgnore]
		internal Encoding Encoding { get; private set;}

		//NOTE: The XmlSerializer chokes if we use { get; private set;} on the Processors property,
		//      but works fine if we use an underlying variable (_processors).
		/// <summary>
		/// Stores the list of Processors.
		/// </summary>
		private ProcessorInfoCollection _processors = new ProcessorInfoCollection();
		/// <summary>
		/// Gets the list of Processor Information objects.
		/// </summary>
		[XmlElement("Processor")]
		public ProcessorInfoCollection Processors { get { return _processors; } }

		/// <summary>
		/// Validates the settings contained in the configuration file.
		/// </summary>
		public override void Validate()
		{
			if (ThreadSleep == 0) ThreadSleep = 1000;
			if (Encoding == null) Encoding = System.Text.Encoding.ASCII;
		}
	}
}
