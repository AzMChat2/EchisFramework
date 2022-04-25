using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace System.Scheduler.Processors
{
	/// <summary>
	/// Standard File Mover Processor used for Moving and optionally Copying files from one location to another.
	/// </summary>
	public class FileMoverProcessor : FileMoverProcessor<FileMoverSettings> { }

	/// <summary>
	/// Base class from which File Mover Processors can be derived.  This processor is used for Moving and optionally Copying files from one location to another.
	/// </summary>
	/// <typeparam name="TSettings">The Settings used by the File Mover Processor.</typeparam>
	public abstract class FileMoverProcessor<TSettings> : Processor<TSettings> where TSettings : IFileMoverSettings
	{
		/// <summary>
		/// Gets the total number of files which have been moved.
		/// </summary>
		protected int FileCount { get; private set; }
	
		/// <summary>
		/// Gets the current processing time
		/// </summary>
		protected DateTime ProcessTime { get; private set; }

		/// <summary>
		/// Gets the current job being processed.
		/// </summary>
		protected FileMoverJob CurrentJob { get; private set; }

		/// <summary>
		/// Processes each File Mover Job.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Derived classes may raise unknown exceptions.")]
		protected override void Execute()
		{
			DateTime __methodStart = DateTime.Now;
			TS.Logger.WriteMethodCallIf(TS.Verbose);

			try
			{
				ProcessTime = Runtime.AddSeconds(-Runtime.Second);
				ProcessorSettings.Jobs.ForEach(RunJob);
			}
			catch (Exception ex)
			{
				LogException(new StackFrame(0).GetMethod(), ex, null);
			}
			finally
			{
				TS.Logger.WritePerformanceIf(TS.Info, __methodStart);
			}
		}

		/// <summary>
		/// Logs Exceptions raised while processing File Mover Jobs
		/// </summary>
		/// <param name="method">The method in which the exception was caught.</param>
		/// <param name="ex">The exception which was caught.</param>
		/// <param name="additionalInfo">Additional information (if any) about what caused the exception.</param>
		/// <remarks>Derived classes may override to handle Processing Exceptions</remarks>
		protected virtual void LogException(MethodBase method, Exception ex, string additionalInfo)
		{
			if (TS.Error) TS.Logger.WriteExceptionMessage(method, ex);
		}

		/// <summary>
		/// Processes a File Mover Job. 
		/// </summary>
		/// <param name="job">The File Mover Job to be processed.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Derived classes may raise unknown exceptions.")]
		protected virtual void RunJob(FileMoverJob job)
		{
			if (job == null) throw new ArgumentNullException("job");

			if (job.Enabled)
			{
				DateTime __methodStart = DateTime.Now;
				TS.Logger.WriteMethodCallIf(TS.Verbose);

				try
				{

					CurrentJob = job;

					// Check file mover info
					Process(IOExtensions.GetFiles(CurrentJob.SourcePath, CurrentJob.SearchPattern, CurrentJob.RecursiveSearch));

					// Move files that are ready to move.
					Move(CurrentJob.FileList.FindAll(item => item.ReadyToMove(ProcessTime, CurrentJob)));

					// Allow derived classes to do something with or log files that have been moved
					NotifyMoved(CurrentJob.FileList.FindAll(item => item.Moved));

					// Remove file mover info for moved or deleted files.
					CurrentJob.FileList.RemoveAll(item => item.Moved || !File.Exists(item.SourceFileName));

					// Log any failed files.
					NotifyFailed(CurrentJob.FileList.FindAll(item => item.SendErrorMessage(CurrentJob.ErrorThreshold)));

				}
				catch (Exception ex)
				{
					LogException(new StackFrame(0).GetMethod(), ex, string.Format(CultureInfo.InvariantCulture, "FileMoverJob: {0}", job.Name));
				}
				finally
				{
					CurrentJob.SaveFileList();
					CurrentJob = null;

					TS.Logger.WritePerformanceIf(TS.Info, __methodStart);
				}
			}
		}

		/// <summary>
		/// Processes a list of file names, adding them to the File mover info List.
		/// </summary>
		/// <param name="fileNames">An array of strings containing the file names of files found for the current job.</param>
		protected virtual void Process(List<string> fileNames)
		{
			if (fileNames == null) throw new ArgumentNullException("fileNames");
			fileNames.ForEach(Process);
		}

		/// <summary>
		/// Processes a file, adding it to the File mover info list or confirming that the size has not changed and scheduling the file for being moved.
		/// </summary>
		/// <param name="fileName">The path and file name of the file.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Derived classes may raise unknown exceptions.")]
		protected virtual void Process(string fileName)
		{
			DateTime __methodStart = DateTime.Now;
			TS.Logger.WriteMethodCallIf(TS.Verbose);

			try
			{
				if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
				if (!File.Exists(fileName)) throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
					"The specified file '{0}' does not exist.", fileName));

				string fileNameWithoutPath = Path.GetFileName(fileName);

				if (!CurrentJob.ExcludedFiles.Exists(item => item.Equals(fileNameWithoutPath, StringComparison.OrdinalIgnoreCase)))
				{
					FileInfo fileInfo = new FileInfo(fileName);
					FileMoverInfo moveInfo = CurrentJob.FileList.Find(item => item.SourceFileName.Equals(fileName, StringComparison.OrdinalIgnoreCase));

					if (moveInfo == null) 
					{
						
						// TODO: Consider adding an "attribute flags to process" setting?
						//fileInfo.Attributes = FileAttributes.

						if (FileShouldMove(fileInfo))
						{
							moveInfo = new FileMoverInfo()
							{
								RelativeFileName = fileName.Substring(CurrentJob.SourcePath.Length),
								SourceFileName = fileName,
								Size = fileInfo.Length,
								CreateDate = fileInfo.CreationTime,
								TimeToMove = DateTime.MaxValue
							};

							if (moveInfo.RelativeFileName.StartsWith("\\", StringComparison.OrdinalIgnoreCase)) moveInfo.RelativeFileName = moveInfo.RelativeFileName.Substring(1);

							CurrentJob.FileList.Add(moveInfo);
							NotifyAdded(moveInfo);
						}
					}
					else if (!moveInfo.Failed || CurrentJob.RetryFailures)
					{
						if ((moveInfo.TimeToMove == DateTime.MaxValue) && (moveInfo.Size == fileInfo.Length))
						{
							moveInfo.TimeToMove = ProcessTime.AddSeconds(CurrentJob.FileMoveWait);
						}
						else if (moveInfo.Size != fileInfo.Length)
						{
							moveInfo.Size = fileInfo.Length;
							TS.Logger.WriteLineIf(TS.Verbose, TS.Categories.Info, "Still writing to {0}", Path.GetFileName(moveInfo.SourceFileName));
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogException(new StackFrame(0).GetMethod(), ex, string.Format(CultureInfo.InvariantCulture, "File: {0}", fileName));
			}
			finally
			{
				TS.Logger.WritePerformanceIf(TS.Info, __methodStart);
			}
		}

		/// <summary>
		/// Determines if the file should be moved.
		/// </summary>
		/// <param name="fileInfo">A file info object containing information about the File Move candidate</param>
		protected virtual bool FileShouldMove(FileInfo fileInfo)
		{
			if (fileInfo == null) throw new ArgumentNullException("fileInfo");

			return (fileInfo.Length >= CurrentJob.MinimumFileSize) &&
				((fileInfo.Attributes & CurrentJob.IncludeAttributes) == CurrentJob.IncludeAttributes) &&
				((fileInfo.Attributes & CurrentJob.ExcludeAttributes) == 0);
		}

		/// <summary>
		/// Moves files that are ready to be moved.
		/// </summary>
		/// <param name="filesToMove">A list of file mover info objects representing files that are ready to be moved.</param>
		protected virtual void Move(List<FileMoverInfo> filesToMove)
		{
			if (filesToMove == null) throw new ArgumentNullException("filesToMove");

			// Sort by created date, then move
			filesToMove.Sort((a, b) => a.CreateDate.CompareTo(b.CreateDate));
			filesToMove.ForEach(Move);
		}

		/// <summary>
		/// Confirms that the file size has not been changed, and that the file is available to be moved,
		/// then it copies it to any Copy Targets and moves it to the specified target directory.
		/// </summary>
		/// <param name="moverInfo">The file mover info object representing the file to be moved.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification="Multiple exceptions are possible which indicate the file cannot be moved.")]
		protected virtual void Move(FileMoverInfo moverInfo)
		{
			if (moverInfo == null) throw new ArgumentNullException("moverInfo");

			if (File.Exists(moverInfo.SourceFileName))
			{
				long size = new FileInfo(moverInfo.SourceFileName).Length;
				bool canMove = true;

				if (moverInfo.Size == size)
				{
					try
					{
						using (Stream stream = File.Open(moverInfo.SourceFileName, FileMode.Open, FileAccess.Read, FileShare.None))
						{
							canMove = stream.CanRead;
						}
					}
					catch (Exception ex)
					{
						TS.Logger.WriteExceptionIf(TS.Verbose, ex);
						canMove = false;
					}
				}
				else
				{
					canMove = false;
				}

				if (canMove && File.Exists(moverInfo.SourceFileName))
				{
					try
					{
						CurrentJob.CopyTargets.ForEach(copyTarget => Copy(copyTarget, moverInfo));

						moverInfo.TargetFileName = IOExtensions.CombinePath(CurrentJob.TargetPath, moverInfo.RelativeFileName);

						TS.Logger.WriteLineIf(TS.Info, TS.Categories.Event, "Moving file '{0}' to '{1}'", moverInfo.RelativeFileName, moverInfo.TargetFileName);

						IOExtensions.CreateDirectoryIfNotExists(Path.GetDirectoryName(moverInfo.TargetFileName));
						if (CurrentJob.OverwriteExisting) IOExtensions.DeleteIfExists(moverInfo.TargetFileName);

						File.Move(moverInfo.SourceFileName, moverInfo.TargetFileName);
						moverInfo.Moved = true;
						FileCount++;
					}
					catch (Exception ex)
					{
						TS.Logger.WriteLineIf(TS.Warning, TS.Categories.Warning, "Could not move file '{0}' =>> {1}", Path.GetFileName(moverInfo.SourceFileName), ex.GetExceptionMessage());

						moverInfo.TimeToMove = DateTime.MaxValue; // Force it wait a bit longer before resending.
						moverInfo.ErrorCount++;
					}
				}
				else
				{
					TS.Logger.WriteLineIf(TS.Warning, TS.Categories.Warning, "File not yet ready to move {0}.", Path.GetFileName(moverInfo.SourceFileName));

					moverInfo.TimeToMove = DateTime.MaxValue;
					moverInfo.Size = size;
				}
			}
			else
			{
				TS.Logger.WriteLineIf(TS.Warning, TS.Categories.Warning, "Unable to move file '{0}': File does not exist.", moverInfo.SourceFileName);
			}
		}

		/// <summary>
		/// Copies a file to the specified target.
		/// </summary>
		/// <param name="copyTarget">The Copy Target representing the directory to which the file will be copied.</param>
		/// <param name="moverInfo">The file mover info object representing the file to be copied.</param>
		protected virtual void Copy(CopyTarget copyTarget, FileMoverInfo moverInfo)
		{
			if (copyTarget == null) throw new ArgumentNullException("copyTarget");
			if (moverInfo == null) throw new ArgumentNullException("moverInfo");

			try
			{
				string targetPath = IOExtensions.CombinePath(copyTarget.TargetPath, moverInfo.RelativeFileName);

				TS.Logger.WriteLineIf(TS.Info, TS.Categories.Event, "Copying '{0}' to '{1}'", moverInfo.RelativeFileName, targetPath);

				IOExtensions.CreateDirectoryIfNotExists(Path.GetDirectoryName(targetPath));

				// NOTE: Not using the File.Copy(source, target, overwriteExisting) method on purpose.
				//   If the copyTarget setting is to not overwrite, and to fail on error,
				//   then this method will throw an exception if the file already exists
				//   and we don't want false failures.
				if (copyTarget.OverwriteExisting) IOExtensions.DeleteIfExists(targetPath);
				if (!File.Exists(targetPath)) File.Copy(moverInfo.SourceFileName, targetPath);

				// Copy was successful, add it to the file mover info copy targets list.
				moverInfo.CopyTargets.Add(targetPath);
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(TS.Warning, ex, TS.Categories.Warning);
				if (copyTarget.FailOnError) throw;
			}
		}

		/// <summary>
		/// Used by derived classes to be notified when a file has been successfully moved.
		/// </summary>
		/// <param name="addedFile">The file mover info object representing the file that has been moved.</param>
		protected virtual void NotifyAdded(FileMoverInfo addedFile)
		{
			if (addedFile == null) throw new ArgumentNullException("addedFile");

			TS.Logger.WriteLineIf(TS.Info, TS.Categories.Info, "Added file '{0}' with a create date of '{1:yyyy-MM-dd HH:mm:ss}'.",
				addedFile.RelativeFileName, addedFile.CreateDate);

			// Derived classes may provide implementation.
		}

		/// <summary>
		/// Processes each file that has been moved for notification.
		/// </summary>
		/// <param name="filesMoved">The list of file mover info objects representing files that have been moved.</param>
		protected virtual void NotifyMoved(List<FileMoverInfo> filesMoved)
		{
			if (filesMoved == null) throw new ArgumentNullException("filesMoved");
			filesMoved.ForEach(NotifyMoved);
		}

		/// <summary>
		/// Used by derived classes to be notified when a file has been successfully moved.
		/// </summary>
		/// <param name="movedFile">The file mover info object representing the file that has been moved.</param>
		protected virtual void NotifyMoved(FileMoverInfo movedFile)
		{
			if (movedFile == null) throw new ArgumentNullException("movedFile");
			// Derived classes may provide implementation.
		}

		/// <summary>
		/// Processes each file that has failed for notification.
		/// </summary>
		/// <param name="failedToMoveList">The list of file mover info objects representing files that have failed.</param>
		protected virtual void NotifyFailed(List<FileMoverInfo> failedToMoveList)
		{
			if (failedToMoveList == null) throw new ArgumentNullException("failedToMoveList");
			failedToMoveList.ForEach(NotifyFailed);
		}

		/// <summary>
		/// Used by derived classes to be notified when a file has failed to be moved.
		/// </summary>
		/// <param name="failedFile">The file mover info object representing the file that has failed.</param>
		protected virtual void NotifyFailed(FileMoverInfo failedFile)
		{
			if (failedFile == null) throw new ArgumentNullException("failedFile");

			failedFile.Failed = true;
			TS.Logger.WriteLineIf(TS.Warning, TS.Categories.Warning, "Unable to process file '{0}'.", failedFile.SourceFileName);
		}

		/// <summary>
		/// Writes the File Mover Processor mover info to an Xml Writer.
		/// </summary>
		/// <param name="writer">The Xml Writer to which the mover info will be written</param>
		protected override void WriteStatusXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			writer.WriteAttribute("Moved", FileCount);
			ProcessorSettings.Jobs.ForEach(job => WriteStatusXml(writer, job));
		}

		/// <summary>
		/// Writes the File Mover Job mover info to an Xml Writer.
		/// </summary>
		/// <param name="writer">The Xml Writer to which the mover info will be written</param>
		/// <param name="job">The File Mover Job object.</param>
		protected virtual void WriteStatusXml(XmlWriter writer, FileMoverJob job)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			if (job == null) throw new ArgumentNullException("job");

			writer.WriteStartElement("Job");

			writer.WriteAttribute("Name", job.Name);
			writer.WriteAttribute("Failed", job.FileList.Count(item => item.Failed));
			writer.WriteAttribute("ReadyToMove", job.FileList.Count(item => item.TimeToMove != DateTime.MaxValue));
			writer.WriteAttribute("Waiting", job.FileList.Count(item => !item.Failed && (item.TimeToMove == DateTime.MaxValue)));

			WriteAdditionalJobStatusXml(writer, job);

			writer.WriteEndElement();
		}

		/// <summary>
		/// Writes additional xml data for the File Mover Job to an Xml Writer
		/// </summary>
		/// <param name="writer">The Xml Writer to which the mover info will be written</param>
		/// <param name="job">The File Mover Job object.</param>
		/// <remarks>Overriding classes should add Xml Attributes before calling base method.</remarks>
		protected virtual void WriteAdditionalJobStatusXml(XmlWriter writer, FileMoverJob job)
		{
			if (job == null) throw new ArgumentNullException("job");
			job.FileList.ForEach(info => WriteStatusXml(writer, info));
		}

		/// <summary>
		/// Writes the File mover info object's information to an Xml Writer.
		/// </summary>
		/// <param name="writer">The Xml Writer to which the mover info will be written</param>
		/// <param name="moverInfo">The File mover info object.</param>
		protected virtual void WriteStatusXml(XmlWriter writer, FileMoverInfo moverInfo)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			if (moverInfo == null) throw new ArgumentNullException("moverInfo");

			string timeToMove = (moverInfo.TimeToMove == DateTime.MaxValue) ? "Never" : moverInfo.TimeToMove.ToString("HH:mm", CultureInfo.InvariantCulture);

			writer.WriteStartElement("Filemover info");

			writer.WriteAttribute("FileName", moverInfo.RelativeFileName);
			writer.WriteAttribute("Failed", moverInfo.Failed);
			writer.WriteAttribute("Size", moverInfo.Size);
			writer.WriteAttribute("TimeToMove", timeToMove);

			writer.WriteEndElement();
		}
	}
}
