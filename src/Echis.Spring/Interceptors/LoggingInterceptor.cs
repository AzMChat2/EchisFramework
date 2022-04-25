using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AopAlliance.Intercept;

namespace System.Spring.Interceptors
{
	// IMethodInvocation is not CLS compliant, since it comes from the Spring framework, there's nothing we
	// can do about it, so disable CLS compliancy for this class.

	/// <summary>
	/// Interceptor used to write MethodCall, Exception and Performance Trace messages using the StandardMessages device.
	/// </summary>
	[CLSCompliant(false)]
	public class LoggingInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public LoggingInterceptor()
		{
			StackLevel = TraceLevel.Verbose;
			ErrorLevel = TraceLevel.Error;
			PerformanceLevel = TraceLevel.Info;
			ContextId = System.Diagnostics.Settings.Values.DefaultContext;
			ExceptionHandler = new DefaultExceptionHandler();
		}

		/// <summary>
		/// Gets or sets the optional Exception Handler (default behavior is to rethrow the exception after logging).
		/// </summary>
		public IExceptionHandler ExceptionHandler { get; set; }

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
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Exception handling is accomplished via the ExceptionHandler which expected to handle specific exceptions and/or rethrow the exception")]
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
				return ExceptionHandler.HandleException(invocation.Method, ex, invocation.Arguments);
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

		/// <summary>
		/// Default behavior, rethrow the exception.
		/// </summary>
		private class DefaultExceptionHandler : IExceptionHandler
		{
			public object HandleException(MethodInfo methodInfo, Exception ex, params object[] arguments)
			{
				throw ex;
			}
		}
	}
}
