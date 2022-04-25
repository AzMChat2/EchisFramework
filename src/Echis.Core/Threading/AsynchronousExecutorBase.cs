using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Threading
{
	/// <summary>
	/// Base class for executing an action on a background thread.
	/// </summary>
	public abstract class AsynchronousExecutorBase
	{
		/// <summary>
		/// Stores the number of times the AsynchronousExecutor has been executed.
		/// </summary>
		private static Dictionary<string, List<int>> _execCount = new Dictionary<string, List<int>>();

		/// <summary>
		/// Event raised when an unhandled exception is caught.
		/// </summary>
		public event EventHandler<ExceptionEventArgs> Error;

		/// <summary>
		/// Event raised when the action is performed without error.
		/// </summary>
		public event EventHandler Success;

		/// <summary>
		/// Gets the thread upon which the action will be executed.
		/// </summary>
		public Thread ExecutingThread { get; private set; }

		/// <summary>
		/// Gets a flag indicating if the Action is currently Executing.
		/// </summary>
		public bool Executing { get { return ExecutingThread != null; } }

		/// <summary>
		/// A Regular Expression which will scrub invalid characters from the Thread Name
		/// </summary>
		private Regex _scrubber = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

		#region Abstract Members
		/// <summary>
		/// Gets the method information about the action to be invoked.
		/// </summary>
		protected abstract MethodInfo Method { get; }
		/// <summary>
		/// Invokes the action.
		/// </summary>
		protected abstract void InvokeAction();
		#endregion

		/// <summary>
		/// Gets the thread name for a new Execution Thread.
		/// </summary>
		/// <returns>Returns a unique thread name based on the Action's Method Name.</returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "A Property is not appropriate here.")]
		protected virtual string GetThreadName()
		{
			_threadName = _scrubber.Replace(Method.Name, "_");

			AssignThreadNumber();

			return string.Format(CultureInfo.InvariantCulture, "{0}_{1}", _threadName, _threadNumber);
		}

		/// <summary>
		/// Assigns a unique number to this thread.
		/// </summary>
		private void AssignThreadNumber()
		{
			if (!_execCount.ContainsKey(_threadName))
			{
				lock (_execCount)
				{
					if (!_execCount.ContainsKey(_threadName)) _execCount.Add(_threadName, new List<int>());
				}
			}

			List<int> used = _execCount[_threadName];
			_threadNumber = 0;

			lock (used)
			{
				while (used.Contains(++_threadNumber)) { }
				used.Add(_threadNumber);
			}
		}

		/// <summary>
		/// Releases this thread's unique number.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Any exception at this point should be ignored.")]
		private void ReleaseThreadNumber()
		{
			try
			{
				_execCount[_threadName].Remove(_threadNumber);
			}
			catch { }
			finally
			{
				_threadNumber = 0;
				ExecutingThread = null;
			}
		}

		/// <summary>
		/// Stores the number (used for generating Thread Name) assigned to the Executing Thread.
		/// </summary>
		private int _threadNumber;
		/// <summary>
		/// Stores the basic name (used for generating Thread Name) 
		/// </summary>
		private string _threadName;

		/// <summary>
		/// Queues the Action to be executed using the Thread Pool.
		/// </summary>
		public virtual void ExecuteThreadPool()
		{
			ThreadPool.QueueUserWorkItem(ExecuteThreadPool);
		}

		/// <summary>
		/// Executes the action on a background thread.
		/// </summary>
		public virtual void Execute()
		{
			Execute(false);
		}

		/// <summary>
		/// Executes the action on a background thread.
		/// </summary>
		/// <param name="allowMultiExec">A flag which indicates if multiple executions are allowed.</param>
		/// <remarks>Use multiple executions with caution.  When multiple executions are used, the ExecutingThread and Executing properties are not valid.</remarks>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
			Justification = "Multi is short for Multiple")]
		public virtual void Execute(bool allowMultiExec)
		{
			if (!allowMultiExec && (ExecutingThread != null)) throw new ThreadStateException("Asynchronous Executor is currently executing.");

			ExecutingThread = new Thread(ExecuteAction);
			ExecutingThread.Name = GetThreadName();

			TS.Logger.WriteLineIf(TS.Verbose, TS.Categories.Event, "Starting Thread '{0}' for Asynchronous Execution.", ExecutingThread.Name);
			ExecutingThread.Start();
		}

		/// <summary>
		/// Raises the Success event.
		/// </summary>
		protected virtual void OnSuccess()
		{
			if (Success != null) Success.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Raises the Error event.
		/// </summary>
		/// <param name="exception"></param>
		protected virtual void OnError(Exception exception)
		{
			if (Error != null) Error.Invoke(this, new ExceptionEventArgs(exception));
		}

		/// <summary>
		/// This method is used when the Action is queued to be executed on the Thread Pool.
		/// </summary>
		/// <param name="unused"></param>
		private void ExecuteThreadPool(object unused)
		{
			ExecuteAction();
		}

		/// <summary>
		/// The Execution Thread entry point; invokes the action, traps any exceptions and raises the appropriate event (Error or Success).
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Unknown what type of exception the action might throw.  The need is to handle the exception and raise the Error event.")]
		protected virtual void ExecuteAction()
		{
			DateTime __methodStart = DateTime.Now;
			if (TS.Info) TS.Logger.WriteMethodCallMessage(Method);

			try
			{
				InvokeAction();
				OnSuccess();
			}
			catch (Exception ex)
			{
				if (TS.Error) TS.Logger.WriteExceptionMessage(Method, ex);
				OnError(ex);
			}
			finally
			{
				ReleaseThreadNumber();
				if (TS.Info) TS.Logger.WritePerformanceMessage(Method, DateTime.Now.Subtract(__methodStart));
			}
		}
	}
}
