using System.Globalization;
using System.Data;

namespace System.Configuration.Managers.Database
{
	/// <summary>
	/// Credentials Validator which uses a database to store User, Machine, Environment and Application configuration.
	/// </summary>
	/// <typeparam name="TCredentials">The Type of Credentials this Validator can Validate.</typeparam>
	public class DatabaseCredentialsValidator<TCredentials> : CredentialsValidator<TCredentials> where TCredentials : Credentials
	{
		#region Sql
		/// <summary>
		/// The Sql Query used to validate the credentials
		/// </summary>
		private const string Sql = @"
SELECT  COUNT(*)
FROM    {0}.Users U
          INNER JOIN
        {0}.UserGroups UG ON U.UserId = UG.UserId
          INNER JOIN 
        {0}.Applications A ON UG.ApplicationId = A.ApplicationId
          INNER JOIN
        {0}.Environments E ON UG.EnvironmentId = E.EnvironmentId
          INNER JOIN
        {0}.MachineGroups MG ON UG.ApplicationId = MG.ApplicationId AND
                                UG.EnvironmentId = MG.EnvironmentId
          INNER JOIN
        {0}.Machines M ON MG.MachineId = M.MachineId
WHERE   ((U.Name = @UserName) OR (U.Name = '*')) AND
        ((U.Domain = @UserDomain) OR (U.Domain = '*')) AND
        ((M.Name = @MachineName) OR (M.Name = '*')) AND
        A.Name = @ApplicationName AND
        E.Name = @EnvironmentName
";
		#endregion

		/// <summary>
		/// Validates the specified credentials.
		/// </summary>
		/// <param name="credentials">The credentials to be validated.</param>
		/// <returns>Returns true if the credentials are valid.</returns>
		protected override bool ValidateCredentials(TCredentials credentials)
		{
			string sql = string.Format(CultureInfo.InvariantCulture, Sql, Settings.Values.DatabaseSchemaName);

			IDataCommand command = CommandFactory.CreateSqlCommand(Settings.Values.ConfigurationDataAccessName, sql,
				new QueryParameter("UserName", credentials.User),
				new QueryParameter("UserDomain", credentials.UserDomain),
				new QueryParameter("MachineName", credentials.Machine),
				new QueryParameter("ApplicationName", credentials.Application),
				new QueryParameter("EnvironmentName", credentials.Environment.ToString()));

			int retVal = (int)DataAccess.ExecuteScalar(command);

			return (retVal != 0);
		}
	}

	/// <summary>
	/// Default Credentials Validator which uses a database to store User, Machine, Environment and Application configuration.
	/// </summary>
	public class DefaultDatabaseCredentialsValidator : DatabaseCredentialsValidator<Credentials> { }
}
