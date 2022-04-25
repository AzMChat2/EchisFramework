using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Configuration.Managers.FileSystem
{
	/// <summary>
	/// Represents a User Group.
	/// </summary>
	public class UserGroup
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public UserGroup()
		{
			Users = new UserList();
		}

		/// <summary>
		/// Gets or sets the Name of the User Group.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the list of Users belonging to this User Group.
		/// </summary>
		[XmlElement("User")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
			Justification="Property Setter is required by the XmlSerializer.")]
		public UserList Users { get; set; }

		/// <summary>
		/// Determines if the specified user exists in this group.
		/// </summary>
		/// <param name="userName">The name of the User.</param>
		/// <param name="userDomain">The Domain name of the User.</param>
		/// <returns></returns>
		public bool HasUser(string userName, string userDomain)
		{
			return Users.Exists(item => item.IsMatch(userName, userDomain));
		}
	}

	/// <summary>
	/// Represents the configured list of user groups.
	/// </summary>
	[XmlType("UserGroups")]
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", 
		Justification = "List is the appropriate suffix.")]
	public class UserGroupList : List<UserGroup>
	{
		/// <summary>
		/// Stores the global UserGroupList instance.
		/// </summary>
		private static UserGroupList _instance = XmlSerializer<UserGroupList>.DeserializeFromXmlFile(Settings.Values.UserGroupsFilePath);

		/// <summary>
		/// Gets a list of User Groups the specified user belongs to.
		/// </summary>
		/// <param name="userName">The name of the User.</param>
		/// <param name="userDomain">The Domain name of the User.</param>
		/// <returns></returns>
		public static List<string> GetUserGroups(string userName, string userDomain)
		{
			List<string> retVal = new List<string>();

			List<UserGroup> groups = _instance.FindAll(item => item.HasUser(userName, userDomain));
			groups.ForEach(item => retVal.Add(item.Name));

			return retVal;
		}
	}

	/// <summary>
	/// Represents a User.
	/// </summary>
	public class User
	{
		/// <summary>
		/// Gets or sets the name of the User.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Domain name of the User.
		/// </summary>
		[XmlAttribute]
		public string Domain { get; set; }

		/// <summary>
		/// Determines if the specified user matches this instance.
		/// </summary>
		/// <param name="userName">The name of the User.</param>
		/// <param name="userDomain">The Domain name of the User.</param>
		/// <returns>Returns true if the specified user is a match for the user this object represents.</returns>
		public bool IsMatch(string userName, string userDomain)
		{
			bool matchName = (Name == "*" || Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
			bool matchDomain = (Domain == "*" || Domain.Equals(userDomain, StringComparison.OrdinalIgnoreCase));

			return (matchName && matchDomain);
		}
	}

	/// <summary>
	/// Represents a list of Users.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "List is the correct suffix.")]
	public class UserList : List<User> { }

}
