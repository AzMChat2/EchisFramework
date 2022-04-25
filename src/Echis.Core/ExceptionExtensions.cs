using System.Globalization;
using System.Text;

namespace System
{
	/// <summary>
	/// Utility methods for Exceptions
	/// </summary>
	public static class ExceptionExtensions
	{
		private const string MsgFormat = "{2}{0}: {1}";

		/// <summary>
		/// Gets text representing the exception, exception message and all inner exception messages.
		/// </summary>
		/// <param name="ex">The exception from which the message will be generated.</param>
		/// <returns>Returns text representing the exception, exception message and all inner exception messages.</returns>
		public static string GetExceptionMessage(this Exception ex)
		{
			return ex.GetExceptionMessage(Environment.NewLine);
		}

		/// <summary>
		/// Gets text representing the exception, exception message and all inner exception messages.
		/// </summary>
		/// <param name="ex">The exception from which the message will be generated.</param>
		/// <param name="delimiter">A delimeter used to separate exception messages.</param>
		/// <returns>Returns text representing the exception, exception message and all inner exception messages.</returns>
		public static string GetExceptionMessage(this Exception ex, string delimiter)
		{
			StringBuilder retVal = new StringBuilder();

			if (ex != null)
			{
				AppendExceptionMessage(ex, delimiter, retVal);
			}

			MultipleErrorException mex = ex as MultipleErrorException;

			if (mex != null)
			{
				mex.Exceptions.ForEach(exception => AppendExceptionMessage(exception, delimiter, retVal));
			}

			return retVal.ToString();
		}

		private static void AppendExceptionMessage(Exception ex, string delimiter, StringBuilder msgBuilder)
		{
			msgBuilder.AppendFormat(CultureInfo.InvariantCulture, MsgFormat, ex.GetType().Name, ex.Message.Trim(), string.Empty);

			while ((ex = ex.InnerException) != null)
			{
				msgBuilder.AppendFormat(CultureInfo.InvariantCulture, "{2}{0}: {1}", ex.GetType().Name, ex.Message.Trim(), delimiter);
			}
		}
	}
}
