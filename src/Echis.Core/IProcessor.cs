namespace System
{
	/// <summary>
	/// Defines a processor that processes the specified object.
	/// </summary>
	/// <typeparam name="TObject">The type of object to be processed.</typeparam>
	public interface IProcessor<in TObject>
	{
		/// <summary>
		/// Processes the specified item.
		/// </summary>
		/// <param name="item">The object to be processed.</param>
		void Process(TObject item);
	}
}
