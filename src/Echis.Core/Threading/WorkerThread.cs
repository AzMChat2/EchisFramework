using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading
{
	/// <summary>
	/// Base class for creating worker threads.
	/// </summary>
	public abstract class WorkerThread : IWorkerThread
	{
		/// <summary>
		/// Gets the manual reset event which signals when the thread has completed execution.
		/// </summary>
		/// <remarks>Originally this was public.  Instead I have made this private, and exposed the WaitOne methods through the WorkerThread class itself.</remarks>
		private ManualResetEvent ThreadComplete { get; set; }
		/// <summary>
		/// Event raised when the thread is starting.
		/// </summary>
		public event EventHandler Starting;
		/// <summary>
		/// Event raised when the thread has been stopped.
		/// </summary>
		public event EventHandler Stopped;
		/// <summary>
		/// Event raised when the thread has completed normally.
		/// </summary>
		public event EventHandler Completed;
		/// <summary>
		/// Event raised when an unhandled exception is caught.
		/// </summary>
		public event EventHandler<ExceptionEventArgs> ProcessException;

		/// <summary>
		/// Default Constructor.
		/// </summary>
		/// <remarks>IsBackground is defaulted to true for WorkerThread.</remarks>
		protected WorkerThread() : this(null) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="threadName">The name of the thread.  If null or empty, no thread name will be assigned.</param>
		/// <remarks>IsBackground is defaulted to true for WorkerThread.</remarks>
		protected WorkerThread(string threadName)
		{
			ThreadComplete = new ManualResetEvent(false);
			ProcessThread = new Thread(StartThread);
			if (!string.IsNullOrEmpty(threadName))
			{
				ProcessThread.Name = threadName;
			}
			ProcessThread.IsBackground = true;
		}

		/// <summary>
		/// Method to be implemented by derived classes.  This should contain the process logic.
		/// </summary>
		protected abstract void Execute();

		/// <summary>
		/// The thread on which the process is executing.
		/// </summary>
		public Thread ProcessThread { get; private set; }
		/// <summary>
		///  Flag indicating if trace messages should be written.
		/// </summary>
		public bool Tracing { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the worker thread is a background thread.
		/// </summary>
		public bool IsBackground
		{
			get { return ProcessThread.IsBackground; }
			set { ProcessThread.IsBackground = value; }
		}

		/// <summary>
		/// Flag indicating if the thread is currently running 
		/// </summary>
		public virtual bool IsRunning
		{
			get
			{
				return ((ProcessThread != null) &&
					((ProcessThread.ThreadState == System.Threading.ThreadState.Running) ||
					 (ProcessThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)));
			}
		}

		/// <summary>
		/// Flag indicating a stop has been requested.
		/// Derived classes need to check this flag during processing to determine if they should 
		/// abandon processing.
		/// </summary>
		protected bool Stopping { get; set; }

		/// <summary>
		/// The processing thread entry point.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Because the impementation of the Execute method is determined by derived classes, we don't know what exception may bubble up to this point.")]
		private void StartThread()
		{
			ThreadComplete.Reset();
			OnStarting();

			Stopwatch sw = Stopwatch.StartNew();
			TS.Logger.WriteMethodCallIf(Tracing && TS.EC.TraceVerbose);

			bool wasException = false;

			try
			{
				Execute();
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(Tracing && TS.EC.TraceError, ex);
				OnProcessException(ex);
				wasException = true;
			}
			finally
			{
				ThreadComplete.Set();
				sw.Stop();
				TS.Logger.WritePerformanceIf(Tracing && TS.EC.TraceInfo, sw.Elapsed);
			}

			if (!wasException)
			{
				if (Stopping)
				{
					OnStopped();
				}
				else
				{
					OnCompleted();
				}
			}
		}

		/// <summary>
		/// Starts the Worker Thread.
		/// </summary>
		public virtual void Start()
		{
			ProcessThread.Start();
		}

		/// <summary>
		/// Sends a stop request which sets the stopping flag to true.
		/// Derived classes need to check this flag during processing to determine if they should 
		/// abandon processing.
		/// </summary>
		public virtual void StopProcess()
		{
			Stopping = true;
		}

		/// <summary>
		/// Aborts the processing.
		/// </summary>
		public virtual void Abort()
		{
			ProcessThread.Abort();
		}

		/// <summary>
		/// Blocks the current thread until the worker thread completes.
		/// </summary>
		/// <returns>True if the worker thread completes. If the worker thread does not complete, WaitOne never returns</returns>
		/// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
		/// <exception cref="System.Threading.AbandonedMutexException">System.Threading.AbandonedMutexException</exception>
		/// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			Justification = "WaitOne is a method name.")]
		public bool WaitOne()
		{
			if (Thread.CurrentThread == ProcessThread) throw new InvalidOperationException("Method 'WaitOne' called from worker thread.  This would cause the thread to lock.");

			if (IsRunning)
			{
				return ThreadComplete.WaitOne();
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Blocks the current thread until the worker thread completes, using an int to measure the time interval.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
		/// <returns>True if the worker thread completes within the timeout specified; otherwise, false.</returns>
		/// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
		/// <exception cref="System.Threading.AbandonedMutexException">System.Threading.AbandonedMutexException</exception>
		/// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework",
			MessageId = "System.Threading.WaitHandle.#WaitOne(System.Int32)",
			Justification="Seems to be a bug in Code Analysis, method was introduced in .Net 2.0 SP2, should be availabe in .Net 3.5")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			Justification = "WaitOne is a method name.")]
		public bool WaitOne(int millisecondsTimeout)
		{
			if (Thread.CurrentThread == ProcessThread) throw new InvalidOperationException("Method 'WaitOne' called from worker thread.  This would cause the thread to lock.");
			
			if (IsRunning)
			{
				return ThreadComplete.WaitOne(millisecondsTimeout);
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Blocks the current thread until the worker thread completes, using a TimeSpan to measure the time interval.
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns></returns>
		/// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
		/// <exception cref="System.Threading.AbandonedMutexException">System.Threading.AbandonedMutexException</exception>
		/// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework",
			Justification = "Seems to be a bug in Code Analysis, method was introduced in .Net 2.0 SP2, should be availabe in .Net 3.5")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			Justification = "WaitOne is a method name.")]
		public bool WaitOne(TimeSpan timeout)
		{
			if (Thread.CurrentThread == ProcessThread) throw new InvalidOperationException("Method 'WaitOne' called from worker thread.  This would cause the thread to lock.");
		
			if (IsRunning)
			{
				return ThreadComplete.WaitOne(timeout);
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Blocks the current thread until the worker thread completes, using an int to measure the time interval and specifying whether to exit the synchronization domain before the wait.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
		/// <param name="exitContext">True to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false.</param>
		/// <returns>True if the worker thread completes within the timeout specified; otherwise, false.</returns>
		/// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
		/// <exception cref="System.Threading.AbandonedMutexException">System.Threading.AbandonedMutexException</exception>
		/// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			Justification = "WaitOne is a method name.")]
		public bool WaitOne(int millisecondsTimeout, bool exitContext)
		{
			if (Thread.CurrentThread == ProcessThread) throw new InvalidOperationException("Method 'WaitOne' called from worker thread.  This would cause the thread to lock.");

			if (IsRunning)
			{
				return ThreadComplete.WaitOne(millisecondsTimeout, exitContext);
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Blocks the current thread until the worker thread completes, using a TimeSpan to measure the time interval and specifying whether to exit the synchronization domain before the wait.
		/// </summary>
		/// <param name="timeout"></param>
		/// <param name="exitContext">True to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false.</param>
		/// <returns></returns>
		/// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
		/// <exception cref="System.Threading.AbandonedMutexException">System.Threading.AbandonedMutexException</exception>
		/// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			Justification = "WaitOne is a method name.")]
		public bool WaitOne(TimeSpan timeout, bool exitContext)
		{
			if (Thread.CurrentThread == ProcessThread) throw new InvalidOperationException("Method 'WaitOne' called from worker thread.  This would cause the thread to lock.");

			if (IsRunning)
			{
				return ThreadComplete.WaitOne(timeout, exitContext);
			}
			else
			{
				return true;
			}
		}

		#region Raise Event Methods
		/// <summary>
		/// Raises the Starting event.
		/// </summary>
		protected virtual void OnStarting()
		{
			if (Starting != null) Starting.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Raises the Completed event.
		/// </summary>
		protected virtual void OnCompleted()
		{
			if (Completed != null) Completed.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Raises the Stopped event.
		/// </summary>
		protected virtual void OnStopped()
		{
			if (Stopped != null) Stopped.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Raises the ProcessException event.
		/// </summary>
		/// <param name="ex">The unhandled exception which was caught.</param>
		protected virtual void OnProcessException(Exception ex)
		{
			if (ProcessException != null) ProcessException.Invoke(this, new ExceptionEventArgs(ex));
		}
		#endregion

	}
}
