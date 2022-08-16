using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class ReferenceCounter
	{
		public void GetCellIndex(CellInfo cellInfo, out int rowIndex, out int columnIndex)
		{
			if (cellInfo.IsCellIndexKnown)
			{
				rowIndex = cellInfo.RowIndex;
				columnIndex = cellInfo.ColumnIndex;
			}
			else
			{
				if (!this.isNewRow)
				{
					columnIndex = this.lastCellColumn + 1;
				}
				else
				{
					columnIndex = 0;
				}
				rowIndex = this.rowIndex;
			}
			if (this.isNewRow)
			{
				this.isNewRow = false;
			}
			this.lastCellColumn = columnIndex;
		}

		public int GetRow(RowInfo rowInfo)
		{
			int num;
			if (rowInfo.RowIndex != null)
			{
				num = rowInfo.RowIndex.Value;
			}
			else
			{
				num = this.rowIndex + 1;
				if (num >= SpreadsheetDefaultValues.RowCount)
				{
					throw new InvalidOperationException("The total number of rows is greater than the allowed maximum.");
				}
			}
			this.isNewRow = true;
			this.rowIndex = num;
			return num;
		}

		int lastCellColumn;

		int rowIndex = -1;

		bool isNewRow = true;
	}
}
