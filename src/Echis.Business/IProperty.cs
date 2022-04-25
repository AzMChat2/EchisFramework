using System.ComponentModel;
using System.Data;

namespace Echis.Data.Objects
{
	#region IProperty
	/// <summary>
	/// Defines the basic Business Object Property.
	/// </summary>
	public interface IProperty : IValidatable, IUpdatable, INotifyPropertyChanged, INotifyPropertyChanging
	{
		/// <summary>
		/// Gets the name of the property
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Gets or sets the value of the property.
		/// </summary>
		object Value { get; set; }
		/// <summary>
		/// Gets the original (or stored) value of the property.
		/// </summary>
		object OldValue { get; }
	}
	#endregion

	#region IProperty<T>
	/// <summary>
	/// Defines a strongly type Property.
	/// </summary>
	/// <typeparam name="T">The property type.</typeparam>
	public interface IProperty<T> : IProperty
	{
		/// <summary>
		/// Gets or sets the value of the property.
		/// </summary>
		new T Value { get; set; }
		/// <summary>
		/// Gets the original (or stored) value of the property.
		/// </summary>
		new T OldValue { get; }
	}
	#endregion
}
