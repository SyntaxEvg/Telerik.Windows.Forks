using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Documents.SpreadsheetStreaming.Model
{
	class SheetView
	{
		public SheetView()
		{
			this.Pane = new Pane();
			this.TopLeftSelection = new Selection(SpreadPane.TopLeft);
			this.TopRightSelection = new Selection(SpreadPane.TopRight);
			this.BottomLeftSelection = new Selection(SpreadPane.BottomLeft);
			this.BottomRightSelection = new Selection(SpreadPane.BottomRight);
			this.ShouldShowGridLines = new bool?(true);
			this.ShouldShowRowColumnHeaders = new bool?(true);
		}

		public bool HasFreezePanes { get; set; }

		public bool IsFirstRowFreeze { get; set; }

		public bool IsFirstColumnFreeze { get; set; }

		public int? ScaleFactor { get; set; }

		public bool? ShouldShowGridLines { get; set; }

		public bool? ShouldShowRowColumnHeaders { get; set; }

		public Pane Pane { get; set; }

		public Selection TopLeftSelection { get; set; }

		public Selection TopRightSelection { get; set; }

		public Selection BottomLeftSelection { get; set; }

		public Selection BottomRightSelection { get; set; }

		public void PrepareForExport()
		{
			Selection selection = this.TopLeftSelection;
			if (this.HasFreezePanes)
			{
				selection = this.BottomRightSelection;
			}
			if (!this.isActiveSelectionCellSet && this.selectedRanges != null)
			{
				SpreadCellRange spreadCellRange = this.selectedRanges.First<SpreadCellRange>();
				this.activeSelectionCellRowIndex = spreadCellRange.FromRowIndex;
				this.activeSelectionCellColumnIndex = spreadCellRange.FromColumnIndex;
			}
			if (this.isActiveSelectionCellSet && this.selectedRanges != null)
			{
				IEnumerable<SpreadCellRange> source = from p in this.selectedRanges
					where p.ContainsCell(this.activeSelectionCellRowIndex, this.activeSelectionCellColumnIndex)
					select p;
				if (!source.Any<SpreadCellRange>())
				{
					throw new InvalidOperationException("The active cell should be part of the selection");
				}
			}
			selection.SetActiveCell(this.activeSelectionCellRowIndex, this.activeSelectionCellColumnIndex);
			selection.SetSelectedRanges(this.selectedRanges);
		}

		internal void SetTopLeftCell(int rowIndex, int columnIndex)
		{
			this.SetTopLeftCell(SpreadPane.TopLeft, rowIndex, columnIndex);
		}

		internal void SetTopLeftCell(SpreadPane pane, int rowIndex, int columnIndex)
		{
			switch (pane)
			{
			case SpreadPane.TopLeft:
				this.topLeftCellRowIndex = rowIndex;
				this.topLeftCellColumnIndex = columnIndex;
				this.TopLeftSelection.SetActiveCell(rowIndex, columnIndex);
				this.TopRightSelection.SetActiveCellRowIndex(rowIndex);
				this.BottomLeftSelection.SetActiveCellColumnIndex(columnIndex);
				return;
			case SpreadPane.TopRight:
				this.topLeftCellRowIndex = rowIndex;
				this.TopRightSelection.SetActiveCell(rowIndex, columnIndex);
				this.TopLeftSelection.SetActiveCellRowIndex(rowIndex);
				this.Pane.SetTopLeftColumnIndex(columnIndex);
				return;
			case SpreadPane.BottomLeft:
				this.BottomLeftSelection.SetActiveCell(rowIndex, columnIndex);
				this.TopLeftSelection.SetActiveCellColumnIndex(columnIndex);
				this.topLeftCellColumnIndex = columnIndex;
				this.Pane.SetTopLeftRowIndex(rowIndex);
				return;
			case SpreadPane.BottomRight:
				this.TopRightSelection.SetActiveCellColumnIndex(columnIndex);
				this.BottomLeftSelection.SetActiveCellRowIndex(rowIndex);
				this.Pane.SetTopLeftCell(rowIndex, columnIndex);
				return;
			default:
				return;
			}
		}

		internal void GetTopLeftCell(SpreadPane pane, out int rowIndex, out int columnIndex)
		{
			switch (pane)
			{
			case SpreadPane.TopLeft:
				rowIndex = this.topLeftCellRowIndex;
				columnIndex = this.topLeftCellColumnIndex;
				return;
			case SpreadPane.TopRight:
				this.TopRightSelection.GetActiveCell(out rowIndex, out columnIndex);
				return;
			case SpreadPane.BottomLeft:
				this.BottomLeftSelection.GetActiveCell(out rowIndex, out columnIndex);
				return;
			case SpreadPane.BottomRight:
				this.Pane.GetTopLeftCell(out rowIndex, out columnIndex);
				return;
			default:
				throw new ArgumentException("Not expected pane value.");
			}
		}

		internal void SetSplitOffset(int rowOffset, int columnOffset)
		{
			this.HasFreezePanes = rowOffset > 0 || columnOffset > 0;
			this.IsFirstColumnFreeze = rowOffset == 0 && columnOffset > 0;
			this.IsFirstRowFreeze = rowOffset > 0 && columnOffset == 0;
			this.Pane.SetSplitOffset(rowOffset, columnOffset);
			this.Pane.SetActivePane(SpreadPane.BottomRight);
			this.Pane.SetTopLeftCell(this.topLeftCellRowIndex + rowOffset, this.topLeftCellColumnIndex + columnOffset);
			this.TopRightSelection.SetActiveCell(this.topLeftCellRowIndex, this.topLeftCellColumnIndex + columnOffset);
			this.BottomLeftSelection.SetActiveCell(this.topLeftCellRowIndex + rowOffset, this.topLeftCellColumnIndex);
			this.BottomRightSelection.SetActiveCell(this.topLeftCellRowIndex + rowOffset, this.topLeftCellColumnIndex + columnOffset);
		}

		internal void SetScaleFactor(double percents)
		{
			this.ScaleFactor = new int?((int)(percents * 100.0));
		}

		internal void SetShouldShowGridLines(bool value)
		{
			this.ShouldShowGridLines = new bool?(value);
		}

		internal void SetShouldShowRowColumnHeaders(bool value)
		{
			this.ShouldShowRowColumnHeaders = new bool?(value);
		}

		internal void SetActiveSelectionCell(int rowIndex, int columnIndex)
		{
			this.isActiveSelectionCellSet = true;
			this.activeSelectionCellRowIndex = rowIndex;
			this.activeSelectionCellColumnIndex = columnIndex;
		}

		internal void AddSelectedRange(SpreadCellRange selectedRange)
		{
			if (this.selectedRanges == null)
			{
				this.selectedRanges = new List<SpreadCellRange>();
			}
			this.selectedRanges.Add(selectedRange);
		}

		int topLeftCellRowIndex;

		int topLeftCellColumnIndex;

		int activeSelectionCellRowIndex;

		int activeSelectionCellColumnIndex;

		List<SpreadCellRange> selectedRanges;

		bool isActiveSelectionCellSet;
	}
}
