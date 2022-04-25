using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Scheduler.Processors
{
	/// <summary>
	/// Configuration settings for the Folder Cleaner Processor.
	/// </summary>
	public class FolderCleanerSettings
	{
		/// <summary>
		/// Gets or sets the list of folders to be cleaned.
		/// </summary>
		[XmlElement("Folder")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Property Setter is required by the XmlSerializer.")]
		public FolderList Folders { get; set; }
	}

	/// <summary>
	/// Represents a folder to be cleaned
	/// </summary>
	public class Folder
	{
		/// <summary>
		/// Gets or sets the path to be cleaned.
		/// </summary>
		[XmlAttribute]
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets the search pattern to find files needing to be purged.
		/// </summary>
		[XmlAttribute]
		public string SearchPattern { get; set; }

		/// <summary>
		/// Gets or set the maximum age of files.  Files older than this will be deleted.
		/// </summary>
		[XmlAttribute]
		public int MaxFileAge { get; set; }

		/// <summary>
		/// Gets or sets a flag which determines if sub-directories are cleaned.
		/// </summary>
		[XmlAttribute]
		public bool Recursive { get; set; }

		/// <summary>
		/// Gets or sets a flag which determines if emtpy sub-directories are deleted.
		/// </summary>
		[XmlAttribute]
		public bool DeleteEmptyFolders { get; set; }
	}

	/// <summary>
	/// Represents a list of folders to be cleaned.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "List is the correct suffix for this class.")]
	public class FolderList : List<Folder>
	{
	}
}
