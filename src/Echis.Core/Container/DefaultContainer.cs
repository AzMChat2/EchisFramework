using System.Diagnostics.CodeAnalysis;
using System;

namespace System.Container
{
	internal sealed class DefaultContainer : IContainer
	{
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", 
			Justification="This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification="This is a purposefully empty class used only when no container is specified.")]
		public void InjectObjectDependencies(object obj)
		{
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public void InjectObjectDependencies(string contextId, object obj)
		{
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public bool ContainsObject<T>()
		{
			return false;
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public bool ContainsObject(string objectId)
		{
			return false;
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public bool ContainsObject(string contextId, string objectId)
		{
			return false;
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public bool ContainsContext(string contextId)
		{
			return false;
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObject<T>()
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObject<T>(string objectId)
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObject<T>(string contextId, string objectId)
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectUnsafe<T>()
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectUnsafe<T>(string objectId)
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectUnsafe<T>(string contextId, string objectId)
		{
			return default(T);
		}


		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInject<T>()
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInject<T>(string objectId)
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInject<T>(string contextId, string objectId)
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInjectUnsafe<T>()
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInjectUnsafe<T>(string objectId)
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInjectUnsafe<T>(string contextId, string objectId)
		{
			return default(T);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObject<T>(Type defaultType)
		{
			return ReflectionExtensions.CreateObject<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObject<T>(string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObject<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObject<T>(string contextId, string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObject<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectUnsafe<T>(Type defaultType)
		{
			return ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectUnsafe<T>(string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectUnsafe<T>(string contextId, string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInject<T>(Type defaultType)
		{
			return ReflectionExtensions.CreateObject<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInject<T>(string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObject<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInject<T>(string contextId, string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObject<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInjectUnsafe<T>(Type defaultType)
		{
			return ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInjectUnsafe<T>(string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
			Justification = "This is a purposefully empty class used only when no container is specified.")]
		public T GetObjectAndInjectUnsafe<T>(string contextId, string objectId, Type defaultType)
		{
			return ReflectionExtensions.CreateObjectUnsafe<T>(defaultType);
		}
	}
}
