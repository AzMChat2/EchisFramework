using System;
using System.Collections.Generic;
using Kontac.Net.SmartCode.Model;
using Kontac.Net.SmartCode.Model.Templates;

namespace System.Templates
{
	public class DataObject : EntityTemplate
	{
		public DataObject()
		{
			CreateOutputFile = true;
			Description = "Generates a C# class for the Data Object";
			Name = "DataObject";
			OutputFolder = "/Data";
		}

		public override string OutputFileName()
		{
			return String.Format("{0}Base.cs", Helper.PascalCase(Helper.MakeSingle(Entity.Code)));
		}

		public override void ProduceCode()
		{
			string objectName = Helper.PascalCase(Helper.MakeSingle(Entity.Code));

			/*  Usings and Namespace */
			WriteLine("using System;");
			WriteLine();
			WriteLine("namespace {0}", Project.Code);
			WriteLine("{");

			/* Business Object Base class */
			WriteLine("\tpublic abstract partial class {0}Base", objectName);
			WriteLine("\t{");
			WriteLine();

			/* Constructor */
			WriteLine("\t\t/// <summary>");
			WriteLine("\t\t/// Default Constructor.");
			WriteLine("\t\t/// </summary>");
			WriteLine("\t\tprotected {0}Base()", objectName);
			WriteLine("\t\t{");
			WriteLine("\t\t}");
			WriteLine();
			WriteLine("\t}");
			WriteLine();

			/* Business Object List Base class */
			WriteLine("\tpublic abstract partial class {0}CollectionBase<T>", objectName);
			WriteLine("\t{");
			WriteLine(string.Empty);
			WriteLine("\t}");

			/* Namespace */
			WriteLine("}");
		}
	}
}
