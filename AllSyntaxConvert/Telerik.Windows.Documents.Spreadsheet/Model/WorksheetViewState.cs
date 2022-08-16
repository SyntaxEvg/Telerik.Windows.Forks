using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class WorksheetViewState : SheetViewStateBase
	{
		public Size ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
			set
			{
				this.scaleFactor = SpreadsheetHelper.EnsureValidScaleFactor(value);
			}
		}

		public CellIndex TopLeftCellIndex
		{
			get
			{
				return this.topLeftCellIndex;
			}
			set
			{
				Guard.ThrowExceptionIfNull<CellIndex>(value, "value");
				this.topLeftCellIndex = value;
			}
		}

		public SelectionState SelectionState
		{
			get
			{
				return this.selectionState;
			}
			set
			{
				Guard.ThrowExceptionIfNull<SelectionState>(value, "value");
				this.selectionState = value;
			}
		}

		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				this.isSelected = value;
			}
		}

		public bool ShowGridLines
		{
			get
			{
				return this.showGridLines;
			}
			set
			{
				this.showGridLines = value;
			}
		}

		public bool ShowRowColHeaders
		{
			get
			{
				return this.showRowColHeaders;
			}
			set
			{
				this.showRowColHeaders = value;
			}
		}

		public Pane Pane
		{
			get
			{
				return this.pane;
			}
			set
			{
				this.pane = value;
			}
		}

		public bool CircleInvalidData
		{
			get
			{
				return this.circleInvalidData;
			}
			set
			{
				this.circleInvalidData = value;
			}
		}

		public WorksheetViewState()
		{
			this.scaleFactor = new Size(1.0, 1.0);
			this.topLeftCellIndex = new CellIndex(0, 0);
			this.selectionState = new SelectionState();
			this.showGridLines = true;
			this.showRowColHeaders = true;
		}

		public void FreezePanes(int frozenRowsCount, int frozenColumnsCount)
		{
			this.FreezePanes(this.topLeftCellIndex, frozenRowsCount, frozenColumnsCount);
		}

		public void FreezePanes(CellIndex fixedPaneTopLeftCellIndex, int frozenRowsCount, int frozenColumnsCount)
		{
			this.FreezePanes(fixedPaneTopLeftCellIndex, frozenRowsCount, frozenColumnsCount, null);
		}

		public void FreezePanes(CellIndex fixedPaneTopLeftCellIndex, int frozenRowsCount, int frozenColumnsCount, CellIndex scrollableTopLeftCellIndex)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, frozenRowsCount, "frozenRows");
			Guard.ThrowExceptionIfLessThan<int>(0, frozenColumnsCount, "frozenColumns");
			if (scrollableTopLeftCellIndex != null && scrollableTopLeftCellIndex.RowIndex <= fixedPaneTopLeftCellIndex.RowIndex + frozenRowsCount)
			{
				throw new ArgumentException("The top left cell index of the scrollable pane must be within the scrollable pane.");
			}
			if (scrollableTopLeftCellIndex != null && scrollableTopLeftCellIndex.ColumnIndex <= fixedPaneTopLeftCellIndex.ColumnIndex + frozenColumnsCount)
			{
				throw new ArgumentException("The top left cell index of the scrollable pane must be within the scrollable pane.");
			}
			this.TopLeftCellIndex = fixedPaneTopLeftCellIndex;
			if (scrollableTopLeftCellIndex == null)
			{
				scrollableTopLeftCellIndex = new CellIndex(fixedPaneTopLeftCellIndex.RowIndex + frozenRowsCount, fixedPaneTopLeftCellIndex.ColumnIndex + frozenColumnsCount);
			}
			Pane pane;
			if (frozenRowsCount == 0 && frozenColumnsCount == 0)
			{
				pane = null;
			}
			else
			{
				pane = new Pane(scrollableTopLeftCellIndex, frozenColumnsCount, frozenRowsCount, ViewportPaneType.Scrollable, PaneState.Frozen);
			}
			this.Pane = pane;
		}

		internal CellIndex GetFrozenCellIndex()
		{
			CellIndex result = null;
			Pane pane = this.Pane;
			if (pane != null && pane.State == PaneState.Frozen)
			{
				int xsplit = pane.XSplit;
				int ysplit = pane.YSplit;
				int rowIndex = ((ysplit == 0) ? 0 : (pane.YSplit + this.TopLeftCellIndex.RowIndex));
				int columnIndex = ((xsplit == 0) ? 0 : (pane.XSplit + this.TopLeftCellIndex.ColumnIndex));
				result = new CellIndex(rowIndex, columnIndex);
			}
			return result;
		}

		internal WorksheetViewState Clone()
		{
			WorksheetViewState worksheetViewState = new WorksheetViewState();
			worksheetViewState.IsSelected = this.IsSelected;
			worksheetViewState.ScaleFactor = this.ScaleFactor;
			worksheetViewState.TabColor = base.TabColor;
			worksheetViewState.TopLeftCellIndex = this.TopLeftCellIndex;
			worksheetViewState.ShowGridLines = this.ShowGridLines;
			worksheetViewState.ShowRowColHeaders = this.ShowRowColHeaders;
			worksheetViewState.CircleInvalidData = this.CircleInvalidData;
			if (this.Pane != null)
			{
				worksheetViewState.Pane = this.Pane.Clone();
			}
			if (this.SelectionState != null)
			{
				worksheetViewState.SelectionState = this.SelectionState.Clone();
			}
			return worksheetViewState;
		}

		internal void CopyFrom(WorksheetViewState fromWorksheetViewState)
		{
			this.isSelected = fromWorksheetViewState.IsSelected;
			this.scaleFactor = fromWorksheetViewState.ScaleFactor;
			this.topLeftCellIndex = fromWorksheetViewState.TopLeftCellIndex;
			this.showGridLines = fromWorksheetViewState.ShowGridLines;
			this.showRowColHeaders = fromWorksheetViewState.ShowRowColHeaders;
			this.circleInvalidData = fromWorksheetViewState.CircleInvalidData;
			base.TabColor = fromWorksheetViewState.TabColor;
			this.pane = ((fromWorksheetViewState.Pane != null) ? fromWorksheetViewState.Pane.Clone() : null);
			this.selectionState = ((fromWorksheetViewState.SelectionState != null) ? fromWorksheetViewState.SelectionState.Clone() : null);
		}

		public override string ToString()
		{
			return string.Format("ScaleFactor: {0}; TopLeftCellIndex: {1}; SelectionState: {2}", this.scaleFactor, this.topLeftCellIndex, this.selectionState);
		}

		Size scaleFactor;

		CellIndex topLeftCellIndex;

		SelectionState selectionState;

		Pane pane;

		bool isSelected;

		bool showGridLines;

		bool showRowColHeaders;

		bool circleInvalidData;
	}
}
