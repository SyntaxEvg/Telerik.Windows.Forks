using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	class CellPropertyDataInfo
	{
		public CellPropertyDataInfo()
		{
			this.rowUsedRange = new SortedList<int, Range>();
			this.columnUsedRange = new SortedList<int, Range>();
		}

		public void SetRowUsedRangeValue(int rowIndex, Range value)
		{
			this.maxRowUsedRange = Range.MaxOrNull(this.maxRowUsedRange, value);
			this.rowUsedRange[rowIndex] = value;
		}

		public void SetColumnUsedRangeValue(int columnIndex, Range value)
		{
			this.maxColumnUsedRange = Range.MaxOrNull(this.maxColumnUsedRange, value);
			this.columnUsedRange[columnIndex] = value;
		}

		public Range GetRowUsedRange(int rowIndex)
		{
			if (this.rowUsedRange.ContainsKey(rowIndex))
			{
				return this.rowUsedRange[rowIndex];
			}
			return null;
		}

		public Range GetColumnUsedRange(int columnIndex)
		{
			if (this.columnUsedRange.ContainsKey(columnIndex))
			{
				return this.columnUsedRange[columnIndex];
			}
			return null;
		}

		public bool GetRowIsEmpty(int rowIndex)
		{
			return this.GetRowUsedRange(rowIndex) == null;
		}

		public bool GetColumnIsEmpty(int columnIndex)
		{
			return this.GetColumnUsedRange(columnIndex) == null;
		}

		public CellRange GetUsedCellRange()
		{
			if (this.maxColumnUsedRange == null || this.maxRowUsedRange == null)
			{
				return null;
			}
			return new CellRange(this.maxColumnUsedRange.Start, this.maxRowUsedRange.Start, this.maxColumnUsedRange.End, this.maxRowUsedRange.End);
		}

		readonly SortedList<int, Range> rowUsedRange;

		readonly SortedList<int, Range> columnUsedRange;

		Range maxRowUsedRange;

		Range maxColumnUsedRange;
	}
}
