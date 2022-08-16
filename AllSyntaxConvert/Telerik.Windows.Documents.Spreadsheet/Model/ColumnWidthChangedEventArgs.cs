using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class ColumnWidthChangedEventArgs : EventArgs
	{
		public int FromIndex { get; set; }

		public int ToIndex { get; set; }

		public ColumnWidthChangedEventArgs(int fromIndex, int toIndex)
		{
			this.FromIndex = fromIndex;
			this.ToIndex = toIndex;
		}
	}
}
