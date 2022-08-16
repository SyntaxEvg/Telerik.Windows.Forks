using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class CellRef
	{
		public CellRef(int rowIndex, int columnIndex)
		{
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

		public override string ToString()
		{
			return NameConverter.ConvertCellIndexToName(this.rowIndex, this.columnIndex);
		}

		readonly int rowIndex;

		readonly int columnIndex;
	}
}
