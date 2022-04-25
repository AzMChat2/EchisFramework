using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace System.Diagnostics.Loggers
{
	/// <summary>
	/// Provides Trace messages containing extra information, such as method signatures.
	/// </summary>
	public sealed class ExtendedTraceLogger : TraceLoggerBase
	{
		/// <summary>
		/// Provides constants used by the ExtendedTraceMessages class.
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
			/// Message displaying Method Call parameter Information.
			/// </summary>
			/// <remarks>Format accepts 2 parameters. 0=ParameterInfo</remarks>
			public const string Parameter = "{0}";
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
			public const string PerformanceFormat = "\t{0}\t{1}\t{2}\t{3}\t{4}";
		}

		/// <summary>
		/// Stores performance information about a particular method.
		/// </summary>
		private class PerformanceInfo
		{
			public double ExecutionTime { get; set; }
			public int CallCount { get; set; }
		}

		/// <summary>
		/// Stores method performance information.
		/// </summary>
		private Dictionary<MethodBase, PerformanceInfo> methodPerformance = new Dictionary<MethodBase, PerformanceInfo>();

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

			if (methodPerformance.ContainsKey(mb))
			{
				methodPerformance[mb].ExecutionTime += elapsed.TotalSeconds;
				methodPerformance[mb].CallCount++;
			}
			else
			{
				lock (perfLock)
				{
					if (methodPerformance.ContainsKey(mb))
					{
						methodPerformance[mb].ExecutionTime += elapsed.TotalSeconds;
						methodPerformance[mb].CallCount++;
					}
					else
					{
						PerformanceInfo info = new PerformanceInfo();
						info.ExecutionTime = elapsed.TotalSeconds;
						info.CallCount = 1;
						methodPerformance.Add(mb, info);
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
			mb.GetParameters().ForEach(WriteParameterInfo);
		}

		private void WriteParameterInfo(ParameterInfo param)
		{
			WriteLine(TS.Categories.Param, Constants.Parameter, param);
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

			WriteLine(category, Constants.Error, mb.DeclaringType.FullName, mb.Name, ex);
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

				foreach (KeyValuePair<MethodBase, PerformanceInfo> item in methodPerformance)
				{
					StringBuilder sb = new StringBuilder();
					ParameterInfo[] parameters = item.Key.GetParameters();

					sb.Append(item.Key.Name);
					sb.Append("(");
					if (parameters.Length != 0)
					{
						Array.ForEach(parameters, info => sb.AppendFormat("{0} {1}, ", info.ParameterType.Name, info.Name));
						sb.Remove(sb.Length - 2, 2);
					}
					sb.Append(")");

					WriteLine("\t", Constants.PerformanceFormat, item.Key.DeclaringType.FullName, sb.ToString(),
						item.Value.CallCount, item.Value.ExecutionTime, (item.Value.ExecutionTime / item.Value.CallCount));
				}
			}
			Flush();
		}
	}
}
