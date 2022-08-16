using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellRangeInsertedOrRemovedEventArgs : EventArgs
	{
		public CellRange AffectedRange { get; set; }

		public CellRange Range { get; set; }

		public RangeType RangeType { get; set; }

		public bool IsRemove { get; set; }

		public CellRangeInsertedOrRemovedEventArgs(CellRange range, RangeType rangeType, bool isRemove)
		{
			Guard.ThrowExceptionIfNull<CellRange>(range, "range");
			this.Range = range;
			this.RangeType = rangeType;
			this.IsRemove = isRemove;
			switch (this.RangeType)
			{
			case RangeType.Rows:
				this.AffectedRange = new CellRange(0, range.FromIndex.ColumnIndex, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
				return;
			case RangeType.Columns:
				this.AffectedRange = new CellRange(range.FromIndex.RowIndex, 0, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
				return;
			case RangeType.CellsInRow:
				this.AffectedRange = new CellRange(range.FromIndex.RowIndex, range.FromIndex.ColumnIndex, range.ToIndex.RowIndex, SpreadsheetDefaultValues.ColumnCount - 1);
				return;
			case RangeType.CellsInColumn:
				this.AffectedRange = new CellRange(range.FromIndex.RowIndex, range.FromIndex.ColumnIndex, SpreadsheetDefaultValues.RowCount - 1, range.ToIndex.ColumnIndex);
				return;
			default:
				return;
			}
		}
	}
}
