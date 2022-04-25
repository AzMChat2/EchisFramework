using System;
using System.Configuration;
using System.Globalization;

namespace System.Configuration.Managers
{
	/// <summary>
	/// The Configuration Manager Base class contains core security functionality for Configuration Manager classes.
	/// </summary>
	/// <remarks>This class recieves a requests, validates the credentials and if valid, then passes the request on to the derived class.
	/// </remarks>
	/// <typeparam name="TCredentials">The Type of the Credentials.</typeparam>
	public abstract class ConfigurationManagerBase<TCredentials> : IConfigurationManager where TCredentials : Credentials
	{
		/// <summary>
		/// Constants used by the ConfigurationManagerBase class.
		/// </summary>
		private static class Constants
		{
			/// <summary>
			/// The default IOC ObjectId for the Credentials Validator.
			/// </summary>
			public const string CredentialsValidatorObjectId = "System.Configuration.CredentialsValidator";
		}

		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		public string GetConfigurationSection(string configSectionName, string credentials)
		{
			TCredentials credentialsObject = CredentialsValidator.ValidateCredentials(credentials);
			return GetConfigurationSection(configSectionName, credentialsObject);
		}

		/// <summary>
		/// In derived classes, gets the configuration section for the specified section namae and validated Credentials.
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A validated CredentialsType object containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns></returns>
		protected abstract string GetConfigurationSection(string configSectionName, TCredentials credentials);

		/// <summary>
		/// Stores the CredentialsValidator instance used to validate credentials.
		/// </summary>
		private ICredentialsValidator<TCredentials> _credentialsValidator;
		/// <summary>
		/// Gets the CredentialsValidator instance used to validate credentials.
		/// </summary>
		protected ICredentialsValidator<TCredentials> CredentialsValidator
		{
			get
			{
				if (_credentialsValidator == null)
				{
					_credentialsValidator = IOC.GetFrameworkObject<ICredentialsValidator<TCredentials>>(Settings.Values.CredentialsValidator, Constants.CredentialsValidatorObjectId, false);

					if (_credentialsValidator == null)
					{
						// Credentials Validator not defined, use default.
						_credentialsValidator = new DefaultCredentialsValidator<TCredentials>();
					}
				}
				return _credentialsValidator;
			}
		}
	}
}
