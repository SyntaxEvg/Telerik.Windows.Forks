using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming.Model
{
	class Selection
	{
		public Selection(SpreadPane pane)
		{
			this.Pane = pane;
		}

		public IEnumerable<SpreadCellRange> SelectionRanges { get; set; }

		public SpreadPane Pane { get; set; }

		public int ActiveCellRowIndex
		{
			get
			{
				return this.activeCellRowIndex;
			}
			set
			{
				this.activeCellRowIndex = value;
			}
		}

		public int ActiveCellColumnIndex
		{
			get
			{
				return this.activeCellColumnIndex;
			}
			set
			{
				this.activeCellColumnIndex = value;
			}
		}

		internal void GetActiveCell(out int rowIndex, out int columnIndex)
		{
			rowIndex = this.activeCellRowIndex;
			columnIndex = this.activeCellColumnIndex;
		}

		internal void SetActiveCell(int rowIndex, int columnIndex)
		{
			this.activeCellRowIndex = rowIndex;
			this.activeCellColumnIndex = columnIndex;
		}

		internal void SetActiveCellColumnIndex(int columnIndex)
		{
			this.activeCellColumnIndex = columnIndex;
		}

		internal void SetActiveCellRowIndex(int rowIndex)
		{
			this.activeCellRowIndex = rowIndex;
		}

		internal void SetSelectedRanges(IEnumerable<SpreadCellRange> selectionRanges)
		{
			this.SelectionRanges = selectionRanges;
		}

		int activeCellRowIndex;

		int activeCellColumnIndex;
	}
}
