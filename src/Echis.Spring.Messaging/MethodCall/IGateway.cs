using System;
using System.Security;
using Spring.Context;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Defines the Messaging Gateway used to send Method Call Messages
	/// </summary>
  [CLSCompliant(false)]
	public interface IGateway
	{
		/// <summary>
		/// Sends a delayed message to the specified queue.
		/// </summary>
		/// <param name="queueName">The queue to which the message will be sent.</param>
		/// <param name="message">The message to be sent.</param>
		/// <param name="delayMilliseconds">The length of time in milliseconds to delay sending the message.</param>
		void Send(string queueName, Message message, long delayMilliseconds);

		/// <summary>
		/// Sends a message to the specified queue.
		/// </summary>
		/// <param name="queueName">The queue to which the message will be sent.</param>
		/// <param name="message">The message to be sent.</param>
		void Send(string queueName, RequestMessage message);

		/// <summary>
		/// Creates a Method Call Message Listener using the specified listener information.
		/// </summary>
		/// <param name="queueName">The queue to which the listener will listen.</param>
		/// <param name="service">The service on which method calls will be invoked.</param>
		/// <param name="concurrentConsumers">The number of concurrent listeners to be created.</param>
		/// <param name="securityProvider">The Security Provider which will handle Message Credential authentication</param>
		ILifecycle CreateListener(string queueName, object service, int concurrentConsumers, ISecurityProvider securityProvider);
  }
}
