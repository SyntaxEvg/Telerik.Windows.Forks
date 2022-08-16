using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetSortRangeCommandContext : WorksheetCommandContextBase
	{
		public CellRange NewSortRange
		{
			get
			{
				return this.newSortRange;
			}
		}

		public CellRange OldSortRange
		{
			get
			{
				return this.oldSortRange;
			}
			set
			{
				if (this.oldSortRange != value)
				{
					this.oldSortRange = value;
				}
			}
		}

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public SetSortRangeCommandContext(Worksheet worksheet, CellRange newRange)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<CellRange>(newRange, "newRange");
			this.newSortRange = newRange;
		}

		readonly CellRange newSortRange;

		CellRange oldSortRange;
	}
}
