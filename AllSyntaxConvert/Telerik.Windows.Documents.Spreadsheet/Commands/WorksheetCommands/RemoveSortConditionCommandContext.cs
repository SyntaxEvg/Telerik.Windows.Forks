using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveSortConditionCommandContext : WorksheetCommandContextBase
	{
		public ISortCondition Condition
		{
			get
			{
				return this.condition;
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

		public RemoveSortConditionCommandContext(Worksheet worksheet, ISortCondition condition, int orderIndex)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<ISortCondition>(condition, "condition");
			this.condition = condition;
			this.orderIndex = orderIndex;
		}

		readonly ISortCondition condition;

		readonly int orderIndex;
	}
}
