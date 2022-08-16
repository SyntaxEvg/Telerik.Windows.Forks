using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveSortConditionCommand : UndoableWorksheetCommandBase<RemoveSortConditionCommandContext>
	{
		protected override void PreserveStateBeforeExecute(RemoveSortConditionCommandContext context)
		{
		}

		protected override void UndoOverride(RemoveSortConditionCommandContext context)
		{
			context.Worksheet.SortState.Insert(context.OrderIndex, context.Condition);
		}

		protected override bool AffectsLayoutOverride(RemoveSortConditionCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(RemoveSortConditionCommandContext context)
		{
			context.Worksheet.SortState.Remove(context.Condition);
		}
	}
}
