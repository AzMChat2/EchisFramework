using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace System.Diagnostics.Loggers
{
	/// <summary>
	/// Provides only performance related Trace messages.
	/// </summary>
	public sealed class PerformanceTraceLogger : TraceLoggerBase
	{
		/// <summary>
		/// Contains constants used by the PerformanceOnlyTraceMessages class.
		/// </summary>
		private static class Constants
		{
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
			public string ClassName { get; set; }
			public string MethodName { get; set; }
			public double ExecutionTime { get; set; }
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
		/// Writes a standard performance Trace message for the Method provided.
		/// </summary>
		/// <param name="mb">The method for which the Trace message will be generated.</param>
		/// <param name="elapsed">The elapsed time to be recorded</param>
		/// <param name="category">The logging category for the message.</param>
		public override void WritePerformanceMessage(MethodBase mb, TimeSpan elapsed, string category)
		{
			if (mb == null) throw new ArgumentNullException("mb");

			WriteLine(TS.Categories.Performance, Constants.Performance, mb.DeclaringType.FullName, mb.Name, elapsed.TotalMilliseconds);
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

		/// <summary>
		/// Not Used.
		/// </summary>
		/// <param name="ex"></param>
		public override void WriteException(Exception ex) { }
		/// <summary>
		/// Not Used.
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="condition"></param>
		public override void WriteExceptionIf(bool condition, Exception ex) { }
		/// <summary>
		/// Not Used.
		/// </summary>
		public override void WriteMethodCall() { }
		/// <summary>
		/// Not Used.
		/// </summary>
		/// <param name="condition"></param>
		public override void WriteMethodCallIf(bool condition) { }
		/// <summary>
		/// Not Used.
		/// </summary>
		/// <param name="mb"></param>
		/// <param name="category"></param>
		public override void WriteMethodCallMessage(MethodBase mb, string category) { }
		/// <summary>
		/// Not Used.
		/// </summary>
		/// <param name="mb"></param>
		/// <param name="ex"></param>
		/// <param name="category"></param>
		public override void WriteExceptionMessage(MethodBase mb, Exception ex, string category) { }
	}
}
