using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace System.Diagnostics.Loggers
{
	/// <summary>
	/// Provides a standard set of Trace messages.
	/// </summary>
	public sealed class TraceLogger : TraceLoggerBase
	{
		/// <summary>
		/// Contains constants used by the StandardTraceMessages class
		/// </summary>
		private static class Constants
		{
			/// <summary>
			/// Message indicating method execution failed.
			/// </summary>
			/// <remarks>Format accepts 3 parameters. 0=Class Name, 1=Method Name, 2=Exception</remarks>
			public const string Error = "{0}.{1} Failed: {2}";
			/// <summary>
			/// Message indicating method execution started.
			/// </summary>
			/// <remarks>Format accepts 2 parameters. 0=Class Name, 1=Method Name</remarks>
			public const string Method = "{0}.{1}";
			/// <summary>
			/// Message indicating method execution completed.
			/// </summary>
			/// <remarks>Format accepts 3 parameters. 0=Class Name, 1=Method Name, 2=Execution Time</remarks>
			public const string Performance = "{0}.{1}: {2}";
			/// <summary>
			/// Message header for Method Performance Output.
			/// </summary>
			public const string PerformanceInfo = "\tClass Name\tMethod Name\tTotal Calls\tTotal Execution Time\tAverage Execution Time";
			/// <summary>
			/// Message line for Method Performance Output. 
			/// </summary>
			/// <remarks>Format accepts 5 parameters. 0=Class Name, 1=Method Name, 2=Total Calls, 3=Total Execution Time, 4=Average Execution Time</remarks>
			public const string PerformanceFormat = "\t{0}\t{1}\t{2}\t{3:N3}\t{4:N4}";
		}

		/// <summary>
		/// Stores performance information about a particular method.
		/// </summary>
		private class PerformanceInfo
		{
			/// <summary>
			/// The name of the class to which the method belongs
			/// </summary>
			public string ClassName { get; set; }
			/// <summary>
			/// The name of the method.
			/// </summary>
			public string MethodName { get; set; }
			/// <summary>
			/// A running total of execution time for the method.
			/// </summary>
			public double ExecutionTime { get; set; }
			/// <summary>
			/// A running total of the number of times the method has been called.
			/// </summary>
			public int CallCount { get; set; }
		}

		/// <summary>
		/// Stores method performance information.
		/// </summary>
		private Dictionary<string, PerformanceInfo> methodPerformance = new Dictionary<string, PerformanceInfo>();

		/// <summary>
		/// Object used for thread safety in adding keys to the methodPerformance dictionary
		/// </summary>
		private object perfLock = new object();

		/// <summary>
		/// Records performance information for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		public override void RecordPerformance(MethodBase mb, TimeSpan elapsed)
		{
			if (mb == null) throw new ArgumentNullException("mb");

			string key = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", mb.DeclaringType.Name, mb.Name);

			if (methodPerformance.ContainsKey(key))
			{
				methodPerformance[key].ExecutionTime += elapsed.TotalSeconds;
				methodPerformance[key].CallCount++;
			}
			else
			{
				lock (perfLock)
				{
					if (methodPerformance.ContainsKey(key))
					{
						methodPerformance[key].ExecutionTime += elapsed.TotalSeconds;
						methodPerformance[key].CallCount++;
					}
					else
					{
						PerformanceInfo info = new PerformanceInfo();
						info.ClassName = mb.DeclaringType.FullName;
						info.MethodName = mb.Name;
						info.ExecutionTime = elapsed.TotalSeconds;
						info.CallCount = 1;
						methodPerformance.Add(key, info);
					}
				}
			}
		}

		/// <summary>
		/// Writes a standard method call Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="category">The logging category for the message.</param>
		public override void WriteMethodCallMessage(MethodBase mb, string category)
		{
			if (mb == null) throw new ArgumentNullException("mb");

			WriteLine(category, Constants.Method, mb.DeclaringType.FullName, mb.Name);
		}

		/// <summary>
		/// Writes a standard exception Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="ex">The exception which has been caught.</param>
		/// <param name="category">The logging category for the message.</param>
		public override void WriteExceptionMessage(MethodBase mb, Exception ex, string category)
		{
			if (mb == null) throw new ArgumentNullException("mb");
			if (ex == null) throw new ArgumentNullException("ex");

			WriteLine(category, Constants.Error, mb.DeclaringType.FullName, mb.Name, ex.GetExceptionMessage());
		}

		/// <summary>
		/// Writes a standard performance Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		/// <param name="category">The logging category for the message.</param>
		public override void WritePerformanceMessage(MethodBase mb, TimeSpan elapsed, string category)
		{
			if (mb == null) throw new ArgumentNullException("mb");

			WriteLine(category, Constants.Performance, mb.DeclaringType.FullName, mb.Name, elapsed.TotalMilliseconds);
		}

		/// <summary>
		/// Writes the collection of Performance data to the Trace output.
		/// </summary>
		public override void OutputPerformanceStats()
		{
			if (methodPerformance.Count != 0)
			{
				WriteLine(string.Empty);
				WriteMessage(TS.Categories.Performance, Constants.PerformanceInfo);

				foreach (PerformanceInfo item in methodPerformance.Values)
				{
					double average = ((item.ExecutionTime / item.CallCount) * 1000);
					WriteLine("\t", Constants.PerformanceFormat, item.ClassName, item.MethodName, item.CallCount, item.ExecutionTime, average);
				}
			}

			Flush();
		}
	}
}
