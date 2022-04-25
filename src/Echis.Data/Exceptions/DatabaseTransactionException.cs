using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Security;

namespace System.Data
{
	/// <summary>
	/// Exception thrown when an exception occurs during the creation of a Transaction collection.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", 
		Justification="Additional exception constructors are not desired for this exception.")]
	public class DatabaseTransactionException : DataException
	{
		/// <summary>
		/// The actions which can be performed on a transaction
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "Enumeration is only used by DatabaseTransactionException")]
		public enum TransactionActions
		{
			/// <summary>
			/// Begin Transaction
			/// </summary>
			Begin,
			/// <summary>
			/// Rollback Transaction
			/// </summary>
			Rollback,
			/// <summary>
			/// Commit Transaction
			/// </summary>
			Commit
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="exceptions">The exception details which prevented the action from being performed.</param>
		/// <param name="action">The action which was being performed.</param>
		public DatabaseTransactionException(TransactionActions action, ExceptionDetail<IDataCommand>[] exceptions)
			: base(GetMessage(action))
		{
			Exceptions = exceptions;
		}

		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DatabaseTransactionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Gets the exceptions which were thrown while attempting to perform the action.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
			Justification = "An array is appropriate here.")]
		public ExceptionDetail<IDataCommand>[] Exceptions { get; private set; }

		/// <summary>
		/// Gets the appropriate transaction error message depending upon the action specified.
		/// </summary>
		/// <param name="action">The transaction action which caused the exception.</param>
		/// <returns>Returns the appropriate transaction error message depending upon the action specified.</returns>
		protected static string GetMessage(TransactionActions action)
		{
			switch (action)
			{
				case TransactionActions.Rollback:
					return "Unable to Rollback Transaction for one or more commands; see exception details.";
				case TransactionActions.Commit:
					return "Unable to Commit Transaction for one or more commands; see exception details.";
				default:
					return "Unable to Begin Transaction for one or more commands; see exception details.";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
