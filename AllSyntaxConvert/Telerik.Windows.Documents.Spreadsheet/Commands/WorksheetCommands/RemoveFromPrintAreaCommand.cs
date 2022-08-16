using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveFromPrintAreaCommand : UndoableWorksheetCommandBase<AddToRemoveFromPrintAreaCommandContext>
	{
		protected override void PreserveStateBeforeExecute(AddToRemoveFromPrintAreaCommandContext context)
		{
		}

		protected override void UndoOverride(AddToRemoveFromPrintAreaCommandContext context)
		{
			context.Worksheet.WorksheetPageSetup.PrintArea.InsertRangesInternal(context.Ranges, context.Index);
		}

		protected override bool AffectsLayoutOverride(AddToRemoveFromPrintAreaCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(AddToRemoveFromPrintAreaCommandContext context)
		{
			context.Worksheet.WorksheetPageSetup.PrintArea.RemoveRangesInternal(context.Index, context.Ranges.Count);
		}
	}
}
