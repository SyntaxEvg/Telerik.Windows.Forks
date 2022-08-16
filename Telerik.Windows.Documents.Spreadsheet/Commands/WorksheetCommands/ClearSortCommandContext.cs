using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class ClearSortCommandContext : WorksheetCommandContextBase
	{
		public CellRange SortRange { get; set; }

		public IEnumerable<ISortCondition> SortConditions { get; set; }

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public ClearSortCommandContext(Worksheet worksheet)
			: base(worksheet)
		{
		}
	}
}
