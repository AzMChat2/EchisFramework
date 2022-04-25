using System;
using System.Collections.Generic;

namespace System.Diagnostics.TraceListeners
{
	/// <summary>
	/// Summary description for FilteredTraceListener.
	/// </summary>
	[DebuggerStepThrough]
	public abstract class FilteredTraceListener : ThreadTraceListener
	{
		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected abstract void WriteMessage(string message, string category);
		/// <summary>
		/// Writes a trace message to the log.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The message's category</param>
		protected abstract void WriteMessageLine(string message, string category);

		#region Constructors
		/// <summary>
		/// Default Constructor.
		/// </summary>
		protected FilteredTraceListener()
			: base()
		{
			IncludeFilters = new List<string>();
			ExcludeFilters = new List<string>();
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the Trace Listener</param>
		protected FilteredTraceListener(string name)
			: base(name)
		{
			IncludeFilters = new List<string>();
			ExcludeFilters = new List<string>();
		}
		#endregion
		/// <summary>
		/// Stores the array list containing all include filters
		/// </summary>
		protected List<string> IncludeFilters { get; private set; }

		/// <summary>
		/// Stores the array list containing all exclude filters
		/// </summary>
		protected List<string> ExcludeFilters { get; private set; }

		/// <summary>
		/// Sets custom parameters.
		/// </summary>
		/// <param name="paramName">The parameter name to set.</param>
		/// <param name="paramValue">The value for the parameter.</param>
		protected override void SetParameter(string paramName, string paramValue)
		{
			if (paramName == null) throw new ArgumentNullException("paramName");
			if (paramValue == null) throw new ArgumentNullException("paramValue");

			if (paramName.Equals("includefilter", StringComparison.OrdinalIgnoreCase))
			{
				IncludeFilters.Clear();
				IncludeFilters.AddRange(paramValue.Split(';'));
			}
			else if (paramName.Equals("excludefilter", StringComparison.OrdinalIgnoreCase))
			{
				ExcludeFilters.Clear();
				ExcludeFilters.AddRange(paramValue.Split(';'));
			}
		}

		/// <summary>
		/// Determines if the Category is in the filters list.
		/// </summary>
		/// <param name="category">The category to be checked against the list.</param>
		/// <returns>Returns true if the category is in the list, otherwise returns false.</returns>
		protected virtual bool InFilterList(string category)
		{
			bool retVal = false;

			if (IsCorrectThread)
			{
				if (category == null)
				{
					retVal = ((IncludeFilters.Count == 0) || IncludeFilters.Contains("*"));
				}
				else
				{
					if ((IncludeFilters.Count == 0) || (IncludeFilters.Contains("*")))
					{
						retVal = !ExcludeFilters.Exists(test => category.Equals(test, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						retVal = IncludeFilters.Exists(test => category.Equals(test, StringComparison.OrdinalIgnoreCase));
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Writes an object to the TraceListener.
		/// </summary>
		/// <param name="o">The object to be written.</param>
		/// <param name="category">The category of the trace message</param>
		public override void Write(object o, string category)
		{
			if (o == null) throw new ArgumentNullException("o");

			if (InFilterList(category))
			{
				WriteMessage(o.ToString(), category);
			}
		}

		/// <summary>
		/// Writes an message to the TraceListener.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The category of the trace message</param>
		public override void Write(string message, string category)
		{
			if (InFilterList(category))
			{
				WriteMessage(message, category);
			}
		}

		/// <summary>
		/// Writes an message to the TraceListener.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		public override void Write(string message)
		{
			string messagePart;
			string category;

			GetParts(message, out messagePart, out category);

			if (InFilterList(category))
			{
				WriteMessage(messagePart, category);
			}
		}

		/// <summary>
		/// Writes an object and appends a new line to the TraceListener.
		/// </summary>
		/// <param name="o">The object to be written.</param>
		/// <param name="category">The category of the trace message</param>
		public override void WriteLine(object o, string category)
		{
			if (o == null) throw new ArgumentNullException("o");

			if (InFilterList(category))
			{
				WriteMessageLine(o.ToString(), category);
			}
		}

		/// <summary>
		/// Writes an message and appends a new line to the TraceListener.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		public override void WriteLine(string message)
		{
			string messagePart;
			string category;

			GetParts(message, out messagePart, out category);

			if (InFilterList(category))
			{
				WriteMessageLine(messagePart, category);
			}
		}

		/// <summary>
		/// Writes an message and appends a new line to the TraceListener.
		/// </summary>
		/// <param name="message">The message to be written.</param>
		/// <param name="category">The category of the trace message</param>
		public override void WriteLine(string message, string category)
		{
			if (InFilterList(category))
			{
				WriteMessageLine(message, category);
			}
		}

		/// <summary>
		/// Separates a message into category and message.
		/// </summary>
		/// <param name="input">The input string containg both category and message.</param>
		/// <param name="message">The message part of the input string.</param>
		/// <param name="category">The category part of the input string.</param>
		private static void GetParts(string input, out string message, out string category)
		{
			if (input == null)
			{
				message = null;
				category = null;
			}
			else
			{
				string[] parts = input.Split(':');
				if (parts.Length >= 2)
				{
					category = parts[0];
					message = string.Join(":", parts, 1, parts.Length - 1);
				}
				else
				{
					message = input;
					category = null;
				}
			}
		}
	}
}
