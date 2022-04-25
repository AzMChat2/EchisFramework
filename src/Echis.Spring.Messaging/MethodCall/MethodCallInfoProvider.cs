using System;
using System.Reflection;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Provides Method Messaging information from a configuration file.
	/// </summary>
	public sealed class MethodCallInfoProvider : IMethodInfoProvider
	{
		/// <summary>
		/// Gets the Messaging information for the specified method.
		/// </summary>
		/// <param name="method">The method for which the Message Queue Method info is being retrieved.</param>
		public MethodCallInfo GetMethodMessagingInfo(MethodInfo method)
		{
			if (method == null) throw new ArgumentNullException("method");

			Settings.Service serviceInfo = Settings.Values.FindService(method.DeclaringType.FullName);

			return (serviceInfo == null) ? new MethodCallInfo() : serviceInfo.FindMethod(method.Name);
		}
	}
}
