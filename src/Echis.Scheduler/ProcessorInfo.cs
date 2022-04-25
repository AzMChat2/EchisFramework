using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using System.Scheduler.Schedules;

namespace System.Scheduler
{
	/// <summary>
	/// Stores information needed to create and configure a Processor.
	/// </summary>
	[Serializable]
	public class ProcessorInfo 
	{
		/// <summary>
		/// Gets or sets the name of the Processor
		/// </summary>
    [XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the status file used to record the current status of the processor.
		/// </summary>
    [XmlAttribute]
    public string StatusFile { get; set; }

		/// <summary>
		/// Gets or sets the Container ContextId of the Processor (if using a container to define the Processor)
		/// </summary>
    [XmlAttribute]
    public string ContextId { get; set; }

    /// <summary>
    /// Gets or sets the full Type name of the Processor.
    /// </summary>
    [XmlAttribute]
    public string ProcessorType { get; set; }

		/// <summary>
		/// Gets or sets a flag which indicates if the processor is enabled.
		/// </summary>
    [XmlAttribute]
    public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets a flag which indicates if the processor should execute when the Scheduler Service starts.
		/// </summary>
    [XmlAttribute]
    public bool ExecuteOnStartup { get; set; }

		/// <summary>
		/// Gets or sets the Schedules for the Process.
		/// </summary>
    [XmlElement("Schedule")]
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
      Justification = "The property setter is required by the XmlSerializer.")]
    public ScheduleCollection Schedules { get; set; }

		/// <summary>
		/// Gets or sets the settings object for the Process.
		/// </summary>
		/// <remarks>This will be used to instantiate the Process's settings class.</remarks>
    [XmlElement]
		public XmlWrapper Settings { get; set; }
	}

	/// <summary>
	/// Stores a collection of Processor Informatin objects.
	/// </summary>
  [Serializable]
  public class ProcessorInfoCollection : List<ProcessorInfo> { }

}
