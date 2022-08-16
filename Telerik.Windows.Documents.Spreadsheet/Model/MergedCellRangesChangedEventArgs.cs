using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class MergedCellRangesChangedEventArgs : EventArgs
	{
		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		public MergedCellRangesChangedEventArgs(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.cellRange = cellRange;
		}

		readonly CellRange cellRange;
	}
}
