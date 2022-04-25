using System;
using System.Reflection;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Defines a Message Queue Provider which will convert Method calls into Messages.
	/// </summary>
	public interface IMessagingProvider
	{
    /// <summary>
    /// Creates and configures a listener for the specified service on the specified Message Queue
    /// </summary>
		/// <param name="queueName">The queue to which the listener will listen.</param>
		/// <param name="service">The service on which method calls will be invoked.</param>
		/// <param name="concurrentConsumers">The number of concurrent listeners to be created.</param>
		void CreateServiceListener(string queueName, object service, int concurrentConsumers);

    /// <summary>
    /// Stops all listeners.
    /// </summary>
    void StopListeners();

		/// <summary>
		/// Sends a Method Call Message using the specified Message Information
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		void SendMessage(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo);

		/// <summary>
		/// Sends a Method Call Message using the specified Message Information
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		/// <param name="returnValue">The return value of the Method Call.</param>
		void SendMessage(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo, object returnValue);

		/// <summary>
		/// Sends a Method Call Message using the specified Message Information
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		/// <param name="exception">The exception which was thrown by the Method Call.</param>
		void SendMessage(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo, Exception exception);

		/// <summary>
		/// Sends a Message using the configured Message Queue
		/// </summary>
		/// <param name="methodInfo">The Method Information of the method being invoked.</param>
		/// <param name="args">The Method Parameter Values for the method being invoked.</param>
		/// <param name="messageInfo">Information used to send the message.</param>
		object SendMessageAndWaitForResult(MethodInfo methodInfo, object[] args, IMessageInfo messageInfo);

	}
}
