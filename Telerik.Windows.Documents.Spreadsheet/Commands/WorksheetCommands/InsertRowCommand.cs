using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class InsertRowCommand : UndoableWorksheetCommandBase<InsertRowCommandContext>
	{
		protected override bool AffectsLayoutOverride(InsertRowCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(InsertRowCommandContext context)
		{
		}

		protected override bool CanExecuteOverride(InsertRowCommandContext context)
		{
			return context.Worksheet.Rows.CanInsert(context.Index, context.ItemCount);
		}

		protected override void ExecuteOverride(InsertRowCommandContext context)
		{
			context.Worksheet.Rows.PropertyBag.Insert(context.Index, context.ItemCount);
			context.Worksheet.Cells.InsertRow(context.Index, context.ItemCount, null);
		}

		protected override void UndoOverride(InsertRowCommandContext context)
		{
			context.Worksheet.Rows.PropertyBag.Remove(context.Index, context.ItemCount);
			context.Worksheet.Cells.RemoveRow(context.Index, context.ItemCount);
		}
	}
}
