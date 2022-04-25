using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// Summary description for EMailTraceListener.
	/// </summary>
	public class EmailTraceListener : FilteredTraceListener
	{
		/// <summary>
		/// Stores the SMTP Server name or IP Address
		/// </summary>
		protected string SmtpServer { get; set; }
		/// <summary>
		/// Stores the SMTP Sever User Name to be used to send the message.
		/// </summary>
		protected string SmtpUser { get; set; }
		/// <summary>
		/// Stores the SMTP Sever Password to be used to send the message.
		/// </summary>
		protected string SmtpPassword { get; set; }
		/// <summary>
		/// Stores the E-Mail address to be used as the "Sender"
		/// </summary>
		protected string SenderEmail { get; set; }
		/// <summary>
		/// Stores the E-Mail address(es) the message will be sent to
		/// </summary>
		protected string ReceiverEmail { get; set; }
		/// <summary>
		/// Stores the E-Mail subject line.
		/// </summary>
		protected string MessageSubject { get; set; }

		/// <summary>
		/// Stores the string builder object used to generate the e-mail message body
		/// </summary>
		protected StringBuilder MessageText { get; set; }

		/// <summary>
		/// Stores the time the trace listener started writing the message body.
		/// </summary>
		protected DateTime BeginTime { get; set; }
		/// <summary>
		/// Stores the time the trace listener finished writing the message body.
		/// </summary>
		protected DateTime EndTime { get; set; }

		/// <summary>
		/// Stores the first trace message of the message body.
		/// </summary>
		protected string BeginMessage { get; set; }
		/// <summary>
		/// Stores the last trace message of the message body.
		/// </summary>
		protected string EndMessage { get; set; }

		/// <summary>
		/// Constructor. Creates an EMailTraceListener
		/// </summary>
		public EmailTraceListener()
		{
			BeginTime = DateTime.MinValue;
			EndTime = DateTime.MaxValue;
		}

		/// <summary>
		/// Sets custom parameters.
		/// </summary>
		/// <param name="paramName">The parameter name to set.</param>
		/// <param name="paramValue">The value for the parameter.</param>
		protected override void SetParameter(string paramName, string paramValue)
		{
			if (paramName == null) throw new ArgumentNullException("paramName");

			base.SetParameter(paramName, paramValue);

			if (paramName.Equals("smtpserver", StringComparison.OrdinalIgnoreCase))
			{
				SmtpServer = paramValue;
			}
			else if (paramName.Equals("smtpuser", StringComparison.OrdinalIgnoreCase))
			{
				SmtpUser = paramValue;
			}
			else if (paramName.Equals("smtppassword", StringComparison.OrdinalIgnoreCase))
			{
				SmtpPassword = paramValue;
			}
			else if (paramName.Equals("from", StringComparison.OrdinalIgnoreCase))
			{
				SenderEmail = paramValue;
			}
			else if (paramName.Equals("to", StringComparison.OrdinalIgnoreCase))
			{
				ReceiverEmail = paramValue;
			}
			else if (paramName.Equals("subject", StringComparison.OrdinalIgnoreCase))
			{
				MessageSubject = paramValue;
			}
		}

		/// <summary>
		/// Determines if the message should be written to the Message Body String Builder
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The category of the message.</param>
		/// <returns>Returns true if the message should be written to the Message Body String Builder</returns>
		public virtual bool RecordMessage(string message, string category)
		{
			bool retVal = false;

			if (MessageText == null)
			{
				if (category == TS.Commands.End)
				{
					EndTime = DateTime.Now;
					EndMessage = message;
					SendMessage();
					MessageText = null;
				}
				else
				{
					retVal = InFilterList(category);
				}
			}
			else
			{
				if (category == TS.Commands.Begin)
				{
					MessageText = new StringBuilder();
					BeginTime = DateTime.Now;
					BeginMessage = message;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Sends the e-mail message.
		/// </summary>
		public virtual void SendMessage()
		{
			if (MessageText.Length != 0)
			{
				MessageText.Insert(0, string.Format(CultureInfo.InvariantCulture, BeginMessage, BeginTime));
				MessageText.AppendFormat(EndMessage, EndTime);

				using (SmtpClient smtp = new SmtpClient(SmtpServer))
				{
					if (!string.IsNullOrEmpty(SmtpUser))
					{
						smtp.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
					}

					smtp.Send(SenderEmail, ReceiverEmail, MessageSubject, MessageText.ToString());
				}
			}
		}

		/// <summary>
		/// Stores the last message category used for the Write method.
		/// </summary>
		private string LastMessageCategory;

		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected override void WriteMessage(string message, string category)
		{
			if (RecordMessage(message, category))
			{
				string messageFormat = null;

				if (LastMessageCategory == category)
				{
					messageFormat = "{1} ";
				}
				else if (LastMessageCategory == null)
				{
					messageFormat = "               {0}: {1} ";
					LastMessageCategory = category;
				}
				else
				{
					messageFormat = "\r\n               {0}: {1} ";
					LastMessageCategory = category;
				}

				MessageText.AppendFormat(messageFormat, category, message);
			}
		}

		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected override void WriteMessageLine(string message, string category)
		{
			if (RecordMessage(message, category))
			{
				string messageFormat = null;

				if (LastMessageCategory == null)
				{
					messageFormat = "{0:hh:mm:ss.fff} - {1}: {2}\r\n";
				}
				else
				{
					LastMessageCategory = null;
					messageFormat = "\r\n{0:hh:mm:ss.fff} - {1}: {2}\r\n";
				}

				MessageText.AppendFormat(messageFormat, DateTime.Now, category, message);
			}
		}
	}
}
