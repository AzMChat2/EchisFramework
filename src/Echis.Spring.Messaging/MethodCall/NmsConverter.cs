using System.Diagnostics.CodeAnalysis;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Provides conversion methods for converting a Method Call Message to and from a standard Message
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nms")]
	public class NmsConverter : XmlNmsMessageConverter<Message>
	{
	}
}
