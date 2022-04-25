using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using SpringHandler = Spring.Context.Support.ContextHandler;

namespace Echis.Spring
{
	/// <summary>
	/// This class allows the Spring Context configuration be stored in a separate file from the Application Configuration.
	/// </summary>
	public class SpringSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Used to read the Spring Context configuration from a configuration file.
		/// </summary>
		/// <param name="parent">Parent node</param>
		/// <param name="configContext">Configuration Context</param>
		/// <param name="section">Section to read.</param>
		/// <returns>Returns a Spring Context configuration depending upon the Context Handler Type defined (or the Spring.Context.Support.ContextHandler by default).</returns>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
			MessageId = "SpringSectionHandler",
			Justification = "SpringSectionHandler is the name of a class.")]
		public object Create(object parent, object configContext, XmlNode section)
		{
			if (section == null) throw new ArgumentNullException("section");

			string fileName = (section.Attributes["Filename"] == null) ? null : section.Attributes["Filename"].Value;
			string type = (section.Attributes["Type"] == null) ? null : section.Attributes["Type"].Value;

			if (string.IsNullOrEmpty(fileName))
			{
				throw new ConfigurationErrorsException("SpringSectionHandler: Required attribute 'Filename' is missing or empty");
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			XmlNode springNode = doc.ChildNodes[1];

			// The Spring caller doesn't actually do anything with the results, but we'll return them anyway.
			List<object> retVal = new List<object>();

			IConfigurationSectionHandler handler = (type == null) ? new SpringHandler() : ReflectionHelper.CreateObjectUnsafe<IConfigurationSectionHandler>(type);

			foreach (XmlNode contextNode in springNode.ChildNodes)
			{
				retVal.Add(handler.Create(parent, configContext, contextNode));
			}

			return retVal.ToArray();
		}
	}
}
