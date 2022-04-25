using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Message handler which handles Result Messages.
	/// </summary>
  public class ResultHandler
  {
		/// <summary>
		/// Gets or sets the Broker used to match results with their corresponding request.
		/// </summary>
    public IBroker Broker { get; set; }

		/// <summary>
		/// Handles the Result Message.
		/// </summary>
		/// <param name="message">The result message.</param>
    public void Handle(ResultMessage message)
    {
      if (message == null) throw new ArgumentNullException("message");

      Broker.RegisterResult(message);
    }

		/// <summary>
		/// Handles the Result Message.
		/// </summary>
		/// <param name="message">The result message.</param>
		public void Handle(ExceptionMessage message)
		{
			if (message == null) throw new ArgumentNullException("message");

			Broker.RegisterException(message);
		}

		/// <summary>
		/// Default Handler, throws an exception as the message cannot be handled.
		/// </summary>
		/// <param name="message">A message of unknown type.</param>
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
      Justification = "Instance method is called using reflection by Spring's Messaging framework.")]
    public void Handle(object message)
    {
      if (message == null) throw new ArgumentNullException("message");

      throw new MessagingException("Method Message Result Handler is unable to process '{0}' messages.", message.GetType().FullName);
    }
  }
}
