using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;

namespace System.Scheduler.Processors
{
	/// <summary>
	/// The Folder Cleaner Processor purges folders of files and sub-directories.
	/// </summary>
	public class FolderCleanerProcessor : Processor<FolderCleanerSettings>
	{
		/// <summary>
		/// Stores the total number of files deleted since startup.
		/// </summary>
		private int _totalFilesDeleted;
		/// <summary>
		/// Stores the total nubmer of folders deleted since startup.
		/// </summary>
		private int _totalFoldersDeleted;

		/// <summary>
		/// Stores the list of files deleted in the latest execution.
		/// </summary>
		private List<string> _filesDeleted;
		/// <summary>
		/// Stores the list of folders deleted in the latest execution.
		/// </summary>
		private List<string> _foldersDeleted;

		/// <summary>
		/// Cleans the directories specified in the configuration.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) derived classes will possibly throw")]
		protected override void Execute()
		{
			TS.Logger.WriteMethodCallIf(TS.Verbose);
			DateTime __methodStart = DateTime.Now;

			try
			{
				_filesDeleted = new List<string>();
				_foldersDeleted = new List<string>();

				ProcessorSettings.Folders.ForEach(ProcessFolder);

				_totalFilesDeleted += _filesDeleted.Count;
				_totalFoldersDeleted += _foldersDeleted.Count;
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(TS.Error, ex);
			}
			finally
			{
				TS.Logger.WritePerformanceIf(TS.Info, __methodStart);
			}
		}

		/// <summary>
		/// Processes a Folder.  Deletes all files matching the criteria specified in the Folder object.
		/// </summary>
		/// <param name="folder">The Folder object containing all of the criteria for purging files.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) derived classes will possibly throw")]
		protected virtual void ProcessFolder(Folder folder)
		{
			if (folder == null) throw new ArgumentNullException("folder");

			if (Directory.Exists(folder.Path))
			{
				try
				{
					DateTime purgeDate = DateTime.Now.AddDays(-folder.MaxFileAge);

					TS.Logger.WriteLineIf(TS.Info, TS.Categories.Info, "Cleaning directory '{0}', deleting files older than '{1:yyyy-MM-dd}'.", folder.Path, purgeDate);

					IOExtensions.GetFiles(folder.Path, folder.SearchPattern, folder.Recursive).ForEach(item => ProcessFile(item, purgeDate));

					if (folder.DeleteEmptyFolders)
					{
						Directory.GetDirectories(folder.Path).ForEach(directory => DeleteIfEmpty(directory, folder.Recursive));
					}
				}
				catch (Exception ex)
				{
					TS.Logger.WriteExceptionIf(TS.Error, ex);
				}
			}
			else
			{
				TS.Logger.WriteLineIf(TS.Info, TS.Categories.Info, "The directory '{0}' does not exist.", folder.Path);
			}
		}

		/// <summary>
		/// Deletes a directory if it is empty.
		/// </summary>
		/// <param name="path">The directory to be deleted.</param>
		/// <param name="recursive">Flag indicating if sub-directories should be checked for deletion.</param>
		protected virtual void DeleteIfEmpty(string path, bool recursive)
		{
			if (recursive)
			{
				Directory.GetDirectories(path).ForEach(directory => DeleteIfEmpty(directory, recursive));
			}

			if (Directory.GetDirectories(path).IsNullOrEmpty() && Directory.GetFiles(path).IsNullOrEmpty())
			{
				_foldersDeleted.Add(path);
				Directory.Delete(path);
			}
		}

		/// <summary>
		/// Checks a file and deletes it if it is older than the purge date specified.
		/// </summary>
		/// <param name="fileName">The file to be deleted.</param>
		/// <param name="purgeDate">The purge date.  Files older than this date will be deleted.</param>
		protected virtual void ProcessFile(string fileName, DateTime purgeDate)
		{
			FileInfo file = new FileInfo(fileName);
			if (file.Exists && (file.LastWriteTime <= purgeDate))
			{
				_filesDeleted.Add(fileName);
				file.Delete();
			}
		}

		/// <summary>
		/// Writes the status of the Processor.
		/// </summary>
		protected override void WriteStatusXml(XmlWriter writer)
		{
			writer.WriteAttribute("TotalFilesDeleted", _totalFilesDeleted);
			writer.WriteAttribute("TotalFoldersDeleted", _totalFoldersDeleted);

			if (_filesDeleted != null) _filesDeleted.ForEach(item => writer.WriteElement("FileDeleted", item));
			if (_foldersDeleted != null) _foldersDeleted.ForEach(item => writer.WriteElement("FolderDeleted", item));
		}
	}
}
