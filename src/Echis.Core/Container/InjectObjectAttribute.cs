using System;

namespace System.Container
{
	/// <summary>
	/// Attribute used to automatically inject properties with values from the IOC Container.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
	public sealed class InjectObjectAttribute : Attribute
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public InjectObjectAttribute() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		public InjectObjectAttribute(string objectId)
		{
			ObjectId = objectId;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public InjectObjectAttribute(string contextId, string objectId)
		{
			ContextId = contextId;
			ObjectId = objectId;
		}

		/// <summary>
		/// Gets or sets a value which defines the order in which Injection Attributes are processed if more than one Injection Attribute is defined.
		/// </summary>
		public int ExecutionOrder { get; set; }

		/// <summary>
		/// Gets the Context Id of the object to inject.
		/// </summary>
		public string ContextId { get; private set; }

		/// <summary>
		/// Gets the Object Id of the object to inject.
		/// </summary>
		public string ObjectId { get; private set; }
	}
}

