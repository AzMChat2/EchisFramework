
namespace System.Collections.Generic
{
	/// <summary>
	/// Contains event data for the ItemAdded event.
	/// </summary>
	/// <typeparam name="T">The type of object which was added to the list.</typeparam>
	public class ItemEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="item">The item which was added to the list.</param>
		public ItemEventArgs(T item)
		{
			Item = item;
		}

		/// <summary>
		/// Gets the item which was added to the List.
		/// </summary>
		public T Item
		{
			get;
			private set;
		}
	}
}
