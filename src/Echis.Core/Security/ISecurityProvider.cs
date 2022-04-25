using System.Reflection;
using System.Security.Principal;

namespace System.Security
{
	/// <summary>
	/// Defines service methods for Message Credential Authentication
	/// </summary>
  public interface ISecurityProvider
  {
		/// <summary>
		/// Validates that the supplied credentials have not been tampered with via the Security Token.
		/// </summary>
		/// <param name="authenticationContext">The authentication context in which the userId is valid.</param>
		/// <param name="userId">The userId for the user sending the message.</param>
		/// <param name="securityToken">A security token used to validate that the authentication information has not been tampered with.</param>
		/// <returns>Returns a Windows Principal object containing the credentials</returns>
		IPrincipal AuthenticateUser(string authenticationContext, string userId, string securityToken);

		/// <summary>
		/// Creates a security token for the specified authentication context and userId.
		/// </summary>
		/// <param name="authenticationContext">The authentication context in which the userId is valid.</param>
		/// <param name="userId">The userId for the user sending the message.</param>
		string CreateSecurityToken(string authenticationContext, string userId);

		/// <summary>
		/// Checks security authorization for the specified Type and Method.
		/// </summary>
		/// <param name="method">The Method Information for the secured method.</param>
		void CheckSecurity(MethodInfo method);
  }
}
