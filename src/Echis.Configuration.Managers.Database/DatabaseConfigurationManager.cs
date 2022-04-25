using System.Globalization;
using System.Data;
using System;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Configuration Manager which retrieves configuration sections from a database.
	/// </summary>
	public class DatabaseConfigurationManager : ConfigurationManagerBase<Credentials>
	{
		#region Sql
		/// <summary>
		/// The Sql Query used to retrieve the Configuration Section from the database.
		/// </summary>
		protected const string Sql = @"
SELECT	CS.Data
FROM		{0}.ConfigSections CS
					INNER JOIN
				{0}.Applications A ON A.ApplicationId = CS.ApplicationId
					INNER JOIN
				{0}.Environments E ON E.EnvironmentId = CS.EnvironmentId
WHERE		CS.Name = @ConfigSectionName AND
				A.Name = @ApplicationName AND
				E.Name = @EnvironmentName
";
		#endregion

		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		protected override string GetConfigurationSection(string configSectionName, Credentials credentials)
		{
			if (credentials == null) throw new ArgumentNullException("credentials");

			string sql = string.Format(CultureInfo.InvariantCulture, Sql, Database.Settings.Values.DatabaseSchemaName);

			IDataCommand command = CommandFactory.CreateSqlCommand(Database.Settings.Values.ConfigurationDataAccessName, sql,
				new QueryParameter("ConfigSectionName", configSectionName),
				new QueryParameter("ApplicationName", credentials.Application),
				new QueryParameter("EnvironmentName", credentials.Environment.ToString()));

			return DataAccess.ExecuteScalar(command) as string;
		}
	}
}
