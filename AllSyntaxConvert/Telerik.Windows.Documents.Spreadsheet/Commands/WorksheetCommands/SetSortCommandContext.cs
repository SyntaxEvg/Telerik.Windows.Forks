using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetSortCommandContext : WorksheetCommandContextBase
	{
		public int[] OldSortedIndexes
		{
			get
			{
				return this.oldSortedIndexes;
			}
			set
			{
				this.oldSortedIndexes = value;
			}
		}

		public CellRange SortRange
		{
			get
			{
				return this.sortRange;
			}
		}

		public ISortCondition SortCondition
		{
			get
			{
				return this.sortCondition;
			}
		}

		public int[] SortedIndexes
		{
			get
			{
				return this.sortedIndexes;
			}
		}

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public SetSortCommandContext(Worksheet worksheet, CellRange sortRange, ISortCondition sortCondition, int[] sortedIndexes)
			: base(worksheet)
		{
			this.sortRange = sortRange;
			this.sortCondition = sortCondition;
			this.sortedIndexes = sortedIndexes;
		}

		readonly CellRange sortRange;

		readonly ISortCondition sortCondition;

		readonly int[] sortedIndexes;

		int[] oldSortedIndexes;
	}
}
