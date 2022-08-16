using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddToRemoveFromPrintAreaCommandContext : WorksheetCommandContextBase
	{
		public List<CellRange> Ranges
		{
			get
			{
				return this.ranges;
			}
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public AddToRemoveFromPrintAreaCommandContext(Worksheet worksheet, IEnumerable<CellRange> ranges, int index)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<IEnumerable<CellRange>>(ranges, "printRange");
			this.ranges = new List<CellRange>(ranges);
			this.index = index;
		}

		readonly List<CellRange> ranges;

		readonly int index;
	}
}
