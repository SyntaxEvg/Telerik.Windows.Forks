using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class UpdateCellPropertyCommand<T> : UndoableWorksheetCommandBase<UpdateCellPropertyCommandContext<T>>
	{
		protected override bool AffectsLayoutOverride(UpdateCellPropertyCommandContext<T> context)
		{
			return context.Property.AffectsLayout;
		}

		protected override void PreserveStateBeforeExecute(UpdateCellPropertyCommandContext<T> context)
		{
			context.OldValues = context.Worksheet.Cells.PropertyBag.GetPropertyValue<T>(context.Property, context.CellRange);
		}

		protected override void ExecuteOverride(UpdateCellPropertyCommandContext<T> context)
		{
			context.Worksheet.Cells.PropertyBag.UpdatePropertyValue<T>(context.Property, context.CellRange, context.NewValueTransform);
		}

		protected override void UndoOverride(UpdateCellPropertyCommandContext<T> context)
		{
			context.Worksheet.Cells.PropertyBag.SetPropertyValue<T>(context.Property, context.CellRange, context.OldValues);
		}
	}
}
