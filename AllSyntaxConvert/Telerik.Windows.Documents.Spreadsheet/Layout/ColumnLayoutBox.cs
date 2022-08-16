using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public class ColumnLayoutBox : LayoutBox
	{
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		public ColumnLayoutBox(int columnIndex, Rect rect)
			: base(rect)
		{
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			this.columnIndex = columnIndex;
		}

		public override bool Equals(object obj)
		{
			ColumnLayoutBox columnLayoutBox = obj as ColumnLayoutBox;
			return columnLayoutBox != null && this.ColumnIndex == columnLayoutBox.ColumnIndex;
		}

		public override int GetHashCode()
		{
			return this.columnIndex;
		}

		public override string ToString()
		{
			return string.Format("ColumnLayoutBox {0}: {1}", this.ColumnIndex, base.ToString());
		}

		readonly int columnIndex;
	}
}
