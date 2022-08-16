using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class RowHeightChangedEventArgs : EventArgs
	{
		public int FromIndex { get; set; }

		public int ToIndex { get; set; }

		public RowHeightChangedEventArgs(int fromIndex, int toIndex)
		{
			this.FromIndex = fromIndex;
			this.ToIndex = toIndex;
		}
	}
}
