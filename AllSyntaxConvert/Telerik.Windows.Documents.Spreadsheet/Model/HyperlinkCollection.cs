using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class HyperlinkCollection : CellRangeInsertedOrRemovedAwareCollection<SpreadsheetHyperlink>
	{
		public HyperlinkCollection(Worksheet worksheet)
			: base(worksheet)
		{
		}

		internal void Sort(CellRange sortedRange, int[] sortedIndexes)
		{
			Guard.ThrowExceptionIfNotEqual<int>(sortedRange.RowCount, sortedIndexes.Length, "RowCount");
			this.OnChanging();
			SpreadsheetHyperlink[] array = (from p in this.GetContainingHyperlinks(sortedRange)
				where p.Range.RowCount == 1
				select p).ToArray<SpreadsheetHyperlink>();
			List<SpreadsheetHyperlink>[] array2 = new List<SpreadsheetHyperlink>[sortedIndexes.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = new List<SpreadsheetHyperlink>();
			}
			int rowIndex = sortedRange.FromIndex.RowIndex;
			int rowIndex2 = sortedRange.ToIndex.RowIndex;
			int row;
			for (row = rowIndex; row <= rowIndex2; row++)
			{
				array2[row - rowIndex].AddRange(from p in array
					where p.Range.FromIndex.RowIndex == row
					select p);
			}
			foreach (SpreadsheetHyperlink spreadsheetHyperlink in array)
			{
				base.Elements.Remove(spreadsheetHyperlink.Range);
			}
			for (int k = 0; k < sortedIndexes.Length; k++)
			{
				foreach (SpreadsheetHyperlink spreadsheetHyperlink2 in array2[sortedIndexes[k]])
				{
					int num = k + rowIndex;
					CellRange cellRange = new CellRange(num, spreadsheetHyperlink2.Range.FromIndex.ColumnIndex, num, spreadsheetHyperlink2.Range.ToIndex.ColumnIndex);
					SpreadsheetHyperlink value = new SpreadsheetHyperlink(cellRange, spreadsheetHyperlink2.HyperlinkInfo);
					base.Elements.Add(cellRange, value);
				}
			}
			this.OnChanged();
		}

		public SpreadsheetHyperlink Add(CellIndex cellIndex, HyperlinkInfo hyperlinkInfo)
		{
			return this.Add(cellIndex.ToCellRange(), hyperlinkInfo);
		}

		public SpreadsheetHyperlink Add(CellRange cellRange, HyperlinkInfo hyperlinkInfo)
		{
			SpreadsheetHyperlink spreadsheetHyperlink = new SpreadsheetHyperlink(cellRange, hyperlinkInfo);
			AddRemoveHyperlinkCommandContext context = new AddRemoveHyperlinkCommandContext(base.Worksheet, spreadsheetHyperlink);
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				SpreadsheetHyperlink hyperlink;
				if (this.TryGetHyperlinkExact(cellRange, out hyperlink))
				{
					AddRemoveHyperlinkCommandContext context2 = new AddRemoveHyperlinkCommandContext(base.Worksheet, hyperlink);
					base.Worksheet.ExecuteCommand<AddRemoveHyperlinkCommandContext>(WorkbookCommands.RemoveHyperlink, context2);
				}
				base.Worksheet.ExecuteCommand<AddRemoveHyperlinkCommandContext>(WorkbookCommands.AddHyperlink, context);
				base.Worksheet.Cells[cellRange].SetStyleNameAndExpandToFitNumberValuesWidth(SpreadsheetDefaultValues.HyperlinkStyleName);
			}
			return spreadsheetHyperlink;
		}

		public bool Remove(SpreadsheetHyperlink hyperlink)
		{
			return this.Remove(hyperlink, true);
		}

		internal bool Remove(SpreadsheetHyperlink hyperlink, bool removeFormatting)
		{
			AddRemoveHyperlinkCommandContext addRemoveHyperlinkCommandContext = new AddRemoveHyperlinkCommandContext(base.Worksheet, hyperlink);
			bool flag = false;
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				flag = base.Worksheet.ExecuteCommand<AddRemoveHyperlinkCommandContext>(WorkbookCommands.RemoveHyperlink, addRemoveHyperlinkCommandContext);
				if (removeFormatting && flag)
				{
					base.Worksheet.Cells[addRemoveHyperlinkCommandContext.Hyperlink.Range].SetStyleNameAndExpandToFitNumberValuesWidth(SpreadsheetDefaultValues.DefaultStyleName);
				}
			}
			return flag;
		}

		internal bool RemoveRange(IEnumerable<SpreadsheetHyperlink> hyperlinks, bool removeFormatting)
		{
			bool flag = true;
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				foreach (SpreadsheetHyperlink hyperlink in hyperlinks)
				{
					flag &= this.Remove(hyperlink, removeFormatting);
					if (!flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		public bool Contains(SpreadsheetHyperlink hyperlink)
		{
			return base.Elements.ContainsKey(hyperlink.Range);
		}

		internal IEnumerable<SpreadsheetHyperlink> GetIntersectingHyperlinks(int rowIndex, int columnIndex)
		{
			return from sh in base.Elements.Values
				where sh.Range.IntersectsWith(rowIndex, columnIndex, rowIndex, columnIndex)
				select sh;
		}

		public IEnumerable<SpreadsheetHyperlink> GetIntersectingHyperlinks(CellRange cellRange)
		{
			return from sh in base.Elements.Values
				where sh.Range.IntersectsWith(cellRange)
				select sh;
		}

		public IEnumerable<SpreadsheetHyperlink> GetContainingHyperlinks(CellRange cellRange)
		{
			return from sh in base.Elements.Values
				where cellRange.Contains(sh.Range)
				select sh;
		}

		public IEnumerable<SpreadsheetHyperlink> GetContainingHyperlinks(IEnumerable<CellRange> cellRanges)
		{
			List<SpreadsheetHyperlink> list = new List<SpreadsheetHyperlink>();
			foreach (CellRange cellRange in cellRanges)
			{
				list.AddRange(base.Worksheet.Hyperlinks.GetContainingHyperlinks(cellRange));
			}
			return list;
		}

		public bool TryGetHyperlink(CellRange cellRange, out SpreadsheetHyperlink hyperlink)
		{
			hyperlink = this.GetIntersectingHyperlinks(cellRange).LastOrDefault<SpreadsheetHyperlink>();
			return hyperlink != null;
		}

		public bool TryGetHyperlink(CellIndex cellIndex, out SpreadsheetHyperlink hyperlink)
		{
			return this.TryGetHyperlink(cellIndex.RowIndex, cellIndex.ColumnIndex, out hyperlink);
		}

		internal bool TryGetHyperlink(int rowIndex, int columnIndex, out SpreadsheetHyperlink hyperlink)
		{
			hyperlink = this.GetIntersectingHyperlinks(rowIndex, columnIndex).LastOrDefault<SpreadsheetHyperlink>();
			return hyperlink != null;
		}

		public bool TryGetHyperlinkExact(CellRange cellRange, out SpreadsheetHyperlink hyperlink)
		{
			hyperlink = (from sh in base.Elements.Values
				where sh.Range.Equals(cellRange)
				select sh).LastOrDefault<SpreadsheetHyperlink>();
			return hyperlink != null;
		}

		internal void AddInternal(SpreadsheetHyperlink hyperlink)
		{
			Guard.ThrowExceptionIfNull<SpreadsheetHyperlink>(hyperlink, "hyperlink");
			this.OnChanging();
			base.Elements.Add(hyperlink.Range, hyperlink);
			this.OnChanged();
		}

		internal void RemoveInternal(SpreadsheetHyperlink hyperlink)
		{
			Guard.ThrowExceptionIfNull<SpreadsheetHyperlink>(hyperlink, "hyperlink");
			this.OnChanging();
			base.Elements.Remove(hyperlink.Range);
			this.OnChanged();
		}

		protected override SpreadsheetHyperlink Add(CellRange cellRange, SpreadsheetHyperlink element)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			Guard.ThrowExceptionIfNull<SpreadsheetHyperlink>(element, "element");
			return this.Add(cellRange, element.HyperlinkInfo);
		}

		protected override bool Remove(CellRange cellRange)
		{
			SpreadsheetHyperlink hyperlink = base.Elements[cellRange];
			return this.Remove(hyperlink, true);
		}

		internal void CopyFrom(HyperlinkCollection fromHyperlinkCollection)
		{
			foreach (SpreadsheetHyperlink spreadsheetHyperlink in fromHyperlinkCollection)
			{
				base.Elements.Add(spreadsheetHyperlink.Range, spreadsheetHyperlink);
			}
		}

		internal void Clear()
		{
			base.Elements.Clear();
		}

		internal event EventHandler Changing;

		void OnChanging()
		{
			if (this.Changing != null)
			{
				this.Changing(this, EventArgs.Empty);
			}
		}

		public event EventHandler Changed;

		protected virtual void OnChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}
	}
}
