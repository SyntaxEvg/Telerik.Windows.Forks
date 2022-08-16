using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public class RowLayoutBox : LayoutBox
	{
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		public RowLayoutBox(int rowIndex, Rect rect)
			: base(rect)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			this.rowIndex = rowIndex;
		}

		public override bool Equals(object obj)
		{
			RowLayoutBox rowLayoutBox = obj as RowLayoutBox;
			return rowLayoutBox != null && this.RowIndex == rowLayoutBox.RowIndex;
		}

		public override int GetHashCode()
		{
			return this.rowIndex;
		}

		public override string ToString()
		{
			return string.Format("RowLayoutBox {0}: {1}", this.RowIndex, base.ToString());
		}

		readonly int rowIndex;
	}
}
