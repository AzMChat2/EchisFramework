using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Reflection;

namespace System.Security
{
	/// <summary>
	/// A default implementation of the Security Provider which uses a Token Provider to validate authentication information.
	/// </summary>
	public class SecurityProvider : ISecurityProvider
	{
		/// <summary>
		/// Gets or sets the Token Provider used to generate security tokens.
		/// </summary>
		public ITokenProvider TokenProvider { get; set; }

		/// <summary>
		/// Validates that the supplied credentials have not been tampered with via the Security Token.
		/// </summary>
		/// <param name="authenticationContext">The authentication context in which the userId is valid.</param>
		/// <param name="userId">The userId for the user sending the message.</param>
		/// <param name="securityToken">A security token used to validate that the authentication information has not been tampered with.</param>
		/// <returns>Returns a Windows Principal object containing the credentials</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "The disposable object is being returned")]
		public virtual IPrincipal AuthenticateUser(string authenticationContext, string userId, string securityToken)
		{
			if (securityToken != CreateSecurityToken(authenticationContext, userId)) throw new SecurityException("Security Token mismatch.");

			return new GenericPrincipal(new GenericIdentity(userId, authenticationContext), null);
		}

		/// <summary>
		/// Creates a security token for the specified authentication context and userId.
		/// </summary>
		/// <param name="authenticationContext">The authentication context in which the userId is valid.</param>
		/// <param name="userId">The userId for the user sending the message.</param>
		public virtual string CreateSecurityToken(string authenticationContext, string userId)
		{
			return TokenProvider.GetToken(authenticationContext, userId);
		}

		/// <summary>
		/// Checks security authorization for the specified Type and Method.
		/// </summary>
		/// <param name="method">The Method Information for the secured method.</param>
		public virtual void CheckSecurity(MethodInfo method)
		{
		}
	}
}
