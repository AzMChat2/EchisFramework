namespace System.Configuration.Managers
{
	/// <summary>
	/// An interface used to provide external Configuration Management
	/// </summary>
	public interface IConfigurationManager
	{
		/// <summary>
		/// Gets a string of Data containing the specified Configuration Section
		/// </summary>
		/// <param name="configSectionName">The name of the Configuration Section to be retrieved.</param>
		/// <param name="credentials">A string of Data containing the credentials used to retrieve the Configuration Section data.</param>
		/// <returns>Returns a string of Data containing the specified Configuration Section</returns>
		string GetConfigurationSection(string configSectionName, string credentials);
	}
}
