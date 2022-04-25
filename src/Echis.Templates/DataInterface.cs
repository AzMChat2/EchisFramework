using System;
using System.Collections.Generic;
using Kontac.Net.SmartCode.Model;
using Kontac.Net.SmartCode.Model.Templates;

namespace System.Templates
{
	public class DataInterface : EntityTemplate
	{
		public DataInterface()
		{
			CreateOutputFile = true;
			Description = "Generates a C# class for the Data Interface";
			Name = "DataInterface";
			OutputFolder = "/Interfaces";
		}

		public override string OutputFileName()
		{
			return String.Format("I{0}.cs", Helper.PascalCase(Helper.MakeSingle(Entity.Code)));
		}

		public override void ProduceCode()
		{
			string objectName = Helper.PascalCase(Helper.MakeSingle(Entity.Code));

			/* Usings and Namespace */
			WriteLine("using System;");
			WriteLine(string.Empty);
			WriteLine("namespace {0}", Project.Code);
			WriteLine("{");

			/* Business Object Interface */
			WriteLine("\tpublic partial interface I{0}", objectName);
			WriteLine("\t{");
			WriteLine(string.Empty);
			WriteLine("\t}");
			WriteLine(string.Empty);

			/* Busines Object List Interface */
			WriteLine("\tpublic partial interface I{0}Collection", objectName);
			WriteLine("\t{");
			WriteLine(string.Empty);
			WriteLine("\t}");
			WriteLine(string.Empty);

			/* Constants Class */
			WriteLine("\tpublic static partial class {0}Constants", objectName);
			WriteLine("\t{");

			/* Constants.PropertyNames */
			WriteLine("\t\tpublic static partial class PropertyNames");
			WriteLine("\t\t{");
			WriteLine(string.Empty);
			WriteLine("\t\t}");
			WriteLine(string.Empty);

			/* Constants.StringLengths */
			WriteLine("\t\tpublic static partial class StringLengths");
			WriteLine("\t\t{");
			WriteLine(string.Empty);
			WriteLine("\t\t}");

			/* Constants Class */
			WriteLine("\t}");

			/* namespace */
			WriteLine("}");
		}
	}
}
