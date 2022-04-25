
namespace System.Data
{
	/// <summary>
	/// Defines an object that is updatable.
	/// </summary>
	public interface IUpdatable
	{
		/// <summary>
		/// Gets a flag indicating if the object is new.
		/// </summary>
		bool IsNew { get; set; }
		/// <summary>
		/// Gets a flag indicating if any of the object's properties have changed.
		/// </summary>
		bool IsDirty { get; }
		/// <summary>
		/// Updates the object. This is called after the object has been persisted.
		/// </summary>
		void Update();
		/// <summary>
		/// Resets the object to it's original state.
		/// </summary>
		void Reset();
	}
}
