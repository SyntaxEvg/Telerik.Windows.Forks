using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class CellRangeInsertedOrRemovedAwareCollectionBase
	{
		protected abstract ICollection<CellRange> CellRanges { get; }

		protected Cells Cells { get; set; }

		internal CellRangeInsertedOrRemovedAwareCollectionBase(Cells cells)
		{
			Guard.ThrowExceptionIfNull<Cells>(cells, "cells");
			this.Cells = cells;
		}

		internal void Update(CellRangeInsertedOrRemovedEventArgs eventArgs)
		{
			if (this.ShouldTranslateUpDown(eventArgs.RangeType) || this.ShouldTranslateLeftRight(eventArgs.RangeType))
			{
				ShiftType shiftType = eventArgs.RangeType.ToShiftType(eventArgs.IsRemove);
				this.TranslateRanges(eventArgs.Range, shiftType);
			}
		}

		internal bool CanInsertOrRemove(CellRange selectedRange, ShiftType shiftType)
		{
			Guard.ThrowExceptionIfNull<CellRange>(selectedRange, "selectedRange");
			foreach (CellRange range in this.CellRanges)
			{
				if (!this.CanInsertOrRemove(shiftType, range, selectedRange))
				{
					return false;
				}
			}
			return true;
		}

		protected virtual bool CanInsertOrRemove(ShiftType shiftType, CellRange range, CellRange selectedRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(range, "range");
			Guard.ThrowExceptionIfNull<CellRange>(selectedRange, "selectedRange");
			return true;
		}

		protected abstract void TranslateRange(CellRange oldRange, CellRange newRange);

		protected virtual bool ShouldTranslateUpDown(RangeType rangeType)
		{
			return rangeType == RangeType.Rows || rangeType == RangeType.CellsInColumn;
		}

		protected virtual bool ShouldTranslateLeftRight(RangeType rangeType)
		{
			return rangeType == RangeType.Columns || rangeType == RangeType.CellsInRow;
		}

		protected virtual void TranslateRanges(Dictionary<CellRange, CellRange> oldAndNewTranslatedRangesPositions)
		{
			foreach (KeyValuePair<CellRange, CellRange> keyValuePair in oldAndNewTranslatedRangesPositions)
			{
				this.TranslateRange(keyValuePair.Key, keyValuePair.Value);
			}
		}

		protected virtual void OnBeforeTranslation(Dictionary<CellRange, CellRange> oldAndNewTranslatedRangesPositions, CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
		}

		protected virtual void OnAfterTranslation(Dictionary<CellRange, CellRange> oldAndNewTranslatedRangesPositions, CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
		}

		Dictionary<CellRange, CellRange> CalculateOldAndNewTranslatedRangesPositions(CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
			Guard.ThrowExceptionIfNull<CellRange>(rangeToInsertOrRemove, "rangeToInsertOrRemove");
			Dictionary<CellRange, CellRange> dictionary = new Dictionary<CellRange, CellRange>();
			foreach (CellRange cellRange in this.CellRanges)
			{
				CellRange value;
				if (cellRange.TryTranslate(rangeToInsertOrRemove, shiftType, out value))
				{
					dictionary[cellRange] = value;
				}
			}
			return dictionary;
		}

		void TranslateRanges(CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
			Dictionary<CellRange, CellRange> oldAndNewTranslatedRangesPositions = this.CalculateOldAndNewTranslatedRangesPositions(rangeToInsertOrRemove, shiftType);
			this.OnBeforeTranslation(oldAndNewTranslatedRangesPositions, rangeToInsertOrRemove, shiftType);
			this.TranslateRanges(oldAndNewTranslatedRangesPositions);
			this.OnAfterTranslation(oldAndNewTranslatedRangesPositions, rangeToInsertOrRemove, shiftType);
		}
	}
}
