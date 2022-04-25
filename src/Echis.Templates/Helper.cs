using System.Text.RegularExpressions;
using Kontac.Net.SmartCode.Model;
using System;
using System.Text;

namespace System.Templates
{
	public class Helper
	{
		private static Regex cleanRegEx;
		static Helper()
		{
			cleanRegEx = new Regex(@"\s+|_|-|\.", RegexOptions.Compiled);
		}


		public static string ClassName(string name)
		{
			return MakeSingle(name);
		}

		public static string CleanName(string name)
		{
			return cleanRegEx.Replace(name, "");
		}

		public static string CamelCase(string name)
		{
			string output = CleanName(name);
			return char.ToLower(output[0]) + output.Substring(1);
		}

		public static string PascalCase(string name)
		{
			string output = CleanName(name);
			return char.ToUpper(output[0]) + output.Substring(1);
		}

		public static string MakePlural(string name)
		{
			Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
			Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
			Regex plural3 = new Regex("(?<keep>[sxzh])$");
			Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

			if (plural1.IsMatch(name))
				return plural1.Replace(name, "${keep}ies");
			else if (plural2.IsMatch(name))
				return plural2.Replace(name, "${keep}s");
			else if (plural3.IsMatch(name))
				return plural3.Replace(name, "${keep}es");
			else if (plural4.IsMatch(name))
				return plural4.Replace(name, "${keep}s");

			return name;
		}

		public static string MakeSingle(string name)
		{
			Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
			Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
			Regex plural3 = new Regex("(?<keep>[sxzh])es$");
			Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

			if (plural1.IsMatch(name))
				return plural1.Replace(name, "${keep}y");
			else if (plural2.IsMatch(name))
				return plural2.Replace(name, "${keep}");
			else if (plural3.IsMatch(name))
				return plural3.Replace(name, "${keep}");
			else if (plural4.IsMatch(name))
				return plural4.Replace(name, "${keep}");

			return name;
		}

		public static bool IsManyToManyTable(TableSchema table)
		{
			return (table.Columns.Count == 2 && table.HasPrimaryKey() && table.PrimaryKeyColumns().Count == 2 && table.InReferences.Count == 2);
		}

		public static bool IsChildFKColumn(ColumnSchema column, TableSchema table)
		{
			foreach (ReferenceSchema inReference in table.InReferences)
			{
				foreach (ReferenceJoin join in inReference.Joins)
				{
					//Found the child Column...
					if (join.ChildColumn.ObjectID == column.ObjectID)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsNullableType(ColumnSchema column)
		{
			return (!column.IsRequired && column.NetDataType != "System.String");
		}

		public static string SimpleNetType(ColumnSchema column)
		{
			string retVal = null;
			bool isNullable = false;

			if (IsSpecialType(column))
			{
				string dataType = SpecialTypeDataType(column);
				if (dataType.EndsWith("?"))
				{
					isNullable = true;
					retVal = dataType.Substring(0, (dataType.Length - 1));
				}
				else
				{
					retVal = dataType;
				}
			}
			else
			{
				retVal = GetBasicType(column.NetDataType);
				isNullable = Helper.IsNullableType(column);
			}

			if (isNullable)
			{
				retVal += "?";
			}

			return retVal;
		}

		private static string GetBasicType(string dataType)
		{
			string retVal = dataType.Replace("System.", string.Empty);

			switch (retVal)
			{
				case "Int16":
					retVal = "short";
					break;
				case "Int32":
					retVal = "int";
					break;
				case "Int64":
					retVal = "long";
					break;
				case "Boolean":
					retVal = "bool";
					break;
				case "Guid":
				case "DateTime":
					// Do nothing, case is correct
					break;
				default:
					retVal = retVal.ToLowerInvariant();
					break;
			}

			return retVal;
		}

		public static string SpecialTypeDataType(ColumnSchema column)
		{
			string retVal = null;

			if (IsSpecialType(column))
			{
				string[] parts = column.NetDataType.Split(new string[] {"::"}, StringSplitOptions.None);
				retVal = parts[0].Trim();
			}

			return retVal;
		}

		public static string SpecialTypePropertyType(ColumnSchema column)
		{
			string retVal = null;

			if (IsSpecialType(column))
			{
				string[] parts = column.NetDataType.Split(new string[] {"::"}, StringSplitOptions.None);
				retVal = parts[1].Trim();
			}

			return retVal;
		}

		public static bool IsSpecialType(ColumnSchema column)
		{
			return column.NetDataType.Contains("::");
		}

		public static string GeneratedFileWarning
		{
			get
			{
				return @"// Warning this file is a generated file.
// Any changes you make to this file will be discarded during the next generation of this file.";
			}
		}

		public static string GenerateNamespace(NamespaceTypes namespaceType, string domainCode)
		{
			StringBuilder retVal = new StringBuilder();
			string[] parts = domainCode.Split(new string[] { "::", "." }, StringSplitOptions.RemoveEmptyEntries);

			retVal.Append(PascalCase(parts[0]));

			switch (namespaceType)
			{
				case NamespaceTypes.DataInterface:
					retVal.Append(".Interfaces.Data");
					break;
				case NamespaceTypes.DataObject:
					retVal.Append(".Data");
					break;
				case NamespaceTypes.BusinessObject:
					retVal.Append(".Business");
					break;
			}

			if (parts.Length > 1)
			{
				for (int idx = 1; idx < parts.Length; idx++)
				{
					retVal.AppendFormat(".{0}", PascalCase(parts[idx]));
				}
			}

			return retVal.ToString();
		}
	}

	public enum NamespaceTypes
	{
		DataInterface,
		DataObject,
		BusinessObject
	}
}
