using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// The TheadTraceListener provides a base Trace Listener class.
	/// </summary>
	/// <remarks>This has to be an abstract class as an interface may not inherit from a class.</remarks>
	[DebuggerStepThrough]
	public abstract class ThreadTraceListener : TraceListener
	{
		#region Constructors
		/// <summary>
		/// Default Constructor.
		/// </summary>
		protected ThreadTraceListener() : base() 
		{
			// Default to Current Thread.
			ThreadName = Thread.CurrentThread.GetSafeThreadName();
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the Trace Listener</param>
		protected ThreadTraceListener(string name) : base(name)
		{
			// Default to Current Thread.
			ThreadName = Thread.CurrentThread.GetSafeThreadName();
		}
		#endregion

		#region Abstract Methods
		/// <summary>
		/// Sets custom parameters.
		/// </summary>
		/// <param name="paramName">The parameter name to set.</param>
		/// <param name="paramValue">The value for the parameter.</param>
		protected abstract void SetParameter(string paramName, string paramValue);
		#endregion

		#region Methods
		/// <summary>
		/// Initializes the trace listener.
		/// </summary>
		/// <param name="args">Trace Listener parameters.</param>
		public virtual void Initialize(ParameterCollection args)
		{
			Parameters = args;

			if (args != null)
			{
				args.ForEach(item => ReadParameter(item));
			}
		}

		private void ReadParameter(Parameter item)
		{
			if (item.Name.Equals("threadname", System.StringComparison.OrdinalIgnoreCase) &&
				item.Value.Equals("null", System.StringComparison.OrdinalIgnoreCase))
			{
				ThreadName = null;
			}
			else
			{
				// TODO: Research if the Replace {EQ} with = is necessary.
				SetParameter(item.Name, item.Value.Replace("{EQ}", "="));
			}
		}

		/// <summary>
		/// Stores the parameters which were used to initialize the trace listener.
		/// </summary>
		protected internal ParameterCollection Parameters { get; private set; }

		/// <summary>
		/// Determines if the trace message is originating from the thread for which this trace listener was created.
		/// </summary>
		/// <returns></returns>
		protected virtual bool IsCorrectThread
		{
			get
			{
				// If thread name is null, this TraceListener is configured as a "normal" non-thread specific TraceListener.
				// Otherwise check if the configured threadname equals the Current Thread's name or Id.
				return ((ThreadName == null) || (ThreadName == Thread.CurrentThread.Name) ||
					(ThreadName == Thread.CurrentThread.GetSafeThreadName()));
			}
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets the name of the thread for which this trace listener records trace messages.
		/// </summary>
		public string ThreadName { get; protected internal set; }
		#endregion

	}
}
