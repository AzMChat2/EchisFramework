using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Objects
{
	/// <summary>
	/// Represents a list of Business Objects
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", 
		Justification = "Base interface type for IBusinessObjectCollection<T>")]
	public interface IBusinessObjectCollection : IEnumerable, IDataLoader, IValidatable, IUpdatable
	{
		/// <summary>
		/// Performs the specified action on each element of the list.
		/// </summary>
		/// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the list.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		void ForEach(Action<IBusinessObject> action);
	}

	/// <summary>
	/// Represents a list of Business Objects
	/// </summary>
	/// <typeparam name="T">The interface type of the Business Object elements stored in the List.</typeparam>
	public interface IBusinessObjectCollection<T> : IList<T>, IBusinessObjectCollection where T : IBusinessObject
	{
	}
}
