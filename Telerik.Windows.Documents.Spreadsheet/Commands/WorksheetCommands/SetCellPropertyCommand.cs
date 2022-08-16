using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetCellPropertyCommand<T> : UndoableWorksheetCommandBase<SetCellPropertyCommandContext<T>>
	{
		protected override bool AffectsLayoutOverride(SetCellPropertyCommandContext<T> context)
		{
			return context.Property.AffectsLayout;
		}

		protected override void PreserveStateBeforeExecute(SetCellPropertyCommandContext<T> context)
		{
			context.OldValues = context.Worksheet.Cells.PropertyBag.GetPropertyValue<T>(context.Property, context.CellRange);
		}

		protected override void ExecuteOverride(SetCellPropertyCommandContext<T> context)
		{
			if (context.NewValue == null)
			{
				context.Worksheet.Cells.PropertyBag.ClearPropertyValue<T>(context.Property, context.CellRange);
				return;
			}
			context.Worksheet.Cells.PropertyBag.SetPropertyValue<T>(context.Property, context.CellRange, context.NewValue.Value);
		}

		protected override void UndoOverride(SetCellPropertyCommandContext<T> context)
		{
			context.Worksheet.Cells.PropertyBag.SetPropertyValue<T>(context.Property, context.CellRange, context.OldValues);
		}
	}
}
