using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class MoveSortConditionCommandContext : WorksheetCommandContextBase
	{
		public ISortCondition Condition
		{
			get
			{
				return this.condition;
			}
		}

		public int NewIndex
		{
			get
			{
				return this.newIndex;
			}
		}

		public int OrderIndex
		{
			get
			{
				return this.orderIndex;
			}
		}

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public MoveSortConditionCommandContext(Worksheet worksheet, ISortCondition condition, int newIndex, int orderIndex)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<ISortCondition>(condition, "condition");
			this.newIndex = newIndex;
			this.condition = condition;
			this.orderIndex = orderIndex;
		}

		readonly ISortCondition condition;

		readonly int newIndex;

		readonly int orderIndex;
	}
}
