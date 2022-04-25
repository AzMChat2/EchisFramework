using System;
using System.Collections.Generic;
using Kontac.Net.SmartCode.Model;
using Kontac.Net.SmartCode.Model.Templates;
using System.IO;
using System.Xml;
using System.Text;

namespace System.Templates
{
	public class BusinessObjectRules : EntityTemplate
	{
		public BusinessObjectRules()
		{
			CreateOutputFile = true;
			Description = "Generates an Xml file for the Business Object Rules";
			Name = "BusinessObjectRules";
			OutputFolder = "/Business";
		}

		public override string OutputFileName()
		{
			return String.Format("{0}.Rules.xml", Helper.PascalCase(Helper.MakeSingle(Entity.Code)));
		}

		public override void ProduceCode()
		{
			string objectName = Helper.PascalCase(Helper.MakeSingle(Entity.Code));

			using (MemoryStream stream = new MemoryStream())
			{
				using (XmlTextWriter writer = new XmlTextWriter(stream, ASCIIEncoding.ASCII))
				{
					writer.Formatting = Formatting.Indented;
					writer.WriteStartDocument();
					writer.WriteStartElement("Rulesets");
					writer.WriteStartElement("Ruleset");

					writer.WriteStartElement("Domain");
					writer.WriteAttributeString("DomainId", objectName);

					foreach (ColumnSchema column in Table.Columns)
					{
						if (Helper.SimpleNetType(column) == "string")
						{
							string pascalName = Helper.PascalCase(column.Code);

							writer.WriteStartElement("Property");
							writer.WriteAttributeString("Name", pascalName);

							writer.WriteStartElement("PropertyRules");
							
							if (column.IsRequired)
							{
								/* String Not Null Rule */
								writer.WriteStartElement("Add");
								writer.WriteAttributeString("RuleId", "StringNotNullRule");
								writer.WriteAttributeString("Type", "System.Business.Rules.StringNotNullRule, System.Business");
								writer.WriteEndElement(); // Add

								/* String Not Empty Rule */
								writer.WriteStartElement("Add");
								writer.WriteAttributeString("RuleId", "StringNotEmptyRule");
								writer.WriteAttributeString("Type", "System.Business.Rules.StringNotEmptyRule, System.Business");
								writer.WriteEndElement(); // Add
							}

							/* String Length Rule */
							if (column.Length != 0)
							{
								writer.WriteStartElement("Add");
								writer.WriteAttributeString("RuleId", "StringLengthRule");
								writer.WriteAttributeString("Type", "System.Business.Rules.StringLengthRule, System.Business");
								writer.WriteStartElement("Parameter");
								writer.WriteAttributeString("Name", "MaxLength");
								writer.WriteAttributeString("Value", column.Length.ToString());
								writer.WriteEndElement(); // Parameter
								writer.WriteEndElement(); // Add
							}

							writer.WriteEndElement(); // PropertyRules
							writer.WriteEndElement(); // Property
						}
					}

					writer.WriteEndElement(); // Domain
					writer.WriteEndElement(); // Ruleset
					writer.WriteEndElement(); // Rulesets
					writer.WriteEndDocument();

					writer.Flush();
					writer.Close();
				}

				WriteLine(ASCIIEncoding.ASCII.GetString(stream.ToArray()));
			}
		}
	}
}
