using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace System.Spring.Messaging
{
	/// <summary>
	/// Base class for handling Messages of the specified type.
	/// </summary>
	/// <typeparam name="TMessage">The type of message to be processed.</typeparam>
	public abstract class HandlerBase<TMessage>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		protected HandlerBase()
		{
			LoggingLevel = TS.Level;
			MessageType = typeof(TMessage).FullName;
		}

		/// <summary>
		/// Gets or sets the trace switch used for logging.
		/// </summary>
		protected TraceLevel LoggingLevel { get; set; }
		
		/// <summary>
		/// Gets or sets the name of the Message Type (for logging purposes only)
		/// </summary>
		protected string MessageType { get; set; }

		/// <summary>
		/// Processes the specified message
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "False Positive. The HandleException method will rethrow the exception unless overridden by derived classes.")]
		public virtual void Handle(TMessage message)
		{
			Stopwatch sw = Stopwatch.StartNew();

			TS.Logger.WriteLineIf(LoggingLevel <= TraceLevel.Verbose, TS.Categories.Info, "New '{0}' Message recieved.", MessageType);
			SetCurrentPrincipalForMessage(message);

			try
			{
				HandleMessage(message);
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(LoggingLevel <= TraceLevel.Error, ex);
				HandleException(message, ex);
			}
			finally
			{
				sw.Stop();
				TS.Logger.WritePerformanceIf(LoggingLevel <= TraceLevel.Info, sw.Elapsed);
			}
		}

		/// <summary>
		/// Checks the Thread.CurrentPrincipal object and sets it to a valid IPrincipal instance.
		/// </summary>
		protected virtual void SetCurrentPrincipalForMessage(TMessage message)
		{
			// Set the Current Principal to the service account's principal
			if (Thread.CurrentPrincipal.Identity is GenericIdentity) Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
		}

		/// <summary>
		/// Processes the specified message
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		protected abstract void HandleMessage(TMessage message);

		/// <summary>
		/// Handles any exception caught during the processing of the message.
		/// </summary>
		/// <remarks>The default behavior is to simply rethrow the exception.  Derived classes may override.</remarks>
		protected virtual void HandleException(TMessage message, Exception ex)
		{
			throw ex;
		}

		/// <summary>
		/// Raises an exception that the message cannot be processed.
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		public virtual void Handle(object message)
		{
			throw new MessagingException("The '{0}' Message Handler is unable to process '{1}' messages.",
				this.GetType().FullName,
				message == null ? "NULL" : message.GetType().FullName);
		}
	}

	/// <summary>
	/// Defines a Message Handler which uses a Processor to process the message.
	/// </summary>
	/// <typeparam name="TMessage">The type of message to be processed.</typeparam>
	public class ProcessorHandler<TMessage> : HandlerBase<TMessage>
	{
		/// <summary>
		/// Gets or sets the Processor used to process messages.
		/// </summary>
		protected IProcessor<TMessage> Processor { get; set; }

		/// <summary>
		/// Processes the specified message
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		protected override void HandleMessage(TMessage message)
		{
			Processor.Process(message);
		}
	}

	/// <summary>
	/// Defines a Message Handler which uses a Processor to process the message.
	/// </summary>
	/// <typeparam name="TMessage">The type of message to be processed.</typeparam>
	public abstract class MultiprocessorHandler<TMessage> : HandlerBase<TMessage>
	{
		/// <summary>
		/// Gets or sets the Processor used to process messages.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification =  "The property setter is required for property injection.")]
		protected IDictionary<string, IProcessor<TMessage>> Processors { get; set; }

		/// <summary>
		/// Gets the key which will be used to retrieve the processor to process the message.
		/// </summary>
		/// <param name="message">The message context for which the Processor is required.</param>
		protected abstract string GetKey(TMessage message);

		/// <summary>
		/// Processes the specified message
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		protected override void HandleMessage(TMessage message)
		{
			string key = GetKey(message);

			if (!Processors.ContainsKey(key))
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
				"No processor configured to for key '{0}'.", key));

			Processors[key].Process(message);
		}
	}

}
