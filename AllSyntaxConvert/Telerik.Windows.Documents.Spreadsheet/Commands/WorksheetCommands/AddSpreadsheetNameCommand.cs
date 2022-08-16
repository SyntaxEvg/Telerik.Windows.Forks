using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddSpreadsheetNameCommand : UndoableWorksheetCommandBase<AddRemoveSpreadsheetNameCommandContext>
	{
		protected override bool AffectsLayoutOverride(AddRemoveSpreadsheetNameCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(AddRemoveSpreadsheetNameCommandContext context)
		{
			if (context.Owner.IsGlobal)
			{
				context.Workbook.Names.AddInternal(context.Name);
				return;
			}
			context.Owner.CurrentWorksheet.Names.AddInternal(context.Name);
		}

		protected override void PreserveStateBeforeExecute(AddRemoveSpreadsheetNameCommandContext context)
		{
		}

		protected override void UndoOverride(AddRemoveSpreadsheetNameCommandContext context)
		{
			if (context.Owner.IsGlobal)
			{
				context.Workbook.Names.RemoveInternal(context.Name);
				return;
			}
			context.Owner.CurrentWorksheet.Names.RemoveInternal(context.Name);
		}
	}
}
