
namespace System.Configuration.Managers
{
	/// <summary>
	/// Default implmentation of the Credentials Validator
	/// </summary>
	public class DefaultCredentialsValidator : DefaultCredentialsValidator<Credentials>
	{
	}

	/// <summary>
	/// Default implmentation of the Credentials Validator
	/// </summary>
	public class DefaultCredentialsValidator<TCredentials> : CredentialsValidator<TCredentials> where TCredentials : Credentials
	{
		/// <summary>
		/// Always returns True.
		/// </summary>
		/// <param name="credentials">Not used.</param>
		/// <returns>Always returns True</returns>
		protected override bool ValidateCredentials(TCredentials credentials)
		{
			return true;
		}
	}
}
