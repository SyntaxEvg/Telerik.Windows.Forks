using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class CellRefConverter : IStringConverter<CellRef>
	{
		public CellRef ConvertFromString(string value)
		{
			int rowIndex;
			int columnIndex;
			NameConverter.ConvertCellNameToIndex(value, out rowIndex, out columnIndex);
			return new CellRef(rowIndex, columnIndex);
		}

		public string ConvertToString(CellRef value)
		{
			return NameConverter.ConvertCellIndexToName(value.RowIndex, value.ColumnIndex);
		}
	}
}
