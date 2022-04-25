using System;
using System.Xml;
using AopAlliance.Intercept;
using System.Caching;

namespace System.Spring.Interceptors
{
	// IMethodInvocation is not CLS compliant, since it comes from the Spring framework, there's nothing we
	// can do about it, so disable CLS compliancy for this class.

	/// <summary>
	/// The Cache Interceptor is used to Cache results of method calls to service methods.  Actual caching is performed by the Cache Provider
	/// </summary>
	[CLSCompliant(false)]
	public class CacheInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// Gets the instance of the Cache Provider
		/// </summary>
		protected ICacheProvider CacheProvider { get; set; }

		/// <summary>
		/// Calls the Cache Provider to see if the return value for this method has been cached.
		/// </summary>
		/// <param name="invocation">The method invocation object to be called if the object has not been cached.</param>
		/// <returns>Returns the result of the Method Invocation or the value from the cache if cached.</returns>
		public object Invoke(IMethodInvocation invocation)
		{
			if (invocation == null) throw new ArgumentNullException("invocation");

			CacheInfo info = CacheProvider.GetCacheInfo(invocation.Method, invocation.Arguments);

			if (info == null)
			{
				return invocation.Proceed();
			}
			else
			{
				if (info.Value == null)
				{
					info.Value = new XmlWrapper() { Value = invocation.Proceed() };
					CacheProvider.AddToCache(info);
				}

				return info.Value.Value;
			}
		}

	}
}
