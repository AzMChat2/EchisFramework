using System.ComponentModel;
using System.Data;

namespace System.Data.Objects
{
	#region IBusinessObject
	/// <summary>
	/// Defines a Business Object
	/// </summary>
	public interface IBusinessObject : IDataLoader, IValidatable, IUpdatable, INotifyPropertyChanged, INotifyPropertyChanging
	{
		/// <summary>
		/// Gets the list of Properties.
		/// </summary>
		PropertyCollection Properties { get; }
	}
	#endregion
}
