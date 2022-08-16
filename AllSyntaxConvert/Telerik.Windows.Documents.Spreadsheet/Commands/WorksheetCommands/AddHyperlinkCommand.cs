using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddHyperlinkCommand : UndoableWorksheetCommandBase<AddRemoveHyperlinkCommandContext>
	{
		protected override bool AffectsLayoutOverride(AddRemoveHyperlinkCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(AddRemoveHyperlinkCommandContext context)
		{
		}

		protected override bool CanExecuteOverride(AddRemoveHyperlinkCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(AddRemoveHyperlinkCommandContext context)
		{
			context.Worksheet.Hyperlinks.AddInternal(context.Hyperlink);
		}

		protected override void UndoOverride(AddRemoveHyperlinkCommandContext context)
		{
			context.Worksheet.Hyperlinks.RemoveInternal(context.Hyperlink);
		}
	}
}
