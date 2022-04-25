using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace System.ObjectPools
{
	/// <summary>
	/// Defines a Named Pool object.
	/// </summary>
	public interface INamedPoolObject : IPoolableObject
	{
		/// <summary>
		/// Gets the name of the object pool.
		/// </summary>
		string Name { get; }
	}

	/// <summary>
	/// Represents a pool of named objects.
	/// </summary>
	/// <typeparam name="TPool">The derived type.</typeparam>
	/// <typeparam name="TObject">The type of objects contained in the Object Pool.</typeparam>
	public abstract class NamedObjectPool<TPool, TObject> : ObjectPool<TObject>
		where TPool : NamedObjectPool<TPool, TObject>, new()
		where TObject : INamedPoolObject
	{

		#region Static members
		/// <summary>
		/// Contains a list of Named ObjectPools.
		/// </summary>
		private static List<TPool> _poolList;
		/// <summary>
		/// Used to create Pool Status information.
		/// </summary>
		private static StringBuilder _poolInfo;

		/// <summary>
		/// Static Constructor.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
			Justification = "Not possible to initialize static fields inline.")]
		static NamedObjectPool()
		{
			Refresh();
		}

		/// <summary>
		/// Gets the size of the Object Pool.
		/// </summary>
		public static int PoolSize
		{
			get { return _poolList.Count; }
		}

		/// <summary>
		/// Gets or sets the PoolName to use if the specified Name does not exist.
		/// </summary>
		protected static string DefaultPoolName { get; set; }

		/// <summary>
		/// Gets the current status of the Object Pool.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
			Justification = "A property is not appropriate here.")]
		public static string GetPoolStatus()
		{
			StringBuilder retVal = new StringBuilder();
			_poolList.ForEach(item => retVal.AppendLine(item.Status));
			return retVal.ToString();
		}

		/// <summary>
		/// Gets a list of all the Pool names.
		/// </summary>
		public static string[] GetPoolNames()
		{
			List<string> retVal = new List<string>();
			_poolList.ForEach(item => retVal.Add(item.Name));
			return retVal.ToArray();
		}

		/// <summary>
		/// Stores a list of assembly file names which have been interogated for Pool object types.
		/// </summary>
		private static List<string> _assemblyFileNames;

		/// <summary>
		/// Refreshes the Object Pools, re-interogating loaded assemblies and assembly files located in the application directory, or subdirectory.
		/// </summary>
		/// <returns>Returns a string containing any messages generated while interogating assemblies for Pool Object types.</returns>
		public static string Refresh()
		{
			_poolInfo = new StringBuilder();
			_poolList = new List<TPool>();
			_assemblyFileNames = new List<string>();

			try
			{
				// Get current AppDomain
				AppDomain currentDomain = AppDomain.CurrentDomain;

				// Get Search Options for this Object Pool.
				SearchOptions searchOptions = Settings.Values.GetSearchOptions(typeof(TObject));

				// Check loaded assemblies for the Pooled Object type
				if (searchOptions.SearchLoadedAssemblies) currentDomain.GetAssemblies().ForEach(SearchAssemblyTypes);

				// Search for Assemblies containing the Pooled Object type in AppDomain folders.
				if (searchOptions.SearchDynamicDirectory) SearchFolder(currentDomain.DynamicDirectory);
				if (searchOptions.SearchBaseDirectory) SearchFolder(currentDomain.BaseDirectory);
				if (searchOptions.SearchRelativeSearchPath) SearchFolder(currentDomain.RelativeSearchPath);

				// Search additional paths for Assemblies containing the Pooled Object type.
				searchOptions.AdditionalSearchPaths.ForEach(SearchFolder);

				// Finally search for the Pooled Object type in the specified assemblies
				searchOptions.AssembliesToSearch.ForEach(SearchAssembly);

				// Return status
				return _poolInfo.ToString();
			}
			catch
			{
				_poolList = null; // Don't allow a partially initialized pool... 
				throw;
			}
			finally
			{
				// Cleanup
				_poolInfo = null;
				_assemblyFileNames = null;
			}
		}

		private static void SearchFolder(string rootDir)
		{
			string[] assemblies = Directory.GetFiles(rootDir, "*.dll", SearchOption.AllDirectories);

			if (TS.Verbose)
			{
				string msg = string.Format(CultureInfo.InvariantCulture, "Searching {0} assemblies found in '{1}'.", assemblies.Length, rootDir);
				_poolInfo.AppendLine(msg);
				Trace.WriteLine(msg, TS.Categories.Event);
			}

			assemblies.ForEach(SearchAssemblyFile);
		}

		/// <summary>
		/// Searches an assembly for Pool Object Types.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly to search.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Multiple types of exceptions are possible and all are handled the same way.")]
		//[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
		//  MessageId = "System.Reflection.Assembly.LoadFile",
		//  Justification = "The call to Assembly.LoadFile is necessary.")]
		private static void SearchAssembly(string assemblyName)
		{
			try
			{
				if (TS.Verbose)
				{
					string msg = string.Format(CultureInfo.InvariantCulture, "Loading and searching assembly '{0}' for Pooled Object Types.", assemblyName);

					_poolInfo.AppendLine(msg);
					Trace.WriteLine(msg, TS.Categories.Event);
				}

				SearchAssemblyTypes(Assembly.Load(assemblyName));
			}
			catch (Exception ex)
			{
				if (TS.Verbose)
				{
					string msg = string.Format(CultureInfo.InvariantCulture, "Error Searching '{0}' assembly file: {1}",
						Path.GetFileName(assemblyName), ex.GetExceptionMessage("->"));

					_poolInfo.AppendLine(msg);
					Trace.WriteLine(msg, TS.Categories.Event);
				}
			}
		}

		/// <summary>
		/// Searches an assembly file for Pool Object Types.
		/// </summary>
		/// <param name="assemblyFilename">The filename of the assembly file.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Multiple types of exceptions are possible and all are handled the same way.")]
		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
			MessageId = "System.Reflection.Assembly.LoadFile",
			Justification = "The call to Assembly.LoadFile is necessary.")]
		private static void SearchAssemblyFile(string assemblyFilename)
		{
			if (!_assemblyFileNames.Contains(assemblyFilename.ToUpperInvariant()))
			{
				try
				{
					if (TS.Verbose)
					{
						string msg = string.Format(CultureInfo.InvariantCulture, "Searching '{0}' assembly file for Pooled Object Types.",
							Path.GetFileName(assemblyFilename));

						_poolInfo.AppendLine(msg);
						Trace.WriteLine(msg, TS.Categories.Event);
					}

					SearchAssemblyTypes(Assembly.LoadFile(assemblyFilename));
				}
				catch (Exception ex)
				{
					if (TS.Verbose)
					{
						string msg = string.Format(CultureInfo.InvariantCulture, "Error Searching '{0}' assembly file: {1}",
							Path.GetFileName(assemblyFilename), ex.GetExceptionMessage("->"));

						_poolInfo.AppendLine(msg);
						Trace.WriteLine(msg, TS.Categories.Event);
					}
				}
			}
		}

		/// <summary>
		/// Searches an assembly for Pool Object Types.
		/// </summary>
		/// <param name="assembly">The assembly to be searched.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Multiple types of exceptions are possible and all are handled the same way.")]
		private static void SearchAssemblyTypes(Assembly assembly)
		{
			try
			{
				if ((assembly != null) && !_assemblyFileNames.Contains(assembly.Location.ToUpperInvariant()))
				{
					if (TS.Verbose)
					{
						string msg = string.Format(CultureInfo.InvariantCulture, "Searching '{0}' assembly for Pooled Object Types.", assembly.GetName().Name);

						_poolInfo.AppendLine(msg);
						Trace.WriteLine(msg, TS.Categories.Event);
					}

					_assemblyFileNames.Add(assembly.Location.ToUpperInvariant());

					assembly.GetTypes().ForEach(CheckType);
				}
			}
			catch (Exception ex)
			{
				if (TS.Verbose)
				{
					string msg = string.Format(CultureInfo.InvariantCulture, "Error Searching '{0}' assembly: {1}",
						assembly.GetName().Name, ex.GetExceptionMessage("->"));

					_poolInfo.AppendLine(msg);
					Trace.WriteLine(msg, TS.Categories.Event);
				}
			}
		}

		/// <summary>
		/// Checks if a type is a Pool Object Type, if it is, it adds it to the Pool.
		/// </summary>
		/// <param name="type">The Type of the potential Pool Object.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Multiple types of exceptions are possible and all are handled the same way.")]
		private static void CheckType(Type type)
		{
			if ((type.GetInterface(typeof(TObject).Name) != null) && !type.IsAbstract && !type.IsGenericTypeDefinition)
			{
				_poolInfo.AppendFormat("Named Pool Object found in {0}:{1}", type.Assembly.FullName, Environment.NewLine);
				try
				{
					TPool newPool = new TPool();
					newPool.Initialize(type);
					TPool oldPool = _poolList.Find(item => (item.Name == newPool.Name));

					if (oldPool == null)
					{
						_poolInfo.Append("\tAdded: ");
						_poolInfo.AppendLine(newPool.Status);
						_poolList.Add(newPool);
					}
					else if ((newPool.AssemblyVersion > oldPool.AssemblyVersion) ||
							((newPool.AssemblyVersion == oldPool.AssemblyVersion) && (newPool.AssemblyDate > oldPool.AssemblyDate)))
					{
						_poolInfo.Append("\tUpdated: ");
						_poolInfo.AppendLine(newPool.Status);
						_poolList.Remove(oldPool);
						_poolList.Add(newPool);
					}
					else
					{
						_poolInfo.Append("\tSkipped: ");
						_poolInfo.AppendLine(newPool.Status);
					}
				}
				catch (Exception ex)
				{
					string msg = string.Format(CultureInfo.InvariantCulture, "Unable to add Named Object Pool for Type '{0}'.\r\n\t{1}: {2}",
						type.FullName, ex.GetType().Name, ex.GetExceptionMessage());

					_poolInfo.AppendLine(msg);
					TS.Logger.WriteLineIf(TS.EC.TraceWarning, TS.Categories.Warning, msg);
				}
			}
		}

		/// <summary>
		/// Gets a Pooled Object by Name from the Pool.
		/// </summary>
		/// <param name="name">The context name of the Pooled Object.</param>
		/// <remarks>This method will wait indefinitely for an object to become free.</remarks>
		protected static PooledObject<TObject> GetPooledObject(string name)
		{
			return GetPooledObject(name, 0);
		}

		/// <summary>
		/// Gets a Pooled Object by Name from the Pool.
		/// </summary>
		/// <param name="name">The context name of the Pooled Object.</param>
		/// <param name="timeout">The amount of time to wait for an object to become free.</param>
		protected static PooledObject<TObject> GetPooledObject(string name, int timeout)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

			TPool pool = _poolList.Find(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

			if ((pool == null) && !string.IsNullOrEmpty(DefaultPoolName))
			{
				pool = _poolList.Find(item => item.Name.Equals(DefaultPoolName, StringComparison.OrdinalIgnoreCase));
			}

			if (pool == null)
			{
				throw new PoolNotFoundException(name);
			}
			else
			{
				return pool.Get(timeout);
			}
		}
		#endregion

		#region Instance Members
		/// <summary>
		/// Binding flags used to find the Constructor for the Pooled Object type.
		/// </summary>
		private static readonly BindingFlags _flags = (BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

		/// <summary>
		/// Initializes a type for use by the Object Pool.
		/// </summary>
		/// <param name="type"></param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Multiple types of exceptions are possible and all are handled the same way.")]
		private void Initialize(Type type)
		{
			Constructor = type.GetConstructor(_flags, null, Type.EmptyTypes, null);
			AssemblyVersion = type.Assembly.GetName().Version;

			try
			{
				AssemblyDate = File.GetLastWriteTime(type.Assembly.Location);
			}
			catch { }

			base.Initialize();
		}

		/// <summary>
		/// The assembly version for the Pooled Object Type.
		/// </summary>
		protected Version AssemblyVersion { get; private set; }
		/// <summary>
		///  The assembly date for the Pooled Object Type
		/// </summary>
		protected DateTime AssemblyDate { get; private set; }

		/// <summary>
		/// Stores the name property value for the Pooled Object Type.
		/// </summary>
		private string _name;
		/// <summary>
		/// Gets the Name of the Pooled Object.
		/// </summary>
		protected string Name
		{
			get
			{
				if (_name == null)
				{
					using (PooledObject<TObject> wrapper = Get())
					{
						_name = wrapper.Value.Name;
					}
				}
				return _name;
			}
		}

		/// <summary>
		/// Gets a status message for the Object Pool.
		/// </summary>
		protected string Status
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}: {1} instances\tVersion: {2}\tDate:{3:MM/dd/yyyy hh:mm:ss}",
					WrappedType.Name, Count, AssemblyVersion, AssemblyDate);
			}
		}
		#endregion

	}
}
