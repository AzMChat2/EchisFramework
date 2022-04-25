using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Business.Rules;
using System.Data.Objects;

namespace System.Business.Configuration
{
	/// <summary>
	/// Loads Rules from Xml Data Files
	/// </summary>
	public sealed class XmlRuleLoader : IRuleLoader
	{
		/// <summary>
		/// Stores the Ruleset containing rule definitions
		/// </summary>
		private static RulesetCollection _rulesets = LoadRulesets();

		#region Inheritance
		/// <summary>
		/// Processes inheritance from one domain to another
		/// </summary>
		private static void ProcessDomainInheritance(DomainInfo domain, Ruleset ruleset)
		{
			if (!string.IsNullOrEmpty(domain.Inherits))
			{
				DomainInfo parentDomain = ruleset.Domains.Find(item => item.DomainId.Equals(domain.Inherits, StringComparison.OrdinalIgnoreCase));
				
				if (parentDomain == null)
				{
					TS.Logger.WriteLineIf(TS.EC.TraceWarning, TS.Categories.Warning, "Unable to process Domain inheritance.  The Ruleset context '{0}' does not contain Domain '{1}.", ruleset.ContextId, domain.Inherits);
				}
				else
				{
					TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Processing domain inheritance for {0} from {1} for context {2}.", domain.DomainId, parentDomain.DomainId, ruleset.ContextId);
					InheritDomain(domain, parentDomain);
				}

				domain.Inherits = null;
			}
		}

		/// <summary>
		/// Processes inheritance from one ruleset to another.
		/// </summary>
		private static void ProcessRulesetInheritance(Ruleset ruleset)
		{
			if (!string.IsNullOrEmpty(ruleset.Inherits))
			{
				Ruleset parent = _rulesets.Find(item => item.ContextId.Equals(ruleset.Inherits, StringComparison.OrdinalIgnoreCase));

				if (parent == null)
				{
					TS.Logger.WriteLineIf(TS.EC.TraceWarning, TS.Categories.Warning, "Unable to process Ruleset inheritance.  The Ruleset context '{0}' does not exist.", ruleset.Inherits);
				}
				else
				{
					TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Processing Ruleset inheritance for context {0} from context {1}.", ruleset.ContextId, parent.ContextId);
					// Need to resolve parent's inheritance first
					if (!string.IsNullOrEmpty(parent.Inherits)) ProcessRulesetInheritance(parent);
					InheritDomains(ruleset, parent);
				}

				ruleset.Inherits = null;
			}
		}

		/// <summary>
		/// Processes inheritance from one domain to another
		/// </summary>
		private static void InheritDomains(Ruleset ruleset, Ruleset parent)
		{
			parent.Domains.ForEach(parentDomain => InheritDomain(ruleset, parentDomain));
		}

		/// <summary>
		/// Processes inheritance from one domain to another
		/// </summary>
		private static void InheritDomain(Ruleset ruleset, DomainInfo parentDomain)
		{
			DomainInfo childDomain = ruleset.Domains.Find(domain => domain.DomainId.Equals(parentDomain.DomainId, StringComparison.OrdinalIgnoreCase));
			if (childDomain == null)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Adding Domain {0} to Ruleset context {1}", parentDomain.DomainId, ruleset.ContextId);
				ruleset.Domains.Add(parentDomain);
			}
			else
			{
				TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Updating Domain {0}", childDomain.DomainId);
				InheritDomain(childDomain, parentDomain);
			}
		}

		/// <summary>
		/// Processes inheritance from one domain to another
		/// </summary>
		private static void InheritDomain(DomainInfo childDomain, DomainInfo parentDomain)
		{
			parentDomain.CollectionRules.AddRules.ForEach(childDomain.CollectionRules.AddRules.Add);
			parentDomain.CollectionRules.RemoveRules.ForEach(childDomain.CollectionRules.RemoveRules.Add);

			parentDomain.ObjectRules.AddRules.ForEach(childDomain.CollectionRules.AddRules.Add);
			parentDomain.ObjectRules.RemoveRules.ForEach(childDomain.CollectionRules.RemoveRules.Add);

			parentDomain.Properties.ForEach(parentProperty => InheritProperty(childDomain, parentProperty));
		}

