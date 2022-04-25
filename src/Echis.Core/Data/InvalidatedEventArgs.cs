using System;

namespace System.Data
{
	/// <summary>
	/// Contains event information for the Invalidated event.
	/// </summary>
	public class InvalidatedEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">The message containing the reason the object was invalidated.</param>
		public InvalidatedEventArgs(string message)
		{
			Message = message;
		}

		/// <summary>
		/// Gets the messagecontaining the reason the object was invalidated.
		/// </summary>
		public string Message { get; private set; }
	}
}
