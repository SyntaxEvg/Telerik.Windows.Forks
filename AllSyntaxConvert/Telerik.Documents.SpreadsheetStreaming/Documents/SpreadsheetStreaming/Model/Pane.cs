using System;

namespace Telerik.Documents.SpreadsheetStreaming.Model
{
	class Pane
	{
		public int? XSplit { get; set; }

		public int? YSplit { get; set; }

		public SpreadPane? ActivePane { get; set; }

		public PaneState? State { get; set; }

		internal void GetTopLeftCell(out int rowIndex, out int columnIndex)
		{
			rowIndex = this.topLeftCellRowIndex;
			columnIndex = this.topLeftCellColumnIndex;
		}

		internal void SetTopLeftRowIndex(int rowIndex)
		{
			this.topLeftCellRowIndex = rowIndex;
		}

		internal void SetTopLeftColumnIndex(int columnIndex)
		{
			this.topLeftCellColumnIndex = columnIndex;
		}

		internal void SetTopLeftCell(int rowIndex, int columnIndex)
		{
			this.topLeftCellRowIndex = rowIndex;
			this.topLeftCellColumnIndex = columnIndex;
		}

		internal void SetSplitOffset(int rowOffset, int columnOffset)
		{
			this.XSplit = new int?(columnOffset);
			this.YSplit = new int?(rowOffset);
			this.State = new PaneState?(PaneState.Frozen);
		}

		internal void SetActivePane(SpreadPane spreadPane)
		{
			this.ActivePane = new SpreadPane?(spreadPane);
		}

		int topLeftCellRowIndex;

		int topLeftCellColumnIndex;
	}
}
