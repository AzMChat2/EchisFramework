
namespace System.Configuration.Managers
{
	/// <summary>
	/// Validates Credentials.
	/// </summary>
	/// <typeparam name="TCredentials">The Type of credentials the Validator can Validate.</typeparam>
	public interface ICredentialsValidator<TCredentials> where TCredentials : Credentials
	{
		/// <summary>
		/// Validates the specified credentials.
		/// </summary>
		/// <param name="credentials">The credentials to be validated.</param>
		/// <returns>Returns true if the credentials are valid.</returns>
		TCredentials ValidateCredentials(string credentials);
	}
}
