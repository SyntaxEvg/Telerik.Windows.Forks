using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class AutoFilter
	{
		public CellRange FilterRange
		{
			get
			{
				return this.filterRange;
			}
			set
			{
				if (this.filterRange != value)
				{
					using (new UpdateScope(new Action(this.worksheet.BeginUndoGroup), new Action(this.worksheet.EndUndoGroup)))
					{
						if (this.filterRange != null)
						{
							this.ClearFilters();
						}
						SetFilterRangeCommandContext context = new SetFilterRangeCommandContext(this.worksheet, value);
						WorkbookCommands.SetFilterRange.Execute(context);
					}
				}
			}
		}

		internal CellRange ActualFilteredRange
		{
			get
			{
				return SortAndFilterHelper.GetActualFilteredRange(this.filterRange, this.worksheet);
			}
		}

		internal List<IFilter> Filters
		{
			get
			{
				return this.filters;
			}
			set
			{
				if (this.filters != value)
				{
					this.filters = value;
					this.SyncFilterColumnListToDictionary();
				}
			}
		}

		internal bool FilterIsApplied
		{
			get
			{
				return this.Filters.Count != 0;
			}
		}

		internal AutoFilter(Worksheet owner)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(owner, "owner");
			this.worksheet = owner;
			this.filterManager = new FilterManager(this.worksheet);
			this.filters = new List<IFilter>();
			this.relativeColumnIndexToFilter = new Dictionary<int, IFilter>();
			this.collectionChangedCounter = new BeginEndCounter(new Action(this.OnFiltersChanged));
		}

		public void SetFilters(IEnumerable<IFilter> filters)
		{
			using (new UpdateScope(new Action(this.worksheet.BeginUndoGroup), new Action(this.worksheet.EndUndoGroup)))
			{
				foreach (IFilter filter in filters)
				{
					this.SetFilter(filter);
				}
			}
		}

		public void SetFilter(IFilter filter)
		{
			Guard.ThrowExceptionIfNull<IFilter>(filter, "filter");
			this.CheckIfRangeIsNull();
			Guard.ThrowExceptionIfOutOfRange<int>(0, this.filterRange.ColumnCount - 1, filter.RelativeColumnIndex, "relativeColumnIndex");
			IFilter oldFilter = (this.relativeColumnIndexToFilter.ContainsKey(filter.RelativeColumnIndex) ? this.relativeColumnIndexToFilter[filter.RelativeColumnIndex] : null);
			SetRemoveFilterCommandContext context = new SetRemoveFilterCommandContext(this.worksheet, filter, oldFilter);
			WorkbookCommands.SetFilter.Execute(context);
		}

		internal void SetFilterInternal(IFilter filter)
		{
			Guard.ThrowExceptionIfNull<IFilter>(filter, "filter");
			this.CheckIfRangeIsNull();
			Guard.ThrowExceptionIfOutOfRange<int>(0, this.filterRange.ColumnCount - 1, filter.RelativeColumnIndex, "relativeColumnIndex");
			this.SetFilterWithoutApplying(filter);
			this.filterManager.FilterSheet(this.ActualFilteredRange, filter);
		}

		internal void SetFilterWithoutApplying(IFilter filter)
		{
			Guard.ThrowExceptionIfNull<IFilter>(filter, "filter");
			this.CheckIfRangeIsNull();
			if (this.relativeColumnIndexToFilter.ContainsKey(filter.RelativeColumnIndex))
			{
				IFilter filter2 = this.relativeColumnIndexToFilter[filter.RelativeColumnIndex];
				this.filterManager.ClearFilter(filter2);
				this.filters.Remove(filter2);
				this.relativeColumnIndexToFilter.Remove(filter2.RelativeColumnIndex);
			}
			IWorksheetFilter worksheetFilter = filter as IWorksheetFilter;
			if (worksheetFilter != null)
			{
				worksheetFilter.SetWorksheet(this.worksheet);
			}
			IRangeFilter rangeFilter = filter as IRangeFilter;
			if (rangeFilter != null)
			{
				CellRange columnRange = ((this.ActualFilteredRange == null) ? null : this.ActualFilteredRange.GetColumn(this.filterRange.FromIndex.ColumnIndex + filter.RelativeColumnIndex));
				rangeFilter.SetColumnRange(columnRange);
			}
			this.filters.Add(filter);
			this.relativeColumnIndexToFilter.Add(filter.RelativeColumnIndex, filter);
			this.DoOnFiltersChanged();
		}

		public IFilter GetFilter(int relativeColumnIndex)
		{
			if (this.relativeColumnIndexToFilter.ContainsKey(relativeColumnIndex))
			{
				return this.relativeColumnIndexToFilter[relativeColumnIndex];
			}
			return null;
		}

		public bool RemoveFilter(IFilter filter)
		{
			return this.RemoveFilter(filter.RelativeColumnIndex);
		}

		public bool RemoveFilter(int relativeColumnIndex)
		{
			this.CheckIfRangeIsNull();
			Guard.ThrowExceptionIfOutOfRange<int>(0, this.filterRange.ColumnCount - 1, relativeColumnIndex, "relativeColumnIndex");
			if (!this.relativeColumnIndexToFilter.ContainsKey(relativeColumnIndex))
			{
				return false;
			}
			SetRemoveFilterCommandContext context = new SetRemoveFilterCommandContext(this.worksheet, null, this.relativeColumnIndexToFilter[relativeColumnIndex]);
			return WorkbookCommands.RemoveFilter.Execute(context);
		}

		internal void RemoveFilterInternal(int relativeColumnIndex)
		{
			this.CheckIfRangeIsNull();
			Guard.ThrowExceptionIfOutOfRange<int>(0, this.filterRange.ColumnCount - 1, relativeColumnIndex, "relativeColumnIndex");
			if (!this.relativeColumnIndexToFilter.ContainsKey(relativeColumnIndex))
			{
				throw new FilteringException("This column is not filtered.", new InvalidOperationException("This column is not filtered."), "Spreadsheet_Filtering_UnfilteredColumn");
			}
			IFilter filter = this.relativeColumnIndexToFilter[relativeColumnIndex];
			this.RemoveFilterWithoutUnhidingRows(relativeColumnIndex);
			this.filterManager.ClearFilter(filter);
		}

		void RemoveFilterWithoutUnhidingRows(int relativeColumnIndex)
		{
			IFilter item = this.relativeColumnIndexToFilter[relativeColumnIndex];
			this.relativeColumnIndexToFilter.Remove(relativeColumnIndex);
			this.filters.Remove(item);
			this.DoOnFiltersChanged();
		}

		public void ClearFilters()
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.worksheet.BeginUndoGroup();
				this.collectionChangedCounter.BeginUpdate();
			}, delegate()
			{
				this.collectionChangedCounter.EndUpdate();
				this.worksheet.EndUndoGroup();
			});
			using (updateScope)
			{
				foreach (IFilter filter in this.filters.ToList<IFilter>())
				{
					this.RemoveFilter(filter.RelativeColumnIndex);
				}
			}
		}

		public void ReapplyFilter(IFilter filter)
		{
			this.ReapplyFilter(filter.RelativeColumnIndex);
		}

		public void ReapplyFilter(int relativeColumnIndex)
		{
			if (!this.relativeColumnIndexToFilter.ContainsKey(relativeColumnIndex))
			{
				throw new FilteringException("No filter was found to reapply.", new InvalidOperationException("No filter was found to reapply."), "Spreadsheet_Filtering_NoFilterFound");
			}
			IFilter filter = this.relativeColumnIndexToFilter[relativeColumnIndex];
			this.SetFilter(filter);
		}

		internal void SetFilterRangeInternal(CellRange newFilterRange)
		{
			if (this.filterRange != newFilterRange)
			{
				this.filterRange = newFilterRange;
				this.OnFilterRangeChanged();
			}
		}

		internal void CopyFrom(AutoFilter fromAutoFilter, CopyContext context)
		{
			this.filterRange = fromAutoFilter.FilterRange;
			List<IFilter> list = new List<IFilter>();
			foreach (ICopyable<IFilter> copyable in fromAutoFilter.Filters.Cast<ICopyable<IFilter>>())
			{
				list.Add(copyable.Copy(context));
			}
			this.filterManager.CopyFrom(fromAutoFilter.filterManager);
			this.Filters = list;
		}

		internal ICompressedList<bool> GetRowsHiddenUniquelyByFilter(int relativeColumnIndex)
		{
			return this.filterManager.GetRowsHiddenUniquelyByFilter(relativeColumnIndex);
		}

		internal ICompressedList<bool> GetHiddenRowsState(int relativeColumnIndex)
		{
			return this.filterManager.GetHiddenRowsState(relativeColumnIndex);
		}

		internal void RestoreHiddenRowsState(int relativeColumnIndex, ICompressedList<bool> state)
		{
			this.filterManager.RestoreHiddenRowsState(relativeColumnIndex, state);
		}

		internal bool CanInsertOrRemove(ShiftType shiftType, CellRange selectedRange)
		{
			if (this.filterRange == null)
			{
				return true;
			}
			if (this.FilterIsApplied)
			{
				return this.InsertionIsAfterFilteredRange(selectedRange);
			}
			return this.FilterRange.CanInsertOrRemove(shiftType, selectedRange);
		}

		bool InsertionIsAfterFilteredRange(CellRange selectedRange)
		{
			return selectedRange.FromIndex.RowIndex > this.FilterRange.ToIndex.RowIndex || selectedRange.FromIndex.ColumnIndex > this.FilterRange.ToIndex.ColumnIndex;
		}

		internal void Update(CellRangeInsertedOrRemovedEventArgs eventArgs)
		{
			ShiftType shiftType = eventArgs.RangeType.ToShiftType(eventArgs.IsRemove);
			CellRange range = eventArgs.Range;
			if (this.filterRange == null)
			{
				return;
			}
			using (new UpdateScope(new Action(this.worksheet.BeginUndoGroup), new Action(this.worksheet.EndUndoGroup)))
			{
				bool flag = this.FilterIsApplied && range.FromIndex.RowIndex <= this.FilterRange.ToIndex.RowIndex && (shiftType == ShiftType.Down || shiftType == ShiftType.Up);
				bool flag2 = this.FilterIsApplied && range.FromIndex.ColumnIndex > this.FilterRange.FromIndex.ColumnIndex && range.FromIndex.ColumnIndex <= this.FilterRange.ToIndex.ColumnIndex && (shiftType == ShiftType.Left || shiftType == ShiftType.Right);
				this.TranslateRange(range, shiftType);
				if (flag2)
				{
					this.TranslateFiltersHorizontally(range, shiftType);
				}
				if (flag)
				{
					this.TranslateFiltersVertically(range, shiftType);
				}
			}
		}

		void TranslateRange(CellRange insertRemoveRange, ShiftType shiftType)
		{
			CellRange cellRange;
			bool flag = this.FilterRange.TryTranslate(insertRemoveRange, shiftType, out cellRange);
			if (flag)
			{
				if (cellRange == null)
				{
					this.FilterRange = cellRange;
					return;
				}
				SetFilterRangeCommandContext context = new SetFilterRangeCommandContext(this.worksheet, cellRange);
				WorkbookCommands.SetFilterRange.Execute(context);
			}
		}

		void TranslateFiltersHorizontally(CellRange insertRemoveRange, ShiftType shiftType)
		{
			List<IFilter> list = new List<IFilter>(this.filters);
			int relativeRearrangeIndex = InsertRemoveCellsHelper.GetRelativeRearrangeIndex(this.filterRange, insertRemoveRange);
			int rearrangeDelta = InsertRemoveCellsHelper.GetRearrangeDelta(insertRemoveRange, shiftType);
			RemoveShiftType removeShiftType;
			bool flag = shiftType.TryGetRemoveShiftType(out removeShiftType);
			foreach (IFilter filter in list)
			{
				if (flag && this.ShouldRemoveFilter(filter.RelativeColumnIndex, insertRemoveRange))
				{
					this.RemoveFilter(filter);
				}
				else if (filter.RelativeColumnIndex >= relativeRearrangeIndex)
				{
					int newIndex = filter.RelativeColumnIndex;
					newIndex = filter.RelativeColumnIndex + rearrangeDelta;
					MoveFilterCommandContext context = new MoveFilterCommandContext(this.worksheet, filter, newIndex);
					WorkbookCommands.MoveFilter.Execute(context);
				}
			}
		}

		void TranslateFiltersVertically(CellRange insertRemoveRange, ShiftType shiftType)
		{
			int rowIndex = insertRemoveRange.FromIndex.RowIndex;
			int rowCount = insertRemoveRange.RowCount;
			RearrangeFilterRowsCommandContext context = new RearrangeFilterRowsCommandContext(this.worksheet, rowCount, rowIndex, shiftType);
			WorkbookCommands.RearrangeFilterRows.Execute(context);
		}

		internal void RearrangeFilterRows(int absoluteRearrangeIndex, int delta, ShiftType shiftType)
		{
			this.filterManager.RearrangeRows(absoluteRearrangeIndex, delta, shiftType);
		}

		internal IFilter MoveFilter(IFilter filter, int newIndex)
		{
			IFilter filter2 = (IFilter)((ITranslatable)filter).Copy(newIndex);
			this.RemoveFilterWithoutUnhidingRows(filter.RelativeColumnIndex);
			this.SetFilterWithoutApplying(filter2);
			this.filterManager.MoveFilter(filter2.RelativeColumnIndex, filter.RelativeColumnIndex);
			return filter2;
		}

		bool ShouldRemoveFilter(int filterRelativeIndex, CellRange removedRange)
		{
			int num = this.FilterRange.FromIndex.ColumnIndex + filterRelativeIndex;
			return num >= removedRange.FromIndex.ColumnIndex && num <= removedRange.ToIndex.ColumnIndex;
		}

		void CheckIfRangeIsNull()
		{
			if (this.FilterRange == null)
			{
				throw new FilteringException("The filter range must be assigned before a filter is applied.", new InvalidOperationException("The filter range must be assigned before a filter is applied."), "Spreadsheet_Filtering_RangeMustBeAssigned");
			}
		}

		void SyncFilterColumnListToDictionary()
		{
			this.relativeColumnIndexToFilter = new Dictionary<int, IFilter>();
			foreach (IFilter filter in this.filters)
			{
				this.relativeColumnIndexToFilter.Add(filter.RelativeColumnIndex, filter);
			}
		}

		void DoOnFiltersChanged()
		{
			if (!this.collectionChangedCounter.IsUpdateInProgress)
			{
				this.OnFiltersChanged();
			}
		}

		internal event EventHandler FiltersChanged;

		void OnFiltersChanged()
		{
			if (this.FiltersChanged != null)
			{
				this.FiltersChanged(this, EventArgs.Empty);
			}
		}

		internal event EventHandler FilterRangeChanged;

		void OnFilterRangeChanged()
		{
			if (this.FilterRangeChanged != null)
			{
				this.FilterRangeChanged(this, EventArgs.Empty);
			}
		}

		CellRange filterRange;

		readonly FilterManager filterManager;

		readonly Worksheet worksheet;

		readonly BeginEndCounter collectionChangedCounter;

		List<IFilter> filters;

		Dictionary<int, IFilter> relativeColumnIndexToFilter;
	}
}
