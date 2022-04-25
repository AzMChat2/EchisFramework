using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using System.Configuration;
using System.Security;

namespace System.Data.Objects
{
	/// <summary>
	/// Contains Configuration Settings for System Data Objects
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the Full Type name or ObjectId of the DecryptionProvider instance used to decrypt connection strings.
		/// </summary>
		[XmlAttribute]
		public string DecryptionProvider { get; set; }

		/// <summary>
		/// Gets or sets the collection of Db Connections which the Entity Contexts will use to connect.
		/// </summary>
		[XmlElement("DbConnection")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db",
			Justification = "Casing matches that of Microsoft's .Net Framework.")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public List<DbConnectionInfo> DbConnections { get; set; }

		/// <summary>
		/// Gets the configured collection of EntityConnectionInfo objects.
		/// </summary>
		[XmlElement("EntityConnection")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public List<EntityConnectionInfo> EntityConnections { get; set; }
	}

	/// <summary>
	/// Stores information used to create an EntityConnection object.
	/// </summary>
	public class EntityConnectionInfo
	{
		/// <summary>
		/// Gets the Container Name which uses this Entity Connection Information.
		/// </summary>
		[XmlAttribute]
		public string ContainerName { get; set; }

		/// <summary>
		/// Gets or sets the name of the Db Connection which this Entity Connection will use.
		/// </summary>
		[XmlAttribute("DbConnection")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db",
			Justification = "Casing matches that of Microsoft's .Net Framework.")]
		public string DbConnectionName { get; set; }

		/// <summary>
		/// Stores the Db Connection Info instance for this Entity Connection info instance.
		/// </summary>
		private DbConnectionInfo _dbConnection;
		/// <summary>
		/// Gets or sets the Db Connection Info instance for this Entity Connection info instance.
		/// </summary>
		[XmlIgnore]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db",
			Justification = "Casing matches that of Microsoft's .Net Framework.")]
		public DbConnectionInfo DbConnection
		{
			get
			{
				if (_dbConnection == null) _dbConnection = Settings.Values.DbConnections.Find(item => item.Name.Equals(DbConnectionName, StringComparison.OrdinalIgnoreCase));
				return _dbConnection;
			}
		}

		/// <summary>
		/// Gets the list of Resources used to generate the Metadata Workspace.
		/// </summary>
		[XmlElement("Resource")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public List<string> Resources { get; set; }

		/// <summary>
		/// Gets or sets the Entity Framework MetadataWorkspace associated with the entity connection.
		/// </summary>
		[XmlIgnore]
		public MetadataWorkspace Workspace { get; set; }

	}

	/// <summary>
	/// Stores information used to connect to a Database
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db",
		Justification = "Casing matches that of Microsoft's .Net Framework.")]
	public class DbConnectionInfo
	{
		/// <summary>
		/// Gets or sets the name of the Db Connection Information instance.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets the DBConnection Type for the Entity Connection.
		/// </summary>
		[XmlIgnore]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db",
			Justification = "The term DbConnection matches the Microsoft .Net object name.")]
		public Type DbConnectionType { get; private set; }

		/// <summary>
		/// Gets or sets the DBConnection Type Name for the Entity Connection.
		/// </summary>
		[XmlAttribute("DbConnectionType")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db",
			Justification = "The therm DbConnection matches the Microsoft .Net object name.")]
		public string DbConnectionTypeName
		{
			get { return DbConnectionType.FullName; }
			set { DbConnectionType = Type.GetType(value, true, true); }
		}

		private string _connectionString;
		private bool _isEncrypted;
		/// <summary>
		/// Gets the Connection String for the Entity Connection
		/// </summary>
		[XmlAttribute]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "It is unknown what exception(s) the Decryption Provider might throw.")]
		public string ConnectionString
		{
			get
			{
				if (_isEncrypted)
				{
					try
					{
						_connectionString = Decryptor.I.DecryptString(_connectionString);
					}
					catch { }

					_isEncrypted = false;
				}

				return _connectionString;
			}
			set
			{
				_isEncrypted = true;
				_connectionString = value;
			}
		}
	}


	/// <summary>
	/// Used internally to decrypt Connection Strings.
	/// </summary>
	internal static class Decryptor
	{
		/// <summary>
		/// The default ObjectId for the Decryption Provider.
		/// </summary>
		private const string DecryptionProviderObjectId = "System.Data.DecryptionProvider";

		/// <summary>
		/// Stores the singleton instance of the DecryptionProvider.
		/// </summary>
		private static IDecryptionProvider _i;
		/// <summary>
		/// Gets the singleton instance of the DecryptionProvider.
		/// </summary>
		public static IDecryptionProvider I
		{
			get
			{
				if (_i == null) _i = IOC.GetFrameworkObject<IDecryptionProvider>(Settings.Values.DecryptionProvider, DecryptionProviderObjectId, typeof(DefaultCryptographyProvider));
				return _i;
			}
		}
	}
	
}
