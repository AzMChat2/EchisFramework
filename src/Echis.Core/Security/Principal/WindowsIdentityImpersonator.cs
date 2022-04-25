using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	/// <summary>
	/// Provides Windows Identity Impersionation
	/// </summary>
	public sealed class WindowsIdentityImpersonator : IDisposable
	{
		[SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
			Justification = "Method is only used within this class")]
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		[SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
			Justification = "Method is only used within this class")]
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

		[SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
			Justification = "Method is only used within this class")]
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool RevertToSelf();

		[SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
			Justification = "Method is only used within this class")]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr handle);

		private const int SecurityImpersonation = 2;

		/// <summary>
		/// Initializes a new WindowsIdentityImpersonator object and begins impersionation.
		/// </summary>
		/// <param name="userName">The user to impersonate</param>
		/// <param name="domain">The domain of the user to impersonate.</param>
		/// <param name="password">The password of the user to impersonate.</param>
		/// <returns>Returns a new WindowsIdentityImpersonator object.</returns>
		public static WindowsIdentityImpersonator BeginImpersonation(string userName, string domain, string password)
		{
			return BeginImpersonation(userName, domain, password, LogOnTypes.Interactive, LogOnProviders.Default);
		}

				/// <summary>
		/// Initializes a new WindowsIdentityImpersonator object and begins impersionation.
		/// </summary>
		/// <param name="userName">The user to impersonate</param>
		/// <param name="domain">The domain of the user to impersonate.</param>
		/// <param name="password">The password of the user to impersonate.</param>
		/// <param name="logOnProvider">The LogonProvider used to authenticate the impersonation user.</param>
		/// <param name="logOnType">The LogonType used to authenticate the impersonation user.</param>
		/// <returns>Returns a new WindowsIdentityImpersonator object.</returns>
		public static WindowsIdentityImpersonator BeginImpersonation(string userName, string domain, string password, LogOnTypes logOnType, LogOnProviders logOnProvider)
		{
			ImpersonationCredentials credentials = new ImpersonationCredentials()
			{
				UserName = userName,
				Domain = domain,
				Password = password,
			};

			return BeginImpersonation(credentials, logOnType, logOnProvider);
		}

		/// <summary>
		/// Initializes a new WindowsIdentityImpersonator object and begins impersionation.
		/// </summary>
		/// <param name="credentials">The credentials for the user to impersonate</param>
		/// <returns>Returns a new WindowsIdentityImpersonator object.</returns>
		public static WindowsIdentityImpersonator BeginImpersonation(ImpersonationCredentials credentials)
		{
			return BeginImpersonation(credentials, LogOnTypes.Interactive, LogOnProviders.Default);
		}

		/// <summary>
		/// Initializes a new WindowsIdentityImpersonator object and begins impersionation.
		/// </summary>
		/// <param name="credentials">The credentials for the user to impersonate</param>
		/// <param name="logOnProvider">The LogonProvider used to authenticate the impersonation user.</param>
		/// <param name="logOnType">The LogonType used to authenticate the impersonation user.</param>
		/// <returns>Returns a new WindowsIdentityImpersonator object.</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		public static WindowsIdentityImpersonator BeginImpersonation(ImpersonationCredentials credentials, LogOnTypes logOnType, LogOnProviders logOnProvider)
		{
			WindowsIdentityImpersonator retVal = new WindowsIdentityImpersonator()
			{
				Credentials = credentials,
				LogOnType = logOnType,
				LogOnProvider = logOnProvider
			};

			retVal.BeginImpersonation();
			return retVal;
		}

		/// <summary>
		/// Gets the Windows Impersionation Context object.
		/// </summary>
		public WindowsImpersonationContext ImpersonationContext { get; private set; }

		/// <summary>
		/// Gets the Windows Identity object.
		/// </summary>
		public WindowsIdentity ImpersonationIdentity { get; private set; }


		/// <summary>
		/// Gets or sets the credentials used to impersonate a user.
		/// </summary>
		public ImpersonationCredentials Credentials { get; set; }

		/// <summary>
		/// Gets or sets the LogonType used to authenticate the impersonation user.
		/// </summary>
		public LogOnTypes LogOnType { get; set; }
		/// <summary>
		/// Gets or sets the LogonProvider used to authenticate the impersonation user.
		/// </summary>
		public LogOnProviders LogOnProvider { get; set; }

		/// <summary>
		/// Gets a value indicating if the Windows Identity Impersonator object is currently impersonating the specified user.
		/// </summary>
		public bool Impersonating
		{
			get { return (ImpersonationContext != null); }
		}

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public WindowsIdentityImpersonator()
		{
			LogOnType = LogOnTypes.Interactive;
			LogOnProvider = LogOnProviders.Default;
			Credentials = new ImpersonationCredentials();
		}

		/// <summary>
		/// Begins impersionation using the specified user credentials.
		/// </summary>
		[SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke",
			Justification = "FxCop is misanalysing the code.")]
		public void BeginImpersonation()
		{
			if (ImpersonationContext != null) EndImpersonation();

			IntPtr handle = IntPtr.Zero;
			IntPtr duplicate = IntPtr.Zero;

			try
			{
				if (RevertToSelf())
				{
					string domain = string.IsNullOrEmpty(Credentials.Domain) ? "." : Credentials.Domain;

					if (LogonUser(Credentials.UserName, domain, Credentials.Password, (int)LogOnType, (int)LogOnProvider, ref handle))
					{
						if (DuplicateToken(handle, SecurityImpersonation, ref duplicate))
						{
							ImpersonationIdentity = new WindowsIdentity(duplicate);
							ImpersonationContext = ImpersonationIdentity.Impersonate();
						}
						else
						{
							throw new ImpersonationException("Unable to duplicate user token.", new Win32Exception(Marshal.GetLastWin32Error()));
						}
					}
					else
					{
						throw new ImpersonationException("Unable to logon impersonated user.", new Win32Exception(Marshal.GetLastWin32Error()));
					}
				}
			}
			catch (ImpersonationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ImpersonationException("Unexpected exception while attempting to begin impersonation.", ex);
			}
			finally
			{
				if (handle != IntPtr.Zero) CloseHandle(handle);
				if (duplicate != IntPtr.Zero) CloseHandle(duplicate);
			}
		}

		/// <summary>
		/// Ends Impersonation.
		/// </summary>
		public void EndImpersonation()
		{
			if (ImpersonationContext != null)
			{
				ImpersonationContext.Undo();
				ImpersonationContext.Dispose();
				ImpersonationContext = null;
			}
		}

		/// <summary>
		/// The LogonTypes used to authenticate the impersonation user.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This enumeration is only used within this class")]
		[SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue",
			Justification = "A value of 'None' is not appropriate for this enumeration")]
		public enum LogOnTypes : int
		{
			/// <summary>
			/// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
			/// by a terminal server, remote shell, or similar process.
			/// This logon type has the additional expense of caching logon information for disconnected operations; 
			/// therefore, it is inappropriate for some client/server applications,
			/// such as a mail server.
			/// </summary>
			Interactive = 2,

			/// <summary>
			/// This logon type is intended for high performance servers to authenticate plaintext passwords.
			/// The LogonUser function does not cache credentials for this logon type.
			/// </summary>
			Network = 3,

			/// <summary>
			/// This logon type is intended for batch servers, where processes may be executing on behalf of a user without 
			/// their direct intervention. This type is also for higher performance servers that process many plaintext
			/// authentication attempts at a time, such as mail or Web servers. 
			/// The LogonUser function does not cache credentials for this logon type.
			/// </summary>
			Batch = 4,

			/// <summary>
			/// Indicates a service-type logon. The account provided must have the service privilege enabled. 
			/// </summary>
			Service = 5,

			/// <summary>
			/// This logon type is for GINA DLLs that log on users who will be interactively using the computer. 
			/// This logon type can generate a unique audit record that shows when the workstation was unlocked. 
			/// </summary>
			Unlock = 7,

			/// <summary>
			/// This logon type preserves the name and password in the authentication package, which allows the server to make 
			/// connections to other network servers while impersonating the client. A server can accept plaintext credentials 
			/// from a client, call LogonUser, verify that the user can access the system across the network, and still 
			/// communicate with other servers.
			/// NOTE: Windows NT:  This value is not supported. 
			/// </summary>
			NetworkClearText = 8,

			/// <summary>
			/// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
			/// The new logon session has the same local identifier but uses different credentials for other network connections. 
			/// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
			/// NOTE: Windows NT:  This value is not supported. 
			/// </summary>
			NewCredentials = 9
		}

		/// <summary>
		/// The LogonProviders used to authenticate the impersonation user.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This enumeration is only used within this class")]
		public enum LogOnProviders : int
		{
			/// <summary>
			/// Use the standard logon provider for the system. 
			/// The default security provider is negotiate, unless you pass NULL for the domain name and the user name 
			/// is not in UPN format. In this case, the default provider is NTLM. 
			/// NOTE: Windows 2000/NT:   The default security provider is NTLM.
			/// </summary>
			Default = 0,
			/// <summary>
			/// Logon provider for Windows NT 3.5
			/// </summary>
			WinNT35 = 1,
			/// <summary>
			/// Logon provider for Windows NT 4.0
			/// </summary>
			WinNT40 = 2,
			/// <summary>
			/// Logon provider for Windows NT 5.0
			/// </summary>
			WinNT50 = 3
		}

		/// <summary>
		/// Ends impersonation.
		/// </summary>
		public void Dispose()
		{
			EndImpersonation();
		}
	}
}
