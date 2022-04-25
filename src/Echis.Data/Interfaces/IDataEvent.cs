using System;

namespace System.Data
{
	/// <summary>
	/// Interface used to raise Data Events.
	/// </summary>
	public interface IDataEvents
	{
		/// <summary>
		/// Occurs before execution of the command.
		/// </summary>
		event EventHandler<EventArgs> Executing;

		/// <summary>
		/// Occurs after execution of the command.
		/// </summary>
		event EventHandler<EventArgs> Executed;

		/// <summary>
		/// Occurs when a SystemException is caught during execution of the command.
		/// </summary>
		event EventHandler<ExceptionEventArgs> DataException;
	
		/// <summary>
		/// Called by the DataAccess object before execution.
		/// </summary>
		void OnBeforeExecute();
		/// <summary>
		/// Called by the DataAccess object after execution.
		/// </summary>
		void OnAfterExecute();
		/// <summary>
		/// Called by the DataAccess object if an exception is thrown.
		/// </summary>
		/// <param name="ex">The exception which was thrown.</param>
		/// <returns>Returns true if the exception event was handled.</returns>
		bool OnDataException(SystemException ex);
	}
}
