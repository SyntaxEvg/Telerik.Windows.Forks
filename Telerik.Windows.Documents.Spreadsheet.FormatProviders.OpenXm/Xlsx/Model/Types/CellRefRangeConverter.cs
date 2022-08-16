using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class CellRefRangeConverter : IStringConverter<CellRefRange>
	{
		public CellRefRange ConvertFromString(string value)
		{
			CellRange range;
			NameConverter.ConvertCellNameToCellRange(value, out range);
			return new CellRefRange(range);
		}

		public string ConvertToString(CellRefRange value)
		{
			CellIndex cellIndex = value.FromCell.ToCellIndex();
			CellIndex cellIndex2 = value.ToCell.ToCellIndex();
			string result = string.Empty;
			if (cellIndex == cellIndex2)
			{
				result = value.FromCell.ToString();
			}
			else
			{
				result = NameConverter.ConvertCellRangeToName(cellIndex, cellIndex2);
			}
			return result;
		}
	}
}
