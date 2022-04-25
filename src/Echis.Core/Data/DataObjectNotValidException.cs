using System.Runtime.Serialization;

namespace System.Data
{
	/// <summary>
	/// Exception to be thrown when an IValidatable object is not valid.
	/// </summary>
	[Serializable]
	public class DataObjectNotValidException : ExceptionBase
	{
		/// <summary>
		/// Gets the format for the Exception Message.
		/// </summary>
		private const string _exceptionMessage = "The object '{0}' has not passed validation.";

		/// <summary>
		/// Gets all rule messages of the specified object as a single string.
		/// </summary>
		private static string GetMessages(IValidatable validatable)
		{
			return string.Join(Environment.NewLine, validatable.GetAllRuleMessages());
		}

		/// <summary>
		/// Gets the DomainId of the specified object.
		/// </summary>
		private static string GetDomain(IValidatable dataObject)
		{
			return (dataObject == null) ? "Unknown" : dataObject.GetType().Name;
		}

		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DataObjectNotValidException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			ValidationMessage = (string)info.GetValue("ValidationMessage", typeof(string));
		}


		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		public DataObjectNotValidException() { }
		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="message">The exception message.</param>
		public DataObjectNotValidException(string message) : base(message) { }
		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="inner">The exception which was thrown immediately prior to this exception.</param>
		public DataObjectNotValidException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="dataObject">The validatable object which is not valid.</param>
		public DataObjectNotValidException(IValidatable dataObject) : this(GetDomain(dataObject), GetMessages(dataObject)) { }

		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="name">The domain name of the invalid object.</param>
		/// <param name="validationMessage">The rule messages for the invalid object.</param>
		public DataObjectNotValidException(string name, string validationMessage)
			: base(_exceptionMessage, name)
		{
			ValidationMessage = validationMessage;
		}

		/// <summary>
		/// Creates a new instance of the Exception
		/// </summary>
		/// <param name="name">The domain name of the invalid object.</param>
		/// <param name="validationMessage">The rule messages for the invalid object.</param>
		/// <param name="inner">The exception which was thrown immediately prior to this exception.</param>
		public DataObjectNotValidException(string name, string validationMessage, Exception inner)
			: base(inner, _exceptionMessage, name)
		{
			ValidationMessage = validationMessage;
		}

		/// <summary>
		/// Gets the Rule Messages for the invalid object.
		/// </summary>
		public string ValidationMessage { get; private set; }

		/// <summary>
		/// Serialization method.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null) throw new ArgumentNullException("info");

			info.AddValue("ValidationMessage", ValidationMessage);
			base.GetObjectData(info, context);
		}
	}
}
