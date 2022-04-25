using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Configuration.Managers.FileSystem
{
	/// <summary>
	/// Represents a Machine Group.
	/// </summary>
	public class MachineGroup
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public MachineGroup()
		{
			Machines = new MachineList();
		}

		/// <summary>
		/// Gets or sets the name of the Machine Group.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the list of Machines belonging to the Machine Group.
		/// </summary>
		[XmlElement("Machine")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
			Justification="Property Setter is required by the XmlSerializer.")]
		public MachineList Machines { get; set; }

		/// <summary>
		/// Determines if the Machine Group contains the specified Machine.
		/// </summary>
		/// <param name="machineName">The name of the Machine.</param>
		/// <returns>Returns true if the Machine Group contains the specified Machine.</returns>
		public bool HasMachine(string machineName)
		{
			return Machines.Exists(item => item.IsMatch(machineName));
		}
	}

	/// <summary>
	/// Represents the configured Machine Groups list.
	/// </summary>
	[XmlType("MachineGroups")]
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", 
		Justification="List is the appropriate suffix.")]
	public class MachineGroupList : List<MachineGroup>
	{
		/// <summary>
		/// Stores the global instance of the Machine Groups List
		/// </summary>
		private static MachineGroupList _instance = XmlSerializer<MachineGroupList>.DeserializeFromXmlFile(Settings.Values.MachineGroupsFilePath);

		/// <summary>
		/// Gets the list of Machine Groups that the specified machine belongs to.
		/// </summary>
		/// <param name="machineName">The name of the Machine.</param>
		/// <returns>Returns the list of Machine Groups that the specified machine belongs to.</returns>
		public static List<string> GetMachineGroups(string machineName)
		{
			List<string> retVal = new List<string>();

			List<MachineGroup> groups = _instance.FindAll(item => item.HasMachine(machineName));
			groups.ForEach(item => retVal.Add(item.Name));

			return retVal;
		}
	}

	/// <summary>
	/// Represents a Machine
	/// </summary>
	public class Machine
	{
		/// <summary>
		/// Gets or set the name of the Machine.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Determines if the specified machine is a match for the Machine this object represents.
		/// </summary>
		/// <param name="machineName">The name of the Machine.</param>
		/// <returns>Returns true if the specified machine is a match for the Machine this object represents.</returns>
		public bool IsMatch(string machineName)
		{
			return (Name == "*" || Name.Equals(machineName, StringComparison.OrdinalIgnoreCase));
		}
	}

	/// <summary>
	/// Represents a list of Machines.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "List is the appropriate suffix.")]
	public class MachineList : List<Machine> { }

}
