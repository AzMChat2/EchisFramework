using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using System.Configuration;
using System.Diagnostics;

namespace System.ObjectPools
{
	/// <summary>
	/// Represents configuration settings used by the Object Pool.
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the Default Pool Size.  This is the initial pool size.
		/// </summary>
		[XmlAttribute]
		public int DefaultPoolSize { get; set; }

		/// <summary>
		/// Gets or sets the default search options which are used if no Search Option definition exists for the specific object pool type.
		/// </summary>
		[XmlElement]
		public SearchOptions DefaultSearchOptions { get; set; }

		/// <summary>
		/// Gets or sets the specific object pool type search options list.
		/// </summary>
		[XmlElement("SearchOptions")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Property Setter is required by the XmlSerializer.")]
		public List<SearchOptions> SearchOptionsList { get; set; }

		/// <summary>
		///  Gets the search options for the specified object pool.
		/// </summary>
		/// <param name="poolType"></param>
		/// <returns></returns>
		public SearchOptions GetSearchOptions(Type poolType)
		{
			SearchOptions retVal = SearchOptionsList.Find(item => item.PoolTypeName.Equals(poolType.Name, StringComparison.OrdinalIgnoreCase));
			if (retVal == null) retVal = SearchOptionsList.Find(item => item.PoolTypeName.Equals(poolType.FullName, StringComparison.OrdinalIgnoreCase));

			if (retVal == null)
			{
				TS.Logger.WriteLineIf(TS.Info, TS.Categories.Info, "Unable to find Search Options for PoolType '{0}' ('{1}') using Default Search Options.", poolType.Name, poolType.FullName);
				retVal = DefaultSearchOptions;
			}
			return retVal;
		}

		/// <summary>
		/// Validates the settings object.
		/// </summary>
		public override void Validate()
		{
			// Ensure that we have default search options
			if (DefaultSearchOptions == null)
			{
				DefaultSearchOptions = new SearchOptions()
				{
					SearchBaseDirectory = true,
					SearchDynamicDirectory = true,
					SearchRelativeSearchPath = true
				};
			}

			Validate(DefaultSearchOptions);

			// Remove any search options with an empty PoolTypeName.
			SearchOptionsList.RemoveAll(item => string.IsNullOrEmpty(item.PoolTypeName));
			SearchOptionsList.ForEach(Validate);
		}

		private static void Validate(SearchOptions searchOptions)
		{
			if (searchOptions.AdditionalSearchPaths == null) searchOptions.AdditionalSearchPaths = new List<string>();
		}

		/// <summary>
		/// Ignores any configuration exceptions.
		/// </summary>
		protected override void HandleException(System.Exception exception)
		{
			// Ignore errors
		}
	}

	/// <summary>
	/// Represents search options used when dynamically loading types from assembly files.
	/// </summary>
	public class SearchOptions
	{
		/// <summary>
		/// Gets or sets the name of the Object Pool.
		/// </summary>
		[XmlAttribute]
		public string PoolTypeName { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if Loaded Assemblies should be searched for Pooled Object Types.
		/// </summary>
		[XmlAttribute]
		public bool SearchLoadedAssemblies { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the Dynamic Directory should be searched for assemblies containing Pooled Object Types.
		/// </summary>
		[XmlAttribute]
		public bool SearchDynamicDirectory { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the Base Directory should be searched for assemblies containing Pooled Object Types.
		/// </summary>
		[XmlAttribute]
		public bool SearchBaseDirectory { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the Relative Search Path should be searched for assemblies containing Pooled Object Types.
		/// </summary>
		[XmlAttribute]
		public bool SearchRelativeSearchPath { get; set; }

		/// <summary>
		/// Gets or sets a list of additional folders to be searched for assemblies.
		/// </summary>
		[XmlElement("AdditionalSearchPath")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Property Setter is required by the XmlSerializer.")]
		public List<string> AdditionalSearchPaths { get; set; }

		/// <summary>
		/// Gets or sets a list of assemblies to be searched for Pooled Object Types.
		/// </summary>
		[XmlElement("Assembly")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Property Setter is required by the XmlSerializer.")]
		public List<string> AssembliesToSearch { get; set; }

	}
}
