using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class UnmergeCellsCommandContext : WorksheetCommandContextBase
	{
		public CellRange CellRangeToUnmerge
		{
			get
			{
				return this.cellRangeToUnmerge;
			}
		}

		public ISet<CellRange> OldMergedCellRanges
		{
			get
			{
				return this.oldMergedCellRanges;
			}
			internal set
			{
				this.oldMergedCellRanges = value;
			}
		}

		public UnmergeCellsCommandContext(Worksheet worksheet, CellRange cellRangeToUnmerge)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRangeToUnmerge, "cellRangeToUnmerge");
			this.cellRangeToUnmerge = cellRangeToUnmerge;
			this.oldMergedCellRanges = new HashSet<CellRange>();
		}

		readonly CellRange cellRangeToUnmerge;

		ISet<CellRange> oldMergedCellRanges;
	}
}
