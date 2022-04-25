using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.Loggers;
using System.Diagnostics.Loggers.Registry;
using System.Diagnostics.TraceListeners;

namespace System.Diagnostics
{

	/// <summary>
	/// The TS class provides a global Trace Switch for the current named thread.
	/// </summary>
	/// <remarks>This class is placed in the System.Diagnostics namespace as it is intended to be
	/// used in conjunction with the Debug.WriteLineIf and TS.Logger.WriteLineIf methods.</remarks>
	[DebuggerStepThrough]
	public static class TS
	{
		#region Constants
		/// <summary>
		/// The Categories class contains all Trace Category Constants.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This is a non-instantiable static class which only contains constants.")]
		public static class Categories
		{
			/// <summary>
			/// Category indicating a critical, process ending exception
			/// </summary>
			public const string Critical = "Critical Error";
			/// <summary>
			/// Category indicating a recoverable exception
			/// </summary>
			public const string Error = "Error";
			/// <summary>
			/// Category indicating an unexpected condition occurred
			/// </summary>
			public const string Warning = "Warning";
			/// <summary>
			/// Category indicating a performance message
			/// </summary>
			public const string Performance = "Performance";
			/// <summary>
			/// Category indicating a method call message
			/// </summary>
			public const string Method = "Method Call";
			/// <summary>
			/// Category indicating an input parameter message for a method call
			/// </summary>
			public const string Param = "Method Parameter";
			/// <summary>
			/// Category indicating the return value message for a method
			/// </summary>
			public const string Return = "Method Return";
			/// <summary>
			/// Category indicating a generic debugging message
			/// </summary>
			public const string Debug = "Debug";
			/// <summary>
			/// Category indicating an event message
			/// </summary>
			public const string Event = "Event";
			/// <summary>
			/// Category indication an informational message
			/// </summary>
			public const string Info = "Info";

			/// <summary>
			/// The default category used when no category is specified in the Trace Message.
			/// </summary>
			public const string Default = Categories.Debug;
		}

		/// <summary>
		/// The Commands class contains all Category Command Constants
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This is a non-instantiable static class which only contains constants.")]
		public static class Commands
		{
			/// <summary>
			/// Command prefix.  Used to let inform the trace listener to perform an action.
			/// </summary>
			public const string Prefix = "COMMAND: ";
			/// <summary>
			/// Command used to indicate start of process.
			/// </summary>
			public const string Begin = Prefix + "Begin";
			/// <summary>
			/// Command used to indicate process completion.
			/// </summary>
			public const string End = Prefix + "End";
		}

		/// <summary>
		/// The Constants class contains constants used in the GlobalTraceSwitch Class
		/// </summary>
		private static class Constants
		{
			/// <summary>
			/// Default container ObjectId for the Logger.
			/// </summary>
			public const string LoggerObjectId = "System.Diagnostics.Logger";
			/// <summary>
			/// Default container ObjectId for the Logger Registry.
			/// </summary>
			public const string LoggerRegistryObjectId = "System.Diagnostics.LoggerRegistry";
			/// <summary>
			/// Format for generating thread names from a thread Id when the thread name is empty.
			/// </summary>
			public const string ThreadIdFormat = "Thread_{0}";
			/// <summary>
			/// Value indicating configuration is for all unconfigured threads.
			/// </summary>
			public const string AllThreads = "*";
		}

		#endregion

		#region Fields
		/// <summary>
		/// Contains the Thread Trace Switch Collection.
		/// </summary>
		private static Dictionary<string, TraceSwitch> threads = new Dictionary<string, TraceSwitch>();
		/// <summary>
		/// Stores the list of contexts for each thread.
		/// </summary>
		private static Dictionary<string, List<string>> threadContexts = new Dictionary<string, List<string>>();
		/// <summary>
		/// Trace listener used to alert Log Registry that the application is shutting down. 
		/// </summary>
		private static AlerterTraceListener alerter;
		/// <summary>
		/// Trace switch to be used when there is no configuration for the context specified.
		/// </summary>
		private static TraceSwitch noConfigTraceSwitch = new TraceSwitch("NoConfig", "TraceSwitch used when the specified context is not configured.");
		#endregion

