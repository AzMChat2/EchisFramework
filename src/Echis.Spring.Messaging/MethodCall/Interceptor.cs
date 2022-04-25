using System;
using System.Linq;
using AopAlliance.Intercept;

namespace System.Spring.Messaging.MethodCall
{
	// IMethodInvocation is not CLS compliant, since it comes from the Spring framework, there's nothing we
	// can do about it, so disable CLS compliancy for this class.

	/// <summary>
	/// The Messaging Interceptor is used to generate messages from method calls to service methods.  Actual messaging is performed by the Method Messaging Provider
	/// </summary>
	[CLSCompliant(false)]
	public class Interceptor : IMethodInterceptor
	{
		/// <summary>
		/// Gets or sets the Method Messaging Provider used to send messages based on the method call.
		/// </summary>
		public IMessagingProvider MethodMessagingProvider { get; set; }

		/// <summary>
		/// Gets or sets the Messaging Info Provider used to retrieve messaging information for the method call.
		/// </summary>
		public IMethodInfoProvider MethodMessagingInfoProvider { get; set; }

		/// <summary>
		/// Calls the Method Messaging Provider to see if this method generates a message.
		/// If a message is to be generated, then the Method Messaging Provider is called to send the message.
		/// </summary>
		/// <param name="invocation">The method invocation object to be called if the call is not being redirected to a message.</param>
		/// <returns>Returns the result of the Method Invocation or the value returned from the Message Queue.</returns>
    public object Invoke(IMethodInvocation invocation)
    {
      if (invocation == null) throw new ArgumentNullException("invocation");

      MethodCallInfo info = MethodMessagingInfoProvider.GetMethodMessagingInfo(invocation.Method);

			// Send PreInvoke Notification Messages
			if (info.Notifications != null)
				(from notify in info.Notifications where notify.NotificationType == NotificationType.PreInvocation select notify)
					.ForEach(notify => MethodMessagingProvider.SendMessage(invocation.Method, invocation.Arguments, notify));

			try
			{
				object retVal = info.Redirect ? Redirect(invocation, info) : invocation.Proceed();

				// Send Success Notification Messages
				if (info.Notifications != null)
					(from notify in info.Notifications where notify.NotificationType == NotificationType.Success select notify)
						.ForEach(notify => MethodMessagingProvider.SendMessage(invocation.Method, invocation.Arguments, notify, retVal));

				return retVal;
			}
			catch (Exception ex)
			{
				// Send Failure Notification Messages
				if (info.Notifications != null)
					(from notify in info.Notifications where notify.NotificationType == NotificationType.Fail select notify)
						.ForEach(notify => MethodMessagingProvider.SendMessage(invocation.Method, invocation.Arguments, notify, ex));

				throw;
			}
    }

		/// <summary>
		/// Redirects a Method Call to the Message Queue
		/// </summary>
		/// <param name="invocation">The method invocation object to be called if the call is not being redirected to a message.</param>
		/// <param name="info">Information used to send the message.</param>
		/// <returns>Returns the value returned from the Message Queue or null if the Wait For Response flag is false.</returns>
		protected object Redirect(IMethodInvocation invocation, MethodCallInfo info)
		{
			if (invocation == null) throw new ArgumentNullException("invocation");
			if (info == null) throw new ArgumentNullException("info");

			if (info.WaitForResponse)
			{
				return MethodMessagingProvider.SendMessageAndWaitForResult(invocation.Method, invocation.Arguments, info);
			}
			else
			{
				MethodMessagingProvider.SendMessage(invocation.Method, invocation.Arguments, info);
				return null;
			}
		}
	}
}
