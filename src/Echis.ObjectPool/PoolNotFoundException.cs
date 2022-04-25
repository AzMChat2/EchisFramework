using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.ObjectPools
{
	/// <summary>
	/// Exception thrown when an Object Pool was not found.
	/// </summary>
	[Serializable]
	public class PoolNotFoundException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public PoolNotFoundException() : base(GetMessage("unknown")) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		public PoolNotFoundException(string poolName) : base(GetMessage(poolName)) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		public PoolNotFoundException(string poolName, Exception inner) : base(GetMessage(poolName), inner) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		protected PoolNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		protected static string GetMessage(string poolName)
		{
			return string.Format(CultureInfo.InvariantCulture, "The Named Pool specified ('{0}') does not exist.", poolName);
		}
	}
}
