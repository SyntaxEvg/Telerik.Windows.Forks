using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public class CellLayoutBox : LayoutBox
	{
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		public CellIndex Index
		{
			get
			{
				if (this.index == null)
				{
					this.index = WorksheetPropertyBagBase.ConvertLongToCellIndex(this.longIndex);
				}
				return this.index;
			}
		}

		internal long LongIndex
		{
			get
			{
				return this.longIndex;
			}
		}

		public CellMergeState MergeState
		{
			get
			{
				return this.mergeState;
			}
		}

		public CellLayoutBox(int rowIndex, int columnIndex, Rect rect, CellMergeState mergeState)
			: base(rect)
		{
			this.columnIndex = columnIndex;
			this.rowIndex = rowIndex;
			this.longIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			this.mergeState = mergeState;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", this.RowIndex, this.ColumnIndex);
		}

		public override bool Equals(object obj)
		{
			CellLayoutBox cellLayoutBox = obj as CellLayoutBox;
			return cellLayoutBox != null && this.LongIndex == cellLayoutBox.LongIndex;
		}

		public override int GetHashCode()
		{
			return this.LongIndex.GetHashCode();
		}

		CellIndex index;

		readonly int columnIndex;

		readonly int rowIndex;

		readonly long longIndex;

		readonly CellMergeState mergeState;
	}
}
