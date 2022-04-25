using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Objects.Factory.Xml;
using Spring.Objects.Factory.Config;
using System.Xml;
using System.Diagnostics;

namespace Echis.Spring
{
	[NamespaceParser(
		Namespace = "http://www.azchatfield.net/Echis.Framework",
		SchemaLocationAssemblyHint = typeof(XmlSerializedObjectParser),
		SchemaLocation = "/Echis.Spring.Schemas/Objects_XmlSerializedObjectParser.xsd")]
	public class XmlSerializedObjectParser : ObjectsNamespaceParser
	{
		protected override object ParsePropertyValue(XmlElement element, string name, ParserContext parserContext)
		{
			throw new ApplicationException("Test");
		}

		protected override object ParsePropertySubElement(XmlElement element, string name, ParserContext parserContext)
		{
			throw new ApplicationException("Test");
		}
	}
}
