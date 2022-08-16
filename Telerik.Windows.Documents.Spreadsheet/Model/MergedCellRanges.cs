using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class MergedCellRanges : CellRangeInsertedOrRemovedAwareCollection
	{
		public IEnumerable<CellRange> Ranges
		{
			get
			{
				return this.CellRanges;
			}
		}

		public MergedCellRanges(Cells cells)
			: base(cells)
		{
		}

		protected override void TranslateRanges(Dictionary<CellRange, CellRange> oldAndNewTranslatedRangesPositions)
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				using (new UpdateScope(new Action(base.Cells.Worksheet.BeginUndoGroup), new Action(base.Cells.Worksheet.EndUndoGroup)))
				{
					base.Cells[oldAndNewTranslatedRangesPositions.Keys].Unmerge();
					foreach (CellRange cellRange in oldAndNewTranslatedRangesPositions.Values)
					{
						if (cellRange != null)
						{
							base.Cells[cellRange].Merge();
						}
					}
				}
			}
		}

		protected override void Add(CellRange cellRange)
		{
			base.Add(cellRange);
			this.UpdateChangedCellRange(cellRange);
		}

		protected override void Remove(CellRange cellRange)
		{
			base.Remove(cellRange);
			this.UpdateChangedCellRange(cellRange);
		}

		internal bool CanInsertOrRemoveInternal(ShiftType shiftType, CellRange range, CellRange selectedRange)
		{
			return this.CanInsertOrRemove(shiftType, range, selectedRange);
		}

		internal bool IsSingleMergedRange(CellRange range)
		{
			Guard.ThrowExceptionIfNull<CellRange>(range, "range");
			CellRange cellRange;
			return this.TryGetContainingMergedRange(range.ToIndex, out cellRange) && cellRange.Equals(range);
		}

		protected override bool CanInsertOrRemove(ShiftType shiftType, CellRange range, CellRange selectedRange)
		{
			return range.CanInsertOrRemove(shiftType, selectedRange);
		}

		static ISet<CellRange> GetIntersectingMergedRanges(CellRange cellRange, IEnumerable<CellRange> mergedCellRanges)
		{
			HashSet<CellRange> hashSet = new HashSet<CellRange>();
			foreach (CellRange cellRange2 in mergedCellRanges)
			{
				if (cellRange.IntersectsWith(cellRange2))
				{
					hashSet.Add(cellRange2);
				}
			}
			return hashSet;
		}

		ISet<CellRange> GetIntersectingMergedRanges(int rowIndex, int columnIndex, ICollection<CellRange> mergedCellRanges)
		{
			HashSet<CellRange> hashSet = new HashSet<CellRange>();
			foreach (CellRange cellRange in mergedCellRanges)
			{
				if (cellRange.Contains(rowIndex, columnIndex))
				{
					hashSet.Add(cellRange);
				}
			}
			return hashSet;
		}

		public bool IntersectsWithMergedRanges(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			foreach (CellRange cellRange2 in this.CellRanges)
			{
				if (cellRange2.IntersectsWith(cellRange))
				{
					return true;
				}
			}
			return false;
		}

		public ISet<CellRange> GetIntersectingMergedRanges(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			return MergedCellRanges.GetIntersectingMergedRanges(cellRange, this.CellRanges);
		}

		ISet<CellRange> GetIntersectingMergedRanges(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			return this.GetIntersectingMergedRanges(rowIndex, columnIndex, this.CellRanges);
		}

		public CellRange ExpandRangeToNotIntersectMergedCells(CellRange cellRange)
		{
			List<CellRange> list = new List<CellRange>(this.CellRanges);
			CellRange cellRange2 = cellRange;
			ISet<CellRange> intersectingMergedRanges;
			do
			{
				intersectingMergedRanges = MergedCellRanges.GetIntersectingMergedRanges(cellRange2, list);
				foreach (CellRange cellRange3 in intersectingMergedRanges)
				{
					list.Remove(cellRange3);
					cellRange2 = CellRange.Max(cellRange2, cellRange3);
				}
			}
			while (intersectingMergedRanges.Count > 0);
			return cellRange2;
		}

		public bool MergedCellIsSplitByHiddenRowColumn(CellRange cellRange)
		{
			Worksheet worksheet = base.Cells.Worksheet;
			ISet<CellRange> intersectingMergedRanges = this.GetIntersectingMergedRanges(cellRange);
			bool flag = false;
			foreach (CellRange cellRange2 in intersectingMergedRanges)
			{
				flag |= worksheet.Rows[cellRange2].GetHidden().IsIndeterminate;
				flag |= worksheet.Columns[cellRange2].GetHidden().IsIndeterminate;
			}
			return flag;
		}

		public void AddMergedRange(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			if (cellRange.RowCount == 1 && cellRange.ColumnCount == 1)
			{
				throw new LocalizableException("Cannot merge a single cell.", new InvalidOperationException("Cannot merge a single cell."), "Spreadsheet_ErrorExpressions_CantMergeSingle", null);
			}
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				CellRange cellRange2 = cellRange;
				ISet<CellRange> intersectingMergedRanges;
				do
				{
					intersectingMergedRanges = this.GetIntersectingMergedRanges(cellRange2);
					foreach (CellRange cellRange3 in intersectingMergedRanges)
					{
						this.Remove(cellRange3);
						cellRange2 = CellRange.Max(cellRange2, cellRange3);
					}
				}
				while (intersectingMergedRanges.Count > 0);
				this.Add(cellRange2);
			}
		}

		public void AddMergedRanges(IEnumerable<CellRange> cellRanges)
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				foreach (CellRange cellRange in cellRanges)
				{
					this.AddMergedRange(cellRange);
				}
			}
		}

		public void RemoveMergedRange(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				ISet<CellRange> intersectingMergedRanges = this.GetIntersectingMergedRanges(cellRange);
				foreach (CellRange cellRange2 in intersectingMergedRanges)
				{
					this.Remove(cellRange2);
				}
			}
		}

		public void RemoveMergedRanges(IEnumerable<CellRange> cellRanges)
		{
			if (!cellRanges.Any<CellRange>())
			{
				return;
			}
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				foreach (CellRange cellRange in cellRanges)
				{
					this.RemoveMergedRange(cellRange);
				}
			}
		}

		public bool TryGetContainingMergedRange(CellIndex cellIndex, out CellRange mergedRange)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			return this.TryGetContainingMergedRange(cellIndex.RowIndex, cellIndex.ColumnIndex, out mergedRange);
		}

		internal bool TryGetContainingMergedRange(int rowIndex, int columnIndex, out CellRange mergedRange)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			ISet<CellRange> intersectingMergedRanges = this.GetIntersectingMergedRanges(rowIndex, columnIndex);
			mergedRange = intersectingMergedRanges.FirstOrDefault<CellRange>();
			return intersectingMergedRanges.Count == 1;
		}

		public bool TryGetContainingMergedRange(CellRange cellRange, out CellRange mergedRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			ISet<CellRange> intersectingMergedRanges = this.GetIntersectingMergedRanges(cellRange);
			mergedRange = intersectingMergedRanges.FirstOrDefault<CellRange>();
			return intersectingMergedRanges.Count == 1;
		}

		public void BeginUpdate()
		{
			this.OnChanging();
			this.beginUpdateCount++;
		}

		public void EndUpdate()
		{
			if (this.beginUpdateCount == 0)
			{
				return;
			}
			this.beginUpdateCount--;
			this.OnChanged();
		}

		internal void CopyFrom(MergedCellRanges fromMergedCellRanges)
		{
			foreach (CellRange item in fromMergedCellRanges.CellRanges)
			{
				this.CellRanges.Add(item);
			}
		}

		void UpdateChangedCellRange(CellRange cellRange)
		{
			if (this.changedCellRange == null)
			{
				this.changedCellRange = cellRange;
				return;
			}
			this.changedCellRange = CellRange.Max(this.changedCellRange, cellRange);
		}

		internal event EventHandler Changing;

		protected virtual void OnChanging()
		{
			if (this.beginUpdateCount > 0)
			{
				return;
			}
			if (this.Changing != null)
			{
				this.Changing(this, EventArgs.Empty);
			}
		}

		void OnChanged()
		{
			if (this.beginUpdateCount > 0)
			{
				return;
			}
			if (this.changedCellRange != null)
			{
				this.OnChanged(new MergedCellRangesChangedEventArgs(this.changedCellRange));
				this.changedCellRange = null;
			}
		}

		public event EventHandler<MergedCellRangesChangedEventArgs> Changed;

		protected virtual void OnChanged(MergedCellRangesChangedEventArgs args)
		{
			if (this.beginUpdateCount > 0)
			{
				return;
			}
			if (this.Changed != null)
			{
				this.Changed(this, args);
			}
		}

		int beginUpdateCount;

		CellRange changedCellRange;
	}
}