		/// <summary>
		/// Processes inheritance from one domain to another
		/// </summary>
		private static void InheritProperty(DomainInfo childDomain, PropertyInfo parentProperty)
		{
			PropertyInfo childProperty = childDomain.Properties.Find(property => property.Name.Equals(parentProperty.Name, StringComparison.OrdinalIgnoreCase));
			if (childProperty == null)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Adding Property {0} to Domain {1}", parentProperty.Name, childDomain.DomainId);
				childDomain.Properties.Add(parentProperty);
			}
			else
			{
				TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Updating Property {0}.{1}", childDomain.DomainId, childProperty.Name);
				parentProperty.PropertyRules.AddRules.ForEach(childProperty.PropertyRules.AddRules.Add);
				parentProperty.PropertyRules.RemoveRules.ForEach(childProperty.PropertyRules.RemoveRules.Add);
			}
		}

		#endregion

		#region Loading Rulesets
		/// <summary>
		/// Loads the Rulesets from Xml Files found in the configured location(s).
		/// </summary>
		/// <returns></returns>
		private static RulesetCollection LoadRulesets()
		{
			DateTime __methodStart = DateTime.Now;

			try
			{
				TS.Logger.WriteLineIf(TS.EC.TraceInfo, TS.Categories.Event, "Loading Rulesets");
				RulesetCollection retVal = new RulesetCollection();
				LoadFromManifest(retVal, Settings.Values.PrimaryManifest);
				return retVal;
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				throw;
			}
			finally
			{
				TS.Logger.WritePerformanceIf(TS.EC.TraceInfo, __methodStart);
			}
		}

		/// <summary>
		/// Loads the rulsets from the specified manifest.
		/// </summary>
		/// <param name="rulesets">The ruleset to be loaded.</param>
		/// <param name="manifest">The manifest containing Xml file locations.</param>
		private static void LoadFromManifest(RulesetCollection rulesets, RuleManifest manifest)
		{
			manifest.RulesetLocations.ForEach(searchLocation => LoadFromFiles(rulesets, searchLocation));
			manifest.ManifestLocations.ForEach(searchLocation => LoadFromManifests(rulesets, searchLocation));
		}

		/// <summary>
		/// Loads the rulesets from manifest files found in the specified location
		/// </summary>
		/// <param name="rulesets">The ruleset to be loaded.</param>
		/// <param name="searchLocation">The location to search for manifest files.</param>
		private static void LoadFromManifests(RulesetCollection rulesets, SearchLocation searchLocation)
		{
			IOExtensions.GetFiles(searchLocation.Path, searchLocation.SearchPattern, searchLocation.Recursive)
				.ForEach(fileName => LoadFromManifest(rulesets, fileName));
		}

		/// <summary>
		/// Loads the rulesets from the specified manifest file
		/// </summary>
		/// <param name="rulesets">The ruleset to be loaded.</param>
		/// <param name="fileName">The filename of the Manifest file</param>
		private static void LoadFromManifest(RulesetCollection rulesets, string fileName)
		{
			try
			{
				TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Loading rule manifest file '{0}'", fileName);
				LoadFromManifest(rulesets, XmlSerializer<RuleManifest>.DeserializeFromXmlFile(fileName));
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				throw new RuleConfigurationException(ex, "Unable to read manifest file '{0}': {1}", fileName, ex.Message);
			}
		}

		/// <summary>
		/// Loads the rulesets from the ruleset files found in the specified search location.
		/// </summary>
		/// <param name="rulesets">The ruleset to be loaded.</param>
		/// <param name="searchLocation">The location to search for ruleset files.</param>
		private static void LoadFromFiles(RulesetCollection rulesets, SearchLocation searchLocation)
		{
			IOExtensions.GetFiles(searchLocation.Path, searchLocation.SearchPattern, searchLocation.Recursive)
				.ForEach(fileName => LoadFromFile(rulesets, fileName));
		}

		/// <summary>
		/// Loads the rulesets from the specified ruleset file
		/// </summary>
		/// <param name="rulesets">The ruleset to be loaded.</param>
		/// <param name="fileName">The filename of the ruleset file</param>
		private static void LoadFromFile(RulesetCollection rulesets, string fileName)
		{
			try
			{
				TS.Logger.WriteLineIf(TS.EC.TraceVerbose, TS.Categories.Event, "Loading ruleset file '{0}'", fileName);
				rulesets.AddRange(XmlSerializer<RulesetCollection>.DeserializeFromXmlFile(fileName));
			}
			catch (Exception ex)
			{
				TS.Logger.WriteExceptionIf(TS.EC.TraceError, ex);
				throw new RuleConfigurationException(ex, "Unable to read ruleset file '{0}': {1}", fileName, ex.Message);
			}
		}
		#endregion

		/// <summary>
		/// Gets the Domain Info object containing rule definitions for the specified Domain and Context
		/// </summary>
		private static DomainInfo GetDomain(string contextId, string domainId)
		{
			DomainInfo retVal = null;

			Ruleset ruleset = _rulesets[contextId];
			if (ruleset != null)
			{
				ProcessRulesetInheritance(ruleset);
				retVal = ruleset.Domains[domainId];
				if (retVal != null)
				{
					ProcessDomainInheritance(retVal, ruleset);
					ProcessRemoves(retVal);
				}
			}

			return retVal;
		}

		/// <summary>
		/// Removes rules from the AddRules collection matching a RemoveRules entry for all rule collections.
		/// </summary>
		private static void ProcessRemoves(DomainInfo domain)
		{
			domain.CollectionRules.ProcessRemoves();
			domain.ObjectRules.ProcessRemoves();
			domain.Properties.ForEach(property => property.PropertyRules.ProcessRemoves());
		}

		/// <summary>
		/// Creates a Rule from the specified Rule Information
		/// </summary>
		private static Rule<T> CreateRule<T>(AddRuleInfo ruleInfo)
		{
			Rule<T> retVal = ReflectionExtensions.CreateObjectUnsafe<Rule<T>>(ruleInfo.RuleType);
			
			if (retVal == null)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceWarning, TS.Categories.Warning, "Unable to create rule of type '{0}'.", ruleInfo.RuleType);
			}
			else
			{
				ruleInfo.Parameters.ForEach(retVal.SetParameter);
			}

			return retVal;
		}

		/// <summary>
		/// Gets the rules for the specified property by context and domain.
		/// </summary>
		/// <typeparam name="T">The type of property.</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		/// <param name="propertyName">The name of the property.</param>
		public RuleCollection<Property<T>> GetPropertyRules<T>(string contextId, string domainId, string propertyName)
		{
			RuleCollection<Property<T>> retVal = new RuleCollection<Property<T>>();

			DomainInfo domain = GetDomain(contextId, domainId);
			PropertyInfo property = (domain == null) ? null : domain.Properties[propertyName];

			if (property != null)
			{
				property.PropertyRules.AddRules.ForEach(ruleInfo => retVal.AddIf(CreateRule<Property<T>>(ruleInfo), item => item != null));
			}

			return retVal;
		}

		/// <summary>
		/// Gets the rules for the specified Business Object
		/// </summary>
		/// <typeparam name="T">The Business Object Type</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		public RuleCollection<T> GetObjectRules<T>(string contextId, string domainId)
		{
			RuleCollection<T> retVal = new RuleCollection<T>();

			DomainInfo domain = GetDomain(contextId, domainId);
			if (domain != null)
			{
				domain.ObjectRules.AddRules.ForEach(ruleInfo => retVal.AddIf(CreateRule<T>(ruleInfo), item => item != null));
			}

			return retVal;
		}

		/// <summary>
		/// Gets the rules for the specified Business Object List
		/// </summary>
		/// <typeparam name="T">The interface type of the Business Object Collection</typeparam>
		/// <param name="contextId">The Context Id of the Ruleset to use.</param>
		/// <param name="domainId">The Domain Id of the Ruleset to use.</param>
		public RuleCollection<T> GetCollectionRules<T>(string contextId, string domainId)
		{
			RuleCollection<T> retVal = new RuleCollection<T>();

			DomainInfo domain = GetDomain(contextId, domainId);
			if (domain != null)
			{
				domain.CollectionRules.AddRules.ForEach(ruleInfo => retVal.AddIf(CreateRule<T>(ruleInfo), item => item != null));
			}

			return retVal;
		}
	}
}
