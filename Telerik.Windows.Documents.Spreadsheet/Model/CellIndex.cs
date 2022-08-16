using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellIndex : CellIndexBase
	{
		public override int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		public override int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		public CellIndex(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
		}

		public CellIndex Offset(int rowOffset, int columnOffset)
		{
			int num = this.rowIndex + rowOffset;
			int num2 = this.columnIndex + columnOffset;
			if (!TelerikHelper.IsValidRowIndex(num) || !TelerikHelper.IsValidColumnIndex(num2))
			{
				return null;
			}
			return new CellIndex(num, num2);
		}

		internal CellRange ToCellRange()
		{
			return new CellRange(this, this);
		}

		internal static CellIndex Min(CellIndex first, CellIndex second)
		{
			return new CellIndex(Math.Min(first.RowIndex, second.RowIndex), Math.Min(first.ColumnIndex, second.ColumnIndex));
		}

		internal static CellIndex Max(CellIndex first, CellIndex second)
		{
			return new CellIndex(Math.Max(first.RowIndex, second.RowIndex), Math.Max(first.ColumnIndex, second.ColumnIndex));
		}

		internal static CellIndex RestrictCellIndex(CellIndex cellIndex, SizeI size)
		{
			int num = size.Height - 1;
			int num2 = size.Width - 1;
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.RowCount - 1, num, "maxRowIndex");
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.ColumnCount - 1, num2, "maxColumnIndex");
			return new CellIndex(Math.Min(cellIndex.RowIndex, num), Math.Min(cellIndex.ColumnIndex, num2));
		}

		public long ToNumber()
		{
			return (long)this.RowIndex * (long)SpreadsheetDefaultValues.ColumnCount + (long)this.ColumnIndex;
		}

		public override string ToString()
		{
			return string.Format("{0},{1}", this.RowIndex, this.ColumnIndex);
		}

		readonly int rowIndex;

		readonly int columnIndex;
	}
}
