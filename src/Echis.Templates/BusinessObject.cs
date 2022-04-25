using System;
using System.Collections.Generic;
using Kontac.Net.SmartCode.Model;
using Kontac.Net.SmartCode.Model.Templates;

namespace System.Templates
{
	public class BusinessObject : EntityTemplate

	{
		public BusinessObject()
		{
			CreateOutputFile = true;
			Description = "Generates a C# class for the Business Object";
			Name = "BusinessObject";
			OutputFolder = "/Business";
		}

		public override string OutputFileName()
		{
			return String.Format("{0}.cs", Helper.PascalCase(Helper.MakeSingle(Entity.Code)));
		}

		public override void ProduceCode()
		{
			string domainName = Helper.PascalCase(Project.Code);
			string objectName = Helper.PascalCase(Helper.MakeSingle(Entity.Code));

			/* Usings */
			WriteLine("using System;");
			WriteLine("using System.Business.Rules;");
			WriteLine(string.Empty);

			/* Domain Namespace */
			WriteLine("namespace {0}", Project.Code);
			WriteLine("{");

			/* Business Object class */
			WriteLine("\t/// <summary>");
			WriteLine("\t/// Represents a {0} Business Object.", Helper.MakeSingle(Entity.Code));
			WriteLine("\t/// </summary>");
			WriteLine("\tpublic class {0} : {0}Base", objectName);
			WriteLine("\t{");

			/* Business Object class */
			WriteLine("\t}");
			WriteLine(string.Empty);

			/* Business Object List class */
			WriteLine("\t/// <summary>");
			WriteLine("\t/// Represents a {0} Business Object Collection.", objectName);
			WriteLine("\t/// </summary>");
			WriteLine("\tpublic class {0}Collection : {0}CollectionBase<{0}>", objectName);
			WriteLine("\t{");

			/* Business Object List class */
			WriteLine("\t}");
			WriteLine(string.Empty);

			/* Domain Rules Namespace */
			WriteLine("\tnamespace {0}Rules", objectName);
			WriteLine("\t{");

			/* Property Rules region */
			WriteLine("\t\t#region Property Rules");
			WriteLine("\t\t/* Add Custom Property Rule Classes Here */");
			WriteLine("\t\t#endregion");
			WriteLine(string.Empty);

			/* Object Rules Region */
			WriteLine("\t\t#region Object Rules");
			WriteLine("\t\t/* Add Custom Object Rule Classes Here */");
			WriteLine("\t\t#endregion");
			WriteLine(string.Empty);

			/* List Rules Region */
			WriteLine("\t\t#region Collection Rules");
			WriteLine("\t\t/* Add Custom Collection Rule Classes Here */");
			WriteLine("\t\t#endregion");

			/* Domain Rules Namespace */
			WriteLine("\t}");

			/* Domain Namespace */
			WriteLine("}");
		}
	}
}
