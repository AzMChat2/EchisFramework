using System;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;

namespace System.Data
{
	/// <summary>
	/// Provides connection information
	/// </summary>
	internal sealed class DataConnection : IDisposable
	{
		/// <summary>
		/// Gets an open IdbConnection object for the specific Database which this client represents.
		/// </summary>
		/// <returns>Returns an open IDbConnection</returns>
		[DebuggerHidden]
		public static IDbConnection OpenConnection(IDataClient client)
		{
			IDbConnection retVal = client.CreateConnection();
			retVal.ConnectionString = ConnectionInfoDictionary.GetConnectionString(client.Name);

			if (ConnectionInfoDictionary.CredentialsExists(client.Name))
			{
				using (WindowsIdentityImpersonator impersonator =
					WindowsIdentityImpersonator.BeginImpersonation(ConnectionInfoDictionary.GetCredentials(client.Name)))
				{
					retVal.Open();
				}
			}
			else
			{
				retVal.Open();
			}

			return retVal;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="command">The command which will use the connection and transaction this class contains.</param>
		/// <param name="client">The IDataClient which supplies this object with data access objects.</param>
		[DebuggerHidden]
		public DataConnection(IDataCommand command, IDataClient client)
		{
			if (command.Transaction != null)
			{
				Transaction = command.Transaction;
				Connection = Transaction.Connection;
			}
			else
			{
				Connection = OpenConnection(client);
				CreatedConnection = true;
			}

			if (!string.IsNullOrEmpty(command.DatabaseName))
			{
				Connection.ChangeDatabase(command.DatabaseName);
			}
		}

		/// <summary>
		/// Gets the Database Connection.
		/// </summary>
		[DebuggerHidden]
		public IDbConnection Connection { get; private set; }
		/// <summary>
		/// Gets the Transaction.
		/// </summary>
		[DebuggerHidden]
		public IDbTransaction Transaction { get; private set; }
		/// <summary>
		/// Gets a flag indicating if a connection was created when this class was instantiated.
		/// </summary>
		[DebuggerHidden]
		public bool CreatedConnection { get; private set; }

		/// <summary>
		/// Closes any opened connections (previously opened connections remain open).
		/// </summary>
		[DebuggerHidden]
		public void Dispose()
		{
			if (CreatedConnection && (Connection != null))
			{
				if (Connection.State == ConnectionState.Open)
				{
					Connection.Close();
				}

				Connection.Dispose();
			}

			Connection = null;
			Transaction = null;

			GC.SuppressFinalize(this);
		}
	}
}
