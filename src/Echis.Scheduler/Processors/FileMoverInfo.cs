using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Scheduler.Processors
{
	/// <summary>
	/// Stores File Status Information used by the File Mover Processor
	/// </summary>
	public sealed class FileMoverInfo
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public FileMoverInfo()
		{
			CopyTargets = new List<string>();
		}

		/// <summary>
		/// Gets or sets the full file name of the source file.
		/// </summary>
		[XmlAttribute]
		public string SourceFileName { get; set; }

		/// <summary>
		/// Gets or sets the full file name of the target file.
		/// </summary>
		/// <remarks>This property is populated immediately before the file is moved.</remarks>
		[XmlIgnore]
		public string TargetFileName { get; set; }

		/// <summary>
		/// Gets the additional Copy targets.
		/// </summary>
		/// <remarks>This property is populated immediately before the file is copied.</remarks>
		[XmlIgnore]
		public List<string> CopyTargets { get; private set; }

		/// <summary>
		/// Gets or sets the relative path and file name.
		/// </summary>
		[XmlAttribute]
		public string RelativeFileName { get; set; }

		/// <summary>
		/// Gets or sets date and time the file was created.
		/// </summary>
		[XmlAttribute]
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// Gets or sets the size of the file.
		/// </summary>
		[XmlAttribute]
		public long Size { get; set; }

		/// <summary>
		/// Gets or sets the time which the file will be moved.
		/// </summary>
		[XmlAttribute]
		public DateTime TimeToMove { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the file has been moved.
		/// </summary>
		[XmlIgnore]
		public bool Moved { get; set; }

		/// <summary>
		/// Gets or sets the number of times an error has occured while attempting to move the file.
		/// </summary>
		[XmlAttribute]
		public int ErrorCount { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the File Mover Processor has failed to move the file (ErrorCount exceeds threshold).
		/// </summary>
		[XmlAttribute]
		public bool Failed { get; set; }

		/// <summary>
		/// Indicates if the file is ready to be moved.
		/// </summary>
		/// <param name="processTime">The Date and Time of the current Process Execution.</param>
		/// <param name="job">The File Move Job which is being processed.</param>
		public bool ReadyToMove(DateTime processTime, FileMoverJob job)
		{
			if (job == null) throw new ArgumentNullException("job");

			return ((!Failed || job.RetryFailures) && (processTime >= TimeToMove));
		}

		/// <summary>
		/// Indicates if the Error Count has exceeded the threshold for the first time.
		/// </summary>
		/// <param name="errorThreshhold">The Error Count threshold.</param>
		internal bool SendErrorMessage(int errorThreshhold)
		{
			return (ErrorCount >= errorThreshhold) && !Failed;
		}
	}

	/// <summary>
	/// Stores a list of File Status objects used by the File Mover Processor.
	/// </summary>
	[XmlType("FileMoverInfoCollection")]
	public sealed class FileMoverInfoCollection : List<FileMoverInfo>
	{
	}
}
