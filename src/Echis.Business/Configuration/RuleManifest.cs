using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

namespace System.Business
{
	/// <summary>
	/// Contains search information for Manifest Files and Ruleset Files.
	/// </summary>
	public class RuleManifest
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public RuleManifest()
		{
			ManifestLocations = new List<SearchLocation>();
			RulesetLocations = new List<SearchLocation>();
		}

		/// <summary>
		/// Gets the list of manifest file locations.
		/// </summary>
		[XmlElement("ManifestLocation")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public List<SearchLocation> ManifestLocations { get; set; }

		/// <summary>
		/// Gets the list of Ruleset file locations.
		/// </summary>
		[XmlElement("RulesetLocation")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public List<SearchLocation> RulesetLocations { get; set; }
	}

	/// <summary>
	/// Contains search information for locating Rule Manifest Files
	/// </summary>
	public class SearchLocation
	{
		/// <summary>
		/// Stores Base paths which are used to find a full path based on a partial path.
		/// </summary>
		private static List<string> _basePaths;
		/// <summary>
		/// Gets Base paths which are used to find a full path based on a partial path.
		/// </summary>
		[XmlIgnore]
		protected static List<string> BasePaths
		{
			get
			{
				if (_basePaths == null)
				{
					_basePaths = new List<string>();
					_basePaths.AddIf(Environment.CurrentDirectory, IsValidPath);
					_basePaths.AddIf(AppDomain.CurrentDomain.BaseDirectory, IsValidPath);
					_basePaths.AddIf(AppDomain.CurrentDomain.DynamicDirectory, IsValidPath);
					_basePaths.AddIf(AppDomain.CurrentDomain.RelativeSearchPath, IsValidPath);
					_basePaths.AddIf(Environment.GetFolderPath(Environment.SpecialFolder.System), IsValidPath);
					_basePaths.AddIf(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), IsValidPath);
				}
				return _basePaths;
			}
		}

		private static bool IsValidPath(string path)
		{
			return !string.IsNullOrEmpty(path);
		}

		/// <summary>
		/// Stores the Path property value.
		/// </summary>
		private string _path;

		/// <summary>
		/// Gets the path to be searched for Rule Manifest Files.
		/// </summary>
		[XmlAttribute]
		public string Path
		{
			get { return _path; }
			set
			{
				if (Directory.Exists(value))
				{
					_path = System.IO.Path.GetFullPath(value);
				}
				else
				{
					_path = null;
					for(int idx = 0; ((_path == null) && (idx < BasePaths.Count)); idx++)
					{
						if (!string.IsNullOrEmpty(BasePaths[idx]))
						{
							_path = IOExtensions.CombinePath(BasePaths[idx], value);
							if (!Directory.Exists(_path)) _path = null;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the search pattern used to find Rule Manifest Files.
		/// </summary>
		[XmlAttribute]
		public string SearchPattern { get; set; }

		/// <summary>
		/// Gets a boolean flag which indicates if sub directories should be searched for Rule Manifest Files.
		/// </summary>
		[XmlAttribute]
		public bool Recursive { get; set; }
	}
}
