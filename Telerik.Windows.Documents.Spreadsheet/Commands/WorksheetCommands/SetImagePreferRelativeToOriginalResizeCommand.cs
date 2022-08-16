using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetImagePreferRelativeToOriginalResizeCommand : UndoableWorksheetCommandBase<SetImagePreferRelativeToOriginalResizeCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetImagePreferRelativeToOriginalResizeCommandContext context)
		{
			context.OldPreferRelativeToOriginalResize = context.Image.PreferRelativeToOriginalResize;
		}

		protected override void UndoOverride(SetImagePreferRelativeToOriginalResizeCommandContext context)
		{
			context.Image.Image.PreferRelativeToOriginalResize = context.OldPreferRelativeToOriginalResize;
		}

		protected override bool AffectsLayoutOverride(SetImagePreferRelativeToOriginalResizeCommandContext context)
		{
			return false;
		}

		protected override void ExecuteOverride(SetImagePreferRelativeToOriginalResizeCommandContext context)
		{
			context.Image.Image.PreferRelativeToOriginalResize = context.PreferRelativeToOriginalResize;
		}
	}
}
