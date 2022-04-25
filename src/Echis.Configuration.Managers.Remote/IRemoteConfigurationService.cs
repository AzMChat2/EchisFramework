using System.ServiceModel;

namespace System.Configuration.Managers.Remote
{
	/// <summary>
	/// Remote Service Contract for the Remote Configuration Service.
	/// </summary>
	[ServiceContract(SessionMode = SessionMode.NotAllowed, CallbackContract = typeof(RemoteConfigurationClient))]
	public interface IRemoteConfigurationService
	{
		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		[OperationContract]
		string GetConfigurationSection(string configSectionName, string credentials);
	}
}
