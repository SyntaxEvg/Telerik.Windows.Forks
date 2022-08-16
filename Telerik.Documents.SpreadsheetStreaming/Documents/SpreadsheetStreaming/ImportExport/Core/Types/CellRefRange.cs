using System;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class CellRefRange
	{
		public CellRefRange(SpreadCellRange range)
			: this(new CellRef(range.FromRowIndex, range.FromColumnIndex), new CellRef(range.ToRowIndex, range.ToColumnIndex))
		{
		}

		public CellRefRange(CellRef fromCell, CellRef toCell)
		{
			this.fromCell = fromCell;
			this.toCell = toCell;
		}

		public CellRefRange(string cellRefRange)
		{
			SpreadCellRange spreadCellRange;
			NameConverter.ConvertCellNameToCellRange(cellRefRange, out spreadCellRange);
			this.fromCell = new CellRef(spreadCellRange.FromRowIndex, spreadCellRange.FromColumnIndex);
			this.toCell = new CellRef(spreadCellRange.ToRowIndex, spreadCellRange.ToColumnIndex);
		}

		public CellRef FromCell
		{
			get
			{
				return this.fromCell;
			}
		}

		public CellRef ToCell
		{
			get
			{
				return this.toCell;
			}
		}

		public SpreadCellRange ToCellRange()
		{
			return new SpreadCellRange(this.FromCell.RowIndex, this.fromCell.ColumnIndex, this.ToCell.RowIndex, this.ToCell.ColumnIndex);
		}

		readonly CellRef fromCell;

		readonly CellRef toCell;
	}
}
