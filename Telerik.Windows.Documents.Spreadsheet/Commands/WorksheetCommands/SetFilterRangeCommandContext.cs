using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetFilterRangeCommandContext : WorksheetCommandContextBase
	{
		public CellRange NewRange
		{
			get
			{
				return this.newRange;
			}
		}

		public CellRange OldRange
		{
			get
			{
				return this.oldRange;
			}
			set
			{
				this.oldRange = value;
			}
		}

		public List<IFilter> OldAppliedFilters
		{
			get
			{
				return this.oldFilters;
			}
			set
			{
				this.oldFilters = value;
			}
		}

		public Dictionary<int, ICompressedList<bool>> RelativeColumnIndexToHiddenRowsState
		{
			get
			{
				if (this.relativeColumnIndexToHiddenRowsState == null)
				{
					this.relativeColumnIndexToHiddenRowsState = new Dictionary<int, ICompressedList<bool>>();
				}
				return this.relativeColumnIndexToHiddenRowsState;
			}
		}

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public SetFilterRangeCommandContext(Worksheet worksheet, CellRange newRange)
			: base(worksheet)
		{
			this.newRange = newRange;
		}

		readonly CellRange newRange;

		CellRange oldRange;

		List<IFilter> oldFilters;

		Dictionary<int, ICompressedList<bool>> relativeColumnIndexToHiddenRowsState;
	}
}
