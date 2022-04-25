using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Apache.NMS;
using System.Security;
using Spring.Context;
using Spring.Messaging.Nms.Core;
using Spring.Messaging.Nms.Listener;
using Spring.Messaging.Nms.Listener.Adapter;
using Spring.Messaging.Nms.Support.Destinations;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// NMS Implementation of the Method Call Gateway
	/// </summary>
	[CLSCompliant(false)]
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nms")]
	public class NmsGateway : NmsGatewaySupport, IGateway
	{
		/// <summary>
		/// The message property name for delaying message delivery.
		/// </summary>
		protected const string MessageDelayProperty = "AMQ_SCHEDULED_DELAY";

		/// <summary>
		/// The Queue to which Result Message will be sent.
		/// </summary>
		public IDestination ResponseQueue { get; set; }

		/// <summary>
		/// Configures a Request Message for round trip messaging.
		/// </summary>
		/// <param name="message">The Request Message.</param>
		/// <param name="correlationId">The Correlation Id used to identify the Result for the Request.</param>
		protected virtual IMessage ConfigureMessage(IMessage message, string correlationId)
		{
			if (message == null) throw new ArgumentNullException("message");

			message.NMSReplyTo = ResponseQueue;
			message.NMSCorrelationID = correlationId;
			return message;
		}

		/// <summary>
		/// Configures a Request Message for delayed messaging.
		/// </summary>
		/// <param name="message">The Request Message.</param>
		/// <param name="delayMilliseconds">The delay in milliseconds.</param>
		protected virtual IMessage ConfigureMessage(IMessage message, long delayMilliseconds)
		{
			if (message == null) throw new ArgumentNullException("message");

			message.Properties.SetLong(MessageDelayProperty, delayMilliseconds);
			return message;
		}

		/// <summary>
		/// Sends a delayed message to the specified queue.
		/// </summary>
		/// <param name="queueName">The queue to which the message will be sent.</param>
		/// <param name="message">The message to be sent.</param>
		/// <param name="delayMilliseconds">The length of time in milliseconds to delay sending the message.</param>
		public virtual void Send(string queueName, Message message, long delayMilliseconds)
		{
			if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException("queueName");
			if (message == null) throw new ArgumentNullException("message");
			if (delayMilliseconds < 0) throw new ArgumentException("The parameter delayMilliseconds must be a positive value or zero.");

			if (delayMilliseconds == 0)
			{
				NmsTemplate.ConvertAndSend(queueName, message);
				TS.Logger.WriteLineIf(TS.Verbose, TS.Categories.Event, "Sent message for {0}.{1}", message.ClassName, message.MethodName);
			}
			else
			{
				NmsTemplate.ConvertAndSendWithDelegate(queueName, message, msg => ConfigureMessage(msg, delayMilliseconds));
				TS.Logger.WriteLineIf(TS.Verbose, TS.Categories.Event, "Sent delayed message for {0}.{1}, delay is {0} milliseconds.",
					message.ClassName, message.MethodName, delayMilliseconds);
			}
		}

		/// <summary>
		/// Sends a message to the specified queue.
		/// </summary>
		/// <param name="queueName">The queue to which the message will be sent.</param>
		/// <param name="message">The message to be sent.</param>
		public virtual void Send(string queueName, RequestMessage message)
		{
			if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException("queueName");
			if (message == null) throw new ArgumentNullException("message");

			NmsTemplate.ConvertAndSendWithDelegate(queueName, message, msg => ConfigureMessage(msg, message.CorrelationId));
			TS.Logger.WriteLineIf(TS.Verbose, TS.Categories.Event, "Sent message for {0}.{1}", message.ClassName, message.MethodName);
		}

		/// <summary>
		/// Creates a Method Call Message Listener using the specified listener information.
		/// </summary>
		/// <param name="queueName">The queue to which the listener will listen.</param>
		/// <param name="service">The service on which method calls will be invoked.</param>
		/// <param name="concurrentConsumers">The number of concurrent listeners to be created.</param>
		/// <param name="securityProvider">The Security Provider which will handle Message Credential authentication</param>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "This is a factory method which is used to create the IDisposable object")]
		public virtual ILifecycle CreateListener(string queueName, object service, int concurrentConsumers, ISecurityProvider securityProvider)
		{
			if (queueName == null) throw new ArgumentNullException("queueName");
			if (service == null) throw new ArgumentNullException("service");
			if (securityProvider == null) throw new ArgumentNullException("securityProvider");

			if (concurrentConsumers == 0) concurrentConsumers = 1;

			return new SimpleMessageListenerContainer()
			{
				ConcurrentConsumers = concurrentConsumers,
				ConnectionFactory = NmsTemplate.ConnectionFactory,
				DestinationName = queueName,
				MessageListener = new MessageListenerAdapter()
				{
					HandlerObject = new RequestHandler() { Service = service, SecurityProvider = securityProvider },
					DefaultHandlerMethod = RequestHandler.DefaultHandlerMethod,
					MessageConverter = NmsTemplate.MessageConverter
				}
			};
		}
	}
}
