using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetFilterCommand : UndoableWorksheetCommandBase<SetRemoveFilterCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetRemoveFilterCommandContext context)
		{
			context.HiddenRowsState = context.Worksheet.Filter.GetHiddenRowsState(context.RelativeColumnIndex);
		}

		protected override void UndoOverride(SetRemoveFilterCommandContext context)
		{
			if (context.OldFilter != null)
			{
				context.Worksheet.Filter.SetFilterWithoutApplying(context.OldFilter);
				context.Worksheet.Filter.RestoreHiddenRowsState(context.RelativeColumnIndex, context.HiddenRowsState);
				return;
			}
			context.Worksheet.Filter.RemoveFilterInternal(context.RelativeColumnIndex);
		}

		protected override bool AffectsLayoutOverride(SetRemoveFilterCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetRemoveFilterCommandContext context)
		{
			context.Worksheet.Filter.SetFilterInternal(context.NewFilter);
		}
	}
}
