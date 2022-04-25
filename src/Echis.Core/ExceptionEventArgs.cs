namespace System
{
	/// <summary>
	/// Event arguments for an Exception Event.
	/// </summary>
	public class ExceptionEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="exception">The exception which caused the event to be fired.</param>
		public ExceptionEventArgs(Exception exception)
			: base()
		{
			Exception = exception;
		}

		/// <summary>
		/// Gets the exception which caused the event to be fired.
		/// </summary>
		public Exception Exception { get; private set; } 
	}
}
