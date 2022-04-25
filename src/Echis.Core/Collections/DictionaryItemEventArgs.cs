
namespace System.Collections.Generic
{
	/// <summary>
	/// Contains event data for the ItemAdded event.
	/// </summary>
	/// <typeparam name="TKey">The type of key which was added to the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of object which was added to the dictionary.</typeparam>
	public class DictionaryItemEventArgs<TKey, TValue> : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="key">The Key which was added to the list.</param>
		/// <param name="value">The Value which was added to the list.</param>
		public DictionaryItemEventArgs(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		/// <summary>
		/// Gets the item key which was added to the Dictionary.
		/// </summary>
		public TKey Key { get; private set; }

		/// <summary>
		/// Gets the item value which was added to the Dictionary.
		/// </summary>
		public TValue Value { get; private set; }
	}
}
