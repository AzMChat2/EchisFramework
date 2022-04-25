using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace System.Data
{
	/// <summary>
	/// Contains settings used by the System.Data framework.
	/// </summary>
	[Serializable]
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the Full Type name or ObjectId of the DecryptionProvider instance used to decrypt connection strings.
		/// </summary>
		[XmlAttribute]
		public string DecryptionProvider { get; set; }

		//NOTE: The XmlSerializer chokes if we use { get; private set;} on the DataAccessObjects property,
		//      but works fine if we use an underlying variable (_dataAccessObjects).
		/// <summary>
		/// Stores the list of Data Access Objects.
		/// </summary>
		private List<DataAccessObject> _dataAccessObjects = new List<DataAccessObject>();

		/// <summary>
		/// Gets the list of configured Data Access clients.
		/// </summary>
		[XmlElement("DataAccessObject")]
		public List<DataAccessObject> DataAccessObjects { get { return _dataAccessObjects; } }

		/// <summary>
		/// Validates that there is only one Default Data Access Object
		/// </summary>
		public override void Validate()
		{
			List<DataAccessObject> defaults = _dataAccessObjects.FindAll(item => item.Default);
			if (defaults.Count > 1) throw new InvalidOperationException("More than one Default Data Access Object defined.");
		}
	}

	/// <summary>
	/// The DataAccessObject contains the information needed to instantiate a Data Access Client object.
	/// </summary>
	[Serializable]
	public class DataAccessObject
	{
		/// <summary>
		/// Gets or sets the name of the Data Access client.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Sets the connection string use to connect to the database for the Data Access client.
		/// </summary>
		[XmlAttribute]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
			Justification = "Simulating an internal method call and subsequent ArgumentNullException")]
		[DebuggerHidden]
		public string ConnectionString
		{
			get { return string.Empty; }
			set
			{
				if (string.IsNullOrEmpty(Name))
				{
					throw new ArgumentNullException("Name");
				}
				ConnectionInfoDictionary.SetConnectionString(Name, value);
			}
		}

		/// <summary>
		/// Gets or sets the Type or ObjectId of the Data Access Client.
		/// </summary>
		[XmlAttribute]
		public string DataAccessClient { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if this is the default database connection.
		/// </summary>
		/// <remarks>
		/// Only one Data Access Client can be configured as the Default.
		/// </remarks>
		[XmlAttribute]
		public bool Default { get; set; }

		/// <summary>
		/// Gets or sets the Impersonation Credentials used to connect to the database.
		/// </summary>
		/// <remarks>If not specified, the connection will be made using the current user account under which the application is running.</remarks>
		[XmlElement]
		[DebuggerHidden]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
			Justification = "Simulating an internal method call and subsequent ArgumentNullException")]
		public DataAccessCredentials Credentials
		{
			get { return null; }
			set
			{
				if (string.IsNullOrEmpty(Name)) throw new ArgumentNullException("Name");

				ConnectionInfoDictionary.SetCredentials(Name, value);
			}
		}
	}
}
