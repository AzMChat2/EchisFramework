using System;
using System.Diagnostics;

using AopAlliance.Intercept;

namespace Echis.Spring
{
	// IMethodInvocation is not CLS compliant, since it comes from the Spring framework, there's nothing we
	// can do about it, so disable CLS compliancy for this class.

	/// <summary>
	/// Interceptor used to write MethodCall, Exception and Performance Trace messages using the StandardMessages device.
	/// </summary>
	[CLSCompliant(false)]
	public class PerformanceInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public PerformanceInterceptor()
		{
			StackLevel = TraceLevel.Verbose;
			ErrorLevel = TraceLevel.Error;
			PerformanceLevel = TraceLevel.Info;
			ContextId = Echis.Diagnostics.Settings.Values.DefaultContext;
		}

		/// <summary>
		/// Gets or sets the TraceLevel at which method-call messages will be output to the Trace mechanism.
		/// </summary>
		public TraceLevel StackLevel { get; set; }

		/// <summary>
		/// Gets or sets the TraceLevel at which performance messages will be output to the Trace mechanism.
		/// </summary>
		public TraceLevel PerformanceLevel { get; set; }
	
		/// <summary>
		/// Gets or sets the TraceLevel at which error messages will be output to the Trace mechanism.
		/// </summary>
		public TraceLevel ErrorLevel { get; set; }

		/// <summary>
		/// Gets or sets the Tracing Context Id.
		/// </summary>
		public string ContextId { get; set; }

		/// <summary>
		/// Writes Trace messages using the StandardMessages device.
		/// </summary>
		/// <param name="invocation">Information and access to the method being invoked.</param>
		/// <returns>Returns the results of the invocation.</returns>
		public object Invoke(IMethodInvocation invocation)
		{
			if (invocation == null) throw new ArgumentNullException("invocation");

			if (Check(StackLevel)) TS.Logger.WriteMethodCallMessage(invocation.Method);

			Stopwatch sw = Stopwatch.StartNew();

			try
			{
				return invocation.Proceed();
			}
			catch (Exception ex)
			{
				if (Check(ErrorLevel)) TS.Logger.WriteExceptionMessage(invocation.Method, ex);
				throw;
			}
			finally
			{
				sw.Stop();
				TS.Logger.RecordPerformance(invocation.Method, sw.Elapsed);
				if (Check(PerformanceLevel)) TS.Logger.WritePerformanceMessage(invocation.Method, sw.Elapsed);
			}
		}

		/// <summary>
		/// Checks if a logger message should be written.
		/// </summary>
		private bool Check(TraceLevel level)
		{
			return ((level != TraceLevel.Off) && (TS.Context(ContextId).Level >= level));
		}

	}
}
