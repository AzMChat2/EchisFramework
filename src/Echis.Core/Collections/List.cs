using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
	#region ListEx<TClass, TInterface>
	/// <summary>
	/// Represents a strongly typed list of objects which can be accessed by index. Provides methods to sort, search and manipulate lists.
	/// </summary>
	/// <typeparam name="TClass">The type of elements contained in the list</typeparam>
	/// <typeparam name="TInterface">The interface type of the elements contained in the list.</typeparam>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "ListEx<T> is an EXtended List<T>.")]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix",
		Justification = "ListEx<T> is an EXtended List<T>.")]
	[Serializable]
	public class List<TClass, TInterface> : List<TClass>, IList<TInterface>
		where TClass : class, TInterface
	{
		/// <summary>
		/// Translates an IEnumerable collection of Interface objects to a List of Class objects.
		/// </summary>
		/// <param name="collection">The collection to be translated.</param>
		protected static List<TClass> Translate(IEnumerable<TInterface> collection)
		{
			List<TClass> retVal = new List<TClass>();
			collection.ForEach(item => retVal.Add(Translate(item)));
			return retVal;
		}

		/// <summary>
		/// Translates an object of the Interface Type to the Class Type.
		/// </summary>
		/// <param name="item">The item to be translated.</param>
		/// <returns></returns>
		protected static TClass Translate(TInterface item)
		{
			return (TClass)item;
		}

		/// <summary>
		/// Initializes a new instance of the ListEx class that is empty and has the default initial capacity.
		/// </summary>
		public List() : base() { }

		/// <summary>
		/// Initializes a new instance of the ListEx class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new list can initially store.</param>
		public List(int capacity) : base(capacity) { }

		/// <summary>
		/// Initializes a new instance of the ListEx class that contains elements copied from the specified collection
		/// and has sufficient capacity to accomodate the number of elements copied.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		public List(IEnumerable<TClass> collection) : base(collection) { }

		/// <summary>
		/// Initializes a new instance of the ListEx class that contains elements copied from the specified collection
		/// and has sufficient capacity to accomodate the number of elements copied.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		public List(IEnumerable<TInterface> collection) : base(Translate(collection)) { }

		/// <summary>
		/// Gets a value indicating whether the IList is read-only.
		/// </summary>
		public bool IsReadOnly { get { return false; } }


		int IList<TInterface>.IndexOf(TInterface item)
		{
			return IndexOf(Translate(item));
		}

		void IList<TInterface>.Insert(int index, TInterface item)
		{
			Insert(index, Translate(item));
		}

		TInterface IList<TInterface>.this[int index]
		{
			get { return this[index]; }
			set { this[index] = value as TClass; }
		}

		void ICollection<TInterface>.Add(TInterface item)
		{
			Add(Translate(item));
		}

		bool ICollection<TInterface>.Contains(TInterface item)
		{
			return Contains(Translate(item));
		}

		void ICollection<TInterface>.CopyTo(TInterface[] array, int arrayIndex)
		{
			if (array == null) throw new ArgumentNullException("array");

			TClass[] cArray = new TClass[array.Length];
			CopyTo(cArray, arrayIndex);
			for (int idx = 0; idx < array.Length; idx++) array[idx] = cArray[idx];
		}

		bool ICollection<TInterface>.Remove(TInterface item)
		{
			return Remove(Translate(item));
		}

		IEnumerator<TInterface> IEnumerable<TInterface>.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
#endregion
}


