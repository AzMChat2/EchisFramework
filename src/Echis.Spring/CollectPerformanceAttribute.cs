using System;

namespace Echis.Spring
{
	/// <summary>
	/// Attribute used by Spring Framework to invoke the Performance Interceptor.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class CollectPerformanceAttribute : Attribute
	{
	}
}
