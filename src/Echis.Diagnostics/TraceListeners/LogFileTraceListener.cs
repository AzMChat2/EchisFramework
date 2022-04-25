using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// The Log File Trace Listener class represents a Filtered Trace Listener which writes trace messages to a rolling file.
	/// </summary>
	public class LogFileTraceListener : FilteredTraceListener
	{
		/// <summary>
		/// Gets or sets the format string for the Date/Time Stamp.
		/// </summary>
		protected string TimestampFormat { get; set; }
		/// <summary>
		/// Gets or sets the Path of the Log File.
		/// </summary>
		protected string LogPath { get; set; }
		/// <summary>
		/// Gets or sets the Path of archived log files.
		/// </summary>
		protected string ArchivePath { get; set; }
		/// <summary>
		/// Gets or sets a value which indicates if the archive folder will be organized using folders by date.
		/// </summary>
		protected bool UseDateFolder { get; set; }
		/// <summary>
		/// Gets or sets the name of the Log File.  The date of the log file will be appended to the end of the filename.
		/// </summary>
		protected string FileName { get; set; }
		/// <summary>
		/// Gets or sets the current date of the Log File.
		/// </summary>
		protected DateTime CurrentDate { get; set; }
		/// <summary>
		/// Gets or sets Stream Writer used to write to the Log File.
		/// </summary>
		protected StreamWriter LogWriter { get; set; }

		/// <summary>
		/// Gets or sets the extension used for all Log Files.
		/// </summary>
		protected string LogFileExt { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			Justification = "fff and ss are parts of date/time format")]
		public LogFileTraceListener()
		{
			TimestampFormat = "HH:mm:ss.fff";
			CurrentDate = DateTime.MinValue;
			LogFileExt = "log";
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~LogFileTraceListener()
		{
			Dispose(false);
		}

		/// <summary>
		/// Flag indicating if the TraceListener and LogFile stream is closed
		/// </summary>
		private bool _isClosed;
		/// <summary>
		/// Closes the log file.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "TraceListener is closing/disposing, any exception should be ignored.")]
		public override void Close()
		{
			if (!_isClosed)
			{
				try
				{
					if ((LogWriter != null) && (LogWriter.BaseStream.CanWrite)) LogWriter.Dispose();
				}
				catch { }
				finally
				{
					LogWriter = null;
				}
				base.Close();
			}
			_isClosed = true;
		}

		/// <summary>
		/// Closes the output stream and releases unmanaged resources used by the TraceListener.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			Close();
			base.Dispose(disposing);
		}

		/// <summary>
		/// Sets the specified initialization parameter to the specified value.
		/// </summary>
		/// <param name="paramName">The name of the specified initialization parameter.</param>
		/// <param name="paramValue">The new value for the the initialization parameter.</param>
		protected override void SetParameter(string paramName, string paramValue)
		{
			if (paramName == null) throw new ArgumentNullException("paramName");

			base.SetParameter(paramName, paramValue);

			if (paramName.Equals("path", StringComparison.OrdinalIgnoreCase))
			{
				LogPath = paramValue;
				if (!LogPath.EndsWith("\\", StringComparison.Ordinal))
				{
					LogPath += "\\";
				}
			}
			else if (paramName.Equals("filename", StringComparison.OrdinalIgnoreCase))
			{
				string threadName = (string.IsNullOrEmpty(ThreadName)) ? string.Empty : ThreadName.Replace(" ", string.Empty);
				FileName = string.Format(CultureInfo.InvariantCulture, paramValue, threadName);
			}
			else if (paramName.Equals("fileext", StringComparison.OrdinalIgnoreCase))
			{
				LogFileExt = paramValue;
			}
			else if (paramName.Equals("archivepath", StringComparison.OrdinalIgnoreCase))
			{
				ArchivePath = paramValue;
				if (!ArchivePath.EndsWith("\\", StringComparison.Ordinal))
				{
					ArchivePath += "\\";
				}
			}
			else if (paramName.Equals("usedatefolder", StringComparison.OrdinalIgnoreCase))
			{
				bool useDateFolder;
				if (bool.TryParse(paramValue, out useDateFolder))
				{
					UseDateFolder = useDateFolder;
				}
			}
			else if (paramName.Equals("timestampformat", StringComparison.OrdinalIgnoreCase))
			{
				TimestampFormat = paramValue;
			}
		}

		/// <summary>
		/// Gets the full path and filename of the log file.
		/// </summary>
		private string FullFileName
		{
			get { return string.Format(CultureInfo.InvariantCulture, "{0}{1}.{2}", LogPath, FileName, LogFileExt); }
		}

		private StringBuilder _errorMessages;
		/// <summary>
		/// Checks the current log file date against the current date.  If different, closes the existing log file,
		/// renames it using the log file date and opens a new Log File.
		/// </summary>
		[DebuggerHidden]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "File operations can throw a variety of exceptions, our handling is the same in each case: log and ignore.")]
		protected void CheckDate()
		{
			if (_errorMessages == null) _errorMessages = new StringBuilder(256, IOExtensions.BufferSize.Kilobytes.Four);

			try
			{
				if (DateTime.Today > CurrentDate)
				{
					if (LogWriter == null)
					{
						if (File.Exists(FullFileName))
						{
							DateTime fileDate = new FileInfo(FullFileName).LastWriteTime;
							if (DateTime.Today > fileDate)
							{
								ArchiveFile(fileDate);
							}
						}
					}
					else
					{
						LogWriter.Flush();
						LogWriter.Dispose();
						LogWriter = null;

						ArchiveFile(CurrentDate);
					}

					CurrentDate = DateTime.Today;

					IOExtensions.CreateDirectoryIfNotExists(LogPath);
					if (File.Exists(FullFileName) && _archiveError)
					{
						OpenAlternateFile();
					}
					else
					{
						LogWriter = new StreamWriter(FullFileName, true);
					}
				}
			}
			catch (Exception ex)
			{
				LogFileException("{0} - Warning: Error accessing log file. {1}",
					DateTime.Now.ToString(TimestampFormat, CultureInfo.InvariantCulture), ex.GetExceptionMessage());

				OpenAlternateFile();
			}
			finally
			{
				if (_errorMessages.Length != 0) WriteToLog(_errorMessages.ToString(), false);
				_errorMessages = null;
			}
		}

		[DebuggerHidden]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "File operations can throw a variety of exceptions, our handling is the same in each case: ignore.")]
		private void OpenAlternateFile()
		{
			try
			{
				// There was an issue archiving the old log.  Let's add the current date to the name of the file.
				string newExt = string.Format(CultureInfo.InvariantCulture, "{0:yyyyMMdd-HHmm}.{1}", CurrentDate, IOExtensions.GetExtension(FullFileName));
				string fileName = Path.ChangeExtension(FullFileName, newExt);

				LogFileException("{0} - Warning: Old log file still exists opening new log file as '{1}'.",
					DateTime.Now.ToString(TimestampFormat, CultureInfo.InvariantCulture), Path.GetFileName(fileName));

				LogWriter = new StreamWriter(fileName, true);
			}
			catch { }
		}

		private bool _archiveError;

		/// <summary>
		/// Archives the previous log file.
		/// </summary>
		/// <param name="archiveDate">The date of the previous log file.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "File operations can throw a variety of exceptions, our handling is the same in each case: log and ignore.")]
		protected void ArchiveFile(DateTime archiveDate)
		{
			try
			{
				_archiveError = false;
				string format = UseDateFolder ? "{0}{2:yyyy-MM-dd}\\{1}.{3}" : "{0}{1}_{2:yyyy-MM-dd}.{3}";
				string path = string.IsNullOrEmpty(ArchivePath) ? LogPath : ArchivePath;

				string archiveFilename = string.Format(CultureInfo.InvariantCulture, format, path, FileName, archiveDate, LogFileExt);
				string originalArchiveName = archiveFilename;
				IOExtensions.CreateDirectoryIfNotExists(Path.GetDirectoryName(archiveFilename));

				int count = 0;
				while (File.Exists(archiveFilename))
				{
					archiveFilename = Path.ChangeExtension(originalArchiveName, string.Format(CultureInfo.InvariantCulture, "{0}.{1}", ++count, LogFileExt));
				}

				File.Move(FullFileName, archiveFilename);
			}
			catch (Exception ex)
			{
				_archiveError = true;
				LogFileException("{0} - Warning: Error archiving log file. {1}",
					DateTime.Now.ToString(TimestampFormat, CultureInfo.InvariantCulture), ex.GetExceptionMessage());
			}
		}

		private void LogFileException(string message, params object[] args)
		{
			try
			{
				if ((_errorMessages != null) && ((_errorMessages.Length + message.Length) < _errorMessages.MaxCapacity))
				{
					message = string.Format(CultureInfo.InvariantCulture, message, args);
					_errorMessages.AppendLine(message);
				}
			}
			catch (ArgumentOutOfRangeException) { }
		}

		/// <summary>
		/// Writes a message to the log file.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="checkDate">Flag indicating if the date should be checked in order to archive the log file if necessary.</param>
		protected void WriteToLog(string message, bool checkDate)
		{
			if (message != null)
			{
				if (checkDate) CheckDate();
				if (LogWriter != null) LogWriter.Write(message);
			}
		}

		/// <summary>
		/// Stores the last message category used for the Write method.
		/// </summary>
		private string _lastMessageCategory;

		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected override void WriteMessage(string message, string category)
		{
			string messageFormat = null;

			if ((_lastMessageCategory == category) || (category == null))
			{
				messageFormat = "{1}";
				if (category == null) _lastMessageCategory = string.Empty;
			}
			else if (_lastMessageCategory == null)
			{
				messageFormat = "\t\t{0}: {1}";
				_lastMessageCategory = category;
			}
			else
			{
				messageFormat = "\r\n\t\t{0}: {1}";
				_lastMessageCategory = category;
			}

			WriteToLog(string.Format(CultureInfo.InvariantCulture, messageFormat, category, message), true);
		}

		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected override void WriteMessageLine(string message, string category)
		{
			string messageFormat = null;

			if (_lastMessageCategory == null)
			{
				messageFormat = "{0} - {1}: {2}\r\n";
			}
			else
			{
				_lastMessageCategory = null;
				messageFormat = "{2}\r\n";
			}

			WriteToLog(string.Format(CultureInfo.InvariantCulture, messageFormat, DateTime.Now.ToString(TimestampFormat, CultureInfo.InvariantCulture), category, message), true);
		}

		/// <summary>
		/// Writes any buffered trace messages to the log file.
		/// </summary>
		public override void Flush()
		{
			base.Flush();
			if (LogWriter != null) LogWriter.Flush();
		}
	}
}
