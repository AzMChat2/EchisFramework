using System;
using System.Xml.Serialization;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Validates Credentials
	/// </summary>
	/// <typeparam name="TCredentials">The Type of Credentials the derived class can validate.</typeparam>
	public abstract class CredentialsValidator<TCredentials> : ICredentialsValidator<TCredentials> where TCredentials : Credentials
	{
		/// <summary>
		/// Validates the specified credentials.
		/// </summary>
		/// <param name="credentials">The credentials to be validated.</param>
		/// <returns>Returns true if the credentials are valid.</returns>
		public TCredentials ValidateCredentials(string credentials)
		{
			TCredentials retVal = XmlSerializer<TCredentials>.DeserializeFromXml(credentials);

			if (ValidateCredentials(retVal))
			{
				return retVal;
			}
			else
			{
				throw new CredentialsValidationException();
			}
		}

		/// <summary>
		/// Validates the specified credentials.
		/// </summary>
		/// <param name="credentials">The credentials to be validated.</param>
		/// <returns>Returns true if the credentials are valid.</returns>
		protected abstract bool ValidateCredentials(TCredentials credentials);
	}
}