		#region Methods
		/// <summary>
		/// Adds Thread Trace Switch for the current Named Thread 
		/// </summary>
		/// <remarks>The current thread must be named. If the current thead is not named, no Thread Trace Switch is added.</remarks>
		private static TraceSwitch GetNewTraceSwitch(string contextId, string threadName, string key)
		{
			if (alerter == null)
			{
				alerter = new AlerterTraceListener();
				Trace.Listeners.Add(alerter);
				alerter.Closing += new EventHandler(TraceClosing);
			}

			// Create a new TraceSwitch for this thread.
			TraceSwitch retVal = new TraceSwitch(key, string.Format(CultureInfo.InvariantCulture, "Thread Trace Switch for {0} - {1}", contextId, threadName));

			// Check the config file to see if Trace Level and or Listeners have been configured for this thread.
			if (Settings.Values.ThreadTraceLevels.Exists(item => item.ThreadName == threadName))
			{
				// Specific configuration for thread found.
				retVal.Level = SetThreadTracing(contextId, threadName, threadName);
			}
			else if (Settings.Values.ThreadTraceLevels.Exists(item => item.ThreadName == Constants.AllThreads))
			{
				// Generic configuration for all undefined threads found.
				retVal.Level = SetThreadTracing(contextId, Constants.AllThreads, threadName);
			}
			else
			{
				// No Thread configuration: Set Trace Switch Trace Level to the Default Value
				ContextTraceLevel level = Settings.Values.ContextLevels.Find(item => item.ContextId.Equals(contextId, StringComparison.OrdinalIgnoreCase));

				if (level == null)
				{
					retVal.Level = Settings.Values.DefaultLevel;
				}
				else
				{
					retVal.Level = level.TraceLevel;
				}
			}

			LoggerRegistry.Register(threadName);

			return retVal;
		}

		private static void TraceClosing(object sender, EventArgs e)
		{
			LoggerRegistry.Shutdown();
		}

		private static List<string> _threadListenersInstantiated = new List<string>();

		private static TraceLevel SetThreadTracing(string contextId, string threadName, string realThreadName)
		{
			ThreadTraceLevel thread = Settings.Values.ThreadTraceLevels.Find(item => item.ThreadName == threadName);

			if (!_threadListenersInstantiated.Contains(realThreadName))
			{
				foreach (TraceListenerInfo listener in thread.TraceListeners)
				{
					AddTraceListener(listener);
				}

				_threadListenersInstantiated.Add(realThreadName);
			}

			ContextTraceLevel level = thread.ContextLevels.Find(item => item.ContextId.Equals(contextId, StringComparison.OrdinalIgnoreCase));
			if (level == null)
			{
				return thread.TraceLevel;
			}
			else
			{
				return level.TraceLevel;
			}
		}


		/// <summary>
		/// Removes a thread's TraceSwitch from the Global Trace Switch collection.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "We don't want problems with Tracing/Logging to interfere with the application no matter what exception may bubble up to this point.")]
		public static void RemoveThread()
		{
			try
			{
				// Get the name of the current thread
				string threadName = Thread.CurrentThread.GetSafeThreadName();

				// Remove entry from the Logger Registry
				LoggerRegistry.Unregister(threadName);

				// Remove TraceSwitch 
				lock (threads)
				{
					if (threads.ContainsKey(threadName))
					{
						threads.Remove(threadName);
					}
				}

				if (threads.Count == 0)
				{
					LoggerRegistry.Shutdown();
					LoggerRegistry = null;
				}

				TS.Logger.Flush();

				// Remove associated TraceListeners
				TraceListener[] listeners = new TraceListener[Trace.Listeners.Count];
				Trace.Listeners.CopyTo(listeners, 0);
				foreach (TraceListener listener in listeners)
				{
					ThreadTraceListener threadListener = listener as ThreadTraceListener;
					if ((threadListener != null) && (threadListener.ThreadName == threadName))
					{
						Trace.Listeners.Remove(listener);
						listener.Flush();
						listener.Dispose();
					}
				}
			}
			catch { }
		}

		/// <summary>
		/// Adds a Trace Listener to the Trace Listener's Collection.
		/// </summary>
		/// <param name="listenerInfo">Data object containing the information needed to create the trace listener.</param>
		public static void AddTraceListener(TraceListenerInfo listenerInfo)
		{
			// Get the name of the current thread
			AddTraceListener(Thread.CurrentThread.GetSafeThreadName(), listenerInfo);
		}

		/// <summary>
		/// Adds a Trace Listener to the Trace Listener's Collection.
		/// </summary>
		/// <param name="listenerInfo">Data object containing the information needed to create the trace listener.</param>
		/// <param name="threadName">The name of the thread to which the listener is assigned.</param>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Listener is being added to the active Trace Listener collection.")]
		public static void AddTraceListener(string threadName, TraceListenerInfo listenerInfo)
		{
			if (listenerInfo == null) throw new ArgumentNullException("listenerInfo");

			TraceListener listener = IOC.GetFrameworkObject<TraceListener>(listenerInfo.Listener, null, false);
			if (listener != null)
			{
				ThreadTraceListener threadListener = listener as ThreadTraceListener;

				listener.Name = string.Format(CultureInfo.InvariantCulture, "{0}::{1}", threadName, listenerInfo.Name);
				if (threadListener != null)
				{
					threadListener.ThreadName = threadName;
					threadListener.Initialize(listenerInfo.Parameters);
				}

				Trace.Listeners.Add(listener);
			}
		}

