using System;
using System.Collections.Generic;
using Kontac.Net.SmartCode.Model;
using Kontac.Net.SmartCode.Model.Templates;

namespace System.Templates
{
	public class DataObjectGenerated : EntityTemplate
	{
		public DataObjectGenerated()
		{
			CreateOutputFile = true;
			Description = "Generates a C# class for the Data Object";
			Name = "DataObject.Generated";
			OutputFolder = "/Data";
		}

		public override string OutputFileName()
		{
			return String.Format("{0}Base.generated.cs", Helper.PascalCase(Helper.MakeSingle(Entity.Code)));
		}

		public override void ProduceCode()
		{
			string objectName = Helper.PascalCase(Helper.MakeSingle(Entity.Code));
			string domainName = Helper.PascalCase(Project.Code);

			// This will be used later on in the constructor.
			string properties = null;
			foreach (ColumnSchema column in Table.Columns)
			{
				if (properties == null)
				{
					properties = string.Format("{0}Property", Helper.PascalCase(column.Code));
				}
				else
				{
					properties += string.Format(",\r\n\t\t\t\t{0}Property", Helper.PascalCase(column.Code));
				}
			}

			/* Usings and Namespace */
			WriteLine(Helper.GeneratedFileWarning);
			WriteLine("using System;");
			WriteLine("using System.Diagnostics.CodeAnalysis;");
			WriteLine("using System.Business;");
			WriteLine();
			WriteLine("namespace {0}", Project.Code);
			WriteLine("{");

			/* Business Object Base class */
			WriteLine("\t/// <summary>");
			WriteLine("\t/// Represents a base class from which a {0} Business Object can be derived.", objectName);
			WriteLine("\t/// </summary>");
			WriteLine("\tpublic abstract partial class {0}Base : BusinessObjectBase<I{0}>, I{0}", objectName);
			WriteLine("\t{");

			/* Initialize Properties */
			WriteLine("\t\t/// <summary>");
			WriteLine("\t\t/// Initializes the underlying property objects");
			WriteLine("\t\t/// </summary>");
			WriteLine("\t\tprotected override void InitializeProperties()");
			WriteLine("\t\t{");

			foreach (ColumnSchema column in Table.Columns)
			{
				string dataType = Helper.SimpleNetType(column);
				string colName = Helper.PascalCase(column.Code);

				if (Helper.IsSpecialType(column))
				{
					string propertyType = Helper.SpecialTypePropertyType(column);
					WriteLine("\t\t\t{0}Property = new {2}({1}Constants.PropertyNames.{0});", colName, objectName, propertyType);
				}
				else
				{
					switch (dataType)
					{
						case "string":
							WriteLine("\t\t\t{0}Property = new StringProperty({1}Constants.PropertyNames.{0});", colName, objectName);
							break;
						case "bool":
							WriteLine("\t\t\t{0}Property = new BooleanProperty({1}Constants.PropertyNames.{0});", colName, objectName);
							break;
						case "bool?":
							WriteLine("\t\t\t{0}Property = new NullableBooleanProperty({1}Constants.PropertyNames.{0});", colName, objectName);
							break;
						case "DateTime":
							WriteLine("\t\t\t{0}Property = new DateTimeProperty({1}Constants.PropertyNames.{0});", colName, objectName);
							break;
						case "DateTime?":
							WriteLine("\t\t\t{0}Property = new NullableDateTimeProperty({1}Constants.PropertyNames.{0});", colName, objectName);
							break;
						default:
							WriteLine("\t\t\t{0}Property = new GenericProperty<{2}>({1}Constants.PropertyNames.{0});", colName, objectName, dataType);
							break;
					}
				}
			}
			WriteLine();
			WriteLine("\t\t\tAddProperties({0});", properties);
			WriteLine("\t\t}"); // InitializeProperties()
			WriteLine();

			/* Properties */
			foreach (ColumnSchema column in Table.Columns)
			{
				string dataType = Helper.SimpleNetType(column);
				string colName = Helper.PascalCase(column.Code);

				WriteLine("\t\t/// <summary>");
				WriteLine("\t\t/// Gets or sets the underlying property object of the {0} Property", colName);
				WriteLine("\t\t/// </summary>");
				WriteLine("\t\tprotected IProperty<{0}> {1}Property {{ get; private set; }}", dataType, colName);
				WriteLine("\t\t/// <summary>");
				WriteLine("\t\t/// Gets or sets the value of the {0} property", colName);
				WriteLine("\t\t/// </summary>");
				WriteLine("\t\tpublic {0} {1}", dataType, colName);
				WriteLine("\t\t{");
				WriteLine("\t\t\tget {{ return {0}Property.Value; }}", colName);
				WriteLine("\t\t\tset {{ {0}Property.Value = value; }}", colName);
				WriteLine("\t\t}");
			}

			/* Business Object Base class */
			WriteLine("\t}");
			WriteLine();

			/* Business Object List Base class */
			WriteLine("\t/// <summary>");
			WriteLine("\t/// Represents a List Base class from which a {0} Business Object Collection can be derived.", objectName);
			WriteLine("\t/// </summary>");
			WriteLine("\t[SuppressMessage(\"Microsoft.Naming\", \"CA1710:IdentifiersShouldHaveCorrectSuffix\", Justification = \"This is an abstract BASE class\")]");
			WriteLine("\tpublic abstract partial class {0}CollectionBase<T> : BusinessObjectCollection<I{0}>, I{0}Collection", objectName);
			WriteLine("\t\twhere T : {0}Base, new()", objectName);
			WriteLine("\t{");
			WriteLine();
			WriteLine("\t\t/// <summary>");
			WriteLine("\t\t/// Creates and returns a new object which implents the I{0} interface", objectName);
			WriteLine("\t\t/// </summary>");
			WriteLine("\t\tprotected override I{0} CreateNewItem()", objectName);
			WriteLine("\t\t{");
			WriteLine("\t\t\treturn new T();");
			WriteLine("\t\t}");
			WriteLine();
			WriteLine("\t\t/// <summary>");
			WriteLine("\t\t/// Gets the name of the Domain or Business object.");
			WriteLine("\t\t/// </summary>");
			WriteLine("\t\tprotected override string DomainName");
			WriteLine("\t\t{");
			WriteLine("\t\t\tget { return typeof(T).Name; }");
			WriteLine("\t\t}");
			WriteLine();
			WriteLine("\t}");

			/* Namespace */
			WriteLine("}");
		}
	}
}
