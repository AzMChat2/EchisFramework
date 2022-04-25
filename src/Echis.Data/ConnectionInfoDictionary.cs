using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Security;

namespace System.Data
{
	/// <summary>
	/// Used to store Connection String information for the Data Access Clients
	/// </summary>
	internal static class ConnectionInfoDictionary
	{
		#region Connection Strings
		/// <summary>
		/// Stores the connection string information for the Data Access Objects and Clients
		/// </summary>
		/// <remarks>
		/// This is done so that connection string information is not visible even
		///  to the debugger or via reflection of the Data Access Object or Client.
		/// </remarks>
		private static Dictionary<string, ConnectionStringInfo> connectionStrings = new Dictionary<string, ConnectionStringInfo>();

		/// <summary>
		/// Sets the connection string information for the specified Data Access name.
		/// </summary>
		/// <param name="dataAccessName">The data access name which uses the specified connection string.</param>
		/// <param name="connectionString">The connection string used to connect to the database the data access object represents.</param>
		[DebuggerHidden]
		public static void SetConnectionString(string dataAccessName, string connectionString)
		{
			if (connectionStrings.ContainsKey(dataAccessName))
			{
				connectionStrings[dataAccessName] = new ConnectionStringInfo(connectionString);
			}
			else
			{
				connectionStrings.Add(dataAccessName, new ConnectionStringInfo(connectionString));
			}
		}

		/// <summary>
		/// Determines if the specified Data Access name has a corresponding connection string.
		/// </summary>
		/// <param name="dataAccessName">The data access name to which the connection string belongs.</param>
		/// <returns>Returns true if the Data Access name has a corresponding connection string.</returns>
		[DebuggerHidden]
		public static bool ConnectionStringExists(string dataAccessName)
		{
			return connectionStrings.ContainsKey(dataAccessName);
		}

		/// <summary>
		/// Gets the connection string information for the specified Data Access name.
		/// </summary>
		/// <param name="dataAccessName">The data access name which uses the specified connection string.</param>
		/// <returns>Returns the connection string information for the specified Data Access Object.</returns>
		[DebuggerHidden]
		public static string GetConnectionString(string dataAccessName)
		{
			ConnectionStringInfo info = connectionStrings[dataAccessName];
			if (info.IsEncrypted) info.Decrypt();
			return info.ConnectionString;
		}
		#endregion

		#region Data Access Credentials
		/// <summary>
		/// Stores the Data Access Credentials for the Data Access Objects and Clients
		/// </summary>
		/// <remarks>
		/// This is done so that Credential information is not visible even
		///  to the debugger or via reflection of the Data Access Object or Client.
		/// </remarks>
		private static Dictionary<string, DataAccessCredentials> dataAccessCredentials = new Dictionary<string, DataAccessCredentials>();

		/// <summary>
		/// Sets the Data Access Credentials for the specified Data Access name.
		/// </summary>
		/// <param name="dataAccessName">The data access name which uses the specified connection string.</param>
		/// <param name="credentials">The Data Access Credentials used to connect to the database the data access object represents.</param>
		[DebuggerHidden]
		public static void SetCredentials(string dataAccessName, DataAccessCredentials credentials)
		{
			if (dataAccessCredentials.ContainsKey(dataAccessName))
			{
				dataAccessCredentials[dataAccessName] = credentials;
			}
			else
			{
				dataAccessCredentials.Add(dataAccessName, credentials);
			}
		}

		/// <summary>
		/// Determines if the specified Data Access name has a corresponding Data Access Credentials.
		/// </summary>
		/// <param name="dataAccessName">The data access name to which the Data Access Credentials belongs.</param>
		/// <returns>Returns true if the Data Access name has a corresponding Data Access Credentials.</returns>
		[DebuggerHidden]
		public static bool CredentialsExists(string dataAccessName)
		{
			return dataAccessCredentials.ContainsKey(dataAccessName);
		}

		/// <summary>
		/// Gets the Data Access Credentials for the specified Data Access name.
		/// </summary>
		/// <param name="dataAccessName">The data access name which uses the specified Data Access Credentials.</param>
		/// <returns>Returns the Data Access Credentials for the specified Data Access Object.</returns>
		[DebuggerHidden]
		public static DataAccessCredentials GetCredentials(string dataAccessName)
		{
			DataAccessCredentials retVal = dataAccessCredentials[dataAccessName];
			if (retVal.IsEncrypted) retVal.Decrypt();
			return retVal;
		}
		#endregion
	}
}
