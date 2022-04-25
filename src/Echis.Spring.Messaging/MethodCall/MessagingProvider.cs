using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Security;
using Spring.Context;
using Spring.Messaging.Nms.Support.Destinations;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Provides Messaging services for Method Call based messaging.
	/// </summary>
  [CLSCompliant(false)]
	public class MessagingProvider : IMessagingProvider
	{
		/// <summary>
		/// Gets or sets the Broker which is used to match Result messages with their corresponding Requests
		/// </summary>
		public IBroker Broker { get; set; }

		/// <summary>
		/// Gets or sets the Messaging Gateway used to send Method Call messages.
		/// </summary>
		public IGateway Gateway { get; set; }

		/// <summary>
		/// Gets or sets the Messaging Gateway used to send Method Call Pub-Sub messages.
		/// </summary>
		public IGateway PublishGateway { get; set; }

		/// <summary>
		/// Gets or sets the Security Provider which is used to authenticate Message credentials.
		/// </summary>
		public ISecurityProvider SecurityProvider { get; set; }

		/// <summary>
		/// Gets the Identity from the Thread's Current Principal if authenticated otherwise returns null
		/// </summary>
		protected virtual IIdentity DetermineIdentity()
		{
			return (Thread.CurrentPrincipal == null) ||
				(Thread.CurrentPrincipal.Identity == null) ||
				(!Thread.CurrentPrincipal.Identity.IsAuthenticated) ? null :
				Thread.CurrentPrincipal.Identity;
		}

		/// <summary>
		/// Gets a security token for the specified identity.
		/// </summary>
		/// <param name="identity">An identity object containing message credentials</param>
    protected virtual string GetSecurityToken(IIdentity identity)
    {
      return ((identity == null) || !identity.IsAuthenticated) ? string.Empty :
				SecurityProvider.CreateSecurityToken(identity.AuthenticationType, identity.Name);
    }

		/// <summary>
		/// Creates a standard Method Call message using the specified information.
		/// </summary>
		/// <param name="methodInfo">The method information for the method which is being invoked.</param>
		/// <param name="args">The arguments which are being passed into the method.</param>
		protected Message CreateMessage(MethodInfo methodInfo, object[] args)
		{
			IIdentity identity = DetermineIdentity();
			string securityToken = GetSecurityToken(identity);
			return new Message(methodInfo, identity, args, securityToken);
		}

		/// <summary>
		/// Creates a Request Method Call message using the specified information.
		/// </summary>
		/// <param name="methodInfo">The method information for the method which is being invoked.</param>
		/// <param name="args">The arguments which are being passed into the method.</param>
		protected RequestMessage CreateRequestMessage(MethodInfo methodInfo, object[] args)
		{
			IIdentity identity = DetermineIdentity();
			string securityToken = GetSecurityToken(identity);
			return new RequestMessage(methodInfo, identity, args, securityToken);
		}

		/// <summary>
		/// Creates a Result Method Call message using the specified information.
		/// </summary>
		/// <param name="methodInfo">The method information for the method which is being invoked.</param>
		/// <param name="args">The arguments which are being passed into the method.</param>
		/// <param name="returnValue">The return value of the Method Call.</param>
		protected ResultMessage CreateResultMessage(MethodInfo methodInfo, object[] args, object returnValue)
		{
			IIdentity identity = DetermineIdentity();
			string securityToken = GetSecurityToken(identity);
			return new ResultMessage(methodInfo, identity, args, securityToken, returnValue);
		}

		/// <summary>
		/// Creates a Exception Method Call message using the specified information.
		/// </summary>
		/// <param name="methodInfo">The method information for the method which is being invoked.</param>
		/// <param name="args">The arguments which are being passed into the method.</param>
		/// <param name="exception">The exception which was thrown by the Method Call.</param>
		protected ExceptionMessage CreateExceptionMessage(MethodInfo methodInfo, object[] args, Exception exception)
		{
			IIdentity identity = DetermineIdentity();
			string securityToken = GetSecurityToken(identity);
			return new ExceptionMessage(methodInfo, identity, args, securityToken, exception);
		}

		/// <summary>
		/// Sends a message using the information provide in the specified Message Info object.
		/// </summary>
		/// <param name="message">The message to be sent.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		protected virtual void SendMessage(Message message, IMessageInfo messageInfo)
		{
			if (message == null) throw new ArgumentNullException("message");
			if (messageInfo == null) throw new ArgumentNullException("messageInfo");

			if (messageInfo.PubSubdomain)
			{
				PublishGateway.Send(messageInfo.Destination, message, messageInfo.Delay);
			}
			else
			{
				Gateway.Send(messageInfo.Destination, message, messageInfo.Delay);
			}
		}

		/// <summary>
		/// Sends a Method Call Message using the specified Message Information
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		public virtual void SendMessage(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo)
		{
			if (methodInfo == null) throw new ArgumentNullException("methodInfo");
			if (args == null) args = new object[0];

			SendMessage(CreateMessage(methodInfo, args), messageInfo);
		}

		/// <summary>
		/// Sends a Method Call Message using the specified Message Information
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		/// <param name="returnValue">The return value of the Method Call.</param>
		public virtual void SendMessage(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo, object returnValue)
		{
			if (methodInfo == null) throw new ArgumentNullException("methodInfo");
			if (args == null) args = new object[0];

			SendMessage(CreateResultMessage(methodInfo, args, returnValue), messageInfo);
		}

		/// <summary>
		/// Sends a Method Call Message using the specified Message Information
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		/// <param name="exception">The exception which was thrown by the Method Call.</param>
		public virtual void SendMessage(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo, Exception exception)
		{
			if (methodInfo == null) throw new ArgumentNullException("methodInfo");
			if (exception == null) throw new ArgumentNullException("exception");
			if (args == null) args = new object[0];

			SendMessage(CreateExceptionMessage(methodInfo, args, exception), messageInfo);
		}

		/// <summary>
		/// Sends a Message using the configured Message Queue
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		public virtual object SendMessageAndWaitForResult(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo)
		{
			if (methodInfo == null) throw new ArgumentNullException("methodInfo");
			if (messageInfo == null) throw new ArgumentNullException("messageInfo");
			if (args == null) args = new object[0];

			RequestMessage message = CreateRequestMessage(methodInfo, args);

			Broker.RegisterCall(message);
			Gateway.Send(messageInfo.Destination, message);
			return Broker.WaitForResult(message);
		}

		/// <summary>
		/// Stores the collection of Listeners which are receiving message call messages.
		/// </summary>
    Dictionary<string, ILifecycle> _listenerContainers = new Dictionary<string, ILifecycle>();

		/// <summary>
		/// Creates a message queue listener for the specified queue using the specified service to invoke method calls.
		/// </summary>
		/// <param name="queueName">The queue to which the listener will listen.</param>
		/// <param name="service">The service on which method calls will be invoked.</param>
		/// <param name="concurrentConsumers">The number of concurrent listeners to be created.</param>
		public void CreateServiceListener(string queueName, object service, int concurrentConsumers)
    {
			if (queueName == null) throw new ArgumentNullException("queueName");
			if (service == null) throw new ArgumentNullException("service");
			if (_listenerContainers.ContainsKey(queueName)) throw new MessagingException("A message listener already exists for the Message Queue specified '{0}'", queueName);

			ILifecycle container = Gateway.CreateListener(queueName, service, concurrentConsumers, SecurityProvider);
      container.Start();

      _listenerContainers.Add(queueName, container);
    }

		/// <summary>
		/// Stops all listeners.
		/// </summary>
    public void StopListeners()
    {
      _listenerContainers.Values.ForEach(StopListener);
			_listenerContainers.Clear();
    }

		/// <summary>
		/// Stops the specified listener.
		/// </summary>
		/// <param name="listener">The listener to be stopped.</param>
    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
      Justification = "Unknown what exceptions may be caught, and at this point we simply want to shut down.")]
    private void StopListener(ILifecycle listener)
    {
      try
      {
        listener.Stop();
      }
      catch { }
    }
  }
}
