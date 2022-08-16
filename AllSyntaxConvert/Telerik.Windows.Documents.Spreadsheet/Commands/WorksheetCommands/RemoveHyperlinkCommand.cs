using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveHyperlinkCommand : UndoableWorksheetCommandBase<AddRemoveHyperlinkCommandContext>
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
			return context.Worksheet.Hyperlinks.Contains(context.Hyperlink);
		}

		protected override void ExecuteOverride(AddRemoveHyperlinkCommandContext context)
		{
			context.Worksheet.Hyperlinks.RemoveInternal(context.Hyperlink);
		}

		protected override void UndoOverride(AddRemoveHyperlinkCommandContext context)
		{
			context.Worksheet.Hyperlinks.AddInternal(context.Hyperlink);
		}
	}
}
