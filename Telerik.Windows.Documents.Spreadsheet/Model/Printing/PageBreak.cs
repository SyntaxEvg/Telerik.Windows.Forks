using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class PageBreak
	{
		internal PageBreak(PageBreakType type, int placementIndex, int fromIndex, int toIndex)
		{
			this.type = type;
			this.index = placementIndex;
			this.fromIndex = fromIndex;
			this.toIndex = toIndex;
			int num;
			int num2;
			int num3;
			int num4;
			if (this.Type == PageBreakType.Horizontal)
			{
				num = this.Index;
				num2 = this.FromIndex;
				num3 = this.Index;
				num4 = this.ToIndex;
			}
			else
			{
				num = this.FromIndex;
				num2 = this.Index;
				num3 = this.ToIndex;
				num4 = this.Index;
			}
			num = Math.Max(num, 0);
			num2 = Math.Max(num2, 0);
			num3 = System.Math.Min(num3, SpreadsheetDefaultValues.RowCount - 1);
			num4 = System.Math.Min(num4, SpreadsheetDefaultValues.ColumnCount - 1);
			this.range = new CellRange(num, num2, num3, num4);
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public PageBreakType Type
		{
			get
			{
				return this.type;
			}
		}

		public int FromIndex
		{
			get
			{
				return this.fromIndex;
			}
		}

		public int ToIndex
		{
			get
			{
				return this.toIndex;
			}
		}

		internal CellRange CellRange
		{
			get
			{
				return this.range;
			}
		}

		internal bool IsInfinite
		{
			get
			{
				return this.FromIndex == 0 && this.ToIndex == ((this.Type == PageBreakType.Horizontal) ? PageBreak.HorizontalPageBreakMaximumIndex : PageBreak.VerticalPageBreakMaximumIndex);
			}
		}

		public override string ToString()
		{
			return string.Format("PageBreakType: {0}, Index: {1}, FromIndex: {2}, ToIndex: {3}", new object[] { this.Type, this.Index, this.FromIndex, this.ToIndex });
		}

		readonly PageBreakType type;

		readonly int index;

		readonly int fromIndex;

		readonly int toIndex;

		readonly CellRange range;

		internal static readonly int HorizontalPageBreakMaximumIndex = SpreadsheetDefaultValues.ColumnCount - 1;

		internal static readonly int VerticalPageBreakMaximumIndex = SpreadsheetDefaultValues.RowCount - 1;
	}
}
