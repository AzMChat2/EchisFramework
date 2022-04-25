using System;
using System.Collections.Generic;
using Kontac.Net.SmartCode.Model;
using Kontac.Net.SmartCode.Model.Templates;
using System.Text;

namespace System.Templates
{
	public class FactoryInterface : ProjectTemplate
	{
		public FactoryInterface()
		{
			CreateOutputFile = true;
			Description = "Generates a C# interface for the Business Object Factory";
			Name = "FactoryInterface";
			OutputFolder = "/Interfaces";
		}

		public override string OutputFileName()
		{
			return String.Format("{0}.IFactory.cs", Project.Code);
		}

		public override void ProduceCode()
		{
			StringBuilder impCode = new StringBuilder();
			string domainName = Helper.PascalCase(Project.Code);

			/* Usings and Namespace */
			WriteLine("using System;");
			WriteLine(string.Empty);
			WriteLine("namespace {0}", Project.Code);
			WriteLine("{");

			WriteLine("\t/// <summary>");
			WriteLine("\t/// Provides factory methods for creating new {0} objects", domainName);
			WriteLine("\t/// </summary>");
			WriteLine("\tpublic interface IFactory");
			WriteLine("\t{");

			foreach (TableSchema table in Project.DatabaseSchema.Tables)
			{
				string objectName = Helper.PascalCase(Helper.MakeSingle(table.Code));

				WriteLine("\t\t/// <summary>");
				WriteLine("\t\t/// Creates a new {0} Business Object", objectName);
				WriteLine("\t\t/// </summary>");
				WriteLine("\t\tI{0} New{0}();", objectName);
				WriteLine();

				StringBuilder pk = new StringBuilder();

				IList<ColumnSchema> primaryKeys = table.PrimaryKeyColumns();
				if (primaryKeys.Count != 0)
				{
					pk.AppendLine("\t\t/// <summary>");
					pk.AppendFormat("\t\t/// Retrieves an existing {0} Business Object by Primary Key{1}", objectName, Environment.NewLine);
					pk.AppendLine("\t\t/// </summary>");

					pk.AppendFormat("\t\t{{0}}I{0} Get{0}(", objectName);

					string end = ", ";
					for (int idx = 0; idx < primaryKeys.Count; idx++)
					{
						ColumnSchema column = primaryKeys[idx];
						if ((idx + 1) >= primaryKeys.Count) end = "){1}";
						pk.AppendFormat("{0} {1}{2}", Helper.SimpleNetType(column), Helper.CamelCase(column.Code), end);
					}

					WriteLine(pk.ToString(), string.Empty, ";");
					WriteLine();
				}

				WriteLine("\t\t/// <summary>");
				WriteLine("\t\t/// Creates a new {0} Business Object Collection", objectName);
				WriteLine("\t\t/// </summary>");
				WriteLine("\t\tI{0}Collection New{0}Collection();", objectName);
				WriteLine();

				impCode.AppendLine("\t\t/// <summary>");
				impCode.AppendFormat("\t\t/// Creates a new {0} Business Object", objectName);
				impCode.AppendLine();
				impCode.AppendLine("\t\t/// </summary>");
				impCode.AppendFormat("\t\tpublic I{0} New{0}() {{ return new {0}(); }}", objectName);
				impCode.AppendLine();

				if (pk.Length != 0)
				{
					impCode.AppendFormat(pk.ToString(), "public ", Environment.NewLine);
					impCode.AppendLine("\t\t{");
					impCode.AppendLine("#warning Not Implemented");
					impCode.AppendLine("\t\t\tthrow new NotImplementedException();");
					impCode.AppendLine("\t\t}");
					impCode.AppendLine();
					impCode.AppendLine();
				}

				impCode.AppendLine();
				impCode.AppendLine("\t\t/// <summary>");
				impCode.AppendFormat("\t\t/// Creates a new {0} Business Object Collection", objectName);
				impCode.AppendLine();
				impCode.AppendLine("\t\t/// </summary>");
				impCode.AppendFormat("\t\tpublic I{0}Collection New{0}Collection() {{ return new {0}Collection(); }}", objectName);
				impCode.AppendLine();
				impCode.AppendLine();
			}

			/* Interface */
			WriteLine("\t}");
			WriteLine();

			WriteLine("\t/*  The following code snippet is provided for you to copy into the Domain Factory object:");
			WriteLine();
			WriteLine(impCode.ToString());
			WriteLine("\t*/");

			/* Namespace */
			WriteLine("}");
		}
	}
}
