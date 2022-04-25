using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Security;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Handles Request Messages
	/// </summary>
  public class RequestHandler
  {
		/// <summary>
		/// The name of the Default Handler Method.
		/// </summary>
    public const string DefaultHandlerMethod = "Handle";

		/// <summary>
		/// Gets or sets the Security Provider used to authenticate the message credentials.
		/// </summary>
    public ISecurityProvider SecurityProvider { get; set; }

		/// <summary>
		/// Gets or sets the service which will recieve the method call contained in the message.
		/// </summary>
    public object Service { get; set; }

		/// <summary>
		/// Handles the Method Call Message.
		/// </summary>
		/// <param name="message">The message containing Method Call information</param>
    public void Handle(Message message)
    {
      if (message == null) throw new ArgumentNullException("message");

      GetResult(message);
    }

		/// <summary>
		/// Handles the Method Call Message.
		/// </summary>
		/// <param name="message">The message containing Method Call information</param>
		/// <returns>Returns a Result message containing the contents of the Request Message and the result from the method invocation.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Result is being serialized and sent to a message queue, the exception will be deserialized and handled on by the message receiver.")]
		public Message Handle(RequestMessage message)
    {
      if (message == null) throw new ArgumentNullException("message");

			try
			{
				return new ResultMessage(message, GetResult(message));
			}
			catch (Exception ex)
			{
				return new ExceptionMessage(message, ex);
			}
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

      throw new MessagingException("Method Message Request Handler is unable to process '{0}' messages.", message.GetType().FullName);
    }

		/// <summary>
		/// The binding flags used by Reflection's InvokeMember method.
		/// </summary>
    private static readonly BindingFlags _bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public;

		/// <summary>
		/// Invokes the method using the information contained within the Method Call Message
		/// </summary>
		/// <param name="message">The message containing Method Call information</param>
		/// <returns>Returns the result from the method invocation</returns>
    private object GetResult(Message message)
    {
			if (Service == null) throw new InvalidOperationException("No service has been provided to receive the method invocation");

      try
      {
        // Set the Current Principal using the Authentication information from the Message.
        Thread.CurrentPrincipal = SecurityProvider.AuthenticateUser(message.AuthenticationContext, message.UserId, message.SecurityToken);

        // Get the parameter values
        object[] parameters = (from p in message.Parameters select p.Value).ToArray();

        // Invoke the method on the service
        return Service.GetType().InvokeMember(message.MethodName, _bindingFlags, null, Service, parameters, CultureInfo.InvariantCulture);
      }
      finally
      {
        Thread.CurrentPrincipal = null;
      }
    }
  }
}
