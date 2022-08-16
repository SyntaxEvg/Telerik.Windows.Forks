using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class MergeCellsCommandContext : WorksheetCommandContextBase
	{
		public CellRange NewMergedCellRange
		{
			get
			{
				return this.newMergedCellRange;
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

		public MergeCellsCommandContext(Worksheet worksheet, CellRange newMergedCellRange)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<CellRange>(newMergedCellRange, "newMergedCellRange");
			this.newMergedCellRange = newMergedCellRange;
			this.oldMergedCellRanges = new HashSet<CellRange>();
		}

		readonly CellRange newMergedCellRange;

		ISet<CellRange> oldMergedCellRanges;
	}
}
