using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetSortRangeCommand : UndoableWorksheetCommandBase<SetSortRangeCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetSortRangeCommandContext context)
		{
			context.OldSortRange = context.Worksheet.SortState.SortRange;
		}

		protected override void UndoOverride(SetSortRangeCommandContext context)
		{
			context.Worksheet.SortState.SetSortRangeInternal(context.OldSortRange);
		}

		protected override bool AffectsLayoutOverride(SetSortRangeCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetSortRangeCommandContext context)
		{
			context.Worksheet.SortState.SetSortRangeInternal(context.NewSortRange);
		}
	}
}
