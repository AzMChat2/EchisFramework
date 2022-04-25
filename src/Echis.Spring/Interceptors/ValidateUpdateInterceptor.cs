using System;
using System.Data;
using AopAlliance.Intercept;

namespace System.Spring.Interceptors
{
	// IMethodInvocation is not CLS compliant, since it comes from the Spring framework, there's nothing we
	// can do about it, so disable CLS compliancy for this class.

	/// <summary>
	/// The Validate and Update Interceptor is used to Validate any objects which implement the IValidatable interface unless the Skip Validate Attribute is specified
	/// and to Update any object which implement the IUpdatable interface unless the Skip Update Attribute is specified
	/// </summary>
	[CLSCompliant(false)]
	public class ValidateUpdateInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// The Context under which the Business Objects will be validated
		/// </summary>
		public string ContextId { get; set; }

		/// <summary>
		/// Validates any objects which implement IBusinessObject unless the Skip Validation Attribute is specified
		/// </summary>
		/// <param name="invocation">Information and access to the method being invoked.</param>
		/// <returns>Returns the results of the invocation.</returns>
		public object Invoke(IMethodInvocation invocation)
		{
			if (invocation == null) throw new ArgumentNullException("invocation");

			invocation.InvokeExclusiveAction<SkipValidateAttribute, IValidatable>(Validate);
			object retVal = invocation.Proceed();
			invocation.InvokeExclusiveAction<SkipUpdateAttribute, IUpdatable>(Update);
			return retVal;
		}

		/// <summary>
		/// Validates the IValidatable Object, throws a DataObjectNotValidException if the valdiation fails.
		/// </summary>
		/// <param name="arg">The IValidatable Object to be validated.</param>
		private void Validate(IValidatable arg)
		{
			if (!arg.IsValid(ContextId)) throw new DataObjectNotValidException(arg);
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
