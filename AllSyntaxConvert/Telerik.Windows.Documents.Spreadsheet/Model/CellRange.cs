using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellRange
	{
		public CellIndex FromIndex
		{
			get
			{
				return this.fromIndex;
			}
		}

		public CellIndex ToIndex
		{
			get
			{
				return this.toIndex;
			}
		}

		public int RowCount
		{
			get
			{
				return this.ToIndex.RowIndex - this.FromIndex.RowIndex + 1;
			}
		}

		public int ColumnCount
		{
			get
			{
				return this.ToIndex.ColumnIndex - this.FromIndex.ColumnIndex + 1;
			}
		}

		internal bool IsTopToBottom
		{
			get
			{
				return this.isTopToBottom;
			}
		}

		internal bool IsLeftToRight
		{
			get
			{
				return this.isLeftToRight;
			}
		}

		public bool IsSingleCell
		{
			get
			{
				return this.RowCount == 1 && this.ColumnCount == 1;
			}
		}

		public CellRange(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			this.isLeftToRight = fromColumnIndex <= toColumnIndex;
			this.isTopToBottom = fromRowIndex <= toRowIndex;
			this.fromIndex = new CellIndex(Math.Min(fromRowIndex, toRowIndex), Math.Min(fromColumnIndex, toColumnIndex));
			this.toIndex = new CellIndex(Math.Max(fromRowIndex, toRowIndex), Math.Max(fromColumnIndex, toColumnIndex));
		}

		public CellRange(CellIndex fromIndex, CellIndex toIndex)
			: this(fromIndex.RowIndex, fromIndex.ColumnIndex, toIndex.RowIndex, toIndex.ColumnIndex)
		{
		}

		public CellRange Offset(int rowOffset, int columnOffset)
		{
			CellIndex left = this.FromIndex.Offset(rowOffset, columnOffset);
			CellIndex left2 = this.ToIndex.Offset(rowOffset, columnOffset);
			if (left == null || left2 == null)
			{
				return null;
			}
			return new CellRange(left, left2);
		}

		internal CellRange OffsetFromRow(int offset)
		{
			int num = this.FromIndex.RowIndex + offset;
			int num2 = this.ToIndex.RowIndex + offset;
			if (!TelerikHelper.IsValidRowIndex(num) || !TelerikHelper.IsValidRowIndex(num2))
			{
				return null;
			}
			return new CellRange(num, this.FromIndex.ColumnIndex, num2, this.ToIndex.ColumnIndex);
		}

		internal CellRange OffsetFromColumn(int offset)
		{
			int num = this.FromIndex.ColumnIndex + offset;
			int num2 = this.ToIndex.ColumnIndex + offset;
			if (!TelerikHelper.IsValidColumnIndex(num) || !TelerikHelper.IsValidColumnIndex(num2))
			{
				return null;
			}
			return new CellRange(this.FromIndex.RowIndex, num, this.ToIndex.RowIndex, num2);
		}

		public CellRange GetFirstRow()
		{
			return new CellRange(this.FromIndex.RowIndex, this.FromIndex.ColumnIndex, this.FromIndex.RowIndex, this.ToIndex.ColumnIndex);
		}

		public CellRange GetLastRow()
		{
			return new CellRange(this.ToIndex.RowIndex, this.FromIndex.ColumnIndex, this.ToIndex.RowIndex, this.ToIndex.ColumnIndex);
		}

		public CellRange GetFirstColumn()
		{
			return new CellRange(this.FromIndex.RowIndex, this.FromIndex.ColumnIndex, this.ToIndex.RowIndex, this.FromIndex.ColumnIndex);
		}

		public CellRange GetLastColumn()
		{
			return new CellRange(this.FromIndex.RowIndex, this.ToIndex.ColumnIndex, this.ToIndex.RowIndex, this.ToIndex.ColumnIndex);
		}

		internal CellRange GetRow(int rowIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(this.FromIndex.RowIndex, this.ToIndex.RowIndex, rowIndex, "rowIndex");
			return new CellRange(rowIndex, this.FromIndex.ColumnIndex, rowIndex, this.ToIndex.ColumnIndex);
		}

		internal CellRange GetColumn(int columnIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(this.FromIndex.ColumnIndex, this.ToIndex.ColumnIndex, columnIndex, "columnIndex");
			return new CellRange(this.FromIndex.RowIndex, columnIndex, this.ToIndex.RowIndex, columnIndex);
		}

		internal bool ContainsColumnsProjection(CellRange other)
		{
			return this.IsFirstIntervalContainingSecond(this.FromIndex.ColumnIndex, this.ToIndex.ColumnIndex, other.FromIndex.ColumnIndex, other.ToIndex.ColumnIndex);
		}

		internal bool ContainsRowsProjection(CellRange other)
		{
			return this.IsFirstIntervalContainingSecond(this.FromIndex.RowIndex, this.ToIndex.RowIndex, other.FromIndex.RowIndex, other.ToIndex.RowIndex);
		}

		internal bool ContainsProjection(CellRange other, bool isRowProjection)
		{
			if (!isRowProjection)
			{
				return this.ContainsColumnsProjection(other);
			}
			return this.ContainsRowsProjection(other);
		}

		bool IsFirstIntervalContainingSecond(int firstStart, int firstEnd, int secondStart, int secondEnd)
		{
			return firstStart <= secondStart && secondEnd <= firstEnd;
		}

		public bool Contains(CellIndex index)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(index, "index");
			return this.Contains(index.RowIndex, index.ColumnIndex);
		}

		public bool Contains(int rowIndex, int columnIndex)
		{
			return this.fromIndex.RowIndex <= rowIndex && rowIndex <= this.toIndex.RowIndex && this.fromIndex.ColumnIndex <= columnIndex && columnIndex <= this.toIndex.ColumnIndex;
		}

		public bool Contains(CellRange cellRange)
		{
			return this.Contains(cellRange.FromIndex) && this.Contains(cellRange.ToIndex);
		}

		internal bool IntersectsRowsProjections(CellRange other)
		{
			return Utils.AreIntersectingIntervals(this.FromIndex.RowIndex, this.ToIndex.RowIndex, other.FromIndex.RowIndex, other.ToIndex.RowIndex);
		}

		internal bool IntersectsColumnsProjections(CellRange other)
		{
			return Utils.AreIntersectingIntervals(this.FromIndex.ColumnIndex, this.ToIndex.ColumnIndex, other.FromIndex.ColumnIndex, other.ToIndex.ColumnIndex);
		}

		public bool IntersectsWith(CellRange other)
		{
			Guard.ThrowExceptionIfNull<CellRange>(other, "other");
			return other.FromIndex.ColumnIndex <= this.ToIndex.ColumnIndex && other.ToIndex.ColumnIndex >= this.FromIndex.ColumnIndex && other.FromIndex.RowIndex <= this.ToIndex.RowIndex && other.ToIndex.RowIndex >= this.FromIndex.RowIndex;
		}

		public CellRange Intersect(CellRange other)
		{
			if (!this.IntersectsWith(other))
			{
				return CellRange.Empty;
			}
			return new CellRange(CellIndex.Max(this.FromIndex, other.FromIndex), CellIndex.Min(this.ToIndex, other.ToIndex));
		}

		internal static CellRange MaxOrNull(CellRange firstCellRange, CellRange secondCellRange)
		{
			if (firstCellRange == null)
			{
				return secondCellRange;
			}
			if (secondCellRange == null)
			{
				return firstCellRange;
			}
			return CellRange.Max(firstCellRange, secondCellRange);
		}

		internal static CellRange Max(CellRange first, CellRange second)
		{
			Guard.ThrowExceptionIfNull<CellRange>(first, "first");
			Guard.ThrowExceptionIfNull<CellRange>(second, "second");
			return new CellRange(Math.Min(first.FromIndex.RowIndex, second.FromIndex.RowIndex), Math.Min(first.FromIndex.ColumnIndex, second.FromIndex.ColumnIndex), Math.Max(first.ToIndex.RowIndex, second.ToIndex.RowIndex), Math.Max(first.ToIndex.ColumnIndex, second.ToIndex.ColumnIndex));
		}

		internal static CellRange RestrictCellRange(CellRange cellRange, SizeI size)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.RowCount - 1, size.Height - 1, "maxRowIndex");
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.ColumnCount - 1, size.Width - 1, "maxColumnIndex");
			CellIndex cellIndex = CellIndex.RestrictCellIndex(cellRange.FromIndex, size);
			CellIndex cellIndex2 = CellIndex.RestrictCellIndex(cellRange.ToIndex, size);
			if (cellRange.IsLeftToRight && cellRange.IsTopToBottom)
			{
				return new CellRange(cellIndex, cellIndex2);
			}
			if (cellRange.IsLeftToRight && !cellRange.IsTopToBottom)
			{
				return new CellRange(cellIndex2.RowIndex, cellIndex.ColumnIndex, cellIndex.RowIndex, cellIndex2.ColumnIndex);
			}
			if (!cellRange.IsLeftToRight && cellRange.IsTopToBottom)
			{
				return new CellRange(cellIndex.RowIndex, cellIndex2.ColumnIndex, cellIndex2.RowIndex, cellIndex.ColumnIndex);
			}
			return new CellRange(cellIndex2, cellIndex);
		}

		public static CellRange FromRow(int rowIndex)
		{
			return CellRange.FromRowRange(rowIndex, rowIndex);
		}

		public static CellRange FromRowRange(int fromRowIndex, int toRowIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(fromRowIndex);
			Guard.ThrowExceptionIfInvalidRowIndex(toRowIndex);
			return new CellRange(fromRowIndex, 0, toRowIndex, SpreadsheetDefaultValues.ColumnCount - 1);
		}

		public static CellRange FromColumn(int columnIndex)
		{
			return CellRange.FromColumnRange(columnIndex, columnIndex);
		}

		public static CellRange FromColumnRange(int fromColumnIndex, int toColumnIndex)
		{
			Guard.ThrowExceptionIfInvalidColumnIndex(fromColumnIndex);
			return new CellRange(0, fromColumnIndex, SpreadsheetDefaultValues.RowCount - 1, toColumnIndex);
		}

		internal IEnumerable<CellRange> SplitToRangesSurroundingRange(CellRange cellRange)
		{
			if (!this.IntersectsWith(cellRange))
			{
				yield return this;
			}
			else
			{
				if (this.FromIndex.ColumnIndex < cellRange.FromIndex.ColumnIndex)
				{
					yield return new CellRange(this.FromIndex.RowIndex, this.FromIndex.ColumnIndex, this.ToIndex.RowIndex, cellRange.FromIndex.ColumnIndex - 1);
				}
				if (this.ToIndex.ColumnIndex > cellRange.ToIndex.ColumnIndex)
				{
					yield return new CellRange(this.FromIndex.RowIndex, cellRange.ToIndex.ColumnIndex + 1, this.ToIndex.RowIndex, this.ToIndex.ColumnIndex);
				}
				if (this.FromIndex.RowIndex < cellRange.FromIndex.RowIndex)
				{
					yield return new CellRange(this.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, cellRange.FromIndex.RowIndex - 1, cellRange.ToIndex.ColumnIndex);
				}
				if (this.ToIndex.RowIndex > cellRange.ToIndex.RowIndex)
				{
					yield return new CellRange(cellRange.ToIndex.RowIndex + 1, cellRange.FromIndex.ColumnIndex, this.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex);
				}
			}
			yield break;
		}

		internal static IEnumerable<CellRange> GetDifference(CellRange firstRange, CellRange secondRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(firstRange, "firstRange");
			Guard.ThrowExceptionIfNull<CellRange>(secondRange, "secondRange");
			if (!firstRange.IntersectsWith(secondRange))
			{
				yield return firstRange;
			}
			else
			{
				CellRange intersectionRange = firstRange.Intersect(secondRange);
				int innerRangeFromRow = intersectionRange.FromIndex.RowIndex - 1;
				int innerRangeFromColumn = intersectionRange.FromIndex.ColumnIndex - 1;
				int innerRangeToRow = intersectionRange.ToIndex.RowIndex + 1;
				int innerRangeToColumn = intersectionRange.ToIndex.ColumnIndex + 1;
				if (firstRange.FromIndex.ColumnIndex <= innerRangeFromColumn)
				{
					yield return new CellRange(firstRange.FromIndex.RowIndex, firstRange.FromIndex.ColumnIndex, intersectionRange.ToIndex.RowIndex, innerRangeFromColumn);
				}
				if (firstRange.FromIndex.RowIndex <= innerRangeFromRow)
				{
					yield return new CellRange(firstRange.FromIndex.RowIndex, intersectionRange.FromIndex.ColumnIndex, innerRangeFromRow, firstRange.ToIndex.ColumnIndex);
				}
				if (innerRangeToColumn <= firstRange.ToIndex.ColumnIndex)
				{
					yield return new CellRange(intersectionRange.FromIndex.RowIndex, innerRangeToColumn, firstRange.ToIndex.RowIndex, firstRange.ToIndex.ColumnIndex);
				}
				if (innerRangeToRow <= firstRange.ToIndex.RowIndex)
				{
					yield return new CellRange(innerRangeToRow, firstRange.FromIndex.ColumnIndex, firstRange.ToIndex.RowIndex, intersectionRange.ToIndex.ColumnIndex);
				}
			}
			yield break;
		}

		internal int StartIndex(bool isRowIndex)
		{
			if (!isRowIndex)
			{
				return this.FromIndex.ColumnIndex;
			}
			return this.FromIndex.RowIndex;
		}

		internal int EndIndex(bool isRowIndex)
		{
			if (!isRowIndex)
			{
				return this.ToIndex.ColumnIndex;
			}
			return this.ToIndex.RowIndex;
		}

		public override bool Equals(object obj)
		{
			CellRange cellRange = obj as CellRange;
			return cellRange != null && this.FromIndex.Equals(cellRange.FromIndex) && this.ToIndex.Equals(cellRange.ToIndex);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.FromIndex.GetHashCode(), this.ToIndex.GetHashCode());
		}

		public override string ToString()
		{
			return string.Format("{0}:{1}", this.FromIndex, this.ToIndex);
		}

		public static readonly CellRange Empty = new CellRange(0, 0, 0, 0);

		readonly CellIndex fromIndex;

		readonly CellIndex toIndex;

		readonly bool isTopToBottom;

		readonly bool isLeftToRight;
	}
}
