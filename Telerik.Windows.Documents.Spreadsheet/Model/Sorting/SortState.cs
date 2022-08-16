using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public class SortState
	{
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		public IEnumerable<ISortCondition> SortConditions
		{
			get
			{
				return this.innerList;
			}
		}

		public CellRange SortRange
		{
			get
			{
				return this.sortRange;
			}
			internal set
			{
				if (this.sortRange != value)
				{
					SetSortRangeCommandContext context = new SetSortRangeCommandContext(this.worksheet, value);
					WorkbookCommands.SetSortRange.Execute(context);
				}
			}
		}

		internal SortState(Worksheet worksheet)
		{
			this.worksheet = worksheet;
			this.innerList = new List<ISortCondition>();
		}

		public void Set(CellRange sortRange, params ISortCondition[] sortConditions)
		{
			Guard.ThrowExceptionIfNull<CellRange>(sortRange, "sortRange");
			this.worksheet.Cells[sortRange].Sort(sortConditions);
		}

		public void Clear()
		{
			ClearSortCommandContext context = new ClearSortCommandContext(this.worksheet);
			this.worksheet.ExecuteCommand<ClearSortCommandContext>(WorkbookCommands.ClearSort, context);
		}

		internal void CopyFrom(SortState fromSortState, CopyContext context)
		{
			List<ISortCondition> list = new List<ISortCondition>();
			foreach (ISortCondition sortCondition in fromSortState.SortConditions)
			{
				ISortCondition item = ((ICopyable<ISortCondition>)sortCondition).Copy(context);
				list.Add(item);
			}
			this.SetInternal(fromSortState.SortRange, list.ToArray());
		}

		internal void SetInternal(CellRange sortRange, params ISortCondition[] sortConditions)
		{
			this.sortRange = sortRange;
			this.innerList.AddRange(sortConditions);
		}

		internal void Remove(ISortCondition sortCondition)
		{
			this.innerList.Remove(sortCondition);
		}

		internal ISortCondition this[int index]
		{
			get
			{
				return this.innerList[index];
			}
			set
			{
				this.innerList[index] = value;
			}
		}

		internal void Insert(int index, ISortCondition condition)
		{
			this.innerList.Insert(index, condition);
		}

		internal void AddRange(IEnumerable<ISortCondition> sortConditions)
		{
			this.innerList.AddRange(sortConditions);
		}

		internal void AddFirstInternal(ISortCondition sortCondition)
		{
			this.innerList.Insert(0, sortCondition);
		}

		internal void RemoveFirst()
		{
			if (this.innerList.Count > 0)
			{
				this.innerList.RemoveAt(0);
			}
		}

		internal void ClearInternal()
		{
			this.SetSortRangeInternal(null);
			this.innerList.Clear();
		}

		internal void SetSortRangeInternal(CellRange range)
		{
			if (this.sortRange != range)
			{
				this.sortRange = range;
			}
		}

		internal void Update(CellRangeInsertedOrRemovedEventArgs eventArgs)
		{
			ShiftType shiftType = eventArgs.RangeType.ToShiftType(eventArgs.IsRemove);
			CellRange range = eventArgs.Range;
			if (this.SortRange == null)
			{
				return;
			}
			using (new UpdateScope(new Action(this.worksheet.BeginUndoGroup), new Action(this.worksheet.EndUndoGroup)))
			{
				bool flag = this.Count != 0 && range.FromIndex.ColumnIndex > this.SortRange.FromIndex.ColumnIndex && range.FromIndex.ColumnIndex <= this.SortRange.ToIndex.ColumnIndex && (shiftType == ShiftType.Left || shiftType == ShiftType.Right) && this.SortRange.CanInsertOrRemove(shiftType, range);
				this.TranslateRange(eventArgs.Range, shiftType);
				if (flag)
				{
					this.TranslateConditions(range, shiftType);
				}
			}
		}

		void TranslateRange(CellRange insertRemoveRange, ShiftType shiftType)
		{
			CellRange cellRange;
			bool flag = this.SortRange.TryTranslate(insertRemoveRange, shiftType, out cellRange);
			if (flag)
			{
				if (cellRange != null)
				{
					this.SortRange = cellRange;
					return;
				}
				this.Clear();
			}
		}

		void TranslateConditions(CellRange insertRemoveRange, ShiftType shiftType)
		{
			List<ISortCondition> list = new List<ISortCondition>(this.SortConditions);
			int relativeRearrangeIndex = InsertRemoveCellsHelper.GetRelativeRearrangeIndex(this.sortRange, insertRemoveRange);
			int rearrangeDelta = InsertRemoveCellsHelper.GetRearrangeDelta(insertRemoveRange, shiftType);
			RemoveShiftType removeShiftType;
			bool flag = shiftType.TryGetRemoveShiftType(out removeShiftType);
			foreach (ISortCondition sortCondition in list)
			{
				int orderIndex = this.innerList.IndexOf(sortCondition);
				if (flag && this.ShouldRemoveCondition(sortCondition.RelativeIndex, insertRemoveRange))
				{
					RemoveSortConditionCommandContext context = new RemoveSortConditionCommandContext(this.worksheet, sortCondition, orderIndex);
					WorkbookCommands.RemoveSortCondition.Execute(context);
				}
				else if (sortCondition.RelativeIndex >= relativeRearrangeIndex)
				{
					int newIndex = sortCondition.RelativeIndex;
					newIndex = sortCondition.RelativeIndex + rearrangeDelta;
					MoveSortConditionCommandContext context2 = new MoveSortConditionCommandContext(this.worksheet, sortCondition, newIndex, orderIndex);
					WorkbookCommands.MoveSortCondition.Execute(context2);
				}
			}
		}

		bool ShouldRemoveCondition(int conditionRelativeIndex, CellRange removedRange)
		{
			int num = this.sortRange.FromIndex.ColumnIndex + conditionRelativeIndex;
			return num >= removedRange.FromIndex.ColumnIndex && num <= removedRange.ToIndex.ColumnIndex;
		}

		readonly Worksheet worksheet;

		readonly List<ISortCondition> innerList;

		CellRange sortRange;
	}
}
