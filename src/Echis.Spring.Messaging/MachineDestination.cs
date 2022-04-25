using System;
using System.Globalization;
using Apache.NMS.ActiveMQ.Commands;

namespace System.Spring.Messaging
{
	/// <summary>
	/// Represents a destination containing the current Machine Name
	/// </summary>
	public class MachineDestination : ActiveMQQueue
	{
		/// <summary>
		/// Creates a Destination using the current Machine Name
		/// </summary>
		/// <param name="destinationName">The destination name with a format place-holder for the Machine Name</param>
		public MachineDestination(string destinationName)
			: base(string.Format(CultureInfo.InvariantCulture, destinationName, Environment.MachineName)) { }
	}
}
