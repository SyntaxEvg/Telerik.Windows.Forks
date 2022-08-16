using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveFilterCommand : UndoableWorksheetCommandBase<SetRemoveFilterCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetRemoveFilterCommandContext context)
		{
			context.HiddenRowsState = context.Worksheet.Filter.GetHiddenRowsState(context.RelativeColumnIndex);
		}

		protected override void UndoOverride(SetRemoveFilterCommandContext context)
		{
			context.Worksheet.Filter.SetFilterWithoutApplying(context.OldFilter);
			context.Worksheet.Filter.RestoreHiddenRowsState(context.RelativeColumnIndex, context.HiddenRowsState);
		}

		protected override bool AffectsLayoutOverride(SetRemoveFilterCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetRemoveFilterCommandContext context)
		{
			context.Worksheet.Filter.RemoveFilterInternal(context.RelativeColumnIndex);
		}
	}
}
