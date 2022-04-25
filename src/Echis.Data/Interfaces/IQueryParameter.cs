using System.Collections.Generic;
using System.Data;

namespace System.Data
{
	/// <summary>
	/// Represents a generic Data Parameter
	/// </summary>
	public interface IQueryParameter
	{
		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Gets the Value of the parameter.
		/// </summary>
		object Value { get; set; }
		/// <summary>
		///  Gets a flag indicating if the direction of the parameter.
		/// </summary>
		ParameterDirection Direction { get; }

		/// <summary>
		/// Used internally by the System Framework to get or set the actual IDataParameter used.
		/// </summary>
		IDataParameter Parameter { get; set; }

		/// <summary>
		/// Used internally by the System Framework to update the IQueryParamter value from the actual IDataParameter.
		/// </summary>
		void UpdateParameterValue();
	}

	/// <summary>
	/// Represents a list of DataParameters.
	/// </summary>
	public interface IQueryParameterCollection : IList<IQueryParameter>
	{
		/// <summary>
		/// Used internally by the System Framework to update the IQueryParamter values from the actual IDataParameters.
		/// </summary>
		void UpdateParameterValues();
	}

}
