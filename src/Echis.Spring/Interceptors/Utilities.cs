using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AopAlliance.Intercept;

namespace System.Spring.Interceptors
{
	/// <summary>
	/// Provides utility helper methods for Interceptors
	/// </summary>
	[CLSCompliant(false)]
	public static class Utilities
	{
		/// <summary>
		/// Invokes an action against invocation arguments which match the specified type and are not included in the parameters to skip
		/// </summary>
		/// <typeparam name="TAttribute">The type of attribute specifying parameters to skip</typeparam>
		/// <typeparam name="TArgument">The type of attribute upon which the action will be invoked</typeparam>
		/// <param name="invocation">The AOP invocation information about the method which is being invoked.</param>
		/// <param name="action">The action to be invoked against the arguments which match the specified type and are not included in the parameters to skip</param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method")]
		public static void InvokeExclusiveAction<TAttribute, TArgument>(this IMethodInvocation invocation, Action<TArgument> action)
			where TAttribute : Attribute, IParameterNames
			where TArgument : class
		{
			if (invocation == null) throw new ArgumentNullException("invocation");
			if (action == null) throw new ArgumentNullException("action");

			if (!invocation.Arguments.IsNullOrEmpty())
			{
				TAttribute attribute = invocation.Method.FindAttribute<TAttribute>();

				List<string> parametersToSkip = attribute == null ? null : attribute.ParameterNames;

				invocation.Arguments.GetExclusiveArguments<TArgument>(invocation.Method.GetParameters(), parametersToSkip)
					.ForEach(action);
			}
		}

		/// <summary>
		/// Invokes an action against invocation arguments which match the specified type and are included in the parameters to add
		/// </summary>
		/// <typeparam name="TAttribute">The type of attribute specifying parameters to add</typeparam>
		/// <typeparam name="TArgument">The type of attribute upon which the action will be invoked</typeparam>
		/// <param name="invocation">The AOP invocation information about the method which is being invoked.</param>
		/// <param name="action">The action to be invoked against the arguments which match the specified type and are included in the parameters to add</param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method")]
		public static void InvokeInclusiveAction<TAttribute, TArgument>(this IMethodInvocation invocation, Action<TArgument> action)
			where TAttribute : Attribute, IParameterNames
			where TArgument : class
		{
			if (invocation == null) throw new ArgumentNullException("invocation");
			if (action == null) throw new ArgumentNullException("action");

			if (!invocation.Arguments.IsNullOrEmpty())
			{
				TAttribute attribute = invocation.Method.FindAttribute<TAttribute>();

				List<string> parametersToSkip = attribute == null ? null : attribute.ParameterNames;

				invocation.Arguments.GetInclusiveArguments<TArgument>(invocation.Method.GetParameters(), parametersToSkip)
					.ForEach(action);
			}
		}

		/// <summary>
		/// Gets all arguments of the specified type which is included in the parameters to add.
		/// </summary>
		public static List<T> GetInclusiveArguments<T>(this object[] arguments, ParameterInfo[] parameters, List<string> parametersToAdd)
			where T : class
		{
			if (arguments == null) throw new ArgumentNullException("arguments");
			if (parameters == null) throw new ArgumentNullException("parameters");

			List<T> args = new List<T>();

			for (int idx = 0; idx < parameters.Length; idx++)
			{
				T arg = arguments[idx] as T;
				if ((arg != null) && parametersToAdd.ContainsParameter(parameters[idx].Name)) args.Add(arg);
			}

			return args;
		}

		/// <summary>
		/// Gets all arguments of the specified type which is not included in the parameters to skip.
		/// </summary>
		public static List<T> GetExclusiveArguments<T>(this object[] arguments, ParameterInfo[] parameters, List<string> parametersToSkip)
			where T : class
		{
			if (arguments == null) throw new ArgumentNullException("arguments");
			if (parameters == null) throw new ArgumentNullException("parameters");

			List<T> args = new List<T>();

			for (int idx = 0; idx < parameters.Length; idx++)
			{
				T arg = arguments[idx] as T;
				if ((arg != null) && !parametersToSkip.ContainsParameter(parameters[idx].Name)) args.Add(arg);
			}

			return args;
		}

		/// <summary>
		/// Determines if the specified parameter should be skipped.
		/// </summary>
		public static bool ContainsParameter(this List<string> parametersToSkip, string parameterName)
		{
			return parametersToSkip == null ? false : parametersToSkip.Contains(parameterName);
		}
	}
}
