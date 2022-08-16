using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Maths.Number;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class PageBreaks
	{
		public IEnumerable<PageBreak> HorizontalPageBreaks
		{
			get
			{
				return PageBreaks.EnumeratePageBreaks(this.horizontalPageBreaks);
			}
		}

		public IEnumerable<PageBreak> VerticalPageBreaks
		{
			get
			{
				return PageBreaks.EnumeratePageBreaks(this.verticalPageBreaks);
			}
		}

		internal IEnumerable<PageBreak> SortedHorizontalPageBreaks
		{
			get
			{
				return PageBreaks.GetSortedPageBreaks(this.HorizontalPageBreaks);
			}
		}

		internal IEnumerable<PageBreak> SortedVerticalPageBreaks
		{
			get
			{
				return PageBreaks.GetSortedPageBreaks(this.VerticalPageBreaks);
			}
		}

		internal bool HasAnyPageBreaks
		{
			get
			{
				return this.HorizontalPageBreaks.FirstOrDefault<PageBreak>() != null || this.VerticalPageBreaks.FirstOrDefault<PageBreak>() != null;
			}
		}

		internal BeginEndCounter PageBreaksChangedUpdateCounter
		{
			get
			{
				return this.pageBreaksChangedUpdateCounter;
			}
		}

		PrintArea PrintArea
		{
			get
			{
				return this.worksheet.WorksheetPageSetup.PrintArea;
			}
		}

		internal PageBreaks(Worksheet worksheet)
		{
			this.worksheet = worksheet;
			this.horizontalPageBreaks = new Dictionary<int, List<PageBreak>>();
			this.verticalPageBreaks = new Dictionary<int, List<PageBreak>>();
			this.pageBreaksChangedUpdateCounter = new BeginEndCounter(new Action(this.OnPageBreaksChanged));
		}

		public bool TryInsertPageBreaks(int rowIndex, int columnIndex)
		{
			bool flag = false;
			bool flag2 = false;
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				flag = this.TryInsertHorizontalPageBreakInternal(rowIndex, columnIndex);
				flag2 = this.TryInsertVerticalPageBreakInternal(rowIndex, columnIndex);
			}
			return flag || flag2;
		}

		public bool TryInsertHorizontalPageBreak(int rowIndex, int columnIndex)
		{
			return this.TryInsertHorizontalPageBreakInternal(rowIndex, columnIndex);
		}

		public bool TryInsertVerticalPageBreak(int rowIndex, int columnIndex)
		{
			return this.TryInsertVerticalPageBreakInternal(rowIndex, columnIndex);
		}

		public bool TryRemovePageBreaks(int rowIndex, int columnIndex)
		{
			bool flag = false;
			bool flag2 = false;
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				flag = this.TryRemoveHorizontalPageBreakInternal(rowIndex, columnIndex);
				flag2 = this.TryRemoveVerticalPageBreakInternal(rowIndex, columnIndex);
			}
			return flag || flag2;
		}

		public bool TryRemoveHorizontalPageBreak(int rowIndex, int columnIndex)
		{
			return this.TryRemoveHorizontalPageBreakInternal(rowIndex, columnIndex);
		}

		public bool TryRemoveVerticalPageBreak(int rowIndex, int columnIndex)
		{
			return this.TryRemoveVerticalPageBreakInternal(rowIndex, columnIndex);
		}

		public void Clear()
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				List<PageBreak> list = new List<PageBreak>();
				list.AddRange(this.HorizontalPageBreaks);
				list.AddRange(this.VerticalPageBreaks);
				this.RemovePageBreaks(list);
			}
		}

		internal bool TryInsertInfinitePageBreak(int pageBreakId, PageBreakType breakType)
		{
			if (pageBreakId <= 0)
			{
				return false;
			}
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				List<PageBreak> pageBreaks = ((breakType == PageBreakType.Horizontal) ? this.HorizontalBreaksCollection(pageBreakId) : this.VerticalBreaksCollection(pageBreakId));
				this.RemovePageBreaks(pageBreaks);
				this.InsertInfinitePageBreak(pageBreakId, breakType);
			}
			return true;
		}

		internal void MakeAllPageBreaksInfinite()
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				foreach (KeyValuePair<int, List<PageBreak>> keyValuePair in this.horizontalPageBreaks)
				{
					List<PageBreak> value = keyValuePair.Value;
					if (value != null && value.Count > 0 && !value[0].IsInfinite)
					{
						this.TryInsertInfinitePageBreak(keyValuePair.Key, PageBreakType.Horizontal);
					}
				}
				foreach (KeyValuePair<int, List<PageBreak>> keyValuePair2 in this.verticalPageBreaks)
				{
					List<PageBreak> value2 = keyValuePair2.Value;
					if (value2 != null && value2.Count > 0 && !value2[0].IsInfinite)
					{
						this.TryInsertInfinitePageBreak(keyValuePair2.Key, PageBreakType.Vertical);
					}
				}
			}
		}

		internal void AddPageBreak(PageBreak pageBreak)
		{
			AddRemovePageBreakCommandContext context = new AddRemovePageBreakCommandContext(this.worksheet, pageBreak);
			this.worksheet.ExecuteCommand<AddRemovePageBreakCommandContext>(WorkbookCommands.AddPageBreak, context);
		}

		internal void RemovePageBreak(PageBreak pageBreak)
		{
			AddRemovePageBreakCommandContext context = new AddRemovePageBreakCommandContext(this.worksheet, pageBreak);
			this.worksheet.ExecuteCommand<AddRemovePageBreakCommandContext>(WorkbookCommands.RemovePageBreak, context);
		}

		internal void AddPageBreakInternal(PageBreak pageBreak)
		{
			if (pageBreak.Type == PageBreakType.Horizontal)
			{
				this.HorizontalBreaksCollection(pageBreak.Index).Add(pageBreak);
			}
			else
			{
				this.VerticalBreaksCollection(pageBreak.Index).Add(pageBreak);
			}
			this.OnPageBreaksChanged();
		}

		internal void RemovePageBreakInternal(PageBreak pageBreak)
		{
			if (pageBreak.Type == PageBreakType.Horizontal)
			{
				this.HorizontalBreaksCollection(pageBreak.Index).Remove(pageBreak);
			}
			else
			{
				this.VerticalBreaksCollection(pageBreak.Index).Remove(pageBreak);
			}
			this.OnPageBreaksChanged();
		}

		internal void OnBeforePrintAreaTranslation(IEnumerable<CellRange> printAreaRangesToTranslate, CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
			this.BeginUpdate();
			List<PageBreak> pageBreaksToTranslate = this.GetPageBreaksToTranslate(printAreaRangesToTranslate, shiftType);
			this.TranslatePageBreaks(pageBreaksToTranslate, rangeToInsertOrRemove, shiftType);
		}

		internal void OnAfterPrintAreaTranslation()
		{
			this.TrimExtendPageBreaks(true);
			this.TrimExtendPageBreaks(false);
			this.EndUpdate();
		}

		void TrimExtendPageBreaks(bool areBreaksHorizontal)
		{
			List<PageBreak> list = new List<PageBreak>(areBreaksHorizontal ? this.HorizontalPageBreaks : this.VerticalPageBreaks);
			foreach (PageBreak pageBreak in list)
			{
				if (!pageBreak.IsInfinite)
				{
					int rowIndex = (areBreaksHorizontal ? pageBreak.Index : pageBreak.FromIndex);
					int columnIndex = (areBreaksHorizontal ? pageBreak.FromIndex : pageBreak.Index);
					FiniteInterval<int> printAreaContainedInterval = this.GetPrintAreaContainedInterval(rowIndex, columnIndex, areBreaksHorizontal);
					int rowIndex2 = (areBreaksHorizontal ? pageBreak.Index : pageBreak.ToIndex);
					int columnIndex2 = (areBreaksHorizontal ? pageBreak.ToIndex : pageBreak.Index);
					FiniteInterval<int> printAreaContainedInterval2 = this.GetPrintAreaContainedInterval(rowIndex2, columnIndex2, areBreaksHorizontal);
					if (printAreaContainedInterval != null || printAreaContainedInterval2 != null)
					{
						this.RemovePageBreak(pageBreak);
						if (printAreaContainedInterval != null)
						{
							this.AddPageBreak(new PageBreak(pageBreak.Type, pageBreak.Index, printAreaContainedInterval.Start, printAreaContainedInterval.End));
						}
						if (printAreaContainedInterval2 != null && !printAreaContainedInterval2.Equals(printAreaContainedInterval))
						{
							this.AddPageBreak(new PageBreak(pageBreak.Type, pageBreak.Index, printAreaContainedInterval2.Start, printAreaContainedInterval2.End));
						}
					}
				}
			}
		}

		FiniteInterval<int> GetPrintAreaContainedInterval(int rowIndex, int columnIndex, bool isHorizontalInterval)
		{
			bool flag;
			if (isHorizontalInterval)
			{
				return this.TryGetHorizontalPrintAreaContainedInterval(rowIndex, columnIndex, out flag);
			}
			return this.TryGetVerticalPrintAreaContainedInterval(rowIndex, columnIndex, out flag);
		}

		List<PageBreak> GetPageBreaksToTranslate(IEnumerable<CellRange> printAreaRangesToTranslate, ShiftType shiftType)
		{
			List<PageBreak> list = new List<PageBreak>();
			bool flag = shiftType == ShiftType.Down || shiftType == ShiftType.Up;
			IEnumerable<PageBreak> collection = (flag ? this.HorizontalPageBreaks : this.VerticalPageBreaks);
			IEnumerable<PageBreak> enumerable = (flag ? this.VerticalPageBreaks : this.HorizontalPageBreaks);
			list.AddRange(collection);
			foreach (PageBreak pageBreak in enumerable)
			{
				if (PageBreaks.ShouldTranslate(pageBreak, printAreaRangesToTranslate))
				{
					list.Add(pageBreak);
				}
			}
			return list;
		}

		static bool ShouldTranslate(PageBreak pageBreak, IEnumerable<CellRange> printAreaRangesToTranslate)
		{
			bool flag = pageBreak.Type == PageBreakType.Horizontal;
			foreach (CellRange cellRange in printAreaRangesToTranslate)
			{
				int num = cellRange.StartIndex(!flag);
				int num2 = cellRange.EndIndex(!flag);
				bool flag2 = (num == pageBreak.FromIndex || num2 == pageBreak.ToIndex) && cellRange.StartIndex(flag) <= pageBreak.Index && pageBreak.Index <= cellRange.EndIndex(flag);
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		void TranslatePageBreaks(List<PageBreak> pageBreaks, CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
			foreach (PageBreak pageBreak in pageBreaks)
			{
				PageBreak pageBreak2;
				if (pageBreak.TryTranslate(rangeToInsertOrRemove, shiftType, out pageBreak2))
				{
					this.RemovePageBreak(pageBreak);
					if (pageBreak2 != null)
					{
						this.AddPageBreak(pageBreak2);
					}
				}
			}
		}

		bool TryRemoveHorizontalPageBreakInternal(int rowIndex, int columnIndex)
		{
			List<PageBreak> pageBreakCollection = this.HorizontalBreaksCollection(rowIndex);
			PageBreak pageBreak = PageBreaks.TryGetExistingPageBreak(pageBreakCollection, columnIndex);
			if (pageBreak != null)
			{
				this.RemovePageBreak(pageBreak);
				return true;
			}
			return false;
		}

		bool TryRemoveVerticalPageBreakInternal(int rowIndex, int columnIndex)
		{
			List<PageBreak> pageBreakCollection = this.VerticalBreaksCollection(columnIndex);
			PageBreak pageBreak = PageBreaks.TryGetExistingPageBreak(pageBreakCollection, rowIndex);
			if (pageBreak != null)
			{
				this.RemovePageBreak(pageBreak);
				return true;
			}
			return false;
		}

		bool TryInsertHorizontalPageBreakInternal(int rowIndex, int columnIndex)
		{
			List<PageBreak> pageBreakCollection = this.HorizontalBreaksCollection(rowIndex);
			PageBreak pageBreak = PageBreaks.TryGetExistingPageBreak(pageBreakCollection, columnIndex);
			if (pageBreak != null)
			{
				return false;
			}
			bool flag;
			FiniteInterval<int> finiteInterval = this.TryGetHorizontalPrintAreaContainedInterval(rowIndex, columnIndex, out flag);
			if (finiteInterval == null)
			{
				return this.TryInsertInfinitePageBreak(rowIndex, PageBreakType.Horizontal);
			}
			if (!flag)
			{
				this.AddPageBreak(new PageBreak(PageBreakType.Horizontal, rowIndex, finiteInterval.Start, finiteInterval.End));
				return true;
			}
			return false;
		}

		FiniteInterval<int> TryGetHorizontalPrintAreaContainedInterval(int rowIndex, int columnIndex, out bool isOnlyOnTopRowsInAreas)
		{
			isOnlyOnTopRowsInAreas = true;
			FiniteIntervalsUnion<int> finiteIntervalsUnion = new FiniteIntervalsUnion<int>();
			foreach (CellRange cellRange in this.PrintArea.Ranges)
			{
				if (cellRange.Contains(rowIndex, columnIndex) && cellRange.FromIndex.RowIndex < rowIndex)
				{
					isOnlyOnTopRowsInAreas = false;
				}
				if (cellRange.FromIndex.RowIndex <= rowIndex && rowIndex <= cellRange.ToIndex.RowIndex)
				{
					finiteIntervalsUnion.AddInterval(cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex, false, false);
				}
			}
			return finiteIntervalsUnion.GetContainingInterval(columnIndex);
		}

		bool TryInsertVerticalPageBreakInternal(int rowIndex, int columnIndex)
		{
			List<PageBreak> pageBreakCollection = this.VerticalBreaksCollection(columnIndex);
			PageBreak pageBreak = PageBreaks.TryGetExistingPageBreak(pageBreakCollection, rowIndex);
			if (pageBreak != null)
			{
				return false;
			}
			bool flag;
			FiniteInterval<int> finiteInterval = this.TryGetVerticalPrintAreaContainedInterval(rowIndex, columnIndex, out flag);
			if (finiteInterval == null)
			{
				return this.TryInsertInfinitePageBreak(columnIndex, PageBreakType.Vertical);
			}
			if (!flag)
			{
				this.AddPageBreak(new PageBreak(PageBreakType.Vertical, columnIndex, finiteInterval.Start, finiteInterval.End));
				return true;
			}
			return false;
		}

		FiniteInterval<int> TryGetVerticalPrintAreaContainedInterval(int rowIndex, int columnIndex, out bool isOnlyOnLeftColumnsInAreas)
		{
			isOnlyOnLeftColumnsInAreas = true;
			FiniteIntervalsUnion<int> finiteIntervalsUnion = new FiniteIntervalsUnion<int>();
			foreach (CellRange cellRange in this.PrintArea.Ranges)
			{
				if (cellRange.Contains(rowIndex, columnIndex) && cellRange.FromIndex.ColumnIndex < columnIndex)
				{
					isOnlyOnLeftColumnsInAreas = false;
				}
				if (cellRange.FromIndex.ColumnIndex <= columnIndex && columnIndex <= cellRange.ToIndex.ColumnIndex)
				{
					finiteIntervalsUnion.AddInterval(cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex, false, false);
				}
			}
			return finiteIntervalsUnion.GetContainingInterval(rowIndex);
		}

		void RemovePageBreaks(IEnumerable<PageBreak> pageBreaks)
		{
			List<PageBreak> list = new List<PageBreak>(pageBreaks);
			foreach (PageBreak pageBreak in list)
			{
				this.RemovePageBreak(pageBreak);
			}
		}

		void InsertInfinitePageBreak(int pageBreakId, PageBreakType breakType)
		{
			int toIndex = ((breakType == PageBreakType.Horizontal) ? PageBreak.HorizontalPageBreakMaximumIndex : PageBreak.VerticalPageBreakMaximumIndex);
			this.AddPageBreak(new PageBreak(breakType, pageBreakId, 0, toIndex));
		}

		List<PageBreak> HorizontalBreaksCollection(int rowIndex)
		{
			List<PageBreak> list;
			if (!this.horizontalPageBreaks.TryGetValue(rowIndex, out list))
			{
				list = new List<PageBreak>();
				this.horizontalPageBreaks.Add(rowIndex, list);
			}
			return list;
		}

		List<PageBreak> VerticalBreaksCollection(int columnIndex)
		{
			List<PageBreak> list;
			if (!this.verticalPageBreaks.TryGetValue(columnIndex, out list))
			{
				list = new List<PageBreak>();
				this.verticalPageBreaks.Add(columnIndex, list);
			}
			return list;
		}

		void BeginUpdate()
		{
			this.PageBreaksChangedUpdateCounter.BeginUpdate();
			this.worksheet.BeginUndoGroup();
		}

		void EndUpdate()
		{
			this.worksheet.EndUndoGroup();
			this.PageBreaksChangedUpdateCounter.EndUpdate();
		}

		static PageBreak TryGetExistingPageBreak(List<PageBreak> pageBreakCollection, int breakInsertionPoint)
		{
			for (int i = 0; i < pageBreakCollection.Count; i++)
			{
				PageBreak pageBreak = pageBreakCollection[i];
				if (pageBreak.FromIndex <= breakInsertionPoint && breakInsertionPoint <= pageBreak.ToIndex)
				{
					return pageBreak;
				}
			}
			return null;
		}

		static IEnumerable<PageBreak> EnumeratePageBreaks(Dictionary<int, List<PageBreak>> pageBreaks)
		{
			foreach (List<PageBreak> breaksCollection in pageBreaks.Values)
			{
				foreach (PageBreak pageBreak in breaksCollection)
				{
					yield return pageBreak;
				}
			}
			yield break;
		}

		static PageBreak[] GetSortedPageBreaks(IEnumerable<PageBreak> pageBreaks)
		{
			return (from pageBreak in pageBreaks
				orderby pageBreak.Index
				select pageBreak).ToArray<PageBreak>();
		}

		internal void CopyFrom(PageBreaks fromPageBreaks)
		{
			foreach (KeyValuePair<int, List<PageBreak>> keyValuePair in fromPageBreaks.horizontalPageBreaks)
			{
				List<PageBreak> list = new List<PageBreak>();
				foreach (PageBreak item in keyValuePair.Value)
				{
					list.Add(item);
				}
				this.horizontalPageBreaks[keyValuePair.Key] = list;
			}
			foreach (KeyValuePair<int, List<PageBreak>> keyValuePair2 in fromPageBreaks.verticalPageBreaks)
			{
				List<PageBreak> list2 = new List<PageBreak>();
				foreach (PageBreak item2 in keyValuePair2.Value)
				{
					list2.Add(item2);
				}
				this.verticalPageBreaks[keyValuePair2.Key] = new List<PageBreak>(keyValuePair2.Value);
			}
		}

		internal event EventHandler PageBreaksChanged;

		void OnPageBreaksChanged()
		{
			if (this.PageBreaksChangedUpdateCounter.BeginUpdateCounter > 0)
			{
				return;
			}
			if (this.PageBreaksChanged != null)
			{
				this.PageBreaksChanged(this, EventArgs.Empty);
			}
		}

		readonly Worksheet worksheet;

		readonly Dictionary<int, List<PageBreak>> horizontalPageBreaks;

		readonly Dictionary<int, List<PageBreak>> verticalPageBreaks;

		readonly BeginEndCounter pageBreaksChangedUpdateCounter;
	}
}
