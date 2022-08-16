using System;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	class XlsxWorksheetViewExporter : EntityBase, IWorksheetViewExporter, IDisposable
	{
		public XlsxWorksheetViewExporter(SheetViewsElement sheetViewsElement)
		{
			this.sheetViewsElement = sheetViewsElement;
		}

		SheetView SheetView
		{
			get
			{
				if (this.sheetView == null)
				{
					this.sheetView = new SheetView();
				}
				return this.sheetView;
			}
		}

		public void SetFirstVisibleCell(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			if (this.SheetView.HasFreezePanes)
			{
				Pane pane = this.SheetView.Pane;
				if (pane.YSplit != null)
				{
					int value = pane.YSplit.Value;
					Guard.ThrowExceptionIfGreaterThan<int>(this.SheetView.BottomRightSelection.ActiveCellRowIndex - value, rowIndex, "rowIndex");
				}
				if (pane.XSplit != null)
				{
					int value2 = pane.XSplit.Value;
					Guard.ThrowExceptionIfGreaterThan<int>(this.SheetView.BottomRightSelection.ActiveCellColumnIndex - value2, columnIndex, "columnIndex");
				}
			}
			this.SheetView.SetTopLeftCell(rowIndex, columnIndex);
		}

		public void SetFreezePanes(int rowsCount, int columnsCount)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.RowCount - 1, rowsCount, "rowsCount");
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.ColumnCount - 1, columnsCount, "columnsCount");
			this.SheetView.SetSplitOffset(rowsCount, columnsCount);
		}

		public void SetFreezePanes(int rowsCount, int columnsCount, int scrollablePaneFirstVisibleCellRowIndex, int scrollablePaneFirstVisibleCellColumnIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.RowCount - 1, rowsCount, "rowsCount");
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.ColumnCount - 1, columnsCount, "columnsCount");
			int activeCellRowIndex = this.SheetView.TopLeftSelection.ActiveCellRowIndex;
			int lowerBound = activeCellRowIndex + rowsCount;
			Guard.ThrowExceptionIfLessThan<int>(lowerBound, scrollablePaneFirstVisibleCellRowIndex, "scrollablePaneFirstVisibleCellRowIndex");
			int activeCellColumnIndex = this.sheetView.TopLeftSelection.ActiveCellColumnIndex;
			int lowerBound2 = activeCellColumnIndex + columnsCount;
			Guard.ThrowExceptionIfLessThan<int>(lowerBound2, scrollablePaneFirstVisibleCellColumnIndex, "scrollablePaneFirstVisibleCellColumnIndex");
			this.SheetView.SetSplitOffset(rowsCount, columnsCount);
			this.SheetView.SetTopLeftCell(SpreadPane.BottomRight, scrollablePaneFirstVisibleCellRowIndex, scrollablePaneFirstVisibleCellColumnIndex);
		}

		public void SetScaleFactor(double percent)
		{
			Guard.ThrowExceptionIfLessThan<double>(0.0, percent, "percent");
			this.SheetView.SetScaleFactor(percent);
		}

		public void SetShouldShowGridLines(bool value)
		{
			this.SheetView.SetShouldShowGridLines(value);
		}

		public void SetShouldShowRowColumnHeaders(bool value)
		{
			this.SheetView.SetShouldShowRowColumnHeaders(value);
		}

		public void SetActiveSelectionCell(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			this.SheetView.SetActiveSelectionCell(rowIndex, columnIndex);
		}

		public void AddSelectionRange(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, fromRowIndex, "fromRowIndex");
			Guard.ThrowExceptionIfLessThan<int>(0, fromColumnIndex, "fromColumnIndex");
			Guard.ThrowExceptionIfLessThan<int>(fromRowIndex, toRowIndex, "toRowIndex");
			Guard.ThrowExceptionIfLessThan<int>(fromColumnIndex, toColumnIndex, "toColumnIndex");
			SpreadCellRange selectedRange = new SpreadCellRange(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
			this.SheetView.AddSelectedRange(selectedRange);
		}

		internal override void CompleteWriteOverride()
		{
			if (this.sheetView != null)
			{
				this.sheetViewsElement.EnsureWritingStarted();
				SheetViewElement sheetViewElement = this.sheetViewsElement.CreateSheetViewWriter();
				sheetViewElement.Write(this.SheetView);
				this.sheetViewsElement.EnsureWritingEnded();
			}
		}

		readonly SheetViewsElement sheetViewsElement;

		SheetView sheetView;
	}
}
