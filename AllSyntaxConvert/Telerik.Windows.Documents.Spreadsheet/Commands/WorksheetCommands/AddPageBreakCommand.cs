using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddPageBreakCommand : UndoableWorksheetCommandBase<AddRemovePageBreakCommandContext>
	{
		protected override void PreserveStateBeforeExecute(AddRemovePageBreakCommandContext context)
		{
		}

		protected override void UndoOverride(AddRemovePageBreakCommandContext context)
		{
			context.Worksheet.WorksheetPageSetup.PageBreaks.RemovePageBreakInternal(context.PageBreak);
		}

		protected override bool AffectsLayoutOverride(AddRemovePageBreakCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(AddRemovePageBreakCommandContext context)
		{
			context.Worksheet.WorksheetPageSetup.PageBreaks.AddPageBreakInternal(context.PageBreak);
		}
	}
}
