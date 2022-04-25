using System;
using System.Collections.Generic;
using Kontac.Net.SmartCode.Model;
using Kontac.Net.SmartCode.Model.Templates;

namespace System.Templates
{
	public class DataInterfaceGenerated : EntityTemplate
	{
		public DataInterfaceGenerated()
		{
			CreateOutputFile = true;
			Description = "Generates a C# class for the Data Interface";
			Name = "DataInterface.Generated";
			OutputFolder = "/Interfaces";
		}

		public override string OutputFileName()
		{
			return String.Format("I{0}.generated.cs", Helper.PascalCase(Helper.MakeSingle(Entity.Code)));
		}

		public override void ProduceCode()
		{
			string objectName = Helper.PascalCase(Helper.MakeSingle(Entity.Code));

			/* Usings and Namespace */
			WriteLine(Helper.GeneratedFileWarning);
			WriteLine("using System;");
			WriteLine("using System.Diagnostics.CodeAnalysis;");
			WriteLine("using System.Data;");
			WriteLine("using System.Business;");
			WriteLine(string.Empty);
			WriteLine("namespace {0}", Project.Code);
			WriteLine("{");

			/* Business Object Interface */
			WriteLine("\t/// <summary>");
			WriteLine("\t/// Defines a {0} Business Object", objectName);
			WriteLine("\t/// </summary>");
			WriteLine("\tpublic partial interface I{0} : IBusinessObject<I{0}>", objectName);
			WriteLine("\t{");
			foreach (ColumnSchema column in Table.Columns)
			{
				string colName = Helper.PascalCase(column.Code);

				WriteLine("\t\t/// <summary>");
				WriteLine("\t\t/// Gets or sets the value of the {0} property", colName);
				WriteLine("\t\t/// </summary>");
				if (column.IsPrimaryKey) WriteLine("\t\t[PrimaryKey]");
				WriteLine("\t\t{0} {1} {{ get; set; }}", Helper.SimpleNetType(column), colName);
			}
			WriteLine("\t}");
			WriteLine(string.Empty);

			/* Busines Object List Interface */
			WriteLine("\t/// <summary>");
			WriteLine("\t/// Defines a {0} Business Object Collection", objectName);
			WriteLine("\t/// </summary>");
			WriteLine("\tpublic partial interface I{0}Collection : IBusinessObjectCollection<I{0}> {{ }}", objectName);
			WriteLine(string.Empty);

			/* Constants Class */
			WriteLine("\t/// <summary>");
			WriteLine("\t/// Defines constants associated with the {0} Business Object", objectName);
			WriteLine("\t/// </summary>");
			WriteLine("\t[SuppressMessage(\"Microsoft.Design\", \"CA1034:NestedTypesShouldNotBeVisible\",");
			WriteLine("\t\tJustification = \"This is a non-instantiable static class which only contains constants.\")]");
			WriteLine("\tpublic static partial class {0}Constants", objectName);
			WriteLine("\t{");

			/* Constants.PropertyNames */
			WriteLine("\t\t/// <summary>");
			WriteLine("\t\t/// Defines Property Name constants associated with the {0} Business Object", objectName);
			WriteLine("\t\t/// </summary>");
			WriteLine("\t\t[SuppressMessage(\"Microsoft.Design\", \"CA1034:NestedTypesShouldNotBeVisible\",");
			WriteLine("\t\t\tJustification = \"This is a non-instantiable static class which only contains constants.\")]");
			WriteLine("\t\tpublic static partial class PropertyNames");
			WriteLine("\t\t{");
			foreach (ColumnSchema column in Table.Columns)
			{
				string colName = Helper.PascalCase(column.Code);

				WriteLine("\t\t/// <summary>");
				WriteLine("\t\t/// Name of the {0} property", colName);
				WriteLine("\t\t/// </summary>");
				WriteLine("\t\t\tpublic const string {0} = \"{0}\";", colName);
			}
			WriteLine("\t\t}");
			WriteLine(string.Empty);

			/* Constants.StringLengths */
			WriteLine("\t\t/// <summary>");
			WriteLine("\t\t/// Defines Property String Length constants associated with the {0} Business Object", objectName);
			WriteLine("\t\t/// </summary>");
			WriteLine("\t\t[SuppressMessage(\"Microsoft.Design\", \"CA1034:NestedTypesShouldNotBeVisible\",");
			WriteLine("\t\t\tJustification = \"This is a non-instantiable static class which only contains constants.\")]");
			WriteLine("\t\tpublic static partial class StringLengths");
			WriteLine("\t\t{");
			foreach (ColumnSchema column in Table.Columns)
			{
				string colName = Helper.PascalCase(column.Code);

				if (Helper.SimpleNetType(column) == "string")
				{
					WriteLine("\t\t/// <summary>");
					WriteLine("\t\t/// Maximum length of the {0} property", colName);
					WriteLine("\t\t/// </summary>");
					WriteLine("\t\t\tpublic const int {0} = {1};", colName, column.Length);
				}
			}
			WriteLine("\t\t}");

			/* Constants Class */
			WriteLine("\t}");

			/* namespace */
			WriteLine("}"); 
		}
	}
}
