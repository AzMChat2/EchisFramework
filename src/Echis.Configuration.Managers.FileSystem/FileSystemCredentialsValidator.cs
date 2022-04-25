using System.Collections.Generic;

namespace System.Configuration.Managers.FileSystem
{
	/// <summary>
	/// Credentials Validator which uses a File System to store User, Machine, Environment and Application configuration.
	/// </summary>
	/// <typeparam name="TCredentials">The Type of Credentials this Validator can Validate.</typeparam>
	public class FileSystemCredentialsValidator<TCredentials> : CredentialsValidator<TCredentials> where TCredentials : Credentials
	{
		/// <summary>
		/// Validates the specified credentials.
		/// </summary>
		/// <param name="credentials">The credentials to be validated.</param>
		/// <returns>Returns true if the credentials are valid.</returns>
		protected override bool ValidateCredentials(TCredentials credentials)
		{
			List<string> userGroups = UserGroupList.GetUserGroups(credentials.User, credentials.UserDomain);
			bool hasUser = ApplicationEnvironments.HasUserGroup(credentials.Application, credentials.Environment.ToString(), userGroups);

			List<string> machineGroups = MachineGroupList.GetMachineGroups(credentials.Machine);
			bool hasMachine = ApplicationEnvironments.HasMachineGroup(credentials.Application, credentials.Environment.ToString(), machineGroups);

			return (hasUser && hasMachine);
		}
	}

	/// <summary>
	/// Default Credentials Validator which uses a File System to store User, Machine, Environment and Application configuration.
	/// </summary>
	public class DefaultFileSystemCredentialsValidator : FileSystemCredentialsValidator<Credentials> { }
}
