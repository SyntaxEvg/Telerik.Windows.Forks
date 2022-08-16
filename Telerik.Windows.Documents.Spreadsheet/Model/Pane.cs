using System;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class Pane
	{
		public int XSplit
		{
			get
			{
				return this.xSplit;
			}
		}

		public int YSplit
		{
			get
			{
				return this.ySplit;
			}
		}

		public CellIndex TopLeftCellIndex
		{
			get
			{
				return this.topLeftCellIndex;
			}
		}

		public ViewportPaneType ActivePane
		{
			get
			{
				return this.activePane;
			}
		}

		public PaneState State
		{
			get
			{
				return this.state;
			}
		}

		public Pane(CellIndex topLeftCellIndex, int xSplit, int ySplit, ViewportPaneType activePane, PaneState state = PaneState.Frozen)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(topLeftCellIndex, "topLeftCellIndex");
			Guard.ThrowExceptionIfLessThan<int>(0, xSplit, "xSplit");
			Guard.ThrowExceptionIfLessThan<int>(0, ySplit, "ySplit");
			this.topLeftCellIndex = topLeftCellIndex;
			this.xSplit = xSplit;
			this.ySplit = ySplit;
			this.activePane = activePane;
			this.state = state;
		}

		public Pane Clone()
		{
			return new Pane(this.TopLeftCellIndex, this.XSplit, this.YSplit, this.ActivePane, this.State);
		}

		public override bool Equals(object obj)
		{
			Pane pane = obj as Pane;
			return pane != null && (this.XSplit.Equals(pane.XSplit) && this.YSplit.Equals(pane.YSplit) && this.TopLeftCellIndex.Equals(pane.TopLeftCellIndex) && this.ActivePane.Equals(pane.ActivePane)) && this.State.Equals(pane.State);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.XSplit.GetHashCodeOrZero(), this.YSplit.GetHashCodeOrZero(), this.TopLeftCellIndex.GetHashCodeOrZero(), this.ActivePane.GetHashCodeOrZero(), this.State.GetHashCodeOrZero());
		}

		readonly int xSplit;

		readonly int ySplit;

		readonly CellIndex topLeftCellIndex;

		readonly ViewportPaneType activePane;

		readonly PaneState state;
	}
}
