using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace System.Web.Mvc
{
	/// <summary>
	/// Generates simple success or failure JSON-formatted messages for ExtJS.
	/// </summary>
	public static class ExtJsonResult
	{
		#region Success
		/// <summary>
		/// Creates a new JsonResult containing a value indicating Success.
		/// </summary>
		public static JsonResult Success()
		{
			return new JsonResult()
			{
				Data = new SuccessResult()
			};
		}

		/// <summary>
		/// Creates a new JsonResult containing a value indicating Success.
		/// </summary>
		/// <param name="jsonRequestBehavior">A value indicating if HTTP GET requests are allowed from the client</param>
		public static JsonResult Success(JsonRequestBehavior jsonRequestBehavior)
		{
			return new JsonResult()
			{
				Data = new SuccessResult(),
				JsonRequestBehavior = jsonRequestBehavior
			};
		}

		/// <summary>
		/// Creates a new JsonResult containing a value indicating Success and a data object.
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		/// <param name="data">The data object to be serialized.</param>
		public static JsonResult Success<TData>(TData data)
		{
			return new JsonResult()
			{
				Data = GetSuccessResult(data)
			};
		}

		/// <summary>
		/// Creates a new JsonResult containing a value indicating Success and a data object.
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		/// <param name="data">The data object to be serialized.</param>
		/// <param name="jsonRequestBehavior">A value indicating if HTTP GET requests are allowed from the client</param>
		public static JsonResult Success<TData>(TData data, JsonRequestBehavior jsonRequestBehavior)
		{
			return new JsonResult()
			{
				Data = GetSuccessResult(data),
				JsonRequestBehavior = jsonRequestBehavior
			};
		}

		/// <summary>
		/// Gets the Result object for the data object.
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		/// <param name="data">The data object to be serialized.</param>
		/// <returns>Returns PageableSuccessResult if TData implements IPageable, or DataSuccessResult if it does not.</returns>
		private static SuccessResult GetSuccessResult<TData>(TData data)
		{
			IPageable pageable = data as IPageable;

			return (pageable == null) ? new DataSuccessResult<TData>(data) : new PageableSuccessResult<TData>(data, pageable);
		}

		#region Result Classes
		/// <summary>
		/// Represents a class used internally to represent the ExtJS Result.
		/// </summary>
		private class SuccessResult
		{
			/// <summary>
			/// Gets the boolean flag indicating success or failure.
			/// </summary>
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Value is for the JavaScriptSerializer")]
			[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Value is for the JavaScriptSerializer")]
			public bool Success { get { return true; } }
		}

		/// <summary>
		/// Represents a class used internally to represent the ExtJS Data Result
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		private class DataSuccessResult<TData> : SuccessResult
		{
			/// <summary>
			/// Creates a success JSON Data Result.
			/// </summary>
			/// <param name="data">The Data object to be JSON-serialized and send to the client ExtJS component.</param>
			public DataSuccessResult(TData data)
			{
				Data = data;
			}

			/// <summary>
			/// Gets the Data object to be JSON-serialized and send to the client ExtJS component.
			/// </summary>
			public TData Data { get; private set; }
		}

		/// <summary>
		/// Represents a class used internally to represent the ExtJS Pageable Data Result
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		private class PageableSuccessResult<TData> : DataSuccessResult<TData>
		{
			/// <summary>
			/// Creates a success JSON Data Result.
			/// </summary>
			/// <param name="data">The Data object to be JSON-serialized and send to the client ExtJS component.</param>
			/// <param name="pageable">The pageable information used for paging operations.</param>
			public PageableSuccessResult(TData data, IPageable pageable) : base(data)
			{
				Total = pageable.TotalCount;
			}

			/// <summary>
			/// Gets the total number of records available across all pages.
			/// </summary>
			public int Total { get; private set; }
		}
		#endregion
		#endregion

		#region Failure
		/// <summary>
		/// Creates a new JsonResult containing a value indicating Failure and a failure message.
		/// </summary>
		/// <param name="message">The failure message.</param>
		public static JsonResult Failure(string message)
		{
			return new JsonResult()
			{
				Data = new FailureResult(message)
			};
		}

		/// <summary>
		/// Creates a new JsonResult containing a value indicating Failure and a failure message.
		/// </summary>
		/// <param name="message">The failure message.</param>
		/// <param name="jsonRequestBehavior">A value indicating if HTTP GET requests are allowed from the client</param>
		public static JsonResult Failure(string message, JsonRequestBehavior jsonRequestBehavior)
		{
			return new JsonResult()
			{
				Data = new FailureResult(message),
				JsonRequestBehavior = jsonRequestBehavior
			};
		}

		/// <summary>
		/// Creates a new JsonResult containing a value indicating Failure, a failure message and a data object.
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		/// <param name="data">The data object to be serialized.</param>
		/// <param name="message">The failure message.</param>
		public static JsonResult Failure<TData>(TData data, string message)
		{
			return new JsonResult()
			{
				Data = new DataFailureResult<TData>(data, message)
			};
		}

		/// <summary>
		/// Creates a new JsonResult containing a value indicating Failure, a failure message and a data object.
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		/// <param name="data">The data object to be serialized.</param>
		/// <param name="message">The failure message.</param>
		/// <param name="jsonRequestBehavior">A value indicating if HTTP GET requests are allowed from the client</param>
		public static JsonResult Failure<TData>(TData data, string message, JsonRequestBehavior jsonRequestBehavior)
		{
			return new JsonResult()
			{
				Data = new DataFailureResult<TData>(data, message),
				JsonRequestBehavior = jsonRequestBehavior
			};
		}

		#region Result Classes
		/// <summary>
		/// Represents a class used internally to represent the ExtJS Result.
		/// </summary>
		private class FailureResult
		{
			/// <summary>
			/// Creates a failure Result.
			/// </summary>
			/// <param name="message">The message to pass on to the client ExtJS component.</param>
			public FailureResult(string message)
			{
				Message = message;
			}

			/// <summary>
			/// Gets the boolean flag indicating success or failure.
			/// </summary>
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Value is for the JavaScriptSerializer")]
			[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Value is for the JavaScriptSerializer")]
			public bool Success { get { return false; } }
			/// <summary>
			/// Gets the message to pass on to the client ExtJS component.
			/// </summary>
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Value is for the JavaScriptSerializer")]
			public string Message { get; private set; }
		}

		/// <summary>
		/// Represents a class used internally to represent the ExtJS Data Result
		/// </summary>
		/// <typeparam name="TData">The Type of Data object to be serialized.</typeparam>
		private class DataFailureResult<TData> : FailureResult
		{
			/// <summary>
			/// Creates a success JSON Data Result.
			/// </summary>
			/// <param name="message">The message to pass on to the client ExtJS component.</param>
			/// <param name="data">The Data object to be JSON-serialized and send to the client ExtJS component.</param>
			public DataFailureResult(TData data, string message)
				: base(message)
			{
				Data = data;
			}

			/// <summary>
			/// Gets the Data object to be JSON-serialized and send to the client ExtJS component.
			/// </summary>
			public TData Data { get; private set; }
		}
		#endregion
		#endregion
	}
}
