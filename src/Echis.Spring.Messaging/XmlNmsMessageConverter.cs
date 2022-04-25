using System;
using System.Globalization;
using System.Xml.Serialization;
using Apache.NMS;
using Spring.Messaging.Nms.Support.Converter;
using System.Diagnostics.CodeAnalysis;

namespace System.Spring.Messaging
{
	/// <summary>
	/// Provide conversion of Xml Serializable objects to and from Nms Messages
	/// </summary>
	/// <typeparam name="TMessage"></typeparam>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nms")]
	public class XmlNmsMessageConverter<TMessage> : IMessageConverter
		where TMessage : class, new()
	{
		/// <summary>
		/// Converts a text message to the specified Message type using Xml Serialization.
		/// </summary>
		/// <param name="messageToConvert">The standard message to be converted.</param>
		public object FromMessage(IMessage messageToConvert)
		{
			if (messageToConvert == null) throw new ArgumentNullException("messageToConvert");

      ITextMessage message = messageToConvert as ITextMessage;
      if (message == null) throw new ArgumentException("The message specified is not of the expected type (Text Message).");

      return XmlSerializer<TMessage>.DeserializeFromXml(message.Text);
		}

		/// <summary>
		/// Converts a Message of the specified type to a text message.
		/// </summary>
		/// <param name="objectToConvert">The Method Call Message to be converted.</param>
		/// <param name="session">The messaging session.</param>
		public IMessage ToMessage(object objectToConvert, ISession session)
		{
			if (session == null) throw new ArgumentNullException("session");
			if (objectToConvert == null) throw new ArgumentNullException("objectToConvert");

      TMessage message = objectToConvert as TMessage;
      if (message == null) throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The specified object to convert is not of the expected type ({0})", typeof(TMessage).FullName));

			return session.CreateTextMessage(XmlSerializer<TMessage>.SerializeToXml(message, CultureInfo.InvariantCulture));
		}
	}
}
