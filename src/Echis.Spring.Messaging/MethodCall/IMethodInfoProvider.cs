using System.Reflection;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Defines a Message Info Provider which will provide Messaging Information for a given method call.
	/// </summary>
	public interface IMethodInfoProvider
	{
		/// <summary>
		/// Gets the Messaging information for the specified method.
		/// </summary>
		/// <param name="method">The method for which the Message Queue Method info is being retrieved.</param>
		MethodCallInfo GetMethodMessagingInfo(MethodInfo method);
	}
}
