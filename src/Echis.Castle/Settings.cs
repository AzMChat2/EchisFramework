using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

using System.Configuration;

namespace System.Castle
{
	/// <summary>
	/// Stores settings used by the System.Castle Inversion Of Control.
	/// </summary>
	[Serializable]
	public class Settings : SettingsBase<Settings>
	{
		//NOTE: The XmlSerializer chokes if we use { get; private set;} on the Containers property,
		//      but works fine if we use an underlying variable (_containers).
		/// <summary>
		/// Stores the list of Containers.
		/// </summary>
		private List<ContainerInfo> _containers = new List<ContainerInfo>();
		/// <summary>
		/// Gets the collection of container information used to initialize the WindsorContainers.
		/// </summary>
		[XmlElement("Container")]
		public List<ContainerInfo> Containers { get { return _containers; } }
	}

	/// <summary>
	/// Stores information used to create a WindsorContainer.
	/// </summary>
	[Serializable]
	public class ContainerInfo
	{
		/// <summary>
		/// Gets or sets the name (ContextId) of the Container
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of Container to be created.
		/// </summary>
		[XmlIgnore]
		public Type ContainerType { get; set; }

		/// <summary>
		/// Gets or sets the full name of the type of Container to be created.
		/// </summary>
		[XmlAttribute("ContainerType")]
		public string ContainerTypeName
		{
			get { return (ContainerType == null) ? null : ContainerType.FullName; }
			set { ContainerType = Type.GetType(value); }
		}

		/// <summary>
		/// Gets or sets the type of Configuration Interpreter to be created.
		/// </summary>
		[XmlIgnore]
		public Type InterpreterType { get; set; }

		/// <summary>
		/// Gets or sets the full name of the type of Configuration Interpreter to be created.
		/// </summary>
		[XmlAttribute("InterpreterType")]
		public string InterpreterTypeName
		{
			get { return (InterpreterType == null) ? null : InterpreterType.FullName; }
			set { InterpreterType = Type.GetType(value); }
		}
	}
}
