using System;
using System.Diagnostics;
using System.Web;
using System.Xml.Serialization;
using System.Globalization;

namespace System.Configuration.Managers
{
	/// <summary>
	/// Default implementation of the ICredentialsProvider interface.
	/// If no custom implementation is provided, this class will generate basic credentials information using
	/// Process name, Machine name, User domain, User name and if available Web User name.
	/// </summary>
	public class DefaultCredentialsProvider : ICredentialsProvider
	{
		/// <summary>
		/// Gets a string representing credentials used to retrieve configuration sections.
		/// </summary>
		/// <returns>Returns a string representing credentials used to retrieve configuration sections.</returns>
		public virtual string GetCredentials()
		{
			Credentials credentials = new Credentials();

			string application = Process.GetCurrentProcess().ProcessName;

			if (application.EndsWith(".vshost", StringComparison.OrdinalIgnoreCase))
			{
				application = application.Substring(0, application.Length - 7);
			}

			credentials.Application = application;
			credentials.Machine = Environment.MachineName;
			credentials.UserDomain = Environment.UserDomainName;
			credentials.User = Environment.UserName;
			credentials.Environment = Settings.Values.Environment;

			if (HttpContext.Current != null)
			{
				credentials.WebUser = HttpContext.Current.User.Identity.Name;
			}

			return XmlSerializer<Credentials>.SerializeToXml(credentials, CultureInfo.CurrentCulture);

		}
	}
}
