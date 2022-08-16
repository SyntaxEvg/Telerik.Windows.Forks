using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class CellRefRange
	{
		public CellRefRange(CellRange range)
			: this(new CellRef(range.FromIndex.RowIndex, range.FromIndex.ColumnIndex), new CellRef(range.ToIndex.RowIndex, range.ToIndex.ColumnIndex))
		{
		}

		public CellRefRange(CellRef fromCell, CellRef toCell)
		{
			this.fromCell = fromCell;
			this.toCell = toCell;
		}

		public CellRefRange(string cellRefRange)
		{
			CellRange cellRange;
			NameConverter.ConvertCellNameToCellRange(cellRefRange, out cellRange);
			this.fromCell = new CellRef(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex);
			this.toCell = new CellRef(cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex);
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

		public CellRange ToCellRange()
		{
			return new CellRange(this.FromCell.ToCellIndex(), this.ToCell.ToCellIndex());
		}

		public override string ToString()
		{
			CellIndex cellIndex = this.FromCell.ToCellIndex();
			CellIndex cellIndex2 = this.ToCell.ToCellIndex();
			string result = string.Empty;
			if (cellIndex == cellIndex2)
			{
				result = this.FromCell.ToString();
			}
			else
			{
				result = NameConverter.ConvertCellRangeToName(cellIndex, cellIndex2);
			}
			return result;
		}

		readonly CellRef fromCell;

		readonly CellRef toCell;
	}
}