		/// <summary>
		/// Gets the thread name for the specified thread.  If the thread has no name returns the managed thread Id instead.
		/// </summary>
		/// <param name="thread">The thread from which the name or Id will be retrieved.</param>
		public static string GetSafeThreadName(this Thread thread)
		{
			string retVal = null;

			if (thread != null)
			{
				retVal = (string.IsNullOrEmpty(thread.Name)) ? GetNameFromThreadId(thread) : thread.Name;
			}

			return retVal;
		}

		/// <summary>
		/// Gets the list of Diagnostics Contexts for the specified thread.
		/// </summary>
		/// <param name="threadName">The name of the thread.</param>
		/// <returns>Returns the list of Diagnostics Contexts for the specified thread.</returns>
		public static List<string> GetContexts(string threadName)
		{
			List<string> retVal;

			if (threadContexts.ContainsKey(threadName))
			{
				retVal = threadContexts[threadName];
			}
			else
			{
				retVal = new List<string>();
				threadContexts.Add(threadName, retVal);
			}

			return retVal;
		}

		/// <summary>
		/// Generates a name based on the Thread's ManagedThreadId property.
		/// </summary>
		/// <param name="thread">The thread for which a name will be generated.</param>
		/// <returns>Returns a generated name based on the thread's ThreadId.</returns>
		private static string GetNameFromThreadId(Thread thread)
		{
			return string.Format(CultureInfo.InvariantCulture, Constants.ThreadIdFormat, thread.ManagedThreadId);
		}

		/// <summary>
		/// Gets the default Trace Switch for the current thread.
		/// </summary>
		/// <returns>Returns the Trace Switch for the current named thread.</returns>
		private static TraceSwitch GetTraceSwitch()
		{
			return GetTraceSwitch(null, Thread.CurrentThread.GetSafeThreadName());
		}

		/// <summary>
		/// Gets the Trace Switch for the current thread and specified context.
		/// </summary>
		/// <param name="contextId">The Tracing Context</param>
		/// <returns>Returns the Trace Switch for the current named thread.</returns>
		private static TraceSwitch GetTraceSwitch(string contextId)
		{
			return GetTraceSwitch(contextId, Thread.CurrentThread.GetSafeThreadName());
		}

		/// <summary>
		/// Stores a generic object used to prevent multiple threads from instantiating the Settings simultaneously.
		/// </summary>
		private static object _settingsLock = new object();
		/// <summary>
		/// Gets the trace switch for the specified thread.
		/// </summary>
		/// <param name="contextId">The Tracing Context</param>
		/// <param name="threadName">The name of the thread for the trace switch.</param>
		/// <returns>Returns the trace switch for the specified thread, if available, otherwise returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "We don't want problems with Tracing/Logging to interfere with the application no matter what exception may bubble up to this point.")]
		internal static TraceSwitch GetTraceSwitch(string contextId, string threadName)
		{
			TraceSwitch retVal = noConfigTraceSwitch;

			try
			{
				// Make sure settings have been read.
				if (!Settings.IsLoaded)
				{
					lock (_settingsLock)
					{
						// Recheck after receiving the lock in case another thread already instantiated the Settings instance.
						if (!Settings.IsLoaded) Settings.Load();
					}
				}

				if (contextId == null) contextId = Settings.Values.DefaultContext;

				string key = GetContextThreadKey(contextId, threadName);
				// Check to see if the thread is named
				if (!string.IsNullOrEmpty(key))
				{
					// Lock the Thread Trace Switch Collection for multi-thread safety.
					lock (threads)
					{
						// Add to Thread Contexts
						List<string> contexts = GetContexts(threadName);
						if (!contextId.Contains(contextId))
						{
							contexts.Add(contextId);
						}

						// Add to Thread TraceSwitches
						if (threads.ContainsKey(key))
						{
							retVal = threads[key];
						}
						else
						{
							// Add the Trace Switch to the Thread Trace Switch Collection
							retVal = GetNewTraceSwitch(contextId, threadName, key);
							if (retVal != noConfigTraceSwitch)
							{
								threads.Add(key, retVal);
							}
						}
					}
				}
			}
			catch { } // We don't want problems with Tracing/Logging to interfere with the application.

			return retVal;
		}

