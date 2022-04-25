using System;
using System.Threading;
using System.Security;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Base class for handling a specific parameter type of Method Call Messages
	/// </summary>
	/// <typeparam name="TProcessor">The processor which will process the parameter value.</typeparam>
	/// <typeparam name="TObject">The type of parameter to be processed.</typeparam>
	public abstract class MethodCallHandlerBase<TProcessor, TObject> : HandlerBase<Message>
		where TProcessor : IProcessor<TObject>
		where TObject : class
	{

		/// <summary>
		/// Gets or sets the Security Provider used to authenticate the message credentials.
		/// </summary>
		protected ISecurityProvider SecurityProvider { get; set; }

		/// <summary>
		/// Gets or sets the Processor used to process parameter values.
		/// </summary>
		protected TProcessor Processor { get; set; }

		/// <summary>
		/// Processes the specified message's parameters and return value which match the type of TObject
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		public virtual void Handle(ResultMessage message)
		{
			if (message == null) throw new ArgumentNullException("message");

			HandleMessage(message);
			Process(message.ReturnValue as TObject);
		}

		/// <summary>
		/// Processes the specified message's parameters which match the type of TObject
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		public virtual void Handle(ExceptionMessage message)
		{
			HandleMessage(message);
		}

		/// <summary>
		/// Processes the specified message's parameters which match the type of TObject
		/// </summary>
		/// <param name="message">The message to be processed.</param>
		protected override void HandleMessage(Message message)
		{
			if (message == null) throw new ArgumentNullException("message");

			// Set the Current Principal using the Authentication information from the Message.
			Thread.CurrentPrincipal = SecurityProvider.AuthenticateUser(message.AuthenticationContext, message.UserId, message.SecurityToken);

			message.Parameters.ForEach(item => Process(item.Value as TObject));
		}

		/// <summary>
		/// Processes the parameter.
		/// </summary>
		/// <param name="item">The parameter value to be processed.</param>
		protected virtual void Process(TObject item)
		{
			if (item != null) Processor.Process(item);
		}
	}
}
