using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace System.Spring.Messaging.MethodCall
{
  /// <summary>
  /// Manages matching Response Messages with their corresponding Request Message
  /// </summary>
  public class Broker : IBroker
  {
    /// <summary>
    /// Error message when a Method Call has not been registered.
    /// </summary>
    private const string _notRegistered = "The message for '{0}.{1}' has not been registered for a result.";

    /// <summary>
    /// Error message when the timeout period expires before the Response Message has been recieved
    /// </summary>
    private const string _callTimeout = "The method message for '{0}.{1}' failed to be processed within the specified time.";

    /// <summary>
    /// Logging message for performance.
    /// </summary>
    private const string _performance = "Message for '{0}.{1}' processed in {2} milliseconds.";

    /// <summary>
    /// Container for registered Method Calls.
    /// </summary>
    private static Dictionary<string, Result> _registeredCalls = new Dictionary<string, Result>();

    /// <summary>
    /// Gets or sets the timeout period for the Response.
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// Registeres a call to be Brokered.
    /// </summary>
    /// <param name="message">The Request Message to be Brokered.</param>
		public virtual void RegisterCall(RequestMessage message)
    {
      if (message == null) throw new ArgumentNullException("message");

      _registeredCalls.Add(message.CorrelationId, new Result());
    }

    /// <summary>
    /// Blocks the current thread until the Result Message for the specified Request Message has been recieved.
    /// </summary>
    /// <param name="message">The Request Message.</param>
		public virtual object WaitForResult(RequestMessage message)
    {
      if (message == null) throw new ArgumentNullException("message");
      if (!_registeredCalls.ContainsKey(message.CorrelationId)) throw new MessagingException(_notRegistered, message.ClassName, message.MethodName);

      Stopwatch sw = Stopwatch.StartNew();
      Result result = _registeredCalls[message.CorrelationId];

      try
      {
        if (!result.Signal.WaitOne(Timeout))
          throw new TimeoutException(string.Format(CultureInfo.InvariantCulture, _callTimeout, message.ClassName, message.MethodName));

				Exception ex = result.Value as Exception;
				if (result.Value != null) throw ex;

        return result.Value;
      }
      finally
      {
        result.Dispose();
        _registeredCalls.Remove(message.CorrelationId);

        sw.Stop();
        TS.Logger.WriteLineIf(TS.Info, TS.Categories.Performance, _performance, message.ClassName, message.MethodName, sw.Elapsed.TotalMilliseconds);
      }
    }

    /// <summary>
    /// Records the result and releases the Blocked Thread.
    /// </summary>
    /// <param name="message">The result message.</param>
		public virtual void RegisterResult(ResultMessage message)
    {
      if (message == null) throw new ArgumentNullException("message");

      if (_registeredCalls.ContainsKey(message.CorrelationId)) _registeredCalls[message.CorrelationId].Value = message.ReturnValue.Value;
    }

		/// <summary>
		/// Records the exception and releases the Blocked Thread.
		/// </summary>
		/// <param name="message">The exception message.</param>
		public virtual void RegisterException(ExceptionMessage message)
		{
			if (message == null) throw new ArgumentNullException("message");

			if (_registeredCalls.ContainsKey(message.CorrelationId)) _registeredCalls[message.CorrelationId].Value = message.Exception.Value;
		}

    /// <summary>
    /// Represents a Method Result.
    /// </summary>
    private class Result : IDisposable
    {
      /// <summary>
      /// Constructor.
      /// </summary>
      public Result()
      {
        Signal = new ManualResetEvent(false);
      }

      ~Result()
      {
        Dispose();
      }

      public ManualResetEvent Signal { get; set; }

      private object _value;
      public object Value
      {
        get { return _value; }
        set
        {
          _value = value;
          Signal.Set();
        }
      }

      public void Dispose()
      {
        if (Signal != null)
        {
          Signal.Dispose();
          Signal = null;
        }
        GC.SuppressFinalize(this);
      }
    }
  }
}
