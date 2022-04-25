using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.IO
{
	/// <summary>
	/// Contains utility methods for IO
	/// </summary>
	public static class IOExtensions
	{
		#region Buffer Size
		/// <summary>
		/// Constains constants used to create byte array buffers.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This is a non-instantiable static class which only contains constants.")]
		public static class BufferSize
		{
			/// <summary>
			/// Constains constants used to create byte array buffers in Kilobytes.
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
				Justification = "This is a non-instantiable static class which only contains constants.")]
			public static class Kilobytes
			{
				/// <summary>
				/// One (1) Kilobyte
				/// </summary>
				public const int One = 0x400;
				/// <summary>
				/// Two (2) Kilobytes
				/// </summary>
				public const int Two = 0x800;
				/// <summary>
				/// Four (4) Kilobytes
				/// </summary>
				public const int Four = 0x1000;
				/// <summary>
				/// Eight (8) Kilobytes
				/// </summary>
				public const int Eight = 0x2000;
				/// <summary>
				/// Sixteen (16) Kilobytes
				/// </summary>
				public const int Sixteen = 0x4000;
				/// <summary>
				/// Thirty-Two (32) Kilobytes
				/// </summary>
				public const int ThirtyTwo = 0x8000;
				/// <summary>
				/// Sixty-Four (64) Kilobytes
				/// </summary>
				public const int SixtyFour = 0x10000;
				/// <summary>
				/// One Hundred Twenty-Eight (128) Kilobytes
				/// </summary>
				public const int OneTwentyEight = 0x20000;
				/// <summary>
				/// Two Hundred Fifty-Six (256) Kilobytes
				/// </summary>
				public const int TwoFiftySix = 0x40000;
				/// <summary>
				/// Five Hundred Twelve (512) Kilobytes
				/// </summary>
				public const int FiveTwelve = 0x80000;
			}

			/// <summary>
			/// Constains constants used to create byte array buffers in Megabytes.
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
				Justification = "This is a non-instantiable static class which only contains constants.")]
			public static class Megabytes
			{
				/// <summary>
				/// One (1) Megabyte
				/// </summary>
				public const int One = 0x100000;
				/// <summary>
				/// Two (2) Megabytes
				/// </summary>
				public const int Two = 0x200000;
				/// <summary>
				/// Four (4) Megabytes
				/// </summary>
				public const int Four = 0x400000;
				/// <summary>
				/// Eight (8) Megabytes
				/// </summary>
				public const int Eight = 0x800000;
				/// <summary>
				/// Sixteen (16) Megabytes
				/// </summary>
				public const int Sixteen = 0x1000000;
				/// <summary>
				/// Thirty-Two (32) Megabytes
				/// </summary>
				public const int ThirtyTwo = 0x2000000;
				/// <summary>
				/// Sixty-Four (64) Megabytes
				/// </summary>
				public const int SixtyFour = 0x4000000;
				/// <summary>
				/// One Hundred Twenty-Eight (128) Megabytes
				/// </summary>
				public const int OneTwentyEight = 0x8000000;
				/// <summary>
				/// Two Hundred Fifty-Six (256) Megabytes
				/// </summary>
				public const int TwoFiftySix = 0x10000000;
				/// <summary>
				/// Five Hundred Twelve (512) Megabytes
				/// </summary>
				public const int FiveTwelve = 0x20000000;
			}

			/// <summary>
			/// Constains constants used to create byte array buffers in Gigabytes.
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
				Justification = "This is a non-instantiable static class which only contains constants.")]
			public static class Gigabytes
			{
				/// <summary>
				/// One (1) Gigabyte
				/// </summary>
				public const int One = 0x40000000;
			}
		}
		#endregion

		#region OutputData
		/// <summary>
		/// Creates or opens a file and writes the data provided.
		/// </summary>
		/// <param name="fileName">The path and filename of the file.</param>
		/// <param name="data">The data to be written.</param>
		/// <param name="append">A flag indicating if the data should be appended to the end of the file.</param>
		/// <remarks>If append is false and the file already exists, it will be over-written.</remarks>
		/// <param name="bubbleException">A flag indicating if exceptions should bubble up to the caller.</param>
		public static void OutputData(string fileName, string data, bool append, bool bubbleException)
		{
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
			if (data == null) throw new ArgumentNullException("data");

			try
			{
				CreateDirectoryIfNotExists(Path.GetDirectoryName(fileName));
				FileMode mode = append ? FileMode.OpenOrCreate : FileMode.Create;

				using (Stream stream = File.Open(fileName, mode, FileAccess.Write, FileShare.None))
				{
					if (append) stream.Position = stream.Length;
					StreamWriter writer = new StreamWriter(stream);
					writer.Write(data);
					writer.Flush();
				}
			}
			catch (Exception)
			{
				if (bubbleException) throw;
			}
		}

		/// <summary>
		/// Creates a new file and writes the data provided to the new file.
		/// </summary>
		/// <remarks>If the file already exists, it will be over-written.</remarks>
		/// <param name="fileName">The path and filename of the file.</param>
		/// <param name="data">The data to be written.</param>
		/// <param name="bubbleException">A flag indicating if exceptions should bubble up to the caller.</param>
		public static void OutputData(string fileName, string data, bool bubbleException)
		{
			OutputData(fileName, data, false, bubbleException);
		}

		/// <summary>
		/// Creates or opens a file and writes the data provided.
		/// </summary>
		/// <param name="condition">The condition to evaluate.</param>
		/// <param name="fileName">The path and filename of the file.</param>
		/// <param name="data">The data to be written.</param>
		/// <param name="append">A flag indicating if the data should be appended to the end of the file.</param>
		/// <remarks>If append is false and the file already exists, it will be over-written.</remarks>
		/// <param name="bubbleException">A flag indicating if exceptions should bubble up to the caller.</param>
		public static void OutputDataIf(bool condition, string fileName, string data, bool append, bool bubbleException)
		{
			if (condition) OutputData(fileName, data, append, bubbleException);
		}

		/// <summary>
		/// Creates a new file and writes the data provided to the new file, if the condition is true.
		/// </summary>
		/// <remarks>If the file already exists, it will be over-written.</remarks>
		/// <param name="condition">The condition to evaluate.</param>
		/// <param name="fileName">The path and filename of the file.</param>
		/// <param name="data">The data to be written.</param>
		/// <param name="bubbleException">A flag indicating if exceptions should bubble up to the caller.</param>
		public static void OutputDataIf(bool condition, string fileName, string data, bool bubbleException)
		{
			if (condition) OutputData(fileName, data, false, bubbleException);
		}
		#endregion

		/// <summary>
		/// Copies a file from the source to the target.  Will create the target directory if it does not exist.
		/// </summary>
		/// <param name="source">The file to be copied.</param>
		/// <param name="target">The destination file to be created.</param>
		/// <param name="overwriteExisting">Flag indicating if existing file should be overwritten.</param>
		public static void CopyFile(string source, string target, bool overwriteExisting)
		{
			if (string.IsNullOrEmpty(source)) throw new ArgumentNullException("source");
			if (string.IsNullOrEmpty(target)) throw new ArgumentNullException("target");

			CreateDirectoryIfNotExists(Path.GetDirectoryName(target));
			if (overwriteExisting) DeleteIfExists(target);
			File.Copy(source, target);
		}

		/// <summary>
		/// Moves a file from the source to the target.  Will create the target directory if it does not exist.
		/// </summary>
		/// <param name="source">The file to be moved.</param>
		/// <param name="target">The destination pf the file to be moved.</param>
		/// <param name="overwriteExisting">Flag indicating if existing file should be overwritten.</param>
		public static void MoveFile(string source, string target, bool overwriteExisting)
		{
			if (string.IsNullOrEmpty(source)) throw new ArgumentNullException("source");
			if (string.IsNullOrEmpty(target)) throw new ArgumentNullException("target");

			CreateDirectoryIfNotExists(Path.GetDirectoryName(target));
			if (overwriteExisting) DeleteIfExists(target);
			File.Move(source, target);
		}

		/// <summary>
		/// Returns the extension of the specified path string, without the period.
		/// </summary>
		/// <param name="path">The path string from which to get the extension.</param>
		/// <returns>Returns the extension of the specified path string, without the period.</returns>
		/// <remarks>This function is used to retrieve JUST the extension without a preceeding period.<br />
		/// Path.GetExtension returns the extension preceeded with a period.</remarks>
		public static string GetExtension(string path)
		{
			string retVal = Path.GetExtension(path);

			if (!string.IsNullOrEmpty(retVal) && retVal.StartsWith(".", StringComparison.OrdinalIgnoreCase))
			{
				retVal = retVal.Substring(1);
			}

			return retVal;
		}

		/// <summary>
		/// Returns the drive letter of the specified path string.
		/// </summary>
		/// <param name="path">The path string from which to get the drive letter.</param>
		/// <returns>Returns the drive letter of the specified path string.</returns>
		public static string GetDrive(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return null;
			var idx = path.IndexOf(":", StringComparison.OrdinalIgnoreCase);
			return (idx < 0) ? string.Empty : path.Substring(0, idx + 2);
		}

		/// <summary>
		/// Combines two path strings
		/// </summary>
		/// <param name="path1">The first path.</param>
		/// <param name="path2">The second path.</param>
		/// <remarks>This function is used to get around the undesireable behavior of
		/// Path.Combine when the 2nd path starts with a backslash (\).<br/>
		/// Path.Combine when passed the paths "C:\Temp\" and "\Debug\Test.txt" will return only the 2nd path ("\Debug\Test.txt").<br/>
		/// This method will combine "C:\Temp\" and "\Debug\Test.txt" to return "C:\Temp\Debug\Test.txt"</remarks>
		public static string CombinePath(string path1, string path2)
		{
			if (path1 == null) throw new ArgumentNullException("path1");
			if (path2 == null) throw new ArgumentNullException("path2");

			if (path2.StartsWith("\\", StringComparison.OrdinalIgnoreCase) && !path2.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase)) path2 = path2.Substring(1);
			return Path.Combine(path1, path2);
		}

		/// <summary>
		/// The Search Pattern slit character
		/// </summary>
		private static readonly char[] _searchPatternSplitChar = new char[] { '|' };

		/// <summary>
		/// Gets all of the files matching the search pattern in the specified path.
		/// </summary>
		/// <param name="path">The path to search.</param>
		/// <param name="searchPattern">The search pattern(s) to match.  Separate search patterns with the pipe | character.</param>
		/// <param name="recursive">Flag indicating if sub-directories should be included in the search.</param>
		/// <returns>Returns a list of the files in the path matching the search pattern(s) specified.</returns>
		public static List<string> GetFiles(string path, string searchPattern, bool recursive)
		{
			if (string.IsNullOrEmpty(searchPattern)) searchPattern = "*";
			string[] searchPatterns = searchPattern.Split(_searchPatternSplitChar, StringSplitOptions.RemoveEmptyEntries);
			return GetFiles(path, searchPatterns, recursive);
		}

		/// <summary>
		/// Gets all of the files matching the search pattern in the specified path.
		/// </summary>
		/// <param name="path">The path to search.</param>
		/// <param name="searchPatterns">The search patterns to match.</param>
		/// <param name="recursive">Flag indicating if sub-directories should be included in the search.</param>
		/// <returns>Returns a list of the files in the path matching the search pattern(s) specified.</returns>
		private static List<string> GetFiles(string path, string[] searchPatterns, bool recursive)
		{
			if (searchPatterns == null) throw new ArgumentNullException("searchPatterns");

			List<string> retVal = new List<string>();

			SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			searchPatterns.ForEach(searchPattern => retVal.AddRange(Directory.GetFiles(path, searchPattern, searchOption)));

			return retVal;
		}

		/// <summary>
		/// Verifies the specified directory exists, if not the directory is created.
		/// </summary>
		/// <param name="path">The path of the directory to verify or create.</param>
		/// <returns>
		/// Returns true if the directory was successfully created or if the directory already exists.
		/// Returns false if there was an exception caught while attempting to create the directory.
		/// </returns>
		public static bool CreateDirectoryIfNotExists(string path)
		{
			return CreateDirectoryIfNotExists(path, false);
		}

		/// <summary>
		/// Verifies the specified directory exists, if not the directory is created.
		/// </summary>
		/// <param name="path">The path of the directory to verify or create.</param>
		/// <param name="bubbleExceptions">Flag indicating if exceptions should bubble up to the caller.</param>
		/// <returns>
		/// Returns true if the directory was successfully created or if the directory already exists.
		/// Returns false if there was an exception caught while attempting to create the directory (bubbleExceptions must be false).
		/// </returns>
		public static bool CreateDirectoryIfNotExists(string path, bool bubbleExceptions)
		{
			bool retVal = true;

			try
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
			catch
			{
				if (bubbleExceptions) throw;
				retVal = false;
			}

			return retVal;
		}

		/// <summary>
		/// Removes all files and subdirectories from the specified directory.
		/// </summary>
		/// <param name="path">The directory path.</param>
		/// <returns>
		/// Returns true if the directory was cleaned.
		/// Returns false if an exception was caught while attempting to clear the directory.
		/// </returns>
		public static bool ClearDirectory(string path)
		{
			return ClearDirectory(path, false);
		}

		/// <summary>
		/// Removes all files and subdirectories from the specified directory.
		/// </summary>
		/// <param name="path">The directory path.</param>
		/// <param name="bubbleExceptions">Flag indicating if exceptions should bubble up to the caller.</param>
		/// <returns>
		/// Returns true if the directory was cleaned.
		/// Returns false if an exception was caught while attempting to clear the directory.
		/// </returns>
		public static bool ClearDirectory(string path, bool bubbleExceptions)
		{
			bool retVal = true;

			try
			{
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}

				Directory.CreateDirectory(path);
			}
			catch
			{
				if (bubbleExceptions) throw;
				retVal = false;
			}

			return retVal;
		}

		/// <summary>
		/// Copies the contents of one directory to another directory.
		/// </summary>
		/// <param name="source">The directory containing the files to be copied.</param>
		/// <param name="target">The destination directory where the files will be copied.</param>
		public static void CopyDirectoryContents(string source, string target)
		{
			CopyDirectoryContents(source, target, true);
		}

		/// <summary>
		/// Copies the contents of one directory to another directory.
		/// </summary>
		/// <param name="source">The directory containing the files to be copied.</param>
		/// <param name="target">The destination directory where the files will be copied.</param>
		/// <param name="merge">Flag indicating if the target directory contents should be merged with the contents from the source directory.
		/// If true, then any files which exist in the target directory, but not in the source will still remain after the copy and 
		/// files that exist in both target and source will be overwritten from the source.
		/// If false, then the target directory will be cleared before the copy begins; any existing files or directories in the target
		/// directory will be deleted.</param>
		public static void CopyDirectoryContents(string source, string target, bool merge)
		{
			if (string.IsNullOrEmpty(source)) throw new ArgumentNullException("source");
			if (string.IsNullOrEmpty(target)) throw new ArgumentNullException("target");
			if (!Directory.Exists(source)) throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The source directory specified '{0}' does not exist.", source));

			if (!source.EndsWith("\\", StringComparison.OrdinalIgnoreCase)) source += "\\";
			if (!target.EndsWith("\\", StringComparison.OrdinalIgnoreCase)) target += "\\";

			if (merge)
			{
				CreateDirectoryIfNotExists(target);
			}
			else
			{
				ClearDirectory(target);
			}

			string[] files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
			files.ForEach(file => File.Copy(file, GetTargetFile(source, target, file), true));
		}

		private static string GetTargetFile(string source, string target, string fileName)
		{
			string retVal = Path.Combine(target, fileName.Substring(source.Length));
			CreateDirectoryIfNotExists(Path.GetDirectoryName(retVal));
			return retVal;
		}

		/// <summary>
		/// Deletes the specified file if it exists.
		/// </summary>
		/// <param name="fileName">The file to delete.</param>
		/// <returns>
		/// Returns true if the file does not exist or if the file was successfully deleted.
		/// Returns false if there was an exception caught while attempting to delete the file.
		/// </returns>
		public static bool DeleteIfExists(string fileName)
		{
			return DeleteIfExists(fileName, false);
		}

		/// <summary>
		/// Deletes the specified file if it exists.
		/// </summary>
		/// <param name="fileName">The file to delete.</param>
		/// <param name="bubbleExceptions">Flag indicating if exceptions should bubble up to the caller.</param>
		/// <returns>
		/// Returns true if the file does not exist or if the file was successfully deleted.
		/// Returns false if there was an exception caught while attempting to delete the file (bubbleExceptions must be false).
		/// </returns>
		public static bool DeleteIfExists(string fileName, bool bubbleExceptions)
		{
			bool retVal = true;

			try
			{
				if (File.Exists(fileName))
					File.Delete(fileName);
			}
			catch
			{
				if (bubbleExceptions) throw;
				retVal = false;
			}

			return retVal;
		}

		/// <summary>
		/// Peforms an action on every line read from a Stream Reader.
		/// </summary>
		/// <param name="reader">The Stream Reader containing the lines to be read.</param>
		/// <param name="action">The action to be performed on the line read.</param>
		public static void WhileReadLine(this StreamReader reader, Action<string> action)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (action == null) throw new ArgumentNullException("action");

			while (!reader.EndOfStream) action.Invoke(reader.ReadLine());
		}

		/// <summary>
		/// Gets just the first n number of bytes from a file.
		/// </summary>
		/// <param name="fileName">The name of the file from which the header bytes will be read.</param>
		/// <param name="headerLength">The number of bytes to read from the file.</param>
		/// <returns>A byte array containing the first n number of bytes from a file.</returns>
		public static byte[] GetFileHeader(string fileName, int headerLength)
		{
			using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				return GetFileHeader(stream, headerLength);
			}
		}

		/// <summary>
		/// Gets just the first n number of bytes from a file.
		/// </summary>
		/// <param name="stream">The file stream from which the header bytes will be read.</param>
		/// <param name="headerLength">The number of bytes to read from the file.</param>
		/// <returns>A byte array containing the first n number of bytes from a file.</returns>
		public static byte[] GetFileHeader(Stream stream, int headerLength)
		{
			if (stream == null) throw new ArgumentNullException("stream");

			byte[] retVal = new byte[headerLength];

			long position = stream.Position;
			stream.Position = 0;
			stream.Read(retVal, 0, headerLength);
			stream.Position = position;

			return retVal;
		}

	}
}
