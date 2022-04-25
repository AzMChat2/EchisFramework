using System;
using System.Collections.Generic;
using System.Text;
using Kontac.Net.SmartCode.Model.Templates;
using Kontac.Net.SmartCode.Model;

namespace System.Templates
{
	public class ExtJSModel: EntityTemplate
	{
		public ExtJSModel()
		{
			CreateOutputFile = true;
			Description = "Generates a javascript file for the ExtJS 4.0 MVC Model";
			Name = "ExtJS.Model";
			OutputFolder = "/ExtJS/model";
		}

		public override string OutputFileName()
		{
			return String.Format("{0}.js", Helper.PascalCase(Helper.MakeSingle(Entity.Code)));
		}

		public override void ProduceCode()
		{
			string objectName = Helper.PascalCase(Helper.MakeSingle(Entity.Code));

			/* Usings and Namespace */
			WriteLine(Helper.GeneratedFileWarning);

			/* ExtJS Model */
			WriteLine("// ExtJS Model (see http://docs.sencha.com/ext-js/4-0/#!/guide/application_architecture for guidance).");
			WriteLine("///<reference path=\"~/Scripts/Common/ExtJS/builds/ext-core-debug.js\" />");
			WriteLine(string.Empty);
			WriteLine("Ext.define('{0}.model.{1}', {{", Entity.Comment, objectName);
			WriteLine("\textend: 'Ext.data.Model',");

			IList<ColumnSchema> pkColumns = Table.PrimaryKeyColumns();
			if (pkColumns.Count == 1)
			{
				WriteLine("\tidProperty: '{0}',", Helper.PascalCase(pkColumns[0].Code));
			}

			WriteLine("\tfields: [");

			string end = ",";

			for(int idx = 0; idx < Table.Columns.Count; idx++)
			{
				if ((idx + 1) >= Table.Columns.Count) end = string.Empty;
				WriteLine("\t\t'{0}'{1}", Helper.PascalCase(Table.Columns[idx].Code), end);
			}

			WriteLine("\t]");
			WriteLine("});");
			WriteLine(string.Empty);

		}
	}
}

