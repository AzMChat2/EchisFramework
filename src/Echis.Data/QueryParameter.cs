using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace System.Data
{
	/// <summary>
	/// Represents a generic Data Parameter
	/// </summary>
	public class QueryParameter : IQueryParameter
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public QueryParameter() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The value of the parameter.</param>
		public QueryParameter(string name, object value) : this(name, value, ParameterDirection.Input) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="direction">A flag indicating if the direction of the parameter.</param>
		public QueryParameter(string name, object value, ParameterDirection direction)
		{
			Name = name;
			Value = value;
			Direction = direction;
		}

		/// <summary>
		/// Gets or sets the name of the parameter.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the value of the parameter.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the direction of the parameter.
		/// </summary>
		public ParameterDirection Direction { get; set; }


		private IDataParameter _parameter;
		/// <summary>
		/// Used internally by the System Framework to get or set the actual IDataParameter used.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification="This is only used internally by the System Framework")]
		IDataParameter IQueryParameter.Parameter
		{
			get { return _parameter; }
			set { _parameter = value; }
		}

		/// <summary>
		/// Used internally by the System Framework to update the IQueryParamter value from the actual IDataParameter.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "This is only used internally by the System Framework")]
		void IQueryParameter.UpdateParameterValue()
		{
			if (_parameter != null)
			{
				Value = _parameter.Value;
				_parameter = null;
			}
		}
	}

	/// <summary>
	/// Represents a collection of Query Parameters.
	/// </summary>
	public class QueryParameterCollection : List<IQueryParameter>, IQueryParameterCollection
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public QueryParameterCollection() : base() { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="capacity">The intitial capacity of the collection.</param>
		public QueryParameterCollection(int capacity) : base(capacity) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="collection">The initial set of Query Parameter objects to be placed in the collection.</param>
		public QueryParameterCollection(IEnumerable<IQueryParameter> collection) : base(collection) { }

		/// <summary>
		/// Creates and adds a new QueryParameter to the collection.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The value of the parameter.</param>
		public void Add(string name, object value)
		{
			Add(new QueryParameter(name, value));
		}

		/// <summary>
		/// Creates and adds a new QueryParameter to the collection.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="direction">A flag indicating if the direction of the parameter.</param>
		public void Add(string name, object value, ParameterDirection direction)
		{
			Add(new QueryParameter(name, value, direction));
		}

		/// <summary>
		/// Used internally by the System Framework to update the IQueryParamter values from the actual IDataParameters.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "This is only used internally by the System Framework")]
		void IQueryParameterCollection.UpdateParameterValues()
		{
			ForEach(item => item.UpdateParameterValue());
		}
	}
}
