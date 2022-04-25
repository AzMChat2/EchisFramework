
namespace System.Diagnostics.Loggers
{
	/// <summary>
	/// Provides a default behavior for standard Trace messages.
	/// </summary>
	/// <remarks>This class intentionally does nothing.  It is used only as a placeholder
	/// which is called when no other IStandardMessages implementation is configured.</remarks>
	internal class DefaultLogger : LoggerBase
	{

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void Write(string message) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void Write(string category, string message) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void WriteLine(string message) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		protected override void WriteMessage(string category, string message) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void WriteMethodCallMessage(Reflection.MethodBase mb, string category) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void WriteExceptionMessage(Reflection.MethodBase mb, Exception ex, string category) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void WritePerformanceMessage(Reflection.MethodBase mb, TimeSpan elapsed, string category) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void RecordPerformance(Reflection.MethodBase mb, TimeSpan elapsed) { }

		/// <summary>
		/// Not Used.
		/// </summary>
		public override void OutputPerformanceStats() { }
	}
}
