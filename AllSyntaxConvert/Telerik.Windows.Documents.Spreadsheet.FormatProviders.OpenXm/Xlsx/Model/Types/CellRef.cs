using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class CellRef
	{
		public CellRef(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
		}

		public CellRef(string cellRef)
		{
			NameConverter.ConvertCellNameToIndex(cellRef, out this.rowIndex, out this.columnIndex);
		}

		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		public CellIndex ToCellIndex()
		{
			return new CellIndex(this.RowIndex, this.ColumnIndex);
		}

		public override string ToString()
		{
			return NameConverter.ConvertCellIndexToName(this.rowIndex, this.columnIndex);
		}

		readonly int rowIndex;

		readonly int columnIndex;
	}
}
