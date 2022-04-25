using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace System.Security.Principal
{
	/// <summary>
	/// Represents credentials used to impersonate a user.
	/// </summary>
	public class ImpersonationCredentials
	{
		/// <summary>
		/// Gets or sets the Impersionation User Name.
		/// </summary>
		[XmlAttribute]
		public string UserName { get; set; }
		/// <summary>
		/// Gets or sets the Impersionation User Domain.
		/// </summary>
		/// <remarks>Leave this property empty or null to indicate the user account is a local account.</remarks>
		[XmlAttribute]
		public string Domain { get; set; }
		/// <summary>
		/// Gets or sets the Impersionation User Password.
		/// </summary>
		[XmlAttribute]
		public string Password { get; set; }
	}
}
