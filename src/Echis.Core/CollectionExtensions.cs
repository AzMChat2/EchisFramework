using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System
{
	/// <summary>
	/// Provided extended methods for collections.
	/// </summary>
	public static class CollectionExtensions
	{

		/// <summary>
		/// Enqueues an enumerable collection of elements into the queue.
		/// </summary>
		/// <typeparam name="T">The type of elements in the queue and collection.</typeparam>
		/// <param name="queue">The queue to which the elements will be enqueued.</param>
		/// <param name="collection">The collection containing the elements to be enqueued.</param>
		public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> collection)
		{
			collection.ForEach(queue.Enqueue);
		}

		/// <summary>
		/// Dequeues a number of elements from a queue.
		/// </summary>
		/// <typeparam name="T">The type of elements in the queue and collection.</typeparam>
		/// <param name="queue">The queue to which the elements will be enqueued.</param>
		/// <param name="count">The number of elements to be dequeued.</param>
		/// <returns>Returns an array containing the elements which have been dequeued.</returns>
		public static T[] Dequeue<T>(this Queue<T> queue, int count)
		{
			if (queue == null) throw new ArgumentNullException("queue");
			if (count <= 0) throw new ArgumentOutOfRangeException("count");

			if (count > queue.Count) count = queue.Count;

			T[] retVal = new T[count];

			for (int idx = 0; idx < retVal.Length; idx++)
			{
				retVal[idx] = queue.Dequeue();
			}

			return retVal;
		}

		/// <summary>
		/// Performs the specified action on each element of the collection.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="validator">The validator to be used to test the item.</param>
		/// <param name="action">The delegate to perform on each element of the collection.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public static void ForEachIf<T>(this IEnumerable<T> collection, Predicate<T> validator, Action<T> action)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (validator == null) throw new ArgumentNullException("validator");
			if (action == null) throw new ArgumentNullException("action");
			foreach (T item in collection)
			{
				if (validator(item)) action(item);
			}
		}

		/// <summary>
		/// Performs the specified action on each element of the collection.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="validator">The validator to be used to test the item.</param>
		/// <param name="action">The delegate to perform on each element of the collection.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public static void ForEachIf<T>(this IEnumerable collection, Predicate<T> validator, Action<T> action)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (validator == null) throw new ArgumentNullException("validator");
			if (action == null) throw new ArgumentNullException("action");
			foreach (T item in collection)
			{
				if (validator(item)) action(item);
			}
		}

		/// <summary>
		/// Performs the specified action on each element of the collection.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="action">The delegate to perform on each element of the collection.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public static void ForEach<T>(this IEnumerable collection, Action<T> action)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (action == null) throw new ArgumentNullException("action");
			foreach (T item in collection)
			{
				action(item);
			}
		}

		/// <summary>
		/// Adds the specified item to the collection, if it passes validation.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="item">The item to be validated and added to the collection.</param>
		/// <param name="validator">The item validator.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public static void AddIf<T>(this ICollection<T> collection, T item, Predicate<T> validator)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (item == null) throw new ArgumentNullException("item");
			if (validator == null) throw new ArgumentNullException("validator");

			if (validator(item)) collection.Add(item);
		}

		/// <summary>
		/// Adds the specified items to the collection, if they passes validation.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="items">The items to be validated and added to the collection.</param>
		/// <param name="validator">The item validator.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public static void AddRangeIf<T>(this ICollection<T> collection, IEnumerable<T> items, Predicate<T> validator)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (items == null) throw new ArgumentNullException("items");
			if (validator == null) throw new ArgumentNullException("validator");

			foreach(var item in items)
				if (validator(item)) collection.Add(item);
		}
		/// <summary>
		/// Performs the specified action on each element of the collection even if an exception is thrown for one of the elements.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="action">The delegate to perform on each element of the collection.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		/// <returns>Returns an array of exceptions caught while performing the action on each element of the collection.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here.  Any exception thrown needs to be handled the same way: add to exceptions detail.")]
		public static ExceptionDetail<T>[] ForEachGuaranteed<T>(this IEnumerable<T> collection, Action<T> action)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (action == null) throw new ArgumentNullException("action");

			List<ExceptionDetail<T>> exceptions = new List<ExceptionDetail<T>>();

			foreach (T item in collection)
			{
				try
				{
					action(item);
				}
				catch (Exception ex)
				{
					exceptions.Add(new ExceptionDetail<T>(item, ex));
				}
			}

			return exceptions.ToArray();
		}

		/// <summary>
		/// Performs the specified action on each element of the collection even if an exception is thrown for one of the elements.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="action">The delegate to perform on each element of the collection.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		/// <returns>Returns an array of exceptions caught while performing the action on each element of the collection.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here.  Any exception thrown needs to be handled the same way: add to exceptions detail.")]
		public static ExceptionDetail<T>[] ForEachGuaranteed<T>(this IEnumerable collection, Action<T> action)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (action == null) throw new ArgumentNullException("action");

			List<ExceptionDetail<T>> exceptions = new List<ExceptionDetail<T>>();

			foreach (T item in collection)
			{
				try
				{
					action(item);
				}
				catch (Exception ex)
				{
					exceptions.Add(new ExceptionDetail<T>(item, ex));
				}
			}

			return exceptions.ToArray();
		}

		/// <summary>
		/// Performs the specified action on each element of the collection even if an exception is thrown for one of the elements.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="validator">The validator to be used to test the item.</param>
		/// <param name="action">The delegate to perform on each element of the collection.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		/// <returns>Returns an array of exceptions caught while performing the action on each element of the collection.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here.  Any exception thrown needs to be handled the same way: add to exceptions detail.")]
		public static ExceptionDetail<T>[] ForEachGuaranteedIf<T>(this IEnumerable<T> collection, Predicate<T> validator, Action<T> action)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (action == null) throw new ArgumentNullException("action");
			if (validator == null) throw new ArgumentNullException("validator");

			List<ExceptionDetail<T>> exceptions = new List<ExceptionDetail<T>>();

			foreach (T item in collection)
			{
				try
				{
					if (validator(item)) action(item);
				}
				catch (Exception ex)
				{
					exceptions.Add(new ExceptionDetail<T>(item, ex));
				}
			}

			return exceptions.ToArray();
		}

		/// <summary>
		/// Performs the specified action on each element of the collection even if an exception is thrown for one of the elements.
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements upon which the action will be performed.</param>
		/// <param name="validator">The validator to be used to test the item.</param>
		/// <param name="action">The delegate to perform on each element of the collection.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		/// <returns>Returns an array of exceptions caught while performing the action on each element of the collection.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It doesn't matter what exceptions might be thrown here.  Any exception thrown needs to be handled the same way: add to exceptions detail.")]
		public static ExceptionDetail<T>[] ForEachGuaranteedIf<T>(this IEnumerable collection, Predicate<T> validator, Action<T> action)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (action == null) throw new ArgumentNullException("action");
			if (validator == null) throw new ArgumentNullException("validator");

			List<ExceptionDetail<T>> exceptions = new List<ExceptionDetail<T>>();

			foreach (T item in collection)
			{
				try
				{
					if (validator(item)) action(item);
				}
				catch (Exception ex)
				{
					exceptions.Add(new ExceptionDetail<T>(item, ex));
				}
			}

			return exceptions.ToArray();
		}

		/// <summary>
		/// Determines whether every element in the List matches the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="collection">The collection of elements to be tested.</param>
		/// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions to check against the elements.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		public static bool TrueForAll<T>(this IEnumerable<T> collection, Predicate<T> match)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (match == null) throw new ArgumentNullException("match");

			bool retVal = true;

			foreach (T item in collection)
			{
				if (!match(item))
				{
					retVal = false;
					break;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Test to see if the specified array is null or contains no elements.
		/// </summary>
		/// <param name="collection">The array to be tested.</param>
		/// <returns>Returns true if the array is null or contains no elements.</returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return ((collection == null) || (collection.Count() == 0));
		}

		/// <summary>
		/// Joins the elements of a collection into a delimited string
		/// </summary>
		/// <typeparam name="T">The type of elements in the collection.</typeparam>
		/// <param name="collection">The collection of elements whose values will be concatenated into a string.</param>
		/// <param name="delimiter">The string to use as a separator.</param>
		/// <returns>Returns a delimited string containing the values of the collection</returns>
		public static string Join<T>(this IEnumerable<T> collection, string delimiter)
		{
			if (collection.IsNullOrEmpty()) return null;
			return string.Join(delimiter, collection);
		}

		/// <summary>
		/// Calculates the edit distance between two Enumerable objects.
		/// </summary>
		/// <typeparam name="T">The type of Enumerable object.</typeparam>
		/// <param name="source">The source Enumerable object.</param>
		/// <param name="target">The target Enumerable object.</param>
		/// <returns>A count of how many adds/deletes/changes are required to modify the Source to match the Target.</returns>
		public static int CalculateEditDistance<T>(this IEnumerable<T> source, IEnumerable<T> target)
		where T : IEquatable<T>
		{
			// Validate parameters
			if (source == null) throw new ArgumentNullException("source");
			if (target == null) throw new ArgumentNullException("target");

			// Convert the parameters into List instances
			// in order to obtain indexing capabilities
			List<T> first = source.ToList();
			List<T> second = target.ToList();

			// Get the length of both.  If either is 0, return
			// the length of the other, since that number of insertions
			// would be required.
			int n = first.Count, m = second.Count;
			if (n == 0) return m;
			if (m == 0) return n;

			// Rather than maintain an entire matrix (which would require O(n*m) space),
			// just store the current row and the next row, each of which has a length m+1,
			// so just O(m) space. Initialize the current row.
			int curRow = 0, nextRow = 1;
			int[][] rows = new int[][] { new int[m + 1], new int[m + 1] };
			for (int j = 0; j <= m; ++j) rows[curRow][j] = j;

			// For each virtual row (since we only have physical storage for two)
			for (int i = 1; i <= n; ++i)
			{
				// Fill in the values in the row
				rows[nextRow][0] = i;
				for (int j = 1; j <= m; ++j)
				{
					int dist1 = rows[curRow][j] + 1;
					int dist2 = rows[nextRow][j - 1] + 1;
					int dist3 = rows[curRow][j - 1] +
							(first[i - 1].Equals(second[j - 1]) ? 0 : 1);

					rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
				}

				// Swap the current and next rows
				if (curRow == 0)
				{
					curRow = 1;
					nextRow = 0;
				}
				else
				{
					curRow = 0;
					nextRow = 1;
				}
			}

			// Return the computed edit distance
			return rows[curRow][m];

		}

	}
}
