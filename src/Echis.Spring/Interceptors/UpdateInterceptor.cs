using System;
using System.Data;
using AopAlliance.Intercept;

namespace System.Spring.Interceptors
{
	// IMethodInvocation is not CLS compliant, since it comes from the Spring framework, there's nothing we
	// can do about it, so disable CLS compliancy for this class.

	/// <summary>
	/// The Update Interceptor is used to Upate any objects which implement the IUpdatabe interface unless the Skip Update Attribute is specified.
	/// </summary>
	[CLSCompliant(false)]
	public class UpdateInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// Updates any objects which implement IUpdatable unless the Skip Update Attribute is specified
		/// </summary>
		/// <param name="invocation">Information and access to the method being invoked.</param>
		/// <returns>Returns the results of the invocation.</returns>
		public object Invoke(IMethodInvocation invocation)
		{
			if (invocation == null) throw new ArgumentNullException("invocation");

			object retVal = invocation.Proceed();
			invocation.InvokeExclusiveAction<SkipUpdateAttribute, IUpdatable>(Update);
			return retVal;
		}

		/// <summary>
		/// Updates the IUpdatable Object
		/// </summary>
		/// <param name="arg">The IUpdatable Object to be updated.</param>
		private static void Update(IUpdatable arg)
		{
			arg.Update();
		}

	}
}
