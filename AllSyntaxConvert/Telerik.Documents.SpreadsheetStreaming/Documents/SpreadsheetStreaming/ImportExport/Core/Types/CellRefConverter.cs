using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
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
