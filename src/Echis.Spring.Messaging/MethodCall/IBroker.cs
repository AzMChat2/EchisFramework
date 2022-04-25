
namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Manages matching Response Messages with their corresponding Request Message
	/// </summary>
	public interface IBroker
  {
		/// <summary>
		/// Gets or sets the timeout period for the Response.
		/// </summary>
		int Timeout { get; set; }

		/// <summary>
		/// Registeres a call to be Brokered.
		/// </summary>
		/// <param name="message">The Request Message to be Brokered.</param>
		void RegisterCall(RequestMessage message);

		/// <summary>
		/// Blocks the current thread until the Result Message for the specified Request Message has been recieved.
		/// </summary>
		/// <param name="message">The Request Message.</param>
		object WaitForResult(RequestMessage message);

		/// <summary>
		/// Records the result and releases the Blocked Thread.
		/// </summary>
		/// <param name="message">The result message.</param>
		void RegisterResult(ResultMessage message);

		/// <summary>
		/// Records the exception and releases the Blocked Thread.
		/// </summary>
		/// <param name="message">The exception message.</param>
		void RegisterException(ExceptionMessage message);
	}
}
