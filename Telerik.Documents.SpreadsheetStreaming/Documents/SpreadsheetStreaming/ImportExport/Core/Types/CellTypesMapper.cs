using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class CellTypesMapper
	{
		static CellTypesMapper()
		{
			CellTypesMapper.RegisterPair(CellTypesMapper.Boolean, CellValueType.Boolean);
			CellTypesMapper.RegisterPair(CellTypesMapper.Error, CellValueType.Error);
			CellTypesMapper.RegisterPair(CellTypesMapper.InlineString, CellValueType.RichText);
			CellTypesMapper.RegisterPair(CellTypesMapper.Number, CellValueType.Number);
			CellTypesMapper.RegisterPair(CellTypesMapper.Date, CellValueType.Date);
			CellTypesMapper.RegisterPair(CellTypesMapper.String, CellValueType.Text);
		}

		public static string GetCellTypeName(CellValueType valueType)
		{
			return CellTypesMapper.cellValueTypeToName[valueType];
		}

		public static CellValueType GetCellValueType(string name)
		{
			return CellTypesMapper.nameToCellValueType[name];
		}

		static void RegisterPair(string name, CellValueType value)
		{
			CellTypesMapper.nameToCellValueType.Add(name, value);
			CellTypesMapper.cellValueTypeToName.Add(value, name);
		}

		public static readonly string Boolean = "b";

		public static readonly string Error = "e";

		public static readonly string InlineString = "inlineStr";

		public static readonly string Number = "n";

		public static readonly string Date = "d";

		public static readonly string String = "str";

		static readonly Dictionary<string, CellValueType> nameToCellValueType = new Dictionary<string, CellValueType>();

		static readonly Dictionary<CellValueType, string> cellValueTypeToName = new Dictionary<CellValueType, string>();
	}
}
