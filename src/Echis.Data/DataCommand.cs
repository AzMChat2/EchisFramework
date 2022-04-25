using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using CommandTypes = System.Data.CommandType;

namespace System.Data
{
	/// <summary>
	/// Provides the information needed to create a Database Specific IDbCommand object.
	/// </summary>
	public class DataCommand : IDataCommand
	{
		/// <summary>
		/// Contains default values used by the DataCommand class
		/// </summary>
		private static class Defaults
		{
			/// <summary>
			/// Default value for the CommandTimeout property.
			/// </summary>
			public const int CommandTimeout = 30;
			/// <summary>
			/// Default value for the CommandType property.
			/// </summary>
			public const CommandTypes CommandType = CommandTypes.Text;
		}

		/// <summary>
		/// Occurs before execution of the command.
		/// </summary>
		public event EventHandler<EventArgs> Executing;

		/// <summary>
		/// Occurs after execution of the command.
		/// </summary>
		public event EventHandler<EventArgs> Executed;

		/// <summary>
		/// Occurs when a SystemException is caught during execution of the command.
		/// </summary>
		public event EventHandler<ExceptionEventArgs> DataException;

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public DataCommand()
		{
			Parameters = new QueryParameterCollection();
			CommandType = Defaults.CommandType;
			CommandTimeout = Defaults.CommandTimeout;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dataAccessName">The name of the DataAccess object which will execute this command.</param>
		/// <param name="commandText">The command text to be executed.</param>
		/// <param name="commandType">The type of command to be executed.</param>
		/// <param name="queryParams">Parameters (if any) of the command to be executed.</param>
		public DataCommand(string dataAccessName, string commandText, CommandType commandType, params IQueryParameter[] queryParams)
		{
			DataAccessName = dataAccessName;
			CommandText = commandText;
			CommandType = commandType;
			CommandTimeout = Defaults.CommandTimeout;
			Parameters = queryParams.IsNullOrEmpty() ? new QueryParameterCollection() : new QueryParameterCollection(queryParams);
		}

		/// <summary>
		/// Gets or sets the name of the DataAccess object which will execute this command. (Optional)
		/// </summary>
		public string DataAccessName { get; set; }

		/// <summary>
		/// Gets or sets the name of the Database which will execute this command. (Optional)
		/// </summary>
		public string DatabaseName { get; set; }

		/// <summary>
		/// Gets or sets the command text to be executed.
		/// </summary>
		public string CommandText { get; set; }

		/// <summary>
		/// Gets or sets the type of command to be executed.
		/// </summary>
		public CommandTypes CommandType { get; set; }

		/// <summary>
		/// Gets or sets the maximum amount of time allowed for the command to execute.
		/// </summary>
		/// <remarks>Defaults to 30 seconds.</remarks>
		public int CommandTimeout { get; set; }

		/// <summary>
		/// Gets or sets the Parameters of the command to be executed.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Intentionally allowing consumers to set the Parameters Collection instance.")]
		public IQueryParameterCollection Parameters { get; set; }

		/// <summary>
		/// Gets the IDbTransaction associated with this command.
		/// </summary>
		public IDbTransaction Transaction { get; set; }

		/// <summary>
		/// Fires the BeforeExecute event.
		/// </summary>
		public void OnBeforeExecute()
		{
			if (Executing != null) Executing.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fires the AfterExecute event.
		/// </summary>
		public void OnAfterExecute()
		{
			if (Executed != null) Executed.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fires the Exception event.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public bool OnDataException(SystemException ex)
		{
			bool retVal = false;

			if (DataException != null)
			{
				retVal = true;
				DataException.Invoke(this, new ExceptionEventArgs(ex));
			}

			return retVal;
		}
	}

	/// <summary>
	/// Represents a collection of IDataCommand objects.
	/// </summary>
	public sealed class DataCommandCollection : List<IDataCommand>, IDataCommandCollection
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="commands">The initial IDataCommand objects to be placed in the collection.</param>
		public DataCommandCollection(params IDataCommand[] commands)
		{
			if (!commands.IsNullOrEmpty())
			{
				AddRange(commands);
			}
		}
	}
}
