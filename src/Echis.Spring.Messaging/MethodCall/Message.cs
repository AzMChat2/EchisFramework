using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

namespace System.Spring.Messaging.MethodCall
{
	#region Message
	/// <summary>
	/// An XmlSerializable Message object which represents a Method Call
	/// </summary>
  [XmlInclude(typeof(ResultMessage))]
  [XmlInclude(typeof(RequestMessage))]
  [XmlInclude(typeof(ExceptionMessage))]
  public class Message
	{
		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		public Message() { }

		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		/// <param name="method">A Method Info object containing information about the method which is being invoked.</param>
		/// <param name="identity">The user Identity which is responsible for invoking the method.</param>
		/// <param name="parameters">The method invocation parameters</param>
		/// <param name="securityToken">The Security Token used to validate authentication information.</param>
		public Message(MethodInfo method, IIdentity identity, object[] parameters, string securityToken)
		{
			if (method == null) throw new ArgumentNullException("method");
      if (identity == null) throw new ArgumentNullException("identity");

			Parameters = new NameValueList((from p in method.GetParameters() select p.Name).ToArray(), parameters);

			ClassName = method.DeclaringType.FullName;
			MethodName = method.Name;

			AuthenticationContext = identity.AuthenticationType;
			UserId = identity.Name;
      SecurityToken = securityToken;
		}

		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		/// <param name="originalMessage">The original message from which property values will be copied.</param>
    public Message(Message originalMessage)
    {
      if (originalMessage == null) throw new ArgumentNullException("originalMessage");

      ClassName = originalMessage.ClassName;
      MethodName = originalMessage.MethodName;

      AuthenticationContext = originalMessage.AuthenticationContext;
      UserId = originalMessage.UserId;
      SecurityToken = originalMessage.SecurityToken;

      Parameters = originalMessage.Parameters;
    }

		/// <summary>
		/// Gets or sets the Class Name of the service object whose method is being invoked.
		/// </summary>
    [XmlAttribute]
		public string ClassName { get; set; }

		/// <summary>
		/// Gets or sets the name of the method which is being invoked.
		/// </summary>
		[XmlAttribute]
		public string MethodName { get; set; }


		/// <summary>
		/// Gets or sets the Authentication Context under which the UserId is valid.
		/// </summary>
		[XmlAttribute]
		public string AuthenticationContext { get; set; }

		/// <summary>
		/// Gets or sets the User Id.
		/// </summary>
		[XmlAttribute]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the Security Token.
		/// </summary>
    [XmlAttribute]
    public string SecurityToken { get; set; }


		/// <summary>
		/// Gets or sets the method invocation parameters.
		/// </summary>
    [XmlElement]
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
      Justification = "The property setter is required by the XmlSerializer")]
		public NameValueList Parameters { get; set; }

  }
	#endregion

	#region Request Message
	/// <summary>
	/// An XmlSerializable Message representing a Method Call Request.
	/// </summary>
  public class RequestMessage : Message
  {
		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		public RequestMessage() { }

		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		/// <param name="method">A Method Info object containing information about the method which is being invoked.</param>
		/// <param name="identity">The user Identity which is responsible for invoking the method.</param>
		/// <param name="parameters">The method invocation parameters</param>
		/// <param name="securityToken">The Security Token used to validate authentication information.</param>
		public RequestMessage(MethodInfo method, IIdentity identity, object[] parameters, string securityToken)
      : base(method, identity, parameters, securityToken)
    {
      CorrelationId = Guid.NewGuid().ToString();
    }

		/// <summary>
		/// Gets or sets the identifier used to match Results with their corresponding Request.
		/// </summary>
    [XmlAttribute]
    public string CorrelationId { get; set; }
  }
	#endregion

	#region Result Message
	/// <summary>
	/// An XmlSerializable Message representing a Method Call Result.
	/// </summary>
  public class ResultMessage : Message
  {
		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		public ResultMessage() { }

		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		/// <param name="request">The original request message from which property values will be copied.</param>
		/// <param name="returnValue">The return value from the method invocation.</param>
		public ResultMessage(RequestMessage request, object returnValue)
			: base(request)
    {
			if (request == null) throw new ArgumentNullException("request");

      CorrelationId = request.CorrelationId;
      ReturnValue = new XmlWrapper() { Value = returnValue };
    }

		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		/// <param name="method">A Method Info object containing information about the method which is being invoked.</param>
		/// <param name="identity">The user Identity which is responsible for invoking the method.</param>
		/// <param name="parameters">The method invocation parameters</param>
		/// <param name="securityToken">The Security Token used to validate authentication information.</param>
		/// <param name="returnValue">The return value from the method invocation.</param>
		public ResultMessage(MethodInfo method, IIdentity identity, object[] parameters, string securityToken, object returnValue)
      : base(method, identity, parameters, securityToken)
    {
      ReturnValue = new XmlWrapper() { Value = returnValue };
    }

		/// <summary>
		/// Gets or sets the identifier used to match Results with their corresponding Request.
		/// </summary>
		[XmlAttribute]
    public string CorrelationId { get; set; }

		/// <summary>
		/// Gets or sets the Return Value (wrapped in an Xml Wrapper) of the Method Invocation
		/// </summary>
    [XmlElement]
    public XmlWrapper ReturnValue { get; set; }

  }
	#endregion

	#region Exception Message
	/// <summary>
	/// An XmlSerializable Message representing a Method Call Exception
	/// </summary>
	public class ExceptionMessage : Message
	{
		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		public ExceptionMessage() { }

		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		/// <param name="request">The original request message from which property values will be copied.</param>
		/// <param name="exception">The exception thrown by the method invocation.</param>
		public ExceptionMessage(RequestMessage request, Exception exception)
			: base(request)
    {
			if (request == null) throw new ArgumentNullException("request");

      CorrelationId = request.CorrelationId;
			Exception = new XmlExceptionWrapper() { Value = exception };
    }

		/// <summary>
		/// Creates a new Message instance
		/// </summary>
		/// <param name="method">A Method Info object containing information about the method which is being invoked.</param>
		/// <param name="identity">The user Identity which is responsible for invoking the method.</param>
		/// <param name="parameters">The method invocation parameters</param>
		/// <param name="securityToken">The Security Token used to validate authentication information.</param>
		/// <param name="exception">The exception thrown by the method invocation.</param>
		public ExceptionMessage(MethodInfo method, IIdentity identity, object[] parameters, string securityToken, Exception exception)
      : base(method, identity, parameters, securityToken)
    {
			Exception = new XmlExceptionWrapper() { Value = exception };
    }

		/// <summary>
		/// Gets or sets the identifier used to match Results with their corresponding Request.
		/// </summary>
		[XmlAttribute]
    public string CorrelationId { get; set; }

		/// <summary>
		/// Gets or sets the Return Value (wrapped in an Xml Wrapper) of the Method Invocation
		/// </summary>
    [XmlElement]
    public XmlExceptionWrapper Exception { get; set; }
	}
	#endregion

}