		/// <summary>
		/// Gets the key used to store the TraceSwitch for the specified Context and Thread.
		/// </summary>
		private static string GetContextThreadKey(string contextId, string threadName)
		{
			return string.Format(CultureInfo.InvariantCulture, "|{0}|{1}|", contextId, threadName);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Stores the instance of the ILogger object
		/// </summary>
		private static LoggerBase _logger;
		/// <summary>
		/// Stores a generic object used to prevent multiple threads from instantiating the logger simultaneously.
		/// </summary>
		private static object _loggerLock = new object();
		/// <summary>
		/// Gets or sets the instance of the Logger object used to write messages.
		/// </summary>
		/// <remarks>
		/// The configured IOC is used to find the Logger device.  Either the devices provided in
		/// System.Diagnostics or a custom device may be used.  Custom devices must inherit the ILogger
		/// interface and optionally may be derived from LoggerBase in System.Diagnostics. To use a
		/// custom device configure the IOC with an Interface Type of ILogger and an Object Id of
		/// 'System.Diagnostics.Logger'.
		/// Alternatively, if an IOC is not being used, you can configure the custom device to be the default
		/// device in the System.Diagnostics.Settings by setting the value of the Logger attribute
		/// to the full name (including assembly name) of the custom device.
		/// If no device is defined in the IOC, and no default device is defined in the configuration, or the
		/// type defined is invalid then an empty Logger device will be used.
		/// </remarks>
		public static LoggerBase Logger
		{
			get
			{
				if (_logger == null)
				{
					lock (_loggerLock)
					{
						// Double check in the event another thread instantiated the logger while we were waiting for the lock.
						if (_logger == null)
						{
							// Create a default logger while we get the real logger -- this is to prevent recursive calls.
							_logger = new DefaultLogger();

							// Force the settings object to load.
							if (!Settings.IsLoaded) Settings.Load();

							if (Settings.Values.Logger == null)
							{
								// Use Trace here since we don't have a real logger.
								Trace.WriteLine("Unable to create the Logger, setting is null or a configuration error has occured; check diagnostics settings.", TS.Categories.Warning);
							}
							else
							{
								// Get the real logger from the Container.
								LoggerBase logger = ReflectionExtensions.CreateObject<LoggerBase>(Settings.Values.Logger);
								if (logger == null)
								{
									string message = string.Format(CultureInfo.InvariantCulture, "Unable to create the Logger from setting value of '{0}'; check diagnostics settings.", Settings.Values.Logger);
									// Use Trace here since we don't have a real logger.
									Trace.WriteLine(message, TS.Categories.Warning);
								}
								else
								{
									_logger = logger;
									logger.WriteLineIf(Verbose, Categories.Event, "Using '{0}' for Logging.", logger.GetType().Name);
								}
							}
						}
					}
				}
				return _logger;
			}
			set
			{
				_logger = value;
			}
		}

		/// <summary>
		/// Stores the instance of the IRegistry object
		/// </summary>
		private static IRegistry _loggerRegistry;
		/// <summary>
		/// Stores a generic object used to prevent multiple threads from instantiating the Registry simultaneously.
		/// </summary>
		private static object _registryLock = new object();
		/// <summary>
		/// Gets the instance of the LoggerRegistry object.
		/// </summary>
		/// <remarks>
		/// The configured IOC is used to find the Logger Registry device.  Either the devices provided in
		/// System.Diagnostics or a custom device may be used.  Custom devices must inherit the IRegistry
		/// interface. To use a custom device configure the IOC with an Interface Type of IRegistry and
		/// an Object Id of 'System.Diagnostics.LoggerRegistry'.
		/// Alternatively, if an IOC is not being used, you can configure the custom device to be the default
		/// device in the System.Diagnostics.Settings by setting the value of the LoggerRegistryType attribute
		/// to the full name (including assembly name) of the custom device.
		/// If no device is defined in the IOC, and no default device is defined in the configuration, or the
		/// type defined is invalid then an empty Logger Registry device will be used.
		/// </remarks>
		public static IRegistry LoggerRegistry
		{
			get
			{
				if (_loggerRegistry == null)
				{
					lock (_registryLock)
					{
						// Double check in the event another thread instantiated the registry while we were waiting for the lock.
						if (_loggerRegistry == null)
						{
							// Create a default registry while we get the real registry -- this is to prevent recursive calls.
							_loggerRegistry = new DefaultRegistry();

							// Force the settings object to load.
							if (!Settings.IsLoaded) Settings.Load();

							// Get the real registry from the Container.
							IRegistry loggerRegistry = IOC.GetFrameworkObject<IRegistry>(Settings.Values.LoggerRegistry, Constants.LoggerRegistryObjectId, false);
							if (loggerRegistry == null)
							{
								string message = string.Format(CultureInfo.InvariantCulture, "Unable to create the default Logger Registry from setting value of '{0}'; check diagnostics settings.", Settings.Values.LoggerRegistry);
								Trace.WriteLine(message, TS.Categories.Warning);
							}
							else
							{
								_loggerRegistry = loggerRegistry;
								_loggerRegistry.Initialize();
							}
						}
					}
				}

				return _loggerRegistry;
			}
			set
			{
				_loggerRegistry = value;
			}
		}

		/// <summary>
		/// Gets the TraceSwitch for the specified context.
		/// </summary>
		/// <param name="contextId">The context Id.</param>
		/// <returns>Returns the TraceSwitch for the specified context.</returns>
		public static TraceSwitch Context(string contextId)
		{
			return GetTraceSwitch(contextId);
		}

		/// <summary>
		/// Gets the TraceSwitch for the System Framework context.
		/// </summary>
		public static TraceSwitch EC
		{
			get
			{
				if (Settings.IsLoaded)
				{
					return GetTraceSwitch(Settings.Values.SystemFrameworkContext);
				}
				else
				{
					return noConfigTraceSwitch;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Trace Level for the current thread that specifies the messages to output for tracing and debugging.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "The link demand is only to set the trace level.")]
		public static TraceLevel Level
		{
			get
			{
				return GetTraceSwitch().Level;
			}
			set
			{
				TraceSwitch traceSwitch = GetTraceSwitch();

				if (traceSwitch != noConfigTraceSwitch)
				{
					traceSwitch.Level = value;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the current thread is set to Error.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error.</value>
		public static bool Error
		{
			get
			{
				return GetTraceSwitch().TraceError;
			}
		}

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the current thread is set to Error or Warning.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error or Warning.</value>
		public static bool Warning
		{
			get
			{
				return GetTraceSwitch().TraceWarning;
			}
		}

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the current thread is set to Error, Warning or Info.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error, Warning or Info.</value>
		public static bool Info
		{
			get
			{
				return GetTraceSwitch().TraceInfo;
			}
		}

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the current thread is set to Error, Warning, Info or Verbose.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error, Warning, Info or Verbose.</value>
		public static bool Verbose
		{
			get
			{
				return GetTraceSwitch().TraceVerbose;
			}
		}

		#endregion
	}

	#region Context Trace Switch
	/// <summary>
	/// Provides a base class for quick access to a Trace Context's Trace Switch.
	/// </summary>
	/// <typeparam name="T">The type of the derived class.</typeparam>
	public abstract class ContextTraceSwitch<T> where T : ContextTraceSwitch<T>, new()
	{
		/// <summary>
		/// Stores the instance of the Context Trace Switch
		/// </summary>
		private static T _i = new T();

		/// <summary>
		/// Gets the instance of the Context Trace Switch
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "I",
			Justification = "I is an abreviation for Instance, I is used to make consuming code more terse.")]
		protected static T I { get { return _i; } }

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the context is set to Error.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error.</value>
		public static bool Error { get { return I.Switch.TraceError; } }

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the context is set to Error or Warning.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error or Warning.</value>
		public static bool Warning { get { return I.Switch.TraceWarning; } }

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the context is set to Error, Warning or Info.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error, Warning or Info.</value>
		public static bool Info { get { return I.Switch.TraceInfo; } }

		/// <summary>
		/// Gets a value indicating if the TraceLevel for the context is set to Error, Warning, Info or Verbose.
		/// </summary>
		/// <value>Returns true if the the TraceLevel is set to Error, Warning, Info or Verbose.</value>
		public static bool Verbose { get { return I.Switch.TraceVerbose; } }

		/// <summary>
		/// Gets or sets the Trace Level for the context that specifies the messages to output for tracing and debugging.
		/// </summary>
		public static TraceLevel Level { get { return I.Switch.Level; } set { I.Switch.Level = value; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		protected ContextTraceSwitch() { }

		/// <summary>
		/// Derived classes will specify the Trace Context Id of the Trace Switch
		/// </summary>
		protected abstract string ContextId { get; }

		/// <summary>
		/// Gets the Trace Switch for the configured Trace Context Id.
		/// </summary>
		protected TraceSwitch Switch { get { return TS.Context(ContextId); } }
	}
	#endregion

}
