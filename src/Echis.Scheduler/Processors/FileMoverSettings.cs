using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

namespace System.Scheduler.Processors
{
	/// <summary>
	/// Defines settings for the File Mover Processor
	/// </summary>
	public interface IFileMoverSettings
	{
		/// <summary>
		/// Gets or sets the list of File Mover Jobs to be processed by the File Mover Processor
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Property Setter is required by the XmlSerializer.")]
		FileMoverJobList Jobs { get; set; }
	}

	/// <summary>
	/// Settings for the File Mover Processor
	/// </summary>
	public class FileMoverSettings : IFileMoverSettings
	{
		/// <summary>
		/// Gets or sets the list of File Mover Jobs to be processed by the File Mover Processor
		/// </summary>
		[XmlElement("FileMoverJob")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Property Setter is required by the XmlSerializer.")]
		public FileMoverJobList Jobs { get; set; }
	}

	/// <summary>
	/// The File Mover Job contains information the File Mover Processor uses to find and move files.
	/// </summary>
	public sealed class FileMoverJob
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public FileMoverJob()
		{
			Enabled = true; // Default to true
		}

		/// <summary>
		/// Gets or sets the name of the File Mover Job
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the a flag indicating if the File Mover Job is enabled or not.
		/// </summary>
		[XmlAttribute]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if sub-directories should also be searched for files.
		/// </summary>
		[XmlAttribute]
		public bool RecursiveSearch { get; set; }

		/// <summary>
		/// Gets or sets the search pattern used to find files.
		/// </summary>
		[XmlAttribute]
		public string SearchPattern { get; set; }

		/// <summary>
		/// Gets or sets the File Attributes of files to be moved.
		/// </summary>
		[XmlAttribute]
		public FileAttributes IncludeAttributes { get; set; }

		/// <summary>
		/// Gets or sets the File Attributes of files to be not moved.
		/// </summary>
		[XmlAttribute]
		public FileAttributes ExcludeAttributes { get; set; }

		/// <summary>
		/// Gets or sets the path which will be searched for files.
		/// </summary>
		[XmlAttribute]
		public string SourcePath { get; set; }

		/// <summary>
		/// Gets or sets the path to which the files will be moved.
		/// </summary>
		[XmlAttribute]
		public string TargetPath { get; set; }

		/// <summary>
		/// Gets or sets the amount of time, in seconds, to wait before moving a files.
		/// </summary>
		/// <remarks>Allows processes such as FTP to finish writing the file before attempting to move it.</remarks>
		[XmlAttribute]
		public int FileMoveWait { get; set; }

		/// <summary>
		/// Gets or sets the number of times an error can occur while attempting to move a file before it is failed.
		/// </summary>
		[XmlAttribute]
		public int ErrorThreshold { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the File Mover Processor should continute to attempt to move failed files.
		/// </summary>
		[XmlAttribute]
		public bool RetryFailures { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if existing files should be overwritten by the file being moved.
		/// </summary>
		[XmlAttribute]
		public bool OverwriteExisting { get; set; }

		/// <summary>
		/// Gets or sets the minimum file size (in bytes) of files to be moved.
		/// </summary>
		[XmlAttribute]
		public long MinimumFileSize { get; set; }

		/// <summary>
		/// Gets or sets a list of File Names to exclude from being moved.
		/// </summary>
		[XmlElement("ExcludeFile")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Property Setter is required by the XmlSerializer.")]
		public ExcludedFileList ExcludedFiles { get; set; }

		/// <summary>
		/// Gets or sets a list of additional target paths to which the file will be copied (before being moved).
		/// </summary>
		[XmlElement("CopyTarget")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
			Justification = "Property Setter is required by the XmlSerializer.")]
		public CopyTargetList CopyTargets { get; set; }

		/// <summary>
		/// Gets or sets the File Name to which the File Status List will be persisted after the File Mover Processor executes.
		/// </summary>
		[XmlAttribute]
		public string FileStatusFileName { get; set; }

		/// <summary>
		/// Stores the list of File Status objects for this File Mover Job
		/// </summary>
		private FileMoverInfoCollection _fileList;
		/// <summary>
		/// Gets or sets the list of File Status objects for this File Mover Job
		/// </summary>
		[XmlIgnore]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
			Justification="Multiple exceptions are possible, process is designed to continue even if the FileStatus file is corrupt or missing")]
		public FileMoverInfoCollection FileList
		{
			get
			{
				if (_fileList == null)
				{
					if (!string.IsNullOrEmpty(FileStatusFileName) && File.Exists(FileStatusFileName))
					{
						try
						{
							_fileList = XmlSerializer<FileMoverInfoCollection>.DeserializeFromXmlFile(FileStatusFileName);
						}
						catch
						{
							_fileList = new FileMoverInfoCollection();
						}
					}
					else
					{
						_fileList = new FileMoverInfoCollection();
					}
				}
				return _fileList;
			}
		}

		/// <summary>
		/// Persists the File Status List to an Xml File.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Multiple exceptions are possible, process is designed to continue even if the FileStatus file cannot be saved.")]
		internal void SaveFileList()
		{
			if (!string.IsNullOrEmpty(FileStatusFileName) && (_fileList != null))
			{
				try
				{
					IOExtensions.CreateDirectoryIfNotExists(Path.GetDirectoryName(FileStatusFileName));
					XmlSerializer<FileMoverInfoCollection>.SerializeToXmlFile(FileStatusFileName, _fileList);
				}
				catch (Exception ex)
				{
					TS.Logger.WriteExceptionIf(TS.Error, ex);
				}
			}
		}
	}

	/// <summary>
	/// Represents a list of File Mover Jobs which the File Mover Processor uses to find and move files.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "List is the appropriate suffix.")]
	public sealed class FileMoverJobList : List<FileMoverJob> { }

	/// <summary>
	/// Represents a list of File Names to exclude from being moved.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "List is the appropriate suffix.")]
	public sealed class ExcludedFileList : List<string> { }

	/// <summary>
	/// Stores information about additional target paths to which the file will be copied.
	/// </summary>
	public sealed class CopyTarget
	{
		/// <summary>
		/// Gets or sets the Path to which the file will be copied.
		/// </summary>
		[XmlAttribute]
		public string TargetPath { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if an error during copying should be counted as a Move Error.
		/// </summary>
		[XmlAttribute]
		public bool FailOnError { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if existing files should be overwritten by the file being copied.
		/// </summary>
		[XmlAttribute]
		public bool OverwriteExisting { get; set; }
	}

	/// <summary>
	/// Represents a list of additional target paths to which the file will be copied (before being moved).
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "List is the appropriate suffix.")]
	public sealed class CopyTargetList : List<CopyTarget> { }
}
