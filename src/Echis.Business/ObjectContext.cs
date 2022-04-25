using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace System.Data.Objects
{
	/// <summary>
	/// Provides facilities for querying and working with entity data as objects.
	/// </summary>
	public abstract class ObjectContext<T> : ObjectContext
		where T : ObjectContext
	{
		/// <summary>
		/// Stores EntityConnection instances by the given container name.
		/// </summary>
		private static Dictionary<string, EntityConnectionInfo> _connections = new Dictionary<string, EntityConnectionInfo>();
		
		/// <summary>
		/// Looks up Entity Connection information for the given container name from the settings instance.
		/// </summary>
		/// <param name="containerName">The name of the entity container.</param>
		/// <param name="additionalPaths">A collection of additional paths provided by the constructor.</param>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "This is a factory method for creating an Entity Connection, consuming code will dispose the object.")]
		protected static EntityConnection GetEntityConnection(string containerName, string[] additionalPaths)
		{
			if (string.IsNullOrEmpty(containerName)) throw new ArgumentNullException("containerName");

			EntityConnectionInfo connectionInfo = GetEntityConnectionInfo(containerName, additionalPaths);

			DbConnection connection = ReflectionExtensions.CreateObject<DbConnection>(connectionInfo.DbConnection.DbConnectionType);
			connection.ConnectionString = connectionInfo.DbConnection.ConnectionString;
			return new EntityConnection(connectionInfo.Workspace, connection);
		}

		/// <summary>
		/// Gets the Entity Connection information for the given container name from the Connection Info Cache, or creates a new information object and adds it to the cache.
		/// </summary>
		/// <param name="containerName">The name of the entity container.</param>
		/// <param name="additionalPaths">A collection of additional paths provided by the constructor.</param>
		private static EntityConnectionInfo GetEntityConnectionInfo(string containerName, string[] additionalPaths)
		{
			if (!_connections.ContainsKey(containerName))
			{
				lock (_connections)
				{
					if (!_connections.ContainsKey(containerName))
					{
						EntityConnectionInfo newInfo = Settings.Values.EntityConnections.Find(item => containerName.Equals(item.ContainerName, StringComparison.OrdinalIgnoreCase));
						if (newInfo == null) throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "The Container Name '{0}' is not configured.", containerName));

						newInfo.Workspace = new MetadataWorkspace(new ModelPathList(newInfo.Resources, additionalPaths), new Assembly[] { typeof(T).Assembly });
						_connections.Add(containerName, newInfo);
					}
				}
			}

			return _connections[containerName];
		}

		/// <summary>
		/// Initializes a new instance of the Context class with the given Container Name.
		/// </summary>
		/// <param name="containerName">The name of the entity container.  This name is used to lookup Entity Connection information.</param>
		/// <param name="additionalPaths">A collection of additional paths provided by the derived class.</param>
		protected ObjectContext(string containerName, params string[] additionalPaths)
			: base(GetEntityConnection(containerName, additionalPaths), containerName) { }
	
		/// <summary>
		/// Initializes a new instance of the Context class with the given Container Name.
		/// </summary>
		/// <param name="containerName">The name of the entity container.</param>
		/// <param name="connection">An EntityConnection instance which contains references to the model and data source information.</param>
		protected ObjectContext(EntityConnection connection, string containerName)
			: base(connection, containerName) { }


		/// <summary>
		/// Used to combine configured paths, with additional paths specified by the constructor.
		/// </summary>
		private class ModelPathList : List<string>
		{
			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="configuredPaths">The collection of paths from the configuration.</param>
			/// <param name="additionalPaths">Additional paths specified by the constructor.</param>
			public ModelPathList(IEnumerable<string> configuredPaths, string[] additionalPaths) : base(configuredPaths ?? new string[0])
			{
				additionalPaths.ForEach(AddAdditionalPath);
			}

			/// <summary>
			/// Adds an additional path to the collection.
			/// If the path contains a placeholder, the placeholder is replaced with "csdl", "ssdl" and "msl" and each variation is added to the collection.
			/// </summary>
			private void AddAdditionalPath(string additionalPath)
			{
				if (additionalPath.Contains("{0}"))
				{
					AddAdditionalPath(string.Format(CultureInfo.InvariantCulture, additionalPath, "csdl"));
					AddAdditionalPath(string.Format(CultureInfo.InvariantCulture, additionalPath, "ssdl"));
					AddAdditionalPath(string.Format(CultureInfo.InvariantCulture, additionalPath, "msl"));
				}
				else
				{
					if (!Contains(additionalPath)) Add(additionalPath);
				}
			}
		}
	}
}
