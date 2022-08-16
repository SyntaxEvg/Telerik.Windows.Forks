using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class InsertColumnCommand : UndoableWorksheetCommandBase<InsertColumnCommandContext>
	{
		protected override bool AffectsLayoutOverride(InsertColumnCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(InsertColumnCommandContext context)
		{
		}

		protected override bool CanExecuteOverride(InsertColumnCommandContext context)
		{
			return context.Worksheet.Columns.CanInsert(context.Index, context.ItemCount);
		}

		protected override void ExecuteOverride(InsertColumnCommandContext context)
		{
			context.Worksheet.Columns.PropertyBag.Insert(context.Index, context.ItemCount);
			context.Worksheet.Cells.InsertColumn(context.Index, context.ItemCount, null);
		}

		protected override void UndoOverride(InsertColumnCommandContext context)
		{
			context.Worksheet.Columns.PropertyBag.Remove(context.Index, context.ItemCount);
			context.Worksheet.Cells.RemoveColumn(context.Index, context.ItemCount);
		}
	}
}
