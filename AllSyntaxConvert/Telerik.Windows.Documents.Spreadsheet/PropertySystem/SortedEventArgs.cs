using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class SortedEventArgs : EventArgs
	{
		public CellRange Range { get; set; }

		public int[] SortedIndexes { get; set; }

		internal SortedEventArgs(CellRange range, int[] sortedIndexes)
		{
			this.Range = range;
			this.SortedIndexes = sortedIndexes;
		}
	}
}
