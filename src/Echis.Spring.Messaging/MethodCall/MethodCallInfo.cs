using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Interface defining information used to send a message.
	/// </summary>
	public interface IMessageInfo
	{
		/// <summary>
		/// Gets the name of the Queue to which the Message will be sent.
		/// </summary>
		string Destination { get; }
		/// <summary>
		/// Gets a flag indicating if the Message should be sent to the Pub-Sub Domain
		/// </summary>
		bool PubSubdomain { get; }
		/// <summary>
		/// Gets the delivery delay in milliseconds for the Message.
		/// </summary>
		long Delay { get; }
	}

	/// <summary>
	/// Represents information used to Send Messages based on a Method Call.
	/// </summary>
	public class MethodCallInfo : IMessageInfo
	{
		/// <summary>
		/// Gets or sets the Method Name of the method being invoked.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the call should be redirected.
		/// </summary>
		[XmlAttribute]
		public bool Redirect { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the call should wait for a response or return null.
		/// </summary>
		[XmlAttribute]
		public bool WaitForResponse { get; set; }

		/// <summary>
		/// Gets or sets the name of the Queue to which the Message will be sent.
		/// </summary>
		[XmlAttribute]
		public string Destination { get; set; }

		/// <summary>
		/// Gets a flag indicating if the Message should be sent to the Pub-Sub Domain
		/// </summary>
		/// <remarks>Always false for Redirected Method Calls.</remarks>
		[XmlIgnore]
		public bool PubSubdomain { get { return false; } }

		/// <summary>
		/// Gets the delivery delay in milliseconds for the Message.
		/// </summary>
		/// <remarks>Always no delay (zero) for Redirected Method Calls.</remarks>
		[XmlIgnore]
		public long Delay { get { return 0L; } }

		/// <summary>
		/// Gets or sets the list of Notifications for the message call.
		/// </summary>
		[XmlElement("Notification")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public List<MethodNotifyInfo> Notifications { get; set; }
	}

	/// <summary>
	/// Contains information about sending Notification Messages for Method Calls.
	/// </summary>
	public class MethodNotifyInfo : IMessageInfo
	{
		/// <summary>
		/// Gets the name of the Queue to which the notify Message will be sent.
		/// </summary>
		[XmlAttribute]
		public string Destination { get; set; }

		/// <summary>
		/// Gets or sets the Message Queue Usage type for the method.
		/// </summary>
		[XmlAttribute]
		public NotificationType NotificationType { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the Message should be sent to the Pub-Sub Domain
		/// </summary>
		[XmlAttribute]
		public bool PubSubdomain { get; set; }

		/// <summary>
		/// Gets or sets the delivery delay in milliseconds for the Message.
		/// </summary>
		[XmlAttribute]
		public long Delay { get; set; }
	}

	/// <summary>
	/// Defines when the notification message is sent in relation to a method call.
	/// </summary>
	public enum NotificationType
	{
    /// <summary>
    /// A message is sent prior to the Method invocation.
    /// </summary>
    PreInvocation,
    /// <summary>
		/// The Method invocation proceeds and a message is sent upon successful completion.
		/// </summary>
		Success,
		/// <summary>
		/// The Method invocation proceeds and a message is sent upon a failure.
		/// </summary>
		Fail
	}
}
