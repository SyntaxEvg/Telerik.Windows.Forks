using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class PrintArea : CellRangeInsertedOrRemovedAwareOrderedCollection
	{
		public IEnumerable<CellRange> Ranges
		{
			get
			{
				for (int i = 0; i < this.PrintRanges.Count; i++)
				{
					yield return this.PrintRanges[i];
				}
				yield break;
			}
		}

		public bool HasPrintAreaRanges
		{
			get
			{
				return this.PrintRanges.Count > 0;
			}
		}

		internal int RangesCount
		{
			get
			{
				return this.PrintRanges.Count;
			}
		}

		internal BeginEndCounter PrintAreaChangedUpdateCounter
		{
			get
			{
				return this.printAreaChangedUpdateCounter;
			}
		}

		List<CellRange> PrintRanges
		{
			get
			{
				return base.RangesList;
			}
		}

		PageBreaks PageBreaks
		{
			get
			{
				return this.worksheet.WorksheetPageSetup.PageBreaks;
			}
		}

		internal PrintArea(Worksheet worksheet)
			: base(worksheet.Cells)
		{
			this.worksheet = worksheet;
			this.printAreaChangedUpdateCounter = new BeginEndCounter(new Action(this.OnPrintAreaChanged));
		}

		public void SetPrintArea(CellRange range)
		{
			this.SetPrintArea(new CellRange[] { range });
		}

		public void SetPrintArea(IEnumerable<CellRange> ranges)
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.Clear();
				this.TryAddToPrintArea(ranges);
			}
		}

		public bool TryAddToPrintArea(CellRange range)
		{
			return this.TryAddToPrintArea(new CellRange[] { range });
		}

		public bool TryAddToPrintArea(IEnumerable<CellRange> ranges)
		{
			if (this.CanAddToPrintArea(ranges))
			{
				this.InsertRange(this.RangesCount, ranges);
				return true;
			}
			return false;
		}

		public bool CanAddToPrintArea(CellRange range)
		{
			return this.CanAddToPrintArea(new CellRange[] { range });
		}

		public bool CanAddToPrintArea(IEnumerable<CellRange> ranges)
		{
			foreach (CellRange cellRange in ranges)
			{
				foreach (CellRange other in this.Ranges)
				{
					if (cellRange.IntersectsWith(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		public void Clear()
		{
			if (this.HasPrintAreaRanges)
			{
				using (new UpdateScope(new Action(this.worksheet.BeginUndoGroup), new Action(this.worksheet.EndUndoGroup)))
				{
					this.RemoveRange(0, this.RangesCount);
				}
			}
		}

		internal void InsertAt(int index, CellRange range)
		{
			this.InsertRange(index, new CellRange[] { range });
		}

		internal void InsertRange(int index, IEnumerable<CellRange> ranges)
		{
			AddToRemoveFromPrintAreaCommandContext context = new AddToRemoveFromPrintAreaCommandContext(this.worksheet, ranges, index);
			this.worksheet.ExecuteCommand<AddToRemoveFromPrintAreaCommandContext>(WorkbookCommands.AddToPrintArea, context);
		}

		internal void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		internal void RemoveRange(int index, int count)
		{
			IEnumerable<CellRange> range = this.PrintRanges.GetRange(index, count);
			AddToRemoveFromPrintAreaCommandContext context = new AddToRemoveFromPrintAreaCommandContext(this.worksheet, range, index);
			this.worksheet.ExecuteCommand<AddToRemoveFromPrintAreaCommandContext>(WorkbookCommands.RemoveFromPrintArea, context);
		}

		internal void InsertRangesInternal(IEnumerable<CellRange> range, int index)
		{
			this.PrintRanges.InsertRange(index, range);
			this.OnPrintAreaChanged();
		}

		internal void RemoveRangesInternal(int index, int count)
		{
			this.PrintRanges.RemoveRange(index, count);
			this.OnPrintAreaChanged();
		}

		internal void CopyFrom(PrintArea fromPrintArea)
		{
			foreach (CellRange item in fromPrintArea.CellRanges)
			{
				this.CellRanges.Add(item);
			}
		}

		protected override void Insert(int index, CellRange range)
		{
			this.InsertAt(index, range);
		}

		protected override void RemoveCellRangeAt(int index)
		{
			this.RemoveAt(index);
		}

		protected override void OnBeforeTranslation(Dictionary<CellRange, CellRange> oldAndNewTranslatedRangesPositions, CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
			this.BeginUpdate();
			this.PageBreaks.OnBeforePrintAreaTranslation(oldAndNewTranslatedRangesPositions.Keys, rangeToInsertOrRemove, shiftType);
		}

		protected override void OnAfterTranslation(Dictionary<CellRange, CellRange> oldAndNewTranslatedRangesPositions, CellRange rangeToInsertOrRemove, ShiftType shiftType)
		{
			this.PageBreaks.OnAfterPrintAreaTranslation();
			this.EndUpdate();
		}

		void BeginUpdate()
		{
			this.PrintAreaChangedUpdateCounter.BeginUpdate();
			this.worksheet.BeginUndoGroup();
		}

		void EndUpdate()
		{
			this.worksheet.EndUndoGroup();
			this.PrintAreaChangedUpdateCounter.EndUpdate();
		}

		internal event EventHandler PrintAreaChanged;

		void OnPrintAreaChanged()
		{
			if (this.PrintAreaChangedUpdateCounter.BeginUpdateCounter > 0)
			{
				return;
			}
			if (this.PrintAreaChanged != null)
			{
				this.PrintAreaChanged(this, EventArgs.Empty);
			}
		}

		readonly Worksheet worksheet;

		readonly BeginEndCounter printAreaChangedUpdateCounter;
	}
}
