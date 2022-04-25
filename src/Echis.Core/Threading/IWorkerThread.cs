using System;
using System.Threading;

namespace System.Threading
{
	/// <summary>
	/// Definition for worker threads.
	/// </summary>
	public interface IWorkerThread
	{
		/// <summary>
		/// Event raised when the thread is starting.
		/// </summary>
		event EventHandler Starting;

		/// <summary>
		/// Event raised when the thread has been stopped.
		/// </summary>
		event EventHandler Stopped;

		/// <summary>
		/// Event raised when the thread has completed normally.
		/// </summary>
		event EventHandler Completed;

		/// <summary>
		/// Event raised when an unhandled exception is caught.
		/// </summary>
		event EventHandler<ExceptionEventArgs> ProcessException;


		/// <summary>
		/// The thread on which the process is executing.
		/// </summary>
		Thread ProcessThread { get; }

		/// <summary>
		///  Flag indicating if trace messages should be written.
		/// </summary>
		bool Tracing { get; set; }

		/// <summary>
		/// Flag indicating if the thread is currently running 
		/// </summary>
		bool IsRunning { get; }


		/// <summary>
		/// Starts the Worker Thread.
		/// </summary>
		void Start();

		/// <summary>
		/// Sends a stop request.
		/// </summary>
		void StopProcess();

		/// <summary>
		/// Aborts the processing.
		/// </summary>
		void Abort();
	}
}
