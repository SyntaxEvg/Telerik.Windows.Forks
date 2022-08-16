using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	static class InsertRemoveCellsHelper
	{
		static InsertRemoveCellsHelper()
		{
			InsertRemoveCellsHelper.shiftTypeToInsertRemoveShiftType[ShiftType.Down] = InsertShiftType.Down;
			InsertRemoveCellsHelper.shiftTypeToInsertRemoveShiftType[ShiftType.Right] = InsertShiftType.Right;
			InsertRemoveCellsHelper.shiftTypeToInsertRemoveShiftType[ShiftType.Up] = RemoveShiftType.Up;
			InsertRemoveCellsHelper.shiftTypeToInsertRemoveShiftType[ShiftType.Left] = RemoveShiftType.Left;
		}

		public static RangeType ToRangeType(this InsertShiftType insertShiftType, bool isFullRowColumn)
		{
			if (isFullRowColumn)
			{
				if (insertShiftType != InsertShiftType.Right)
				{
					return RangeType.Rows;
				}
				return RangeType.Columns;
			}
			else
			{
				if (insertShiftType != InsertShiftType.Right)
				{
					return RangeType.CellsInColumn;
				}
				return RangeType.CellsInRow;
			}
		}

		public static RangeType ToRangeType(this RemoveShiftType removeShiftType, bool isFullRowColumn)
		{
			if (isFullRowColumn)
			{
				if (removeShiftType != RemoveShiftType.Left)
				{
					return RangeType.Rows;
				}
				return RangeType.Columns;
			}
			else
			{
				if (removeShiftType != RemoveShiftType.Left)
				{
					return RangeType.CellsInColumn;
				}
				return RangeType.CellsInRow;
			}
		}

		public static ShiftType ToShiftType(this RangeType rangeType, bool isRemove)
		{
			switch (rangeType)
			{
			case RangeType.Rows:
			case RangeType.CellsInColumn:
				if (!isRemove)
				{
					return ShiftType.Down;
				}
				return ShiftType.Up;
			case RangeType.Columns:
			case RangeType.CellsInRow:
				if (!isRemove)
				{
					return ShiftType.Right;
				}
				return ShiftType.Left;
			default:
				throw new NotSupportedException(string.Format("Not supported RangeType enumeration: {0}!", rangeType));
			}
		}

		public static ShiftType ToShiftType(this RemoveShiftType removeShiftType)
		{
			if (removeShiftType != RemoveShiftType.Left)
			{
				return ShiftType.Up;
			}
			return ShiftType.Left;
		}

		public static ShiftType ToShiftType(this InsertShiftType insertShiftType)
		{
			if (insertShiftType != InsertShiftType.Down)
			{
				return ShiftType.Right;
			}
			return ShiftType.Down;
		}

		public static InsertShiftType ReverseShiftType(this RemoveShiftType shiftType)
		{
			switch (shiftType)
			{
			case RemoveShiftType.Left:
				return InsertShiftType.Right;
			case RemoveShiftType.Up:
				return InsertShiftType.Down;
			default:
				throw new NotSupportedException();
			}
		}

		public static RemoveShiftType ReverseShiftType(this InsertShiftType shiftType)
		{
			switch (shiftType)
			{
			case InsertShiftType.Right:
				return RemoveShiftType.Left;
			case InsertShiftType.Down:
				return RemoveShiftType.Up;
			default:
				throw new NotSupportedException();
			}
		}

		public static bool TryGetInsertShiftType(this ShiftType shiftType, out InsertShiftType insertShiftType)
		{
			insertShiftType = InsertShiftType.Down;
			object obj;
			if (InsertRemoveCellsHelper.shiftTypeToInsertRemoveShiftType.TryGetValue(shiftType, out obj) && obj is InsertShiftType)
			{
				insertShiftType = (InsertShiftType)obj;
				return true;
			}
			return false;
		}

		public static bool TryGetRemoveShiftType(this ShiftType shiftType, out RemoveShiftType removeShiftType)
		{
			removeShiftType = RemoveShiftType.Left;
			object obj;
			if (InsertRemoveCellsHelper.shiftTypeToInsertRemoveShiftType.TryGetValue(shiftType, out obj) && obj is RemoveShiftType)
			{
				removeShiftType = (RemoveShiftType)obj;
				return true;
			}
			return false;
		}

		public static bool ShouldTranslate(this CellRange range, CellRange insertedRange, InsertShiftType shiftType)
		{
			bool flag = shiftType == InsertShiftType.Down;
			int num = insertedRange.StartIndex(flag);
			int num2 = (flag ? insertedRange.RowCount : insertedRange.ColumnCount);
			int num3 = (flag ? SpreadsheetDefaultValues.RowCount : SpreadsheetDefaultValues.ColumnCount);
			int num4 = range.EndIndex(flag);
			return insertedRange.ContainsProjection(range, !flag) && num <= num4 && num4 + num2 < num3;
		}

		public static bool ShouldTranslate(this CellRange range, CellRange removedRange, RemoveShiftType shiftType)
		{
			bool flag = shiftType == RemoveShiftType.Up;
			int num = removedRange.StartIndex(flag);
			int num2 = range.EndIndex(flag);
			return removedRange.ContainsProjection(range, !flag) && num <= num2;
		}

		public static bool TryTranslate(this CellRange range, CellRange insertedRange, InsertShiftType shiftType, out CellRange translatedObject)
		{
			Guard.ThrowExceptionIfNull<CellRange>(range, "range");
			Guard.ThrowExceptionIfNull<CellRange>(insertedRange, "insertedRange");
			translatedObject = null;
			if (range.ShouldTranslate(insertedRange, shiftType))
			{
				bool flag = shiftType == InsertShiftType.Down;
				int num = (flag ? insertedRange.FromIndex.RowIndex : insertedRange.FromIndex.ColumnIndex);
				int num2 = (flag ? insertedRange.RowCount : insertedRange.ColumnCount);
				int num3 = (flag ? range.FromIndex.RowIndex : range.FromIndex.ColumnIndex);
				bool flag2 = num > num3;
				int rowOffset = (flag ? num2 : 0);
				int columnOffset = (flag ? 0 : num2);
				if (flag2)
				{
					CellIndex cellIndex = range.ToIndex.Offset(rowOffset, columnOffset);
					if (cellIndex != null)
					{
						translatedObject = new CellRange(range.FromIndex, cellIndex);
					}
				}
				else
				{
					translatedObject = range.Offset(rowOffset, columnOffset);
				}
				return true;
			}
			return false;
		}

		public static bool TryTranslate(this CellRange range, CellRange removedRange, RemoveShiftType shiftType, out CellRange translatedObject)
		{
			Guard.ThrowExceptionIfNull<CellRange>(range, "range");
			Guard.ThrowExceptionIfNull<CellRange>(removedRange, "removedRange");
			translatedObject = null;
			if (range.ShouldTranslate(removedRange, shiftType))
			{
				bool flag = shiftType == RemoveShiftType.Up;
				int num = (flag ? removedRange.FromIndex.RowIndex : removedRange.FromIndex.ColumnIndex);
				int val = (flag ? removedRange.RowCount : removedRange.ColumnCount);
				CellIndex cellIndex = range.FromIndex;
				int num2 = range.StartIndex(flag);
				int num3 = System.Math.Min(num2 - num, val);
				if (num3 > 0)
				{
					int rowOffset = (flag ? (-num3) : 0);
					int columnOffset = (flag ? 0 : (-num3));
					cellIndex = cellIndex.Offset(rowOffset, columnOffset);
				}
				CellIndex cellIndex2 = range.ToIndex;
				int num4 = range.EndIndex(flag);
				int num5 = System.Math.Min(num4 - num + 1, val);
				int rowOffset2 = (flag ? (-num5) : 0);
				int columnOffset2 = (flag ? 0 : (-num5));
				cellIndex2 = cellIndex2.Offset(rowOffset2, columnOffset2);
				bool flag2 = false;
				if (cellIndex != null && cellIndex2 != null)
				{
					flag2 = cellIndex.RowIndex <= cellIndex2.RowIndex && cellIndex.ColumnIndex <= cellIndex2.ColumnIndex;
				}
				if (flag2)
				{
					translatedObject = new CellRange(cellIndex, cellIndex2);
				}
				return true;
			}
			return false;
		}

		public static bool TryTranslate(this CellRange range, CellRange insertedOrRemovedRange, ShiftType shiftType, out CellRange translatedObject)
		{
			Guard.ThrowExceptionIfNull<CellRange>(range, "range");
			Guard.ThrowExceptionIfNull<CellRange>(insertedOrRemovedRange, "insertedOrRemovedRange");
			InsertShiftType shiftType2;
			if (shiftType.TryGetInsertShiftType(out shiftType2))
			{
				return range.TryTranslate(insertedOrRemovedRange, shiftType2, out translatedObject);
			}
			RemoveShiftType shiftType3;
			if (shiftType.TryGetRemoveShiftType(out shiftType3))
			{
				return range.TryTranslate(insertedOrRemovedRange, shiftType3, out translatedObject);
			}
			throw new NotSupportedException("Not supported ShiftType!");
		}

		public static bool ShouldTranslate(this PageBreak pageBreak, CellRange insertedRange, InsertShiftType shiftType)
		{
			return pageBreak.CellRange.ShouldTranslate(insertedRange, shiftType);
		}

		public static bool ShouldTranslate(this PageBreak pageBreak, CellRange removedRange, RemoveShiftType shiftType)
		{
			return pageBreak.CellRange.ShouldTranslate(removedRange, shiftType);
		}

		public static bool TryTranslate(this PageBreak pageBreak, CellRange insertedRange, InsertShiftType shiftType, out PageBreak translatedObject)
		{
			translatedObject = null;
			CellRange cellRange;
			if (pageBreak.CellRange.TryTranslate(insertedRange, shiftType, out cellRange))
			{
				if (cellRange != null)
				{
					translatedObject = InsertRemoveCellsHelper.GetPageBreakFromCellRange(cellRange, pageBreak.Type);
				}
				return true;
			}
			return false;
		}

		public static bool TryTranslate(this PageBreak pageBreak, CellRange removedRange, RemoveShiftType shiftType, out PageBreak translatedObject)
		{
			translatedObject = null;
			CellRange cellRange;
			if (pageBreak.CellRange.TryTranslate(removedRange, shiftType, out cellRange))
			{
				if (cellRange != null)
				{
					translatedObject = InsertRemoveCellsHelper.GetPageBreakFromCellRange(cellRange, pageBreak.Type);
				}
				return true;
			}
			return false;
		}

		public static bool TryTranslate(this PageBreak pageBreak, CellRange insertedOrRemovedRange, ShiftType shiftType, out PageBreak translatedObject)
		{
			InsertShiftType shiftType2;
			if (shiftType.TryGetInsertShiftType(out shiftType2))
			{
				return pageBreak.TryTranslate(insertedOrRemovedRange, shiftType2, out translatedObject);
			}
			RemoveShiftType shiftType3;
			if (shiftType.TryGetRemoveShiftType(out shiftType3))
			{
				return pageBreak.TryTranslate(insertedOrRemovedRange, shiftType3, out translatedObject);
			}
			throw new NotSupportedException("Not supported ShiftType!");
		}

		public static bool CanInsertOrRemove(this CellRange range, ShiftType shiftType, CellRange selectedRange)
		{
			switch (shiftType)
			{
			case ShiftType.Left:
			case ShiftType.Right:
			{
				bool flag = selectedRange.FromIndex.ColumnIndex > range.ToIndex.ColumnIndex;
				bool flag2 = selectedRange.IntersectsRowsProjections(range);
				bool flag3 = selectedRange.ContainsRowsProjection(range);
				return flag || !flag2 || flag3;
			}
			case ShiftType.Up:
			case ShiftType.Down:
			{
				bool flag4 = selectedRange.FromIndex.RowIndex > range.ToIndex.RowIndex;
				bool flag5 = selectedRange.IntersectsColumnsProjections(range);
				bool flag6 = selectedRange.ContainsColumnsProjection(range);
				return flag4 || !flag5 || flag6;
			}
			default:
				return true;
			}
		}

		public static int GetRelativeRearrangeIndex(CellRange range, CellRange selectedRange)
		{
			int result = range.ColumnCount;
			bool flag = range.FromIndex.ColumnIndex < selectedRange.FromIndex.ColumnIndex;
			if (flag)
			{
				result = selectedRange.ToIndex.ColumnIndex - range.FromIndex.ColumnIndex;
			}
			return result;
		}

		public static int GetRearrangeDelta(CellRange range, ShiftType shiftType)
		{
			int num = range.ColumnCount;
			RemoveShiftType removeShiftType;
			bool flag = shiftType.TryGetRemoveShiftType(out removeShiftType);
			if (flag)
			{
				num *= -1;
			}
			return num;
		}

		static PageBreak GetPageBreakFromCellRange(CellRange range, PageBreakType type)
		{
			bool flag = type == PageBreakType.Horizontal;
			int placementIndex = range.StartIndex(flag);
			int fromIndex = range.StartIndex(!flag);
			int toIndex = range.EndIndex(!flag);
			return new PageBreak(type, placementIndex, fromIndex, toIndex);
		}

		static readonly Dictionary<ShiftType, object> shiftTypeToInsertRemoveShiftType = new Dictionary<ShiftType, object>();
	}
}
