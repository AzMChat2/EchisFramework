using System;
using System.Threading;
using AopAlliance.Intercept;
using System.Security;

namespace System.Spring.Interceptors
{
	// IMethodInvocation is not CLS compliant, since it comes from the Spring framework, there's nothing we
	// can do about it, so disable CLS compliancy for this class.

	/// <summary>
	/// The Security Interceptor is used to perform authorization security checks at the Method level.
	/// </summary>
	[CLSCompliant(false)]
	public class SecurityInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// Gets the instance of the Security Provider
		/// </summary>
		protected ISecurityProvider SecurityProvider { get; set; }

		/// <summary>
		/// Calls the Security Provider to check if the user has permission to invoke the method.
		/// </summary>
		/// <param name="invocation">The method invocation object to be called if the user has appropriate permissions.</param>
		/// <returns>Returns the result of the Method Invocation.</returns>
		public object Invoke(IMethodInvocation invocation)
		{
			if (invocation == null) throw new ArgumentNullException("invocation");

			SecurityProvider.CheckSecurity(invocation.Method);
			return invocation.Proceed();
		}
	}
}
