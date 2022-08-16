using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class CellTypes
	{
		static CellTypes()
		{
			CellTypes.RegisterPair(CellTypes.Boolean, CellValueType.Boolean);
			CellTypes.RegisterPair(CellTypes.Error, CellValueType.Error);
			CellTypes.RegisterPair(CellTypes.InlineString, CellValueType.RichText);
			CellTypes.RegisterPair(CellTypes.Number, CellValueType.Number);
			CellTypes.nameToCellValueType.Add(CellTypes.String, CellValueType.Text);
			CellTypes.cellValueTypeToName.Add(CellValueType.Text, CellTypes.SharedString);
		}

		public static string GetCellTypeName(CellValueType valueType)
		{
			return CellTypes.cellValueTypeToName[valueType];
		}

		public static CellValueType GetCellValueType(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return CellTypes.nameToCellValueType[name];
		}

		static void RegisterPair(string name, CellValueType value)
		{
			CellTypes.nameToCellValueType.Add(name, value);
			CellTypes.cellValueTypeToName.Add(value, name);
		}

		public static readonly string Boolean = "b";

		public static readonly string Error = "e";

		public static readonly string InlineString = "inlineStr";

		public static readonly string Number = "n";

		public static readonly string SharedString = "s";

		public static readonly string String = "str";

		public static readonly string Date = "d";

		static readonly Dictionary<string, CellValueType> nameToCellValueType = new Dictionary<string, CellValueType>();

		static readonly Dictionary<CellValueType, string> cellValueTypeToName = new Dictionary<CellValueType, string>();
	}
}
